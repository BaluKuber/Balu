<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" 
CodeFile="S3GRptProbableDelinquency.aspx.cs" Inherits="Reports_S3GRptProbableDelinquency" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Probable Delinquency Report" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--  <asp:Panel runat="server" ID="Panel1" CssClass="stylePanel" GroupingText="Demand Type"
                            Width="100%">--%>
                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label ID="lblDemandMonth" runat="server" CssClass="styleReqFieldLabel" Text="Demand Month">
                                    </asp:Label>
                                </td>
                                <td align="left" class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlDemandMonth" runat="server" AutoPostBack="True" 
                                    OnSelectedIndexChanged="ddlDemandMonth_SelectedIndexChanged" Visible="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvDemandMonth" runat="server" ControlToValidate="ddlDemandMonth"
                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Demand Month"
                                        InitialValue="0" ValidationGroup="Go"></asp:RequiredFieldValidator>
                                </td>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlLOB" runat="server" Visible="true" AutoPostBack="true" 
                                    OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvLOB" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="ddlLOB" InitialValue="0" ErrorMessage="Select Line of Business"
                                        Display="None" ValidationGroup="Go"></asp:RequiredFieldValidator>
                                </td>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Receipts Up to" ID="lblCutoffDate" CssClass="styleReqFieldLabel"></asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtCutoffDate" runat="server" ></asp:TextBox>
                                    <asp:Image ID="imgCutoffDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                    <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtCutoffDate"
                                        OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgCutoffDate"
                                        ID="CalendarExtender1">
                                    </cc1:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvCutoffDate" runat="server" ErrorMessage="Enter Receipts Up to"
                                        ValidationGroup="Go" Display="None" SetFocusOnError="True" ControlToValidate="txtCutoffDate"
                                        CssClass="styleMandatoryLabel"></asp:RequiredFieldValidator>
                                    <cc1:FilteredTextBoxExtender ID="fltCutoffDate" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                        TargetControlID="txtCutoffDate" ValidChars=" -,/" runat="server" Enabled="True">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                            <td height="8px">
                            </td>
                        </tr>
                            <tr>
                                <td colspan="6" align="center" style="padding-top: 5px">
                                    <asp:Button ID="btnGo" runat="server" CausesValidation="True" CssClass="styleSubmitButton"
                                        OnClick="btnGo_Click" Text="Go" ValidationGroup="Go"/>
                                    &nbsp;<asp:Button ID="btnClear" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                            OnClick="btnClear_Click" Text="Clear" />
                             &nbsp;<asp:Button ID="Button1" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                            OnClick="btnCancel_Click" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                        <%--  </asp:Panel>--%>
                    </td>
                </tr>
                <tr>
                <td></td>
                </tr>
                <tr>
                <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel runat="server" ID="pnlDemandProcess" CssClass="stylePanel" GroupingText="Demand Processing"
                            Width="100%" Visible="false">
                            <asp:GridView ShowFooter="true" runat="server" AutoGenerateColumns="false" Width="100%"
                                ID="grvDemandProcess" HeaderStyle-CssClass="styleGridHeader" HeaderStyle-HorizontalAlign="Center"
                                RowStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" 
                                 OnRowDataBound="grvDemandProcess_RowDataBound">
                                <Columns>
                                   <%-- <asp:TemplateField HeaderText="Sellect All" SortExpression="Sellect All" Visible="false">
                                        <HeaderTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="spnAll">Select All</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkAll" runat="server"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </HeaderTemplate>
                                        <HeaderStyle Width="10%" HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelectBranch" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "SelectLocation")))%> ' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Location">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                                            <asp:Label ID="lblProcess" runat="server" Text="0" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotal" runat="server" Text="Total" MaxLength="3"></asp:Label>
                                        </FooterTemplate>
                                        <HeaderStyle Width="30%" />
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="LocationId" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranch_Id" runat="server" Text='<%# Bind("Location_id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="No of Accounts">
                                        <ItemTemplate>
                                            <asp:Label Style="text-align: right" Width="100%" ID="lblNoofAccounts" runat="server"
                                                Text='<%# Bind("No_of_Accounts") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="txtNoofAcc" runat="server" Style="text-align: right;"></asp:Label>
                                        </FooterTemplate>
                                        <HeaderStyle Width="15%" />
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Arrear Due">
                                        <ItemTemplate>
                                            <asp:Label ID="lblArrearsAmount" runat="server" Text='<%# Bind("Arrears_Amount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="txtArrearsAmount" runat="server" Style="text-align: right"></asp:Label>
                                        </FooterTemplate>
                                        <HeaderStyle Width="15%" />
                                        <ItemStyle HorizontalAlign="Right" />
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField HeaderText="Current Due">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Current_Due") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="txtAmount" runat="server" Style="text-align: right"></asp:Label>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatus" runat="server" Text='<%# Bind("Status") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="txtstatus" runat="server" Style="text-align: right"></asp:Label>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField  HeaderText ="Query">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                             CommandName="Query" OnClick ="FunProShow_AccountLevel" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"  />
                                    </asp:TemplateField>
                                    <asp:TemplateField  HeaderText ="XL Porting">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnXL" Enabled="false" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEditDisabled"
                                               CommandName="XLPorting" OnClick="FunExcelExport" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"  />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                            <td height="8px">
                            </td>
                        </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnProcess" runat="server" CausesValidation="true" CssClass="styleSubmitButton"
                            OnClick="btnProcess_Click" Text="Process" ValidationGroup="Save" Visible="false"/>
                        &nbsp;<asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                            OnClick="btnCancel_Click" Text="Cancel" Visible="false" />
                    </td>
                </tr>
                <tr>
                <td>
                <asp:Panel runat="server" ID="Pnlprbldelinq" CssClass="stylePanel" GroupingText="Calculation Details"
                            Width="100%" Visible="false">
                            <asp:GridView ShowFooter="true" runat="server" AutoGenerateColumns="false" Width="100%"
                                ID="grvprbldelinq" HeaderStyle-CssClass="styleGridHeader" HeaderStyle-HorizontalAlign="Center"
                                RowStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" >
                                <Columns>
                                    <asp:TemplateField HeaderText="Location">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("LOCATION_NAME") %>'></asp:Label>
                                            <asp:Label ID="lblProcess" runat="server" Text="0" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="30%" />
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("CUSTOMER_NAME") %>'></asp:Label>
                                            <asp:Label ID="lblProcess" runat="server" Text="0" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="30%" />
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Account Number">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("PANUM") %>'></asp:Label>
                                            <asp:Label ID="lblProcess" runat="server" Text="0" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="30%" />
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sub Account Number">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("SANUM") %>'></asp:Label>
                                            <asp:Label ID="lblProcess" runat="server" Text="0" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotal" runat="server" Text="Total" MaxLength="3"></asp:Label>
                                        </FooterTemplate>
                                        <HeaderStyle Width="30%" />
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Arrear Due">
                                        <ItemTemplate>
                                            <asp:Label ID="lblArrearsAmount1" runat="server" Text='<%# Bind("ARREAR_DUE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="txtArrearsAmount1" runat="server" Style="text-align: right"></asp:Label>
                                        </FooterTemplate>
                                        <HeaderStyle Width="15%" />
                                        <ItemStyle HorizontalAlign="Right" />
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField HeaderText="Current Due">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount1" runat="server" Text='<%# Bind("CURRENT_DUE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="txtAmount1" runat="server" Style="text-align: right"></asp:Label>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle Width="15%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        </td>
                </tr>
                <tr>
                    <td>
                        <asp:CustomValidator ID="CVDemandProcessing" runat="server" Display="None" ValidationGroup="Go"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsDemandProcess" runat="server" CssClass="styleSummaryStyle"
                            HeaderText="Correct the following validation(s):" ShowMessageBox="false" ShowSummary="true"
                            ValidationGroup="Go" />
                    </td>
                </tr>
            </table>

</asp:Content>

