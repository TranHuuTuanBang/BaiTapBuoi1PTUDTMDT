window.addEventListener("load", function () {
    const fields = document.querySelectorAll('input[type="text"], input[type="password"]');
    for (const field of fields) {
        field.addEventListener("focus", () => { field.style.backgroundColor = "#ffe393"; });
        field.addEventListener("blur", () => { field.style.backgroundColor = "white"; });
    }
    // Biểu mẫu dùng để thực hành trên trang tĩnh.
    document.forms.sampleForm.addEventListener("submit", event => event.preventDefault());
});
