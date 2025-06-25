function populateUserDetsByCode(modulename, uniqueRowId) {
    //alert('abc');
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
                var arrObj = obj.split("|");
                var userName = arrObj[0];
                var userEmail = arrObj[1];
                var userDept = arrObj[2];

                userDets(modulename, userName, userEmail, userDept, uniqueRowId);
            }
        }
    }
    let code = '';
    if (modulename == 'SecActionable') {
        code = document.getElementById("txtOwnerEmpCode" + uniqueRowId).value;
    }
    else if (modulename == 'HelpDesk') {
        code = document.getElementById("ctl00_ContentPlaceHolder1_txtUserId").value;
    }
    else if (modulename == 'CertSPOCEmpCode') {
        code = document.getElementById("txtUserId" + uniqueRowId).value;
    }
    else if (modulename == 'CertSPOCMas') {
        code = document.getElementById("txtUserId" + uniqueRowId).value;
    }
    else if (modulename == 'CertUnitHeadEmpCode') {
        code = document.getElementById("ctl00_ContentPlaceHolder1_txtCertDeptUserId").value;
    }
    else if (modulename == 'UserDetsPersonResponsible') {
        code = document.getElementById("txtPersonResponsibleId" + uniqueRowId).value;
    }
    else if (modulename == 'UserDetsReprtMgr') {
        code = document.getElementById("txtReportingMgrId" + uniqueRowId).value;
    }
    else if (modulename == 'UserDetsActPersonResponsible') {
        code = document.getElementById("txtResponsiblePersonID" + uniqueRowId).value;
    }
    else if (modulename == 'UserDetsActReportingMgr') {
        code = document.getElementById("txtReportingMgrId" + uniqueRowId).value;
    }

    //<< Added by Ritesh Tak on 07-04-2023
    else if (modulename == 'L0Reviewer') {
        code = document.getElementById("ctl00_ContentPlaceHolder1_txtRId").value;
    }
    else if (modulename == 'L1Reviewer') {
        code = document.getElementById("ctl00_ContentPlaceHolder1_txtL1RId").value;
    }
    else if (modulename == 'L2Reviewer') {
        code = document.getElementById("ctl00_ContentPlaceHolder1_txtL2RId").value;
    }
    else if (modulename == 'UnitSPOCID') {
        code = document.getElementById("ctl00_ContentPlaceHolder1_txtUnitSPOCID").value;
    }
    else if (modulename == 'UnitHeadID') {
        code = document.getElementById("ctl00_ContentPlaceHolder1_txtUnitHeadId").value;
    }
    //>>
    else if (modulename == 'RD') {
        code = document.getElementById("ctl00_ContentPlaceHolder1_txtOUserId").value;
    }
    else if (modulename == 'TD') {
        code = document.getElementById("ctl00_ContentPlaceHolder1_txtOUserId").value;
    }

    //UserDetsActPersonResponsible

    //<<
    if (code != '') {
        var url = "../PopulateUserDets.aspx?UserCode=" + code + "&Type="
            + modulename;
        ajaxRequest.open("POST", url);
        ajaxRequest.send(null);
        return true;
    }
    else {
        userDets(modulename, '', '', uniqueRowId);
        return true;
    }
}

function userDets(modulename, userName, userEmail, userDept, uniqueRowId) {
    if (modulename == 'SecActionable') {
        document.getElementById("txtOwnerName" + uniqueRowId).value = userName;
        document.getElementById("txtOwnerEmailId" + uniqueRowId).value = userEmail;
        document.getElementById("txtOwnerDept" + uniqueRowId).value = userDept;
    }
    else if (modulename == 'HelpDesk') {
        document.getElementById("ctl00_ContentPlaceHolder1_txtName").value = userName;
        document.getElementById("ctl00_ContentPlaceHolder1_txtEmailId").value = userEmail;
    }
    else if (modulename == 'CertSPOCEmpCode') {
        document.getElementById("txtUserName" + uniqueRowId).value = userName;
        document.getElementById("txtEmailId" + uniqueRowId).value = userEmail;
    }
    else if (modulename == 'CertSPOCMas') {
        document.getElementById("txtUserName" + uniqueRowId).value = userName;
        document.getElementById("txtEmailId" + uniqueRowId).value = userEmail;
    }
    else if (modulename == 'CertUnitHeadEmpCode') {
        document.getElementById("ctl00_ContentPlaceHolder1_txtCertDeptUserName").value = userName;
        document.getElementById("ctl00_ContentPlaceHolder1_txtCertDeptEmailID").value = userEmail;
    }
    else if (modulename == 'UserDetsPersonResponsible') {
        document.getElementById("txtPersonResponsibleName" + uniqueRowId).value = userName;
        document.getElementById("txtPersonResponsibleEmailId" + uniqueRowId).value = userEmail;
    }
    else if (modulename == 'UserDetsReprtMgr') {
        document.getElementById("txtReportingMgrName" + uniqueRowId).value = userName;
        document.getElementById("txtReportingMgrEmailId" + uniqueRowId).value = userEmail;
    }
    else if (modulename == 'UserDetsActPersonResponsible') {
        document.getElementById('txtResponsiblePersonName' + uniqueRowId).value = userName;
        document.getElementById('txtResponsiblePersonEmail' + uniqueRowId).value = userEmail;
    }
    else if (modulename == 'UserDetsActReportingMgr') {
        document.getElementById('txtReportingMgrName' + uniqueRowId).value = userName;
        document.getElementById('txtReportingMgrEmail' + uniqueRowId).value = userEmail;
    }

    //<< Added by Ritesh Tak on 07-04-2023
    else if (modulename == 'L0Reviewer') {
        document.getElementById("ctl00_ContentPlaceHolder1_txtRName").value = userName;
        document.getElementById("ctl00_ContentPlaceHolder1_txtREmail").value = userEmail;
    }
    else if (modulename == 'L1Reviewer') {
        document.getElementById("ctl00_ContentPlaceHolder1_txtL1RName").value = userName;
        document.getElementById("ctl00_ContentPlaceHolder1_txtL1REmail").value = userEmail;
    }
    else if (modulename == 'L2Reviewer') {
        document.getElementById("ctl00_ContentPlaceHolder1_txtL2RName").value = userName;
        document.getElementById("ctl00_ContentPlaceHolder1_txtL2REmail").value = userEmail;
    }
    else if (modulename == 'UnitHeadID') {
        document.getElementById("ctl00_ContentPlaceHolder1_txtUnitHeadName").value = userName;
        document.getElementById("ctl00_ContentPlaceHolder1_txtUnitHeadEmail").value = userEmail;
    }
    else if (modulename == 'UnitSPOCID') {
        document.getElementById("ctl00_ContentPlaceHolder1_txtUnitSPOCName").value = userName;
        document.getElementById("ctl00_ContentPlaceHolder1_txtUnitSPOCEmail").value = userEmail;
    }
    //>>
    else if (modulename == 'RD') {
        document.getElementById("ctl00_ContentPlaceHolder1_txtOName").value = userName;
        document.getElementById("ctl00_ContentPlaceHolder1_txtOEmailId").value = userEmail;
    }
    else if (modulename == 'TD') {
        document.getElementById("ctl00_ContentPlaceHolder1_txtOName").value = userName;
        document.getElementById("ctl00_ContentPlaceHolder1_txtOEmailId").value = userEmail;
    }
}