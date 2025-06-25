<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Async="true" Inherits="Fiction2Fact.Projects.Certification.Certification_Certification1" Title="Quarterly Certifications" CodeBehind="Certification.aspx.cs" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="a" Namespace="Fiction2Fact.Controls" Assembly="RequiredIfValidatorRadioButtonList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href='<%= Fiction2Fact.Global.site_url("Content/js/legacy/jquery-ui.css") %>' rel="stylesheet" />
    
    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/Exception.js")%>'></script>
    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js") %>'></script>
    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator1.js") %>'></script>
    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/dateformatvalidation.js")%>'></script>
    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/commonvalidations.js") %>'></script>
    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/tabcontrol.js")%>'></script>
    
    <script type="text/javascript">
        document.write('<style type="text/css">.tabber{display:none;}<\/style>');
        /* Set the tabber options (must do this before including tabber.js)*/
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
    </script>

    <script type="text/javascript">
        function checkCertificationStatus() {
            var certStatus = document.getElementById('<%= hfCertStatus.ClientID %>').value;
            if (certStatus == '') {
                alert('Please First Click on "Save Draft" button and than click on "Export to Excel".');
                return false;
            }
        }

        function onChklistClick() {

            document.getElementById('ctl00_ContentPlaceHolder1_hfSeeChecklistStatus').value = 'Y';
            var deptId = document.getElementById('ctl00_ContentPlaceHolder1_hfDepartmentID').value

            window.open('CertificationChecklistDetails.aspx?Id=' + deptId,
                'FILE', 'location=0,status=0,scrollbars=1,width=800,height=600');
            return false;
        }

        function compareTargetDtSystemDates(source, arguments) {
            try {
                var cnt = 0;
                var TargetDate;
                var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurDate');
                //alert('SystemDate: '+SystemDate.value);

                var grid = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist");
                if (grid != null) {
                    if (grid.rows.length > 0) {
                        for (var j = 2; j < grid.rows.length + 1; j++) {
                            cnt++;
                            if (j < 10) {
                                j = "0" + j;
                            } else {
                                j = j;
                            }
                            TargetDate = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_txtTargetDate");

                            if (TargetDate != null) {
                                //alert(TargetDate.value);
                                //alert(compare2Dates(SystemDate,TargetDate));
                                if (compare2Dates(SystemDate, TargetDate) == 0) {
                                    //alert("False");
                                    arguments.IsValid = false;
                                    break;
                                }
                                else {
                                    //alert("True");
                                    arguments.IsValid = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (e) {
                alert(e);
                arguments.IsValid = true;
            }
        }
        <%--<<Added by Ankur Tyagi on 05Feb2024 for CR_1948--%>
        function compareNCSinceDtSystemDates(source, arguments) {
            try {
                var cnt = 0;
                var TargetDate;
                var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurDate');
                //alert('SystemDate: '+SystemDate.value);

                var grid = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist");
                if (grid != null) {
                    if (grid.rows.length > 0) {
                        for (var j = 2; j < grid.rows.length + 1; j++) {
                            cnt++;
                            if (j < 10) {
                                j = "0" + j;
                            } else {
                                j = j;
                            }
                            NCSince = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_txtNCSinceDate");

                            if (NCSince != null) {
                                //alert(TargetDate.value);
                                //alert(compare2Dates(SystemDate,TargetDate));
                                if (compare2Dates(NCSince, SystemDate) == 0) {
                                    //alert("False");
                                    arguments.IsValid = false;
                                    break;
                                }
                                else {
                                    //alert("True");
                                    arguments.IsValid = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (e) {
                alert(e);
                arguments.IsValid = true;
            }
        }
        //>>

        //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
        function onClientImportfromExcelClick() {

            var popup = window.open('ImportComplianceChecklist.aspx', '', 'fullscreen');

            if (popup.outerWidth < screen.availWidth || popup.outerHeight < screen.availHeight) {
                popup.moveTo(0, 0);
                popup.resizeTo(screen.availWidth, screen.availHeight);
            }
            return false;
        }
        //>>

    </script>

    <script type="text/javascript">

        function openpopupChecklistAttachments(rowNo, Type) {
            window.open("./UploadChecklistFile.aspx?rowNo=" + rowNo + "&Type=" + Type, "FILE",
                "location=0,status=0,scrollbars=0,width=600,height=250");
            return false;
        }

        function deleteChecklistAttachments(rowNo, Type) {
            if (rowNo >= 10) {
                controlIdPrefix = 'ctl00_ContentPlaceHolder1_gvChecklist_ctl';
            }
            else {
                controlIdPrefix = 'ctl00_ContentPlaceHolder1_gvChecklist_ctl0';
            }
            var filename = document.getElementById(controlIdPrefix + rowNo + '_ServerFileName').value;
            var ChecklistDetsId = document.getElementById(controlIdPrefix + rowNo + '_hfChecklistDetsId').value;
            if (!confirm('Are you sure that you want to delete this file?')) return false;
            window.open(
                "./DeleteChecklistFile.aspx?Type=ChecklistFile&rowNo="
                + rowNo + "&filename=" + filename + "&Id=" + ChecklistDetsId,
                "FILE", "location=0,status=0,scrollbars=0,width=10,height=10");
            return false;
        }

        function getFileLink(ServerFileName, ClientFileName, intSrNo) {
            var serverfilename = document.getElementById(ServerFileName).value;
            var clientFileName = document.getElementById(ClientFileName).value;
            if (!serverfilename == '') {
                document.getElementById('AttachFileImg' + intSrNo).style.visibility = 'hidden';
                document.getElementById('DeleteFileImg' + intSrNo).style.visibility = 'visible';

                var alink = document.getElementById('Filelink' + intSrNo);
                alink.innerHTML = clientFileName;
                alink.href = '../CommonDownloadAnyFile.aspx?type=ChecklistFilesFolder&downloadFileName=' + serverfilename + '&fileName=' + clientFileName;
                alink.onclick = '';
            }
            else {
                document.getElementById('DeleteFileImg' + intSrNo).style.visibility = 'hidden';
                document.getElementById('AttachFileImg' + intSrNo).style.visibility = 'visible';
            }
        }

        function onClientViewCircClick(CMId) {
            window.open('../Circulars/ViewCircularDetails.aspx?CircularId=' + CMId, '', 'location=0,status=0,resizable=1,scrollbars=1,width=1000,height=800');
            return false;
        }
    </script>

    <script type="text/javascript">
        //Added by Milan yadav on 09-Aug-2016
        //>>
        function openViewChecklistPopup(requestId) {

            //alert(requestId);
            window.open('../Certification/ViewChecklistData.aspx?ChecklistId=' + requestId,
                '', 'location=0,status=0,scrollbars=1,resizable=1,width=700,height=500');
            return false;
        }
        //<<
    </script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/jquery-ui.min.js")%>'></script>

    <script type="text/javascript">
        jQuery.browser = {};
        (function () {
            jQuery.browser.msie = false;
            jQuery.browser.version = 0;
            if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
                jQuery.browser.msie = true;
                jQuery.browser.version = RegExp.$1;
            }
        })();
    </script>

    <script>
        var $j = jQuery.noConflict();
        $j(document).ready(function () {
            $j('.targetDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd-M-yy'
            });
        });
    </script>

    <style>
        .custom-span-input {
            height: auto;
            min-height: 38px;
            background-color: #f9f9f9;
        }
    </style>

    <asp:HiddenField ID="hfSeeChecklistStatus" runat="server" />
    <asp:HiddenField ID="hfQuarterId" runat="server" />
    <asp:HiddenField ID="hfCertMId" runat="server" />
    <asp:HiddenField ID="hfUserFullName" runat="server" />
    <asp:HiddenField ID="hfExceptions" runat="server" />
    <asp:HiddenField ID="hfSymbol" runat="server" />
    <asp:HiddenField ID="hfCertId" runat="server" />
    <asp:HiddenField runat="server" ID="hfTabberId" />
    <asp:HiddenField ID="hfQuarterEndDt" runat="server" />
    <asp:HiddenField ID="hfCertDepartment" runat="server" />
    <asp:HiddenField ID="hfEmailId" runat="server" />
    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField ID="hfDepartmentID" runat="server" />
    <asp:HiddenField ID="hfCertStatus" runat="server" />
    <asp:HiddenField ID="hfQuarter" runat="server" />
    <asp:HiddenField ID="hfCurDate" runat="server" />
    <asp:HiddenField ID="hfCurrentDate" runat="server" />
    <asp:HiddenField ID="hfCXOName" runat="server" />
    <asp:HiddenField ID="hfUnitHead" runat="server" />
    <asp:HiddenField ID="hfUnitName" runat="server" />
    <asp:HiddenField ID="hfUnitID" runat="server" />
    <asp:HiddenField ID="hfFunctionId" runat="server" />
    <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">
                            <asp:Label runat="server" ID="lblHeader"></asp:Label></h4>
                        <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
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

    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-body">
                    <asp:Panel ID="pnlCertificationDashboards" runat="server" Visible="true">
                        <div class="mb-3">
                            <asp:Label ID="Label1" runat="server" Text="Quarterly Compliance Certificate Checklist for the quarter is pending for submission."
                                CssClass="custom-info-alert"></asp:Label>
                        </div>
                        <div class="mb-3">
                            <asp:Label ID="Label2" runat="server" Text="Please click on the 'View' icon to view the quarterly compliance checklist."
                                CssClass="custom-info-alert"></asp:Label>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="gvCertDashboard" runat="server" BorderStyle="None" BorderWidth="1px"
                                CellPadding="4" GridLines="None" AutoGenerateColumns="false" CssClass="table table-bordered footable"
                                OnSelectedIndexChanged="gvCertDashboard_SelectedIndexChanged" DataKeyNames="CSSDM_ID"
                                AllowSorting="false" AllowPaging="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="View" ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Select"
                                                CssClass="btn btn-sm btn-soft-info btn-circle">
                                                <i class="fa fa-eye"></i>	
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sub Unit Name" SortExpression="CSSDM_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDeptName" runat="server" Text='<%# Bind("CSSDM_NAME") %>' Visible="true"></asp:Label>
                                            <asp:Label ID="lblCertId" runat="server" Text='<%# Bind("CERT_ID") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quarter">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuarter" runat="server" Text='<%# Eval("CQM_FROM_DATE","{0:dd-MMM-yyyy}").ToString() + " to " + Eval("CQM_TO_DATE","{0:dd-MMM-yyyy}").ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Status" DataField="Status" SortExpression="Status" />
                                    <asp:BoundField HeaderText="Revision Suggested By" DataField="Rejection_By" />
                                    <asp:TemplateField HeaderText="Revision Suggested On">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRejectionOn" runat="server" Text='<%# Eval("Rejection_On","{0:dd-MMM-yyyy}").ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Suggested Revision">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblRejectionRemarks" runat="server" Text='<%# Eval("Rejection_Comments").ToString().Replace("\n", "<br />") %>'
                                                Visible="true"></asp:Label>--%>
                                            <asp:Label ID="lblRejectionRemarks" Width="300px" runat="server" ToolTip='<%#Eval("Rejection_Comments").ToString()%>'
                                                Text='<%#Eval("Rejection_Comments").ToString().Length > 200 ? (Eval("Rejection_Comments") as string).Substring(0, 200).Replace("\n", "<br />") 
                                                    + " ..." : Eval("Rejection_Comments").ToString().Replace("\n", "<br />") %>'
                                                Visible="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PnlCertStatus" runat="server" Visible="false">
                        <div class="mb-3">
                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSaveDraft1" runat="server" OnClientClick="return getExceptionDetails()" ValidationGroup="Save"
                                Text="Save Draft" OnClick="btnSaveDraft_Click">
                                <i class="fa fa-save me-2"></i> Save Draft                    
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSubmit1" runat="server" Text="Submit" OnClientClick="return getExceptionDetailsOnSave();" ValidationGroup="Save"
                                OnClick="btnSubmit_Click">
                                 <i class="fa fa-save me-2"></i> Submit
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack1" runat="server" Text="Back" OnClick="btnBack_Click">
                                <i class="fa fa-arrow-left me-2"></i> Back
                            </asp:LinkButton>
                        </div>

                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link active" data-bs-toggle="tab" href="#home" role="tab" aria-selected="true">Certification</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" data-bs-toggle="tab" href="#profile" role="tab" aria-selected="false">Compliance Checklist</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" data-bs-toggle="tab" href="#settings" role="tab" aria-selected="false">Compliance Deviations</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" data-bs-toggle="tab" href="#settings1" role="tab" aria-selected="false">Regulatory Filing</a>
                            </li>
                        </ul>

                        <!-- Tab panes -->
                        <div class="tab-content">
                            <div class="tab-pane p-3 active" id="home" role="tabpanel">
                                <asp:Label runat="server" ID="lblCertContents"></asp:Label>
                            </div>
                            <div class="tab-pane p-3" id="profile" role="tabpanel">
                                <asp:Panel ID="pnlChecklistShow" runat="server">
                                    <div class="mb-3" id="divExportImport" runat="server" visible="false">
                                        <asp:LinkButton runat="server" ID="btnExportToExcel" CssClass="btn btn-outline-secondary" OnClick="btnExportToExcel_Click" ToolTip="Export to Excel">
                                            <i class="fa fa-download"></i> Export to Excel               
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="lbImportfromExcel" CssClass="btn btn-outline-secondary" ToolTip="Import from Excel" OnClientClick="return onClientImportfromExcelClick()">
                                         <i class="fa fa-upload"></i> Import from Excel               
                                        </asp:LinkButton>
                                    </div>
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvChecklist" runat="server" BorderStyle="None" BorderWidth="1px"
                                            CellPadding="4" GridLines="None" AutoGenerateColumns="false" CssClass="table table-bordered footable"
                                            DataKeyNames="CCM_ID" OnRowDataBound="gvChecklist_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Sr. No.
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfChecklistDetsId" runat="server" Value='<%#Eval("CCD_ID") %>' />
                                                        <asp:HiddenField ID="hfChecklistFileId" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="ClientFileName" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="ServerFileName" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hfaction" runat="server" Value='<%#Eval("CCD_YES_NO_NA") %>'></asp:HiddenField>
                                                        <center>
                                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="View Circulars">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:LinkButton ID="lnkViewCirc" runat="server" CssClass="btn btn-sm btn-soft-success btn-circle">
                                                                <%--OnClientClick='<%# string.Format("onClientViewCircClick(\"{0}\");", Eval("CCM_CM_ID")) %>'--%>
                                                                <i class="fa fa-eye"></i>
                                                            </asp:LinkButton>
                                                            <asp:HiddenField ID="hfCircularId" runat="server" Value='<%#Eval("CCM_CM_ID") %>' />
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Act/Regulation/Circular">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActRegCirc" Text='<%#Eval("CDTM_TYPE_OF_DOC") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reference Circular/Notification/Act">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtRegulations"
                                                            Text='<%#Eval("CCM_REFERENCE").ToString().Length>100?(Eval("CCM_REFERENCE") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_REFERENCE").ToString().Replace("\n", "<br />")  %>'
                                                            ToolTip='<%#Eval("CCM_REFERENCE")%>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Section/Clause">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkbtntxtSections"
                                                            runat="server" Text='<%#Eval("CCM_CLAUSE").ToString().Length>100?(Eval("CCM_CLAUSE") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_CLAUSE").ToString().Replace("\n", "<br />")  %>'
                                                            ToolTip='<%#Eval("CCM_CLAUSE")%>' Font-Underline="false" CssClass="badge rounded-pill badge-soft-pink" OnClientClick='<%# string.Format("return openViewChecklistPopup(\"{0}\");", Eval("CCD_ID")) %>'> </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Compliance of/Heading of Compliance checklist">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtCheckpoints"
                                                            Text='<%#Eval("CCM_CHECK_POINTS").ToString().Length>100?(Eval("CCM_CHECK_POINTS") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_CHECK_POINTS").ToString().Replace("\n", "<br />")  %>'
                                                            ToolTip='<%#Eval("CCM_CHECK_POINTS")%>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtSelfAssessmentStatus"
                                                            Text='<%#Eval("CCM_PARTICULARS").ToString().Length>100?(Eval("CCM_PARTICULARS") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_PARTICULARS").ToString().Replace("\n", "<br />")  %>'
                                                            ToolTip='<%#Eval("CCM_PARTICULARS")%>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Consequences of non Compliance">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPenalty"
                                                            Text='<%#Eval("CCM_PENALTY").ToString().Length>100?(Eval("CCM_PENALTY") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_PENALTY").ToString().Replace("\n", "<br />")  %>'
                                                            ToolTip='<%#Eval("CCM_PENALTY")%>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Frequency" DataField="CCM_FREQUENCY" HeaderStyle-Width="200px" />
                                                <asp:BoundField HeaderText="Forms" DataField="CCM_FORMS" HeaderStyle-Width="200px" />
                                                <asp:TemplateField HeaderText="Compliance Status" HeaderStyle-Width="250px">
                                                    <ItemTemplate>
                                                        <asp:DropDownList CssClass="form-select" Width="160px" AppendDataBoundItems="true" ID="ddlrbyesnona"
                                                            runat="server" DataTextField="RC_NAME" DataValueField="RC_CODE">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="Remarks">--%>
                                                <asp:TemplateField HeaderText="Remarks / Reason of non compliance">
                                                    <ItemTemplate>
                                                        <F2FControls:F2FTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="120px" Rows="5" CssClass="form-control"
                                                            Columns="20" Text='<%#Eval ("CCD_REMARKS") %>'></F2FControls:F2FTextBox>
                                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<< Added by Amarjeet on 28-Jul-2021--%>
                                                <%--<<Added by Ankur Tyagi on 05Feb2024 for CR_1948--%>
                                                <asp:TemplateField HeaderText="Non-compliant since">
                                                    <ItemTemplate>
                                                        <div style="width: 160px;">
                                                            <div class="input-group">
                                                                <F2FControls:F2FTextBox ID="txtNCSinceDate" runat="server" CssClass="form-control" MaxLength="11"
                                                                    Columns="8" Text='<%#Bind("CCD_NC_SINCE_DT","{0:dd-MMM-yyyy}") %>'></F2FControls:F2FTextBox>
                                                                <asp:ImageButton ToolTip="PopUp Calendar" CssClass="custom-calendar-icon" runat="server" ImageUrl="~/Content/images/legacy/calendar.jpg"
                                                                    ID="imgNCSince" OnClientClick="return false" />
                                                            </div>
                                                        </div>
                                                        <cc1:CalendarExtender ID="ceNCSinceDt" runat="server" PopupButtonID="imgNCSince"
                                                            TargetControlID="txtNCSinceDate" Format="dd-MMM-yyyy"></cc1:CalendarExtender>

                                                        <asp:RegularExpressionValidator ID="revNCSinceDate" runat="server" ControlToValidate="txtNCSinceDate" CssClass="text-danger"
                                                            ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                                            ValidationGroup="Save" Display="Dynamic">Date Format has to be dd-MMM-yyyy.</asp:RegularExpressionValidator>

                                                        <asp:CustomValidator ID="cvNCSinceDt" runat="server" ClientValidationFunction="compareNCSinceDtSystemDates" CssClass="text-danger"
                                                            ControlToValidate="txtNCSinceDate" ErrorMessage="Non Compliant Date should be less than or equal to System Date."
                                                            Display="Dynamic" OnServerValidate="cvTargetDt_ServerValidate" ValidationGroup="Save">Non-compliant since Date should be less than or equal to System Date.</asp:CustomValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-->>--%>
                                                <asp:TemplateField HeaderText="Action Plan">
                                                    <ItemTemplate>
                                                        <F2FControls:F2FTextBox ID="txtActionPlan" runat="server" TextMode="MultiLine" Rows="5" Width="120px"
                                                            CssClass="form-control" Text='<%#Eval("CCD_ACTION_PLAN") %>'></F2FControls:F2FTextBox>
                                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtActionPlan" />
                                                        <asp:RegularExpressionValidator ID="revActionPlan" ControlToValidate="txtActionPlan" CssClass="text-danger"
                                                            Display="Dynamic" Text="Exceeding 4000 characters" ValidationExpression="^[\s\S]{0,4000}$"
                                                            runat="server" SetFocusOnError="True" ValidationGroup="Save" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Target Date">
                                                    <ItemTemplate>
                                                        <div style="width: 160px;">
                                                            <div class="input-group">
                                                                <F2FControls:F2FTextBox ID="txtTargetDate" runat="server" CssClass="form-control" MaxLength="11"
                                                                    Columns="8" Text='<%#Bind("CCD_TARGET_DATE","{0:dd-MMM-yyyy}") %>'></F2FControls:F2FTextBox>
                                                                <asp:ImageButton ToolTip="PopUp Calendar" CssClass="custom-calendar-icon" runat="server" ImageUrl="~/Content/images/legacy/calendar.jpg"
                                                                    ID="ImageButton2" OnClientClick="return false" />
                                                            </div>
                                                        </div>
                                                        <asp:RegularExpressionValidator ID="revTargetDate" runat="server" ControlToValidate="txtTargetDate" CssClass="text-danger"
                                                            ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                                            ValidationGroup="Save" Display="Dynamic">Date Format has to be dd-MMM-yyyy.</asp:RegularExpressionValidator>
                                                        <cc1:CalendarExtender ID="ceTargetDt" runat="server" PopupButtonID="ImageButton2"
                                                            TargetControlID="txtTargetDate" Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                                                        <asp:CustomValidator ID="cvTargetDt" runat="server" ClientValidationFunction="compareTargetDtSystemDates" CssClass="text-danger"
                                                            ControlToValidate="txtTargetDate" ErrorMessage="Target Date should be greater than or equal to System Date."
                                                            Display="Dynamic" OnServerValidate="cvTargetDt_ServerValidate" ValidationGroup="Save">Target Date should be greater than or equal to System Date.</asp:CustomValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-->>--%>
                                                <asp:TemplateField HeaderText="Attach Files" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" ControlStyle-Width="100px">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <center>
                                                            <a style="visibility: hidden;" id="AttachFileImg<%# Container.DataItemIndex + 2 %>"
                                                                onclick="return openpopupChecklistAttachments('<%# Container.DataItemIndex + 2 %>','ChecklistFile');">
                                                                <img alt="" border="0" src="../../Content/images/legacy/attach_1.png" />
                                                            </a>
                                                            <a style="visibility: hidden;" class="btn btn-sm btn-soft-danger btn-circle" id="DeleteFileImg<%# Container.DataItemIndex + 2 %>"
                                                                onclick="return deleteChecklistAttachments('<%# Container.DataItemIndex + 2  %>','ChecklistFile')">
                                                                <i class="fa fa-trash"></i>
                                                            </a>
                                                        </center>
                                                        <a id="Filelink<%# Container.DataItemIndex + 2  %>" visible="true" style="display: block;"></a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>

                            </div>
                            <div class="tab-pane p-3" id="settings" role="tabpanel">
                                <div class="mb-3">
                                    <asp:Label ID="lblMsg1" runat="server" Text="Note: Please enter Target Date in DD-MON-YYYY Format. " CssClass="custom-info-alert"></asp:Label>
                                </div>
                                <div class="mb-3">
                                    <asp:LinkButton runat="server" ID="imgAdd" CssClass="btn btn-sm btn-soft-primary btn-circle" OnClientClick="return addExceptionRow()">
                                        <i class="fa fa-plus"></i>	                            
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="imgDelete" CssClass="btn btn-sm btn-soft-danger btn-circle" OnClientClick="return deleteExceptionRow()">
                                        <i class="fa fa-trash"></i>	
                                    </asp:LinkButton>
                                </div>
                                <div class="table-responsive">
                                    <asp:Literal ID="litControls" runat="server"></asp:Literal>
                                </div>
                                <div id="tblRejctionRemarks" runat="server" class="mt-3">
                                    <div class="row">
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Rejection Remarks, if any:</label>
                                            <asp:Label runat="server" ID="lblRejectionRemarks" CssClass="form-control custom-span-input"></asp:Label>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Remarks, if any:</label>
                                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtRemarks" runat="server" TextMode="MultiLine"
                                                Rows="5" Columns="120" Width="100%"></F2FControls:F2FTextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane p-3" id="settings1" role="tabpanel">
                                <div class="table-responsive">
                                    <asp:Literal runat="server" ID="litRegulatoryFilling" />
                                </div>
                            </div>
                        </div>
                        <div class="mt-3">
                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSaveDraft" runat="server" OnClientClick="return getExceptionDetails()" ValidationGroup="Save"
                                Text="Save Draft" OnClick="btnSaveDraft_Click">
                                <i class="fa fa-save me-2"></i> Save Draft   
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return getExceptionDetailsOnSave();" ValidationGroup="Save"
                                OnClick="btnSubmit_Click">
                                <i class="fa fa-save me-2"></i> Submit
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click">
                                <i class="fa fa-arrow-left me-2"></i> Back
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->

</asp:Content>
