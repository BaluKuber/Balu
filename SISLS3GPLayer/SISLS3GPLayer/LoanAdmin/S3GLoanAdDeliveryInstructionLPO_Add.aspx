<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GLoanAdDeliveryInstructionLPO_Add.aspx.cs"
    EnableEventValidation="false" Inherits="LoanAdmin_S3GLoanAdDeliveryInstructionLPO_Add"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 0.3%">
                        <asp:Panel GroupingText="Delivery Details" ID="pnlCustomerInfo" runat="server" CssClass="stylePanel">
                            <table width="100%">
                                <tr>
                                    <td class="styleFieldLabel" width="18%">
                                        <asp:Label ID="lblLob" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="32%">
                                        <asp:DropDownList ID="ddlLOB" runat="server" Width="205px" AutoPostBack="True" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                            Height="26px" ToolTip="Line of Business" ValidationGroup="btnSave">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvLOB" runat="server" Display="None" InitialValue="0"
                                            ErrorMessage="Select a Line of Business" ControlToValidate="ddlLOB" SetFocusOnError="True"
                                            ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" width="18%">
                                        <asp:Label ID="lblBranch" runat="server" Text="Location" CssClass="styleReqFieldLabel"></asp:Label>
                                        <asp:Label ID="lblIsActivated" runat="server" Text="Label" Visible="false"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="32%">
                                        <%-- <asp:DropDownList ID="ddlBranch" runat="server" Width="205px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" ToolTip="Location" ValidationGroup="btnSave">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvBranch" runat="server" Display="None" InitialValue="0"
                                            ErrorMessage="Select a Location" ControlToValidate="ddlBranch" SetFocusOnError="True"
                                            ValidationGroup="btnSave"></asp:RequiredFieldValidator>--%>
                                        <uc2:Suggest ID="ddlBranch" runat="server" ToolTip="Location" ServiceMethod="GetBranchList"
                                            AutoPostBack="true" OnItem_Selected="ddlBranch_SelectedIndexChanged" ValidationGroup="btnSave"
                                            ErrorMessage="Select a Location" IsMandatory="true" />
                                    </td>
                                    <tr>
                                        <td class="styleFieldLabel" width="18%">
                                            <asp:Label ID="lblMLA" runat="server" Text="Prime Account Number" CssClass="styleReqFieldLabel"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign" width="32%">
                                            <asp:DropDownList ID="ddlMLA" runat="server" Width="205px" AutoPostBack="True" OnSelectedIndexChanged="ddlMLA_SelectedIndexChanged"
                                                ToolTip="Prime Account Number" ValidationGroup="btnSave">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvMLA" runat="server" Display="None" InitialValue="0"
                                                ErrorMessage="Select a Prime Account Number" ControlToValidate="ddlMLA" SetFocusOnError="True"
                                                ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None"
                                                ErrorMessage="Select a Prime Account Number" ControlToValidate="ddlMLA" SetFocusOnError="True"
                                                ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                        </td>
                                        <td class="styleFieldLabel" width="18%">
                                            <asp:Label ID="lblSLA" runat="server" Text="Sub Account Number" CssClass="styleDisplayLabel"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign" width="32%">
                                            <asp:DropDownList ID="ddlSLA" runat="server" Width="205px" AutoPostBack="True" OnSelectedIndexChanged="ddlSLA_SelectedIndexChanged"
                                                ToolTip="Sub Account Number">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvSLA" runat="server" Display="None" InitialValue="0"
                                                ErrorMessage="Select a Sub Account Number" ControlToValidate="ddlSLA" SetFocusOnError="True"
                                                Visible="false" Enabled="false"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="styleFieldLabel" width="18%">
                                            <asp:Label ID="lblDate" runat="server" Text="DI/LPO Date" CssClass="styleDisplayLabel"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign" width="32%">
                                            <asp:TextBox ID="txtDate" runat="server" Width="100px" ToolTip="DI/LPO Date"></asp:TextBox>
                                        </td>
                                        <td class="styleFieldLabel" width="18%">
                                            <asp:Label ID="lblDLNo" runat="server" Text="DI/LPO Number" CssClass="styleDisplayLabel"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign" width="32%">
                                            <asp:TextBox ID="txtLPONo" runat="server" Width="150px" ReadOnly="true" ToolTip="DI/LPO Number"></asp:TextBox>
                                        </td>
                                    </tr>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table width="100%">
                            <tr>
                                <td width="50%">
                                    <asp:Panel GroupingText="Customer Details" ID="Panel2" runat="server" CssClass="stylePanel"
                                        ToolTip="Customer Details">
                                        <table width="100%" align="center" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" FirstColumnStyle="styleFieldLabel"
                                                        FirstColumnWidth="39%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td width="50%">
                                    <asp:Panel GroupingText="Vendor Details" ID="Panel1" runat="server" CssClass="stylePanel"
                                        ToolTip="Vendor Details">
                                        <%--Mani--%>
                                        <table width="100%" align="center" border="0" cellspacing="0">
                                            <tr>
                                                <td class="styleFieldLabel" width="34%">
                                                    <asp:Label ID="Label4" runat="server" Text="Vendor Code" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" width="66%">
                                                    <asp:DropDownList ID="ddlVendorCode" runat="server" Width="200px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlVendorCode_SelectedIndexChanged" ToolTip="Vendor Code"
                                                        ValidationGroup="btnSave">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvVendorCode" runat="server" Display="None" InitialValue="0"
                                                        ErrorMessage="Select a Vendor Code" ControlToValidate="ddlVendorCode" SetFocusOnError="True"
                                                        ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="None"
                                                        ErrorMessage="Select a Vendor Code" ControlToValidate="ddlVendorCode" SetFocusOnError="True"
                                                        ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" width="34%">
                                                    <asp:Label ID="Label10" runat="server" Text="Vendor Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" width="66%">
                                                    <asp:TextBox ID="txtVendorName" runat="server" Width="90%" ReadOnly="true" Visible="True"
                                                        ToolTip="Vendor Name">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <uc1:S3GCustomerAddress ID="S3GVendorAddress" runat="server" FirstColumnStyle="styleFieldLabel"
                                                        FirstColumnWidth="35%%" SecondColumnWidth="61%" ShowCustomerCode="false" ShowCustomerName="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel GroupingText="Asset Details" ID="Panel3" runat="server" CssClass="stylePanel">
                            <asp:Label ID="lblAssetCodeOl" runat="server" Text="Asset Code" CssClass="styleReqFieldLabel"
                                Visible="false"></asp:Label>
                            <asp:DropDownList ID="ddlAssetCode" runat="server" Width="205px" AutoPostBack="True"
                                Height="26px" ToolTip="Asset Code" ValidationGroup="btnSave" Visible="false">
                            </asp:DropDownList>
                            <asp:Button runat="server" ID="btnAdd" CssClass="styleSubmitShortButton" Text="ADD"
                                OnClick="btnAdd_Click" ValidationGroup="btn_Add" Visible="false" />
                            <%-- <asp:RequiredFieldValidator ID="rfvAssetCode" runat="server" Display="None" InitialValue="0"
                                ErrorMessage="Select a Asset Code" ControlToValidate="ddlAssetCode" SetFocusOnError="True"
                                ValidationGroup="btn_Add" Enabled="false"></asp:RequiredFieldValidator>--%>
                            <asp:GridView ID="gvDlivery" runat="server" CssClass="styleInfoLabel" Width="100%"
                                AutoGenerateColumns="False" OnRowDataBound="gvDlivery_RowDataBound" DataKeyNames="Asset_ID">
                                <Columns>
                                    <asp:TemplateField Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetID" runat="server" Text='<%# Eval("Asset_ID") %>' /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SL.No." HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                            <%---<asp:Label ID="lblProgramRefNo" runat="server" Text='<%# Eval("SerialNo") %>' />--%></ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Code" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetCode" runat="server" Text='<%# Eval("Asset_Code") %>' ToolTip="Asset Code" /></ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Description" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetDesc" runat="server" Text='<%# Eval("Asset_Description") %>'
                                                ToolTip="Asset Description" /></ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Model Description" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtModelDesc" runat="server" TextMode="MultiLine" Width="200px"
                                                MaxLength="100" Text='<%# Eval("Model_description") %>' ToolTip="Model Description"
                                                onkeyup="maxlengthfortxt(100)"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="ftbModelDesc" runat="server" TargetControlID="txtModelDesc"
                                                FilterType="Custom" FilterMode="InvalidChars" InvalidChars="<>&'">
                                            </cc1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit Quantity" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuantity" runat="server" Width="50px" MaxLength="4" Text='<%# Eval("Asset_quantity") %>'
                                                ToolTip="Unit Quantity" Style="text-align: right" onblur="fnBlur()"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender" runat="server" TargetControlID="txtQuantity"
                                                FilterType="Numbers" ValidChars="" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Value" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAssetValue" Text='<%# Bind("Asset_Value")%>' Width="180px" runat="server"
                                                Style="text-align: right" ToolTip="Asset Value"></asp:TextBox>
                                            <asp:TextBox ID="txtDyAsset" Text='<%# Bind("ASSET_COST")%>' Width="80px" runat="server"
                                                ReadOnly="true" Style="display: none;"></asp:TextBox>
                                            <%--<asp:TextBox ID="txtAssetValue1" Text='<%# Bind("Asset_Value")%>' Width="180px" runat="server"
                                                Style="text-align: right" ToolTip="Asset Value" Visible="false" MaxLength="14">
                                            </asp:TextBox>--%>
                                            <cc1:FilteredTextBoxExtender ID="txtValue1" runat="server" TargetControlID="txtAssetValue"
                                                FilterType="Numbers,Custom" ValidChars="." Enabled="true">
                                            </cc1:FilteredTextBoxExtender>
                                            <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtAssetValue1"
                                                 FilterType="Numbers,Custom" ValidChars="." Enabled="true">
                                            </cc1:FilteredTextBoxExtender>--%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-CssClass="styleGridHeader">
                                        <HeaderTemplate>
                                            <table>
                                                <tr align="center">
                                                    <td align="center">
                                                        Select All
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td align="center">
                                                        <asp:CheckBox runat="server" ID="chAll" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <table>
                                                <tr align="center">
                                                    <td align="center">
                                                        <asp:CheckBox runat="server" ID="CbAssets" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRemarks" runat="server" Width="200px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                MaxLength="100" Text='<%# Eval("Remarks") %>' ToolTip="Remarks"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="ftbRemarks" runat="server" TargetControlID="txtRemarks"
                                                FilterType="Custom" FilterMode="InvalidChars" InvalidChars="<>&'">
                                            </cc1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td id="Td1" runat="server" align="center">
                        <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                            OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" ValidationGroup="btnSave" />
                        <asp:Button runat="server" ID="btnClear" CssClass="styleSubmitButton" Text="Clear"
                            CausesValidation="False" OnClick="btnClear_Click" />
                        <cc1:ConfirmButtonExtender ID="btnClear_ConfirmButtonExtender" runat="server" ConfirmText="Do you want to clear?"
                            Enabled="True" TargetControlID="btnClear">
                        </cc1:ConfirmButtonExtender>
                        <asp:Button runat="server" ID="btnCancel" CssClass="styleSubmitButton" CausesValidation="False"
                            Text="Cancel" OnClick="btnCancel_Click" ToolTip="Cancel" />
                        <asp:Button runat="server" ID="btnDLGeneration" CssClass="styleSubmitButton" Text="DI/LPO Generation"
                            CausesValidation="true" OnClick="btnDLGeneration_Click" ToolTip="DI/LPO Generation" />
                        <asp:Button runat="server" ID="btnEmail" CssClass="styleSubmitButton" Text="EMail"
                            CausesValidation="False" OnClick="btnEmail_Click" ToolTip="EMail" Enabled="false" />
                        <asp:Button runat="server" ID="btnPrint" CssClass="styleSubmitButton" Text="Print"
                            CausesValidation="False" OnClick="btnPrint_Click" ToolTip="Print" Enabled="false" />
                        <asp:Button runat="server" ID="btnDICancel" CssClass="styleSubmitButton" CausesValidation="False"
                            Text="Cancel DI/LPO" OnClick="btnDICancel_Click" ToolTip="Cancel DI/LPO" OnClientClick="return fnConfirmCancelDI()" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsDelivery" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  " />
                        <asp:CustomValidator ID="cvDelivery" runat="server" Display="None" CssClass="styleMandatoryLabel"></asp:CustomValidator>
                        <asp:Label ID="lblCustID" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" Style="color: Red; font-size: medium"></asp:Label>
                        <asp:TextBox ID="txtSan" runat="server" Visible="false"></asp:TextBox>
                        <asp:Label ID="lblCompanyName" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblCCity" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblCZipcode" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblIsPrint" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblStatus" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblVAddress1" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblVAddress2" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblVCity" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblVPincode" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblVState" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblVCountry" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblCuAddress1" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblCuAddress2" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblCuCity" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblCuPincode" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblCuState" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblCuCountry" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
    function fnConfirmCancelDI()
    {

        if(confirm('Do you want to cancel DI/LPO?'))
            {
                return true;
            }
        else
            return false;
            
    }
    function fnBlur()
    {
        var Quantity;
        var total;
        var actualvalue;
        var mode = ("<%= Request.QueryString["qsMode"] %>");
       if (mode == "C")
       {
        var Grid= document.getElementById("<%=gvDlivery.ClientID%>");
        if(Grid != null)
        {
        var intGridLen= Grid.rows.length;
        for(var i=2;i<=intGridLen;i++)
            {
                if(i<10)
                    i="0"+i;
                  var txtQuantity = document.getElementById(Grid.id+"_ctl"+i+"_txtQuantity");
                  var txtDyAsset = document.getElementById(Grid.id+"_ctl"+i+"_txtDyAsset");
                  var txtAssetValue = document.getElementById(Grid.id+"_ctl"+i+"_txtAssetValue");
                  if (txtAssetValue == null)
                  {
                    txtAssetValue.value = 0;
                  }
                  else if (txtDyAsset == null)
                  {
                    txtDyAsset.value = 0;
                  }
                
                 if((txtQuantity.value != "") && (txtAssetValue.value != "") && (txtDyAsset.value != ""))
                  {
                     txtAssetValue.value = ((txtQuantity.value) * (txtDyAsset.value));
                     
                  }
                  else
                  {
                     txtAssetValue.value = 0;
                  }
              }
              }
           }
           }
    </script>

</asp:Content>
