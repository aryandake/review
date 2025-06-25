//checked records delete
var focuson = null;


function setfocuson(obj) {
    if (focuson == null) {
        focuson = obj;
    }
}

function addExceptionRow() {
    try {
        var uniqueId = 0;
        var cellElements;
        var table = document.getElementById('tblException');
        var rowCount = table.rows.length;
        var idx = rowCount - 1;
        //alert('1');
        if (idx != 0) {
            cellElements = table.rows[idx].cells[0]
                .getElementsByTagName("input");

            uniqueId = cellElements[0].value;
            uniqueId = parseInt(uniqueId) + 1;
        }

        //alert('2');
        var row = table.insertRow(rowCount);

        var cell0 = row.insertCell(0);
        var cellElement = document.createElement("input");
        cellElement.type = "hidden";
        cellElement.id = "uniqueRowId" + uniqueId;
        cellElement.value = uniqueId;
        cell0.appendChild(cellElement);


        cellElement = document.createElement("input");
        cellElement.type = "hidden";
        cellElement.id = "certId" + uniqueId;
        cellElement.value = '0';
        cell0.appendChild(cellElement);

        //alert('3');
        var cellElement = document.createElement("input");
        cellElement.type = "checkbox";
        cellElement.id = "cbException" + uniqueId;
        cell0.className = "tabbody3";
        cell0.appendChild(cellElement);

        //alert('4');
        var cell01 = row.insertCell(1);
        cellElement = document.createElement("a");
        cellElement.id = "EX_AttachFileImg" + uniqueId;
        cellElement.innerHTML = '<img border="0" src="../../Content/images/legacy/attach.png" />';
        cellElement.href = "#";
        cellElement.onclick = Function("return openpopupExceptionAttachments("
            + uniqueId + ")");
        cell01.className = "tabbody3";
        cell01.appendChild(cellElement);

        cellElement = document.createElement("a");
        cellElement.id = "EX_DeleteFileImg" + uniqueId;
        cellElement.innerHTML = '<img border="0" src="../../Content/images/legacy/delete.gif" />';
        cellElement.href = "#";
        cellElement.onclick = Function("return deleteExceptionFile("
            + uniqueId + ")");

        cellElement.style.visibility = "hidden";
        cell01.appendChild(cellElement);

        //alert('5');
        var cell02 = row.insertCell(2);
        cellElement = document.createElement("input");
        cellElement.type = "hidden";
        cellElement.id = "ClientFileName" + uniqueId;
        cell02.appendChild(cellElement);
        cellElement = document.createElement("input");
        cellElement.type = "hidden";
        cellElement.id = "ServerFileName" + uniqueId;
        cell02.appendChild(cellElement);
        cellElement = document.createElement("a");
        cellElement.id = "EX_Filelink" + uniqueId;
        //cellElement.innerHTML = clientfilename;
        cellElement.href = "#";
        cell02.className = "tabbody3";
        cell02.appendChild(cellElement);

        //alert('6');
        var cell03 = row.insertCell(3);
        cellElement = document.createElement("textarea");
        //cellElement.type = "text";
        cellElement.id = "ExceptionType" + uniqueId;
        cellElement.maxLength = "4000";
        cellElement.className = "form-control";
        //cellElement.size = 21;
        cellElement.rows = 3;
        cellElement.cols = 25;
        cell03.className = "tabbody3";
        cell03.appendChild(cellElement);

        //alert('7');
        var cell04 = row.insertCell(4);
        cellElement = document.createElement("textarea");
        cellElement.id = "Details" + uniqueId;
        cellElement.maxLength = "4000";
        cellElement.rows = 3;
        cellElement.cols = 25;
        //		cellElement.size = 21;
        cellElement.className = "form-control";
        cell04.className = "tabbody3";
        cell04.appendChild(cellElement);

        //Added By Milan Yadav on 05-Feb-2016
        //<<
        //alert('8');
        var cell05 = row.insertCell(5);
        cellElement = document.createElement("textarea");
        cellElement.id = "RootCause" + uniqueId;
        cellElement.maxLength = "4000";
        //		cellElement.size = 21;
        cellElement.rows = 3;
        cellElement.cols = 25;
        cellElement.className = "form-control";
        cell05.className = "tabbody3";
        cell05.appendChild(cellElement);

        //alert('9');
        var cell06 = row.insertCell(6);
        cellElement = document.createElement("textarea");
        cellElement.id = "Actiontaken" + uniqueId;
        cellElement.maxLength = "4000";
        //		cellElement.size = 21;
        cellElement.rows = 3;
        cellElement.cols = 25;
        cellElement.className = "form-control";
        cell06.className = "tabbody3";
        cell06.appendChild(cellElement);

        var cell07 = row.insertCell(7);
        cellElement = document.createElement('select');
        cellElement.id = "ddlActionStatus" + uniqueId;
        var options = ['Select', 'Open', 'Closed'];
        options.forEach(function (text) {
            var option = document.createElement('option');
            option.value = text;
            option.textContent = text;
            cellElement.appendChild(option);
        });
        cellElement.className = "form-select";
        cellElement.style.width = "120px";
        cellElement.onchange = function () {
            if (this.value === "Select") {
                document.getElementById("txtTargetDate" + uniqueId).value = "";
            }
        };
        cell07.appendChild(cellElement);

        //alert('10');
        var cell08 = row.insertCell(8);
        cellElement = document.createElement("input");
        cellElement.type = "text";
        cellElement.id = "txtTargetDate" + uniqueId;
        cellElement.maxLength = "11";
        cellElement.size = 10;
        cellElement.className = "form-control";
        cellElement.readOnly = true;
        cellElement.style.width = "120px";
        cell08.appendChild(cellElement);

        $j("#" + cellElement.id).datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd-M-yy'
        });


        //>>
        cellElement = document.getElementById("ExceptionType" + uniqueId);
        cellElement.focus();

        //document.getElementById("ExceptionType" + uniqueId).focus();

        return false;

    } catch (e) {
        alert(e.message);
        return false;
    }
}

function deleteExceptionFile(rowNo) {
    //alert('123');
    var filename = document.getElementById("ServerFileName" + rowNo).value;
    //alert(filename);
    var certId = document.getElementById("certId" + rowNo).value;
    //alert(certId);
    if (!confirm('Are you sure that you want to delete this record?')) return false;
    window.open(
        "../DeleteFile.aspx?calledFrom=Exception&fileId=" + certId + "&rowNo="
        + rowNo + "&filename=" + filename,
        "FILE", "location=0,status=0,scrollbars=0,width=10,height=10");
    //alert('123456');
    return false;
}

function openpopupExceptionAttachments(rowNo) {
    window.open("../UploadFile.aspx?rowNo=" + rowNo, "FILE",
        "location=0,status=0,scrollbars=0,width=650,height=350");
    return false;
}

function deleteExceptionRow() {
    try {

        var table = document.getElementById("tblException");
        var rowCount = table.rows.length;
        var cellElements, cellUniqueRow, uniqueRowId;
        var strUniqueRowId = "", strId = "";
        var certId = "";
        var rowIdFrmTbl;
        var deleteFromDB = 'N';
        for (var i = 1; i < rowCount; i++) {
            cellElements = table.rows[i].cells[0].getElementsByTagName("input");
            rowIdFrmTbl = cellElements[1].value;
            rowIdchecked = cellElements[2].checked;
            if (rowIdchecked == true) {
                cellUniqueRow = table.rows[i].cells[0].getElementsByTagName("input");
                uniqueRowId = cellUniqueRow[0].value;

                certId = document.getElementById("certId" + uniqueRowId).value;
                if (certId != '0') {
                    deleteFromDB = 'Y';
                }
                strUniqueRowId = strUniqueRowId + uniqueRowId + ",";
                strId = strId + rowIdFrmTbl + ",";
            }
        }

        if (strUniqueRowId == '') {
            alert('Please select one or more records for deletion.');
            return false;
        }

        if (!confirm('Are you sure that you want to delete these records?')) {
            return false;
        }

        else {
            deleteFromHTML(strUniqueRowId);
            if (deleteFromDB == 'Y') {
                window.open("../DeleteDBRecords.aspx?calledFrom=Exception&Ids=" + strId,
                    "DBRecords", "location=0,status=0,scrollbars=0,width=10,height=10");
            }
            return false;

        }
    }
    catch (e) {
        alert(e);
        return false;
    }
}


function deleteFromHTML(strUniqueRowId) {
    var table = document.getElementById("tblException");
    var rowCount = table.rows.length;
    //alert('rowCount:' + rowCount);
    var cellElements;
    var uniqueRowId;
    var uniqueRowIds = strUniqueRowId.split(",");

    for (var cnt1 = 0; cnt1 < uniqueRowIds.length - 1; cnt1++) {
        uniqueRowId = uniqueRowIds[cnt1];
        for (var cnt2 = 1; cnt2 < rowCount; cnt2++) {
            var row = table.rows[cnt2];
            cellElements = table.rows[cnt2].cells[0].getElementsByTagName("input");
            rowIdFrmTbl = cellElements[0].value;
            if (rowIdFrmTbl == uniqueRowId) {
                table.deleteRow(cnt2);
                break;
            }
        }
    }
}

function getExceptionDetails() {
    try {
        var validated;
        var checklistGrid = document.getElementById('ctl00_ContentPlaceHolder1_gvChecklist');

        if (checklistGrid != null) {
            validated = Page_ClientValidate('Save');
        }
        else
            validated = true;

        //var validated = Page_ClientValidate('Save');
        if (validated) {
            var IsDoubleClickFlagSet = document.getElementById('ctl00_ContentPlaceHolder1_hfDoubleClickFlag').value;
            if (IsDoubleClickFlagSet == 'Yes') {
                alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                return false;
            }
            else {
                var table = document.getElementById("tblException");
                var hiddenFormData = document.getElementById("ctl00_ContentPlaceHolder1_hfExceptions");

                if (table == null) {
                    return;
                }

                var rowCount = table.rows.length;
                var row;
                var ExceptionNames = "";
                var certID, uniqueRowId, cellElements0;
                var exceptionType;
                var details;
                var clientFileName, serverFileName;
                var errMsg = "";
                var RootCause;
                var Actiontaken;
                var ActionStatus;
                var TargetDate;

                hiddenFormData.value = "";


                for (var i = 1; i < rowCount; i++) {
                    row = table.rows[i];
                    var cellElements0 = row.cells[0].getElementsByTagName("input");
                    uniqueRowId = cellElements0[0].value;
                    certID = document.getElementById('certId' + uniqueRowId).value;
                    exceptionTypeField = document.getElementById('ExceptionType' + uniqueRowId)
                    exceptionType = exceptionTypeField.value;
                    details = document.getElementById('Details' + uniqueRowId).value;
                    clientFileName = document
                        .getElementById('ClientFileName' + uniqueRowId).value;

                    serverFileName = document
                        .getElementById('ServerFileName' + uniqueRowId).value;

                    RootCause = document.getElementById('RootCause' + uniqueRowId).value;
                    Actiontaken = document.getElementById('Actiontaken' + uniqueRowId).value;
                    ActionStatus = document.getElementById('ddlActionStatus' + uniqueRowId).value;
                    TargetDate = document.getElementById('txtTargetDate' + uniqueRowId).value;

                    ExceptionNames = ExceptionNames + "|" + certID + "|" + exceptionType + "|" + details + "|" + clientFileName
                        + "|" + serverFileName + "|" + RootCause + "|" + Actiontaken + "|" + ActionStatus + "|" + TargetDate + "~";
                }

                if (ExceptionNames != "") {
                    hiddenFormData.value = ExceptionNames;
                }

                if (errMsg == '') {
                    if (!confirm("Are you sure you want to save draft this certificate?")) {
                        return false;
                    }
                    return true;
                }
                else {
                    alert(errMsg);
                    return false;
                }
            }
        }
    } catch (e) {
        alert(e.message);
        return false;
    }
}

function getExceptionDetailsOnSave()// *****
{
    try {
        var validated;
        var checklistGrid = document.getElementById('ctl00_ContentPlaceHolder1_gvChecklist');

        if (checklistGrid != null) {
            validated = Page_ClientValidate('Save');
        }
        else
            validated = true;

        //var validated = Page_ClientValidate('Save');
        if (validated) {
            var IsDoubleClickFlagSet = document.getElementById('ctl00_ContentPlaceHolder1_hfDoubleClickFlag').value;
            if (IsDoubleClickFlagSet == 'Yes') {
                alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                return false;
            }
            else {
                var table = document.getElementById("tblException");
                var hiddenFormData = document.getElementById("ctl00_ContentPlaceHolder1_hfExceptions");
                var hfCurrentDate = document.getElementById("ctl00_ContentPlaceHolder1_hfCurrentDate");

                if (table == null) {
                    return;
                }

                var rowCount = table.rows.length;
                var row;
                var ExceptionNames = "";
                var certID, uniqueRowId, cellElements0;
                var exceptionType;
                var details;
                var clientFileName, serverFileName, chklstServerFileName;
                var errMsg = "";
                var RootCause;
                var Actiontaken;
                var ActionStatus;
                var TargetDate;
                hiddenFormData.value = "";
                var cnt = 0;
                var rblYesNoNA0, rblYesNoNA1, rblYesNoNA2, rblYesNoNA3, txtRemarks;
                var ddlComplianceStatusObj;
                var grid = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist");

                if (grid != null) {
                    if (grid.rows.length > 0) {
                        for (var j = 2; j < grid.rows.length + 1; j++) {
                            cnt++;
                            if (j < 10) {
                                j = "0" + j
                            } else {
                                j = j
                            }

                            ddlComplianceStatusObj = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_ddlrbyesnona");
                            txtRemarks = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_txtRemarks");
                            txtTargetDate = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_txtTargetDate");
                            txtActionPlan = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_txtActionPlan");
                            txtNCSinceDate = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_txtNCSinceDate");

                            var ComplainceStatus = ddlComplianceStatusObj.options[ddlComplianceStatusObj.selectedIndex].value;

                            //if (ComplainceStatus == '') {
                            //    errMsg = errMsg + bullet
                            //        + 'Please select Compliance Status for Checklist Grid Row ' + cnt + '.\n';
                            //}

                            if (ComplainceStatus == "N") {
                                //<< Added by Amarjeet on 07-Oct-2021
                                if (!required(txtRemarks)) {
                                    errMsg = errMsg + bullet
                                        + 'Please enter Reason of non compliance for Checklist Grid Row ' + cnt + '.\n';
                                }
                                //>>
                                //<<Added by Ankur Tyagi on 05Feb2024 for CR_1948
                                if (!required(txtNCSinceDate)) {
                                    errMsg = errMsg + bullet
                                        + 'Please enter Non Compliant since from Date for Checklist Grid Row ' + cnt + '.\n';
                                }
                                //>>

                                if (!required(txtActionPlan)) {
                                    errMsg = errMsg + bullet
                                        + 'Please enter Action Plan for Checklist Grid Row ' + cnt + '.\n';
                                }


                                if (!required(txtTargetDate)) {
                                    errMsg = errMsg + bullet
                                        + 'Please enter Target Date for Checklist Grid Row ' + cnt + '.\n';
                                }

                            }

                            //<< Modified by Ankur Tyagi on 17-Jan-2024
                            else if (ComplainceStatus == "W") {

                                if (!required(txtRemarks)) {
                                    errMsg = errMsg + bullet
                                        + 'Please enter Remarks for Checklist Grid Row ' + cnt + '.\n';
                                }

                                if (!required(txtTargetDate)) {
                                    errMsg = errMsg + bullet
                                        + 'Please enter Target Date for Checklist Grid Row ' + cnt + '.\n';
                                }
                            }

                            else if (ComplainceStatus == "NA") {

                                if (!required(txtRemarks)) {
                                    errMsg = errMsg + bullet
                                        + 'Please enter Remarks for Checklist Grid Row ' + cnt + '.\n';
                                }

                            }

                            //if (ComplainceStatus == "C" && txtRemarks.value != "") {
                            //    errMsg = errMsg + bullet
                            //        + 'Reason of non compliance shall not be added for Checklist Grid Row ' + cnt + '.\n';
                            //}
                            //>>
                            if (ComplainceStatus != "N" && txtActionPlan.value != "") {
                                errMsg = errMsg + bullet
                                    + 'Action Plan shall not be added for Checklist Grid Row ' + cnt + '.\n';
                            }

                            if ((ComplainceStatus != "N" && ComplainceStatus != "W") && txtTargetDate.value != "") {
                                errMsg = errMsg + bullet
                                    + 'Target Date shall not be added for Checklist Grid Row ' + cnt + '.\n';
                            }
                            //>>
                        }
                    }
                }

                for (var i = 1; i < rowCount; i++) {
                    row = table.rows[i];
                    var cellElements0 = row.cells[0].getElementsByTagName("input");
                    uniqueRowId = cellElements0[0].value;
                    certID = document.getElementById('certId' + uniqueRowId).value;
                    exceptionTypeField = document.getElementById('ExceptionType' + uniqueRowId);
                    exceptionType = exceptionTypeField.value;

                    ActionStatus = document.getElementById('ddlActionStatus' + uniqueRowId);
                    TargetDate = document.getElementById('txtTargetDate' + uniqueRowId);

                    if (ActionStatus.value != 'Select') {
                        if (ActionStatus.value == 'Open') {
                            if (TargetDate.value != '') {
                                if (!checkDateIfEntered(TargetDate)) {
                                    errMsg = errMsg + bullet + 'Target date format should be dd-mon-yyyy for Compliance Deviations grid row no. ' + i + '.\n';
                                }
                                else
                                    if (compareDates(TargetDate, hfCurrentDate) != 0) {
                                        errMsg = errMsg + bullet
                                            + 'Target date should be greater than or equal to current date for Compliance Deviations grid row no. ' + i + '.\n';
                                    }
                            }
                            else {
                                errMsg = errMsg + bullet + 'Target date can not be blank for Compliance Deviations grid row no. ' + i + '.\n';
                            }
                        }
                        else {
                            if (TargetDate.value != '') {
                                if (!checkDateIfEntered(TargetDate)) {
                                    errMsg = errMsg + bullet + 'Closure date format should be dd-mon-yyyy for Compliance Deviations grid row no. ' + i + '.\n';
                                }
                                else
                                    if (compareDates(hfCurrentDate, TargetDate) != 0) {
                                        errMsg = errMsg + bullet
                                            + 'Closure date should be less than or equal to current date for Compliance Deviations grid row no. ' + i + '.\n';
                                    }
                            }
                            else {
                                errMsg = errMsg + bullet + 'Closure date can not be blank for Compliance Deviations grid row no. ' + i + '.\n';
                            }
                        }
                    }
                    else {
                        errMsg = errMsg + bullet + 'Please select current status for Compliance Deviations grid row no. ' + i + '.\n';
                    }

                    details = document.getElementById('Details' + uniqueRowId).value;
                    clientFileName = document
                        .getElementById('ClientFileName' + uniqueRowId).value;

                    serverFileName = document
                        .getElementById('ServerFileName' + uniqueRowId).value;

                    RootCause = document.getElementById('RootCause' + uniqueRowId).value;
                    Actiontaken = document.getElementById('Actiontaken' + uniqueRowId).value;


                    ExceptionNames = ExceptionNames + "|" + certID + "|" + exceptionType + "|" + details + "|" + clientFileName + "|"
                        + serverFileName + "|" + RootCause + "|" + Actiontaken + "|" + ActionStatus.value + "|" + TargetDate.value + "~";
                }

                if (ExceptionNames != "") {
                    hiddenFormData.value = ExceptionNames;
                }

                if (errMsg == '') {
                    if (!confirm("Please ensure that you have duly filled in/reviewed the deviations relating to non-compliant points." + '\n\n' + "Are you sure you want to submit this certificate?")) {
                        return false;
                    }
                    return true;
                }
                else {
                    alert(errMsg);
                    return false;
                }
            }
        }
    } catch (e) {
        alert(e.message);
        return false;
    }
}

//>>Added By Milan Yadav on 11-Apr-2017
function getExceptionDetailsOnSaveFH()// *****
{
    try {
        var table = document.getElementById("tblException");
        var hiddenFormData = document.getElementById("ctl00_ContentPlaceHolder1_hfExceptions");
        var hfCurrentDate = document.getElementById("ctl00_ContentPlaceHolder1_hfCurrentDate");

        if (table == null) {

            return;
        }
        var rowCount = table.rows.length;
        var row;
        var ExceptionNames = "";
        var certID, uniqueRowId, cellElements0;
        var exceptionType;
        var details;
        var clientFileName, serverFileName, chklstServerFileName;
        var errMsg = "";
        var RootCause;
        var Actiontaken;
        var ActionStatus;
        var TargetDate;

        hiddenFormData.value = "";
        var cnt = 0;
        var rblYesNoNA0, rblYesNoNA1, rblYesNoNA2, rblYesNoNA3, txtRemarks, txtTargetDate, txtActionPlan;
        var ddlComplianceStatusObj;
        var grid = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist");

        if (grid != null) {
            if (grid.rows.length > 0) {
                for (var j = 2; j < grid.rows.length + 1; j++) {
                    cnt++;
                    if (j < 10) {
                        j = "0" + j
                    } else {
                        j = j
                    }


                    //rblYesNoNA0 = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_rbyesnona" + '_0');                
                    //rblYesNoNA1 = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_rbyesnona" + '_1');
                    //rblYesNoNA2 = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_rbyesnona" + '_2');



                    ddlComplianceStatusObj = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_rbyesnona");
                    txtRemarks = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_txtRemarks");
                    txtTargetDate = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_txtTargetDate");
                    txtActionPlan = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_txtActionPlan");
                    txtNCSinceDate = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_txtNCSinceDate");

                    var ComplainceStatus = ddlComplianceStatusObj.options[ddlComplianceStatusObj.selectedIndex].value;

                    //if (!rblYesNoNA0.checked && !rblYesNoNA1.checked && !rblYesNoNA2.checked)
                    //if (ComplainceStatus == '') {
                    //    errMsg = errMsg + bullet
                    //        + 'Please select Compliance Status for Checklist Grid Row ' + cnt
                    //        + '.\n';
                    //}

                    if (ComplainceStatus == "N") {
                        //<< Added by Amarjeet on 07-Oct-2021
                        if (!required(txtRemarks)) {
                            errMsg = errMsg + bullet
                                + 'Please enter Reason of non compliance for Checklist Grid Row ' + cnt + '.\n';
                        }
                        //>>

                        //<<Added by Ankur Tyagi on 05Feb2024 for CR_1948
                        if (!required(txtNCSinceDate)) {
                            errMsg = errMsg + bullet
                                + 'Please enter Non Compliant since from Date for Checklist Grid Row ' + cnt + '.\n';
                        }
                        //>>


                        if (!required(txtActionPlan)) {
                            errMsg = errMsg + bullet
                                + 'Please enter Action Plan for Checklist Grid Row ' + cnt + '.\n';
                        }

                        if (!required(txtTargetDate)) {
                            errMsg = errMsg + bullet
                                + 'Please enter Target Date for Checklist Grid Row ' + cnt + '.\n';
                        }

                    }
                    //<< Modified by Ankur Tyagi on 17-Jan-2024
                    else if (ComplainceStatus == "W") {

                        if (!required(txtRemarks)) {
                            errMsg = errMsg + bullet
                                + 'Please enter Remarks for Checklist Grid Row ' + cnt + '.\n';
                        }

                        if (!required(txtTargetDate)) {
                            errMsg = errMsg + bullet
                                + 'Please enter Target Date for Checklist Grid Row ' + cnt + '.\n';
                        }
                    }

                    else if (ComplainceStatus == "NA") {

                        if (!required(txtRemarks)) {
                            errMsg = errMsg + bullet
                                + 'Please enter Remarks for Checklist Grid Row ' + cnt + '.\n';
                        }

                    }

                    //if (ComplainceStatus == "C" && txtRemarks.value != "") {
                    //    errMsg = errMsg + bullet
                    //        + 'Reason of non compliance shall not be added for Checklist Grid Row ' + cnt + '.\n';
                    //}
                    //>>
                    if (ComplainceStatus != "N" && txtActionPlan.value != "") {
                        errMsg = errMsg + bullet
                            + 'Action Plan shall not be added for Checklist Grid Row ' + cnt + '.\n';
                    }

                    if ((ComplainceStatus != "N" && ComplainceStatus != "W") && txtTargetDate.value != "") {
                        errMsg = errMsg + bullet
                            + 'Target Date shall not be added for Checklist Grid Row ' + cnt + '.\n';
                    }
                    //>>
                }
            }
        }

        for (var i = 1; i < rowCount; i++) {
            row = table.rows[i];
            var cellElements0 = row.cells[0].getElementsByTagName("input");
            uniqueRowId = cellElements0[0].value;
            certID = document.getElementById('certId' + uniqueRowId).value;
            exceptionTypeField = document.getElementById('ExceptionType' + uniqueRowId);
            exceptionType = exceptionTypeField.value;

            ActionStatus = document.getElementById('ddlActionStatus' + uniqueRowId);
            TargetDate = document.getElementById('txtTargetDate' + uniqueRowId);

            if (ActionStatus.value != 'Select') {
                if (ActionStatus.value == 'Open') {
                    if (TargetDate.value != '') {
                        if (!checkDateIfEntered(TargetDate)) {
                            errMsg = errMsg + bullet + 'Target date format should be dd-mon-yyyy for Compliance Deviations grid row no. ' + i + '.\n';
                        }
                        else
                            if (compareDates(TargetDate, hfCurrentDate) != 0) {
                                errMsg = errMsg + bullet
                                    + 'Target date should be greater than or equal to current date for Compliance Deviations grid row no. ' + i + '.\n';
                            }
                    }
                    else {
                        errMsg = errMsg + bullet + 'Target date can not be blank for Compliance Deviations grid row no. ' + i + '.\n';
                    }
                }
                else {
                    if (TargetDate.value != '') {
                        if (!checkDateIfEntered(TargetDate)) {
                            errMsg = errMsg + bullet + 'Closure date format should be dd-mon-yyyy. \n';
                        }
                        else
                            if (compareDates(hfCurrentDate, TargetDate) != 0) {
                                errMsg = errMsg + bullet
                                    + 'Closure date should be less than or equal to current date for Compliance Deviations grid row no. ' + i + '.\n';
                            }
                    }
                    else {
                        errMsg = errMsg + bullet + 'Closure date can not be blank for Compliance Deviations grid row no. ' + i + '.\n';
                    }
                }
            }
            else {
                errMsg = errMsg + bullet + 'Please select current status for Compliance Deviations grid row no. ' + i + '.\n';
            }

            details = document.getElementById('Details' + uniqueRowId).value;
            clientFileName = document
                .getElementById('ClientFileName' + uniqueRowId).value;

            serverFileName = document
                .getElementById('ServerFileName' + uniqueRowId).value;

            RootCause = document.getElementById('RootCause' + uniqueRowId).value;
            Actiontaken = document.getElementById('Actiontaken' + uniqueRowId).value;


            ExceptionNames = ExceptionNames + "|" + certID + "|" + exceptionType + "|" + details + "|" + clientFileName + "|"
                + serverFileName + "|" + RootCause + "|" + Actiontaken + "|" + ActionStatus.value + "|" + TargetDate.value + "~";
        }

        if (ExceptionNames != "") {
            hiddenFormData.value = ExceptionNames;
        }

        if (errMsg == '') {

            if (!confirm("Please ensure that you have duly filled in/reviewed the deviations relating to non-compliant points." + '\n\n' + "Are you sure you want to submit this certificate?")) {
                return false;
            }
            return true;
        }
        else {
            alert(errMsg);
            return false;
        }
    } catch (e) {
        alert(e.message);
        return false;
    }
}
//<<

function onHeaderRowChecked() {
    var table = document.getElementById("tblException");
    var allCheck = document.getElementById("HeaderLevelCheckBox");
    var rowCount = table.rows.length;
    var uniqueRowId;
    var check;
    for (var i = 1; i < rowCount; i++) {
        var row = table.rows[i];
        cellElements = table.rows[i].cells[0].getElementsByTagName("input");
        uniqueRowId = cellElements[0].value;
        check = document.getElementById("cbException" + uniqueRowId);
        check.checked = allCheck.checked;
    }
}

//Added By Milan Yadav on 27-Sep-2016
//>>
function getDeactivationDetails() {
    try {
        var msgtext = "";
        focuson = null;
        var EffectiveToDate = document.getElementById("ctl00_ContentPlaceHolder1_txtEffectiveToDate");
        var Remarks = document.getElementById("ctl00_ContentPlaceHolder1_txtRemarks");

        if (!required(Remarks)) {
            msgtext = msgtext + bullet
                + 'Please enter Deactivation Remarks. \n';
            setfocuson(Remarks);
        }
        if (!required(EffectiveToDate)) {
            msgtext = msgtext + bullet
                + 'Please enter Effective To Date. \n';
            setfocuson(EffectiveToDate);
        }

        if (msgtext != "") {
            alert(msgtext);
            return false;
        }
        else {
            return true;
        }

    }
    catch (e) {
        alert(e.message);
        return false;
    }
}




