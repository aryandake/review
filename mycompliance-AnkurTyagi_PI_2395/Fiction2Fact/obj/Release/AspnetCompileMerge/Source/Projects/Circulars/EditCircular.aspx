<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Title="Edit Circulars" Inherits="Fiction2Fact.Projects.Circulars.Circulars_EditCircular"
    EnableEventValidation="false" ValidateRequest="false" Async="true" CodeBehind="EditCircular.aspx.cs" %>

<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="custom" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .ck-editor__editable {
            min-height: 250px;
        }
    </style>
    <script src="<%=Fiction2Fact.Global.site_url("Scripts/ckeditor/ckeditor.js")%>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            if (document.getElementById('ctl00_ContentPlaceHolder1_fvCircularMaster_FCKE_EditCircularDetails') != null) {
                ClassicEditor
                    .create(document.querySelector('#ctl00_ContentPlaceHolder1_fvCircularMaster_FCKE_EditCircularDetails'), {
                        // toolbar: [ 'heading', '|', 'bold', 'italic', 'link' ]
                    })
                    .then(editor => {
                        window.editor = editor;
                    })
                    .catch(err => {
                        console.error(err.stack);
                    });
            }
            if (document.getElementById('ctl00_ContentPlaceHolder1_fvCircularMaster_FCKE_EditorImplications') != null) {
                ClassicEditor
                    .create(document.querySelector('#ctl00_ContentPlaceHolder1_fvCircularMaster_FCKE_EditorImplications'), {
                        // toolbar: [ 'heading', '|', 'bold', 'italic', 'link' ]
                    })
                    .then(editor => {
                        window.editor = editor;
                    })
                    .catch(err => {
                        console.error(err.stack);
                    });
            }

            //<< onDeactivationClick
            $("#<%= btnDeactivate.ClientID %>").click(() => {
                if (Page_ClientValidate("DeactivateCircular")) {
                    $("#<%= btnDeactivate.ClientID %>").attr('disabled', 'disabled');
                    $("#<%= btnDeactivate.ClientID %>").css({ "background-color": "#d2d2d2" });
                    $("#<%= btnDeactivate.ClientID %>").val("Please wait...");
                }
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
            $('[id*="btnSubmit"]').click(() => {
                if (Page_ClientValidate('SendMailFor')) {
                    $('[id*="btnSubmit"]').attr('disabled', 'disabled');
                    $('[id*="btnSubmit"]').css({ "background-color": "#d2d2d2" });
                    $('[id*="btnSubmit"]').val("Please wait...");
                }
            });
            //>>
        });

        function onViewDetailClick(Id) {
            window.open('ViewCircularDetails.aspx?CircularId=' + Id);
        }

        function onViewClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
        }

        function onClientDeleteClick() {
            if (!confirm('Are you sure that you want to delete this record?')) return false;
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Delete";
        }

        function onClientDeactivateClick(CircId) {
            document.getElementById('<%=hfSelectedRecord.ClientID%>').value = CircId;
            $("#divModal").modal('show');
            return false;
        }
    </script>

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/Actionables.js")%>"></script>

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js")%>"></script>

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/populateUserDetsAJAX.js")%>"></script>

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/dateformatvalidation.js")%>"></script>

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/commonvalidations.js")%>"></script>

    <%--<script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/base64.js") %>"></script>--%>

    <link type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/css/legacy/ui-lightness/jquery-ui-1.8.19.custom.css")%>" rel="stylesheet" />
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/jquery-ui-1.8.19.custom.min.js")%>"></script>

    <script type="text/javascript" charset="utf-8">
        var asInitVals = new Array();
        $(document).ready(function () {
            try {
                if ($("#<%= hfSelectedOperation.ClientID %>").val() == "Edit") {
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
                }
            } catch (e) {
                alert(e);
            }
        });

        const onRequirementForTheBoardChange = () => {
            if ($("[id*='ddlRequirementForTheBoard']").val() == "Y") {
                $("#trToBePlacedBefore").css({ "visibility": "visible", "display": "table-row" });
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_fvCircularMaster_cvToBePlacedBefore'), true);

                $("#trDetails").css({ "visibility": "visible", "display": "table-row" });
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_fvCircularMaster_rfvDetails'), true);

                $("#trNameOfThePolicy").css({ "visibility": "visible", "display": "table-row" });
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_fvCircularMaster_rfvNameOfThePolicy'), true);

                $("#trFrequency").css({ "visibility": "visible", "display": "table-row" });
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_fvCircularMaster_rfvFrequency'), true);
            }
            else {
                $("#trToBePlacedBefore").css({ "visibility": "hidden", "display": "none" });
                clearCheckboxList('ctl00_ContentPlaceHolder1_fvCircularMaster_cbToBePlacedBefore');
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_fvCircularMaster_cvToBePlacedBefore'), false);

                $("#trDetails").css({ "visibility": "hidden", "display": "none" });
                $("[id*='txtDetails']").val("");
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_fvCircularMaster_rfvDetails'), false);

                $("#trNameOfThePolicy").css({ "visibility": "hidden", "display": "none" });
                $("[id*='txtNameOfThePolicy']").val("");
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_fvCircularMaster_rfvNameOfThePolicy'), false);

                $("#trFrequency").css({ "visibility": "hidden", "display": "none" });
                $("[id*='txtFrequency']").val("");
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_fvCircularMaster_rfvFrequency'), false);
            }
        }

        const onLinkageWithEarlierCircularChange = () => {
            if ($("[id*='ddlLinkageWithEarlierCircular']").val() == "Y") {
                $("#trSOCEOC").css({ "visibility": "visible", "display": "table-row" });
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_fvCircularMaster_rfvSOCEOC'), true);

                $("#trOldCircSubjectNo").css({ "visibility": "visible", "display": "table-row" });
            }
            else {
                $("#trSOCEOC").css({ "visibility": "hidden", "display": "none" });
                $("[id*='ddlSOCEOC']").val("");
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_fvCircularMaster_rfvSOCEOC'), false);

                $("#trOldCircSubjectNo").css({ "visibility": "hidden", "display": "none" });
                $("[id*='dlUserList'] tr:not(:first-child)").remove();
                $("[id*='hfOldCircularId']").val("");
            }
        }

        function compareEndSystemDates(source, arguments) {
            try {
                var Fromdate = document.getElementById('ctl00_ContentPlaceHolder1_fvCircularMaster_txtCircularDate');
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

        function onClientValidateEdit() {
            var validated = Page_ClientValidate('SaveCircularDetails');
            if (validated) {
                var ddlLinkageWithEarlierCircular = document.getElementById('ctl00_ContentPlaceHolder1_fvCircularMaster_ddlLinkageWithEarlierCircular').value;
                var table = document.getElementById("ctl00_ContentPlaceHolder1_fvCircularMaster_dlUserList");
                var flag = true;

                if (ddlLinkageWithEarlierCircular == 'Y') {
                    if (table != null) {
                        var rowCount = table.rows.length;

                        if (rowCount <= 0) {
                            flag = false;
                        }
                    }
                    else {
                        flag = false;
                    }
                }

                if (!flag) {
                    alert('Please enter Old Circular Subject/No.');
                    return flag;
                }

                return getAttachmentData();
            }
            else {
                return false;
            }
        }

        function compareEndDates(source, arguments) {
            try {
                var Fromdate = document.getElementById('ctl00_ContentPlaceHolder1_txtFromDate');
                var ToDate = document.getElementById('ctl00_ContentPlaceHolder1_txtToDate');

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

    <script type="text/javascript">
        function OldCircSubjectNoClientItemSelected(source, eventArgs) {
            var hdnValueID = "ctl00_ContentPlaceHolder1_fvCircularMaster_hfOldCircularId";
            document.getElementById(hdnValueID).value = eventArgs.get_value();
            __doPostBack(hdnValueID, "");
        }

        function AdditionalEmailsTOClientItemSelected(source, eventArgs) {
            var hdnAddValueID = "ctl00_ContentPlaceHolder1_fvCircularMaster_hfAdditionalEmailsTO";
            document.getElementById(hdnAddValueID).value = eventArgs.get_value();
            __doPostBack(hdnAddValueID, "");
        }

        function AdditionalEmailsCCClientItemSelected(source, eventArgs) {
            var hdnAddValueID = "ctl00_ContentPlaceHolder1_fvCircularMaster_hfAdditionalEmailsCC";
            document.getElementById(hdnAddValueID).value = eventArgs.get_value();
            __doPostBack(hdnAddValueID, "");
        }

        function validToBePlacedBefore(source, arg) {
            var atLeast = 1;
            var valid = false;
            var CHK = document.getElementById("ctl00_ContentPlaceHolder1_fvCircularMaster_cbToBePlacedBefore");
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

        function validateSendMailFor(source, arg) {
            var atLeast = 1;
            var valid = false;
            var CHK = document.getElementById("ctl00_ContentPlaceHolder1_fvCircularMaster_cbSendMailFor");
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

            return false;
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

    <asp:HiddenField ID="hfActionables" runat="server" />
    <asp:HiddenField ID="hfActionablesData" runat="server" />
    <asp:HiddenField ID="hfAttachment" runat="server" />
    <asp:HiddenField ID="hfSelectedRecord" runat="server" />
    <asp:HiddenField ID="hfSelectedOperation" runat="server" />
    <asp:HiddenField ID="hfCircularAuthority" runat="server" />
    <asp:HiddenField ID="hfUserType" runat="server" />
    <asp:HiddenField ID="hfCurDate" runat="server" />
    <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />

    <asp:HiddenField ID="hfCircularDate" runat="server" />
    <asp:HiddenField ID="hfTopic" runat="server" />
    <asp:HiddenField ID="hfIsRegulatoryReportingAdded" runat="server" />
    <%--<asp:HiddenField ID="hfSendMailFor" runat="server" />
    <asp:HiddenField ID="hfReasonForBroadcast" runat="server" />--%>
    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Edit Circular Master</h4>
                        <asp:Label ID="lblInfo" runat="server" CssClass="custom-alert-box"></asp:Label>
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
                <asp:MultiView ID="mvMultiView" runat="server">
                    <asp:View ID="vwGrid" runat="server">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-3 mb-3">
                                    <label class="form-label">Global Search:</label>
                                    <F2FControls:F2FTextBox ID="txtGlobalSearch" ToolTip="Implications" CssClass="form-control" runat="server"
                                        Columns="30"></F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtGlobalSearch" />
                                </div>
                                <div class="col-md-3 mb-3" style="display: none">
                                    <label class="form-label">Type of Document</label>
                                    <asp:DropDownList CssClass="form-select" ID="ddlSearchTypeofCircular" AppendDataBoundItems="true"
                                        runat="server" ToolTip="Type of Circular" DataTextField="CDTM_TYPE_OF_DOC" DataValueField="CDTM_ID">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3 mb-3" style="display: none">
                                    <label class="form-label">Topic</label>
                                    <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlSArea" runat="server">
                                    </f2f:DropdownListNoValidation>
                                    <ajaxToolkit:CascadingDropDown ID="cddTopic" runat="server" TargetControlID="ddlSArea"
                                        ParentControlID="ddlSIssuingauthority" Category="Topic" PromptText="Select Topic"
                                        ServicePath="AJAXDropdownCirculars.asmx" ServiceMethod="GetTopicByIssuingAuthority" />
                                </div>
                                <div class="col-md-3 mb-3">
                                    <label class="form-label">From Date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtFromDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="ibFrmDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <asp:RegularExpressionValidator ID="revFromDate" runat="server" ControlToValidate="txtFromDate" CssClass="text-danger"
                                        Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                        ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-3 mb-3">
                                    <label class="form-label">To Date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtToDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="ibToDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <asp:RegularExpressionValidator ID="revToDate" runat="server" ControlToValidate="txtToDate" CssClass="text-danger"
                                        Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                        ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                                    <asp:CustomValidator ID="cvCompletionDate" runat="server" ClientValidationFunction="compareEndDates"
                                        ControlToValidate="txtToDate" ErrorMessage="From Date should be less than or Equal to To Date."
                                        Display="Dynamic" OnServerValidate="cvTodate_ServerValidate" ValidationGroup="SEARCH"></asp:CustomValidator>
                                </div>
                                <div class="col-md-3 mb-3" style="display: none">
                                    <label class="form-label">Issuing Authority</label>
                                    <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlSIssuingauthority"
                                        runat="server">
                                    </f2f:DropdownListNoValidation>
                                    <ajaxToolkit:CascadingDropDown ID="cddIssuingAuthority" runat="server" TargetControlID="ddlSIssuingauthority"
                                        Category="IssuingAuthority" PromptText="Select a Issuing Authority" ServicePath="AJAXDropdownCirculars.asmx"
                                        ServiceMethod="GetIssuingAuthority" />
                                </div>
                                <div class="col-md-3 mb-3" style="display: none">
                                    <label class="form-label">SPOC From Compliance Function:</label>
                                    <asp:DropDownList ID="ddlSpocFromCompFnSearch" CssClass="form-select" runat="server"
                                        DataTextField="CCS_NAME" DataValueField="CCS_ID" AppendDataBoundItems="True"
                                        ToolTip="SPOC From Compliance Function">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3 mb-3" style="display: none">
                                    <label class="form-label">Have Actionables been logged:</label>
                                    <asp:DropDownList ID="ddlActionableHaveBeenLogged" CssClass="form-select" runat="server"
                                        ToolTip="Have Actionable has been logged">
                                        <asp:ListItem Selected="True" Value="">(Select)</asp:ListItem>
                                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                                        <asp:ListItem Value="N">No</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-12 mb-3" style="display: none">
                                    <label class="form-label">Associated Keywords:</label>
                                    <asp:Panel ID="pnlAssociatedKeywordsSearch" runat="server">
                                        <div class="custom-checkbox-table">
                                            <asp:CheckBoxList ID="cbAssociatedKeywordsSearch" RepeatColumns="6" runat="server" DataTextField="CKM_NAME" CssClass="form-control"
                                                DataValueField="CKM_ID" AppendDataBoundItems="True" ToolTip="Associated Keywords" RepeatDirection="Horizontal">
                                            </asp:CheckBoxList>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div class="col-md-12 mb-3" style="display: none">
                                    <label class="form-label">To be placed before:</label>
                                    <div class="custom-checkbox-table">
                                        <asp:Panel ID="pnlToBePlacedBeforeSearch" runat="server">
                                            <asp:CheckBoxList ID="cbToBePlacedBeforeSearch" RepeatColumns="4" runat="server" DataTextField="RC_NAME" CssClass="form-control"
                                                DataValueField="RC_CODE" AppendDataBoundItems="True" ToolTip="To be placed before" RepeatDirection="Horizontal">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div class="col-md-3 mb-3">
                                    <label class="form-label">Status:</label>
                                    <asp:DropDownList ID="ddlStatus" CssClass="form-select" runat="server"
                                        ToolTip="Select status">
                                        <asp:ListItem Value="">(Select)</asp:ListItem>
                                        <asp:ListItem Value="A">Active</asp:ListItem>
                                        <asp:ListItem Value="I">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="text-center mt-3">
                            <asp:LinkButton CssClass="btn btn-outline-primary" ID="Button1" Text="Search" runat="server" ValidationGroup="SEARCH"
                                AccessKey="s" OnClick="btnSearch_Click">
                                <i class="fa fa-search"></i> Search                     
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="lnkReset" Text="Reset" runat="server"
                            OnClick="lnkReset_Click">Reset
                        </asp:LinkButton>
                            <asp:LinkButton ID="btnExportToExcel" Visible="false" runat="server" CssClass="btn btn-outline-secondary"
                                Text="Export to Excel" OnClick="btnExportToExcel_Click">
                                <i class="fa fa-download"></i> Export to Excel               
                            </asp:LinkButton>
                            <cc1:CalendarExtender ID="ceFrmDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibFrmDate"
                                TargetControlID="txtFromDate"></cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="ceTodate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibToDate"
                                TargetControlID="txtToDate"></cc1:CalendarExtender>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gvCircularMaster" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                    GridLines="Both" DataKeyNames="CM_ID" OnSelectedIndexChanged="gvCircularMaster_SelectedIndexChanged"
                                    AllowPaging="true" AllowSorting="true" OnPageIndexChanging="gvCircularMaster_PageIndexChanging"
                                    CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" OnSorting="gvCircularMaster_Sorting"
                                    OnRowCreated="OnRowCreated" OnRowDataBound="gvCircularMaster_RowDataBound" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCircularId" runat="server" Text='<%# Bind("CM_ID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                    <asp:HiddenField ID="hfStatus" runat="server" Value='<%# Eval("CM_STATUS") %>' />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="View Details">
                                            <ItemTemplate>
                                                <center>
                                                   <%-- <a class="btn btn-sm btn-soft-info btn-circle" onclick='<%# "onViewDetailClick("+ Eval("CM_ID")+")" %>'>
                                                        <i class="fa fa-eye"></i>
                                                    </a>--%>
                                                    <asp:LinkButton ID="lnkView" runat="server" CausesValidation="False" CssClass="btn btn-sm btn-soft-success btn-circle" 
                                                        CommandName="Select">
                                                        <i class="fa fa-eye"></i>
                                                    </asp:LinkButton>
                                                </center>
                                                <asp:Label ID="lbId" Text='<% # Eval("CM_ID") %>' Visible="false" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="true" HeaderText="Edit">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:LinkButton ID="lbEdit" runat="server" CausesValidation="False" CssClass="btn btn-sm btn-soft-success btn-circle" OnClientClick="onViewClick()"
                                                        CommandName="Select">
                                                        <i class="fa fa-pen"></i>
                                                    </asp:LinkButton>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="true" HeaderText="Delete" Visible="false">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:LinkButton ID="lbDelete" runat="server" CssClass="btn btn-sm btn-soft-danger btn-circle " CausesValidation="False" OnClientClick="return onClientDeleteClick()"
                                                        CommandName="Select">
                                                        <i class="fa fa-trash"></i>
                                                    </asp:LinkButton>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="true" HeaderText="Deactivate">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:LinkButton ID="lbDeactivate" runat="server" CausesValidation="False" CssClass="btn btn-sm btn-soft-danger btn-circle" OnClientClick='<%# string.Format("return onClientDeactivateClick(\"{0}\");", Eval("CM_ID")) %>'>
                                                        <i class="fa fa-ban"></i>
                                                    </asp:LinkButton>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Line of Business" SortExpression="LEM_NAME" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLOB" runat="server" Text='<%# Eval("LEM_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Subject" SortExpression="CM_TOPIC">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTopic" runat="server" Text='<%# Eval("CM_TOPIC") %>' Width="400px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Topic" SortExpression="CAM_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblArea" runat="server" Text='<%# Eval("CAM_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Issuing Authority" SortExpression="CIA_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssAuth" runat="server" Text='<%# Eval("CIA_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Circular No" SortExpression="CM_CIRCULAR_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCircNo" runat="server" Text='<%# Eval("CM_CIRCULAR_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Circular Date" SortExpression="CM_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCircularDate" runat="server" Text='<%# Eval("CM_DATE", "{0:dd-MMM-yyyy }") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SPOC From Compliance Department">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSpocFromcompliance" runat="server" Text='<%# Eval("SPOCName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Circular Effective Date" SortExpression="CM_CIRC_EFF_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCircularEffDate" runat="server" Text='<%# Eval("CM_CIRC_EFF_DATE", "{0:dd-MMM-yyyy }") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Old Circular Subject/No." SortExpression="CM_OLD_CIRC_SUB_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOldCircSubNo" runat="server" Text='<%# Eval("CM_OLD_CIRC_SUB_NO") %>' Width="400px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Files">
                                            <ItemTemplate>
                                                <%--<asp:DataList ID="dlCircularFiles" runat="server" DataKeyField="CF_ID" RepeatColumns="1"
                                                    RepeatDirection="Horizontal" DataSource='<%#LoadCircularFileList(Eval("CM_ID"))%>'>--%>
                                                <asp:DataList ID="dlCircularFiles" runat="server" DataKeyField="CF_ID" RepeatColumns="1" CssClass="custom-datalist-border"
                                                    RepeatDirection="Vertical" DataSource='<%#LoadCircularFileList(Eval("CM_ID"))%>'>
                                                    <ItemTemplate>
                                                        <%--<a href="#" onclick="javascript:window.open('../CommonDownload.aspx?type=Circular&downloadFileName=<%#getFileName(Eval("CF_SERVERFILENAME"))%>','','location=1,status=0,scrollbars=2,width=650,height=500');">
                                                            <asp:Image ID="lblIssuerLink" runat="server" ImageUrl="../../Content/images/legacy/viewfulldetails.png" />
                                                        </a>--%>
                                                        <a href="javascript:window.open('../CommonDownload.aspx?type=Circular&downloadFileName=<%#getFileName(Eval("CF_SERVERFILENAME"))%>','','location=1,status=0,scrollbars=2,width=650,height=500');">
                                                            <%#getFileName(Eval("CF_FILENAME"))%>
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Issuer Link">
                                            <ItemTemplate>
                                                <a href='<%# IsValidUrl(Eval("CM_ISSUING_LINK").ToString()) %>' target="_blank" onclick='<%# IsValidUrl(Eval("CM_ISSUING_LINK").ToString())=="#"?"return false;":"return true;" %>'>
                                                    <%--<i class="fa fa-eye"></i>--%>
                                                    <%# Eval("CM_ISSUING_LINK") %>
                                                </a>
                                                <asp:Label ID="lblIssuerLink" runat="server" Text='<%# Eval("CM_ISSUING_LINK") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="CircStatus" SortExpression="CircStatus" />

                                        <asp:TemplateField HeaderText="Created By" SortExpression="CDM_NAME" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CDM_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<< Added by Amarjeet on 04-Aug-2021--%>
                                        <asp:TemplateField HeaderText="Linkage With Earlier Circular" SortExpression="LinkageWithEarlierCircular" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLinkageWithEarlierCircular" runat="server" Text='<%# Eval("LinkageWithEarlierCircular") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Supersedes or Extends/Amends Old Circular(s)" SortExpression="SOCEOC" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSOCEOC" runat="server" Text='<%# Eval("SOCEOC") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-->>--%>
                                        <asp:TemplateField HeaderText="Type of Document" SortExpression="CDTM_TYPE_OF_DOC" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTypeofDocument" runat="server" Text='<%# Eval("CDTM_TYPE_OF_DOC") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gist of the Circular" SortExpression="CM_DETAILS" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGist" runat="server" ToolTip='<%# replaceHTMLTagsFromLableTooltip(Eval("CM_DETAILS").ToString()) %>'
                                                    Text='<%#Eval("CM_DETAILS").ToString().Length > 200 ? (Eval("CM_DETAILS") as string).Substring(0, 200).Replace("\n", "<br />") + " ..." : Eval("CM_DETAILS").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                                <asp:Label ID="lblGist1" Visible="false" runat="server" Text='<%#Eval("CM_DETAILS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Implications" SortExpression="CM_IMPLICATIONS" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblImplications" runat="server" ToolTip='<%# replaceHTMLTagsFromLableTooltip(Eval("CM_IMPLICATIONS").ToString()) %>'
                                                    Text='<%#Eval("CM_IMPLICATIONS").ToString().Length > 200 ? (Eval("CM_IMPLICATIONS") as string).Substring(0, 200).Replace("\n", "<br />") + " ..." : Eval("CM_IMPLICATIONS").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                                <asp:Label ID="lblImplications1" Visible="false" runat="server" Text='<%#Eval("CM_IMPLICATIONS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Requirement for the Board/Audit committee to approve" DataField="AuditCommitteeToApprove" SortExpression="AuditCommitteeToApprove" Visible="false" />
                                        <asp:BoundField HeaderText="To be placed before" DataField="ToBePlacedBefore" SortExpression="ToBePlacedBefore" Visible="false" />
                                        <asp:TemplateField HeaderText="Details" SortExpression="CM_IMPLICATIONS" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDetails" runat="server" ToolTip='<%# replaceHTMLTagsFromLableTooltip(Eval("CM_REMARKS").ToString()) %>'
                                                    Text='<%#Eval("CM_REMARKS").ToString().Length > 200 ? (Eval("CM_REMARKS") as string).Substring(0, 200).Replace("\n", "<br />") + " ..." : Eval("CM_REMARKS").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                                <asp:Label ID="lblDetails1" Visible="false" runat="server" Text='<%#Eval("CM_REMARKS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Name of the Policy/Guidelines" DataField="CM_NAME_OF_THE_POLICY" SortExpression="CM_NAME_OF_THE_POLICY" Visible="false" />
                                        <asp:BoundField HeaderText="Frequency" DataField="CM_FREQUENCY" SortExpression="CM_FREQUENCY" Visible="false" />
                                        <asp:TemplateField HeaderText="Certification Checklist Added" SortExpression="IsCertChecklistAdded" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCertChklistAdded" runat="server" Text='<%# Eval("IsCertChecklistAdded") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reporting Actionable Added" SortExpression="IsRegulatoryReportingAdded" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRegulatoryReportingAdded" runat="server" Text='<%# Eval("IsRegulatoryReportingAdded") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Deactivated By" DataField="CM_DEACTIVATED_BY" SortExpression="CM_DEACTIVATED_BY" Visible="false" />
                                        <asp:BoundField HeaderText="Deactivated On" DataField="CM_DEACTIVATED_ON" DataFormatString="{0: dd-MMM-yyyy HH:mm:ss}" SortExpression="CM_DEACTIVATED_ON" Visible="false" />
                                        <asp:TemplateField HeaderText="Deactivation Remarks" SortExpression="CM_DEACTIVATION_REMARKS" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeactivationRemarks" runat="server" ToolTip='<%# replaceHTMLTagsFromLableTooltip(Eval("CM_DEACTIVATION_REMARKS").ToString()) %>'
                                                    Text='<%#Eval("CM_DEACTIVATION_REMARKS").ToString().Length > 200 ? (Eval("CM_DEACTIVATION_REMARKS") as string).Substring(0, 200).Replace("\n", "<br />") + " ..." : Eval("CM_DEACTIVATION_REMARKS").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                                <asp:Label ID="lblDeactivationRemarks1" Visible="false" runat="server" Text='<%#Eval("CM_DEACTIVATION_REMARKS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Circular Creation Date" DataField="CM_CREAT_DT" DataFormatString="{0: dd-MMM-yyyy HH:mm:ss}" SortExpression="CM_CREAT_DT" Visible="false" />
                                        <asp:BoundField HeaderText="Circular Broadcast Date" DataField="CM_BROADCAST_DATE" DataFormatString="{0: dd-MMM-yyyy HH:mm:ss}" SortExpression="CM_BROADCAST_DATE" Visible="false" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                        <div class="modal fade" id="divModal" tabindex="-1" aria-labelledby="exampleModalFullscreenLgLabel" aria-hidden="true">
                            <div class="modal-dialog modal-fullscreen-lg-down">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h6 class="modal-title">Deactivate Circular</h6>
                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-md-12 mb-3">
                                                <label class="form-label">Deactivation Remarks: <span class="text-danger">*</span></label>
                                                <F2FControls:F2FTextBox ID="txtDeactivationRemarks" ToolTip="Deactivation Remarks"
                                                    CssClass="form-control" runat="server" Columns="64" Rows="5"
                                                    TextMode="MultiLine">
                                                </F2FControls:F2FTextBox>
                                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtDeactivationRemarks" />
                                                <asp:RequiredFieldValidator ID="rfvDeactivationRemarks" runat="server" ControlToValidate="txtDeactivationRemarks"
                                                    CssClass="text-danger" ValidationGroup="DeactivateCircular" Display="Dynamic" SetFocusOnError="True"
                                                    ErrorMessage="Please enter Deactivation Remarks."></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <input type="button" id="btnDeactivate" runat="server" value="Deactivate" class="btn btn-outline-success"
                                            validationgroup="DeactivateCircular" onserverclick="btnDeactivate_ServerClick" />
                                        <input id="btncCancel" runat="server" type="button" value="Cancel" data-bs-dismiss="modal"
                                            class="modalCloseImg simplemodal-close btn btn-outline-danger" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwForm" runat="server">
                        <asp:FormView ID="fvCircularMaster" runat="server" DefaultMode="Edit" AllowPaging="True"
                            Width="100%" DataKeyNames="CM_ID">
                            <EditItemTemplate>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Created By:</label>
                                            <asp:DropDownList ID="ddlDepartment" CssClass="form-select" runat="server" DataTextField="CDM_NAME"
                                                DataValueField="CDM_ID" AppendDataBoundItems="True" ToolTip="Created By">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Linkage with earlier circular: <span class="text-danger">*</span></label>
                                            <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlLinkageWithEarlierCircular" onchange="onLinkageWithEarlierCircularChange();"
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
                                            <label class="form-label">Old Circular Subject/No.: <span class="text-danger">*</span></label>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <div>
                                                        <asp:DataList ID="dlUserList" runat="server" CellPadding="2" Font-Size="Small" CssClass="custom-datalist"
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
                                                    <F2FControls:F2FTextBox ID="txtOldCircSubjectNo" CssClass="form-control" runat="server"
                                                        MaxLength="4000" ToolTip="Old Circular Subject/No.">
                                                    </F2FControls:F2FTextBox>
                                                    <ajaxToolkit:AutoCompleteExtender ServiceMethod="getCircularDetailsFromSubjectAndCircularNo" MinimumPrefixLength="2"
                                                        ServicePath="AJAXDropdownCirculars.asmx" EnableCaching="false" runat="server"
                                                        TargetControlID="txtOldCircSubjectNo" ID="aceOldCircSubjectNo" FirstRowSelected="true"
                                                        CompletionListItemCssClass="cssListItem" CompletionListHighlightedItemCssClass="cssListItemHighlight"
                                                        OnClientItemSelected="OldCircSubjectNoClientItemSelected">
                                                    </ajaxToolkit:AutoCompleteExtender>
                                                    <asp:HiddenField ID="hfOldCircularId" runat="server" OnValueChanged="hfOldCircularId_ValueChanged" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Issuing Authority:</label>
                                            <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlCircularAuthority"
                                                runat="server" ToolTip="Issuing Authority">
                                            </f2f:DropdownListNoValidation>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Topic:</label>
                                            <f2f:DropdownListNoValidation ID="ddlArea" CssClass="form-select" runat="server"
                                                ToolTip="Topic">
                                            </f2f:DropdownListNoValidation>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Type of Document: </label>
                                            <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlTypeofCircular" runat="server"
                                                ToolTip="Type of Circular">
                                            </f2f:DropdownListNoValidation>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Private / Public :<span class="text-danger">*</span></label>
                                            <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlSubTypeofCircular"
                                                AppendDataBoundItems="true" runat="server" ToolTip="Sub Type of Circular"
                                                DataTextField="RC_NAME" DataValueField="RC_CODE">
                                            </f2f:DropdownListNoValidation>
                                            <span id="SubTypeDesc" style="color: blue;"></span>
                                            <asp:RequiredFieldValidator ID="rfvddlSubTypeofCircular" runat="server" ControlToValidate="ddlSubTypeofCircular" CssClass="text-danger"
                                                ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="True"
                                                ErrorMessage="Select Public, Private or Semi-private."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">SPOC From Compliance Function:<span class="text-danger">*</span></label>
                                            <asp:DropDownList ID="ddlSpocFromCompFn" CssClass="form-select" runat="server"
                                                DataTextField="CCS_NAME" DataValueField="CCS_ID" AppendDataBoundItems="True"
                                                ToolTip="Created By">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvSpocFromCompFn" runat="server" ControlToValidate="ddlSpocFromCompFn" CssClass="text-danger"
                                                ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="true"
                                                ErrorMessage="Select SPOC From Compliance Function"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Circular No:</label>
                                            <F2FControls:F2FTextBox ID="txtCircularNo" CssClass="form-control" runat="server"
                                                MaxLength="50" Text='<%# Bind("CM_CIRCULAR_NO") %>'>
                                            </F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtCircularNo" />
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Circular Date:<span class="text-danger">*</span></label>
                                            <div class="input-group">
                                                <F2FControls:F2FTextBox ID="txtCircularDate" CssClass="form-control" runat="server"
                                                    Text='<%# Bind("CM_DATE", "{0:dd-MMM-yyyy}") %>' MaxLength="11"></F2FControls:F2FTextBox>
                                                <asp:ImageButton ID="ibCircularDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                            </div>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="SaveCircularDetails" CssClass="text-danger"
                                                runat="server" ControlToValidate="txtCircularDate" Display="Dynamic" SetFocusOnError="True"
                                                ErrorMessage="Please enter Circular Date."></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvFromDate" runat="server" ClientValidationFunction="compareEndSystemDates" CssClass="text-danger"
                                                ControlToValidate="txtCircularDate" ErrorMessage="Circular Date should be less than System Date."
                                                Display="Dynamic" OnServerValidate="cvTodate_ServerValidate" ValidationGroup="SaveCircularDetails"></asp:CustomValidator>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Circular Effective Date : <span class="text-danger">*</span></label>
                                            <div class="input-group">
                                                <F2FControls:F2FTextBox ID="txtCircEffDate" CssClass="form-control" runat="server"
                                                    MaxLength="11" ToolTip="Circular Effective Date" Text='<%# Bind("CM_CIRC_EFF_DATE", "{0:dd-MMM-yyyy}") %>'></F2FControls:F2FTextBox>
                                                <asp:ImageButton ID="imgCircEffDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                                    ImageUrl="../../Content/images/legacy/calendar.jpg" ToolTip="Select Date" />
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfvCircEffDate" Width="275px" runat="server" ControlToValidate="txtCircEffDate" CssClass="text-danger"
                                                Display="Dynamic" ValidationGroup="SaveCircularDetails" SetFocusOnError="True"
                                                ErrorMessage="Please enter Circular Effective Date."></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator Width="275px" ID="revCircEffDate" runat="server" ControlToValidate="txtCircEffDate"
                                                ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" CssClass="text-danger"
                                                ValidationGroup="SaveCircularDetails" Display="Dynamic"></asp:RegularExpressionValidator>

                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Subject of the Circular:</label>
                                            <F2FControls:F2FTextBox ID="txtTopic" runat="server" MaxLength="200" ToolTip="Subject of the Circular"
                                                CssClass="form-control" Text='<%# Bind("CM_TOPIC") %>'></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtTopic" />
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Associated Keywords:</label>
                                            <asp:Panel ID="pnlAssociatedKeywords" runat="server">
                                                <div class="custom-checkbox-table">
                                                    <asp:CheckBoxList ID="cbAssociatedKeywords" RepeatColumns="5" runat="server" DataTextField="CKM_NAME" CssClass="form-control"
                                                        DataValueField="CKM_ID" AppendDataBoundItems="True" ToolTip="Associated Keywords" RepeatDirection="Horizontal">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Gist of the Circular:</label>
                                            <F2FControls:F2FTextBox runat="server" ID="FCKE_EditCircularDetails" TextMode="MultiLine" CssClass="ckeditor" Text='<%# Bind("CM_DETAILS") %>'>
                                            </F2FControls:F2FTextBox>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Implications:</label>
                                            <F2FControls:F2FTextBox runat="server" ID="FCKE_EditorImplications" TextMode="MultiLine" CssClass="ckeditor" Text='<%# Bind("CM_IMPLICATIONS") %>'>
                                            </F2FControls:F2FTextBox>
                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label">Issuer Link:</label>
                                            <F2FControls:F2FTextBox ID="txtLink" CssClass="form-control" runat="server" Text='<%# Bind("CM_ISSUING_LINK") %>'
                                                MaxLength="200" Columns="60"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredHyperlinks runat="server" TargetControlID="txtLink" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" />
                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label">Requirement for the Board/Audit committee to approve: <span class="text-danger">*</span></label>
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
                                            <label class="form-label">Details: <span class="text-danger">*</span></label>
                                            <F2FControls:F2FTextBox ID="txtDetails" ToolTip="Details" CssClass="form-control"
                                                runat="server" Columns="64" Rows="5" TextMode="MultiLine">
                                            </F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtDetails" />
                                            <asp:RequiredFieldValidator ID="rfvDetails" runat="server" ControlToValidate="txtDetails"
                                                ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please enter Details."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6 mb-3" id="trNameOfThePolicy" style="visibility: hidden; display: none;">
                                            <label class="form-label">Name of the Policy/Guidelines: <span class="text-danger">*</span></label>
                                            <F2FControls:F2FTextBox ID="txtNameOfThePolicy" ToolTip="Details" CssClass="form-control"
                                                runat="server" Columns="64">
                                            </F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtNameOfThePolicy" />
                                            <asp:RequiredFieldValidator ID="rfvNameOfThePolicy" runat="server" ControlToValidate="txtNameOfThePolicy"
                                                ValidationGroup="SaveCircularDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please enter Name of the Policy/Guidelines."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6 mb-3" id="trFrequency" style="visibility: hidden; display: none;">
                                            <label class="form-label">Frequency:</label>
                                            <F2FControls:F2FTextBox ID="txtFrequency" ToolTip="Details" CssClass="form-control"
                                                runat="server" Columns="64"> </F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtFrequency" />
                                            <asp:RequiredFieldValidator ID="rfvFrequency" runat="server" ControlToValidate="txtFrequency"
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
                                        <div class="col-md-3 mb-3" runat="server" id="trSelectAllto">
                                            <label class="form-label">Select All to be Intimated to:</label>
                                            <asp:CheckBox ID="cbSelectAll" runat="server" ToolTip="Select All to be Intimated to"></asp:CheckBox>
                                        </div>
                                        <div class="col-md-9 mb-3">
                                            <label class="form-label">Intimated To:</label>
                                            <asp:Panel ID="pnlSubmissions" runat="server">
                                                <div class="custom-checkbox-table">
                                                    <asp:CheckBoxList ID="cbSubmissions" RepeatColumns="4" runat="server" DataTextField="CIM_TYPE" CssClass="form-control"
                                                        DataValueField="CIM_ID" AppendDataBoundItems="True" RepeatDirection="Horizontal">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-md-12 mb-3" runat="server" id="trAdditionalMailTo">
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
                                        <div class="col-md-12 mb-3" runat="server" id="trAdditionalMail">
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
                                        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="btn btn-outline-success"
                                            Text="Save Draft" OnClientClick="return onClientValidateEdit()">
                                                                <i class="fa fa-save me-2"></i> Save Draft
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnRefresh" runat="server" Text="Refresh Details" OnClick="btnRefresh_Click">
                                                                <i data-feather="refresh-cw"></i> Refresh Details 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnSendMailFor" runat="server" Text="Send Mail For" CssClass="btn btn-outline-primary">
                                                                 <i class="fa fa-paper-plane me-2"></i> Send Mail For
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnCancel" runat="server" CausesValidation="false"
                                            OnClick="btnCancel_Click" Text="Cancel">
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
                                            </div>
                                        </div>
                                    </div>
                                    <div class="mt-3">
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
                                                        <div>
                                                            <asp:LinkButton ID="lnkListOfReports" CssClass="btn btn-sm btn-info" runat="server" Text="click here" OnClientClick="return onOthersClick('VCRR')">
                                                         <i class="fa fa-download"></i> click here
                                                            </asp:LinkButton>
                                                        </div>
                                                        <div class="ml-2">
                                                            <span>, to view the regulatory reporting added.</span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <cc1:CalendarExtender ID="ceDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibCircularDate"
                                    TargetControlID="txtCircularDate" PopupPosition="TopRight"></cc1:CalendarExtender>
                                <cc1:CalendarExtender ID="ceEffDate" runat="server" PopupButtonID="imgCircEffDate"
                                    TargetControlID="txtCircEffDate" Format="dd-MMM-yyyy" PopupPosition="TopRight"></cc1:CalendarExtender>
                                <ajaxToolkit:CascadingDropDown ID="cddEditIssuingAuthority" runat="server" TargetControlID="ddlCircularAuthority"
                                    Category="IssuingAuthority" PromptText="Select a Issuing Authority" ServicePath="AJAXDropdownCirculars.asmx"
                                    ServiceMethod="GetIssuingAuthority" SelectedValue='<%#Bind("CM_CIA_ID")%>' />
                                <ajaxToolkit:CascadingDropDown ID="cddEditTopic" runat="server" TargetControlID="ddlArea"
                                    ParentControlID="ddlCircularAuthority" Category="Topic" PromptText="Select Topic"
                                    ServicePath="AJAXDropdownCirculars.asmx" ServiceMethod="GetTopicByIssuingAuthority"
                                    SelectedValue='<%#Bind("CM_CAM_ID")%>' />
                                <ajaxToolkit:CascadingDropDown ID="cddddlTypeofCircular" runat="server" TargetControlID="ddlTypeofCircular"
                                    Category="IssuingAuthority" PromptText="Select Type of Document" ServicePath="AJAXDropdownCirculars.asmx"
                                    ServiceMethod="GetTypeofCircular" SelectedValue='<%#Bind("CM_CDTM_ID")%>' />

                                <div>
                                    <div class="modal fade bd-example-modal-xl" id="divSendMailForModal" tabindex="-1" aria-labelledby="myExtraLargeModalLabel" role="dialog">
                                        <div class="modal-dialog modal-xl" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h6 class="modal-title">Send mail</h6>
                                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col-md-12 mb-3">
                                                            <label class="form-label">Audit Trail: <span class="text-danger">*</span></label>
                                                            <asp:Label ID="lblSendMailAuditTrail" runat="server" Text='<%# Convert.ToBase64String(Encoding.UTF8.GetBytes(Eval("CM_AUDIT_TRAIL").ToString().Replace("\r\n", "<br>")))%>'></asp:Label>
                                                        </div>
                                                        <div class="col-md-12 mb-3">
                                                            <label class="form-label">Send Mail For: <span class="text-danger">*</span></label>
                                                            <div class="custom-checkbox-table">
                                                                <asp:CheckBoxList ID="cbSendMailFor" RepeatColumns="3" runat="server" DataTextField="RC_NAME"
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
                                                        <%--<asp:Button ID="btnSubmit" runat="server" Text="Send" CssClass="btn btn-primary"
                                                            OnClick="btnSubmit_ServerClick" ValidationGroup="SendMailFor"
                                                            OnClientClick="return onClientValidateSendMail()" />--%>
                                                        <input type="button" id="btnSubmit" runat="server" value="Send" class="btn btn-outline-success"
                                                            onserverclick="btnSubmit_ServerClick" validationgroup="SendMailFor" />
                                                        <input id="btncCancel" runat="server" type="button" value="Cancel" data-bs-dismiss="modal"
                                                            class="modalCloseImg simplemodal-close btn btn-outline-danger" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </EditItemTemplate>
                        </asp:FormView>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
    <!-- end row -->
</asp:Content>
