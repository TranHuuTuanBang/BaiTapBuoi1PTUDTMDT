window.addEventListener("load", function () {
    const div = document.getElementById("div1");
    document.getElementById("event-result").textContent = "load → " + div.textContent;
});
