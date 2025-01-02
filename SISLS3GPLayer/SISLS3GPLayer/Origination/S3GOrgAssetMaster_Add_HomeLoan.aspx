<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GOrgAssetMaster_Add_HomeLoan.aspx.cs" Inherits="Origination_S3GOrgAssetMaster_Add_HomeLoan" %>
<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Asset Master Home Loan" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
      <tr>
            <td>
                <cc1:TabContainer ID="TabContainerAP" runat="server" CssClass="styleTabPanel" Width="100%"
                                        ScrollBars="None" ActiveTabIndex="2" onchange="fnSetDivWidth();">
                                        <cc1:TabPanel runat="server" ID="TabMainPage" CssClass="tabpan" BackColor="Red" Width="100%" TabIndex="0">
                                            <HeaderTemplate>
                                                Main Page
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="upMainPage" runat="server">
                                                    <ContentTemplate>
                                                       
                        <asp:Panel runat="server" ID="pnlassettype" GroupingText="Asset Type" CssClass="stylePanel">
                            &nbsp;&nbsp;
                             <table width="100%">
                                                            <tr>
                                                                <td class="styleFieldLabel"  width="25%">
                            <asp:Label runat="server" Text="Asset Type" ID="lblAssetType" CssClass="styleFieldLabel">
                            </asp:Label>
                                                                    </td>
                                                                <td class="styleFieldLabel"  width="25%">
                            <asp:DropDownList ID="ddlAssetType" runat="server" OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged"
                                AutoPostBack="True" 
                               >
                            </asp:DropDownList>
                                                                    </td>
                                                                    <td class="styleFieldLabel"  width="25%" >
                                                                                                <asp:Label runat="server" ID="lblassetcode" CssClass="styleFieldLabel" Text="Asset Code"></asp:Label>
                                                                                            </td>
                                                                                            <td class="styleFieldAlign"  width="25%">
                                                                                                <asp:TextBox ID="txtassetcode" runat="server"></asp:TextBox>
                                                                                              
                                                                                            </td>
                                                            </tr>
                                  <tr>
                                                                <td class="styleFieldLabel"  width="25%">
                            <asp:Label runat="server" Text="Asset Description" ID="Label1" CssClass="styleFieldLabel">
                            </asp:Label>
                                                                    </td>
                                                                <td class="styleFieldLabel"  width="25%">
                          <asp:TextBox ID="txtassetdesc" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                                    </td>
                                                                    <td class="styleFieldLabel"  width="25%" >
                                                                                                <asp:Label runat="server" ID="lblisactive" Text="Is Active" CssClass="styleFieldLabel"></asp:Label>
                                                                                            </td>
                                                                                            <td class="styleFieldAlign"  width="25%">
                                                                                                 <asp:CheckBox runat="server" ID="chkactive" Text="Is Active" CssClass="styleFieldLabel"></asp:CheckBox>
                                                                                              </td>
                                                            </tr>
                                 </table>
                        </asp:Panel>
                                                                   
                    </ContentTemplate>
                </asp:UpdatePanel>
                                                </ContentTemplate>
                                            </cc1:TabPanel>

                     <cc1:TabPanel runat="server" ID="tabpaneldetail" CssClass="tabpan" BackColor="Red" Width="100%">
                                            <HeaderTemplate>
                                                Details
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                       
                        <asp:Panel runat="server" ID="pnldetails" GroupingText="Details" CssClass="stylePanel">
                            <asp:GridView ID="gvAccountLedger" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                            RowStyle-Width="0" Width="100%" OnRowDataBound="gvAccountLedger_RowDataBound" OnRowCommand="gvAccountLedger_RowCommand" >
                                            <Columns>
                                                <%--Serial Number--%>
                                                 <asp:TemplateField HeaderText="Sl No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1%>' ToolTip="Serial Number" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--Location--%>
                                                <asp:TemplateField HeaderText="Flag">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblflag" runat="server" Text='<%#Eval("Flag") %>' ToolTip="Flag" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                         <asp:DropDownList ID="ddlflagfooter" runat="server" ToolTip="Flag" />
                                                    </FooterTemplate>
                                                     <ItemStyle HorizontalAlign="Left"/>
                                                </asp:TemplateField>
                                                <%--Doc Date --%>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtdescripition" runat="server" Text='<%#Eval("Description") %>' ToolTip="Description" />
                                                    </ItemTemplate>
                                                      <FooterTemplate>
                                                         <asp:TextBox ID="txtdescripitionfooter" runat="server" ToolTip="Description" />
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Left"/>
                                                </asp:TemplateField>
                                                <%--Doc No --%>
                                                <asp:TemplateField HeaderText="Characteristics">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="ddlchar" runat="server" Text='<%#Eval("Characteristics") %>' ToolTip="Characteristics" />
                                                    </ItemTemplate>
                                                     <FooterTemplate>
                                                         <asp:DropDownList ID="ddlcharfooter" runat="server" ToolTip="Characteristics" />
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Left"/>
                                                </asp:TemplateField>
                                        
                                              <asp:TemplateField HeaderText="Mandatory Value">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkmandatory" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Mandatory_value")))%>' ToolTip="Mandatory Value" />
                                                        <asp:Label ID="lblmandatoryvalue" runat="server" Text='<%#Eval("Mandatory_value") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                   <FooterTemplate>
                                                         <asp:CheckBox ID="chkmandatoryvalue" runat="server" ToolTip="Mandatory Value" />
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Mandatory Scan">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkmandatoryscan" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "mandatory_scan")))%>' ToolTip="Mandatory Scan" />
                                                         <asp:Label ID="lblmandatoryscan" runat="server" Text='<%#Eval("mandatory_scan") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                     <FooterTemplate>
                                                         <asp:CheckBox ID="chkmandatoryscanfooter" runat="server" ToolTip="Mandatory Scan" />
                                                        
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Mandatory Documents">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkmandatorydocuments" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Mandatory_Document")))%>'  ToolTip="Mandatory Documents" />
                                                          <asp:Label ID="lblmandatorydocuments" runat="server" Text='<%#Eval("Mandatory_Document") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                         <FooterTemplate>
                                                         <asp:CheckBox ID="chkmandatorydocumentsfooter" runat="server" ToolTip="Mandatory Documents" />
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Link">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkLink" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "link")))%>'  ToolTip="Link" />
                                                        <asp:Label ID="lbllink" runat="server" Text='<%#Eval("link") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                              <FooterTemplate>
                                                         <asp:CheckBox ID="chkLinkfooter" runat="server" ToolTip="Link" />
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                    
                                                  <asp:TemplateField HeaderText="GP Desc.">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtgpdesc" runat="server" Text='<%#Eval("Gp_Desc") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                       <FooterTemplate>
                                                         <asp:TextBox ID="txtgpdescfooter" runat="server" ToolTip="Group Description" />
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                          <asp:TemplateField>
                                                                <ItemTemplate>
                                                                   <asp:LinkButton ID="lnkRemove" runat="server" Text="Delete" CommandName="Delete"
                                                                        ToolTip="Delete" 
                                                                        CausesValidation="false"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                
                                                                <FooterTemplate>
                                                                    <br></br>
                                                                    <asp:LinkButton ID="lnkAdd" runat="server" Text="Add" CommandName="Add" ValidationGroup="vgAdd"
                                                                        ToolTip="Add" CausesValidation="true"></asp:LinkButton>
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <FooterStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                           
                        </asp:Panel>
                                                                   
                    </ContentTemplate>
                </asp:UpdatePanel>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                    </cc1:TabContainer>
            </td>
        </tr>
    </table>
      <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td height="10px">
                    </td>
                </tr>
                <tr width="100%">
                    <td align="center" width="10%">
                        <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" Text="Save" ToolTip="Save" OnClick="btnSave_Click" />
                         <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                             Text="Clear" ToolTip="Clear" />
                        
                    </td>
                </tr>
                <tr>
                    <td height="10px">
                    </td>
                </tr>
                <tr>
                    <td align="Left">
                        <asp:ValidationSummary HeaderText="Please correct the following validation(s):" ID="vsFIR"
                            runat="server" ValidationGroup="VGRequest" CssClass="styleMandatoryLabel" />
                       
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel" />
                    </td>
                </tr>
                <tr>
                    <td height="10px">
                    </td>
                </tr>
            </table>
    
</asp:Content>

