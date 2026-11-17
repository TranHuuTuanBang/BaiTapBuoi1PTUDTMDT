$(function () {
    const message = $("#msgArea");
    message.val("My class is " + message.attr("class"));
    $("#first").html("new Title");
    $("button").css("background-color", "red");
    $("body").css("background-color", "ivory");
    $(".center-icons").addClass("selected");
});
