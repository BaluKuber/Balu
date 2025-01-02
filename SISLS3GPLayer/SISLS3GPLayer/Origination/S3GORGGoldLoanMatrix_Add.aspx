<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeFile="S3GORGGoldLoanMatrix_Add.aspx.cs" Inherits="Origination_S3GORGGoldLoanMatrix_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table width="100%">
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
                                <td style="width: 100%">
                                    <table style="width: 100%">
                                        <tr>
                                            <td valign="top" style="width: 100%">
                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblAssetCategory" runat="server" CssClass="styleReqFieldLabel" Text="Asset Category"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" style="padding-left: 10px">
                                                            <asp:DropDownList ID="ddlAssetCategory" ValidationGroup="Header" ToolTip="Asset Category"
                                                                OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged" runat="server"
                                                                AutoPostBack="True">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ErrorMessage="Select the Asset Category" ValidationGroup="Header"
                                                                ID="rfvAssetCategory" runat="server" InitialValue="0" ControlToValidate="ddlAssetCategory"
                                                                CssClass="styleMandatoryLabel" Display="None"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblConstitutionName" runat="server" Text="Constitution Name"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlConstitutionName" ValidationGroup="Header" ToolTip="Constitution Name"
                                                                runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlConstitutionName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <%--<asp:RequiredFieldValidator ErrorMessage="Select the Constitution Name" ValidationGroup="Header"
                                                                ID="rfvConstitutionName" runat="server" InitialValue="0" ControlToValidate="ddlConstitutionName" CssClass="styleMandatoryLabel"
                                                                Display="None"></asp:RequiredFieldValidator>--%>
                                                        </td>
                                                        <td rowspan="4" valign="top">
                                                            <asp:Panel GroupingText="Asset Hierarchy" runat="server" ID="panHierarchy" CssClass="stylePanel"
                                                                ScrollBars="Auto">
                                                                <%--style="background-color:aliceblue"--%>
                                                                <table>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" Text="Asset Class" ID="lblAssetClass"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:Label runat="server" ID="lblClassDesc"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" Text="Asset Make" ID="lblAssetMake"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:Label runat="server" ID="lblAssetMakeDesc"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" Text="Asset Type" ID="lblAssetType"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:Label runat="server" ID="lblAssetTypDesc"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" Text="Asset Model" ID="lblAssetModel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:Label runat="server" ID="lblAssetModelDesc"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblAssetDesc" runat="server" CssClass="styleReqFieldLabel" Text="Asset Desc"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlAssetDesc" ValidationGroup="Header" ToolTip="Asset Desc"
                                                                OnSelectedIndexChanged="ddlAssetDesc_SelectedIndexChanged" AutoPostBack="True"
                                                                runat="server">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ErrorMessage="Select the Asset Desc" ValidationGroup="Header"
                                                                ID="rfvAssetDesc" runat="server" InitialValue="0" ControlToValidate="ddlAssetDesc"
                                                                CssClass="styleMandatoryLabel" Display="None"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblProductName" runat="server" CssClass="styleReqFieldLabel" Text="Product Name"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlProductName" ValidationGroup="Header" ToolTip="Prooduct Name"
                                                                runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlProductName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ErrorMessage="Select the Prooduct" ValidationGroup="Header"
                                                                ID="rfvProductName" runat="server" InitialValue="0" ControlToValidate="ddlProductName"
                                                                CssClass="styleMandatoryLabel" Display="None"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblLocation" runat="server" CssClass="styleReqFieldLabel" Text="Location"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" style="padding-left: 10px">
                                                            <asp:DropDownList ID="ddlLocation" ValidationGroup="Header" ToolTip="State" runat="server"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ErrorMessage="Select the Location" ValidationGroup="Header"
                                                                ID="rfvState" runat="server" InitialValue="0" ControlToValidate="ddlLocation"
                                                                CssClass="styleMandatoryLabel" Display="None"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblEffectiveDate" runat="server" CssClass="styleReqFieldLabel" Text="Effective Date"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtEffectiveDate" Width="100px" ToolTip="Effective Date" runat="server"
                                                                AutoPostBack="True" OnTextChanged="txtEffectiveDate_TextChanged"></asp:TextBox>
                                                            <asp:Image ID="imgEffectivedate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                            <asp:RequiredFieldValidator ErrorMessage="Select the Effective Date" ValidationGroup="Header"
                                                                ID="rfvEffectiveDate" runat="server" ControlToValidate="txtEffectiveDate" CssClass="styleMandatoryLabel"
                                                                Display="None"></asp:RequiredFieldValidator>
                                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                                PopupButtonID="imgEffectivedate" TargetControlID="txtEffectiveDate" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate">
                                                            </cc1:CalendarExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblUnit" runat="server" CssClass="styleReqFieldLabel" Text="Unit"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" style="padding-left: 10px">
                                                            <asp:DropDownList ID="ddlUnit" ValidationGroup="Header" ToolTip="Unit" runat="server"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlUnit_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ErrorMessage="Select the Unit" ValidationGroup="Header"
                                                                ID="RequiredFieldValidator1" runat="server" InitialValue="0" ControlToValidate="ddlUnit"
                                                                CssClass="styleMandatoryLabel" Display="None"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblTenure" CssClass="styleReqFieldLabel" Text="Tenure"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtTenure" runat="server" MaxLength="3" Style="text-align: right"
                                                                onchange="FunRestIrr();" Width="25px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtTenure" runat="server" TargetControlID="txtTenure"
                                                                FilterType="Numbers" Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rfvTenure" runat="server" ControlToValidate="txtTenure"
                                                                ValidationGroup="Header" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                ErrorMessage="Enter the Tenure"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" ID="lblTenureType" CssClass="styleReqFieldLabel" Text="Tenure Type"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlTenureType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTenureType_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvTenureType" runat="server" ControlToValidate="ddlTenureType"
                                                                    ValidationGroup="Header" CssClass="styleMandatoryLabel" Display="None" InitialValue="-1"
                                                                    SetFocusOnError="True" ErrorMessage="Select a Tenure Type"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td align="left" class="styleFieldAlign">
                                                                <asp:Button CssClass="styleSubmitShortButton" Text="Go" ID="btnGo" runat="server"
                                                                    ValidationGroup="Header" OnClick="btnGo_Click" OnClientClick="return fnCheckPageValidators('Header', false);">
                                                                </asp:Button>
                                                            </td>
                                                        </tr>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top: 10px;">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="50%" valign="top">
                                                            <asp:Panel GroupingText="Funding Amount" runat="server" ID="Panel1" CssClass="stylePanel">
                                                                <asp:GridView runat="server" ShowFooter="true" DataKeyNames="FundingID" OnRowDataBound="grvFundDetails_RowDataBound"
                                                                    OnRowDeleting="grvFundDetails_RowDeleting" ID="grvFundDetails" Width="100%" AutoGenerateColumns="False"
                                                                    OnRowCommand="grvFundDetails_RowCommand">
                                                                    <Columns>
                                                                        <asp:TemplateField ItemStyle-Width="25px" HeaderText="Sl.No.">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lblFundingID" Text='<%#Eval("FundingID")%>' Visible="false"></asp:Label>
                                                                                <asp:Label runat="server" ID="lblSNO" ToolTip="Sl.No." Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Purity From">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPurityFromID" runat="server" Text='<%#Eval("PurityFromID")%>' Visible="false"></asp:Label>
                                                                                <asp:Label runat="server" ToolTip="Purity From" Text='<%#Eval("PurityFrom")%>' ID="lblPurityFrom"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:DropDownList ID="ddlPurityFrom" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvPurityFrom" CssClass="styleMandatoryLabel" runat="server"
                                                                                    InitialValue="0" ControlToValidate="ddlPurityFrom" ErrorMessage="Select the Purity From"
                                                                                    ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <FooterStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Purity To">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPurityToID" runat="server" Text='<%#Eval("PurityToID")%>' Visible="false"></asp:Label>
                                                                                <asp:Label runat="server" ToolTip="Purity To" Text='<%#Eval("PurityTo")%>' ID="lblPurityTo"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:DropDownList ID="ddlPurityTo" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvPurityTo" CssClass="styleMandatoryLabel" runat="server"
                                                                                    InitialValue="0" ControlToValidate="ddlPurityTo" ErrorMessage="Select the Purity To"
                                                                                    ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <FooterStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Max Amount">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" Text='<%#Eval("MaxAmount")%>' ID="lblMaxAmount" ToolTip="Max Amount"
                                                                                    Style="text-align: right;" Visible="false"></asp:Label>
                                                                                <asp:TextBox Width="100px" ID="txtIMaxAmount" ToolTip="Max Amount" Style="text-align: right;"
                                                                                    runat="server" MaxLength="13" Text='<%#Eval("MaxAmount")%>'></asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox Width="100px" ID="txtMaxAmount" ToolTip="Max Amount" ValidationGroup="Footer"
                                                                                    Style="text-align: right;" runat="server" MaxLength="13">
                                                                                </asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="fltMaxAmount" runat="server" TargetControlID="txtMaxAmount"
                                                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="rfvMaxAmount" CssClass="styleMandatoryLabel" runat="server"
                                                                                    ControlToValidate="txtMaxAmount" ErrorMessage="Enter the Max Amount" ValidationGroup="Footer"
                                                                                    Display="None"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Active">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox runat="server" ID="chkActive" ToolTip="Active" AutoPostBack="true"
                                                                                    OnCheckedChanged="chkFundActive_OnCheckedChanged" Checked='<%#Convert.ToBoolean(Eval("Active")) %>'
                                                                                    Enabled="false" />
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:CheckBox runat="server" ID="chkActive1" ToolTip="Active" Enabled="false" Checked="true" />
                                                                            </FooterTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <FooterStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Action">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkDelete" runat="server" ToolTip="Delete" CommandName="Delete"
                                                                                    Text="Delete" OnClientClick="return confirm('Are you sure you want to Delete this record?');">
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Button ID="btnAdd" runat="server" ToolTip="Add" CommandName="AddNew" ValidationGroup="Footer"
                                                                                    CssClass="styleSubmitShortButton" Text="Add"></asp:Button>
                                                                            </FooterTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <FooterStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </td>
                                                        <td width="50%" valign="top">
                                                            <asp:Panel GroupingText="Rate Fixing" runat="server" ID="Panel2" CssClass="stylePanel">
                                                                <asp:GridView runat="server" ShowFooter="true" ID="grvRateDetails" Width="100%" AutoGenerateColumns="False"
                                                                    OnRowCommand="grvRateDetails_RowCommand" OnRowDeleting="grvRateDetails_RowDeleting"
                                                                    OnRowDataBound="grvRateDetails_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField ItemStyle-Width="25px" HeaderText="Sl.No.">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lblRate_ID" Text='<%#Eval("Rate_ID")%>' Visible="false"></asp:Label>
                                                                                <asp:Label runat="server" ID="lblSNO" ToolTip="Sl.No." Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="LTV From">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ToolTip="From" Text='<%#Eval("FromAmt")%>' ID="lblFrom"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox Width="100px" ID="txtFromAmt" ToolTip="From Year" MaxLength="13" AutoPostBack="false"
                                                                                    runat="server" Style="text-align: right;">
                                                                                </asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="fltFromYear" runat="server" TargetControlID="txtFromAmt"
                                                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="rfvFromAmt" CssClass="styleMandatoryLabel" runat="server"
                                                                                    ControlToValidate="txtFromAmt" ErrorMessage="Enter the From amount" ValidationGroup="Rate"
                                                                                    Display="None"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="LTV To">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ToolTip="To" Text='<%#Eval("ToAmt")%>' ID="lblTo"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox Width="100px" ID="txtToAmt" ToolTip="To Year" MaxLength="13" ValidationGroup="Rate"
                                                                                    runat="server" Style="text-align: right;">
                                                                                </asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="fltToAmt" runat="server" TargetControlID="txtToAmt"
                                                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="rfvToYear" CssClass="styleMandatoryLabel" runat="server"
                                                                                    ControlToValidate="txtToAmt" ErrorMessage="Enter the To amount" ValidationGroup="Rate"
                                                                                    Display="None"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" HeaderText="Rate">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" Text='<%#Eval("Rate")%>' ID="lblMaxAmount" ToolTip="Rate"
                                                                                    Style="text-align: right;"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox Width="60px" ID="txtRate" ToolTip="Rate" Style="text-align: right;"
                                                                                    MaxLength="6" runat="server">
                                                                                </asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="flttxtRate" runat="server" TargetControlID="txtRate"
                                                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="rfvtxtRate" CssClass="styleMandatoryLabel" runat="server"
                                                                                    ControlToValidate="txtRate" ErrorMessage="Enter the Rate" ValidationGroup="Rate"
                                                                                    Display="None"></asp:RequiredFieldValidator>
                                                                                <asp:RangeValidator ID="rgvRate" runat="server" ControlToValidate="txtRate" Type="Double"
                                                                                    ErrorMessage="Rate should not exceed 100%" MaximumValue="100" MinimumValue="1"
                                                                                    ValidationGroup="Rate" Display="None"></asp:RangeValidator>
                                                                            </FooterTemplate>
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Active">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox runat="server" ID="chkActive" ToolTip="Active" Checked='<%#Convert.ToBoolean(Eval("Active")) %>'
                                                                                    AutoPostBack="true" OnCheckedChanged="chkRateActive_OnCheckedChanged" Enabled="false" />
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:CheckBox runat="server" ID="chkActive1" ToolTip="Active" Enabled="false" Checked="true" />
                                                                            </FooterTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <FooterStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Action">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkDelete" runat="server" ToolTip="Delete" CommandName="Delete"
                                                                                    Text="Delete" OnClientClick="return confirm('Are you sure you want to Delete this record?');">
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Button ID="btnAdd" Width="50px" runat="server" ToolTip="Add" CommandName="AddNew"
                                                                                    ValidationGroup="Rate" CssClass="styleSubmitShortButton" Text="Add"></asp:Button>
                                                                            </FooterTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <FooterStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                    <RowStyle HorizontalAlign="left" />
                                                                </asp:GridView>
                                                            </asp:Panel>
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
                                    <asp:Button runat="server" ID="btnSave" Enabled="false" ToolTip="Save" OnClientClick="return fnCheckPageValidators('Header');"
                                        CssClass="styleSubmitButton" ValidationGroup="Header" Text="Save" OnClick="btnSave_Click" />
                                    <asp:Button runat="server" ID="btnClear" CausesValidation="false" ToolTip="Clear"
                                        CssClass="styleSubmitButton" Text="Clear" OnClientClick="return fnConfirmClear();"
                                        OnClick="btnClear_Click" />
                                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" ToolTip="Cancel" CausesValidation="false"
                                        CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                            <tr class="styleButtonArea">
                                <td>
                                    <asp:ValidationSummary runat="server" ID="vsUserMgmt" HeaderText="Correct the following validation(s):"
                                        CssClass="styleMandatoryLabel" ValidationGroup="Header" Width="500px" ShowMessageBox="false"
                                        ShowSummary="true" />
                                    <asp:ValidationSummary runat="server" ID="ValidationSummary1" HeaderText="Correct the following validation(s):"
                                        CssClass="styleMandatoryLabel" ValidationGroup="Footer" Width="500px" ShowMessageBox="false"
                                        ShowSummary="true" />
                                    <asp:ValidationSummary runat="server" ID="ValidationSummary2" HeaderText="Correct the following validation(s):"
                                        CssClass="styleMandatoryLabel" ValidationGroup="Rate" Width="500px" ShowMessageBox="false"
                                        ShowSummary="true" />
                                    <input type="hidden" runat="server" value="0" id="hdnRowID" />
                                    <input type="hidden" runat="server" value="1" id="hdnAccValid" />
                                    <input type="hidden" runat="server" value="0" id="hdnStatus" />
                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        function fnCancelClick() {
            if (confirm('Are you sure you want to cancel?')) {
                return true;
            }
            else {
                return false;
            }
        }      
    </script>

</asp:Content>
