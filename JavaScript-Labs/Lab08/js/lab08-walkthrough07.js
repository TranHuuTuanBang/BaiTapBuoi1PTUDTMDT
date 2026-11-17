const days = ["Mon", "Tues", "Wed", "Thur", "Fri"];
days.push("Sat");
days.unshift("Sun");
document.write("<table border='1'><tr>");
for (const day of days) {
    document.write("<th>" + (day.length > 3 ? "<em>" + day + "</em>" : day) + "</th>");
}
document.write("</tr>");
for (let week = 0; week < 5; week++) {
    document.write("<tr>");
    for (let weekday = 0; weekday < 7; weekday++) {
        const date = week * 7 + weekday + 1;
        document.write("<td>" + (date <= 30 ? date : "") + "</td>");
    }
    document.write("</tr>");
}
document.write("</table>");
