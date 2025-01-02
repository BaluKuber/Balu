<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GOrgCreditParameterApproval_EN_Add.aspx.cs"
    Inherits="Origination_S3GOrgCreditParameterApproval_EN_Add" MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ PreviousPageType VirtualPath="~/Origination/S3GOrgCreditParameterApproval.aspx" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="ContentCreditParameterApproval" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="Server">
    <script type="text/javascript">
        function fnTogglePanel() {
            if (document.getElementById('ctl00_ContentPlaceHolder1_tcCPA_TBCusMode_pnlReamarks').style.display == 'block') {
                document.getElementById('ctl00_ContentPlaceHolder1_tcCPA_TBCusMode_pnlReamarks').style.display = 'none';
            }
            else {
                document.getElementById('ctl00_ContentPlaceHolder1_tcCPA_TBCusMode_pnlReamarks').style.display = 'block';
            }
        }
    </script>

    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <asp:Label runat="server" Text="Approval Based on Enquiry Number" ID="lblHeading"
                    CssClass="styleDisplayLabel">
                </asp:Label>
            </td>
        </tr>
        <%-- <asp:Label ID="lblEmployeeId" runat="server" Text='<%#Eval("Modified_By")%>' />
                                            ---%>
        <tr>
            <td height="10px"></td>
        </tr>
    </table>
    <cc1:TabContainer ID="tcCPA" runat="server" Width="100%" CssClass="styleTabPanel"
        ActiveTabIndex="1">
        <cc1:TabPanel TabIndex="1" runat="server" ID="TPABEN" CssClass="tabpan" BackColor="Red"
            Width="100%" HeaderText="Approval Based on Enquiry Number">
            <ContentTemplate>
                <table width="100%" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="Panel_Tap" runat="server" GroupingText="Sanction Details" CssClass="stylePanel">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 20%">
                                                        <asp:Label runat="server" Text="Approval for" ID="lblApprovalFor" CssClass="styleDisplayLabel" />
                                                    </td>
                                                    <td style="width: 30%">
                                                        <asp:TextBox Text="Enquiry" ReadOnly="True" ID="txtApprovalFor" runat="server" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 20%"></td>
                                                    <td style="width: 22%">
                                                        <asp:TextBox ID="txtReqApprovals" runat="server" Width="5px" Visible="False"></asp:TextBox>
                                                        <asp:TextBox ID="txtReqApprovalDone" runat="server" Width="5px" Visible="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%">
                                                        <asp:Label runat="server" Text="Sanction Number" ID="lblSanctionNumber" CssClass="styleDisplayLabel" />
                                                    </td>
                                                    <td style="width: 30%">
                                                        <asp:TextBox ReadOnly="True" ID="txtSanctionNumber" runat="server" Width="160px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 20%">
                                                        <asp:Label runat="server" Text="Sanction Date" ID="lblSanctionDate" CssClass="styleDisplayLabel" />
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox ReadOnly="True" ID="txtSanctionDate" runat="server" Width="80px"></asp:TextBox>
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
                            <asp:Panel ID="Panel1" runat="server" GroupingText="Document Number Details" CssClass="stylePanel">
                                <table width="100%">
                                    <tr>
                                        <td width="20%" valign="top">
                                            <asp:Label ID="lblLOB" runat="server" CssClass="styleDisplayLabel" Text="Line of Business"></asp:Label>
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txtLOB" runat="server" ReadOnly="True" Width="160px"></asp:TextBox>
                                        </td>
                                        <td width="20%" valign="top">
                                            <asp:Label ID="lblEnquiryNumber" runat="server" CssClass="styleDisplayLabel" Text="Document Number"></asp:Label>
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txtEnquiryNumber" runat="server" ReadOnly="True" Width="160px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="20%" valign="top">
                                            <asp:Label ID="lblBranch" runat="server" CssClass="styleDisplayLabel" Text="Location"></asp:Label>
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txtBranch" runat="server" ReadOnly="True" Width="160px"></asp:TextBox>
                                        </td>
                                        <td width="20%" valign="top">
                                            <asp:Label ID="lblEnquiryDate" runat="server" CssClass="styleDisplayLabel" Text="Document Date"></asp:Label>
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txtEnquiryDate" runat="server" ReadOnly="True" Width="80px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="20%" valign="top">
                                            <asp:Label runat="server" Text="Product" ID="lblProduct" CssClass="styleDisplayLabel" />
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txtProduct" runat="server" ReadOnly="True" Width="230px"></asp:TextBox>
                                        </td>
                                        <td width="20%" valign="top">
                                            <asp:Label ID="lblAssetDetails" runat="server" CssClass="styleDisplayLabel" Text="Asset Details"></asp:Label>
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txtAssetDetails" runat="server" ReadOnly="True" Width="230px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="20%" valign="top">
                                            <asp:Label ID="lblCustomerCode" runat="server" CssClass="styleDisplayLabel" Text="Customer Code"></asp:Label>
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txtCustomerCode" runat="server" ReadOnly="True" Width="160px"></asp:TextBox>
                                        </td>
                                        <td width="20%" valign="top">
                                            <asp:Label ID="lblAssetAmount" runat="server" CssClass="styleDisplayLabel" Text="Asset Amount"></asp:Label>
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txtAssetAmount" runat="server" ReadOnly="True" Width="90px" Style="text-align: right"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="20%" valign="top">
                                            <asp:Label ID="lblCustomerName" runat="server" CssClass="styleDisplayLabel" Text="Customer Name"></asp:Label>
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txtCustomerName" runat="server" ReadOnly="True" Width="230px"></asp:TextBox>
                                        </td>
                                        <td width="20%" valign="top">
                                            <asp:Label ID="lblOfferCard" runat="server" CssClass="styleDisplayLabel" Text="Offer Card"></asp:Label>
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txtOfferCard" runat="server" ReadOnly="True" Width="90px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="20%" valign="top">
                                            <asp:Label ID="lblRequFinAmt" runat="server" CssClass="styleDisplayLabel" Text="Required Finance Amount"></asp:Label>
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txtRequFinAmt" runat="server" ReadOnly="True" Width="90px" Style="text-align: right"></asp:TextBox>
                                        </td>
                                        <td width="20%" valign="top">
                                            <asp:Label ID="lblSanctionedAmt" runat="server" CssClass="styleDisplayLabel" Text="Sanctioned Amount"></asp:Label>
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txtSanctionedAmt" runat="server" ReadOnly="True" Width="90px" Style="text-align: right"></asp:TextBox>
                                            <input type="hidden" value="0" runat="server" id="hdnID"></input>
                                            <asp:LinkButton ID="ReqID" runat="server" CausesValidation="False" Text="View Details"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" width="20%">Guide Line Value
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txtGuideLineValue" runat="server" Enabled="False" ReadOnly="True"
                                                Style="text-align: right" Width="90px"></asp:TextBox></td>
                                        <td valign="top" width="20%">&nbsp;
                                        </td>
                                        <td width="30%">&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%">
                            <asp:Panel ID="Panel2" runat="server" GroupingText="Approval Details" CssClass="stylePanel">
                                <table width="100%">
                                    <tr>
                                        <td width="100%">
                                            <asp:UpdatePanel ID="updEnqDetails" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:GridView ID="gvApprovalDetails" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvApprovalDetails_RowDataBound"
                                                        Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Approval S.No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblApprovalSno" runat="server" Text='<%#Eval("ApprovalSno")%>'></asp:Label><asp:Label
                                                                        ID="lblApproverID" Visible="false" runat="server" Text='<%#Eval("ApproverID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="12%" CssClass="styleGridHeader" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="User Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpName" runat="server" Text='<%#Eval("EmployeeName")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="16%" CssClass="styleGridHeader" />
                                                                <ItemStyle Width="16%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Approval Status">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkApprove" OnCheckedChanged="chkApprove_CheckedChanged" runat="server"
                                                                        AutoPostBack="true" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "ApprovalStatus")))%>'
                                                                        Enabled="false" />
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="styleGridHeader" />
                                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Approval Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblApprovalDate" runat="server" Text='<%# Bind("ApprovalDate")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="14%" CssClass="styleGridHeader" />
                                                                <ItemStyle Width="14%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remark">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtRemark" runat="server" Width="200px" Height="35px" Text='<%# Bind("Remark") %>'
                                                                        ReadOnly="true" onkeyup="maxlengthfortxt(80);" onchange="maxlengthfortxt(80);"
                                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="38%" CssClass="styleGridHeader" />
                                                                <ItemStyle HorizontalAlign="Center" Width="38%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Password">
                                                                <ItemTemplate>
                                                                    <asp:TextBox EnableViewState="true" ID="txtPassword" runat="server" MaxLength="15"
                                                                        Enabled="false" onfocus="this.select();" AutoPostBack="False" Text='<%# Bind("Password") %>'
                                                                        TextMode="Password" Width="95%"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="10%" CssClass="styleGridHeader" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <RowStyle ForeColor="Blue" HorizontalAlign="Left" />
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%">
                                <tr width="100%" align="center">
                                    <td align="center">
                                        <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" OnClick="Save_Click"
                                            Text="Save" />
                                        &nbsp;&nbsp;
                                        <asp:Button runat="server" ID="btnMainPage" Text="Cancel" OnClick="MainPage_Click"
                                            CssClass="styleSubmitButton" />
                                        &nbsp; &nbsp;
                                        <asp:Button runat="server" Visible="False" ID="btnCancel" Text="Cancel" OnClick="Cancel_Click"
                                            CssClass="styleSubmitButton" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="10px"></td>
                    </tr>
                </table>
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel TabIndex="2" runat="server" ID="TBCusMode" CssClass="tabpan" BackColor="Red"
            Width="100%" HeaderText="Approval Based on Customer Code">
            <ContentTemplate>
                <table width="100%" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Panel ID="Panel4" runat="server" GroupingText="Sanction Details" CssClass="stylePanel">
                                <table width="100%">
                                    <tr width="100%">
                                        <td width="25%" valign="top">
                                            <asp:Label runat="server" Text="Approval for" ID="lblCusApprovalFor" CssClass="styleDisplayLabel" />
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox Text="Customer" ReadOnly="True" ID="txtCusApprovalFor" runat="server"
                                                Width="70px"></asp:TextBox></td>
                                        <td>
                                            <asp:Label ID="lblCusSanctionDate" runat="server" CssClass="styleDisplayLabel" Text="Sanction Date"></asp:Label></td>
                                        <td>
                                            <asp:TextBox runat="server" ReadOnly="True" Width="80px" ID="txtCusSanctionDate"></asp:TextBox><asp:LinkButton
                                                Text="View Details" CausesValidation="False" runat="server" ID="ReqID2"></asp:LinkButton></td>
                                    </tr>
                                    <tr width="100%">
                                        <td width="25%" valign="top">
                                            <asp:Label runat="server" Text="Sanction Number" ID="lblCusSanctionNumber" CssClass="styleDisplayLabel" />
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox ReadOnly="True" ID="txtCusSanctionNumber" runat="server" Width="160px"></asp:TextBox></td>
                                        <td width="25%" valign="top" cssclass="styleDisplayLabel">
                                            <asp:Label runat="server" Text="Password" ID="Label1" CssClass="styleDisplayLabel" />
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox ID="txtCusPassword" runat="server" TextMode="Password" Width="160px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td width="25%" valign="top">
                                            <asp:Label runat="server" Text="Customer Code" ID="lblCusCustomerCode" CssClass="styleDisplayLabel" />
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox ReadOnly="True" ID="txtCusCustomerCode" runat="server" Width="160px"></asp:TextBox></td>
                                        <td width="25%" valign="top">
                                            <asp:Label runat="server" Text="Customer Name" ID="lblCusCustomerName" CssClass="styleDisplayLabel" />
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox ReadOnly="True" ID="txtCusCustomerName" runat="server" Width="160px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            
                                        </td>
                                        <td valign="top">
                                            <asp:Label runat="server" Text="Remarks" ID="Label8" CssClass="styleDisplayLabel" />
                                            &nbsp;&nbsp;&nbsp;
                                            <img src="../Images/Task_pending.jpg" onclick="fnTogglePanel();" runat="server" id="imgRemarksHistory" style="width:23px; position:absolute; cursor:pointer"  title="View History"/>
                                            <asp:Panel ID="pnlReamarks" runat="server" style="display:none; right:200px; position:absolute" BackColor="White">
                                                 <asp:GridView ID="grvRemarks" runat="server" AutoGenerateColumns="false" ShowFooter="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl.No">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblSlNo"  Text='<%# Bind("Approval_Serial_No") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Date">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblDate"  Text='<%# Bind("Approval_Date") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="User">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblRemarksUser"  Text='<%# Bind("User_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblRemarks"  Text='<%# Bind("Remarks") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRemark" runat="server" Width="200px" Height="35px" Text='<%# Bind("Remark") %>'
                                                onkeyup="maxlengthfortxt(80);" onchange="maxlengthfortxt(80);"
                                                TextMode="MultiLine" Rows="3"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="10px"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="Panel3" runat="server" HorizontalAlign="Center" CssClass="stylePanel"
                                Width="99%">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updEnqCustDetails">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td width="100%" align="center">
                                                    <table cellpadding="0" cellspacing="0" runat="server" id="tblFacility">
                                                        <tr>
                                                            <td>
                                                                <table class="styleGridView" width="100%" cellspacing="0">
                                                                    <tr>
                                                                        <th class="stylePagingControl" align="left">
                                                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td>&nbsp;&nbsp;<asp:Label ID="lblCaption" runat="server" Text="FACILITY APPROVAL" />
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <span style="font-weight: normal; display: none">Total</span>
                                                                                    </td>
                                                                                    <td style="width: 100px" align="right">
                                                                                        <asp:Label ID="lblTotalAmount" Style="font-weight: normal" runat="server" CssClass="txtRightAlign" Visible="false"></asp:Label>
                                                                                    </td>

                                                                                    <td style="width: 65px" align="center"></td>
                                                                                </tr>
                                                                            </table>
                                                                        </th>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-top: 10px">
                                                                <table class="styleGridView" width="100%" cellspacing="0">
                                                                    <tr>
                                                                        <th class="stylePagingControl" align="left">
                                                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td width="160px" align="center">
                                                                                        <asp:Label ID="Label2" runat="server" Text="Line of Bussiness" />
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="Label4" runat="server" Text="Product" />
                                                                                    </td>
                                                                                    <td width="100px" align="center">
                                                                                        <asp:Label ID="Label5" runat="server" Text="Facility Amount" />
                                                                                    </td>
                                                                                    <td width="100px" align="center">
                                                                                        <asp:Label ID="Label3" runat="server" Text="Sanctioned Amount" />
                                                                                    </td>
                                                                                    <td width="100px" align="center">
                                                                                        <asp:Label ID="Label6" runat="server" Text="Approved Amount" />
                                                                                    </td>
                                                                                    <td style="width: 65px" align="center" runat="server" id="tdAction">
                                                                                        <asp:Label ID="Label7" runat="server" Text="Action" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </th>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-top: 10px">
                                                                <asp:GridView ID="grvFacilityGroup" runat="server" ShowFooter="true" ShowHeader="false" Style="min-width: 500px; border: none;" GridLines="None"
                                                                    AutoGenerateColumns="false" OnRowDataBound="grvFacilityGroup_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <table width="100%" class="styleGridView" cellspacing="0" cellpadding="0" style="border-bottom: none; padding: 0px;">
                                                                                    <tr>
                                                                                        <th align="left" style="height: 20px; width: 100%" class="stylePagingControl">
                                                                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Label Visible="false" ID="lblHGroupId" runat="server" Text='<%#Eval("Group_ID")%>' />
                                                                                                        &nbsp;&nbsp;<asp:Label ID="lblhead1" runat="server" Text="Group : " />
                                                                                                        <asp:Label ID="lblHGroup" runat="server" Text='<%#Eval("Group_Text")%>' />
                                                                                                    </td>
                                                                                                    <td align="right">
                                                                                                        <%--<span style="font-weight: normal">Sub-Total</span>--%>
                                                                                                    </td>
                                                                                                    <td style="width: 102px">
                                                                                                        <asp:Label ID="lblHFacilityAmount" runat="server" Width="100px" Text='<%#Eval("Total_Amount")%>' CssClass="txtRightAlign"></asp:Label>
                                                                                                        <asp:TextBox ID="txtHFacilityAmount" Visible="true" MaxLength="10" runat="server" Text='<%#Eval("Total_Amount")%>'
                                                                                                            Width="100px" CssClass="txtRightAlign" AutoPostBack="true" OnTextChanged="txtHFacilityAmount_TextChanged"></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="fteAmount1" runat="server" TargetControlID="txtHFacilityAmount"
                                                                                                            FilterType="Numbers" Enabled="True">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </td>
                                                                                                    <td style="width: 100px">
                                                                                                        <asp:Label ID="lblHSanctionedAmount" runat="server" Width="100px" Text='<%#Eval("Sanctioned_Group_Limit")%>' CssClass="txtRightAlign"></asp:Label>
                                                                                                    </td>
                                                                                                    <td style="width: 102px">
                                                                                                        <asp:Label ID="lblHApprovedAmount" runat="server" Width="100px" Text='<%#Eval("Final_Groupt_Sanctioned")%>' CssClass="txtRightAlign"></asp:Label>
                                                                                                        <asp:TextBox ID="txtHApprovedAmount" Visible="true" MaxLength="10" runat="server" Text='<%#Eval("Final_Groupt_Sanctioned")%>'
                                                                                                            Width="100px" CssClass="txtRightAlign" AutoPostBack="true" OnTextChanged="txtHApprovedAmount_TextChanged"></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtHApprovedAmount"
                                                                                                            FilterType="Numbers" Enabled="True">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </td>
                                                                                                    <td style="width: 63px" align="center" runat="server" id="tdGroupDelete">
                                                                                                        <asp:ImageButton ID="imgbtnGroupDelete" OnClick="imgbtnGroupDelete_OnClick" runat="server" ImageUrl="~/Images/delete1.png" Style="width: 13px" ToolTip="Delete Group" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </th>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td width="100%" style="padding: 0px;">
                                                                                            <asp:GridView ShowHeader="false" ID="grvFacility" Style="padding: 0px; border-left: none; border-right: none;" Width="100%" runat="server" AutoGenerateColumns="false" OnRowDataBound="grvFacility_RowDataBound" ShowFooter="true">
                                                                                                <Columns>
                                                                                                    <asp:TemplateField HeaderText="Line of Bussiness" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="160px" FooterStyle-Width="160px">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label Visible="false" ID="lblLOBId" runat="server" Text='<%#Eval("LOB_ID")%>' />
                                                                                                            <asp:Label ID="lblLOB" runat="server" Text='<%#Eval("LOB")%>' />
                                                                                                            <asp:DropDownList ID="ddlLOB" Visible="false" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                                                                                AutoPostBack="true" runat="server">
                                                                                                            </asp:DropDownList>
                                                                                                        </ItemTemplate>
                                                                                                        <FooterTemplate>
                                                                                                            <asp:DropDownList ID="ddlFLOB" runat="server" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                                                                                AutoPostBack="true">
                                                                                                            </asp:DropDownList>
                                                                                                        </FooterTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Product" ItemStyle-HorizontalAlign="Left">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label Visible="false" ID="lblProductId" runat="server" Text='<%#Eval("Product_ID")%>' />
                                                                                                            <asp:Label ID="lblProduct" runat="server" Text='<%#Eval("Product")%>' />
                                                                                                            <asp:DropDownList Visible="false" ID="ddlProduct" runat="server" Style="min-width: 120px">
                                                                                                            </asp:DropDownList>
                                                                                                        </ItemTemplate>
                                                                                                        <FooterTemplate>
                                                                                                            <asp:DropDownList ID="ddlFProduct" runat="server" Style="min-width: 120px">
                                                                                                            </asp:DropDownList>
                                                                                                        </FooterTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <%--<asp:TemplateField HeaderText="Grouping">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label Visible="false" ID="lblGroupId" runat="server" Text='<%#Eval("Group_ID")%>' />
                                                                                                                <asp:DropDownList ID="ddlGroup" runat="server">
                                                                                                                </asp:DropDownList>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:DropDownList ID="ddlFGroup" runat="server">
                                                                                                                </asp:DropDownList>
                                                                                                            </FooterTemplate>
                                                                                                        </asp:TemplateField>--%>
                                                                                                    <asp:TemplateField HeaderText="Facility Amount" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" FooterStyle-Width="100px">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblFacilityAmount" runat="server" Text='<%#Eval("Facility_Amount")%>'></asp:Label>
                                                                                                            <asp:TextBox ID="txtFacilityAmount" Visible="false" MaxLength="10" runat="server" Text='<%#Eval("Facility_Amount")%>'
                                                                                                                Width="100px" CssClass="txtRightAlign" AutoPostBack="true" OnTextChanged="txtFacilityAmount_TextChanged"></asp:TextBox>
                                                                                                            <cc1:FilteredTextBoxExtender ID="fteAmount1" runat="server" TargetControlID="txtFacilityAmount"
                                                                                                                FilterType="Numbers" Enabled="True">
                                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                                        </ItemTemplate>
                                                                                                        <FooterTemplate>
                                                                                                            <asp:TextBox ID="txtFFacilityAmount" MaxLength="10" runat="server" AutoPostBack="true" OnTextChanged="txtFFacilityAmount_TextChanged"
                                                                                                                Width="100px" CssClass="txtRightAlign"></asp:TextBox>
                                                                                                            <cc1:FilteredTextBoxExtender ID="fteFAmount1" runat="server" TargetControlID="txtFFacilityAmount"
                                                                                                                FilterType="Numbers" Enabled="True">
                                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                                        </FooterTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Sanctioned Amount" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" FooterStyle-Width="100px">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblSanctionedAmount" runat="server" Text='<%#Eval("Sanctioned_limit")%>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <FooterTemplate>
                                                                                                            <asp:Label ID="lblFSanctionedAmount" runat="server"></asp:Label>
                                                                                                        </FooterTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Approved Amount" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" FooterStyle-Width="100px">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblApprovedAmount" runat="server" Text='<%#Eval("Final_Sanctioned_Limit")%>'></asp:Label>
                                                                                                            <asp:TextBox ID="txtApprovedAmount" Visible="false" MaxLength="10" runat="server" Text='<%#Eval("Final_Sanctioned_Limit")%>'
                                                                                                                Width="100px" CssClass="txtRightAlign"></asp:TextBox>
                                                                                                            <cc1:FilteredTextBoxExtender ID="fteApproved" runat="server" TargetControlID="txtApprovedAmount"
                                                                                                                FilterType="Numbers" Enabled="True">
                                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                                        </ItemTemplate>
                                                                                                        <FooterTemplate>
                                                                                                            <asp:TextBox ID="txtFApprovedAmount" MaxLength="10" runat="server"
                                                                                                                Width="100px" CssClass="txtRightAlign"></asp:TextBox>
                                                                                                            <cc1:FilteredTextBoxExtender ID="fteFApprovedAmount" runat="server" TargetControlID="txtFApprovedAmount"
                                                                                                                FilterType="Numbers" Enabled="True">
                                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                                        </FooterTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Action" ItemStyle-Width="60px" FooterStyle-Width="60px">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Panel runat="server" ID="pnlItemActions">
                                                                                                                <asp:ImageButton ID="imgbtnEdit" runat="server" ImageUrl="~/Images/Blue_2/modify_blue.gif" OnClick="imgbtnEdit_OnClick" ToolTip="Edit" />
                                                                                                                &nbsp;
                                                                                                                <asp:ImageButton ID="imgbtnDelete" OnClick="imgbtnDelete_OnClick" runat="server" ImageUrl="~/Images/delete1.png" Style="width: 13px" ToolTip="Delete" />
                                                                                                            </asp:Panel>
                                                                                                            <asp:Panel runat="server" ID="pnlEditActions" Visible="false">
                                                                                                                <asp:ImageButton ID="imgbtnUpdate" runat="server" ImageUrl="~/Images/Blue_2/modify_blue.gif" OnClick="imgbtnUpdate_OnClick" ToolTip="Update" />
                                                                                                                &nbsp;
                                                                                                                <asp:ImageButton ID="imgbtnCancel" OnClick="imgbtnCancel_OnClick" runat="server" ImageUrl="~/Images/uparrow_07.png" Style="width: 15px; height: 15px" ToolTip="Cancel" />
                                                                                                            </asp:Panel>
                                                                                                            <%--<asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="true" ValidationGroup="Inflow"
                                                                            OnClick="btnAdd_OnClick" CssClass="styleGridShortButton" />--%>
                                                                                                        </ItemTemplate>
                                                                                                        <FooterTemplate>
                                                                                                            <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="true" ValidationGroup="Grid"
                                                                                                                OnClick="btnAdd_OnClick" CssClass="styleGridShortButton" />
                                                                                                        </FooterTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <div style="height: 10px; border: none"></div>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <table cellpadding="0" class="styleGridView" cellspacing="0" width="100%">
                                                                                    <tr>
                                                                                        <td align="left">&nbsp;&nbsp;<asp:Label ID="lblFoot1" runat="server" Text="Group : " />
                                                                                            <asp:DropDownList ID="ddlHFGroup" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlHFGroup_SelectedIndexChanged">
                                                                                            </asp:DropDownList>
                                                                                            <asp:Label runat="server" ID="lblSpace" Width="150px"></asp:Label>
                                                                                        </td>
                                                                                        <td align="right">
                                                                                            <span style="font-weight: normal">Sub-Total</span>
                                                                                        </td>
                                                                                        <td style="width: 100px">
                                                                                            <asp:TextBox ID="txtFFacilityAmount" MaxLength="10" runat="server"
                                                                                                Width="100px" CssClass="txtRightAlign"></asp:TextBox>
                                                                                            <cc1:FilteredTextBoxExtender ID="fteFFacilityAmount" runat="server" TargetControlID="txtFFacilityAmount"
                                                                                                FilterType="Numbers" Enabled="True">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </td>
                                                                                        <td style="width: 60px">
                                                                                            <asp:Button ID="btnHAdd" runat="server" Text="Add" CausesValidation="true"
                                                                                                OnClick="btnHAdd_OnClick" CssClass="styleGridShortButton" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvEnquiryByCustomer" Visible="false" runat="server" AutoGenerateColumns="False"
                                            OnRowDataBound="gvApprovalDetailsByCust_RowDataBound" Width="100%" ShowFooter="True"
                                            OnSelectedIndexChanged="gvEnquiryByCustomer_SelectedIndexChanged" OnRowCommand="gvApprovalDetailsByCust_RowCommand"
                                            OnRowEditing="gvApprovalDetailsByCust_RowEditing" OnRowUpdating="gvApprovalDetailsByCust_RowUpdating"
                                            OnRowDeleting="gvApprovalDetailsByCust_RowDeleting" OnRowCancelingEdit="gvApprovalDetailsByCust_RowCancelingEdit">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Line of Business">
                                                    <ItemTemplate>
                                                        <asp:Label Visible="false" ID="lblCusOfferCard" runat="server" Text="" />
                                                        <asp:Label Visible="false" ID="lblNewRecord" runat="server" Text='<%#Eval("newRow")%>' />
                                                        <asp:Label Visible="false" ID="lblCusCreditParameterApprovalID" runat="server" Text='<%#Eval("Credit_Parameter_Approval_ID")%>' />
                                                        <asp:Label Visible="false" ID="lblLOBId" runat="server" Text='<%#Eval("LOB_ID")%>' />
                                                        <asp:DropDownList ID="ddlCusLOB" OnSelectedIndexChanged="ddlCusLOB_SelectedIndexChanged"
                                                            AutoPostBack="true" runat="server" Width="100%">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlCusLOBedit" AutoPostBack="true" runat="server" Width="100%">
                                                        </asp:DropDownList>
                                                        <%--OnSelectedIndexChanged="ddlCusLOBedit_SelectedIndexChanged"--%>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlCusLOB1" runat="server" Width="100%">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Required Facility">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCusRequiredFacility" runat="server" AutoPostBack="true" Text='<%#Eval("Required_facility")%>'
                                                            OnTextChanged="txtCusRequiredFacility_TextChanged" CssClass="txtRightAlign" onkeypress="fnAllowNumbersOnly(true,false,this);"
                                                            Enabled="false" Width="100px" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtCusRequiredFacility1" runat="server" AutoPostBack="true" CssClass="txtRightAlign"
                                                            onkeypress="fnAllowNumbersOnly(true,false,this);" Width="100px" MaxLength="10"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtCusRequiredFacility" runat="server" AutoPostBack="true" OnTextChanged="txtCusRequiredFacility_TextChanged"
                                                            CssClass="txtRightAlign" onkeypress="fnAllowNumbersOnly(true,false,this);" Width="100px"
                                                            MaxLength="10"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Center" Width="12%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sanctioned Limit">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCusSanctionedLimit" runat="server" Text='<%#Eval("Sanctioned_Limit")%>'
                                                            MaxLength="10" CssClass="txtRightAlign" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                            Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtCusSanctionedLimit1" runat="server" MaxLength="10" CssClass="txtRightAlign"
                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" Width="100px"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtCusSanctionedLimit" runat="server" MaxLength="10" CssClass="txtRightAlign"
                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" Width="100px"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Center" Width="13%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Over Ride">
                                                    <ItemTemplate>
                                                        <asp:DropDownList AutoPostBack="true" ID="ddlCusOverRide" OnSelectedIndexChanged="DdlCusOverRide_SelectedIndexChanged"
                                                            runat="server">
                                                            <asp:ListItem Text="Enable" Value="0"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Disable" Value="1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList AutoPostBack="true" ID="ddlCusOverRide1" runat="server">
                                                            <asp:ListItem Text="Enable" Value="0"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Disable" Value="1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList AutoPostBack="true" ID="ddlCusOverRide" OnSelectedIndexChanged="DdlCusOverRide_SelectedIndexChanged"
                                                            runat="server">
                                                            <asp:ListItem Text="Enable" Value="0"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Disable" Value="1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Center" Width="12%" />
                                                    <FooterStyle HorizontalAlign="Center" Width="12%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="User Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("User_Name")%>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("User_Name")%>' />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("User_Name")%>' />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Center" Width="12%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Final Sanctioned Amount">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCusFinalSanctionedAmount" MaxLength="10" runat="server" Text='<%#Eval("Final_Sanctioned_Limit")%>'
                                                            Width="100px" CssClass="txtRightAlign" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtCusFinalSanctionedAmount1" MaxLength="10" runat="server" Width="100px"
                                                            CssClass="txtRightAlign" onkeypress="fnAllowNumbersOnly(true,false,this)" />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtCusFinalSanctionedAmount" MaxLength="10" runat="server" Width="100px" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Center" Width="13%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Approved Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtApprovedDate" runat="server" Text='<%#Eval("Approved_Date")%>'
                                                            Enabled="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="txtApprovedDate" runat="server" Text='<%#Eval("Approved_Date")%>'
                                                            Enabled="false"></asp:Label>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="txtApprovedDate" runat="server" Text='<%#Eval("Approved_Date")%>'></asp:Label>
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" Visible="true">
                                                    <%--<asp:CommandField ButtonType="Button" SelectText="Add" ShowSelectButton="False" ItemStyle-Width="8%"
                                                    ControlStyle-Width="75px"  />--%>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" ToolTip="Edit"
                                                            CausesValidation="false"></asp:LinkButton>&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkRemove"
                                                                runat="server" Text="Delete" ToolTip="Delete" CommandName="Delete" OnClientClick="return confirm('Do you want to delete?');"
                                                                CausesValidation="false"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" ToolTip="Update" CommandName="Update"
                                                            CausesValidation="false" />
                                                        <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" ToolTip="Cancel" CommandName="Cancel"
                                                            CausesValidation="false" />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lnkAdd" runat="server" Text="Add" ToolTip="Add" CommandName="Add"
                                                            ValidationGroup="vgAdd" CausesValidation="false" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td height="10px"></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <hr xml:lang />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%">
                                <tr width="100%" align="center">
                                    <td align="center">
                                        <asp:Button runat="server" ID="btnCusMainPage" Text="Main Page" Visible="False" OnClick="MainPage_Click"
                                            CssClass="styleSubmitButton" />
                                        &nbsp;
                                        <asp:Button runat="server" ID="btnCusSave" Text="Save" OnClick="SaveCus_Click" CssClass="styleSubmitButton"
                                            Style="height: 26px" />
                                        &nbsp;
                                        <asp:Button runat="server" ID="btnCusCancel" Text="Cancel" OnClick="CancelCus_Click"
                                            CssClass="styleSubmitButton" CausesValidation="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="10px"></td>
                    </tr>
                </table>
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel TabIndex="3" runat="server" ID="TPScoreBoard" CssClass="tabpan" BackColor="Red"
            Width="100%" HeaderText="Score Board">
            <ContentTemplate>
                <table width="100%" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="10px"></td>
                    </tr>
                    <tr>
                        <td align="center">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:GridView runat="server" ID="gvCScoreBoard" Width="75%" AutoGenerateColumns="False"
                                CellSpacing="8" OnRowDataBound="gvCScoreBoard_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Credit Score">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreditScore" runat="server" Text='<%#Eval("CreditScore")%>' Style="padding-left: 10px"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" Width="40%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="% of Importance">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPercentOfImport" runat="server" Text='<%#Eval("PercentageOfImportance")%>'
                                                Style="padding-right: 10px"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right" Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Required Score">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequiredScore" runat="server" Text='<%#Eval("RequiredScore")%>'
                                                Style="padding-right: 10px"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right" Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual Score">
                                        <ItemTemplate>
                                            <asp:Label ID="lblActualScore" runat="server" Text='<%#Eval("ActualScore")%>' Style="padding-right: 10px"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right" Width="20%" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle ForeColor="Blue" HorizontalAlign="Left" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%">
                                <tr width="100%" align="center">
                                    <td align="center">
                                        <asp:Button runat="server" ID="CmdClick" Text="Cancel" OnClick="MainPage_Click" CssClass="styleSubmitButton" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>
</asp:Content>
