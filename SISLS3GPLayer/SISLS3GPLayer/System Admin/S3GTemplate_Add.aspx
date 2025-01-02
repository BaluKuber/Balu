<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    CodeFile="S3GTemplate_Add.aspx.cs" Inherits="System_Admin_S3GTemplate" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--Header--%>
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <asp:Label runat="server" Text="Template" ID="lblHeading" CssClass="styleDisplayLabel">
                </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <cc1:TabContainer ID="TCTemplate" runat="server" CssClass="styleTabPanel" Width="100%"
                    ScrollBars="None">
                    <cc1:TabPanel runat="server" ID="TPTemplateDtls" CssClass="tabpan" BackColor="Red">
                        <HeaderTemplate>
                            Template Details
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top">
                                        <table width="100%" align="center" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Panel runat="server" ID="pnlTemplateHeader" CssClass="stylePanel" GroupingText="Template Details"
                                                        Width="99%">
                                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" Text="Line of Business" ID="lblLOB"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RFVddlLOB" runat="server" SetFocusOnError="True"
                                                                        ControlToValidate="ddlLOB" InitialValue="0" Display="None" ErrorMessage="Select Line of Business"
                                                                        ValidationGroup="btnSave" Enabled="false"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" Text="Template Ref No" ID="lblTemplateRefNo"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtTemplateRefNo" runat="server" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" Text="Location" ID="lblLocation"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <uc2:Suggest ID="ddlLocation" runat="server" ServiceMethod="GetBranchList" ErrorMessage="Select a Location"
                                                                        ValidationGroup="btnSave" IsMandatory="false" />
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" Text="Template Name" ID="lblTemplateType"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:DropDownList ID="ddlTemplateType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTemplateType_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RFVddlTemplateType" runat="server" SetFocusOnError="True"
                                                                        ControlToValidate="ddlTemplateType" InitialValue="0" Display="None" ErrorMessage="Select Template Name"
                                                                        ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" Text="Template Type" ID="lblModeofMail"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:DropDownList ID="ddlModeofMail" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlModeofMail_SelectedIndexChanged" Width="105px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RFVddlModeofMail" runat="server" SetFocusOnError="True"
                                                                        ControlToValidate="ddlModeofMail" InitialValue="0" Display="None" ErrorMessage="Select Template Type"
                                                                        ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" Text="Document Path" ID="lblDocumentPath"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtDocumentPath" runat="server" ReadOnly="True"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RFVtxtDocumentPath" runat="server" SetFocusOnError="True"
                                                                        ControlToValidate="txtDocumentPath" Display="None" ErrorMessage="Enter Document Path"
                                                                        ValidationGroup="btnSave" Enabled="False"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" Text="Template Description" ID="Label2"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                                                                     <asp:RequiredFieldValidator ID="rfvDescription" runat="server" SetFocusOnError="True"
                                                                        ControlToValidate="txtDescription" Display="None" ErrorMessage="Enter Template Description"
                                                                        ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" Text="To Language" ID="lblLanguage"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:DropDownList ID="ddlLanguage" runat="server" AutoPostBack="false" Width="105px">
                                                                        <asp:ListItem Value="en">English</asp:ListItem>
                                                                        <asp:ListItem Value="ar">&#1575;&#1604;&#1593;&#1585;&#1576;&#1610;&#1577;</asp:ListItem>
                                                                        <asp:ListItem Value="ta">&#2980;&#2990;&#3007;&#2996;&#3021;</asp:ListItem>
                                                                         <asp:ListItem Value="te">&#3108;&#3142;&#3122;&#3137;&#3095;&#3137;</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                      <asp:LinkButton ID="lnkViewTemplate" OnClick="lnkViewTemplate_Click" runat="server"
                                                                            Text="View">
                                                                        </asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" Text="Active" ID="lblActvie"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign" align="left">
                                                                    <asp:CheckBox ID="chkIsActive" runat="server" Enabled="False" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:HiddenField runat="server" ID="hdnWrap" Value="1" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <FTB:FreeTextBox ID="FTBTemplate" EnableHtmlMode="False" runat="server" Width="99%"
                                            SupportFolder="~/aspnet_client/FreeTextBox/" JavaScriptLocation="ExternalFile"
                                            ImageGalleryPath="~/aspnet_client/FreeTextBox/images" ToolbarImagesLocation="ExternalFile" 
                                            ButtonImagesLocation="ExternalFile" ToolbarLayout="ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu,Cut,Copy,Paste,Undo,Redo,Print;Bold,Italic,Underline,fontforecolorpicker,fontbackcolorpicker,Strikethrough;Superscript,Subscript,RemoveFormat|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,insertdate,inserttime,WordClean,preview,InsertRule,InsertTable,InsertImage" AllowHtmlMode="True" AssemblyResourceHandlerPath="" AutoConfigure="" AutoGenerateToolbarsFromString="True" AutoHideToolbar="True" AutoParseStyles="True" BackColor="158, 190, 245" BaseUrl="" BreakMode="Paragraph" ButtonDownImage="False" ButtonFileExtention="gif" ButtonFolder="Images" ButtonHeight="20" ButtonOverImage="False" ButtonPath="" ButtonSet="Office2003" ButtonWidth="21" ClientSideTextChanged="" ConvertHtmlSymbolsToHtmlCodes="False" DesignModeBodyTagCssClass="" DesignModeCss="" DisableIEBackButton="False" DownLevelCols="50" DownLevelMessage="" DownLevelMode="TextArea" DownLevelRows="10" EditorBorderColorDark="Gray" EditorBorderColorLight="Gray" EnableSsl="False" EnableToolbars="True" Focus="False" FormatHtmlTagsToXhtml="True" GutterBackColor="129, 169, 226" GutterBorderColorDark="Gray" GutterBorderColorLight="White" Height="350px" HelperFilesParameters="" HelperFilesPath="" HtmlModeCss="" HtmlModeDefaultsToMonoSpaceFont="True" ImageGalleryUrl="ftb.imagegallery.aspx?rif={0}&amp;cif={0}" InstallationErrorMessage="InlineMessage" Language="en-US" PasteMode="Default" ReadOnly="False" RemoveScriptNameFromBookmarks="True" RemoveServerNameFromUrls="True" RenderMode="NotSet" ScriptMode="External" ShowTagPath="False" SslUrl="/." StartMode="DesignMode" StripAllScripting="False" TabIndex="-1" TabMode="InsertSpaces" Text="" TextDirection="LeftToRight" ToolbarBackColor="Transparent" ToolbarBackgroundImage="True" ToolbarStyleConfiguration="Office2003" UpdateToolbar="True" UseToolbarBackGroundImage="True" />
                                        <asp:RequiredFieldValidator ID="RFVFTBTemplate" runat="server" SetFocusOnError="True"
                                            ControlToValidate="FTBTemplate" Display="None" ErrorMessage="Enter Letter Format"
                                            ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr style="display:none">
                                    <td>
                                        <asp:FileUpload ID="FUpload" runat="server"  ToolTip="Upload Image"/>
                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" onClick="btnUpload_Click" CssClass="styleGridShortButton"/>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TPExclusion" CssClass="tabpan" BackColor="Red">
                        <HeaderTemplate>
                            Exclusion
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <table width="100%" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>&nbsp;
                                                <asp:Button ID="btnLoadCustomer" runat="server" Style="display: none" Text="Load Customer"
                                                    OnClick="btnLoadCustomer_OnClick" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView FooterStyle-HorizontalAlign="Center" ID="GrvExclusion" runat="server"
                                                    OnRowDeleting="GrvExclusion_RowDeleting" Width="99%" ShowFooter="true" OnRowCommand="GrvExclusion_RowCommand"
                                                    AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="CategoryId" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCategoryId" ToolTip="Id" runat="server" Text='<%#Eval("CategoryId")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Category">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCategory" ToolTip="Category" runat="server" Text='<%#Eval("Category")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <%--OnSelectedIndexChanged="ddlFooterCategory_SelectedIndexChanged"--%>
                                                                <asp:DropDownList ID="ddlFooterCategory" ToolTip="Category" onchange="fnLoadCustomerg()"
                                                                    runat="server">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator Display="None" ID="RFVddlFooterCategory" CssClass="styleMandatoryLabel"
                                                                    SetFocusOnError="True" ValidationGroup="lnkAdd" runat="server" ControlToValidate="ddlFooterCategory"
                                                                    InitialValue="0" ErrorMessage="Select Category"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Id" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblId" ToolTip="Id" runat="server" Text='<%#Eval("Id")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblFooterId" ToolTip="Id" runat="server" Text='<%#Eval("Id")%>'></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCode" ToolTip="Code" runat="server" Text='<%#Eval("Code")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtCode" runat="server" Visible="false"></asp:TextBox>
                                                                <uc2:LOV ID="ucCustomerLov" runat="server" onblur="fnLoadCustomerg()" />
                                                                <asp:RequiredFieldValidator Display="None" ID="RFVtxtCode" CssClass="styleMandatoryLabel"
                                                                    SetFocusOnError="True" ValidationGroup="lnkAdd" runat="server" ControlToValidate="txtCode"
                                                                    InitialValue="0" ErrorMessage="Select Code"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDescription" ToolTip="Description" runat="server" Text='<%#Eval("Description")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtDescription" runat="server" ReadOnly="true"></asp:TextBox>
                                                                <asp:RequiredFieldValidator Display="None" ID="RFVtxtDescription" CssClass="styleMandatoryLabel"
                                                                    SetFocusOnError="True" ValidationGroup="lnkAdd" runat="server" ControlToValidate="txtDescription"
                                                                    InitialValue="0" ErrorMessage="Select Description"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="10%" FooterStyle-Width="10%"
                                                            ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lnkRemove" ToolTip="Remove from the grid" runat="server" CausesValidation="false"
                                                                    CommandName="Delete" Text="Remove" CssClass="styleGridShortButton"></asp:Button>
                                                                <%--  <asp:ImageButton ImageUrl="~/Images/btn_delete.gif" runat="server"  CausesValidation="false"
                                                                    CommandName="Delete" />--%>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Button ID="lnkAdd" ValidationGroup="lnkAdd" ToolTip="Add to the grid" runat="server"
                                                                    Text="Add" CommandName="Add" CssClass="styleGridShortButton"></asp:Button>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
        <tr align="center" class="styleButtonArea" style="padding-left: 4px">
            <td colspan="3" align="center">
                <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                    ValidationGroup="btnSave" OnClick="btnSave_Click" OnClientClick="return fnCheckPageValidators('btnSave',false)" />
                &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                    Text="Clear" OnClientClick="return confirm('Do you sure want to Clear?');" OnClick="btnClear_Click" />
                &nbsp;<asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                    CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                <%--&nbsp;<asp:Button runat="server" ID="btnGenerate" Text="Generate Letter" CausesValidation="false"
                    CssClass="styleSubmitButton" OnClick="btnGenerate_Click" />--%>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CustomValidator ID="CVTemplate" runat="server" Display="None" ValidationGroup="btnSave"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary runat="server" ID="VSDelinquentParameter" HeaderText="Correct the following validation(s):"
                    CssClass="styleSummaryStyle" ShowMessageBox="false" ValidationGroup="btnSave"
                    ShowSummary="true" />
            </td>

        </tr>
        <tr>
            <td>
                <asp:ValidationSummary runat="server" ID="vsadd" HeaderText="Correct the following validation(s):"
                    CssClass="styleSummaryStyle" ShowMessageBox="false" ValidationGroup="lnkAdd"
                    ShowSummary="true" />
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">

        function pageLoad() {
            FTBTextDesignWrap();
        }

        function FTBDesignWrap() {

            FTBTextDesignWrap();
            var hdnval = document.getElementById("ctl00_ContentPlaceHolder1_TCTemplate_TPTemplateDtls_hdnWrap");
            if (hdnval.value == 0)
                hdnval.value = 1;
            else
                hdnval.value = 0;

        }

        function FTBTextDesignWrap() {
            var hdnval = document.getElementById("ctl00_ContentPlaceHolder1_TCTemplate_TPTemplateDtls_hdnWrap");
            var SetWrapText = hdnval.value;
            var freetextboxDesignFrame = document.getElementById("ctl00_ContentPlaceHolder1_TCTemplate_TPTemplateDtls_FTBTemplate_designEditor");
            var iframe = freetextboxDesignFrame.contentDocument;
            if (iframe == undefined)
                iframe = freetextboxDesignFrame.contentWindow.document;
            iframe.body.style.wordWrap = SetWrapText == 1 ? "break-word" : "normal";


        }


        function fnLoadCustomerg() {
            document.getElementById('ctl00_ContentPlaceHolder1_TCTemplate_TPExclusion_btnLoadCustomer').click();
        }

    </script>

</asp:Content>
