var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
        $(".btn-active").off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/User/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (res) {
                    console.log(res);
                    if (res.status == true) {
                        return btn.text('Kích hoạt');
                    } else {
                        return btn.text('Khóa');
                    }
                }

            })
        })
    }
}
user.init();