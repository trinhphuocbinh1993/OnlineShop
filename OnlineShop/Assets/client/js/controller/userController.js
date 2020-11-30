var user = {
    init: function () {
        user.loadProvince();
        user.registerEvent();
    },
    registerEvent: function () {
        $("#ddlProvince").off('change').on('change', function () {
            var id = $(this).val();
            if (id != "") {
                user.loadDistrict(parseInt(id));
            } else {
                $("#ddlDistrict").html('')
            }
        })
    },
    loadProvince: function () {
        $.ajax({
            url: "/User/LoadProvince",
            type: "Post",
            dataType: "json",
            success: function (res) {
                var html = "<option value=''>-- Chọn Tỉnh/Thành --</option>";
                if (res.status == true) {
                    var data = res.data
                    $.each(data, function (i, item) {
                        html += "<option value='" + item.ID + "'>" + item.Name + "</option>"
                    })

                    $("#ddlProvince").html(html)
                }
            }
        })
    },
    loadDistrict: function (id) {
        
        $.ajax({
            url: "/User/LoadDistrict",
            data: {
                id: id
            },
            type: "Post",
            dataType: "json",
            success: function (res) {
                if (res.status == true) {
                    var html = "<option value=''>-- Chọn Quận/Huyện --</option>";
                    var data = res.data
                    $.each(data, function (i, item) {
                        html += "<option value='" + item.ID + "'>" + item.Name + "</option>"
                    })
                    console.log(html);
                    $("#ddlDistrict").html(html)
                }
            }
        })
    }
}
user.init();