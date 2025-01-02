<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" 
CodeFile="S3GRptPricing.aspx.cs" Inherits="Reports_S3GRptPricing" Title="Pricing Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Pricing Report">
                </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlInward" runat="server" GroupingText="Input Criteria" CssClass="stylePanel"
                    Width="100%">
                    <table width="100%">
                        <tr>
                             <td class="styleFieldLabel">
                                <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel" ToolTip="Line of Business">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlLOB" runat="server" Width="85%" AutoPostBack="true" CausesValidation="true"
                                    OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged" ToolTip="Line of Business">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvLOB" CssClass="styleMandatoryLabel" runat="server"
                                    ControlToValidate="ddlLOB" InitialValue="0" ErrorMessage="Select Line of Business"
                                    Display="None" ValidationGroup="Go">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Location1" ID="lblLoc1" CssClass="styleReqFieldLabel"
                                    ToolTip="Location1">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlLoc1" runat="server" AutoPostBack="true" CausesValidation="true"
                                    ToolTip="Location1" CssClass="WindowsStyle" Width="225px" OnSelectedIndexChanged="ddlLoc1_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvLoc1" CssClass="styleMandatoryLabel" runat="server"
                                    ControlToValidate="ddlLoc1" InitialValue="0" ErrorMessage="Select Location1" Display="None"
                                    ValidationGroup="Go">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                             <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Location2" ID="lblLoc2" CssClass="styleDisplayLabel"
                                    ToolTip="Location2">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlLoc2" runat="server" AutoPostBack="true" CausesValidation="true"
                                    ToolTip="Location2" CssClass="WindowsStyle" Width="225px">
                                    <%--OnSelectedIndexChanged="ddlLoc2_SelectedIndexChanged"--%>
                                </asp:DropDownList>
                                <%--<asp:RequiredFieldValidator ID="rfvLoc2" CssClass="styleMandatoryLabel" runat="server"
                                    ControlToValidate="ddlLoc2" InitialValue="0" ErrorMessage="Select Location2" Display="None"
                                    ValidationGroup="Go">
                                </asp:RequiredFieldValidator>--%>
                            </td>
                             <td class="styleFieldLabel" width="20%">
                                <asp:Label ID="lblDenomination" runat="server" Text="Denomination" CssClass="styleReqFieldLabel"
                                    ToolTip="Denomination">
                                </asp:Label>
                        </td>
                        <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlDenomination" runat="server" ToolTip="Denomination"
                                    CssClass="WindowsStyle" Width="225px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvDenomination" CssClass="styleMandatoryLabel" runat="server"
                                    ControlToValidate="ddlDenomination" InitialValue="0" ErrorMessage="Select Denomination"
                                    Display="None" ValidationGroup="Go">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Start Date" ID="lblStartDate" CssClass="styleReqFieldLabel"
                                    ToolTip="Start Date">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:TextBox ID="txtStartDate" runat="server" Width="200px" ToolTip="Start Date"></asp:TextBox>
                                <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtStartDate" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                    PopupButtonID="imgStartDate" ID="CalendarExtender1">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Select Start Date"
                                    ValidationGroup="Go" Display="None" SetFocusOnError="True" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="End Date" ID="lblEndDate" CssClass="styleReqFieldLabel"
                                    ToolTip="End Date">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:TextBox ID="txtEndDate" runat="server" Width="200px" ToolTip="End Date"></asp:TextBox>
                                <asp:Image ID="ImgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtEndDate" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                    PopupButtonID="imgEndDate" ID="CalendarExtender2">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="Select End Date"
                                    ValidationGroup="Go" Display="None" SetFocusOnError="True" ControlToValidate="txtEndDate">
                                </asp:RequiredFieldValidator>
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
        <tr>
            <td class="styleFieldLabel" colspan="4" height="8px">
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <asp:Button runat="server" ID="btnGo" CssClass="styleSubmitButton" OnClientClick="return fnCheckPageValidators('Go',false);" Text="Go" CausesValidation="true"
                    ValidationGroup="Go" ToolTip="Go" OnClick="btnOk_Click" />
                &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                    Text="Clear" OnClientClick="return fnConfirmClear();" ToolTip="Clear" OnClick="btnClear_Click"/>
            </td>
        </tr>
        <tr>
             <td align="right" colspan="2">
                   <asp:Label ID="lblAmounts" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
                   <%--<asp:Label ID="lblCurrency" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>--%>
             </td>
        </tr>
        <tr>
            <td class="styleFieldAlign" colspan="4">
                <asp:Panel ID="pnlPricingDet" runat="server" CssClass="stylePanel" Width="100%" GroupingText="Pricing Details" Visible="false">
                    <div id="divPricing" runat="server" style="overflow: scroll; height: 200px;">
                        <asp:GridView ID="grvPricingDet" runat="server" AutoGenerateColumns="False"  BorderWidth="2"
                            CssClass="styleInfoLabel" Width="100%"   ShowFooter="true" ShowHeader="true" >
                            <Columns>
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustName" runat="server"  Text='<%# Bind("CUSTOMER_NAME") %>' ToolTip="Customer Name"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Offer Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOffNum" runat="server"  Text='<%# Bind("OFFER_NUMBER") %>' ToolTip="Offer Number"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Round No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRoundNo" runat="server"  Text='<%# Bind("ROUND_NO") %>' ToolTip="Round No."></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right"  />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Offer Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOffDate" runat="server"  Text='<%# Bind("OFFER_DATE") %>' ToolTip="Offer Date"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Offer Valid Till">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOffVldTl" runat="server"  Text='<%# Bind("OFFER_VALID_TILL") %>' ToolTip="Offer Valid Till"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Facility Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFacAmt" runat="server"  Text='<%# Bind("FACILITY_AMOUNT1") %>' ToolTip="Facility Amount"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotFacAmt" runat="server" ToolTip="Sum of Facility Amount"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right"  />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="RV Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRVAmt" runat="server"  Text='<%# Bind("RV_AMOUNT1") %>' ToolTip="RV Amount"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotRVAmt" runat="server" ToolTip="Sum of RV Amount"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right"  />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="LAN">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLAN" runat="server"  Text='<%# Bind("LAN") %>' ToolTip="LAN"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="LAN Depreciation">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLANDep" runat="server"  Text='<%# Bind("LAN_DEPRECIATION") %>' ToolTip="LAN Depreciation"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right"  />
                                </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Margin Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMrgnAmt" runat="server"  Text='<%# Bind("MARGIN_AMOUNT1") %>' ToolTip="Margin Amount"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotMrgnAmt" runat="server" ToolTip="Sum of Margin Amount"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right"  />
                                </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Book Depreciation %">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookDepPer" runat="server"  Text='<%# Bind("BOOK_DEP_PER") %>' ToolTip="Book Depreciation %"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right"  />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Company IRR">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompIRR" runat="server"  Text='<%# Bind("COMPANY_IRR1") %>' ToolTip="Company IRR"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotCompIRR" runat="server" ToolTip="Average of Company IRR"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right"  />
                                </asp:TemplateField>
                                </Columns>
                          </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr align="center">
            <td>
                <asp:Button ID="BtnPrint" CssClass="styleSubmitButton" runat="server" Text="Print"
                    Visible="false" ToolTip="Print" OnClick="BtnPrint_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary ID="VSPricing" runat="server" CssClass="styleMandatoryLabel"
                    CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Go" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CustomValidator ID="cvPricing" runat="server" CssClass="styleMandatoryLabel" Enabled="true" />
            </td>
        </tr>
    </table>
</asp:Content>

