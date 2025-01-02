<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeFile="S3GLoanAdManualJournal_Add.aspx.cs" Inherits="S3GLoanAdManualJournal_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc3" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<link href="../App_Themes/S3GTheme_Blue/AutoSuggestBox.css" rel="Stylesheet" type="text/css" />--%>

    <script type="text/javascript">
        function Common_ItemSelected(sender, e) {
            var hdnCommonID = $get('<%= hdnCommonID.ClientID %>');
            hdnCommonID.value = e.get_value();
        }
        function Common_ItemPopulated(sender, e) {
            var hdnCommonID = $get('<%= hdnCommonID.ClientID %>');
            hdnCommonID.value = '';

        }
    </script>

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
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td valign="top">
                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlLOB" ValidationGroup="Header" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                                runat="server" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="styleFieldLabel"></td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblMJVNO" runat="server" Text="MJV Number"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtMJVNo" Enabled="false" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblBranch" runat="server" CssClass="styleReqFieldLabel" Text="Location"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <%--<asp:DropDownList ID="ddlBranch" ValidationGroup="Header" runat="server">
                                                            </asp:DropDownList>--%>
                                                            <uc3:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" ErrorMessage="Select a Location"
                                                                ValidationGroup="Header" IsMandatory="true" />
                                                        </td>
                                                        <td class="styleFieldLabel"></td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblMJVDate" runat="server" CssClass="styleReqFieldLabel" Text="MJV Date"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtMJVDate" Width="100px" runat="server"></asp:TextBox>
                                                            <asp:Image ID="imgMJVDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                            <asp:RequiredFieldValidator ID="rfvLOB" SetFocusOnError="true" runat="server" ValidationGroup="Header"
                                                                ControlToValidate="ddlLOB" CssClass="styleMandatoryLabel" Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                                            <%--  <asp:RequiredFieldValidator ID="rfvBranch" SetFocusOnError="true" runat="server"
                                                                ValidationGroup="Header" ControlToValidate="ddlBranch" CssClass="styleMandatoryLabel"
                                                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                                Format="dd/MM/yyyy" PopupButtonID="imgMJVDate" TargetControlID="txtMJVDate">
                                                            </cc1:CalendarExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="Label3" runat="server" Text="MJV Status"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtMJVStatus" Enabled="false" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="styleFieldLabel"></td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="Label4" runat="server" CssClass="styleReqFieldLabel" Text="MJV Value Date"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtMJVValueDate" Width="100px" runat="server"></asp:TextBox>
                                                            <asp:Image ID="imgInvoiceDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                            <asp:RequiredFieldValidator ErrorMessage="Enter the MJV Value Date" ValidationGroup="Header"
                                                                ID="rfvMJVValueDate" runat="server" ControlToValidate="txtMJVValueDate" CssClass="styleMandatoryLabel"
                                                                Display="None"></asp:RequiredFieldValidator>
                                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                                PopupButtonID="imgInvoiceDate" TargetControlID="txtMJVValueDate">
                                                            </cc1:CalendarExtender>
                                                            <asp:CustomValidator ID="rfvCompareMJVDate" runat="server" Display="None" CssClass="styleMandatoryLabel"
                                                                ValidationGroup="Header" ErrorMessage="Difference between MJV Date and MJV Value Date must be 30 days"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblTranche" runat="server" Text="Tranche"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <uc3:Suggest ID="ddlTranche" runat="server" AutoPostBack="true" ServiceMethod="GetTrancheList" OnItem_Selected="ddlTranche_SelectedIndexChanged" />
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Button ID="btnViewRS" runat="server" OnClick="btnViewRS_Click" CssClass="styleSubmitShortButton" Text="View RS"></asp:Button>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblDrCr" runat="server" Text="Dr. / Cr."></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlDrCr" runat="server">
                                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Dr." Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Cr." Value="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top: 10px;">
                                                <table width="98%">
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:HiddenField ID="hdnCommonID" runat="server" />
                                                                    <asp:GridView runat="server" ShowFooter="true" DataKeyNames="MJVRow_ID" OnRowDataBound="grvManualJournal_RowDataBound"
                                                                        OnRowCommand="grvManualJournal_RowCommand" OnRowEditing="grvManualJournal_RowEditing"
                                                                        OnRowDeleting="grvManualJournal_RowDeleting" OnRowCancelingEdit="grvManualJournal_RowCancelingEdit"
                                                                        OnRowUpdating="grvManualJournal_RowUpdating" ID="grvManualJournal" Width="99%"
                                                                        AutoGenerateColumns="False">
                                                                        <Columns>
                                                                            <asp:TemplateField ItemStyle-Width="2%" HeaderText="Sl.No.">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" ID="lblSNO" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Rental Schedule No.">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" Text='<%#Eval("MLA_Desc")%>' ID="lblMLA"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>

                                                                                    <uc3:Suggest ID="txtMLASearch" runat="server" ServiceMethod="GetMLAList" AutoPostBack="true"
                                                                                        OnItem_Selected="txtMLASearch_OnTextChanged" />

                                                                                </FooterTemplate>
                                                                                <EditItemTemplate>

                                                                                    <uc3:Suggest ID="txtMLASearchhdr" runat="server" ServiceMethod="GetMLAList" AutoPostBack="true"
                                                                                        OnItem_Selected="txtMLASearchhdr_OnTextChanged" />

                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Sub A/c No." ItemStyle-Width="15%" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" Text='<%#Eval("SLA")%>' ID="lblSLA"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:DropDownList ID="ddlSLA" AutoPostBack="true" OnSelectedIndexChanged="ddlSLA_SelectedIndexChanged"
                                                                                        ValidationGroup="Footer" runat="server">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvSLA" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                                        runat="server" ControlToValidate="ddlSLA" InitialValue="0" ErrorMessage="Select the Sub A/c No."
                                                                                        ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                                                </FooterTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:DropDownList ID="ddlSLAhdr" AutoPostBack="true" OnSelectedIndexChanged="ddlSLAhdr_SelectedIndexChanged"
                                                                                        ValidationGroup="Footer" runat="server">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvSLAhdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                                        runat="server" ControlToValidate="ddlSLAhdr" InitialValue="0" ErrorMessage="Select the Sub A/c No."
                                                                                        ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Entity Type">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" Text='<%#Eval("EntityType")%>' ID="lblEntityType"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:DropDownList ID="ddlEntityType" AutoPostBack="true" Width="130px" ValidationGroup="Footer"
                                                                                        OnSelectedIndexChanged="ddlEntityType_SelectedIndexChanged" runat="server">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="rfvEntityType" CssClass="styleMandatoryLabel"
                                                                                        runat="server" ControlToValidate="ddlEntityType" InitialValue="0" ErrorMessage="Select the Entity Type"
                                                                                        ValidationGroup="Footer" Display="None">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </FooterTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:DropDownList ID="ddlEntityTypeHdr" Width="130px" AutoPostBack="true" ValidationGroup="Footer"
                                                                                        OnSelectedIndexChanged="ddlEntityTypeHdr_SelectedIndexChanged" runat="server">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="rfvEntityTypeHdr" CssClass="styleMandatoryLabel"
                                                                                        runat="server" ControlToValidate="ddlEntityTypeHdr" InitialValue="0" ErrorMessage="Select the Entity Type"
                                                                                        ValidationGroup="Footer" Display="None">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Transaction Flag">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" Text='<%#Eval("PostingFlagDesc")%>' ID="lblPostingFlag"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:DropDownList ID="ddlPostingFlag" ValidationGroup="Footer" AutoPostBack="true"
                                                                                        runat="server" OnSelectedIndexChanged="ddlPostingFlag_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="rfvPostingFlag" CssClass="styleMandatoryLabel"
                                                                                        runat="server" ControlToValidate="ddlPostingFlag" InitialValue="0" ErrorMessage="Select the Transaction Flag"
                                                                                        ValidationGroup="Footer" Display="None">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </FooterTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:DropDownList ID="ddlPostingFlagHdr" ValidationGroup="Footer" AutoPostBack="true"
                                                                                        runat="server" OnSelectedIndexChanged="ddlPostingFlagHdr_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="rfvPostingFlagHdr" CssClass="styleMandatoryLabel"
                                                                                        runat="server" ControlToValidate="ddlPostingFlagHdr" InitialValue="0" ErrorMessage="Select the Transaction Flag"
                                                                                        ValidationGroup="Footer" Display="None">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Dim 2" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" Text='<%#Eval("Dim2")%>' ID="lblDim"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:DropDownList ID="ddlDim2" ValidationGroup="Footer" runat="server">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="rfvDim2" CssClass="styleMandatoryLabel"
                                                                                        runat="server" ControlToValidate="ddlDim2" InitialValue="0" ErrorMessage="Select Dim 2"
                                                                                        ValidationGroup="Footer" Display="None">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </FooterTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:DropDownList ID="ddlDim2Hdr" ValidationGroup="Footer" runat="server">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="rfvDim2Hdr" CssClass="styleMandatoryLabel"
                                                                                        runat="server" ControlToValidate="ddlDim2Hdr" InitialValue="0" ErrorMessage="Select Dim 2"
                                                                                        ValidationGroup="Footer" Display="None">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Wrap="false" HeaderText="GL A/c">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" Text='<%#Eval("GL_Description")%>' ID="lblGLAcc"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <%--  <asp:DropDownList ID="ddlGLAcc" AutoPostBack="true" ValidationGroup="Footer" OnSelectedIndexChanged="ddlGLAcc_SelectedIndexChanged"
                                                                                runat="server">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="rfvGLAcc" CssClass="styleMandatoryLabel"
                                                                                runat="server" ControlToValidate="ddlGLAcc" InitialValue="0" ErrorMessage="Select the GL A/c"
                                                                                ValidationGroup="Footer" Display="None">
                                                                            </asp:RequiredFieldValidator>--%>

                                                                                    <uc3:Suggest ID="ddlGLAcc" runat="server" ServiceMethod="GetGLAccF" ErrorMessage="Select a GL A/c" AutoPostBack="true"
                                                                                        OnItem_Selected="ddlGLAcc_SelectedIndexChanged" ValidationGroup="Footer" IsMandatory="true" />

                                                                                </FooterTemplate>
                                                                                <EditItemTemplate>
                                                                                    <%--       <asp:DropDownList ID="ddlGLAccHdr" AutoPostBack="true" ValidationGroup="Footer" OnSelectedIndexChanged="ddlGLAccHdr_SelectedIndexChanged"
                                                                                        runat="server">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="rfvGLAccHdr" CssClass="styleMandatoryLabel"
                                                                                        runat="server" ControlToValidate="ddlGLAccHdr" InitialValue="0" ErrorMessage="Select the GL A/c"
                                                                                        ValidationGroup="Footer" Display="None">
                                                                                    </asp:RequiredFieldValidator>--%>
                                                                                    <uc3:Suggest ID="ddlGLAccHdr" runat="server" ServiceMethod="GetGLAccE" ErrorMessage="Select a GL A/c" AutoPostBack="true"
                                                                                        OnItem_Selected="ddlGLAccHdr_SelectedIndexChanged" ValidationGroup="Footer" IsMandatory="true" />
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="SL A/c" ItemStyle-Width="10%">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" Text='<%#Eval("SLAcc_Desc")%>' ID="lblSLAcc"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <uc3:Suggest ID="ddlSLAcc" runat="server" ServiceMethod="GetSLAccF" ErrorMessage="Select a SL A/c"
                                                                                        ValidationGroup="Footer" IsMandatory="false" />
                                                                                    <%-- <asp:DropDownList ID="ddlSLAcc" AutoPostBack="true" OnSelectedIndexChanged="ddlSLAcc_SelectedIndexChanged"
                                                                                runat="server" ValidationGroup="Footer">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="rfvSLAcc" CssClass="styleMandatoryLabel"
                                                                                runat="server" ControlToValidate="ddlSLAcc" InitialValue="0" ErrorMessage="Select the SL A/c"
                                                                                ValidationGroup="Footer" Display="None">
                                                                            </asp:RequiredFieldValidator>--%>
                                                                                </FooterTemplate>
                                                                                <EditItemTemplate>
                                                                                    <%-- <asp:DropDownList ID="ddlSLAccHdr" AutoPostBack="true" OnSelectedIndexChanged="ddlSLAccHdr_SelectedIndexChanged"
                                                                                        runat="server" ValidationGroup="Footer">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="rfvSLAccHdr" CssClass="styleMandatoryLabel"
                                                                                        runat="server" ControlToValidate="ddlSLAccHdr" InitialValue="0" ErrorMessage="Select the SL A/c"
                                                                                        ValidationGroup="Footer" Display="None">
                                                                                    </asp:RequiredFieldValidator>--%>
                                                                                    <uc3:Suggest ID="ddlSLAccHdr" runat="server" ServiceMethod="GetSLAccE" ErrorMessage="Select a SL A/c"
                                                                                        ValidationGroup="Footer" IsMandatory="false" />
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Wrap="false" Visible="false" FooterStyle-Font-Bold="false"
                                                                                ItemStyle-Font-Size="Small" FooterStyle-Font-Size="Small" HeaderText="Description">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" Text='<%#Eval("Description")%>' ID="lblDesc"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label runat="server" Text='<%#Eval("Description")%>' ID="lblDescF"></asp:Label>
                                                                                </FooterTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:Label runat="server" Text='<%#Eval("Description")%>' ID="lblDescHdr"></asp:Label>
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Debit">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" Text='<%#Eval("Debit")%>' ID="lblDebit"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox MaxLength="11" Width="60px" runat="server" ID="txtDebit"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtDebit"
                                                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <asp:RequiredFieldValidator SetFocusOnError="true" Display="None" Enabled="false"
                                                                                        CssClass="styleMandatoryLabel" ControlToValidate="txtDebit" ValidationGroup="Footer"
                                                                                        runat="server" ID="rfvDebit">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </FooterTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox MaxLength="11" Width="60px" runat="server" ID="txtDebitHdr"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1Hdr" runat="server" TargetControlID="txtDebitHdr"
                                                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <asp:RequiredFieldValidator SetFocusOnError="true" Display="None" Enabled="false"
                                                                                        CssClass="styleMandatoryLabel" ControlToValidate="txtDebitHdr" ValidationGroup="Footer"
                                                                                        runat="server" ID="rfvDebitHdr">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Credit">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" Text='<%#Eval("Credit")%>' ID="lblCredit"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox MaxLength="11" Width="60px" runat="server" ID="txtCredit"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator SetFocusOnError="true" Display="None" Enabled="false"
                                                                                        CssClass="styleMandatoryLabel" ControlToValidate="txtCredit" ValidationGroup="Footer"
                                                                                        runat="server" ID="rfvCredit" ErrorMessage="Enter Debit or Credit Value">
                                                                                    </asp:RequiredFieldValidator>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtCredit"
                                                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </FooterTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox MaxLength="11" Width="60px" runat="server" ID="txtCreditHdr"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator SetFocusOnError="true" Display="None" Enabled="false"
                                                                                        CssClass="styleMandatoryLabel" ControlToValidate="txtCreditHdr" ValidationGroup="Footer"
                                                                                        runat="server" ID="rfvCreditHdr" ErrorMessage="Enter Debit or Credit Value">
                                                                                    </asp:RequiredFieldValidator>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2Hdr" runat="server" TargetControlID="txtCreditHdr"
                                                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="15%">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" Text='<%#Eval("Remarks")%>' ID="lblRemarks"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox runat="server" Width="90px" TextMode="MultiLine" ValidationGroup="Footer"
                                                                                        MaxLength="100" onkeyup="maxlengthfortxt(100);" ID="txtRemarks"></asp:TextBox>
                                                                                </FooterTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox runat="server" Width="90px" TextMode="MultiLine" ValidationGroup="Footer"
                                                                                        MaxLength="100" onkeyup="maxlengthfortxt(100);" ID="txtRemarksHdr"></asp:TextBox>
                                                                                </EditItemTemplate>

                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Width="2px" Visible="false" HeaderText="EntityTypeValue">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" Text='<%#Eval("EntityTypeValue")%>' ID="lblEntityTypeValue"></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"
                                                                                        ToolTip="Edit">
                                                                                    </asp:LinkButton>&nbsp;|
                                                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="Delete"
                                                                                OnClientClick="return confirm('Do you want to Delete this record?');"
                                                                                ToolTip="Delete">
                                                                            </asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Button ID="btnAdd" Width="50px" runat="server" CommandName="AddNew" ValidationGroup="Footer"
                                                                                        CssClass="styleSubmitShortButton" Text="Add"></asp:Button>
                                                                                </FooterTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                                                        ValidationGroup="Footer" ToolTip="Update"></asp:LinkButton>&nbsp;|
                                                                            <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                                                                CausesValidation="false" ToolTip="Cancel"></asp:LinkButton>
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Width="2%" HeaderText="Occurance_ID" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" ID="lblOccurance_ID" Text='<%#Eval("Occurance_ID")%>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                                        <RowStyle HorizontalAlign="left" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>

                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView runat="server" CellPadding="4" ShowFooter="false" AllowPaging="false"
                                                                ID="grvPrintManualJournal" Width="730px" Height="300px" AutoGenerateColumns="true">
                                                                <Columns>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <table>
                                                                <tr>
                                                                    <td class="styleFieldLabel" style="padding-right: 2px;">
                                                                        <%--Total Debit/Credit:&nbsp;--%>
                                                                        <asp:TextBox ID="txtTotDebit" Style="border: none; font-family: Calibri; font-size: 13px"
                                                                            CssClass="styleTxtRightAlign" ReadOnly="true" Width="80px" runat="server" Visible="false">
                                                                        </asp:TextBox>
                                                                        <asp:TextBox ID="txtTotCredit" Style="border: none; font-family: Calibri; font-size: 13px"
                                                                            CssClass="styleTxtRightAlign" ReadOnly="true" Width="80px" runat="server" Visible="false">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                    <td runat="server" id="tdAlign" style="text-align: left; width: 140px;">
                                                                        <asp:TextBox ID="txtTally" Width="60px" Visible="false" ReadOnly="true" runat="server">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
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
                                    <asp:Button runat="server" ID="btnMapInvoice" Enabled="true" CssClass="styleSubmitButton" CausesValidation="false"
                                        ToolTip="Map RS Invoice details" Text="Map Vendor Invoice" OnClick="btnMapInvoice_Click" />
                                    <asp:Button runat="server" ID="btnMapSalesInvoice" Enabled="true" CssClass="styleSubmitButton" CausesValidation="false"
                                        ToolTip="Map RS Invoice details" Text="Map Sales Invoice" OnClick="btnMapSalesInvoice_Click" />
                                    <asp:Button runat="server" ID="btnSave" Enabled="false" OnClientClick="return fnCheckPageValidators('Header');"
                                        CssClass="styleSubmitButton" ValidationGroup="Header" Text="Save" OnClick="btnSave_Click" />
                                    &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                                        Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                                    &nbsp;<asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                                        CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                                    <asp:Button runat="server" ID="btnPrint" Text="Print" Enabled="false" CausesValidation="false"
                                        CssClass="styleSubmitButton" OnClientClick="return fnClick();" />
                                    <asp:Button runat="server" ID="btnCancelMJV" OnClick="btnCancelMJV_Click" Text="Cancel MJV"
                                        Enabled="false" CausesValidation="false" CssClass="styleSubmitButton" OnClientClick="return fnCancelClick();" />
                                </td>
                            </tr>
                            <tr class="styleButtonArea">
                                <td>
                                    <asp:ValidationSummary runat="server" ID="vsUserMgmt" HeaderText="Correct the following validation(s):"
                                        Height="250px" CssClass="styleMandatoryLabel" ValidationGroup="Header" Width="500px"
                                        ShowMessageBox="false" ShowSummary="true" />
                                    <asp:ValidationSummary runat="server" ID="ValidationSummary1" HeaderText="Correct the following validation(s):"
                                        Height="250px" CssClass="styleMandatoryLabel" ValidationGroup="Footer" Width="500px"
                                        ShowMessageBox="false" ShowSummary="true" />
                                    <input type="hidden" runat="server" value="0" id="hdnRowID" />
                                    <input type="hidden" runat="server" value="1" id="hdnAccValid" />
                                    <input type="hidden" runat="server" value="0" id="hdnStatus" />
                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlInvoiceDetails" runat="server" GroupingText="Vendor Invoice Details" CssClass="stylePanel" Width="90%" Visible="false">
                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td width="100%">
                                        <asp:GridView ID="grvInvoiceDetail" runat="server" AutoGenerateColumns="false" Width="98%" HorizontalAlign="Center">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="5%" HeaderText="Sl.No.">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblInvocieSNO" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Account Invoice ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAccountInvoiceID" runat="server" Text='<%#Eval("Invoice_ID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rental Schedule ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRSID" runat="server" Text='<%#Eval("Pa_Sa_Ref_ID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rental Schedule No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRSNumber" runat="server" Text='<%#Eval("RS_Number")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PO Number" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblP0Number" runat="server" Text='<%#Eval("PO_Number")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Vendor Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendor_Name" Text='<%# Bind("Entity_Name")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="15%" HeaderText="PI Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPINumber" runat="server" Text='<%#Eval("PI_Number")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PI Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPI_Date" Text='<%# Bind("PI_Date")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="VI Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVINumber" runat="server" Text='<%#Eval("VI_Number")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="VI Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVI_Date" Text='<%# Bind("VI_Date")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Invoice Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotal_Bill_Amount" Text='<%# Bind("Total_Bill_Amount")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Payable Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNet_Payable" Text='<%# Bind("Net_Payable_Amount")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="10%" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkRemoveInvoice" runat="server" Text="Remove" ToolTip="Remove Invoice Detail"
                                                            OnClick="lnkRemoveInvoice_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                        <asp:Panel ID="pnlSalesInvoiceDetails" runat="server" GroupingText="Sales Invoice Details" CssClass="stylePanel" Width="90%" Visible="false">
                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td width="100%">
                                        <asp:GridView ID="grvSalesInvoiceDetail" runat="server" AutoGenerateColumns="false" Width="98%" HorizontalAlign="Center">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl.No.">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSlNo" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sales Invoice ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSalesInvoiceID" Text='<%# Bind("Sales_Invoice_ID")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rental Schedule ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblmpRSID" Text='<%# Bind("Pa_Sa_Ref_ID")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rental Schedule No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblmpRSNo" Text='<%# Bind("RS_Number")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rental Invoice No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRental_Invoice_No" Text='<%# Bind("Rental_Invoice_No")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="AMF Invoice No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAMF_Invoice_No" Text='<%# Bind("AMF_Invoice_No")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Invoice Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoice_Date" Text='<%# Bind("Invoice_Date")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rental">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRental" Text='<%# Bind("Rental")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="VAT">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVAT" Text='<%# Bind("VAT")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="AMF">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAMF" Text='<%# Bind("AMF")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Service Tax">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblService_Tax" Text='<%# Bind("Service_Tax")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="10%" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkRemoveSalesInvoice" runat="server" Text="Remove" ToolTip="Remove Invoice Detail"
                                                            OnClick="lnkRemoveSalesInvoice_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="display: none">
                <asp:Button runat="server" ID="btnPrintNew" BackColor="White" Height="0px" CausesValidation="false"
                    OnClick="btnPrint_Click" />
            </td>
        </tr>
    </table>

    <cc1:ModalPopupExtender ID="moePoInvoiceDtls" runat="server" TargetControlID="btnModal" PopupControlID="pnlPoInvoiceDtls"
        BackgroundCssClass="styleModalBackground" Enabled="true" />

    <asp:Panel ID="pnlPoInvoiceDtls" Style="display: none; vertical-align: middle" runat="server"
        BorderStyle="Solid" BackColor="White" Width="90%" ScrollBars="Auto">
        <div id="divPoInvoiceDtls" runat="server" style="max-height: 400px; overflow: auto">
            <asp:UpdatePanel ID="updtPnlPoInvoiceDtls" runat="server">
                <ContentTemplate>
                    <table width="99%" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="right">
                                <asp:ImageButton ID="imgPopupClose" runat="server" ImageUrl="~/Images/delete1.png" Height="20px" ToolTip="Close"
                                    OnClick="imgPopupClose_Click" />
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="grvPoInvoiceDetails" runat="server" AutoGenerateColumns="false" Width="99%" OnRowDataBound="grvPoInvoiceDetails_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="5%" HeaderText="Sl.No.">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblmpSNO" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Account Invoice ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmpInvoiceID" Text='<%# Bind("Account_Invoice_ID")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rental Schedule ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmpRSID" Text='<%# Bind("Pa_Sa_Ref_ID")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rental Schedule No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmpRSNo" Text='<%# Bind("RS_Number")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendor_Name" Text='<%# Bind("Entity_Name")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PI Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmpPINumber" Text='<%# Bind("PI_Number")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PI Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPI_Date" Text='<%# Bind("PI_Date")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VI Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmpVINumber" Text='<%# Bind("VI_Number")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VI Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVI_Date" Text='<%# Bind("VI_Date")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotal_Bill_Amount" Text='<%# Bind("Total_Bill_Amount")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payable Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNet_Payable" Text='<%# Bind("Net_Payable_Amount")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select All">
                                            <HeaderTemplate>
                                                <span id="spnAll" style="text-align: center">Select All</span>
                                                <asp:CheckBox ID="chkSelectAll" runat="server"></asp:CheckBox>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" onclick="javascript:fnChkIsSelect(this);"></asp:CheckBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <asp:Button runat="server" ID="btnAddPOInvoice" CssClass="styleSubmitButton" Text="ADD" ToolTip="Add Invoice Details"
                                    OnClick="btnAddPOInvoice_Click" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary runat="server" ID="vsPOSearch" ValidationGroup="vsPOSearch"
                        HeaderText="Please correct the following validation(s):" Height="10px" CssClass="styleMandatoryLabel"
                        Width="500px" ShowMessageBox="false" ShowSummary="true" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>

    <cc1:ModalPopupExtender ID="moeSalesInvoiceDtls" runat="server" TargetControlID="btnSalesModal" PopupControlID="pnlSalesInvoice"
        BackgroundCssClass="styleModalBackground" Enabled="true" />

    <asp:Panel ID="pnlSalesInvoice" Style="display: none; vertical-align: middle" runat="server"
        BorderStyle="Solid" BackColor="White" Width="90%" ScrollBars="Auto">
        <div id="divSalesInvoice" runat="server" style="max-height: 400px; overflow: auto">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <table width="99%" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="right">
                                <asp:ImageButton ID="imgSalesPopupClose" runat="server" ImageUrl="~/Images/delete1.png" Height="20px" ToolTip="Close"
                                    OnClick="imgSalesPopupClose_Click" />
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="grvSalesInvoice" runat="server" AutoGenerateColumns="false" Width="99%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl.No.">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblSlNo" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sales Invoice ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmpInvoiceID" Text='<%# Bind("Sales_Invoice_ID")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rental Schedule ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmpRSID" Text='<%# Bind("Pa_Sa_Ref_ID")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rental Schedule No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmpRSNo" Text='<%# Bind("RS_Number")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rental Invoice No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRental_Invoice_No" Text='<%# Bind("Rental_Invoice_No")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="AMF Invoice No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAMF_Invoice_No" Text='<%# Bind("AMF_Invoice_No")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoice_Date" Text='<%# Bind("Invoice_Date")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rental">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRental" Text='<%# Bind("Rental")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VAT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVAT" Text='<%# Bind("VAT")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="AMF">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAMF" Text='<%# Bind("AMF")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Tax">
                                            <ItemTemplate>
                                                <asp:Label ID="lblService_Tax" Text='<%# Bind("Service_Tax")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select Indicator">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="7%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <asp:Button runat="server" ID="btnAddSalesInvoice" CssClass="styleSubmitButton" Text="ADD" ToolTip="Add Sales Invoice Details"
                                    OnClick="btnAddSalesInvoice_Click" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary runat="server" ID="ValidationSummary2" ValidationGroup="vsPOSearch"
                        HeaderText="Please correct the following validation(s):" Height="10px" CssClass="styleMandatoryLabel"
                        Width="500px" ShowMessageBox="false" ShowSummary="true" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>

    <cc1:ModalPopupExtender ID="moeRS" runat="server" TargetControlID="btnRS" PopupControlID="plnRS"
        BackgroundCssClass="styleModalBackground" Enabled="true" />

    <asp:Panel ID="plnRS" Style="display: none; vertical-align: middle" runat="server"
        BorderStyle="Solid" BackColor="White" Width="30%" ScrollBars="Auto">
        <div id="div1" runat="server" style="max-height: 500px; overflow: auto">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <table width="100%" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="right">
                                <asp:ImageButton ID="imgRS" runat="server" ImageUrl="~/Images/delete1.png" Height="20px" ToolTip="Close"
                                    OnClick="imgRS_Click" />
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:GridView ID="grvRS" runat="server" AutoGenerateColumns="false" Width="99%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl.No.">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblSlNo" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rental Schedule ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmpRSID" Text='<%# Bind("PA_SA_REF_ID")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rental Schedule No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmpRSNo" Text='<%# Bind("PANum")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select Indicator">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# (Eval("IsSelect").ToString() == "1" ? true : false) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="7%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <asp:Button runat="server" ID="btnAddRS" CssClass="styleSubmitShortButton" Text="Ok" ToolTip="Select RS"
                                    OnClick="btnAddRS_Click" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary runat="server" ID="ValidationSummary3" ValidationGroup="vsPOSearch"
                        HeaderText="Please correct the following validation(s):" Height="10px" CssClass="styleMandatoryLabel"
                        Width="500px" ShowMessageBox="false" ShowSummary="true" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>

    <asp:Button ID="btnModal" Style="display: none" runat="server" />
    <asp:Button ID="btnSalesModal" Style="display: none" runat="server" />
    <asp:Button ID="btnRS" Style="display: none" runat="server" />

    <script language="javascript" type="text/javascript">

        function fnChkIsSelect(chkSelect) {
            var gv = document.getElementById('<%= grvPoInvoiceDetails.ClientID%>');
            var chk = document.getElementById('ctl00_ContentPlaceHolder1_grvPoInvoiceDetails_ctl01_chkSelectAll');

            if (chkSelect.checked == false) {
                chk.checked = false;
            }
            else {
                var gvRwCnt = gv.rows.length - 1;
                var ChcCnt = 0;
                for (var i = 0; i < gv.rows.length; ++i) {
                    var Inputs = gv.rows[i].getElementsByTagName("input");
                    for (var n = 0; n < Inputs.length; ++n) {
                        if (Inputs[n].type == 'checkbox') {
                            if (Inputs[n].checked == true)
                                ++ChcCnt;
                        }
                    }
                }
                if (ChcCnt == gvRwCnt)
                    chk.checked = true;
                else
                    chk.checked = false;
            }
        }

        function FnValidate(txtDebit, txtCredit, idReqCredit, idReqDebit) {
            //alert(idReqDebit);
            //document.getElementById(idReqDebit).enabled=false;
            if ((document.getElementById(txtDebit).value == '') && (document.getElementById(txtCredit).value == '')) {

                document.getElementById(idReqCredit).enabled = true;
                document.getElementById(idReqDebit).enabled = true;
                //document.getElementById(idReqCredit).className = 'styleReqFieldFocus';
                //document.getElementById(idReqDebit).className = 'styleReqFieldFocus';
            }
            else {
                document.getElementById(idReqCredit).enabled = false;
                document.getElementById(idReqDebit).enabled = false;

            }
            if (!fnCheckPageValidators('Footer', false))
                return false;

            return true;
        }

        function fnDiableCredit(idDebit, idCredit, ctrlId) {

            var txtDebit = document.getElementById(idDebit);
            var txtCredit = document.getElementById(idCredit);

            //var txtDebit=document.getElementById('ctl00_ContentPlaceHolder1_grvManualJournal_ctl03_txtDebit');
            //var txtCredit=document.getElementById('ctl00_ContentPlaceHolder1_grvManualJournal_ctl03_txtCredit');
            txtCredit.disabled = false;
            txtDebit.disabled = false;
            //alert(txtDebit.value);
            if ((txtDebit.value == "") && (txtCredit.value == "")) {
                txtDebit.value = "";
                txtCredit.value = "";
                return;
            }

            if ((txtDebit.value != "") && (ctrlId == 'C')) {
                txtCredit.value = "";
                return;
            }
            if ((txtCredit.value != "") && (ctrlId == 'D')) {
                txtDebit.value = "";
                return;
            }

        }

        function fnClick() {

            document.getElementById('<%=btnPrintNew.ClientID%>').click();
            return true;
        }

        function fnCancelClick() {
            if (confirm('Do you want to cancel?')) {
                return true;
            }
            else {
                return false;
            }
        }

    </script>

</asp:Content>
