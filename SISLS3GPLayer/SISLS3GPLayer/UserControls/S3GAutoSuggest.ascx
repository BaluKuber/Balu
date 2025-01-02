<%@ Control Language="C#" AutoEventWireup="true" CodeFile="S3GAutoSuggest.ascx.cs"
    Inherits="UserControls_S3GAutoSuggest" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table cellpadding="0" cellspacing="0" runat="server" id="tblContent">
    <tr>
        <td width="100%">
            <asp:UpdatePanel ID="dfd" runat="server">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <asp:TextBox ID="txtItemName" runat="server" Width="160px"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="txtBranchSearchExtender" runat="server" TargetControlID="txtItemName"
                                    WatermarkText="--Select--">
                                </cc1:TextBoxWatermarkExtender>
                                <asp:Button ID="btnSelected" runat="server" Style="display: none; position: absolute"
                                    OnClick="btnItem_Selected" CausesValidation="false" />
                                <asp:RequiredFieldValidator ControlToValidate="hdnSelectedValue" ID="rfvMultiSuggest"
                                    Enabled="false" runat="server" Display="None" InitialValue="0">
                                </asp:RequiredFieldValidator>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                  TargetControlID="txtItemName" ValidChars=" ,(,),/,&,.,-" InvalidChars=",<,>,;,'" runat="server" Enabled="True">
                                </cc1:FilteredTextBoxExtender>
                                <cc1:AutoCompleteExtender ID="autoCompletor" MinimumPrefixLength="3" runat="server" FirstRowSelected="true"
                                    TargetControlID="txtItemName" Enabled="True" CompletionSetCount="5" CompletionListCssClass="CompletionList"
                                    DelimiterCharacters=";,:" CompletionListItemCssClass="CompletionListItemCssClass"
                                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                    ShowOnlyCurrentWordInCompletionListItem="true" CompletionInterval="2">
                                </cc1:AutoCompleteExtender>
                                <asp:HiddenField ID="hdnAutoPostBack" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnControlID" runat="server" Value="0" />
                                <asp:TextBox ID="hdnSelectedValue" runat="server" Text="0" Style="display: none;
                                    position: absolute" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>

<script language="javascript" type="text/javascript">

    function AutoSuggest_ItemSelected(sender, e) 
    {    
        var strControlID = sender._id.replace('_autoCompletor','');
        var hdnSelectedValue =  document.getElementById(strControlID + '_hdnSelectedValue');
        if (e.get_value() == "-1") 
        {
            document.getElementById(strControlID + '_txtItemName').value = "";
            hdnSelectedValue.value = "0";
        }
        else 
        {
            document.getElementById(strControlID + '_txtItemName').value = e._text;
            hdnSelectedValue.value = e.get_value();
        }
        if(e.get_value() != "-1")
        {
            AutoSuggest_doPostBack(strControlID);
        }
    }

    function AutoSuggest_ItemPopulated(sender, e) {
        var strControlID = sender._id.replace('_autoCompletor','');
        document.getElementById(strControlID + '_hdnSelectedValue').value = 0;
    }

    function AutoSuggest_doPostBack(strControlID) {

        var AutoPostBack = document.getElementById(strControlID + '_hdnAutoPostBack').value;
        if (AutoPostBack == '1') {
            document.getElementById(strControlID + '_btnSelected').click();            
        }
    }

    function AutoSuggest_ShowOptions(sender, e) {
        sender._completionListElement.style.zIndex = 10000001;
    }

</script>

