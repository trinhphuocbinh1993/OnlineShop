﻿
var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        $("#btnContinue").off('click').on('click', function () {
            window.location.href = "/"
        })

        $("#btnPayment").off('click').on('click', function () {
            window.location.href = "/thanh-toan"
        })

        $("#btnUpdate").off('click').on('click', function () {
            var listProduct = $(".txtQuantity");
            var cartList = [];
            $.each(listProduct, function (i, item) {
                cartList.push({
                    Quantity: $(item).val(),
                    Product: {
                        ID: $(item).data("id")
                    }
                })
            })
            console.log(cartList);
            $.ajax({
                url: "/Cart/Update",
                data: { cartModel: JSON.stringify(cartList) },
                dataType: "json",
                type: "POST",
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        })

        $("#btnDeleteAll").off('click').on('click', function () {
            $.ajax({
                url: "/Cart/DeleteAll",
                dataType: "json",
                type: "POST",
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        })


        $(".btn-delete").off('click').on('click', function (e) {
            e.preventDefault();
            var id = $(this).data("id");
            $.ajax({
                url: "/Cart/Delete",
                dataType: "json",
                data: { id: id },
                type: "POST",
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        })

    }
}
cart.init();