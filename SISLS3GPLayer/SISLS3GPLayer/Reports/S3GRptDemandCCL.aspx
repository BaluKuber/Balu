<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GRptDemandCCL.aspx.cs" 
Inherits="Reports_S3GRptDemandCCL" Title="Demand Collection Customer Level"%>

<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Demand Collection Customer Level Report">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
           <tr>
            <td>
                <table width="100%" border="0">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlDemand" runat="server" GroupingText="Input Criteria" CssClass="stylePanel" Width="100%">
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" Text="Line of Business" ID="Label1" CssClass="styleReqFieldLabel" ToolTip="Line ofBusiness">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlLOB" runat="server" Width="80%" AutoPostBack="true" CausesValidation="true"
                                    OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged" ToolTip="Line ofBusiness">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="styleMandatoryLabel" runat="server"
                                    ControlToValidate="ddlLOB" InitialValue="-1" ErrorMessage="Select Line of Business"
                                    Display="None" ValidationGroup="Ok">
                                </asp:RequiredFieldValidator>
                            </td>
                            
                               <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" ID="lblCustomerName"  Text="Customer Name" ToolTip="Customer Name"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:TextBox ID="txtCustomerName" runat="server" Style="display: none;" Width="100%"
                                    CausesValidation="true" ></asp:TextBox>
                                <uc2:LOV ID="ucCustomerCodeLov" onfocus="return fnLoadCustomer();" runat="server"
                                    strLOV_Code="CMD" />
                                <asp:Button ID="btnLoadCustomer" runat="server" Text="Load Customer" Style="display: none"
                                    OnClick="btnLoadCustomer_OnClick" ToolTip="Customer Name" />
                                <input id="hdnCustID" type="hidden" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvcustname" runat="server"
                                    ControlToValidate="txtCustomerName" InitialValue="-1" ErrorMessage="Select a Customer Name"
                                    Display="None" ValidationGroup="Ok" Enabled="false">
                                </asp:RequiredFieldValidator>
                            </td>
                            </tr>
                                    <tr>
                                        <td height="8px">
                                        </td>
                                    </tr>
                           <tr>
                                        <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" ID="lblCustomerGroup" Text="Customer Group Name"  ToolTip="Customer Group Name"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlCustomerGroup" runat="server" Width="80%" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlCustomerGroup_SelectedIndexChanged" ToolTip="Customer Group Name">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvcustgroup" runat="server"
                                    ControlToValidate="ddlCustomerGroup" InitialValue="-1" ErrorMessage="Select a Customer Group Name"
                                    Display="None" ValidationGroup="Ok" Enabled="false">
                                </asp:RequiredFieldValidator>
                            </td>
                                        <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" ID="lblPNum"  Text="Account Number" ToolTip="Account Number"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlPNum" runat="server" Width="80%" AutoPostBack="true" OnSelectedIndexChanged="ddlPNum_SelectedIndexChanged" ToolTip="Account Number">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvpanum" runat="server"
                                    ControlToValidate="ddlPNum" InitialValue="-1" ErrorMessage="Select a Prime Account Number"
                                    Display="None" ValidationGroup="Ok" Enabled="false">
                                </asp:RequiredFieldValidator>
                            </td>
                                    </tr>
                                    <tr>
                                        <td height="8px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" ID="lblSNum" CssClass="styleDisplayLabel" Text="Sub Account Number" ToolTip="Sub Account Number"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlSNum" runat="server" Width="80%" AutoPostBack="true" ToolTip="Sub Account Number">
                                </asp:DropDownList>
                            </td>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" ID="lblReportDate" CssClass="styleReqFieldLabel" Text="Report Date" ToolTip="Report Date"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:TextBox ID="txtReportDate" runat="server" Width="78%" ToolTip="Report Date"></asp:TextBox>
                                <asp:Image ID="imgReportDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender runat="server" TargetControlID="txtReportDate" PopupButtonID="imgReportDate" Format="dd/MM/yyyy" ID="CalendarExtender1" OnClientDateSelectionChanged="checkDate_NextSystemDate"></cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvReportDate" runat="server" ControlToValidate="txtReportDate" Display="None" ValidationGroup="Ok" CssClass="styleMandatoryLabel" SetFocusOnError="True" ErrorMessage="Select Report Date"></asp:RequiredFieldValidator>
                            </td>
                            </tr>
                            <tr>
                            <td height="8px">
                            </td>
                            </tr>
                            <tr>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" Text="Denomination" ID="lblDenomination" CssClass="styleReqFieldLabel" ToolTip="Denomination">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlDenomination" runat="server" Width="80%" ToolTip="Denomination">
                                </asp:DropDownList>
                                <%-- <asp:RequiredFieldValidator ID="rfvddlDenomination" runat="server" ErrorMessage="Select Denomination." 
                                ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="ddlDenomination" 
                                InitialValue="-1"></asp:RequiredFieldValidator>--%>
                            </td>
                            <td class="styleFieldLabel">  
                            </td>
                            </tr>
                            <tr>
                            <td height="8px">
                            </td>
                           </tr>
                           </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
            <tr>
            <td>
            <table width ="100%" border="0">
            <tr>
            <td>  
            <asp:Panel ID="PnlDemandData" runat="server" GroupingText="Compare Data" CssClass="stylePanel" Width="100%">
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%" align="center">
                        <tr>
                            <td align="center" class="styleFieldLabel">
                                <asp:Label ID="lblFinancialYearBase" runat="server" Text="Financial Years " ToolTip="Financial Years"></asp:Label>
                            </td>
                            <td align="center" class="styleFieldAlign">
                                <asp:DropDownList ID="ddlFinacialYearBase" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFinacialYearBase_SelectedIndexChanged" ToolTip="Financial Years">
                                </asp:DropDownList>
                            </td>
                            <td align="center" class="styleFieldLabel">
                                <asp:Label ID="lblFromYearMonthBase" runat="server" Text="From Year Month" CssClass="styleReqFieldLabel" ToolTip="From Year Month"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="center">
                                <asp:DropDownList ID="ddlFromYearMonthBase" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFromYearMonthBase_SelectedIndexChanged" ToolTip="From Year Month">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlFromYearMonthBase" runat="server" ErrorMessage="Select From Year Month" ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="ddlFromYearMonthBase" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td align="center" class="styleFieldLabel">
                                <asp:Label ID="lblToYearMonthBase" runat="server" Text="To Year Month" CssClass="styleReqFieldLabel" ToolTip="To Year Month"> </asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="center">
                                <asp:DropDownList ID="ddlToYearMonthBase" runat="server" OnSelectedIndexChanged="ddlToYearMonthBase_SelectedIndexChanged" ToolTip="To Year Month"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlToYearMonthBase" runat="server" ErrorMessage="Select To Year Month" ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="ddlToYearMonthBase" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="center" class="styleFieldLabel">
                                <asp:Label ID="lblFinancialYearCompare" runat="server" Text="Financial Years " ToolTip="Financial Years"></asp:Label>
                            </td>
                            <td align="center" class="styleFieldAlign">
                                <asp:DropDownList ID="ddlFinancialYearCompare" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFinancialYearCompare_SelectedIndexChanged" ToolTip="Financial Years ">
                                </asp:DropDownList>
                                <%--                     <asp:RequiredFieldValidator ID="rfvddlFinancialYearCompare" runat="server" ErrorMessage="Select Finacial Years to Compare." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="ddlFinancialYearCompare" InitialValue="0"></asp:RequiredFieldValidator>--%>
                            </td>
                            <td align="center" class="styleFieldLabel">
                                <asp:Label ID="lblFromYearMonthCompare" runat="server" Text="From Year Month" ToolTip="From Year Month"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="center">
                                <asp:DropDownList ID="ddlFromYearMonthCompare" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFromYearMonthCompare_SelectedIndexChanged" ToolTip="From Year Month">
                                </asp:DropDownList>
                            </td>
                            <td align="center" class="styleFieldLabel">
                                <asp:Label ID="lblToYearMonthCompare" runat="server" Text="To Year Month" ToolTip="To Year Month"> </asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="center">
                                <asp:DropDownList ID="ddlToYearMonthCompare" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlToYearMonthCompare_SelectedIndexChanged" ToolTip="To Year Month">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                </td>
            </tr>
            </table>
               
            </td>
        </tr>
    <table width="100%" border="0" align="center">
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td colspan="3" align="center">
                <asp:Button runat="server" ID="btnOk" CssClass="styleSubmitButton" Text="GO" CausesValidation="true" ValidationGroup="Ok" OnClick="btnOk_Click" ToolTip="GO"/>
                &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" ToolTip="Clear" OnClientClick="return fnConfirmClear();" />
            </td>
        </tr>
        <tr>
            <td height="8px">
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblAmounts" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
                <%--<asp:Label ID="lblCurrency" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>--%>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlDemandCollection" runat="server" GroupingText="Demand Vs Collection Customer Level" CssClass="stylePanel" Width="100%" Visible="false">
                <asp:Label ID="lblError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                    <div id="divDemand" runat="server" style="overflow: auto; height: 300px; display: none">
                        <asp:GridView ID="grvDemand" runat="server" Width="98%" AutoGenerateColumns="false" HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center" ShowFooter="true" OnRowDataBound ="grvDemand_RowDataBound">
                            <Columns>
                               <%-- <asp:TemplateField HeaderText="Frequency" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Center" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFrequency" runat="server" Text='<%# Bind("Frequency") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Text="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Month" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Center" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMonth" runat="server" Text='<%# Bind("Month") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Demand Month" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDemandMonth" runat="server" Text='<%# Bind("DemandMonth") %>' ToolTip="Demand Month"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lbllocation" runat="server" Text='<%# Bind("Location") %>' ToolTip="Location"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>                               
                                <asp:TemplateField HeaderText="Customer Name" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerCode" runat="server" Text='<%# Bind("CustomerName") %>' ToolTip="Customer Name"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account No" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrime" runat="server" Text='<%# Bind("Panum") %>' ToolTip="Account No"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sub Account No" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSub" runat="server" Text='<%# Bind("Sanum") %>' ToolTip="Sub Account No"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Opening Demand" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOpeningDemand" runat="server" Text='<%# Bind("OpeningDemand") %>' ToolTip="Opening Demand"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbltotOpeningDemand" runat="server" ToolTip="Total Opening Demand"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Opening Collection" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOpeningCollection" runat="server" Text='<%# Bind("OpeningCollection") %>' ToolTip="Opening Collection"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbltotOpeningCollection" runat="server" ToolTip="Total Opening Collection"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Collection %" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOpeningPercentage" runat="server" Text='<%# Bind("OpeningPercentage") %>' ToolTip="Opening %"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbltotOpeningPercentage" runat="server" ToolTip="Total Opening percentage"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Monthly Demand" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMonthlyDemand" runat="server" Text='<%# Bind("MonthlyDemand") %>' ToolTip="Monthly Demand"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbltotMonthlyDemand" runat="server" ToolTip="Total Monthly Demand"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Monthly Collection" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMonthlyCollection" runat="server" Text='<%# Bind("MonthlyCollection") %>' ToolTip="Monthly Collection"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbltotMonthlyCollection" runat="server" ToolTip="Total Monthly Collection"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Collection %" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMonthlyPercentage" runat="server" Text='<%# Bind("MonthlyPercentage") %>' ToolTip="Monthly %"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbltotMonthlyPercentage" runat="server" ToolTip="Total Monthly %"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cumulative Demand" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosingDemand" runat="server" Text='<%# Bind("ClosingDemand") %>' ToolTip="Closing Demand"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbltotClosingDemand" runat="server" ToolTip="Total Closing Demand"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cumulative Collection" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosingCollection" runat="server" Text='<%# Bind("ClosingCollection") %>' ToolTip="ClosingCollection"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbltotClosingCollection" runat="server" ToolTip="Total ClosingCollection"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Collection %" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosingPercentage" runat="server" Text='<%# Bind("ClosingPercentage") %>' ToolTip="Closing %"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbltotClosingPercentage" runat="server" ToolTip="Total Closing %"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td height="8px">
            </td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td colspan="4" align="center">
                <asp:Button runat="server" ID="btnPrint" CssClass="styleSubmitButton" Text="Print" CausesValidation="false" ValidationGroup="Print" Visible="false" OnClick="btnPrint_Click" ToolTip="Print" />
            </td>
        </tr>
        <tr>
            <td height="8px">
            </td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary ID="vsDemandCollection" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Ok" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CustomValidator ID="CVDemandCollection" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
                <asp:CustomValidator ID="CVAssetClass" runat="server" Display="None" ValidationGroup="Ok"></asp:CustomValidator>
                <asp:CustomValidator ID="CVFrequency" runat="server" Display="None" ValidationGroup="Ok"></asp:CustomValidator>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
      function fnLoadCustomer() 
     {
        document.getElementById('<%=btnLoadCustomer.ClientID%>').click();
     }
    </script>
    <script language="javascript" type="text/javascript">
    function Resize()
     {
       if(document.getElementById('ctl00_ContentPlaceHolder1_divDemand') != null)
       {
         if(document.getElementById('divMenu').style.display=='none')
            {
             (document.getElementById('ctl00_ContentPlaceHolder1_divDemand')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
            }
         else
           {
             (document.getElementById('ctl00_ContentPlaceHolder1_divDemand')).style.width = screen.width - 270;
           }
        }  
      }
      
      
    function showMenu(show) 
      {
       if (show == 'T') 
         {
        
          if(document.getElementById('divGrid1')!=null)
            {
                document.getElementById('divGrid1').style.width="800px";
                document.getElementById('divGrid1').style.overflow="scroll";
            }
        
            document.getElementById('divMenu').style.display = 'Block';
            document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

            document.getElementById('ctl00_imgShowMenu').style.display = 'none';
            document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

            if(document.getElementById('ctl00_ContentPlaceHolder1_divDemand') != null)
            (document.getElementById('ctl00_ContentPlaceHolder1_divDemand')).style.width = screen.width - 270;
        }
        if (show == 'F') 
        {
            if(document.getElementById('divGrid1')!=null)
                {
                    document.getElementById('divGrid1').style.width="960px";
                    document.getElementById('divGrid1').style.overflow="auto";
                }
                
            document.getElementById('divMenu').style.display = 'none';
            document.getElementById('ctl00_imgHideMenu').style.display = 'none';
            document.getElementById('ctl00_imgShowMenu').style.display = 'Block';
           
            if(document.getElementById('ctl00_ContentPlaceHolder1_divDemand') != null)
           (document.getElementById('ctl00_ContentPlaceHolder1_divDemand')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
        }
    }

    </script>
    </table>
</asp:Content>
