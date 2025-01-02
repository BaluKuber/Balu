<%@ Control Language="C#" AutoEventWireup="true" CodeFile="S3GFileUpload.ascx.cs" Inherits="S3GFileUpload" %>

  <script language="javascript" type="text/javascript">
  
        function getfile(objFileupload,objtxtFilePath,hidFilePath)
        {           
            objFileupload.click();
            if(objFileupload.value=="")
            {
                hidFilePath.value =  objtxtFilePath.value ;
            }
            else
            {
                hidFilePath.value = objFileupload.value;
            }
        }
     
    </script>

     
  <table>
    <tr>
     <td><input type="file" runat="server" runat="server" id="fileupload" style="display:none "  /></td> 
     <td>   <asp:TextBox ID="txtFilePath" ReadOnly  runat ="server"  ></asp:TextBox> 
     <asp:HiddenField ID="hidFilePath"   runat ="server" />
     <asp:Button ID="btnBrowse" Text="Browse" runat ="server" /> </td>
        
    </tr>
  </table> 
    