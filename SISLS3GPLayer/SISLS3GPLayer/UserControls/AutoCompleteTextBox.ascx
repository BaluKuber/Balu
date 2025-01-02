<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AutoCompleteTextBox.ascx.cs"
    Inherits="UserControls_AutoCompleteTextBox" %>
<style type="text/css">
    .ui-autocomplete
    {
        height: 180px;
        overflow-y: scroll;
    }
    .ui-autocomplete-loading
    {
        background-position: right;
        background-repeat: no-repeat;
        background-image: url(Images/animation_processing.gif);
    }
</style>

<script type="text/javascript">
    $(document).ready(function() {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);

        // Place here the first init of the autocomplete
        //StartUp();
    });

    function InitializeRequest(sender, args) {
        //        var Param = document.getElementById('<%= HdnParams.ClientID %>').value;
        //        StartUp(Param);
    }

    function EndRequest(sender, args) {
        // after update occur on UpdatePanel re-init the Autocomplete
        var Param = document.getElementById('<%= HdnParams.ClientID %>').value;
        StartUp_<%= Control_ID %>(Param);
    }





    function StartUp_<%= Control_ID %>(Param) {
        var Controls = document.getElementById('<%= HdnControlClientID.ClientID %>').value;
        var ControlParam = document.getElementById('<%= HdnControlParameters.ClientID %>').value;
        var txtBox = document.getElementById('<%= txtAutoComplete.ClientID %>');
        var Properties = document.getElementById('<%= HdnOtherProps.ClientID %>').value.split("|");
        var MinimumLength = Properties[0];
        var MethodName = Properties[1];
        var SearchFieldName = Properties[2];
        var TextField = Properties[3];
        var ValueField = Properties[4];
        var Parameters = Param;


        if (ValueField == "" || TextField == "" || MethodName == "" || SearchFieldName == "") {
            alert('TextField,ValueField,ProcName,SearchField is Mandatory');
            return;
        }

        $('.autocomplete').autocomplete({
            source: function(request, response) {

                var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
                var Control = Controls.split("|");
                var ControlsParam = ControlParam.split("|");
                var ExtraParams = "";
                if (Control[0] != "" && ControlsParam != "") {
                    for (var i = 0; i < Control.length; i++) {
                        if (Control[i] != "undefined" && ControlsParam[i] != "undefined") {
                            var s = document.getElementById(Control[i]);
                            var SelectedValues = "";
                            if (s.nodeName == "SELECT") {
                                SelectedValues = s.options[s.selectedIndex].value;
                            } else if (s.nodeName == "INPUT") {
                                SelectedValues = s.value;
                            }
                            if (ExtraParams == "") {
                                ExtraParams = ControlsParam[i] + "," + SelectedValues;
                            } else {
                                ExtraParams += "#" + ControlsParam[i] + "," + SelectedValues;
                            }
                        } else {
                            alert('Error in Control Parameters');
                            return;
                        }
                    }
                }

                
                $.ajax('WebService.aspx', {
                    dataType: "json",
                    data: { MethodName: MethodName, Parameters: Parameters, SearchFieldName: SearchFieldName, Search: request.term, ValueField: ValueField, TextField: TextField, ExtraParams: ExtraParams },
                    success: function(data) {

                        response($.map(data, function(v, i) {
                            var text = v.Name;
                            if (text && (!request.term || matcher.test(text))) {
                                return {
                                    label: v.Name,
                                    value: v.Name,
                                    hidval: v.Value
                                };
                            } else {
                                document.getElementById('<%= HdnSelValue.ClientID %>').value = "";
                            }
                        }));
                    }, error: function(d) {
                        alert(d);
                    }
                });
            },
            select: function(event, ui) {
                document.getElementById('<%= HdnSelValue.ClientID %>').value = ui.item.hidval;
                document.getElementById('<%= txtAutoComplete.ClientID %>').value = ui.item.label;
                if (document.getElementById('<%= HdnAutoPostBack.ClientID %>').value == "1") {
                    document.getElementById('<%= BtnPostBack.ClientID %>').click();
                } else {
                    document.getElementById('<%= BtnAsync.ClientID %>').click();
                }

            },
            minLength: MinimumLength,
            delay: 300,
            matchContains: true,
            change: function(event, ui) {
                //document.getElementById('<%= txtAutoComplete.ClientID %>').focus();
                //                debugger;
                //                if (document.getElementById('<%= HdnAutoPostBack.ClientID %>').value == "1") {
                //                    document.getElementById('<%= BtnPostBack.ClientID %>').click();
                //                } else {
                //                    document.getElementById('<%= BtnAsync.ClientID %>').click();
                //                }
            }
        });
    }
</script>

<asp:UpdatePanel ID="up" runat="server">
    <ContentTemplate>
        <asp:TextBox ID="txtAutoComplete" runat="server" Width="200px" CssClass="autocomplete"></asp:TextBox>
        <asp:Button ID="BtnAsync" runat="server" OnClick="BtnAsync_Click" Style="display: none;" />
        <asp:Button ID="BtnPostBack" runat="server" OnClick="BtnPostBack_Click" Style="display: none;" />
        <asp:HiddenField ID="HdnSelValue" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="HdnAutoPostBack" runat="server" />
        <asp:HiddenField ID="HdnOtherProps" runat="server" />
        <asp:HiddenField ID="HdnControlClientID" runat="server" />
        <asp:HiddenField ID="HdnControlParameters" runat="server" />
        <asp:HiddenField ID="HdnParams" runat="server" />
        <asp:HiddenField ID="hdnControlID" runat="server" />
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="BtnPostBack" />
    </Triggers>
</asp:UpdatePanel>
