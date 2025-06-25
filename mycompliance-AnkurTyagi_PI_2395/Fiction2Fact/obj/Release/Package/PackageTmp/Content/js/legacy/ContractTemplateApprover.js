// JScript File

var bullet = '- ';

function addApproverDetsRow() {
	try {

	    var uniqueId = 0;
		var cellElements;
		var table = document.getElementById('tblApproverDets');
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
		cellElement.id = "cbApproverDets" + uniqueId;
		cell0.className = "tabbody3";
		cell0.appendChild(cellElement);
       
        var cell1 = row.insertCell(1);
		
		var cellElement = document.createElement("input");
		cellElement.type = "hidden";
		cellElement.id = "txtApproverDetsId" + uniqueId;
		cellElement.value = 0;
		cell1.appendChild(cellElement);
		
        cellElement = document.createElement("select");
        cellElement.id = "ddlApprovalRequiredFrom" + uniqueId;

        var ApprovalRequiredFrom = document.getElementById(ClientIDJS('hfApprovalRequiredFrom')).value;
        var cnt1 = 0;
        cellElement.options[0] = new Option("(Select)", "");
        var arrRecs1 = ApprovalRequiredFrom.split("~");
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
		cellElement.id = "txtSortOrder" + uniqueId;
		cellElement.maxLength = "200";
		cellElement.size = "20";
		cellElement.className = "textbox1";
		cellElement.onkeypress = function(e) {
			e = e || window.event;
			return filterInput(1, e, true, '');
		};
		cell2.className = "tabbody3";
		cell2.appendChild(cellElement);	
		
		
		cellElement = document.getElementById("txtSortOrder" + uniqueId);
		
		var cell3 = row.insertCell(3);
		
		cellElement = document.createElement("select");
        cellElement.id = "ddlApproverType" + uniqueId;

        var ApproverType = document.getElementById(ClientIDJQ('hfApproverType')).value;
        var cnt1 = 0;
        cellElement.options[0] = new Option("(Select)", "");
        var arrRecs1 = ApproverType.split("~");
        while (cnt1 < arrRecs1.length) {
	        Rec = arrRecs1[cnt1];
	        Fields = Rec.split("|");
	        cellElement.options[cnt1 + 1] = new Option(Fields[1], Fields[0]);
	        cnt1 = cnt1 + 1;
        }
//        cellElement.maxLength = "100";
//        cellElement.size = "18";		
        cellElement.className = "dropdownlist1";
        cell3.className = "tabbody3";
        cell3.appendChild(cellElement);	
		cellElement.focus();
		return false;
	} 
	catch (e) 
	{
		alert(e.message);
		return false;
	}
}  
function onApproverDetsHeaderRowChecked()
{
    var table = document.getElementById("tblApproverDets");
    var allCheck = document.getElementById("HeaderLevelCheckBoxApproverDets");
	var rowCount = table.rows.length;
	var uniqueRowId;
	var check;
	for (var i = 1; i < rowCount; i++) 
	{
		var row = table.rows[i];
		cellElements = table.rows[i].cells[0].getElementsByTagName("input");
		uniqueRowId = cellElements[0].value;
		check = document.getElementById("cbApproverDets" + uniqueRowId);
		check.checked = allCheck.checked;
	}
}  

function deleteApproverDetsRow() {
	try {
		var table = document.getElementById("tblApproverDets");
		var rowCount = table.rows.length;
		var cellElements, cellUniqueRow, uniqueRowId;
		var strUniqueRowId = "", strId = "", strIds="";
		var rowIdFrmTbl;
		var Ids ="";
		var deleteFromDB = 'N';
		for (var i = 1; i < rowCount; i++) {
			cellElements = table.rows[i].cells[0].getElementsByTagName("input");
			rowIdchecked = cellElements[1].checked;
			if (rowIdchecked == true)
			{
			    cellUniqueRow = table.rows[i].cells[0].getElementsByTagName("input");
			    uniqueRowId = cellUniqueRow[0].value;
			    Ids = document.getElementById("txtApproverDetsId"+uniqueRowId).value;
			    if(Ids != '0')
			    {
			        deleteFromDB = 'Y';
			    }
			    strIds = strIds + Ids + ",";
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

		        window.open(SiteUrlJS("Projects/DeleteDBRecords.aspx") + "?calledFrom=ContractDepartmentDetailsId&Ids="+strIds,
			    "DBRecords", "location=0,status=0,scrollbars=0,width=300,height=200");
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
    var table = document.getElementById("tblApproverDets");
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

function addApproverDets()
{
    try 
    {
        var hiddenFormData = document.getElementById(ClientIDJQ("hfApproverDetails"));
		var row;
		var ApproverDets="";		
		var uniqueRowId;	
		hiddenFormData.value = "";
	    var msgtext ="";
	    //
	    
	    var ddlContractStatus = document.getElementById(ClientIDJQ("ddlContractStatus"));	   
	    var ddlContractType = document.getElementById(ClientIDJQ("ddlContractType"));	    
	    var txtContractName = document.getElementById(ClientIDJQ("txtContractName"));
	    var txtFromDate = document.getElementById(ClientIDJQ("txtFromDate"));  
		if (!required(ddlContractType)) {
			msgtext = msgtext + bullet
					+ 'Please enter Contract Type. \n';
			setfocuson(ddlContractType);
		}
		
		if (!required(txtContractName)) {
			msgtext = msgtext + bullet
					+ 'Please enter Contract Name. \n';
			setfocuson(txtContractName);
		}
		
		if (!required(txtFromDate)) {
			msgtext = msgtext + bullet
					+ 'Please enter From Date. \n';
			setfocuson(txtFromDate);
		}
		 if (!required(ddlContractStatus)) {
				msgtext = msgtext + bullet
						+ 'Please select Contract Status. \n';
				setfocuson(ddlContractStatus);
		}
	    //
        var table = document.getElementById("tblApproverDets"); 
        var rowCount = table.rows.length;
        
		for (var i = 1; i < rowCount; i++) 
		{
			row = table.rows[i];
			var cellElements0 = row.cells[0].getElementsByTagName("input");
			uniqueRowId = cellElements0[0].value;
			var ApproverDetsId = document.getElementById('txtApproverDetsId' + uniqueRowId).value;
            var ddlApprovalRequiredFrom = document.getElementById('ddlApprovalRequiredFrom' + uniqueRowId);
			var ApprovalRequiredFrom = ddlApprovalRequiredFrom.value;
			var txtSortOrder = document.getElementById('txtSortOrder' + uniqueRowId);
			var SortOrder = txtSortOrder.value;
			var ddlApproverType = document.getElementById('ddlApproverType' + uniqueRowId);
			var ApproverType = ddlApproverType.value;
			
            ApproverDets = ApproverDets + ApproverDetsId + "~";
//			           alert("addKMPDirectorDets"+detailsKMPData);
			if (!required(ddlApprovalRequiredFrom)) 
			{
				msgtext = msgtext + bullet
						+ 'Please select Department Name for row ' + i
						+ '.\n';
				setfocuson(ddlApprovalRequiredFrom);
			}
			
	        if (!required(txtSortOrder)) 
	        {
				msgtext = msgtext + bullet
						+ 'Please enter Sort Order for row ' + i
						+ '.\n';
				setfocuson(txtSortOrder);
			}
			
			//<< Adedd by Archana Gosavi on 01-Jan-2018
			if(SortOrder == 0)
			{
			    msgtext = msgtext + bullet
						+ 'Sort order should not be zero for row ' + i
						+ '.\n';
				setfocuson(txtSortOrder);
			}
			//>>
			
			if (!required(ddlApproverType)) 
			{
				msgtext = msgtext + bullet
						+ 'Please select Approver Type for row ' + i
						+ '.\n';
				setfocuson(ddlApproverType);
			}

		    //<< Adedd by Sandesh on 11-Nov-2019
			if (ddlApproverType.value=="REQ" && ddlApprovalRequiredFrom.value!=1)
			{
			    msgtext = msgtext + bullet
						+ 'When Approver Type is "Requester", Department Name must be "Requesting Function" for row ' + i
						+ '.\n';
			    setfocuson(ddlApprovalRequiredFrom);
			}
		    //>>
		}
		if (msgtext != "")
		{
		    alert(msgtext);
		    if (focuson != null) 
		    {
				focuson.focus();
			}
		    return false;
		}
		else
        {
            getaddApproverData();
            return true;
        }
		return true;
	} 
	catch (e)
	{
	    alert(e.message);
		return false;
		
	}
}
//


function getaddApproverData(){
try {
	    
        var table = document.getElementById("tblApproverDets");
        var hiddenFormData = document.getElementById(ClientIDJQ("hfApproverDetails"));
        if (table == null) 
		{
		    return;
		}		
		var rowCount = table.rows.length;	
		var row;
		//alert(rowCount);
		
		var ApprovalDepartmentData="";		
		var uniqueRowId;	
		hiddenFormData.value = "";	
		for ( var i = 1; i < rowCount; i++) {		    
			row = table.rows[i];
			var cellElements0 = row.cells[0].getElementsByTagName("input");
			uniqueRowId = cellElements0[0].value;
			var ApproverDetsId = document.getElementById('txtApproverDetsId' + uniqueRowId).value;
            var ddlApprovalRequiredFrom = document.getElementById('ddlApprovalRequiredFrom' + uniqueRowId);
			var ApprovalRequiredFrom = ddlApprovalRequiredFrom.value;
			var txtSortOrder = document.getElementById('txtSortOrder' + uniqueRowId);
			var SortOrder = txtSortOrder.value;
			var ddlApproverType = document.getElementById('ddlApproverType' + uniqueRowId);
			var ApproverType = ddlApproverType.value;
			
            ApprovalDepartmentData = ApprovalDepartmentData + ApproverDetsId + "|" + ApprovalRequiredFrom + "|" + SortOrder + "|" + ApproverType +"~"; 
		}
		 
		if (ApprovalDepartmentData != "") 
		{
			hiddenFormData.value = ApprovalDepartmentData;
		}
		
		return true;
	} catch (e) {
	    alert(e.message);
		return false;
	}
}