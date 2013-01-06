﻿require.config({
    paths: {
        jquery: 'libs/jquery/jquery-1.8.3.min' // '//ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min'
        , knockout: 'libs/knockout/knockout-2.2.0'
        , async: 'libs/require/async'
        , jqui: 'libs/jquery/jquery-ui-1.8.20'
        , jqballoon: 'libs/jquery/jquery.balloon'
        , json: 'libs/json2/json2'
        , tinymce: 'libs/tiny_mce/tiny_mce'
        , text: 'libs/require/text.min'
        , debug: 'libs/debug/ba-debug.min'
        , twitter: 'libs/twitter/bootstrap',
        map: 'http://maps.googleapis.com/maps/api/js?key=AIzaSyBXgUOTPfgbS4kHE7fm_xr2za_O1ApA_TM&sensor=false'
        //map : 'http://maps.googleapis.com/maps/api/js?sensor=false'
    },
    shim: {
        twitter: {
            deps: ["jquery"]
        }
    }
    //urlArgs: "bust=" + (new Date()).getTime()
});

// common setup for all pages
require(["jquery", "twitter"], function ($) {

    // setup search popup
    $('#search-button').popover({
        placement: "left",
        html: true,
        content: $("#popover-search-form")[0].outerHTML,
        template: '<div class="popover"><div class="arrow"></div><div class="popover-inner"><h3 class="popover-title" style="display:none"></h3><div class="popover-content"><p></p></div></div></div>'
    });
});

