// JScript File

//checked records delete
var focuson = null;


function setfocuson(obj) {
	if (focuson == null) {
		focuson = obj;
	}
}

//function addExceptionRow() {
//	try {
//		var uniqueId = 0;
//		var cellElements;
//		var table = document.getElementById('tblException');
//		
//		var rowCount = table.rows.length;
//		var idx = rowCount - 1;

//		if (idx != 0) 
//		{
//			cellElements = table.rows[idx].cells[0]
//					.getElementsByTagName("input");
//					
//			uniqueId = cellElements[0].value;
//			uniqueId = parseInt(uniqueId) + 1;
//		}
//		
//		var row = table.insertRow(rowCount);
//		
//		var cell0 = row.insertCell(0);
//		var cellElement = document.createElement("input");
//		cellElement.type = "hidden";
//		cellElement.id = "uniqueRowId" + uniqueId;
//		cellElement.value = uniqueId;
//		cell0.appendChild(cellElement);
//		
//        cellElement = document.createElement("input");
//		cellElement.type = "hidden";
//		cellElement.id = "certId" + uniqueId;
//		cellElement.value = '0';
//		cell0.appendChild(cellElement);

//		var cellElement = document.createElement("input");
//		cellElement.type = "checkbox";
//		cellElement.id = "cbException" + uniqueId;
//		cell0.className = "tabbody3";
//		cell0.appendChild(cellElement);
//       
//		var cell01 = row.insertCell(1);
//		cellElement = document.createElement("a");
//		cellElement.id = "AttachFileImg" + uniqueId;
//		cellElement.innerHTML = '<img border="0" src="../../Content/images/legacy/attach.png" />';
//		cellElement.href = "#";
//		cellElement.onclick = Function("return openpopupExceptionAttachments("
//				+ uniqueId + ")");
//		cell01.className = "tabbody3";
//		cell01.appendChild(cellElement);
//		
//		cellElement = document.createElement("a");
//		cellElement.id = "DeleteFileImg" + uniqueId;
//		cellElement.innerHTML = '<img border="0" src="../../Content/images/legacy/delete.gif" />';
//		cellElement.href = "#";
//		cellElement.onclick = Function("return deleteExceptionFile("
//				+ uniqueId + ")");

//		cellElement.style.visibility = "hidden";
//		cell01.appendChild(cellElement);
//		
//		var cell02 = row.insertCell(2);
//		cellElement = document.createElement("input");
//		cellElement.type = "hidden";
//		cellElement.id = "ClientFileName" + uniqueId;
//		cell02.appendChild(cellElement);
//		cellElement = document.createElement("input");
//		cellElement.type = "hidden";
//		cellElement.id = "ServerFileName" + uniqueId;
//		cell02.appendChild(cellElement);
//		cellElement = document.createElement("a");
//		cellElement.id = "Filelink" + uniqueId;
//		//cellElement.innerHTML = clientfilename;
//		cellElement.href = "#"; 
//		cell02.className = "tabbody3";
//		cell02.appendChild(cellElement);

//		var cell03 = row.insertCell(3);
//		cellElement = document.createElement("input");
//		cellElement.type = "text";
//		cellElement.id = "ExceptionType" + uniqueId;
//		cellElement.maxLength = "100";
//		cellElement.className = "textbox1";
//		cellElement.size = '50';
//		cell03.className = "tabbody3";
//		cell03.appendChild(cellElement);
//       
//		var cell04 = row.insertCell(4);
//		cellElement = document.createElement("textarea");
//		cellElement.id = "Details" + uniqueId;
//		cellElement.maxLength = "500";
//		cellElement.rows = 3;
//		cellElement.cols = 50;
//		cellElement.className = "textbox1";
//		cell04.className = "tabbody3";
//		cell04.appendChild(cellElement);
//		
//		document.getElementById("ExceptionType" + uniqueId).focus();
//		
//		return false;

//	} catch (e) {
//		alert(e.message);
//		return false;
//	}
//}

//function deleteExceptionFile(rowNo) {
//    //alert('123');
//	var filename = document.getElementById("ServerFileName" + rowNo).value;	
//	//alert(filename);
//	var certId = document.getElementById("certId" + rowNo).value;	
//	//alert(certId);
//	if (!confirm('Are you sure that you want to delete this record?'))return false;
//	window.open(
//			"../DeleteFile.aspx?calledFrom=Exception&fileId="+certId+"&rowNo="
//					+ rowNo + "&filename=" + filename,
//			"FILE", "location=0,status=0,scrollbars=0,width=10,height=10");
//	//alert('123456');
//	return false;
//}

function openpopupExceptionAttachments(rowNo) 
{    
	window.open("../UploadFile.aspx?rowNo="+rowNo,"FILE",
			"location=0,status=0,scrollbars=0,width=400,height=100");
	return false;
}

function deleteExceptionRow() {
	try {
	
		var table = document.getElementById("tblException");
		var rowCount = table.rows.length;
		var cellElements, cellUniqueRow, uniqueRowId;
		var strUniqueRowId = "", strId = "";
		var certId="";
		var rowIdFrmTbl;
		var deleteFromDB = 'N';
		for (var i = 1; i < rowCount; i++) {
			cellElements = table.rows[i].cells[0].getElementsByTagName("input");
			rowIdFrmTbl = cellElements[1].value;
			rowIdchecked = cellElements[2].checked;
			if (rowIdchecked == true) 
			{			
			    cellUniqueRow = table.rows[i].cells[0].getElementsByTagName("input");
			    uniqueRowId = cellUniqueRow[0].value;
			    
			    certId = document.getElementById("certId"+uniqueRowId).value;
			    if(certId != '0')
			    {
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
		    if(deleteFromDB == 'Y')
		    {
		        window.open("../DeleteDBRecords.aspx?calledFrom=Exception&Ids="+strId,
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


function deleteFromHTML(strUniqueRowId)
{
    var table = document.getElementById("tblException");
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

function getExceptionDetails() {
	try {
	    
        var table = document.getElementById("tblException");
		var hiddenFormData = document.getElementById("ctl00_ContentPlaceHolder1_hfExceptions");
		
		if (table == null) 
		{
		    return;
		}
		
		var rowCount = table.rows.length;
		var row;
		var ExceptionNames="";
		var certID, uniqueRowId, cellElements0;
		var exceptionType;
		var details;
		var clientFileName, serverFileName;
		var errMsg ="";
		hiddenFormData.value = "";
	    
	    
		for ( var i = 1; i < rowCount; i++) {
			row = table.rows[i];
			var cellElements0 = row.cells[0].getElementsByTagName("input");
			uniqueRowId = cellElements0[0].value;
			certID = document.getElementById('certId' + uniqueRowId).value;
			exceptionTypeField = document.getElementById('ExceptionType' + uniqueRowId)
			exceptionType = exceptionTypeField.value;
			if(exceptionType == "")
			{
			    errMsg = errMsg + "Particulars is mandatory for grid row no." +i+ "\n";
			    setfocuson(exceptionTypeField);
			}
			details = document.getElementById('Details' + uniqueRowId).value;
			clientFileName = document
					.getElementById('ClientFileName' + uniqueRowId).value;
					
			serverFileName = document
					.getElementById('ServerFileName' + uniqueRowId).value;
					
			ExceptionNames = ExceptionNames + "|" + certID + "|" + exceptionType 
			        + "|" + details + "|" + clientFileName + "|" + serverFileName + "~";
		}
		
		if (ExceptionNames != "") 
		{
			hiddenFormData.value = ExceptionNames;
		}
		
		if(errMsg == '')	
		{	

            
            if (!confirm("Are you sure you want to save draft this certificate?"))
            {
                return false;
            }
            return true;
		}
		else
		{
		    alert(errMsg);
		    return false;
		}
	} catch (e) {
		return false;
		alert(e.message);
	}
}

function getExceptionDetailsOnSave() 
{
	try 
	{
		var row;
		var ExceptionNames="";
		var certID, uniqueRowId, cellElements0;
		var exceptionType;
		var details;
		var clientFileName, serverFileName;
		var errMsg ="";
		//hiddenFormData.value = "";
	    
	    var cnt=0;
		var rblYesNoNA0, rblYesNoNA1, rblYesNoNA2, rblYesNoNA3, txtRemarks,txtTargetDate,txtActionPlan;
	
		var grid = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist");
		
		if(grid != null)
		{
            if (grid.rows.length > 0) 
            {
                for (var j = 2; j < grid.rows.length + 1; j++) 
                {
                    cnt++;
                    if (j < 10)
                    {
                        j = "0" + j
                    } else 
                    {
                        j = j
                    } 
                    rblYesNoNA0 = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_rbyesnona" + '_0');                
                    rblYesNoNA1 = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_rbyesnona" + '_1');
                    rblYesNoNA2 = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_rbyesnona" + '_2');
                    rblYesNoNA3 = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_rbyesnona" + '_3');
                    
                    txtRemarks = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_txtRemarks");       
                    //Added By Milan Yadav on 23-Jun-2016
                    //<<
                    txtActionPlan = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_txtActionPlan") ;
                    txtTargetDate = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_txtTargetDate") ;
                    //>>
                    if (!rblYesNoNA0.checked && !rblYesNoNA1.checked && !rblYesNoNA2.checked && !rblYesNoNA3.checked)
                    {                    
                        errMsg = errMsg  + bullet
                                + 'Please select Compliance Status for Checklist Grid Row ' + cnt
                                +'.\n';                                        
                    }
                    
                    if(rblYesNoNA1.checked || rblYesNoNA2.checked || rblYesNoNA3.checked)
                    {
                         if (!required(txtRemarks))
                         {                                                 
		                    errMsg = errMsg  + bullet                                                
				                + 'Please enter Remarks for Checklist Grid Row ' + cnt
				                +'.\n';  
                        }
                       
                    }
                    //Added By Milan Yadav on 23-Jun-2016
                    //<<
                    if(rblYesNoNA1.checked || rblYesNoNA3.checked)
                    {
                        if (!required(txtActionPlan))
                         {
                            errMsg = errMsg + bullet
                                    + 'Please Enter Action Plan for Checklist Grid Row '+ cnt
                                    +'.\n';
                         }
                         
                         if (!required(txtTargetDate))
                         {
                            errMsg = errMsg + bullet
                                    + 'Please Enter Target Date for Checklist Grid Row '+ cnt
                                    +'.\n';
                         }
                    }
                    //>>
                    
                    if(txtRemarks.value.length > 1000)
                    {
                        errMsg = errMsg + bullet
                                    + 'Remarks cannot exceed 1000 characters for Checklist Grid Row ' + cnt
                                    +'.\n';                                        
                        setfocuson(txtRemarks);
                    }             
                }
            }
        }

		if(errMsg == '')	
		{	
            //if (!confirm("Are you sure you want to submit this certificate?"))
            //{
            //    return false;
            //}
            return true;
		}
		else
		{
		    //<< Added by Archana Gosavi on 15-Mar-2017
		    errMsg = 'Please Go to the "Consolidated Checklist (Editable)" tab to '+
		    'correct the checklist as per the following validation message (if Any).\n' 
		    + errMsg;
		    alert(errMsg);
		    //>>
		    return false;
		}
	} 
	catch (e) 
	{
		return false;
		alert(e.message);
	}
}

function onHeaderRowChecked() 
{
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

// << Commented by Bhavik @04Aug2014
function onIAgreeChecked()
{
    var ChecklistStatus =  document.getElementById("ctl00_ContentPlaceHolder1_hfSeeChecklistStatus");
    var chkIAgree = document.getElementById("ctl00_ContentPlaceHolder1_chkIAgree");
	if(chkIAgree.checked)
	{
	    if (ChecklistStatus.value == "")
        {
            alert("Please view the key regulatory checklist before checking the 'I Agree...' checkbox.");
            return false;
        }
	}
}
// >>



