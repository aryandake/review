<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" Async="true"
    EnableEventValidation="true" Inherits="Fiction2Fact.Projects.Circulars.NewCircularMaster" Title="Upload New Circular"
    CodeBehind="NewCircularMaster.aspx.cs" ValidateRequest="false" %>

<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="custom" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="aspajax" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .ck-editor__editable {
            min-height: 250px;
        }
    </style>
    <script src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/checkduplicateCircularNo.js")%>"></script>
    <script src="<%=Fiction2Fact.Global.site_url("Scripts/ckeditor/ckeditor.js")%>"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            ClassicEditor
                .create(document.querySelector('#ctl00_ContentPlaceHolder1_FCKE_CircularDetails'), {
                    // toolbar: [ 'heading', '|', 'bold', 'italic', 'link' ]
                })
                .then(editor => {
                    window.editor = editor;
                    editor.ui.view.editable.element.style.height = '250px';
                })
                .catch(err => {
                    console.error(err.stack);
                });

            ClassicEditor
                .create(document.querySelector('#ctl00_ContentPlaceHolder1_FCKE_Implications'), {
                    // toolbar: [ 'heading', '|', 'bold', 'italic', 'link' ]
                })
                .then(editor => {
                    window.editor = editor;
                    editor.ui.view.editable.element.style.height = '250px';
                })
                .catch(err => {
                    console.error(err.stack);
                });
        })
    </script>

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/commonvalidations.js")%>"></script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js")%>"></script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/dateformatvalidation.js")%>"></script>
    <%--<link type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/css/legacy/ui-lightness/jquery-ui-1.8.19.custom.css")%>" rel="stylesheet" />--%>
    <%--<script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/jquery-ui-1.12.1.js")%>"></script>--%>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/Actionables.js")%>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            try {
                //<< onRequirementForTheBoardChange
                onRequirementForTheBoardChange();

                $("[id*='ddlRequirementForTheBoard']").change(() => {
                    onRequirementForTheBoardChange();
                });
                //>>

                //<< onLinkageWithEarlierCircularChange
                onLinkageWithEarlierCircularChange();

                $("[id*='ddlLinkageWithEarlierCircular']").change(() => {
                    onLinkageWithEarlierCircularChange();
                });
                //>>

                //<< To be intimated to
                $("[id*='cbSelectAll']").change((event) => {
                    $("[id*='cbSubmissions']").prop('checked', $(event.currentTarget).is(":checked"));
                });

                $("[id*='cbSubmissions']").change(() => {
                    $("[id*='cbSelectAll']").prop('checked',
                        ($('[id*="cbSubmissions"]').length - 1) === $('[id*="cbSubmissions"]:checked').length);
                });
                //>>

                //<< onSendMailForClick
                $('[id*="btnSendMailFor"]').click(() => {
                    var lblSendMailAuditTrail = $("[id*='lblSendMailAuditTrail']").html();
                    var lblSendMailAuditTrailDecoded = "";
                    try {
                        lblSendMailAuditTrailDecoded = atob(lblSendMailAuditTrail);
                    } catch (e) {
                        lblSendMailAuditTrailDecoded = lblSendMailAuditTrail;
                    }
                    $("[id*='lblSendMailAuditTrail']").html(lblSendMailAuditTrailDecoded);
                    $("#divSendMailForModal").modal('show');
                    return false;
                });
                //>>




                //<< ValidateSendMail
                //$('[id*="btnSubmit"]').click(() => {
                //    if (Page_ClientValidate('SendMailFor')) {
                //        $('[id*="btnSubmit"]').attr('disabled', 'disabled');
                //        $('[id*="btnSubmit"]').css({ "background-color": "#d2d2d2" });
                //        $('[id*="btnSubmit"]').val("Please wait...");
                //    }
                //});
                //>>
            } catch (e) {
                alert(e);
            }
        })

        const onRequirementForTheBoardChange = () => {
            if ($("[id*='ddlRequirementForTheBoard']").val() == "Y") {
                $("#trToBePlacedBefore").css({ "visibility": "visible", "display": "table-row" });
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_cvToBePlacedBefore'), true);

                $("#trDetails").css({ "visibility": "visible", "display": "table-row" });
                //ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvDetails'), true);

                $("#trNameOfThePolicy").css({ "visibility": "visible", "display": "table-row" });
                //ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvNameOfThePolicy'), true);

                $("#trFrequency").css({ "visibility": "visible", "display": "table-row" });
                //ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvFrequency'), true);
            }
            else {
                $("#trToBePlacedBefore").css({ "visibility": "hidden", "display": "none" });
                clearCheckboxList('ctl00_ContentPlaceHolder1_cbToBePlacedBefore');
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_cvToBePlacedBefore'), false);

                $("#trDetails").css({ "visibility": "hidden", "display": "none" });
                $("[id*='txtDetails']").val("");
                //ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvDetails'), false);

                $("#trNameOfThePolicy").css({ "visibility": "hidden", "display": "none" });
                $("[id*='txtNameOfThePolicy']").val("");
                //ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvNameOfThePolicy'), false);

                $("#trFrequency").css({ "visibility": "hidden", "display": "none" });
                $("[id*='txtFrequency']").val("");
                //ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvFrequency'), false);
            }
        }

        const onLinkageWithEarlierCircularChange = () => {
            if ($("[id*='ddlLinkageWithEarlierCircular']").val() == "Y") {
                $("#trSOCEOC").css({ "visibility": "visible", "display": "table-row" });
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvSOCEOC'), true);
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_cvOldCircSubjectNo'), true);

                $("#trOldCircSubjectNo").css({ "visibility": "visible", "display": "table-row" });
            }
            else {
                $("#trSOCEOC").css({ "visibility": "hidden", "display": "none" });
                $("[id*='ddlSOCEOC']").val("");
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvSOCEOC'), false);
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_cvOldCircSubjectNo'), false);

                $("#trOldCircSubjectNo").css({ "visibility": "hidden", "display": "none" });
                $("[id*='dlUserList'] tr:not(:first-child)").remove();
                $("[id*='hfOldCircularId']").val("");
            }
        }
    </script>
    <script type="text/javascript">
        function validateBroadcastIntimation() {
            var atLeast = 1;
            var CHK = document.getElementById("<%=cbSubmissions.ClientID%>");
            var checkbox = CHK.getElementsByTagName("input");
            var counter = 0;
            //addActionableDets();
            for (var i = 0; i < checkbox.length; i++) {
                if (checkbox[i].checked) {
                    counter++;
                }
            }

            if (atLeast > counter) {
                alert("Please select to be Intimated To.");
                return false;
            }
            else {
                var IsDoubleClickFlagSet = document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value;
                if (IsDoubleClickFlagSet == 'Yes') {
                    alert("You have double clicked on the same button. " +
                        "Please wait till the operation is successfully completed.");
                    return false;
                }
                else {
                    document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                    return true;
                }
            }
        }

    </script>

    <script type="text/javascript">
        function validateSaveAndBroadcastIntimation() {
            addActionableDets();
            var atLeast = 1;
            var CHK = document.getElementById("<%=cbSubmissions.ClientID%>");
            var checkbox = CHK.getElementsByTagName("input");
            var counter = 0;
            for (var i = 0; i < checkbox.length; i++) {
                if (checkbox[i].checked) {
                    counter++;
                }
            }
            if (atLeast > counter) {
                alert("Please select to be Intimated To.");
                return false;
            }
            else {
                var IsDoubleClickFlagSet = document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value;
                if (IsDoubleClickFlagSet == 'Yes') {
                    alert("You have double clicked on the same button. " +
                        "Please wait till the operation is successfully completed.");
                    return false;
                }
                else {
                    document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                    return true;
                }
            }
        }
    </script>

    <script type="text/javascript">
        function compareEndSystemDates(source, arguments) {
            try {
                var Fromdate = document.getElementById('ctl00_ContentPlaceHolder1_txtCircularDate');
                var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurDate');

                if (compare2Dates(Fromdate, SystemDate) == 0) {
                    arguments.IsValid = false;
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

        function test() {
            var edit = document.getElementById("ctl00_ContentPlaceHolder1_hfSelectedRecord").value;
            if (edit == '') {
                let code = '';
                code = document.getElementById("ctl00_ContentPlaceHolder1_txtCircularNo").value;

                if (code == '') {
                    $("#divCircError").css({ "visibility": "hidden", "display": "none" });
                    document.getElementById("divCircError").innerHTML = "";
                    document.getElementById("ctl00_ContentPlaceHolder1_hfDuplicateFlag").value = "N"
                }
            }
        }

        function onClientValidateSave() {

            var chkMailTo = document.getElementById('<%=txtAdditionalEmailsTO.ClientID%>');
            if (chkMailTo != null) {
                var ToMailId = document.getElementById('<%=txtAdditionalEmailsTO.ClientID%>').value;
                if (ToMailId != '') {
                    alert('Please select mail id from the populated list.');
                    return false;
                }
            }

            var chkMailCC = document.getElementById('<%=txtAdditionalEmails.ClientID%>');
            if (chkMailCC != null) {
                var CCMailId = document.getElementById('<%=txtAdditionalEmails.ClientID%>').value;
                if (CCMailId != '') {
                    alert('Please select mail id from the populated list.');
                    return false;
                }
            }

            //var tableRowsCount = document.getElementById("tblAttachment").rows.length;
            //if (parseInt(tableRowsCount) <= 1) {
            //    alert('Please enter atleast one attachment.');
            //    document.getElementById('tblAttachment').focus();
            //    return false;
            //}

            var validated = Page_ClientValidate('SaveCircularDetails');
            var DuplicateFlag = document.getElementById('<%=hfDuplicateFlag.ClientID%>').value;
            var IsDoubleClickFlagSet = document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value;

            var edit = document.getElementById("ctl00_ContentPlaceHolder1_hfSelectedRecord").value;
            if (edit == '') {
                let code = '';
                code = document.getElementById("ctl00_ContentPlaceHolder1_txtCircularNo").value;

                if (code == '') {
                    $("#divCircError").css({ "visibility": "hidden", "display": "none" });
                    document.getElementById("divCircError").innerHTML = "";
                    document.getElementById("ctl00_ContentPlaceHolder1_hfDuplicateFlag").value = "N"
                }
            }

            if (validated) {

                if (DuplicateFlag == 'Y') {
                    alert('Circular no. already exists please enter different circular number.');
                    document.getElementById('ctl00_ContentPlaceHolder1_txtCircularNo').focus();
                    return false;
                }

                //var actData = addActionableDets();
                //var attachment = $('#' + ClientIDJS('hfAttachment')).val();
                //if (attachment == '') {
                //    alert('Please add attachment');
                //    return false;
                //}
                //return true;
                var ddlLinkageWithEarlierCircular = document.getElementById('ctl00_ContentPlaceHolder1_ddlLinkageWithEarlierCircular').value;
                var table = document.getElementById("ctl00_ContentPlaceHolder1_dlUserList");
                var flag = true;

                //if (ddlLinkageWithEarlierCircular == 'Y') {
                //    if (table != null) {
                //        var rowCount = table.rows.length;

                //        if (rowCount <= 0) {
                //            flag = false;
                //        }
                //    }
                //    else {
                //        flag = false;
                //    }
                //}
                if (ddlLinkageWithEarlierCircular == 'Y') {
                    if (!flag) {
                        alert('Please enter Old Circular Subject/No.');
                        return flag;
                    }
                }

                if (IsDoubleClickFlagSet == 'Yes') {
                    alert("You have double clicked on the same button. " +
                        "Please wait till the operation is successfully completed.");
                    return false;
                }
                else {
                    document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                    //return true;

                }
                return getAttachmentData();
            }
            else {
                return false;
            }
        }

        function OldCircSubjectNoClientItemSelected(source, eventArgs) {
            var hdnValueID = "<%= hfOldCircularId.ClientID %>";
            document.getElementById(hdnValueID).value = eventArgs.get_value();
            __doPostBack(hdnValueID, "");
        }

        function AdditionalEmailsTOClientItemSelected(source, eventArgs) {
            var hdnAddValueID = "ctl00_ContentPlaceHolder1_hfAdditionalEmailsTO";
            document.getElementById(hdnAddValueID).value = eventArgs.get_value();
            __doPostBack(hdnAddValueID, "");
        }

        function AdditionalEmailsCCClientItemSelected(source, eventArgs) {
            var hdnAddValueID = "ctl00_ContentPlaceHolder1_hfAdditionalEmailsCC";
            document.getElementById(hdnAddValueID).value = eventArgs.get_value();
            __doPostBack(hdnAddValueID, "");
        }

        function validToBePlacedBefore(source, arg) {
            var atLeast = 1;
            var valid = false;
            var CHK = document.getElementById("<%=cbToBePlacedBefore.ClientID%>");
            if (CHK != null) {
                var checkbox = CHK.getElementsByTagName("input");
                var counter = 0;
                for (var i = 0; i < checkbox.length; i++) {
                    if (checkbox[i].checked) {
                        counter++;
                    }
                }
                if (atLeast <= counter) {
                    valid = true;
                } else { valid = false; }
            }
            else {
                valid = false;
            }
            arg.IsValid = valid;
        }

        function validateOldCircularSubjectNo(source, arg) {
            var valid = false;
            var ddlLinkageWithEarlierCircular = document.getElementById('ctl00_ContentPlaceHolder1_ddlLinkageWithEarlierCircular').value;

            var OldCircList = document.getElementById('ctl00_ContentPlaceHolder1_txtOldCircSubjectNo').value;
            if (ddlLinkageWithEarlierCircular == 'Y') {
                if (OldCircList == '') {
                    valid = false;
                }
                else {
                    valid = true;
                }
            }


            //var table = document.getElementById("ctl00_ContentPlaceHolder1_dlUserList");

            //if (ddlLinkageWithEarlierCircular == 'Y') {
            //    if (table != null) {
            //        var rowCount = table.rows.length;

            //        if (rowCount <= 0) {
            //            valid = false;
            //        }
            //        else {
            //            valid = true;
            //        }
            //    }
            //    else {
            //        valid = false;
            //    }
            //}
            //else {
            //    valid = true;
            //}

            arg.IsValid = valid;
        }

        function validateSendMailFor(source, arg) {
            var atLeast = 1;
            var valid = false;
            var CHK = document.getElementById("ctl00_ContentPlaceHolder1_cbSendMailFor");
            if (CHK != null) {
                var checkbox = CHK.getElementsByTagName("input");
                var counter = 0;
                for (var i = 0; i < checkbox.length; i++) {
                    if (checkbox[i].checked) {
                        counter++;
                    }
                }
                if (atLeast <= counter) {
                    valid = true;
                } else { valid = false; }
            }
            else {
                valid = false;
            }
            arg.IsValid = valid;
        }

        function clearCheckboxList(cblControl) {
            var checkboxList = document.getElementById(cblControl);

            if (checkboxList != null) {
                var checkboxCount = checkboxList.getElementsByTagName("input");

                for (var i = 0; i < checkboxCount.length; i++) {
                    checkboxCount[i].checked = false;
                }
            }

            return false;
        }

        function onOthersClick(Type) {
            var hfSelectedRecord = document.getElementById("ctl00_ContentPlaceHolder1_hfSelectedRecord").value;
            if (hfSelectedRecord == "0" || hfSelectedRecord == "") {
                alert("Please save the Circular details.");
            }
            else {
                if (Type == 'CAP') {
                    window.open('AddCircularActionable.aspx?CirId=' + hfSelectedRecord, '_blank');
                }
                else if (Type == 'CCC') {
                    window.open('AddCircularCertChecklists.aspx?CirId=' + hfSelectedRecord, '_blank');
                }
                else if (Type == 'UCCC') {
                    window.open('../Certification/UploadChecklistData.aspx?Type=CIRC&CircId=' + hfSelectedRecord, '_blank');
                }
                else if (Type == 'CRR') {
                    window.open('../Submissions/CommonSubmission.aspx?Type=CIRC&CircId=' + hfSelectedRecord, '_blank');
                }
                else if (Type == 'VCRR') {
                    window.open('../Submissions/ListOfReports.aspx?Type=CIRC&CircId=' + hfSelectedRecord, '_blank');
                }
            }

            return false;
        }

        $(document).ready(function () {
            var CircularId = $("#<%= hfSelectedRecord.ClientID%>").val();
            if (CircularId != '' && CircularId != 0) {
                /* $("#ctl00_ContentPlaceHolder1_btnRefresh").css({ "background-color": "#08315f" });*/
                $("#ctl00_ContentPlaceHolder1_btnRefresh").prop('disabled', false);
            }
            else {
                /*$("#ctl00_ContentPlaceHolder1_btnRefresh").css({ "background-color": "#d2d2d2" });*/
                $("#ctl00_ContentPlaceHolder1_btnRefresh").prop('disabled', true);
            }
        })


        function openpopup() {
            var ID = document.getElementById('<%=hfCircularID.ClientID%>').value;
            var RegulationName = document.getElementById('<%=txtOldCircSubjectNo.ClientID%>').value;

            //window.open("../Circulars/ViewCircularAdd.aspx?Type=SOP&ID=" + ID + "&ActionType=''&Text=" + RegulationName);

            window.open("<%=Fiction2Fact.Global.site_url("/Projects/Circulars/ViewCircularAdd.aspx")%>" + "?Type=SOP&ID=" + ID + "&ActionType=''&Text=" + RegulationName, "FILE2",
                "location=0,status=0,scrollbars=1,width=800,height=800,resizable=1");

        }

        function SelectFirst() {
            var list = document.getElementById("ctl00_ContentPlaceHolder1_ddlSArea").options.length;
            if (list == 2) {
                $('#ctl00_ContentPlaceHolder1_ddlSArea').prop('selectedIndex', 1);
            }
        }

        function onClientValidateSendMail() {
            var IsDoubleClickFlagSet = document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value;
            var validated = Page_ClientValidate('SendMailFor');
            if (validated) {
                getAttachmentData();

                if (IsDoubleClickFlagSet == 'Yes') {
                    alert("You have double clicked on the same button. " +
                        "Please wait till the operation is successfully completed.");
                    return false;
                }
                else {
                    document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                    return true;
                }
            }
            else {
                return false;
            }
        }

    </script>

    <asp:HiddenField ID="hfCircularID" runat="server" />
    <asp:HiddenField ID="hfCircularSubjects" runat="server" />

    <asp:HiddenField ID="hfDuplicateFlag" runat="server" />

    <asp:HiddenField ID="hfFileNameOnServer" runat="server" />
    <asp:HiddenField ID="hfActionables" runat="server" />
    <asp:HiddenField ID="hfAttachment" runat="server" />
    <asp:HiddenField ID="hfActionableStatus" runat="server" />
    <asp:HiddenField runat="server" ID="hfCurDate" />
    <asp:HiddenField ID="hfSelectedRecord" runat="server" />
    <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Create New Circular</h4>
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
                <asp:Panel ID="pnlCircular" runat="server" Visible="true">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Created by Department: <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlDepartment" CssClass="form-select" runat="server" DataTextField="CDM_NAME"
                                    DataValueField="CDM_ID" AppendDataBoundItems="True" ToolTip="Created by Department">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" ControlToValidate="ddlDepartment"
                                    ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                    ErrorMessage="Please select Department."></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-4 mb-3" id="divLob" runat="server" visible="false">
                                <label class="form-label">Line of Business: <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlLOB" CssClass="form-select" runat="server" DataTextField="LEM_NAME"
                                    DataValueField="LEM_ID" AppendDataBoundItems="True" ToolTip="Line of Business">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB" Enabled="false"
                                    ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                    ErrorMessage="Please select Line of Business."></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-4 mb-3">
                                <label class="form-label">Linkage with earlier circular: <span class="text-danger">*</span></label>
                                <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlLinkageWithEarlierCircular"
                                    runat="server" ToolTip="Linkage with earlier circular">
                                </f2f:DropdownListNoValidation>
                                <asp:RequiredFieldValidator ID="rfvLinkageWithEarlierCircular" runat="server" ControlToValidate="ddlLinkageWithEarlierCircular"
                                    CssClass="text-danger" ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please select Linkage with earlier circular."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 mb-3" id="trSOCEOC" style="visibility: hidden; display: none;">
                                <label class="form-label">Supersedes or Extends/Amends Old Circular(s): <span class="text-danger">*</span></label>
                                <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlSOCEOC" DataValueField="RC_CODE" DataTextField="RC_NAME"
                                    runat="server" ToolTip="Supersedes or Extends/Amends Old Circular(s)">
                                </f2f:DropdownListNoValidation>
                                <asp:RequiredFieldValidator ID="rfvSOCEOC" runat="server" ControlToValidate="ddlSOCEOC"
                                    CssClass="text-danger" ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please select Supersedes or Extends/Amends Old Circular(s)."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-12 mb-3" id="trOldCircSubjectNo" style="visibility: hidden; display: none;">
                                <label class="form-label">
                                    Old Circular Subject/No.: <span class="text-danger">*</span>&nbsp;&nbsp;<a onclick="return openpopup()" class="btn btn-outline-primary"><i class="fa fa-plus"></i> Add
                                    </a>
                                </label>
                                <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>--%>
                                <div>
                                    <asp:DataList ID="dlUserList" runat="server" CellPadding="2" CssClass="custom-datalist"
                                        Font-Underline="False" HorizontalAlign="Left" RepeatDirection="Horizontal" Width="100%">
                                        <FooterStyle Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle Font-Bold="True" ForeColor="White" />
                                        <ItemStyle Font-Size="Small" CssClass="items" />
                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" VerticalAlign="Top" Wrap="true" />
                                        <ItemTemplate>
                                            <div class="input-group mb-2">
                                                <span class="form-control custom-span-input"><%# Eval("Name") %></span>
                                                <asp:LinkButton ToolTip="Remove" ID="ImageButton1" runat="server" CssClass="btn btn-sm btn-soft-danger btn-circle d-flex align-self-stretch align-items-center"
                                                    OnClick="ImageButton1_Click">
                                                                 <i class="fa fa-trash"></i> 
                                                </asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </div>
                                <F2FControls:F2FTextBox ID="txtOldCircSubjectNo" ReadOnly="true" CssClass="form-control" runat="server" Rows="3" TextMode="MultiLine"
                                    MaxLength="4000" ToolTip="Old Circular Subject/No.">
                            </F2FControls:F2FTextBox>
                                <ajaxToolkit:AutoCompleteExtender ServiceMethod="getCircularDetailsFromSubjectAndCircularNo" MinimumPrefixLength="2"
                                    ServicePath="AJAXDropdownCirculars.asmx" EnableCaching="false" runat="server"
                                    TargetControlID="txtOldCircSubjectNo" ID="aceOldCircSubjectNo" FirstRowSelected="true"
                                    CompletionListItemCssClass="cssListItem" CompletionListHighlightedItemCssClass="cssListItemHighlight"
                                    OnClientItemSelected="OldCircSubjectNoClientItemSelected">
                                </ajaxToolkit:AutoCompleteExtender>
                                <asp:HiddenField ID="hfOldCircularId" runat="server" OnValueChanged="hfOldCircularId_ValueChanged" />
                                <asp:CustomValidator ID="cvOldCircSubjectNo" ValidationGroup="SaveCircularDetails" CssClass="text-danger"
                                    ClientValidationFunction="validateOldCircularSubjectNo" Display="Dynamic"
                                    ErrorMessage="Please enter Old Circular Subject/No." runat="server"></asp:CustomValidator>
                                <%--</ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Issuing Authority: <span class="text-danger">*</span></label>
                                <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlCircularAuthority"
                                    runat="server" ToolTip="Issuing Authority">
                                </f2f:DropdownListNoValidation>
                                <asp:RequiredFieldValidator ID="rfvCircularAuthority" runat="server" ControlToValidate="ddlCircularAuthority" CssClass="text-danger"
                                    ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please select Issuing Authority."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Topic: <span class="text-danger">*</span></label>
                                <f2f:DropdownListNoValidation ID="ddlArea" CssClass="form-select" runat="server"
                                    ToolTip="Topics">
                                </f2f:DropdownListNoValidation>
                                <asp:RequiredFieldValidator ID="rfvArea" runat="server" ControlToValidate="ddlArea"
                                    ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                    ErrorMessage="Please select Topic."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Type of Document: <span class="text-danger">*</span></label>
                                <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlTypeofCircular" AppendDataBoundItems="true"
                                    runat="server" ToolTip="Type of Circular" DataTextField="CDTM_TYPE_OF_DOC" DataValueField="CDTM_ID">
                                </f2f:DropdownListNoValidation>
                                <asp:RequiredFieldValidator ID="rfvddlTypeofCircular" runat="server" ControlToValidate="ddlTypeofCircular" CssClass="text-danger"
                                    ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please select Type of Circular."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Private / Public : <span class="text-danger">*</span></label>
                                <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlSubTypeofCircular"
                                    AppendDataBoundItems="true" runat="server" ToolTip="Sub Type of Circular"
                                    DataTextField="RC_NAME" DataValueField="RC_CODE">
                                </f2f:DropdownListNoValidation>
                                <span id="SubTypeDesc" style="color: blue;"></span>
                                <asp:RequiredFieldValidator ID="rfvddlSubTypeofCircular" runat="server" ControlToValidate="ddlSubTypeofCircular" CssClass="text-danger"
                                    ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please select Public, Private or Semi-private."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Circular No.: <span class="text-danger">*</span></label>
                                <F2FControls:F2FTextBox ID="txtCircularNo" CssClass="form-control" runat="server"
                                    MaxLength="50" ToolTip="Circular Number">
                                </F2FControls:F2FTextBox>
                                <p id="divCircError" style="display: none" class="text-danger"></p>
                                <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtCircularNo" />
                                <asp:RequiredFieldValidator ID="rfvCircularNo" runat="server" ControlToValidate="txtCircularNo"
                                    ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                    ErrorMessage="Please enter Circular No."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Circular Date: <span class="text-danger">*</span></label>
                                <div class="input-group">
                                    <F2FControls:F2FTextBox ID="txtCircularDate" CssClass="form-control" runat="server"
                                        MaxLength="11" ToolTip="Circular Date"></F2FControls:F2FTextBox>
                                    <asp:ImageButton ID="imgCircularDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                        ImageUrl="../../Content/images/legacy/calendar.jpg" ToolTip="Select Date" />

                                </div>
                                <asp:RequiredFieldValidator ID="rfvCircularDate" runat="server" ControlToValidate="txtCircularDate" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="SaveCircularDetails" SetFocusOnError="True"
                                    ErrorMessage="Please enter Circular Date."></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revCircularDate" runat="server" ControlToValidate="txtCircularDate"
                                    ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" CssClass="text-danger"
                                    ValidationGroup="SaveCircularDetails" Display="Dynamic"></asp:RegularExpressionValidator>

                                <asp:CustomValidator ID="cvFromDate" runat="server" ClientValidationFunction="compareEndSystemDates"
                                    ControlToValidate="txtCircularDate" ErrorMessage="Circular Date should be less than System Date." CssClass="text-danger"
                                    Display="Dynamic" OnServerValidate="cvTodate_ServerValidate" ValidationGroup="SaveCircularDetails"></asp:CustomValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Circular Effective Date: <span class="text-danger">*</span></label>
                                <div class="input-group">
                                    <F2FControls:F2FTextBox ID="txtCircEffDate" CssClass="form-control" runat="server"
                                        MaxLength="11" ToolTip="Circular Effective Date"></F2FControls:F2FTextBox>
                                    <asp:ImageButton ID="imgCircEffDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                        ImageUrl="../../Content/images/legacy/calendar.jpg" ToolTip="Select Date" />
                                </div>
                                <asp:RequiredFieldValidator ID="rfvCircEffDate" runat="server" ControlToValidate="txtCircEffDate" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="SaveCircularDetails" SetFocusOnError="True"
                                    ErrorMessage="Please enter Circular Effective Date."></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revCircEffDate" runat="server" ControlToValidate="txtCircEffDate"
                                    ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" CssClass="text-danger"
                                    ValidationGroup="SaveCircularDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-8 mb-3">
                                <label class="form-label">Subject of the Circular: <span class="text-danger">*</span></label>
                                <F2FControls:F2FTextBox ID="txtTopic" ToolTip="Subject of the Circular" CssClass="form-control"
                                    runat="server" MaxLength="200" Columns="64">
                    </F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtTopic" />
                                <asp:RequiredFieldValidator ID="rfvTopic" runat="server" ControlToValidate="txtTopic"
                                    ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                    ErrorMessage="Please enter Subject of the Circular."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-12 mb-3">
                                <label class="form-label">SPOC From Compliance Department: <%--<span class="text-danger">*</span>--%></label>
                                <asp:Panel ID="PnlSPOCFCF" runat="server">
                                    <div class="custom-checkbox-table">
                                        <asp:CheckBoxList ID="cbSpocFromCompFn" RepeatColumns="6" runat="server" DataTextField="CCS_NAME" CssClass="form-control"
                                            DataValueField="CCS_ID" AppendDataBoundItems="True" ToolTip="SPOC From Compliance Department" RepeatDirection="Horizontal">
                                        </asp:CheckBoxList>
                                    </div>
                                </asp:Panel>
                                <%--<asp:DropDownList ID="ddlSpocFromCompFn" CssClass="form-select" runat="server" DataTextField="CCS_NAME"
                                    DataValueField="CCS_ID" AppendDataBoundItems="True" ToolTip="Created By">
                                </asp:DropDownList>--%>
                                <%--<asp:RequiredFieldValidator ID="rfvSpocFromCompFn" runat="server" ControlToValidate="ddlSpocFromCompFn" CssClass="text-danger"
                                    ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please select SPOC From Compliance Department"></asp:RequiredFieldValidator>--%>
                            </div>
                            <div class="col-md-12 mb-3">
                                <label class="form-label">Associated Keywords:</label>
                                <asp:Panel ID="pnlAssociatedKeywords" runat="server">
                                    <div class="custom-checkbox-table">
                                        <asp:CheckBoxList ID="cbAssociatedKeywords" RepeatColumns="6" runat="server" DataTextField="CKM_NAME" CssClass="form-control"
                                            DataValueField="CKM_ID" AppendDataBoundItems="True" ToolTip="Associated Keywords" RepeatDirection="Horizontal">
                                        </asp:CheckBoxList>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="col-md-12 mb-3">
                                <label class="form-label">Gist of the Circular:</label>
                                <F2FControls:F2FTextBox runat="server" ID="FCKE_CircularDetails" TextMode="MultiLine" CssClass="ckeditor"></F2FControls:F2FTextBox>
                            </div>
                            <div class="col-md-12 mb-3">
                                <label class="form-label">Implications:</label>
                                <F2FControls:F2FTextBox runat="server" ID="FCKE_Implications" TextMode="MultiLine" CssClass="ckeditor"></F2FControls:F2FTextBox>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Issuer Link:</label>
                                <F2FControls:F2FTextBox ID="txtLink" ToolTip="Issuer Link" CssClass="form-control"
                                    runat="server" MaxLength="200" Columns="64">
                                </F2FControls:F2FTextBox>
                                <%--<F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtLink" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="._-:/&=" />--%>
                                <F2FControls:F2FFilteredHyperlinks runat="server" TargetControlID="txtLink" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" />
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Requirement for the Board/Audit committee to approve:</label>
                                <asp:DropDownList ID="ddlRequirementForTheBoard" CssClass="form-select" runat="server"
                                    ToolTip="Requirement for the Board/Audit committee to approve">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvRequirementForTheBoard" runat="server" ControlToValidate="ddlRequirementForTheBoard"
                                    CssClass="text-danger" ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please select Requirement for the Board/Audit committee to approve"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-12 mb-3" id="trToBePlacedBefore" style="visibility: hidden; display: none;">
                                <label class="form-label">To be placed before: <span class="text-danger">*</span></label>
                                <asp:Panel ID="pnlToBePlacedBefore" runat="server">
                                    <div class="custom-checkbox-table">
                                        <asp:CheckBoxList ID="cbToBePlacedBefore" RepeatColumns="4" runat="server" DataTextField="RC_NAME" CssClass="form-control"
                                            DataValueField="RC_CODE" AppendDataBoundItems="True" ToolTip="To be placed before" RepeatDirection="Horizontal">
                                        </asp:CheckBoxList>
                                    </div>
                                </asp:Panel>
                                <asp:CustomValidator ID="cvToBePlacedBefore" ValidationGroup="SaveCircularDetails" CssClass="text-danger"
                                    ClientValidationFunction="validToBePlacedBefore" Display="Dynamic"
                                    ErrorMessage="Please select To be placed before" runat="server"></asp:CustomValidator>
                            </div>
                            <div class="col-md-12 mb-3" id="trDetails" style="visibility: hidden; display: none;">
                                <label class="form-label">Details: <%--<span class="text-danger">*</span>--%></label>
                                <F2FControls:F2FTextBox ID="txtDetails" ToolTip="Details" CssClass="form-control"
                                    runat="server" Columns="64" Rows="2" TextMode="MultiLine">
                    </F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtDetails" />
                                <asp:RequiredFieldValidator ID="rfvDetails" runat="server" ControlToValidate="txtDetails" Enabled="false"
                                    ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                    ErrorMessage="Please enter Details."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6 mb-3" id="trNameOfThePolicy" style="visibility: hidden; display: none;">
                                <label class="form-label">Name of the Policy/Guidelines: <%--<span class="text-danger">*</span>--%></label>
                                <F2FControls:F2FTextBox ID="txtNameOfThePolicy" ToolTip="Details" CssClass="form-control"
                                    runat="server" Columns="64">
                                      </F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtNameOfThePolicy" />
                                <asp:RequiredFieldValidator ID="rfvNameOfThePolicy" runat="server" ControlToValidate="txtNameOfThePolicy" Enabled="false"
                                    ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                    ErrorMessage="Please enter Name of the Policy/Guidelines."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6 mb-3" id="trFrequency" style="visibility: hidden; display: none;">
                                <label class="form-label">Frequency: <%--<span class="text-danger">*</span>--%></label>
                                <F2FControls:F2FTextBox ID="txtFrequency" ToolTip="Details" CssClass="form-control"
                                    runat="server" Columns="64"> </F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtFrequency" />
                                <asp:RequiredFieldValidator ID="rfvFrequency" runat="server" ControlToValidate="txtFrequency" Enabled="false"
                                    ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                    ErrorMessage="Please enter Frequency."></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="mt-1">
                            <div class="card mb-1 mt-1 border">
                                <div class="card-header py-0 custom-ch-bg-color">
                                    <h6 class="font-weight-bold text-white mtb-5">Attachment: </h6>
                                </div>
                                <div class="card-body mt-1">
                                    <div class="mb-3">
                                        <asp:LinkButton CssClass="btn btn-sm btn-soft-primary btn-circle" ID="ImageButton1" runat="server" Text="+" OnClientClick="return addAttachmentRow()">
                                            <i class="fa fa-plus"></i>	                            
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-sm btn-soft-danger btn-circle" ID="ImageButton3" runat="server" Text="-"
                                            OnClientClick="return deleteAttachmentRow()">
                                            <i class="fa fa-trash"></i> 
                                        </asp:LinkButton>
                                    </div>
                                    <div class="table-responsive">
                                        <asp:Literal ID="litAttachment" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-md-3 mb-3" runat="server" id="trSelectAllto" visible="false">
                                <label class="form-label">Select All to be Intimated to:</label>
                                <asp:CheckBox ID="cbSelectAll" runat="server" ToolTip="Select All to be Intimated to" onClick="checkUncheckAll(this)"></asp:CheckBox>
                            </div>
                            <div class="col-md-9 mb-3" runat="server" id="trTobeintimate" visible="false">
                                <label class="form-label">To be Intimated To:</label>
                                <asp:Panel ID="pnlSubmissions" runat="server">
                                    <div class="custom-checkbox-table">
                                        <asp:CheckBoxList ID="cbSubmissions" RepeatColumns="4" runat="server" DataTextField="CIM_TYPE" CssClass="form-control"
                                            DataValueField="CIM_ID" AppendDataBoundItems="True" ToolTip="To be Intimated To" RepeatDirection="Horizontal">
                                        </asp:CheckBoxList>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="col-md-12 mb-3" runat="server" id="trAdditionalMailTo" visible="false">
                                <label class="form-label">Additional E-Mail Ids (To Be Added In To):</label>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <asp:DataList ID="dlAdditionalEmailsTO" runat="server" CellPadding="2" CssClass="custom-datalist"
                                                Font-Underline="False" HorizontalAlign="Left" RepeatDirection="Horizontal" Width="100%">
                                                <FooterStyle Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle Font-Bold="True" ForeColor="White" />
                                                <ItemStyle Font-Size="Small" CssClass="items" />
                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" VerticalAlign="Top" Wrap="true" />
                                                <ItemTemplate>
                                                    <div class="input-group mb-2">
                                                        <span class="form-control custom-span-input">
                                                            <%# Eval("EmailId") %></span>

                                                        <asp:LinkButton ToolTip="Remove" ID="btnRemoveAdditionalEmailsTo" runat="server" CssClass="btn btn-sm btn-soft-danger btn-circle d-flex align-self-stretch align-items-center"
                                                            OnClick="btnRemoveAdditionalEmailsTo_Click">
                                                            <i class="fa fa-trash"></i> 
                                                        </asp:LinkButton>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                        <F2FControls:F2FTextBox ID="txtAdditionalEmailsTO" CssClass="form-control" runat="server"
                                            MaxLength="4000" ToolTip="Additional E-Mail Ids.">
                                        </F2FControls:F2FTextBox>
                                        <asp:RegularExpressionValidator ID="revMailTo" runat="server" ErrorMessage="Please enter correct mail id." Display="Dynamic" CssClass="text-danger"
                                            ControlToValidate="txtAdditionalEmailsTO" ValidationGroup="SaveCircularDetails" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                        <ajaxToolkit:AutoCompleteExtender ServiceMethod="getUserEmailIdbyPhoneBook" MinimumPrefixLength="2"
                                            ServicePath="~/Projects/AJAXDropdown.asmx" EnableCaching="false" runat="server"
                                            TargetControlID="txtAdditionalEmailsTO" ID="aceAdditionalEmailsTO" FirstRowSelected="true"
                                            CompletionListItemCssClass="cssListItem" CompletionListHighlightedItemCssClass="cssListItemHighlight"
                                            OnClientItemSelected="AdditionalEmailsTOClientItemSelected">
                                        </ajaxToolkit:AutoCompleteExtender>
                                        <asp:HiddenField ID="hfAdditionalEmailsTO" runat="server" OnValueChanged="hfAdditionalEmailsTO_ValueChanged" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="col-md-12 mb-3" runat="server" id="trAdditionalMail" visible="false">
                                <label class="form-label">Additional E-Mail Ids (To Be Added In CC):</label>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <asp:DataList ID="dlAdditionalEmails" runat="server" CellPadding="2" CssClass="custom-datalist"
                                                Font-Underline="False" HorizontalAlign="Left" RepeatDirection="Horizontal" Width="100%">
                                                <FooterStyle Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle Font-Bold="True" ForeColor="White" />
                                                <ItemStyle Font-Size="Small" CssClass="items" />
                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" VerticalAlign="Top" Wrap="true" />
                                                <ItemTemplate>
                                                    <div class="input-group mb-2">
                                                        <span class="form-control custom-span-input"><%# Eval("EmailId") %></span>
                                                        <asp:LinkButton ToolTip="Remove" ID="btnRemoveAdditionalEmailsCC" runat="server" CssClass="btn btn-sm btn-soft-danger btn-circle d-flex align-self-stretch align-items-center"
                                                            OnClick="btnRemoveAdditionalEmailsCC_Click">
                                                                 <i class="fa fa-trash"></i> 
                                                        </asp:LinkButton>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                        <F2FControls:F2FTextBox ID="txtAdditionalEmails" CssClass="form-control" runat="server"
                                            MaxLength="4000" ToolTip="Additional E-Mail Ids.">
                                        </F2FControls:F2FTextBox>
                                        <asp:RegularExpressionValidator ID="rev" runat="server" ErrorMessage="Please enter correct mail id." Display="Dynamic" CssClass="text-danger"
                                            ControlToValidate="txtAdditionalEmails" ValidationGroup="SaveCircularDetails" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                        <ajaxToolkit:AutoCompleteExtender ServiceMethod="getUserEmailIdbyPhoneBook" MinimumPrefixLength="2"
                                            ServicePath="~/Projects/AJAXDropdown.asmx" EnableCaching="false" runat="server"
                                            TargetControlID="txtAdditionalEmails" ID="AutoCompleteExtender1" FirstRowSelected="true"
                                            CompletionListItemCssClass="cssListItem" CompletionListHighlightedItemCssClass="cssListItemHighlight"
                                            OnClientItemSelected="AdditionalEmailsCCClientItemSelected">
                                        </ajaxToolkit:AutoCompleteExtender>
                                        <asp:HiddenField ID="hfAdditionalEmailsCC" runat="server" OnValueChanged="hfAdditionalEmailsCC_ValueChanged" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="text-center mt-3">
                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSave" runat="server" CausesValidation="TRUE" ValidationGroup="SaveCircularDetails"
                                Text="Save Draft" OnClick="btnSave_Click" OnClientClick="return onClientValidateSave()">
                                <i class="fa fa-save me-2"></i> Save Draft                   
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnRefresh" runat="server" Text="Refresh Details" OnClick="btnRefresh_Click">
                                <i data-feather="refresh-cw"></i> Refresh Details 
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnSendMailFor" runat="server" Text="Send Mail For" CssClass="btn btn-outline-primary" Visible="false">
                                <i class="fa fa-paper-plane me-2"></i> Send Mail For
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnCancel" runat="server" CausesValidation="false"
                                Text="Cancel" OnClick="btnCancel_Click">
                                <i class="fa fa-arrow-left me-2"></i> Cancel
                            </asp:LinkButton>
                        </div>
                        <div class="mt-3">
                            <div class="card mb-1 mt-1 border">
                                <div class="card-header py-0 custom-ch-bg-color">
                                    <h6 class="font-weight-bold text-white mtb-5">Add Actionables </h6>
                                </div>
                                <div class="card-body mt-1">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvActionables" runat="server" CssClass="table table-bordered footable" GridLines="Both" AllowPaging="false" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Actionable" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("CA_ACTIONABLE").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CFM_NAME" HeaderText="Responsible Function" />
                                                <asp:BoundField DataField="CA_PERSON_RESPONSIBLE" HeaderText="Responsible Person" />
                                                <%--<asp:BoundField DataField="CA_PERSON_RESPONSIBLE_NAME" HeaderText="Person Responsible Name" />
                                                <asp:BoundField DataField="CA_PERSON_RESPONSIBLE_EMAIL_ID" HeaderText="Person Responsible Email" />--%>
                                                <asp:BoundField DataField="CA_REPORTING_MANAGER" HeaderText="Reporting Manager" />
                                                <%--<asp:BoundField DataField="CA_Reporting_Mgr_Name" HeaderText="Reporting Manager Name" />
                                                <asp:BoundField DataField="CA_Reporting_Mgr_EMAIL_ID" HeaderText="Reporting Manager Email" />--%>
                                                <asp:BoundField DataField="RC_NAME" HeaderText="Status" />
                                                <asp:BoundField DataField="CA_TARGET_DATE" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Target Date" />
                                                <asp:BoundField DataField="CA_REGULATORY_DUE_DATE" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Regulatory Due Date" />
                                                <asp:BoundField DataField="CA_COMPLETION_DATE" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Completion Date" />
                                                <asp:TemplateField HeaderText="Remarks" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("CA_REMARKS").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CA_CREATE_DATE" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}" HeaderText="Added On" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <asp:LinkButton CssClass="btn btn-outline-primary" ID="BtnSaveAddActionables" runat="server"
                                        OnClientClick="return onOthersClick('CAP')"> 
                                        <i class="fa fa-pen"></i> Add/Edit Actionables
                                    </asp:LinkButton>
                                    <div style="display: none;">
                                        <div>
                                            <asp:LinkButton CssClass="btn btn-sm btn-soft-primary btn-circle" ID="imgAdd" runat="server" Text="+" OnClientClick="return addActionablesRow()">
                                                <i class="fa fa-plus"></i> 
                                            </asp:LinkButton>
                                            <asp:LinkButton CssClass="btn btn-sm btn-soft-danger btn-circle" ID="Button1" runat="server" Text="-" OnClientClick="return deleteActionablesRow()">
                                                <i class="fa fa-trash"></i> 
                                            </asp:LinkButton>
                                        </div>
                                        <asp:Literal ID="litActionables" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="mt-3" id="divCertificationChecklist" runat="server">
                            <div class="card mb-1 mt-1 border">
                                <div class="card-header py-0 custom-ch-bg-color">
                                    <h6 class="font-weight-bold text-white mtb-5">Add Certification Checklists: </h6>
                                </div>
                                <div class="card-body mt-1">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvCertChecklists" runat="server" CssClass="table table-bordered footable" GridLines="Both" AllowPaging="false" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DeptName" HeaderText="Department Name" />
                                                <asp:TemplateField HeaderText="Act/Regulation/Circular" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("CDTM_TYPE_OF_DOC").ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reference Circular / Notification / Act" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("CCC_REFERENCE").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Section/Clause" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("CCC_CLAUSE").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="Compliance of/Heading of Compliance checklist" ShowHeader="true">--%>
                                                <asp:TemplateField HeaderText="Particulars" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("CCC_CHECK_POINTS").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("CCC_PARTICULARS").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Consequences of non Compliance" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("CCC_PENALTY").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CCC_FREQUENCY" HeaderText="Frequency" />
                                                <asp:BoundField DataField="CCC_FORMS" HeaderText="Forms" />
                                                <asp:BoundField DataField="CCC_EFFECTIVE_FROM" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Effective From" />
                                                <asp:BoundField DataField="CCC_CREATE_DT" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}" HeaderText="Added On" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <asp:LinkButton CssClass="btn btn-outline-primary" ID="BtnSaveAddCertChecklist" runat="server"
                                        Text="Add/Edit Certification Checklist" OnClientClick="return onOthersClick('CCC')">
                                    <i class="fa fa-plus"></i> Add/Edit Certification Checklist
                                    </asp:LinkButton>
                                    <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnUploadChecklist" runat="server"
                                        Text="Upload Certification Checklist" OnClientClick="return onOthersClick('UCCC')">
                                    <i class="fa fa-upload"></i> Upload Certification Checklist
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <div class="mt-1">
                            <div class="card mb-1 mt-1 border">
                                <div class="card-header py-0 custom-ch-bg-color">
                                    <h6 class="font-weight-bold text-white mtb-5">Add Regulatory Reporting: </h6>
                                </div>
                                <div class="card-body mt-1">
                                    <div>
                                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnAddRegReporting" runat="server"
                                            Text="Add Regulatory Reporting" OnClientClick="return onOthersClick('CRR')">
                                            Add Regulatory Reporting
                                        </asp:LinkButton>

                                        <div class="input-group mt-3">
                                            <asp:LinkButton ID="lnkListOfReports" runat="server" CssClass="btn btn-sm btn-info" Text="click here" OnClientClick="return onOthersClick('VCRR')">
                                            click here
                                            </asp:LinkButton>
                                            <div>
                                                &nbsp; to view the regulatory reporting added.
                                           
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </asp:Panel>
            </div>
        </div>
    </div>
    <!-- end row -->

    <ajaxToolkit:CalendarExtender ID="ceDate" runat="server" PopupButtonID="imgCircularDate"
        TargetControlID="txtCircularDate" Format="dd-MMM-yyyy" PopupPosition="TopRight"></ajaxToolkit:CalendarExtender>
    <ajaxToolkit:CalendarExtender ID="ceEffDate" runat="server" PopupButtonID="imgCircEffDate"
        TargetControlID="txtCircEffDate" Format="dd-MMM-yyyy" PopupPosition="TopRight"></ajaxToolkit:CalendarExtender>
    <ajaxToolkit:CascadingDropDown ID="cddIssuingAuthority" runat="server" TargetControlID="ddlCircularAuthority"
        Category="IssuingAuthority" PromptText="(Select an Issuing Authority)" ServicePath="AJAXDropdownCirculars.asmx"
        ServiceMethod="GetIssuingAuthority" />
    <ajaxToolkit:CascadingDropDown ID="cddTopic" runat="server" TargetControlID="ddlArea"
        ParentControlID="ddlCircularAuthority" Category="Topic" PromptText="(Select a Topic)"
        ServicePath="AJAXDropdownCirculars.asmx" ServiceMethod="GetTopicByIssuingAuthority" />


    <div class="modal fade bd-example-modal-xl" id="divSendMailForModal" tabindex="-1" aria-labelledby="myExtraLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title">Send mail</h6>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12 mb-3">
                            <label class="form-label">Audit Trail:  </label>
                            <asp:Label ID="lblSendMailAuditTrail" runat="server" Text='<%# Convert.ToBase64String(Encoding.UTF8.GetBytes(Eval("CM_AUDIT_TRAIL").ToString().Replace("\r\n", "<br>")))%>'></asp:Label>
                        </div>
                        <div class="col-md-12 mb-3">
                            <label class="form-label">Send Mail For: <span class="text-danger">*</span></label>
                            <div class="custom-checkbox-table">
                                <asp:CheckBoxList ID="cbSendMailFor" RepeatColumns="3" runat="server" DataTextField="RC_NAME" CssClass="form-control"
                                    DataValueField="RC_CODE" AppendDataBoundItems="True" RepeatDirection="Horizontal">
                                </asp:CheckBoxList>
                            </div>
                            <asp:CustomValidator ID="cvSendMailFor" ValidationGroup="SendMailFor" CssClass="text-danger"
                                ClientValidationFunction="validateSendMailFor" Display="Dynamic"
                                ErrorMessage="Please select atleast one record" runat="server"></asp:CustomValidator>
                        </div>
                        <div class="col-md-12 mb-3">
                            <label class="form-label">Reason for Broadcast:</label>
                            <F2FControls:F2FTextBox ID="txtReasonForBroadcast" ToolTip="Reason for Broadcast" CssClass="form-control"
                                runat="server" Columns="64" Rows="5" TextMode="MultiLine">
                                        </F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtReasonForBroadcast" />
                        </div>
                    </div>
                    <asp:Label ID="lblNote" runat="server" CssClass="custom-info-alert" Text="Reason for Broadcast shall be saved only when Circular is broadcasted."></asp:Label>
                </div>
                <div class="modal-footer" style="flex-direction: column;">
                    <div>
                        <asp:Button ID="btnSubmit" runat="server" Text="Send" CssClass="btn btn-primary"
                            OnClick="btnSubmit_ServerClick" ValidationGroup="SendMailFor"
                            OnClientClick="return onClientValidateSendMail()" />
                        <%--<input type="button" id="btnSubmit" runat="server" value="Send" class="btn btn-outline-success"
                            onserverclick="btnSubmit_ServerClick" validationgroup="SendMailFor" />--%>
                        <input id="btncCancel" runat="server" type="button" value="Cancel" data-bs-dismiss="modal"
                            class="modalCloseImg simplemodal-close btn btn-outline-danger" />
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
