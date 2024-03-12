

function expandDetails(_leadId) {
    if ($("#" + _leadId).is(':visible')) {
        $("#" + _leadId).hide();
    }
    else {
        $("#" + _leadId).show();
    }
}