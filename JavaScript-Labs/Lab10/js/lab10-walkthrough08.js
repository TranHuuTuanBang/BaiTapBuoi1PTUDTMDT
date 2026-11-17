$(function () {
    $("button").on("click", function () {
        const box = $("#box");
        box.stop(true).css({left: 0, top: 0});
        box.animate({left: Math.min(495, Math.max(0, window.innerWidth - 140)), opacity: 1, height: 50})
            .animate({top: 400, opacity: 0.2})
            .animate({height: 200}, 2000);
    });
});
