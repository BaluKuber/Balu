<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_Rpt_LeadAnalysis.aspx.cs" Inherits="Reports_S3G_Rpt_LeadAnalysis" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Lead Analysis Report">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                <asp:Panel ID="pnlHeaderDetails" runat="server" GroupingText="Input Criteria" CssClass="stylePanel" Width="100%">
                    <table cellpadding="0" cellspacing="0" style="width: 100%" align="center">
                        <tr visible="false">
                            <td class="styleFieldLabel" width="20%" visible="false">
                                <asp:Label runat="server" Text="Line Of Business" Visible="false" ToolTip="Entity Name" ID="lblEntityName" CssClass="styleReqFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%" visible="false">
                                <asp:DropDownList ID="ddlLob" ToolTip="Line Of Business" Visible="false" runat="server" AutoPostBack="true" Width="65%" >
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvLob" runat="server" ErrorMessage="Select Line of Business" ValidationGroup="Ok" Enabled="false" Visible="false" Display="None" SetFocusOnError="True" InitialValue="0" ControlToValidate="ddlLob">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="20%" visible="false">
                                <asp:Label runat="server" Text="Source Type" ToolTip="Source Type" Visible="false" ID="lblSourceType" CssClass="styleDisplayLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%" visible="false">
                                <asp:DropDownList ID="ddlsourcetype" runat="server" Visible="false"  ToolTip="Source Type" Width="65%">
                                </asp:DropDownList>
                                </td>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                         <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Location" ToolTip="Entity Name" ID="Label1" CssClass="styleReqFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddllocation" ToolTip="Line Of Business" runat="server" AutoPostBack="true" Width="65%" >
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Select Line of Business" ValidationGroup="Ok" Display="None" SetFocusOnError="True" InitialValue="0" ControlToValidate="ddllocation">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Lead Status" ToolTip="Source Type" ID="Label2" CssClass="styleDisplayLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlleadstatus" runat="server"  ToolTip="Source Type" Width="65%">
                                </asp:DropDownList>
                                </td>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Start Date" ToolTip="Start Date" ID="lblStartDate" CssClass="styleReqFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:TextBox ID="txtStartDate" ToolTip="Start Date" runat="server" ></asp:TextBox>
                                <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtStartDate" PopupButtonID="imgStartDate" ID="CalendarExtender1" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Select Start Date" ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="End Date" ToolTip="End Date" ID="lblEndDate" CssClass="styleReqFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:TextBox ID="txtEndDate" runat="server"  ToolTip="End Date"></asp:TextBox>
                                <asp:Image ID="ImgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtEndDate" PopupButtonID="imgEndDate" ID="CalendarExtender2" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="Select End Date" ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="txtEndDate">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                        <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Rows" ToolTip="Entity Name" ID="Label3" CssClass="styleReqFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlRows" ToolTip="Line Of Business" runat="server" AutoPostBack="true" Width="65%" >
                                 <asp:ListItem Value=-1 Text="Select"></asp:ListItem>
                                <asp:ListItem Value=0 Text="Line Of Business"></asp:ListItem>
                                 <asp:ListItem Value=1 Text="Lead Source Type"></asp:ListItem>
                                  <asp:ListItem Value=2 Text="Lead Status"></asp:ListItem>
                                   <asp:ListItem Value=3 Text="Account Status"></asp:ListItem>
                                    <asp:ListItem Value=4 Text="Customer Status"></asp:ListItem>
                                     <asp:ListItem Value=5 Text="Asset"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Select Rows" ValidationGroup="Ok" Display="None" SetFocusOnError="True" InitialValue="-1" ControlToValidate="ddlRows">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Columns" ToolTip="Columns" ID="Label4" CssClass="styleDisplayLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlcolumn" runat="server"  ToolTip="Column" Width="65%">
                                <asp:ListItem Value=-1 Text="Select"></asp:ListItem>
                                   <asp:ListItem Value=0 Text="Line Of Business"></asp:ListItem>
                                 <asp:ListItem Value=1 Text="Lead Source Type"></asp:ListItem>
                                  <asp:ListItem Value=2 Text="Lead Status"></asp:ListItem>
                                   <asp:ListItem Value=3 Text="Account Status"></asp:ListItem>
                                    <asp:ListItem Value=4 Text="Customer Status"></asp:ListItem>
                                     <asp:ListItem Value=5 Text="Asset"></asp:ListItem>
                                </asp:DropDownList>
                                </td>
                             </tr>
                          <tr>
                            <td height="8px">
                            </td>
                        </tr>
                             <tr>
                                  <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Type" ToolTip="Type" ID="Label5" CssClass="styleReqFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlType" ToolTip="Type" runat="server" AutoPostBack="true" Width="65%" >
                                 <asp:ListItem Value=0 Text="Select"></asp:ListItem>
                                <asp:ListItem Value=1 Text="Count of Leads"></asp:ListItem>
                                 <asp:ListItem Value=2 Text="Average TAT"></asp:ListItem>
                                  <asp:ListItem Value=3 Text="Amount Financed"></asp:ListItem>
                                   <asp:ListItem Value=4 Text="Average Amount"></asp:ListItem>
                                  
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Select Type" ValidationGroup="Ok" Display="None" SetFocusOnError="True" InitialValue="0" ControlToValidate="ddlType">
                                </asp:RequiredFieldValidator>
                            </td>
                             </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td height="8px">
            </td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td colspan="4" align="center">
                <asp:Button runat="server" ID="btnOk" CssClass="styleSubmitButton" Text="GO" OnClick="btnOk_Click" onCausesValidation="true"  ValidationGroup="Ok" ToolTip="Go" />
                &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" ToolTip="Clear" OnClientClick="return fnConfirmClear();" />
            </td>
        </tr>
          <tr>
            <td>
                <asp:Panel ID="pnllead" runat="server" Visible="false" GroupingText="CRM Details"
                    Width="100%" CssClass="stylePanel">
                    <table width="100%">
                        <tr>
                            <td>
                                <div style="height: 250px; overflow: auto">
                                   
                                        <asp:GridView ID="gvlead" runat="server" AutoGenerateColumns="True" ShowFooter="false"
                                            RowStyle-Width="0" Width="100%" OnRowDataBound="gvlead_OnRowDataBound">
                                            <Columns>
                                                <%--Serial Number--%>
                                                <%-- <asp:TemplateField HeaderText="Sl No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1%>' ToolTip="Serial Number" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <%--Location--%>
                                               <%-- <asp:TemplateField HeaderText="GL_Code" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgl_code" runat="server" Text='<%#Eval("GL_Code") %>' ToolTip="GL Code" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                  <%--Dimension--%>
                                                <%--<asp:TemplateField HeaderText="GL_Desc">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgl_desc" runat="server" Text='<%#Eval("GL_Desc") %>' ToolTip="GL Description" />
                                                       
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                           
                                            </Columns>
                                        </asp:GridView>
                              
                                   
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
      
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td colspan="4" align="center">
                <asp:Button runat="server" ID="btnPrint" ToolTip="Print" CssClass="styleSubmitButton" Text="Print" CausesValidation="false" ValidationGroup="Print" Visible="false"  />
                  <asp:Button ID="btnexcel" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                    Visible="false" Text="Excel" OnClick="FunExcelExport"  />
            </td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary ID="vsVendor" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Ok" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CustomValidator ID="CVVendorDetails" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
            </td>
        </tr>
    </table>
</asp:Content>

