<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true" CodeFile="S3G_ORG_AccountHomeLoanAsset.aspx.cs" Inherits="LoanAdmin_S3G_ORG_AccountHomeLoanAsset" %>

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
                                <td>
                                    <asp:Label runat="server" ID="lblassettype" CssClass="styleReqFieldLabel" Text="Asset Type"></asp:Label>  
                                 </td>
                                      <td class="styleFieldAlign">
                                            <asp:DropDownList ID="ddlAssettypeList" runat="server" AutoPostBack="True">
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem> 
                                              <asp:ListItem Text="Flat" Value="1"></asp:ListItem>
                                                       <asp:ListItem Text="Home" Value="2"></asp:ListItem>                                                
                                                        <asp:ListItem Text="Land" Value="3"></asp:ListItem>                                                
                                                  </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvAssetTypeList" runat="server" ControlToValidate="ddlAssettypeList"
                                                ValidationGroup="TabAssetDetails" CssClass="styleMandatoryLabel" Display="None"
                                                InitialValue="0" SetFocusOnError="True" ErrorMessage="Select an Asset Type"></asp:RequiredFieldValidator>
                                        </td>
                                    
                                    
                                      
                                    </tr>
                                    <tr>
                                   <td class="styleFieldLabel">
                                            <asp:Label ID="lblbuildername" runat="server" CssClass="styleReqFieldLabel" Text="Builder Name"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign">
                                            <asp:TextBox ID="txtbuildername" runat="server" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                <td>
                                    <asp:Label runat="server" ID="lblflatarea" CssClass="styleReqFieldLabel" Text="Flat Area"></asp:Label>  
                                 </td>
                                      <td class="styleFieldAlign">
                                            <asp:TextBox ID="txtflatarea" runat="server" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                    
                                    
                                   
                                    </tr>
                                 <tr>
                                      <td class="styleFieldLabel">
                                            <asp:Label ID="lblbuilduparea" runat="server" CssClass="styleReqFieldLabel" Text="Build up Area"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign">
                                            <asp:TextBox ID="txtbuilduparea" runat="server" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                <td>
                                    <asp:Label runat="server" ID="lblflatno" CssClass="styleReqFieldLabel" Text="Flat No"></asp:Label>  
                                 </td>
                                      <td class="styleFieldAlign">
                                            <asp:TextBox ID="txtflatno" runat="server" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                      
                                    </tr>
                                 <tr>
                                   <td class="styleFieldLabel">
                                            <asp:Label ID="lbladdress" runat="server" CssClass="styleReqFieldLabel" Text="Address"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign">
                                            <asp:TextBox ID="txtaddress" runat="server" AutoPostBack="true" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                <td>
                                    <asp:Label runat="server" ID="lblvalue" CssClass="styleReqFieldLabel" Text="Value"></asp:Label>  
                                 </td>
                                      <td class="styleFieldAlign">
                                            <asp:TextBox ID="txtvalue" runat="server" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                     
                                    </tr>
                               <tr>
                                   <td class="styleFieldLabel">
                                            <asp:Label ID="lbllandsurveyor" runat="server" CssClass="styleReqFieldLabel" Text="Land Surveyor"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign">
                                            <asp:TextBox ID="txtsurveyor" runat="server" AutoPostBack="true"></asp:TextBox>
                                        </td>  
                               </tr>
                             
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="99%">
                            <tr>
                                <td class="styleFieldLabel" align="center" style="width: 99%; padding-left: 35%">
                                    <asp:Button ID="btnOK" ValidationGroup="TabAssetDetails" runat="server" CausesValidation="true"
                                        CssClass="styleSubmitButton" Text="OK" UseSubmitBehavior="False" OnClick="btnOK_Click" />
                                    &nbsp;<asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                                        Text="Cancel" UseSubmitBehavior="False" ValidationGroup="TabAssetDetails" OnClick="btnCancel_Click"/>
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

            window.showModalDialog('../Origination/S3G_ORG_Application_Asset_Homeloan.aspx?qsMaster=Yes', 'Application Asset Details', 'dialogwidth:900px;dialogHeight:900px;');
        }
    </script>

</asp:Content>


