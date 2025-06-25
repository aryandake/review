
function chkIfPolicyNoExists(isOnBlur, operationType, type) {
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
	
	
	if(type == 'UW')
	{
	    var PolicyNo = document.getElementById(ClientIDJQ("txtPolicyNo")).value;
	    var CaseId = document.getElementById(ClientIDJQ("lblUWId")).innerText;
	}
	else if(type == 'Fraud')
	{	
	    var PolicyNo = document.getElementById(ClientIDJQ("hfPolicyNo")).value;
	    var CaseId = document.getElementById(ClientIDJQ("lblFCId")).innerText;
	}	
	//Added by Bhavik @ 18 Sep 2013
	else if(type == 'MS')
	{
	
	    var PolicyNo = document.getElementById(ClientIDJQ("txtPolicyNo")).value;
	    var CaseId = document.getElementById(ClientIDJQ("lblMSCId")).innerText;
	}
	
	//Added by Bhavik @ 19 Sep 2013
	else if(type == 'NV')
	{
	
	    var PolicyNo = document.getElementById(ClientIDJQ("txtCurPolicyNo")).value;
	    var CaseId = document.getElementById(ClientIDJQ("lblNVCId")).innerText;
	}	
	
    var url = "../CommonFraud/CheckPolicyNo.aspx?Type=" + type +"&CaseId=" + CaseId +"&PolicyNo=" + PolicyNo;   
    ajaxRequest.open("POST", url, false);
    ajaxRequest.send(null);
    var msg = ajaxRequest.responseText;
    //alert('1' + operationType);
    //alert('2' + isOnBlur);
    if (msg == '')
	{
	    if(!isOnBlur)
	    {
	        //alert(operationType);
	        
	        if(operationType == 'Submit')
	        {
                if (!confirm('Are you sure you want to submit this record?'))return false;
                {
	               return true;
                }
	        }
	        else if(operationType == 'SaveDraft')
	        {
                if (!confirm('Are you sure you want to save draft this record?'))return false;
                {
	               return true;
                }
	        }
	    }
	    else
	        return true;
	}
	else
	{
	    alert(msg);
	    return false;
	}
}


