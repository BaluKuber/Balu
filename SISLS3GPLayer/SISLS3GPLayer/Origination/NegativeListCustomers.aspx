<%@ Page Language="C#" Title="Negative List Customers" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="NegativeListCustomers.aspx.cs" Inherits="Origination_NegativeListCustomers" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">




    <table width="100%">
        <tr>
            <td class="stylePageHeading" width="100%">
                
                <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Negative List customers">
                </asp:Label>
            </td>
        </tr>
        

        <tr>
            <td class="styleFieldAlign" align="center">
                 <table width="100%">
                     <tr>

                <td class="styleFieldAlign" align="center">
            <td class="styleFieldLabel">
                <asp:Label ID="lbltext" runat="server" CssClass="styleReqFieldLabel" Text="Browse XML" ></asp:Label></td>
                <td class="styleFieldAlign">
                <asp:FileUpload ID="FileUpload" runat="server" Height="24px" Style="position: static" Width="400px" /> </td></tr></table></td></td>
        </tr>
        <td class="styleFieldAlign">
            <table width="100%">
                <tr>
       
        <td  align="center">
            <asp:Button ID="labelxml" runat="server" Style="position: static" OnClick="Uploadbtn_Click" Font-Size="Small" Text="Upload" Font-Bold="false" Width="104px"></asp:Button>
        </td>
        </tr>

        </table>


    </td>
 <tr>
     <td colspan="100 px"></td>
 </tr>
        <tr>

            <asp:GridView ID="Grid1" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="NAME">
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text='<%# Bind("FIRST_NAME") %>'></asp:Label>


                        </ItemTemplate>



                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Second Name">
 <ItemTemplate>
 <asp:Label ID="lblSec" runat="server" Text='<%Bind("SECOND_NAME") %>' Visible="true"></asp:Label>
 </ItemTemplate>
 </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Passport Number">
                        <ItemTemplate>
                            <asp:Label ID="lblPass" runat="server" Text='<%#Bind("NUMBER") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- <asp:TemplateField HeaderText="Quality">
 <ItemTemplate>
 <asp:Label ID="lblQuality" runat="server" Text='<%#Bind("QUALITY") %>'></asp:Label>
 </ItemTemplate>
 </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="National Identification Number">
                        <ItemTemplate>
                            <asp:Label ID="lblNat" runat="server" Text='<%#Bind("NOTE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </tr>


        <%--    
<td> <asp:Label ID="StatusMessageLbl" runat="server" Style="position: static" Text="Label" Font-Bold="true" Width="304px">
 </asp:Label></td>
        --%>
        <%--<asp:Table ID="Table1" runat="server" Font-Bold="False" BackColor="#ABCDEF" GridLines="Both" CellSpacing="1" HorizontalAlign="Left" EnableViewState="False" BorderStyle="Solid" BorderWidth="1px">
 <asp:TableRow> <asp:TableHeaderCell>Serial No.</asp:TableHeaderCell> <asp:TableHeaderCell>Download Files</asp:TableHeaderCell>
 </asp:TableRow> 
</asp:Table>
 
    </div>
    </form>--%>
    </table>
</asp:Content>

