<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
 CodeFile="S3G_ORG_FIRMasterAdd.aspx.cs" Inherits="Origination_S3G_ORG_FIRMasterAdd" Title="Field Information Report" %>

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
                            <tr id="trConstitutionCode">
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlLOB" Width="205px" OnSelectedIndexChanged="ddlLOB_OnSelectedIndexChanged"
                                        AutoPostBack="true" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:RequiredFieldValidator ID="rfvLOB" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="ddlLOB" InitialValue="0" ValidationGroup="PRDDC" ErrorMessage="Select the Line of Business"
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
                                    <asp:DropDownList ID="ddlProductCode" Width="205px" runat="server" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlProductCode_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Constitution" ID="lblConstitution" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlConstitution" Width="205px" runat="server" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlConstitution_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvConstitution" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="ddlConstitution" InitialValue="0" ErrorMessage="Select the Constitution"
                                        ValidationGroup="PRDDC" Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Scan location path" ID="lblScanPath">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox runat="server" ID="txtScanPath" Width="98%" MaxLength="350">
                                    </asp:TextBox>
                                    <%--
                                          <asp:RegularExpressionValidator ValidationGroup="PRDDC" Display="None" ID="regdir" runat ="server" ControlToValidate = "txtScanPath" 
      ErrorMessage= "pls enter path in the format " ValidationExpression ="^(([a-zA-Z]:)|(([a-zA-Z]:)\\(([a-zA-Z]( ?)) *?([0-9]*?) *\\?)*?))$" ></asp:RegularExpressionValidator> --%>


                                </td>
                            </tr>
                          
                            
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center" style="width: 100%">
                        <br />
                        <asp:Button CssClass="styleSubmitShortButton" Text="Go" ID="btnGo" runat="server"
                            OnClick="btnGo_Click" ValidationGroup="PRDDC"></asp:Button>
                        <asp:Button OnClientClick="return fnCopyProfile();" CssClass="styleSubmitButton"
                            Text="Copy Profile" ID="lnkCopyProfile" runat="server"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="styleFieldLabel">
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="divCopyProfile" style="display: none">
                            <table>
                                <tr class="styleRecordCount" runat="server" id="trCopyProfileMessage" visible="false">
                                    <td align="center" width="100%">
                                    <br />
                                        <asp:Label runat="server" ID="Label4" Text="No Records Found" Font-Size="Medium"
                                            class="styleMandatoryLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="overflow-x: hidden; overflow-y: auto; height: 112px; width: 750px">
                                            <asp:GridView runat="server" ID="grvPRDDC" Width="748px" 
                                                AutoGenerateColumns="false" RowStyle-HorizontalAlign="Center" OnRowDataBound="grvPRDDC_RowDataBound" HeaderStyle-CssClass="styleGridHeader">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSel" OnCheckedChanged="FunPriGetCopyProfileDetails" AutoPostBack="true"
                                                                runat="server"></asp:CheckBox>
                                                            <asp:Label Visible="false" ID="lblPRDDCID" runat="server" Text='<%#Eval("FIR_ID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="LOB_Code" ItemStyle-HorizontalAlign="Left" HeaderText="Line of Business" />--%>
                                                    <asp:TemplateField HeaderText="Line of Business" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLOBCode" runat="server" Text='<%#Eval("LOB_Code")%>'></asp:Label>
                                                            <asp:Label ID="lblLOBID" Visible="false" runat="server" Text='<%#Eval("LOB_ID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="Product_Code" ItemStyle-HorizontalAlign="Left" HeaderText="Product" />--%>
                                                    <asp:TemplateField HeaderText="Product" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductCode" runat="server" Text='<%#Eval("Product_Code")%>'></asp:Label>
                                                            <asp:Label ID="lblProductID" Visible="false" runat="server" Text='<%#Eval("Product_ID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Constitution" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblConstitution" runat="server" Text='<%#Eval("Constitution_Code")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="Constitution_Code" ItemStyle-HorizontalAlign="Left" HeaderText="Constitution" />--%>
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
                    <td style="padding-top: 5px" align="center">
                        <div style="overflow-x: hidden; overflow-y: auto; width: 100%;" visible="false" id="divPRDDC"
                            runat="server">
                            <asp:GridView runat="server" ID="grvPRDDCDocuments" Width="100%" 
                                AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="grvPRDDCDocuments_RowDataBound">
                                <Columns>
                                    <%-- <asp:BoundField DataField="PRDDCType" ItemStyle-HorizontalAlign="Left" HeaderText="PRDDC Type" />--%>
                                    <%--<asp:BoundField DataField="PRDDCDesc" ItemStyle-HorizontalAlign="Left" HeaderText="Description" />--%>
                                    <asp:TemplateField HeaderText="FIR Type" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                       >
                                        <ItemTemplate>
                                            <asp:Label ID="lblPRDDCType" runat="server" Text='<%#Eval("PRDDCType")%>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlPRDDCType" Width="350px" AutoPostBack="true" OnSelectedIndexChanged="ddlPRDDCType_OnSelectedIndexChanged"
                                                runat="server">
                                            </asp:DropDownList>
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr align="center">
                                                    <td>
                                                        <asp:Label ID="lblPRDDCTypeF" runat="server" Text="Others" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" FooterStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" >
                                        <ItemTemplate>
                                            <asp:Label Visible="false" ID="lblDocCatID" runat="server" Text='<%#Eval("Doc_Cat_ID")%>'></asp:Label>
                                            <asp:Label ID="lblPRDDCDesc" runat="server" Text='<%#Eval("PRDDCDesc")%>'></asp:Label>
                                            <asp:Label Visible="false" ID="lblOptMan" runat="server" Text='<%#Eval("Doc_Cat_OptMan")%>'></asp:Label>
                                            <asp:Label Visible="false" ID="lblDocCatIDAssigned" runat="server" Text='<%#Eval("Doc_Cat_IDAssigned")%>'></asp:Label>
                                            <asp:Label Visible="false" ID="lblImageCopy" runat="server" Text='<%#Eval("Doc_Cat_ImageCopy")%>'></asp:Label>
                                            <asp:Label Visible="false" ID="lblPgmID" runat="server" Text='<%#Eval("Program_ID")%>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr align="right">
                                                    <td>
                                                        <%-- <asp:TextBox ID="txtPRDDCDesc" runat="server"  MaxLength="50" ></asp:TextBox>--%>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:DropDownList ID="ddlPRDDCDesc" Width="150px" runat="server" Visible="false">
                                            </asp:DropDownList>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    &nbsp;<asp:TextBox ID="txtPRDDCDesc" runat="server" Text="--Select--" MaxLength="50"
                                                        Width="130px" AutoPostBack="true" OnTextChanged="txtPRDDCDesc_TextChanged" Enabled="False"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtenderDemo" runat="server" TargetControlID="txtPRDDCDesc"
                                                        ServiceMethod="getPRDDCDescription" MinimumPrefixLength="1" CompletionSetCount="20"
                                                        DelimiterCharacters="" Enabled="True" ServicePath="">
                                                    </cc1:AutoCompleteExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Mandatory">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkOptMan" runat="server"></asp:CheckBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="chkFootOptMan" runat="server"></asp:CheckBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Scan">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkImageCopy" runat="server"></asp:CheckBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="chkScan" runat="server"></asp:CheckBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last Program ID">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlPgmID" Width="150px" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle Width="25%" HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlFooterPgmID" Width="150px" runat="server">
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <FooterStyle Width="25%" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks" ItemStyle-HorizontalAlign="Left" >
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtRemarks" Width="170px" Wrap="true" Text='<%#Eval("Remarks")%>'
                                                onkeyup="maxlengthfortxt(50);" Columns="25" Rows="2" TextMode="MultiLine" ></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:TextBox runat="server" ID="txtFooterRemarks" Wrap="true" onkeyup="maxlengthfortxt(50);"
                                                Columns="25" Rows="2" TextMode="MultiLine" Width="170px"></asp:TextBox>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Wrap="false"  FooterStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                           
                                            <asp:CheckBox ID="chkAll" runat="server" Visible="false"></asp:CheckBox>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSel" runat="server" Visible="false"></asp:CheckBox>
                                            <asp:LinkButton ID="lnkRemovePRDDC" runat="server" Text="Remove" OnClick="lnkRemovePRDDC_Click"
                                                CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="8%" />
                                        <FooterTemplate>
                                          
                                            <asp:Button ID="addPRDDC" runat="server" Text="Add" CssClass="styleGridShortButton"
                                                OnClick="lnkAAdd_Click" CausesValidation="false"></asp:Button>
                                        
                                        </FooterTemplate>
                                        <FooterStyle Width="8%" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="styleGridHeader" />
                                <RowStyle HorizontalAlign="Center" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr class="styleButtonArea" align="center" style="padding-left: 4px">
                    <td>
                        <br />
                        <asp:Button runat="server" ID="btnSave" 
                            CssClass="styleSubmitButton" ValidationGroup="PRDDC" Text="Save" OnClientClick="return fnCheckPageValidation('PRDDC');" OnClick="btnSave_Click" />
                        <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" ValidationGroup="PRDDC" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();"
                             />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" ValidationGroup="PRDDC" CausesValidation="false"
                            CssClass="styleSubmitButton"  OnClick="btnCancel_Click" />
                        <div id="divTooltip" runat="server" style="border: 1px solid #000000; background-color: #FFFFCE;
                            position: absolute; display: none;">
                        </div>
                        <div id="divReam" runat="server" style="display: inline; width: 10px">
                        </div>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <input type="hidden" value="0" runat="server" id="hdnPRDDC" />
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="ValidationSummary1" ValidationGroup="PRDDC"
                            HeaderText="Correct the following validation(s):" CssClass="styleSummaryStyle"
                            Width="500px" ShowMessageBox="false" ShowSummary="true" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">

    var bResult;
    
    function fnTextAlign(objRemarks)
    {     
//       var intDiv= parseFloat(objRemarks.value.length)/parseFloat(10);
//       var intRem= parseFloat(objRemarks.value.length)%parseFloat(10);
//       var strRemarks="";
//       for(var i=0;i<intDiv;i++)
//       {       
//          strRemarks = strRemarks + objRemarks.value.substring(i*10,((i+1)*10))+"</br>";
//       }       
//       divReam.innerHTML=strRemarks;       
//       objRemarks.value =  divReam.innerHTML;
       
    }
    
    function fnCheckMan(objChkSel,objMan,objScan,objProgram,objRemarks)
    {        
        if(document.getElementById(objChkSel).checked)
        {
            document.getElementById(objMan).checked=true;            
        }
        else
        {
            document.getElementById(objMan).checked=false;           
            document.getElementById(objScan).checked=false;    
            document.getElementById(objProgram).value="0";
            document.getElementById(objRemarks).value="";
        }
    }   
    
    function fnDisplayDocType()
    {
     
    }
    
    function fnCopyProfile()
    {      
        if(document.getElementById('<%=lnkCopyProfile.ClientID%>').value=='Hide Copy Profile')
        {
            document.getElementById('<%=lnkCopyProfile.ClientID%>').value='Copy Profile';
            document.getElementById('divCopyProfile').style.display='none';
            document.getElementById('<%=divPRDDC.ClientID%>').style.display='inline';
            document.getElementById('<%=btnSave.ClientID%>').disabled=false;
            document.getElementById('<%=btnClear.ClientID%>').disabled=false;
        }
        else
        {
            document.getElementById('<%=lnkCopyProfile.ClientID%>').value='Hide Copy Profile';
            document.getElementById('divCopyProfile').style.display='Block';
            document.getElementById('<%=divPRDDC.ClientID%>').style.display='none';
            document.getElementById('<%=btnSave.ClientID%>').disabled=true;
            document.getElementById('<%=btnClear.ClientID%>').disabled=true;
        }
        return false;
    }
    
    function fnCheckPageValidation(grpName)
    {
        if( (!fnCheckPageValidators(grpName,'false')) && ( Page_ClientValidate(grpName)==false ) )
        {
            Page_BlockSubmit=false;
            return false ;
        }
        
      if(Page_ClientValidate(grpName))
      {
            
        bResult=fnIsCheckboxCheckedDoc('<%=grvPRDDCDocuments.ClientID%>','chkSel','document from FIR documents');
        if(bResult)
         {
          if (confirm('Are you sure want to save?')) 
            {
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
      
       function fnIsCheckboxCheckedDoc(grdid, objid,msg) {
    
    var chkbox;
    var objRemarksID;
    var objTxtRemarks;
    var reqRemarks;
    var txtRemarks;
    var bChecked = false;
    var bRemarks=true;
    var i = 2;
    //var gridId = 'ctl00_ContentPlaceHolder1_' + grdid ;
    var gridId = grdid;
    //objRemarksID='rfvRemarks';
    objTxtRemarks='ddlPgmID';
    
    chkbox = document.getElementById(gridId + '_ctl0' + i + '_' + objid);
    //reqRemarks=document.getElementById(gridId + '_ctl0' + i + '_' + objRemarksID);
    txtRemarks=document.getElementById(gridId + '_ctl0' + i + '_' + objTxtRemarks);
    
    //while ((chkbox != null)) 
    while ((txtRemarks != null)) 
    {
        txtRemarks.className="styleReqFieldDefalut";
//        if (chkbox.checked) 
//        {
//       
            bChecked = true;
            
            //if(txtRemarks.value!='')
              //  {
                    //reqRemarks.enabled=false;      
                //}
                if(txtRemarks.options.value ==0)
                {
                    bRemarks=false;
                    txtRemarks.className="styleReqFieldFocus";
                    //reqRemarks.enabled=true;      
                }
            //break;
        //}
        i = i + 1;
        if(i<=9) 
        {
            chkbox = document.getElementById(gridId + '_ctl0' + i + '_' + objid);
            //reqRemarks = document.getElementById(gridId + '_ctl0' + i + '_' + objRemarksID);
            txtRemarks = document.getElementById(gridId + '_ctl0' + i + '_' + objTxtRemarks);
        }
        else
        {
            chkbox = document.getElementById(gridId + '_ctl' + i + '_' + objid);
            //reqRemarks = document.getElementById(gridId + '_ctl' + i + '_' + objRemarksID);
            txtRemarks = document.getElementById(gridId + '_ctl' + i + '_' + objTxtRemarks);
        }
    }
    
    if ((bChecked)&&(bRemarks))
        return true;
    if (!bChecked)
        {
        alert('Select atleast one ' + msg);
        return false;
        }

    if (!bRemarks)
        {
        alert('Select the Last Program ID for the selected document(s)');
        return false;
        }        
        
    }

      
      
    }
    
    </script>

</asp:Content>

