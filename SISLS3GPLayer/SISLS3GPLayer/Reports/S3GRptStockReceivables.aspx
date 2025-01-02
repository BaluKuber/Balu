<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GRptStockReceivables.aspx.cs" Inherits="Reports_S3GRptStockReceivables"
    Title="Stock and Receivables" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    &nbsp;&nbsp;&nbsp;
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
    <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Stock and Receivables Report">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="100%" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlDemand" runat="server" GroupingText="Input Criteria" CssClass="stylePanel"
                    Width="100%">
                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel"
                                    ToolTip="Line of Business">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlLOB" runat="server" Width="85%" AutoPostBack="true" CausesValidation="true"
                                    OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged" ToolTip="Line of Business">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvLOB" CssClass="styleMandatoryLabel" runat="server"
                                    ControlToValidate="ddlLOB" InitialValue="-1" ErrorMessage="Select Line of Business"
                                    Display="None" ValidationGroup="Ok">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label ID="lblBranch" runat="server" Text="Location 1" CssClass="StyleReqFieldLabel"
                                    ToolTip="Location 1">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlBranch" runat="server" Visible="true" Width="85%" ToolTip="Location 1"
                                    OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="8px"></td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label ID="lbllocation" runat="server" Text="Location 2" CssClass="StyleReqFieldLabel"
                                    ToolTip="Location 2">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddllocation2" runat="server" Visible="true" Width="85%" ToolTip="Location 2">
                                </asp:DropDownList>
                            </td>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" ID="lblDate" CssClass="styleReqFieldLabel" Text="Date"
                                    ToolTip="Date"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:TextBox ID="txtDate" runat="server" Width="75%" ToolTip="Date"></asp:TextBox>
                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtDate"
                                    OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgStartMonth"
                                    ID="CalendarExtender1">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgStartMonth" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" ErrorMessage="Select the Date."
                                    ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="txtDate"
                                    CssClass="styleMandatoryLabel"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="8px"></td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" Text="Denomination" ID="lblDenomination" CssClass="styleReqFieldLabel"
                                    ToolTip="Denomination">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlDenomination" Width="85%" runat="server" ToolTip="Denomination">
                                </asp:DropDownList>
                            </td>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" Text="Reference" ID="lblCustomerReference" CssClass="styleReqFieldLabel"
                                    ToolTip="Reference"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlCustomerReference" Width="85%" runat="server" AutoPostBack="true" ToolTip="Reference" OnSelectedIndexChanged="ddlCustomerReference_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Value="0">Contract</asp:ListItem>
                                    <asp:ListItem Value="1">Customer</asp:ListItem>
                                    <asp:ListItem Value="2">Group</asp:ListItem>
                                    <asp:ListItem Value="3">Industry</asp:ListItem>
                                    
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="8px"></td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td height="8px"></td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td colspan="4" align="center">
                <asp:Button runat="server" ID="btnOk" CssClass="styleSubmitButton" Text="Go" CausesValidation="true"
                    ValidationGroup="Ok" OnClientClick="return fnCheckPageValidators('Ok',false);" OnClick="btnOk_Click" ToolTip="Go" />
                &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                    Text="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();"
                    ToolTip="Clear" />
            </td>
        </tr>
        <tr>
            <td height="8px"></td>
        </tr>
        <tr>
            <td align="right" colspan="2">
                <asp:Label ID="lblAmounts" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
                <%--<asp:Label ID="lblCurrency" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>--%>
            </td>
        </tr>


        <tr>
            <td>
                <asp:Panel ID="pnlStockReceivables" runat="server" GroupingText="Stock and Receivables Customer wise Details"
                    CssClass="stylePanel" Width="100%" Visible="false">
                    <div id="div2" runat="server" style="overflow: auto; height: 300px; display: block">
                        <asp:GridView ID="grvStockRec" runat="server" Width="99%" AutoGenerateColumns="false"
                            HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center" ShowFooter="true"
                            BorderColor="Gray">
                            <RowStyle HorizontalAlign="Center" />
                            <Columns>
                                <%--  <asp:TemplateField HeaderText="Month" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMonth" runat="server" Text='<%# Bind("Month") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblGrndTotal" runat="server" Text="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="LOB Id" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOBId" runat="server" Text='<%# Bind("LOB_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line of Business" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStockLOB" runat="server" Text='<%# Bind("LOB") %>' ToolTip="Line of Business"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location ID" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranchId" runat="server" Text='<%# Bind("LOCATION_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDemandBranch" runat="server" Text='<%# Bind("LOCATION_NAME") %>'
                                            ToolTip="Location"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Id" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcustomerid" runat="server" Text='<%# Bind("CUSTOMER_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lnkCustomername" runat="server" Text='<%# Bind("CUSTOMERNAME") %>' ToolTip="Group Name"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Principal" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGrossprincipal" runat="server" Text='<%# Bind("Gross_principal1") %>' ToolTip="Principal"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbltotGrossPrincipal" runat="server" ToolTip="Total Gross Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UMFC" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUMFC" runat="server" Text='<%# Bind("UMFC1") %>' ToolTip="UMFC"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbltotUMFC" runat="server" ToolTip="Total UMFC"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                                       <asp:TemplateField HeaderText="Gross Stock" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgrossstock" runat="server" Text='<%# Bind("gross_stock1") %>'
                                            ToolTip="Gross Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbltotgrossstock" runat="server" ToolTip="Gross Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Principal Outstanding" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNetPrincipal" runat="server" Text='<%# Bind("Net_Principal1") %>' ToolTip="Dues UnCollected Billed Finance Charges"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbltotNetPrincipal" runat="server" ToolTip="Total Net Principal"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Interest Outstanding"  ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNIC" runat="server" Text='<%# Bind("Interest_OS1") %>' ToolTip="NO Of Installments"></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:Label ID="lbltotInterestOS" runat="server" ToolTip="Interest Outstanding"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Future Installments" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblfutureInstallments" runat="server" Text='<%# Bind("future_instal1") %>' ToolTip="Net Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbltotfutureInstallments" runat="server" ToolTip="Total Future Installments"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Net Stock" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblnetstock" runat="server" Text='<%# Bind("net_stock1") %>' ToolTip="Net Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbltotnetstock" runat="server" ToolTip="Total Net Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Details">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" Enabled="true" ImageUrl="~/Images/spacer.gif" ToolTip="Details"
                                            CssClass="styleGridEdit" runat="server" OnClick="imgbtnQuery_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="styleGridHeader" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>

          <tr>
            <td>
                <asp:Panel ID="pnlcontractwise" runat="server" GroupingText="Stock and Receivables Contractwise Details"
                    CssClass="stylePanel" Width="100%" Visible="false">
                    <div id="div3" runat="server" style="overflow: auto; height: 300px; display: block">
                        <asp:GridView ID="grvcontract" runat="server" Width="99%" AutoGenerateColumns="false"
                            HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center" ShowFooter="true"
                            BorderColor="Gray">
                            <RowStyle HorizontalAlign="Center" />
                            <Columns>
                                <%--  <asp:TemplateField HeaderText="Month" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcontMonth" runat="server" Text='<%# Bind("Month") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblcontGrndTotal" runat="server" Text="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="LOB Id" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcontcontLOBId" runat="server" Text='<%# Bind("LOB_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line of Business" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcontcontStockLOB" runat="server" Text='<%# Bind("LOB") %>' ToolTip="Line of Business"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblcontcontTotal" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location ID" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcontBranchId" runat="server" Text='<%# Bind("LOCATION_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcontDemandBranch" runat="server" Text='<%# Bind("LOCATION_NAME") %>'
                                            ToolTip="Location"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Id" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcontcustomerid" runat="server" Text='<%# Bind("CUSTOMER_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcontCustomername" runat="server" Text='<%# Bind("CUSTOMERNAME") %>' ToolTip="Group Name"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="Account" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcontpanum" runat="server" Text='<%# Bind("PANUM") %>' ToolTip="Group Name"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Sub Account" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lnkcontsanum" runat="server" Text='<%# Bind("SANUM") %>' ToolTip="Group Name"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Principal" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcontGrossprincipal" runat="server" Text='<%# Bind("Gross_principal1") %>' ToolTip="Principal"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblconttotGrossPrincipal" runat="server" ToolTip="Total Gross Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UMFC" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcontUMFC" runat="server" Text='<%# Bind("UMFC1") %>' ToolTip="UMFC"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblconttotUMFC" runat="server" ToolTip="Total UMFC"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Gross Stock" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcontgrossstock" runat="server" Text='<%# Bind("gross_stock1") %>'
                                            ToolTip="Gross Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblconttotgrossstock" runat="server" ToolTip="Gross Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                  <asp:TemplateField HeaderText="NOI" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcontNOI" runat="server" Text='<%# Bind("NOI") %>'
                                            ToolTip="Gross Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblconttotNOI" runat="server" ToolTip="Gross Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Principal Outstanding" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcontNetPrincipal" runat="server" Text='<%# Bind("Net_Principal1") %>' ToolTip="Dues UnCollected Billed Finance Charges"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblconttotNetPrincipal" runat="server" ToolTip="Total Net Principal"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Interest Outstanding"  ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcontNIC" runat="server" Text='<%# Bind("Interest_OS1") %>' ToolTip="NO Of Installments"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblconttotInterest_OS" runat="server" ToolTip="Total Interest Outstanding"></asp:Label>
                                    </FooterTemplate>
                                      <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Future Installments" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcontfutureInstallments" runat="server" Text='<%# Bind("future_instal1") %>' ToolTip="Net Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblconttotfutureInstallments" runat="server" ToolTip="Total Future Installments"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Net Stock" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcontnetstock" runat="server" Text='<%# Bind("net_stock1") %>' ToolTip="Net Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblconttotnetstock" runat="server" ToolTip="Total Net Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Details">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" Enabled="true" ImageUrl="~/Images/spacer.gif" ToolTip="Details"
                                            CssClass="styleGridEdit" runat="server" OnClick="imgbtnQuery_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="styleGridHeader" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>

         <tr>
            <td>
                <asp:Panel ID="pnlgroup" runat="server" GroupingText="Stock and Receivables Group Wise Details"
                    CssClass="stylePanel" Width="100%" Visible="false">
                    <div id="div4" runat="server" style="overflow: auto; height: 300px; display: block">
                        <asp:GridView ID="grvgroupwise" runat="server" Width="99%" AutoGenerateColumns="false"
                            HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center" ShowFooter="true"
                            BorderColor="Gray">
                            <RowStyle HorizontalAlign="Center" />
                            <Columns>
                                <%--  <asp:TemplateField HeaderText="Month" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgpMonth" runat="server" Text='<%# Bind("Month") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblgpGrndTotal" runat="server" Text="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="LOB Id" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgpcontLOBId" runat="server" Text='<%# Bind("LOB_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line of Business" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgpcontStockLOB" runat="server" Text='<%# Bind("LOB") %>' ToolTip="Line of Business"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblgpcontTotal" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location ID" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgpBranchId" runat="server" Text='<%# Bind("LOCATION_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgpDemandBranch" runat="server" Text='<%# Bind("LOCATION_NAME") %>'
                                            ToolTip="Location"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Id" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgpcustomerid" runat="server" Text='<%# Bind("GROUP_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Group Name" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgpCustomername" runat="server" Text='<%# Bind("GroupNAME") %>' ToolTip="Group Name"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                             <asp:TemplateField HeaderText="Principal" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgpGrossprincipal" runat="server" Text='<%# Bind("Gross_principal1") %>' ToolTip="Principal"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblgptotGrossPrincipal" runat="server" ToolTip="Total Gross Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UMFC" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgpUMFC" runat="server" Text='<%# Bind("UMFC1") %>' ToolTip="UMFC"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblgptotUMFC" runat="server" ToolTip="Total UMFC"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                                       <asp:TemplateField HeaderText="Gross Stock" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgpgrossstock" runat="server" Text='<%# Bind("gross_stock1") %>'
                                            ToolTip="Gross Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblgptotgrossstock" runat="server" ToolTip="Gross Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
      <asp:TemplateField HeaderText="Principal Outstanding" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgpNetPrincipal" runat="server" Text='<%# Bind("Net_Principal1") %>' ToolTip="Dues UnCollected Billed Finance Charges"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblgptotNetPrincipal" runat="server" ToolTip="Total Net Principal"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Interest Outstanding"  ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgpNIC" runat="server" Text='<%# Bind("Interest_OS1") %>' ToolTip="NO Of Installments"></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:Label ID="lblgptotInterestOS" runat="server" ToolTip="Total Interest Outstanding"></asp:Label>
                                    </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Future Installments" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgpfutureInstallments" runat="server" Text='<%# Bind("future_instal1") %>' ToolTip="Net Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblgptotfutureInstallments" runat="server" ToolTip="Total Future Installments"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Net Stock" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgpnetstock" runat="server" Text='<%# Bind("net_stock1") %>' ToolTip="Net Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblgptotnetstock" runat="server" ToolTip="Total Net Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Details">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" Enabled="true" ImageUrl="~/Images/spacer.gif" ToolTip="Details"
                                            CssClass="styleGridEdit" runat="server" OnClick="imgbtnQuery_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="styleGridHeader" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>

          <tr>
            <td>
                <asp:Panel ID="pnlIndustry" runat="server" GroupingText="Stock and Receivables Industry Wise Details"
                    CssClass="stylePanel" Width="100%" Visible="false">
                    <div id="div5" runat="server" style="overflow: auto; height: 300px; display: block">
                        <asp:GridView ID="grvindustry" runat="server" Width="99%" AutoGenerateColumns="false"
                            HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center" ShowFooter="true"
                            BorderColor="Gray">
                            <RowStyle HorizontalAlign="Center" />
                            <Columns>
                                <%--  <asp:TemplateField HeaderText="Month" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblindustryMonth" runat="server" Text='<%# Bind("Month") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblindustryGrndTotal" runat="server" Text="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="LOB Id" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblindustrycontLOBId" runat="server" Text='<%# Bind("LOB_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line of Business" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblindustrycontStockLOB" runat="server" Text='<%# Bind("LOB") %>' ToolTip="Line of Business"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblindustrycontTotal" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location ID" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblindustryBranchId" runat="server" Text='<%# Bind("LOCATION_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblindustryDemandBranch" runat="server" Text='<%# Bind("LOCATION_NAME") %>'
                                            ToolTip="Location"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Industry Id" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblindustrycustomerid" runat="server" Text='<%# Bind("Industry_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Industry Name" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblindustryCustomername" runat="server" Text='<%# Bind("IndustryName") %>' ToolTip="Group Name"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                             <asp:TemplateField HeaderText="Principal" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblindustryGrossprincipal" runat="server" Text='<%# Bind("Gross_principal1") %>' ToolTip="Principal"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblindustrytotGrossPrincipal" runat="server" ToolTip="Total Gross Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UMFC" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblindustryUMFC" runat="server" Text='<%# Bind("UMFC1") %>' ToolTip="UMFC"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblindustrytotUMFC" runat="server" ToolTip="Total UMFC"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                           <asp:TemplateField HeaderText="Gross Stock" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblindustrygrossstock" runat="server" Text='<%# Bind("gross_stock1") %>'
                                            ToolTip="Gross Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblindustrytotgrossstock" runat="server" ToolTip="Gross Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
      <asp:TemplateField HeaderText="Principal Outstanding" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblindustryNetPrincipal" runat="server" Text='<%# Bind("Net_Principal1") %>' ToolTip="Dues UnCollected Billed Finance Charges"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblindustrytotNetPrincipal" runat="server" ToolTip="Total Net Principal"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Interest Outstanding"  ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblindustryNIC" runat="server" Text='<%# Bind("Interest_OS1") %>' ToolTip="NO Of Installments"></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:Label ID="lblindustrytotindustryNIC" runat="server" ToolTip="Total Interest Outstanding"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Future Installments" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblindustryfutureInstallments" runat="server" Text='<%# Bind("future_instal1") %>' ToolTip="Net Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblindustrytotfutureInstallments" runat="server" ToolTip="Total Future Installments"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Net Stock" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblindustrynetstock" runat="server" Text='<%# Bind("net_stock1") %>' ToolTip="Net Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblindustrytotnetstock" runat="server" ToolTip="Total Net Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Details">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" Enabled="true" ImageUrl="~/Images/spacer.gif" ToolTip="Details"
                                            CssClass="styleGridEdit" runat="server" OnClick="imgbtnQuery_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="styleGridHeader" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td height="8px"></td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlDetails" runat="server" GroupingText="Stock and Receivables Detail Level"
                    CssClass="stylePanel" Width="100%" Visible="false" >
                    <div id="div1" runat="server" style="overflow: auto; height: 300px; display: block">
                        <asp:GridView ID="grvdetails" runat="server" Width="99%" AutoGenerateColumns="false"
                            HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center" ShowFooter="true"
                            BorderColor="Gray" OnRowDataBound="grvdetails_OnRowDataBound">
                            <RowStyle HorizontalAlign="Center" />
                            <Columns>
                                <asp:TemplateField HeaderText="LOB Id" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOBId" runat="server" Text='<%# Bind("LOB_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line of Business" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStockLOB" runat="server" Text='<%# Bind("LOB") %>' ToolTip="Line of Business"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location ID" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranchId" runat="server" Text='<%# Bind("LOCATION_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDemandBranch" runat="server" Text='<%# Bind("LOCATION_NAME") %>' ToolTip="Location"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer ID" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomer_id" runat="server" Text='<%# Bind("level_id") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Level Name" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustCodeName" runat="server" Text='<%# Bind("LevelName") %>'
                                            ToolTip="Customer Name"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account No" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrime" runat="server" Text='<%# Bind("PANum") %>' ToolTip="Account No"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sub Account No" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSub" runat="server" Text='<%# Bind("SANum") %>' ToolTip="Sub Account No"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                           <asp:TemplateField HeaderText="Principal" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldetGrossprincipal" runat="server" Text='<%# Bind("Gross_principal1") %>' ToolTip="Principal"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbldettotGrossPrincipal" runat="server" ToolTip="Total Gross Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UMFC" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldetUMFC" runat="server" Text='<%# Bind("UMFC1") %>' ToolTip="UMFC"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbldettotUMFC" runat="server" ToolTip="Total UMFC"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Gross Stock" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldetgrossstock" runat="server" Text='<%# Bind("gross_stock1") %>'
                                            ToolTip="Gross Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbldettotgrossstock" runat="server" ToolTip="Gross Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                  <asp:TemplateField HeaderText="NOI" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldetNOI" runat="server" Text='<%# Bind("NOI") %>'
                                            ToolTip="Gross Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbldettotNOI" runat="server" ToolTip="Gross Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Principal Outstanding" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldetNetPrincipal" runat="server" Text='<%# Bind("Net_Principal1") %>' ToolTip="Dues UnCollected Billed Finance Charges"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbldettotNetPrincipal" runat="server" ToolTip="Total Net Principal"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Interest Outstanding"  ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldetNIC" runat="server" Text='<%# Bind("Interest_OS1") %>' ToolTip="NO Of Installments"></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:Label ID="lbldettotinterestOutstanding" runat="server" ToolTip="Total Interest Outstanding"></asp:Label>
                                    </FooterTemplate>
                                      <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Future Installments" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldetfutureInstallments" runat="server" Text='<%# Bind("future_instal1") %>' ToolTip="Net Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbldettotfutureInstallments" runat="server" ToolTip="Total Future Installments"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Net Stock" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldetnetstock" runat="server" Text='<%# Bind("net_stock1") %>' ToolTip="Net Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbldettotnetstock" runat="server" ToolTip="Total Net Stock"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                             
                            </Columns>
                            <HeaderStyle CssClass="styleGridHeader" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td align="center">
                <asp:Button runat="server" ID="btnPrint" CssClass="styleSubmitButton" Text="Print"
                    CausesValidation="false" ValidationGroup="Print" Visible="false" OnClick="btnPrint_Click"
                    ToolTip="Print" />
            </td>
        </tr>
        <tr>
            <td height="8px"></td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary ID="vsStockRec" runat="server" CssClass="styleMandatoryLabel"
                    CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Ok" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CustomValidator ID="CVStockRec" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        function Resize() {
            if (document.getElementById('ctl00_ContentPlaceHolder1_divDemand') != null) {
                if (document.getElementById('divMenu').style.display == 'none') {
                    (document.getElementById('ctl00_ContentPlaceHolder1_divDemand')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
                }
                else {
                    (document.getElementById('ctl00_ContentPlaceHolder1_divDemand')).style.width = screen.width - 270;
                }
            }
        }


        function showMenu(show) {
            if (show == 'T') {

                if (document.getElementById('divGrid1') != null) {
                    document.getElementById('divGrid1').style.width = "800px";
                    document.getElementById('divGrid1').style.overflow = "scroll";
                }

                document.getElementById('divMenu').style.display = 'Block';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

                document.getElementById('ctl00_imgShowMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

                if (document.getElementById('ctl00_ContentPlaceHolder1_divDemand') != null)
                    (document.getElementById('ctl00_ContentPlaceHolder1_divDemand')).style.width = screen.width - 270;
            }
            if (show == 'F') {
                if (document.getElementById('divGrid1') != null) {
                    document.getElementById('divGrid1').style.width = "960px";
                    document.getElementById('divGrid1').style.overflow = "auto";
                }

                document.getElementById('divMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'none';
                document.getElementById('ctl00_imgShowMenu').style.display = 'Block';

                if (document.getElementById('ctl00_ContentPlaceHolder1_divDemand') != null)
                    (document.getElementById('ctl00_ContentPlaceHolder1_divDemand')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
            }
        }

    </script>
</ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
