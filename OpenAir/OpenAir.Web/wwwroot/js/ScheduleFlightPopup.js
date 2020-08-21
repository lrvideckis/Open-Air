function updateDay(dropDownValue, email) {
    var RouteValue = document.getElementById('RouteList').value;

    var emailInput = document.getElementById('pilotList');
    if (emailInput != null) {
        email = emailInput.value;
    }

    window.location.href = '/Schedule/UpdateScheduleViewFlights?currentDate=' + dropDownValue + "&Route=" + RouteValue + "&email=" + email;
}

function updateRoute(dropDownValue, email) {
    var DateValue = document.getElementById('ScheduleID').value;
    var emailInput = document.getElementById('pilotList');
    if (emailInput != null) {
        email = emailInput.value;
    }
    window.location.href = '/Schedule/UpdateScheduleViewFlights?currentDate=' + DateValue + "&Route=" + dropDownValue + "&email=" + email;
}

function openForm(closeFormID, openFormID) {
    for (var k = 7; k < 20; ++k) {
        var a = "closeForm" + k;
        var b = "openForm" + k;

        document.getElementById(b).style.display = "block";//display "schedule" button
        document.getElementById(a).style.display = "none";//close form
    }

    document.getElementById(closeFormID).style.display = "block";//open form
    document.getElementById(openFormID).style.display = "none";//close "schedule" button
}

function closeForm(closeFormID, openFormID, minutesName) {
    document.getElementById(openFormID).style.display = "block";//display "schedule" button
    document.getElementById(closeFormID).style.display = "none";//close form
    document.getElementById(minutesName).value = null;
}

function addAFlight(hour, email) {
    var minutes = parseInt(document.getElementById("minutes" + hour).value);
    if (isNaN(minutes) || minutes < 0 || minutes >= 60) {
        alert("minutes must be between 0 and 59");
        document.getElementById("minutes" + hour).value = '';
        return 0;
    }

    var flightSpeed = parseFloat(document.getElementById("flightSpeed" + hour).value);
    if (isNaN(flightSpeed) || flightSpeed < 0) {
        alert("flight speed must be a positive number");
        document.getElementById("flightSpeed" + hour).value = '';
        return 0;
    }

    var emailInput = document.getElementById('pilotList');
    if (emailInput != null) {
        email = emailInput.value;
    }

    var newUrl = "/Schedule/ScheduleSubmit";

    newUrl += "?currentDate=" + document.getElementById("ScheduleID").value;
    newUrl += "&hours=" + parseInt(hour);
    newUrl += "&minutes=" + minutes;
    newUrl += "&Route=" + document.getElementById("RouteList").value;
    newUrl += "&Callsign=" + "testCallsign";
    newUrl += "&flightSpeed=" + flightSpeed;
    newUrl += "&pilotEmail=" + email;

    window.location.href = newUrl;
}