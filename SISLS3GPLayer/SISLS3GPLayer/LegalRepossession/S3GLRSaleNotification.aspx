<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLRSaleNotification.aspx.cs" Inherits="LegalRepossession_S3GLRSaleNotification"
    Title="Sale Notification" %>
    <%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
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
                    <td>
                        <asp:Panel ID="pnlGeneral" runat="server" CssClass="stylePanel" GroupingText="General">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <%-- LOB --%>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="True" ToolTip="Line of Business"
                                            TabIndex="1" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVLOB" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlLOB" InitialValue="0" ValidationGroup="VGNoc" ErrorMessage="Select a Line of Business"
                                            Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <%--Branch--%>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Location" ID="lblBranch" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                     <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                                        OnItem_Selected="ddlBranch_SelectedIndexChanged" IsMandatory="true" ValidationGroup="Submit" ErrorMessage="Enter Location" />
                                        <%--<asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" ToolTip="Branch"
                                            TabIndex="2" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVNocBranch" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlBranch" InitialValue="0" ValidationGroup="VGNoc" ErrorMessage="Select a Location"
                                            Display="None"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <%-- Second Row --%>
                                <tr>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Repossession Doc No" ID="Label1" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:DropDownList ID="ddlrepdocno" runat="server" AutoPostBack="True" ToolTip="Repssession Docket No"
                                            TabIndex="4" OnSelectedIndexChanged="ddlrepdocno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVReportDocNo" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlrepdocno" InitialValue="0" ValidationGroup="VGNoc" ErrorMessage="Select a Repossession Document No"
                                            Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="SN Date" ID="lblNocDate" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtSNDate" ContentEditable="False" ToolTip="SN Date"
                                            Width="50%"></asp:TextBox>
                                    </td>
                                </tr>
                                <%-- Third Row--%>
                                <tr>
                                    <%--MLA --%>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Prime Account Number" ID="lblprimeaccno" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtPAN" ReadOnly="True" ToolTip="Prime A/c Number"></asp:TextBox>
                                    </td>
                                    <%--SLA--%>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Sub Account Number" ID="lblsubAccno" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtSAN" ReadOnly="True" ToolTip="Sub A/c Number"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="SN Number" ID="lblNocNo" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtSNNo" ReadOnly="True" ToolTip="SN Number"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label ID="lblActive" runat="server" CssClass="styleDisplayLabel" Text="Active"> </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlCommAddress" runat="server" CssClass="stylePanel" GroupingText="Customer Information"
                            Width="99%">
                            <table cellpadding="0" cellspacing="0" width="99%">
                                <tr>
                                    <td colspan="4">
                                        <asp:HiddenField ID="hidcuscode" runat="server" />
                                        <asp:HiddenField ID="hidcusID" runat="server" />
                                        <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" ActiveViewIndex="1" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <%--Gridview --%>
                <tr>
                    <%--<td style="padding-top: 10px">--%>
                    <td>
                        <asp:Panel ID="PNLAssetDetails" runat="server" CssClass="stylePanel" GroupingText="Asset Details"
                            Width="98%" Visible="false">
                            <table cellpadding="0" cellspacing="0" width="98%">
                                <tr>
                                    <td colspan="4 " width="80%">
                                        <%--<div style="overflow-x: hidden; overflow-y: auto; height: 225px;">--%>
                                        <asp:GridView runat="server" ID="GRVAssetDetails" AutoGenerateColumns="False" Width="100%"
                                            ToolTip="Asset Details" OnRowDataBound="GRVAssetDetails_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Asset Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtAssetCode" runat="server" Text='<%# Bind("Asset_Code")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Asset Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtAssetDesc" runat="server" Text='<%# Bind("Asset_Description")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Asset Reg Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtAssetRegNo" runat="server" Text='<%# Bind("Regno")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Repossessed Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRepDate" runat="server" Text='<%# Bind("Repossession_Date")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Latest Market Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtMarketvalue" runat="server" Text='<%# Bind("Current_Market_Value")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleGridHeader" />
                                            <RowStyle HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <%--</div>--%>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <%--<td style="padding-top: 10px">--%>
                    <td>
                        <asp:Panel ID="pnlGridview" runat="server" CssClass="stylePanel" GroupingText="Sale Notification Details"
                            Width="98%">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView ID="GRVSaleBid" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                                            ToolTip="Sale Bid" Width="99%" OnRowCommand="GRVSaleBid_RowCommand" OnRowDeleting="GRVSaleBid_RowDeleting"
                                            OnRowDataBound="GRVSaleBid_RowDataBound" OnRowCreated="GRVSaleBid_RowCreated">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl.No" ItemStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Sale Bid Published">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSaleBidpub" runat="server" Text='<%#Bind("SaleBidpub")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtSaleBidpub" runat="server" MaxLength="30" Text='<%# Bind("SaleBidpub")%>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvsalebid" runat="server" ControlToValidate="txtSaleBidpub"
                                                            SetFocusOnError="True" CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Sale Bid Published"
                                                            ValidationGroup="Add"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                    <HeaderStyle Width="20%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Date of Published">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldataofpub" runat="server" Text='<%#Bind("dataofpub")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtdataofpub" runat="server" Text='<%# Bind("dataofpub")%>' ></asp:TextBox>
                                                        <cc1:CalendarExtender runat="server" TargetControlID="txtdataofpub" ID="CalendarExtenderSD_FollowupDate"
                                                            Enabled="True">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="rfvDateofpub" runat="server" ControlToValidate="txtdataofpub"
                                                            SetFocusOnError="True" CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Date of Published"
                                                            ValidationGroup="Add"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                    <HeaderStyle Width="20%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRemarks" Width="100%" runat="server" MaxLength="100" Text='<%# Bind("Remarks")%>'
                                                            TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                                        <%--<asp:Label ID="lblRemarks" runat="server" Text='<%#Bind("Remarks")%>' Width="50%" ></asp:Label>--%>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" Text='<%# Bind("Remarks")%>'
                                                            TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <HeaderStyle Width="20%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Floor Price">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFloorPrice" runat="server" Text='<%#Bind("FloorPrice")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFloorPrice" runat="server" MaxLength="19" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                            Text='<%# Bind("FloorPrice")%>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvFloor" runat="server" ControlToValidate="txtFloorPrice"
                                                            SetFocusOnError="True" CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Floor Price"
                                                            ValidationGroup="Add"></asp:RequiredFieldValidator>
                                                        <cc1:FilteredTextBoxExtender ID="FTEFloorPrice" runat="server" TargetControlID="txtFloorPrice"
                                                            FilterType="Numbers,Custom" Enabled="true" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="right"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <%--<asp:LinkButton Text="Update"  runat="server" ID="lnkbtnDelete" CommandName="Delete"></asp:LinkButton>--%>
                                                        <asp:LinkButton ID="lnkbtnDelete" runat="server" CausesValidation="false"  CommandName="Delete" Text="Delete" 
                                                         OnClientClick="return confirm('Are you sure want to delete?');"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="btnAddrowvalue" runat="server" Text="Add" CommandName="AddNew"
                                                            OnClientClick="return fnCheckPageValidators('Add',false);" CausesValidation="false"></asp:LinkButton>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleGridHeader" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <%--Total--%>
              
                <tr class="styleButtonArea" align="center" style="padding-left: 4px">
                    <td>
                        <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators('VGNoc');"
                            CssClass="styleSubmitButton" Text="Save" ValidationGroup="VGNOC" ToolTip="Save"
                            TabIndex="5" OnClick="btnSave_Click" />
                        &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="return fnConfirmClear();" ToolTip="Clear" TabIndex="6"
                            OnClick="btnClear_Click" />
                        &nbsp;<asp:Button runat="server" ID="btnCancel" Text="Cancel" ValidationGroup="VGNOC"
                            CausesValidation="false" CssClass="styleSubmitButton" ToolTip="Cancel" TabIndex="7"
                            OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:ValidationSummary runat="server" ID="vsFactoringInvoiceLoadinf" HeaderText="Correct the following validation(s):"
                            Height="177px" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" ValidationGroup="VGNoc" />
                        <asp:ValidationSummary runat="server" ID="ValidationSummary1" HeaderText="Correct the following validation(s):"
                            Height="177px" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" ValidationGroup="Add" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                        <asp:HiddenField ID="HiddenField2" runat="server" />
                        <asp:CustomValidator ID="cvCustomerMaster" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
