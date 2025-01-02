<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GRptDocumentDeficiencyReport.aspx.cs" Inherits="Reports_S3GRptDocumentDeficiencyReport"
    Title="Document Deficiency Report" %>

<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Document Deficiency Report">
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
                            <asp:Panel ID="pnlHeader" runat="server" GroupingText="Input Criteria" CssClass="stylePanel"
                                Width="100%">
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td class="styleFieldLabel" width="20%">
                                            <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel"
                                                ToolTip="Line of Business">
                                            </asp:Label>
                                        </td>
                                        <td class="styleFieldAlign" align="left" width="30%">
                                            <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="true" Width="80%" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                ToolTip="Line of Business">
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="rfvddlLOB" runat="server" ErrorMessage="Select Line of Business" ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="ddlLOB" InitialValue="-1"></asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td class="styleFieldLabel" width="20%">
                                            <asp:Label runat="server" ID="lblCustomerName" Text="Customer Name" ToolTip="Customer Name"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign" width="30%">
                                            <asp:TextBox ID="txtCustomerName" runat="server" Style="display: none;" Width="100%"
                                                CausesValidation="true"></asp:TextBox>
                                            <uc2:LOV ID="ucCustomerCodeLov" onfocus="return fnLoadCustomer();" runat="server"
                                                strLOV_Code="CMD" />
                                            <asp:Button ID="btnLoadCustomer" runat="server" Text="Load Customer" Style="display: none"
                                                OnClick="btnLoadCustomer_OnClick" ToolTip="Customer Name" />
                                            <input id="hdnCustID" type="hidden" runat="server" />
                                            <%--<asp:RequiredFieldValidator ID="rfvcustname" runat="server" ControlToValidate="txtCustomerName"
                                                InitialValue="-1" ErrorMessage="Select a Customer Name" Display="None" ValidationGroup="Ok"
                                                Enabled="false">
                                            </asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="8px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="styleFieldLabel" width="20%">
                                            <asp:Label runat="server" ID="lblType" Text="Type" CssClass="styleDisplayLabel" ToolTip="Type"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign" width="30%">
                                            <asp:DropDownList ID="ddlType" runat="server" Width="80%" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                                                ToolTip="Type">
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="rfvddlBranch" Display="None"  runat="server" ErrorMessage="Select Location2" ValidationGroup="Ok" SetFocusOnError="True" ControlToValidate="ddlBranch" InitialValue="-1"></asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td class="styleFieldLabel" width="20%">
                                            <asp:Label runat="server" ID="lblStatus" Text="Status" CssClass="styleDisplayLabel"
                                                ToolTip="Status"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign" width="30%">
                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="80%" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                                ToolTip="Status">
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="rfvddlBranch" Display="None"  runat="server" ErrorMessage="Select Location2" ValidationGroup="Ok" SetFocusOnError="True" ControlToValidate="ddlBranch" InitialValue="-1"></asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="8px">
                                        </td>
                                    </tr>
                                    <tr>
                                    <td class="styleFieldLabel" width="20%">
                                            <asp:Label runat="server" ID="lblfrom" Text="Report From Date" CssClass="styleDisplayLabel"
                                                ToolTip="Report From Date"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign" width="30%">
                                            <asp:TextBox ID="txtFrom" ContentEditable="true" runat="server" Width="73%" ToolTip="Report From Date"></asp:TextBox>
                                            <asp:Image ID="imgFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                            <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtFrom"
                                                PopupButtonID="imgFrom" ID="CalendarExtender1" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                            </cc1:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Select Report From Date"
                                                ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="txtFrom"></asp:RequiredFieldValidator>
                                        </td>
                                        <td class="styleFieldLabel" width="20%">
                                            <asp:Label runat="server" ID="lblto" Text="Report To Date" CssClass="styleDisplayLabel"
                                                ToolTip="Report To Date"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign" width="30%">
                                            <asp:TextBox ID="txtTo" ContentEditable="true" runat="server" Width="73%" ToolTip="Report To Date"></asp:TextBox>
                                            <asp:Image ID="imgTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                            <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtTo"
                                                PopupButtonID="imgTo" ID="CalendarExtender2" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                            </cc1:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Select Report To Date"
                                                ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="txtTo"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="8px">
                                        </td>
                                    </tr>
                                    <tr class="styleButtonArea" style="padding-left: 4px">
                        <td align="center" colspan="4">
                            <asp:Button runat="server" ID="btnOk" CssClass="styleSubmitButton" OnClientClick="return fnCheckPageValidators('Ok',false);" Text="GO" CausesValidation="true" ValidationGroup="Ok" OnClick="btnOk_Click" ToolTip="Go" />
                            &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" ToolTip="Clear" />
                        </td>
                    </tr>
                                    <tr>
                        <td class="styleFieldAlign" colspan="4">
                            <asp:Panel ID="pnlDemand" runat="server" GroupingText="Document Deficiency Report" CssClass="stylePanel" Width="100%" Visible="false">
                            <asp:Label ID="lblError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                <div id="divDemand" runat="server" style="overflow: Scroll; height: 200px;">
                                    <asp:GridView ID="grvDemand" runat="server" Width="100%" AutoGenerateColumns="false" HeaderStyle-CssClass="styleGridHeader" 
                                    RowStyle-HorizontalAlign="Center" ShowFooter="true">
                                        <Columns>
                                           <%-- <asp:TemplateField HeaderText="Doctran" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPDDTID" runat="server" Text='<%# Bind("DOCTRANID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoc" runat="server" Text='<%# Bind("LOCATION") %>' ToolTip="Location"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Customer Name" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCusName" runat="server" Text='<%# Bind("CUSNAME") %>' ToolTip="Customer Name"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Document Type" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDoctype" runat="server" Text='<%# Bind("DOCTYPE") %>' ToolTip="Document Type"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Document Description" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDocdesc" runat="server" Text='<%# Bind("DOCDESC") %>' ToolTip="Document Description"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Collected On" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCollon" runat="server" Text='<%# Bind("COLLECTEDON") %>' ToolTip="Collected On"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Collected By" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCollby" runat="server" Text='<%# Bind("COLLECTEDBY") %>' ToolTip="Collected By"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Image Scan By" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblImgscnby" runat="server" Text='<%# Bind("IMGSCNBY") %>' ToolTip="Image Scan By"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Image Scan Date" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblImgscndt" runat="server" Text='<%# Bind("IMGSCNDT") %>' ToolTip="Image Scan Date"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="View Image Scan">
                                                <ItemTemplate>
                                                        <asp:ImageButton ID="hyplnkView" CommandArgument='<%# Bind("SCANREF") %>'
                                                            OnClick="hyplnkView_Click" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                            runat="server" ToolTip="View Image Scan" />
                                                        <asp:Label runat="server" ID="lblPath" Text='<%# Eval("SCANREF")%>' Visible="false"></asp:Label>
                                                        <asp:Label runat="server" ID="lblIs_Mandatory" Text='<%# Eval("ISMAND")%>'
                                                            Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
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
                        <td align="center" colspan="4">
                            <asp:Button runat="server" ID="btnPrint" CssClass="styleSubmitButton" Text="Print" CausesValidation="false" ValidationGroup="Print" Visible="false" OnClick="btnPrint_Click" ToolTip="Export to Crystal Report" />
                                                        <%--&nbsp;<asp:Button runat="server" ID="btnChart" CausesValidation="false" CssClass="styleSubmitButton" Text="View Chart" Visible="false" OnClick="btnChart_Click"  ToolTip="Export to Chart" />--%>

                        </td>
                    </tr>
                    <tr>
                    <td height="8px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:ValidationSummary ID="VSDocDef" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Ok" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CustomValidator ID="CVDocDef" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
                        </td>
                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
