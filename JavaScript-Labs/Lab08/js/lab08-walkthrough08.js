const boxClass = "movingDiv";
function outputBox(number) {
    const box = `<div class="${boxClass}" id="div${number}">This is div ${number}</div>`;
    return box;
}
