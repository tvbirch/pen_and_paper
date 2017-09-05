$(document).ready(function () {
    $(".pop").popover({ trigger: "manual", html: true, animation: false })
    .on("mouseenter", function () {
        var _this = this;
        $(this).popover("show");
        $(".popover").on("mouseleave", function () {
            $(_this).popover('hide');
        });
    }).on("mouseleave", function () {
        var _this = this;
        setTimeout(function () {
            if (!$(".popover:hover").length) {
                $(_this).popover("hide");
            }
        }, 300);
    });

    $(".attackTargetModalMoveClass").click(function (e) {
        var mousePosWidth = e.pageX;
        var mousePosHeight = e.pageY;
        var width = $("#attackTargetModal").width();
        var height = $("#attackTargetModal").height();
        //var height = 80;

        var newPosWidth = mousePosWidth - width / 2;
        var newPosHeight = mousePosHeight - height / 2;
        
        $("#attackTargetModal").css({ top: newPosHeight, left: newPosWidth, position: 'fixed' });
    });

    $(".js-select2-basic-multiple").select2({ width: "100%" });
    $(".js-select2-basic-multiple-conditionlist").select2({ width: "100%" })
        .on("select2:select", function (e) {
            AddCondtion(e.params.data.id, GetCharId());
        })
        .on("select2:unselect", function (e) {
            RemoveCondtion(e.params.data.id, GetCharId());
        });
});


function ReloadWindow(val,charId) {
    window.location.href = "/CharacterEquipmentModifier?id=" + charId + "&itemId=" + val.value;
}

function GetCharId() {
    var hv = $('#charId').val();
    return hv;
}