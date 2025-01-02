<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Origination_Default" %>

<%@ Register TagPrefix="ucfile" TagName="ucf" Src="~/UserControls/S3GFileUpload.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script language="javascript" type="text/javascript">
function getID()
{
 if(document.getElementById('<%=txtName.ClientID%>').value>0)
 {
 return true;
 }
 else
 {
 document.getElementById('<%=txtName.ClientID%>').focus();
 return false;
 }
}
</script>
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblHeading" runat="server" CssClass="styleDisplayLabel">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" align="center">
                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td valign="top" align="left">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td valign="top" colspan="2">
                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                            <asp:TextBox ID="txtName" Style="text-align: right;"  runat="server" ></asp:TextBox>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" colspan="2">
                                        <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                            <tr>
                                                <td colspan="5">
                                                    
                                                </td>
                                            </tr>
                                            <tr id="trSerial" runat="server" visible="false">
                                                <td style="width: 22%;" class="styleFieldLabel">
                                                    <asp:Label ID="lblSerialNumber" runat="server" CssClass="styleReqFieldLabel" Text="Serial Number"></asp:Label>
                                                </td>
                                                <td style="width: 25%;">
                                                    <asp:TextBox ID="txtSerialNumber" runat="server" Text="1"></asp:TextBox>
                                                </td>
                                                <td style="width: 5%;">
                                                </td>
                                                <td style="width: 22%;" class="styleFieldLabel">
                                                </td>
                                                <td style="width: 25%;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line of Business"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlLineofBusiness" ReadOnly="true" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblProduct" runat="server" Text="Product"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlProduct" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblModuleID" runat="server" Text="Module ID"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtModuleID" runat="server"></asp:TextBox>
                                                    
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblProgramID" runat="server" Text="Program ID"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtProgramID" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5" align="center">
                                                    <asp:Button runat="server" ID="btnSave" OnClientClick="return getID()" CssClass="styleSubmitButton" OnClick="Save_Click"
                                                        Text="Save" />
                                                    &nbsp;
                                                    <asp:Button ID="btnReset" runat="server" CausesValidation="False" OnClientClick="return fnConfirmClear();"
                                                        CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" />
                                                    &nbsp;
                                                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="False"
                                                        CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5" align="center">
                                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                                HeaderText="Please correct the following validation(s):" ShowSummary="true" ValidationGroup="btnSave" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnWorkFlowStatus" runat="server" />


    <div>  
       
       <asp:UpdatePanel runat="server" ID="TEst" >
       <ContentTemplate>
            
             <cc1:TabContainer ID="tcPDDT" runat="server" CssClass="styleTabPanel" Width="100%"
                    AutoPostBack="false"  ActiveTabIndex="0">
                    
                     <cc1:TabPanel runat="server" HeaderText="Customer" CssClass="tabpan" ID="TabPanel1"
                        BackColor="Red">
                        <HeaderTemplate>
                            Customer Details teSTTTT
                        </HeaderTemplate>
                        <ContentTemplate>
                        
                        
                        </ContentTemplate>
                        
                        </cc1:TabPanel>
                        
                    <cc1:TabPanel runat="server" HeaderText="Customer" CssClass="tabpan" ID="tpcust"
                        BackColor="Red">
                        <HeaderTemplate>
                            Customer Details
                        </HeaderTemplate>
                        <ContentTemplate>
            
            
             <asp:GridView ID="gv1" runat="server" >
             
                <Columns >
               <asp:TemplateField>
               
               <ItemTemplate>
                    Please upload a image:<br />  
                    <cc1:AsyncFileUpload ID="AsyncFileUpload1" runat="server" OnUploadedComplete="AsyncFileUpload1_UploadedComplete" />  
                    <br />  
                    <br />  
                    Here is your image:<br />  
                    <asp:Label runat="server" Text=" " ID="uploadResult" /><br />  
                    <asp:Image ID="Image1" runat="server" />
                
                </ItemTemplate>
                
                </asp:TemplateField>
                <asp:TemplateField >
                <ItemTemplate>
                <asp:Label ID="LBL" runat="server" ></asp:Label>
                
                </ItemTemplate>
                </asp:TemplateField>
                </Columns>
                </asp:GridView>
                
                </ContentTemplate>
                
                </cc1:TabPanel>
                </cc1:TabContainer>
                
          </ContentTemplate>
          </asp:UpdatePanel>
   


    </div>   
    
    
     <asp:UpdatePanel ID="up1" runat ="server" > 
     <ContentTemplate >
      <%--<asp:TextBox ID="txtFilePath" runat ="server" EnableViewState ="false" /> 
           <ucfile:ucf ID="Ucf1" runat ="server" />
           <asp:Button ID="btnSave" Text="  Save" runat ="server" 
                    onclick="btnSave_Click1" style="height: 26px" />--%>
     
   <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" >
        <Columns>
            <asp:TemplateField>
            <ItemTemplate>
         
            <ucfile:ucf ID="Ucf1"   runat ="server" />
             <%--<asp:Button ID="Button1" Text="  Save" runat ="server" 
                    onclick="btnSave_Click1" style="height: 26px" />--%>
                    </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
    <asp:Button ID="Button1" Text="  Save" runat ="server" 
                    onclick="btnSave_Click1" style="height: 26px" />
                    
                     </ContentTemplate>
      </asp:UpdatePanel>
       
</asp:Content>
