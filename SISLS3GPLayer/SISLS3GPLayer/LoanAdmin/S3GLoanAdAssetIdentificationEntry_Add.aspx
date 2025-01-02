<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLOANADAssetIdentificationEntry_Add.aspx.cs" Inherits="LoanAdmin_S3GLOANADAssetIdentificationEntry_Add" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td valign="top" colspan="4">
                        <table width="100%" class="stylePageHeading" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td style="height: 19px">
                                    <asp:Label runat="server" ID="lblHeading" Text="Asset Identification" CssClass="styleDisplayLabel"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel" width="17%">
                        <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign" width="23%">
                        <asp:DropDownList ID="ddlLOB" runat="server" ToolTip="Line of Business" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="styleFieldLabel" width="15%">
                        <asp:Label runat="server" ID="lblBranch" Text="Location" CssClass="styleReqFieldLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign" width="24%">
                        <%-- <asp:DropDownList ID="ddlBranch" AutoPostBack="true" ToolTip="Location" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                            runat="server">
                        </asp:DropDownList>--%>
                        <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                            ErrorMessage="Select a Location" IsMandatory="true" OnItem_Selected="ddlBranch_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblPAN" runat="server" Text="Prime Account Number" CssClass="styleReqFieldLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <uc2:Suggest ID="ddlMLA" runat="server" ServiceMethod="GetPANUM" AutoPostBack="true"
                            OnItem_Selected="ddlMLA_SelectedIndexChanged" IsMandatory="true" ErrorMessage="Enter a Prime Account" />
                        <%-- <asp:DropDownList ID="ddlMLA" runat="server" Width="95%" ToolTip="Prime Account Number"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlMLA_SelectedIndexChanged">
                        </asp:DropDownList>--%>
                    </td>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblSLA" runat="server" Text="Sub Account Number" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:DropDownList ID="ddlSLA" runat="server" ToolTip="Sub Account Number" Width="95%"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlSLA_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblInvoice" runat="server" Text="Vendor Invoice No." CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:DropDownList ID="ddlInvoice" runat="server" ToolTip="Vendor Invoice No." AutoPostBack="true"
                            OnSelectedIndexChanged="ddlInvoice_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblInvoiceDate" runat="server" Text="Vendor Invoice Date" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="txtInvoiceDate" runat="server" ContentEditable="False" Width="40%"
                            ToolTip="Vendor Invoice Date"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td width="50%">
                        <asp:Panel ID="Panel3" runat="server" GroupingText="Customer Information" CssClass="stylePanel">
                            <uc1:S3GCustomerAddress ID="S3GCustomerAddress" runat="server" FirstColumnStyle="styleFieldLabel"
                                SecondColumnStyle="styleFieldAlign" />
                        </asp:Panel>
                    </td>
                    <td width="50%">
                        <asp:Panel ID="Panel4" runat="server" GroupingText="Vendor Information" CssClass="stylePanel">
                            <uc1:S3GCustomerAddress ID="S3GVendorAddress" runat="server" Caption="Vendor" FirstColumnStyle="styleFieldLabel"
                                SecondColumnStyle="styleFieldAlign" />
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel2" runat="server" GroupingText="Asset Details" CssClass="stylePanel">
                <table width="100%">
                    <tr>
                        <td class="styleFieldLabel" width="17%">
                            <asp:Label ID="lblAIENo" runat="server" Text="Asset Identity Number" ContentEditable="False"
                                CssClass="styleDisplayLabel"></asp:Label>
                        </td>
                        <td class="styleFieldAlign" width="32%">
                            <asp:TextBox ID="txtAIE" ContentEditable="False" runat="server" ToolTip="Asset Identity Number"></asp:TextBox>
                        </td>
                        <td class="styleFieldLabel" width="18%">
                            <asp:Label ID="lblAIEDate" runat="server" Text="Asset Identification Date" ContentEditable="False"
                                CssClass="styleReqFieldLabel"></asp:Label>
                        </td>
                        <td class="styleFieldAlign" width="31%">
                            <asp:TextBox ID="txtAIEDate" runat="server" ContentEditable="False" Width="40%" ToolTip="Asset Identification Date">  </asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            <table width="100%">
                                <tr visible="false" class="styleRecordCount" runat="server" id="trAssetMessage">
                                    <td align="center">
                                        <asp:Label runat="server" ID="Label4" Text="Asset Details already updated" Font-Size="Medium"
                                            class="styleMandatoryLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="divAstGrid" runat="server" style="overflow-x: scroll; display: none" class="styleContentTable">
                                            <asp:GridView ID="grvAsset" runat="server" HeaderStyle-CssClass="styleGridHeader"
                                                RowStyle-HorizontalAlign="Center" AutoGenerateColumns="false" OnRowDataBound="grvAsset_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetID" Visible="false" runat="server" Text='<%#Eval("Asset_Number")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit SI.Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSINO" MaxLength="20" Text='<%#Eval("Row")%>' runat="server" ToolTip="Unit SI.Number"></asp:Label>
                                                            <asp:Label ID="lblAssetType" Text='<%#Eval("AssetType_ID")%>' runat="server" Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Asset Code" DataField="Asset_Code" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField HeaderText="Asset Description" DataField="ASSET_DESCription" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:TemplateField HeaderText="Registration Number">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRegNo" MaxLength="20" Text='<%#Eval("REGN_NUMBER")%>' Width="100px"
                                                                runat="server" ToolTip="Registration Number"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtRegNo" runat="server" FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers"
                                                                TargetControlID="txtRegNo" ValidChars="">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Engine Number">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtEngineNo" MaxLength="20" Text='<%#Eval("ENGINE_NUMBER")%>' Width="100px"
                                                                runat="server" ToolTip="Engine Number"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtEngine" runat="server" FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers"
                                                                TargetControlID="txtEngineNo" ValidChars="">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Chassis Number">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtChassisNo" MaxLength="20" Text='<%#Eval("CHASIS_NUMBER")%>' Width="100px"
                                                                runat="server" ToolTip="Chassis Number"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtChassis" runat="server" FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers"
                                                                TargetControlID="txtChassisNo" ValidChars="">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Serial Number">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSerialNo" MaxLength="20" Width="100px" Text='<%#Eval("SERIAL_NUMBER")%>'
                                                                runat="server" ToolTip="Serial Number"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtSerialNo" runat="server" FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers"
                                                                TargetControlID="txtSerialNo" ValidChars="">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Installation Ref Number">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtInstallationNo" MaxLength="20" Width="100px" Text='<%#Eval("INSTALLATION_REF_NO")%>'
                                                                runat="server" ToolTip="Installation Ref Number"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtInstRefNo" runat="server" FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers"
                                                                TargetControlID="txtInstallationNo" ValidChars="">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Installation Date">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtInstallationDate" MaxLength="20" Width="80px" runat="server"
                                                                ToolTip="Installation Date" Text='<%#Eval("INSTALLATION_DATE")%>'></asp:TextBox>
                                                            <cc1:CalendarExtender runat="server" TargetControlID="txtInstallationDate" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                                ID="CEInsDate" Enabled="True">
                                                            </cc1:CalendarExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--changed By Thangam M on 18/Nov/2011 to fix UAT bug--%>
                                                    <asp:TemplateField HeaderText="Upload Document">
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel ID="tempUpdate" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="lblActualPath" runat="server" Visible="false" Text='<%# Bind("INSTALLATION_DOC_PATH") %>'></asp:Label>
                                                                    <asp:Label ID="lblCurrentPath" runat="server" Visible="false" />
                                                                    <asp:HiddenField ID="hdnSelectedPath" runat="server" />
                                                                    <asp:Button ID="btnBrowse" runat="server" OnClick="btnBrowse_OnClick" Style="display: none">
                                                                    </asp:Button>
                                                                    <table align="left">
                                                                        <tr>
                                                                            <td style="padding: 0px">
                                                                                <asp:CheckBox ID="chkSelect" runat="server" Text="" Width="20px" Enabled="false" />
                                                                            </td>
                                                                            <td style="padding: 0px">
                                                                                <asp:FileUpload runat="server" ID="flUpload" Width="0px" ToolTip="Upload File" />
                                                                                <asp:Button CssClass="styleGridShortButton" ID="btnDlg" runat="server" Text="Browse"
                                                                                    Style="display: none"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:PostBackTrigger ControlID="btnBrowse" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="View" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="BtnView" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                                runat="server" ToolTip="View" OnClick="hyplnkView_Click" Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table id="Table1" width="100%" runat="server">
                <tr>
                    <td>
                    </td>
                    <td align="center">
                        <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="styleSubmitButton"
                            OnClick="btnSave_Click" OnClientClick="return fnCheckPageValidators();" ToolTip="Save" />
                        <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                            Text="Clear" OnClientClick="return fnConfirmClear();" meta:resourcekey="btnClearResource1"
                            OnClick="btnClear_Click" ToolTip="Clear" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                            CssClass="styleSubmitButton" OnClick="btnCancel_Click" ToolTip="Cancel" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvddlLOB" runat="server" ControlToValidate="ddlLOB"
                            ErrorMessage="Select a Line of Business" CssClass="styleMandatoryLabel" Display="None"
                            InitialValue="0" SetFocusOnError="True"></asp:RequiredFieldValidator>
                       <%-- <asp:RequiredFieldValidator ID="rfvddlBranch" runat="server" ControlToValidate="ddlBranch"
                            ErrorMessage="Select a Location" CssClass="styleMandatoryLabel" Display="None"
                            InitialValue="0" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                        <%-- <asp:RequiredFieldValidator ID="rfvddlMLA" InitialValue="0" CssClass="styleMandatoryLabel"
                            runat="server" ControlToValidate="ddlMLA" SetFocusOnError="True" ErrorMessage="Select a Prime Account Number"
                            Display="None"></asp:RequiredFieldValidator>--%>
                        <asp:RequiredFieldValidator ID="rfvddlSLA" InitialValue="0" CssClass="styleMandatoryLabel"
                            runat="server" ControlToValidate="ddlSLA" SetFocusOnError="True" ErrorMessage="Select a Sub Account Number"
                            Display="None"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvInvoice" InitialValue="0" CssClass="styleMandatoryLabel"
                            runat="server" ControlToValidate="ddlInvoice" SetFocusOnError="True" ErrorMessage="Select a Vendor Invoice No."
                            Display="None" Enabled="false"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="custAsset" runat="server" OnServerValidate="custAsset_ServerValidate"
                            Display="None"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <br />
                        <div id="div1" style="height: 100%; width: 100%">
                            <asp:ValidationSummary ID="vsAssetVerification" runat="server" HeaderText="Correct the following validation(s):"
                                CssClass="styleMandatoryLabel" Width="100%" />
                        </div>
                        <asp:HiddenField ID="hdnScreenWidth" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">

        function pageLoad() {
            //if (document.getElementById('divMenu').style.display == 'none') {
            //    (document.getElementById('ctl00_ContentPlaceHolder1_divAstGrid')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
            //}
            //else {
            //    (document.getElementById('ctl00_ContentPlaceHolder1_divAstGrid')).style.width = screen.width - 280;
            //}
        }

        // Not Used
        function setRight() {
            var divx = document.getElementById('ctl00_ContentPlaceHolder1_divAstGrid');
            divx.scrollTop = divx.scrollHeight;
        }

        function showMenu(show) {
            if (show == 'T') {

                if (document.getElementById('divGrid1') != null) {
                    document.getElementById('divGrid1').style.width = "800px";
                    document.getElementById('divGrid1').style.overflow = "scroll";
                }

                document.getElementById('divMenu').style.display = 'Block';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

                document.getElementById('ctl00_imgShowMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

                (document.getElementById('ctl00_ContentPlaceHolder1_divAstGrid')).style.width = screen.width - 280;
            }
            if (show == 'F') {
                if (document.getElementById('divGrid1') != null) {
                    document.getElementById('divGrid1').style.width = "960px";
                    document.getElementById('divGrid1').style.overflow = "auto";
                }

                document.getElementById('divMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'none';
                document.getElementById('ctl00_imgShowMenu').style.display = 'Block';

                (document.getElementById('ctl00_ContentPlaceHolder1_divAstGrid')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
            }
        }

        function fnLoadPath(btnBrowse) {
            if (btnBrowse != null)
                document.getElementById(btnBrowse).click();
        }

        function fnAssignPath(btnBrowse, hdnPath) {
            if (btnBrowse != null)
                document.getElementById(hdnPath).value = document.getElementById(btnBrowse).value;
        }

        function FunCheckForZero(input, strName) {


            // debugger        
            var Score = document.getElementById(input).value;
            if (Score != '') {
                if (!isNaN(Score)) {
                    if (Score == 0) {
                        alert(strName + 'cannot be Zero');
                        document.getElementById(input).value = '';
                        document.getElementById(input).focus();
                    }
                }
                //            else
                //            {
                //                var IsSlashContains=true;
                //                while(IsSlashContains)
                //                {
                //                  if(Score.indexOf('/') == -1 && Score.indexOf('-') == -1)
                //                  {  
                //                    IsSlashContains = false;
                //                  }
                //                  else
                //                  {
                //                    Score = Score.replace('/','');
                //                    Score = Score.replace('-','');
                //                  }
                //                }               
                //                
                //                if(Score.trim() == '' || Score.trim() == 0)
                //                {
                //                    alert(strName + 'is not valid');
                //                    document.getElementById(input).value = '';
                //                    document.getElementById(input).focus();
                //                }                
                //            }
            }
        }
    </script>

</asp:Content>
