const stringOne = new String("Test");
const stringTwo = "Test";
const stringThree = "Test";
const stringFour = new String("Test");
document.write(`<p>typeof stringOne = ${typeof stringOne}<br>typeof stringThree = ${typeof stringThree}</p>
<p>Object == literal: ${stringOne == stringTwo}<br>Object == object: ${stringOne == stringFour}
<br>Object === literal: ${stringOne === stringTwo}<br>Literal === literal: ${stringTwo === stringThree}</p>
<p>${new Date()}</p><p>Math.PI = ${Math.PI}<br>Math.sqrt(4) = ${Math.sqrt(4)}<br>Math.random() = ${Math.random()}</p>`);
