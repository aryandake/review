// JavaScript Document

// DropDownMenu by Miha Hribar
// http://hribar.info

function addLoadEvent(func) {
    var oldonload = window.onload;
    if (typeof window.onload != 'function') {
        window.onload = func;
    } else {
        window.onload = function() {
            oldonload();
            func();
        }
    }
}

function prepareMenu() {
    // first lets make sure the browser understands the DOM methods we will be using
  	if (!document.getElementsByTagName) return false;
  	if (!document.getElementById) return false;
  	
  	// lets make sure the element exists
  	if (!document.getElementById("menu")) return false;
  	var menu = document.getElementById("menu");
  	
  	// for each of the li on the root level check if the element has any children
  	// if so append a function that makes the element appear when hovered over
  	var root_li = menu.getElementsByTagName("li");
  	for (var i = 0; i < root_li.length; i++) {
  	    var li = root_li[i];
  	    
  	    // search for children
  	    var child_ul = li.getElementsByTagName("ul");
  	    if (child_ul.length >= 1) {
  	        //alert(li.innerHTML);
  	    //li.innerHTML = "<span>" + li.innerHTML + "<img src='../images/right.gif' border='0' /></span>";
  	    	
  	    	// we have children - append hover function to the parent
  	        li.onmouseover = function () {
  	           if (!this.getElementsByTagName("ul")) return false;
  	            var ul = this.getElementsByTagName("ul");
  	            ul[0].style.display = "block";
  	            return true;
  	        }
  	        li.onmouseout = function () {
  	            if (!this.getElementsByTagName("ul")) return false;
  	            var ul = this.getElementsByTagName("ul");
  	            ul[0].style.display = "none";
  	            return true;
  	        }
  	    }
  	}
  	
  	return true;
}

//var timeout = 5;
//var closetimer = 0;
//var ddmenuitem = 0;
////var menuDiv = document.getElementById("menuDiv");
//
////cancel close timer
//function mcancelclosetime() {
//	if (closetimer) {
//		window.clearTimeout(closetimer);
//		closetimer = null;
//	}
//}
//
//function MM_swapImgRestore() { //v3.0
//	var i, x, a = document.MM_sr;
//	for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++)
//		x.src = x.oSrc;
//}
//
//function MM_preloadImages() { //v3.0
//	var d = document;
//	if (d.images) {
//		if (!d.MM_p)
//			d.MM_p = new Array();
//		var i, j = d.MM_p.length, a = MM_preloadImages.arguments;
//		for (i = 0; i < a.length; i++)
//			if (a[i].indexOf("#") != 0) {
//				d.MM_p[j] = new Image;
//				d.MM_p[j++].src = a[i];
//			}
//	}
//}
//
//function MM_findObj(n, d) { //v4.01
//	var p, i, x;
//	if (!d)
//		d = document;
//	if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
//		d = parent.frames[n.substring(p + 1)].document;
//		n = n.substring(0, p);
//	}
//	if (!(x = d[n]) && d.all)
//		x = d.all[n];
//	for (i = 0; !x && i < d.forms.length; i++)
//		x = d.forms[i][n];
//	for (i = 0; !x && d.layers && i < d.layers.length; i++)
//		x = MM_findObj(n, d.layers[i].document);
//	if (!x && d.getElementById)
//		x = d.getElementById(n);
//	return x;
//}
//
//function MM_swapImage() { //v3.0
//	var i, j = 0, x, a = MM_swapImage.arguments;
//	document.MM_sr = new Array;
//	for (i = 0; i < (a.length - 2); i += 3)
//		if ((x = MM_findObj(a[i])) != null) {
//			document.MM_sr[j++] = x;
//			if (!x.oSrc)
//				x.oSrc = x.src;
//			x.src = a[i + 2];
//		}
//}
//
//// open hidden layer
//function mopen(id) {
//	try{
//	//alert('here' + id);
//	if (id == 'menu1')
//		MM_swapImage('imgHome', '', '../images/top_buttons/home_1_but.gif', 1);
//	else if (id == 'menu2')
//		MM_swapImage('imgMaster', '', '../images/top_buttons/master_1_but.gif', 1);
//	else if (id == 'menu3')
//		MM_swapImage('imgIssue', '', '../images/top_buttons/issue_1_but.gif', 1)
//	else if (id == 'menu4')
//		MM_swapImage('imgReports', '', '../images/top_buttons/report_1_but.gif', 1)
//	else if (id == 'menu5')
//		MM_swapImage('imgHelp', '', '../images/top_buttons/help_1_but.gif', 1)
//		// cancel close timer
//	mcancelclosetime();
//
//	// close old layer
//	if (ddmenuitem)
//		ddmenuitem.style.visibility = 'hidden';
//
//	// get new layer and show it
//	ddmenuitem = document.getElementById(id);
//
//	ddmenuitem.style.visibility = 'visible';
//	} catch(e){
//		alert(e.message);
//	}
//}
//
//// close showed layer
//function mclose() {
//	if (ddmenuitem)
//		ddmenuitem.style.visibility = 'hidden';
//}
//
//// go close timer
//function mclosetime() {
//	MM_swapImgRestore();
//	closetimer = window.setTimeout(mclose, timeout);
//}
//
//// close layer when click-out
//document.onclick = mclose;

addLoadEvent(prepareMenu);