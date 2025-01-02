<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerEnquiry.aspx.cs" MasterPageFile="~/Common/S3GMasterPageCollapse.master" Inherits="Add_TabControlPage" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc2" %>
<asp:Content runat="server"   ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" >
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
<tr><td>
<table  class="stylePageHeading"><tr><td>
<asp:Label runat="server" Text="Heading" ID="lblHeading" CssClass="styleInfoLabel" ></asp:Label>
</td></tr></table></td></tr>

<tr>
<td valign="top">
<table cellpadding="0" cellspacing="0" align="left" 
        style="margin-right: 0px; width: 75%;" >
<tr>
<td class="styleFieldLabel" style="width: 142px">Company Code</td>
<td class="styleFieldAlign" style="width:198px">
    <asp:TextBox ID="txtProductCode"  Style="text-transform: uppercase" MaxLength="7"
     
     runat="server" ></asp:TextBox>
    <span class="styleMandatory">*</span>
    </td>
    <td align="left" style="width:200px">

<asp:RequiredFieldValidator ID="reqProductCode"  CssClass="styleMandatoryLabel" 
 runat="server" ControlToValidate="txtProductCode"
   Text="Enter Company Code" Display="Static">
</asp:RequiredFieldValidator></td>
</tr>
<tr>
<td class="styleFieldLabel" style="width: 142px">Company Name</td>
<td  class="styleFieldAlign" style="width: 198px">
    <asp:TextBox ID="txtProductDesc"  runat="server"></asp:TextBox>
    <span class="styleMandatory">*</span>
    
    </td>
    <td>
<cc2:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"  runat="server" TargetControlID="txtProductCode" 
        FilterType="UppercaseLetters,LowercaseLetters"   >
</cc2:FilteredTextBoxExtender>
<cc2:MaskedEditExtender Mask="999-9999-99" runat="server"
 ClearMaskOnLostFocus="false"  ID="maskEditorNum"
 OnFocusCssClass="MaskedEditFocus"
        ErrorTooltipEnabled="true"
        TargetControlID="txtPhone"
 ></cc2:MaskedEditExtender>
<asp:RequiredFieldValidator ID="reqProductDesc" runat="server" ControlToValidate="txtProductDesc"
    Text="Enter Company Name" CssClass="styleMandatoryLabel"   Display="Static">
</asp:RequiredFieldValidator></td>


</td>
</tr>
<tr>
<td class="styleFieldLabel" style="width: 142px" >Constitutional Status</td>
<td  class="styleFieldAlign" style="width: 210px">
    <asp:DropDownList ID="ddlLOB" runat="server" 
      >
    <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
    <asp:ListItem Text="Partnership" Value="1"></asp:ListItem>
    <asp:ListItem Text="Company" Value="2"></asp:ListItem>
    </asp:DropDownList>
    <span class="styleMandatory">*</span>
    </td>
    <td>
    <asp:RequiredFieldValidator ID="reqLOB" CssClass="styleMandatoryLabel" 
     runat="server" ControlToValidate="ddlLOB"
     Text="Select Constitutional Status" InitialValue="-1" Display="Static">
</asp:RequiredFieldValidator>
    </td>
</tr>
<tr>
<td class="styleFieldLabel" style="width: 142px" >Telephone Number</td>
<td  class="styleFieldAlign" style="width: 210px">
    <asp:TextBox ID="txtPhone"  Style="text-transform: uppercase" MaxLength="7"
     
     runat="server"></asp:TextBox>
    
    </td>
    <td>
    
    </td>
</tr>
<tr>
<td class="styleFieldLabel" style="width: 142px; height: 6px;">Active Indicator</td>
<td  colspan="1" class="styleFieldAlign" 
        style="padding-left:2px; width: 198px; height: 6px;">
    <asp:CheckBox ID="chkActive" runat="server" Text=" " />

</td>
    <td style="height: 6px">
    
    </td>
</tr>

</table>
</td>
</tr>

<tr>

<td >
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

    
<table width="100%" >

<tr>
<td valign="bottom" align="Right" > 
    <asp:ImageButton ID="imgbtnLeft" runat="server" CausesValidation="False" 
        ImageUrl="~/Images/layout_button_left.gif" onclick="imgbtnLeft_Click" />
    <asp:ImageButton ID="imgbtnRight" runat="server" CausesValidation="False" 
        ImageUrl="~/Images/layout_button_right.gif" onclick="imgbtnRight_Click" />
</td>
</tr>
<tr>

<td style="Width:95%; height: 246px;" valign="top">

    
<cc2:TabContainer ID="TabContainer1" runat="server"   ActiveTabIndex="0" CssClass="styleTabPanel"
         Width="100%" Height="210px" >
    
        <cc2:TabPanel runat="server"  HeaderText="General Details" ID="TabPanel1" CssClass="tabpan" BackColor="Red">
        
            <HeaderTemplate>
                General Details
            </HeaderTemplate>
        
            <ContentTemplate>
          
            <asp:GridView ID="GridView1" runat="server"  
                                    AllowPaging="True"  
        AllowSorting="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                             onrowdatabound="GridView1_RowDataBound" PageSize=7 
                    AutoGenerateColumns="False">
                                                         <Columns>
   
                                <asp:TemplateField HeaderText="ID" Visible="False" >
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblHeadId" Text='<%# Eval("id") %>'  ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
   
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblName" Text='<%# Eval("name") %>'></asp:Label>
                                        <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("name") %>' 
                                            Visible="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <%--<table>
                                            <tr align="center">
                                                <td>
                                                 --%>
                                        <asp:LinkButton ID="lnkbtnSortName" runat="server" OnClick="DoSort" text="Name" CssClass="styleGridHeader"></asp:LinkButton>
                                        <br />
                                        <br />
                                        <%--    </td>
                                            </tr>
                                            <tr>
                                                <td>--%>
                                        <asp:TextBox ID="txtHeadName" runat="server" AutoPostBack="true" 
                                            OnTextChanged="HeadSearch"></asp:TextBox>
                                        <%--   </td>
                                            </tr>
                                        </table>--%>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField >
                                    <HeaderTemplate>
                                        <%--<table>
                                            <tr align="center">
                                                <td>--%>
                                               <asp:LinkButton runat="server" ID="lnkbtnSortPlace"  text="Place" OnClick="DoSort" CssClass="styleGridHeader"></asp:LinkButton><br /><br />
                                            <%--    </td>
                                            </tr>
                                            <tr>
                                                <td>--%>
                                                    <asp:TextBox runat="server" ID="txtHeadPlace" AutoPostBack="true" 
                                                        OnTextChanged="HeadSearch"></asp:TextBox>
                                          <%--      </td>
                                            </tr>
                                        </table>--%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblPlace" Text='<%# Eval("place") %>'></asp:Label>
                                         <asp:TextBox runat="server" ID="txtPlace" Text='<%# Eval("place") %>' 
                                            Visible="false" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                 <asp:TemplateField >
                                    <HeaderTemplate>
                                        <%--<table>
                                            <tr align="center">
                                                <td>--%>
                                               <asp:LinkButton runat="server" ID="lnkbtnSortCapital"  text="Capital" CssClass="styleGridHeader"
                                                        OnClick="DoSort"></asp:LinkButton><br /><br />
                                              <%--  </td>
                                            </tr>
                                            <tr>
                                                <td>--%>
                                                    <asp:TextBox runat="server" ID="txtHeadCapital" AutoPostBack="true" 
                                                        OnTextChanged="HeadSearch"></asp:TextBox>
                                            <%--    </td>
                                            </tr>
                                        </table>--%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblCapital" Text='<%# Eval("capital") %>'></asp:Label>
                                         <asp:TextBox runat="server" ID="txtCapital" Text='<%# Eval("capital") %>' 
                                            Visible="false" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                  <asp:TemplateField >
                                    <HeaderTemplate>
                                       <%-- <table>
                                            <tr align="center">
                                                <td>--%>
                                               <asp:LinkButton runat="server" ID="lnkbtnSortCountry"  text="Country" CssClass="styleGridHeader"
                                                        OnClick="DoSort"></asp:LinkButton><br /><br />
                                               <%-- </td>
                                            </tr>
                                            <tr>
                                                <td>--%>
                                                    <asp:TextBox runat="server" ID="txtHeadCountry" AutoPostBack="true" 
                                                        OnTextChanged="HeadSearch" EnableViewState="true"></asp:TextBox>
                                           <%--     </td>
                                            </tr>
                                        </table>--%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblCountry" Text='<%# Eval("country") %>'></asp:Label>
                                         <asp:TextBox runat="server" ID="txtCountry" Text='<%# Eval("country") %>' 
                                            Visible="false" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                            </Columns>
                                                        
                                                        
                                                        
                        </asp:GridView>
            
            
            
            </ContentTemplate></cc2:TabPanel>
        
        <cc2:TabPanel runat="server"  HeaderText="Personal Details" ID="TabPanel2">
        <ContentTemplate>Page 2</ContentTemplate>
        </cc2:TabPanel>
      
        <cc2:TabPanel runat="server"    HeaderText="Bank Details" ID="TabPanel3">
        <ContentTemplate>Page 3</ContentTemplate>
        </cc2:TabPanel>
       
        <cc2:TabPanel runat="server" HeaderText="Asset Details" ID="TabPanel4"></cc2:TabPanel>
    
    <cc2:TabPanel runat="server" HeaderText="Loan Details" ID="TabPanel5"></cc2:TabPanel>
        
        <cc2:TabPanel runat="server" HeaderText="Vehicle Details" ID="TabPanel6"></cc2:TabPanel>
      
        <cc2:TabPanel runat="server" HeaderText="Family Details" ID="TabPanel7"></cc2:TabPanel>
       
        <cc2:TabPanel runat="server" HeaderText="Loan Enquiry" ID="TabPanel8"></cc2:TabPanel>
    
    <cc2:TabPanel runat="server" HeaderText="Business Details" ID="TabPanel9">
    <ContentTemplate>Page 9</ContentTemplate>
    </cc2:TabPanel>
        
        <cc2:TabPanel runat="server" HeaderText="EMI Details" ID="TabPanel10">
           
            <ContentTemplate>Page 10</ContentTemplate></cc2:TabPanel>
            
<cc2:TabPanel runat="server" HeaderText="Other Informations" ID="TabPanel11">
           
            <ContentTemplate>Page 11</ContentTemplate></cc2:TabPanel>

<cc2:TabPanel runat="server" HeaderText="Documents Details" ID="TabPanel12">
            
            <ContentTemplate>Page 12</ContentTemplate></cc2:TabPanel>
            
<cc2:TabPanel runat="server" HeaderText="Terms & Conditions" ID="TabPanel13">
            
            <ContentTemplate>Page 13</ContentTemplate></cc2:TabPanel>            
            
            
</cc2:TabContainer>


</td>

</tr>
<tr><td>
<table cellpadding="0" width="100%" cellspacing="0"><tr>
<td align="right" class="styleTabSubmitTd"><asp:Button runat="server" ID="btnSave"
 CssClass="styleSubmitButton" Text="Submit"  onclick="btnSave_Click"  /></td>
 <td align="right">
<asp:Button runat="server" ID="btnNext" runat="server" CausesValidation="false"
 CssClass="styleButtonNext" Text=">> Next"  onclick="btnNext_Click"  />
</td>
 </tr>
</table>
</td></tr>
</table>


</ContentTemplate>
    </asp:UpdatePanel>
    
</td>

</tr>
</table>

<script language="javascript" type="text/javascript">


    function getUpperCase(e) 
    {
        alert(e.keyCode);
    }

  
</script>



</asp:Content>