function onchangeCharacterData(age, race, abis) {
    var raceVal = null;
    var ageVal = null;
    var abisMap = new Object();
    var abisVal = new Object();
    if (age !== null) {
        ageVal = $("#" + age).val();
    }
    if (race !== null) {
        raceVal = $("#" + race).val();
    }
    if (abis !== null) {
        for (var i = 0; i < abis.length; i++) {
            var currentId = $("#" + abis[i].Key).val();
            abisMap[currentId] = abis[i].Key;
        }
        for (var i = 0; i < abis.length; i++) {
            var currentId = $("#" + abis[i].Key).val();
            var currentVal = $("#" + abis[i].Value).val();
            abisVal[currentId] = currentVal;
        }
    }

    var data = {
        age: ageVal,
        raceId: raceVal,
        abilityKeyVal: abisVal,
        abilityMap: abisMap
    };
    CallAjaxService("CharacterCreator", "UpdateCharacterData", data, onchangeonchangeCharacterDataSuccess);
}
function onchangeonchangeCharacterDataSuccess(result) {
    $("#resultingAgeCategoryId").val(result.currentCategory);
    $("#ageCategoryForRaceId").val(result.ageCategories);
    for (var i = 0; i < result.abilityModifiersWithBonusStat.length; i++) {
        $("#" + result.abilityModifiersWithBonusStat[i].Key + "_Final_Stat").val(result.abilityModifiersWithBonusStat[i].Value);
    }
    for (var j = 0; j < result.abilityModifiersWithBonus.length; j++) {
        $("#" + result.abilityModifiersWithBonus[j].Key + "_Final_Bonus").val(result.abilityModifiersWithBonus[j].Value);
        //Setting skillmodifiers
        $('input[name^="' + result.abilityModifiersWithBonus[j].Key + "_skillMod" + '"]').each(function () {
            var modValue = result.abilityModifiersWithBonus[j].Value;
            $(this).val(modValue);

            var currentId = this.id;
            var skillRanks = $("#" + currentId.slice(0, -6));
            var totalSkillRanks = $("#" + currentId + "_total");

            var currentRanks = skillRanks.val();
            totalSkillRanks.val((parseInt(currentRanks) + modValue));
        });
        
    }
    for (var k = 0; k < result.abilityRaceModifiers.length; k++) {
        $("#" + result.abilityRaceModifiers[k].Key + "_Race_Bonus").val(result.abilityRaceModifiers[k].Value);
    }
    for (var l = 0; l < result.abilityAgeModifiers.length; l++) {
        $("#" + result.abilityAgeModifiers[l].Key + "_Age_Bonus").val(result.abilityAgeModifiers[l].Value);
    }
}
