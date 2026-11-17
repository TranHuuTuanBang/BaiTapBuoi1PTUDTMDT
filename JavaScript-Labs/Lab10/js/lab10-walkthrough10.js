$(function () {
    $(".animLoading").hide();
    $("#consume, #fail-request").on("click", function () {
        const buttons = $("#consume, #fail-request").prop("disabled", true);
        const url = this.id === "consume" ? "data/cities.json" : "data/missing.json";
        $(".animLoading").stop(true, true).show();
        $("#results").text("Đang tải…");
        $.get(url, null, null, "json")
            .done(function (data) {
                const list = $("<ul>");
                $.each(data, (_, city) => $("<li>").text(city.name).appendTo(list));
                $("#results").empty().append(list);
            })
            .fail(function (xhr, status) {
                $("#results").text("Không tải được dữ liệu (" + (xhr.status || status) + ").");
            })
            .always(function () {
                $(".animLoading").fadeOut(250);
                buttons.prop("disabled", false);
            });
    });
});
