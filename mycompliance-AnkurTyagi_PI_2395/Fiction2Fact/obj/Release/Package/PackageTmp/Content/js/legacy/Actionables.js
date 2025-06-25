var bullet = '- ';

function addActionablesRow() {
    try {

        var uniqueId = 0;
        var cellElements;
        var table = document.getElementById('tblActionables');
        //alert("h"+table);
        var rowCount = table.rows.length;
        var idx = rowCount - 1;

        if (idx != 0) {
            cellElements = table.rows[idx].cells[0]
					.getElementsByTagName("input");

            uniqueId = cellElements[0].value;
            uniqueId = parseInt(uniqueId) + 1;
        }
        
        var row = table.insertRow(rowCount);

        var cell0 = row.insertCell(0);
        var cellElement = document.createElement("input");
        cellElement.type = "hidden";
        cellElement.id = "uniqueRowId" + uniqueId;
        cellElement.value = uniqueId;
        cell0.appendChild(cellElement);

        var cellElement = document.createElement("input");
        cellElement.type = "checkbox";
        cellElement.id = "cbActionables" + uniqueId;
        cell0.className = "tabbody3";
        cell0.appendChild(cellElement);

        var cell01 = row.insertCell(1);

        var cellElement = document.createElement("input");
        cellElement.type = "hidden";
        cellElement.id = "txtActId" + uniqueId;
        cellElement.value = 0;
        cell01.appendChild(cellElement);

        cellElement = document.createElement("textarea");
        cellElement.id = "txtActionable" + uniqueId;
        cellElement.rows = 2;
        cellElement.cols = 30;
        cellElement.maxLength = "5000";
        cellElement.className = "textbox1";
        cell01.className = "tabbody3";
        cell01.appendChild(cellElement);

        //Added By milan yadav on 26-Aug-2016
        //>>
        var cell001 = row.insertCell(2);
        cellElement = document.createElement("input");
        cellElement.type = "text";
        cellElement.id = "txtPersonResponsibleId" + uniqueId;
        cellElement.maxLength = "100";
        cellElement.size = "18";
        cellElement.onchange = Function("populateUserDetsByCode('UserDetsPersonResponsible'," + uniqueId + ")");
        cellElement.className = "textbox1";
        cell001.className = "tabbody3";
        cell001.appendChild(cellElement);


        var cell002 = row.insertCell(3);
        cellElement = document.createElement("input");
        cellElement.type = "text";
        cellElement.id = "txtPersonResponsibleName" + uniqueId;
        cellElement.maxLength = "100";
        cellElement.className = "textbox1";
        cellElement.size = "18";
        cell002.className = "tabbody3";
        cell002.appendChild(cellElement);

        var cell003 = row.insertCell(4);
        cellElement = document.createElement("input");
        cellElement.type = "text";
        cellElement.id = "txtPersonResponsibleEmailId" + uniqueId;
        cellElement.maxLength = "100";
        cellElement.className = "textbox1";
        cellElement.size = "18";
        cell003.className = "tabbody3";
        cell003.appendChild(cellElement);
        //<<
        
        var cell0001 = row.insertCell(5);
        cellElement = document.createElement("input");
        cellElement.type = "text";
        cellElement.id = "txtReportingMgrId" + uniqueId;
        cellElement.maxLength = "100";
        cellElement.size = "18";
        cellElement.onchange = Function("populateUserDetsByCode('UserDetsReprtMgr'," + uniqueId + ")");
        cellElement.className = "textbox1";
        cell0001.className = "tabbody3";
        cell0001.appendChild(cellElement);


        var cell0002 = row.insertCell(6);
        cellElement = document.createElement("input");
        cellElement.type = "text";
        cellElement.id = "txtReportingMgrName" + uniqueId;
        cellElement.maxLength = "100";
        cellElement.className = "textbox1";
        cellElement.size = "18";
        cell0002.className = "tabbody3";
        cell0002.appendChild(cellElement);

        var cell0003 = row.insertCell(7);
        cellElement = document.createElement("input");
        cellElement.type = "text";
        cellElement.id = "txtReportingMgrEmailId" + uniqueId;
        cellElement.maxLength = "100";
        cellElement.className = "textbox1";
        cellElement.size = "18";
        cell0003.className = "tabbody3";
        cell0003.appendChild(cellElement);

        var cell02 = row.insertCell(8);
        cellElement = document.createElement("input");
        cellElement.type = "text";
        cellElement.id = "txtTargetDate" + uniqueId;
        cellElement.maxLength = "11";
        //cellElement.onchange = Function("compareTargetDates('txtTargetDate'," + uniqueId + ")");
        cellElement.size = 11;
        cellElement.className = "textbox1";
        cell02.className = "tabbody3";
        cell02.appendChild(cellElement);

        $("#txtTargetDate" + uniqueId).datepicker({
            showOn: 'button',
            buttonImage: '../../Content/images/legacy/calendar.gif',
           // buttonImage: 'images/calendar.gif',
            buttonImageOnly: true,
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd-M-yy'
        });

        var cell0002 = row.insertCell(9);
        cellElement = document.createElement("input");
        cellElement.type = "text";
        cellElement.id = "txtCompletionDate" + uniqueId;
        cellElement.maxLength = "11";
        cellElement.size = 11;
        cellElement.className = "textbox1";
        cell0002.className = "tabbody3";
        cell0002.appendChild(cellElement);

        $("#txtCompletionDate" + uniqueId).datepicker({
            showOn: 'button',
            buttonImage: '../../Content/images/legacy/calendar.gif',
            buttonImageOnly: true,
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd-M-yy'
        });

        var cell4 = row.insertCell(10);
        cellElement = document.createElement("select");
        cellElement.id = "ddlStatus" + uniqueId;

        //var Status = document.getElementById('hfActionableStatus').value;
        var Status = document.getElementById('ctl00_ContentPlaceHolder1_hfActionableStatus').value;

        var cnt = 0;
        cellElement.options[0] = new Option("(Select)", "");
        var arrRecs = Status.split("~");
        while (cnt < arrRecs.length) {
            Rec = arrRecs[cnt];
            Fields = Rec.split("|");
            cellElement.options[cnt + 1] = new Option(Fields[1], Fields[0]);
            cnt = cnt + 1;
        }
        cellElement.className = "dropdownlist1";
        cell4.className = "tabbody3";
        cell4.appendChild(cellElement);

        var cell03 = row.insertCell(11);
        cellElement = document.createElement("textarea");
        cellElement.id = "txtRemarks" + uniqueId;
        cellElement.maxLength = "100";
        //cellElement.rows = 2;
        //cellElement.cols = 30;
        cellElement.size = 18;
        cellElement.className = "textbox1";
        cell03.className = "tabbody3";
        cell03.appendChild(cellElement);
        cellElement = document.getElementById("txtActionable" + uniqueId);
        cellElement.focus();
        return false;

    } catch (e) {
        alert(e.message);
        return false;
    }
}

function deleteActionablesRow() {
    try {

        var table = document.getElementById("tblActionables");
        var rowCount = table.rows.length;
        var cellElements, cellUniqueRow, uniqueRowId;
        var strUniqueRowId = "", strId = "", strActionableId = "";
        var rowIdFrmTbl;
        var ActionableId = "";
        var deleteFromDB = 'N';
        for (var i = 1; i < rowCount; i++) {
            cellElements = table.rows[i].cells[0].getElementsByTagName("input");
            rowIdchecked = cellElements[1].checked;
            if (rowIdchecked == true) {
                cellUniqueRow = table.rows[i].cells[0].getElementsByTagName("input");
                uniqueRowId = cellUniqueRow[0].value;
                ActionableId = document.getElementById("txtActId" + uniqueRowId).value;
                if (ActionableId != '0') {
                    deleteFromDB = 'Y';
                }
                strActionableId = strActionableId + ActionableId + ",";
                strUniqueRowId = strUniqueRowId + uniqueRowId + ",";
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
            deleteFromHTML("tblActionables", strUniqueRowId);
            if (deleteFromDB == 'Y') {
                window.open("../DeleteDBRecords.aspx?calledFrom=CircularActionables&Ids=" + strActionableId,
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

function deleteFromHTML(strTable, strUniqueRowId) {
    var table = document.getElementById(strTable);
    var rowCount = table.rows.length;
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

function deleteFromHTMLSingle(strTable, uniqueRowId) {
    var table = document.getElementById(strTable);
    var rowCount = table.rows.length;
    var cellElements;
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


function onActionablesHeaderRowChecked() {
    var table = document.getElementById("tblActionables");
    var allCheck = document.getElementById("HeaderLevelCheckBoxActionables");
    var rowCount = table.rows.length;
    var uniqueRowId;
    var check;
    for (var i = 1; i < rowCount; i++) {
        var row = table.rows[i];
        cellElements = table.rows[i].cells[0].getElementsByTagName("input");
        uniqueRowId = cellElements[0].value;
        check = document.getElementById("cbActionables" + uniqueRowId);
        check.checked = allCheck.checked;
    }
}

function getActionableData() {
    try {

        var table = document.getElementById("tblActionables");
        var hiddenFormData = document.getElementById("ctl00_ContentPlaceHolder1_hfActionablesData");
        if (table == null) {
            return;
        }
        var rowCount = table.rows.length;
        var row;
        var actionData = "";
        var uniqueRowId;
        hiddenFormData.value = "";
        for (var i = 1; i < rowCount; i++) {
            row = table.rows[i];
            var cellElements0 = row.cells[0].getElementsByTagName("input");
            uniqueRowId = cellElements0[0].value;
            var actId = document.getElementById('txtActId' + uniqueRowId).value;
            var txtActionableField = document.getElementById('txtActionable' + uniqueRowId);
            var actionable = txtActionableField.value;
            var txtTargetDateField = document.getElementById('txtTargetDate' + uniqueRowId);
            var targetDate = txtTargetDateField.value;
            var txtCompletionDateField = document.getElementById('txtCompletionDate' + uniqueRowId);
            var completionDate = txtCompletionDateField.value;
            var ddlStatusField = document.getElementById('ddlStatus' + uniqueRowId);
            var status = ddlStatusField.value;
            var txtRemarksField = document.getElementById('txtRemarks' + uniqueRowId);
            var remarks = txtRemarksField.value;
            //Added By Milan Yadav on 26-Aug-2016
            //>>
            
            var txtPersonResponsibleId = document.getElementById('txtPersonResponsibleId' + uniqueRowId);
            var PersonResponsibleId = txtPersonResponsibleId.value;
            var txtPersonResponsibleName = document.getElementById('txtPersonResponsibleName' + uniqueRowId);
            var PersonResponsibleName = txtPersonResponsibleName.value;
            var txtPersonResponsibleEmailId = document.getElementById('txtPersonResponsibleEmailId' + uniqueRowId);
            var PersonResponsibleEmailId = txtPersonResponsibleEmailId.value;
            
            var txtReportingMgrId = document.getElementById('txtReportingMgrId' + uniqueRowId);
            var ReportingMgrId = txtReportingMgrId.value;
            
            var txtReportingMgrName = document.getElementById('txtReportingMgrName' + uniqueRowId);
            var ReportingMgrName = txtReportingMgrName.value;
            
            var txtReportingMgrEmailId = document.getElementById('txtReportingMgrEmailId' + uniqueRowId);
            var ReportingMgrEmailId = txtReportingMgrEmailId.value;
            //<<
            actionData = actionData + actId + "|" + actionable + "|" + PersonResponsibleId + "|" + PersonResponsibleName + "|" + PersonResponsibleEmailId
			        + "|" + targetDate + "|" + completionDate + "|" + status + "|" + remarks +  "|" + 
			        ReportingMgrId + "|" + ReportingMgrName + "|" + ReportingMgrEmailId + "~";
            //alert(actionData);       
        }

        if (actionData != "") {
            hiddenFormData.value = actionData;
        }

        return true;
    } catch (e) {
        alert(e.message);
        return false;
    }
}

function addAttachmentRow() {
    window.open("../UploadAnyFile.aspx?type=Circular", "FILE",
			"location=0,status=0,scrollbars=0,width=650,height=350");
    return false;
}

function onAttachmentHeaderRowChecked() {
    var table = document.getElementById("tblAttachment");
    var allCheck = document.getElementById("HeaderLevelCheckBoxAttachment");
    var rowCount = table.rows.length;
    var uniqueRowId;
    var check;
    for (var i = 1; i < rowCount; i++) {
        var row = table.rows[i];
        cellElements = table.rows[i].cells[0].getElementsByTagName("input");
        uniqueRowId = cellElements[0].value;
        check = document.getElementById("checkAttachment" + uniqueRowId);
        check.checked = allCheck.checked;
    }
}

function deleteAttachmentRow() {
    try {
        var table = document.getElementById("tblAttachment");
        var rowCount = table.rows.length;
        var cellElements, cellUniqueRow, uniqueRowId;
        var strUniqueRowId = "", strId = "", strAttachId = "";
        var rowIdFrmTbl;
        var AttachId = "";
        var deleteFromDB = 'N';

        for (var i = 1; i < rowCount; i++) {
            cellElements = table.rows[i].cells[0].getElementsByTagName("input");
            rowIdchecked = cellElements[2].checked;
            if (rowIdchecked == true) {
                cellUniqueRow = table.rows[i].cells[0].getElementsByTagName("input");
                uniqueRowId = cellUniqueRow[0].value;
                AttachId = document.getElementById("attachId" + uniqueRowId).value;
                //			    DeleteFromDb should always be equal to Y as files are already uploaded
                //              on the server file system.    
                //			    if(AttachId != '0')
                //			    {
                //			        deleteFromDB = 'Y';
                //			    }
                deleteFromDB = 'Y';
                strAttachId = strAttachId + AttachId + ",";
                strUniqueRowId = strUniqueRowId + uniqueRowId + ",";
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
            deleteFromHTML("tblAttachment", strUniqueRowId);
            if (deleteFromDB == 'Y') {
                window.open("../DeleteDBRecords.aspx?calledFrom=CircularAttachment&Ids=" + strAttachId,
			    "FILE", "location=0,status=0,scrollbars=0,width=10,height=10");
            }
            return false;
        }
    }
    catch (e) {
        alert(e);
        return false;
    }
}

function getAttachmentData() {
    try {
        var table = document.getElementById("tblAttachment");
        var hiddenFormData = document.getElementById("ctl00_ContentPlaceHolder1_hfAttachment");
        if (table == null) {
            return;
        }
        var rowCount = table.rows.length;
        var row;
        var attachmentData = "";
        var uniqueRowId;
        hiddenFormData.value = "";

        for (var i = 1; i < rowCount; i++) {
            row = table.rows[i];
            var cellElements0 = row.cells[0].getElementsByTagName("input");
            uniqueRowId = cellElements0[0].value;
            //alert(uniqueRowId);
            //var AttachmentId = document.getElementById('txtPolicyDetsId' + uniqueRowId).value;
            var AttachmentIdField = document.getElementById('attachId' + uniqueRowId);
            var AttachmentNo = AttachmentIdField.value;
            var attachClientFileNameField = document.getElementById('attachClientFileName' + uniqueRowId);
            var AttachClientFileName = attachClientFileNameField.value;
            var attachServerFileNameField = document.getElementById('attachServerFileName' + uniqueRowId);
            var AttachServerFileName = attachServerFileNameField.value;
            attachmentData = attachmentData + AttachmentNo + "|" + AttachClientFileName + "|" + AttachServerFileName + "~";
            //alert(attachmentData);
        }

        if (attachmentData != "") {
            hiddenFormData.value = attachmentData;
        }

        return true;
    }
    catch (e) {
        alert(e.message);
        return false;
    }
}

function onAppAttachUploaded(serverfilename, clientfilename) {
    try {
        var table = document.getElementById('tblAttachment');
        var rowCount = table.rows.length;
        var idx = rowCount - 1;
        var uniqueId = 0;
        var cellElements;

        //check if there are no rows in the table.
        if (idx != 0) {

            cellElements = table.rows[idx].cells[0]
					.getElementsByTagName("input");
            uniqueId = cellElements[0].value;
            uniqueId = parseInt(uniqueId) + 1;
        } else {
            //alert("no records in table");
        }

        var row = table.insertRow(rowCount);
        var cell0 = row.insertCell(0);

        var cellElement = document.createElement("input");
        cellElement.type = "hidden";
        cellElement.id = "uniqueRowId" + uniqueId;
        cellElement.value = uniqueId;
        cell0.appendChild(cellElement);

        cellElement = document.createElement("input");
        cellElement.type = "hidden";
        cellElement.id = "attachId" + uniqueId;
        cellElement.value = 0;
        cell0.appendChild(cellElement);

        cellElement = document.createElement("input");
        cellElement.type = "checkbox";
        cellElement.id = "checkAttachment" + uniqueId;
        cellElement.value = '0';
        cell0.className = "tabbody3";
        cell0.appendChild(cellElement);

        var cell1 = row.insertCell(1);

        cellElement = document.createElement("input");
        cellElement.type = "hidden";
        cellElement.id = "attachClientFileName" + uniqueId;
        cellElement.value = clientfilename;
        cell1.appendChild(cellElement);

        cellElement = document.createElement("input");
        cellElement.type = "hidden";
        cellElement.id = "attachServerFileName" + uniqueId;
        cellElement.value = serverfilename;
        cell1.appendChild(cellElement);

        cellElement = document.createElement("a");
        cellElement.id = "attachfilelink" + uniqueId;
        cellElement.innerHTML = clientfilename;
        cellElement.href = "../CommonDownload.aspx?type=Circular&downloadFileName=" + escape(serverfilename);
        //>>
        cell1.className = "tabbody3";
        cell1.appendChild(cellElement);

    } catch (e) {
        alert(e.message);
    }
}
function EditActionableDets() {
    try {
        var hiddenFormData = document.getElementById("ctl00_ContentPlaceHolder1_hfActionablesData");
        var row;
        var actionData = "";
        var uniqueRowId;
        hiddenFormData.value = "";
        var msgtext = "";
        var systemdate = document.getElementById("ctl00_ContentPlaceHolder1_hfCurDate");
        var ddlDepartmentField = document.getElementById("ctl00_ContentPlaceHolder1_fvCircularMaster_ddlDepartment");
        var ddlCircularAuthorityField = document.getElementById("ctl00_ContentPlaceHolder1_fvCircularMaster_ddlCircularAuthority");
        var ddlAreaField = document.getElementById("ctl00_ContentPlaceHolder1_fvCircularMaster_ddlArea");
        var txtCircularNoField = document.getElementById("ctl00_ContentPlaceHolder1_fvCircularMaster_txtCircularNo");
        var txtCircularDateField = document.getElementById("ctl00_ContentPlaceHolder1_fvCircularMaster_txtCircularDate");
        var txtTopicField = document.getElementById("ctl00_ContentPlaceHolder1_fvCircularMaster_txtTopic");
        var ddlTypeofCircular = document.getElementById("ctl00_ContentPlaceHolder1_fvCircularMaster_ddlTypeofCircular");
        //Added By Milan Yadav on 26-Aug-2016
        //>>
        var ddlSpocFromCompFn = document.getElementById("ctl00_ContentPlaceHolder1_fvCircularMaster_ddlSpocFromCompFn");
        //<<

        if (!required(ddlDepartmentField)) {
            msgtext = msgtext + bullet
                    + 'Please select Created by. \n';
            setfocuson(ddlDepartmentField);
        }
        if (!required(ddlCircularAuthorityField)) {
            msgtext = msgtext + bullet
					+ 'Please select Issuing Authority. \n';
            setfocuson(ddlCircularAuthorityField);
        }

        if (!required(ddlTypeofCircular)) {
            msgtext = msgtext + bullet
					+ 'Please select Type of Document. \n';
            setfocuson(ddlTypeofCircular);
        }

        if (!required(ddlAreaField)) {
            msgtext = msgtext + bullet
					+ 'Please select Topic. \n';
            setfocuson(ddlAreaField);
        }
        if (!required(ddlSpocFromCompFn)) {
            msgtext = msgtext + bullet
					+ 'Please select SPOC From Compliance Function. \n';
            setfocuson(ddlSpocFromCompFn);
        }

        if (!required(txtCircularNoField)) {
            msgtext = msgtext + bullet
					+ 'Please enter Circular No. \n';
            setfocuson(txtCircularNoField);
        }
        if (!required(txtTopicField)) {
            msgtext = msgtext + bullet
					+ 'Please enter subject of the Circular. \n';
            setfocuson(txtTopicField);
        }
        if (!required(txtCircularDateField)) {
            msgtext = msgtext + bullet
					+ 'Please enter Circular Date. \n';
            setfocuson(txtCircularDateField);
        }
        if (!checkDateIfEntered(txtCircularDateField)) {
            msgtext = msgtext + bullet + 'Circular Date format should be dd-mon-yyyy. \n';
            setfocuson(txtCircularDateField);
        }
        //var table = document.getElementById("tblActionables");
        //var rowCount = table.rows.length;
        //		if(rowCount < 2){
        //		    msgtext = msgtext + bullet
        //					+ 'Please enter at least one actionable. \n';
        //		}
        //for (var i = 1; i < rowCount; i++) {

        //    row = table.rows[i];
        //    var cellElements0 = row.cells[0].getElementsByTagName("input");
        //    uniqueRowId = cellElements0[0].value;
        //    var actId = document.getElementById('txtActId' + uniqueRowId).value;
        //    var txtActionableField = document.getElementById('txtActionable' + uniqueRowId);
        //    var actionable = txtActionableField.value;
        //    //var ddlPersonResponsibleField = document.getElementById('ddlPersonResponsible' + uniqueRowId);
        //    //var personResp = ddlPersonResponsibleField.value;
        //    var txtTargetDateField = document.getElementById('txtTargetDate' + uniqueRowId);
        //    var targetDate = txtTargetDateField.value;
        //    var txtCompletionDateField = document.getElementById('txtCompletionDate' + uniqueRowId);
        //    var completionDate = txtCompletionDateField.value;
        //    var ddlStatusField = document.getElementById('ddlStatus' + uniqueRowId);
        //    var status = ddlStatusField.value;
        //    var txtRemarksField = document.getElementById('txtRemarks' + uniqueRowId);
        //    var remarks = txtRemarksField.value;

        //    //Added By Milan Yadav on 26-Aug-2016
        //    //>>
        //    var txtPersonResponsibleId = document.getElementById('txtPersonResponsibleId' + uniqueRowId);
        //    var PersonResponsibleId = txtPersonResponsibleId.value;
        //    var txtPersonResponsibleName = document.getElementById('txtPersonResponsibleName' + uniqueRowId);
        //    var PersonResponsibleName = txtPersonResponsibleName.value;
        //    var txtPersonResponsibleEmailId = document.getElementById('txtPersonResponsibleEmailId' + uniqueRowId);
        //    var PersonResponsibleEmailId = txtPersonResponsibleEmailId.value;
        //    //<<
            
        //    var txtReportingMgrId = document.getElementById('txtReportingMgrId' + uniqueRowId);
        //    var ReportingMgrId = txtReportingMgrId.value;
            
        //    var txtReportingMgrName = document.getElementById('txtReportingMgrName' + uniqueRowId);
        //    var ReportingMgrName = txtReportingMgrName.value;
            
        //    var txtReportingMgrEmailId = document.getElementById('txtReportingMgrEmailId' + uniqueRowId);
        //    var ReportingMgrEmailId = txtReportingMgrEmailId.value;

        //    actionData = actionData + actId + "|" + actionable + "|" + PersonResponsibleId + "|" + PersonResponsibleName + "|" + PersonResponsibleEmailId
		//	        + "|" + targetDate + "|" + completionDate + "|" + status + "|" + remarks + "|" + 
		//	        ReportingMgrId + "|" + ReportingMgrName + "|" + ReportingMgrEmailId + "~";
			        
        //    if (!required(txtActionableField)) {
        //        msgtext = msgtext + bullet
		//				+ 'Please enter Actionable for Actionable Grid Row ' + i
		//				+ '\n';
        //        setfocuson(txtActionableField);
        //    }
        //    if (txtActionableField.value.length > 5000) {
        //        msgtext = msgtext + bullet
		//                    + 'Actionable cannot exceed 5000 characters for Actionable Grid Row ' + i
		//				+ '\n';
        //        setfocuson(txtActionableField);
        //    }

        //    //if (!required(ddlPersonResponsibleField)) {
        //    //	msgtext = msgtext + bullet
        //    //			+ 'Please select Person responsible for Actionable Grid Row ' + i
        //    //			+ '\n';
        //    //	setfocuson(ddlPersonResponsibleField);
        //    //}
        //    if (!required(txtPersonResponsibleId)) {
        //        msgtext = msgtext + bullet
		//    			+ 'Please Enter Person responsible User Id for Actionable Grid Row ' + i
		//    			+ '\n';
        //        setfocuson(txtPersonResponsibleId);
        //    }
        //    if (!required(txtPersonResponsibleName)) {
        //        msgtext = msgtext + bullet
		//    			+ 'Please Enter Person responsible User Name for Actionable Grid Row ' + i
		//    			+ '\n';
        //        setfocuson(txtPersonResponsibleName);
        //    }
        //    if (!required(txtPersonResponsibleEmailId)) {
        //        msgtext = msgtext + bullet
		//    			+ 'Please Enter Person responsible User Email Id for Actionable Grid Row ' + i
		//    			+ '\n';
        //        setfocuson(txtPersonResponsibleEmailId);
        //    }
            
        //    if (!required(txtReportingMgrId)) {
        //        msgtext = msgtext + bullet
		//    			+ 'Please Enter Reporting Manager User Id for Actionable Grid Row ' + i
		//    			+ '\n';
        //        setfocuson(txtReportingMgrId);
        //    }
        //    if (!required(txtReportingMgrName)) {
        //        msgtext = msgtext + bullet
		//    			+ 'Please Enter Reporting Manager Name for Actionable Grid Row ' + i
		//    			+ '\n';
        //        setfocuson(txtReportingMgrName);
        //    }
        //    if (!required(txtReportingMgrEmailId)) {
        //        msgtext = msgtext + bullet
		//    			+ 'Please Enter Reporting Manager User Email Id for Actionable Grid Row ' + i
		//    			+ '\n';
        //        setfocuson(txtReportingMgrEmailId);
        //    }

        //    if (!required(txtTargetDateField))
        //    {
        //        msgtext = msgtext + bullet
		//				+ 'Please enter Target Date for Actionable Grid Row ' + i
		//				+ '\n';
        //        setfocuson(txtTargetDateField);
        //    }
        //    else if (!checkDateIfEntered(txtTargetDateField))
        //    {
        //        msgtext = msgtext + bullet + 'Target Date format should be dd-mon-yyyy for Actionable Grid Row ' + i
		//				+ '\n';
        //        setfocuson(txtTargetDateField);
        //    }
        //    else if (compare2Dates(systemdate, txtTargetDateField) == 0)
        //    {
        //        //<< If condition added Archana Gosavi on 05-May-2017
        //        //<< So that validation shall be fired only for the new records and not for the fast records
        //        if (actId == 0)
        //        {
        //            msgtext = msgtext + bullet
        //                + 'Target Date has to be greater than or equal to System Date for Actionable Grid Row ' + i
        //                + '\n';
        //            setfocuson(txtTargetDateField);
        //        }
        //    }
        //    if (!checkDateIfEntered(txtCompletionDateField))
        //    {

        //        msgtext = msgtext + bullet + 'Completion Date format should be dd-mon-yyyy for Actionable Grid Row ' + i
		//				+ '\n';
        //        setfocuson(txtCompletionDateField);
        //    }
        //    if (!required(ddlStatusField)) {
        //        msgtext = msgtext + bullet
		//				+ 'Please select status for Actionable Grid Row ' + i
		//				+ '\n';
        //        setfocuson(ddlStatusField);
        //    }

        //}
        if (msgtext != "") {
            alert(msgtext);
            if (focuson != null) {
                focuson.focus();
            }
            return false;
        }
        else {
            //getActionableData();
            getAttachmentData();
            return true;
        }
        return true;
    } catch (e) {
        alert(e.message);
        return false;

    }
}

function addActionableDets() {
    try {

        var hiddenFormData = document.getElementById("ctl00_ContentPlaceHolder1_hfActionablesData");
        var row;
        var actionData = "";
        var uniqueRowId;
        hiddenFormData.value = "";
        var msgtext = "";
        var systemdate = document.getElementById("ctl00_ContentPlaceHolder1_hfCurDate");

        var ddlDepartmentField = document.getElementById("ctl00_ContentPlaceHolder1_ddlDepartment");
        var ddlCircularAuthorityField = document.getElementById("ctl00_ContentPlaceHolder1_ddlCircularAuthority");
        var ddlAreaField = document.getElementById("ctl00_ContentPlaceHolder1_ddlArea");
        var txtCircularNoField = document.getElementById("ctl00_ContentPlaceHolder1_txtCircularNo");
        var txtCircularDateField = document.getElementById("ctl00_ContentPlaceHolder1_txtCircularDate");
        var txtTopicField = document.getElementById("ctl00_ContentPlaceHolder1_txtTopic");
        var ddlTypeofCircular = document.getElementById("ctl00_ContentPlaceHolder1_ddlTypeofCircular");
        //Added By Milan Yadav on 26-Aug-2016
        //>>
        var ddlSpocFromCompFn = document.getElementById("ctl00_ContentPlaceHolder1_ddlSpocFromCompFn");
        //<<

        if (!required(ddlDepartmentField)) {
            msgtext = msgtext + bullet
                    + 'Please select Creted by. \n';
            setfocuson(ddlDepartmentField);
        }
        if (!required(ddlCircularAuthorityField)) {
            msgtext = msgtext + bullet
					+ 'Please select Issuing Authority. \n';
            setfocuson(ddlCircularAuthorityField);
        }

        if (!required(ddlTypeofCircular)) {
            msgtext = msgtext + bullet
					+ 'Please select Type of Document. \n';
            setfocuson(ddlTypeofCircular);
        }

        if (!required(ddlAreaField)) {
            msgtext = msgtext + bullet
					+ 'Please select Topic. \n';
            setfocuson(ddlAreaField);
        }
        if (!required(txtCircularNoField)) {
            msgtext = msgtext + bullet
					+ 'Please enter Circular No. \n';
            setfocuson(txtCircularNoField);
        }
        if (!required(txtTopicField)) {
            msgtext = msgtext + bullet
					+ 'Please enter subject of the Circular. \n';
            setfocuson(txtTopicField);
        }
        if (!required(ddlSpocFromCompFn)) {
            msgtext = msgtext + bullet
					+ 'Please select SPOC From Compliance Function. \n';
            setfocuson(ddlSpocFromCompFn);
        }
        if (!required(txtCircularDateField)) {
            msgtext = msgtext + bullet
					+ 'Please enter Circular Date. \n';
            setfocuson(txtCircularDateField);
        }
        if (!checkDateIfEntered(txtCircularDateField)) {
            msgtext = msgtext + bullet + 'Circular Date format should be dd-mon-yyyy. \n';
            setfocuson(txtCircularDateField);
        }
        //var table = document.getElementById("tblActionables");
        //var rowCount = table.rows.length;

        //for (var i = 1; i < rowCount; i++) {

        //    row = table.rows[i];
        //    var cellElements0 = row.cells[0].getElementsByTagName("input");
        //    uniqueRowId = cellElements0[0].value;
        //    var actId = document.getElementById('txtActId' + uniqueRowId).value;
        //    var txtActionableField = document.getElementById('txtActionable' + uniqueRowId);
        //    var actionable = txtActionableField.value;

        //    //var ddlPersonResponsibleField = document.getElementById('ddlPersonResponsible' + uniqueRowId);********
        //    //var personResp = ddlPersonResponsibleField.value;

        //    var txtTargetDateField = document.getElementById('txtTargetDate' + uniqueRowId);
        //    var targetDate = txtTargetDateField.value;
        //    var txtCompletionDateField = document.getElementById('txtCompletionDate' + uniqueRowId);
        //    var completionDate = txtCompletionDateField.value;
        //    var ddlStatusField = document.getElementById('ddlStatus' + uniqueRowId);
        //    var status = ddlStatusField.value;
        //    var txtRemarksField = document.getElementById('txtRemarks' + uniqueRowId);
        //    var remarks = txtRemarksField.value;
        //    //Added By Milan Yadav on 26-Aug-2016
        //    //>>
        //    //debugger;
        //    var txtPersonResponsibleId = document.getElementById('txtPersonResponsibleId' + uniqueRowId);
        //    var PersonResponsibleId = txtPersonResponsibleId.value;

        //    var txtPersonResponsibleName = document.getElementById('txtPersonResponsibleName' + uniqueRowId);
        //    var PersonResponsibleName = txtPersonResponsibleName.value;
        //    var txtPersonResponsibleEmailId = document.getElementById('txtPersonResponsibleEmailId' + uniqueRowId);
        //    var PersonResponsibleEmailId = txtPersonResponsibleEmailId.value;
            
        //    var txtReportingMgrId = document.getElementById('txtReportingMgrId' + uniqueRowId);
        //    var ReportingMgrId = txtReportingMgrId.value;
            
        //    var txtReportingMgrName = document.getElementById('txtReportingMgrName' + uniqueRowId);
        //    var ReportingMgrName = txtReportingMgrName.value;
            
        //    var txtReportingMgrEmailId = document.getElementById('txtReportingMgrEmailId' + uniqueRowId);
        //    var ReportingMgrEmailId = txtReportingMgrEmailId.value;

            
            
            
        //    //alert("PersonResponsibleId" + PersonResponsibleId + "PersonResponsibleName" + PersonResponsibleName + "PersonResponsibleEmailId" + PersonResponsibleEmailId);
        //    //<<
           
        //    actionData = actionData + actId + "|" + actionable + "|" + PersonResponsibleId + "|" + PersonResponsibleName + "|" + PersonResponsibleEmailId
		//	        + "|" + targetDate + "|" + completionDate + "|" + status
		//	         + "|" + remarks +  
		//	        ReportingMgrId + "|" + ReportingMgrName + "|" + ReportingMgrEmailId + "~";;

        //    if (!required(txtActionableField)) {
        //        msgtext = msgtext + bullet
		//				+ 'Please enter actionable for Actionable Grid Row ' + i
		//				+ '\n';
        //        setfocuson(txtActionableField);
        //    }
        //    if (txtActionableField.value.length > 5000) {
        //        msgtext = msgtext + bullet
		//                    + 'Actionable cannot exceed 5000 characters for Actionable Grid Row ' + i
		//				+ '\n';
        //        setfocuson(txtActionableField);
        //    }

        //    if (!required(txtPersonResponsibleId)) {
        //        msgtext = msgtext + bullet
		//				+ 'Please enter Person responsible User Id for Actionable Grid Row ' + i
		//				+ '\n';
        //        setfocuson(txtPersonResponsibleId);
        //    }
        //    if (!required(txtPersonResponsibleName)) {
        //        msgtext = msgtext + bullet
		//				+ 'Please enter Person responsible User Name for Actionable Grid Row ' + i
		//				+ '\n';
        //        setfocuson(txtPersonResponsibleName);
        //    }
        //    if (!required(txtPersonResponsibleEmailId)) {
        //        msgtext = msgtext + bullet
		//				+ 'Please enter Person responsible User Email Id for Actionable Grid Row ' + i
		//				+ '\n';
        //        setfocuson(txtPersonResponsibleEmailId);
        //    }
            
        //     if (!required(txtReportingMgrId)) {
        //        msgtext = msgtext + bullet
		//    			+ 'Please Enter Reporting Manager User Id for Actionable Grid Row ' + i
		//    			+ '\n';
        //        setfocuson(txtReportingMgrId);
        //    }
        //    if (!required(txtReportingMgrName)) {
        //        msgtext = msgtext + bullet
		//    			+ 'Please Enter Reporting Manager Name for Actionable Grid Row ' + i
		//    			+ '\n';
        //        setfocuson(txtReportingMgrName);
        //    }
        //    if (!required(txtReportingMgrEmailId)) {
        //        msgtext = msgtext + bullet
		//    			+ 'Please Enter Reporting Manager User Email Id for Actionable Grid Row ' + i
		//    			+ '\n';
        //        setfocuson(txtReportingMgrEmailId);
        //    }

        //    if (!required(txtTargetDateField)) {
        //        msgtext = msgtext + bullet
		//				+ 'Please enter Target Date for Actionable Grid Row ' + i
		//				+ '\n';
        //        setfocuson(txtTargetDateField);
        //    }
        //    else if (!checkDateIfEntered(txtTargetDateField)) {
        //        msgtext = msgtext + bullet + 'Target Date format should be dd-mon-yyyy for Actionable Grid Row ' + i
		//				+ '\n';
        //        setfocuson(txtTargetDateField);
        //    }           
        //    else if (compare2Dates(systemdate, txtTargetDateField) == 0)
        //        {
        //        msgtext = msgtext + bullet
        //            + 'Target Date has to be greater than or equal to System Date for Actionable Grid Row ' + i
        //            + '\n';
        //        setfocuson(txtTargetDateField);
        //    }

        //    if (!checkDateIfEntered(txtCompletionDateField)) {

        //        msgtext = msgtext + bullet + 'Completion Date format should be dd-mon-yyyy for Actionable Grid Row ' + i
		//				+ '\n';
        //        setfocuson(txtCompletionDateField);
        //    }
        //    if (!required(ddlStatusField)) {
        //        msgtext = msgtext + bullet
		//				+ 'Please select status for Actionable Grid Row ' + i
		//				+ '\n';
        //        setfocuson(ddlStatusField);
        //    }
        //    //			
        //}
        if (msgtext != "") {
            alert(msgtext);
            if (focuson != null) {
                focuson.focus();
            }
            return false;
        }
        else {
            //getActionableData();
            getAttachmentData();
            return true;
        }
        return true;
    } catch (e) {
        alert(e.message);
        return false;

    }
}

