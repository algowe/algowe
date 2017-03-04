// Firefox bug (need to implement) - on submit true, false -> true
$(window).load(function () {
    $("input:checkbox").each(function () {
        fixHiddenCheckField(this);
    });

    $("input:checkbox").change(function () {
        fixHiddenCheckField(this);
    });
});

function fixHiddenCheckField(checkField) {
    var name = $(checkField).attr("name");
    var hiddenSelector = "input:hidden[name=" + name + "]";
    var checkBoxesInListSelector = "input:checkbox[name=" + name + "]";
    var checkedCheckBoxesInListSelector = "input:checked[name=" + name + "]";

    if ($(checkedCheckBoxesInListSelector).length >= 1 || $(checkBoxesInListSelector).length > 1) {
        $("input").remove(hiddenSelector);
    }
    else {
        if ($(hiddenSelector).length == 0)
            $(checkField).parents("form").append("<input type='hidden' name='" + name + "' value='false' />");
    }
}