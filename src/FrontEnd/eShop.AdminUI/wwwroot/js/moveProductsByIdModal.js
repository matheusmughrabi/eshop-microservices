var moveProductByIdModal = document.getElementById('move-product-by-id-modal');

moveProductByIdModal.addEventListener('show.bs.modal', function (event) {
    let productId = event.relatedTarget.getAttribute('productId');
    $('#move-product-by-id-confirm-button').attr('productId', productId);
})

$("#move-product-by-id-confirm-button").click(function () {

    var destineCategoryId = $('#destine-category-dropdown-modal-by-id').val();
    var productId = $(this).attr("productId");

    $.ajax({
        url: '/Catalog/Category/Details?handler=MoveProductById',
        type: 'POST',
        contentType: 'application/json',
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        data: JSON.stringify({ DestineCategoryId: destineCategoryId, ProductId: productId }),
        success: function (data, textStatus, jQxhr) {
            alert(data.notifications[0].message);
            $('#move-product-by-id-modal').modal('hide');
            if (data.notifications[0].type == "1") {
                location.reload(true);
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            alert("error when trying to delete product");
        }
    });
});