<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewCircularAdd.aspx.cs" Inherits="Fiction2Fact.Projects.Circulars.ViewCircularAdd" %>


<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DSPAM</title>
    <asp:PlaceHolder runat="server">

        <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/jquery.min.js") %>"></script>
        <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/jQuery_3.6.0.js") %>"></script>
        <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/jQuery_Migrate_3_3_2.js") %>"></script>
        <script src="<%=Fiction2Fact.Global.site_url("Scripts/popper.js")%>" type="module"></script>
        <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/bootstrap.js") %>"></script>
        <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/jquery-ui.js")%>"></script>

        <link rel="stylesheet" href="<%=Fiction2Fact.Global.site_url("Content/assets/plugins/jvectormap/jquery-jvectormap-2.0.2.css") %>" />
        <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />

        <script type="text/javascript">
            var SiteTitleJS = '<%= Fiction2Fact.Global.site_title() %>';
            function SiteUrlJS(sPath) {
                if (sPath === undefined) { sPath = ''; }
                return '<%= Fiction2Fact.Global.site_url() %>' + sPath;
            }
            function ClientIDJQ(sId) {
                if (sId === undefined) { return; }
                return $('[id$="' + sId + '"]').attr('id');
            }
            function ClientIDJS(sId) {
                if (sId === undefined) { return; }
                var ele = document.querySelector('[id$="' + sId + '"]')
                if (ele === null) { return; }
                return ele.id;
            }
            $(document).ready(function () {
                $('input,textarea,select').not('[type="hidden"],[type="button"],[type="submit"]').first().focus();
            });
        </script>

        <script type="text/javascript">
            if (window.opener == null || window.opener.location == null) {
                window.location = "<%=Fiction2Fact.Global.site_url("Login.aspx") %>";
            }
        </script>
        <script type="text/javascript">

            /* Optional: Temporarily hide the "tabber" class so it does not "flash"
               on the page as plain HTML. After tabber runs, the class is changed
               to "tabberlive" and it will appear. */

            document.write('<style type="text/css">.tabber{display:none;}<\/style>');

            /*==================================================
              Set the tabber options (must do this before including tabber.js)
              ==================================================*/
            var tabberOptions = {
                'onLoad': function (argsObj) {
                    var t = argsObj.tabber;
                    var i;
                    i = document.getElementById('<%=hfTabberId.ClientID%>').value;
                    if (isNaN(i)) { return; }
                    t.tabShow(i);
                },

                'onClick': function (argsObj) {
                    var i = argsObj.index;
                    document.getElementById('<%=hfTabberId.ClientID%>').value = i;
                }
            };

            function tabChange(tabId) {
                if (tabId == 1) {

                    document.getElementById("lnkAdd").className = "nav-link active";
                    document.getElementById("add").className = "tab-pane p-3 active";

                    document.getElementById("lnkExisting").className = "nav-link";
                    document.getElementById("existing").className = "tab-pane p-3";

                }
                else {
                    document.getElementById("lnkAdd").className = "nav-link";
                    document.getElementById("add").className = "tab-pane p-3";

                    document.getElementById("lnkExisting").className = "nav-link active";
                    document.getElementById("existing").className = "tab-pane p-3 active";
                }
            }

            function onViewDetailClick(Id) {
                window.open('ViewCircularDetails.aspx?CircularId=' + Id);
                //window.open('ViewCircularDetails.aspx?CircularId=' + Id, "FILE2",
                //    "location=0,status=0,scrollbars=1,width=800,height=800,resizable=1");
            }

            function GetSelected() {
                //Reference the GridView.
                var grid = document.getElementById("gvCircularMaster");

                var Count = 0;

                if (grid != null) {
                    //Reference the CheckBoxes in GridView.
                    var checkBoxes = grid.getElementsByTagName("INPUT");

                    //Loop through the CheckBoxes.
                    for (var i = 0; i < checkBoxes.length; i++) {
                        if (checkBoxes[i].checked) {
                            Count = Count + 1;
                        }
                    }
                }

                return Count;

            }

    </script>
        <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/tabcontrol.js")%>"></script>
        <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js")%>"></script>
    </asp:PlaceHolder>
    <script type="text/javascript">
        function closePopupWindow() {
            var ID = document.getElementById('hfID').value;
            var Name = document.getElementById('hfName').value;
            var Type = document.getElementById('hfType').value;

            //if (Type == "SOP") {
            window.opener.document.getElementById('ctl00_ContentPlaceHolder1_hfCircularID').value = ID;
            window.opener.document.getElementById('ctl00_ContentPlaceHolder1_txtOldCircSubjectNo').value = Name;
            window.opener.document.getElementById('ctl00_ContentPlaceHolder1_hfCircularSubjects').value = Name;
            //window.opener.document.getElementById('ctl00_ContentPlaceHolder1_hfRegulation').value = Name;

            //}
            //else {
            //    window.opener.document.getElementById('ctl00_ContentPlaceHolder1_hfCircularID').value = ID;
            //    //window.opener.document.getElementById('ctl00_ContentPlaceHolder1_hfPertainingtoSOPID').value = Sopid;
            //    window.opener.document.getElementById('ctl00_ContentPlaceHolder1_txtPertainingtoRegName').value = Name;
            //    //window.opener.document.getElementById('ctl00_ContentPlaceHolder1_hfPertainingtoRegName').value = Name;
            //    //window.opener.document.getElementById('ctl00_ContentPlaceHolder1_txtPertainingtoSOPName').value = SopName;
            //    //window.opener.document.getElementById('ctl00_ContentPlaceHolder1_hfPertainingtoSOPName').value = SopName;
            //}

            window.close();
        }

        function closewindow() { window.close(); }

        function onAddClick() {

            if (parseInt(GetSelected()) == 0) {
                alert("Please select atleast one regulation.");
                return false;
            }

            var grid = document.getElementById("gvCircularMaster");

            if (grid != null) {
                var cnt = 1;
                var isAnyChkboxChecked = false;

                if (grid.rows.length > 0) {
                    for (var i = 1; i < grid.rows.length; i++) {
                        cnt++;

                        var controlIdPrefix;

                        if (cnt > 9) {
                            controlIdPrefix = 'gvCircularMaster_ctl';
                        }
                        else {
                            controlIdPrefix = 'gvCircularMaster_ctl0';
                        }

                        var cb = document.getElementById(controlIdPrefix + cnt + "_chkbx");

                        if (cb.checked) {
                            isAnyChkboxChecked = true;
                        }
                    }
                }
                if (!isAnyChkboxChecked) {
                    alert("Please select atleast one regulation.");
                    return false;
                }
            }
            else {
                alert("Please select atleast one regulation.");
                return false;
            }
        }

        function onClose() {
            var grid1 = document.getElementById("gvCircularMaster");
            var grid2 = document.getElementById("gvCircularUploaded");

            if (grid1 != null) {
                debugger;
                if (grid2 == null) {
                    if (!confirm("Are you sure, you want to close the window without adding any circulars?")) return false;
                }
                else {
                    var cnt = 1;
                    var isAnyChkboxChecked = false;

                    if (grid1.rows.length > 0) {
                        for (var i = 1; i < grid1.rows.length; i++) {
                            cnt++;

                            var controlIdPrefix;

                            if (cnt >= 9) {
                                controlIdPrefix = 'gvCircularMaster_ctl';
                            }
                            else {
                                controlIdPrefix = 'gvCircularMaster_ctl0';
                            }

                            var cb = document.getElementById(controlIdPrefix + cnt + "_chkbx");

                            if (cb.checked) {
                                isAnyChkboxChecked = true;
                            }
                        }

                        if (isAnyChkboxChecked) {
                            alert("Please add the selected circulars.");
                            return false;
                        }
                        else {
                            if (!confirm("Are you sure, you want to close the window with the added circulars?")) return false;
                        }
                    }
                }
            }
            else {
                if (!confirm("Are you sure, you want to close the window with the added circulars?")) return false;
            }
        }

        function compareEndSystemDates(source, arguments) {
            try {

                var Fromdate = document.getElementById('txtFromDate');
                var ToDate = document.getElementById('txtToDate');

                if (Fromdate.value != '') {
                    if (compare2Dates(ToDate, Fromdate) > 1) {
                        arguments.IsValid = false;
                    }
                    else {
                        arguments.IsValid = true;
                    }
                }
                else {
                    arguments.IsValid = true;
                }
            }
            catch (e) {
                alert(e);
                arguments.IsValid = false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>

        <div class="page-wrapper">

            <div class="page-content" style="margin-top: 0px">
                <div class="container-fluid">
                    <!-- Page-Title -->
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="page-title-box" style="padding: 20px 0 0 0;">
                                <div class="row">
                                    <div class="col">
                                        <h4 class="page-title">View Circulars</h4>
                                        <asp:Label ID="lblMsg" runat="server" CssClass="custom-alert-box"></asp:Label>
                                    </div>
                                    <!--end col-->
                                </div>
                                <!--end row-->
                            </div>
                            <!--end page-title-box-->
                        </div>
                        <!--end col-->
                    </div>
                    <!--end row-->
                    <!-- end page title end breadcrumb -->

                    <asp:HiddenField runat="server" ID="hfTabberId" />

                    <asp:HiddenField ID="hfSelectedRecord" runat="server" />
                    <asp:HiddenField ID="hfName" runat="server" />
                    <asp:HiddenField ID="hfID" runat="server" />
                    <asp:HiddenField ID="hfType" runat="server" />
                    <asp:HiddenField ID="hfActionType" runat="server" />
                    <asp:HiddenField ID="hfTopic" runat="server" />
                    <br />

                    <div class="row">
                        <div class="col-12">
                            <div class="card">
                                <div class="card-body">
                                    <div class="mb-3">
                                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack" runat="server" Text="Back" OnClientClick="closewindow()">
                                        <i class="fa fa-arrow-left me-2"></i> Back                   
                                        </asp:LinkButton>
                                    </div>
                                    <!-- Nav tabs -->
                                    <ul class="nav nav-tabs" role="tablist">
                                        <li class="nav-item">
                                            <a id="lnkAdd" class="nav-link active" data-bs-toggle="tab" href="#add" role="tab" aria-selected="true">Add Circulars</a>
                                        </li>
                                        <li class="nav-item">
                                            <a id="lnkExisting" class="nav-link" data-bs-toggle="tab" href="#existing" role="tab" aria-selected="false">Linked Circular</a>
                                        </li>
                                    </ul>
                                    <div class="tab-content">
                                        <div class="tab-pane p-3 active" id="add" role="tabpanel">
                                            <div class="row">
                                                <div class="col-md-4 mb-3">
                                                    <label class="form-label">From Date</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtFromDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imtFromDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" ToolTip="Select Date" />
                                                        <cc1:CalendarExtender ID="ceFrmDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imtFromDate"
                                                            TargetControlID="txtFromDate"></cc1:CalendarExtender>
                                                    </div>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtFromDate"
                                                        Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" span class="text-danger"
                                                        ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                                                </div>
                                                <div class="col-md-4 mb-3">
                                                    <label class="form-label">To Date</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtToDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imtToDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                        <cc1:CalendarExtender ID="ceTodate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imtToDate"
                                                            TargetControlID="txtToDate"></cc1:CalendarExtender>
                                                    </div>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtToDate"
                                                        Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" span class="text-danger"
                                                        ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                                                    <asp:CustomValidator ID="cvCompletionDate" runat="server" ClientValidationFunction="compareEndSystemDates"
                                                        ControlToValidate="txtToDate" ErrorMessage="From Date should be less than or Equal to To Date." span class="text-danger"
                                                        Display="Dynamic" OnServerValidate="cvTodate_ServerValidate" ValidationGroup="SEARCH"></asp:CustomValidator>
                                                </div>
                                                <div class="col-md-4 mb-3">
                                                    <label class="form-label">Global Search:</label>
                                                    <F2FControls:F2FTextBox ID="txtGlobalSearch" ToolTip="Implications" CssClass="form-control" runat="server"
                                                        Columns="30"></F2FControls:F2FTextBox>
                                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtGlobalSearch" />
                                                </div>
                                            </div>
                                            <div class="text-center mt-2">
                                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="LinkButton1" Text="Search" runat="server" ValidationGroup="SEARCH"
                                                    AccessKey="s" OnClick="btnSearch_Click">
                                            <i class="fa fa-search"></i> Search                          
                                                </asp:LinkButton>

                                                <asp:LinkButton ID="btnAddNew" CssClass="btn btn-outline-primary" Text="Add" runat="server"
                                                    OnClick="btnAddNew_Click" OnClientClick="return onAddClick();">
                                                <i class="fa fa-plus"></i> Add

                                                </asp:LinkButton>

                                                <%--<asp:Button ID="btnExportToExcel" runat="server" CssClass="Button" Text="Export to Excel" OnClick="btnExportToExcel_Click" />--%>
                                            </div>
                                            <div class="row mt-2">
                                                <asp:GridView ID="gvCircularMaster" runat="server" AutoGenerateColumns="False" DataKeyNames="CM_ID"
                                                    AllowSorting="true" AllowPaging="true" GridLines="None" OnSorting="gvCircularMaster_Sorting"
                                                    CellPadding="4" OnPageIndexChanging="gvCircularMaster_PageIndexChanging" OnRowDataBound="gvCircularMaster_RowDataBound"
                                                    OnSelectedIndexChanged="gvCircularMaster_SelectedIndexChanged" CssClass="table table-bordered footable"
                                                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">

                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCircularId" runat="server" Text='<%# Bind("CM_ID") %>' Visible="true"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkbx" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sr.No.">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="View Details">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <a class="btn btn-sm btn-soft-info btn-circle" onclick='<%# "onViewDetailClick("+ Eval("CM_ID")+")" %>'>
                                                                        <i class="fa fa-eye"></i>
                                                                    </a>
                                                                </center>
                                                                <asp:Label ID="lbId" Text='<% # Eval("CM_ID") %>' Visible="false" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Issuing Authority" SortExpression="CIA_NAME">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIssAuth" runat="server" Text='<%# Eval("CIA_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Topic" SortExpression="CIA_NAME">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblArea" runat="server" Text='<%# Eval("CAM_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Circular No" SortExpression="CM_CIRCULAR_NO" ControlStyle-Width="200px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCircNo" runat="server" Text='<%# Eval("CM_CIRCULAR_NO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Circular Date" SortExpression="CM_DATE">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCircularDate" runat="server" Text='<%# Eval("CM_DATE", "{0:dd-MMM-yyyy }") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Subject" SortExpression="CM_TOPIC" ControlStyle-Width="200px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTopic" runat="server" Text='<%# Eval("CM_TOPIC") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <div class="tab-pane p-3" id="existing" role="tabpanel">
                                            <div class="row mt-2">
                                                <asp:GridView ID="gvCircularUploaded" runat="server"
                                                    CellPadding="4" GridLines="None" AutoGenerateColumns="False" DataKeyNames="ID"
                                                    OnSelectedIndexChanged="gvCircularUploaded_SelectedIndexChanged" CssClass="table table-bordered footable"
                                                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Delete">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbDelete" runat="server" CommandName="Select">
                                                            <i class="fa fa-trash"></i>
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sr.No.">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="View Details">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <a class="btn btn-sm btn-soft-info btn-circle" onclick='<%# "onViewDetailClick("+ Eval("ID")+")" %>'>
                                                                        <i class="fa fa-eye"></i>
                                                                    </a>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Subject">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Topic">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblArea" runat="server" Text='<%# Eval("Topic") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Issuing Authority">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIssAuth" runat="server" Text='<%# Eval("Issuing Authority") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Circular No" ControlStyle-Width="200px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCircNo" runat="server" Text='<%# Eval("Circular No") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Circular Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCircularDate" runat="server" Text='<%# Eval("Circular Date", "{0:dd-MMM-yyyy }") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <div class="text-center mt-2 mb-3">
                                                <asp:LinkButton ID="btnDone" CssClass="btn btn-outline-primary" Text="Done" runat="server"
                                                    OnClick="btnDone_Click" OnClientClick="return onClose();"></asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- end row -->
                </div>
            </div>
        </div>
        <!-- jQuery  -->
        <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/jquery.min.js") %>"></script>
        <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/bootstrap.bundle.min.js") %>"></script>
        <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/metismenu.min.js") %>"></script>
        <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/waves.js") %>"></script>
        <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/feather.min.js") %>"></script>
        <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/simplebar.min.js") %>"></script>
        <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/moment.js") %>"></script>

        <!-- App js -->
        <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/custom.js") %>"></script>
        <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/app.js") %>"></script>
        <script src="<%=Fiction2Fact.Global.site_url("Content/assets/footable/js/footable.min.js") %>"></script>
    </form>
</body>
</html>
