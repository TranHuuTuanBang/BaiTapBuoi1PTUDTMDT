$(function () {
    $("#consume").on("click", function () {
        $("#results").text("Đang tải…");
        $.get("data/cities.json", function (data, status) {
            if (status === "success") {
                const list = $("<ul>");
                $.each(data, (_, city) => $("<li>").text(city.name).appendTo(list));
                $("#results").empty().append(list);
            }
        }, "json").fail(() => $("#results").text("Không tải được dữ liệu. Hãy chạy trang qua HTTP."));
    });
});
