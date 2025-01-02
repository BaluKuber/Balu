<%@ Page Title="S3G - Tax Guide Master" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GSysAdminTaxGuide_View.aspx.cs" Inherits="S3GSysAdminTaxGuide_View" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc3" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Tax Guide Master - Details" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td class="styleFieldAlign">
                        <asp:Panel ID="pnlSearch" runat="server" CssClass="stylePanel">
                            <table width="100%" cellpadding="0" cellspacing="2px" border="0">

                                <tr>
                                    <%--Branch--%>
                                    <td class="styleFieldLabel">
                                        <asp:Label Text="Tax Class" runat="server" ID="lblTaxClass" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlTaxClass" runat="server" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlTaxClass_OnSelectedIndexChanged">

                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Purchase" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Sales" Value="2"></asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                    <%--End Date--%>
                                    <td class="styleFieldLabel">
                                        <asp:Label Text="Asset Category" runat="server" ID="lblAssetCategory" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">

                                        <asp:DropDownList ID="ddlAssetCategory" runat="server"
                                            Width="150px" />

                                    </td>

                                </tr>

                                <tr>
                                    <%--Branch--%>
                                    <td class="styleFieldLabel">
                                        <asp:Label Text="Tax Type" runat="server" ID="lblTaxType" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList AutoPostBack="true" ID="ddlTaxType" runat="server" OnSelectedIndexChanged="ddlTaxType_OnSelectedIndexChanged"
                                            Width="150px">
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <%--End Date--%>
                                    <td class="styleFieldLabel">
                                        <asp:Label Text="Asset Type" runat="server" ID="lblAssetType" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc3:Suggest ID="txtAssetType" AutoPostBack="True" runat="server" ServiceMethod="GetAssettypeList" />

                                        <%--OnTextChanged="txtAssetType_OnTextChanged" <asp:TextBox ID="txtAssetType" runat="server" MaxLength="50"
                                            AutoPostBack="true" Width="150px"></asp:TextBox>
                                        OnTextChanged="txtAssetType_OnTextChanged"
                                        <cc1:AutoCompleteExtender ID="autoAssetType" MinimumPrefixLength="3"
                                            runat="server" TargetControlID="txtAssetType"
                                            ServiceMethod="GetAssettypeList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                            CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                            CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                            ShowOnlyCurrentWordInCompletionListItem="true">
                                        </cc1:AutoCompleteExtender>
                                        <cc1:TextBoxWatermarkExtender ID="txtAssetTypeExtender" runat="server" TargetControlID="txtAssetType"
                                            WatermarkText="--Select--">
                                        </cc1:TextBoxWatermarkExtender>
                                        <asp:HiddenField ID="hdnAssetType" runat="server" />--%>

                                    </td>

                                </tr>

                                <tr>
                                    <%--Branch--%>
                                    <td class="styleFieldLabel">
                                        <asp:Label Text="State" runat="server" ID="lblState" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc3:Suggest ID="txtState" AutoPostBack="True" runat="server" ServiceMethod="GetStateList" />
                                        <%--  OnItem_Selected="txtState_SelectedIndexChanged" <asp:TextBox ID="txtState" runat="server" MaxLength="50"
                                            AutoPostBack="true" Width="182px" OnTextChanged="txtState_OnTextChanged"></asp:TextBox>
                                       
                                           <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                  TargetControlID="txtState" ValidChars=".,-,/,' '" runat="server" Enabled="True">
                                </cc1:FilteredTextBoxExtender>
                                        <cc1:AutoCompleteExtender ID="autoState" MinimumPrefixLength="3" OnClientPopulated="State_ItemPopulated" OnClientItemSelected="State_ItemSelected"
                                            runat="server" TargetControlID="txtState"
                                            ServiceMethod="GetStateList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                            CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                            CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                            ShowOnlyCurrentWordInCompletionListItem="true">
                                        </cc1:AutoCompleteExtender>
                                        <cc1:TextBoxWatermarkExtender ID="txtStateExtender" runat="server" TargetControlID="txtState"
                                            WatermarkText="--Select--">
                                        </cc1:TextBoxWatermarkExtender>
                                        <asp:HiddenField ID="hdnState" runat="server" />--%>
                                    </td>
                                    <%--End Date--%>
                                    <td class="styleFieldLabel">
                                        <asp:Label Text="Reverse Charge Type/Zone" runat="server" ID="lblReverseCharge" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlReverseCharge" runat="server"
                                            Width="150px">
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        </asp:DropDownList>

                                    </td>

                                </tr>
                                <tr>

                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblEffectiveDateSearch" Text="Effective Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtEffectiveDateSearch" runat="server" Width="100px"></asp:TextBox>
                                        <%--OnTextChanged="txtEffectiveDateSearch_TextChanged"--%>
                                        <asp:Image ID="imgEffectiveDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarEffectiveDateSearch" runat="server" Enabled="True"
                                            PopupButtonID="imgEffectiveDateSearch" TargetControlID="txtEffectiveDateSearch" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>

                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblHSN" Text="HSN / SAC Code" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                       <uc3:Suggest ID="txtHSNCode" AutoPostBack="True" runat="server" ServiceMethod="GetHSNCode" />

                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px; padding-left: 5px;" align="center">
                        <asp:Button ID="btnSearch" OnClick="btnSearch_Click" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Search" runat="server"></asp:Button>
                        <asp:Button ID="btnCreate" OnClick="btnCreate_Click" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Create" runat="server"></asp:Button>
                        <asp:Button ID="btnExportToExcel" UseSubmitBehavior="true" OnClick="btnExportToExcel_Click" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Export To Excel" runat="server"></asp:Button>
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="styleSubmitButton"
                            OnClick="btnClear_Click" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <br />
                        <asp:GridView runat="server" ID="grvTaxGuide" AllowSorting="true" OnRowDataBound="grvTaxGuide_RowDataBound"
                            Width="100%" OnRowCommand="grvTaxGuide_RowCommand" AutoGenerateColumns="false"
                            RowStyle-HorizontalAlign="left" HeaderStyle-CssClass="styleGridHeader">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("Tax_Guide_ID") %>' CommandName="Query" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-CssClass="styleGridHeader">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" CssClass="styleGridEdit" ImageUrl="~/Images/spacer.gif"
                                            CommandArgument='<%# Bind("Tax_Guide_ID") %>' CommandName="Modify" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax Class" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxClass" runat="server" Text='<%# Bind("TaxClass") %>'></asp:Label>
                                    </ItemTemplate>
                                    <%--  <HeaderTemplate>
                                         
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnTaxClass" OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"
                                                        runat="server" Text="TaxClass"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnTaxClass" runat="server" AlternateText="Sort By TaxClass"
                                                        ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" MaxLength="8" runat="server"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtHeaderSearch4"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" " Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>--%>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxType" runat="server" Text='<%# Bind("TaxType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <%-- <HeaderTemplate>
                                       
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSortTaxType" OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"
                                                        runat="server" Text="TaxType"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortTaxType" runat="server" AlternateText="Sort By TaxType"
                                                        ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" MaxLength="12" runat="server"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtHeaderSearch3"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars="- " Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>--%>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="State" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblState_Name" runat="server" Text='<%# Bind("State_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <%-- <HeaderTemplate>
                                       
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnstate" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                        OnClick="FunProSortingColumn" Text="State"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnstate" runat="server" CssClass="styleImageSortingAsc"
                                                        AlternateText="Sort By State" ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" MaxLength="15"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtHeaderSearch1"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars="/"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>--%>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reverse Charge Type/Zone" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRev_Charge_typ" runat="server" Text='<%# Bind("REV_CHARGE_DESC") %>'></asp:Label>
                                    </ItemTemplate>
                                    <%--<HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnRev_Charge_typ" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                        OnClick="FunProSortingColumn" Text="Reverse Charge Type/Zone"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnRev_Charge_typ" runat="server" CssClass="styleImageSortingAsc"
                                                        AlternateText="Sort By Reverse Charge Type/Zone" ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox ID="txtHeaderSearch8" AutoCompleteType="None" runat="server" MaxLength="15"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtHeaderSearch8"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars="/"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>--%>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Effective From" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEffFrom" runat="server" Text='<%# Eval("Effective_From").ToString() %>'></asp:Label>
                                    </ItemTemplate>
                                    <%--  <HeaderTemplate>
                                        
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnEffFrom" CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"
                                                        runat="server" Text="Effective From"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnEffFrom" runat="server" AlternateText="Sort By Effective_From"
                                                        ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>--%>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="HSN / SAC Code" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                        <itemtemplate>
                                        <asp:Label ID="lblHSN_Code" runat="server" Text='<%# Bind("HSN_Code") %>'></asp:Label>
                                    </itemtemplate>
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asset Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAsset_Category" runat="server" Text='<%# Bind("Asset_Category") %>'></asp:Label>
                                    </ItemTemplate>
                                   
                                    <%-- <HeaderTemplate>
                                          
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnAsset_Category" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                        OnClick="FunProSortingColumn" Text="Asset Category"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnAsset_Category" runat="server" CssClass="styleImageSortingAsc"
                                                        AlternateText="Sort By Asset_Category" ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch6" runat="server" MaxLength="15"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtHeaderSearch6"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars="/"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>--%>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asset Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAsset_Type" runat="server" Text='<%# Bind("Asset_Type") %>'></asp:Label>
                                    </ItemTemplate>
                                    <%--  <HeaderTemplate>
                                        
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnAsset_Type" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                        OnClick="FunProSortingColumn" Text="Asset Type"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnAsset_Type" runat="server" CssClass="styleImageSortingAsc"
                                                        AlternateText="Sort By Asset_Type" ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch7" runat="server" MaxLength="15"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtHeaderSearch7"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars="/"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>--%>
                                </asp:TemplateField>
                                <asp:BoundField HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Right"
                                    DataField="RatePercentage" HeaderText="Rate%" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:CheckBox Enabled="false" runat="server" ID="chkActive" />
                                        <asp:Label ID="lblActive" Visible="false" runat="server" Text='<%#Eval("Is_Active")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%# Bind("USRID")%>'></asp:Label>
                                        <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%# Bind("User_Level_ID")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="top">
                        <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px; padding-left: 5px;" align="center">
                        <span runat="server" id="lblErrorMessage" class="styleMandatoryLabel"></span>
                    </td>
                </tr>

            </table>
            <input type="hidden" id="hdnSortDirection" runat="server" />
            <input type="hidden" id="hdnSortExpression" runat="server" />
            <input type="hidden" id="hdnSearch" runat="server" />
            <input type="hidden" id="hdnOrderBy" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
