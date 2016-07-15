$(document).ready(function () {
    var loc = $.cookie('_locale');

    if (typeof loc === 'undefined') {
        console.log("Locale: Cookie not set");
        loc = document.documentElement.lang;
    } else
        console.log("Locale: Cookie found");

    console.log(" --> " + loc);

    // set default culture for datepickers
    $.datepicker.setDefaults($.datepicker.regional[loc]);

    $('input[type=datetime]').datepicker();
});