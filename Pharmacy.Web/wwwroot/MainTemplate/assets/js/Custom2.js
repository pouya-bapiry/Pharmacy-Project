const cookieWishlist = "wishlist-items";
let productColorPrice = 0;

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
        showDuration: 5000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'اعلان',
        message: decodeURI(text)
    });
}

$(document).ready(function () {
    var editors = $("[ckeditor]");
    if (editors.length > 0) {
        $.getScript('/Theme/assets/js/ckeditor.js', function () {
            $(editors).each(function (index, value) {
                var id = $(value).attr('ckeditor');
                ClassicEditor.create(document.querySelector('[ckeditor="' + id + '"]'),
                    {
                        toolbar: {
                            items: [
                                'heading',
                                '|',
                                'bold',
                                'italic',
                                'link',
                                '|',
                                'fontSize',
                                'fontColor',
                                '|',
                                'imageUpload',
                                'blockQuote',
                                'insertTable',
                                'undo',
                                'redo',
                                'codeBlock'
                            ]
                        },
                        language: 'fa',
                        table: {
                            contentToolbar: [
                                'tableColumn',
                                'tableRow',
                                'mergeTableCells'
                            ]
                        },
                        licenseKey: '',
                        simpleUpload: {
                            // The URL that the images are uploaded to.
                            uploadUrl: '/Uploader/UploadImage'
                        }

                    })
                    .then(editor => {
                        window.editor = editor;
                    }).catch(err => {
                        console.error(err);
                    });
            });
        });
    }


});

function FillPageId(pageId) {
    $('#PageId').val(pageId);
    $('#filter-form').submit();
}

$('[main_category_checkbox]').on('change', function (e) {
    var isChecked = $(this).is(':checked');
    var selectedCategoryId = $(this).attr('main_category_checkbox');
    console.log(selectedCategoryId);
    if (isChecked) {
        $('#sub_categories_' + selectedCategoryId).slideDown(300);
    } else {
        $('#sub_categories_' + selectedCategoryId).slideUp(300);
        $('[parent-category-id="' + selectedCategoryId + '"]').prop('checked', false);
    }
});

// Add an event listener to the product checkboxes
const productCheckboxes = document.querySelectorAll('input[name="SelectedProducts"]');
productCheckboxes.forEach(checkbox => {
    checkbox.addEventListener('change', function () {
        const productId = this.value;
        const productSizesContainer = document.querySelector(`#productSizes_${productId}`);
        if (this.checked) {
            productSizesContainer.style.display = 'block'; // Show the product sizes container
        } else {
            productSizesContainer.style.display = 'none'; // Hide the product sizes container
        }
    });
});





$('#add_color_button').on('click', function (e) {

    e.preventDefault();
    var colorName = $('#product_color_name_input').val();
    var colorPrice = $('#product_color_price_input').val();
    var colorCode = $('#product_color_code_input').val();

    if (colorName !== '' && colorPrice !== '' && colorCode !== '') {
        var currentColorsCount = $('#list_of_product_colors tr');
        var index = currentColorsCount.length;


        var isExistsSelectedColor = $('[color-name-hidden-input][value="' + colorName + '"]');

        if (isExistsSelectedColor.lenght !== 0) {

            var colorNameNode = `<input type="hidden" value="${colorName}" name="ProductColors[${index}].ColorName" color-name-hidden-input="${colorName}-${colorPrice}">`;
            var colorPriceNode = `<input type="hidden" value="${colorPrice}" name="ProductColors[${index}].Price" color-price-hidden-input="${colorName}-${colorPrice}">`;
            var colorCodeNode = `<input type="hidden" value="${colorCode}" name="ProductColors[${index}].ColorCode" color-code-hidden-input="${colorName}-${colorPrice}">`;

            $('#create_product_form').append(colorNameNode);
            $('#create_product_form').append(colorPriceNode);
            $('#create_product_form').append(colorCodeNode);


            var colorTableNode = `
          <tr color-table-item="${colorName}-${colorPrice}">
          <td>${colorName}</td> 
          <td>${colorPrice}</td>  
          <td>
          <div style="border-radius: 50%; width:40px; height: 40px; border:2px solid black; background-color: ${colorCode}"></div>
          </td>
          <td> <a class="btn btn-lg text-danger" style="float: none;" title="حذف رنگ" onclick="removeProductColor('${colorName}-${colorPrice}')">
          <i class="bi bi-x-octagon-fill"></i>
          </a>
          </td>
          </tr>`;
            $('#list_of_product_colors').append(colorTableNode);
            $('#product_color_name_input').val('');
            $('#product_color_price_input').val('');
            $('#product_color_code_input').val('');


        } else {
            ShowMessage('اخطار', 'رنگ وارد شده تکراری می باشد', 'warning');
            $('#product_color_name_input').val('');
            $('#product_color_price_input').val('');
            $('#product_color_code_input').val('');

            $('#product_color_name_input').val('').focus();
        }

    } else {
        ShowMessage('اخطار', 'لطفا نام رنگ و قیمت آن را به درستی وارد نمایید', 'warning');
    }

});


$('#add_feature_button').on('click', function (e) {

    e.preventDefault();
    var featureTitle = $('#product_feature_title_input').val();
    var featureValue = $('#product_feature_value_input').val();

    if (featureTitle !== '' && featureValue !== '') {

        var currentFeaturesCount = $('#list_of_product_features tr');
        var index = currentFeaturesCount.length;


        var isExistsSelectedFeature = $('[feature-title-hidden-input][value="' + featureTitle + '"]');

        if (isExistsSelectedFeature.lenght !== 0) {

            var featureTitleNode = `<input type="hidden" value="${featureTitle}" name="ProductFeatures[${index}].featureTitle" feature-title-hidden-input="${featureTitle}-${featureValue}">`;
            var featureValueNode = `<input type="hidden" value="${featureValue}" name="ProductFeatures[${index}].FeatureValue" feature-value-hidden-input="${featureTitle}-${featureValue}">`;

            $('#create_product_form').append(featureTitleNode);
            $('#create_product_form').append(featureValueNode);


            var featureTableNode = `
          <tr feature-table-item="${featureTitle}-${featureValue}">
          <td>${featureTitle}</td>
          <td>${featureValue}</td>
          <td> <a class="btn btn-lg text-danger" style="float: none;" title="حذف ویژگی" onclick="removeProductFeature('${featureTitle}-${featureValue}')">
          <i class="bi bi-x-octagon-fill"></i>
          </a>
          </td>
          </tr>`;
            $('#list_of_product_features').append(featureTableNode);
            $('#product_feature_title_input').val('');
            $('#product_feature_value_input').val('');

        } else {
            ShowMessage('اخطار', 'ویژگی وارد شده تکراری می باشد', 'warning');
            $('#product_feature_title_input').val('');
            $('#product_feature_value_input').val('');

            $('#product_feature_title_input').val('').focus();
        }

    } else {
        ShowMessage('اخطار', 'لطفا نام ویژگی و مقدار آن را به درستی وارد نمایید', 'warning');
    }

});


$('#add_size_button').on('click', function (e) {

    e.preventDefault();
    var sizeTitle = $('#product_size_title_input').val();
    var sizePrice = $('#product_size_price_input').val();
    if (sizeTitle !== '' && sizePrice !== '') {

        var currentSizeCount = $('#list_of_product_sizes tr');
        var index = currentSizeCount.length;


        var isExistsSelectedSize = $('[size-title-hidden-input][value="' + sizeTitle + '"]');

        if (isExistsSelectedSize.lenght !== 0) {

            var sizeTitleNode = `<input type="hidden" value="${sizeTitle}" name="ProductSizes[${index}].sizeTitle" size-title-hidden-input="${sizeTitle}-${sizePrice}">`;
            var sizePriceNode = `<input type="hidden" value="${sizePrice}" name="ProductSizes[${index}].Price" size-price-hidden-input="${sizeTitle}-${sizePrice}">`;

            $('#create_product_form').append(sizeTitleNode);
            $('#create_product_form').append(sizePriceNode);


            var sizeTableNode = `
          <tr size-table-item="${sizeTitle}-${sizePrice}">
          <td>${sizeTitle}</td>
          <td>${sizePrice}</td>
          <td> <a class="btn btn-lg text-danger" style="float: none;" title="حذف سایز" onclick="removeProductSize('${sizeTitle}-${sizePrice}')">
          <i class="bi bi-x-octagon-fill"></i>
          </a>
          </td>
          </tr>`;
            $('#list_of_product_sizes').append(sizeTableNode);
            $('#product_size_title_input').val('');
            $('#product_size_price_input').val('');

        } else {
            ShowMessage('اخطار', 'سایز وارد شده تکراری می باشد', 'warning');
            $('#product_size_title_input').val('');
            $('#product_size_price_input').val('');

            $('#product_size_title_input').val('').focus();
        }

    } else {
        ShowMessage('اخطار', 'لطفا عنوان سایز  و قیمت آن را به درستی وارد نمایید', 'warning');
    }

});

function removeProductColor(index) {
    $('[color-name-hidden-input="' + index + '"]').remove();
    $('[color-price-hidden-input="' + index + '"]').remove();
    $('[color-code-hidden-input="' + index + '"]').remove();

    $('[color-table-item="' + index + '"]').remove();
    reOrderProductColorHiddenInputs();
}

function removeProductFeature(index) {
    $('[feature-title-hidden-input="' + index + '"]').remove();
    $('[feature-value-hidden-input="' + index + '"]').remove();

    $('[feature-table-item="' + index + '"]').remove();
    reOrderProductFeatureHiddenInputs();
}

function removeProductSize(index) {
    $('[size-title-hidden-input="' + index + '"]').remove();
    $('[size-price-hidden-input="' + index + '"]').remove();

    $('[size-table-item="' + index + '"]').remove();
    reOrderProductSizeHiddenInputs();
}

function reOrderProductColorHiddenInputs() {
    var hiddenColors = $('[color-name-hidden-input]');
    $.each(hiddenColors, function (index, value) {

        var hiddenColor = $(value);
        var colorId = $(value).attr('color-name-hidden-input');
        var hiddenPrice = $('[color-price-hidden-input="' + colorId + '"]');
        var hiddenCode = $('[color-code-hidden-input="' + colorId + '"]');

        $(hiddenColor).attr('name', 'ProductColors[' + index + '].ColorName');
        $(hiddenPrice).attr('name', 'ProductColors[' + index + '].Price');
        $(hiddenCode).attr('name', 'ProductColors[' + index + '].ColorCode');
    });
}

function reOrderProductFeatureHiddenInputs() {
    var hiddenFeatures = $('[feature-title-hidden-input]');
    $.each(hiddenFeatures, function (index, value) {

        var hiddenFeature = $(value);
        var featureId = $(value).attr('feature-title-hidden-input');
        var hiddenFeatureValue = $('[feature-value-hidden-input="' + featureId + '"]');

        $(hiddenFeature).attr('name', 'ProductFeatures[' + index + '].FeatureTitle');
        $(hiddenFeatureValue).attr('name', 'ProductFeatures[' + index + '].FeatureValue');
    });
}

function reOrderProductSizeHiddenInputs() {
    var hiddenSize = $('[size-title-hidden-input]');
    $.each(hiddenSize, function (index, value) {

        var hiddenSize = $(value);
        var sizeId = $(value).attr('size-title-hidden-input');
        var hiddenSizePrice = $('[size-price-hidden-input="' + sizeId + '"]');

        $(hiddenSize).attr('name', 'ProductSizes[' + index + '].SizeTitle');
        $(hiddenSizePrice).attr('name', 'ProductSizes[' + index + '].SizePrice');
    });
}


$('#OrderBy').on('change', function () {
    $('#filter-form').submit();
});


window.$("input[type=radio]").change(function () {
    window.$("#pageId").val(1);
    window.$("#filter-form").submit();
});





function changeProductPriceBasedOnColor(colorId, priceOfColor, colorName) {

    var basePrice = parseInt($('#ProductBasePrice').val());
    var newPrice = (basePrice + priceOfColor).toLocaleString();

    $('.se-cart-price-new').html((newPrice) + ' ' + 'تومان');
    $('#add_product_to_order_ProductColorId').val(colorId);

    productColorPrice = priceOfColor;

}



function changeProductPriceBasedOnSize(selectedSizePrice, selectedSizeId) {
    var basePrice = parseInt($('#ProductBasePrice').val());
    var discountPrice = parseInt($('#DiscountPrice').val());
    var sizePrice = selectedSizePrice.toLocaleString('fa') + 'تومان';

    if (isNaN(discountPrice)) {
        discountPrice = 0; // Set discountPrice to 0 if it's NaN
    }

    var priceOfSizeWithDiscount = parseInt((selectedSizePrice * discountPrice) / 100);
    var priceOfSize = parseInt(selectedSizePrice - priceOfSizeWithDiscount);

    var newPrice = (priceOfSize + productColorPrice).toLocaleString();

    $('.se-cart-price-new').html((newPrice) + ' ' + 'تومان');
    $('#OldPrice').html(selectedSizePrice.toLocaleString() + ' تومان');

    $('#add_product_to_order_ProductSizeId').val(selectedSizeId);
}





$('#number_of_products_in_basket').on('change',
    function (e) {
        var numberOfProduct = parseInt(e.target.value, 0);
        $('#add_product_to_order_Count').val(numberOfProduct);
    });


// function onSuccessAddProductToOrder(result, e) {
//     e.preventDefault();
//     if (result.status === 'Success') {
//         ShowMessage('اعلان موفقیت', result.message);

//         setTimeout(function () {
//             close_waiting();
//         }, 3000);
//     }

//     else {
//         ShowMessage('اعلان هشدار', result.message, 'warning');
//     }

//     //location.reload();
// }



// $('#submitOrderForm').on('click', function (e) {
//     e.preventDefault();
//     $('#addProductToOrderForm').submit();
//     open_waiting();
// });

$('#submitOrderForm').on('click', function (e) {
    e.preventDefault();

    open_waiting();

    var form = $('#addProductToOrderForm');

    $.ajax({
        url: form.attr('action'),
        type: 'POST',
        data: form.serialize(),

        success: function (result) {

            if (result.status === 'Success') {

                ShowMessage(
                    'اعلان موفقیت',
                    result.message
                );

                setTimeout(function () {
                    close_waiting();
                }, 3000);

            } else {

                close_waiting();

                ShowMessage(
                    'اعلان هشدار',
                    result.message,
                    'warning'
                );
            }
        },

        error: function (xhr) {

            close_waiting();

            console.log(xhr);

            ShowMessage(
                'اعلان هشدار',
                'خطایی در ثبت محصول رخ داد',
                'warning'
            );
        }
    });
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

// Auto Complete

var productNameOption = {
    url: function (phrases) {
        return `/seller/products-autocomplete?productName=${phrases}`;
    },
    getValue: function (element) {
        return element.title;
    },
    list: {
        match: {
            enabled: true
        },
        onSelectItemEvent: function () {
            var value = $("#ProductName").getSelectedItemData().id;

            $("#ProductId").val(value).trigger("change");
        }
    },
    theme: "plate-dark",


};


var productBrandNameOption = {
    url: function (phrase) {
        return `/seller/brands-autocomplete?brandName=${phrase}`;
    },
    getValue: function (element) {
        return element.brandName;
    },
    list: {
        match: {
            enabled: true
        },
        onSelectItemEvent: function () {
            var value = $("#ProductBrandName").getSelectedItemData().id;

            $("#BrandId").val(value).trigger("change");
        }
    },
    theme: "plate-dark",


};




var $item = $('.carousel-item');
var $wHeight = $(window).height();
$item.eq(0).addClass('active');
$item.height($wHeight);
$item.addClass('full-screen');

$('.carousel img').each(function () {
    var $src = $(this).attr('src');
    var $color = $(this).attr('data-color');
    $(this).parent().css({
        'background-image': 'url(' + $src + ')',
        'background-color': $color
    });
    $(this).remove();
});

//$(window).on('resize', function () {
//    $wHeight = $(window).height();
//    $item.height($wHeight);
//});

$('.carousel').carousel({
    interval: 6000,
    pause: "false"
});





function readURL(input) {

    if (input.files && input.files[0]) {

        var reader = new FileReader();

        reader.onload = function (e) {
            $('.image-upload-wrap').hide();

            $('.file-upload-image').attr('src', e.target.result);
            $('.file-upload-content').show();

            $('.image-title').html(input.files[0].name);
        };

        reader.readAsDataURL(input.files[0]);

    } else {
        removeUpload();
    }
}

function removeUpload() {
    $('.file-upload-input').replaceWith($('.file-upload-input').clone());
    $('.file-upload-content').hide();
    $('.image-upload-wrap').show();
}
$('.image-upload-wrap').bind('dragover', function () {
    $('.image-upload-wrap').addClass('image-dropping');
});
$('.image-upload-wrap').bind('dragleave', function () {
    $('.image-upload-wrap').removeClass('image-dropping');
});


////////////////////////////Privacy and Policy Check////////////////////////////////////////

$(document).ready(function () {
    var recommend = document.getElementById('isRecommend');
    recommend.style.visibility = 'hidden';
    document.querySelector('#btn-register').disabled = true;
    $('[register_checkbox]').on('change', function (e) {

        var isChecked = $(this).is(':checked');

        if (isChecked) {
            document.querySelector('#btn-register').disabled = false;
        } else {
            document.querySelector('#btn-register').disabled = true;
        }
    });
});


///////////////////////////////CountDownt Timer////////////////////////////////////////////////////

var timeLeft = 90;
var elem = document.getElementById('some_div');
var sendActiveCode = document.getElementById('send_active_code');


var notification = document.createElement('p');
notification.className = 'text-dark';
notification.textContent = 'اگر کد فعالسازی حساب کاربری خود را دریافت نکرده اید، لطفا از طریق دکمه زیر به پشتیبانی سایت پیام دهید یا تماس حاصل فرمایید تا حساب کاربری تان را فعال نمایند.';
notification.style.paddingBottom = '20px';

var newTag = document.createElement('a');
newTag.id = 'resend_btn';
newTag.className = 'btnSubmit btn btn-round btn-block text-danger';
newTag.textContent = 'تماس با پشتیبانی'
newTag.href = 'https://jibicenter.com/Contact-Us';

elem.style.fontSize = '20px';
elem.style.color = 'black';

var timerId = setInterval(countdown, 1000);

function countdown() {
    if (timeLeft == -1) {
        clearTimeout(timerId);
        sendActiveCode.remove();
        elem.innerHTML = '';
        elem.append(notification);
        elem.append(newTag);


    } else {
        elem.innerHTML = timeLeft + '  ثانیه ';
        timeLeft--;

    }
}



/////////////////////////////////Add Product to WishList/////////////////////////////////////////


function addToWishlist(id, productTitle, price, storeName, image) {
    /*    debugger;*/
    let wishlists = $.cookie(cookieWishlist);
    if (wishlists === undefined) {
        wishlists = [];
    } else {
        wishlists = JSON.parse(wishlists);
    }

    var count = 1;
    const currentWishlist = wishlists.find(x => x.id === id);

    if (currentWishlist !== undefined) {
        wishlists.find(x => x.id === id).count = parseInt(currentWishlist.count) + (count);
    } else {
        const wishlist = {
            id,
            productTitle,
            price,
            storeName,
            image,
            count
        }
        wishlists.push(wishlist);
    }

    swal({
        title: "پیغام موفقیت",
        text: "محصول مورد نظر به لیست علاقه مندی ها اضافه شد",
        icon: "success",
        button: "تایید",
    });

    $.cookie(cookieWishlist, JSON.stringify(wishlists), { expires: 60, path: "/" });
    updateWishlist();



}


function updateWishlist() {

    let wishlists = $.cookie(cookieWishlist);
    wishlists = JSON.parse(wishlists);
    const count = $("#wishlist_count").text(wishlists.length);

    const currentWishlist = wishlists.find(x => x.id === id);
    if (currentWishlist !== undefined) {
        wishlists.find(x => x.id === id).count = parseInt(currentWishlist.count) + parseInt(count);
    } else {
        const wishlist = {
            id,
            productTitle,
            price,
            storeName,
            image,
            count
        }

        wishlists.push(wishlist);
        $.cookie(cookieWishlist, JSON.stringify(wishlists), { expires: 60, path: "/" });
    }


}


function removeFromWishlist(id) {
    let wishlists = $.cookie(cookieWishlist);
    wishlists = JSON.parse(wishlists);
    const itemToRemove = wishlists.findIndex(x => x.id === id);
    wishlists.splice(itemToRemove, 1);

    swal({
        title: "پیغام موفقیت",
        text: "محصول مورد نظر از لیست علاقه مندی ها حذف گردید",
        icon: "error",
        button: "تایید",
    });


    $.cookie(cookieWishlist, JSON.stringify(wishlists), { expires: 60, path: "/" });


    setTimeout(function () {
        location.reload();
    }, 1000);

    updateWishlist();

}

// Image Upload File
function showPreview(event, previewId) {
    if (event.target.files.length > 0) {
        var src = URL.createObjectURL(event.target.files[0]);
        var preview = document.getElementById("file-ip-" + previewId + "-preview");
        preview.src = src;
        preview.style.display = "block";
    }
}


const actualBtn = document.getElementById('actual-btn');

const fileChosen = document.getElementById('file-chosen');

actualBtn.addEventListener('change', function () {
    fileChosen.textContent = this.files[0].name
})


// Suggest Product in Product Comments

function SuggestProduct() {
    // Get button element
    var btnAgree = document.getElementById('btnAgree');
    var lblSuggest = document.getElementById('lblSuggest');


    // Set the spinner 
    btnAgree.innerHTML = '<div class="spinner-border text-light" role="status"><span class="sr-only" ></span ></div> ';

    // Do the work. A sample method to wait for 3 second.
    setTimeout(function () {

        swal({
            title: "محصول فوق را پیشنهاد می دهم",
            text: "با تشکر از شما، نظر شما به کاربران سایت کمک می کند تا خرید بهتری داشته باشند",
            icon: "success",
            button: "تایید",
        });

        // When the work is done, reset the button to original state
        lblSuggest.innerHTML = 'با تشکر از شما بابت پیشنهاد محصول. خیلی ممنونیم !';
        lblSuggest.style.backgroundColor = 'green';
        lblSuggest.style.color = 'white';
        lblSuggest.style.fontSize = '20px';
        lblSuggest.style.padding = '5px';
        btnAgree.style.visibility = 'hidden';
        btnDisagree.style.visibility = 'hidden';


        $("#isRecommended").attr('checked', 'true');
        $("#isRecommended").attr('value', 'true');


    }, 2000);

}

function UnSuggestProduct() {
    // Get button element
    var btnDisagree = document.getElementById('btnDisagree');
    var lblSuggest = document.getElementById('lblSuggest');

    // Set the spinner 
    btnDisagree.innerHTML = '<div class="spinner-border text-light" role="status"><span class="sr-only" ></span ></div> ';

    // Do the work. A sample method to wait for 3 second.
    setTimeout(function () {

        swal({
            title: "محصول فوق را پیشنهاد نمی دهم",
            text: "با تشکر از شما، نظر شما به کاربران سایت کمک می کند تا خرید بهتری داشته باشند",
            icon: "error",
            button: "تایید",
        });

        // When the work is done, reset the button to original state
        lblSuggest.innerHTML = 'بابت شرکت در نظرسنجی پیشنهاد محصول سپاسگزاریم !';
        lblSuggest.style.backgroundColor = 'red';
        lblSuggest.style.color = 'white';
        lblSuggest.style.fontSize = '20px';
        lblSuggest.style.padding = '5px';
        btnAgree.style.visibility = 'hidden';
        btnDisagree.style.visibility = 'hidden';
        btnAgree.style.visibility = 'hidden';

        $("#isRecommended").attr('checked', 'false');
        $("#isRecommended").attr('value', 'false');



    }, 2000);

}


// Product Comment Validation Form

function CommentValidation() {
    // Get the form fields
    const emailField = document.getElementById("floatingInputEmail");
    const commentField = document.getElementById("floatingTextarea2");
    const nameField = document.getElementById("floatingInputName");

    // Get the values entered by the user
    const email = emailField.value.trim();
    const comment = commentField.value.trim();
    const name = nameField.value.trim();

    // Check if the email is valid (you can add more complex validation)
    const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    if (email !== "") {
        if (!email.match(emailPattern)) {
            emailValidation.innerHTML = 'فرمت ایمیل صحیح نمی باشد';
            emailField.focus();
            return false;
        }
    }

    // Check if the comment is not empty
    if (comment.length === 0) {
        commentValidation.innerHTML = 'لطفا متن نظر خود را بنویسید';
        commentField.focus();
        return false;
    }

    // Check if the fullname is not empty
    if (name.length === 0) {
        fullnameValidation.innerHTML = 'لطفا نام و نام خانوادگی خود را وارد نمایید';
        nameField.focus();
        return false;
    }

    return true;
}


// Popup in Product Slider

document.querySelectorAll('.popup-trigger').forEach(trigger => {
    trigger.addEventListener('mouseover', () => {
        const popupContent = trigger.querySelector('.text-effect').textContent;
        const popup = trigger.querySelector('.popup-content');
        popup.textContent = popupContent;
    });
});


$(document).ready(function () {
    $('#file-ip-1').change(function () {
        var fileName = $(this).val().split('\\').pop();
        $('#customFileInputLabel').text(fileName || 'آپلود تصویر پروفایل');
    });
});


// Function to set a cookie
function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}

// Function to get a cookie by name
function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function handleRadioClick(radio) {
    // Reset background color for all labels
    $('.buttonFilter label').css('background-color', '');

    // Set background color for the clicked label
    $(radio).next('label').css('background-color', 'aqua');

    // Store the selected value in a cookie
    setCookie('selectedValue', $(radio).val(), 7); // Cookie expires in 7 days
}

// Check if there is a previously selected value in the cookie
var selectedValue = getCookie('selectedValue');
if (selectedValue) {
    // Find the corresponding radio button and set the background color
    $('#' + selectedValue).next('label').css('background-color', 'aqua');
}

// Check Some Fields in Employment

function CheckChild(e) {
    if (e === "1") {
        $('#ChildNumber').show(); // Show the additional input field
    } else {
        $('#ChildNumber').hide(); // Hide the additional input field
    }
}

function CheckInsurancePeriod(e) {
    if (e === "0") {
        $('#InsuranceTimePayment').show(); // Show the additional input field
    } else {
        $('#InsuranceTimePayment').hide(); // Hide the additional input field
    }
}

function CheckMilitaryType(e) {
    if (e === "1") {
        $('#ExemptType').show(); // Show the additional input field
    } else {
        $('#ExemptType').hide(); // Hide the additional input field
    }
}

///////////////////////Adding Employment Education//////////////////////////////////