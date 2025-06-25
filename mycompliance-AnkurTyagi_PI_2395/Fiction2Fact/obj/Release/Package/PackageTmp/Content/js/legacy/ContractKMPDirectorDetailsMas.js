var bullet = '- ';

function addKMPMembersDetsRow() {
	try {

	    var uniqueId = 0;
		var cellElements;
		var table = document.getElementById('tblKMPMembersDets');
		var rowCount = table.rows.length;
		var idx = rowCount - 1;

		if (idx != 0) 
		{
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
		cellElement.id = "cbMembersDets" + uniqueId;
		cell0.className = "tabbody3";
		cell0.appendChild(cellElement);
       
        var cell1 = row.insertCell(1);
		
		var cellElement = document.createElement("input");
		cellElement.type = "hidden";
		cellElement.id = "txtMembersDetsId" + uniqueId;
		cellElement.value = 0;
		cell1.appendChild(cellElement);
		
        cellElement = document.createElement("select");
        cellElement.id = "ddlNameOfKMP" + uniqueId;

        var NameOfKMP = document.getElementById('ctl00_ContentPlaceHolder1_hfNameOfKMP').value;
        var cnt1 = 0;
        cellElement.options[0] = new Option("(Select)", "");
        var arrRecs1 = NameOfKMP.split("~");
        while (cnt1 < arrRecs1.length) {
	        Rec = arrRecs1[cnt1];
	        Fields = Rec.split("|");
	        cellElement.options[cnt1 + 1] = new Option(Fields[1], Fields[0]);
	        cnt1 = cnt1 + 1;
        }
//        cellElement.maxLength = "100";
//        cellElement.size = "18";		
        cellElement.className = "dropdownlist1";
        cell1.className = "tabbody3";
        cell1.appendChild(cellElement);	
		
		var cell2 = row.insertCell(2);
		cellElement = document.createElement("input");
		cellElement.type = "text";
		cellElement.id = "txtPosition" + uniqueId;
		cellElement.maxLength = "200";
		cellElement.size = "20";
		cellElement.className = "textbox1";
		cell2.className = "tabbody3";
		cell2.appendChild(cellElement);	
		
		var cell3 = row.insertCell(3);
		cellElement = document.createElement("input");
		cellElement.type = "text";		
		cellElement.id = "txtEffectiveFromDate" + uniqueId;
		cellElement.maxLength = "11";
		cellElement.size = 11;
		cellElement.className = "textbox1";
		cell3.className = "tabbody3";
		cell3.appendChild(cellElement);

		$("#txtEffectiveFromDate" + uniqueId).datepicker( {
		showOn : 'button',
		buttonImage: '../../Content/images/legacy/calendar.gif',
		buttonImageOnly : true,
		changeMonth : true,
		changeYear : true,
		dateFormat : 'dd-M-yy'
		});
		
		var cell4 = row.insertCell(4);
		cellElement = document.createElement("input");
		cellElement.type = "text";
		cellElement.id = "txtEffectiveToDate" + uniqueId;
		cellElement.maxLength = "11";
		cellElement.size = 11;
		cellElement.className = "textbox1";
		cell4.className = "tabbody3";
		cell4.appendChild(cellElement);
		
		$("#txtEffectiveToDate" + uniqueId).datepicker( {
		showOn : 'button',
		buttonImage: '../../Content/images/legacy/calendar.gif',
		buttonImageOnly : true,
		changeMonth : true,
		changeYear : true,
		dateFormat : 'dd-M-yy'
		});
		
		
		var cell5 = row.insertCell(5);
		cellElement = document.createElement("input");
		cellElement.type = "text";
		cellElement.id = "txtRemarks" + uniqueId;
//		cellElement.rows = 3;
//		cellElement.cols = 50;
	    cellElement.maxLength = "500";
		//cellElement.size = "18";
		cellElement.className = "textbox1";
		cell5.className = "tabbody3";
		cell5.appendChild(cellElement);	
	
		
		cellElement = document.getElementById("txtPosition" + uniqueId);
		cellElement.focus();
		return false;
	} 
	catch (e) 
	{
		alert(e.message);
		return false;
	}
}  
function onMembersDetsHeaderRowChecked()
{
    var table = document.getElementById("tblKMPMembersDets");
    var allCheck = document.getElementById("HeaderLevelCheckBoxMembersDets");
	var rowCount = table.rows.length;
	var uniqueRowId;
	var check;
	for (var i = 1; i < rowCount; i++) 
	{
		var row = table.rows[i];
		cellElements = table.rows[i].cells[0].getElementsByTagName("input");
		uniqueRowId = cellElements[0].value;
		check = document.getElementById("cbMembersDets" + uniqueRowId);
		check.checked = allCheck.checked;
	}
}  

function deleteKMPMembersDetsRow() {
	try {
		var table = document.getElementById("tblKMPMembersDets");
		var rowCount = table.rows.length;
		var cellElements, cellUniqueRow, uniqueRowId;
		var strUniqueRowId = "", strId = "", strCLPDId="";
		var rowIdFrmTbl;
		var CLPDId ="";
		var deleteFromDB = 'N';
		for (var i = 1; i < rowCount; i++) {
			cellElements = table.rows[i].cells[0].getElementsByTagName("input");
			rowIdchecked = cellElements[1].checked;
			if (rowIdchecked == true)
			{
			    cellUniqueRow = table.rows[i].cells[0].getElementsByTagName("input");
			    uniqueRowId = cellUniqueRow[0].value;
			    CLPDId = document.getElementById("txtMembersDetsId"+uniqueRowId).value;
			    if(CLPDId != '0')
			    {
			        deleteFromDB = 'Y';
			    }
			    strCLPDId = strCLPDId + CLPDId + ",";
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
		    
		    if(deleteFromDB == 'Y')
	    { 
            //	alert("Fn");
		        window.open("../DeleteDBRecords.aspx?calledFrom=ContractDirectorDetailsId&Ids="+strCLPDId,
			    "DBRecords", "location=0,status=0,scrollbars=0,width=10,height=10");
			    deleteFromHTML2(strUniqueRowId);
			}
			else 
			{
			    deleteFromHTML2(strUniqueRowId);
			}
		    return false;		    
		}
	} 
	catch (e) {
		alert(e);
		return false;
	}
} 
 
function deleteFromHTML2(strUniqueRowId)
{
    var table = document.getElementById("tblKMPMembersDets");
    var rowCount = table.rows.length;
    var cellElements; 
    var uniqueRowId;
	var uniqueRowIds = strUniqueRowId.split(",");

	for (var cnt1 = 0; cnt1 < uniqueRowIds.length - 1; cnt1++) 
	{
		uniqueRowId = uniqueRowIds[cnt1];
		for (var cnt2 = 1; cnt2 < rowCount; cnt2++) 
		{
			var row = table.rows[cnt2];
			cellElements = table.rows[cnt2].cells[0].getElementsByTagName("input");
			rowIdFrmTbl = cellElements[0].value;
			if (rowIdFrmTbl == uniqueRowId) 
			{
				table.deleteRow(cnt2);
				break;
			}
		}
	}
}   


//
function addKMPDirectorDets(){
try {

        var hiddenFormData = document.getElementById(ClientIDJQ("hfdetailsOfKMPDATA"));
		var row;
		var detailsKMPData="";		
		var uniqueRowId;	
		hiddenFormData.value = "";
	    var msgtext ="";
	    //
	    
	    var ddlStatus = document.getElementById(ClientIDJQ("ddlStatus"));	   
	    var txtVendorName = document.getElementById(ClientIDJQ("txtVendorName"));	    
	   // var txtVendorCode = document.getElementById(ClientIDJQ("txtVendorCode");
	    var txtVendorAcronymName = document.getElementById(ClientIDJQ("txtVendorAcronymName"));
	    var txtVendorAddress = document.getElementById(ClientIDJQ("txtVendorAddress"));   
        var txtPan = document.getElementById(ClientIDJQ("txtPan"));
	   
		if (!required(txtVendorName)) {
			msgtext = msgtext + bullet
					+ 'Please enter Vendor Name. \n';
			setfocuson(txtVendorName);
		}
//		if (!required(txtVendorCode)) {
//			msgtext = msgtext + bullet
//					+ 'Please enter Vendor Code. \n';
//			setfocuson(txtVendorCode);
//		}
//		if (!required(txtVendorAcronymName)) {
//			msgtext = msgtext + bullet
//					+ 'Please enter Vendor Acronym. \n';
//			setfocuson(txtVendorAcronymName);
//		}
		if (!required(txtVendorAddress)) {
			msgtext = msgtext + bullet
					+ 'Please enter Vendor Address. \n';
			setfocuson(txtVendorAddress);
		}
		
		if (!required(txtPan)) {
			msgtext = msgtext + bullet
					+ 'Please enter Pan Number. \n';
			setfocuson(txtPan);
		}
		 if (!required(ddlStatus)) {
				msgtext = msgtext + bullet
						+ 'Please select Status. \n';
				setfocuson(ddlStatus);
		}
	    //
        var table = document.getElementById("tblKMPMembersDets"); 
        var rowCount = table.rows.length;
		for ( var i = 1; i < rowCount; i++) {
			row = table.rows[i];
			var cellElements0 = row.cells[0].getElementsByTagName("input");
			uniqueRowId = cellElements0[0].value;
			var membersDetsId = document.getElementById('txtMembersDetsId' + uniqueRowId).value;
            var ddlNameOfKMP = document.getElementById('ddlNameOfKMP' + uniqueRowId);
			var NameOfKMP = ddlNameOfKMP.value;
			var txtPositionField = document.getElementById('txtPosition' + uniqueRowId);
			var position = txtPositionField.value;
			var txtEffectiveFromDateField = document.getElementById('txtEffectiveFromDate' + uniqueRowId);
			var EfeectiveFromtDate = txtEffectiveFromDateField.value;
			var txtEffectiveToDateField = document.getElementById('txtEffectiveToDate' + uniqueRowId);
			var EffectiveToDate = txtEffectiveToDateField.value;
			var txtRemarksField = document.getElementById('txtRemarks' + uniqueRowId);
			var remarks = txtRemarksField.value;

            detailsKMPData = detailsKMPData + membersDetsId + "|" + position + "|" + EfeectiveFromtDate 
			        + "|" + EffectiveToDate 
			         + "|" + remarks +"~";
//			           alert("addKMPDirectorDets"+detailsKMPData);
			if (!required(ddlNameOfKMP)) {
				msgtext = msgtext + bullet
						+ 'Please select Name of Director/KMP for KMP/Director Details Grid Row ' + i
						+ '\n';
				setfocuson(ddlNameOfKMP);
			}
	        if (!required(txtPositionField)) {
				msgtext = msgtext + bullet
						+ 'Please enter position for KMP/Director Details Grid Row ' + i
						+ '\n';
				setfocuson(txtPositionField);
			}
			if(txtPositionField.value.length > 1000)
            {
                msgtext = msgtext + bullet
		                    + 'Psition cannot exceed 1000 characters for KMP/Director Details Grid Row ' + i
						+ '\n';
                setfocuson(txtPositionField);
            }

			
			if (!required(txtEffectiveFromDateField)) {
				msgtext = msgtext + bullet
						+ 'Please enter Effective From Date for KMP/Director Details Grid Row ' + i
						+ '\n';
				setfocuson(txtEffectiveFromDateField);
			}
			else if (!checkDateIfEntered(txtEffectiveFromDateField)) {                                
			    msgtext = msgtext + bullet + 'Effective From Date format should be dd-mon-yyyy for Actionable Grid Row ' + i
						+ '\n';
			    setfocuson(txtEffectiveFromDateField);
	        }

            if(txtEffectiveToDateField.value != '')
            {
                if (!checkDateIfEntered(txtEffectiveToDateField))
                {                                
			        msgtext = msgtext + bullet + 'Effective To Date format should be dd-mon-yyyy for Actionable Grid Row ' + i
						+ '\n';
			        setfocuson(txtEffectiveToDateField);
			    }
			    else
			    {
                    if (compare2Dates(txtEffectiveFromDateField, txtEffectiveToDateField) >= 1)
                    {
                    }
                    else 
                    {
                        msgtext = msgtext + bullet
		                        + 'Effective To Date should be greater than or equal to Effective From Date. ' + i
						        + '\n';
						setfocuson(txtEffectiveToDateField);
                    }
                }
            }
		}
		if (msgtext != "")
		{
		    alert(msgtext);
		    if (focuson != null) {
				focuson.focus();
			}
		    return false;
		}
		else
        {
            getDirctorKMPData();
            return true;
        }
		return true;
	} catch (e) {
	    alert(e.message);
		return false;
		
	}
}
//


function getDirctorKMPData(){
try {
	    
        var table = document.getElementById("tblKMPMembersDets");
        var hiddenFormData = document.getElementById(ClientIDJQ("hfdetailsOfKMPDATA"));
        if (table == null)
		{
		    return;
		}		
		var rowCount = table.rows.length;	
		var row;
		//alert(rowCount);
		if(rowCount ==1)
		{
			    var ddlIsRelatedParty = document.getElementById(ClientIDJQ("ddlIsRelatedParty")).value;   
			    ddlIsRelatedParty = document.getElementById(ClientIDJQ("ddlIsRelatedParty")).value ='N';
		        //ddlIsRelatedParty.options[ddlIsRelatedParty.selectedIndex].value ='N';
		        //alert("ddlIsRelatedParty"+ddlIsRelatedParty);
		}
		var DirectorMembersData="";		
		var uniqueRowId;	
		hiddenFormData.value = "";	
		for ( var i = 1; i < rowCount; i++) {		    
			row = table.rows[i];
			var cellElements0 = row.cells[0].getElementsByTagName("input");
			uniqueRowId = cellElements0[0].value;
			var membersDetsId = document.getElementById('txtMembersDetsId' + uniqueRowId).value;
            var ddlNameOfKMP = document.getElementById('ddlNameOfKMP' + uniqueRowId);
			var NameOfKMP = ddlNameOfKMP.value;
			var txtPositionField = document.getElementById('txtPosition' + uniqueRowId);
			var position = txtPositionField.value;
			var txtEffectiveFromDateField = document.getElementById('txtEffectiveFromDate' + uniqueRowId);
			var EfeectiveFromtDate = txtEffectiveFromDateField.value;
			var txtEffectiveToDateField = document.getElementById('txtEffectiveToDate' + uniqueRowId);
			var EffectiveToDate = txtEffectiveToDateField.value;
			var txtRemarksField = document.getElementById('txtRemarks' + uniqueRowId);
			var remarks = txtRemarksField.value;

            DirectorMembersData = DirectorMembersData + membersDetsId + "|" + NameOfKMP + "|" + position 
			        + "|" + EfeectiveFromtDate + "|" + EffectiveToDate +"|" + remarks +"~"; 
//			        alert("getDirctorKMPData"+ DirectorMembersData);    
		}
		 
		if (DirectorMembersData != "") 
		{
			hiddenFormData.value = DirectorMembersData;
		}
		
		return true;
	} catch (e) {
	    alert(e.message);
		return false;
	}
}
