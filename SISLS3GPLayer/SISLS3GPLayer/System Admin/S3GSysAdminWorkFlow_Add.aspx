<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GSysAdminWorkFlow_Add.aspx.cs" Inherits="S3GSysAdminWorkFlow_Add" %>

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
                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr>
                                <td colspan="2">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvLOB" CssClass="styleMandatoryLabel" runat="server"
                                                    ControlToValidate="ddlLOB" InitialValue="0" ErrorMessage="Select the Line of Business"
                                                    Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvProductCode" CssClass="styleMandatoryLabel" runat="server"
                                                    ControlToValidate="ddlProductCode" InitialValue="0" ErrorMessage="Select the Product"
                                                    Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvModuleCode" CssClass="styleMandatoryLabel" runat="server"
                                                    ControlToValidate="ddlModuleCode" InitialValue="0" ErrorMessage="Select the Module"
                                                    Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvProgramID" CssClass="styleMandatoryLabel" runat="server"
                                                    ControlToValidate="ddlProgramID" InitialValue="0" ErrorMessage="Select the Program"
                                                    Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>--%>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLOB_OnSelectedIndexChanged"
                                        Width="205px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Product" ID="lblProductCode" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlProductCode" runat="server" Width="205px" onchange="fnDisplayWorkflow();">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <%-- <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Module" ID="lblModuleCode" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlModuleCode" runat="server" Width="205px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Program" ID="lblProgramID" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">

                                    <asp:DropDownList ID="ddlProgramID" runat="server" Width="265px">
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Workflow Sequence" ID="lblWorkflowSequence" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtWorkflowSequence" Enabled="false" runat="server" Width="240px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Active" ID="lblActive" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign" style="padding-left: 7px;">
                                    <asp:CheckBox ID="chkActive" Checked="true" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div style="height: 235px; overflow-x: hidden; overflow-y: auto;">
                                        <asp:GridView ID="gvWorkFlowSequence" runat="server" AutoGenerateColumns="false"
                                            Width="100%" ShowFooter="true" DataKeyNames="WorkFlow_ID" OnRowDataBound="gvWorkFlowSequence_RowDataBound"
                                            OnRowCommand="gvWorkFlowSequence_RowCommand" OnRowEditing="gvWorkFlowSequence_RowEditing"
                                            OnRowCancelingEdit="gvWorkFlowSequence_RowCancelingEdit" OnRowUpdating="gvWorkFlowSequence_RowUpdating"
                                            OnRowDeleting="gvWorkFlowSequence_RowDeleting">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Module">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblModuleCode_Grd" runat="server" Text='<%#Bind("Module_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlModuleCode_Grd" runat="server" OnSelectedIndexChanged ="ddlModuleCode_Grd_SelectedIndexChanged" AutoPostBack ="true">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlModuleCode_Grd" runat="server" OnSelectedIndexChanged ="ddlModuleCode_Grd_SelectedIndexChanged" AutoPostBack ="true" >
                                                        </asp:DropDownList>
                                                        
                                                    </EditItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Program">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProgramID_Grd" runat="server" Text='<%#Bind("Program_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlProgramID_Grd" runat="server" OnSelectedIndexChanged="ddlProgramID_Grd_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlProgramID_Grd" runat="server" OnSelectedIndexChanged="ddlProgramID_Grd_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <ItemStyle Width="25%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Program Flow Number">
                                                <ItemStyle HorizontalAlign = "Right"  />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProgramFlowNumber" runat="server" Text='<%#Bind("Program_Flow_ID") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlProgramFlowNumber_Grd" runat="server">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlProgramFlowNumber_Grd" runat="server">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sequence ID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSequenceID" runat="server" Text='<%#Bind("WokfFlow_Sequence_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtSequenceID" runat="server" Visible="false" ReadOnly="true"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                            CausesValidation="false"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                                            CausesValidation="false"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="btnAdd" runat="server" Text="Add" CommandName="Add" CausesValidation="false"></asp:LinkButton>
                                                    </FooterTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnRemove" runat="server" Text="Delete" CommandName="Delete"
                                                            OnClientClick="return confirm('Do you want to delete?');" CausesValidation="false"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr class="styleButtonArea" style="padding-left: 4px">
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators();"
                                        CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click" />
                                    <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                                        Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                                        CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                            <tr class="styleButtonArea">
                                <td colspan="2">
                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="hidden" id="hdnCompanyCode" runat="server" />
                        <input type="hidden" id="hdnLOB" runat="server" />
                        <input type="hidden" id="hdnID" runat="server" />
                        <asp:ValidationSummary runat="server" ID="vsUserMgmt" HeaderText="Please correct the following validation(s):"
                            Height="250px" CssClass="styleSummaryStyle" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">

function fnDisplayWorkflow(intModuleID, intProgramID, ProgramFlowID, SequenceID)
{   
    var idLOBCode=document.getElementById('<%=ddlLOB.ClientID%>');   
    var idLOBCode=document.getElementById('<%=hdnLOB.ClientID%>').value;
    var idProCode=document.getElementById('<%=ddlProductCode.ClientID%>');
    
    var strPgmCode;
    if (idProCode.options.selectedIndex>0)
    //if ((idProCode.options.selectedIndex>0)&&(idModCode.options.selectedIndex>0)&&(idPgmCode.options.selectedIndex>0))
    {
        //strPgmCode=idPgmCode.options.value;    
        //if(idPgmCode.options.value.length==1)
          //  strPgmCode="00"+idPgmCode.options.value;    
        //if(idPgmCode.options.value.length==2)
          //  strPgmCode="0"+idPgmCode.options.value;    
            
        document.getElementById('<%=txtWorkflowSequence.ClientID%>').value=document.getElementById('<%=hdnCompanyCode.ClientID%>').value + idLOBCode + idProCode.options.value;
    }
    else
    {
        document.getElementById('<%=txtWorkflowSequence.ClientID%>').value="";
    }
    

}


function fnDisplayWorkflow1(intModuleID, intProgramID, ProgramFlowID, SequenceID)
{
// alert(intModuleID);
 //return;   
    
    //var idLOBCode=document.getElementById('<%=ddlLOB.ClientID%>');
    
    //var idLOBCode=document.getElementById('<%=hdnLOB.ClientID%>').value;
    //var idProCode=document.getElementById('<%=ddlProductCode.ClientID%>');
    var idModCode=document.getElementById(intModuleID);
    var idPgmCode=document.getElementById(intProgramID);
    var idProgramFlowID=document.getElementById(ProgramFlowID);
    var idSequenceID=document.getElementById(SequenceID);
    if(idModCode.value == 0)
    {
        alert('Please select the Module');
        idModCode.focus();
        return;
    }
    if(idPgmCode.value == 0)
    {
        alert('Please select the Program');
        idPgmCode.focus();
        return;
    }
    if(idProgramFlowID.value == '')
    {
        alert('Please enter Program Flow Number');
        idProgramFlowID.focus();
        return;    
    }
    idSequenceID.value = "" + idModCode.value + idPgmCode.value + idProgramFlowID.value;
    /*
    var strPgmCode;
    if ((idProCode.options.selectedIndex>0)&&(idModCode.options.selectedIndex>0)&&(idPgmCode.options.selectedIndex>0))
    {
        strPgmCode=idPgmCode.options.value;    
        if(idPgmCode.options.value.length==1)
            strPgmCode="00"+idPgmCode.options.value;    
        if(idPgmCode.options.value.length==2)
            strPgmCode="0"+idPgmCode.options.value;    
            
        document.getElementById('<%=txtWorkflowSequence.ClientID%>').value=document.getElementById('<%=hdnCompanyCode.ClientID%>').value + idLOBCode + idProCode.options.value + idModCode.options.value + strPgmCode;
    }
    else
    {
        document.getElementById('<%=txtWorkflowSequence.ClientID%>').value="";
    }
    */

}

    </script>

</asp:Content>
