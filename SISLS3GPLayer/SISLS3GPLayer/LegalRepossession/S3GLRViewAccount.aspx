<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" 
    CodeFile="S3GLRViewAccount.aspx.cs" Inherits="LegalRepossession_S3GLRViewAccount" Title="Untitled Page" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

  <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
              
                <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Statement Of Accounts">
                </asp:Label>
            </td>
         
        </tr>
        
            <%-- Design for space in between Row       --%>
                <tr>
                    <td class="styleFieldLabel">
                        &nbsp;
                    </td>
                </tr>
       <%--    <tr>
            <td >
              <table >
              <tr>
              <td class="styleFieldLabel">
               <asp:Label runat="server" ID="lblPANum" Text="Prime Account Number">
                </asp:Label>
              </td>
              <td>
              <asp:TextBox runat ="server" ID="txtPANum" />
              </td>
              <td class="styleFieldLabel">
               <asp:Label runat="server" ID="lblSANum" Text="Sub Account Number">
                </asp:Label>
              </td>
              <td>
              <asp:TextBox runat ="server" ID="txtSANum" />
              </td>
              </tr>
              </TABLE>
              </td> </tr> --%>
              <tr>
              <td align ="center" >
              <asp:Label ID="lblGridDetails" runat="server" Text="No Transaction details Available for this Account"
                                                        Visible="false"  Font-Size ="Larger"  Font-Bold ="true" />
               <asp:Panel ID="pnlTransactionDetails" runat="server" CssClass="stylePanel" GroupingText="Account Transaction Details" HorizontalAlign ="Center" >
                 <%--    <asp:Label ID="lblOpeningBalance" runat="server" ></asp:Label>
                        <br /><br />
              --%>
                    <%--<asp:GridView ID="GV1" runat ="server" />--%>
                    <%--<asp:Label ID="lblGridDetails" runat="server" Text="No Transaction details Available for this Accoount"
                                                        Visible="false"  Font-Size ="Larger"  Font-Bold ="true" />--%>
                       <asp:GridView ID="grvtransaction" runat="server" AutoGenerateColumns="False" CssClass="styleInfoLabel" Width="100%" ShowFooter="true" ShowHeader="true" HeaderStyle-CssClass="Freezing">
                            <Columns>
                                <asp:TemplateField HeaderText="PANum">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPANum" runat="server"  Text='<%# Bind("PrimeAccountNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                   <ItemStyle HorizontalAlign ="Left"  />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SANum">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSANum" runat="server" Text='<%# Bind("SubAccountNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign ="Left"  />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Doc Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocDate" runat="server" Text='<%# Bind("DocumentDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign ="Left"  />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Value Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblvaluedate" runat="server" Text='<%# Bind("ValueDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign ="Left"  />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Doc Reference">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocumentReference" runat="server" Text='<%# Bind("DocumentReference") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign ="Left"  />
                                    
                                     <FooterTemplate>
                                      <asp:Label ID="lblTotDue" runat="server" text ="Total"></asp:Label>
                                     
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                   <%--            <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldesc" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server" ToolTip="sum of  Dues amount" Text="Total"></asp:Label>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="25%" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Dues">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDues" runat="server" Text='<%# Bind("Dues") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                      <%--<asp:Label ID="lblTotDue" runat="server" text ="Total Due"></asp:Label>--%>
                                        <asp:Label ID="lblTotalDues" runat="server" ToolTip="sum of  Dues amount"></asp:Label>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign ="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Receipts">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReceipts" runat="server" Text='<%# Bind("Receipts") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalReceipts" runat="server" ToolTip="sum of  Receipts amount"></asp:Label>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign ="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Balance">
                                    <ItemTemplate>
                                        <asp:Label ID="lblbalance" runat="server" Text='<%# Bind("Balance") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalbalance" runat="server" ToolTip="sum of  Balance amount"></asp:Label>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign ="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                            </Columns>
                          </asp:GridView>
                       
                        </asp:Panel>
              
              </td>
              </tr>
              
                   
          
        
        </table>
       
</asp:Content>

