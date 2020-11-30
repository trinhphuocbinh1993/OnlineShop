var contact = {
    init: function () {
        contact.registerEvent();
    },
    registerEvent: function () {
        $("#btnSend").off("click").on("click", function () {
            var name = $("#txtName").val();
            var phone = $("#txtPhone").val();
            var address = $("#txtAddress").val();
            var email = $("#txtEmail").val();
            var content = $("#txtContent").val();

            $.ajax({
                url: "/Contact/Send",
                type: "POST",
                dataType: 'json',
                data: {
                    name: name,
                    phone: phone,
                    address: address,
                    email: email,
                    content: content,
                },
                success: function (res) {
                    if (res.status == true) {
                        alert("Gửi thành công");
                        contact.resetForm();
                    }
                }
            });
        })
    },
    resetForm: function () {
        $("#txtName").val("");
        $("#txtPhone").val("");
        $("#txtAddress").val("");
        $("#txtEmail").val("");
        $("#txtContent").val("");
    }
}

contact.init();