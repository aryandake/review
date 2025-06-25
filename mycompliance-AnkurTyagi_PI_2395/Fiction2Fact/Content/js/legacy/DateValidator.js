function compare2Dates(date1, date2)
{
    var strDatePart1;
    var strMonthPart1;
    var strYearPart1;
    var strDatePart2;
    var strMonthPart2;
    var strYearPart2;
    var strDate1;
    var strDate2;
    var intTemp1;
    var intTemp2;
    var intTemp3;
    var intTemp4;
    strDate1 = date1.value;
    strDate2 = date2.value;
    
 //    strDate1 = date1;
//    strDate2 = date2;
		
    intTemp1 = strDate1.indexOf("-");
    intTemp2 = strDate1.lastIndexOf("-");
	
    intTemp3 = strDate2.indexOf("-");
    intTemp4 = strDate2.lastIndexOf("-");

    strDatePart1 = strDate1.substring(0,intTemp1);
    strMonthPart1 = strDate1.substring(intTemp1+1, intTemp2);
    strYearPart1 = strDate1.substr(intTemp2+1,4);
	
    strDatePart2 = strDate2.substring(0,intTemp3);
    strMonthPart2 = strDate2.substring(intTemp3+1, intTemp4);
    strYearPart2 = strDate2.substr(intTemp4+1,4);
    
      
    
	var dt1 = new Date(strYearPart1, getNumericValueForMonth(strMonthPart1), strDatePart1);
    var dt2 = new Date(strYearPart2, getNumericValueForMonth(strMonthPart2), strDatePart2);
      


    
    if(dt1.getTime() > dt2.getTime())
    {   
    
        return 0;
    }
    else if(dt1.getTime() == dt2.getTime())
    {
    
        return 1;
    }
    else if(dt1.getTime() < dt2.getTime())
    {
    
        return 2;
    }
    

}

function compare2DatesNew(date1, date2) {
    var strDatePart1;
    var strMonthPart1;
    var strYearPart1;
    var strDatePart2;
    var strMonthPart2;
    var strYearPart2;
    var strDate1;
    var strDate2;
    var intTemp1;
    var intTemp2;
    var intTemp3;
    var intTemp4;
    //strDate1 = date1.value;
    //strDate2 = date2.value;

    strDate1 = date1;
    strDate2 = date2;

    intTemp1 = strDate1.indexOf("-");
    intTemp2 = strDate1.lastIndexOf("-");

    intTemp3 = strDate2.indexOf("-");
    intTemp4 = strDate2.lastIndexOf("-");

    strDatePart1 = strDate1.substring(0, intTemp1);
    strMonthPart1 = strDate1.substring(intTemp1 + 1, intTemp2);
    strYearPart1 = strDate1.substr(intTemp2 + 1, 4);

    strDatePart2 = strDate2.substring(0, intTemp3);
    strMonthPart2 = strDate2.substring(intTemp3 + 1, intTemp4);
    strYearPart2 = strDate2.substr(intTemp4 + 1, 4);



    var dt1 = new Date(strYearPart1, getNumericValueForMonth(strMonthPart1), strDatePart1);
    var dt2 = new Date(strYearPart2, getNumericValueForMonth(strMonthPart2), strDatePart2);




    if (dt1.getTime() > dt2.getTime()) {

        return 0;
    }
    else if (dt1.getTime() == dt2.getTime()) {

        return 1;
    }
    else if (dt1.getTime() < dt2.getTime()) {

        return 2;
    }


}

function getNumericValueForMonth(monthName)
{
    switch (monthName)
	{
	    case 'Jan':
	        return 0;
	        break;
	    case 'Feb':
	        return 1;
	        break;
	    case 'Mar':
	        return 2;
	        break;
	    case 'Apr':
	        return 3;
	        break;
	    case 'May':
	        return 4;
	        break;
	    case 'Jun':
	        return 5;
	        break;
	    case 'Jul':
	        return 6;
	        break;
	    case 'Aug':
	        return 7;
	        break;
	    case 'Sep':
	        return 8;
	        break;
	    case 'Oct':
	        return 9;
	        break;
	    case 'Nov':
	        return 10;
	        break;
	    case 'Dec':
	        return 11;
	        break;
	}
}
