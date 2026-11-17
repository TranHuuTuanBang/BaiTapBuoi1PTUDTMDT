window.addEventListener("load", function () {
    for (const div of document.querySelectorAll(".movingDiv")) {
        div.addEventListener("mouseover", function (event) {
            // Đặt breakpoint ở dòng sau rồi xem event.target trong DevTools.
            const message = "triggered by " + event.target.id;
            document.getElementById("event-result").textContent = message;
            console.log(message, event.target);
        });
    }
});
