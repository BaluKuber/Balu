<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GOrgBankStatement_Add.aspx.cs" Inherits="Origination_S3GOrgBankStatement_Add"
    Title="S3G - Bank Statement Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="ContentBankStatement" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td class="stylePageHeading"> 
                        <asp:Label runat="server" Text="Bank Statement Data Capture" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td valign="top">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel">
                                                </asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="80%">
                                                <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLOB_SelectIndexChanged"
                                                    Visible="true" >
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvLOB" CssClass="styleMandatoryLabel" runat="server"
                                                    ControlToValidate="ddlLOB" InitialValue="0" ErrorMessage="Select the Line of Business"
                                                    ValidationGroup="Submit" Display="None">
                                                </asp:RequiredFieldValidator>
                                                <%--<asp:RequiredFieldValidator ID="rfvLOB1" CssClass="styleMandatoryLabel" runat="server"
                                                    ControlToValidate="ddlLOB" InitialValue="0" ErrorMessage="Select Line of Business"
                                                    ValidationGroup="Go" Display="None">
                                                </asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label runat="server" Text="Product" ID="lblProductCode">
                                                </asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="80%">
                                                <asp:DropDownList ID="ddlProductCode" Enabled="false" runat="server" Visible="true">
                                                    
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label runat="server" Text="Constitution" ID="lblConstitutionCode" CssClass="styleReqFieldLabel">
                                                </asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="80%">
                                                <asp:DropDownList ID="ddlConstitution" Enabled="false" runat="server" Visible="true"
                                                    >
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvConstitution" CssClass="styleMandatoryLabel" runat="server"
                                                    ControlToValidate="ddlConstitution" InitialValue="0" ValidationGroup="Submit"
                                                    ErrorMessage="Select the Constitution" Display="None">
                                                </asp:RequiredFieldValidator>
                                                <%-- <asp:RequiredFieldValidator ID="rfvConstitution1" CssClass="styleMandatoryLabel"
                                                    runat="server" ControlToValidate="ddlConstitution" InitialValue="" ValidationGroup="go"
                                                    ErrorMessage="Select Constitution" Display="None">
                                                </asp:RequiredFieldValidator>--%>
                                                <asp:RequiredFieldValidator ID="rfvConstitution2" CssClass="styleMandatoryLabel"
                                                    runat="server" ControlToValidate="ddlConstitution" InitialValue="" ValidationGroup="Submit"
                                                    ErrorMessage="Select the Constitution" Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="Label1" runat="server" Text="Bank Statement">
                                                </asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" align="left"  width="80%">
                                                <asp:RadioButton Checked="true" ID="RBDownloadBankStatement" AutoPostBack="true"
                                                    Text="Download" GroupName="BankStatementSource" runat="server"
                                                    OnCheckedChanged="RBDownloadBankStatement_CheckedChanged" />
                                                <asp:RadioButton ID="RBScanBankStatement" AutoPostBack="true" Text="Scan"
                                                    GroupName="BankStatementSource" runat="server" OnCheckedChanged="RBDownloadBankStatement_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label runat="server" Text="Download/Scan Location Path" ID="lblScanPath">
                                                </asp:Label>
                                            </td>
                                            <td class="styleFieldAlign"  width="80%">
                                                <asp:TextBox runat="server" ID="txtScanPath" Width="20%" ReadOnly="true" MaxLength="350" onblur="TrimTxt(this)" Enabled="false">
                                                </asp:TextBox>
                                                <asp:RegularExpressionValidator ID="regdir" Display="None" ValidationExpression="^(([a-zA-Z]:)|(([a-zA-Z]:)\\(([a-zA-Z]( ?)) *?([0-9]*?) *\\?)*?))$"
                                                    runat="server" ControlToValidate="txtScanPath" ErrorMessage="Enter the path in the format (* : \******\) "
                                                    ValidationGroup="Submit">
                                                    
                                                    
                                                </asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center" style="width: 100%">
                                    <br />
                                    <asp:Button CssClass="styleSubmitShortButton" Text="Go" ID="btnGo" runat="server"
                                        OnClick="btnGo_Click" ValidationGroup="Submit"></asp:Button>
                                    <asp:Button OnClientClick="return fnCopyProfile();" CssClass="styleSubmitButton"
                                        Text="Copy Profile" ID="lnkCopyProfile" runat="server"></asp:Button>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table align="center">
                                        <%--<tr class="styleRecordCount" runat="server" id="trCopyProfileMessage" >
                                            <td align="center" width="100%">
                                                <br />
                                               <<asp:Label runat="server" ID="Label4" Text="No Records Found" Font-Size="Medium"
                                                    class="styleMandatoryLabel"></asp:Label>-
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td style="padding-top: 15px; padding-bottom: 10px">
                                                <div id="divCopyProfile" style="display: none">
                                                 <asp:Label runat="server" ID="trCopyProfileMessage" Text="No Records Found" Font-Size="Medium"
                                                    class="styleMandatoryLabel"></asp:Label>
                                                    <asp:GridView ID="grvLOBProduct" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="styleGridHeader"
                                                        OnRowDataBound="grvLOBProduct_RowDataBound" RowStyle-HorizontalAlign="Left" Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Bank Statement" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBankID" runat="server" Text='<%#Eval("Bank_stmt_Data_Capture_ID")%>'
                                                                        Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Serial No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSerial" runat="server" Text='<%#Eval("RowNumber")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Line of Business">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLOBName" runat="server" Text='<%#Eval("LOB_Name")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Product">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%#Eval("Product_Name")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Constitution">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblConstitutionCode" runat="server" Text='<%#Eval("Constitution_Name")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                Select
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="center">
                                                                            <td>
                                                                                <asp:CheckBox ID="chkStatementCaptured" runat="server" AutoPostBack="true" OnCheckedChanged="chkStatementCaptured_OnCheckedChanged" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <div style="overflow-x: hidden; overflow-y: auto; width: 100%;" visible="false" id="divBankStatement"
                                                    runat="server">
                                                    <asp:GridView ID="grvListValues" runat="server" AutoGenerateColumns="false" DataKeyNames="Bank_Stmt_LOV_ID"
                                                        OnRowDataBound="grvListOfValues_RowDataBound" FooterStyle-HorizontalAlign="Center"
                                                        HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader" OnRowCommand="grvListValues_RowCommand"
                                                        OnRowDeleting="grvListValues_RowDeleting" RowStyle-HorizontalAlign="Center" ShowFooter="true"
                                                        Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="SI.No" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="txtSerial" runat="server" MaxLength="50" Text='<%# Eval("Bank_Stmt_LOV_ID")%>'
                                                                        Visible="false" Width="110px"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtSerialAdd" runat="server" MaxLength="50" Width="110px"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftxtSerialAdd" runat="server" FilterType="Numbers"
                                                                        TargetControlID="txtSerialAdd">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </FooterTemplate>
                                                                <HeaderStyle Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="List of Values">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtListValues" runat="server" MaxLength="50" Text='<%# Bind("Field_Name") %>'
                                                                        Width="150px"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftxtListValues" runat="server" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                        TargetControlID="txtListValues" ValidChars=" ">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtListValuesAdd" runat="server"  MaxLength="50" Width="150px"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftxtListAdd" runat="server" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                        TargetControlID="txtListValuesAdd" ValidChars=" ">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:RequiredFieldValidator ID="rqvListValue" runat="server" ControlToValidate="txtListValuesAdd" ValidationGroup="Add"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter value in the List of values">
                                                                    </asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                                <HeaderStyle Width="25%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="From">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtFrom" runat="server"  MaxLength="4" Style="text-align: right"
                                                                        Text='<%# Bind("From_Position") %>' Width="40px"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftxtFrom" runat="server" FilterType="Numbers"  TargetControlID="txtFrom">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtFromAdd" runat="server" MaxLength="4" Style="text-align: right"
                                                                        Width="40px"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftxtFromAdd" runat="server" FilterType="Numbers"
                                                                        TargetControlID="txtFromAdd">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:RequiredFieldValidator ID="rqvFrom" runat="server" ControlToValidate="txtFromAdd" ValidationGroup="Add"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter From value">
                                                                    </asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                                <HeaderStyle Width="20%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="To">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtTo" runat="server" MaxLength="4" Style="text-align: right" Text='<%# Bind("To_Position")%>'
                                                                        Width="40px"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftxtTo" runat="server" FilterType="Numbers" TargetControlID="txtTo">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtToAdd" runat="server" MaxLength="4" Style="text-align: right"
                                                                        Width="40px"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftxtToAdd" runat="server" FilterType="Numbers" TargetControlID="txtToAdd">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                       <asp:RequiredFieldValidator ID="rqvto" runat="server" ControlToValidate="txtToAdd" ValidationGroup="Add"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter To value">
                                                                    </asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                                <HeaderStyle Width="20%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <FooterTemplate>
                                                                    <asp:Button ID="btnAdd" runat="server" CausesValidation="true" CommandName="AddNew"
                                                                        CssClass="styleGridShortButton" Text="Add" ValidationGroup="Add" />
                                                                </FooterTemplate>
                                                                <HeaderStyle Width="10%" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnRemove" runat="server" CausesValidation="false" CommandName="Delete"
                                                                        OnClientClick="return confirm('Do you want to delete?');" Text="Remove" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="FromCopy"  Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromCopy" runat="server" 
                                                                        Text='<%# Bind("FromCopy")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <RowStyle HorizontalAlign="Center" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="div1" style="display: none">
                                        <table>
                                            <tr>
                                                <td>
                                                    <div style="overflow-x: hidden; overflow-y: auto; height: 112px; width: 750px">
                                                        <tr id="trLOBProduct" runat="server">
                                                            <td colspan="3">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr class="styleButtonArea" style="padding-left: 4px">
                                <%--<td>
                                </td>--%>
                                <td colspan="3" align="center">
                                    <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                                        OnClick="btnSave_Click" ValidationGroup="Submit" OnClientClick="return fnCheckPageValidators(submit);" />
                                    <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                                        Text="Clear" OnClick="btnClear_Click" />
                                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                                        CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                            <tr class="styleButtonArea">
                                <td colspan="3">
                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    &nbsp;
                                </td>
                                <td class="styleFieldAlign" colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    &nbsp;
                                </td>
                                <td class="styleFieldAlign" colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="hidden" id="hdnCompanyCode" runat="server" />
                        <asp:ValidationSummary runat="server" ID="vsUserMgmt" HeaderText="Correct the following validation(s):"
                            Height="250px" CssClass="styleSummaryStyle" Width="500px" ShowMessageBox="false"
                            ValidationGroup="Submit" ShowSummary="true" />
                        <%--<asp:ValidationSummary runat="server" ID="ValidationSummary1" HeaderText="Please Correct the following validation(s):"
                            Height="250px" CssClass="styleSummaryStyle" Width="500px" ShowMessageBox="false"
                            ValidationGroup="Go" ShowSummary="true" />--%>
                            <asp:ValidationSummary runat="server" ID="ValidationSummary1" HeaderText="Correct the following validation(s):"
                            Height="250px" CssClass="styleSummaryStyle" Width="500px" ShowMessageBox="false"
                            ValidationGroup="Add" ShowSummary="true" />
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hdnID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
function fnCopyProfile()
    {      
        if(document.getElementById('<%=lnkCopyProfile.ClientID%>').value=='Hide Copy Profile')
        {
            document.getElementById('<%=lnkCopyProfile.ClientID%>').value='Copy Profile';
            document.getElementById('divCopyProfile').style.display='none';
            document.getElementById('<%=divBankStatement.ClientID%>').style.display='inline';
            document.getElementById('<%=btnSave.ClientID%>').disabled=false;
            document.getElementById('<%=btnClear.ClientID%>').disabled=false;
        }
        else
        {
            document.getElementById('<%=lnkCopyProfile.ClientID%>').value='Hide Copy Profile';
            document.getElementById('divCopyProfile').style.display='Block';
            document.getElementById('<%=divBankStatement.ClientID%>').style.display='none';
            document.getElementById('<%=btnSave.ClientID%>').disabled=true;
            document.getElementById('<%=btnClear.ClientID%>').disabled=true;
        }
        return false;
    }
    
    function TrimTxt(strInput)
    {
        var FieldValue = strInput.value;
        strInput.value = FieldValue.trim();
    }
        
    </script>

</asp:Content>
