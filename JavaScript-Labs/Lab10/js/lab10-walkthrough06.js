$(function () {
    const figure = $("figure");
    $("#btnFadeIn").on("click", () => figure.stop(true, true).fadeIn(1000));
    $("#btnFadeOut").on("click", () => figure.stop(true, true).fadeOut(1000));
    $("#btnFade").on("click", () => figure.stop(true, true).fadeToggle(500));
    $("#btnSlideDown").on("click", () => figure.stop(true, true).slideDown("slow"));
    $("#btnSlideUp").on("click", () => figure.stop(true, true).slideUp("slow"));
    $("#btnSlide").on("click", () => figure.stop(true, true).slideToggle(2000));
});
