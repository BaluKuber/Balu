<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLoanAdGlobalParameterSetup_Add.aspx.cs" Inherits="LoanAdmin_S3GLoanAdGlobalParameterSetup_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--Name: Srivatsan
        Changes Performed on the page: Add the code for Boolean conversion for all the fields involved in checkboxes.
    --%>

    <script language="javascript" type="text/javascript">
//Created by Tamilselvan.S on 22/04/2011 for Gridview checkbox control uncheck all column in a row when select a column
function fnDGUnselectAllCoulmnRowIncomeMethod(gridid,SelectedChkboxid,objid,CurrentRowId) {
   
  
    var chkboxIRR = document.getElementById(gridid + '_ctl' + CurrentRowId + '_CbxIRR' );
  
    
}

function fnDGUnselectAllCoulmnRowIncomeType(gridid,SelectedChkboxid,objid,CurrentRowId) {
   
    var chkboxIns = document.getElementById(gridid + '_ctl' + CurrentRowId + '_CbxIns');
    var chkboxAccounting = document.getElementById(gridid + '_ctl' + CurrentRowId + '_CbxAccounting' );
    if(objid=='Ins')
    {
         chkboxAccounting.checked = false; 
    }
    else if(objid=='Accounting')
    {
         chkboxIns.checked = false; 
    }    
}
//Code end
function fnDGUnselectAllCoulmnRowPreClosureType(gridid,SelectedChkboxid,objid,CurrentRowId) {
   
    var chkboxIRR_Type = document.getElementById(gridid + '_ctl' + CurrentRowId + '_chkIRR_Type');
    var chkboxNPV_Type = document.getElementById(gridid + '_ctl' + CurrentRowId + '_chkNPV_Type' );
    var chkboxPrinciple_Type = document.getElementById(gridid + '_ctl' + CurrentRowId + '_chkPrinciple_Type' );
    if(objid=='IRR_Type')
    {
         chkboxNPV_Type.checked = false;  chkboxPrinciple_Type.checked = false; 
    }
    else if(objid=='NPV_Type')
    {
         chkboxIRR_Type.checked = false;  chkboxPrinciple_Type.checked = false; 
    }  
    else if(objid=='Principle_Type')
    {
         chkboxIRR_Type.checked = false;  chkboxNPV_Type.checked = false; 
    }   
}

function pageLoad()
{
    var divx = document.getElementById('ctl00_ContentPlaceHolder1_tcGPS_tpcust_divAcc');
    divx.scrollTop = divx.scrollHeight; /* The above two line for set focus for Account tab grid lower */

    var tab = $find('ctl00_ContentPlaceHolder1_tcGPS');
    tab.add_activeTabChanged(on_Change);
}


function on_Change(sender,e)
{                           
    document.getElementById('ctl00_ContentPlaceHolder1_btnResetMessage').click();
}



    </script>

    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px;">
                        <div style="margin-left: 2px; margin-right: 2px;">
                            <cc1:TabContainer ID="tcGPS" runat="server" CssClass="styleTabPanel" ActiveTabIndex="0"
                                AutoPostBack="false">
                                <cc1:TabPanel runat="server" HeaderText="Account Activation" CssClass="tabpan" ID="tpcust"
                                    BackColor="Red">
                                    <HeaderTemplate>
                                        Account Activation
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td>
                                                    <asp:Panel GroupingText="Applicability of Invoice, Asset Identification Entry and Asset Insurance Entry"
                                                        ToolTip="Applicability of Invoice, Asset Identification Entry and Asset Insurance Entry"
                                                        ID="pnlAccount" runat="server" CssClass="stylePanel" Visible="false">
                                                        <div runat="server" id="divAcc" class="container" style="height: 139px; overflow-x: hidden;
                                                            overflow-y: scroll;">
                                                            <asp:GridView ID="gvAccount" runat="server" Width="98%" ShowHeader="true" AutoGenerateColumns="False"
                                                               ShowFooter="True" DataKeyNames="LOB_ID">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLobID" runat="server" Text='<%# Eval("LOB_ID") %>' /></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProductID" runat="server" Text='<%# Eval("Product_ID") %>' /></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblModuleID" runat="server" Text="7" /></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Line of Business">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtLOB" runat="server" ReadOnly="true" ToolTip="Line of Business"
                                                                                Width="175px" Text='<%# Eval("LOB") %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="true" Width="175px" ToolTip="Line of Business"
                                                                                ValidationGroup="vgGPSAccount" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator runat="server" ID="rfvAccLOB" ValidationGroup="vgGPSAccount"
                                                                                ControlToValidate="ddlLOB" ErrorMessage="Select the Line of Business" InitialValue="0"
                                                                                Display="None"> </asp:RequiredFieldValidator>
                                                                        </FooterTemplate>
                                                                        <FooterStyle Width="24%" HorizontalAlign="Center" />
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="24%" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Product">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtProduct" runat="server" ReadOnly="true" Width="175px" ToolTip="Product"
                                                                                Text='<%# Eval("Product_Code")%>'></asp:TextBox>
                                                                            <asp:CheckBox runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "IsActive")))%> '
                                                                                ID="chkActive" Visible="false" ToolTip="Invoice" />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:DropDownList ID="ddlProduct" runat="server" AutoPostBack="false" ToolTip="Product"
                                                                                Width="175px">
                                                                            </asp:DropDownList>
                                                                        </FooterTemplate>
                                                                        <FooterStyle Width="24%" HorizontalAlign="Center" />
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="24%" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Invoice(INV)">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Parameter_Value")))%> '
                                                                                ID="CbxFInvoice" ToolTip="Invoice" />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:CheckBox runat="server" ID="CbxInvoice" Visible="false"  />
                                                                        </FooterTemplate>
                                                                        <FooterStyle Width="12%" HorizontalAlign="Center" />
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="12%" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Asset Indentification Entry(AIE)">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Parameter_Value1")))%> '
                                                                                ID="CbxFAsset" ToolTip="Asset Indentification Entry" />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:CheckBox runat="server" ID="CbxAsset" Visible="false"  />
                                                                        </FooterTemplate>
                                                                        <FooterStyle Width="13%" HorizontalAlign="Center" />
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="13%" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Asset Insurance Entry(AInsE)">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Parameter_Value2")))%> '
                                                                                ID="CbxFAssetIns" ToolTip="Asset Insurance Entry" />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:CheckBox runat="server" ID="CbxAssetIns" Visible="false"  />
                                                                        </FooterTemplate>
                                                                        <FooterStyle Width="12%" HorizontalAlign="Center" />
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="12%" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Add">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnRemove" Text="Remove" CommandName="Delete" runat="server"
                                                                                ToolTip="Remove" CausesValidation="false" />
                                                                            <cc1:ConfirmButtonExtender ID="CnmBEbtnRemove" runat="server" ConfirmText="Do you want to Delete?"
                                                                                Enabled="True" TargetControlID="btnRemove">
                                                                            </cc1:ConfirmButtonExtender>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Button ID="btnAddrow" CausesValidation="true" runat="server" CommandName="AddNew"
                                                                                ToolTip="Add" ValidationGroup="vgGPSAccount" CssClass="styleSubmitShortButton"
                                                                                Text="Add"></asp:Button>
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" Width="13%" />
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" Width="13%" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <%--<FooterStyle CssClass="FrozenHeader" />--%>
                                                                <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                                                <EmptyDataTemplate>
                                                                    <span>No Records Found...</span>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td valign="top" style="width: 50%;">
                                                                <asp:Panel GroupingText="Paisa round off in installments" ToolTip="Paisa round off in installments"
                                                                    ID="Panel2" runat="server" CssClass="stylePanel">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td width="25%">
                                                                                <asp:Label ID="lblPaise" runat="server" Text="Paisa Round off" CssClass="styleReqFieldLabel" />
                                                                            </td>
                                                                            <td width="25%">
                                                                                <asp:TextBox ID="txtPaise" runat="server" Width="100px" MaxLength="2" Style="text-align: right"
                                                                                    ToolTip="Paisa Round off" ValidationGroup="btnSave" />
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtPaise"
                                                                                    FilterType="Numbers" Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="rfvPaise" runat="server" Display="None" ErrorMessage="Enter the Paisa Round off"
                                                                                    ControlToValidate="txtPaise" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                            <td style="width: 50%;">
                                                                <asp:Panel GroupingText="Gap Days b/w the agreement & account activation date" ToolTip="Gap Days b/w the agreement & account activation date"
                                                                    ID="Panel1" runat="server" CssClass="stylePanel" Visible="false">
                                                                    <div class="container" style="height: 223px; width: 99%; overflow-x: hidden; overflow-y: scroll">
                                                                        <asp:GridView ID="gvGapDays" runat="server" CssClass="styleInfoLabel" Width="94%"
                                                                            ShowHeader="true" AutoGenerateColumns="False" DataKeyNames="LOB_ID" OnRowDataBound="gvGapDays_RowDataBound">
                                                                            <Columns>
                                                                                <asp:TemplateField Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblLobID" runat="server" Text='<%# Eval("LOB_ID") %>' /></ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Line of Business">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblLOB" runat="server" Text='<%# Eval("LOB") %>' ToolTip="Line of Business" />
                                                                                        <%--Name: Srivatsan.S
                                                                                            Changes: Added the code '<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Active")))%>' to implement Boolean features.--%>
                                                                                        <%--Change begins--%>
                                                                                        <asp:CheckBox runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Active")))%>'
                                                                                            ID="chkActive" Visible="false" />
                                                                                        <%--Change ends--%>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                                    <ItemStyle Width="60%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Days">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtDays" runat="server" Width="80px" MaxLength="2" Text='<%# Eval("Parameter_Value") %>'
                                                                                            Style="text-align: right" ToolTip="Days"></asp:TextBox>
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtDays"
                                                                                            FilterType="Numbers" ValidChars="" Enabled="True">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                                    <ItemStyle Width="24%" HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                                                            <EmptyDataTemplate>
                                                                                <span>No Records Found...</span>
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </asp:Panel>
                                                            </td>
                                                            
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" HeaderText="Income Recognition" CssClass="tabpan" ID="TabPanel1"
                                    BackColor="Red">
                                    <HeaderTemplate>
                                        Income Recognition
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel GroupingText="Income Recognition Method" ToolTip="Income Recognition Method"
                                            ID="Panel3" runat="server" CssClass="stylePanel">
                                            <div class="container" style="height: 139px; overflow-x: hidden; overflow-y: scroll">
                                                <asp:GridView ID="gvIncome" runat="server" CssClass="styleInfoLabel" Width="98%"
                                                    AutoGenerateColumns="False" DataKeyNames="LOB_ID" OnRowDataBound="gvIncome_RowDataBound"
                                                    OnRowCreated="gvIncome_RowCreated">
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLobID" runat="server" Text='<%# Eval("LOB_ID") %>' /></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Line of Business">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLOB" runat="server" ToolTip="Line of Business" Text='<%# Eval("LOB") %>' />
                                                                <asp:CheckBox runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Active")))%>'
                                                                    ID="chkActive" Visible="false" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                            <ItemStyle Width="30%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false" HeaderText="ESM">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CbxEsm" runat="server" ToolTip="ESM" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Parameter_Value")))%> ' />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="IRR">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CbxIRR" runat="server" ToolTip="IRR" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Parameter_Value1")))%> ' />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false" HeaderText="SOD">
                                                            <ItemTemplate>
                                                                <asp:CheckBox  ID="CbxSOD" runat="server" ToolTip="SOD" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Parameter_Value2")))%> ' />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false" HeaderText="Product">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CbxProduct" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Parameter_Value3")))%> '
                                                                    ToolTip="Product" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <span>No Records Found...</span>
                                                    </EmptyDataTemplate>
                                                    <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                                </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel GroupingText="Income Recognition Type" ToolTip="Income Recognition Type"
                                            ID="Panel4" runat="server" CssClass="stylePanel">
                                            <div class="container" style="height: 139px; overflow-x: hidden; overflow-y: scroll">
                                                <asp:GridView ID="gvIncomeType" runat="server" CssClass="styleInfoLabel" Width="50%"
                                                    AutoGenerateColumns="False" DataKeyNames="LOB_ID" OnRowDataBound="gvIncomeType_RowDataBound"
                                                    OnRowCreated="gvIncomeType_RowCreated">
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLobID" runat="server" Text='<%# Eval("LOB_ID") %>' /></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Line of Business">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLOB" runat="server" Text='<%# Eval("LOB") %>' ToolTip="Line of Business" />
                                                                <asp:CheckBox runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Active")))%>'
                                                                    ID="chkActive" Visible="false" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                            <ItemStyle Width="30%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Installment Basis">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CbxIns" runat="server" ToolTip="Installment Basis" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Parameter_Value")))%> ' />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Accounting Period Basis">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CbxAccounting" runat="server" ToolTip="Accounting Period Basis"
                                                                    Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Parameter_Value1")))%> ' />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <span>No Records Found...</span>
                                                    </EmptyDataTemplate>
                                                    <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                                </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" HeaderText="Revision" CssClass="tabpan" ID="tpRevision"
                                    BackColor="Red" Visible="false">
                                    <HeaderTemplate>
                                        Revision
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 50%;" align="left">
                                                    <asp:Panel GroupingText="Revision Rate Parameter" ToolTip="Revision Rate Parameter"
                                                        ID="tbRevision" runat="server" CssClass="stylePanel">
                                                        <div class="container" style="width: 100%; height: 180px; overflow-x: hidden; overflow-y: scroll">
                                                            <asp:GridView ID="gvRevision" runat="server" CssClass="styleInfoLabel" Width="94%"
                                                                AutoGenerateColumns="False" DataKeyNames="LOB_ID" OnRowDataBound="gvRevision_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLobID" runat="server" Text='<%# Eval("LOB_ID") %>' /></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Line of Business">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLOB" runat="server" Text='<%# Eval("LOB") %>' ToolTip="Line of Business" />
                                                                            <asp:CheckBox runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Active")))%>'
                                                                                ID="chkActive" Visible="false" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="30%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="+ / - %">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtAlpha" runat="server" Width="80px" onkeypress="fnAllowNumbersAndPlusMin(true,true,this)"
                                                                                Text='<%# Eval("Parameter_Value") %>' ToolTip="+ / - %"></asp:TextBox>
                                                                            <%--<cc1:FilteredTextBoxExtender ID="ftbetxtAlpha" runat="server" TargetControlID="txtAlpha"
                                                                    ValidChars="-+." FilterType="Numbers,Custom"
                                                                    Enabled="True">
                                                                </cc1:FilteredTextBoxExtender>--%>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                                                <EmptyDataTemplate>
                                                                    <span>No Records Found...</span>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </td>
                                                <td style="width: 50%;" valign="top">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 100%;">
                                                                <asp:Panel GroupingText="Intervening Period Interest" ToolTip="Intervening Period Interest"
                                                                    ID="tbInvert" runat="server" CssClass="stylePanel">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td width="25%">
                                                                                <asp:Label ID="lblInterst" runat="server" Text="Intervening Period Interest" CssClass="styleReqFieldLabel" />
                                                                            </td>
                                                                            <td width="25%">
                                                                                <asp:DropDownList ID="ddlInterest" runat="server" Width="100px" ToolTip="Intervening Period Interest"
                                                                                    ValidationGroup="btnSave">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvInterest" runat="server" Display="None" ErrorMessage="Select the Intervening Period Interest"
                                                                                    ControlToValidate="ddlInterest" InitialValue="0" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 50%;">
                                                                <asp:Panel GroupingText="Gap between the Revision Effective and system date" ToolTip="Gap between the Revision Effective and system date"
                                                                    ID="Panel8" runat="server" CssClass="stylePanel">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td width="25%">
                                                                                <asp:Label ID="lblGap" runat="server" Text="Gap Days" CssClass="styleReqFieldLabel" />
                                                                            </td>
                                                                            <td width="25%">
                                                                                <asp:TextBox ID="txtGapRevision" runat="server" Width="100px" MaxLength="2" Style="text-align: right"
                                                                                    ToolTip="Gap Days" ValidationGroup="btnSave"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtGapRevision"
                                                                                    FilterType="Numbers" Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="rfvGap" runat="server" Display="None" ErrorMessage="Enter the Revision Gap Days"
                                                                                    ControlToValidate="txtGapRevision" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" HeaderText="General" CssClass="tabpan" ID="tpCollateral" Visible="false"
                                    BackColor="Red">
                                    <%-- HeaderText="Collateral Transaction Type"--%>
                                    <HeaderTemplate>
                                        General
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel GroupingText="Overdue Interest Calculation" ID="Panel11" runat="server"
                                            ToolTip="Overdue Interest Calculation" CssClass="stylePanel">
                                            <div class="container" style="height: 140px; overflow-x: hidden; overflow-y: scroll">
                                                <asp:GridView ID="gvOverdue" runat="server" CssClass="styleInfoLabel" Width="97%"
                                                    AutoGenerateColumns="False" DataKeyNames="LOB_ID" >
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLobID" runat="server" Text='<%# Eval("LOB_ID") %>' /></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Line of Business">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLOB" runat="server" Text='<%# Eval("LOB") %>' ToolTip="Line of Business" />
                                                                <asp:CheckBox runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Active")))%>'
                                                                    ID="chkActive" Visible="false" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                            <ItemStyle Width="30%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Denominator Days">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDenoDays" runat="server" Width="90px" MaxLength="3" Text='<%# Eval("Parameter_Value") %>'
                                                                    Style="text-align: right" ToolTip="Denominator Days Like 360, 365, 366 etc"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtDenoDays"
                                                                    FilterType="Numbers" ValidChars="" Enabled="True">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                            <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Grace Days">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtGraceDays" runat="server" Width="90px" MaxLength="2" Text='<%# Eval("Parameter_Value1") %>'
                                                                    Style="text-align: right" ToolTip="Grace Days"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtGraceDays"
                                                                    FilterType="Numbers" ValidChars="" Enabled="True">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                            <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ODI Rate">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtODIRate" runat="server" Width="90px" MaxLength="5" Text='<%# Eval("Parameter_Value2") %>'
                                                                    onkeypress="fnAllowNumbersOnly(true,false,this)" ToolTip="ODI Rate"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                            <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                                    <EmptyDataTemplate>
                                                        <span>No Records Found...</span>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 50%;">
                                                    <asp:Panel GroupingText="Collateral Transaction Type" ID="Panel9" runat="server"
                                                        ToolTip="Collateral Transaction Type" CssClass="stylePanel">
                                                        <table width="100%">
                                                            <tr>
                                                                <td width="25%">
                                                                    <asp:Label ID="lblCollateral" runat="server" Text="Margin Percentage" CssClass="styleReqFieldLabel" />
                                                                </td>
                                                                <td width="25%">
                                                                    <asp:TextBox ID="txtCollateral" runat="server" Width="100px" MaxLength="2" Style="text-align: right"
                                                                        ToolTip="Collateral Type" ValidationGroup="btnSave"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtCollateral"
                                                                        FilterType="Numbers" Enabled="True">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:RequiredFieldValidator ID="rfvCollateral" runat="server" Display="None" Enabled="false" ErrorMessage="Enter the Margin Percentage"
                                                                        ControlToValidate="txtCollateral" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                                <td style="width: 50%;">
                                                    <asp:Panel GroupingText="Operating Lease Depreciation" ID="tpBaseValue" runat="server"
                                                        ToolTip="Operating Lease Depreciation" CssClass="stylePanel">
                                                        <table width="100%">
                                                            <tr>
                                                                <td width="25%">
                                                                    <asp:Label ID="lbOperatiing" runat="server" Text="Base Value of Asset" CssClass="styleDisplayLabel" />
                                                                </td>
                                                                <td width="25%">
                                                                    <asp:TextBox ID="txtOperating" runat="server" Width="100px" MaxLength="7" Style="text-align: right"
                                                                        ToolTip="Base Value of Asset"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtOperating"
                                                                        FilterType="Numbers" Enabled="True">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <%--<asp:RequiredFieldValidator ID="rfvBase" runat="server" Display="None" ErrorMessage="Enter the Base Value"
                                                                        ControlToValidate="txtOperating"></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel GroupingText="Current Lending Rate" ToolTip="Current Lending Rate" ID="Panel5"
                                                        runat="server" CssClass="stylePanel">
                                                        <table width="100%">
                                                            <tr>
                                                                <td width="25%">
                                                                    <asp:Label ID="lblCurrentRate" runat="server" Text="Current Lending Rate" CssClass="styleReqFieldLabel" />
                                                                </td>
                                                                <td width="25%">
                                                                    <asp:TextBox ID="txtCurrentRate" runat="server" Width="100px" MaxLength="7" ToolTip="Current Lending Rate"
                                                                        onkeypress="fnAllowNumbersOnly(true,false,this)" ValidationGroup="btnSave" />
                                                                    <asp:RequiredFieldValidator ID="rfvCurrent" runat="server" Display="None" ErrorMessage="Enter the Current Lending Rate" Enabled="false"
                                                                        ControlToValidate="txtCurrentRate" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                                <td style="width: 50%;">
                                                    <asp:Panel GroupingText="Factoring Invoice gap days" ID="pnlGeneral" runat="server"
                                                        ToolTip="Factoring Invoice gap days" CssClass="stylePanel">
                                                        <table width="100%">
                                                            <tr>
                                                                <td width="25%">
                                                                    <asp:Label ID="lblFactory" runat="server" Text="Factoring Invoice gap days" CssClass="styleDisplayLabel" />
                                                                </td>
                                                                <td width="25%">
                                                                    <asp:TextBox ID="txtFactory" runat="server" Width="100px" MaxLength="3" Style="text-align: right"
                                                                        ToolTip="Factoring Invoice gap days"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtFactory"
                                                                        FilterType="Numbers" Enabled="True">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <%--  <asp:RequiredFieldValidator ID="rfvFactory" runat="server" Display="None" ErrorMessage="Enter the Factory Invoice gap days"
                                                                        ControlToValidate="txtFactory"></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" HeaderText="General" CssClass="tabpan" ID="tpGeneral"
                                    BackColor="Red" Visible="false">
                                    <HeaderTemplate>
                                        General
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel GroupingText="General" ID="tpGen" runat="server" CssClass="stylePanel">
                                            <table width="50%">
                                                <tr>
                                                    <td width="25%">
                                                        <asp:Label ID="lblODI" runat="server" Text="ODI Denominator Days" CssClass="styleReqFieldLabel" />
                                                    </td>
                                                    <td width="25%">
                                                        <asp:DropDownList ID="ddlOD1" runat="server" Width="105px" ToolTip="ODI Denominator Days">
                                                        </asp:DropDownList>
                                                        <%--<asp:RequiredFieldValidator ID="rfvODI" runat="server" Display="None" ErrorMessage="Select the ODI Denominator Days"
                                                            ControlToValidate="ddlOD1" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="tpAccountPreClosure" runat="server">
                                    <HeaderTemplate>
                                        Account Closure
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 50%;">
                                                    <asp:Panel GroupingText="Acc Preclosure Gap Days b/w the PMC Request & Approval"
                                                        ToolTip="Account Preclosure Gap Days b/w the PMC Request & Approval" ID="pnlGapDaysBWPMCDateandAppDate"
                                                        runat="server" CssClass="stylePanel">
                                                        <div class="container" style="height: 177px; width: 100%; overflow-x: hidden; overflow-y: scroll">
                                                            <asp:GridView ID="gvGapDaysBWPMCDate" runat="server" CssClass="styleInfoLabel" Width="94%"
                                                                AutoGenerateColumns="False" DataKeyNames="LOB_ID" OnRowDataBound="gvGapDaysBWPMCDate_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLobID" runat="server" Text='<%# Eval("LOB_ID") %>' /></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Line of Business">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLOB" runat="server" Text='<%# Eval("LOB") %>' ToolTip="Line of Business" />
                                                                            <asp:CheckBox runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Active")))%>'
                                                                                ID="chkActive" Visible="false" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="30%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Days">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtDays" runat="server" Width="80px" MaxLength="2" Text='<%# Eval("Parameter_Value") %>'
                                                                                Style="text-align: right" ToolTip="Gap Days"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="ftbeGapDays" runat="server" TargetControlID="txtDays"
                                                                                FilterType="Numbers" ValidChars="" Enabled="True">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <span>No Records Found...</span>
                                                                </EmptyDataTemplate>
                                                                <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </td>
                                                <td style="width: 50%;">
                                                    <asp:Panel GroupingText="Account PreClosure Type" ToolTip="Account PreClosure Type"
                                                        ID="pnlAccountPreClosureType" runat="server" CssClass="stylePanel">
                                                        <div class="container" style="height: 177px; width: 100%; overflow-x: hidden; overflow-y: scroll">
                                                            <asp:GridView ID="gvAccountPreClosureType" runat="server" CssClass="styleInfoLabel"
                                                                Width="94%" AutoGenerateColumns="False" DataKeyNames="LOB_ID" OnRowDataBound="gvAccountPreClosureType_RowDataBound"
                                                                OnRowCreated="gvAccountPreClosureType_RowCreated">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLobID" runat="server" Text='<%# Eval("LOB_ID") %>' /></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Line of Business">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLOB" runat="server" Text='<%# Eval("LOB") %>' ToolTip="Line of Business" />
                                                                            <asp:CheckBox runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Active")))%>'
                                                                                ID="chkActive" Visible="false" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="40%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="IRR">
                                                                        <ItemTemplate>
                                                                            <%--<asp:CheckBox runat="server" Checked='<%# Eval("Parameter_Value")%>' ID="chkIRR_Type" />--%>
                                                                            <asp:CheckBox runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Parameter_Value")))%>  '
                                                                                ID="chkIRR_Type" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="11%" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="NPV (Net Present Value)">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Parameter_Value1")))%> '
                                                                                ID="chkNPV_Type" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="17%" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Principal">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Parameter_Value2")))%> '
                                                                                ID="chkPrinciple_Type" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="12%" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Principal Interest Rate">
                                                                        <%--Added by Tamilselvan.S on 26/04/2011 for Principal Interest Rate --%>
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtPrincipleInterestRate" Width="50px" Style="text-align: right;"
                                                                                Text='<%# Eval("Parameter_Value3")%>' runat="server" MaxLength="5" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                                                ToolTip="Principal Interest Rate"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="ftbePrincipleInterestRate" runat="server" TargetControlID="txtPrincipleInterestRate"
                                                                                FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="18%" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <span>No Records Found...</span>
                                                                </EmptyDataTemplate>
                                                                <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 50%">
                                                    <asp:Panel GroupingText="Account closure Waived off amount" ToolTip="Account closure Waived off amount"
                                                        ID="pnlClosureWaiedoff" runat="server" CssClass="stylePanel">
                                                        <table>
                                                            <tr>
                                                                <td width="25%">
                                                                    <span class="styleReqFieldLabel">Waived off Amount</span>
                                                                </td>
                                                                <td width="25%">
                                                                    <asp:TextBox ID="txtWaivedOffAmount" runat="server" Style="text-align: right;" MaxLength="4"
                                                                        ValidationGroup="btnSave"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftbeWaivedOffAmount" runat="server" TargetControlID="txtWaivedOffAmount"
                                                                        FilterType="Numbers" Enabled="True">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:RequiredFieldValidator ID="rfvWaivedOffAmount" runat="server" Display="None"
                                                                        ErrorMessage="Enter the Waived off Amount" ControlToValidate="txtWaivedOffAmount"
                                                                        ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="tpAccConsolidationNSplit" runat="server" Visible="false">
                                    <HeaderTemplate>
                                        Account Consolidation and Split
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 50%;">
                                                    <asp:Panel GroupingText="Minimum No of Installment Eligible for Acc Consolidation"
                                                        ToolTip="Minimum No of Installment Eligible for Account Consolidation" ID="pnlFutureInstallments"
                                                        runat="server" CssClass="stylePanel">
                                                        <div class="container" style="height: 177px; width: 100%; overflow-x: hidden; overflow-y: scroll">
                                                            <asp:GridView ID="gvFutureInstallments" runat="server" CssClass="styleInfoLabel"
                                                                Width="95%" AutoGenerateColumns="False" DataKeyNames="LOB_ID" >
                                                                <Columns>
                                                                    <asp:TemplateField Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLobID" runat="server" Text='<%# Eval("LOB_ID") %>' /></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Line of Business">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLOB" runat="server" Text='<%# Eval("LOB") %>' ToolTip="Line of Business" />
                                                                            <asp:CheckBox runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Active")))%>'
                                                                                ID="chkActive" Visible="false" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="30%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="No of Installments">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtInstallment" Width="80px" runat="server" MaxLength="2" Text='<%# Eval("Parameter_Value") %>'
                                                                                Style="text-align: right" ToolTip="No of Installments"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="ftbeGapDays" runat="server" TargetControlID="txtInstallment"
                                                                                FilterType="Numbers" ValidChars="" Enabled="True">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                                                <EmptyDataTemplate>
                                                                    <span>No Records Found...</span>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </td>
                                                <td style="width: 50%;">
                                                    <asp:Panel GroupingText="Minimum Time for Account Split accepted" ToolTip="Minimum Time for Account Split accepted"
                                                        ID="Panel7" runat="server" CssClass="stylePanel">
                                                        <div class="container" style="height: 177px; width: 100%; overflow-x: hidden; overflow-y: scroll">
                                                            <asp:GridView ID="gvSplit" runat="server" CssClass="styleInfoLabel" Width="95%" AutoGenerateColumns="False"
                                                                DataKeyNames="LOB_ID" >
                                                                <Columns>
                                                                    <asp:TemplateField Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLobID" runat="server" Text='<%# Eval("LOB_ID") %>' /></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Line of Business">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLOB" runat="server" Text='<%# Eval("LOB") %>' ToolTip="Line of Business" />
                                                                            <asp:CheckBox runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Active")))%>'
                                                                                ID="chkActive" Visible="false" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="30%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Days">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtSplit" runat="server" Width="80px" MaxLength="2" Text='<%# Eval("Parameter_Value") %>'
                                                                                Style="text-align: right" ToolTip="No of Split Days"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="ftbeGapDays" runat="server" TargetControlID="txtSplit"
                                                                                FilterType="Numbers" ValidChars="" Enabled="True">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                                                <EmptyDataTemplate>
                                                                    <span>No Records Found...</span>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="margin-top: 10px; margin-bottom: 10px; margin-left: 10px;">
                            <asp:ValidationSummary ID="vsGPSAccount" runat="server" ValidationGroup="vgGPSAccount"
                                HeaderText="Correct the following validation(s): " />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div style="margin-top: 10px; margin-bottom: 10px;">
                            <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                                OnClientClick="return fnCheckPageValidators('btnSave');" ValidationGroup="btnSave"
                                OnClick="btnSave_Click" ToolTip="Save" CausesValidation="true" />
                            &nbsp;
                            <asp:Button runat="server" ID="btnClear" CssClass="styleSubmitButton" Text="Clear"
                                CausesValidation="False" OnClick="btnClear_Click" ToolTip="Clear" />
                            <cc1:ConfirmButtonExtender ID="btnAssetClear_ConfirmButtonExtender" runat="server"
                                ConfirmText="Do you want to Clear?" Enabled="True" TargetControlID="btnClear">
                            </cc1:ConfirmButtonExtender>
                            &nbsp;
                            <asp:Button runat="server" ID="btnCancel" CssClass="styleSubmitButton" CausesValidation="False"
                                Text="Cancel" OnClick="btnCancel_Click" ToolTip="Cancel" />
                            <asp:Button ID="btnResetMessage" runat="server" Style="display: none;" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsGlobal" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  " ValidationGroup="btnSave" />
                        <asp:CustomValidator ID="cvGlobal" ValidationGroup="btnSave" Display="None" runat="server" CssClass="styleMandatoryLabel"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" Style="color: Red; font-size: medium"></asp:Label>
                        <input id="hdnGlobalParamId" type="hidden" runat="server" value="0" />
                        <input id="hdnUserId" type="hidden" runat="server" value="0" />
                        <input id="hdnUserLevelId" type="hidden" runat="server" value="0" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnResetMessage" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
