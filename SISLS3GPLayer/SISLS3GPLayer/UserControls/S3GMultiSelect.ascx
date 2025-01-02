<%@ Control Language="C#" AutoEventWireup="true" CodeFile="S3GMultiSelect.ascx.cs"
    Inherits="UserControls_S3GMultiSelect" %>
<table cellpadding="0" cellspacing="0" runat="server" id="tblContent">
    <tr>
        <td class="styleFieldAlign" width="100%">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td width="100%">
                        <table cellpadding="0" cellspacing="0" class="styleTableData" width="100%" runat="server" id="tblInner">
                            <tr>
                                <td style="height: 16px">
                                    <asp:TextBox ID="txtItemName" runat="server" Width="160px" Style="border: none; height: 14px;
                                        cursor: default;" Text="--Select--"></asp:TextBox>
                                </td>
                                <td valign="middle" height="10px" align="right" >
                                    <fieldset id="fldButton" runat="server" class="accordionHeaderSelected" style="cursor: pointer; height: 7px; width: 13px; border-width:1px; border-color:#bad4ff; border-style:solid">
                                        <asp:Image ID="imgShowList" runat="server" ImageUrl="../Images/search_blue.gif" Style="cursor: pointer;
                                            height: 14px; vertical-align: top; margin-top: -3px" />
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlDropDownView" runat="server" Style="display: none; position: absolute;"
                            BackColor="White" CssClass="styleTableData" ScrollBars="Vertical" Height="200px">
                            <asp:GridView ID="grvList" runat="server" HorizontalAlign="Center" AutoGenerateColumns="False"
                                EmptyDataText="No Items loaded" OnRowDataBound="grvList_RowDataBound" Style="border: none" Width="188px">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll" runat="server" Text="All" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                            <asp:Label ID="lblID" Style="display: none" runat="server" Text='<%#Bind("ID") %>'></asp:Label>
                                            <asp:Label ID="lblText" runat="server" Style="display: none" Text='<%#Bind("Text") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle BorderColor="White" />
                            </asp:GridView>
                            <asp:HiddenField ID="hdnAutoPostBack" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnControlID" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnDoPostBack" runat="server" Value="0" />
                             <asp:HiddenField ID="hdnDisableCheck" runat="server" Value="0" />
                            <asp:TextBox ID="hdnSelectedValue" runat="server" Text="0" Style="display: none;
                                position: absolute" />
                            <asp:Button ID="btnSelected" runat="server" Style="display: none; position: absolute"
                                OnClick="btnItem_Selected" />
                            <asp:RequiredFieldValidator ControlToValidate="hdnSelectedValue" ID="rfvMultiSelect"
                                Enabled="false" runat="server" Display="None" InitialValue="0">
                            </asp:RequiredFieldValidator>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

<script language="javascript" type="text/javascript">

    var strControlID_<%=Control_ID %> = document.getElementById('<%=hdnControlID.ClientID %>').value + '_';

    function fnSelectOne_DDL_<%=Control_ID %>(chkSelectBranch, chkSelectAllBranch) {
        //debugger;
        var gvList = document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'grvList');
        var TargetChildControl = chkSelectAllBranch;
        var selectall = 0;
        var Inputs = gvList.getElementsByTagName("input");
        if (!chkSelectBranch.checked) {
            chkSelectAllBranch.checked = false;
        }
        else {
            //debugger;
            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'checkbox') {
                    if (Inputs[n].checked) {
                        selectall = selectall + 1;
                    }
                }
            }
            if (selectall == gvList.rows.length - 1) {
                chkSelectAllBranch.checked = true;
            }
        }

        fnAssignText_<%=Control_ID %>(gvList, chkSelectAllBranch);

    }

    function fnAssignText_<%=Control_ID %>(gvList, chkSelectAllBranch) {
        var strSelectedText = "";
        var strSelectedId = "0,";
        for (var n = 1; n < gvList.rows.length; ++n) {
            var index;
            if ((n + 1).toString().length == 1) {
                index = '0' + (n + 1);
            }
            else
                index = n + 1;
            var varChkBx = document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'grvList_ctl' + index + '_chkSelect');
            var varID = document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'grvList_ctl' + index + '_lblID');
            var varText = document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'grvList_ctl' + index + '_lblText');
            if (varChkBx.type == 'checkbox') {
                if (varChkBx.checked) {
                    strSelectedText = strSelectedText + varText.innerText + ',';
                    strSelectedId = strSelectedId + varID.innerText + ',';
                }
            }
        }

        document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'hdnSelectedValue').value = strSelectedId.substring(0, strSelectedId.length - 1);
        document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'txtItemName').value = strSelectedText.substring(0, strSelectedText.length - 1);
        if (chkSelectAllBranch.checked) {
            document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'txtItemName').value = 'All';
        }
        if (strSelectedText.length == 0) {
            document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'txtItemName').value = '--Select--';
        }

        var varAutoPostBack = document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'hdnAutoPostBack').value;
        if (varAutoPostBack == '1')
            document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'hdnDoPostBack').value = '1';

        document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'txtItemName').focus();
    }

    function fnSelectAll_DDL_<%=Control_ID %>(chkSelectAllBranch, chkSelectBranch) {
        //debugger;
        var gvList = document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'grvList');
        var TargetChildControl = chkSelectBranch;
        var Inputs = gvList.getElementsByTagName("input");
        for (var n = 0; n < Inputs.length; ++n)
            if (Inputs[n].type == 'checkbox' &&
            Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
            Inputs[n].checked = chkSelectAllBranch.checked;
        }

        fnAssignText_<%=Control_ID %>(gvList, chkSelectAllBranch);
    }

    function fnShowPopup_<%=Control_ID %>(panel) {
        document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + panel).style.display = 'Block';
    }

    function fnHidePopup_<%=Control_ID %>(panel) {
        //debugger;
        var AutoPostBack = document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'hdnAutoPostBack').value;
        var doPostBack = document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'hdnDoPostBack').value;
        if (AutoPostBack == '1' && doPostBack == '1') {
            document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'hdnDoPostBack').value = '0';
            document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'btnSelected').click();
        }
        document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + panel).style.display = 'none';
    }

    function fnMouseoverTooltip_<%=Control_ID %>(table) {
        table.title = document.getElementById('ctl00_ContentPlaceHolder1_' + strControlID_<%=Control_ID %> + 'txtItemName').value;
    }
</script>

