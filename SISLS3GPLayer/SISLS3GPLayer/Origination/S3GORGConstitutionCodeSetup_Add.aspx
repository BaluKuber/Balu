<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GORGConstitutionCodeSetup_Add.aspx.cs" Inherits="S3GORGConstitutionCodeSetup_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
                    <td valign="top">
                        <asp:Panel GroupingText="Constitution Header" ID="Panel1" runat="server" CssClass="stylePanel">
                            <table width="100%">
                                <tr width="100%">
                                    <td width="55%">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr runat="server" id="trConstitutionCode">
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Text="Constitution Code" ID="lblConstitutionCode" CssClass="styleReqFieldLabel">
                                                    </asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtConstitutionCode" Enabled="false" runat="server" ToolTip="Constitution Code"></asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Text="Constitution Name" ID="lblConstitutionName" CssClass="styleReqFieldLabel">
                                                    </asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtConstitutionName" MaxLength="40" runat="server" Width="250px"
                                                        ToolTip="Enter Constitution Name">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvConstitutionName" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtConstitutionName" ValidationGroup="Constitution"
                                                        ErrorMessage="Enter the Constitution Name" Display="None">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <br />
                                                    <asp:Button OnClientClick="return fnCopyProfile();" CssClass="styleSubmitButton"
                                                        Text="Copy Profile" ID="lnkCopyProfile" runat="server" ToolTip="Copy Profile">
                                                    </asp:Button>
                                                </td>
                                                <td class="styleFieldLabel">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="45%" align="center">
                                        <div style="overflow-x: hidden; overflow-y: auto; height: 95px; width: 100%; text-align: center">
                                            <asp:GridView runat="server" ID="grvLOB" Width="100%" HeaderStyle-CssClass="styleGridHeader"
                                                OnRowDataBound="grvLOB_RowDataBound" AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Line of Business"
                                                        HeaderStyle-Width="70%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLOB" runat="server" ToolTip='<%#Eval("LOB_Name")%>' Text='<%#Eval("LOB_Name")%>'></asp:Label>
                                                            <asp:Label ID="lblLOBID" runat="server" Visible="false" Text='<%#Eval("LOB_ID")%>'></asp:Label>
                                                            <asp:Label ID="lblAssigned" runat="server" Visible="false" Text='<%#Eval("Assigned")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="30%">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox Visible="false" ID="chkSelectAllLOB" runat="server"></asp:CheckBox>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelectLOB" runat="server"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <%--  <HeaderStyle CssClass="styleGridHeader" />--%>
                                                <RowStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                    <td width="50%" style="display: none">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divCopyProfile" runat="server" style="display: none">
                            <table width="100%">
                                <tr class="styleRecordCount" runat="server" id="trCopyProfileMessage" visible="false">
                                    <td align="center" width="100%">
                                        <asp:Label runat="server" ID="Label4" Text="No Records Found" Font-Size="Medium"
                                            class="styleMandatoryLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="divGrid" style="overflow-x: hidden; overflow-y: auto; height: 112px; width: 750px">
                                            <asp:GridView runat="server" ID="grvConstitution" Width="748px" OnRowDataBound="grvConstitution_RowDataBound"
                                                AutoGenerateColumns="false" RowStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader"
                                                OnRowCreated="grvConstitution_RowCreated">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSel" OnCheckedChanged="FunPriGetCopyProfileDetails" AutoPostBack="true"
                                                                runat="server"></asp:CheckBox>
                                                            <asp:Label Visible="false" ID="lblConstitutionID" runat="server" Text='<%#Eval("Constitution_ID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Constitution_Code" ItemStyle-HorizontalAlign="Left" HeaderText="Constitution Code" />
                                                    <asp:BoundField DataField="Constitution_Name" ItemStyle-HorizontalAlign="Left" HeaderText="Constitution Name" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td valign="top" style="padding-top: 6px">
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel GroupingText="Constitution Documents" ID="pnlDocs" runat="server" CssClass="stylePanel">
                            <table width="100%">
                                <tr>
                                    <td style="display: inline">
                                        <asp:GridView ShowFooter="True" runat="server" ID="grvConsDocuments" Width="100%"
                                            Height="10px" OnRowDataBound="grvConsDocs_RowDataBound" DataKeyNames="Sno" AutoGenerateColumns="False"
                                            OnRowCommand="grvConsDocuments_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Document Type" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%#Eval("Doc_Cat_Flag")%>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <table>
                                                            <tr>
                                                                <td align="center" valign="middle">
                                                                    <%--<asp:DropDownList ID="ddlDocCatGird" runat="server" Visible="true" AutoPostBack="true"
                                                                        OnSelectedIndexChanged="ddlDocCatGrid_SelectedIndexChanged" ToolTip="Document Type">
                                                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ErrorMessage="Select Document Flag" ValidationGroup="Constitution"
                                                                        ID="reqDoc" ControlToValidate="ddlDocCatGird" InitialValue="0" runat="server"
                                                                        Enabled="false" Display="None"></asp:RequiredFieldValidator>--%>
                                                                    <uc2:Suggest ID="ddlDocCatGird" runat="server" ServiceMethod="GetDocumentFlags" AutoPostBack="true"
                                                                        OnItem_Selected="ddlDocCatGrid_SelectedIndexChanged" ErrorMessage="Select Document Flag" Width="300px"
                                                                        ValidationGroup="Constitution" IsMandatory="false" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </FooterTemplate>
                                                    <FooterStyle VerticalAlign="Middle" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Doc_Cat_Name" ItemStyle-HorizontalAlign="Left" Visible="false"
                                                    HeaderText="Document Category">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Document Description" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%#Eval("Doc_Cat_Desc")%>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:UpdatePanel ID="upd" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox runat="server" Width="160px" ID="txtOthersGrid" MaxLength="40" AutoPostBack="true"
                                                                    Enabled="false" OnTextChanged="txtOthersGrid_TextChanged" ToolTip="Document Description">
                                                                </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtOthersGrid"
                                                                    FilterType="Numbers, UppercaseLetters, LowercaseLetters,Custom" ValidChars=" "
                                                                    Enabled="True">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtenderDemo" runat="server" TargetControlID="txtOthersGrid"
                                                                    ServiceMethod="getDocumentsList" MinimumPrefixLength="1" CompletionSetCount="20"
                                                                    DelimiterCharacters="" Enabled="True" ServicePath="">
                                                                </cc1:AutoCompleteExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Doc_Cat_Flag" HeaderText="Document Flag" Visible="false">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Doc_Cat_Desc" Visible="false" HeaderText="Document Description">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Mandatory">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkOptMan" runat="server" Enabled="false" ToolTip="Mandatory">
                                                        </asp:CheckBox>
                                                        <asp:Label Visible="false" ID="lblOptMan" runat="server" Text='<%#Eval("Doc_Cat_OptMan")%>'></asp:Label>
                                                        <asp:Label Visible="false" ID="lblDocCatID" runat="server" Text='<%#Eval("Doc_Cat_ID")%>'></asp:Label>
                                                        <asp:Label Visible="false" ID="lblDocCatIDAssigned" runat="server" Text='<%#Eval("Doc_Cat_IDAssigned")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Wrap="True" />
                                                    <FooterTemplate>
                                                        <asp:CheckBox ID="chkOptManIns" runat="server" ToolTip="Mandatory"></asp:CheckBox>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Image Copy">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkImageCopy" runat="server" Enabled="false"></asp:CheckBox>
                                                        <asp:Label Visible="false" ID="lblImageCopy" runat="server" Text='<%#Eval("Doc_Cat_ImageCopy")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:CheckBox ID="chkImageCopyIns" runat="server" ToolTip="Image Copy"></asp:CheckBox>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtRemarks" Wrap="true" onkeydown="maxlengthfortxt(60);"
                                                            Columns="25" Rows="2" Text='<%#Eval("Remarks")%>' Width="210px" Height="35px"
                                                            TextMode="MultiLine" ToolTip="Remarks"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox runat="server" ID="txtRemarks" Wrap="true" onkeydown="maxlengthfortxt(60);"
                                                            Columns="25" Rows="2" Text='<%#Eval("Remarks")%>' Width="210px" Height="35px"
                                                            TextMode="MultiLine" ToolTip="Remarks"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnRemove" runat="server" Text="Remove" CommandName="Remove"
                                                            ToolTip="Remove" OnClick="RemoveRow" OnClientClick="return confirm('Do you want to delete this document?');" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Button ID="btnSave" CssClass="styleGridShortButton" runat="server" Text="Add"
                                                            ToolTip="Add" CausesValidation="false" CommandName="Save" OnClick="SaveRow" />
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" Width="7%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle HorizontalAlign="Center" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="styleButtonArea" align="center" style="padding-left: 4px">
                    <td>
                        <%--<asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                            OnClick="btnSave_Click" ValidationGroup="Constitution" ToolTip="Save"
                            onclientclick="return fnConfirm();" />--%>
                        <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                            OnClick="btnSave_Click" ValidationGroup="Constitution" ToolTip="Save" OnClientClick="return fnCheckPageValidators('Constitution');" />
                        <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click"
                            ToolTip="Clear" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                            ToolTip="Cancel" CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                        <div id="divTooltip" runat="server" style="border: 1px solid #000000; background-color: #FFFFCE;
                            position: absolute; display: none;">
                        </div>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <input type="hidden" value="0" runat="server" id="hdnConstitution" />
                        <input type="hidden" value="0" runat="server" id="hdnMode" />
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="ValidationSummary1" ValidationGroup="Constitution"
                            HeaderText="Correct the following validation(s):" CssClass="styleSummaryStyle"
                            Width="500px" ShowMessageBox="false" ShowSummary="true" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">


        function fnConfirm() {
            if (confirm('Do you want to save?')) {
                return true;
            }
            else
                return false;
        }

        var bResult;
        function fnCheckPageValidation(grpName) {
            if ((!fnCheckPageValidators(grpName, 'false')) && (Page_ClientValidate(grpName) == false)) {
                Page_BlockSubmit = false;
                return false;
            }
            if (Page_ClientValidate(grpName)) {
                bResult = fnIsCheckboxChecked('<%=grvLOB.ClientID%>', 'chkSelectLOB', 'Line of Business');
                if (bResult) {
                    bResult = fnIsCheckboxCheckedDoc('<%=grvConsDocuments.ClientID%>', 'chkSel', 'document from constitutional documents');
                }

                if (bResult) {
                    if (confirm('Do you want to save?')) {
                        return true;
                    }
                    else {
                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        var a = event.srcElement;
                        //a.style.display = 'block';
                        a.style.removeProperty('display');
                        //End here
                        return false;
                    }
                }
                else {
                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                    var a = event.srcElement;
                    //a.style.display = 'block';
                    a.style.removeProperty('display');
                    //End here
                }

                return bResult;

            }
        }


        function fnIsCheckboxCheckedDoc(grdid, objid, msg) {
            var chkbox;
            var objRemarksID;
            var objTxtRemarks;
            var reqRemarks;
            var txtRemarks;
            var bChecked = false;
            var bRemarks = true;
            var i = 2;
            //var gridId = 'ctl00_ContentPlaceHolder1_' + grdid ;
            var gridId = grdid;
            //objRemarksID='rfvRemarks';
            objTxtRemarks = 'txtRemarks';
            chkbox = document.getElementById(gridId + '_ctl0' + i + '_' + objid);
            //reqRemarks=document.getElementById(gridId + '_ctl0' + i + '_' + objRemarksID);
            txtRemarks = document.getElementById(gridId + '_ctl0' + i + '_' + objTxtRemarks);
            while ((chkbox != null)) {
                txtRemarks.className = "styleReqFieldDefalut";
                if (chkbox.checked) {

                    bChecked = true;

                    //if(txtRemarks.value!='')
                    //  {
                    //reqRemarks.enabled=false;      
                    //}
                    if (txtRemarks.value == '') {
                        bRemarks = false;
                        txtRemarks.className = "styleReqFieldFocus";
                        //reqRemarks.enabled=true;      
                    }
                    //break;
                }
                i = i + 1;
                if (i <= 9) {
                    chkbox = document.getElementById(gridId + '_ctl0' + i + '_' + objid);
                    //reqRemarks = document.getElementById(gridId + '_ctl0' + i + '_' + objRemarksID);
                    txtRemarks = document.getElementById(gridId + '_ctl0' + i + '_' + objTxtRemarks);
                }
                else {
                    chkbox = document.getElementById(gridId + '_ctl' + i + '_' + objid);
                    //reqRemarks = document.getElementById(gridId + '_ctl' + i + '_' + objRemarksID);
                    txtRemarks = document.getElementById(gridId + '_ctl' + i + '_' + objTxtRemarks);
                }
            }

            if ((bChecked) && (bRemarks))
                return true;
            if (!bChecked) {
                alert('Select atleast one ' + msg);
                return false;
            }

            if (!bRemarks) {
                alert('Enter the remarks for the selected document(s)');
                return false;
            }

        }

        function pageLoad() {
            //if(document.getElementById('<%=hdnMode.ClientID%>').value=='M')
            // document.getElementById('ctl00_ContentPlaceHolder1_grvConsDocuments_ctl01_chkAllDoc').disabled=true;

        }
        function fnCheck() {
            // alert('calling');
            var chkbox;
            var i = 2;
            var gridId = 'ctl00_ContentPlaceHolder1_grvConsDocuments';
            var chkStatus = true;
            chkbox = document.getElementById(gridId + '_ct' + i + '_chkSel');
            //  alert(gridId + '_ct' + i + '_chkSel');
            while (chkbox != null) {
                chkbox = document.getElementById(gridId + '_ct' + i + '_chkSel');
                if (chkbox != null) {
                    /*  if(chkbox.checked==false)
                    {
                    document.getElementById('ctl00_ContentPlaceHolder1_grvConsDocuments_ct'+ i + '_chkOptMan').disabled=true;
                    document.getElementById('ctl00_ContentPlaceHolder1_grvConsDocuments_ct'+ i + '_chkImageCopy').disabled=true;
                    }
                    else
                    {
                    document.getElementById('ctl00_ContentPlaceHolder1_grvConsDocuments_ct'+ i + '_chkOptMan').disabled=false;
                    document.getElementById('ctl00_ContentPlaceHolder1_grvConsDocuments_ct'+ i + '_chkImageCopy').disabled=false;                
                    }                
                    i=i+1;*/
                }

            }
        }

        function fnEnableCheckes(chkSel, chkOptMan, chkImageCopy) {


            document.getElementById(chkOptMan).disabled = true;
            document.getElementById(chkImageCopy).disabled = true;

            if (document.getElementById(chkSel).checked) {
                document.getElementById(chkOptMan).disabled = false;
                document.getElementById(chkImageCopy).disabled = false;
            }
            else {
                document.getElementById(chkOptMan).disabled = true;
                document.getElementById(chkImageCopy).disabled = true;
            }

            /*
            var chkSel = document.getElementById(chkSel);
            var chkOptMan = document.getElementById(chkOptMan);
            var chkImageCopy = document.getElementById(chkImageCopy);
            //alert(document.getElementById(chkSel)+'-'+document.getElementById(chkOptMan)+'-'+document.getElementById(chkImageCopy));
            //        chkOptMan.Enabled = false;
            //        chkImageCopy.Enabled = false;
            chkOptMan.Disabled = true;
            chkImageCopy.Disabled= = true;
            if (chkSel.checked)
            {
            chkOptMan.Disabled = false;
            chkImageCopy.Disabled= = false;
            chkOptMan.checked = true;
            chkImageCopy.checked = true;
            }
            */
        }


        function fnCopyProfile() {
            if (document.getElementById('<%=lnkCopyProfile.ClientID%>').value == 'Hide Copy Profile') {
                document.getElementById('<%=lnkCopyProfile.ClientID%>').value = 'Copy Profile';
                document.getElementById('ctl00_ContentPlaceHolder1_divCopyProfile').style.display = 'none';
                //            document.getElementById('ctl00_ContentPlaceHolder1_grvConsDocuments').style.height="275px";

                //document.getElementById('ctl00_ContentPlaceHolder1_grvConsDocuments').style.height = "275px";

                document.getElementById('ctl00_ContentPlaceHolder1_pnlDocs').style.display = 'block';
            }
            else {
                document.getElementById('<%=lnkCopyProfile.ClientID%>').value = 'Hide Copy Profile';
                document.getElementById('ctl00_ContentPlaceHolder1_divCopyProfile').style.display = 'Block';

                document.getElementById('ctl00_ContentPlaceHolder1_pnlDocs').style.display = 'none';

                //            document.getElementById('ctl00_ContentPlaceHolder1_grvConsDocuments').style.height="150px";
            }
            document.getElementById('<%=lnkCopyProfile.ClientID%>').title = document.getElementById('<%=lnkCopyProfile.ClientID%>').value;

            return false;
        }

        function fnAddDocType() {
            var gridId = 'ctl00_ContentPlaceHolder1_grvConsDocuments';
            var len = parseFloat(document.getElementById(gridId).rows.length);
            if (document.getElementById(gridId + '_ctl' + len + '_ddlDocCatGird')) {
                if (document.getElementById(gridId + '_ctl' + len + '_ddlDocCatGird').value == "0") {
                    alert("Select the Document Category");
                    document.getElementById(gridId + '_ctl' + len + '_ddlDocCatGird').focus();
                    return false;
                }
            }
            if (document.getElementById(gridId + '_ctl' + len + '_txtOthersGrid')) {
                if (document.getElementById(gridId + '_ctl' + len + '_txtOthersGrid').value == "") {
                    alert("Enter the Document Description");
                    document.getElementById('ctl00_ContentPlaceHolder1_grvConsDocuments_ctl' + len + '_txtOthersGrid').focus();
                    return false;
                }
            }

            if (confirm('Do you want to Add?'))
                return true;
            else
                return false;


            return true;
        }


        function showTip(objDrop, e) {
            if (objDrop.options.length > 0) {
                document.getElementById('ctl00_ContentPlaceHolder1_divTooltip').style.display = "inline";
                document.getElementById('ctl00_ContentPlaceHolder1_divTooltip').innerHTML = "&nbsp;" + objDrop.options[objDrop.selectedIndex].text + "&nbsp;";
                document.getElementById('ctl00_ContentPlaceHolder1_divTooltip').style.left = e.x;
                document.getElementById('ctl00_ContentPlaceHolder1_divTooltip').style.top = e.y;
            }
        }

        function hideText() {
            document.getElementById('ctl00_ContentPlaceHolder1_divTooltip').style.display = "none";
        }

        function fnDGUnselectAllExpectSelected(gridid, SelectedChkboxid) {
            //Get target base & child control.

            var TargetBaseControl =
       document.getElementById(gridid);

            var TargetControl = SelectedChkboxid;

            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            //alert(SelectedChkboxid.checked);
            if (SelectedChkboxid.checked) {
                for (var n = 0; n < Inputs.length; ++n)
                    if (Inputs[n].type == 'checkbox' && Inputs[n].uniqueID != TargetControl.uniqueID) {
                    if (Inputs[n].checked) Inputs[n].checked = false;
                }
            }
        }

    </script>

</asp:Content>
