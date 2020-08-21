function modifyFlight(index, oldDateTime, oldRouteId, email) { 
    var newDate = document.getElementById("scheduleId" + index).value;
    var newTime = document.getElementById("timeId" + index).value;
    var newRouteId = document.getElementById("routeId" + index).value;

    var newUrl = "/Flights/ModifyFlight";

    newUrl += "?oldDateTime=" + oldDateTime;
    newUrl += "&oldRouteId=" + oldRouteId;
    newUrl += "&newDate=" + newDate;
    newUrl += "&newTime=" + newTime;
    newUrl += "&newRouteId=" + newRouteId;
    newUrl += "&email=" + email;

    window.location.href = newUrl;
}

function deleteFlight(date, route, email) {
    var result = confirm("Are you sure you want to delete this flight?");
    if (result) {
        window.location.href = '/Flights/RemoveFlight?takeOff=' + date + "&route=" + route + "&email=" + email;
    }
}

function openForm(closeFormID, openFormID, count) {
    for (var i = 0; i < count; ++i) {
        document.getElementById("closeForm" + i.toString()).style.display = "none";
        document.getElementById("openForm" + i.toString()).style.display = "block";
    }
    document.getElementById(closeFormID).style.display = "block";//open form
    document.getElementById(openFormID).style.display = "none";//close "schedule" button
}

function closeForm(closeFormID, openFormID) {
    document.getElementById(openFormID).style.display = "block";//display "schedule" button
    document.getElementById(closeFormID).style.display = "none";//close form
}
