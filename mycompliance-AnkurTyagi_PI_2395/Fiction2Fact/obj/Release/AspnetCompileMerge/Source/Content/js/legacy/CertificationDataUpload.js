// JScript File
var focuson = null;
function setfocuson(obj) {
    if (focuson == null) {
        focuson = obj;
    }
}

function SearchChecklist() {
    try {
        var msgtext = "", recordExist = "";
        focuson = null;
        var DDLDepartmentName = document.getElementById("ctl00_ContentPlaceHolder1_ddlSearchDeptName");

        if (!required(DDLDepartmentName)) {
            msgtext = msgtext + bullet
                    + 'Please select Department Name. \n';
            setfocuson(DDLDepartmentName);
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

function validateonsavedata() {
    try {
        var msgtext = "", recordExist = "";
        focuson = null;
        var DDLDepartmentName = document.getElementById("ctl00_ContentPlaceHolder1_ddlSearchDeptName");
        //<< Added by Amarjeet on 23-Jul-2021
        var ddlActRegCirc = document.getElementById("ctl00_ContentPlaceHolder1_ddlActRegCirc");
        //>>
        var txtReference = document.getElementById("ctl00_ContentPlaceHolder1_txtReference");
        var txtTitleofSection = document.getElementById("ctl00_ContentPlaceHolder1_txtTitleofSection");
        //var txtParticulars = document.getElementById("ctl00_ContentPlaceHolder1_txtParticulars");

        //var txtCheckpoints = document.getElementById("ctl00_ContentPlaceHolder1_txtCheckpoints");
        var txtFrequency = document.getElementById("ctl00_ContentPlaceHolder1_txtFrequency");
        //<< Added by Amarjeet on 23-Jul-2021
        var txtForms = document.getElementById("ctl00_ContentPlaceHolder1_txtForms");
        //>>
        //var txtDueDate = document.getElementById("ctl00_ContentPlaceHolder1_txtDueDate");
        //var txtSourceDepartment = document.getElementById("ctl00_ContentPlaceHolder1_txtSourceDepartment");
        //var txtDeptRespFurnish = document.getElementById("ctl00_ContentPlaceHolder1_txtDeptRespFurnish");
        //var txtDeptRespSubmit = document.getElementById("ctl00_ContentPlaceHolder1_txtDeptRespSubmit");
        //var txtTobeFiledWith = document.getElementById("ctl00_ContentPlaceHolder1_txtTobeFiledWith");

        var EffectiveFrom = document.getElementById("ctl00_ContentPlaceHolder1_txtEffectiveFromDate");
        var EffectiveTo = document.getElementById("ctl00_ContentPlaceHolder1_txtEffectiveToDate");
        var DDLActIAct = document.getElementById("ctl00_ContentPlaceHolder1_ddlActInAct");
        var Remarks = document.getElementById("ctl00_ContentPlaceHolder1_txtRemarks");
        var txtSelfAssessmentStatus = document.getElementById("ctl00_ContentPlaceHolder1_txtParticulars");
        var txtPenalty = document.getElementById("ctl00_ContentPlaceHolder1_txtPenalty");
        

        if (!required(DDLDepartmentName)) {
            msgtext = msgtext + bullet
		            + 'Please select Department Name. \n';
            setfocuson(DDLDepartmentName);
        }

        if (!required(txtTitleofSection)) {
            msgtext = msgtext + bullet
		            + 'Please enter Section/Clause. \n';
            setfocuson(txtTitleofSection);
        }

        //<< Added by Amarjeet on 23-Jul-2021
        if (!required(ddlActRegCirc)) {
            msgtext = msgtext + bullet
		            + 'Please select Act/Regulation/Circular. \n';
            setfocuson(ddlActRegCirc);
        }
        //>>

        if (!required(txtReference)) {
            msgtext = msgtext + bullet
		            + 'Please enter Reference Circular / Notification / Act. \n';
            setfocuson(txtReference);
        }
        
        //if (!required(txtPenalty)) {
        //    msgtext = msgtext + bullet
		      //      + 'Please enter Consequences of non Compliance. \n';
        //    setfocuson(txtPenalty);
        //}

        //        if (!required(txtParticulars)) 
        //        {
        //            msgtext = msgtext + bullet
        //		            + 'Please enter Control Process. \n';
        //            setfocuson(txtParticulars);
        //        }
        /*
        if (!required(txtCheckpoints)) 
        {
	        msgtext = msgtext + bullet
			        + 'Please enter Checkpoints. \n';
	        setfocuson(txtCheckpoints);
        }*/

        //if (!required(txtFrequency)) {
        //    msgtext = msgtext + bullet
		      //      + 'Please enter Frequency. \n';
        //    setfocuson(txtFrequency);
        //}

        //<< Added by Amarjeet on 23-Jul-2021
        //if (!required(txtForms)) {
        //    msgtext = msgtext + bullet
		      //      + 'Please enter Forms. \n';
        //    setfocuson(txtForms);
        //}
        //>>

        if (!required(txtSelfAssessmentStatus)) {
            msgtext = msgtext + bullet
                    + 'Please enter Description. \n';
            setfocuson(txtSelfAssessmentStatus);
        }
        /*
        if (!required(txtDueDate)) 
        {
		        msgtext = msgtext + bullet                                   
				        + 'Please enter DueDate. \n';                      
		        setfocuson(txtDueDate);
        } 
        
        if (!required(txtSourceDepartment)) 
        {
		        msgtext = msgtext + bullet                                   
				        + 'Please enter Source Department. \n';                      
		        setfocuson(txtSourceDepartment);
        } 
        
        if (!required(txtDeptRespFurnish)) 
        {
		        msgtext = msgtext + bullet                                   
				        + 'Please enter  Department responsible for furnishing the data. \n';                      
		        setfocuson(txtDeptRespFurnish);
        } 
        
        if (!required(txtDeptRespSubmit)) 
        {
		        msgtext = msgtext + bullet                                   
				        + 'Please enter  Department responsible for submitting it. \n';                      
		        setfocuson(txtDeptRespSubmit);
        }*/

        //        if (!required(txtTobeFiledWith)) 
        //        {
        //            msgtext = msgtext + bullet
        //		            + 'Please enter To be filed with. \n';
        //            setfocuson(txtTobeFiledWith);
        //        }

        if (!required(DDLActIAct)) {
            msgtext = msgtext + bullet
		            + 'Please select Active/Inactive. \n';
            setfocuson(DDLActIAct);
        }
        else if (DDLActIAct.value == 'I') {
            if (!required(Remarks)) {
                msgtext = msgtext + bullet
                + 'Please enter Deactivation Remarks. \n';
                setfocuson(Remarks);
            }
            if (!required(EffectiveTo)) {
                msgtext = msgtext + bullet
                + 'Please select Effective Date To. \n';
                setfocuson(EffectiveTo);
            }
            else if (!checkDateIfEntered(EffectiveTo)) {
                msgtext = msgtext + bullet
                + 'Effective Date To format should be dd-mon-yyyy.\n';
                setfocuson(EffectiveTo);
            }
        }

        if (!required(EffectiveFrom)) {
            msgtext = msgtext + bullet
		            + 'Please select Effective Date From. \n';
            setfocuson(EffectiveFrom);
        }
        else if (!checkDateIfEntered(EffectiveFrom)) {
            msgtext = msgtext + bullet
             + 'Effective Date From format should be dd-mon-yyyy.\n';
            setfocuson(EffectiveFrom);
        }

        //        if (!required(EffectiveTo)) 
        //        {
        //		        msgtext = msgtext + bullet                                   
        //				        + 'Please select Effective Date To. \n';                      
        //		        setfocuson(EffectiveTo);
        //        } 
        //        else if (!checkDateIfEntered(EffectiveTo)) 
        //        {
        //            msgtext = msgtext + bullet 
        //             + 'Effective Date To format should be dd-mon-yyyy.\n';
        //             setfocuson(EffectiveTo);
        //        } 


        if (msgtext != "") {
            alert(msgtext);
            return false;
        }
        else {
            if (!confirm('Are you sure that you want to save this checklist clause?')) {
                return false;
            }
            return true;
        }
    }
    catch (e) {
        alert(e.message);
        return false;
    }
}