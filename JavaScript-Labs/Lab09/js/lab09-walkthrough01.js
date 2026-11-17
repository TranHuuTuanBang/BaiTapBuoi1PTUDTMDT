const message = document.getElementById("msgArea");
document.getElementById("pageTitle").textContent = "Even newer title";
const spans = Array.from(document.getElementsByTagName("span"), node => node.innerText);
const counts = Array.from(document.getElementsByClassName("count-number"), node => node.innerText);
const first = document.querySelector("#first p");
const paragraphs = Array.from(document.querySelectorAll("#first p"), node => node.innerHTML);
message.value = ["My class is " + message.getAttribute("class"), ...spans, ...counts,
    "First paragraph: " + first.innerText, ...paragraphs].join("\n");
message.setAttribute("rows", "8");
document.getElementById("features").style.backgroundColor = "green";
