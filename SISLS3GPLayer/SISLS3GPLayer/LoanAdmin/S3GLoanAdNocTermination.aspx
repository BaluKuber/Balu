<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLoanAdNocTermination.aspx.cs" Inherits="LoanAdmin_S3GLoanAdNocTermination"
    Title="NOC" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
    <%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
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
                        <asp:Panel ID="pnlNocTerminationDetails" runat="server" CssClass="stylePanel" GroupingText="NOC Termination Details">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <%-- LOB --%>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                            ToolTip="Line of Business" TabIndex="1">
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
                                      <%--  <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                            ToolTip="Location" TabIndex="2">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVNocBranch" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlBranch" InitialValue="0" ValidationGroup="VGNoc" ErrorMessage="Select a Location"
                                            Display="None"></asp:RequiredFieldValidator>--%>
                                              <uc2:Suggest ID="ddlBranch" ValidationGroup="VGNoc" runat="server" ToolTip="Location"
                                                        ServiceMethod="GetBranchList" AutoPostBack="true" OnItem_Selected="ddlBranch_SelectedIndexChanged"
                                                        ErrorMessage="Select a Location" IsMandatory="true" />
                                    </td>
                                </tr>
                                <%-- Second Row --%>
                                <tr>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="NOC Number" ID="lblNocNo" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtNocNo" ReadOnly="True" ToolTip="NOC Number"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="NOC Date" ID="lblNocDate" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtNocDate" ReadOnly="True" ToolTip="NOC Date" Width="50%"></asp:TextBox>
                                    </td>
                                </tr>
                                <%-- Third Row--%>
                                <tr>
                                    <%--MLA --%>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Prime Account Number" ID="lblprimeaccno" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                    <uc2:Suggest ID="ddlPAN" runat="server" ServiceMethod="GetPANUM" AutoPostBack="true"
                            OnItem_Selected="ddlPAN_SelectedIndexChanged" IsMandatory="true" ValidationGroup="VGNoc" ErrorMessage="Enter a Prime A/C Number" />
                                      <%--  <asp:DropDownList ID="ddlPAN" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPAN_SelectedIndexChanged"
                                            ToolTip="Prime A/C Number " TabIndex="3">
                                        </asp:DropDownList>--%>
                                       <%-- <asp:RequiredFieldValidator ID="RFVMLA" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlPAN" InitialValue="0" ValidationGroup="VGNoc" ErrorMessage="Select a Prime A/C Number"
                                            Display="None"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <%--SLA--%>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Sub Account Number" ID="lblsubAccno" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:DropDownList ID="ddlSAN" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSAN_SelectedIndexChanged"
                                            ToolTip="Sub A/C Number" TabIndex="4">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVSLA" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlSAN" InitialValue="0" ValidationGroup="VGNoc" ErrorMessage="Select a Sub Account Number"
                                            Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <%--   Fourth Row--%>
                                <tr>
                                    <%--Account Date--%>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Account Date" ID="lblAccDate" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtAccDate" ReadOnly="True" ToolTip="Account Date"
                                            Width="50%"></asp:TextBox>
                                    </td>
                                    <%--Maturity Date --%>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Maturity Date" ID="lblMaturityDate" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtMaturityDate" ReadOnly="True" ToolTip="Maturity Date"
                                            Width="50%"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--   Fifth Row--%>
                                <tr>
                                    <%--Closure Date --%>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Closure Date" ID="lblClosureDate" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtClosureDate" ReadOnly="True" ToolTip="Closure Date"
                                            Width="50%"></asp:TextBox>
                                    </td>
                                    <%-- A/c Status --%>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Account Status" ID="lblAccountStatus" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtAccountStatuse" ReadOnly="True" ToolTip="Account Status"
                                            Width="70%"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--   Sixth Row--%>
                                <tr>
                                    <%--Noc Print Status --%>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="NOC Print Status" ID="lblNocPrintstatus" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtNocPrintstatus" ReadOnly="True" ToolTip="NOC Print Status"
                                            Width="50%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <%--   9`th  Row--%>
                <tr>
                    <td>
                        <asp:Panel ID="pnlCommAddress" runat="server" CssClass="stylePanel" GroupingText="Customer Information"
                            Width="99%">
                            <%--  <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="styleFieldLabel" width="145px" >
                                        <asp:Label ID="lblcusname" runat="server" Text="Customer Name" width="125px"  ></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" >
                                        <asp:TextBox ID="txtCustName" runat="server" Width="200px" ReadOnly="True" 
                                            ToolTip="Customer Name"></asp:TextBox>
                                       
                                    </td>
                                    <td class="styleFieldAlign" width="125px">
                                    <asp:HiddenField ID="hidcuscode" runat="server"  /> 
                                    </td>
                                    <td></td>
                                   
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="145px">
                                        <asp:Label ID="lblAddress1" runat="server" Text="Address 1"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtAddress1" runat="server" Width="200px" ReadOnly="True" 
                                            MaxLength="60" ToolTip="Address 1" 
                                            ></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" width="125px">
                                        <asp:Label ID="lbladdress2" runat="server" Text="Address 2"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtAddress2" runat="server" Width="200px" ReadOnly="True" 
                                            MaxLength="60" ToolTip="Address 2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="145px">
                                        <asp:Label ID="lblCity" runat="server" Text="City"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtCity" runat="server" Width="200px" ReadOnly="True" 
                                            MaxLength="30" ToolTip="City"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" width="125px">
                                        <asp:Label ID="lblState" runat="server" Text="State"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtState" runat="server" Width="200px" ReadOnly="True" 
                                            Rows="60" ToolTip="State"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="145px">
                                        <asp:Label ID="lblCountry" runat="server" Text="Country"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtCountry" runat="server" Width="200px" ReadOnly="True" 
                                            MaxLength="60" ToolTip="Country"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" width="125px">
                                        <asp:Label ID="lblPincode" runat="server" Text="Pincode/Zipcode"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtPincode" runat="server" Width="200px" ReadOnly="True" 
                                            MaxLength="10" ToolTip="PinCode/ZipCode"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="145px">
                                        <asp:Label ID="lblMobile" runat="server" Text="Mobile"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtMobile" runat="server" Width="200px" ReadOnly="True" 
                                            MaxLength="12" ToolTip="Mobile"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" width="125px">
                                        <asp:Label ID="lblTelephone" runat="server" Text="Telephone"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtTelephone" runat="server" Width="200px" ReadOnly="True" 
                                            MaxLength="12" ToolTip="TelePhone"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="145px">
                                        <asp:Label ID="lblEmail" runat="server" Text="EMail Id"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" colspan="3">
                                        <asp:TextBox ID="txtEmailid" runat="server" Width="200px" ReadOnly="True" 
                                            MaxLength="60" ToolTip="Email ID"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>--%>
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
                <%--Second Row For Gridview Heading--%>
                <tr>
                    <td class="styleFieldLabel" style="padding-top: 10px">
                        &nbsp;
                        <%--<asp:Label runat="server" Font-Bold="True" Font-Underline="True" Text="Asset Details"
                            ID="lblAssetDetails" Visible="False"></asp:Label>--%>
                    </td>
                </tr>
                <%--Gridview --%>
                <tr>
                    <%--<td style="padding-top: 10px">--%>
                    <td>
                        <asp:Panel ID="PNLAssetDetails" runat="server" CssClass="stylePanel" GroupingText="Asset Details"
                            Width="98%">
                            <table cellpadding="0" cellspacing="0" width="98%">
                                <tr>
                                    <td colspan="4 " width="80%">
                                        <%--<div style="overflow-x: hidden; overflow-y: auto; height: 225px;">--%>
                                        <asp:GridView runat="server" ID="GRVNOC" AutoGenerateColumns="False" Width="100%"
                                            ToolTip="Asset Details">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtsno" runat="server" Text='<%# Bind("SNo")%>' Width="100%"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Asset">
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
                                                <asp:TemplateField HeaderText="Asset Registration/Serial Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtAssetRegNo" runat="server" Text='<%# Bind("Regno")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
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
                <%--Total--%>
                <tr class="styleButtonArea" align="center" style="padding-left: 4px">
                    <td>
                        <asp:Button ID="btnNOCcancel" runat="server" Text="Cancel NOC" CssClass="styleSubmitButton"
                            ValidationGroup="VGNOC" CausesValidation="false" Width="80px" Enabled="False"
                            ToolTip="Cancel NOC" OnClick="btnNOCcancel_Click" TabIndex="6" />
                        <asp:Button ID="btnNocLetter" runat="server" Text="Generate NOC Letter" CssClass="styleSubmitButton"
                            ValidationGroup="VGNOC" CausesValidation="false" OnClick="btnNocLetter_Click"
                            Width="180px" Enabled="False" ToolTip="Generate NOC Letter" TabIndex="7" />
                        <asp:Button ID="btnEmail" runat="server" Text="Email" CssClass="styleSubmitButton"
                            ValidationGroup="VGNOC" CausesValidation="false" OnClick="btnEmail_Click" ToolTip="Email"
                            TabIndex="8" />
                        <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators();"
                            CssClass="styleSubmitButton" Text="Save" ValidationGroup="VGNOC" OnClick="btnSave_Click"
                            ToolTip="Save" TabIndex="9" />
                        &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click"
                            ToolTip="Clear" TabIndex="10" />
                        &nbsp;<asp:Button runat="server" ID="btnCancel" Text="Cancel" ValidationGroup="VGNOC"
                            CausesValidation="false" CssClass="styleSubmitButton" OnClick="btnCancel_Click"
                            ToolTip="Cancel" TabIndex="11" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:ValidationSummary runat="server" ID="vsFactoringInvoiceLoadinf" HeaderText="Correct the following validation(s):"
                            Height="177px" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" ValidationGroup="VGFIL" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
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
