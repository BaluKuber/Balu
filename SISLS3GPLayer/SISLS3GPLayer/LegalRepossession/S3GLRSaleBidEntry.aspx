<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="~/LegalRepossession/S3GLRSaleBidEntry.aspx.cs" Inherits="LegalRepossession_S3GLRSaleBidEntry" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<asp:Content ID="ContentSalesBid" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table align="center" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td class="stylePageHeading">
                <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 10px">
                <asp:Panel ID="PnlAssetDetails" runat="server" GroupingText="Asset Details" CssClass="stylePanel">
                    <asp:UpdatePanel ID="UpdatePanelAsset" runat="server">
                        <ContentTemplate>
                            <table cellspacing="0">
                                <tr>
                                    <td class="styleFieldLabel" width="8%">
                                        <asp:Label ID="LblSBidBidNo" runat="server" CssClass="styleDisplayLabel" Text="Sales Bid Number"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign" width="7%">
                                        <asp:TextBox ID="TxtSBidBidNo" runat="server" MaxLength="12" Width="180px" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldAlign" width="8%">
                                        &nbsp;
                                    </td>
                                    <td class="styleFieldAlign" width="8%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="7%">
                                        <asp:Label ID="LblSBidLOB" runat="server" CssClass="styleDisplayLabel" Text="Line of Business"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign" width="10%">
                                        <asp:DropDownList ID="ddSBidLOB" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddSBidlLOB_SelectIndexChanged"
                                            Visible="true" Width="200px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddSBidlLOB" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddSBidLOB" InitialValue="0" ErrorMessage="Select the Line of Business"
                                            ValidationGroup="Submit" Display="None"> </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" width="4%">
                                        <asp:Label ID="LblSBidBranch" runat="server" Text="Location" CssClass="styleDisplayLabel"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldLabel" width="7%">
                                    
                                    <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                                        OnItem_Selected="ddlSBidBranch_SelectedIndexChanged" IsMandatory="true" ValidationGroup="Submit" ErrorMessage="Enter Location" />
                                    <%--    <asp:DropDownList ID="ddlSBidBranch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSBidBranch_SelectedIndexChanged"
                                            Visible="true" Width="200px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlSBidBranch" runat="server" ControlToValidate="ddlSBidBranch"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Location"
                                            InitialValue="0" ValidationGroup="Submit">
                                        </asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="7%">
                                        <asp:Label ID="LblSBidSSNo" runat="server" CssClass="styleDisplayLabel" Text="Sale Notification Number"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign" width="10%">
                                        <asp:DropDownList ID="ddlSBidSSNo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSBidSSNo_SelectIndexChanged"
                                            Visible="true">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlSBidSSNo" runat="server" ControlToValidate="ddlSBidSSNo"
                                            Display="None" ErrorMessage="Select the Sale Notification Number" ValidationGroup="Submit"
                                            CssClass="styleMandatoryLabel" InitialValue="0"> </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:Label ID="lblSBidIsActive" runat="server" Text="Active" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:CheckBox ID="ChkSBidIsActive" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="LblSBidMLA" runat="server" CssClass="styleDisplayLabel" Text="Prime Account Number"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign" width="10%">
                                        <asp:TextBox ID="TxtSBidMLANo" runat="server" MaxLength="100" Width="180px" ContentEditable="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTxtSBidMLANo" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                            TargetControlID="TxtSBidMLANo" ValidChars="-,/, ">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvTxtSBidMLANo" runat="server" ControlToValidate="TxtSBidMLANo"
                                            Display="None" ErrorMessage="Enter the Prime Account Number" SetFocusOnError="True"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" width="7%">
                                        <asp:Label ID="LblSBidSLANo" runat="server" CssClass="styleDisplayLabel" Text="Sub Account Number"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="TxtSBidSLANo" runat="server" MaxLength="100" Width="180px" ContentEditable="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTxtSBidSLANo" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                            TargetControlID="TxtSBidSLANo" ValidChars="-,/, ">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvTxtSBidSLANo" runat="server" ControlToValidate="TxtSBidSLANo"
                                            Display="None" ErrorMessage="Enter the Sub Account Number" SetFocusOnError="True"
                                            ValidationGroup="Submit" Enabled="False"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="7%">
                                        <asp:Label ID="LblSBidRepDocNo" runat="server" CssClass="styleDisplayLabel" Text="Repossession Docket No"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="TxtSBidRepDocNo" runat="server" MaxLength="100" ContentEditable="false"
                                            Width="180px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTxtSBidRepDocNo" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="TxtSBidRepDocNo"
                                            ValidChars="-,/, ">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvTxtSBidRepDocNo" runat="server" ControlToValidate="TxtSBidRepDocNo"
                                            Display="None" ErrorMessage="Enter the Repossession Docket Number" SetFocusOnError="True"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" width="7%">
                                        <asp:Label ID="LblSBidRefNo" runat="server" CssClass="styleDisplayLabel" Text="Reference Number"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign" width="10%">
                                        <asp:TextBox ID="TxtSBidRefNo" runat="server" MaxLength="100" ContentEditable="false"
                                            Width="180px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTxtSBidRefNo" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                            TargetControlID="TxtSBidRefNo" ValidChars="-, ">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvTxtSBidRefNo" runat="server" ControlToValidate="TxtSBidRefNo"
                                            Display="None" ErrorMessage="Enter the Reference Number" SetFocusOnError="True"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" colspan="4" style="width: 30%">
                                        <asp:Panel ID="pnlSBidCustInfo" runat="server" CssClass="stylePanel" GroupingText="Customer Information"
                                            Width="99%">
                                            <table cellpadding="0" cellspacing="0" width="99%">
                                                <tr>
                                                    <td>
                                                        <uc1:S3GCustomerAddress ID="UcSBiddCustomerDetails" runat="server" ActiveViewIndex="1" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" colspan="4">
                                        <asp:Panel ID="PnlAssetInfo" runat="server" CssClass="stylePanel" GroupingText="Asset Information"
                                            Visible="true" Width="99%">
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <%--<div style="overflow-x: hidden; overflow-y: auto; height: 225px;">--%>
                                                        <asp:GridView ID="GRVAssetDetails" runat="server" AutoGenerateColumns="False" ToolTip="Asset Information"
                                                            Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Asset Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtAssetCode" runat="server" Text='<%# Bind("Asset_Code")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Asset Description">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtAssetDesc" runat="server" Text='<%# Bind("Asset_Description")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Asset Reg Number">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtAssetRegNo" runat="server" Text='<%# Bind("Regno")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Repossessed Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRepDate" runat="server" Text='<%# Bind("Repossession_Date")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Latest Market Value">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtMarketvalue" runat="server" Text='<%# Bind("Current_Market_Value")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
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
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:Panel ID="PnlBidDetails" runat="server" GroupingText="Bidder Details" CssClass="stylePanel"
                    ScrollBars="Horizontal" Width="1000px">
                    <asp:UpdatePanel ID="UpdatePaneBid" runat="server">
                        <ContentTemplate>
                            <%--  <table width="100%">
                                <tr>
                                    <td align="left">--%>
                            <asp:GridView ID="grvBidDetails" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                ToolTip="Sale Bid Entry" OnRowCommand="grvBidDetails_RowCommand" OnRowDeleting="grvBidDetails_RowDeleting"
                                OnRowDataBound="grvBidDetails_RowDataBound" OnRowCreated="grvBidDetails_RowCreated">
                                <Columns>
                                    <asp:TemplateField HeaderText="Bid No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSBidBidNO" runat="server" Text='<%# Bind("BID_NO") %>' Style="text-align: right"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="1%" />
                                        <HeaderStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bidder Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSBidBidderName" runat="server" Text='<%# Bind("BIDDER_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="TxtSBidBidderName" runat="server" Text='<%# Bind("BIDDER_NAME") %>'
                                                MaxLength="25" Width="98%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTxtSBidBidderName" runat="server" TargetControlID="TxtSBidBidderName"
                                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" Enabled="true"
                                                ValidChars="/,\,(,),. ">
                                            </cc1:FilteredTextBoxExtender>
                                        </FooterTemplate>
                                        <ItemStyle VerticalAlign="Top" Wrap="true" />
                                        <HeaderStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bidder Address">
                                        <ItemTemplate>
                                            <%--  <asp:Label ID="lblSBidCntctDtls" runat="server" Text='<%# Bind("BIDDER_ADDRESS") %>'></asp:Label>--%>
                                            <asp:TextBox ID="txtbidderaddress" Width="98%" runat="server" MaxLength="100" Text='<%# Bind("BIDDER_ADDRESS")%>'
                                                TextMode="MultiLine" ReadOnly="true" Wrap="true"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="TxtSBidCntctDtls" runat="server" onkeyup="maxlengthfortxt(60)" onchange="maxlengthfortxt(60)"
                                                TextMode="MultiLine" Text='<%# Bind("BIDDER_ADDRESS") %>' Width="98%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTxtSBidCntctDtls" runat="server" TargetControlID="TxtSBidCntctDtls"
                                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" Enabled="true"
                                                ValidChars="/,\,(,),. ">
                                            </cc1:FilteredTextBoxExtender>
                                        </FooterTemplate>
                                        <ItemStyle VerticalAlign="Top" Wrap="true" />
                                        <HeaderStyle Width="30%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bid Valid Upto">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSBidValidity" runat="server" Text='<%# Bind("VALIDITY") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="TxtSBidValidity" runat="server"  Text='<%# Bind("VALIDITY") %>'
                                                Width="98%" OnTextChanged="TxtSBidValidity_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <cc1:CalendarExtender runat="server" TargetControlID="TxtSBidValidity" ID="CalendarExtenderSD_TxtSBidValidity"
                                                Enabled="True" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                        </FooterTemplate>
                                        <ItemStyle VerticalAlign="Top" Wrap="true" />
                                        <HeaderStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bid Value">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSBidBidValue" runat="server" Text='<%# Bind("BID_VALUE") %>' Style="text-align: right"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="TxtSBidBidValue" runat="server" Text='<%# Bind("BID_VALUE") %>'
                                                Style="text-align: right" Width="96%" ContentEditable="false"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTxtSBidBidValue" runat="server" TargetControlID="TxtSBidBidValue"
                                                FilterType="Numbers,Custom" Enabled="true" ValidChars=".">
                                            </cc1:FilteredTextBoxExtender>
                                        </FooterTemplate>
                                        <ItemStyle VerticalAlign="Top" Wrap="true" HorizontalAlign="Right" />
                                        <HeaderStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bid Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSBidBidAmount" runat="server" Text='<%# Bind("BID_AMOUNT") %>'
                                                Style="text-align: right"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="TxtSBidBidAmount" runat="server" Text='<%# Bind("BID_AMOUNT") %>'
                                                Style="text-align: right" Width="96%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTxtSBidBidAmount" runat="server" TargetControlID="TxtSBidBidAmount"
                                                FilterType="Numbers,Custom" Enabled="true" ValidChars=".">
                                            </cc1:FilteredTextBoxExtender>
                                        </FooterTemplate>
                                        <ItemStyle VerticalAlign="Top" Wrap="true" HorizontalAlign="Right" />
                                        <HeaderStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblSBidRemarks" runat="server" Text='<%# Bind("REMARKS") %>'></asp:Label>--%>
                                            <asp:TextBox ID="txtRemarks" Width="98%" runat="server" MaxLength="100" Text='<%# Bind("REMARKS")%>'
                                                TextMode="MultiLine" ReadOnly="true" Wrap="true"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="TxtSBidRemarks" runat="server" onkeyup="maxlengthfortxt(100)" onchange="maxlengthfortxt(100)"
                                                Columns="60" TextMode="MultiLine" Text='<%# Bind("REMARKS") %>' Width="98%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTxtSBidRemarks" runat="server" TargetControlID="TxtSBidRemarks"
                                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" Enabled="true"
                                                ValidChars="/,\,(,),. ">
                                            </cc1:FilteredTextBoxExtender>
                                        </FooterTemplate>
                                        <ItemStyle VerticalAlign="Top" Wrap="true" />
                                        <HeaderStyle Width="30%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnDelete" runat="server" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <asp:LinkButton ID="lnkbtnDelete" runat="server" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                        </AlternatingItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="btnAddrowvalue" runat="server" Text="Create" CommandName="AddNew"
                                                CausesValidation="false"></asp:LinkButton>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <%--  </td>
                                </tr>
                            </table>--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <br />
                <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                    OnClientClick="return fnCheckPageValidators('Submit');" Text="Save" ValidationGroup="Submit" />
                <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                    Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                <asp:Button ID="btnCancel" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                    Text="Cancel" OnClick="btnCancel_Click" />
                <br />
            </td>
        </tr>
        <tr>
            <td width="100%">
                <asp:ValidationSummary ID="vsSaleBidEntry" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Please correct the following validation(s):" ValidationGroup="Add2" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Please correct the following validation(s):" ValidationGroup="Submit" />
                <asp:ValidationSummary ID="vsSaleBidEntry_Bid" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Please correct the following validation(s):" ValidationGroup="Add" />
                <asp:CustomValidator ID="cvSaleBidentry" runat="server" CssClass="styleMandatoryLabel"
                    Display="None" HeaderText="Please correct the following validation(s):" />
                <asp:CustomValidator ID="cvBidDtls_Add" runat="server" CssClass="styleMandatoryLabel"
                    Display="None" HeaderText="Please correct the following validation(s):" />
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnID" runat="server" />

    <script language="javascript" src="../Scripts/jsBilling.js" type="text/javascript">
    </script>

    <script language="javascript" type="text/javascript">
    
       function funCheckFirstLetterisNumeric(textbox, msg)
        {
        
            var FieldValue=new Array();
             FieldValue = textbox.value.trim();  
             if(FieldValue.length>0 && FieldValue.value != '')
             {         
            if(isNaN(FieldValue[0]))
            {
                return true;
            }
            else
            {
                 alert(msg + ' name cannot begin with a number');
                textbox.focus();
                textbox.value='';
                event.returnValue = false;
                return false;
            } 
            }    
        }
    </script>

    <style type="text/css">
        .wraptext
        {
            word-wrap: break-word;
        }
    </style>
</asp:Content>
