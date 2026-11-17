const button = document.getElementById("testButton");
const image = document.getElementById("mainImage");
const content = document.getElementById("content");
let timer;
button.addEventListener("click", function () {
    clearTimeout(timer);
    const hide = button.textContent.trim() === "Hide";
    button.textContent = hide ? "Show" : "Hide";
    if (hide) {
        content.className = "makeItDisappear";
        timer = setTimeout(() => { content.style.display = "none"; }, 1000);
    } else {
        content.style.display = "block";
        timer = setTimeout(() => { content.className = "makeItNormal"; }, 50);
    }
});
image.addEventListener("mouseover", () => { image.className = "makeItGray"; });
image.addEventListener("mouseout", () => { image.className = "makeItNormal"; });
