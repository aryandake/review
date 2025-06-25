<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" Async="true" Title="Submission Master"
    Inherits="Fiction2Fact.Projects.Submissions.CommonSubmission" EnableEventValidation="true" CodeBehind="CommonSubmission.aspx.cs" %>

<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="custom" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="a" Namespace="Fiction2Fact.Controls" Assembly="RequiredIfValidatorRadioButtonList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js")%>"></script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/dateformatvalidation.js")%>"></script>

    <script type="text/javascript">    

        function validateNumber(e) {
            const pattern = /^[0-9]$/;

            return pattern.test(e.key)
        }

        function showhideTypeBased(rbltypeName) {
            var rb = document.getElementById(rbltypeName + '_0');
            var rb1 = document.getElementById(rbltypeName + '_1');
            var elem = document.getElementById('FixedDateBaseSection');
            var elem1 = document.getElementById('EventBasedSection');
            var hf = document.getElementById('hfFixedOrEvent');
            if (rb == null)
                return;

            if (rb.checked) {
                hf.value = 'F';
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
                elem1.style.display = 'none';
                elem1.style.visibility = 'hidden';
            }
            else if (rb1.checked) {
                document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency_0').checked = false;
                document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency_1').checked = false;
                document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency_2').checked = false;
                document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency_3').checked = false;
                document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency_4').checked = false;
                document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency_5').checked = false;
                document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency_6').checked = false;
                document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency_7').checked = false;

                var elem01 = document.getElementById('OnlyOnceSection');
                var elem02 = document.getElementById('WeeklySection');
                var elem03 = document.getElementById('MonthlySection')
                var elem04 = document.getElementById('QuarterSection')
                var elem05 = document.getElementById('FirstHafSection');
                var elem06 = document.getElementById('YearSection');
                var elem07 = document.getElementById('FortnightlySection');

                hf.value = 'E';
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
                elem1.style.display = 'block';
                elem1.style.visibility = 'visible';

                elem01.style.display = 'none';
                elem01.style.visibility = 'hidden';
                elem02.style.display = 'none';
                elem02.style.visibility = 'hidden';
                elem03.style.display = 'none';
                elem03.style.visibility = 'hidden';
                elem04.style.display = 'none';
                elem04.style.visibility = 'hidden';
                elem05.style.display = 'none';
                elem05.style.visibility = 'hidden';
                elem06.style.display = 'none';
                elem06.style.visibility = 'hidden';
                elem07.style.display = 'none';
                elem07.style.visibility = 'hidden';
            }

            //<<Here, nothing is selected, so hide all sections.
            else {
                document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency_0').checked = false;
                document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency_1').checked = false;
                document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency_2').checked = false;
                document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency_3').checked = false;
                document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency_4').checked = false;
                document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency_5').checked = false;
                document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency_6').checked = false;
                document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency_7').checked = false;

                var elem01 = document.getElementById('OnlyOnceSection');
                var elem02 = document.getElementById('WeeklySection');
                var elem03 = document.getElementById('MonthlySection')
                var elem04 = document.getElementById('QuarterSection')
                var elem05 = document.getElementById('FirstHafSection');
                var elem06 = document.getElementById('YearSection');
                var elem07 = document.getElementById('FortnightlySection');

                hf.value = 'E';
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
                elem1.style.display = 'none';
                elem1.style.visibility = 'hidden';

                elem01.style.display = 'none';
                elem01.style.visibility = 'hidden';
                elem02.style.display = 'none';
                elem02.style.visibility = 'hidden';
                elem03.style.display = 'none';
                elem03.style.visibility = 'hidden';
                elem04.style.display = 'none';
                elem04.style.visibility = 'hidden';
                elem05.style.display = 'none';
                elem05.style.visibility = 'hidden';
                elem06.style.display = 'none';
                elem06.style.visibility = 'hidden';
                elem07.style.display = 'none';
                elem07.style.visibility = 'hidden';
            }
        }

        function showhideOtherFrequencyPanels(rblName) {
            rblName = 'ctl00_ContentPlaceHolder1_rblFrequency';
            var rb = document.getElementById(rblName + '_0');
            var elem = document.getElementById('OnlyOnceSection');

            if (rb == null)
                return;

            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';

            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }

            rb = document.getElementById(rblName + '_2');
            elem = document.getElementById('WeeklySection');

            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';

            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }


            rb = document.getElementById(rblName + '_3');
            elem = document.getElementById('FortnightlySection');
            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }


            rb = document.getElementById(rblName + '_4');
            elem = document.getElementById('MonthlySection');
            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }

            rb = document.getElementById(rblName + '_5');
            elem = document.getElementById('QuarterSection');
            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }

            rb = document.getElementById(rblName + '_6');
            elem = document.getElementById('FirstHafSection');
            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }

            rb = document.getElementById(rblName + '_7');
            elem = document.getElementById('YearSection');
            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }

        }

        function showhideEscalationDaysSection() {
            var rb = document.getElementById('ctl00_ContentPlaceHolder1_rblEscalate_0');
            var elem = document.getElementById('EscalationDaysSection');
            if (rb == null) {
                //ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvlvl0Esc'), false);
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvlvl1Esc'), false);
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvlvl2Esc'), false);
                return;
            }

            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
                //ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvlvl0Esc'), true);
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvlvl1Esc'), true);
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvlvl2Esc'), true);
            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
                //ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvlvl0Esc'), false);
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvlvl1Esc'), false);
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvlvl2Esc'), false);
            }
        }

        //<< Added By Vivek on 27-Jun-2017
        function onClientSaveClick() {

            var fuctrl = document.getElementById('ctl00_ContentPlaceHolder1_fuEditFileUpload').value;
            if (fuctrl != '') {
                alert("Please upload the selected file.");
                return false;
            }

            //ReportingDept
            var tableReportingDept = document.getElementById('ctl00_ContentPlaceHolder1_cbReportingDept');
            if (tableReportingDept != null) {
                var countRD = tableReportingDept.querySelectorAll('input[type=checkbox]:checked').length;
                if (countRD <= 0) {
                    alert("Please select reporting department1.");
                    return false;
                }
            }
            var ddlRD = document.getElementById("ctl00_ContentPlaceHolder1_ddlReportDept");
            if (ddlRD != null) {
                var RD = ddlRD.value;
                if (RD == '') {
                    alert("Please select reporting department2.");
                    return false;
                }
            }

            var tableAuthority = document.getElementById('ctl00_ContentPlaceHolder1_cblSegment');
            var countAuthority = tableAuthority.querySelectorAll('input[type=checkbox]:checked').length;
            if (countAuthority <= 0) {
                //<<Added by Ankur Tyagi on 29-Apr-2025 for Project Id : 2395
                alert("Please select Reporting to.");
                //>>
                return false;
            }

            //---------------------------------

            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvEvent'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvEAgenda'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvStart'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvEndDate'), false);


            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvOnceFromDate'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvOnceToDate'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_cvOnlyOnce'), false);
            //<<Added by Ankur Tyagi on 28-Apr-2025 for Project Id : 2395
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_cvEffectiveOnceFromDt'), false);
            //>>

            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvWeeklyFrom'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvWeeklTo'), false);

            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvMonthlyFrom'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvMonthlyTo'), false);

            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvQ1From'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvQ1To'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvQ2From'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvQ2To'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvQ3From'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvQ3To'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvQ4From'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvQ4To'), false);

            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvFHFrom'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rivFHTo'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rivSHFrom'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rivSHTo'), false);

            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rivYearlyFrom'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvYearlyTo'), false);

            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rivF1FromDate'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rivF1ToDate'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rivF2FromDate'), false);
            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rivF2ToDate'), false);

            //---------------------------------

            var tableType = document.getElementById('ctl00_ContentPlaceHolder1_rblType');
            var countType = tableType.querySelectorAll('input[type=radio]:checked').length;
            if (countType > 0) {
                var rbSel = tableType.querySelectorAll('input[type=radio]:checked')[0].value;
                if (rbSel == 'F') {
                    var tableFreq = document.getElementById('ctl00_ContentPlaceHolder1_rblFrequency');
                    countFreq = tableFreq.querySelectorAll('input[type=radio]:checked').length;
                    if (countFreq > 0) {
                        var selFreq = tableFreq.querySelectorAll('input[type=radio]:checked')[0].value;
                        if (selFreq == "Only Once") {
                            //rfvOnceFromDate / rfvOnceToDate / cvOnlyOnce
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvOnceFromDate'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvOnceToDate'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_cvOnlyOnce'), true);
                            //<<Added by Ankur Tyagi on 28-Apr-2025 for Project Id : 2395
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_cvEffectiveOnceFromDt'), true);
                            //>>
                        }
                        else if (selFreq == "Daily") {
                        }
                        else if (selFreq == "Weekly") {
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvWeeklyFrom'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvWeeklTo'), true);
                        }
                        else if (selFreq == "Monthly") {
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvMonthlyFrom'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvMonthlyTo'), true);
                        }
                        else if (selFreq == "Quarterly") {
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvQ1From'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvQ1To'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvQ2From'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvQ2To'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvQ3From'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvQ3To'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvQ4From'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvQ4To'), true);
                        }
                        else if (selFreq == "Half Yearly") {
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvFHFrom'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rivFHTo'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rivSHFrom'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rivSHTo'), true);
                        }
                        else if (selFreq == "Yearly") {
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rivYearlyFrom'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvYearlyTo'), true);
                        }
                        else if (selFreq == "Fortnightly") {
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rivF1FromDate'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rivF1ToDate'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rivF2FromDate'), true);
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rivF2ToDate'), true);
                        }
                    }
                    else {
                        alert("Please select Frequency.");
                        return false;
                    }
                }
                else if (rbSel == 'E') {
                    ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvEvent'), true);

                    var selevnt = document.getElementById("ctl00_ContentPlaceHolder1_ddlEvent");
                    var evntval = selevnt.value;
                    if (evntval != '') {
                        var evntAgenda = document.getElementById('ctl00_ContentPlaceHolder1_rblAssociatedWith')
                        if (evntAgenda != null)
                            ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvEAgenda'), true);
                    }
                    ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvStart'), true);
                    ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvEndDate'), true);
                }
            }
            else {
                alert("Please select Type.");
                return false;
            }

            var rb = document.getElementById('ctl00_ContentPlaceHolder1_rblEscalate_0');
            if (rb != null && rb.checked) {
                var lv1 = document.getElementById('ctl00_ContentPlaceHolder1_txtlevel1');
                var lv2 = document.getElementById('ctl00_ContentPlaceHolder1_txtlevel2');
                if (parseInt(lv2.value) > parseInt(lv1.value)) {
                    alert("Level 2 Escalation Days cannot be greater than Level 1 Escalation Days.");
                    lv1.focus();
                    return false;
                }
            }

            var validated = Page_ClientValidate("SaveSubmissionDetails");
            if (validated) {

                var IsDoubleClickFlagSet = document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value;
                if (IsDoubleClickFlagSet == 'Yes') {
                    alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                    return false;
                }
                else {
                    document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                    return true;
                }
            }
        }
        //>>
        function onClientDeleteClick() {
            if (!confirm('Are you sure that you want to delete this record?')) return false;
            return true;
        }

        function compareDates(source, arguments) {
            try {
                //var ContractTemplateId = document.getElementById('ctl00_ContentPlaceHolder1_hfCTId').value;
                //if (ContractTemplateId == '' || ContractTemplateId == null) {

                var Fromdate = document.getElementById('ctl00_ContentPlaceHolder1_txtOnceFromDate').value;
                var ToDate = document.getElementById('ctl00_ContentPlaceHolder1_txtOnceToDate').value;

                if (Fromdate.value != '') {
                    if (compare2DatesNew(Fromdate, ToDate) < 1) {
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
    <%--<<Added by Ankur Tyagi on 28-Apr-2025 for Project Id : 2395--%>
    <script>
        function CompareEffectiveOnceFromDt(src, arg) {
            if (Date.parse($("#ctl00_ContentPlaceHolder1_txtEffectiveDate").val()) <= Date.parse($("#ctl00_ContentPlaceHolder1_txtOnceFromDate").val())) {
                arg.IsValid = true;
            }
            else {
                arg.IsValid = false;
            }
        }
    </script>
    <%-->>--%>

    <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />
    <asp:HiddenField ID="hfType" runat="server" />
    <asp:HiddenField ID="hfCircId" runat="server" />
    <input id="hfFixedOrEvent" type="hidden" />
    <asp:HiddenField ID="hfCurDate" runat="server" />
    <asp:HiddenField ID="hfSMId" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">New Submission</h4>
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
                    <div class="mb-3">
                        <asp:LinkButton ID="btnAddMore" runat="server" Visible="false" CssClass="btn btn-outline-primary"
                            Text="Add Another Submission" OnClick="AddMoreSub_Click">
                            <i class="fa fa-plus"></i> Add Another Submission                               
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="lnkBack" runat="server" CausesValidation="false" Visible="false"
                            Text="Back" OnClick="btnCancel_Click">
                            <i class="fa fa-arrow-left me-2"></i> Back                   
                        </asp:LinkButton>
                    </div>
                    <asp:Panel ID="pnlSubmissions" runat="server">

                        <div class="mt-3">
                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSaveTop" runat="server" Text="Save"
                                OnClick="btnSave_Click" OnClientClick="return onClientSaveClick()">
        <i class="fa fa-save me-2"></i> Submit                    
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnCancelTop" runat="server" CausesValidation="false"
                                Text="Cancel" OnClick="btnCancel_Click">
        <i class="fa fa-arrow-left me-2"></i> Cancel                   
                            </asp:LinkButton>
                        </div>

                        <div class="row mt-5">
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Tracking Department:<span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlSubType" CssClass="form-select" AppendDataBoundItems="true"
                                    runat="server" DataValueField="STM_ID" DataTextField="STM_TYPE">
                                    <%--AutoPostBack="true"
                        OnSelectedIndexChanged="ddlSubType_SelectedIndexChanged"--%>
                                    <asp:ListItem Value="">(Select)</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvSubType" runat="server" ControlToValidate="ddlSubType" CssClass="text-danger" ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please select department tracking the submission."></asp:RequiredFieldValidator>
                            </div>


                            <div class="col-md-4 mb-3" style="display: none; visibility: hidden">
                                <label class="form-label">Line of Business: <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlLOB" CssClass="form-select" runat="server" DataTextField="LEM_NAME"
                                    DataValueField="LEM_ID" AppendDataBoundItems="True" ToolTip="Line of Business">
                                </asp:DropDownList>
                                <%--<asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                    ErrorMessage="Please select Line of Business."></asp:RequiredFieldValidator>--%>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Notification/Email date :</label>
                                <div class="input-group">
                                    <F2FControls:F2FTextBox ID="txtCircularDate" runat="server" Columns="15" MaxLength="20"
                                        CssClass="form-control"></F2FControls:F2FTextBox>
                                    <asp:ImageButton ID="imgCircularDate" runat="server" AlternateText="Click to show calendar"
                                        CssClass="custom-calendar-icon" ImageUrl="../../Content/images/legacy/calendar.jpg" />&nbsp;
        <ajaxToolkit:CalendarExtender ID="CalendarExtender6" runat="server" PopupButtonID="imgCircularDate"
            TargetControlID="txtCircularDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>

                                    <asp:CustomValidator ID="cvNotificationDate" runat="server" ValidationGroup="SaveSubmissionDetails"
                                        ControlToValidate="txtCircularDate" CssClass="text-danger" ErrorMessage="Notification/Email date cannot be greater than current date."
                                        Display="Dynamic" ValidateEmptyText="true" ClientValidationFunction="CompareNotificationDateSytemDate"> 
                                    </asp:CustomValidator>

                                </div>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Report Filing Effective Date :<span class="text-danger">*</span></label>
                                <div class="input-group">
                                    <F2FControls:F2FTextBox ID="txtEffectiveDate" runat="server" Columns="15" MaxLength="20"
                                        CssClass="form-control"></F2FControls:F2FTextBox>
                                    <asp:ImageButton ID="imgEffectiveDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />&nbsp;
                                </div>
                                <asp:RequiredFieldValidator ID="rfvEffctDate" runat="server" ControlToValidate="txtEffectiveDate" CssClass="text-danger"
                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please select Report Filing Effective Date."></asp:RequiredFieldValidator>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" PopupButtonID="imgEffectiveDate"
                                    TargetControlID="txtEffectiveDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEffectiveDate" CssClass="text-danger"
                                    ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="SaveSubmissionDetails"
                                    ControlToValidate="txtEffectiveDate" CssClass="text-danger" ErrorMessage="Report Filing Effective Date cannot be lower than Notification/Email date."
                                    Display="Dynamic" ValidateEmptyText="true" ClientValidationFunction="CompareFilingEffectiveDateSytemDate"> 
                                </asp:CustomValidator>
                            </div>
                            <div class="col-md-12 mb-3" id="divReportingDeptcb" runat="server" visible="false">
                                <label class="form-label">Reporting Department:<span class="text-danger">*</span></label>
                                <div class="custom-checkbox-table">
                                    <asp:CheckBoxList ID="cbReportingDept" RepeatColumns="4" CssClass="form-control" AppendDataBoundItems="true"
                                        runat="server" DataValueField="SRD_ID" DataTextField="SRD_NAME">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="col-md-4 mb-3" id="divReportingDeptdd" runat="server" visible="false">
                                <label class="form-label">Reporting Department:</label>
                                <asp:DropDownList ID="ddlReportDept" AppendDataBoundItems="true" CssClass="form-select"
                                    runat="server" DataValueField="SRD_ID" DataTextField="SRD_NAME">
                                    <asp:ListItem Value="0">(Select)</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4 mb-3" style="display: none;">
                                <label class="form-label">Reporting Department Owners:</label>
                                <div class="custom-checkbox-table">
                                    <asp:CheckBoxList ID="cbDeptOwner" RepeatColumns="5" AppendDataBoundItems="true" CssClass="form-control"
                                        runat="server" DataValueField="SRDOM_ID" DataTextField="SRDOM_EMP_NAME">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <%--<<Modified by Ankur Tyagi on 29-Apr-2025 for Project Id : 2395--%>
                            <%--Authority changes to Reporting to--%>
                            <div class="col-md-12 mb-3">
                                <label class="form-label">Reporting to:<span class="text-danger">*</span></label>
                                <div class="custom-checkbox-table">
                                    <asp:CheckBoxList ID="cblSegment" runat="server" RepeatColumns="7" CssClass="form-control" AppendDataBoundItems="true"
                                        DataTextField="SSM_NAME" DataValueField="SSM_ID">
                                    </asp:CheckBoxList>
                                </div>
                                <custom:CheckBoxListValidator runat="server" ID="cblvSegment" ControlToValidate="cblSegment"
                                    Display="Dynamic" ErrorMessage="Please select Reporting to." CssClass="text-danger" Enabled="false" MinimumNumberOfSelectedCheckBoxes="0" ValidationGroup="SaveSubmissionDetails"></custom:CheckBoxListValidator>
                            </div>
                            <%-->>--%>
                            <div class="col-md-12 mb-3">
                                <label class="form-label">Reference Circular / Notification / Act :<span class="text-danger">*</span></label>
                                <F2FControls:F2FTextBox ID="txtReference" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    Rows="3" MaxLength="500"></F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtReference" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtReference" CssClass="text-danger"
                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please enter Reference Circular / Notification / Act."></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revReference" ControlToValidate="txtReference"
                                    Display="Dynamic" ErrorMessage="Reference Circular / Notification / Act exceeding 4000 characters" ValidationExpression="^[\s\S]{0,4000}$" CssClass="text-danger"
                                    runat="server" SetFocusOnError="True" ValidationGroup="SaveSubmissionDetails"> Exceeding
                    4000 characters</asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Section/Clause :<span class="text-danger">*</span></label>
                                <F2FControls:F2FTextBox ID="txtSection" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    Rows="3" MaxLength="2000">
                                </F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtSection" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSection" CssClass="text-danger"
                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please enter Section/Clause."></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtSection"
                                    Display="Dynamic" ErrorMessage="Section/Clause exceeding 2000 characters" ValidationExpression="^[\s\S]{0,2000}$" CssClass="text-danger"
                                    runat="server" SetFocusOnError="True" ValidationGroup="SaveSubmissionDetails"> Exceeding
                    2000 characters</asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Particulars:<span class="text-danger">*</span></label>
                                <F2FControls:F2FTextBox ID="txtParticulars" CssClass="form-control" TextMode="MultiLine"
                                    Columns="70" Rows="3" MaxLength="2000" runat="server">
                                </F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtParticulars" />
                                <asp:RequiredFieldValidator ID="rfvParticulars" runat="server" ControlToValidate="txtParticulars" CssClass="text-danger"
                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please enter Particulars."></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revPerticulars" ControlToValidate="txtParticulars"
                                    Display="Dynamic" ErrorMessage="Particulars exceeding 2000 characters" ValidationExpression="^[\s\S]{0,2000}$" CssClass="text-danger"
                                    runat="server" SetFocusOnError="True" ValidationGroup="SaveSubmissionDetails"> Exceeding
                    2000 characters</asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Brief Description:<span class="text-danger">*</span></label>
                                <F2FControls:F2FTextBox ID="txtDescription" CssClass="form-control" TextMode="MultiLine"
                                    Columns="70" Rows="3" MaxLength="2000" runat="server">
                                </F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtDescription" />
                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" CssClass="text-danger"
                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please enter Description."></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revDescription" ControlToValidate="txtDescription"
                                    Display="Dynamic" ErrorMessage="Brief Description exceeding 2000 characters" ValidationExpression="^[\s\S]{0,2000}$" CssClass="text-danger"
                                    runat="server" SetFocusOnError="True" ValidationGroup="SaveSubmissionDetails">Exceeding 2000 characters</asp:RegularExpressionValidator>
                            </div>

                            <div class="col-md-4 mb-3">
                                <label class="form-label">Priority:<span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="">Select One</asp:ListItem>
                                    <asp:ListItem Value="H">High</asp:ListItem>
                                    <asp:ListItem Value="M">Medium</asp:ListItem>
                                    <asp:ListItem Value="L">Low</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPriority" runat="server" ControlToValidate="ddlPriority"
                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger" ErrorMessage="Please select Submission priority."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Reminder days for reporting function:</label>
                                <F2FControls:F2FTextBox ID="txtlevel0" runat="server" MaxLength="2" CssClass="form-control"
                                    Columns="4"></F2FControls:F2FTextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="ftelevel0" runat="server" TargetControlID="txtlevel0"
                                    FilterType="Numbers"></ajaxToolkit:FilteredTextBoxExtender>

                                <asp:RequiredFieldValidator ID="rfvlvl0Esc" runat="server" ControlToValidate="txtlevel0"
                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger" ErrorMessage="Please enter level 0 escalation days."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">To be Escalated:<span class="text-danger">*</span></label>
                                <div class="custom-checkbox-table">
                                    <asp:RadioButtonList ID="rblEscalate" runat="server" RepeatColumns="2" CssClass="form-control">
                                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                                        <asp:ListItem Value="N">No</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvEscalate" runat="server" ControlToValidate="rblEscalate"
                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger" ErrorMessage="Please select To be Escalated."></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div id="EscalationDaysSection" style="display: none; visibility: hidden">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Level 1 Escalation Days:</label>
                                    <F2FControls:F2FTextBox ID="txtlevel1" runat="server" MaxLength="2" CssClass="form-control"
                                        Columns="4"></F2FControls:F2FTextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="ftelevel1" runat="server" TargetControlID="txtlevel1"
                                        FilterType="Numbers"></ajaxToolkit:FilteredTextBoxExtender>

                                    <asp:RequiredFieldValidator ID="rfvlvl1Esc" runat="server" ControlToValidate="txtlevel1" Enabled="false"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger" ErrorMessage="Please enter level 1 escalation days."></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Level 2 Escalation Days:</label>
                                    <F2FControls:F2FTextBox ID="txtlevel2" runat="server" MaxLength="2" CssClass="form-control"
                                        Columns="4"></F2FControls:F2FTextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="fteleve2" runat="server" TargetControlID="txtlevel2"
                                        FilterType="Numbers"></ajaxToolkit:FilteredTextBoxExtender>

                                    <asp:RequiredFieldValidator ID="rfvlvl2Esc" runat="server" ControlToValidate="txtlevel2" Enabled="false"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger" ErrorMessage="Please enter level 2 escalation days."></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label">Type:<span class="text-danger">*</span></label>
                                <div class="custom-checkbox-table">
                                    <asp:RadioButtonList ID="rblType" runat="server" CssClass="form-control" RepeatColumns="2">
                                        <asp:ListItem Value="F">Fixed Date</asp:ListItem>
                                        <asp:ListItem Value="E">Event Based</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="rblType" CssClass="text-danger" ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please select Type."></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-4 mb-3">
                                <label class="form-label">Finance Approval Required: <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlFSAppReq" CssClass="form-select" runat="server" ToolTip="FS Approval Required">
                                    <asp:ListItem Value="">(Select)</asp:ListItem>
                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                    <asp:ListItem Value="N">No</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlFSAppReq"
                                    CssClass="text-danger" ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please select Finance Approval Required."></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div id="EventBasedSection" style="display: none; visibility: hidden">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Event Name:<span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="ddlEvent" CssClass="form-select" AutoPostBack="true" AppendDataBoundItems="true"
                                        runat="server" OnSelectedIndexChanged="ddlEvent_SelectedIndexChanged" DataValueField="EM_ID"
                                        DataTextField="EM_EVENT_NAME">
                                        <asp:ListItem Value="">(None)</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvEvent" runat="server"
                                        ControlToValidate="ddlEvent" Display="Dynamic" CssClass="text-danger"
                                        Enabled="false" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Select Event Type">
                                    </asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Event Agenda:<span class="text-danger">*</span></label>
                                    <div class="custom-checkbox-table">
                                        <asp:RadioButtonList ID="rblAssociatedWith" RepeatColumns="5" CssClass="form-control" runat="server" DataTextField="EP_NAME"
                                            DataValueField="EP_ID" AppendDataBoundItems="true">
                                        </asp:RadioButtonList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvEAgenda" runat="server" CssClass="text-danger"
                                        ControlToValidate="rblAssociatedWith"
                                        Display="Dynamic" Enabled="false" ValidationGroup="SaveSubmissionDetails"
                                        ErrorMessage="Please Select Agenda Type"> 
                                    </asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Start Date (No. of Days before (-) / after the event (+)):<span class="text-danger">*</span></label>
                                    <F2FControls:F2FTextBox ID="txtStartDays" CssClass="form-control" runat="server" MaxLength="4"
                                        Columns="4"></F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredNumber runat="server" TargetControlID="txtStartDays" FilterType="Numbers, Custom" ValidChars="-" />
                                    <asp:RequiredFieldValidator ID="rfvStart" runat="server" ControlToValidate="txtStartDays" Display="Dynamic"
                                        Enabled="false" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter Start No of Days" CssClass="text-danger">
                                    </asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">End Date (No. of Days before (-) / after the event (+)): <span class="text-danger">*</span></label>
                                    <F2FControls:F2FTextBox ID="txtEndDays" runat="server" CssClass="form-control" MaxLength="4"
                                        Columns="4"></F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredNumber runat="server" TargetControlID="txtEndDays" FilterType="Numbers, Custom" ValidChars="-" />
                                    <asp:RequiredFieldValidator ID="rfvEndDate" runat="server"
                                        ControlToValidate="txtEndDays" Display="Dynamic" Enabled="false" CssClass="text-danger"
                                        ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter End No of Days">
                                    </asp:RequiredFieldValidator>
                                    <asp:CompareValidator runat="server" ID="cmpNumbers" ControlToValidate="txtStartDays"
                                        ControlToCompare="txtEndDays" Operator="LessThanEqual" Type="Integer" ErrorMessage="The Start Date should be less than or equal to the End Date"
                                        ValidationGroup="SaveSubmissionDetails" />
                                </div>
                            </div>
                        </div>
                        <div id="FixedDateBaseSection" style="display: none; visibility: hidden">
                            <div class="row">
                                <div class="col-md-12 mb-3">
                                    <label class="form-label">Frequency <span class="text-danger">(The due dates pertain to current Financial Year)</span>:</label>
                                    <div class="custom-checkbox-table">
                                        <asp:RadioButtonList ID="rblFrequency" runat="server" CssClass="form-control" RepeatColumns="8">
                                            <asp:ListItem Value="Only Once">Adhoc</asp:ListItem>
                                            <asp:ListItem Text="Daily" Value="Daily">Daily</asp:ListItem>
                                            <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                                            <asp:ListItem Value="Fortnightly">Fortnightly</asp:ListItem>
                                            <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                            <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>
                                            <asp:ListItem Value="Half Yearly">Half Yearly</asp:ListItem>
                                            <asp:ListItem Value="Yearly">Yearly</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rifFrequency" runat="server"
                                        ControlToValidate="rblFrequency" Display="Dynamic"
                                        Enabled="false" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Select Frequency" CssClass="text-danger">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div id="OnlyOnceSection" style="display: none; visibility: hidden">
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Adhoc: Internal due date</label>
                                    <div class="input-group">
                                        <%--<F2FControls:F2FTextBox ID="TextBox1" Visible="false" CssClass="form-control" runat="server"
                                        MaxLength="4" Columns="4"></F2FControls:F2FTextBox>--%>
                                        <F2FControls:F2FTextBox ID="txtOnceFromDate" CssClass="form-control" runat="server"
                                            MaxLength="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgOnceFromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />

                                    </div>
                                    <ajaxToolkit:CalendarExtender ID="ceOnceFromDate" runat="server" PopupButtonID="imgOnceFromDate"
                                        TargetControlID="txtOnceFromDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>

                                    <%--<a:RequiredIfValidatorRadioButtonList ID="rifOnlyOnce" runat="server" TriggerIndex="0"
                                        ControlToCompare="rblFrequency" ControlToValidate="txtOnceFromDate" Display="Dynamic"
                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter Only Once: Start Date." CssClass="text-danger">
                                    *</a:RequiredIfValidatorRadioButtonList>--%>

                                    <asp:RequiredFieldValidator ID="rfvOnceFromDate" runat="server" ControlToValidate="txtOnceFromDate" Enabled="false"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                        ErrorMessage="Please enter Adhoc: Internal due date"></asp:RequiredFieldValidator>

                                    <asp:RegularExpressionValidator ID="rev" runat="server" ControlToValidate="txtOnceFromDate"
                                        ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" CssClass="text-danger"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <%--<<Added by Ankur Tyagi on 28-Apr-2025 for Project Id : 2395--%>
                                    <asp:CustomValidator ID="cvEffectiveOnceFromDt" runat="server" ValidationGroup="SaveSubmissionDetails"
                                        ControlToValidate="txtOnceFromDate" CssClass="text-danger" ErrorMessage="Adhoc: Internal due date shall be greater than or equal to Effective Date."
                                        Display="Dynamic" ValidateEmptyText="true" ClientValidationFunction="CompareEffectiveOnceFromDt" />
                                    <%-->>--%>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Adhoc: Regulatory due date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtOnceToDate" CssClass="form-control" runat="server"
                                            MaxLength="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgOnceTODate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <ajaxToolkit:CalendarExtender ID="ceOnceTodate" runat="server" PopupButtonID="imgOnceTODate"
                                        TargetControlID="txtOnceToDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>

                                    <%--<a:RequiredIfValidatorRadioButtonList ID="rifOnceToDate" runat="server" TriggerIndex="0"
                                        ControlToCompare="rblFrequency" ControlToValidate="txtOnceToDate" CssClass="text-danger" Display="Dynamic"
                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter Only Once: End Date.">
                                    *</a:RequiredIfValidatorRadioButtonList>--%>

                                    <asp:RequiredFieldValidator ID="rfvOnceToDate" runat="server" ControlToValidate="txtOnceToDate" Enabled="false"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                        ErrorMessage="Please enter Adhoc: Regulatory due date"></asp:RequiredFieldValidator>

                                    <asp:RegularExpressionValidator ID="revOnceDateTo" runat="server" ControlToValidate="txtOnceToDate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>

                                    <asp:CustomValidator ID="cvOnlyOnce" runat="server" ClientValidationFunction="compareDates"
                                        ControlToValidate="txtOnceToDate" ErrorMessage="Adhoc: Internal due date should be less than or Equal to Adhoc: Regulatory due Date." CssClass="text-danger"
                                        Display="Dynamic" OnServerValidate="cvdate_ServerValidate" ValidationGroup="SaveSubmissionDetails"></asp:CustomValidator>

                                    <%--<asp:CustomValidator ID="cvOnlyOnce" runat="server" ClientValidationFunction="compareEndSystemDates" CssClass="span"
                            ControlToValidate="txtReleaseDate" ErrorMessage="Released Date should be less than System Date."
                            Display="Dynamic" OnServerValidate="cvTodate_ServerValidate" ValidationGroup="Save"></asp:CustomValidator>--%>
                                </div>
                            </div>
                        </div>
                        <div id="WeeklySection" style="display: none; visibility: hidden">
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Weekly From Day</label>
                                    <asp:DropDownList ID="ddlFromWeekDays" runat="server" CssClass="form-select">
                                        <asp:ListItem Value="">(None)</asp:ListItem>
                                        <asp:ListItem Value="Monday">Monday</asp:ListItem>
                                        <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                                        <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                                        <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                                        <asp:ListItem Value="Friday">Friday</asp:ListItem>
                                        <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                                        <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<a:RequiredIfValidatorRadioButtonList ID="rfvWeeklyFrom" runat="server" TriggerIndex="2"
                                        ControlToCompare="rblFrequency" ControlToValidate="ddlFromWeekDays" Display="Dynamic"
                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please select Weekly From Day." CssClass="text-danger">
                                    *</a:RequiredIfValidatorRadioButtonList>--%>
                                    <asp:RequiredFieldValidator ID="rfvWeeklyFrom" runat="server" ControlToValidate="ddlFromWeekDays" Enabled="false" InitialValue=""
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                        ErrorMessage="Please select Weekly From Day."></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Weekly To day</label>
                                    <asp:DropDownList ID="ddlToWeekDays" runat="server" CssClass="form-select">
                                        <asp:ListItem Value="">(None)</asp:ListItem>
                                        <asp:ListItem Value="Monday">Monday</asp:ListItem>
                                        <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                                        <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                                        <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                                        <asp:ListItem Value="Friday">Friday</asp:ListItem>
                                        <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                                        <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<a:RequiredIfValidatorRadioButtonList ID="rfvWeeklTo" runat="server" TriggerIndex="2"
                                        ControlToCompare="rblFrequency" ControlToValidate="ddlToWeekDays" Display="Dynamic"
                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Select Weekly To Day." CssClass="text-danger">
                                    *</a:RequiredIfValidatorRadioButtonList>--%>
                                    <asp:RequiredFieldValidator ID="rfvWeeklTo" runat="server" ControlToValidate="ddlToWeekDays" Enabled="false" InitialValue=""
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                        ErrorMessage="Please select Weekly To Day."></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div id="FortnightlySection" style="display: none; visibility: hidden">
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">First Fortnightly: Internal due date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtFortnightly1FromDate" runat="server" onkeypress="return validateNumber(event)" MaxLength="2" CssClass="form-control"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgF1FD" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <asp:RequiredFieldValidator ID="rivF1FromDate" runat="server"
                                        ControlToValidate="txtFortnightly1FromDate" Display="Dynamic" CssClass="text-danger"
                                        Enabled="false" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter first fortnightly: Internal due date.">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revF1FromDate" runat="server" ControlToValidate="txtFortnightly1FromDate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ceF1FromDate" runat="server" PopupButtonID="imgF1FD"
                                        TargetControlID="txtFortnightly1FromDate" Format="dd"></ajaxToolkit:CalendarExtender>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">First Fortnightly: Regulatory due date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtFortnightly1ToDate" runat="server" MaxLength="2" onkeypress="return validateNumber(event)" CssClass="form-control"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgF1TD" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <asp:RequiredFieldValidator ID="rivF1ToDate" runat="server"
                                        ControlToValidate="txtFortnightly1ToDate" Display="Dynamic" CssClass="text-danger"
                                        Enabled="false" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter first fortnightly: Regulatory due date.">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revF1ToDate" runat="server" ControlToValidate="txtFortnightly1ToDate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ceF1ToDate" runat="server" PopupButtonID="imgF1TD"
                                        TargetControlID="txtFortnightly1ToDate" Format="dd"></ajaxToolkit:CalendarExtender>

                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Second Fortnightly: Internal due date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtFortnightly2FromDate" runat="server" MaxLength="2" onkeypress="return validateNumber(event)" CssClass="form-control"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgF2FD" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <asp:RequiredFieldValidator ID="rivF2FromDate" runat="server"
                                        ControlToValidate="txtFortnightly2FromDate" Display="Dynamic" CssClass="text-danger"
                                        Enabled="false" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter second fortnightly: Internal due date.">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revF2FromDate" runat="server" ControlToValidate="txtFortnightly2FromDate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ceF2FromDate" runat="server" PopupButtonID="imgF2FD"
                                        TargetControlID="txtFortnightly2FromDate" Format="dd"></ajaxToolkit:CalendarExtender>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Second Fortnightly: Regulatory due date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtFortnightly2ToDate" runat="server" MaxLength="2" onkeypress="return validateNumber(event)" CssClass="form-control"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgF2TD" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <asp:RequiredFieldValidator ID="rivF2ToDate" runat="server"
                                        ControlToValidate="txtFortnightly2ToDate" Display="Dynamic" CssClass="text-danger"
                                        Enabled="false" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter second fortnightly: Regulatory due date.">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revF2ToDate" runat="server" ControlToValidate="txtFortnightly2ToDate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ceF2ToDate" runat="server" PopupButtonID="imgF2TD"
                                        TargetControlID="txtFortnightly2ToDate" Format="dd"></ajaxToolkit:CalendarExtender>
                                </div>
                            </div>
                        </div>
                        <div id="MonthlySection" style="display: none; visibility: hidden">
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Monthly: Internal due date </label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtMonthlyFromDate" runat="server" MaxLength="2" onkeypress="return validateNumber(event)" CssClass="form-control"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgMFD" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <%--<a:RequiredIfValidatorRadioButtonList ID="rivMonthlyFrom" runat="server" TriggerIndex="3"
                                        ControlToCompare="rblFrequency" ControlToValidate="txtMonthlyFromDate" Display="Dynamic" CssClass="text-danger"
                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter Monthly: Start Date.">
                                    *</a:RequiredIfValidatorRadioButtonList>--%>

                                    <asp:RequiredFieldValidator ID="rfvMonthlyFrom" runat="server" ControlToValidate="txtMonthlyFromDate" Enabled="false"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                        ErrorMessage="Please enter Monthly: Internal due date."></asp:RequiredFieldValidator>

                                    <asp:RegularExpressionValidator ID="revMFD" runat="server" ControlToValidate="txtMonthlyFromDate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ceFMonthlyFromDate" runat="server" PopupButtonID="imgMFD"
                                        TargetControlID="txtMonthlyFromDate" Format="dd"></ajaxToolkit:CalendarExtender>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Monthly: Regulatory due Date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtMonthlyTodate" runat="server" CssClass="form-control" onkeypress="return validateNumber(event)"
                                            MaxLength="2"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgMTD" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <%--<a:RequiredIfValidatorRadioButtonList ID="rivMonthlyTo" runat="server" TriggerIndex="3"
                                        ControlToCompare="rblFrequency" ControlToValidate="txtMonthlyTodate" Display="Dynamic"
                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter Monthly: End Date." CssClass="text-danger">
                                    *</a:RequiredIfValidatorRadioButtonList>--%>

                                    <asp:RequiredFieldValidator ID="rfvMonthlyTo" runat="server" ControlToValidate="txtMonthlyTodate" Enabled="false"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                        ErrorMessage="Please enter Monthly: Regulatory due date."></asp:RequiredFieldValidator>

                                    <asp:RegularExpressionValidator ID="revMonthlyToDate" runat="server" ControlToValidate="txtMonthlyTodate"
                                        ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$" CssClass="text-danger"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ceMTD" runat="server" PopupButtonID="imgMTD" TargetControlID="txtMonthlyTodate"
                                        Format="dd"></ajaxToolkit:CalendarExtender>
                                </div>
                            </div>
                        </div>
                        <div id="QuarterSection" style="display: none; visibility: hidden">
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">First Quarter: Internal due date </label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtQ1fromDate" runat="server" MaxLength="50" CssClass="form-control"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgFromDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <%--<a:RequiredIfValidatorRadioButtonList ID="rivQ1From" runat="server" TriggerIndex="4"
                                        ControlToCompare="rblFrequency" ControlToValidate="txtQ1fromDate" Display="Dynamic"
                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter First Quarter: Start Date." CssClass="text-danger">
                                    *</a:RequiredIfValidatorRadioButtonList>--%>

                                    <asp:RequiredFieldValidator ID="rfvQ1From" runat="server" ControlToValidate="txtQ1fromDate" Enabled="false"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                        ErrorMessage="Please enter First Quarter: Internal due date."></asp:RequiredFieldValidator>

                                    <asp:RegularExpressionValidator ID="revCircularDate" runat="server" ControlToValidate="txtQ1fromDate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ceDate" runat="server" PopupButtonID="imgFromDate"
                                        TargetControlID="txtQ1fromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">First Quarter: Regulatory due date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtQ1ToDate" CssClass="form-control" runat="server" MaxLength="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgQ1toDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <%--<a:RequiredIfValidatorRadioButtonList ID="rivQ1To" runat="server" TriggerIndex="4"
                                        ControlToCompare="rblFrequency" ControlToValidate="txtQ1ToDate" Display="Dynamic"
                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter First Quarter: End Date." CssClass="text-danger">
                                    *</a:RequiredIfValidatorRadioButtonList>--%>

                                    <asp:RequiredFieldValidator ID="rfvQ1To" runat="server" ControlToValidate="txtQ1ToDate" Enabled="false"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                        ErrorMessage="Please enter First Quarter: Regulatory due date."></asp:RequiredFieldValidator>

                                    <asp:RegularExpressionValidator ID="revQuarterDateto" runat="server" ControlToValidate="txtQ1ToDate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>&nbsp;
                       
                                   

                                    <ajaxToolkit:CalendarExtender ID="ce1" runat="server" PopupButtonID="imgQ1toDate"
                                        TargetControlID="txtQ1ToDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Second Quarter: Internal due date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtQ2FromDate" CssClass="form-control" runat="server"
                                            MaxLength="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgq2FromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <%--<a:RequiredIfValidatorRadioButtonList ID="rivQ2To" runat="server" TriggerIndex="4"
                                        ControlToCompare="rblFrequency" ControlToValidate="txtQ2FromDate" Display="Dynamic"
                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter Second Quarter: Start Date." CssClass="text-danger">
                                    *</a:RequiredIfValidatorRadioButtonList>--%>

                                    <asp:RequiredFieldValidator ID="rfvQ2From" runat="server" ControlToValidate="txtQ2FromDate" Enabled="false"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                        ErrorMessage="Please enter Second Quarter: Internal due date."></asp:RequiredFieldValidator>

                                    <asp:RegularExpressionValidator ID="revQ2to" runat="server" ControlToValidate="txtQ2FromDate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ce2" runat="server" PopupButtonID="imgq2FromDate"
                                        TargetControlID="txtQ2FromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Second Quarter: Regulatory due date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtQ2ToDate" CssClass="form-control" runat="server" MaxLength="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgQ2todate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <%--<a:RequiredIfValidatorRadioButtonList ID="rfvQ2To" runat="server" TriggerIndex="4"
                                        ControlToCompare="rblFrequency" ControlToValidate="txtQ2ToDate" Display="Dynamic"
                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter Second Quarter: End Date." CssClass="text-danger">
                                    *</a:RequiredIfValidatorRadioButtonList>--%>

                                    <asp:RequiredFieldValidator ID="rfvQ2To" runat="server" ControlToValidate="txtQ2ToDate" Enabled="false"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                        ErrorMessage="Please enter Second Quarter: Regulatory due Date."></asp:RequiredFieldValidator>

                                    <asp:RegularExpressionValidator ID="revQ2toDate" runat="server" ControlToValidate="txtQ2ToDate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ce3" runat="server" PopupButtonID="imgQ2todate"
                                        TargetControlID="txtQ2ToDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Third Quarter: Internal due date </label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtQ3FromDate" CssClass="form-control" runat="server"
                                            MaxLength="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgQ3FromDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <%--<a:RequiredIfValidatorRadioButtonList ID="rivQ3From" runat="server" TriggerIndex="4"
                                        ControlToCompare="rblFrequency" ControlToValidate="txtQ3FromDate" Display="Dynamic"
                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter Third Quarter: Start Date." CssClass="text-danger">
                                    *</a:RequiredIfValidatorRadioButtonList>--%>

                                    <asp:RequiredFieldValidator ID="rfvQ3From" runat="server" ControlToValidate="txtQ3FromDate" Enabled="false"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                        ErrorMessage="Please enter Third Quarter: Internal due date."></asp:RequiredFieldValidator>

                                    <asp:RegularExpressionValidator ID="revQ3Date" runat="server" ControlToValidate="txtQ3FromDate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ce4" runat="server" PopupButtonID="imgQ3FromDate"
                                        TargetControlID="txtQ3FromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Third Quarter: Regulatory due date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtQ3Todate" CssClass="form-control" runat="server" MaxLength="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgQ3todate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <%--<a:RequiredIfValidatorRadioButtonList ID="ivQ3To" runat="server" TriggerIndex="4"
                                        ControlToCompare="rblFrequency" ControlToValidate="txtQ3Todate" Display="Dynamic"
                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter Third Quarter: End Date." CssClass="text-danger">
                                    *</a:RequiredIfValidatorRadioButtonList>--%>

                                    <asp:RequiredFieldValidator ID="rfvQ3To" runat="server" ControlToValidate="txtQ3Todate" Enabled="false"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                        ErrorMessage="Please enter Third Quarter: Regulatory due date."></asp:RequiredFieldValidator>

                                    <asp:RegularExpressionValidator ID="revQ3Todate" runat="server" ControlToValidate="txtQ3Todate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ce5" runat="server" PopupButtonID="imgQ3todate"
                                        TargetControlID="txtQ3Todate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Fourth Quarter: Internal due date </label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtQ4FromDate" CssClass="form-control" runat="server"
                                            MaxLength="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgQ4fromdate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />

                                    </div>
                                    <%--<a:RequiredIfValidatorRadioButtonList ID="rivQ4From" runat="server" TriggerIndex="4"
                                        ControlToCompare="rblFrequency" ControlToValidate="txtQ4fFromDate" Display="Dynamic"
                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter Fourth Quarter: Start Date." CssClass="text-danger">
                                    *</a:RequiredIfValidatorRadioButtonList>--%>

                                    <asp:RequiredFieldValidator ID="rfvQ4From" runat="server" ControlToValidate="txtQ4FromDate" Enabled="false"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                        ErrorMessage="Please enter Fourth Quarter: Internal due date."></asp:RequiredFieldValidator>

                                    <asp:RegularExpressionValidator ID="revQ4fromdate" runat="server" ControlToValidate="txtQ4FromDate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ce6" runat="server" PopupButtonID="imgQ4fromdate"
                                        TargetControlID="txtQ4FromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Fourth Quarter: Regulatory due date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtQ4Todate" CssClass="form-control" runat="server" MaxLength="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgQ4ToDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <%--<a:RequiredIfValidatorRadioButtonList ID="rivQ4To" runat="server" TriggerIndex="4"
                                        ControlToCompare="rblFrequency" ControlToValidate="txtQ4Todate" Display="Dynamic"
                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter Fourth Quarter: End Date." CssClass="text-danger">
                                    *</a:RequiredIfValidatorRadioButtonList>--%>

                                    <asp:RequiredFieldValidator ID="rfvQ4To" runat="server" ControlToValidate="txtQ4Todate" Enabled="false"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                        ErrorMessage="Please enter Fourth Quarter: Regulatory due date."></asp:RequiredFieldValidator>

                                    <asp:RegularExpressionValidator ID="revQ4ToDate" runat="server" ControlToValidate="txtQ4Todate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="de7" runat="server" PopupButtonID="imgQ4ToDate"
                                        TargetControlID="txtQ4Todate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                </div>
                            </div>
                        </div>
                        <div id="FirstHafSection" style="display: none; visibility: hidden">
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">First Half: Internal due date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtFirstHalffromDate" CssClass="form-control" runat="server"
                                            MaxLength="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgFHFromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />

                                    </div>

                                    <asp:RequiredFieldValidator ID="rfvFHFrom" runat="server" Enabled="false"
                                        ControlToValidate="txtFirstHalffromDate" Display="Dynamic" CssClass="text-danger"
                                        ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter First Half: Internal due date."></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revFHFDate" runat="server" ControlToValidate="txtFirstHalffromDate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ce8" runat="server" PopupButtonID="imgFHFromDate"
                                        TargetControlID="txtFirstHalffromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">First Half: Regulatory due date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtFirstHalfToDate" CssClass="form-control" runat="server"
                                            MaxLength="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgFHTDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>

                                    <asp:RequiredFieldValidator ID="rivFHTo" runat="server" Enabled="false"
                                        ControlToValidate="txtFirstHalfToDate" Display="Dynamic" CssClass="text-danger"
                                        ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter First Half: Regulatory due date.">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revFHTdate" runat="server" ControlToValidate="txtFirstHalfToDate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgFHTDate"
                                        TargetControlID="txtFirstHalfToDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Second Half: Internal due date </label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtSecondtHalffromDate" CssClass="form-control" runat="server"
                                            MaxLength="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgSHFromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>

                                    <asp:RequiredFieldValidator ID="rivSHFrom" runat="server"
                                        ControlToValidate="txtSecondtHalffromDate" Display="Dynamic" CssClass="text-danger"
                                        Enabled="false" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter Second Half: Internal due date.">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revSHFDate" runat="server" ControlToValidate="txtSecondtHalffromDate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ce9" runat="server" PopupButtonID="imgSHFromDate"
                                        TargetControlID="txtSecondtHalffromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Second Half: Regulatory due date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtSecondtHalffromTo" CssClass="form-control" runat="server"
                                            MaxLength="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgSHTDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>

                                    <asp:RequiredFieldValidator ID="rivSHTo" runat="server"
                                        ControlToValidate="txtSecondtHalffromTo" Display="Dynamic" CssClass="text-danger"
                                        Enabled="false" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter Second Half: Regulatory due date.">
                                    
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revSHTdate" runat="server" ControlToValidate="txtSecondtHalffromTo" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgSHTDate"
                                        TargetControlID="txtSecondtHalffromTo" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                </div>
                            </div>
                        </div>
                        <div id="YearSection" style="display: none; visibility: hidden">
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Yearly: Internal due date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtYearlyfromDate" CssClass="form-control" runat="server"
                                            MaxLength="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgYearlyFromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />

                                    </div>
                                    <asp:RequiredFieldValidator ID="rivYearlyFrom" runat="server"
                                        ControlToValidate="txtYearlyfromDate" Display="Dynamic"
                                        Enabled="false" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter Yearly: Internal due date." CssClass="text-danger">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revYearlyFDate" runat="server" ControlToValidate="txtYearlyfromDate" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgYearlyFromDate"
                                        TargetControlID="txtYearlyfromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Yearly: Regulatory due date</label>
                                    <div class="input-group">

                                        <F2FControls:F2FTextBox ID="txtYearlyDateTo" CssClass="form-control" runat="server"
                                            MaxLength="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgYearlyDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvYearlyTo" runat="server"
                                        ControlToValidate="txtYearlyDateTo" Display="Dynamic"
                                        Enabled="false" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter Yearly: Regulatory due date." CssClass="text-danger">
                                    
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revYearlydate" runat="server" ControlToValidate="txtYearlyDateTo" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" PopupButtonID="imgYearlyDate"
                                        TargetControlID="txtYearlyDateTo" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                </div>
                            </div>
                        </div>

                        <div class="mt-3">
                            <div class="card mb-1 mt-1 border">
                                <div class="card-header py-0 custom-ch-bg-color">
                                    <h6 class="font-weight-bold text-white mtb-5">Relevant Supporting </h6>
                                </div>
                                <div class="card-body mt-1">
                                    <div class="row">
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">File Type:<span class="text-danger">*</span></label>
                                            <asp:DropDownList CssClass="form-select" AppendDataBoundItems="true" ID="ddlFileType"
                                                runat="server" DataTextField="RC_NAME" DataValueField="RC_CODE">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvddlFileType" runat="server" ControlToValidate="ddlFileType"
                                                ValidationGroup="Upload" CssClass="text-danger" Display="Dynamic" SetFocusOnError="True" ErrorMessage="Please select File Type.">Please select file type.</asp:RequiredFieldValidator>

                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">File Description:</label>
                                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtFileDesc" runat="server" Rows="3" Columns="50"
                                                TextMode="MultiLine"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtFileDesc" />
                                            <asp:RegularExpressionValidator ID="revtxtFileDescs" ControlToValidate="txtFileDesc"
                                                ErrorMessage="File description cannot exceed 1000 characters." ValidationExpression="^[\s\S]{0,1000}$"
                                                runat="server" ValidationGroup="SaveSubmissionDetails" />
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">
                                                <asp:Label ID="lblAttachments" runat="server" Text="Browse File:"></asp:Label><span class="text-danger">*</span></label>
                                            <div class="input-group">
                                                <asp:FileUpload ID="fuEditFileUpload" CssClass="form-control" runat="server" />
                                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnAddAttachment" runat="server" Text="Attach"
                                                    OnClick="btnAddAttachment_Click" ValidationGroup="Upload">
                                                <i class="fa fa-upload"></i> Attach
                                                </asp:LinkButton>
                                            </div>
                                            <asp:HiddenField ID="hfFileNameOnServer" runat="server" />
                                            <asp:RegularExpressionValidator ID="revFileUpload1" runat="server" ControlToValidate="fuEditFileUpload"
                                                Display="Dynamic" ErrorMessage="Upload of this file format is not supported."
                                                ValidationGroup="Upload"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Uploaded Files:</label>
                                            <div class="table-responsive">
                                                <asp:GridView ID="gvInsertFileUpload" runat="server" AllowPaging="false" AllowSorting="false"
                                                    BorderStyle="None" BorderWidth="1px" DataKeyNames="FileNameOnServer" AutoGenerateColumns="false"
                                                    OnSelectedIndexChanged="gvInsertFileUpload_SelectedIndexChanged"
                                                    CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                                    <Columns>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbDelete" runat="server" CausesValidation="False" CommandName="Select" CssClass="btn btn-sm btn-soft-danger btn-circle"
                                                                    OnClientClick="return onClientDeleteClick();">
                                                                    <i class="fa fa-trash"></i></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="File Name">
                                                            <ItemTemplate>
                                                                <a href="Javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=SubmissionFiles&downloadFileName=<%# Eval("FileNameOnServer")%>&fileName=<%#getFileName(Eval("FileName"))%>','','location=0,status=0,scrollbars=0,width=10,height=10');">
                                                                    <%#Eval("FileName")%></a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="File Type" DataField="File Type" />
                                                        <asp:BoundField HeaderText="File Description" DataField="File Description" />
                                                        <asp:BoundField HeaderText="File Name on Server" DataField="FileNameOnServer" />
                                                        <asp:BoundField HeaderText="Uploaded By" DataField="Uploaded By" />
                                                        <asp:BoundField HeaderText="Uploaded On" DataField="Uploaded On" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <div class="col-md-12 mb-3" id="divAlreadyUploaded" runat="server" visible="false">
                                            <label class="form-label">Uploaded Files:</label>
                                            <div class="table-responsive">
                                                <asp:GridView ID="gvAlreadyUploaded" runat="server" AllowPaging="false" AllowSorting="false"
                                                    BorderStyle="None" BorderWidth="1px" DataKeyNames="FileNameOnServer" AutoGenerateColumns="false"
                                                    OnSelectedIndexChanged="gvAlreadyUploaded_SelectedIndexChanged"
                                                    CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                                    <Columns>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbDelete" runat="server" CausesValidation="False" CommandName="Select" CssClass="btn btn-sm btn-soft-danger btn-circle"
                                                                    OnClientClick="return onClientDeleteClick();">
                            <i class="fa fa-trash"></i></asp:LinkButton>
                                                                <asp:HiddenField ID="hfSMF_ID" runat="server" Value='<%#Eval("SMF_ID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="File Name">
                                                            <ItemTemplate>
                                                                <a href="Javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=SubmissionFiles&downloadFileName=<%# Eval("FileNameOnServer")%>&fileName=<%#getFileName(Eval("FileName"))%>','','location=0,status=0,scrollbars=0,width=10,height=10');">
                                                                    <%#Eval("FileName")%></a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="File Type" DataField="File Type" />
                                                        <asp:BoundField HeaderText="File Description" DataField="File Description" />
                                                        <asp:BoundField HeaderText="File Name on Server" DataField="FileNameOnServer" />
                                                        <asp:BoundField HeaderText="Uploaded By" DataField="Uploaded By" />
                                                        <asp:BoundField HeaderText="Uploaded On" DataField="Uploaded On" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mt-3" style="display: none">
                            <div class="card mb-1 mt-1 border">
                                <div class="card-header py-0 custom-ch-bg-color">
                                    <h6 class="font-weight-bold text-white mtb-5">Process </h6>
                                </div>
                                <div class="card-body mt-1">
                                    <div class="row">
                                        <asp:ValidationSummary ID="vsSubmissionDetails" runat="server" ShowMessageBox="True"
                                            ShowSummary="false" ValidationGroup="SaveSubmissionDetails" DisplayMode="BulletList" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mt-3">
                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSave" runat="server" Text="Save"
                                OnClick="btnSave_Click" OnClientClick="return onClientSaveClick()">
                                <i class="fa fa-save me-2"></i> Submit                    
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnCancel" runat="server" CausesValidation="false"
                                Text="Cancel" OnClick="btnCancel_Click">
                                <i class="fa fa-arrow-left me-2"></i> Cancel                   
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->
    <script>
        function CompareNotificationDateSytemDate(src, arg) {
            //if ($('[id*="ddlStatus"]').val() == 'P' && arg.Value != '') {
            if (arg.Value != '') {
                if (Date.parse($("#ctl00_ContentPlaceHolder1_txtCircularDate").val()) > Date.parse($("#ctl00_ContentPlaceHolder1_hfCurDate").val())) {
                    arg.IsValid = false;
                }
                else {
                    arg.IsValid = true;
                }
            } else {
                arg.IsValid = true;
            }
        }
        function CompareFilingEffectiveDateSytemDate(src, arg) {
            //if ($('[id*="ddlStatus"]').val() == 'P' && arg.Value != '') {
            if (arg.Value != '') {
                if (Date.parse($("#ctl00_ContentPlaceHolder1_txtCircularDate").val()) > Date.parse($("#ctl00_ContentPlaceHolder1_txtEffectiveDate").val())) {
                    arg.IsValid = false;
                }
                else {
                    arg.IsValid = true;
                }
            } else {
                arg.IsValid = true;
            }
        }
    </script>
</asp:Content>
