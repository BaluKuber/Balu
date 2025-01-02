<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_ORG_UploadPoPiVi.aspx.cs" Inherits="Origination_S3G_ORG_UploadPoPiVi" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <script language="javascript" type="text/javascript">




        function fnSelectAllCF(chkSelectAllCF, chkSelectCashFlow) {
            var grvCashFlow = document.getElementById('ctl00_ContentPlaceHolder1_gvTrancheName');
            var TargetChildControl = chkSelectCashFlow;
            //Get all the control of the type INPUT in the base control.
            var Inputs = grvCashFlow.getElementsByTagName("input");
            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                    Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = chkSelectAllCF.checked;
        }

        function fnChkRs(cbx) {


            fnChkIsRSSelectAll(cbx);
        }

    </script>


    <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server">
             <ContentTemplate>  --%>
    <table width="100%" align="center" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="File Upload" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel runat="server" ID="pnlIMP" GroupingText="Upload Details" CssClass="stylePanel" Width="100%" Height="100%">
                    <table width="98%" border="0" align="center">
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" Text="Activity Name" ID="lblActivityName" CssClass="styleFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlActivityName" runat="server"
                                    OnSelectedIndexChanged="ddlActivityName_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvactivityname" CssClass="styleMandatoryLabel" runat="server"
                                    ControlToValidate="ddlActivityName" InitialValue="0" ErrorMessage="Select Activity Name"
                                    Display="None" ValidationGroup="Export" />
                            </td>
                            <td class="styleFieldLabel">
                                <asp:LinkButton ID="lnkDownload" runat="server" Enabled="false" CausesValidation="false" Text="Download Template"
                                    OnClick="lnkDownload_Click" ToolTip="Download Template">
                                </asp:LinkButton>
                            </td>
                            
                            <td colspan="2" class="styleFieldLabel">
                                <asp:Label runat="server" Text="Lessee Name" ID="lblCustomerName" CssClass="styleFieldLabel"
                                    Visible="false">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <uc2:Suggest ID="ddlCustomerName" runat="server" ServiceMethod="GetCustomerNameDetails" AutoPostBack="true" Visible="false"
                                    ErrorMessage="Select Lessee Name" Width="215px" ValidationGroup="Main" IsMandatory="false" ItemToValidate="Text" OnItem_Selected="ddlCustomerName_Item_Selected" />
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" Text="Invoice Type" ID="lblInvoiceType" CssClass="styleFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlInvoiceType" runat="server">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Rental" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Extension Rental" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="styleMandatoryLabel" runat="server"
                                    ControlToValidate="ddlInvoiceType" InitialValue="0" ErrorMessage="Select Invoice Type"
                                    Display="None" ValidationGroup="Export"/>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" Text="From Date" ID="lblFromDate" CssClass="styleFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtFromDate" runat="server" ToolTip="From Date" Width="100px">
                                </asp:TextBox>
                                <asp:Image ID="imgFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender runat="server" TargetControlID="txtFromDate"
                                    PopupButtonID="imgFromDate" ID="ceFromDate" Enabled="True">
                                    </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="styleMandatoryLabel" runat="server"
                                    ControlToValidate="txtFromDate" ErrorMessage="Enter From Date"
                                    Display="None" ValidationGroup="Export"/>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" Text="To Date" ID="lblToDate" CssClass="styleFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtToDate" runat="server" ToolTip="To Date" Width="100px">
                                </asp:TextBox>
                                <asp:Image ID="imgToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender runat="server" TargetControlID="txtToDate"
                                    PopupButtonID="imgToDate" ID="ceToDate" Enabled="True">
                                    </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="styleMandatoryLabel" runat="server"
                                    ControlToValidate="txtToDate" ErrorMessage="Enter To Date"
                                    Display="None" ValidationGroup="Export"/>
                            </td>
                        </tr>
                        <tr height="30px">
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" Text="File Name" ID="lblfilename" CssClass="styleFieldLabel"> 
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:FileUpload ID="flUpload" runat="server" CssClass="upload" />
                                <asp:RequiredFieldValidator ID="rfvFileUpload" runat="server" ControlToValidate="flUpload"
                                    ErrorMessage="Select a file to Upload" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Button ID="btnReceipt" Text="Receipt Filter" Visible="false" runat="server"
                                    OnClick="btnReceipt_Click" CausesValidation="false" CssClass="styleSubmitButton" />
                                <asp:Label runat="server" Text="Invoice No" ID="lblInvoiceNo" CssClass="styleFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <uc2:Suggest ID="ddlInvoiceNo" runat="server" ServiceMethod="GetCustomerNameDetails" AutoPostBack="true"
                                    ErrorMessage="Select Invoice No" ValidationGroup="Main" IsMandatory="false" ItemToValidate="Text" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="6">
                                <asp:Button ID="btnUpload" runat="server" CssClass="styleSubmitButton" Text="Upload"
                                    ToolTip="Upload" OnClick="btnUpload_Click" />
                                <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                    ToolTip="Clear" OnClick="btnClear_Click" Text="Clear" />
                                <asp:Button ID="BtnRefresh" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                    OnClick="btnRefresh_Click" Text="Refresh" ToolTip="Refersh" />
                                <asp:Button ID="BtnSearch" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                    OnClick="btnSearch_Click" Text="Search" ToolTip="Search" />
                                <asp:Button ID="btnExport" runat="server" ValidationGroup="Export" CssClass="styleSubmitButton"
                                    OnClick="btnExport_Click" Text="Export" ToolTip="Export" />
                                <asp:HiddenField ID="hfuploadid" runat="server" />
                            </td>
                        </tr>
                        <tr height="10px">
                            <td colspan="5"></td>
                        </tr>
                        <tr class="styleButtonArea">
                            <td>
                                <asp:Label ID="lblErr" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                            </td>
                        </tr>
                        <tr class="styleButtonArea">
                            <td>
                                <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"  CssClass="styleMandatoryLabel"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td align="center" colspan="5" style="background-color: #f2f8ff; font-family: calibri; font-size: 13px; font-weight: bold;">
                                <asp:GridView ID="grvUploadedDetails" runat="server" AutoGenerateColumns="false" Width="99%"
                                    OnRowDataBound="grvUploadedDetails_RowDataBound" HeaderStyle-CssClass="styleGridHeader">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Activity Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProgram_ID" runat="server" Visible="false" Text='<%#Eval("Program_ID")%>'></asp:Label>
                                                <asp:Label ID="lblProgram_Name" runat="server" Text='<%#Eval("Program_Name")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="File Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFile_Name" runat="server" Text='<%#Eval("File_Name")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" Width="20%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lessee Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer_ID" runat="server" Visible="false" Text='<%#Eval("Customer_ID")%>' />
                                                <asp:Label ID="lblCustomer_Name" runat="server" Text='<%#Eval("Customer_Name")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus_Id" runat="server" Visible="false" Text='<%#Eval("Status_Id")%>' />
                                                <asp:Label ID="lblStatus_Desc" runat="server" Text='<%#Eval("Status_Desc")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" Width="8%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Upload On">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUpload_On" runat="server" Text='<%#Eval("Upload_On")%>'></asp:Label>
                                                <asp:Label ID="lblUpload_ID" runat="server" Visible="false" Text='<%#Eval("Upload_ID")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Completed On">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompleted_On" runat="server" Text='<%#Eval("Completed_on")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Upload By">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUpload_by_ID" runat="server" Visible="false" Text='<%#Eval("Upload_by_ID")%>' />
                                                <asp:Label ID="lblUpload_by_Name" runat="server" Text='<%#Eval("Upload_by_Name")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Flag" Visible="false">
                                            <ItemTemplate>
                                                <%--<asp:HiddenField ID="gvhissave" runat="server" Value='<%#Eval("Is_Saved")%>' />--%>
                                                <asp:Label ID="gvlisdeleted" runat="server" Text='<%#Eval("Is_Deleted")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" Width="0%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnException" runat="server" CausesValidation="false"
                                                    CommandName="Exception" Text="View/Exception" OnClick="btnException_Click" />
                                                <asp:LinkButton ID="btnSave" runat="server" CausesValidation="false"
                                                    OnClientClick="return confirm('Do you want to Save this record?');" ToolTip="Save"
                                                    CommandName="Save" Text="Save" OnClick="btnSave_Click" />
                                                <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="false"
                                                    OnClientClick="return confirm('Do you want to Delete this record?');" ToolTip="Delete"
                                                    CommandName="Delete" Text="Delete" OnClick="btnDelete_Click" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="16%"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="5">
                                <uc1:PageNavigator ID="ucCustomPaging" runat="server" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                <asp:GridView ID="grvFiles" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="styleGridHeader" Visible="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="File Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblZFileName" runat="server" Text='<%#Eval("FileName")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" Width="0%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Compressed Size (Bytes)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblZCompressedSize" runat="server" Text='<%#Eval("CompressedSize")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" Width="0%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Uncompressed Size (Bytes)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblZUncompressedSize" runat="server" Text='<%#Eval("UncompressedSize")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" Width="0%"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary runat="server" ID="vsUserMgmt" HeaderText="Please correct the following validation(s):"
                                    Height="250px" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false" ValidationGroup="Export"
                                    ShowSummary="true" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <%--      </ContentTemplate>
          <Triggers>
              <asp:PostBackTrigger ControlID="grvUploadedDetails" />
              <asp:AsyncPostBackTrigger ControlID="ddlCustomerName" />
          </Triggers>
          </asp:UpdatePanel>--%>
    <asp:Button ID="Button1" Style="display: none" runat="server" />
    <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>

    <asp:Button ID="btnModal" Style="display: none" runat="server" />
    <cc1:ModalPopupExtender ID="ModalPopupExtenderApprover" runat="server" TargetControlID="btnModal"
        PopupControlID="pnlODICheck" BackgroundCssClass="styleModalBackground" Enabled="true">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlODICheck" Style="display: none; vertical-align: middle" runat="server"
        BorderStyle="Solid" BackColor="White" Width="50%" ScrollBars="Auto">
        <%--<asp:UpdatePanel ID="UpdpnlODICheck" runat="server">
            <ContentTemplate>--%>
        <table width="100%">
            <tr>
                <td>
                    <h3 style="height: 10px;">Error Details
                    </h3>
                    <asp:TextBox ID="TextBox2" runat="server" Text="Records Contains Exception(s).! Do you still want to proceed?"
                        Width="100%" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnModalOk" runat="server" Text="Ok" ToolTip="Ok" class="styleSubmitButton" CausesValidation="false"
                        OnClick="btnModalOk_Click" />
                    <asp:Button ID="btnModalCancel" runat="server" Text="Cancel" OnClick="btnModalCancel_Click" CausesValidation="false"
                        ToolTip="Cancel" class="styleSubmitButton" />
                </td>
            </tr>
        </table>
        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </asp:Panel>

    <cc1:ModalPopupExtender ID="MPEReceipt" runat="server" TargetControlID="btnReceipt"
        PopupControlID="pnlReceipt" BackgroundCssClass="styleModalBackground" Enabled="true">
    </cc1:ModalPopupExtender>

    <asp:Panel ID="pnlReceipt" Visible="false" runat="server"
        BorderStyle="Solid" BackColor="White" Width="70%" Height="90%" ScrollBars="Auto">
        <asp:UpdatePanel ID="updUpload" runat="server">
            <ContentTemplate>
                <table width="100%" border="1">
                    <tr>
                        <td class="styleFieldLabel">
                            <asp:Label runat="server" Text="Funder Name" ID="lblFunder" CssClass="styleFieldLabel"
                                Visible="false">
                            </asp:Label>
                        </td>

                        <td class="styleFieldAlign">
                            <uc2:Suggest ID="ddlFunderName" runat="server" ServiceMethod="GetFunderName" AutoPostBack="true" Visible="false"
                                ErrorMessage="Select Funder Name" Width="195px" ValidationGroup="Main" IsMandatory="false" ItemToValidate="Text" />
                        </td>

                        <td>

                            <asp:Button ID="btnRcptModalShow" Text="SHOW" runat="server"
                                OnClick="Showtranch_Click" CausesValidation="false" CssClass="styleSubmitButton" />

                            <asp:Button ID="btnRcptModalClose" Text="Close" runat="server"
                                OnClick="btnRcptModalClose_Click" CausesValidation="false" CssClass="styleSubmitButton" />

                        </td>
                    </tr>
                    <tr>
                        <td class="styleFieldLabel">
                            <asp:Label ID="lblStartDate" runat="server" Text="From Date" CssClass="styleDisplayLabel">
                            </asp:Label>
                        </td>
                        <td class="styleFieldAlign">

                            <asp:TextBox ID="txtStartDate" runat="server" AutoPostBack="true" OnTextChanged="txtDateFrom_TextChanged"></asp:TextBox>
                            <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />

                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" PopupButtonID="imgStartDate">
                            </cc1:CalendarExtender>

                        </td>
                        <td class="styleFieldLabel">
                            <asp:Label ID="lblEnddate" runat="server" Text="To Date" CssClass="styleDisplayLabel">
                            </asp:Label>
                        </td>
                        <td class="styleFieldAlign">
                            <asp:TextBox ID="txtenddate" runat="server" AutoPostBack="true" OnTextChanged="txtDateTo_TextChanged"></asp:TextBox>
                            <asp:Image ID="Imgenddate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtenddate" PopupButtonID="Imgenddate">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="styleFieldAlign" align="center" colspan="2">
                            <asp:GridView ID="gvTrancheName" runat="server" AutoGenerateColumns="false" ViewStateMode="Enabled" OnRowDataBound="RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblTransferSelectAll" runat="server" Text="Select ALL"></asp:Label>
                                            <center>
                                                <asp:CheckBox ID="chkSelectAllTranche" runat="server" onclick="javascript:fnSelectAllCF(this,'chkgdtrnSelect');" />
                                            </center>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkgdtrnSelect" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tranche Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgdtrnTrancheID" runat="server" Text='<%#Bind("Tranche_Header_id") %>' Style="display: none"></asp:Label>
                                            <asp:Label ID="lblgdtrnTrancheName" runat="server" Text='<%#Bind("Tranche_Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="70%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnRcptModalShow" />
                <asp:PostBackTrigger ControlID="btnRcptModalClose" />
            </Triggers>
        </asp:UpdatePanel>
        <input type="hidden" id="hdnSortDirection" runat="server" />
        <input type="hidden" id="hdnSortExpression" runat="server" />
        <input type="hidden" id="hdnSearch" runat="server" />
        <input type="hidden" id="hdnOrderBy" runat="server" />

    </asp:Panel>


</asp:Content>



