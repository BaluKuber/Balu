<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLRlRepossessionTrack.aspx.cs" Inherits="LegalRepossession_S3GLRlRepossessionTrack"
    Title="Untitled Page" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
    
    function pageLoad() 
    {       
       tab = $find('ctl00_ContentPlaceHolder1_tcRepossTrack');
       var querymode=location.search.split("qsMode=");
   
        if(querymode.length > 1)
        {
                   if(querymode[1].length>1)
                   {
                     querymode =querymode[1].split("&");
                     querymode=querymode[0];
                   }
                   else
                   {
                     querymode=querymode[1];
                   }
                   if(querymode!='Q')
                   {
                        tab.add_activeTabChanged(on_Change);
                   }
        }
       
    }
    
    
    var index=0;
      function on_Change(sender,e)
      {    
      
        var newindex=tab.get_activeTabIndex();
        
        if(newindex > index)          
        {
            switch (newindex)
            {
                case 1:
                    if(!Page_ClientValidate('tbGeneral'))
                    {                  
                        tab.set_activeTabIndex(0);
                        index=0;  
                        
                        break;                
                    }                 
                    break;     
              
                case 2:
                    if(!Page_ClientValidate('tbGeneral'))
                    {                  
                        tab.set_activeTabIndex(0);
                        index=0;  
                        
                        break;                
                    }                 
                    break; 
                      
                case 3 : 
                 
                  if(!Page_ClientValidate('tbGeneral'))
                  {                  
                      tab.set_activeTabIndex(0);
                      index=0;
                      
                      break;   
                  }
                   
                  else if(!Page_ClientValidate('tbTrack'))
                   {                   
                      tab.set_activeTabIndex(2);    
                      index = 2;
                     
                      break;                
                   }                  
                  break;                                         
              }          
          }     
//       else
//       {
//            tab.set_activeTabIndex(newindex);
//            index=tab.get_activeTabIndex(newindex);
//       }     
            
           Page_BlockSubmit=false;  
      
      }


    </script>

    <table id="Table1" width="100%" runat="server">
        <tr>
            <td>
                <table id="Table2" width="100%" runat="server">
                    <tr>
                        <td class="stylePageHeading">
                            <asp:Label runat="server" Text="Repossion Track " ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                        </td>
                    </tr>
                    <%-- <tr>
                        <td class="styleFieldAlign">
                            &nbsp;
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <table id="Table3" width="100%" runat="server">
                                <tr>
                                    <td>
                                        <cc1:TabContainer ID="tcRepossTrack" runat="server" CssClass="styleTabPanel" Width="99%"
                                            ScrollBars="None" ActiveTabIndex="2">
                                            <cc1:TabPanel runat="server" HeaderText="General" ID="tbgeneral" CssClass="tabpan"
                                                BackColor="Red" ToolTip="General" Width="99%">
                                                <HeaderTemplate>
                                                    General
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <table id="Table4" runat="server" width="100%">
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label runat="server" Text="Legal Repossession Number" ID="lblLRNno" CssClass="styleDisplayLabel"></asp:Label>
                                                                        <span class="styleMandatory">*</span>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:DropDownList ID="ddlLRNno" runat="server" AutoPostBack="true" ToolTip="Legal Repossession Number"
                                                                            OnSelectedIndexChanged="ddlLRNno_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvddlLRN" runat="server" ControlToValidate="ddlLRNno"
                                                                            SetFocusOnError="True" CssClass="styleMandatoryLabel" Display="None" InitialValue="0"
                                                                            ErrorMessage="Select Legal Repossession Number" ValidationGroup="tbGeneral"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlLRNno"
                                                                            SetFocusOnError="True" CssClass="styleMandatoryLabel" Display="None" InitialValue="0"
                                                                            ErrorMessage="Select Legal Repossession Number" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label runat="server" Text="Action" ID="lblAction" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtAction" runat="server" ContentEditable="False" ToolTip="Action"
                                                                            Width="100px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label runat="server" Text="Repossession Docket Number" ID="lblDocketno" CssClass="styleDisplayLabel"></asp:Label>
                                                                        <span class="styleMandatory">*</span>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:DropDownList ID="ddlDocketno" runat="server" AutoPostBack="true" ToolTip="Legal Docket Number"
                                                                            OnSelectedIndexChanged="ddlDocketno_SelectedIndexChanged">
                                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvddlDocketno" runat="server" ControlToValidate="ddlDocketno"
                                                                            SetFocusOnError="True" CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Repossession Docket Number"
                                                                            InitialValue="0" ValidationGroup="tbGeneral"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlDocketno"
                                                                            SetFocusOnError="True" CssClass="styleMandatoryLabel" Display="None" InitialValue="0"
                                                                            ErrorMessage="Select Repossession Docket Number" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label runat="server" Text="Repossession Docket Date" ID="lblDocketdate" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtDocketdate" runat="server" ContentEditable="False" Width="80px"
                                                                            ToolTip="Repossession Docket Date"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label runat="server" Text="Repossession Track Number" ID="Label3" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtTrackNo" runat="server" Width="120px" ContentEditable="False"
                                                                            ToolTip="Repossession Track Number"></asp:TextBox>
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label runat="server" Text="Repossession Track Date" ID="Label4" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtTrackDate" runat="server" ContentEditable="False" Width="80px"
                                                                            ToolTip="Repossession Track Date"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtLOB" runat="server" ContentEditable="False" ToolTip="Line of Business"
                                                                            Width="120px"></asp:TextBox>
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label runat="server" Text="Location" ID="lblBranch" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtBranch" runat="server" ContentEditable="False" ToolTip="Location"
                                                                            Width="120px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblPANum" runat="server" Text="Prime Account Number" CssClass="styleDisplayLabel"></asp:Label>
                                                                        <%--<span class="styleMandatory">*</span>--%>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtPANum" runat="server" ContentEditable="False" ToolTip="Prime Account Number"
                                                                            Width="135px"> </asp:TextBox>
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblSLA" runat="server" Text="Sub Account Number" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtSANum" runat="server" ContentEditable="False" ToolTip="Sub Account Number"
                                                                            Width="120px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label runat="server" Text="Account Date" ID="lblAccountDate" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtAcconutDate" runat="server" ContentEditable="False" ToolTip="Account Date"
                                                                            Width="80px"></asp:TextBox>
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label runat="server" Text="Amount Financed" ID="Label2" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtFinAmount" runat="server" ContentEditable="False" ToolTip="Amount Financed"
                                                                            Width="90px" Style="text-align: right"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label runat="server" Text="Tenure" ID="lblTenure" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtTenure" runat="server" ContentEditable="False" ToolTip="Tenure"
                                                                            Width="25px"></asp:TextBox>
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label runat="server" Text="View" ID="lblView" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Button ID="btnViewAccount" Enabled="false" runat="server" CssClass="styleGridShortButton"
                                                                            Text="Account" />
                                                                        <asp:Button ID="btnViewLedger" Enabled="false" runat="server" CssClass="styleGridShortButton"
                                                                            Text="Ledger" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4">
                                                                        <asp:Panel ID="pnlCust" runat="server" ToolTip="Customer Information" GroupingText="Customer Information"
                                                                            CssClass="stylePanel">
                                                                            <table id="Table5" width="100%" runat="server">
                                                                                <tr>
                                                                                    <td>
                                                                                        <uc1:S3GCustomerAddress ID="ucCustDetails" runat="server" FirstColumnWidth="25%"
                                                                                            SecondColumnWidth="30%" ThirdColumnWidth="18%" FourthColumnWidth="26%" FirstColumnStyle="styleFieldLabel"
                                                                                            ActiveViewIndex="1" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4">
                                                                        <asp:Panel ID="pnlGuarantor" runat="server" ToolTip="Guarantor Information" GroupingText="Guarantor Information"
                                                                            CssClass="stylePanel" Visible="false">
                                                                            <asp:GridView ID="grvGuarantor" runat="server" Width="100%" AutoGenerateColumns="false"
                                                                                HeaderStyle-CssClass="styleGridHeader">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="S.No" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %> '></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField HeaderText="Guarantor Code" DataField="Guarantor_Code" ItemStyle-HorizontalAlign="Left" />
                                                                                    <asp:BoundField HeaderText="Guarantor Name" DataField="Guarantor_Name" ItemStyle-HorizontalAlign="Left" />
                                                                                    <asp:BoundField HeaderText="Guarantor Address" DataField="Guarantor_Address1" ItemStyle-HorizontalAlign="Left" />
                                                                                    <asp:BoundField HeaderText="Guarantor Amount" DataField="Guarantee_Amount" ItemStyle-HorizontalAlign="Right" />
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="styleMandatoryLabel"
                                                                HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="tbGeneral" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel runat="server" HeaderText="Repossessed Asset Details" ID="tbAsset"
                                                CssClass="tabpan" BackColor="Red" ToolTip="Asset Details" Width="99%">
                                                <HeaderTemplate>
                                                    Repossessed Asset details
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:UpdatePanel ID="UpAssed" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="pnlAsset" Width="99%" runat="server" ToolTip="Repossessed Asset Information"
                                                                GroupingText="Repossessed Asset Information" CssClass="stylePanel">
                                                                <asp:GridView ID="grvAsset" Width="100%" runat="server" AutoGenerateColumns="false"
                                                                    HeaderStyle-CssClass="styleGridHeader">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="S.No" ItemStyle-Width="50px">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %> '></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAssetID" Visible="false" runat="server" Text='<%#Bind("AssetID") %> '></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Asset Code">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%#Bind("Asset_Code") %> '></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderText="Asset Description" DataField="Asset_Description" ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField HeaderText="Vehicle Registration Number" DataField="REGN_No" ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField HeaderText="Vehicle Serial Number" DataField="Serial_No" ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField HeaderText="Repossession date" DataField="Repossession_Date" ItemStyle-HorizontalAlign="Left" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlGarage" Width="99%" runat="server" ToolTip="Garage Information"
                                                                GroupingText="Garage Information" CssClass="stylePanel">
                                                                <table id="Table6" width="100%" runat="server">
                                                                    <tr>
                                                                        <td width="22%" class="styleFieldLabel">
                                                                            <asp:Label runat="server" Text="Garage Code " ID="lblGcode" CssClass="styleDisplayLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" width="27%">
                                                                            <asp:TextBox ID="txtGID" runat="server" Visible="false" ContentEditable="False" ToolTip=""></asp:TextBox>
                                                                            <asp:TextBox ID="txtGcode" runat="server" ContentEditable="False" ToolTip="Garage Code"
                                                                                Width="100px"></asp:TextBox>
                                                                        </td>
                                                                        <td class="styleFieldLabel" width="24%">
                                                                            <asp:Label runat="server" Text="Garage InDate" ID="lblGname" CssClass="styleDisplayLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtGIn" runat="server" ContentEditable="False" ToolTip="Garage InDate"
                                                                                Width="80px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel" valign="top">
                                                                            <asp:Label runat="server" Text="Garage Address " ID="lblGaddress" CssClass="styleDisplayLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" colspan="3">
                                                                            <asp:TextBox ID="txtGaddress" runat="server" ContentEditable="False" TextMode="MultiLine"
                                                                                ToolTip="Garage Address"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                            <table id="Table7" runat="server" width="100%">
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="22%" valign="top">
                                                                        <asp:Label runat="server" Text="Asset Inventory details" ID="lblAssetInventory" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" width="27%">
                                                                        <asp:TextBox ID="txtAssetInventory" runat="server" TextMode="MultiLine" ContentEditable="False"
                                                                            ToolTip="Asset Inventory details"></asp:TextBox>
                                                                    </td>
                                                                    <td class="styleFieldLabel" width="24%" valign="top">
                                                                        <asp:Label runat="server" Text="Insurance Validity Upto" ID="lblInsValid" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" colspan="3">
                                                                        <asp:TextBox ID="txtInsValid" runat="server" ContentEditable="False" Width="80px"
                                                                            ToolTip="Insurance Validity Upto"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel runat="server" HeaderText="Track Details" ID="tbTrack" CssClass="tabpan"
                                                BackColor="Red" ToolTip="Track Details" Width="99%">
                                                <HeaderTemplate>
                                                    Track Details
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:UpdatePanel ID="upTrack" runat="server">
                                                        <ContentTemplate>
                                                            <table id="tblInst" width="100%" runat="server">
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label runat="server" Text="Inspection done by" ID="tblInspecby" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButtonList runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                                                            ToolTip="Inspection done by" ID="RDInsBy" OnSelectedIndexChanged="RDInsBy_SelectedIndexChanged">
                                                                            <asp:ListItem Text="Agency" Value="1" Selected="True"></asp:ListItem>
                                                                            <asp:ListItem Text="Employee" Value="2"></asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Button ID="BtnNewInspection" runat="server" CssClass="styleSubmitButton" Text="New Inspection"
                                                                            OnClick="BtnNewInspection_Click" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <asp:Panel ID="pnlTrack" Width="100%" runat="server" ToolTip="Inspection Information"
                                                                            GroupingText="Asset Inspection Information" CssClass="stylePanel">
                                                                            <table id="tblTrack" width="100%" runat="server">
                                                                                <tr>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label runat="server" Text="Inspector Name" ID="lblInspecBy" CssClass="styleReqFieldLabel"></asp:Label>
                                                                                       
                                                                                    </td>
                                                                                    <td class="styleFieldAlign">
                                                                                        <asp:DropDownList ID="ddlInspecBy" runat="server" AutoPostBack="true" ToolTip="Inspector Name"
                                                                                            OnSelectedIndexChanged="ddlInspecBy_SelectedIndexChanged" Width="250px">
                                                                                         <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                        <asp:RequiredFieldValidator ID="rfvddlInspecBy" runat="server" ControlToValidate="ddlInspecBy"
                                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Inspector Name "
                                                                                            InitialValue="0" SetFocusOnError="True" ValidationGroup="tbTrack"></asp:RequiredFieldValidator>
                                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlInspecBy"
                                                                                            SetFocusOnError="True" CssClass="styleMandatoryLabel" Display="None" InitialValue="0"
                                                                                            ErrorMessage="Select Inspector Name" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label runat="server" Text="Inspected On " ID="lblInspecDate" CssClass="styleReqFieldLabel"></asp:Label>
                                                                                       
                                                                                    </td>
                                                                                    <td class="styleFieldAlign">
                                                                                        <asp:TextBox ID="txtInspecDate" runat="server" Width="80px" 
                                                                                            ToolTip="Inspector On" AutoPostBack="true" OnTextChanged="txtInspecDate_TextChanged"></asp:TextBox>
                                                                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtInspecDate"
                                                                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="Image1"
                                                                                            ID="CalendarExtender1" Enabled="True">
                                                                                        </cc1:CalendarExtender>
                                                                                        <asp:RequiredFieldValidator ID="rfvtxtInspecDate" runat="server" ControlToValidate="txtInspecDate"
                                                                                            CssClass="styleMandatoryLabel" ErrorMessage="Select Inspection On Date" Display="None"
                                                                                            SetFocusOnError="True" ValidationGroup="tbTrack"></asp:RequiredFieldValidator>
                                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtInspecDate"
                                                                                            CssClass="styleMandatoryLabel" ErrorMessage="Select Inspection On Date" Display="None"
                                                                                            SetFocusOnError="True" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                </tr>
                                                                                </table>
                                                                                <table id="Table8" width="100%" runat="server">
                                                                                <tr>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label runat="server" Style="font-weight: bold" Text="Asset Relocated Details:"
                                                                                            ID="Label1" CssClass="styleDisplayLabel"></asp:Label>
                                                                                        <br />
                                                                                    </td>
                                                                                    <td colspan="3" valign="bottom">
                                                                                        <%--<hr class="styleMainTable" />--%>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label runat="server" Text="New Garage Code" ID="lblGarageCode"  CssClass="styleReqFieldLabel"></asp:Label>
                                                                                    </td>
                                                                                    <td class="styleFieldAlign">
                                                                                        <asp:DropDownList ID="ddlNewGarage" runat="server" AutoPostBack="true" ToolTip="New Garage Code"
                                                                                            OnSelectedIndexChanged="ddlNewGarage_SelectedIndexChanged">
                                                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlNewGarage"
                                                                                            CssClass="styleMandatoryLabel" ErrorMessage="Select New Garage Code" Display="None"
                                                                                            InitialValue="0" SetFocusOnError="True" ValidationGroup="tbTrack"></asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label runat="server" Text="New Garage Name" ID="lblGarageName" CssClass="styleDisplayLabel"></asp:Label>
                                                                                    </td>
                                                                                    <td class="styleFieldAlign">
                                                                                        <asp:TextBox ID="txtGarageName" runat="server" ContentEditable="False" ToolTip="New Garage Name"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label runat="server" Text="New Garage Address" ID="lblGarageAddress" CssClass="styleDisplayLabel"></asp:Label>
                                                                                    </td>
                                                                                    <td class="styleFieldAlign">
                                                                                        <asp:TextBox ID="txtGarageAddress" runat="server" TextMode="MultiLine" ContentEditable="False"
                                                                                            ToolTip="New Garage Address"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label runat="server" Text="Remarks" ID="lblRemarks" CssClass="styleDisplayLabel"></asp:Label>
                                                                                    </td>
                                                                                    <td class="styleFieldAlign">
                                                                                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" onkeyup="maxlengthfortxt(200)"
                                                                                            ToolTip="Remarks"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label runat="server" Text="Old Garage Out Date" ID="lblOldGarageOut" CssClass="styleReqFieldLabel"></asp:Label>
                                                                                    </td>
                                                                                    <td class="styleFieldAlign">
                                                                                        <asp:TextBox ID="txtOldGarageOut" runat="server" AutoPostBack="true" 
                                                                                            ToolTip="Old Garage out Date" OnTextChanged="txtOldGarageOut_TextChanged" Width="80px"></asp:TextBox>
                                                                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtOldGarageOut"
                                                                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="Image2"
                                                                                            ID="CalendarExtender2">
                                                                                        </cc1:CalendarExtender>
                                                                                        <asp:RequiredFieldValidator ID="rfvOldout" runat="server" ControlToValidate="txtOldGarageOut"
                                                                                            Enabled="false" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                            ErrorMessage="Select Old Garage OutDate" ValidationGroup="tbTrack"></asp:RequiredFieldValidator>
                                                                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtOldGarageOut"
                                                                                            Enabled="true" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                            ErrorMessage="Select Old Garage OutDate" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label runat="server" Text="New Garage In date" ID="lblNewGarageIn" CssClass="styleReqFieldLabel"></asp:Label>
                                                                                    </td>
                                                                                    <td class="styleFieldAlign">
                                                                                        <asp:TextBox ID="txtNewGarageIn" runat="server" ToolTip="New Garage In date" 
                                                                                            AutoPostBack="true" OnTextChanged="txtNewGarageIn_TextChanged" Width="80px"></asp:TextBox>
                                                                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtNewGarageIn"
                                                                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="Image3"
                                                                                            ID="CalendarExtender3">
                                                                                        </cc1:CalendarExtender>
                                                                                        <asp:RequiredFieldValidator ID="rfvNewin" runat="server" ControlToValidate="txtNewGarageIn"
                                                                                            Enabled="false" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                            ErrorMessage="Select New Garage InDate" ValidationGroup="tbTrack"></asp:RequiredFieldValidator>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtNewGarageIn"
                                                                                            Enabled="true" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                            ErrorMessage="Select New Garage InDate" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label runat="server" Text="Condition of asset" ID="lblCondAsset" CssClass="styleDisplayLabel"></asp:Label>
                                                                                    </td>
                                                                                    <td class="styleFieldAlign" colspan="3">
                                                                                        <asp:TextBox ID="txtCondAsset" TextMode="MultiLine" runat="server" MaxLength="200"
                                                                                            onkeyup="maxlengthfortxt(200)" ToolTip="Condition of asset"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="styleMandatoryLabel"
                                                                HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="tbTrack" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel runat="server" HeaderText="History Details" ID="tbHistroy" CssClass="tabpan"
                                                BackColor="Red" ToolTip="History Details" Width="99%">
                                                <HeaderTemplate>
                                                    History Details
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:UpdatePanel ID="upHistory" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="GrvHistory" Width="100%" runat="server" AutoGenerateColumns="false"
                                                                HeaderStyle-CssClass="styleGridHeader">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="S.No" ItemStyle-Width="50px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %> '></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Repossession Track Number" DataField="LR_Track_No" ItemStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField HeaderText="Inspector Name" DataField="Inspector_Name" ItemStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField HeaderText="Inspected on" DataField="Inspected_Date" ItemStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField HeaderText="Repossession Track Date" DataField="Repossesion_Track_Date"
                                                                        ItemStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField HeaderText="New Garage Code" DataField="Garage_Code" ItemStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField HeaderText="New Garage Address" DataField="Address" ItemStyle-HorizontalAlign="Left" />
                                                                    <asp:TemplateField HeaderText="Remarks">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Bind("Remarks") %>' Style="text-align: Left"
                                                                                 Width="112px" TextMode="MultiLine" ReadOnly = "true"/>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                        </cc1:TabContainer>
                                        <asp:CustomValidator ID="cvTracks" runat="server" CssClass="styleMandatoryLabel"
                                            Enabled="true" Width="98%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnSave" runat="server" CausesValidation="true" CssClass="styleSubmitButton"
                                            ValidationGroup="Submit" Text="Save" OnClick="btnSave_Click" OnClientClick="return fnCheckPageValidators('Submit');"  />
                                        &nbsp;<asp:Button ID="btnClear" ToolTip="Clear Values" runat="server" CausesValidation="false"
                                            CssClass="styleSubmitButton" OnClientClick="return fnConfirmClear();" Text="Clear"
                                            OnClick="btnClear_Click" />
                                        &nbsp;<asp:Button ID="btnCancel" ToolTip="Goto translander page" runat="server" CausesValidation="false"
                                            CssClass="styleSubmitButton" Text="Cancel" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="msg" runat="server">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                                                    HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="Submit" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
