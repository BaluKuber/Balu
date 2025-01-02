<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HomePage.aspx.cs" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    Inherits="HomePage" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1">
   
    <div id="div3" style="overflow: auto; min-height: 400px;" oncontextmenu="return false;">
        <table width="95%" runat="server" id="tblContainer">
            <tr>
                <td>
                    <asp:Panel ID="WFHeaderPanel" GroupingText="Filter Criteria" Visible="false" runat="server" CssClass="stylePanel">
                        <table width="80%">
                            <tr>
                                <td width="15%">
                                    From Date
                                </td>
                                <td width="20%">
                                    <asp:TextBox runat="server" ID="txtFromDate" Width="70%" ContentEditable="False"
                                        AutoPostBack="True" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                    <asp:Image ID="imgTransactionDate" runat="server" ImageUrl="~/Images/calendaer.gif"
                                        ToolTip="Transaction Date" ImageAlign="AbsMiddle" />
                                    <asp:RequiredFieldValidator ID="RFVTransactionDate" CssClass="styleMandatoryLabel"
                                        runat="server" ValidationGroup="VGPDC" ControlToValidate="txtFromDate" Display="None"></asp:RequiredFieldValidator>
                                    <cc1:CalendarExtender ID="CEFrom" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                        PopupButtonID="imgTransactionDate" TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                                <td width="15%">
                                    To Date
                                </td>
                                <td width="20%">
                                    <asp:TextBox runat="server" ID="txtToDate" Width="70%" ContentEditable="False" AutoPostBack="True"
                                        OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Transaction Date"
                                        ImageAlign="AbsMiddle" /><asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                            CssClass="styleMandatoryLabel" runat="server" ValidationGroup="VGPDC" ControlToValidate="txtToDate"
                                            Display="None"></asp:RequiredFieldValidator><cc1:CalendarExtender ID="CETo" runat="server"
                                                Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="Image1"
                                                TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                </td>
                                <td width="28%">
                                    <asp:CheckBox ID="chkShowAll" runat="server" Text="Show All Work Items" OnCheckedChanged="chkShowAll_CheckedChanged"
                                        AutoPostBack="true" />
                                </td>
                                <td width="20%">
                                    <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="styleSubmitButton"
                                        OnClick="btnShow_Click" Visible="False" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="Left">
                    <%--    <asp:UpdatePanel ID="UPanelAll" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>--%>
                    <asp:Panel ID="WFListPanel" runat="server" Visible="false" GroupingText="Work Items" CssClass="stylePanel">
                        <asp:GridView HorizontalAlign="Center" runat="server" ID="gvListWFDocuments" Width="100%"
                            AutoGenerateColumns="False" DataKeyNames="Workflow_Status_ID,Workflow_Program_ID,Task_Document_No,LOB_ID,Branch_Id,Product_Id,Ref_Document_No,Workflow_Sequnce_ID,RefValue1,RefValue2,Document_Type"
                             BorderStyle="None" BorderWidth="0px" OnSelectedIndexChanged="gvListWFDocuments_SelectedIndexChanged"
                            OnRowCreated="gvListWFDocuments_RowCreated">
                            <RowStyle HorizontalAlign="Left"></RowStyle>
                            <HeaderStyle HorizontalAlign="Left">
                            </HeaderStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="Workflow Sequence Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWofkflowSequenceId" runat="server" Text='<%#Bind("Workflow_Sequnce_Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BranchId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranchId" runat="server" Text='<%#Bind("Branch_Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ProgramRefNo" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProgramRefNo" runat="server" Text='<%#Bind("Workflow_Program_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document Number" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lnkDocumentNo" runat="server" Text='<%#Bind("Task_Document_No") %>'></asp:Label>
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>
                                <asp:BoundField DataField="Enabled_Date" HeaderText="Document Date" HeaderStyle-CssClass="styleGridHeader"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Program_Name" HeaderText="Description" HeaderStyle-CssClass="styleGridHeader"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Status" Visible="true"  HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Bind("Task_Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CompanyId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompanyId" runat="server" Text='<%#Bind("Company_Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document_Type" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocument_Type" runat="server" Text='<%#Bind("Document_Type") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Documents found to load.
                            </EmptyDataTemplate>
                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                        </asp:GridView>
                    </asp:Panel>
                    <%--  </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
