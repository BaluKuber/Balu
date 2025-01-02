var tab_counter = 1;
window.onerror = function() { return true; }
$(function() {
    // variant 2: newly added tabs closable
    var $tabs2 = $('#Container ul').tabs({
        add: function(e, ui, nav) {
            // append close thing
            $(ui.tab).parents('li:first')

                        .append('<em class="ui-tabs-close" title="Close">x</em>')
                        .find('em')
                        .click(function() {
                            $tabs2.tabs('remove', $('li', $tabs2).index($(this).parents('li:first')[0]));
                        });
            $tabs2.tabs('select', '#' + ui.panel.id);
        },
        remove: function(event, ui) {
            // document.frames['ctl00_Body_MainFrame'].RefreshGrid();
        }
    });

    $('.Tab').bind('click', function() {
        $tabs2.tabs('add', '#' + tab_counter, $(this).attr("name"), $(this).attr("value"));
        tab_counter += 1;
    });
});
function CreateTab(elem) {
    var $tabs2 = $('#Container ul').tabs({
        add: function(e, ui) {
            // append close thingy
            $(ui.tab).parents('li:first')
                            .append('<em class="ui-tabs-close" title="Close Tab">x</em>')
                            .find('em')
                            .click(function() {
                                $tabs2.tabs('remove', $('li', $tabs2).index($(this).parents('li:first')[0]));
                            });
            // select just added tab
            $tabs2.tabs('select', '#' + ui.panel.id);
        }
    });
    $tabs2.tabs('add', '#' + "IFrame" + tab_counter, elem.name, elem.value);
    tab_counter += 1;
}
function RemoveTab() {
    var $tabs2 = $('#Container ul').tabs({
        add: function(e, ui) {
            // append close thingy
            $(ui.tab).parents('li:first')
                            .append('<em class="ui-tabs-close" title="Close Tab">x</em>')
                            .find('em')
                            .click(function() {
                                $tabs2.tabs('remove', $('li', $tabs2).index($(this).parents('li:first')[0]));
                            });
            // select just added tab
            $tabs2.tabs('select', '#' + ui.panel.id);
        }
    });
    var selectedIndex = $tabs2.tabs("option", "selected");
    $tabs2.tabs('abort');
    $tabs2.tabs('remove', selectedIndex);
}
//            function LoadData() {
//                document.frames['ctl00_Body_MainFrame'].RefreshGrid();
//            }
function CreateTabForChild(name, value) {
    var $tabs2 = $('#Container ul').tabs({
        add: function(e, ui) {
            // append close thingy
            $(ui.tab).parents('li:first')
                            .append('<em class="ui-tabs-close" title="Close Tab">x</em>')
                             .find('em')
                            .click(function() {
                                $tabs2.tabs('remove', $('li', $tabs2).index($(this).parents('li:first')[0]));
                            });
            // select just added tab
            $tabs2.tabs('select', '#' + ui.panel.id);
        }
    });
    $tabs2.tabs('add', '#' + "IFrame" + tab_counter, name, value);
    tab_counter += 1;
}
function RemoveTabBasedOnCount(sTCount) {
    var $tabs2 = $('#Container ul').tabs({
        add: function(e, ui) {
            // append close thingy
            $(ui.tab).parents('li:first')
                            .append('<em class="ui-tabs-close" title="Close Tab">x</em>')
                            .find('em')
                            .click(function() {
                                $tabs2.tabs('remove', $('li', $tabs2).index($(this).parents('li:first')[0]));
                            });
            // select just added tab
            $tabs2.tabs('select', '#' + ui.panel.id);
        }
    });
    // var selectedIndex = $tabs2.tabs("option", "selected");
    if (sTCount != "") {
       
        var Tab_Count = $tabs2[0].childNodes.length;
        if (Tab_Count > sTCount) {
            $tabs2.tabs('abort');
            $tabs2.tabs('remove', 1);
            Tab_Count = Tab_Count - 1;
        } 
    }


}


//            function pageLoad() {
//                RemoveLoading();
//            }

//function InitializeRequest(sender, args) {

//    if (prm.get_isInAsyncPostBack()) {
//    
//    } 
//}