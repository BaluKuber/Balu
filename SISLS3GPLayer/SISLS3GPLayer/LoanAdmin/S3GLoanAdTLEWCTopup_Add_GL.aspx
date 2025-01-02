<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GLoanAdTLEWCTopup_Add_GL.aspx.cs" Inherits="LoanAdmin_S3GLoanAdTLEWCTopup_Add_GL" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/S3GCustomerAddress.ascx" TagName="CustomerDetails"
    TagPrefix="CD" %>
    <%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function fnLoadCustomer() {
            document.getElementById('<%=btnGetInterest.ClientID%>').click();
        }
                 
        var querymode;
        querymode=location.search.split("qsMode=");
        querymode=querymode[1];
        var tab ;
    var index=0;
    function pageLoad()
    {
        tab = $find('ctl00_ContentPlaceHolder1_tcPLE');
        querymode=location.search.split("qsMode=");
        querypopup=location.search.split("Popup=");
        querymode=querymode[1];  
        querypopup=querypopup[1];        

        if(querymode!='Q' && querypopup!='yes')
        {
            tab.add_activeTabChanged(on_Change);
            var newindex=tab.get_activeTabIndex(index);
            var btnSave = document.getElementById('<%=btnSave.ClientID %>');
            var btnclear = document.getElementById('<%=btnClear.ClientID %>');
            if(newindex == tab._tabs.length-1)
                btnSave.disabled=false;
            else                      
                btnSave.disabled=true;
        }
        else if (querypopup='yes')
        {
          var btnSave = document.getElementById('<%=btnSave.ClientID %>');
         btnSave.disabled=true;
        }
    }
            
    function on_Change(sender,e)
    {
        tab = $find('ctl00_ContentPlaceHolder1_tcPLE');
        querymode=location.search.split("qsMode=");
         querypopup=location.search.split("Popup=");
        querymode=querymode[1];  
        querypopup=querypopup[1];        
                if(querymode!='Q' && querypopup!='yes')
                {
                    var strValgrp = tab._tabs[index]._tab.outerText.trim();
                    var newindex=tab.get_activeTabIndex(index);
                    var txtStatus = document.getElementById('ctl00_ContentPlaceHolder1_tcPLE_tpcust_txtCurrent');
                    var status = txtStatus.value;   
                    var btnSave = document.getElementById('<%=btnSave.ClientID %>');

                      if(newindex == tab._tabs.length - 1)
                        btnSave.disabled=false;
                      else
                        btnSave.disabled=true;     
                    if(newindex >index)
                    {
                        if(!fnCheckPageValidators(strValgrp,false))
                        {
                            tab.set_activeTabIndex(index);  
                           
                        }
                        else
                        {                
                            switch(index)
                            {
                                case 0:
                                    if(status=='' || status.length==0)    
                                    {
                                        tab.set_activeTabIndex(index); 
                                        alert('The current finance required amount cannot be empty.');// should be greater than zero.'); 
                                    }
                                    else if(status ==0)
                                    {
                                        tab.set_activeTabIndex(index);                             
                                        alert('The current finance required should be greater than zero.'); 
                                    }
                                    else
                                    {
                                         document.getElementById('<%=btnGetInterest.ClientID%>').click();
                                    }
                                    break;                    
                                case 1:
                                    break;
                                default:
                                    index=tab.get_activeTabIndex(index);
                                  break;
                            }
                        }
                    }
                    else
                    {           
                         tab.set_activeTabIndex(newindex);
                         index=tab.get_activeTabIndex(newindex);
                    }
        }
        else if (querypopup='yes')
         {
          var btnSave1 = document.getElementById('<%=btnSave.ClientID %>');
              btnSave1.disabled=false;
        
         }
    }
    </script>

    <%-- ctl00_ContentPlaceHolder1_tcPLE_tpcust_btnGetInterest  --%>
    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
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
                    <td style="padding-top: 10px;">
                        <div style="margin-left: 2px; margin-right: 2px;">
                            <cc1:TabContainer ID="tcPLE" runat="server" CssClass="styleTabPanel" AutoPostBack="true"
                                ActiveTabIndex="0">
                                <cc1:TabPanel runat="server" HeaderText="Customer" CssClass="tabpan" ID="tpcust"
                                    BackColor="Red">
                                    <HeaderTemplate>
                                        Customer Details
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel GroupingText="Customer Information" ID="pnlCustomerInfo" runat="server"
                                            CssClass="stylePanel">
                                            <table width="100%">
                                                <tr>
                                                    <td class="styleFieldLabel" width="20%">
                                                        <asp:Label ID="lblLob" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" width="25%">
                                                        <asp:DropDownList ID="ddlLOB" runat="server" Width="205px" ToolTip="Line of Business"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvLOB" runat="server" Display="None" InitialValue="0"
                                                            ErrorMessage="Select the Line of Business" ValidationGroup="btnSave" ControlToValidate="ddlLOB"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td width="5%">
                                                    </td>
                                                    <td class="styleFieldLabel" width="20%">
                                                        <asp:Label ID="lblDate" runat="server" Text="TL Topup Date" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" width="25%">
                                                        <asp:TextBox ID="txtDate" runat="server" Width="200px" ToolTip="TL Topup Date"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel" width="20%">
                                                        <asp:Label ID="lblBranch" runat="server" Text="Location" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" width="25%">
                                                               <uc2:Suggest ID="ddlBranch" runat="server"  ToolTip="Location" ServiceMethod="GetBranchList"
                                                            AutoPostBack="true" OnItem_Selected="ddlBranch_SelectedIndexChanged" ValidationGroup="btnSave"
                                                            ErrorMessage="Select a Location" IsMandatory="true" />
                                                    </td>
                                                    <td width="5%">
                                                    </td>
                                                    <td class="styleFieldLabel" width="10%">
                                                        <asp:Label ID="lblTLENo" runat="server" Text="TL Topup Number" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" width="25%">
                                                        <asp:TextBox ID="txtTLEWC" runat="server" Width="200px" ReadOnly="True" ToolTip="TL Topup Number"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel" width="20%">
                                                        <asp:Label ID="lblMLA" runat="server" Text="Prime Account No." CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" width="25%">
                                                        <asp:DropDownList ID="ddlMLA" runat="server" Width="205px" AutoPostBack="True" ToolTip="Prime Account No."
                                                            OnSelectedIndexChanged="ddlMLA_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvMLA" runat="server" Display="None" InitialValue="0"
                                                            ErrorMessage="Select the Prime Account Number" ValidationGroup="btnSave" ControlToValidate="ddlMLA"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td width="5%">
                                                    </td>
                                                    <td class="styleFieldLabel" width="10%">
                                                        <asp:Label ID="lblAccountDate" runat="server" Text="Account Date" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" width="25%">
                                                        <asp:TextBox ID="txtAccDate" runat="server" Width="200px" ReadOnly="True" ToolTip="Account Date"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel" width="20%">
                                                        <asp:Label ID="lblSLA" runat="server" Text="Sub Account No." CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" width="25%">
                                                        <asp:DropDownList ID="ddlSLA" runat="server" Width="205px" ToolTip="Sub Account No."
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlSLA_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvSLA" runat="server" Display="None" ValidationGroup="btnSave"
                                                            InitialValue="0" ErrorMessage="Select the Sub Account Number" ControlToValidate="ddlSLA"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td width="5%">
                                                    </td>
                                                    <td class="styleFieldLabel" width="20%">
                                                        <asp:Label ID="lblTopupstatus" runat="server" Text="TopUp Status" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" width="25%">
                                                        <asp:TextBox ID="txtTopupStatus" runat="server" Width="200px" ToolTip="TopUp Status"
                                                            ReadOnly="True" Text="Pending"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                 <td class="styleFieldLabel" width="20%">
                                                        <span class="styleDisplayLabel">Sanctioned Amount</span>
                                                    </td>
                                                    <td class="styleFieldAlign" width="25%">
                                                        <asp:TextBox ID="txtSanctionedAmount" runat="server" Width="200px" ToolTip="Sanctioned Amount"
                                                            Style="text-align: right;" onblur="ChkIsZero(this,'Sanctioned Amount');"></asp:TextBox>
                                                        <asp:RangeValidator ID="rvSanctionedAmount" runat="server" ControlToValidate="txtSanctionedAmount"
                                                            Display="None" MinimumValue="1" MaximumValue="1000000" ErrorMessage="Current Finance Required should be less than or Equal to Sanctioned Amount"></asp:RangeValidator>
                                                    </td>
                                                   
                                                    <td width="5%">
                                                    </td>
                                                   <td class="styleFieldLabel" width="20%">
                                                        <asp:Label ID="lblCollateral" runat="server" Text="Collateral Exists" CssClass="styleDisplayLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" width="25%">
                                                        <asp:DropDownList ID="ddlCollateral" runat="server" Width="205px" ToolTip="Collateral Exists"
                                                            AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                   
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel" width="20%">
                                                        <span>Existing Finance Amount </span>
                                                    </td>
                                                    <td class="styleFieldAlign" width="25%">
                                                        <asp:TextBox ID="txtExistingFinanceAmount" runat="server" Width="200px" ToolTip="Existing Finance Amount"
                                                            Style="text-align: right;" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td width="5%">
                                                    </td>
                                                  <td class="styleFieldLabel" width="20%">
                                                        <span>Stages</span>
                                                    </td>
                                                    <td class="styleFieldAlign" width="25%">
                                                        <asp:TextBox ID="txtStages" runat="server" Width="200px" ToolTip="Stages"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                 <td class="styleFieldLabel" width="20%">
                                                        <asp:Label ID="lblCurrent" runat="server" Text="Current Finance Required" CssClass="styleReqFieldLabel"></asp:Label>
                                                        <asp:HiddenField ID="hdnCurrentFinReq" runat="server" />
                                                    </td>
                                                    <td class="styleFieldAlign" width="25%">
                                                        <asp:TextBox ID="txtCurrent" runat="server" Width="200px" ToolTip="Current Finance Required"
                                                            MaxLength="6" Style="text-align: right;" onblur="ChkIsZero(this,'Current Finance Required');"></asp:TextBox>
                                                        <asp:Button ID="btnGetInterest" runat="server" Style="display: none" Text="Calculate Interest"
                                                            OnClick="btnGetInterest_OnClick" CausesValidation="False" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtCurrent"
                                                            FilterType="Numbers" Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvCurrenctFinanceReq" ValidationGroup="btnSave"
                                                            runat="server" Display="None" ErrorMessage="Enter the Current Finance Required"
                                                            ControlToValidate="txtCurrent" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:HiddenField ID="hdnExistingAmount" runat="server" />
                                                    </td>
                                                 <td width="5%">
                                                    </td>
                                                     <td class="styleFieldLabel" width="20%">
                                                        <span>Remarks</span>
                                                    </td>
                                                    <td class="styleFieldAlign" width="25%">
                                                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="200px" 
                                                            ToolTip="Stages" MaxLength="200"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel GroupingText="Customer Address" ID="Panel1" runat="server" CssClass="stylePanel">
                                            <div style="margin-bottom: 10px;">
                                                <CD:CustomerDetails ID="ucdCustomer" runat="server" ActiveViewIndex="1" FirstColumnWidth="18%"
                                                    SecondColumnWidth="26%" ThirdColumnWidth="18%" FourthColumnWidth="22%" />
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" HeaderText="Customer" CssClass="tabpan" ID="tpRepay"
                                    BackColor="Red">
                                    <HeaderTemplate>
                                        Repay Structure
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="upRepayStructure" runat="server">
                                            <ContentTemplate>
                                                <asp:Panel GroupingText="Existing Details" ID="Panel2" runat="server" CssClass="stylePanel">
                                                    <div class="container" style="height: 100px; width: 100%; overflow-x: auto; overflow-y: auto;">
                                                        <asp:GridView ID="gvExisting" runat="server" AutoGenerateColumns="False" BorderColor="Gray"
                                                            CssClass="styleInfoLabel">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="SI.No.">
                                                                    <ItemTemplate>
                                                                        <%# Container.DataItemIndex + 1 %>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Finance Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFinanceAmount" Width="180px" ToolTip="Finance Amount" runat="server"
                                                                            Text='<%# Bind("Finance_Amount") %>' Style="text-align: right;"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Installment Date">
                                                                    <ItemTemplate>
                                                                        <%# FormatDate(Eval("Installment_Date").ToString())%>
                                                                        <asp:Label ID="lblInsDate" Visible="false" Width="180px" runat="server" Text='<%# Bind("Installment_Date", "{0:MM-dd-yyyy}") %>'
                                                                            Style="text-align: left;"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Monthly Installment">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMonthlyIns" ToolTip="Monthly Installment" Width="180px" runat="server"
                                                                            Text='<%# Bind("Installment_Amount") %>' Style="text-align: right;"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Status" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPaidStatus" ToolTip="Paid Status" Width="180px" runat="server"
                                                                            Text='<%# Bind("PaidStatus") %>' Style="text-align: left;"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel GroupingText="New Structure" ID="Panel4" runat="server" CssClass="stylePanel">
                                                    <table width="90%">
                                                        <tr>
                                                            <td class="styleFieldLabel" width="20%">
                                                                <asp:Label ID="lblNextLiner" runat="server" Text="Next Liner" CssClass="styleDisplayLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" width="20%">
                                                                <asp:TextBox ID="txtNextLiner" ToolTip="Next Liner" ValidationGroup="btnGo" runat="server"
                                                                    MaxLength="3" Style="text-align: right;" onblur="ChkIsZero(this,'Next Liner')"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtNextLiner"
                                                                    FilterType="Numbers" Enabled="True" ValidChars=".">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:RangeValidator ID="rvNextLiner" runat="server" ErrorMessage="Enter the Next Liner should be Less than Installment"
                                                                    SetFocusOnError="true" Display="None" ControlToValidate="txtNextLiner" MinimumValue="1"
                                                                    ValidationGroup="btnGo" Type="Integer"></asp:RangeValidator>
                                                                <asp:RequiredFieldValidator ID="rfvNextLiner" runat="server" ControlToValidate="txtNextLiner"
                                                                    ErrorMessage="Enter the Next Liner " SetFocusOnError="true" ValidationGroup="btnGo"
                                                                    Display="None">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="styleFieldLabel" width="20%">
                                                                <asp:Label ID="lblInstallmentAmount" runat="server" Text="Installment Amount" CssClass="styleDisplayLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" width="20%">
                                                                <asp:TextBox ID="txtInstallmentAmount" ToolTip="Installment Amount" ValidationGroup="btnGo"
                                                                    onblur="ChkIsZero(this,'Installment Amount')" runat="server" MaxLength="12" Style="text-align: right;"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtInstallmentAmount"
                                                                    FilterType="Numbers" Enabled="True" ValidChars=".">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="rfvInstallmentAmount" runat="server" ControlToValidate="txtInstallmentAmount"
                                                                    ErrorMessage="Enter the Installment Amount" SetFocusOnError="true" ValidationGroup="btnGo"
                                                                    Display="None">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td width="5%">
                                                                <asp:Button runat="server" ID="btnGO" ValidationGroup="btnGo" CssClass="styleSubmitShortButton"
                                                                    Text="GO" ToolTip="Go" CausesValidation="true" OnClick="btnGO_Click" Width="50px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div class="container" style="height: 150px; width: 100%; overflow-x: auto; overflow-y: auto;">
                                                        <asp:GridView ID="gvNewStructure" runat="server" AutoGenerateColumns="False" BorderColor="Gray"
                                                            CssClass="styleInfoLabel" OnRowDataBound="gvNewStructure_RowDataBound" FooterStyle-Wrap="true"
                                                            HeaderStyle-Wrap="true" ShowFooter="true">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="CbxChange" runat="server" ToolTip="Select" AutoPostBack="true"
                                                                            OnCheckedChanged="CbxChange_CheckedChanged" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Installment No">
                                                                    <ItemTemplate>
                                                                        <%--<%# Container.DataItemIndex + 1 %>--%>
                                                                        <asp:Label ID="lblInstallmentNo" Text='<%# Bind("InstallmentNo") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Installment Date">
                                                                    <ItemTemplate>
                                                                        <%# FormatDate(Eval("Installment_Date").ToString())%>
                                                                        <asp:Label Visible="false" ID="lblInsDate" Width="180px" Style="text-align: left;"
                                                                            runat="server" Text='<%# Bind("Installment_Date", "{0:MM-dd-yyyy}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Repayment Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRepaymentAmount" ToolTip="Repayment Amount" Width="180px" Style="text-align: right;"
                                                                            runat="server" Text='<%# Eval("RepaymentAmount") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="New Monthly Installments">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtNewMontly" ToolTip="New Monthly Installments" Style="text-align: right;"
                                                                            runat="server" widht="100%" AutoPostBack="false" Text='<%# Eval("Installment_Amount") %>'
                                                                            ReadOnly="false"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtNewMontly"
                                                                            FilterType="Numbers,custom" ValidChars="." Enabled="True">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblTotalAmount" runat="server" Style="text-align: right;"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFinanceCharges" runat="server" Text='<%# Eval("FinanceCharges") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPrincipalAmount" runat="server" Text='<%# Eval("PrincipalAmount") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblNoofDays" runat="server" Text='<%# Eval("NoofDays") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("FromDate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblToDate" runat="server" Text='<%# Eval("ToDate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGO" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="tpCollateralDetails" runat="server">
                                    <HeaderTemplate>
                                        Collateral Details
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 25%;">
                                                    <span>Collateral Exists </span>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 25%;">
                                                    <asp:CheckBoxList ID="chklCollateralExists" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                                        Enabled="False">
                                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </td>
                                                <td style="width: 50%;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" style="width: 100%;">
                                                    <asp:Panel GroupingText="Collateral Details" ID="pnlCollateralDetails" runat="server"
                                                        CssClass="stylePanel">
                                                        <div class="container" style="height: 100px; width: 100%; overflow-x: auto; overflow-y: auto;">
                                                            <asp:GridView ID="gvCollateralDetails" runat="server" AutoGenerateColumns="False"
                                                                Width="100%" OnRowDataBound="gvCollateralDetails_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="CollateralRefNo" HeaderText="Collateral Ref No">
                                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CollateralDescription" HeaderText="Collateral Description">
                                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CollateralValue" HeaderText="Collateral Value">
                                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Margin" HeaderText="Margin">
                                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                                    </asp:BoundField>
                                                                   <%-- <asp:BoundField DataField="SanctionedAmount" HeaderText="Sanctioned Amount">
                                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                                    </asp:BoundField>--%>
                                                                </Columns>
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
                    <td align="center">
                        <div style="margin-top: 10px;">
                            <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                                OnClientClick="return fnCheckPageValidators('btnSave');" ValidationGroup="btnSave"
                                OnClick="btnSave_Click" Enabled="false" />
                            &nbsp;
                            <asp:Button runat="server" ID="btnCancelTopup" CssClass="styleSubmitButton" Text="Cancel Topup"
                                OnClick="btnCancelTopup_Click" CausesValidation="False" />
                            <cc1:ConfirmButtonExtender ID="cbeCancelTopup" runat="server" ConfirmText="Do you want to Cancel the Topup?"
                                Enabled="True" TargetControlID="btnCancelTopup">
                            </cc1:ConfirmButtonExtender>
                            &nbsp; &nbsp;
                            <asp:Button runat="server" ID="btnClear" CssClass="styleSubmitButton" Text="Clear"
                                CausesValidation="False" OnClick="btnClear_Click" />
                            <cc1:ConfirmButtonExtender ID="btnAssetClear_ConfirmButtonExtender" runat="server"
                                ConfirmText="Do you want to Clear?" Enabled="True" TargetControlID="btnClear">
                            </cc1:ConfirmButtonExtender>
                            &nbsp;
                            <asp:Button runat="server" ID="btnCancel" CssClass="styleSubmitButton" CausesValidation="False"
                                Text="Cancel" OnClick="btnCancel_Click" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="margin-left: 20px;">
                        <div style="margin-left: 20px; margin-bottom: 20px;">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="styleMandatoryLabel"
                                ValidationGroup="btnGo" HeaderText="Correct the following validation(s):  " />
                            <asp:ValidationSummary ID="vsTLE" runat="server" CssClass="styleMandatoryLabel" ValidationGroup="btnSave"
                                HeaderText="Correct the following validation(s):  " />
                            <asp:CustomValidator ID="cvTLE" runat="server" CssClass="styleMandatoryLabel" Height="50px"></asp:CustomValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" Style="color: Red; font-size: medium"></asp:Label>
                        <asp:TextBox ID="txtHDDate" runat="server" widht="100%" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txtCustID" runat="server" widht="100%" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txtStatus" runat="server" widht="100%" Visible="false"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


