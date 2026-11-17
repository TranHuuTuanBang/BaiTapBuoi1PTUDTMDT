window.addEventListener("load", function () {
    const form = document.getElementById("sampleForm");
    const errors = document.getElementById("errors");
    form.addEventListener("submit", function (event) {
        event.preventDefault();
        const fields = Array.from(form.querySelectorAll('input[type="text"], input[type="password"]'));
        const empty = fields.filter(field => field.value.trim() === "");
        errors.className = "visible";
        errors.textContent = empty.length
            ? "The following fields can't be empty: " + empty.map(field => field.id).join(", ")
            : "Validation passed. Demo complete.";
        if (empty.length) empty[0].focus();
    });
});
