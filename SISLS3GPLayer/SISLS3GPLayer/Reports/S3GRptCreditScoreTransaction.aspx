<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GRptCreditScoreTransaction.aspx.cs" Inherits="Reports_S3GRptCreditScoreTransaction"
    Title="Credit Score Transaction" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Credit Score Transaction Report" ID="lblHeading"
                                        CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="8px">
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel runat="server" ID="pnlinputcriteria" CssClass="stylePanel" GroupingText="Input Criteria"
                            Width="100%">
                            <table width="100%">
                                <tr>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label ID="lblLOB" runat="server" Text="Line Of Business" CssClass="styleReqFieldLabel" ToolTip="Line Of Business">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:DropDownList ID="ddlLOB" runat="server" CausesValidation="true" Width="85%"
                                            OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged" AutoPostBack="true" ToolTip="Line Of Business">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvLOB" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlLOB" InitialValue="-1" ErrorMessage="Select Line of Business"
                                            Display="None" ValidationGroup="Go">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label ID="lblBranch" runat="server" Text="Location 1" CssClass="StyleReqFieldLabel" ToolTip="Location 1">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:DropDownList ID="ddlBranch" runat="server" Visible="true" Width="99%" ToolTip="Location 1" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                <td class="styleFieldLabel" width="25%">
                                    <asp:Label ID="lbllocation" runat="server" Text="Location 2" CssClass="StyleReqFieldLabel" ToolTip="Location 2">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="25%">
                                    <asp:DropDownList ID="ddllocation2" runat="server" Visible="true" Width="99%" ToolTip="Location 2">
                                    </asp:DropDownList>
                                </td>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label ID="lblProduct" runat="server" Text="Product" CssClass="StyleReqFieldLabel" ToolTip="Product">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:DropDownList ID="ddlProduct" runat="server" Width="100%" ToolTip="Product">
                                        </asp:DropDownList>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="Start Date" ID="lblStartDate" CssClass="styleReqFieldLabel" ToolTip="Start Date">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox ID="txtStartDate" runat="server" Width="83%" ToolTip="Start Date"></asp:TextBox>
                                        <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtStartDate"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgStartDate"
                                            ID="CalendarExtender1">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Select Start Date"
                                            ValidationGroup="Go" Display="None" SetFocusOnError="True" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" width="25%">
                                        <asp:Label runat="server" Text="End Date" ID="lblEndDate" CssClass="styleReqFieldLabel" ToolTip="End Date">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox ID="txtEndDate" runat="server" Width="83%" ToolTip="End Date"></asp:TextBox>
                                        <asp:Image ID="ImgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtEndDate"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgEndDate"
                                            ID="CalendarExtender2">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="Select End Date"
                                            ValidationGroup="Go" Display="None" SetFocusOnError="True" ControlToValidate="txtEndDate">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" height="8px">
                        <tr class="styleButtonArea" style="padding-left: 4px">
                            <td align="center" colspan="4">
                                <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="styleSubmitButton"  OnClientClick="return fnCheckPageValidators('Go',false);" ValidationGroup="Go"
                                    OnClick="btnGo_Click" ToolTip="Go"/>
                                &nbsp; &nbsp;
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="styleSubmitButton"
                                    CausesValidation="false" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" ToolTip="Clear"/>
                            </td>
                        </tr>
                    </td>
                </tr>
                <tr>
                    <td height="8px">
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlcreditscoredetails" runat="server" CssClass="stylePanel" GroupingText="Credit Score Details"
                            Width="100%" Visible="false">
                            <asp:GridView ID="grvcreditscoredetails" runat="server" AutoGenerateColumns="False"
                                CssClass="styleInfoLabel" Width="99%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Location">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranchid" runat="server" Text='<%# Bind("BRANCH_ID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblProductid" runat="server" Text='<%# Bind("PRODUCT_ID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblBranchName" runat="server" Text='<%# Bind("BRANCH") %>' ToolTip="Location"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("PRODUCT") %>' ToolTip="Product"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEQC" runat="server" Text='<%# Bind("ECAType") %>' ToolTip="Type"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="No. of Enq/Customer">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNoofAccounts" runat="server" Text='<%# Bind("NOOFACCOUNTS") %>' ToolTip="No: of Accounts"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Required Score%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequiredScore" runat="server" Text='<%# Bind("REQUIREDSCORE") %>' ToolTip="Required Score%"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual Score%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblActualScore" runat="server" Text='<%# Bind("ACTUALSCORE") %>' ToolTip="Actual Score%"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hygiene%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHygiene" runat="server" Text='<%# Bind("HYGIENE") %>' ToolTip="Hygiene%"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Accepted%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LnkBtnAccp" runat="server" OnClick="LnkBtnAccp_Click" Text='<%# Bind("ACCEPTED") %>' ToolTip="Accepted%"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rejected%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LnkBtnRjtd" runat="server" OnClick="LnkBtnRjtd_Click" Text='<%# Bind("REJECTED") %>' ToolTip="Rejected%"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" />
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
                    <td align="right" colspan="2">
                        <asp:Label ID="lblAmounts" runat="server" Text="All Amounts are in" Visible="false"
                            CssClass="styleDisplayLabel"></asp:Label>
                        <asp:Label ID="lblCurrency" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlcustomersdetails" runat="server" CssClass="stylePanel" GroupingText="Accepted Customer Details"
                            Width="100%" Visible="false">
                            <div style="overflow: auto; height: 250px">
                                <asp:GridView ID="grvCustDet" runat="server" AutoGenerateColumns="False" CssClass="styleInfoLabel"
                                    Width="98%">
                                    <Columns>
                                    <asp:TemplateField HeaderText="Constitution">
                                            <ItemTemplate>
                                                <asp:Label ID="lblconstitution" runat="server" Text='<%# Bind("CONSTITUTION") %>' ToolTip="Constitution"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name & Address">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomername" runat="server" Text='<%# Eval("CUSTOMERNAMEADDRESS").ToString().Replace(";","<br>") %>' ToolTip="Customer Name & Address"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Enquiry Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblenquirydate" runat="server" Text='<%# Bind("ENQUIRYDATE") %>' ToolTip="Enquiry Date"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Application Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblApplicationdate" runat="server" Text='<%# Bind("APPLICATIONDATE") %>' ToolTip="Application Date"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" Width="10%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Address">
                                            <ItemTemplate>
                                                <asp:Label ID="lbladdress" runat="server" Text='<%# Bind("ADDRESS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" Width="10%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Loan Borrowed">
                                            <ItemTemplate>
                                                <asp:Label ID="lblloanborrowed" runat="server" Text='<%# Bind("LOANBORROWED") %>' ToolTip="Loan Borrowed"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Account Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblActAcc" runat="server" Text='<%# Bind("PANUM") %>' ToolTip="Account Number"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Account Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRjtAcc" runat="server" Text='<%# Bind("SANUM") %>' ToolTip="Sub Account Number"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstatus" runat="server" Text='<%# Bind("STATUS") %>' ToolTip="Status"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" Width="15%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlcustomersdetailsRej" runat="server" CssClass="stylePanel" GroupingText="Rejected Customer Details"
                            Width="100%" Visible="false">
                            <div style="overflow: auto; height: 250px">
                                <asp:GridView ID="grvcustdetrej" runat="server" AutoGenerateColumns="False" CssClass="styleInfoLabel" Width="98%">
                                    <Columns>
                                    <asp:TemplateField HeaderText="Constitution">
                                            <ItemTemplate>
                                                <asp:Label ID="lblconstitution" runat="server" Text='<%# Bind("CONSTITUTION") %>' ToolTip="Constitution"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name & Address">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomername" runat="server" Text='<%# Eval("CUSTOMERNAMEADDRESS").ToString().Replace(";","<br>") %>' ToolTip="Customer Name & Address"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Enquiry Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblenquirydate" runat="server" Text='<%# Bind("ENQUIRYDATE") %>' ToolTip="Enquiry Date"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Loan Requested">
                                            <ItemTemplate>
                                                <asp:Label ID="lblloanrequested" runat="server" Text='<%# Bind("LOANREQUESTED") %>' ToolTip="Loan Requested"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" Width="10%" />
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
                    <td align="center">
                        &nbsp; &nbsp;
                        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="styleSubmitButton"
                            OnClick="btnPrint_Click" Visible="false" ToolTip="Print"/>
                        &nbsp; &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="vscreditscoretransaction" HeaderText="Please correct the following validation(s):"
                            CssClass="styleSummaryStyle" ShowMessageBox="false" ValidationGroup="Go" ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CustomValidator ID="CVCreditScore" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
