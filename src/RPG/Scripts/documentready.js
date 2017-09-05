$(document).ready(function () {
    var config = {
        '.chosen-select': {},
        '.chosen-select-max1': { max_selected_options: 1 },
        '.chosen-select-deselect': { allow_single_deselect: true },
        '.chosen-select-no-single': { disable_search_threshold: 10 },
        '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
        '.chosen-select-width': { width: "95%" }
    }
    for (var selector in config) {
        $(selector).chosen(config[selector]);
        $(selector).trigger('chosen:updated');
    }
    //$(".chosen-select").chosen({ allow_single_deselect: true });
    $('.chosen-container').width("100%");
});
