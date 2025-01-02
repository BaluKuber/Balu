<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GORGCreditScoreGuide_Add.aspx.cs" Inherits="S3GORGCreditScoreGuide_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="styleFieldAlign">
                                    <asp:RadioButtonList ID="rdbLevel" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdbLevel_SelectedIndexChanged">
                                        <asp:ListItem Text="Transaction Level" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Customer Level" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>

                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlLOB" AutoPostBack="true" Width="240px" OnSelectedIndexChanged="ddlLOB_OnSelectedIndexChanged"
                                        runat="server" ToolTip="Line of Business">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvLOB" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="ddlLOB" InitialValue="0" ErrorMessage="Select the Line of Business"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Product" ID="lblProduct">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlProductCode" Width="240px" runat="server" ToolTip="Product">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Constitution" ID="lblConstitution" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlConstitution" Width="240px" runat="server" ToolTip="Constitution">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvConstitution" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="ddlConstitution" InitialValue="0" ErrorMessage="Select the Constitution"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr style="display: block">
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="No. of Years" ID="lblNoofYears" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtNoofYears" onkeypress="if(window.event.keyCode==48 || window.event.keyCode>54) window.event.keyCode=0;"
                                        onchange="document.getElementById('ctl00_ContentPlaceHolder1_txtPrvYears').value=''; document.getElementById('ctl00_ContentPlaceHolder1_txtNxtYears').value='';"
                                        OnTextChanged="NoofYears_TextChanged" MaxLength="1" Width="50px" runat="server"
                                        Style="text-align: right">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvYears" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="txtNoofYears" ErrorMessage="Enter the No. of Years" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderN" runat="server" TargetControlID="txtNoofYears"
                                        FilterType="Numbers,Custom" InvalidChars="0" Enabled="True">
                                    </cc1:FilteredTextBoxExtender>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label runat="server" Text="Prev." ID="lblPrvYears" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="txtPrvYears" onkeypress="if(window.event.keyCode>51) window.event.keyCode=0;"
                                        onchange="fnSetNextYear();" MaxLength="1" Width="50px" runat="server" Style="text-align: right">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="styleMandatoryLabel"
                                        runat="server" ControlToValidate="txtPrvYears" ErrorMessage="Enter the No. of Previous Years"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtPrvYears"
                                        FilterType="Numbers,Custom" InvalidChars="0" Enabled="True">
                                    </cc1:FilteredTextBoxExtender>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label runat="server" Text="Next" ID="lblNxtYears" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="txtNxtYears" onkeypress="if(window.event.keyCode>54) window.event.keyCode=0;"
                                        MaxLength="1" Width="50px" runat="server" Style="text-align: right">
                                    </asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnYrGo" runat="server" CssClass="styleGridShortButton" Text="Go"
                                        ToolTip="Go" OnClick="btnYrGo_Click"></asp:Button>
                                </td>
                                <td>
                                    <asp:RangeValidator ID="rfvNoofYears" runat="server" ControlToValidate="txtNoofYears"
                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="No of Years cannot exceed 6"
                                        MaximumValue="6"></asp:RangeValidator>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Year Value" ID="lblYear" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList runat="server" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                                        ID="ddlYear" ToolTip="Year">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label ID="lblActive" runat="server" CssClass="styleDisplayLabel" Text="Active"> </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" TabIndex="3" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="99%" class="styleContentTable">
                                    <table style="float: right;" width="100%" cellpadding="0" cellspacing="0">
                                        <tr class="stylePagingControl">
                                            <td>
                                                <asp:RadioButtonList runat="server" ID="rbtYears" RepeatDirection="Horizontal" AutoPostBack="true"
                                                    OnSelectedIndexChanged="rbtYears_SelectedIndexChanged">
                                                </asp:RadioButtonList>
                                            </td>
                                            <td align="right">
                                                <asp:DropDownList runat="server" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                                                    ID="ddlFinanceYear" ToolTip="Year" Visible="false">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnAddYear" CssClass="styleSubmitButton" runat="server" Text="Add Year"
                                                    ToolTip="Add" Style="background-image: url(../Images/btn_Silver.jpg); border-width: 0px; width: 78px; height: 22px;"
                                                    Enabled="false" Visible="true" OnClick="btnAddYear_Click"></asp:Button>
                                                <asp:Button ID="btnUpdateYear" CssClass="styleSubmitButton" runat="server" Text="Update Year"
                                                    ToolTip="Add" Style="background-image: url(../Images/btn_Silver.jpg); border-width: 0px; width: 78px; height: 22px;"
                                                    Enabled="true" Visible="false" OnClick="btnUpdateYear_Click"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                    <cc1:TabContainer ID="tcCreditScore" runat="server" CssClass="styleTabPanel" Width="100%"
                                        ScrollBars="None" TabStripPlacement="top" ActiveTabIndex="1">
                                        <cc1:TabPanel ID="tpMainPage" runat="server" CssClass="tabpan" HeaderText="Main Page"
                                            BackColor="Red" Style="padding: 0px">
                                            <HeaderTemplate>
                                                Main Page
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="upMainPage" runat="server">
                                                    <ContentTemplate>
                                                        <table width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td width="100%">
                                                                    <asp:GridView runat="server" ShowFooter="true" ID="grvCreditScore" Width="100%" OnRowDataBound="grvCreditScore_RowDataBound"
                                                                        OnRowCommand="grvCreditScore_RowCommand" OnRowDeleting="grvCreditScore_RowDeleting"
                                                                        RowStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" AutoGenerateColumns="False">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Line No">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblLineNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                                                    <asp:Label ID="lblScoreAssigned" runat="server" Text="0" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblYear" runat="server" Text='<%# Eval("Year")%>' Visible="false"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="30px" HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Description">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtDesc" Text='<%# Bind("Desc")%>' MaxLength="50" Width="200px"
                                                                                        runat="server" ToolTip="Description"></asp:TextBox>
                                                                                    <%-- code block added for bug fixing - kuppusamy.b - 14-Feb-2012 - Issue ID - 5402--%>
                                                                                    <cc1:FilteredTextBoxExtender ID="fteDescH" runat="server" TargetControlID="txtDesc"
                                                                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" InvalidChars="@();?<>&'"
                                                                                        FilterMode="InvalidChars" Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <asp:Label ID="lblParamID" runat="server" Visible="false" Text='<%#Eval("CrScoreParam_ID")%>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtDescF" MaxLength="50" Width="200px" runat="server" ToolTip="Description"></asp:TextBox>
                                                                                    <%-- code block added for bug fixing - kuppusamy.b - 14-Feb-2012 - Issue ID - 5402--%>
                                                                                    <cc1:FilteredTextBoxExtender ID="fteDescF" runat="server" TargetControlID="txtDescF"
                                                                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" InvalidChars="@();?<>&'"
                                                                                        FilterMode="InvalidChars" Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </FooterTemplate>
                                                                                <HeaderStyle Width="23%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Field Attribute">
                                                                                <ItemTemplate>
                                                                                    <asp:DropDownList ID="ddlFieldAtt" AutoPostBack="true" ToolTip="Field Attribute"
                                                                                        OnSelectedIndexChanged="ddlFieldAtt_SelectedIndexChanged" runat="server" Width="100px">
                                                                                    </asp:DropDownList>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:DropDownList ID="ddlFieldAttF" AutoPostBack="true" ToolTip="Field Attribute"
                                                                                        OnSelectedIndexChanged="ddlFieldAttF_SelectedIndexChanged" runat="server" Width="100px">
                                                                                    </asp:DropDownList>
                                                                                </FooterTemplate>
                                                                                <HeaderStyle Width="10%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Numeric Operator">
                                                                                <ItemTemplate>
                                                                                    <asp:DropDownList ID="ddlNumeric" runat="server" Width="80px" ToolTip="Numeric Operator"
                                                                                        AutoPostBack="false">
                                                                                    </asp:DropDownList>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:DropDownList ID="ddlNumericF" runat="server" Width="80px" ToolTip="Numeric Operator"
                                                                                        AutoPostBack="false">
                                                                                    </asp:DropDownList>
                                                                                </FooterTemplate>
                                                                                <HeaderStyle Width="10%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Wrap="true" HeaderText="Required Parameter">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtReqValue1" Text='<%# Bind("ReqValue")%>' MaxLength="30" Width="80px"
                                                                                        runat="server" ToolTip="Required Parameter" Style="text-align: right"></asp:TextBox>
                                                                                    <asp:DropDownList ID="ddlYes1" runat="server" Width="80px" ToolTip="Required Parameter">
                                                                                        <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
                                                                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <cc1:FilteredTextBoxExtender ID="fteAmount1" runat="server" TargetControlID="txtReqValue1"
                                                                                        FilterType="Numbers,Custom" ValidChars=".:" Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="False" Format="dd/MM/yyyy"
                                                                                        PopupButtonID="imgDate" TargetControlID="txtReqValue1">
                                                                                    </cc1:CalendarExtender>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtReqValue1F" MaxLength="30" Width="80px" runat="server" ToolTip="Required Parameter"
                                                                                        Style="text-align: right"></asp:TextBox>
                                                                                    <asp:DropDownList ID="ddlYes1F" runat="server" Width="80px" ToolTip="Required Parameter">
                                                                                        <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
                                                                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <cc1:FilteredTextBoxExtender ID="fteAmount1F" runat="server" TargetControlID="txtReqValue1F"
                                                                                        FilterType="Numbers,Custom" Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <cc1:CalendarExtender ID="CalendarExtender1F" runat="server" Enabled="False" Format="dd/MM/yyyy"
                                                                                        PopupButtonID="imgDate" TargetControlID="txtReqValue1F">
                                                                                    </cc1:CalendarExtender>
                                                                                </FooterTemplate>
                                                                                <HeaderStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Wrap="true" HeaderText="Score">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtScore1" Text='<%# Bind("Score")%>' AutoPostBack="false" OnTextChanged="Score_TextChanged"
                                                                                        onchange="SumScore();" MaxLength="15" Width="80px" runat="server" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                                                        Style="text-align: right" onblur="funChkDecimial(this,10,4,'Score',true);" ToolTip="Score"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender42" runat="server" TargetControlID="txtScore1"
                                                                                        FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtScore1F" MaxLength="15" Width="80px" AutoPostBack="false" OnTextChanged="Score_TextChanged"
                                                                                        runat="server" onkeypress="fnAllowNumbersOnly(true,false,this)" onblur="funChkDecimial(this,10,4,'Score',true);"
                                                                                        ToolTip="Score" Style="text-align: right"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender42F" runat="server" TargetControlID="txtScore1F"
                                                                                        FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Difference %">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtDiffPer" Text='<%# Bind("DiffPer")%>' MaxLength="5" Width="50px"
                                                                                        Style="text-align: right" onkeypress="fnAllowNumbersOnly(true,false,this)" runat="server"
                                                                                        ToolTip="Difference %"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender429" runat="server" TargetControlID="txtDiffPer"
                                                                                        FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtDiffPerF" MaxLength="5" Width="50px" ToolTip="Difference %" runat="server"
                                                                                        Style="text-align: right" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4112" runat="server" TargetControlID="txtDiffPerF"
                                                                                        FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Difference Mark">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtDiffMark" Text='<%# Bind("DiffMark")%>' MaxLength="6" Width="50px"
                                                                                        Style="text-align: right" runat="server" ToolTip="Difference Mark"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender41" runat="server" TargetControlID="txtDiffMark"
                                                                                        FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtDiffMarkF" MaxLength="6" Width="50px" runat="server" ToolTip="Difference Mark"
                                                                                        Style="text-align: right"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender412" runat="server" TargetControlID="txtDiffMarkF"
                                                                                        FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Action">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton Text="Delete" OnClientClick="return confirm('Do you want to delete?');"
                                                                                        runat="server" ID="lnkbtnDelete" CommandName="Delete" ToolTip="Delete"></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                <FooterTemplate>
                                                                                    <asp:Button ID="btnAddCredit" runat="server" CommandName="AddNew" CssClass="styleGridShortButton"
                                                                                        Text="Add" ToolTip="Add"></asp:Button>
                                                                                </FooterTemplate>
                                                                                <FooterStyle Width="7%" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                                        <RowStyle HorizontalAlign="Center" />
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right; padding-right: 184px">
                                                                    <asp:Label runat="server" Text="Total Score" BackColor="White" ID="lblConstitutionCode"
                                                                        Font-Bold="true" CssClass="styleGridHeader">
                                                                    </asp:Label>
                                                                    <asp:TextBox ID="txtTotalScore" Width="80px" runat="server" ToolTip="Total Score"
                                                                        Style="text-align: right"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel ID="tpNumber" runat="server" CssClass="tabpan" HeaderText="Main Page"
                                            BackColor="Red" Style="padding: 0px">
                                            <HeaderTemplate>
                                                Number
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <table width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td width="100%">
                                                                    <asp:GridView runat="server" ShowFooter="true" ID="grvNumbers" Width="100%" OnRowDataBound="grvNumbers_RowDataBound"
                                                                        OnRowCommand="grvNumbers_RowCommand" OnRowDeleting="grvNumbers_RowDeleting" RowStyle-HorizontalAlign="Center"
                                                                        FooterStyle-HorizontalAlign="Center" AutoGenerateColumns="False">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Line No">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblLineNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="30px" HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Line Type">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblLineType" runat="server" Text='<%#Eval("LineType")%>'></asp:Label>
                                                                                    <asp:Label ID="lblLineTypeID" runat="server" Text='<%#Eval("LineTypeID")%>' Visible="false"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                <FooterTemplate>
                                                                                    <asp:DropDownList ID="ddlFLineType" ToolTip="Line Type" runat="server" AutoPostBack="true"
                                                                                        OnSelectedIndexChanged="ddlFLineType_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvFddlFLineType" InitialValue="0" ValidationGroup="AddNumber"
                                                                                        runat="server" ControlToValidate="ddlFLineType" Display="None" SetFocusOnError="true"
                                                                                        ErrorMessage="Select the Line Type"></asp:RequiredFieldValidator>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Flag">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblFlag" runat="server" Text='<%#Eval("Flag")%>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                <FooterTemplate>
                                                                                    <asp:DropDownList ID="ddlFFlag" ToolTip="Line Type" runat="server" AutoPostBack="true"
                                                                                        OnSelectedIndexChanged="ddlFFlag_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvFddlFFlag" InitialValue="0" ValidationGroup="AddNumber"
                                                                                        runat="server" ControlToValidate="ddlFFlag" Display="None" SetFocusOnError="true"
                                                                                        ErrorMessage="Select the Flag"></asp:RequiredFieldValidator>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Description">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Desc")%>'></asp:Label>
                                                                                    <asp:Label ID="lblDescID" runat="server" Text='<%#Eval("Flag_ID")%>' Visible="false"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblFDesc" runat="server" Visible="true"></asp:Label>
                                                                                    <cc1:ComboBox ID="txtFDesc" runat="server" ToolTip="Description" Visible="false"
                                                                                        AutoCompleteMode="Suggest">
                                                                                    </cc1:ComboBox>
                                                                                    <asp:RequiredFieldValidator ID="rfvFtxtFDesc" ValidationGroup="AddNumber" runat="server"
                                                                                        ControlToValidate="txtFDesc" Display="None" SetFocusOnError="true" ErrorMessage="Enter the Description"></asp:RequiredFieldValidator>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Wrap="true" HeaderText="Value">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblValue" runat="server" Text='<%#Eval("Value")%>' Visible="false"
                                                                                        Style="text-align: right" Width="100%"></asp:Label>
                                                                                    <asp:TextBox ID="txtValue" AutoPostBack="true" runat="server" OnTextChanged="txtValue_TextChanged"
                                                                                        Width="120px" Style="text-align: right" Text='<%#Eval("Value")%>'></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Right" Width="100px" />
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtFValue" MaxLength="15" Width="100px" runat="server" ToolTip="Value"
                                                                                        Style="text-align: right"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="fetxtFValue" runat="server" TargetControlID="txtFValue"
                                                                                        FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <asp:RequiredFieldValidator ID="rfvFtxtFValue" ValidationGroup="AddNumber" runat="server"
                                                                                        ControlToValidate="txtFValue" Display="None" SetFocusOnError="true" ErrorMessage="Enter the Value"></asp:RequiredFieldValidator>
                                                                                </FooterTemplate>
                                                                                <FooterStyle Width="100px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Wrap="true" HeaderText="Formula">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkCarryFml" runat="server" Checked="true" ToolTip="Carry formula result" />
                                                                                    <asp:Label ID="lblCarryFml" Visible="false" runat="server" Text='<%#Eval("CarryFml")%>'></asp:Label>
                                                                                    <asp:Label ID="lblFormula" runat="server" Text='<%#Eval("Formula")%>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                <FooterTemplate>
                                                                                    <asp:Panel runat="server" ID="pnlFFml" Enabled="false">
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td rowspan="2">
                                                                                                    <asp:TextBox ID="txtFFormula" TextMode="MultiLine" Width="100px" runat="server" ToolTip="Value"
                                                                                                        Style="text-align: left; height: 40px; width: 140px;"></asp:TextBox>
                                                                                                    <cc1:FilteredTextBoxExtender ID="fetxtFFormula" runat="server" TargetControlID="txtFFormula"
                                                                                                        FilterType="Numbers,Custom,UppercaseLetters" ValidChars=".,+,-,*,/,%,(,),<,>,?"
                                                                                                        Enabled="True">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                    <asp:RequiredFieldValidator ID="rfvFtxtFFormula" ValidationGroup="AddNumber" runat="server"
                                                                                                        Enabled="false" ControlToValidate="txtFFormula" Display="None" SetFocusOnError="true"
                                                                                                        ErrorMessage="Enter the Formula"></asp:RequiredFieldValidator>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="ddlFFmlLines" runat="server">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <button id="btnAddfml" type="button" runat="server" class="styleGridShortButton"
                                                                                                        value="Add" title="Add">
                                                                                                        <img src="../Images/Prev_Desabled.gif" style="position: absolute" runat="server"
                                                                                                            id="imgPrev1" />
                                                                                                        &nbsp;
                                                                                                        <img src="../Images/Prev_Desabled.gif" runat="server" id="imgPrev2" />
                                                                                                    </button>
                                                                                                    <asp:CheckBox ID="chkFCarryFml" runat="server" Checked="true" ToolTip="Carry formula result" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </asp:Panel>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Wrap="true" HeaderText="Link">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkLink" runat="server" />
                                                                                    <asp:Label ID="lblLink" runat="server" Text='<%#Eval("IsLink")%>' Visible="false"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:CheckBox ID="chkFLink" runat="server" OnCheckedChanged="chkFLink_CheckedChanged"
                                                                                        AutoPostBack="true" />
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Param Num">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblParamText" runat="server" Text='<%#Eval("ParamText")%>'></asp:Label>
                                                                                    <asp:Label ID="lblParamNum" runat="server" Text='<%#Eval("ParamNum")%>' Visible="false"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                <FooterTemplate>
                                                                                    <asp:DropDownList ID="ddlFParamNum" ToolTip="Param Num" runat="server" Width="150px">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvFddlFParamNum" InitialValue="0" ValidationGroup="AddNumber"
                                                                                        runat="server" ControlToValidate="ddlFParamNum" Display="None" SetFocusOnError="true"
                                                                                        ErrorMessage="Select the Param Num"></asp:RequiredFieldValidator>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Action">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton Text="Delete" OnClientClick="return confirm('Do you want to delete?');"
                                                                                        runat="server" ID="lnkbtnDelete" CommandName="Delete" ToolTip="Delete"></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                <FooterTemplate>
                                                                                    <asp:Button ID="btnAddCredit" runat="server" CommandName="AddNew" CssClass="styleGridShortButton"
                                                                                        Text="Add" ToolTip="Add" ValidationGroup="AddNumber"></asp:Button>
                                                                                </FooterTemplate>
                                                                                <FooterStyle Width="7%" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <%-- <cc1:TabPanel ID="TabPanel2" runat="server" CssClass="tabpan" HeaderText="Main Page"
                                BackColor="Red" Style="padding: 0px">
                                <HeaderTemplate>
                                    Text
                                </HeaderTemplate>
                            </cc1:TabPanel>--%>
                                    </cc1:TabContainer>
                                    <table align="right" cellpadding="0" cellspacing="0" runat="server" id="tblYearBtns">
                                        <tr>
                                            <td align="right">
                                                <asp:Button ID="btnYear1" CssClass="styleSubmitButton" runat="server" Text="2010-2011"
                                                    ToolTip="Add" Style="background-image: url(../Images/btn_Silver.jpg); border-width: 0px; width: 78px; height: 22px;"
                                                    Enabled="true" OnClick="btnYear_Click" Visible="false"></asp:Button>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnYear2" CssClass="styleSubmitButton" runat="server" Text="2011-2012"
                                                    ToolTip="Add" Style="background-image: url(../Images/btn_Silver.jpg); border-width: 0px; width: 78px; height: 22px;"
                                                    Visible="false" OnClick="btnYear_Click"></asp:Button>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnYear3" CssClass="styleSubmitButton" runat="server" Text="2012-2013"
                                                    ToolTip="Add" Style="background-image: url(../Images/btn_Silver.jpg); border-width: 0px; width: 78px; height: 22px"
                                                    Visible="false" OnClick="btnYear_Click"></asp:Button>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnYear4" CssClass="styleSubmitButton" runat="server" Text="2012-2013"
                                                    ToolTip="Add" Style="background-image: url(../Images/btn_Silver.jpg); border-width: 0px; width: 78px; height: 22px"
                                                    Visible="false" OnClick="btnYear_Click"></asp:Button>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnYear5" CssClass="styleSubmitButton" runat="server" Text="2012-2013"
                                                    ToolTip="Add" Style="background-image: url(../Images/btn_Silver.jpg); border-width: 0px; width: 78px; height: 22px"
                                                    Visible="false" OnClick="btnYear_Click"></asp:Button>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnYear6" CssClass="styleSubmitButton" runat="server" Text="2012-2013"
                                                    ToolTip="Add" Style="background-image: url(../Images/btn_Silver.jpg); border-width: 0px; width: 78px; height: 22px"
                                                    Visible="false" OnClick="btnYear_Click"></asp:Button>
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
                        <br />
                        <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators();"
                            CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click" ToolTip="Save"
                            Enabled="false" />
                        <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click"
                            ToolTip="Clear" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                            CssClass="styleSubmitButton" OnClick="btnCancel_Click" ToolTip="Cancel" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="ValidationSummary1" HeaderText="Correct the following validation(s):"
                            CssClass="styleMandatoryLabel" ShowMessageBox="false" ShowSummary="true" ValidationGroup="AddNumber" />
                    </td>
                </tr>
                <tr style="padding-top: 10px; padding-bottom: 10px">
                    <td>
                        <asp:GridView runat="server" ID="grvCreditScoreYearValues" Width="100%" OnRowDataBound="grvCreditScoreYearValues_RowDataBound"
                            RowStyle-HorizontalAlign="Right" CellPadding="5" HeaderStyle-Wrap="true" HeaderStyle-HorizontalAlign="Center"
                            CellSpacing="10" AutoGenerateColumns="False">
                            <Columns>
                                <asp:TemplateField HeaderText="Description" HeaderStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblDescription"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-HorizontalAlign="Left" DataField="Field_Attribute" HeaderText="Field Attribute" />
                                <asp:BoundField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" DataField="Numeric_Operator"
                                    HeaderText="Numeric Operator" />
                                <asp:TemplateField HeaderText="Required Value (Year1)" HeaderStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblReqParam1" Text='<%# Bind("ReqParam1") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="Score1" HeaderStyle-Width="60px" HeaderText="Score (Year1)" />--%>
                                <asp:TemplateField HeaderText="Score (Year1)" HeaderStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblScore1" Text='<%# Bind("Score1") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Required Value (Year2)" HeaderStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblReqParam2" Text='<%# Bind("ReqParam2") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Score (Year2)" HeaderStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblScore2" Text='<%# Bind("Score2") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Required Value (Year3)" HeaderStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblReqParam3" Text='<%# Bind("ReqParam3") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Score (Year3)" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblScore3" Text='<%# Bind("Score3") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Required Value (Year4)" HeaderStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblReqParam4" Text='<%# Bind("ReqParam4") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Score (Year4)" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblScore4" Text='<%# Bind("Score4") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Required Value (Year5)" HeaderStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblReqParam5" Text='<%# Bind("ReqParam5") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Score (Year5)" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblScore5" Text='<%# Bind("Score5") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Required Value (Year6)" HeaderStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblReqParam6" Text='<%# Bind("ReqParam6") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Score (Year6)" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblScore6" Text='<%# Bind("Score6") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Difference_Percentage" HeaderText="Difference %" />
                                <asp:BoundField DataField="Difference_Mark" HeaderText="Difference Mark" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:ValidationSummary runat="server" ID="vsUserMgmt" HeaderText="Correct the following validation(s):"
                            CssClass="styleMandatoryLabel" ShowMessageBox="false" ShowSummary="true" />
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="hidden" value="0" runat="server" id="hdnCreditScoreID" />
                        <input type="hidden" value="0" runat="server" id="hdnDelete" />
                        <input type="hidden" value="0" runat="server" id="hdnFooterAdd" />
                        <input type="hidden" value="0" runat="server" id="hdnCreditScoreUpdatedID" />
                        <input type="hidden" value="0" runat="server" id="hdnCanEdit" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">

        function fnChangeAttribute(filedID, numField, ddlYes1, reqFiled1) {
            var intTotYear = document.getElementById('<%=txtNoofYears.ClientID%>').value;

            if (document.getElementById(filedID).options.value == '4') {
                document.getElementById(numField).disabled = true;
                document.getElementById(reqFiled1).style.display = 'none';
                document.getElementById(ddlYes1).style.display = 'Block';
            }
            else {
                document.getElementById(numField).disabled = false;
                document.getElementById(reqFiled1).style.display = 'Block';
                document.getElementById(ddlYes1).style.display = 'none';
            }
        }


        function SumScore() {
            //debugger;
            var TargetBaseControl = document.getElementById('ctl00_ContentPlaceHolder1_tcCreditScore_tpMainPage_grvCreditScore');
            //if(TargetBaseControl==null)
            //TargetBaseControl=document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParamUpdate');
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            var TotalScore = 0;
            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'text') {
                    if (Inputs[n].value != '') {
                        if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 6) == 'Score1') {
                            if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 13) != 'txtTotalScore') {
                                TotalScore = (parseFloat(TotalScore) + parseFloat(Inputs[n].value)).toFixed(4);
                                /*if(parseFloat(TotalScore)>9999)
                                {
                                document.getElementById(Inputs[n].id).value="";
                                document.getElementById(Inputs[n].id).focus();
                                alert('Score should not be greater than 9999');
                                return false;
                                }*/
                            }
                        }

                    }

                    /*if(Inputs[n].id.substring(Inputs[n].id.length,Inputs[n].id.length-13)=='txtTotalScore')
                    {
                    document.getElementById(Inputs[n].id).value="";
                    if(TotalScore==0)
                    document.getElementById(Inputs[n].id).value="";
                    else
                    document.getElementById(Inputs[n].id).value=TotalScore;
                    }
                    */
                }

            }
            if (parseFloat(TotalScore) >= 0)
                document.getElementById('<%=txtTotalScore.ClientID%>').value = parseFloat(TotalScore).toFixed(4);
        }


        function FunDiffMarkEnabled(inputP, inputM) {
            //debugger
            var Percentage = document.getElementById(inputP).value;
            var Mark = document.getElementById(inputM).value;
            if (Percentage == '') {
                document.getElementById(inputM).setAttribute("readOnly", "readOnly");
                document.getElementById(inputM).value = '';
            }
            else {
                document.getElementById(inputM).removeAttribute("readOnly");
            }
        }

        function FunReqScoreEnabled(inputR, inputS) {
            // debugger        
            var ddlReq = document.getElementById(inputR).selectedIndex;
            var Score = document.getElementById(inputS).value;
            if (ddlReq == 0) {
                document.getElementById(inputS).setAttribute("readOnly", "readOnly");
                document.getElementById(inputS).value = '';
                SumScore();
            }
            else {
                document.getElementById(inputS).removeAttribute("readOnly");
            }
        }

        function FunRowControlsEnabled(inputReq, inputScr, inputDiffP, inputDiffM, IsFooter, ddlYear, inputFA, inputNO) {
            //debugger        
            var Req = document.getElementById(inputReq).value;
            var Score = document.getElementById(inputScr).value;
            var DiffPer = document.getElementById(inputDiffP).value;
            var DiffM = document.getElementById(inputDiffM).value;
            var ddlFieldAttri = document.getElementById(inputFA).selectedIndex;
            var ddlNumOper = document.getElementById(inputNO).selectedIndex;
            if (Req == '') {
                document.getElementById(inputScr).setAttribute("readOnly", "readOnly");
                document.getElementById(inputDiffP).setAttribute("readOnly", "readOnly");
                document.getElementById(inputDiffM).setAttribute("readOnly", "readOnly");

                document.getElementById(inputScr).value = '';

                var year = document.getElementById(ddlYear).selectedIndex;

                if (year == 0) {
                    document.getElementById(inputDiffP).value = '';
                    document.getElementById(inputDiffM).value = '';
                }
                if (IsFooter == '0') {
                    SumScore();
                }
            }
            if (ddlFieldAttri == 1 && ddlNumOper == 5) {
                document.getElementById(inputDiffP).value = '';
                document.getElementById(inputDiffM).value = '';
                //            document.getElementById(inputDiffP).setAttribute("readOnly","readOnly");
                //            document.getElementById(inputDiffM).setAttribute("readOnly","readOnly");  
                document.getElementById(inputDiffP).setAttribute("ContentEditable", "false");
                document.getElementById(inputDiffM).setAttribute("ContentEditable", "false");
            }
            else {
                //            document.getElementById(inputDiffP).removeAttribute("readOnly");
                //            document.getElementById(inputDiffM).removeAttribute("readOnly");

                document.getElementById(inputDiffP).setAttribute("ContentEditable", "true");
                document.getElementById(inputDiffM).setAttribute("ContentEditable", "true");
            }
        }


        function FunDiFFPerandMarkEnabled(inputFA, inputNO, inputDiffP, inputDiffM) {
            //      //debugger;
            //        var ddlFieldAttri = document.getElementById(inputFA).selectedIndex;
            //        var ddlNumOper = document.getElementById(inputNO).selectedIndex;
            //        var DiffPer = document.getElementById(inputDiffP).value;
            //        var DiffM = document.getElementById(inputDiffM).value;
            //        
            //        if(ddlFieldAttri == 1 && ddlNumOper == 5)
            //        {
            //            document.getElementById(inputDiffP).value = '';
            //            document.getElementById(inputDiffM).value = '';
            //            
            //            document.getElementById(inputDiffP).setAttribute("ContentEditable", "false");
            //            document.getElementById(inputDiffM).setAttribute("ContentEditable", "false");      
            //           
            //        }
            //        else
            //        {
            //             document.getElementById(inputDiffP).setAttribute("ContentEditable", "true");
            //             document.getElementById(inputDiffM).setAttribute("ContentEditable", "true");  
            //        }
        }

        function fnAppendFormula(txtBx, ddlItem) {
            var itemTxt = document.getElementById(ddlItem).options[document.getElementById(ddlItem).selectedIndex].text;
            if (itemTxt != '--Select--') {
                document.getElementById(txtBx).value = document.getElementById(txtBx).value + '(' + itemTxt + ')';
            }
        }

        function fnChkFreeze(ctrl) {
            document.getElementById(ctrl).checked = !document.getElementById(ctrl).checked;
        }


        function fnSetNextYear() {
            var txtPrvYear = document.getElementById('<%=txtPrvYears.ClientID%>');
            var txtNxtYear = document.getElementById('<%=txtNxtYears.ClientID%>');
            var txtNoofYear = document.getElementById('<%=txtNoofYears.ClientID%>');
            if (parseInt(txtPrvYear.value) >= 0 && parseInt(txtNoofYear.value) > 0) {
                txtNxtYear.value = parseInt(txtNoofYear.value) - parseInt(txtPrvYear.value) - 1;
                if (parseInt(txtNxtYear.value) < 0) {
                    txtNxtYear.value = '0';
                }
            }
            else {
                txtNxtYear.value = '';
            }

        }
    </script>

</asp:Content>
