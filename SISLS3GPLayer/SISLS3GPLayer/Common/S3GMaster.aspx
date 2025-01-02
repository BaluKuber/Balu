<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="S3GMaster.aspx.cs"
    Inherits="Common_S3GMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--    <script type="text/javascript" src="../Scripts/Common.js"></script>--%>
    <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
    <title>SMARTLEND 3G</title>
     <link rel="shortcut icon" href="../Images/TemplateImages/1/CompanyLogo_Old.png"/>
    <link href="../JQueryTabs/CSS/ui.tabs.css" rel="stylesheet" type="text/css" />

    <script src="../JQueryTabs/JQuery/jquery-1.2.6.min.js" type="text/javascript"></script>

    <script src="../JQueryTabs/JQuery/ui.core_New.js" type="text/javascript"></script>

    <script src="../JQueryTabs/JQuery/ui.tabs_New.js?v=1" type="text/javascript"></script>

    <script src="../JQueryTabs/JQuery/CreateTabs.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        window.onerror = function () { return true; }
        function autoResize(id) {
            var newheight;
            var newwidth;
            try {
                if (document.getElementById) {
                    newheight = document.getElementById(id).contentWindow.document.body.scrollHeight;
                    newwidth = document.getElementById(id).contentWindow.document.body.scrollWidth;
                }

                if (newheight < 420) {
                    document.getElementById(id).height = 420;
                }
                else {
                    document.getElementById(id).height = (newheight + 5) + "px";
                }
                //document.getElementById(id).width = (screen.width - 50) + "px";

                for (var i = 0; i <= window.frames.length - 1; i++) {
                    // window.frames[i].document.body.clientWidth = (screen.width - 275);
                    //window.frames[i].document.body.style.width = (screen.width - 275) + "px";
                }

            }
            catch (e) {
            }

        }
        function GetResize(img) {

            try {
                //img = document.getElementById('imgDetails');
                //debugger;
                if (img.title != null) {

                    if (img.title == "Hide Menu") {
                        for (var i = 0; i <= window.frames.length - 1; i++) {

                            window.frames[i].window.frameElement.width = (screen.width - 53);
                            window.frames[i].window.document.body.style.width = (screen.width - 53);

                            try {
                                if (window.frames[i].GetChildGridResize(img.title) != null)
                                    window.frames[i].GetChildGridResize(img.title);
                            }
                            catch (e) {
                            }
                        }
                    }
                    else {
                        for (var i = 0; i <= window.frames.length - 1; i++) {

                            window.frames[i].window.frameElement.width = (screen.width - 257);
                            window.frames[i].window.document.body.style.width = (screen.width - 257);

                            try {
                                if (window.frames[i].GetChildGridResize(img.title) != null)
                                    window.frames[i].GetChildGridResize(img.title);
                            }
                            catch (e) {
                            }
                        }
                    }
                }
                //debugger;
                for (var i = 0; i <= window.frames.length - 1; i++) {
                    var newheight = window.frames[i].frameElement.clientHeight + 20;
                    window.frames[i].window.document.body.style.height = newheight;
                }
            }
            catch (e) {
            }

        }

        function widowminimize() {

            window.blur();
        }

        function funUnload() {
            if (window.opener != null) {
                window.opener.reload(true);
            }
        }

        function CancelF12() {

            var KeyAscii = window.event.keyCode;

            if (KeyAscii == 123) {

                window.event.returnValue = false;

                window.event.keyCode = 0;

            }

        }


        function GetMenuVisibility() {

            var all_Cookies = document.cookie.split(';');

            var tmp_Cookie;
            var Cookie_Value = 1;
            var Cookie_Name;
            //debugger;
            for (var i = 0; i < all_Cookies.length; i++) {
                tmp_Cookie = all_Cookies[i].split('=');
                Cookie_Name = tmp_Cookie[0];
                if (Cookie_Name == 'Menu') {
                    Cookie_Value = tmp_Cookie[1];
                    break;
                }
            }

            if (Cookie_Value == 0) {
                showMenu('F');
            }
            else {
                showMenu('T');
            }
        }

        function SetMenuVisibility(CanShow) {
            document.cookie = "Menu=" + CanShow + ";";
        }

        function FloatMenu() {
            $get('divMenu').style.position = 'Absolute';
            $get('divMenu').style.background = 'White';
            $get('divMenu').style.Height = '450px';
            $get('divMenu').style.border = 1;
        }

        //window.onload = window_onload;
        var prm;
        function window_onload() {
            prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);
            prm.add_pageLoaded(PageLoaded);
            //document.getElementById('divLoading').style.display='none';
            //GetMenuVisibility();
        }


        function InitializeRequest(sender, args) {
            $get('Container').style.cursor = 'wait';
            //$get('Container').style.cursor='wait';
            $get('Container').disabled = true;

            //  document.getElementById('divLoading').style.display='Block';

            //$get('Container').className='Progress';
            if (prm.get_isInAsyncPostBack()) {
                args.set_cancel(true);
                $get('Container').disabled = false;
                //  $get('Container').className='';
                $get('Container').style.cursor = 'default';
                // document.getElementById('divLoading').style.display='none';
            }

        }

        function PageLoaded(sender, args) {
            // alert(sender);
            //debugger;



            $get('Container').style.cursor = 'auto';
            //$get('Container').style.cursor='auto';
            $get('Container').disabled = false;
            //  document.getElementById('divLoading').style.display='none';
            //  $get('Container').className='';
            //autoResize('ifrmMain');



        }
        function EndRequest(sender, args) {
            if (args.get_error() != undefined) {
                $get('Container').style.cursor = 'auto';
                $get('Container').disabled = false;
                // document.getElementById('divLoading').style.display='none';
                args.set_errorHandled(true);
            }

            debugger;
            //This lines are used to create new tab after Master page get postback
            if (sender._postBackSettings != null && document.getElementById(sender._postBackSettings.sourceElement.id).parentNode.parentNode.parentNode.id == 'Accordion1') {
                parent.CreateTabForChild(document.getElementById(sender._postBackSettings.sourceElement.id).lastChild.nodeValue, document.getElementById(sender._postBackSettings.sourceElement.id).attributes["PageUrl"].nodeValue);
            }

        }
        function abort() {
            alert(prm.get_isInAsyncPostBack());
            if (prm.get_isInAsyncPostBack()) {
                //  abort the postback
                prm.abortPostBack();
                //  get the reference to the animation for the gridview
                $get('Container').style.cursor = 'auto';
                $get('Container').disabled = false;
                //  document.getElementById('divLoading').style.visible='none';

            }
        }

        function fnLoadIframe(intModuleCount, intProgCount) {
            //debugger;

            var curProgram = document.getElementById('Accpane' + intModuleCount + '_content_lbtn' + intProgCount);
            var frmTabs = $('#Container ul');
            var iFrames = document.getElementsByTagName('iframe');
            var existTab = -1;

            for (var i = 0; i <= iFrames.length - 1; i++) {
                if (iFrames[i].src.indexOf(curProgram.attributes["ViewPage"].nodeValue) != -1
                    || iFrames[i].src.indexOf(curProgram.attributes["DetailPage"].nodeValue) != -1) {
                    existTab = i;
                }
            }

            if (existTab == -1) {
                parent.CreateTabForChild(curProgram.innerText, curProgram.attributes["PageUrl"].nodeValue);
            }
            else {
                frmTabs.tabs('select', existTab);
            }

            document.getElementById('divTabClose').style.display = 'none';

            //fnSetCurMenuProgram();

            //document.getElementById('hdnImgBtn').value = imgBtn;
        }

        function fnCloseCurrent(ctrl) {

            if (ctrl == '1')
                fnShowTabCloseMenu();
            else
                document.getElementById('divTabClose').style.display = 'none';

            var frmTabs = $('#Container ul');
            var iFrames = document.getElementsByTagName('iframe');
            var selectedIndex = frmTabs.tabs("option", "selected");
            if (selectedIndex != 0) {
                frmTabs.tabs('remove', selectedIndex);
            }
        }

        function fnShowTabCloseMenu() {
            var disply = document.getElementById('divTabClose').style.display;
            if (disply == 'none') {
                document.getElementById('divTabClose').style.display = 'block';
                document.getElementById('divTabClose').focus();
            }
            else
                document.getElementById('divTabClose').style.display = 'none';
        }

        function escapeMenu(e) {
            if (e.keyCode == 27) {
                fnShowTabCloseMenu();
            }
        }

        function fnCloseAll(ExceptTab) {

            fnShowTabCloseMenu();

            var frmTabs = $('#Container ul');
            var iFrames = document.getElementsByTagName('iframe');
            var selectedIndex = 0;
            var frameID = '-1';
            if (ExceptTab != null) {
                selectedIndex = frmTabs.tabs("option", "selected");
                frameID = iFrames[selectedIndex].id;
            }
            //debugger;
            for (var i = 1; i <= iFrames.length - 1; i++) {
                if (iFrames[i].id != frameID) {
                    frmTabs.tabs('select', i);
                    frmTabs.tabs('remove', i);
                    i = i - 1;
                }
            }
            
        }

        function fnSetCurMenuProgram() {
            //debugger;
            var frmTabs = $('#Container ul');
            var selectedIndex = frmTabs.tabs("option", "selected");
            var imgBtns = (document.getElementById('divMenu')).getElementsByTagName('img');
            var lbtns = (document.getElementById('divMenu')).getElementsByTagName('a');
            var existTab = -1;
            var iFrames = document.getElementsByTagName('iframe');
            var iFrame = iFrames[selectedIndex];
            var ddlTheme = document.getElementById('ddlTheme');
            var varTheme = ddlTheme.options(ddlTheme.selectedIndex).value;
            var accIndex = 0;

            document.getElementById('divTabClose').style.display = 'none';

            try {
                if (iFrame != undefined && iFrame.src != null) {
                    for (var i = 0; i <= imgBtns.length - 1; i++) {
                        if (varTheme == 'S3GTheme_Blue2' || varTheme == "S3GTheme_Night" || varTheme == "S3GTheme_Light") {

                            if (iFrame.src.indexOf(lbtns[i].attributes["ViewPage"].nodeValue) != -1 ||
                                iFrame.src.indexOf(lbtns[i].attributes["DetailPage"].nodeValue) != -1) {
                                imgBtns[i].src = '../images/Blue_2/menu_arrow_blue.gif';
                                lbtns[i].style.fontWeight = 'bold';
                                accIndex = lbtns[i].id;
                            }
                            else {
                                imgBtns[i].src = '../images/spacer.gif'
                                lbtns[i].style.fontWeight = 'normal';
                            }
                        }
                        else {
                            if (iFrame.src.indexOf(lbtns[i].attributes["ViewPage"].nodeValue) != -1 ||
                                iFrame.src.indexOf(lbtns[i].attributes["DetailPage"].nodeValue) != -1) {
                                lbtns[i].style.fontWeight = 'bold';
                                accIndex = lbtns[i].id.replace('Accpane', '').substring(0, 1)
                            }
                            else {
                                imgBtns[i].src = '../images/bullet_button.jpg'
                                lbtns[i].style.fontWeight = 'normal';
                            }
                        }
                    }

                    //debugger;
                    accIndex = accIndex.replace('Accpane', '');
                    accIndex = accIndex.replace('Accpane', '').substring(0, accIndex.indexOf('_'));
                    //This line will be used to focus the selected Programs's menu panel
                    //$get('Accordion1').AccordionBehavior.set_SelectedIndex(accIndex);
                }
            } catch (e) {

            }
        }

    </script>

    <script type="text/jscript" language="javascript">

        function shouldCancelbackspace(e) {
            var key;
            if (e) {
                key = e.which ? e.which : e.keyCode;
                if (key == null || (key != 8 && key != 13)) { // return when the key is not backspace key. 
                    return false;
                }
            } else {
                return false;
            }

            if (e.srcElement) { // in IE 
                tag = e.srcElement.tagName.toUpperCase();
                type = e.srcElement.type;
                readOnly = e.srcElement.readOnly;
                if (type == null) { // Type is null means the mouse focus on a non-form field. disable backspace button 
                    return true;
                } else {
                    type = e.srcElement.type.toUpperCase();
                }

            } else { // in FF 
                tag = e.target.nodeName.toUpperCase();
                type = (e.target.type) ? e.target.type.toUpperCase() : "";
            }

            // we don't want to cancel the keypress (ever) if we are in an input/text area 
            if (tag == 'INPUT' || type == 'TEXT' || type == 'TEXTAREA') {
                if (readOnly == true) // if the field has been dsabled, disbale the back space button 
                    return true;
                if (((tag == 'INPUT' && type == 'RADIO') || (tag == 'INPUT' && type == 'CHECKBOX'))
    && (key == 8 || key == 13)) {
                    return true; // the mouse is on the radio button/checkbox, disbale the backspace button 
                }
                return false;
            }

            // if we are not in one of the above things, then we want to cancel (true) if backspace 
            return (key == 8 || key == 13);
        }

        // check the browser type 
        function whichBrs() {
            var agt = navigator.userAgent.toLowerCase();
            if (agt.indexOf("opera") != -1) return 'Opera';
            if (agt.indexOf("staroffice") != -1) return 'Star Office';
            if (agt.indexOf("webtv") != -1) return 'WebTV';
            if (agt.indexOf("beonex") != -1) return 'Beonex';
            if (agt.indexOf("chimera") != -1) return 'Chimera';
            if (agt.indexOf("netpositive") != -1) return 'NetPositive';
            if (agt.indexOf("phoenix") != -1) return 'Phoenix';
            if (agt.indexOf("firefox") != -1) return 'Firefox';
            if (agt.indexOf("safari") != -1) return 'Safari';
            if (agt.indexOf("skipstone") != -1) return 'SkipStone';
            if (agt.indexOf("msie") != -1) return 'Internet Explorer';
            if (agt.indexOf("netscape") != -1) return 'Netscape';
            if (agt.indexOf("mozilla/5.0") != -1) return 'Mozilla';

            if (agt.indexOf('\/') != -1) {
                if (agt.substr(0, agt.indexOf('\/')) != 'mozilla') {
                    return navigator.userAgent.substr(0, agt.indexOf('\/'));
                } else
                    return 'Netscape';
            } else if (agt.indexOf(' ') != -1)
                return navigator.userAgent.substr(0, agt.indexOf(' '));
            else
                return navigator.userAgent;
        }

        // Global events (every key press) 

        var browser = whichBrs();
        if (browser == 'Internet Explorer') {
            document.onkeydown = function () { return !shouldCancelbackspace(event); }
        } else if (browser == 'Firefox') {
            document.onkeypress = function (e) { return !shouldCancelbackspace(e); }
        }
        window.history.forward(-1);
        //window.history.forward();

        function fnSetMenuState() {
            var hdnMenuState = document.getElementById('hdnMenuState');
            if (hdnMenuState.value == '0') {
                hdnMenuState.value = '1';
            }
            else {
                hdnMenuState.value = '0';
            }
        }
    </script>

    <style type="text/css">
        .gvFixedHeader
        {
            font-weight: bold;
            position: relative;
            background-color: White;
        }
        .auto-style1 {
            width: 177px;
        }
    </style>
</head>
<body id="bodyMaster" runat="server" style="margin-right: 0px; margin-left: 1px; margin-bottom: 0px; margin-top: 2px;"
    oncontextmenu="return false;">
    <form id="form1" method="post" enctype="multipart/form-data" style="margin: 0px;"
        runat="server">
        <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
        </cc1:ToolkitScriptManager>
        <input id="hdnMenuLoad" type="hidden" runat="server" />
        <input id="hdnMenuState" value="0" type="hidden" runat="server" />
        <div id="up_container">
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <%--<tr>
                            <td >
                                &nbsp;</td>
                            <td valign="top" align="right" >
                            <asp:Image ID="Image2" runat="server" onclick="widowminimize()" ImageUrl="~/Images/arrow-down.gif" />
                            <asp:Image ID="Image1" runat="server" onclick="widowclose()" ToolTip="Close" ImageUrl="~/Images/delete.png" />
                            </td>
                        </tr>--%>
                            <tr>
                                <%--<td width="2%" class="headerBg"></td>--%>
                                <td class="auto-style1 ">
                                    <asp:Image ID="imgLogo" runat="server" ImageUrl="~/Images/Opclogo_new.png" Width="160px"  ImageAlign="Middle"/>
                                <td valign="top" align="right" class="styleHeaderBg">
                                    <table cellpadding="0" border="0" cellspacing="0" id="tblHeaderControls">
                                        <tr>
                                            <td>
                                                <%--<asp:LinkButton CausesValidation="false" runat="server" CssClass="styleHeaderLinks"
                                                    ID="lnkHome" Text="Home" OnClick="lnkHome_Click"></asp:LinkButton>--%>
                                                <a href="" runat="server" class="styleHeaderLinks" style="cursor: pointer"
                                                    id="Accpane100_content_lbtn0" onclick="fnLoadIframe(100, 0)" ViewPage="HomePage.aspx" DetailPage="HomePage.aspx">Home</a>
                                            </td>
                                            <td>
                                                <span class="styleSpanLine">| </span>
                                                <asp:LinkButton CausesValidation="false" runat="server" CssClass="styleHeaderLinks"
                                                    ID="lnkAbout" Text="About"></asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="styleSpanLine">| </span>
                                                <asp:LinkButton CausesValidation="false" runat="server" CssClass="styleHeaderLinks"
                                                    ID="lnkMore" Text="More.."></asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="styleSpanLine">|</span>
                                                <%--<asp:LinkButton CausesValidation="false" runat="server" CssClass="styleHeaderLinks"
                                                ID="lnkSettings" Text="Settings"></asp:LinkButton>--%>
                                                <%--<asp:LinkButton CausesValidation="false" runat="server" CssClass="styleHeaderLinks"
                                                    ID="lnkChangePassword" Text="Settings" OnClick="lnkChangePassword_Click"></asp:LinkButton>--%>
                                                <a href="" runat="server" class="styleHeaderLinks" style="cursor: pointer"
                                                    id="Accpane100_content_lbtn1" onclick="fnLoadIframe(100, 1)" ViewPage="Changepassword.aspx" 
                                                    DetailPage="../Common/Changepassword.aspx" PageUrl="Changepassword.aspx">Change Password</a>
                                            </td>
                                            <td>
                                                <span class="styleSpanLine">|</span>
                                                <asp:LinkButton CausesValidation="false" runat="server" CssClass="styleHeaderLinks"
                                                    ID="lnkHelp" Text="Help" OnClick="lnkHelp_Click"></asp:LinkButton>
                                            </td>
                                            <td style="padding-right: 3px">
                                                <span class="styleSpanLine">|</span>
                                                <asp:LinkButton CausesValidation="false" runat="server" CssClass="styleHeaderLinks"
                                                    ID="lnkSignOut" Text="Log Out" OnClick="lnkSignOut_Click"></asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" valign="top" align="right">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <!--CssClass="styleInfoLabel"!-->
                                                            <span class="styleHeaderLinks" runat="server" id="lblApplyTheme"></span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlTheme" runat="server" AutoPostBack="True" Width="100px" OnSelectedIndexChanged="ddlTheme_SelectedIndexChanged">
                                                                <asp:ListItem Value="S3GTheme_Blue" Enabled="false">Blistering Blue</asp:ListItem>
                                                                <asp:ListItem Value="S3GTheme_Silver"  Enabled="false">Sizzling Silver</asp:ListItem>
                                                                <asp:ListItem Value="S3GTheme_Blue2" Selected="True">Blue</asp:ListItem>
                                                                <asp:ListItem Value="S3GTheme_Orange"  Enabled="false">Orange</asp:ListItem>
                                                                <asp:ListItem Value="S3GTheme_Night">Glow</asp:ListItem>
                                                                <asp:ListItem Value="S3GTheme_Light">Light</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" class="styleTopTd" style="position: relative">
                        <table cellpadding="0" cellspacing="0" width="100%;">
                            <tr>
                                <td style="width: 60%" align="left;">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="padding-right: 20px">
                                                <span class="styleHeaderInfo" style="padding-left: 3px" runat="server" id="lblCompanyName">Company Name : </span>&nbsp;<asp:Label ID="lblCompany" runat="server" class="styleHeaderInfo"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 40%" align="right">
                                    <table align="right" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="padding-right: 20px">
                                                <span class="styleHeaderInfo" runat="server" id="lblWelcome">Welcome&nbsp; </span>
                                                <asp:Label ID="lblUser" runat="server" class="styleHeaderInfo"></asp:Label>
                                            </td>
                                            <td style="padding-right: 3px">
                                                <span class="styleHeaderInfo" runat="server" id="lblLastLogin">Last Login : </span>
                                                <asp:Label ID="lblLastLoginDate" runat="server" class="styleHeaderInfo"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="styleMarquee">
                        <marquee id="marquee1" scrollamount="2" scrolldelay="15">                    
                      <span runat="server" id="lblMarquee" onmouseover="document.getElementById('marquee1').stop();" onmouseout="document.getElementById('marquee1').start();" class="styleHeaderInfo">  Sundaram Finance offers you easy interest rate loans...</span>                      
                        </marquee>
                    </td>
                </tr>
                <tr>
                    <td class="styleContentTable" style="padding: 0px;">
                        <table cellpadding="0" cellspacing="0" width="100%" style="padding: 0px;" class="tabContainer_BG">
                            <tr>
                                <td id="tdMenu" valign="top" style="padding: 5px">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="styleContentTable">
                                                <asp:Image ID="imgHideMenu" runat="server" ImageUrl="~/Images/Night/layout_button_left.png"
                                                    CssClass="styleMenuImage" Style="clear: left; position: absolute; top: 107px; display: none; left: 215px; width: 15px;" />
                                                <asp:Image ID="imgShowMenu" runat="server" ImageUrl="~/Images/Night/layout_button_right.png"
                                                    Style="display: none; position: absolute; top: 107px; left: 6px;" CssClass="styleMenuImage" />
                                                <div id="div1" style="display: none;" class="styleToolTip">
                                                    Expand Menu
                                                </div>
                                                <div id="div2" class="styleToolTip" style="position: absolute; top: 95px; left: 207px; display: none;">
                                                    Minimize Menu
                                                </div>
                                                <asp:Panel ID="pnlMenu" runat="server" Width="215px">
                                                    <div id="divMenu" title="Menu" style="overflow-y: auto; overflow-x: hidden; height: 445px; width: 210px">
                                                        <asp:UpdatePanel ID="Uppanel1" runat="server">
                                                            <ContentTemplate>
                                                                <cc1:Accordion ID="Accordion1" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
                                                                    HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                                                                    FadeTransitions="false" FramesPerSecond="40" TransitionDuration="300" AutoSize="None"
                                                                    RequireOpenedPane="false" SuppressHeaderPostbacks="true" EnableViewState="true"
                                                                    Width="100%">
                                                                </cc1:Accordion>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </asp:Panel>

                                                <cc1:CollapsiblePanelExtender ID="cpeDemo" runat="Server" TargetControlID="pnlMenu"
                                                    ExpandedText="Hide Menu" CollapsedText="Expand Menu" ExpandControlID="divRoiRules"
                                                    CollapseControlID="divRoiRules" Collapsed="false" ImageControlID="imgDetails"
                                                    ExpandedImage="~/Images/Light/layout_button_left_blue.png" CollapsedImage="~/Images/Light/layout_button_right_blue.png"
                                                    SuppressPostBack="true" SkinID="CollapsiblePanelDemo" ExpandDirection="Horizontal" />
                                                <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" ShowStartingNode="False" />

                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" align="left" valign="top">
                                    <asp:Panel ID="divRoiRules" runat="server" Width="10px" Style="position: absolute; z-index: 99999999;">
                                        <div style="float: left; z-index: 99999999;">
                                            <asp:Image ID="imgDetails" runat="server" onclick="fnSetMenuState(); GetResize(this)" ImageUrl="~/Images/Light/layout_button_left_blue.png"
                                                Style="position: absolute; cursor: pointer; top: 32px;" CssClass="styleMenuImage" />
                                        </div>
                                        <div style="float: left; margin-left: 20px;">
                                        </div>
                                        <div style="float: right; vertical-align: middle;">
                                        </div>
                                    </asp:Panel>
                                </td>
             
                                <td valign="top" align="left" width="100%;"  style="padding-right: 5px;  padding-top: 5px; padding-bottom: 5px; padding-left: 0px;">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr id="trSiteMapPath" runat="server" style="display: none">
                                            <td width="100%">
                                                <table cellpadding="0" width="100%">
                                                    <tr>
                                                        <td width="100%">
                                                            <asp:SiteMapPath ID="SiteMapPath1" runat="server" PathSeparator=" > " EnableTheming="True"
                                                                Visible="false" CssClass="styleSitepath">
                                                                <PathSeparatorStyle CssClass="stylePathsep" />
                                                                <CurrentNodeStyle CssClass="styleCurrentNode" />
                                                                <NodeStyle CssClass="styleNode" />
                                                                <RootNodeStyle CssClass="styleRootNode" />
                                                            </asp:SiteMapPath>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100%">
                                                <img src="../Images/Blue_2/Arrow_down.gif" class="imgDownArw" onclick="fnShowTabCloseMenu();"/>
                                                <img src="../Images/delete1.png" class="imgClose" onclick="fnCloseCurrent(0);" title="Close"/>
                                                <img src="../Images/btn_silver.jpg" style="border:none; margin-bottom:-5px; width:30px; height:20px; position:absolute; right:10px;z-index:100" onclick="fnShowTabCloseMenu();" />
                                                <img src="../Images/btn_silver.jpg" style="border:none; margin-bottom:-5px; width:30px; height:20px; position:absolute; right:24px;z-index:100" onclick="fnCloseCurrent(0);" />
                                                <cc1:DropShadowExtender ID="dseTabMenu" runat="server" TargetControlID="divTabClose"
                                                Opacity=".1" Rounded="false" TrackPosition="true" />
                                                <asp:Panel ID="divTabClose" runat="server" BorderWidth="1px" BorderColor="#e1e6e9" onkeypress="escapeMenu(event);"
                                                    style="position:absolute; right:10px; margin-top:20px; display:none; z-index:1000; 
                                                    background-image: url(../images/login/login_box_bg_01.gif); background-repeat: repeat-x; min-height:70px; width:150px">
                                                    <table width="100%" cellspacing="0" cellpadding="0" id="tblTabMenu">
                                                        <tr class="tabMenuItem" onclick="fnCloseCurrent(1);">
                                                            <td class="tabMenuLeftPane">
                                                                <img src="../Images/delete1.png" style="border:none; margin-bottom:-5px; width:16px; cursor:pointer" title="Close"/>
                                                            </td>
                                                            <td>
                                                                Close
                                                            </td>
                                                        </tr>
                                                        <tr class="tabMenuItem" onclick="fnCloseAll();">
                                                            <td class="tabMenuLeftPane">
                                                                <img src="../Images/delete_all.png" style="border:none; margin-bottom:-5px; width:16px; cursor:pointer" onclick="fnCloseCurrent();" title="Close"/>
                                                            </td>
                                                            <td>
                                                                Close All Programs
                                                            </td>
                                                        </tr>
                                                        <tr class="tabMenuItem" onclick="fnCloseAll('0');">
                                                            <td class="tabMenuLeftPane">
                                                            </td>
                                                            <td>
                                                                Close All But This
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <div id="Container" style="overflow: hidden;" class="tabContainer_BG">

                                                    <%--<asp:ContentPlaceHolder ID="ContentPlaceHolder1" runat="server" >
                                                    </asp:ContentPlaceHolder>--%>
                                                    <%--  <iframe runat="server" id="ifrmMain" width="100%" frameBorder="0" marginheight="0" marginwidth="0" scrolling="auto" 
                                                            style="width:100%;border:0;" visible="true" src="../Common/HomePage.aspx"></iframe><em style="vertical-align:top; top:.5px;" class="ui-tabs-close" onclick="parent.RemoveTab();" title="Close">x</em>--%>
                                                    <div id="Div3" style="overflow: hidden;">
                                                        <%--<asp:ContentPlaceHolder ID="ContentPlaceHolder1" runat="server" >
                                                    </asp:ContentPlaceHolder>--%>
                                                        <div id="Div4" style="margin: 0px; width: 100%;overflow: hidden; vertical-align: top;" >
                                                            <ul id="ulContainer" class="tabContainer_BG">
                                                                <li><a href="#Default">
                                                                    <asp:Label ID="lblHeading" Font-Size="11px" Font-Names="Segoe UI" runat="server"
                                                                        ForeColor="#003d9e" Text="Home" ToolTip="Home"></asp:Label></a></li>
                                                            </ul>
                                                            <div id="Default" style="overflow: hidden; padding-left: 10px;">
                                                                <a href="#" id="hrefTab" name="Metric" value="alter" style="font-family: arial, Helvetica, sans-serif; color: Navy; font-size: 8pt; display: none;"
                                                                    onclick="CreateTab(this);">Dummy</a>
                                                                <iframe runat="server" id="ifrmMain" width="100%" frameborder="0" marginheight="0"
                                                                    marginwidth="0" scrolling="no" style="width: 100%; border: 0; min-height: 420px" visible="true"
                                                                    src="../Common/HomePage.aspx"></iframe>
                                                            </div>
                                                        </div>
                                                        <asp:PlaceHolder runat="server" ID="plcHolder"></asp:PlaceHolder>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="styleTopTd">
                        <span class="styleFooterInfo" style="padding-left: 3px"><a href="#" runat="server"
                            id="lnkSiteMap">Site Mapite Map</a> </span><span class="styleFooterSpanLine">|</span>
                        <span class="styleFooterInfo"><a href="#" runat="server" id="lnkAboutUs">About Us</a>
                        </span><span class="styleFooterSpanLine">| </span><span class="styleFooterInfo"><a
                            href="#" runat="server" runat="server" id="lnkTerms">Terms of Service</a> </span>
                        <span class="styleFooterSpanLine">| </span><span class="styleFooterInfo"><a href="#"
                            runat="server" id="lnkContact">Contact</a></span> <span class="styleFooterSpanLine">| </span><span class="styleFooterInfo"><a href="#" runat="server" id="lnkDisclaimer">Disclaimer</a></span> <span class="styleFooterSpanLine">| </span><span class="styleFooterInfo">
                                <a href="#" runat="server" id="lnkPrivacy">Privacy Status</a> </span>
                        <span class="styleFooterSpanLine">| </span><span class="styleFooterInfo"><a href="#"
                            runat="server" id="lnkFooterMore">More..</a> </span><span class="styleFooterSpanLine"></span><span style="text-align: right; padding-left: 20%" class="styleFooterInfo"
                                runat="server" id="lblCopyRights">Release Version 1.0 | Sundaram Infotech Solutions
                            © 2010 Copyrights </span>
                        <input type="hidden" id="hdnSelectedIndex" enableviewstate="true" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hdnImgBtn" runat="server" />
        <asp:HiddenField ID="hdnTabCount" runat="server" />
        <%--<div id="divLoading" style="display: none;">
            <input type="button" value="Cancel" onclick="abort()"; />
    </div>--%>
    </form>
</body>
</html>
