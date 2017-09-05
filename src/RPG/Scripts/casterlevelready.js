function formatCasterRepo(repo) {
    if (repo.loading) return repo.text;
    var markup =
        '<div class="clearfix">' +
            '<div class="col-sm-12">' +
                '<div class="row"><h4><b>' + repo.text + '</b></h4></div>' +
                '<div class="row"><b>Components: </b>' + repo.Components + '</div>' +
                '<div class="row"><b>Casting Time: </b>' + repo.CastingTime + '</div>' +
                '<div class="row"><b>Range: </b>' + repo.Range + '</div>' +
                '<div class="row"><b>Target: </b>' + repo.Target + '</div>' +
                '<div class="row"><b>Duration: </b>' + repo.Components + '</div>' +
                '<div class="row"><b>Saving Throw: </b>' + repo.Save + '</div>' +
                '<div class="row"><b>Spell Resistance: </b>' + repo.SpellResistance + '</div>' +
                '<div class="row">' + repo.Description + '</div>' +
            '</div>' +
        '</div>';
        
    return markup;
}

function formatCasterRepoSelection(repo) {
    return repo.text || repo.id;
}

$(document).ready(function () {
    var searchUrl = GetBaseUrl() + "CharacterSpell" + '/' + "SearchSpells";
    $(".addSpellToSlotSelector").select2({
        ajax: {
            url: searchUrl,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term, // search term
                    level: $('#addSpellToSlotSlotsLevel').prop('value'),
                    casterlevel: $('#addSpellToSlotCasterLevel').prop('value'),
                    classid: $('#addSpellToSlotClassId').prop('value'),
                    charid: $('#addSpellToSlotCharId').prop('value')
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
        templateResult: formatCasterRepo,
        templateSelection: formatCasterRepoSelection,
        

    });
});