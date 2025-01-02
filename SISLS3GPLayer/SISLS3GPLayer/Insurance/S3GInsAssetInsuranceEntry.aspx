<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GInsAssetInsuranceEntry.aspx.cs" Inherits="Insurance_S3GInsAssetInsuranceEntry"
    EnableEventValidation="false" EnableViewState="true" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td width="100%">
                        <table width="100%" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="stylePageHeading">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" Text="Asset Insurance Entry" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td width="100%" valign="top">
                                                <asp:Panel ID="PnlInputCriteria" runat="server" GroupingText="Input Criteria" CssClass="stylePanel"
                                                    Width="100%">
                                                    <table width="100%" border="0">
                                                        <tr>
                                                            <td class="styleFieldAlign">
                                                                <asp:Label ID="lblAINSENO" runat="server" ToolTip="AINSE No" CssClass="styleReqFieldLabel"
                                                                    Text="AINSE No"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtAINSENO" runat="server" ToolTip="AINSE No" ReadOnly="true"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:Label ID="lblAINSEDate" runat="server" ToolTip="AINSE Date" CssClass="styleDisplayLabel"
                                                                    Text="AINSE Date"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtAINSEDate" runat="server" ToolTip="AINSE Date" ReadOnly="True"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftxtAINSEDate" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                    TargetControlID="txtAINSEDate" ValidChars="/,-">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldAlign">
                                                                <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlLOB" runat="server" ToolTip="Line of Business" Width="165px">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                                    Display="None" InitialValue="0" ErrorMessage="Select Line of Business" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                <asp:RequiredFieldValidator ID="rfvPolicyLOB" runat="server" ControlToValidate="ddlLOB"
                                                                    Display="None" InitialValue="0" ErrorMessage="Select Line of Business" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:Label ID="lblBranch" runat="server" ToolTip="Location" CssClass="styleReqFieldLabel"
                                                                    Text="Location"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" IsMandatory="true" ValidationGroup="Submit" ErrorMessage="Enter Location" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Panel ID="pnlCustomerInformation" runat="server" GroupingText="Customer Information"
                                                                    CssClass="stylePanel" Width="99%">
                                                                    <table width="99%" border="0" cellspacing="0">
                                                                        <tr>
                                                                            <td class="styleFieldLabel" width="35%">
                                                                                <asp:Label runat="server" ID="lblCustomerName" CssClass="styleReqFieldLabel" Text="Customer Name"></asp:Label>
                                                                            </td>
                                                                            <td class="styleFieldAlign">
                                                                                <uc2:LOV ID="ucCustomerCodeLov" runat="server" strLOV_Code="CMD" />
                                                                                <asp:HiddenField ID="hdnCustomerID" runat="server" />
                                                                                <asp:TextBox ID="txtTestforCustomer" runat="server" Style="display: none;"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ID="rfvforCustomer" runat="server" ControlToValidate="txtTestforCustomer"
                                                                                    Display="None" ErrorMessage="Select the Customer Name" ValidationGroup="Submit" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" width="99%">
                                                                                <asp:Button ID="btnLoadCustomer" runat="server" Style="display: none;" OnClick="btnLoadCustomer_OnClick"
                                                                                    Text="Load Customer" CausesValidation="false" />
                                                                                <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" FirstColumnStyle="styleFieldLabel"
                                                                                    SecondColumnStyle="styleFieldAlign" ShowCustomerName="false" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                            <td colspan="2" valign="top">
                                                                <asp:Panel ID="InsuranceCompanyDetails" runat="server" CssClass="stylePanel" GroupingText="Insurance Company Details"
                                                                    Visible="True" Width="99%">
                                                                    <table width="99%" border="0" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td class="styleFieldAlign" width="35%">
                                                                                <asp:Label ID="lblPayTo" runat="server" ToolTip="Insurer Name" Text="Insurer Name"
                                                                                    CssClass="styleReqFieldLabel"></asp:Label>
                                                                            </td>
                                                                            <td class="styleFieldAlign">
                                                                                <asp:DropDownList ID="ddlPayTo" runat="server" ToolTip="Insurer Name" OnSelectedIndexChanged="ddlPayTo_SelectedIndexChanged"
                                                                                    AutoPostBack="true">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvPayTo" runat="server" ControlToValidate="ddlPayTo"
                                                                                    InitialValue="0" Display="None" ErrorMessage="Select the Insurer Name" ValidationGroup="Submit" />
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlPayTo"
                                                                                    Display="None" ErrorMessage="Select the Insurer Name" ValidationGroup="Submit" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <uc1:S3GCustomerAddress ID="S3GCompanyAddress" runat="server" FirstColumnStyle="styleFieldLabel"
                                                                                    SecondColumnStyle="styleFieldAlign" ShowCustomerName="false" Caption="Company"
                                                                                    ShowCustomerCode="false" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldAlign">
                                                                <asp:Label ID="lblSLA" runat="server" ToolTip="Tranche No" CssClass="styleReqFieldLabel"
                                                                    Text="Tranche No"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <uc2:Suggest ID="ddlSLA" runat="server" ServiceMethod="GetTranche" AutoPostBack="true" ValidationGroup="Submit"
                                                                    OnItem_Selected="ddlSLA_SelectedIndexChanged" IsMandatory="true" ErrorMessage="Enter a valid Tranche No" />
                                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSLA"
                                                                    Display="None" InitialValue="0" ErrorMessage="Select Tranche No" ValidationGroup="Submit" />--%>
                                                                <%--<uc2:Suggest ID="" runat="server" ServiceMethod="GetTranche" AutoPostBack="true"
                                                                    OnItem_Selected="ddlSLA_SelectedIndexChanged" IsMandatory="true" ValidationGroup="Submit"
                                                                    ErrorMessage="Enter a Tranche No" />--%>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:Label ID="lblMLA" runat="server" ToolTip="Rental Schedule No"
                                                                    Text="Rental Schedule No"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <uc2:Suggest ID="ddlMLA" runat="server" ServiceMethod="GetRSBasedTranche" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldAlign">
                                                                <asp:Label ID="lblInsuranceDoneBy" runat="server" ToolTip="Insurance Done By" Text="Insurance Done By"
                                                                    CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlInsuranceDoneBy" runat="server" ToolTip="Insurance Done By"
                                                                    Width="165px" OnSelectedIndexChanged="ddlInsuranceDoneBy_SelectedIndexChanged"
                                                                    AutoPostBack="true">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvInsuranceDoneBy" runat="server" ControlToValidate="ddlInsuranceDoneBy"
                                                                    Display="None" InitialValue="0" ErrorMessage="Select Insurance Done By" ValidationGroup="Submit" />
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:Label ID="lblPayType" runat="server" ToolTip="Pay Type"
                                                                    Text="Pay Type" Visible="false"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="height: 22px">
                                                                <asp:DropDownList ID="ddlPayType" runat="server" ToolTip="Pay Type" Width="165px"
                                                                    Visible="false">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvPayType" runat="server" ControlToValidate="ddlPayType"
                                                                    Display="None" InitialValue="0" ErrorMessage="Select Pay Type" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:Label ID="LabelInsType" runat="server" ToolTip="Insurance Type" Text="Insurance Type" Visible="false"
                                                                    CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="DropDownListInsType" Visible="false" runat="server">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorInsType" runat="server" ControlToValidate="DropDownListInsType"
                                                                    Display="None" Enabled="false" InitialValue="0" ErrorMessage="Select Insurance Type" ValidationGroup="Submit" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldAlign">
                                                                <asp:Label ID="lblHeaderInstallmentFrom" runat="server" ToolTip="Installment From"
                                                                    Text="Installment From" Visible="false" CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlInstallmentFrom" Visible="false" runat="server" ToolTip="Installment From">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvInstallmentfrom" runat="server" ControlToValidate="ddlInstallmentFrom" Enabled="false"
                                                                    Display="None" InitialValue="0" ErrorMessage="Select Installment From" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:Label ID="lblHeaderInstallmentTo" runat="server" ToolTip="Installment To" Text="Installment To"
                                                                    Visible="false" CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <%--  <asp:TextBox ID="txtInstallmentTo" runat ="server" />--%>
                                                                <asp:DropDownList ID="ddlInstallmentTo" Visible="false" runat="server" ToolTip="Installment To">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvInstallmentto" runat="server" ControlToValidate="ddlInstallmentTo" Enabled="false"
                                                                    Display="None" InitialValue="0" ErrorMessage="Select Installment To" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                            <td width="50%" valign="top"></td>
                                        </tr>
                                    </table>
                                    <%--onresize="Resize();"--%>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlCurrentInsurance" runat="server" CssClass="stylePanel" GroupingText="Current Insurance policy"
                                                    Visible="true">
                                                    <asp:Panel ID="pnlInner" runat="server" CssClass="stylePanel" Visible="True" ScrollBars="Horizontal">
                                                        <div id="div" style="width: 1170px;" runat="server">
                                                            <asp:GridView ID="gvCurrentInsurance" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvCurrentInsurance_RowDataBound"
                                                                OnRowDeleting="gvCurrentInsurance_RowDeleting" ShowFooter="True" Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="PA_SA_REF_ID1" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRSNo" Text='<%#Bind("PA_SA_REF_ID") %>' runat="server" />
                                                                            <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                            <asp:Label ID="lblAssetNo" Text='<%#Bind("AssetNumber") %>' runat="server" />
                                                                            <asp:Label ID="lblAssetId" Text='<%#Bind("ASSETID") %>' runat="server" ToolTip="Asset Description" />
                                                                            <asp:Label ID="lblAssetDes" Text='<%#Bind("AssetDescription") %>' runat="server" />
                                                                            <asp:Label ID="lblCanEdit" Text='<%#Bind("CanEdit") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Invoice_ID" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInvid" Text='<%#Bind("Invoice_ID") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PA_SA_REF_ID" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPANUM" Text='<%#Bind("PA_SA_REF_ID") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="PolicyTypeID" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPolicyTypeId" Text='<%#Bind("PolicyTypeID") %>' runat="server"
                                                                                ToolTip="Policy Type" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rental Schedule No" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRentalSchedNo" Text='<%#Bind("PANUM") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Asset Description">
                                                                        <%-- <EditItemTemplate>
                                                                        <asp:Button ID="btnAssDesc" OnClick="btnAssDesc_Click" CssClass="styleSubmitShortButton" runat="server" Text="View" />
                                                                    </EditItemTemplate>--%>
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="btnFAssDesc" OnClick="btnFAssDesc_Click" CssClass="styleSubmitShortButton" runat="server" Text="View" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Asset Value">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAssetValue" Text='<%#Bind("ASSETVALUE") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Policy Type">
                                                                        <%-- <EditItemTemplate>
                                                                        <asp:Label ID="lblPolicyType" Text='<%#Bind("PolicyTypeID") %>' runat="server" ToolTip="Policy Type"
                                                                            Visible="false" />
                                                                        <asp:DropDownList ID="ddlPolicyType" Width="97%" runat="server">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvPolicyType" runat="server" ControlToValidate="ddlPolicyType"
                                                                            Display="None" ErrorMessage="Select Policy Type" InitialValue="0" ValidationGroup="Add">
                                                                        </asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>--%>
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="ddlFPolicyType" Width="97%" runat="server" ToolTip="Policy Type">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvPolicyType" runat="server" ControlToValidate="ddlFPolicyType"
                                                                                Display="None" ErrorMessage="Select Policy Type" InitialValue="0" ValidationGroup="Submit">
                                                                            </asp:RequiredFieldValidator>


                                                                            <%--<asp:DropDownList ID="ddlPolicyType" runat="server">
                                                                        </asp:DropDownList>--%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Policy Number">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtPolicyNumber" runat="server" MaxLength="50" Width="97%" Style="text-align: left" Text='<%#Bind("POLICYNUMBER") %>'
                                                                                ToolTip="Policy Number" onblur="FunCheckForZero(this,'Policy Number');"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="ftxtPolicyNumber" runat="server" FilterType="Numbers,Custom,UppercaseLetters,LowercaseLetters"
                                                                                TargetControlID="txtPolicyNumber">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <asp:RequiredFieldValidator ID="rfvtxtPolicyNumber" runat="server" ControlToValidate="txtPolicyNumber"
                                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Submit" SetFocusOnError="True"
                                                                                ErrorMessage="Enter Policy Number"></asp:RequiredFieldValidator>
                                                                        </ItemTemplate>
                                                                        <%--<EditItemTemplate>
                                                                        <asp:TextBox ID="txtPolicyNumberE" runat="server" ToolTip="Policy Number" MaxLength="16" Width="97%"
                                                                            Style="padding-left: 5px" Text='<%# Bind("PolicyNo") %>' onblur="FunCheckForZero(this,'Policy Number');"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftxtPolicyNumber" runat="server" FilterType="Numbers,Custom,UppercaseLetters, LowercaseLetters"
                                                                            TargetControlID="txtPolicyNumberE">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtPolicyNumber" runat="server" ControlToValidate="txtPolicyNumberE"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Add" SetFocusOnError="True"
                                                                            ErrorMessage="Enter Policy Number"></asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>--%>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Policy Date">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtPolicyDate" runat="server" ToolTip="PolicyDate" Width="97%" Text='<%# Eval("POLICYDATE") %>'></asp:TextBox>
                                                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Policy Date"
                                                                                Visible="false" />
                                                                            <cc1:CalendarExtender ID="CEPolicyDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                                                PopupButtonID="Image1" TargetControlID="txtPolicyDate">
                                                                            </cc1:CalendarExtender>
                                                                            <asp:RequiredFieldValidator ID="rfvtxtPolicyDate" runat="server" ControlToValidate="txtPolicyDate"
                                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Submit" SetFocusOnError="True"
                                                                                ErrorMessage="Enter Policy Date"></asp:RequiredFieldValidator>
                                                                            <%--<asp:Label ID="lblPolicyDate" Text='<%# Eval("PolicyDate") %>'
                                                                            runat="server" ToolTip="PolicyDate" />--%>
                                                                        </ItemTemplate>
                                                                        <%--  <EditItemTemplate>
                                                                        <asp:TextBox ID="txtPolicyDateE" runat="server" Width="97%" Text='<%# Eval("PolicyDate") %>'
                                                                            ToolTip="PolicyDate"></asp:TextBox>
                                                                        <asp:Image ID="imgPolicyDateEdit" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                                            ToolTip="Select Policy Date" Visible="false" />
                                                                        <cc1:CalendarExtender ID="calPolicyDateEdit" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                                            PopupButtonID="imgPolicyDateEdit" TargetControlID="txtPolicyDateE">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtPolicyDateE" runat="server" ControlToValidate="txtPolicyDateE"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Add" SetFocusOnError="True"
                                                                            ErrorMessage="Enter Policy Date"></asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>--%>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Valid Till Date">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtValidTill" runat="server" ToolTip="Valid Till Date" Width="97%" Text='<%# Eval("VALIDTILLDATE") %>'></asp:TextBox>
                                                                            <asp:Image ID="imgValidDate" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Valid Till"
                                                                                Visible="false" />
                                                                            <cc1:CalendarExtender ID="calValidDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                                                PopupButtonID="imgValidDate" TargetControlID="txtValidTill">
                                                                            </cc1:CalendarExtender>
                                                                            <asp:RequiredFieldValidator ID="rfvtxtValidTill" runat="server" ControlToValidate="txtValidTill"
                                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Submit" SetFocusOnError="True"
                                                                                ErrorMessage="Enter Valid Till Date"></asp:RequiredFieldValidator>
                                                                            <%-- <asp:CompareValidator ID="cvtxtStartDate" runat="server" ValidationGroup="Submit" 
                                                                            ControlToCompare="txtPolicyDate" ControlToValidate="txtValidTill"
                                                                            ErrorMessage="Valid date must be greater than policy date" Type="Date" Display="None" Operator="LessThan"></asp:CompareValidator>--%>

                                                                            <%--       <asp:CompareValidator ID="CompareValidatorT1" runat="server" ControlToValidate="txtValidTill"
                                                                            ControlToCompare="txtPolicyDate" Type="Date" ValidationGroup="Submit" Operator="GreaterThanEqual"
                                                                            ErrorMessage="Invalid Date Range" SetFocusOnError="True" Display="None"></asp:CompareValidator>--%>

                                                                            <%-- <asp:Label ID="lblValidTill" Text='<%# Eval("ValidTill") %>'
                                                                            runat="server" />--%>
                                                                        </ItemTemplate>
                                                                        <%-- <EditItemTemplate>
                                                                        <asp:TextBox ID="txtValidTillE" runat="server" Width="97%" Text='<%# Eval("ValidTill") %>'
                                                                            ToolTip="Valid Till Date"></asp:TextBox>
                                                                        <asp:Image ID="imgValidTillEdit" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                                            ToolTip="Select Valid Till Date" Visible="false" />
                                                                        <cc1:CalendarExtender ID="calValidDateEdit" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                                            PopupButtonID="imgValidTillEdit" TargetControlID="txtValidTillE">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtValidTillE" runat="server" ControlToValidate="txtValidTillE"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Add" SetFocusOnError="True"
                                                                            ErrorMessage="Enter Valid Till Date"></asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>--%>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Loss Payee Details">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtLoss_Payee_Details" Width="97%" MaxLength="100" runat="server" Text='<%# Bind("LOSS_PAYEE_DETAILS") %>'></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLoss_Payee_Details"
                                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Submit" SetFocusOnError="True"
                                                                                ErrorMessage="Enter Loss Payee Details"></asp:RequiredFieldValidator>
                                                                            <%-- <asp:Label ID="lblLossPayeeDetails" Text='<%# Bind("LOSS_PAYEE_DETAILS") %>'
                                                                            runat="server" />--%>
                                                                        </ItemTemplate>
                                                                        <%--  <EditItemTemplate>
                                                                        <asp:TextBox ID="txtLoss_Payee_DetailsE" runat="server" Width="97%" Text='<%# Bind("LOSS_PAYEE_DETAILS") %>'
                                                                            ToolTip="Loss Payee Details"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLoss_Payee_DetailsE"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Add" SetFocusOnError="True"
                                                                            ErrorMessage="Enter Loss Payee Details"></asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>--%>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Policy Value">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtPolicyValue" Text='<%# Bind("POLICYVALUE") %>' MaxLength="10" Width="97%" runat="server"
                                                                                ToolTip="Policy Value" Style="text-align: right"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvtxtPolicyValue" runat="server" ControlToValidate="txtPolicyValue"
                                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Submit" SetFocusOnError="True"
                                                                                ErrorMessage="Enter Policy Value"></asp:RequiredFieldValidator>
                                                                            <cc1:FilteredTextBoxExtender ID="ftxtPolicyValue" runat="server" FilterType="Custom,Numbers"
                                                                                TargetControlID="txtPolicyValue">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <%--<asp:Label ID="lblPolicyValue" Text='<%#Bind("PolicyValue") %>' Style="text-align: right;"
                                                                            runat="server" ToolTip="Policy Value" />--%>
                                                                        </ItemTemplate>
                                                                        <%--<EditItemTemplate>
                                                                        <asp:TextBox ID="txtPolicyValueE" Width="97%" MaxLength="10" runat="server" Style="text-align: right;" Text='<%# Bind("PolicyValue") %>'
                                                                            ToolTip="PolicyValue" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftxtPolicyValueE" runat="server" FilterType="Custom,Numbers"
                                                                            ValidChars="." TargetControlID="txtPolicyValueE">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtPolicyValueE" runat="server" ControlToValidate="txtPolicyValueE"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Add" SetFocusOnError="True"
                                                                            ErrorMessage="Enter Policy Value"></asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>--%>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Premium">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtPremium" Text='<%#Bind("PREMIUM") %>' runat="server" ToolTip="Premium" Width="97%" Style="text-align: right" MaxLength="10">
                                                                            </asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvtxtPremium" runat="server" ControlToValidate="txtPremium" Enabled="false"
                                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Submit" SetFocusOnError="True"
                                                                                ErrorMessage="Enter Premium"></asp:RequiredFieldValidator>
                                                                            <cc1:FilteredTextBoxExtender ID="ftxtPremium" runat="server" FilterType="Custom,Numbers"
                                                                                TargetControlID="txtPremium">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <%-- <asp:Label ID="lblPremium" Text='<%#Bind("Premium") %>' runat="server" Style="text-align: right" />--%>
                                                                        </ItemTemplate>
                                                                        <%--<EditItemTemplate>
                                                                        <asp:TextBox ID="txtPremiumE" runat="server" Style="padding-left: 5px; text-align: right" MaxLength="10" Width="97%"
                                                                            Text='<%# Bind("Premium") %>' ToolTip="Premium"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftxtPremiumE" runat="server" FilterType="Custom,Numbers"
                                                                            ValidChars="." TargetControlID="txtPremiumE">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtPremiumE" runat="server" ControlToValidate="txtPremiumE"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Add" SetFocusOnError="True"
                                                                            ErrorMessage="Enter Premium"></asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>--%>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Remarks">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtFRemarks" MaxLength="100" Text='<%#Bind("REMARKS") %>' runat="server"
                                                                                TextMode="MultiLine" onkeyup="maxlengthfortxt(100);" Height="40px" ToolTip="Remarks" Width="97%">
                                                                            </asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFRemarks"
                                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Submit" SetFocusOnError="True" Enabled="false"
                                                                                ErrorMessage="Enter Remarks"></asp:RequiredFieldValidator>
                                                                            <%-- <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Custom,Numbers"
                                                    ValidChars="." TargetControlID="txtFRemarks">
                                                </cc1:FilteredTextBoxExtender>--%>
                                                                            <%-- <asp:Label ID="lblRemarks" Text='<%#Bind("REMARKS") %>' runat="server" />--%>
                                                                        </ItemTemplate>
                                                                        <%--<EditItemTemplate>
                                                                        <asp:TextBox ID="txtREmarksE" runat="server" Style="padding-left: 5px;" Width="97%"
                                                                            Text='<%# Bind("REMARKS") %>' ToolTip="Remarks" TextMode="Multiline"></asp:TextBox>
                                                                        <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom,Numbers"
                                                    ValidChars="." TargetControlID="txtREmarks">
                                                </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtREmarksE"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Add" SetFocusOnError="True"
                                                                            ErrorMessage="Enter Remarks"></asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>--%>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="false" CommandName="Edit"
                                                                                Text="Edit" Visible="false" />
                                                                            <asp:LinkButton ID="lnkRemove" runat="server" CommandName="Delete" Visible="false"
                                                                                Text="Delete" />
                                                                        </ItemTemplate>
                                                                        <%-- <EditItemTemplate>
                                                                        <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="true" ValidationGroup="Add"
                                                                            CommandName="Update" Text="Update" />
                                                                        <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="false" CommandName="Cancel"
                                                                            Text="Cancel" />
                                                                    </EditItemTemplate>--%>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Details Id" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDetailsId" Text='<%#Bind("DetailsId") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="styleGridHeader" width="98%">
                                                <asp:Label ID="lblTotal" runat="server" Text="Total Premium  :" Font-Bold="True"></asp:Label>
                                                <asp:Label ID="lblTotalPremium" runat="server" Font-Bold="True" Text="0"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100%">
                                                <asp:Panel ID="pnlInsuranceHistory" Visible="false" runat="server" CssClass="stylePanel"
                                                    GroupingText="Insurance History" Width="99%">
                                                    <div id="div11" style="overflow: auto; width: 98%; padding-left: 1%;" runat="server">
                                                        <asp:GridView ID="gvInsuranceHistory" runat="server" AutoGenerateColumns="false"
                                                            Visible="true" Width="99%" OnRowDataBound="gvInsuranceHistory_RowDataBound">
                                                            <Columns>
                                                                <%-- <asp:TemplateField HeaderText="S.No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                                <asp:BoundField HeaderText="AINSE Number" DataField="AINSE" />
                                                                <asp:BoundField HeaderText="Asset Description" Visible="false" DataField="Asset Description" />
                                                                <asp:BoundField HeaderText="Rental Schedule No" DataField="PANUM" />
                                                                <asp:BoundField HeaderText="Policy Type" DataField="PolicyType" />
                                                                <asp:BoundField HeaderText="Regn No or Serial No" Visible="false" DataField="MachineNo" />
                                                                <asp:BoundField HeaderText="Policy Number" DataField="Policy No" />
                                                                <asp:BoundField HeaderText="Policy Date" DataField="PolicyDate" />
                                                                <asp:BoundField HeaderText="Valid Till Date" DataField="ValidTill" />
                                                                <asp:TemplateField HeaderText="Policy Value">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPolicyValue" runat="server" Text='<%# Eval("PolicyValue") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Premium">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPremium" runat="server" Text='<%# Eval("Premium") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%-- <asp:BoundField HeaderText="PolicyValue" DataField='<%# Eval("PolicyValue").ToString(Funsetsuffix) %>' ItemStyle-HorizontalAlign ="Right"  />
                                                              <asp:BoundField HeaderText="Premium" DataField="Premium" ItemStyle-HorizontalAlign ="Right" />--%>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table width="100%" align="center">
                            <tr>
                                <td align="center">&nbsp;&nbsp;<asp:Button ID="btnSave" runat="server" OnClick="btnSave_OnClick" CausesValidation="true"
                                    CssClass="styleSubmitButton" Text="Save" ValidationGroup="Submit" OnClientClick="return fnCheckPageValidators('Submit');" />
                                    &nbsp;<asp:Button ID="btnClear" runat="server" CausesValidation="False" OnClientClick="return fnConfirmClear();"
                                        OnClick="btnClear_OnClick" CssClass="styleSubmitButton" Text="Clear" />
                                    &nbsp;<asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_OnClick" CausesValidation="False"
                                        CssClass="styleSubmitButton" Text="Cancel" />
                                     <asp:Button ID="Button1" runat="server" CssClass="styleSubmitButton" Text="Save"
                                        CausesValidation="False" OnClick="Button1_Click" Style="display: none;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldAlign"></td>
                            </tr>
                        </table>

                        <asp:CustomValidator ID="cvAssetInsuranceEntry" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true"></asp:CustomValidator>
                        <asp:ValidationSummary ID="vsInsurancePolicy" runat="server" CssClass="styleMandatoryLabel"
                            ValidationGroup="Submit" HeaderText="Correct the following validation(s):" />
                        <asp:ValidationSummary ID="vsPolicyDetails" runat="server" CssClass="styleMandatoryLabel"
                            ValidationGroup="Add" HeaderText="Correct the following validation(s):" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <%--Changed by Chandru K on 17 Aug 2016 For Bug Id : 4835--%>
            <%--<asp:PostBackTrigger ControlID="btnLoadCustomer" />
            <asp:PostBackTrigger ControlID="ddlSLA" />
            <asp:PostBackTrigger ControlID="ddlMLA" />--%>
            <asp:PostBackTrigger ControlID="ddlPayTo" />
            <asp:PostBackTrigger ControlID="ddlPayType" />
            <asp:PostBackTrigger ControlID="ddlInsuranceDoneBy" />
            <asp:PostBackTrigger ControlID="btnModal1" />
            <asp:PostBackTrigger ControlID="gvCurrentInsurance" />
            <asp:PostBackTrigger ControlID="btnSave" />
            <asp:PostBackTrigger ControlID="btnClear" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:Button ID="btnModal1" Style="display: none" runat="server" />

    <cc1:ModalPopupExtender ID="ModalPopupExtenderDEVApprover" runat="server" TargetControlID="btnModal1"
        PopupControlID="PnlApprover" BackgroundCssClass="styleModalBackground" Enabled="true">
    </cc1:ModalPopupExtender>

    <asp:Panel ID="PnlApprover" Visible="false" Style="vertical-align: middle" runat="server"
        BorderStyle="Solid" BackColor="White" Width="610px">

        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td>
                            <div id="div1" class="container" runat="server" style="width: auto; overflow: scroll; height: 500px">
                                <asp:GridView Width="98%" ID="grvRange" runat="server" AutoGenerateColumns="false" OnRowDataBound="grvRange_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Rental Schedule No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRentalSchedNo" Text='<%#Bind("PANUM") %>' runat="server" />
                                                <asp:Label ID="lblPASAREFID" Text='<%#Bind("PA_SA_REF_ID") %>' Visible="false" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Invoice No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvID" runat="server" Text='<%#Bind("INVOICE_NO") %>'></asp:Label>
                                                <asp:Label ID="lblChkSelect" Visible="false" runat="server" Text='<%#Bind("ACTIVE") %>'></asp:Label>
                                                <asp:Label ID="lblInvoiceID" Visible="false" runat="server" Text='<%#Bind("INVOICE_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvDate" runat="server" Text='<% #Bind("INVOICE_DATE")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Category">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssCatg" runat="server" Text='<% #Bind("ASSET_CATEGORY")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssType" runat="server" Text='<% #Bind("ASSET_TYPE")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Desc">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssDesc" runat="server" Text='<% #Bind("ASSET_DESCRIPTION")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Value">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssValue" runat="server" Text='<% #Bind("ASSET_VALUE")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Select All
                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSaveRange" OnClick="btnSaveRange_OnClick" runat="server" Text="Ok"
                                ToolTip="Save" class="styleSubmitButton" />
                            <asp:Button ID="btnCancelRange" runat="server" Text="Close" ToolTip="Close" class="styleSubmitButton"
                                OnClick="btnCancelRange_Click" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSaveRange" />
            </Triggers>
        </asp:UpdatePanel>

    </asp:Panel>

    <script language="javascript" type="text/javascript">

        function fnLoadCustomer() {
            document.getElementById('ctl00_ContentPlaceHolder1_btnLoadCustomer').click();
        }


        function Resize() {

            //   document.getElementById('ctl00_ContentPlaceHolder1_pnlCurrentInsurance').style.width = screen.width - 50;

        }

        function ReHeight(ht) {
            //  (document.getElementById('ctl00_ContentPlaceHolder1_pnlInner')).style.height = ht + 'px';
        }


        function showMenu(show) {
            if (show == 'T') {

                //Added by Kali K to solve error ( when menu is show scroll should appear in grid )
                //This is used to show scroll bar when div is used in GridView
                //if (document.getElementById('divGrid1') != null) {
                //    document.getElementById('divGrid1').style.width = "800px";
                //    document.getElementById('divGrid1').style.overflow = "scroll";
                //}

                //document.getElementById('divMenu').style.display = 'Block';
                //document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

                //document.getElementById('ctl00_imgShowMenu').style.display = 'none';
                //document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

                //(document.getElementById('ctl00_ContentPlaceHolder1_pnlCurrentInsurance')).style.width = screen.width - 260;
            }
            if (show == 'F') {

                //Added by Kali K to solve error ( when menu is hide scroll for is not hiding from grid view )
                //This is used to hide scroll bar when div is used in GridView
                //if (document.getElementById('divGrid1') != null) {
                //    document.getElementById('divGrid1').style.width = "960px";
                //    document.getElementById('divGrid1').style.overflow = "auto";
                //}

                //document.getElementById('divMenu').style.display = 'none';
                //document.getElementById('ctl00_imgHideMenu').style.display = 'none';
                //document.getElementById('ctl00_imgShowMenu').style.display = 'Block';

                //document.getElementById('divMenu').setAttribute('width', 0);

                // (document.getElementById('ctl00_ContentPlaceHolder1_pnlCurrentInsurance')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
            }
        }

        function funCheckFirstLetterisNumeric(textbox, msg) {

            var FieldValue = new Array();
            FieldValue = textbox.value.trim();
            if (FieldValue.length > 0 && FieldValue.value != '') {
                if (isNaN(FieldValue[0])) {
                    return true;
                }
                else {
                    alert(msg + ' name cannot begin with a number');
                    textbox.focus();
                    textbox.value = '';
                    event.returnValue = false;
                    return false;
                }
            }
        }

        function FunCheckForZero(input, strName) {
            var txtBoxVal = input.value;
            if (txtBoxVal != '') {
                if (!isNaN(txtBoxVal)) {
                    if (txtBoxVal == 0) {
                        alert(strName + ' cannot be Zero');
                        input.focus();
                    }
                }
            }
        }

    </script>

</asp:Content>
