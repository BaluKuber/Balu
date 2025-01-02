<%@ Page Title="S3G - User Management" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GSysAdminUserManagement_Add.aspx.cs" Inherits="S3GSysAdminUserManagement_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="User Management" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <cc1:TabContainer ID="tcUserMgmt" runat="server" CssClass="styleTabPanel" Width="99%"
                            ScrollBars="Auto" ActiveTabIndex="0">
                            <cc1:TabPanel runat="server" HeaderText="Basic Details" ID="tbUserMgmt" CssClass="tabpan"
                                BackColor="Red">
                                <HeaderTemplate>
                                    User Management
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td class="styleFieldLabel" style="width: 20%">
                                                        <asp:Label runat="server" Text="User Code" ID="lblUserCode" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtUserCode" runat="server" CssClass="styleTextUpperCase" MaxLength="15"
                                                            Width="45%"></asp:TextBox>
                                                        <asp:Panel ID="PopupTool_UserCode" runat="server" BorderWidth="1" CssClass="styleRecordCount" Style="display: none"
                                                            Width="30%">
                                                            <asp:Label ID="lblTooltip" runat="server" Text=" Must begin with an alphabet and length should be minimum of 4 characters " />
                                                        </asp:Panel>
                                                        <cc1:HoverMenuExtender ID="HoverMenuExtender1" TargetControlID="txtUserCode" runat="server"
                                                            PopupControlID="PopupTool_UserCode" PopupPosition="Right" PopDelay="50" />
                                                    </td>
                                                    <td class="styleFieldLabel" style="width: 320px; text-align: left">
                                                        <asp:CheckBox runat="server" AutoPostBack="True" ID="chkCopyProfile" OnCheckedChanged="chkCopyProfile_CheckedChanged"
                                                            Text=" " /><span>Copy Profile</span>
                                                        <%-- <asp:DropDownList ID="ddlCopyProfile" AutoPostBack="True" Enabled="False" runat="server"
                                                            OnSelectedIndexChanged="ddlCopyProfile_OnSelectedIndexChanged" Width="151px">
                                                        </asp:DropDownList>--%>
                                                        <uc2:Suggest ID="ddlCopyProfile" AutoPostBack="true" runat="server" ServiceMethod="GetUsers" OnItem_Selected="ddlCopyProfile_OnSelectedIndexChanged" IsMandatory="false"
                                                            ValidationGroup="save" ErrorMessage="Enter the 'Copy Profile'" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtUserCode"
                                                            FilterType="Numbers, UppercaseLetters, LowercaseLetters" Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvUserCode" ValidationGroup="save" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtUserCode" Display="None" ErrorMessage="Enter the User Code"></asp:RequiredFieldValidator>
                                                            
                                                             <%--<asp:RegularExpressionValidator
                                                                ID="revUserCode" runat="server" Display="None" ErrorMessage="User Code must begin with an alphabet and length should be minimum of 4 characters"
                                                                ControlToValidate="txtUserCode" ValidationExpression="^[A-Za-z](\w){3,15}"></asp:RegularExpressionValidator>
                                                        <asp:RequiredFieldValidator ID="RFVCopyProfile" Enabled="false" ValidationGroup="save"
                                                            CssClass="styleMandatoryLabel" runat="server" InitialValue="0" ControlToValidate="ddlCopyProfile"
                                                            Display="None" ErrorMessage="Select the 'Copy Profile'"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                            <td colspan="3" class="styleMandatoryNotes">
                                                (Must begin with an alphabet and length should be minimum of 4 characters)
                                            </td>
                                        </tr>--%>
                                                <tr>
                                                    <td class="styleFieldLabel" style="width: 20%">
                                                        <asp:Label runat="server" Text="User Name" ID="lblUserName" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtUserName" runat="server" MaxLength="50" Style="width: 85%"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="rfvUserName" ValidationGroup="save" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtUserName" Display="None" ErrorMessage="Enter the User Name"></asp:RequiredFieldValidator><cc1:FilteredTextBoxExtender
                                                                ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtUserName" FilterType="Custom, UppercaseLetters, LowercaseLetters"
                                                                ValidChars=" " Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel" style="width: 20%">
                                                        <asp:Label runat="server" Text="Password" ID="lblPassword" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:Panel ID="Pnl_Password" runat="server" BorderWidth="1" CssClass="styleRecordCount" Style="display: none"
                                                            Width="50%">
                                                            <asp:Label ID="lblPWDString" runat="server" Text="Should contain atleast one UPPER case,one lower case and a number or a special
                                                            character" />
                                                        </asp:Panel>
                                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="15" Width="32%"></asp:TextBox>
                                                        <cc1:HoverMenuExtender ID="HoverMenuExtender2" TargetControlID="txtPassword" runat="server"
                                                            PopupControlID="Pnl_Password" PopupPosition="Right" PopDelay="50" />
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <div id="divReset" style="display: none" runat="server">
                                                            <input type="checkbox" id="chkResetPwd" runat="server" onclick="fnEnablePwd();" />
                                                            <span>Reset Password</span>
                                                        </div>
                                                        <asp:RequiredFieldValidator ID="rfvPassword" ValidationGroup="save" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtPassword" Display="None" ErrorMessage="Enter the Password"></asp:RequiredFieldValidator>
                                                        <span class="styleMandatoryNotes"></span>
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                            <td colspan="3" class="styleMandatoryNotes">
                                                (Should contain atleast one UPPER case,one lower case and a number or a special
                                                character)
                                            </td>
                                        </tr>--%>
                                                <tr>
                                                    <td class="styleFieldLabel" style="width: 20%">
                                                        <asp:Label runat="server" Text="Mobile Number" ID="lblMobile"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" style="height: 20px">
                                                        <asp:TextBox ID="txtMobileNo" runat="server" MaxLength="12" Width="32%"></asp:TextBox>
                                                    </td>
                                                    <td style="height: 20px">
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtMobileNo"
                                                            FilterType="Numbers" Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel" style="width: 20%">
                                                        <asp:Label runat="server" Text="EMail Id" ID="lblEmail" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtEMailId" runat="server" MaxLength="60" Style="width: 85%"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="rfvEMailId" ValidationGroup="save" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtEMailId" Display="None" ErrorMessage="Enter the EMail Id"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                                ID="revEmailId" runat="server" ControlToValidate="txtEmailId" Display="None"
                                                                ErrorMessage="Enter a valid Email Id" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel" style="width: 20%">
                                                        <asp:Label runat="server" Text="Date of Joining" ID="lblDateofJoining" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtDateofJoining" runat="server" Width="45%"></asp:TextBox>&nbsp;
                                                        <asp:Image ID="imgDateofJoining" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                        <%--<asp:Panel ID="pnl_Date" runat="server" BorderWidth="1" CssClass="styleRecordCount" Style="display: none"
                                                            Width="40%">
                                                            <asp:Label ID="Label3" runat="server" Text="Date of Joining should be greater than the Date of Incorporation of the company" />
                                                        </asp:Panel>--%>
                                                        <cc1:HoverMenuExtender ID="HoverMenuExtender3" TargetControlID="imgDateofJoining"
                                                            runat="server" PopupControlID="pnl_Date" PopupPosition="Right" PopDelay="50" />
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="rfvDateofJoining" ValidationGroup="save" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtDateofJoining" Display="None" ErrorMessage="Enter the Date of Joining"></asp:RequiredFieldValidator><cc1:CalendarExtender
                                                                runat="server" Format="dd/MM/yyyy" TargetControlID="txtDateofJoining" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                                PopupButtonID="imgDateofJoining" ID="CalendarExtender1" Enabled="True">
                                                            </cc1:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <%-- <tr>
                                            <td colspan="3" class="styleMandatoryNotes">
                                                (Date of Joining should be greater than the Date of Incorporation of the company)
                                            </td>
                                        </tr>--%>
                                                <tr>
                                                    <td class="styleFieldLabel" style="width: 20%">
                                                        <asp:Label runat="server" Text="Designation" ID="lblDesignation" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <%--<asp:TextBox ID="txtDesignation" runat="server" MaxLength="40" Style="width: 85%"></asp:TextBox>--%>
                                                        <asp:DropDownList ID="txtDesignation" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="rfvDesignation" ValidationGroup="save" CssClass="styleMandatoryLabel"
                                                            runat="server" Display="None" InitialValue="0" ControlToValidate="txtDesignation"
                                                            ErrorMessage="Select the Designation"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel" style="width: 20%">
                                                        <asp:Label ID="lblLevel" runat="server" CssClass="styleReqFieldLabel" Text="User Level"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:DropDownList ID="ddlLevel" runat="server" onchange="fnDisableReportingLevel();"
                                                            Style="width: 55%">
                                                            <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                                            <asp:ListItem Text="Staff level" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Officer level" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Manager" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Top Management" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="System Admin" Value="5"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="rfvLevel" ValidationGroup="save" CssClass="styleMandatoryLabel"
                                                            runat="server" InitialValue="-1" Display="None" ControlToValidate="ddlLevel"
                                                            ErrorMessage="Select the User Level"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel" style="width: 20%">
                                                        <asp:Label ID="lblReportingLevel" runat="server"  Text="Reporting - Next Level"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <uc2:Suggest ID="ddlReportingLevel" runat="server" ServiceMethod="GetUsers" ErrorMessage="Enter the Reporting - Next Level" />
                                                        <%-- <asp:DropDownList ID="ddlReportingLevel" runat="server" Style="width: 55%">
                                                            
                                                        </asp:DropDownList>--%>
                                                        <%--<asp:ListItem Text= '<%Bind("User_Name") %>'></asp:ListItem>--%>
                                                    </td>
                                                    <td>
                                                        <%-- <asp:RequiredFieldValidator ID="rfvReportingLevel" ValidationGroup="save" CssClass="styleMandatoryLabel"
                                                            runat="server" InitialValue="0" Display="None" ControlToValidate="ddlReportingLevel"
                                                            ErrorMessage="Select the Reporting - Next Level" Enabled="true"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel" style="width: 20%">
                                                        <asp:Label ID="lblHigherLevel" runat="server" Text="Higher Level"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <uc2:Suggest ID="ddlHigherLevel" runat="server" ServiceMethod="GetUsers" ErrorMessage="Enter the Higher Level" />
                                                        <%-- <asp:DropDownList ID="ddlHigherLevel" runat="server" Style="width: 55%">
                                                        </asp:DropDownList>--%>
                                                    </td>
                                                    <td>
                                                        <%--<asp:RequiredFieldValidator ID="rfvHigherLevel" ValidationGroup="save" CssClass="styleMandatoryLabel"
                                                            runat="server" Enabled="false" InitialValue="0" Display="None" ControlToValidate="ddlHigherLevel"
                                                            ErrorMessage="Select the Higher Level"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel" style="width: 20%">
                                                        <asp:Label ID="lblUserGroup" runat="server" CssClass="styleReqFieldLabel" Text="User Group"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <cc1:ComboBox ID="ddlUserGroup" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                            AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                            Width="80px">
                                                        </cc1:ComboBox>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="RFVddlUserGroup" ValidationGroup="save" CssClass="styleMandatoryLabel"
                                                            runat="server" InitialValue="0" Display="None" ControlToValidate="ddlUserGroup"
                                                            SetFocusOnError="true" ErrorMessage="Select the User Group"></asp:RequiredFieldValidator>
                                                        <%--<asp:RequiredFieldValidator ID="RFVddlUserGroup" ValidationGroup="save" CssClass="styleMandatoryLabel"
                                                            runat="server" Display="None" ControlToValidate="ddlUserGroup"  InitialValue="0"  SetFocusOnError="true"
                                                            ErrorMessage="Select the User Group"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel" style="width: 20%">
                                                        <asp:Label runat="server" Text="Active" ID="lblActive"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" colspan="2">
                                                        <asp:CheckBox ID="chkActive" Checked="True" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="ddlCopyProfile" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Access" ID="tbBranchAccess" CssClass="tabpan"
                                BackColor="Red">
                                <HeaderTemplate>
                                    Access
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <table cellpadding="0" cellspacing="0" style="width: 99%">
                                                <tr>
                                                    <td colspan="2">
                                                        <table cellpadding="0" cellspacing="0" style="width: 85%">
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" Text="Action" ID="lblAction" CssClass="styleReqFieldLabel">
                                                                    </asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:DropDownList ID="ddlAction" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged">
                                                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                                        <asp:ListItem Value="0">Deletion</asp:ListItem>
                                                                        <asp:ListItem Value="1">Modification</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RFVddlAction" runat="server" ValidationGroup="save"
                                                                        Display="None" CssClass="styleMandatoryLabel" ErrorMessage="Select the Action"
                                                                        InitialValue="-1" ControlToValidate="ddlAction" Enabled="false" />
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblDelMod" runat="server" CssClass="styleReqFieldLabel" Visible="false" />
                                                                </td>
                                                                <td class="styleFieldAlign" colspan="2">
                                                                    <%--<asp:CheckBox ID="chkModifyAccess" runat="server" AutoPostBack="true" />--%>
                                                                    <asp:DropDownList ID="ddlModification" Visible="false" runat="server" AutoPostBack="true"
                                                                        OnSelectedIndexChanged="ddlModification_SelectedIndexChanged">
                                                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                                        <asp:ListItem Value="2">Access Modification</asp:ListItem>
                                                                        <asp:ListItem Value="3">User Details</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList ID="ddldeletion" Visible="false" runat="server" AutoPostBack="true"
                                                                        OnSelectedIndexChanged="ddldeletion_SelectedIndexChanged">
                                                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                                        <asp:ListItem Value="0">Line of Business</asp:ListItem>
                                                                        <asp:ListItem Value="1">Locations</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RFVddlModification" runat="server" ValidationGroup="save"
                                                                        Display="None" CssClass="styleMandatoryLabel" ErrorMessage="Select the Modification"
                                                                        InitialValue="-1" ControlToValidate="ddlModification" />
                                                                    <asp:RequiredFieldValidator ID="RFVddldeletion" runat="server" ValidationGroup="save"
                                                                        Display="None" CssClass="styleMandatoryLabel" ErrorMessage="Select the Deletion"
                                                                        InitialValue="-1" ControlToValidate="ddldeletion" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel">
                                                                    </asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLOB_OnSelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RfvddlLOB" InitialValue="0" ValidationGroup="save"
                                                                        Display="None" CssClass="styleMandatoryLabel" runat="server" ErrorMessage="Select the Line of Business"
                                                                        ControlToValidate="ddlLOB" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkCopyLobprofile" Enabled="false" runat="server" AutoPostBack="true"
                                                                        OnCheckedChanged="chkCopyLobprofile_OnCheckedChanged" Visible="false" />
                                                                    <asp:Label runat="server" Text="Copy Profile From" ID="lblLOBCpyProfile" CssClass="styleDisplayLabel" Visible="false">
                                                                    </asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:DropDownList ID="ddlCopyLOb" runat="server" Enabled="false" AutoPostBack="true"
                                                                        OnSelectedIndexChanged="ddlCopyLOb_OnSelectedIndexChanged" Visible="false">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RFVCopyLOb" ValidationGroup="save" Visible="false" CssClass="styleMandatoryLabel"
                                                                        runat="server" Enabled="false" InitialValue="0" Display="None" ControlToValidate="ddlCopyLOb"
                                                                        ErrorMessage="Select the 'Copy Profile From'"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="height: 300px">
                                                    <td valign="top" style="width: 50%">
                                                        <asp:Panel ID="pnlLocation" runat="server" CssClass="stylePanel" GroupingText="Location">
                                                            <div style="overflow: auto; height: 300px">
                                                                <asp:TreeView ID="treeview" runat="server" ImageSet="Simple" ShowCheckBoxes="Parent,Leaf"
                                                                    ShowLines="True" OnPreRender="treeview_PreRender" DataSourceID="XmlDataSource1"
                                                                    OnTreeNodeCheckChanged="treeview_OnTreeNodeCheckChanged" RootNodeStyle-ForeColor="#003d9e"
                                                                    LeafNodeStyle-ForeColor="#003d9e" ParentNodeStyle-ForeColor="#003d9e" SelectedNodeStyle-BackColor="#99CCFF"
                                                                    RootNodeStyle-Font-Size="14px" LeafNodeStyle-Font-Size="14px" ParentNodeStyle-Font-Size="14px">
                                                                    <DataBindings>
                                                                        <asp:TreeNodeBinding DataMember="MenuItem" TextField="title" ToolTipField="title"
                                                                            ValueField="Location_ID" SelectAction="Expand" />
                                                                    </DataBindings>
                                                                </asp:TreeView>
                                                                &nbsp;
                                                                <asp:XmlDataSource ID="XmlDataSource1" runat="server" TransformFile="~/TransformXSLT.xsl"
                                                                    EnableCaching="False"></asp:XmlDataSource>
                                                            </div>
                                                        </asp:Panel>
                                                    </td>
                                                    <td valign="top" style="width: 50%">
                                                        <asp:Panel ID="PnlRoleCode" runat="server" CssClass="stylePanel" GroupingText="Role code">
                                                            <div style="overflow: auto; height: 300px">
                                                                <asp:GridView ID="grvRoleCode" runat="server" AutoGenerateColumns="False" OnRowDataBound="grvRoleCode_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Role Code">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRoleCode" runat="server" Text='<%#Eval("Role_Code")%>'></asp:Label><asp:Label
                                                                                    ID="lblRoleCenterID" runat="server" Visible="false" Text='<%#Eval("Role_Code_ID")%>'></asp:Label><asp:Label
                                                                                        ID="lblAssigned" runat="server" Visible="false" Text='<%#Eval("Assigned")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField ItemStyle-HorizontalAlign="Left" DataField="ProgramDesc" HeaderText="Program Description" />
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>Select All
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr align="center">
                                                                                        <td>
                                                                                            <asp:CheckBox ID="chkAll" runat="server" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td></td>
                                                                                    </tr>
                                                                                    <tr align="left">
                                                                                        <td>
                                                                                            <asp:CheckBox ID="chkSelRoleCode" runat="server" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                    <RowStyle HorizontalAlign="Center" />
                                                                </asp:GridView>
                                                            </div>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <%-- <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>--%>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </td>
                </tr>
                <tr style="padding-bottom: 10px; padding-top: 6px;" runat="server" id="trLOBMapping">
                    <td>
                        <asp:Label runat="server" Text="Line of Business" ID="Label1" CssClass="styleFieldLabel"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlLOBMapping" Width="150px">
                        </asp:DropDownList>
                        <asp:Button runat="server" ID="btnRemoveLOB" CssClass="styleSubmitButton" Text="Delete LOB Mapping"
                            OnClientClick="return fnRemoveLOBMap();" Width="120px" OnClick="btnRemoveLOBMapping_Click" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center">
                        <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidation();"
                            CssClass="styleSubmitButton" Text="Save" ValidationGroup="save" OnClick="btnSave_Click" />
                        <asp:Button runat="server" ID="btnClear" Text="Clear" CausesValidation="false" OnClientClick="return fnConfirmClear();"
                            CssClass="styleSubmitButton" OnClick="btnClear_Click" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="False"
                            CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="vsUserMgmt" HeaderText="Please correct the following validation(s):"
                            CssClass="styleSummaryStyle" Width="500px" ShowMessageBox="false" ShowSummary="true" />
                    </td>
                </tr>
                <tr class="styleFieldAlign">
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                        <asp:CustomValidator ID="CVUsermanagement" runat="server" Display="None" ValidationGroup="save"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hdnRegionVal" value="0" />
            <input type="hidden" id="hdnLOBPresent" runat="server" value="0" />
            <input type="hidden" id="hdnID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">

        var bResult;

        function funsetsavetext(iLevel) {

            if (iLevel >= 0) {
                document.getElementById('<%=btnSave.ClientID%>').innerText = 'Delete';
            }
            else {
                document.getElementById('<%=btnSave.ClientID%>').innerText = 'Save';
            }
        }


        function fnDisableReportingLevel() {

            var iLevel = document.getElementById('<%=ddlLevel.ClientID%>').options.value;

            if (iLevel == "4" || iLevel == "5") {

                document.getElementById('ctl00_ContentPlaceHolder1_tcUserMgmt_tbUserMgmt_ddlReportingLevel_rfvMultiSuggest').enabled = false;
                document.getElementById('ctl00_ContentPlaceHolder1_tcUserMgmt_tbUserMgmt_ddlHigherLevel_rfvMultiSuggest').enabled = false;

            }
            else {
                document.getElementById('ctl00_ContentPlaceHolder1_tcUserMgmt_tbUserMgmt_ddlReportingLevel_rfvMultiSuggest').enabled = false;
                document.getElementById('ctl00_ContentPlaceHolder1_tcUserMgmt_tbUserMgmt_ddlHigherLevel_rfvMultiSuggest').enabled = false;
            }
        }


        function fnEnablePwd() {
            if (document.getElementById('<%=chkResetPwd.ClientID%>').checked) {
                document.getElementById('<%=txtPassword.ClientID%>').disabled = false;
                document.getElementById('<%=txtPassword.ClientID%>').value = '';
                document.getElementById('<%=txtPassword.ClientID%>').focus();
            }
            else {
                document.getElementById('<%=txtPassword.ClientID%>').disabled = true;
                document.getElementById('<%=txtPassword.ClientID%>').value = '*************';

            }
        }

        function fnCheckPageValidation() {
            if (!fnCheckPageValidators())
                return false;

            if (bResult) {
                bResult = fnIsCheckboxChecked('<%=grvRoleCode.ClientID%>', 'chkSelRoleCode', 'Role Code from role mapping');
            }
            return bResult;

        }

        function fnRemoveLOBMap() {

            if (document.getElementById('<%=ddlLOBMapping.ClientID%>').options.selectedIndex == 0) {
                alert('Please select Line of Business');
                return false;
            }

            if (!confirm('Do you want to delete?')) {
                return false;
            }


            return true;
        }

        function fnDisableLOBOtherCheckBoxes(objid) {

        }

        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("", "");
            }
        }


    </script>

</asp:Content>
