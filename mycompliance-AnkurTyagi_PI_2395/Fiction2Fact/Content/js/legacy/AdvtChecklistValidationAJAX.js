function advtChecklistValidation(AdvtNature, rowcount, status) {
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

    var url = SiteUrlJS("Projects/Advertisement/AdvtChecklistValidationAjax.aspx?Type=" + AdvtNature + "&RowCount=" + rowcount + "&Status=" + status);
    ajaxRequest.open("POST", url, false);
    ajaxRequest.send(null);
    var strmsg = ajaxRequest.responseText;

    return strmsg;

}
