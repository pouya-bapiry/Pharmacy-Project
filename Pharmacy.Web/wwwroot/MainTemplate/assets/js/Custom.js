
function ShowMessage(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'nfc-bottom-right',
        showDuration: 4000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'اعلان',
        message: decodeURI(text)
       
    });
}


$('input[name="OrderBy"]').on('change', function () {
    var selectedValue = $(this).val();
    var currentUrl = new URL(window.location.href);
    currentUrl.searchParams.set('OrderBy', selectedValue);
    window.location.href = currentUrl.toString();
});


function FillPageId(pageId) {
    $('#PageId').val(pageId);
    $('#filter-form').submit();
}


 