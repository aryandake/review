function checkDuplicateCircularNo() {

    var ajaxRequest; // The variable that makes Ajax possible!
    try {

        // Opera 8.0+, Firefox, Safari
        ajaxRequest = new XMLHttpRequest();
    } catch (e) {
        // Internet Explorer Browsers
        try {
            ajaxRequest = new ActiveXObject("Msxml2.XMLHTTP");
        } catch (e) {
            try {
                ajaxRequest = new ActiveXObject("Microsoft.XMLHTTP");
            } catch (e) {
                // Something went wrong
                alert("Your browser broke!");
                return true;
            }
        }
    }

    // Create a function that will receive data sent from the server
    ajaxRequest.onreadystatechange = function () {
        if (ajaxRequest.readyState == 4) {
            if (ajaxRequest.status == 200) {
                var obj = ajaxRequest.responseText;
                if (obj == "true") {

                    $("#divCircError").css({ "visibility": "visible", "display": "block" });
                    document.getElementById("divCircError").innerHTML = "Circular no. already exists please enter different circular number.";
                    document.getElementById("ctl00_ContentPlaceHolder1_hfDuplicateFlag").value = "Y"

                }
                else {
                    $("#divCircError").css({ "visibility": "hidden", "display": "none" });
                    document.getElementById("divCircError").innerHTML = "";
                    document.getElementById("ctl00_ContentPlaceHolder1_hfDuplicateFlag").value = "N"
                }
            }
        }
    }

    let code = '';
    code = document.getElementById("ctl00_ContentPlaceHolder1_txtCircularNo").value;

    if (code != '') {
        var url = "../checkDuplicateCircularNo.aspx?Cno=" + code;
        ajaxRequest.open("POST", url);
        ajaxRequest.send(null);
        return true;
    }
    else {
        $("#divCircError").css({ "visibility": "hidden", "display": "none" });
        document.getElementById("divCircError").innerHTML = "";
        document.getElementById("ctl00_ContentPlaceHolder1_hfDuplicateFlag").value = "N"
        return false;
    }

}

