function addPosition() {
    $('#myForm table input[disabled]').removeAttr("disabled");

    var url = $('#myForm').attr('action');
    var data = $('#myForm').serializeArray();

    data.push({
        name: "AddPosition",
        value: true
    });

    var request = $.ajax({
        url: url,
        method: "POST",
        data: data
    });

    request.done(function (data) {
        var newDoc = document.open("text/html", "replace");
        newDoc.write(data);
        newDoc.close();
    });
}

function removePosition(idx) {
    var url = $('#myForm').attr('action');
    var data = $('#myForm').serializeArray();
    data.push({
        name: "RemovePosition",
        value: idx
    });

    var request = $.ajax({
        url: url,
        method: "POST",
        data: data
    });

    request.done(function (data) {
        var newDoc = document.open("text/html", "replace");
        newDoc.write(data);
        newDoc.close();
    });
}

function toggleFileInput() {
    var checked = $('#document-toggle').is(':checked');
    if (!checked)
        $('#document-form-group').hide();
    else
        $('#document-form-group').show();
}