﻿require.config({
    paths: {
        jquery: 'libs/jquery/jquery-1.7.2.min' // '//ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min'
        , knockout: 'libs/knockout/knockout-2.1.0'
        , async: 'libs/require/async'
        , jqui: 'libs/jquery/jquery-ui-1.8.20'
        , jqballoon: 'libs/jquery/jquery.balloon'
        , jqjson: 'libs/jquery/jquery.json-2.3'
        , tinymce: 'libs/tiny_mce/tiny_mce'
        , order: 'libs/require/order.min'
        , text: 'libs/require/text.min'
        , debug: 'libs/debug/ba-debug.min'
        , twitter: 'libs/twitter/bootstrap'
    },
    //urlArgs: "bust=" + (new Date()).getTime()
});

// common setup for all pages
require(["order!jquery", "order!twitter"], function ($) {

    // setup search popup
    $('#search-button').popover({
        placement: "left",
        html: true,
        content: "<form action='Search' class='form-search' style='margin:0'><input type='text' name='query' placeholder='Search Criteria' class='input-medium search-query'><button type='submit' class='btn btn-primary'>Search</button></form>",
        template: '<div class="popover"><div class="arrow"></div><div class="popover-inner"><h3 class="popover-title" style="display:none"></h3><div class="popover-content"><p></p></div></div></div>'
    });

});

