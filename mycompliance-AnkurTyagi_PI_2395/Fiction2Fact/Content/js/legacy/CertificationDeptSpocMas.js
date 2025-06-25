var bullet = '- ';

function addMembersDetsRow() {
	try {
	    var uniqueId = 0;
		var cellElements;
		var table = document.getElementById('tblMembersDets');
		
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
		
		cellElement = document.createElement("input");
		cellElement.type = "text";
		cellElement.id = "txtDeptName" + uniqueId;
		cellElement.maxLength = "200";
		cellElement.size = "30";
		cellElement.className = "form-control";
		cell1.className = "tabbody3";
		cell1.appendChild(cellElement);	
		
		var cell2 = row.insertCell(2);
		cellElement = document.createElement("input");
		cellElement.type = "text";
		cellElement.id = "txtUserId" + uniqueId;
		cellElement.onchange = Function("populateUserDetsByCode('CertSPOCMas'," + uniqueId + ")");		
		cellElement.maxLength = "100";
		cellElement.size = "30";
		cellElement.className = "form-control";
		cell2.className = "tabbody3";
		cell2.appendChild(cellElement);	
		
		var cell3 = row.insertCell(3);
		cellElement = document.createElement("input");
		cellElement.type = "text";
		cellElement.id = "txtUserName" + uniqueId;
		cellElement.maxLength = "100";
		cellElement.size = "30";
		cellElement.className = "form-control";
		cell3.className = "tabbody3";
		cell3.appendChild(cellElement);	
		
	    var cell4 = row.insertCell(4);
		cellElement = document.createElement("input");
		cellElement.type = "text";
		cellElement.id = "txtEmailId" + uniqueId;
		cellElement.maxLength = "100";
		cellElement.size = "30";
		cellElement.className = "form-control";
		cell4.className = "tabbody3";
		cell4.appendChild(cellElement);	
		
		cellElement = document.getElementById("txtDeptName" + uniqueId);
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
    var table = document.getElementById("tblMembersDets");
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

function deleteMembersDetsRow() {
	try {
		var table = document.getElementById("tblMembersDets");
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
		    deleteFromHTML2(strUniqueRowId);
		    if(deleteFromDB == 'Y')
		    {
		        window.open("../DeleteDBRecords.aspx?calledFrom=CertificationHeadSpoc&Ids="+strCLPDId,
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
 
function deleteFromHTML2(strUniqueRowId)
{
    var table = document.getElementById("tblMembersDets");
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

function validateAuditorDets()
{
    try 
    {
        var msgtext ="";
        focuson = null;
        
        var AuditorNameField = document.getElementById("ctl00_ContentPlaceHolder1_ddlCertDept");
        var ddlStatusField = document.getElementById("ctl00_ContentPlaceHolder1_ddlStatus");
        
        if (!required(AuditorNameField))
       {
               msgtext = msgtext + bullet		
          		        + 'Please select Auditor Name. \n';
		        setfocuson(AuditorNameField);
        }
        
        if (!required(ddlStatusField))
        {                                          
		        msgtext = msgtext + bullet                                   
				        + 'Please select Status. \n';                                
		        setfocuson(ddlStatusField);                                          
        }       
       
         //here.
        msgtext = msgtext + validateAddMembersDets();
        
        if (msgtext != "")
        {
            alert(msgtext);
            return false;
        }
        else
        {
            getAddMembersDetsData();
            return true;
        }
       
    }
    catch (e)
    {
		alert(e.message);
		return false;
	}
}

//<< Added by Archana Gosavi on 07-Jan-2016 
function validateCertDeptDets()
{
    try 
    {
        var msgtext ="";
        focuson = null;
        
        var CertDeptField = document.getElementById("ctl00_ContentPlaceHolder1_ddlCertDept");
        var CertDeptNameField = document.getElementById("ctl00_ContentPlaceHolder1_txtCertDeptName");
                
        var CertDeptUserIdField = document.getElementById("ctl00_ContentPlaceHolder1_txtCertDeptUserId");
        var CertDeptUserNameField = document.getElementById("ctl00_ContentPlaceHolder1_txtCertDeptUserName");
        var CertDeptEmailIDField = document.getElementById("ctl00_ContentPlaceHolder1_txtCertDeptEmailID");
        
        
       if (!required(CertDeptField))
       {
               msgtext = msgtext + bullet		
          		        + 'Please select Certifying Function. \n';
		        setfocuson(CertDeptField);
        }
        
        if (!required(CertDeptNameField))
        {                                          
		        msgtext = msgtext + bullet                                   
				        + 'Please enter Certifying Unit. \n';                                
		        setfocuson(CertDeptNameField);                                          
        }  
        
        if (!required(CertDeptUserIdField))
        {                                          
		        msgtext = msgtext + bullet                                   
				        + 'Please enter Unit Head NT ID. \n';                                
		        setfocuson(CertDeptUserIdField);                                          
        }   
        if (!required(CertDeptUserNameField))
        {                                          
		        msgtext = msgtext + bullet                                   
				        + 'Please enter Unit Head Name. \n';                                
		        setfocuson(CertDeptUserNameField);                                          
        }   
        if (!required(CertDeptEmailIDField))
        {                                          
		        msgtext = msgtext + bullet                                   
				        + 'Please enter Unit Head Email Id. \n';                                
		        setfocuson(CertDeptEmailIDField);                                          
        }    
       
         //here.
        msgtext = msgtext + validateAddMembersDets();
        
        if (msgtext != "")
        {
            alert(msgtext);
            return false;
        }
        else
        {
            getAddMembersDetsData();
            return true;
        }
       
    }
    catch (e)
    {
		alert(e.message);
		return false;
	}
}
//>>

function validateAddMembersDets()
{
    try 
    {
        var table = document.getElementById("tblMembersDets");
        if (table == null) 
		{
		    return;
		}		
		var rowCount = table.rows.length;	
        var msgtext ="";
        focuson = null;
       
       for (var i = 1; i < rowCount; i++) 
       {
		    row = table.rows[i];
		    var cellElements0 = row.cells[0].getElementsByTagName("input");
		    uniqueRowId = cellElements0[0].value;	
	        
	        var txtDeptNameField = document.getElementById('txtDeptName' + uniqueRowId);
	        var txtEmailIdField = document.getElementById('txtEmailId' + uniqueRowId);
	        var txtUserIdField = document.getElementById('txtUserId' + uniqueRowId);
	        var txtUserNameField = document.getElementById('txtUserName' + uniqueRowId);     
	        
	        if (!required(txtDeptNameField))
            { 
		         msgtext = msgtext + bullet
				            + 'Please enter Department Name for Department SPOC Member Details Grid Row No. ' + i
				            +'.\n';
		         setfocuson(txtDeptNameField);
            }
             if (!required(txtUserIdField))
            { 
		         msgtext = msgtext + bullet
				            + 'Please enter User Id for Department SPOC Member Details Grid Row No. ' + i
				            +'.\n';
		         setfocuson(txtUserIdField);
            }
            if (!required(txtEmailIdField))
            { 
		         msgtext = msgtext + bullet
				            + 'Please enter Email id for Department SPOC Member Details Grid Row No. ' + i
				            +'.\n';
		         setfocuson(txtEmailIdField);
            }
            if(!validateEmail(txtEmailIdField,true)){
			    msgtext = msgtext + bullet
						+ 'Email Id is Invalid for Department SPOC Member Details Grid Row No. ' + i
						+ '.\n';
				setfocuson(txtEmailIdField);
			} 
			if (!required(txtUserNameField))
            {
               msgtext = msgtext + bullet		
          		        + 'Please enter User Name for Department SPOC Member Details Grid Row No. ' + i
          		        + '.\n';
		        setfocuson(txtUserNameField);
             }   			
		 
        }
         return msgtext;
    }
    catch (e) 
    {
		alert(e.message);
		return false;
	}
}

function getAddMembersDetsData()
{
    try{
	    var table = document.getElementById("tblMembersDets");
        var hiddenFormData = document.getElementById("ctl00_ContentPlaceHolder1_hfMembersDetsData");
        if (table == null) 
		{
		    return;
		}		
		var rowCount = table.rows.length;
		var row;
		var MembersDetsData="";		
		var uniqueRowId;	
		hiddenFormData.value = "";	
		for (var i = 1; i < rowCount; i++)
		{
			row = table.rows[i];
			var cellElements0 = row.cells[0].getElementsByTagName("input");
			uniqueRowId = cellElements0[0].value;
			
			var MembersDetsId = document.getElementById('txtMembersDetsId' + uniqueRowId).value;			
			
			var txtDeptNameField = document.getElementById('txtDeptName' + uniqueRowId);
			var DeptName=txtDeptNameField.value;
			
			var txtUserIdField = document.getElementById('txtUserId' + uniqueRowId);
			var UserId=txtUserIdField.value;
			
			var txtEmailIdField = document.getElementById('txtEmailId' + uniqueRowId);
			var EmailId=txtEmailIdField.value;	
						
			var txtUserNameField = document.getElementById('txtUserName' + uniqueRowId);
		    var UserName = txtUserNameField.value; 
		 	
			MembersDetsData = MembersDetsData + MembersDetsId + "|" + DeptName +"|" + UserId 
			+"|" + EmailId +"|" + UserName + "~";	
		}
		if (MembersDetsData != "") 
		{
			hiddenFormData.value = MembersDetsData;
		}
		
		return true;
	}
	 catch (e) 
	 {
	    alert(e.message);
		return false;
	 }
}
    