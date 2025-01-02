<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GSysAdminLocationMaster_Add.aspx.cs" Inherits="System_Admin_S3GSysAdminLocationMaster_Add"
    Title="S3G - Location Master" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../App_Themes/S3GTheme_Blue/AutoSuggestBox.css" rel="Stylesheet" type="text/css" />

    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <asp:Label runat="server" Text="Location Master" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 100%;">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlHierarchyType" runat="server" GroupingText="Hierarchy Type" CssClass="stylePanel"
                                Visible="false">
                                <table>
                                    <tr>
                                        <td class="styleFieldAlign">
                                            <span class="styleReqFieldLabel">Hierarchy Type</span>
                                        </td>
                                        <td class="styleFieldAlign">
                                            <asp:RadioButtonList ID="rblHierarchy" runat="server" RepeatDirection="Horizontal"
                                                ValidationGroup="vgLocationGroup">
                                            </asp:RadioButtonList>
                                            <asp:RequiredFieldValidator ID="rfvHierarchy" runat="server" ValidationGroup="vgLocationGroup"
                                                Display="None" InitialValue="" ControlToValidate="rblHierarchy" ErrorMessage="Select the Hierarchy Type"></asp:RequiredFieldValidator>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 5px;" colspan="4">
                            <asp:Panel Visible="true" runat="server" ID="pnlLocationCatDetails" GroupingText="Location Category Details"
                                CssClass="stylePanel">
                                <asp:UpdatePanel ID="upLocationdetails" runat="server">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblParent" runat="server" Text="Previous Location" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:DropDownList ID="ddlParent" runat="server" DataTextField="CurrenctMapping_Code"
                                                        AutoPostBack="true" DataValueField="Mapping_Description" OnSelectedIndexChanged="ddlParent_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblMappingCodeDescription" runat="server"></asp:Label>
                                                    <asp:RequiredFieldValidator ID="rfvLocationParent" ValidationGroup="vgLocationGroup"
                                                        runat="server" ErrorMessage="Select the Previous Location" SetFocusOnError="True"
                                                        ControlToValidate="ddlParent" InitialValue="--Select--" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td colspan="2"></td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td class="styleFieldLabel" colspan="1">
                                                    <asp:Label ID="lblLastCode" runat="server" Text="Last Generated Code" CssClass="styleInfoLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" colspan="2">
                                                    <asp:Label ID="lblLastGenCode" runat="server" CssClass="styleInfoLabel"></asp:Label>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLocationCode" runat="server" CssClass="styleReqFieldLabel" Text="Location Code"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLocationCode" runat="server" ValidationGroup="vgLocationGroup"
                                                        MaxLength="3" Width="100px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvLocationCode" ValidationGroup="vgLocationGroup"
                                                        runat="server" ErrorMessage="Enter the Location Code" SetFocusOnError="True"
                                                        ControlToValidate="txtLocationCode" Display="None"></asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="ftexLocationCode" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="txtLocationCode" runat="server" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td colspan="2"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLocationDescription" runat="server" Text="Location Description"
                                                        CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="margin-bottom: 10px;">
                                                    <asp:TextBox ID="txtLocationName" runat="server" ValidationGroup="vgLocationGroup"
                                                        onkeypress="return fnCheckSpecialChars(true);"
                                                        onkeyup="fnAvoidFirstCharSpace(event.key,this)"
                                                        MaxLength="50" Width="400px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvLocationName" ValidationGroup="vgLocationGroup"
                                                        runat="server" ErrorMessage="Enter the Location Name" SetFocusOnError="True"
                                                        ControlToValidate="txtLocationName" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="revLocationName" runat="server" ValidationGroup="vgLocationGroup"
                                                        ErrorMessage="Enter a valid Location Name" ControlToValidate="txtLocationName"
                                                        ValidationExpression="^[a-zA-Z_0-9](\w|\W)*" Display="None"></asp:RegularExpressionValidator>
                                                </td>
                                                <td colspan="2"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <span>Active </span>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:CheckBox ID="cbxActive" runat="server" Checked="true" Enabled="false" />
                                                </td>
                                                <td colspan="4"></td>

                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Text="Operational Location" ID="lblOperationalLoc" CssClass="styleDisplayLabel"> </asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:CheckBox ID="cbxOperationalLoc" runat="server" />
                                                </td>
                                                <td colspan="4"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lbldealertype" runat="server" Text="Dealer Type" CssClass="styleDisplayLabel"> </asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:RadioButtonList ID="rblisdealertype" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Value="1" Text="Resident Dealer" />
                                                        <asp:ListItem Value="0" Text="Non-Resident Dealer" />
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td colspan="4"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblVATTIN" runat="server" Text="VAT-TIN" CssClass="styleDisplayLabel"> </asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtVATTIN" runat="server" MaxLength="15"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblVATDate" runat="server" Text="VAT Regn Effective Date" CssClass="styleDisplayLabel"> </asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtVATDate" runat="server" Width="100px"></asp:TextBox>
                                                    <asp:Image ID="imgVATDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderMRACreationFrom" runat="server" Enabled="True"
                                                        OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgVATDate"
                                                        TargetControlID="txtVATDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCSTTIN" runat="server" Text="CST-TIN" CssClass="styleDisplayLabel"> </asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtCSTTIN" runat="server" MaxLength="15"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCSTDate" runat="server" Text="CST Regn Effective Date" CssClass="styleDisplayLabel"> </asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtCSTDate" runat="server" Width="100px"></asp:TextBox>
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                                        OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="Image1"
                                                        TargetControlID="txtCSTDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td colspan="2"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblWCT" runat="server" Text="WCT Regn No" CssClass="styleDisplayLabel"> </asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtWCT" runat="server" MaxLength="15"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblWCTDate" runat="server" Text="WCT Regn Effective Date" CssClass="styleDisplayLabel"> </asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtWCTDate" runat="server" Width="100px"></asp:TextBox>
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
                                                        OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="Image2"
                                                        TargetControlID="txtWCTDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td colspan="2"></td>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblSGSTIN" runat="server" Text="GSTIN" CssClass="styleDisplayLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:TextBox ID="txtSGSTin" runat="server" Width="180px" MaxLength="15"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="filsgstin" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                            TargetControlID="txtSGSTin" runat="server" Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvcgstin" runat="server" Display="None" ErrorMessage="Enter the SGSTIN"
                                                            ControlToValidate="txtSGSTin" ValidationGroup="vgLocationGroup" Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    </td>

                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblSGSTREGDate" runat="server" Text="GST Regn Date"
                                                            CssClass="styleDisplayLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:TextBox ID="TxtSGSTRegDate" runat="server" Width="100px"></asp:TextBox>
                                                        <asp:Image ID="imgSGSTRegDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                        <asp:RequiredFieldValidator ID="rfvcgstregdate" runat="server" Display="None"
                                                            ErrorMessage="Enter GST Registration Date" ValidationGroup="vgLocationGroup" ControlToValidate="TxtSGSTRegDate"
                                                            SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="TxtSGSTRegDate"
                                                            PopupButtonID="imgSGSTRegDate" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                            ID="CalendarExtender3" Enabled="True">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblGSTStateCode" runat="server" CssClass="styleReqFieldLabel" Text="GST State Code"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtGSTStateCode" runat="server" ValidationGroup="vgLocationGroup"
                                                        MaxLength="2" Width="100px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="vgLocationGroup"
                                                        runat="server" ErrorMessage="Enter the GST State Code" SetFocusOnError="True"
                                                        ControlToValidate="txtGSTStateCode" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:Panel GroupingText="Communication Address" ID="pnlCommAddress" runat="server"
                                                        CssClass="stylePanel">
                                                        <table width="100%" align="center" cellspacing="0">
                                                            <tr>
                                                                <td class="styleFieldLabel" width="20%" style="padding-bottom: 0px">
                                                                    <asp:Label ID="lblComAddress1" runat="server" CssClass="styleDisplayLabel" Text="Address 1"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign" style="padding-bottom: 0px">
                                                                    <asp:TextBox ID="txtComAddress1" runat="server" MaxLength="60" Width="250px"></asp:TextBox>
                                                                    <%--<asp:RequiredFieldValidator ID="rfvComAddress1" runat="server" ControlToValidate="txtComAddress1"
                                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Address Details"></asp:RequiredFieldValidator>--%>
                                                                </td>

                                                                 <td class="styleFieldLabel">
                                                    <asp:Label ID="Label1" runat="server" Text="Effective From"  CssClass="styleReqFieldLabel" > </asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtAddressEffectiveFrom" runat="server" Width="100px"></asp:TextBox>
                                                    <asp:Image ID="imgAddressDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderAddressEffFrom" runat="server" Enabled="True"
                                                        OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate" PopupButtonID="imgAddressDate"
                                                        TargetControlID="txtAddressEffectiveFrom">
                                                    </cc1:CalendarExtender>
                                                      <asp:RequiredFieldValidator ID="rfvAddressEffectiveFrom" ValidationGroup="vgLocationGroup"
                                                        runat="server" ErrorMessage="Enter Effective From" SetFocusOnError="True"
                                                        ControlToValidate="txtAddressEffectiveFrom" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel" width="20%" style="padding-bottom: 0px">
                                                                    <asp:Label ID="lblComAddress2" runat="server" CssClass="styleDisplayLabel" Text="Address 2"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtCOmAddress2" runat="server" MaxLength="60" Width="250px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel" width="20%" style="padding-bottom: 0px">
                                                                    <asp:Label ID="lblComcity" runat="server" CssClass="styleDisplayLabel" Text="City"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtComCity" runat="server" MaxLength="30" Width="225px"></asp:TextBox>


                                                                    <cc1:AutoCompleteExtender ID="autoBranchSearch" MinimumPrefixLength="1" runat="server"
                                                                        TargetControlID="txtComCity" ServiceMethod="GetCityList" Enabled="True" ServicePath=""
                                                                        CompletionSetCount="5" CompletionListCssClass="CompletionList" DelimiterCharacters=";,:"
                                                                        CompletionListItemCssClass="CompletionListItemCssClass"
                                                                        CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                                        ShowOnlyCurrentWordInCompletionListItem="true">
                                                                    </cc1:AutoCompleteExtender>

                                                                    <cc1:TextBoxWatermarkExtender ID="txtComCitySearchExtender" runat="server" TargetControlID="txtComCity"
                                                                        WatermarkText="--Select--">
                                                                    </cc1:TextBoxWatermarkExtender>
                                                                    <%-- <asp:RequiredFieldValidator ID="rfvComCity" runat="server" ControlToValidate="txtComCity"
                                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Address Details"
                                                                                InitialValue=""></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel" width="20%">
                                                                    <asp:Label ID="lblComState" runat="server" CssClass="styleDisplayLabel" Text="State"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:DropDownList ID="txtComState" runat="server">
                                                                    </asp:DropDownList>
                                                                    <%--<asp:RequiredFieldValidator ID="rfvComState" runat="server" ControlToValidate="txtComState"
                                                                                InitialValue="-1" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                ValidationGroup="Address Details"></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel" width="20%">
                                                                    <asp:Label ID="lblComCountry" runat="server" CssClass="styleDisplayLabel" Text="Country"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtComCountry" runat="server" Width="80px" Text="India" ReadOnly="true">
                                                                    </asp:TextBox>
                                                                    <%--<asp:RequiredFieldValidator ID="rfvComCountry" runat="server" ControlToValidate="txtComCountry"
                                                                                InitialValue="0" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                ValidationGroup="Address Details"></asp:RequiredFieldValidator>--%>
                                                                    <asp:Label ID="lblCompincode" runat="server" CssClass="styleDisplayLabel" Text="PIN"></asp:Label>
                                                                    <asp:TextBox ID="txtComPincode" runat="server" MaxLength="10" Width="102px"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftxtComPincode" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                        TargetControlID="txtComPincode" ValidChars=" ">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <%--<asp:RequiredFieldValidator ID="rfvComPincode" runat="server" ControlToValidate="txtComPincode"
                                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Address Details"></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel" width="20%">
                                                                    <asp:Label ID="lblComTelephone" runat="server" CssClass="styleDisplayLabel" Text="Telephone"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtComTelephone" runat="server" MaxLength="12" Width="102px"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftxtComTelephone" runat="server" Enabled="True"
                                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtComTelephone"
                                                                        ValidChars=" -">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:Label ID="lblComMobile" runat="server" CssClass="styleDisplayLabel" Text="[M]"></asp:Label>
                                                                    &nbsp;&nbsp;
                                                                            <asp:TextBox ID="txtComMobile" runat="server" MaxLength="12" Width="102px"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftxtComMobile" runat="server" Enabled="True" FilterType="Numbers"
                                                                        TargetControlID="txtComMobile" ValidChars=" -">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <%-- <asp:RequiredFieldValidator ID="rfvCommMobile" runat="server" ControlToValidate="txtComMobile"
                                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Address Details"
                                                                                ErrorMessage="Enter the Mobile No. in Communication Address"></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel" width="20%">
                                                                    <asp:Label ID="lblComEmail" runat="server" CssClass="styleDisplayLabel" Text="EMail Id"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtComEmail" runat="server" MaxLength="60" Width="250px"></asp:TextBox>
                                                                    <%-- <asp:RegularExpressionValidator ID="revEmailId" runat="server" ControlToValidate="txtComEmail"
                                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationExpression="^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$"
                                                                                ValidationGroup="Address Details"></asp:RegularExpressionValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel" width="20%">
                                                                    <asp:Label ID="lblComWebSite" runat="server" CssClass="styleDisplayLabel" Text="Web Site"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtComWebsite" runat="server" MaxLength="60" Width="250px"></asp:TextBox>
                                                                    <%-- <asp:RegularExpressionValidator ID="revComWebsite" runat="server" ControlToValidate="txtComWebsite"
                                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationExpression="([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?"
                                                                                ValidationGroup="Address Details"></asp:RegularExpressionValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel" width="20%">
                                                                    <asp:Label ID="lblCommLandMark" runat="server" CssClass="styleDisplayLabel" Text="Landmark"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtCommLandmark" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                                                                    <asp:Button ID="btnCategoryGo" Text="Go" runat="server" ValidationGroup="vgLocationGroup"
                                                                        CssClass="styleSubmitShortButton" OnClientClick="return fnCheckPageValidators('vgLocationGroup','f')"
                                                                        OnClick="btnCategoryGo_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td colspan="6">
                                                    <div style="margin-bottom: 10px;">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <div style="margin-bottom: 5px; margin-left: 2px; margin-right: 2px; margin-top: 5px;">
                                                        <cc1:TabContainer ID="tcLocCategory" runat="server" CssClass="styleTabPanel" ScrollBars="Auto"
                                                            AutoPostBack="true" OnActiveTabChanged="tcLocCategory_ActiveTabChanged" Height="200px">
                                                        </cc1:TabContainer>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="6">
                                                    <div style="margin-top: 5px; margin-bottom: 5px;">
                                                        <asp:Button ID="btnSaveLocCategory" Text="Save" runat="server" CssClass="styleSubmitButton"
                                                            OnClick="btnSaveLocationCategory_Click" />&nbsp;
                                                        <asp:Button ID="btnCancelLocCategory" Text="Cancel" runat="server" CausesValidation="False"
                                                            CssClass="styleSubmitButton" OnClick="btnCancelLocCategory_Click" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                            <asp:Panel Visible="true" runat="server" ID="pnlLocationMapping" GroupingText="Location Mapping Details"
                                CssClass="stylePanel">
                                <asp:UpdatePanel ID="upLocationMapping" runat="server">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr valign="top" style="width: 100%">
                                                <td class="styleFieldAlign" align="right">
                                                    <span class="styleReqFieldLabel">Location Mapping Code</span>
                                                </td>
                                                <td align="left" class="styleFieldLabel">
                                                    <asp:TextBox ID="txtLocationMappingCode" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <asp:HiddenField ID="hdnMappingCode" runat="server" />
                                                    <asp:RequiredFieldValidator ID="rfvLocationMappingCode" runat="server" ControlToValidate="txtLocationMappingCode"
                                                        ErrorMessage="Select the Mapping" Display="None" ValidationGroup="btnMappingGo">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvLocationMappingCodeSave" runat="server" ControlToValidate="txtLocationMappingCode"
                                                        ErrorMessage="Select the Mapping" Display="None" ValidationGroup="btnSaveMappingGo">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:Button ID="btnLocationMapping_Go" runat="server" Text="Go" OnClick="btnLocationMapping_Go_Click"
                                                        ValidationGroup="btnMappingGo" CssClass="styleSubmitShortButton" Visible="false" />
                                                </td>
                                                <td class="styleFieldAlign" align="right">
                                                    <span class="styleFieldAlign">Location Description</span>
                                                </td>
                                                <td align="left" class="styleFieldLabel">
                                                    <asp:TextBox ID="txtMappingDescription" runat="server" Width="200px" TextMode="MultiLine"
                                                        ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <span>Active </span>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:CheckBox ID="cbxActiveMapping" runat="server" Checked="true" Enabled="false" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <span>Operational Location </span>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:CheckBox ID="cbxOperationalLocM" runat="server" Enabled="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <cc1:TabContainer ID="tcLocationMapping" runat="server" CssClass="styleTabPanel"
                                                        ScrollBars="Auto" Height="100px">
                                                    </cc1:TabContainer>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:GridView ID="gvLocationMapping" runat="server" AutoGenerateColumns="false" EmptyDataText="No Record's Found...">
                                                        <Columns>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="center">
                                                    <asp:Button ID="btnSaveLocationMapping" runat="server" Text="Save" CssClass="styleSubmitButton"
                                                        OnClick="btnSaveLocationMapping_Click" ValidationGroup="btnSaveMappingGo" />
                                                    <asp:Button ID="btnCancelMapping" OnClick="btnCancelLocCategory_Click" runat="server"
                                                        Text="Cancel" CssClass="styleSubmitButton" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:ValidationSummary ID="vsLocationMapping" runat="server" CssClass="styleMandatoryLabel"
                                                        HeaderText="Please correct the following validation(s):" ValidationGroup="btnMappingGo" ShowMessageBox="false" ShowSummary="true" />
                                                    <asp:ValidationSummary ID="vsMappingSave" runat="server" CssClass="styleMandatoryLabel"
                                                        HeaderText="Please correct the following validation(s):" ValidationGroup="btnSaveMappingGo" ShowMessageBox="false" ShowSummary="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:HiddenField ID="hdnID" runat="server" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="vsLocationMaster" runat="server"
                                ValidationGroup="vgLocationGroup"
                                HeaderText="Please correct the following validation(s):"
                                ShowMessageBox="false" ShowSummary="true" />
                            <asp:CustomValidator ID="cvLocationMaster" runat="server"></asp:CustomValidator>
                        </td>
                    </tr>

                <tr>
                    <td>

                        <cc1:TabContainer ID="tcLocationHistory" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                                Width="99%" ScrollBars="Auto" TabStripPlacement="top" AutoPostBack="false">
                                <cc1:TabPanel ID="tpLocationHistory" runat="server" HeaderText="Address History">
                                    <HeaderTemplate>
                                        Address History
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                         <asp:UpdatePanel ID="upLocationHistory" runat="server">
                                            <ContentTemplate>
                                            <div style="height: 235px; overflow-x: auto; overflow-y: auto; width: 100%">
                                                    <div>
                                                        
                                                          <asp:GridView ID="gvLocationHistory" runat="server" AutoGenerateColumns="false" ShowFooter="false" Width="100%">
                                                           <Columns>

                                                               <asp:TemplateField HeaderText="Sl.No." ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSNo"  runat="server" Text='<%#Bind("SlNo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Address 1" ItemStyle-Width="30%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAddress1"  runat="server" Text='<%#Bind("Address1") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                               <asp:TemplateField HeaderText="Address 2" ItemStyle-Width="20%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAddress2" runat="server" Text='<%#Bind("Address2") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="City" ItemStyle-Width="10%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCity" runat="server" Text='<%#Bind("City") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                               <asp:TemplateField HeaderText="PIN Code" ItemStyle-Width="10%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPinCode" runat="server" Text='<%#Bind("PinCode") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                               <asp:TemplateField HeaderText="Effective From" ItemStyle-Width="10%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEffFrom" runat="server" Text='<%#Bind("Effective_From") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                         </Columns>
                                                            </asp:GridView>

                                                    </div>
                                         </div>
                                     </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>

                    </td>
                </tr>

                </table>
            </td>
        </tr>
    </table>
</asp:Content>
