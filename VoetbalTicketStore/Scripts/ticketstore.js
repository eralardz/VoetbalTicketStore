function OnSuccess(result) {
    $('#partial').empty();
    $('#partial').html(result);
}
function OnFailure(ajaxContext) {
    alert('Failure');
    $('#partial').html(ajaxContext);
}

// Tooltips
$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})
