<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GORGEnquiryAppraisal_View.aspx.cs" Inherits="Origination_S3GORGEnquiryAppraisal_View"
    Title="Enquiry Customer Appraisal View" EnableEventValidation="false" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Enquiry/Customer Appraisal" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px">
                        <asp:Panel Width="100%" runat="server" GroupingText="Appraisal Type" CssClass="stylePanel">
                            <table width="100%">
                                <tr>
                                    <td width="10%">
                                        &nbsp;
                                    </td>
                                    <td width="94%">
                                        <asp:RadioButtonList ID="rdbCustomEnquiry" AutoPostBack="true" runat="server" Width="50%"
                                            RepeatDirection="Horizontal" OnSelectedIndexChanged="rdbCustomEnquiry_CheckedChanged">
                                            <asp:ListItem Text="Enquiry" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Pricing" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Application" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Customer" Value="3"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="pnl_Enquiry" runat="server">
                            <table width="100%">
                                <tr>
                                    <td id="tdEnquiry" runat="server">
                                        <asp:GridView ID="gvEnquiryAppraisal" runat="server" AllowPaging="true" PageSize="10"
                                            Width="100%" AutoGenerateColumns="false" OnRowDataBound="gvEnquiryAppraisal_RowDataBound"
                                            RowStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                            <Columns>
                                                <asp:BoundField DataField="Enquiry_Number" HeaderText="EnquiryNo" />
                                                <asp:TemplateField HeaderText="Enquiry No" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%#Eval("EnquiryUpdation_ID")%>' runat="server" ID="lblEnquiryUpdationID"></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="LOB_Name" HeaderText="Line of Business" />
                                                <asp:BoundField DataField="Branch_Name" HeaderText="Branch" />
                                                <asp:BoundField DataField="Product_Name" HeaderText="Product" />
                                                <asp:BoundField DataField="Customer_Code" HeaderText="Customer Code" />
                                                <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name" />
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
                                                                    <asp:CheckBox ID="chkEnquiryNumber" runat="server" AutoPostBack="true" OnCheckedChanged="chkEnquiryNumber_OnCheckedChanged" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="pnl_Customer" runat="server">
                            <table width="100%">
                                <tr>
                                    <td id="tdCustomer" runat="server" visible="false">
                                        <asp:GridView ID="gvCustomerAppraisal" runat="server" Width="100%" AutoGenerateColumns="false"
                                            RowStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader" AllowPaging="true"
                                            EmptyDataText="No Records Found" PageSize="10" EmptyDataRowStyle-BackColor="Red">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Enquiry Number" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%#Eval("Enq_Cus_App_ID")%>' runat="server" ID="lblEnqAppID"></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Company_Name" HeaderText="Company" />
                                                <asp:BoundField DataField="LOB_Name" HeaderText="Line of Business" />
                                                <asp:BoundField DataField="Branch_Name" HeaderText="Branch" Visible="false" />
                                                <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="styleButtonArea" align="center" colspan="3">
                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" OnClick="btnBack_Click"
                            CssClass="styleSubmitButton" Text="Main Page" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <span runat="server" id="lblErrorMessage" class="styleMandatoryLabel"></span>
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hdnSearch" runat="server" />
            <input type="hidden" id="hdnOrderBy" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
