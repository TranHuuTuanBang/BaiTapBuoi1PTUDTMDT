const first = document.getElementById("first");
console.log(first.firstChild, first.firstChild.nextSibling, first.parentNode);
for (const child of first.childNodes) {
    console.log(child, child.nodeType);
    if (child.nodeType === Node.ELEMENT_NODE) child.style.backgroundColor = "#ffe393";
}
