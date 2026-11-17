$(function () {
    $("figure img").on("mouseenter", function (event) {
        const thumbnail = $(this).addClass("gray");
        const preview = $("<div>", {id: "preview"}).css("pointer-events", "none");
        const caption = thumbnail.attr("alt");
        $("<img>", {src: thumbnail.attr("src").replace("small", "medium"), alt: caption}).appendTo(preview);
        $("<p>").text(caption).appendTo(preview);
        preview.hide().appendTo("body").fadeIn(350);
        movePreview(event);
    }).on("mousemove", movePreview).on("mouseleave", function () {
        $(this).removeClass("gray");
        $("#preview").remove();
    });
    function movePreview(event) {
        $("#preview").css({top: event.pageY - 10, left: event.pageX + 30});
    }
});
