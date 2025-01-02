<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLoanAdOperatingLeaseExpenses.aspx.cs" Inherits="LoanAdmin_S3GLoanAdOperatingLeaseExpenses"
    Title="Operating Lease Expenses" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script src="../Scripts/Common.js" language="javascript" type="text/javascript"></script>

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
                    <td width="100%">
                        <asp:Panel ID="pnlOperatingDetails" runat="server" CssClass="stylePanel" GroupingText="Operating Lease Expenses Details"
                            Width="100%">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <%-- First Row --%>
                                <tr>
                                    <%-- LOB --%>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:DropDownList ID="ddlLOB" runat="server" ToolTip="Line of Business" Visible="false">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtLOBName" runat="server" ToolTip="Line of Business" Width="70%"
                                            TabIndex="1"></asp:TextBox>
                                        <asp:HiddenField ID="hidLOBID" runat="server" />
                                        <%--<asp:RequiredFieldValidator ID="RFVLOB" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlLOB" InitialValue="0" ValidationGroup="VGOLE" ErrorMessage="Select the Line of business"
                                            Display="None"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <%--Branch--%>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Location" ID="lblBranch" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                       <%-- <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                            ToolTip="Location" TabIndex="2">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVOLEBranch" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlBranch" InitialValue="0" ValidationGroup="VGOLE" ErrorMessage="Select a Location"
                                            Display="None"></asp:RequiredFieldValidator>--%>
                                              <uc2:Suggest ID="ddlBranch" runat="server" ToolTip="Location" ServiceMethod="GetBranchList" ValidationGroup="VGOLE"
                                                    AutoPostBack="true" OnItem_Selected="ddlBranch_SelectedIndexChanged" ErrorMessage="Select a Location"
                                                    IsMandatory="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Lease Asset Number" ID="lblLeaseassetno" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                     <uc2:Suggest ID="ddlLeaseAssetNo" runat="server" ServiceMethod="GetDocNo" AutoPostBack="true" OnSelectedIndexChanged="ddlLeaseAssetNo_SelectedIndexChanged1"
                                      ValidationGroup="VGOLE" IsMandatory="true"  ErrorMessage="Enter a Lease Asset Number"/>
                                        <%--<asp:DropDownList ID="ddlLeaseAssetNo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLeaseAssetNo_SelectedIndexChanged1"
                                            ToolTip="Lease Asset Number" TabIndex="3">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVleaseassetno" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlLeaseAssetNo" InitialValue="0" ValidationGroup="VGOLE"
                                            ErrorMessage="Select a Lease Asset Number" Display="None"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Asset Description" ID="lblAssetDesc" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtAssetDesc" ReadOnly="True" ToolTip="Asset Description"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label ID="lblDebit" runat="server" CssClass="styleReqFieldLabel" Text="Debit"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:DropDownList ID="ddlDebit" runat="server" AutoPostBack="True" ToolTip="Debit"
                                            OnSelectedIndexChanged="ddlDebit_SelectedIndexChanged" TabIndex="4">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVDebit" runat="server" ControlToValidate="ddlDebit"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a Debit" InitialValue="0"
                                            ValidationGroup="VGOLE"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label ID="lblExpenditureType" runat="server" CssClass="styleReqFieldLabel" Text="Expenditure Type"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:DropDownList ID="ddlExpenditureType" runat="server" ToolTip="Expenditure Type"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlExpenditureType_SelectedIndexChanged"
                                            TabIndex="5">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVExpenditureType" runat="server" ControlToValidate="ddlExpenditureType"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select an Expenditure Type"
                                            InitialValue="0" ValidationGroup="VGOLE" Visible="False"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label ID="lblprimeaccno" runat="server" CssClass="styleDisplayLabel" Text="Prime Account Number"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtprimeAcc" ReadOnly="True" ToolTip="Prime Account Number"></asp:TextBox>
                                        <%--<asp:DropDownList ID="ddlPrimeAcc" runat="server" Width="208px">
                                        </asp:DropDownList>--%>
                                    </td>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label ID="lblsubAccno" runat="server" CssClass="styleDisplayLabel" Text="Sub Account Number"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtsubAccno" ReadOnly="True" ToolTip="Sub Account Number"></asp:TextBox>
                                        <%--<asp:DropDownList ID="ddlSubacc" runat="server" Width="208px">
                                        </asp:DropDownList>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="System JV Number" ID="lblsysJVno" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtSystemJVNo" ReadOnly="True" ToolTip="System JV Number"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="System JV Date" ID="lblsystemjvdate" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtsystemjvdate" ReadOnly="True" ToolTip="System JV Date"
                                            Width="40%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="OLE Number" ID="lblOLENo" CssClass="styleDisplayLabel">  </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtOLENo" ReadOnly="True" ValidationGroup="OLE Number"
                                            ToolTip="OLE Number" TabIndex="9"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Bill Reference Number" ID="lblBillRefNo" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtBillrefno" onkeyup="maxlengthfortxt(30)" ToolTip="Bill Reference Number"
                                            Columns="3" Width="80%" MaxLength="30" TabIndex="7"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVBillRefNo" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="txtBillrefno" ValidationGroup="VGOLE" ErrorMessage="Enter the Bill Reference Number"
                                            Display="None">
                                        </asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender runat="server" ID="FTEBillRefNumber" FilterType="Numbers,Custom ,UppercaseLetters, LowercaseLetters"
                                            ValidChars=".- " TargetControlID="txtBillrefno">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="OLE Date" ID="lblFILDate" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtOLEDate" ToolTip="OLE Date" Columns="4" Width="40%"
                                            TabIndex="8"></asp:TextBox>
                                        <asp:Image ID="imgOLEDate" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="OLE Date" />
                                        <asp:RequiredFieldValidator ID="RFVOLEDate" CssClass="styleMandatoryLabel" runat="server"
                                            ValidationGroup="VGOLE" ControlToValidate="txtOLEDate" Display="None" ErrorMessage="Select the OLE Date"></asp:RequiredFieldValidator>
                                        <cc1:CalendarExtender ID="CECOLEDATE" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDateandAADate"
                                            PopupButtonID="imgOLEDate" TargetControlID="txtOLEDate" Format="dd/MM/yyyy">
                                        </cc1:CalendarExtender>
                                        <asp:HiddenField ID="hidAssetAcquisitionDate" runat="server" />
                                    </td>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Bill Reference Date" ID="lblBillRefDate" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox runat="server" ID="txtBillRefDate" ToolTip="Bill Reference Date" Columns="5"
                                            Width="40%" TabIndex="9"></asp:TextBox>
                                        <asp:Image ID="imgBillRefdate" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Bill Reference Date" />
                                        <asp:RequiredFieldValidator ID="RFVBillRefDate" CssClass="styleMandatoryLabel" runat="server"
                                            ValidationGroup="VGOLE" ControlToValidate="txtBillRefDate" Display="None" ErrorMessage="Select the Bill Reference Date"></asp:RequiredFieldValidator>
                                        <cc1:CalendarExtender ID="CECBillRefDate" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                            PopupButtonID="imgBillRefdate" TargetControlID="txtBillRefDate" Format="dd/MM/yyyy">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" width="25%">
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:Button ID="btnGo" CssClass="styleSubmitShortButton" runat="server" Text="Go"
                                            OnClick="btnGo_Click" ValidationGroup="VGOLE" TabIndex="10" />
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
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <%--<tr>
                                    <td class="styleFieldLabel" width="142px">
                                        <asp:Label ID="lblcusname" runat="server" Text="Customer Name" width="120"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtCustName" runat="server" Width="200px" ReadOnly="True" 
                                            ToolTip="Customer Name"></asp:TextBox>
                                        <asp:HiddenField ID="hidcuscode" runat="server" />
                                        <asp:HiddenField ID="hidcusID" runat="server" />
                                    </td>
                                    <td class="styleFieldLabel" width="135px" >
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="142px">
                                        <asp:Label ID="lblAddress1" runat="server" Text="Address 1"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" >
                                        <asp:TextBox ID="txtAddress1" runat="server" Width="200px" TextMode="MultiLine" 
                                            Rows="3" ReadOnly="True" ToolTip=" Address 1"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" width="135px">
                                        <asp:Label ID="lbladdress2" runat="server" Text="Address 2"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAddress2" runat="server" ReadOnly="True" Width="200px" TextMode="MultiLine"
                                            Rows="3" ToolTip=" Address 2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="142px">
                                        <asp:Label ID="lblCity" runat="server" Text="City"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtCity" runat="server" Width="200px" ReadOnly="True" 
                                            ToolTip="City"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" width="135px">
                                        <asp:Label ID="lblState" runat="server" Text="State"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtState" runat="server" Width="200px" ReadOnly="True" 
                                            ToolTip="State"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="142px">
                                        <asp:Label ID="lblCountry" runat="server" Text="Country"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtCountry" runat="server" Width="200px" ReadOnly="True" 
                                            ToolTip="Country"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" width="135px">
                                        <asp:Label ID="lblPincode" runat="server" Text="PIN Code or Zip Code"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPincode" runat="server" Width="200px" ReadOnly="True" 
                                            ToolTip="Pincode/Zipcode"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="142px">
                                        <asp:Label ID="lblMobile" runat="server" Text="Mobile"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtMobile" runat="server" ReadOnly="True" Width="200px" 
                                            ToolTip="Mobile"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" width="135px">
                                        <asp:Label ID="lblTelephone" runat="server" Text="Telephone"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTelephone" runat="server" ReadOnly="True" Width="200px" 
                                            ToolTip="Telephone"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="142px">
                                        <asp:Label ID="lblEmail" runat="server" Text="EMail Id"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" colspan="3">
                                        <asp:TextBox ID="txtEmailid" runat="server" ReadOnly="True" Width="200px" 
                                            ToolTip="Email ID"></asp:TextBox>
                                    </td>
                                </tr>--%>
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
                <caption>
                    -
                    <%--Second Row For Gridview Heading--%>
                    <%-- <tr>
                    <td class="styleFieldLabel" style="padding-top: 10px">
                        <asp:Label runat="server" Font-Bold="True" Font-Underline="True" Text="Account Level Information"
                            ID="lblAccountLevelInfo" Visible="False"></asp:Label>
                    </td>
                </tr>--%>
                    <%--Gridview --%>
                    <tr>
                        <%--<td style="padding-top: 10px">--%>
                        <td>
                            <asp:Panel ID="pnlGridview" runat="server" CssClass="stylePanel" GroupingText="Account Level Information">
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <%--<div style="overflow-x: hidden; overflow-y: auto; height: 225px;">--%>
                                            <asp:GridView ID="GRVOLE" runat="server" AutoGenerateColumns="False" OnRowCommand="GRVOLE_RowCommand"
                                                OnRowDataBound="GRVOLE_RowDataBound" OnRowDeleting="GRVOLE_RowDeleting" ShowFooter="true"
                                                ToolTip="Account Level Information" Width="99%" TabIndex="11">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl.No" ItemStyle-Width="30px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="30px" HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderText="GL Account">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGLAcc" runat="server" Text='<%#Eval("GLAcc")%>' Width="110px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlGLAcc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGLAcc_SelectedIndexChanged"
                                                                ValidationGroup="Footer">
                                                            </asp:DropDownList>
                                                            <%--  <asp:RequiredFieldValidator ID="rfvGLAcc" CssClass="styleMandatoryLabel"
                                        runat="server" ControlToValidate="ddlGLAcc" InitialValue="0"  
                                        ErrorMessage="Select the GL Acc" ValidationGroup="Footer"
                                        Display="None">
                                    </asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderText="SL Account">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSLAcc" runat="server" Text='<%#Eval("SLAcc")%>' Width="110px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlSLAcc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSLAcc_SelectedIndexChanged"
                                                                ValidationGroup="Footer" Width="100px">
                                                            </asp:DropDownList>
                                                            <%--  <asp:RequiredFieldValidator ID="rfvSLAcc" CssClass="styleMandatoryLabel"
                                        runat="server" ControlToValidate="ddlSLAcc" InitialValue="0"  
                                        ErrorMessage="Select the SL Acc"  ValidationGroup="Footer"
                                        Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="158px" HeaderText="Description">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDesc" runat="server" Text='<%#Eval("Desc")%>' Width="150px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtDesc" runat="server" MaxLength="80" Text='<%# Bind("Desc")%>'
                                                                Width="158px"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="158px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="90px" HeaderText="Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Amount")%>' Width="110px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtAmount" runat="server" MaxLength="13" Text='<%# Bind("Amount")%>'
                                                                Width="100px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FTEAmount" runat="server" FilterType="Numbers,Custom"
                                                                TargetControlID="txtAmount" ValidChars=".">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="90px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="160px" HeaderText="Balancing Credit GL">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBalCreGL" runat="server" Text='<%#Eval("BalCreGL")%>' Width="160px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtBalCreGL" runat="server" ReadOnly="True" Text='<%# Bind("BalCreGL")%>'
                                                                Width="160px"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="160px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="120px" HeaderText="Balancing Credit SL">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBalCreSL" runat="server" Text='<%#Eval("BalCreSL")%>' Width="120px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <FooterTemplate>
                                                            <%--  <asp:TextBox ID="txtBalCreSL" runat="server" ReadOnly="True" 
                                                                Text='<%# Bind("BalCreSL")%>' Width="120px"></asp:TextBox>--%>
                                                            <asp:DropDownList ID="ddlBalCreSL" runat="server" ValidationGroup="Footer" Width="150px" onmouseover="ddl_itemchanged(this);">
                                                            </asp:DropDownList>
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="120px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <%--<asp:LinkButton Text="Update"  runat="server" ID="lnkbtnDelete" CommandName="Delete"></asp:LinkButton>--%>
                                                            <asp:LinkButton ID="lnkbtnDelete" runat="server" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Button ID="btnAdd" runat="server" CommandName="AddNew" CssClass="styleSubmitShortButton"
                                                                Text="Add" ValidationGroup="Footer" Width="100%" />
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="50px" />
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
                    <tr align="center" class="styleButtonArea" style="padding-left: 4px">
                        <td>
                            <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                                OnClientClick="return fnCheckPageValidators();" Text="Save" ToolTip="Save" />
                            &nbsp;<asp:Button ID="btnClear" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                                Height="26px" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();"
                                Text="Clear" ToolTip="Clear" />
                            &nbsp;<asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                                OnClick="btnCancel_Click" Text="Cancel" ToolTip="Cancel" ValidationGroup="PRDDC" />
                        </td>
                    </tr>
                    <tr class="styleButtonArea">
                        <td>
                            <asp:ValidationSummary ID="vsFactoringInvoiceLoadinf" runat="server" CssClass="styleMandatoryLabel"
                                HeaderText="Correct the following validation(s):" Height="177px" ShowMessageBox="false"
                                ShowSummary="true" ValidationGroup="VGOLE" Width="500px" />
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
                            <input id="hdnRowID" runat="server" type="hidden" value="0" />
                        </td>
                    </tr>
                </caption>
            </table>
         
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--Condition for NOt greater than today day and not greater then Asset Acquisition date--%>

    <script language="javascript" type="text/javascript">
        function checkDate_NextSystemDateandAADate(sender, args) {
            var today = new Date();
            var AADate = new Date(document.getElementById('<%=hidAssetAcquisitionDate.ClientID%>').value);
            if (sender._selectedDate > today) {
                alert('Selected date cannot be greater than system date');
                sender._textbox.set_Value(today.format(sender._format));
            }
            else if (sender._selectedDate < AADate) {
                alert('OLE date should be Less then Asset Acquisition Date');
                sender._textbox.set_Value(today.format(sender._format));
            }
        }
    </script>

</asp:Content>
