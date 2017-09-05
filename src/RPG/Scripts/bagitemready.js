function formatRepo(repo) {
    if (repo.loading) return repo.text;
    var markup =
        '<div class="clearfix">' +
            '<div class="col-sm-12">' +
                '<div class="row"><h4><b>' + repo.text + '</b></h4></div>' +
                '<div class="row">' + repo.Description + '</div>' +
                '<div class="row"><b>Weight</b>: ' + repo.Weight + '</div>' +
                '<div class="row"><b>Price</b>: ' + repo.Price + '</div>' +
            '</div>' +
        '</div>';
        
    return markup;
}

function formatRepoSelection(repo) {
    return repo.text || repo.id;
}

$(document).ready(function () {
    var searchUrl = GetBaseUrl() + "CharacterEquipment" + '/' + "SearchItems";
    $(".addItemToBagSelector").select2({
        ajax: {
            url: searchUrl,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term, // search term
                    page: params.page
                };
            },
            processResults: function (data, page) {
                return {
                    results: data.result
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        minimumInputLength: 1,
        templateResult: formatRepo, 
        templateSelection: formatRepoSelection 

    });
});