<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GINSInsuranceRemainder.aspx.cs" Inherits="Insurance_S3GInsInsurancRemainder"
    Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="udpInsDetails" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
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
                    <td style="padding-top: auto">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlGeneral" runat="server" CssClass="stylePanel" GroupingText="General"
                                        Width="100%" Height="50%">
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldAlign" style="width: 17%">
                                                    <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 20%">
                                                    <asp:DropDownList ID="ddlLOB" runat="server" Width="145px" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                        TabIndex="1" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                        Display="None" InitialValue="0" ErrorMessage="Select a Line of Business" ValidationGroup="Govalidation"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 17%">
                                                    <asp:Label ID="lblBranch" runat="server" CssClass="styleDisplayLabel" Text="Location"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 20%">
                                                
                                                <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                                        OnItem_Selected="ddlBranch_SelectedIndexChanged" IsMandatory="true" ValidationGroup="Submit" ErrorMessage="Enter Location" />
                                                    <%--<asp:DropDownList ID="ddlBranch" runat="server" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                                        Width="145px" TabIndex="2" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="ddlBranch"
                                                        Display="None" InitialValue="0" ErrorMessage="Select a Location" ValidationGroup="Govalidation"></asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldAlign" style="width: 17%">
                                                    <asp:Label ID="lblRemainder" runat="server" CssClass="styleReqFieldLabel" Text="Reminder Type"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 20%">
                                                    <asp:DropDownList ID="ddlRemainder" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRemainder_SelectedIndexChanged"
                                                        Width="145px" TabIndex="3">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="1">ALL</asp:ListItem>
                                                        <asp:ListItem Value="2">Specific</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvRemainder" runat="server" ControlToValidate="ddlRemainder"
                                                        Display="None" InitialValue="0" ErrorMessage="Select a Reminder Type" ValidationGroup="Govalidation"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 17%">
                                                    <asp:Label ID="lblInsDoneBy" runat="server" CssClass="styleReqFieldLabel" Text="Insurance Done By"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 20%">
                                                    <asp:DropDownList ID="ddlInsDoneBy" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlInsDoneBy_SelectedIndexChanged"
                                                        Width="145px" TabIndex="4">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvInsDoneBy" runat="server" ControlToValidate="ddlInsDoneBy"
                                                        Display="None" InitialValue="0" ErrorMessage="Select Insurance Done By" ValidationGroup="Govalidation"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldAlign" style="width: 17%">
                                                    <asp:Label ID="lblPath" runat="server" CssClass="styleReqFieldLabel" Text="Path"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 20%">
                                                    <asp:TextBox ID="txtPath" runat="server" Width="150px" ReadOnly="true"  MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvPath" runat="server" ControlToValidate="txtPath"
                                                        Display="None" ErrorMessage="Enter the Path" ValidationGroup="Govalidation"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 17%">
                                                    <asp:Label ID="lblOutput" runat="server" Visible="false" CssClass="styleReqFieldLabel"
                                                        Text="Output"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 20%">
                                                    <asp:DropDownList ID="ddlOutput" runat="server" Visible="false" Width="145px" OnSelectedIndexChanged="ddlOutput_SelectedIndexChanged"
                                                        TabIndex="5">
                                                        <asp:ListItem Value="0">---Select----</asp:ListItem>
                                                        <asp:ListItem Value="1">PDF</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvOutput" runat="server" ControlToValidate="ddlOutput"
                                                        Display="None" InitialValue="0" Enabled ="false" ErrorMessage="Select a Output" ValidationGroup="Govalidation"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldAlign" style="width: 17%">
                                                    <asp:Label ID="lblPANum" runat="server" CssClass="styleReqFieldLabel" Text="Prime Account"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 20%">
                                                    <asp:DropDownList ID="ddlPANum" runat="server" Width="145px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlPANum_SelectedIndexChanged" Style="height: 22px" TabIndex="7">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 17%">
                                                    <asp:Label ID="lblInsDueFromDate" runat="server" CssClass="styleReqFieldLabel" Text="Insurance Due From Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 20%">
                                                    <asp:TextBox ID="txtInsDueFromDate" runat="server" Width="40%" TabIndex="8"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftxtInsDueFromDate" runat="server" Enabled="True"
                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtInsDueFromDate"
                                                        ValidChars="/-">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <cc1:CalendarExtender ID="calInsDueFromDate" runat="server" Enabled="True" Format="DD/MM/YYYY"
                                                        PopupButtonID="imgInsDueFromDate" TargetControlID="txtInsDueFromDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:Image ID="imgInsDueFromDate" runat="server" Height="16px" ImageUrl="~/Images/calendaer.gif" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldAlign" style="width: 17%">
                                                    <asp:Label ID="lblSANum" runat="server" CssClass="styleReqFieldLabel" Text="Sub Account"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 20%">
                                                    <asp:DropDownList ID="ddlSANum" runat="server" Width="145px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlSANum_SelectedIndexChanged" TabIndex="9" Style="height: 22px" >
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 17%">
                                                    <asp:Label ID="lblInsDueToDate" runat="server" CssClass="styleReqFieldLabel" Text="Ins Due To Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 20%">
                                                    <asp:TextBox ID="txtInsDueToDate" runat="server" Width="40%" TabIndex="10"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftxtInsDueToDate" runat="server" Enabled="True"
                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtInsDueToDate"
                                                        ValidChars="/-">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <cc1:CalendarExtender ID="calInsDueToDate" runat="server" Enabled="True" Format="DD/MM/YYYY"
                                                        PopupButtonID="imgInsDueToDate" TargetControlID="txtInsDueToDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:Image ID="imgInsDueToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldAlign" colspan="4">
                                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" CausesValidation="true"
                                                        ValidationGroup="Govalidation" CssClass="styleGridShortButton" TabIndex="11" 
                                                         />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlInsDetails" CssClass="stylePanel" runat="server" Visible="false"
                                        ScrollBars="None" GroupingText="Insurance Details" Height="50%" >
                                        <%--<div id="divInsDetails" style="width: 100%; padding-left: 1%; height:50% " runat="server">--%>
                                            <asp:GridView ID="grvInsDetails" runat="server" Width="100%"  AutoGenerateColumns="False"
                                                EmptyDataText="Insurance Renewal Details Not Found" 
                                            onrowdatabound="grvInsDetails_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Prime Account" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrimeAccount" runat="server" Text='<%#Bind("PANum") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sub Account" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSubAccount" runat="server" Text='<%#Bind("SANum") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCustName" runat="server" Text='<%#Bind("CustomerName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Description" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetDesc" runat="server" Text='<%#Bind("AssetDescription") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Regn No or Serial No" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetRegNo" runat="server" Text='<%#Bind("AssetRegNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Insured By" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInsuredBy" runat="server" Text='<%#Bind("InsuredBy") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Policy No" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPolicyNo" runat="server" Text='<%#Bind("PolicyNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Policy Expiry Date" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPolicyExpiryDate" runat="server" Text='<%#Bind("PolicyExpiryDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Insured Amount" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPolicyInsuredAmount" runat="server" Text='<%#Bind("InsuredAmount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Exclude" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkExclude" runat="server" Checked='<%# GVBoolFormat(Eval("STATUS").ToString()) %>'  />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                       <%-- </div>--%></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                                        OnClientClick="event.srcElement.style.display = 'none';" Text="Save" />
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="styleSubmitButton"
                                        OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" />
                                    <asp:Button ID="btnGenerate" runat="server" CssClass="styleSubmitButton" OnClick="btnGenerate_Click"
                                        Text="Generate" Visible="False" Enabled="false" />
                                    <asp:Button ID="btnCancel" runat="server" Visible="false" CausesValidation="False"
                                        CssClass="styleSubmitButton" OnClick="btnCancel_OnClick" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:ValidationSummary ID="vsInsdetails" runat="server" CssClass="styleMandatoryLabel"
                ValidationGroup="Govalidation" HeaderText="Correct the following validation(s):" />
        </ContentTemplate>
    </asp:UpdatePanel>
  
</asp:Content>
