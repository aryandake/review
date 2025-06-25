var focuson = null;
function setfocuson(obj) {
	if (focuson == null) {
		focuson = obj;
	}
}
var bullet = "- ";

function roundto2Decimals(invalue)
{
    return Math.round(invalue * 100)/100
}

function openSplitPanel()
{
   
    document.getElementById("pnlSplit").style.visibility="visible";
    document.getElementById("pnlSplit").style.display="block";
    
    document.getElementById("ctl00$ContentPlaceHolder1$txtNoofSplit").value = '';
    document.getElementById("ctl00$ContentPlaceHolder1$txtNoofSplit").focus();
    return false;
}

function hideSplitPanel()
{
   
    document.getElementById("pnlSplit").style.visibility="hidden";
    document.getElementById("pnlSplit").style.display="none";
    return false;
}

function hideGroupPanel()
{
    var spanGroupSplitButtonobj = document.getElementById('spanGroupSplitButton');
    spanGroupSplitButtonobj.style.visibility = 'hidden';
    spanGroupSplitButtonobj.style.display = 'none';
    return false;
}

function required(obj) {
	if (obj == null || trim(obj.value) == "") {
		//setfocuson(obj);
		return false;
	} else {

		return true;
	}
}

function trim(stringToTrim) {
	return stringToTrim.replace(/^\s+|\s+$/g, "");
}
function ltrim(stringToTrim) {
	return stringToTrim.replace(/^\s+/, "");
}

function rtrim(stringToTrim) {
	return stringToTrim.replace(/\s+$/, "");
}

function validateEmail(obj, isOnBlur)
{
	if (obj == null || obj.value == '')
		return true;
	if (!validEmail(obj)) {
		if (isOnBlur)
		{
			//alert(' Invalid Email Address.');
			obj.focus();
		}
		return false;
	}
	 return true; 
}

function validEmail(obj) {
	var str = obj.value
	var at = "@"
	var dot = "."
	var lat = str.indexOf(at)
	var lstr = str.length
	var ldot = str.indexOf(dot)
	if (str.indexOf(at) == -1) {
		return false
	}

	if (str.indexOf(at) == -1 || str.indexOf(at) == 0
			|| str.indexOf(at) == lstr) {
		return false
	}

	if (str.indexOf(dot) == -1 || str.indexOf(dot) == 0
			|| str.indexOf(dot) == lstr) {
		return false
	}
	
	//Added by Denil on 29-Jun-11 for checking values are there or not after '.'
	if (str.indexOf(dot) == lstr - 1) {
		return false
	}

	if (str.indexOf(at, (lat + 1)) != -1) {
		return false
	}

	if (str.substring(lat - 1, lat) == dot
			|| str.substring(lat + 1, lat + 2) == dot) {
		return false
	}

	if (str.indexOf(dot, (lat + 2)) == -1) {
		return false
	}

	if (str.indexOf(" ") != -1) {
		return false
	}

	return true
}

function validateDecimal(value, precision, scale){
var patt1;
  if(value.indexOf(".")==-1){
     patt1=new RegExp("^[0-9]{1,"+precision+"}$");
  }else{
//     patt1=new RegExp("^[0-9]{1,"+precision+"}(\.[0-9]{1,"+scale+"})?$");
     patt1=new RegExp("^[0-9]{0,"+precision+"}(\.[0-9]{1,"+scale+"})?$");
  }
  if(patt1.test(value)){
    return true;
	} else {
		return false;
	}
}

function checkDateIfEntered(obj)
{   
	if (obj == null || obj.value == "")
	{
		return true;
	}
	else
	{		
		var strErrMsg = dateCheck(obj.value,'%dd-%mon-%yyyy');
				
		if (strErrMsg == '')
		{			
			return true;
		}
		else
		{
			return false;
		}
	}
}

function isNumeric(value) 
{
  if (value == null || !value.toString().match(/^[-]?\d*\.?\d*$/)) return false;
  return true;
}


function isContactNo(value) {
    if (value == null || !value.toString().match(/^[-]?\d*\-?\d*$/)) return false;
    return true;
}

function isPolicyNo(value)
{
    if(!value.toString().match(/^[A-Z0-9]{1}\d{9}$/)) return false;
    return true;
}

//function isPolicyNo(value)
//{
////var a=document.getElementById('TextBox1').value;
//    alert(value);
//var regex1=/^[A-Z]{5}\d{4}[A-Z]{1}$/;  //this is the pattern of regular expersion
//if(regex1.test(value)== false)
//{
//  alert('Please enter valid pan number');
//  return false;
//}
//else
//{
//    return true;
//}
//}


//function compare2Date(date1, date2)
//{
//	var strCompleteDate = date1;
//	if (strCompleteDate == '')
//		return true;
//	var strDatePart;
//	var strMonthPart;
//	var strYearPart;
//	
//	var intTemp1;
//	var intTemp2;
//	var strCurrDtDatePart;
//	var strCurrDtMonthPart;
//	var strCurrDtYearPart;
//	var strCurrentDate = date2;
//	var intCurrDtTemp1;
//	var intCurrDtTemp2;
//	
//	var currDate=new Date();

//	intTemp1 = strCompleteDate.indexOf("-");
//	intTemp2 = strCompleteDate.lastIndexOf("-");

//	strDatePart = strCompleteDate.substring(0,intTemp1);
//	strMonthPart = returnMonthNo(strCompleteDate.substring(intTemp1+1, intTemp2));
//	strYearPart = strCompleteDate.substr(intTemp2+1,4);
//	
//	intCurrDtTemp1 = strCurrentDate.indexOf("-");
//	intCurrDtTemp2 = strCurrentDate.lastIndexOf("-");

//	strCurrDtDatePart = strCurrentDate.substring(0,intCurrDtTemp1);
//	strCurrDtMonthPart = returnMonthNo(strCurrentDate.substring(intCurrDtTemp1+1, intCurrDtTemp2));
//	strCurrDtYearPart = strCurrentDate.substr(intCurrDtTemp2+1,4);
//	
//	if(new Date(strYearPart, strMonthPart, strDatePart)>new Date(strCurrDtYearPart, strCurrDtMonthPart, strCurrDtDatePart))
//	{
//		return false;
//	}
//	else
//	{
//		return true;
//	}
//}
function filterInput(filterType, evt, allowDecimal, allowCustom)
{ 
    var keyCode, Char, inputField, filter = ''; 
    var alpha = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
    var num   = '0123456789'; 
    // Get the Key Code of the Key pressed if possible else - allow 
    if(window.event){ 
    	keyCode = window.event.keyCode; 
		evt = window.event; 
    }
    else if (evt){
    	keyCode = evt.which;
    }
    else {
    	return true; 
    }
    
    // Setup the allowed Character Set 
    if(filterType == 0) filter = alpha; 
    else if(filterType == 1) filter = num; 
    else if(filterType == 2) filter = alpha + num; 
    if(allowCustom)filter += allowCustom; 
	if(filter == '')return true; 
    // Get the Element that triggered the Event 
    inputField = evt.srcElement ? evt.srcElement : evt.target || evt.currentTarget; 
    // If the Key Pressed is a CTRL key like Esc, Enter etc - allow 
    if((keyCode==null) || (keyCode==0) || (keyCode==8) || (keyCode==9) || (keyCode==13) || (keyCode==27) )return true; 
    // Get the Pressed Character 
    Char = String.fromCharCode(keyCode); 
    // If the Character is a number - allow 
    if((filter.indexOf(Char) > -1)) return true; 
    // Else if Decimal Point is allowed and the Character is '.' - allow 
    else if(filterType == 1 && allowDecimal && (Char == '.') && inputField.value.indexOf('.') == -1)return true; 
    else return false; 
}

function exportToExcel4(moduleType, tableName) {
	try {		
		var tableData = '';
		var table, rowVal = '';
		table = document.getElementById(tableName);
		if (table != null) {
			var rowCount = table.rows.length;			
			var colCount = table.rows[0].cells.length;
			row = table.rows[0];
			
			//<<Added by Prajakta on 6-Jul-2011 for issues in Sanity Testing	
			if(table.rows[2] == null || table.rows[2].cells.length == 1){
				alert("No Data available for Export.");
				return;
			}
			//>>
			
			//Getting the column headers.	
			//<<Added by Prajakta on 10-Feb-2012 for MF and Status report
			if(moduleType == 'EffectiveChanges' || moduleType == 'Symbols'  || moduleType == 'StatusReport'  || moduleType == 'PreToFinalApplications' || moduleType == 'RevocationIssue'){
				for ( var cnt1 = 0; cnt1 < colCount; cnt1++) {
				tableData = tableData + row.cells[cnt1].innerHTML + "|";
				}
			}	
			else if(moduleType == 'User'){
				for ( var cnt1 = 2; cnt1 < colCount; cnt1++) {
				tableData = tableData + row.cells[cnt1].innerHTML + "|";
				}
			}
			else{			
				for ( var cnt1 = 1; cnt1 < colCount; cnt1++) {
				tableData = tableData + row.cells[cnt1].innerHTML + "|";
				}
			}
			
			tableData = tableData + "~";
			//<<Added by Prajakta on 10-Feb-2012 for MF and Status report
			if(moduleType == 'EffectiveChanges' || moduleType == 'Symbols' || moduleType == 'StatusReport' || moduleType == 'PreToFinalApplications' || moduleType == 'RevocationIssue'){
				for ( var i = 0; i < rowCount - 2; i++) {
					row = table.rows[i + 2];
					rowVal = '';
					for ( var cnt1 = 0; cnt1 < colCount; cnt1++) {
						elementVal = row.cells[cnt1].innerHTML;
						cellElements1 = row.cells[cnt1].getElementsByTagName("span");
						if (cellElements1.length ==  1){
							elementVal = cellElements1[0].innerHTML;
						}
						cellElements = row.cells[cnt1].getElementsByTagName("a");
						if (cellElements.length ==  1){
							elementVal = cellElements[0].innerHTML;
						}
						/*elementVal = elementVal.replace("'", "&apos;");
						elementVal = elementVal.replace('"', "&quot;");
						elementVal = elementVal.replace("&", "&amp;");*/
						rowVal = rowVal + escape(elementVal) + "|";
					}
					tableData = tableData + rowVal + "~";
				}
			}
			else if(moduleType == 'User'){
				for ( var i = 2; i < rowCount - 2; i++) {
					row = table.rows[i + 2];
					rowVal = '';
					for ( var cnt1 = 2; cnt1 < colCount; cnt1++) {
						elementVal = row.cells[cnt1].innerHTML;
						cellElements = row.cells[cnt1].getElementsByTagName("a");
						if (cellElements.length ==  1){
							elementVal = cellElements[0].innerHTML;
						}
						/*elementVal = elementVal.replace("'", "&apos;");
						elementVal = elementVal.replace('"', "&quot;");
						elementVal = elementVal.replace("&", "&amp;");*/
						rowVal = rowVal + escape(elementVal) + "|";
					}
					tableData = tableData + rowVal + "~";
				}
			}
			
			
			else{
				for ( var i = 0; i < rowCount - 2; i++) {
					row = table.rows[i + 2];
					rowVal = '';
					for ( var cnt1 = 1; cnt1 < colCount; cnt1++) {
						elementVal = row.cells[cnt1].innerHTML;
						cellElements = row.cells[cnt1].getElementsByTagName("span");
						if (cellElements.length ==  1){
							elementVal = cellElements[0].innerHTML;
						}
						cellElements1 = row.cells[cnt1].getElementsByTagName("a");
						if (cellElements1.length ==  1){
							elementVal = cellElements1[0].innerHTML;
						}
						/*elementVal = elementVal.replace("'", "&apos;");
						elementVal = elementVal.replace('"', "&quot;");
						elementVal = elementVal.replace("&", "&amp;");*/
						rowVal = rowVal + escape(elementVal) + "|";
					}
					tableData = tableData + rowVal + "~";
				}
			}
			var url = "../doExportToExcel.aspx?moduleType=" + moduleType
				+ "&tableData=" + tableData;
			var content = "<iframe id='excelExport' name='excelExport' src='" + url
					+ "'display=hidden></iframe>";
			document.getElementById("container").innerHTML = content;
			return false;
		}
	} catch (e) {
		alert(e.message);
		return false;
	}
}

function validateNumbers(val, isNegativeAllowed, isDecimalAllowed)
 {
	var regExNum = "";

	if (isNegativeAllowed && isDecimalAllowed) 
	{
		//		regExNum = new RegExp("^[-]?[0-9]*[\.\d+]?$");
		//		regExNum = new RegExp("^[-]?[0-9]+[\.]?[0-9]+$");
		//		regExNum = new RegExp("^[-]?[0-9]+[\.]?[0-9]+$");
		//regExNum = new RegExp("^[-]?[0-9]+(\.[0-9]+)?$");
		regExNum = new RegExp("^[-]?[0-9]*(\.[0-9]+)?$");
	} else if (isNegativeAllowed && !isDecimalAllowed) {
		regExNum = new RegExp("^[-]?[0-9]+$");
	} else if (!isNegativeAllowed && isDecimalAllowed) {
		//regExNum = new RegExp("^[0-9]+[\.]?[0-9]+$");
		//regExNum = new RegExp("^[0-9]+(\.[0-9]+)?$");
		regExNum = new RegExp("^[0-9]*(\.[0-9]+)?$");
		//regExNum = new RegExp("^[0-9]+[\.]?[0-9]+$");
	} else if (!isNegativeAllowed && !isDecimalAllowed) {
		regExNum = new RegExp("^[0-9]+$");
	}

	//alert(regExNum);
	//alert(val);
	if (regExNum.test(val)) {
		return true;
	} else {
		return false;
	}
}
