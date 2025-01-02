<%@ Page Title="Asset Details" Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true"
     CodeFile="S3G_ORG_Application_Asset_Homeloan.aspx.cs" Inherits="Origination_S3G_ORG_Application_Asset_Homeloan" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <base target="_self" />
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <table width="99%">
                <tr>
                    <td align="center">
                        <asp:Panel ID="Panel2" runat="server" CssClass="stylePanel" GroupingText="Application Asset Details"
                            Height="75%" Width="100%">
                           <table width="100%">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblSlNo" runat="server" CssClass="styleDisplayLabel" Text="Sl. No."></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtSlNo" runat="server" Width="20px" ReadOnly="true" TabIndex="-1"
                                            Style="text-align: right"></asp:TextBox>
                                    </td>
                                <td class="styleFieldAlign">
                                    <asp:Label runat="server" ID="lblassettype" CssClass="styleReqFieldLabel" Text="Asset Type"></asp:Label>  
                                 </td>
                                      <td class="styleFieldAlign">
                                            <asp:DropDownList ID="ddlAssettypeList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged">
                                                  </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvAssetTypeList" runat="server" ControlToValidate="ddlAssettypeList"
                                                ValidationGroup="TabAssetDetails" CssClass="styleMandatoryLabel" Display="None"
                                                InitialValue="0" SetFocusOnError="True" ErrorMessage="Select an Asset Type"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                  
                             
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                              <asp:Panel runat="server" ID="pnldetails" GroupingText="Details" CssClass="stylePanel">
                            <asp:GridView ID="gvsset" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                            RowStyle-Width="0" Width="100%">
                                            <Columns>
                                                <%--Serial Number--%>
                                                 <asp:TemplateField HeaderText="Sl No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1%>' ToolTip="Serial Number" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--Location--%>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblflag" runat="server" Text='<%#Eval("Description") %>' ToolTip="Flag" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left"/>
                                                </asp:TemplateField>
                                                <%--Doc Date --%>
                                                <asp:TemplateField HeaderText="Values">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtvalues" runat="server" Text='<%#Eval("Values") %>' ToolTip="Description" />
                                                    </ItemTemplate>
                                                    
                                                    <ItemStyle HorizontalAlign="Left"/>
                                                </asp:TemplateField>
                                                  </Columns>
                                        </asp:GridView>
                           
                        </asp:Panel>
                    </td>
                </tr>
                   <tr>
                    <td>
                              <asp:Panel runat="server" ID="pnldoc" GroupingText="Details" CssClass="stylePanel">
                            <asp:GridView ID="grvdoc" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                            RowStyle-Width="0" Width="100%">
                                            <Columns>
                                                  
                                                 <asp:TemplateField HeaderText="Sl No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1%>' ToolTip="Serial Number" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                             
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblflag" runat="server" Text='<%#Eval("Description") %>' ToolTip="Flag" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left"/>
                                                </asp:TemplateField>
                                             
                                              <asp:TemplateField HeaderText="Document Collected By">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDoc" runat="server" Text='<%#Eval("callby") %>' ToolTip="Description" />
                                                    </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"/>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Date">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtdate" runat="server" Text='<%#Eval("date") %>' ToolTip="Description" />
                                                           
                                    <asp:Image ID="imgEndDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                    <cc1:CalendarExtender ID="CalendarExtenderEndDateSearch" runat="server" Enabled="True"
                                        PopupButtonID="imgEndDateSearch" TargetControlID="txtdate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                    </cc1:CalendarExtender>
                                                    </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"/>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Scanned" Visible="false">
                                                    <ItemTemplate>
                                                         <asp:CheckBox ID="chkScanned" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Mandatory_Scan")))%>'></asp:CheckBox>
                                                    </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"/>
                                                </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Mandatory_Documents" Visible="false">
                                                    <ItemTemplate>
                                                         <asp:CheckBox ID="chkdocuments"   runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Mandatory_Documents")))%>'></asp:CheckBox>
                                                    </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"/>
                                                </asp:TemplateField>
                                                              <asp:TemplateField HeaderText="File Upload">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActualPath" runat="server" Visible="false" Text='<%# Bind("Document_Path") %>'></asp:Label>
                                                        <asp:UpdatePanel ID="tempUpdate" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>

                                                                <asp:Button ID="btnBrowse" runat="server" OnClick="btnBrowse_OnClick"></asp:Button>
                                                                <asp:FileUpload runat="server" ID="flUpload" Width="150px" ToolTip="Upload File" />
                                                                <asp:TextBox ID="txtFileupld" runat="server" Style="position: absolute; margin-left: -153px; width: 65px" ReadOnly="true" />

                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:PostBackTrigger ControlID="btnBrowse" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="150px" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="View">
                                                    <ItemTemplate>
                                                        <%--<%# Bind("IsNeedImageCopy") %>--%>
                                                        <asp:LinkButton ID="hlnkView" runat="server" OnClick="hlnkView_Click" Text="View"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                           
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblremarks" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtremarks" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="99%">
                            <tr>
                                <td class="styleFieldLabel" align="center" style="width: 99%; padding-left: 35%">
                                    <asp:Button ID="btnOK" ValidationGroup="TabAssetDetails" runat="server" CausesValidation="true"
                                        CssClass="styleSubmitButton" Text="OK" UseSubmitBehavior="False" OnClick="btnOK_Click"  />
                                    &nbsp;<asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                                        Text="Cancel" UseSubmitBehavior="False" ValidationGroup="TabAssetDetails" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="99%">
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="vs_TabAssetDetails" runat="server" CssClass="styleMandatoryLabel"
                                        Width="705px" ValidationGroup="TabAssetDetails" HeaderText="Correct the following validation(s):  " />
                                    <asp:CustomValidator ID="cv_TabAssetDetails" runat="server" CssClass="styleMandatoryLabel"
                                        Display="None" ValidationGroup="TabAssetDetails"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblErrorMessage" runat="server" Style="color: Red; font-size: 14px">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:CustomValidator ID="cvApplicationAsset" runat="server" CssClass="styleMandatoryLabel"
                Enabled="true" Width="98%" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <input type="hidden" id="hdnCustomerID" runat="server" />
    <input type="hidden" id="hdnAssetAvailDate" runat="server" />

    <script language="javascript" type="text/javascript">
       
        function ViewModal() {

            window.showModalDialog('../Origination/S3G_ORG_Application_Asset_Homeloan.aspx?qsMaster=Yes', 'Application home Details', 'dialogwidth:900px;dialogHeight:900px;');
        }
        function uploadComplete(sender, args)
        {
            var objID = sender._inputFile.id.split("_");
            var obj = args._fileName.split("\\");
            objID = "<%= grvdoc.ClientID %>" + "_" + objID[5];
            if (document.getElementById(objID + "_myThrobber") != null) {
                document.getElementById(objID + "_myThrobber").innerText = args._fileName;
                document.getElementById(objID + "_hidThrobber").value = args._fileName;
                //                document.getElementById(objID + "_hidThrobber").style.Visbility = 'hidden';
                //                document.getElementById(objID + "_hidThrobber").style.display = 'none';
                //document.getElementById(objID + "_myThrobber").visible = false;


                if (obj[obj.length - 1].length > 80) {
                    alert("File Name can't exceed more than 80 characters");
                    document.getElementById(objID + "_myThrobber").innerText = "";
                    document.getElementById(objID + "_hidThrobber").value = "";
                    return false;
                }


            }

          

        }


        function showHand(textBoxId) {
            document.getElementById(textBoxId).style.cursor = 'hand';
            document.getElementById(textBoxId).style.cursor = 'hand';
            document.getElementById(textBoxId).style.cursor = 'hand';
        }

        function FunShowPath(input) {
            if (input != null) {
                var objID = input.id;
                var myThrobber = document.getElementById((input.id).replace('asyFileUpload', 'myThrobber'));
                if (myThrobber != null) {
                    if (myThrobber.innerText != "")
                        input.setAttribute('title', myThrobber.innerText);
                }
            }
        }


    </script>

</asp:Content>
