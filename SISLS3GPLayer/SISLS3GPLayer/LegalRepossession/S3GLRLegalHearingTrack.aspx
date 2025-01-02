<%@ Page Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true"
    CodeFile="S3GLRLegalHearingTrack.aspx.cs" Inherits="LegalRepossession_S3GLRLegalHearingTrack" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
    <%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <table id="table1" runat="server" width="100%" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label ID="lblHeading" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <cc1:TabContainer ID="tabcontainer1" runat="server" AutoPostBack="true" OnActiveTabChanged="User_ActiveTabChanged"
                            CssClass="styleTabPanel" Width="100%" ActiveTabIndex="2">
                            <cc1:TabPanel ID="LegalHearingTrack" runat="server" CssClass="tabpan">
                                <HeaderTemplate>
                                    Legal Hearing Track
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="updatepanellegalhearing" runat="server">
                                        <ContentTemplate>
                                            <table id="table2" runat="server" width="100%">
                                                <tr>
                                                    <td>
                                                        <%-- <asp:Panel ID="pnlHeader" runat="server" GroupingText="Details" CssClass="stylePanel">--%>
                                                        <table id="table3" runat="server" width="100%">
                                                            <tr>
                                                                <td width="50%">
                                                                    <asp:Panel ID="pnlHeader" runat="server" GroupingText="Details" CssClass="stylePanel">
                                                                        <table>
                                                                            <tr>
                                                                                <td width="25%" class="styleFieldLabel">
                                                                                    <asp:Label ID="lblLOB" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                                                                </td>
                                                                                <td width="25%">
                                                                                    <asp:DropDownList ID="ddlLOB" runat="server" Width="165px" AutoPostBack="true" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                                                        ToolTip="Line of Business">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvddlLOB" ValidationGroup="Header" runat="server"
                                                                                        Display="None" ControlToValidate="ddlLOB" SetFocusOnError="true" InitialValue="0"
                                                                                        CssClass="styleMandatoryLabel"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="25%" class="styleFieldLabel">
                                                                                    <asp:Label ID="lblBranch" runat="server" Text="Location" CssClass="styleReqFieldLabel"></asp:Label>
                                                                                </td>
                                                                                <td width="25%">
                                                                                <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                                        OnItem_Selected="ddlBranch_SelectedIndexChanged" IsMandatory="true" ValidationGroup="Submit" ErrorMessage="Enter Location" />
                                                                                    <%--<asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true" Width="165px"
                                                                                        ToolTip="Location" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvddlBranch" ValidationGroup="Header" runat="server"
                                                                                        ControlToValidate="ddlBranch" SetFocusOnError="true" InitialValue="0" CssClass="styleMandatoryLabel"
                                                                                        Display="None"></asp:RequiredFieldValidator>--%>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="25%" class="styleFieldLabel">
                                                                                    <asp:Label ID="lblLNN" runat="server" Text="Legal Note Number" CssClass="styleReqFieldLabel"></asp:Label>
                                                                                </td>
                                                                                <td width="25%">
                                                                                    <asp:DropDownList ID="ddlLNN" runat="server" AutoPostBack="true" Width="165px" ToolTip="Legal Note Number"
                                                                                        OnSelectedIndexChanged="ddlLNN_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvddlLNN" ValidationGroup="Header" runat="server"
                                                                                        SetFocusOnError="true" InitialValue="0" ControlToValidate="ddlLNN" Display="None"
                                                                                        ErrorMessage="Select Legal Note Number" CssClass="styleMandatoryLabel"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="25%">
                                                                                    <asp:Label ID="lblLRNDate" runat="server" Text="LRN Date" CssClass="styleFieldLabel"></asp:Label>
                                                                                </td>
                                                                                <td width="25%">
                                                                                    <asp:TextBox ID="txtLRNDate" Width="165px" runat="server" ContentEditable="False"
                                                                                        ToolTip="LRN Date"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="25%">
                                                                                    <asp:Label ID="lblMLA" runat="server" Text="Prime Account Number" CssClass="styleFieldLabel"></asp:Label>
                                                                                </td>
                                                                                <td width="25%">
                                                                                    <asp:TextBox ID="txtMLA" Width="165px" runat="server" ContentEditable="False" ToolTip="Prime Account Number"></asp:TextBox>
                                                                                    <%--<asp:DropDownList ID="ddlMLA" runat ="server" AutoPostBack ="true" 
                                                        Width ="64%" onselectedindexchanged="ddlMLA_SelectedIndexChanged" ></asp:DropDownList>--%>
                                                                                </td>
                                                                                <%--<td width="25%"><asp:Label ID="lblCustomer" runat ="server" Text ="Customer" CssClass ="styleFieldLabel"></asp:Label></td>
                                                <td width="25%"><asp:TextBox ID="txtCustomer" runat ="server" ></asp:TextBox></td>--%>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="25%">
                                                                                    <asp:Label ID="lblSLA" runat="server" Text="Sub Account Number" CssClass="styleFieldLabel"></asp:Label>
                                                                                </td>
                                                                                <td width="25%">
                                                                                    <asp:TextBox ID="txtSLA" Width="165px" runat="server" ContentEditable="False" ToolTip="Sub Account Number"></asp:TextBox>
                                                                                    <%--<asp:DropDownList ID="ddlSLA" runat ="server" AutoPostBack ="true" 
                                                        Width ="64%" onselectedindexchanged="ddlSLA_SelectedIndexChanged" ></asp:DropDownList>--%>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="25%">
                                                                                    <asp:Label ID="lblLHRNo" runat="server" Text="LHR Number" CssClass="styleFieldLabel"></asp:Label>
                                                                                </td>
                                                                                <td width="25%">
                                                                                    <asp:TextBox ID="txtLHRNo" Width="165px" runat="server" ContentEditable="False" ToolTip="LHR Number"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="25%" class="styleFieldLabel">
                                                                                    <asp:Label ID="lblLHRDate" runat="server" Text="LHR Date" CssClass="styleReqFieldLabel"></asp:Label>
                                                                                </td>
                                                                                <td width="25%">
                                                                                    <asp:TextBox ID="txtLHRDate" Width="165px" runat="server" ContentEditable="False"
                                                                                        ToolTip="LHR Date"></asp:TextBox>
                                                                                    <%-- <asp:Image ID="imgLHRDate" runat ="server" Visible ="false" ImageUrl ="~/Images/calendaer.gif" />
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat ="server" OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID ="imgLHRDate" Enabled ="true" TargetControlID ="txtLHRDate" ></cc1:CalendarExtender>
                                               --%>
                                                                                    <asp:RequiredFieldValidator ID="rfvLHRDate" ValidationGroup="Header" runat="server"
                                                                                        ErrorMessage="Select LHR Date" Display="None" ControlToValidate="txtLHRDate"
                                                                                        CssClass="styleReqFieldLabel"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="Label1" runat="server" Visible="false" Text="Email" CssClass="styleFieldLabel"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkActive" Visible="false" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <%--  <tr>
                                           <td>
                                           <table > <tr>
                                            <td ><asp:Label ID="lblActive"  runat ="server" Text ="SMS" CssClass ="styleFieldLabel"></asp:Label>
                                           </td ><td> <asp:CheckBox ID="CheckBox1" runat ="server"  /></td>
                                           
                                           </tr> </table></td> 
                                           
                                            <td> <table> <tr>  <td ><asp:Label ID="Label1"  runat ="server" Text ="Email" CssClass ="styleFieldLabel"></asp:Label>
                                           </td ><td>   <asp:CheckBox ID="chkActive"  runat ="server"  /></td>
                                           </tr></table> </td>
                                           
                                           </tr>--%>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                                <td width="50%">
                                                                    <asp:Panel ID="PanelCustomerDetails" runat="server" GroupingText="Customer Details"
                                                                        CssClass="stylePanel" Width="100%">
                                                                        <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" ActiveViewIndex="0" />
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <%-- </asp:Panel>--%>
                                                    </td>
                                                </tr>
                                                <%--  <tr>
                                            <td>
                                            <asp:Panel ID="PanelCustomerDetails" runat ="server" GroupingText ="Customer Details" CssClass ="stylePanel" Width ="100%">
                                            <table id ="table8" runat ="server" width ="100%">
                                            <tr><td>
                                            <uc1:S3GCustomerAddress ID ="S3GCustomerAddress1" runat ="server" ActiveViewIndex ="1"  />
                                            </td></tr>
                                            </table
                                            </asp:Panel>
                                            </td>
                                       </tr>--%>
                                                <tr>
                                                    <td width="50%">
                                                        <asp:Panel ID="PanelCaseDetails" runat="server" GroupingText="Case Details" CssClass="stylePanel">
                                                            <table width="99%">
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="25%">
                                                                        <asp:Label ID="lblCaseNo" runat="server" Text="Case No" CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td width="25%">
                                                                        <asp:TextBox ID="txtCaseNo" Width="165px" runat="server" ToolTip="Case No" MaxLength="60"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="fteCaseNo" runat="server" FilterType="Numbers,Custom,Lowercaseletters,uppercaseletters"
                                                                            ValidChars="/\-,()" TargetControlID="txtCaseNo">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvCaseNo" runat="server" ControlToValidate="txtCaseNo"
                                                                            ErrorMessage="Enter Case No" Display="None" ValidationGroup="Header" CssClass="styleMandatoryLabel"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblCourtDetails" runat="server" CssClass="styleReqFieldLabel" Text="Court Details"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCourtDetails" Width="165px" runat="server" TextMode="MultiLine"
                                                                            ToolTip="Court Details" onkeyup="maxlengthfortxt(40);"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="fteCourtDetails" runat="server" FilterType="Numbers,Custom,Lowercaseletters,uppercaseletters"
                                                                            ValidChars=" " TargetControlID="txtCourtDetails">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvtCourtDetails" runat="server" ControlToValidate="txtCourtDetails"
                                                                            ErrorMessage="Enter Court Details" Display="None" CssClass="styleMandatoryLabel"
                                                                            ValidationGroup="Header"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="25%">
                                                                        <asp:Label ID="lblCaseDetails" runat="server" Text="Case Details" CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td width="25%">
                                                                        <asp:TextBox ID="txtCaseDetails" Width="165px" runat="server" TextMode="MultiLine"
                                                                            ToolTip="Case Details" onkeyup="maxlengthfortxt(60);" />
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers,Custom,Lowercaseletters,uppercaseletters"
                                                                            ValidChars=" /\-,()" TargetControlID="txtCaseDetails">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvCaseDetails" runat="server" ControlToValidate="txtCaseDetails"
                                                                            ErrorMessage="Enter Case Details" Display="None" ValidationGroup="Header" CssClass="styleMandatoryLabel"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblRemarks" runat="server" Text="Remarks"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRemarks" Width="165px" runat="server" TextMode="MultiLine" ToolTip="Remarks"
                                                                            onkeyup="maxlengthfortxt(100);"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="PanelAdvocateDetails" runat="server" GroupingText="Advocate Details"
                                                            CssClass="stylePanel">
                                                            <table id="table5" runat="server" width="100%">
                                                                <tr>
                                                                    <td width="25%" class="styleFieldLabel">
                                                                        <asp:Label ID="lblAdvocateName" runat="server" Text="Advocate Name" CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td width="25%">
                                                                        <asp:DropDownList ID="ddlAdvocateName" runat="server" AutoPostBack="true" Width="165px"
                                                                            ToolTip="Advocate Name" OnSelectedIndexChanged="ddlAdvocateName_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvAdvocateName" ValidationGroup="Header" runat="server"
                                                                            ControlToValidate="ddlAdvocateName" ErrorMessage="Select Advocate Name" SetFocusOnError="true"
                                                                            InitialValue="0" CssClass="styleMandatoryLabel" Display="None"></asp:RequiredFieldValidator>
                                                                        <%--<asp:TextBox ID="txtAdvocateName" runat ="server"></asp:TextBox>--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblMobile" runat="server" Text="Mobile" CssClass="styleFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtMobile" Width="165px" runat="server" ToolTip="Mobile" ContentEditable="False"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="25%">
                                                                        <asp:Label ID="lblPhone" runat="server" CssClass="styleFieldLabel" Text="Phone"></asp:Label>
                                                                    </td>
                                                                    <td width="25%">
                                                                        <asp:TextBox ID="txtPhone" Width="165px" runat="server" ToolTip="Phone" ContentEditable="False"></asp:TextBox>
                                                                    </td>
                                                                    <td width="17%">
                                                                        <asp:Label ID="lblEMail" runat="server" CssClass="styleFieldLabel" Text="E-Mail"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtEMail" Width="165px" runat="server" ToolTip="E-Mail" ContentEditable="False"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td>
                                                        <asp:Panel ID="PanelGrid" runat="server" CssClass="stylePanel" GroupingText="Activity Details"
                                                            Width="100%">
                                                            <asp:GridView ID="grdLegalHearing" runat="server" OnRowEditing="grdLegalHearing_OnRowEditing"
                                                                OnRowCancelingEdit="grdLegalHearing_OnRowCancelingEdit" OnRowUpdating="grdLegalHearing_OnRowUpdating"
                                                                AutoGenerateColumns="false" OnRowCommand="grdLegalHearing_RowCommand" OnRowDataBound="grdLegalHearing_RowDataBound"
                                                                ShowFooter="true" Width="99%">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Activity Date">
                                                                        <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDate" runat="server" Text='<% #Bind("ActivityDate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtActivityDate" Width="80px" runat="server" ToolTip="Activity Date"
                                                                                ContentEditable="False"></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="txtActivityDate" Width="80px" runat="server" ToolTip="Activity Date"
                                                                                ContentEditable="False"></asp:TextBox>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Activity Done">
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                        <ItemTemplate>
                                                                            <%-- <div style =" overflow :auto ; height:50px;" id="divDone" >
                                                                            <asp:Label ID="lblActivity" runat="server" Text='<% #Bind("ActivityDone") %>'></asp:Label>
                                                                            </div>--%>
                                                                            <asp:TextBox ID="txtActivity" ReadOnly="true" TextMode="MultiLine" runat="server"
                                                                                Text='<% #Bind("ActivityDone") %>' />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtActivity" runat="server" Width="100px" ToolTip="Activity Done"
                                                                                TextMode="MultiLine" onkeyup="maxlengthfortxt(300);"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers,Custom,Lowercaseletters,uppercaseletters"
                                                                                ValidChars=" /\-,()" TargetControlID="txtActivity">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <asp:RequiredFieldValidator ID="rfvActivity" runat="server" ControlToValidate="txtActivity"
                                                                                ValidationGroup="Footer" ErrorMessage="Enter Activity Done" Display="None"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="txtActivity" runat="server" Width="100px" ToolTip="Activity Done"
                                                                                TextMode="MultiLine" onkeyup="maxlengthfortxt(300);"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers,Custom,Lowercaseletters,uppercaseletters"
                                                                                ValidChars=" /\-,()" TargetControlID="txtActivity">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <asp:RequiredFieldValidator ID="rfvActivity" runat="server" ControlToValidate="txtActivity"
                                                                                ValidationGroup="Footer" ErrorMessage="Enter Activity Done" Display="None"></asp:RequiredFieldValidator>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Actual Points">
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                        <ItemTemplate>
                                                                            <%--<div style =" overflow :auto; height:50px;" id="divDone" >
                                                                            <asp:Label ID="lblActualPoints" runat="server" Text='<% #Bind("ActualPoints") %>'></asp:Label>
                                                                            </div> --%>
                                                                            <asp:TextBox ID="txtActualPoints" ReadOnly="true" TextMode="MultiLine" runat="server"
                                                                                Text='<% #Bind("ActualPoints") %>' />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtActualPoints" Width="100px" runat="server" TextMode="MultiLine"
                                                                                ToolTip="Actual Points" onkeyup="maxlengthfortxt(300);"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="fteCaseNo" runat="server" FilterType="Numbers,Custom,Lowercaseletters,uppercaseletters"
                                                                                ValidChars="/\-,() " TargetControlID="txtActualPoints">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <asp:RequiredFieldValidator ID="rfvActualPoints" runat="server" ControlToValidate="txtActualPoints"
                                                                                ValidationGroup="Footer" ErrorMessage="Enter Actual Points" Display="None"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="txtActualPoints" Width="100px" runat="server" TextMode="MultiLine"
                                                                                ToolTip="Actual Points" onkeyup="maxlengthfortxt(300);"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="fteCaseNo" runat="server" FilterType="Numbers,Custom,Lowercaseletters,uppercaseletters"
                                                                                ValidChars="/\-,() " TargetControlID="txtActualPoints">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <asp:RequiredFieldValidator ID="rfvActualPoints" runat="server" ControlToValidate="txtActualPoints"
                                                                                ValidationGroup="Footer" ErrorMessage="Enter Actual Points" Display="None"></asp:RequiredFieldValidator>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Notify Employee" FooterStyle-Wrap="true">
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNotifyEmployee" runat="server" Text='<% # Bind("NotifyEmployee") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList ID="ddlNotifyEmployee" Width="100px" runat="server" ToolTip="Notify Employee">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvddlNotifyEmployee" ValidationGroup="Footer" runat="server"
                                                                                ControlToValidate="ddlNotifyEmployee" ErrorMessage="Select Notify Employee" SetFocusOnError="true"
                                                                                InitialValue="0" CssClass="styleMandatoryLabel" Display="None"></asp:RequiredFieldValidator></td>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:DropDownList ID="ddlNotifyEmployee" Width="100px" runat="server" ToolTip="Notify Employee">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvddlNotifyEmployee" ValidationGroup="Footer" runat="server"
                                                                                ControlToValidate="ddlNotifyEmployee" ErrorMessage="Select Notify Employee" SetFocusOnError="true"
                                                                                InitialValue="0" CssClass="styleMandatoryLabel" Display="None"></asp:RequiredFieldValidator></td>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Next Hearing Date">
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNextHearingDate" runat="server" Text='<% #Bind("NextHearingDate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtNextHearingDate"  Width="80px" runat="server"
                                                                                ToolTip="Next Hearing Date"></asp:TextBox>
                                                                            <%--<asp:Image ID="imgNextHearingDate" runat ="server" ImageUrl ="~/Images/calendaer.gif" />    --%>
                                                                            <cc1:CalendarExtender ID="CalendarExtender2" Format="dd/MM/yyyy" runat="server" OnClientDateSelectionChanged="checkDate_PrevSystemDate"
                                                                                PopupButtonID="txtNextHearingDate" TargetControlID="txtNextHearingDate">
                                                                            </cc1:CalendarExtender>
                                                                            <asp:RequiredFieldValidator ID="rfvNextHearingDate" runat="server" ControlToValidate="txtNextHearingDate"
                                                                                ValidationGroup="Footer" ErrorMessage="Select Next Hearing Date" Display="None"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="txtNextHearingDate"  runat="server" Width="80px"
                                                                                ToolTip="Next Hearing Date"></asp:TextBox>
                                                                            <%-- <asp:Image ID="imgNextHearingDate" runat ="server" ImageUrl ="~/Images/calendaer.gif" />--%>
                                                                            <cc1:CalendarExtender ID="CalendarExtender2" Format="dd/MM/yyyy" runat="server" OnClientDateSelectionChanged="checkDate_PrevSystemDate"
                                                                                PopupButtonID="txtNextHearingDate" TargetControlID="txtNextHearingDate">
                                                                            </cc1:CalendarExtender>
                                                                            <asp:RequiredFieldValidator ID="rfvNextHearingDate" runat="server" ControlToValidate="txtNextHearingDate"
                                                                                ValidationGroup="Footer" ErrorMessage="Select Next Hearing Date" Display="None"></asp:RequiredFieldValidator>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Notify Employee Value" Visible="false">
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNotifyEmployeeValue" runat="server" Text='<% #Bind("NotifyEmployeeValue") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Mode" Visible="false">
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                        <ItemStyle />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMode" runat="server" Text='<% #Bind("Mode")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblID" runat="server" Text='<% #Bind("ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Action">
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" ToolTip="Edit"></asp:LinkButton>
                                                                            &nbsp;
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                                                ToolTip="Update" CausesValidation="true" ValidationGroup="Footer"></asp:LinkButton>
                                                                            &nbsp;
                                                                            <asp:LinkButton ID="lnkCancelEdit" runat="server" CommandName="Cancel" Text="Cancel"
                                                                                ToolTip="Cancel" CausesValidation="false"></asp:LinkButton>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Button ID="btnAdd" runat="server" CommandName="Add" Text="Add" CssClass="styleSubmitShortButton"
                                                                                ToolTip="Add" CausesValidation="true" ValidationGroup="Footer" />
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                        <asp:Button ID="btnAddNextHearingDetail" runat="server" Text="NextHearingDetail"
                                                            CssClass="styleSubmitButton" OnClick="btnAddNextHearingDetail_Click" Width="105px"
                                                            ToolTip="To Enter NextHearingDetail" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="History" runat="server" CssClass="tabpan">
                                <HeaderTemplate>
                                    History
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="updatepanelhistory" runat="server">
                                        <ContentTemplate>
                                            <table id="table6" runat="server" align="center" border="0" width="100%">
                                                <tr align="center">
                                                    <td>
                                                        <asp:Panel ID="panelHistory" runat="server" CssClass="stylePanel">
                                                            <asp:GridView ID="grvHistory" runat="server" AutoGenerateColumns="false" Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="LHR No">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLHRNoH" runat="server" Text='<% #Bind("LHRNo") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDateH" runat="server" Text='<% #Bind("ActivityDate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Activity Done">
                                                                        <ItemTemplate>
                                                                            <%--<div style ="overflow :auto; height :35px;" >
                                                                            <asp:Label ID="lblActivityDoneH" runat="server" Text='<% #Bind("lblActivityDoneH") %>' style="margin-top:15px;"> ></asp:Label>
                                                                            </div> --%>
                                                                            <asp:TextBox ID="txtActivityDoneH" ReadOnly="true" TextMode="MultiLine" runat="server"
                                                                                Text='<% #Bind("ActivityDone") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" Width="20%" VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Actual Points">
                                                                        <ItemTemplate>
                                                                            <%-- <div style ="overflow :auto; height :35px;">
                                                                            <asp:Label ID="lblActualPointsH" runat="server" Text='<% #Bind("ActualPoints") %>' style="margin-top:15px;"></asp:Label>
                                                                            </div> --%>
                                                                            <asp:TextBox ID="txtActualPointsH" ReadOnly="true" TextMode="MultiLine" runat="server"
                                                                                Text='<% #Bind("ActualPoints") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" Width="20%" VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Notify Employee">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNotifyEmployeeH" runat="server" Text='<% #Bind("NotifyEmployee") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Next Hearing Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNextHearingDate" runat="server" Text='<% #Bind("NextHearingDate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="Closure" runat="server" CssClass="tabpan">
                                <HeaderTemplate>
                                    Closure
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="updatepanelclosure" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="PanelClosure" runat="server" GroupingText="Closure Details" CssClass="stylePanel">
                                                <table id="table7" runat="server" align="center" border="0" cellspacing="15">
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblyn" Text="Close the Legal Hearing Track" runat="server" />
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:RadioButton ID="rbtYes" Text="Yes" runat="server" Checked="false" AutoPostBack="true"
                                                                OnCheckedChanged="FunProEnableClosureYes" />
                                                            <asp:RadioButton ID="rbtNo" Text="No" runat="server" Checked="true" AutoPostBack="true"
                                                                OnCheckedChanged="FunProEnableClosureNo" />
                                                            <%--<asp:RadioButtonList ID="rbtYN" runat ="server" >
                                                    <asp:ListItem Text ="Yes" /> <asp:ListItem Text ="No"  Selected ="True" />
                                                    </asp:RadioButtonList>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblClosureDate" runat="server" Text="Closure Date" CssClass="styleReqFieldLabel"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtClosureDate" runat="server" AutoPostBack="true" ContentEditable="False"
                                                                ToolTip="Closure Date"></asp:TextBox>
                                                            <asp:Image ID="imgClosureDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgClosureDate"
                                                                OnClientDateSelectionChanged="checkDate_NextSystemDate" Enabled="false" TargetControlID="txtClosureDate">
                                                            </cc1:CalendarExtender>
                                                            <asp:RequiredFieldValidator ID="rfvClosureDate" runat="server" ControlToValidate="txtClosureDate"
                                                                Enabled="false" ValidationGroup="Header" ErrorMessage="Select Closure Date" Display="None"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblDegree" runat="server" Text="Degree" CssClass="styleReqFieldLabel"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDegree" runat="server" TextMode="MultiLine" Wrap="true" ToolTip="Degree"
                                                                ReadOnly="true" onkeyup="maxlengthfortxt(200);"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers,Custom,Lowercaseletters,uppercaseletters"
                                                                ValidChars="/\-,() " TargetControlID="txtDegree">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rfvDegree" runat="server" ControlToValidate="txtDegree"
                                                                Enabled="false" ValidationGroup="Header" ErrorMessage="Enter Degree" Display="None"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblRemarksC" runat="server" Text="Remarks"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRemarksC" ReadOnly="true" runat="server" TextMode="MultiLine"
                                                                ToolTip="Remarks" onkeyup="maxlengthfortxt(100);"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </td>
                </tr>
                <tr align="center">
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="styleSubmitButton"
                            ToolTip="To save Details" OnClientClick="return fnCheckPageValidators('Header');"
                            OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="Header" />
                        &nbsp;
                        <asp:Button ID="btnClear" runat="server" Text="Clear" OnClientClick="return fnConfirmClear();"
                            ToolTip="To Clear the changes in page" CssClass="styleSubmitButton" OnClick="btnClear_Click"
                            CausesValidation="false" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="styleSubmitButton"
                            ToolTip="Redirect to ViewPage" OnClick="btnCancel_Click" CausesValidation="false" />
                        <%--&nbsp;
                        
                        <asp:Button  runat="server" ID="btnDelete" CausesValidation ="false" OnClientClick ="return fnConfirmDelete()"  
                        Text ="Delete" CssClass="styleSubmitButton" onclick="btnDelete_Click" />--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="vsHeader" ShowMessageBox="false" ShowSummary="true"
                            HeaderText="Correct the following validation(s):" ValidationGroup="Header" />
                        <asp:ValidationSummary ID="vsFooter" runat="server" ShowMessageBox="false" ShowSummary="true"
                            HeaderText="Correct the following validation(s)" ValidationGroup="Footer" />
                        <asp:HiddenField ID="hdnCustomerId" runat="server" />
                        <asp:HiddenField ID="hdnNHD" runat="server" />
                        <asp:HiddenField ID="hdnLHD" runat="server" />
                        <asp:HiddenField ID="hdncheckdate" runat="server" />
                        <asp:HiddenField ID="hdnLHRDID" runat="server" />
                        <asp:HiddenField ID="hdnaddNHD" runat="server" />
                        <%--<asp:HiddenField ID="hdncutoff" runat ="server" />--%><asp:HiddenField ID="hdnEntry"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleFieldLabel"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
 
// function pageLoad()
//{
//var ddlLOB= document.getElementById('<%=ddlLOB.ClientID%>');
//ddlLOB.focus();
//}
    </script>

</asp:Content>
