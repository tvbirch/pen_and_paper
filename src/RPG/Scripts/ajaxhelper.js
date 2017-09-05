function AddCondtion(conditionId, charId) {
    var data = {
        id: charId,
        conditionId: conditionId,
    };
    AjaxServicePostReload('CharacterAction', 'AddCondtion', data);
}
function RemoveCondtion(conditionId, charId) {
    var data = {
        id: charId,
        conditionId: conditionId,
    };
    AjaxServicePostReload('CharacterAction', 'RemoveCondtion', data);
}

function attackWithWeapon(weapon, charId, target, nrOfEnemies) {
    var data = {
        id: charId,
        weaponId: weapon,
        target: target,
        nrOfEnemies: nrOfEnemies
    };
    AjaxServicePostReload('CharacterAction', 'AttackWithWeapon', data);
}
function HealDamage(amount, charId) {
    var data = {
        id: charId,
        amount: amount,
    };
    AjaxServicePostReload('CharacterAction', 'HealDamage', data);
}
function TakeDamage(amount, normalDmg, type, charId) {
    var data = {
        id: charId,
        normalDamage: normalDmg,
        damageType: type,
        amount: amount,
    };
    AjaxServicePostReload('CharacterAction', 'TakeDamage', data);
}

function ChangeWealth(money, addToWealth, charId) {
    var data = {
        id: charId,
        addToWealth: addToWealth,
        money: money,
    };
    AjaxServicePostReload('CharacterEquipment', 'ChangeWealth', data);
}
function TriggerAbilityOneCharge(charId, abilityId) {
    var data = {
        charId: charId,
        abilityId: abilityId,
        charges: 1
    };
    AjaxServicePostReload('CharacterAction', 'TriggerAbility', data);
}
function AddItemToBag(itemId, payForItem, charId) {
    var data = {
        id: itemId,
        payForItem: payForItem,
        charId: charId
    };
    AjaxServicePostReload('CharacterEquipment', 'Purchase', data);
}
function ChangeQuantity(itemId, newQuantity, charId) {
    var data = {
        id: itemId,
        charId: charId,
        newQuantity:newQuantity
    };
    AjaxServicePostReload('CharacterEquipment', 'ChangeQuantity', data);
}
function DequipItem(charId, ownedItemId) {
    var data = {
        id: ownedItemId,
        charId: charId,
        dequip: true
    };
    AjaxServicePostReload('CharacterEquipment', 'EquipItem', data);
}
function EquipItem(ownedItemId, charId) {
    var data = {
        id: ownedItemId,
        charId: charId,
        dequip: false
    };
    AjaxServicePostReload('CharacterEquipment', 'EquipItem', data);
}

function GetItemsFromBonusApplyToType(type, populateFieldWithResult, selectValue) {
    var url = GetBaseUrl() + 'Bonus/GetItemsFromBonusApplyToType';
    $.ajax({
        type: 'GET',
        url: url,
        data: {type: type},
        success: function (result) {
            var options = $("#" + populateFieldWithResult);
            var $SubItems = [];
            $SubItems.push($("<option/>", { value: "00000000-0000-0000-0000-000000000000", text: "--Non--" }));

            $.each(result.data, function (index, item) {
                $SubItems.push($("<option/>", { value: item.ID, text: item.Name }));
            });

            options.empty().append($SubItems).trigger("chosen:updated");
            if (selectValue !== undefined && selectValue !== null && selectValue !== '') {
                options.val(selectValue);
                options.trigger("chosen:updated");
            }
        },
        error: function (result) {
            alert('error: ' + url);
        }
    });
}
function GetSubItemsFromBonusApplyToType(type, populateFieldWithResult, selectValue) {
    var url = GetBaseUrl() + 'Bonus/GetSubItemsFromBonusApplyToType';
    $.ajax({
        type: 'GET',
        url: url,
        data: { type: type },
        success: function (result) {
            var options = $("#" + populateFieldWithResult);
            var row = $("#SelectedItem_ApplyTo_ApplyToSubtypeGuid_Row");
            if (result.data.length > 0) {
                row.show();
            } else {
                row.hide();
            }

            var $SubItems = [];
            $SubItems.push($("<option/>", { value: "00000000-0000-0000-0000-000000000000", text: "--Non--" }));

            $.each(result.data, function (index, item) {
                $SubItems.push($("<option/>", { value: item.ID, text: item.Name }));
            });

            options.empty().append($SubItems).trigger("chosen:updated");
            if (selectValue !== undefined && selectValue !== null && selectValue !== '') {
                options.val(selectValue);
                options.trigger("chosen:updated");
            }
        },
        error: function (result) {
            alert('error:' + url);
        }
    });
}

function AddSpellToSlot(spellId, slotLevel, classId, charId) {
    var data = {
        id: spellId,
        slotLevel: slotLevel,
        classId: classId,
        charId: charId
    };
    AjaxServicePostReload('CharacterSpell', 'FillSlot', data);
}
function SpendSpellSlot(slotId, slotLevel, classId, charId) {
    var data = {
        id: slotId,
        slotLevel: slotLevel,
        classId: classId,
        charId: charId
    };
    AjaxServicePostReload('CharacterSpell', 'SpendSlot', data);
}
function CastSpellSlot(slotId, slotLevel, classId, charId) {
    alert("TODO");
}

function RemoveSpellFromSlot(slotId, slotLevel, classId, charId) {
    var data = {
        id: slotId,
        slotLevel: slotLevel,
        classId: classId,
        charId: charId
    };
    AjaxServicePostReload('CharacterSpell', 'RemoveSpellFromSlot', data);
}

function SaveHistory(historyId,history) {
    var data = {
        id: historyId,
        history: history
    };
    AjaxServicePostReload('History', 'SaveHistory', data);
}

function CallAjaxService(controller,method,data, onSuccess) {
    var url = GetBaseUrl() + controller +'/'+method;
    $.ajax({
        type: 'GET',
        url: url,
        data: data,
        success: onSuccess,
        error: function (result) {
            alert('error:' + url);
        }
    });
}
function AjaxServicePostReload(controller, method, data) {
    var url = GetBaseUrl() + controller + '/' + method;
    $.ajax({
        type: 'POST',
        url: url,
        data: data,
        success: function (result) {
            if (result === undefined || result === null || result === "" || result === "OK") {
                location.reload();
            }
            else {
                alert(result);
            }

        },
        error: function (result) {
            alert('error:' + url);
        }
    });
}

function GetBaseUrl() {
    var getUrl = window.location;
    var baseUrl = getUrl.protocol + "//" + getUrl.host + "/";
    return baseUrl;
}