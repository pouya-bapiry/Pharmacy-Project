
function open_waiting(selector = 'body') {
    $(selector).waitMe({
        effect: 'img',
        text: 'لطفا منتظر بمانید ...',
        bg: 'rgba(255,255,255,0.7)',
        color: '#000',
        source: '/MainTemplate/assets/image/logo.png',
        fontSize: '22px',
    });
}

function close_waiting(selector = 'body') {
    $(selector).waitMe('hide');
}

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



$('#number_of_products_in_basket').on('change',
    function (e) {
        var numberOfProduct = parseInt(e.target.value, 0);
        $('#add_product_to_order_Count').val(numberOfProduct);
    });


function onSuccessAddProductToOrder(result) {
    if (result.status === 'Success') {
        ShowMessage('اعلان موفقیت', result.message);

        setTimeout(function () {
            close_waiting();
        }, 3000);
    }

    else {
        ShowMessage('اعلان هشدار', result.message, 'warning');
    }

    //location.reload();
}



$('#submitOrderForm').on('click', function () {
    $('#addProductToOrderForm').submit();
    open_waiting();
});

function removeProductFromOrder(detailId) {
    $.get('/user/remove-order-item/' + detailId).then(result => {
        location.reload();
    });
}

function checkDetailCount() {
    $('input[order-detail-count]').on('change', function (event) {
        open_waiting();
        var detailId = $(this).attr('order-detail-count');
        $.get('/user/change-detail-count/' + detailId + '/' + event.target.value).then(result => {
            $("#user-open-order-wrapper").html(result);
            setTimeout(function () {
                close_waiting();
                checkDetailCount();
                reloadPage();
            }, 500);

        });

    });

}



function reloadPage() {
    location.reload();
}

checkDetailCount();

function wait_me() {

    open_waiting();

    setTimeout(function () {
        close_waiting();
    }, 3000);

}


 