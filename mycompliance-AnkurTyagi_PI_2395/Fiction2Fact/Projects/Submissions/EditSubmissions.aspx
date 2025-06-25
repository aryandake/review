<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" Async="true"
    Inherits="Fiction2Fact.Projects.Submissions.EditSubmissions" Title="Edit Submissions Details" CodeBehind="EditSubmissions.aspx.cs" %>

<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="custom" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="a" Namespace="Fiction2Fact.Controls" Assembly="RequiredIfValidatorRadioButtonList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        function showhideTypeBased(rbltypeName) {
            var rb = document.getElementById(rbltypeName + '_0');
            var elem = document.getElementById('FixedDateBaseSection');
            var elem1 = document.getElementById('EventBasedSection');
            var hf = document.getElementById('hfFixedOrEvent');

            if (rb.checked) {
                hf.value = 'F';
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
                elem1.style.display = 'none';
                elem1.style.visibility = 'hidden';
            }
            else {
                hf.value = 'E';
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
                elem1.style.display = 'block';
                elem1.style.visibility = 'visible';
            }
        }

        function showhideOtherFrequencyPanels(rblName) {
            var rb = document.getElementById(rblName + '_0');
            var flag = 0;
            var elem = document.getElementById('OnlyOnceSection');
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
            elem = document.getElementById('MonthlySection');
            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }

            rb = document.getElementById(rblName + '_4');
            elem = document.getElementById('QuarterSection');
            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }

            rb = document.getElementById(rblName + '_5');
            elem = document.getElementById('FirstHalfSection');
            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }

            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }

            rb = document.getElementById(rblName + '_6');
            elem = document.getElementById('YearSection');
            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }

            rb = document.getElementById(rblName + '_7');
            elem = document.getElementById('FortnightlySection');
            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }
        }
    </script>

    <asp:HiddenField ID="hfOpType" runat="server" />
    <asp:HiddenField ID="hfSubId" runat="server" />
    <input id="hfFixedOrEvent" type="hidden" />
    <asp:HiddenField ID="hfType" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Edit Submissions Details</h4>
                        <asp:Label ID="lblInfo" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
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
                        <asp:Button ID="btnBackToSearch" runat="server" Visible="false" CssClass="btn btn-outline-primary"
                            Text="Ok" />
                    </div>
                    <asp:Panel ID="pnlInactive" Visible="false" runat="server">
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label">Effective Date:</label>
                                <div class="input-group">
                                    <F2FControls:F2FTextBox ID="txtEffectiveDt" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                                    <asp:ImageButton ID="ImageButton2" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                </div>
                                <asp:RegularExpressionValidator ID="revDt" runat="server" ControlToValidate="txtEffectiveDt"
                                    ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender7" runat="server" PopupButtonID="ImageButton2"
                                    TargetControlID="txtEffectiveDt" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvDt" runat="server" ControlToValidate="txtEffectiveDt" CssClass="text-danger" ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please enter New Effective Date.">Please enter New Effective Date.</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 mb-3">
                                <label class="form-label">Rationale for deactivation:</label>
                                <div class="input-group">
                                    <F2FControls:F2FTextBox ID="txtDeActivationRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                        Rows="3" MaxLength="2000"></F2FControls:F2FTextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvDeActivationRemarks" runat="server" ControlToValidate="txtDeActivationRemarks" CssClass="text-danger" ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please enter New Effective Date.">Please enter Rationale for deactivation.</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label">&nbsp;</label>
                                <div>
                                    <asp:LinkButton CssClass="btn btn-outline-success" ID="btnInactive" runat="server" Text="Proceed"
                                        ValidationGroup="SaveSubmissionDetails" OnClick="btnInactive_Click">
                                        <i class="fa fa-forward me-2"></i> Proceed                    
                                    </asp:LinkButton>
                                    <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnCancel1" runat="server" CausesValidation="false"
                                        Text="Cancel" OnClick="btnCancel1_Click">
                                        <i class="fa fa-arrow-left me-2"></i> Cancel                   
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlDetails" Visible="false" runat="server">
                        <%--<center>
                            <div class="ContentHeader1">
                                Edit Submissions Details
                            </div>
                        </center>--%>
                        <asp:FormView ID="fvDetails" runat="server" DefaultMode="Edit" AllowPaging="True"
                            DataKeyNames="SM_ID" Width="100%">
                            <EditItemTemplate>
                                <div class="row">
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Effective Date:</label>
                                        <div class="input-group">
                                            <F2FControls:F2FTextBox ID="txtEffectDt" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                                            <asp:ImageButton ID="ImageButton2" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                ImageUrl="../../Content/images/legacy/calendar.jpg" />&nbsp;
                          
                                        </div>
                                        <asp:RegularExpressionValidator ID="revDt" runat="server" ControlToValidate="txtEffectDt"
                                            ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" CssClass="text-danger"
                                            ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender7" runat="server" PopupButtonID="ImageButton2"
                                            TargetControlID="txtEffectDt" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvDt" runat="server" ControlToValidate="txtEffectDt" CssClass="text-danger" ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                            ErrorMessage="Please enter New Effective Date.">Please enter New Effective Date.</asp:RequiredFieldValidator>

                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Tracked By:</label>
                                        <asp:DropDownList ID="ddlEditSubType" CssClass="form-select" AppendDataBoundItems="true"
                                            runat="server" DataValueField="STM_ID" DataTextField="STM_TYPE" AutoPostBack="false">
                                            <%--OnSelectedIndexChanged="ddlEditSubType_SelectedIndexChanged"--%>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvSubType" runat="server" ControlToValidate="ddlEditSubType" CssClass="text-danger" ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                            ErrorMessage="Please select Submission Type.">*</asp:RequiredFieldValidator>
                                    </div>
                                    <%--<div class="col-md-12 mb-3">
                                        <label class="form-label">Owners:</label>
                                        <div class="custom-checkbox-table">
                                            <asp:CheckBoxList ID="cbOwners" RepeatColumns="5" CssClass="form-control" AppendDataBoundItems="true" runat="server"
                                                DataValueField="EM_ID" DataTextField="EM_EMPNAME">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>--%>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Reporting Department:</label>
                                        <asp:DropDownList ID="ddlEditReportDept" CssClass="form-select" AppendDataBoundItems="true"
                                            runat="server" DataValueField="SRD_ID" DataTextField="SRD_NAME" AutoPostBack="false">
                                            <%--OnSelectedIndexChanged="ddlEditReportDept_SelectedIndexChanged"--%>
                                        </asp:DropDownList>
                                    </div>
                                    <%--<div class="col-md-4 mb-3">
                                        <label class="form-label">Reporting Department Owners:</label>
                                        <div class="custom-checkbox-table">
                                            <asp:CheckBoxList ID="cbDeptOwner" RepeatColumns="5" CssClass="form-control" AppendDataBoundItems="true"
                                                runat="server" DataValueField="SRDOM_ID" DataTextField="SRDOM_EMP_NAME">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>--%>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Priority:</label>
                                        <asp:DropDownList ID="ddlPriority" CssClass="form-select" runat="server"
                                            SelectedValue='<%#Eval("SM_PRIORITY") %>'>
                                            <asp:ListItem Value="">Select One</asp:ListItem>
                                            <asp:ListItem Value="H">High</asp:ListItem>
                                            <asp:ListItem Value="M">Medium</asp:ListItem>
                                            <asp:ListItem Value="L">Low</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvPriority" runat="server" ControlToValidate="ddlPriority" CssClass="text-danger" ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                            ErrorMessage="Please select Submission priority.">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Particulars:</label>
                                        <F2FControls:F2FTextBox ID="txtPerticulars" CssClass="form-control" TextMode="MultiLine"
                                            runat="server" Columns="50" MaxLength="200" Text='<%# Eval("SM_PERTICULARS")%>'></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtPerticulars" />
                                        <asp:RequiredFieldValidator ID="rfvPerticulars" runat="server" ControlToValidate="txtPerticulars" CssClass="text-danger"
                                            ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                            ErrorMessage="Please enter Perticulars.">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revPerticulars" ControlToValidate="txtPerticulars"
                                            Display="Dynamic" Text="Exceeding 500 characters" ValidationExpression="^[\s\S]{0,500}$" CssClass="text-danger"
                                            runat="server" SetFocusOnError="True" ValidationGroup="SaveSubmissionDetails" />

                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Brief Description:</label>
                                        <F2FControls:F2FTextBox ID="txtDescription" CssClass="form-control" TextMode="MultiLine"
                                            Columns="70" Rows="3" MaxLength="200" runat="server" Text='<%# Eval("SM_BRIEF_DESCRIPTION")%>'>
                                        </F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtDescription" />
                                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" CssClass="text-danger"
                                            ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                            ErrorMessage="Please enter Description.">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revDescription" ControlToValidate="txtDescription"
                                            Display="Dynamic" Text="Exceeding 500 characters" ValidationExpression="^[\s\S]{0,500}$" CssClass="text-danger"
                                            runat="server" SetFocusOnError="True" ValidationGroup="SaveSubmissionDetails" />
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">To be Escalated:</label>
                                        <asp:RadioButtonList ID="rblEscalate" runat="server" RepeatColumns="2" SelectedValue='<%#Eval("SM_TO_BE_ESC") %>'>
                                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                                            <asp:ListItem Value="N">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RequiredFieldValidator ID="rfvEscalate" runat="server" ControlToValidate="rblEscalate" CssClass="text-danger" ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                            ErrorMessage="Please select any One.">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Level 1 Escalation Days:</label>
                                        <F2FControls:F2FTextBox ID="txtlevel1" CssClass="form-control" runat="server" MaxLength="4"
                                            Columns="4" Text='<%# Eval("SM_L1_ESCALATION_DAYS")%>'></F2FControls:F2FTextBox>
                                        <asp:RequiredFieldValidator ID="rfvL1EscDays" runat="server" ControlToValidate="txtlevel1"
                                            ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger" ErrorMessage="Please enter Level 1 Days.">*</asp:RequiredFieldValidator>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="ftelevel1" runat="server" TargetControlID="txtlevel1" FilterType="Numbers"></ajaxToolkit:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Level 2 Escalation Days:</label>
                                        <F2FControls:F2FTextBox ID="txtlevel2" CssClass="form-control" runat="server" MaxLength="4"
                                            Columns="4" Text='<%# Eval("SM_L2_ESCALATION_DAYS")%>'></F2FControls:F2FTextBox>
                                        <asp:RequiredFieldValidator ID="rfvL2EscDays" runat="server" ControlToValidate="txtlevel2" CssClass="text-danger" ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                            ErrorMessage="Please enter Level 2 Days.">*</asp:RequiredFieldValidator>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="fteleve2" runat="server" TargetControlID="txtlevel2" FilterType="Numbers"></ajaxToolkit:FilteredTextBoxExtender>

                                    </div>
                                </div>
                            </EditItemTemplate>
                        </asp:FormView>
                        <div class="mt-3">
                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnUpdateDets" runat="server" Text="Update Details"
                                ValidationGroup="SaveSubmissionDetails" OnClick="btnUpdateDets_Click">
                                <i class="fa fa-save me-2"></i> Update Details                    
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="Button2" runat="server" CausesValidation="false"
                                Text="Cancel" OnClick="btnCancel2_Click">
                                <i class="fa fa-arrow-left me-2"></i> Cancel                    
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlDuedates" Visible="false" runat="server">
                        <%--<center>
                            <div class="ContentHeader1">
                                Edit Submissions Due Dates
                            </div>
                        </center>--%>
                        <asp:FormView ID="fvSubmissionMaster" runat="server" DefaultMode="Edit" AllowPaging="True"
                            DataKeyNames="SM_ID" Width="100%">
                            <EditItemTemplate>
                                <div class="row">
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Submission Type:</label>
                                        <asp:DropDownList ID="ddlSubType" Enabled="false" AppendDataBoundItems="true" runat="server"
                                            DataValueField="STM_ID" DataTextField="STM_TYPE" CssClass="form-select">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-8 mb-3">
                                        <label class="form-label">Owners:</label>
                                        <div class="custom-checkbox-table">
                                            <asp:CheckBoxList ID="cbOwners" Enabled="false" RepeatColumns="5" CssClass="form-control" AppendDataBoundItems="true"
                                                runat="server" DataValueField="EM_ID" DataTextField="EM_EMPNAME">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Effective Date:</label>
                                        <F2FControls:F2FTextBox ID="txtEffectiveDate" Enabled="false" Text='<%# Eval("SM_EFFECTIVE_DT" , "{0:dd-MMM-yyyy}")%>'
                                            runat="server" Columns="15" MaxLength="20" CssClass="form-control"></F2FControls:F2FTextBox>
                                    </div>
                                    <div class="col-md-8 mb-3">
                                        <label class="form-label">Reporting to:</label>
                                        <div class="custom-checkbox-table">
                                            <asp:CheckBoxList ID="cblSegment" Enabled="false" runat="server" CssClass="form-control" RepeatColumns="7"
                                                DataTextField="SSM_NAME" DataValueField="SSM_ID">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Reporting Function:</label>
                                        <asp:DropDownList ID="ddlReportDept" Enabled="false" CssClass="form-select"
                                            AppendDataBoundItems="true" runat="server" DataValueField="SRD_ID" DataTextField="SRD_NAME"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="0">(Select)</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Reporting Function Owners:</label>
                                        <div class="custom-checkbox-table">
                                            <asp:CheckBoxList ID="cbDeptOwner" Enabled="false" RepeatColumns="5" CssClass="form-control" AppendDataBoundItems="true"
                                                runat="server" DataValueField="SRDOM_ID" DataTextField="SRDOM_EMP_NAME">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Priority:</label>
                                        <asp:DropDownList ID="ddlPriority" Enabled="false" runat="server" CssClass="form-select">
                                            <asp:ListItem Value="">Select One</asp:ListItem>
                                            <asp:ListItem Value="H">High</asp:ListItem>
                                            <asp:ListItem Value="M">Medium</asp:ListItem>
                                            <asp:ListItem Value="L">Low</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Particulars:</label>
                                        <F2FControls:F2FTextBox ID="txtPerticulars" CssClass="form-control" TextMode="MultiLine"
                                            Enabled="false" runat="server" Columns="50" MaxLength="200" Text='<%# Eval("SM_PERTICULARS")%>'></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtPerticulars" />
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Brief Description:</label>
                                        <F2FControls:F2FTextBox ID="txtDescription" CssClass="form-control" TextMode="MultiLine"
                                            Enabled="false" Columns="70" Rows="3" MaxLength="200" runat="server" Text='<%# Eval("SM_BRIEF_DESCRIPTION")%>'>
                                        </F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtDescription" />
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">To be Escalated:</label>
                                        <div class="custom-checkbox-table">
                                            <asp:RadioButtonList ID="rblEscalate" runat="server" CssClass="form-control" RepeatColumns="2" Enabled="false"
                                                SelectedValue='<%#Eval("SM_TO_BE_ESC") %>'>
                                                <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                <asp:ListItem Value="N">No</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Level 1 Escalation Days:</label>
                                        <F2FControls:F2FTextBox ID="txtlevel1" runat="server" CssClass="form-control" MaxLength="4"
                                            Enabled="false" Columns="4" Text='<%# Eval("SM_L1_ESCALATION_DAYS")%>'></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredNumber runat="server" TargetControlID="txtlevel1" />
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Level 2 Escalation Days:</label>
                                        <F2FControls:F2FTextBox ID="txtlevel2" runat="server" CssClass="form-control" MaxLength="4"
                                            Columns="4" Enabled="false" Text='<%# Eval("SM_L2_ESCALATION_DAYS")%>'></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredNumber runat="server" TargetControlID="txtlevel2" />
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Type:</label>
                                        <asp:RadioButtonList ID="rblType" Enabled="false" runat="server" RepeatColumns="7"
                                            SelectedValue='<%#Eval("SM_SUB_TYPE") %>'>
                                            <asp:ListItem Value="F">Fixed Date</asp:ListItem>
                                            <asp:ListItem Value="E">Event Based</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div id="EventBasedSection" style="display: none; visibility: hidden">
                                    <div class="row">
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Depends On Event:</label>
                                            <asp:Label ID="lblEvent" runat="server" Text='<%# Eval("SM_EM_ID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lbAgenda" runat="server" Text='<%# Eval("SM_EP_ID") %>' Visible="false"></asp:Label>
                                            <asp:DropDownList ID="ddlEvent" AppendDataBoundItems="true" runat="server" OnSelectedIndexChanged="ddlEvent_SelectedIndexChanged"
                                                AutoPostBack="true" DataValueField="EM_ID" DataTextField="EM_EVENT_NAME" OnDataBound="ddlEvent_DataBound"
                                                CssClass="form-select">
                                                <asp:ListItem Value="">All</asp:ListItem>
                                            </asp:DropDownList>
                                            <a:RequiredIfValidatorRadioButtonList ID="RequiredIfValidatorRadioButtonList5" runat="server"
                                                TriggerIndex="1" ControlToCompare="rblType" ControlToValidate="ddlEvent" Display="Dynamic" CssClass="text-danger"
                                                EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Select Event Type">
                                                    *
                                            </a:RequiredIfValidatorRadioButtonList>
                                        </div>
                                        <div class="col-md-8 mb-3">
                                            <label class="form-label">Agenda:</label>
                                            <div class="custom-checkbox-table">
                                                <asp:RadioButtonList ID="rblAssociatedWith" runat="server" CssClass="form-control" RepeatColumns="7" AppendDataBoundItems="true"
                                                    DataTextField="EP_NAME" DataValueField="EP_ID">
                                                </asp:RadioButtonList>
                                                <a:RequiredIfValidatorRadioButtonList ID="RequiredIfValidatorRadioButtonList3" runat="server"
                                                    TriggerIndex="1" ControlToCompare="rblType" ControlToValidate="rblAssociatedWith"
                                                    Display="Dynamic" EnableClientScript="true" ValidationGroup="SaveSubmissionDetails"
                                                    ErrorMessage="Please Select Agenda Type" CssClass="text-danger">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RequiredFieldValidator ID="rfvAgenda" runat="server" ControlToValidate="rblAssociatedWith" CssClass="text-danger"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                                    ErrorMessage="Please select Agenda.">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Start Date (No of Days before / after the event):</label>
                                            <F2FControls:F2FTextBox ID="txtStartDays" Text='<%# Eval("SM_START_NO_OF_DAYS")%>' runat="server"
                                                MaxLength="4" Columns="4" CssClass="form-control"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredNumber runat="server" TargetControlID="txtStartDays" FilterType="Numbers, Custom" ValidChars="-" />
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">End Date (No of Days before / after the event):</label>
                                            <F2FControls:F2FTextBox ID="txtEndDays" Text='<%# Eval("SM_END_NO_OF_DAYS")%>' runat="server"
                                                MaxLength="4" Columns="4" CssClass="form-control"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredNumber runat="server" TargetControlID="txtEndDays" FilterType="Numbers, Custom" ValidChars="-" />
                                        </div>
                                    </div>
                                </div>
                                <div id="FixedDateBaseSection" style="display: none; visibility: hidden">
                                    <div class="row">
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Frequency:</label>
                                            <asp:Label ID="lbFreq" runat="server" Text='<%# Eval("sm_frequency") %>' Visible="false"></asp:Label>
                                            <div class="custom-checkbox-table">
                                                <asp:RadioButtonList Enabled="false" ID="rblFrequency" runat="server" CssClass="form-control" RepeatColumns="8">
                                                    <asp:ListItem Value="Only Once">Adhoc</asp:ListItem>
                                                    <asp:ListItem Text="Daily" Value="Daily">Daily</asp:ListItem>
                                                    <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                                                    <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                                    <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>
                                                    <asp:ListItem Value="Half Yearly">Half Yearly</asp:ListItem>
                                                    <asp:ListItem Value="Yearly">Yearly</asp:ListItem>
                                                    <asp:ListItem Value="Fortnightly">Fortnightly</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                            <a:RequiredIfValidatorRadioButtonList ID="RequiredIfValidatorRadioButtonList2" runat="server" CssClass="text-danger"
                                                TriggerIndex="0" ControlToCompare="rblType" ControlToValidate="rblFrequency"
                                                Display="Dynamic" EnableClientScript="true" ValidationGroup="SaveSubmissionDetails"
                                                ErrorMessage="Please Select Frequency">
                                                *
                                            </a:RequiredIfValidatorRadioButtonList>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Effective Date:</label>
                                            <div class="input-group">
                                                <F2FControls:F2FTextBox ID="txtEffectiveDt" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                                                <asp:ImageButton ID="ImageButton2" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                            </div>
                                            <asp:RegularExpressionValidator ID="revDt" runat="server" ControlToValidate="txtEffectiveDt"
                                                ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" CssClass="text-danger"
                                                ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender7" runat="server" PopupButtonID="ImageButton2"
                                                TargetControlID="txtEffectiveDt" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                            <a:RequiredIfValidatorRadioButtonList ID="rivForEffDt" runat="server" TriggerIndex="0"
                                                ControlToCompare="rblType" ControlToValidate="txtEffectiveDt" Display="Dynamic"
                                                EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" CssClass="text-danger" ErrorMessage="Please enter New Effective Date.">Please enter New Effective Date.
                                    
                                            </a:RequiredIfValidatorRadioButtonList>
                                        </div>
                                    </div>
                                    <div id="OnlyOnceSection" style="display: none; visibility: hidden">
                                        <div class="row">
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Adhoc From date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtOnceFromDate" runat="server" CssClass="form-control"
                                                        MaxLength="200" Text='<%# Eval("SM_ONLY_ONCE_FROM_DATE", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgMFD" runat="server" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />&nbsp;
                                  
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivMontthlyFrom" runat="server" TriggerIndex="0" CssClass="text-danger"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtOnceFromDate" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
                                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revMFD" runat="server" ControlToValidate="txtOnceFromDate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="ceFMonthlyFromDate" runat="server" PopupButtonID="imgMFD"
                                                    TargetControlID="txtOnceFromDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>

                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Adhoc To date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtOnceToDate" runat="server" MaxLength="200" CssClass="form-control"
                                                        Text='<%# Eval("SM_ONLY_ONCE_TO_DATE", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgOnceTODate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />&nbsp;
                                               
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rifOnceToDate" runat="server" TriggerIndex="0" CssClass="text-danger"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtOnceToDate" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter date">
                                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revOnceDateTo" runat="server" ControlToValidate="txtOnceToDate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="ceOnceTodate" runat="server" PopupButtonID="imgOnceTODate"
                                                    TargetControlID="txtOnceToDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>

                                            </div>
                                        </div>
                                    </div>
                                    <div id="WeeklySection" style="display: none; visibility: hidden">
                                        <div class="row">
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Weekly From Day:</label>
                                                <asp:DropDownList ID="ddlFromWeekDays" runat="server" CssClass="form-select"
                                                    Enabled="false" SelectedValue='<%# Eval("SM_WEEKLY_DUE_DATE_FROM")%>'>
                                                    <asp:ListItem Value="">(None)</asp:ListItem>
                                                    <asp:ListItem Value="Monday">Monday</asp:ListItem>
                                                    <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                                                    <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                                                    <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                                                    <asp:ListItem Value="Friday">Friday</asp:ListItem>
                                                    <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                                                    <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                                                </asp:DropDownList>
                                                <a:RequiredIfValidatorRadioButtonList ID="rifvFromWeekDays" runat="server" TriggerIndex="2" CssClass="text-danger"
                                                    ControlToCompare="rblFrequency" ControlToValidate="ddlFromWeekDays" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please select Weekly From day">
                                                        *
                                                </a:RequiredIfValidatorRadioButtonList>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Weekly To Day:</label>
                                                <asp:DropDownList ID="ddlTOWeekDays" runat="server" Enabled="false" CssClass="form-select"
                                                    SelectedValue='<%# Eval("SM_WEEKLY_DUE_DATE_TO")%>'>
                                                    <asp:ListItem Value="">(None)</asp:ListItem>
                                                    <asp:ListItem Value="Monday">Monday</asp:ListItem>
                                                    <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                                                    <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                                                    <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                                                    <asp:ListItem Value="Friday">Friday</asp:ListItem>
                                                    <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                                                    <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                                                </asp:DropDownList>
                                                <a:RequiredIfValidatorRadioButtonList ID="rifvTOWeekDays" runat="server" TriggerIndex="2" CssClass="text-danger"
                                                    ControlToCompare="rblFrequency" ControlToValidate="ddlTOWeekDays" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please select Weekly To day">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="MonthlySection" style="display: none; visibility: hidden">
                                        <div class="row">
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Monthly From Date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtMonthlyFromDate" runat="server" CssClass="form-control"
                                                        MaxLength="200" Text='<%# Eval("SM_MONTHLY_DUE_DATE_FROM", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="RequiredIfValidatorRadioButtonList1" runat="server" CssClass="text-danger"
                                                    TriggerIndex="3" ControlToCompare="rblFrequency" ControlToValidate="txtMonthlyFromDate"
                                                    Display="Dynamic" EnableClientScript="true" ValidationGroup="SaveSubmissionDetails"
                                                    ErrorMessage="Please Enter Date">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtMonthlyFromDate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgMFD"
                                                    TargetControlID="txtMonthlyFromDate" Format="dd"></ajaxToolkit:CalendarExtender>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Monthly To Date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtMonthlyTodate" runat="server" CssClass="form-control"
                                                        MaxLength="200" Text=' <%# Eval("SM_MONTHLY_DUE_DATE_TO", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgMTD" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivMonthlyTo" runat="server" TriggerIndex="3" CssClass="text-danger"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtMonthlyTodate" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
                                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revMonthlyToDate" runat="server" ControlToValidate="txtMonthlyTodate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="ceMTD" runat="server" PopupButtonID="imgMTD" TargetControlID="txtMonthlyTodate"
                                                    Format="dd"></ajaxToolkit:CalendarExtender>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="QuarterSection" style="display: none; visibility: hidden">
                                        <div class="row">
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Q1 From Date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtQ1fromDate" runat="server" MaxLength="200" CssClass="form-control"
                                                        Text='<%# Eval("SM_Q1_DUE_DATE_FROM", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgFromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivQ1From" runat="server" TriggerIndex="4"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtQ1fromDate" Display="Dynamic" CssClass="text-danger"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revCircularDate" runat="server" ControlToValidate="txtQ1fromDate"
                                                    ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="ceDate" runat="server" PopupButtonID="imgFromDate"
                                                    TargetControlID="txtQ1fromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Q1 To Date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtQ1ToDate" runat="server" MaxLength="200" CssClass="form-control"
                                                        Text='<%# Eval("SM_Q1_DUE_DATE_TO", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgQ1toDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />

                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivQ1To" runat="server" TriggerIndex="4"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtQ1ToDate" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date" CssClass="text-danger">
                                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revQuarterDateto" runat="server" ControlToValidate="txtQ1ToDate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>&nbsp;
                                   
                                    <ajaxToolkit:CalendarExtender ID="ce1" runat="server" PopupButtonID="imgQ1toDate"
                                        TargetControlID="txtQ1ToDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Q2 From Date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtQ2FromDate" runat="server" MaxLength="200" CssClass="form-control"
                                                        Text='<%# Eval("SM_Q2_DUE_DATE_FROM", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgq2FromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivQ2T0" runat="server" TriggerIndex="4"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtQ2FromDate" Display="Dynamic" CssClass="text-danger"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtQ2FromDate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="ce2" runat="server" PopupButtonID="imgq2FromDate"
                                                    TargetControlID="txtQ2FromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Q2 To Date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtQ2ToDate" runat="server" MaxLength="200" CssClass="form-control"
                                                        Text='<%# Eval("SM_Q2_DUE_DATE_TO", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgQ2todate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rfvQ2To" runat="server" TriggerIndex="4"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtQ2ToDate" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date" CssClass="text-danger">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revQ2toDate" runat="server" ControlToValidate="txtQ2ToDate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="ce3" runat="server" PopupButtonID="imgQ2todate"
                                                    TargetControlID="txtQ2ToDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Q3 From Date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtQ3FromDate" runat="server" MaxLength="200" CssClass="form-control"
                                                        Text='<%# Eval("SM_Q3_DUE_DATE_FROM", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgQ3FromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />

                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivQ3From" runat="server" TriggerIndex="4"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtQ3FromDate" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revQ3Date" runat="server" ControlToValidate="txtQ3FromDate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="ce4" runat="server" PopupButtonID="imgQ3FromDate"
                                                    TargetControlID="txtQ3FromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Q3 To Date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtQ3Todate" runat="server" MaxLength="200" CssClass="form-control"
                                                        Text='<%# Eval("SM_Q3_DUE_DATE_TO", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgQ3todate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="ivQ3To" runat="server" TriggerIndex="4"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtQ3Todate" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date" CssClass="text-danger">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revQ3Todate" runat="server" ControlToValidate="txtQ3Todate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="ce5" runat="server" PopupButtonID="imgQ3todate"
                                                    TargetControlID="txtQ3Todate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Q4 From Date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtQ4fFromDate" runat="server" MaxLength="200" CssClass="form-control"
                                                        Text=' <%# Eval("SM_Q4_DUE_DATE_FROM", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgQ4fromdate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivQ4From" runat="server" TriggerIndex="4"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtQ4fFromDate" Display="Dynamic" CssClass="text-danger"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
                                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revQ4fromdate" runat="server" ControlToValidate="txtQ4fFromDate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="ce6" runat="server" PopupButtonID="imgQ4fromdate"
                                                    TargetControlID="txtQ4fFromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Q4 To Date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtQ4Todate" runat="server" MaxLength="200" CssClass="form-control"
                                                        Text='<%# Eval("SM_Q4_DUE_DATE_TO", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgQ4ToDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivQ4To" runat="server" TriggerIndex="4"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtQ4Todate" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date" CssClass="text-danger">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revQ4ToDate" runat="server" ControlToValidate="txtQ4Todate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="de7" runat="server" PopupButtonID="imgQ4ToDate"
                                                    TargetControlID="txtQ4Todate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                            </div>
                                        </div>
                                    </div>
                                    <div id="FirstHalfSection" style="display: none; visibility: hidden">
                                        <div class="row">
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">First Half Year From Date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtFirstHalffromDate" runat="server" MaxLength="200" CssClass="form-control"
                                                        Text='<%# Eval("SM_FIRST_HALF_YR_DUE_DATE_FROM", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgFHFromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivFHFrom" runat="server" TriggerIndex="5"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtFirstHalffromDate" Display="Dynamic" CssClass="text-danger"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revFHFDate" runat="server" ControlToValidate="txtFirstHalffromDate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="ce8" runat="server" PopupButtonID="imgFHFromDate"
                                                    TargetControlID="txtFirstHalffromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">First Half Year To Date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtFirstHalfToDate" runat="server" MaxLength="200" CssClass="form-control"
                                                        Text='<%# Eval("SM_FIRST_HALF_YR_DUE_DATE_TO", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgFHTDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivFHTo" runat="server" TriggerIndex="5"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtFirstHalfToDate" Display="Dynamic" CssClass="text-danger"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revFHTdate" runat="server" ControlToValidate="txtFirstHalfToDate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgFHTDate"
                                                    TargetControlID="txtFirstHalfToDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Second Half Year From Date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtSecondtHalffromDate" runat="server" CssClass="form-control"
                                                        MaxLength="200" Text='<%# Eval("SM_SECOND_HALF_YR_DUE_DATE_FROM", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgSHFromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivSHForm" runat="server" TriggerIndex="5"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtSecondtHalffromDate" Display="Dynamic" CssClass="text-danger"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
                                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revSHFDate" runat="server" ControlToValidate="txtFirstHalffromDate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="ce9" runat="server" PopupButtonID="imgSHFromDate"
                                                    TargetControlID="txtSecondtHalffromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Second Half Year To Date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtSecondtHalffromTo" runat="server" CssClass="form-control"
                                                        MaxLength="200" Text='<%# Eval("SM_SECOND_HALF_YR_DUE_DATE_TO", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgSHTDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivSHFrom" runat="server" TriggerIndex="5"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtSecondtHalffromTo" Display="Dynamic" CssClass="text-danger"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revSHTdate" runat="server" ControlToValidate="txtSecondtHalffromTo" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgSHTDate"
                                                    TargetControlID="txtSecondtHalffromTo" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                            </div>
                                        </div>
                                    </div>
                                    <div id="YearSection" style="display: none; visibility: hidden">
                                        <div class="row">
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Yearly From Date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtYearlyfromDate" runat="server" CssClass="form-control"
                                                        MaxLength="200" Text='<%# Eval("SM_YEARLY_DUE_DATE_FROM", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgYearlyFromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivYearlyFrom" runat="server" TriggerIndex="6" CssClass="text-danger"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtYearlyfromDate" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revYearlyFDate" runat="server" ControlToValidate="txtFirstHalffromDate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" PopupButtonID="imgYearlyFromDate"
                                                    TargetControlID="txtYearlyfromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Yearly To Date:</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtYearlyDateTo" runat="server" MaxLength="200" CssClass="form-control"
                                                        Text='<%# Eval("SM_YEARLY_DUE_DATE_TO", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgYearlyDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rfvYearlyTo" runat="server" TriggerIndex="6" CssClass="text-danger"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtYearlyDateTo" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revYearlydate" runat="server" ControlToValidate="txtYearlyDateTo" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" PopupButtonID="imgYearlyDate"
                                                    TargetControlID="txtYearlyDateTo" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                            </div>
                                        </div>
                                    </div>
                                    <div id="FortnightlySection" style="display: none; visibility: hidden">
                                        <div class="row">
                                            <div class="col-md-12 mb-3">
                                                <label class="form-label">First Fortnightly: Start Date And End Date</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtFortnightly1FromDate" runat="server" MaxLength="50" CssClass="form-control"
                                                        Text='<%# Eval("SM_FIRST_FORTNIGHTLY_DUE_FROM_DATE", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgF1FD" runat="server" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivF1FromDate" runat="server" TriggerIndex="3" CssClass="text-danger"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtFortnightly1FromDate" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter first fortnightly: Start Date.">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revF1FromDate" runat="server" ControlToValidate="txtFortnightly1FromDate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="ceF1FromDate" runat="server" PopupButtonID="imgF1FD"
                                                    TargetControlID="txtFortnightly1FromDate" Format="dd"></ajaxToolkit:CalendarExtender>

                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtFortnightly1ToDate" runat="server" MaxLength="50" CssClass="form-control"
                                                        Text='<%# Eval("SM_FIRST_FORTNIGHTLY_DUE_TO_DATE", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgF1TD" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivF1ToDate" runat="server" TriggerIndex="3" CssClass="text-danger"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtFortnightly1ToDate" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter first fortnightly: End Date.">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revF1ToDate" runat="server" ControlToValidate="txtFortnightly1ToDate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="ceF1ToDate" runat="server" PopupButtonID="imgF1TD"
                                                    TargetControlID="txtFortnightly1ToDate" Format="dd"></ajaxToolkit:CalendarExtender>

                                            </div>
                                            <div class="col-md-12 mb-3">
                                                <label class="form-label">Second Fortnightly: Start Date And End Date</label>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtFortnightly2FromDate" runat="server" MaxLength="50" CssClass="form-control"
                                                        Text='<%# Eval("SM_SECOND_FORTNIGHTLY_DUE_FROM_DATE", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgF2FD" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivF2FromDate" runat="server" TriggerIndex="3" CssClass="text-danger"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtFortnightly2FromDate" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter second fortnightly: Start Date.">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revF2FromDate" runat="server" ControlToValidate="txtFortnightly2FromDate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="ceF2FromDate" runat="server" PopupButtonID="imgF2FD"
                                                    TargetControlID="txtFortnightly2FromDate" Format="dd"></ajaxToolkit:CalendarExtender>
                                                <div class="input-group">
                                                    <F2FControls:F2FTextBox ID="txtFortnightly2ToDate" runat="server" MaxLength="50" CssClass="form-control"
                                                        Text='<%# Eval("SM_SECOND_FORTNIGHTLY_DUE_TO_DATE", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                    <asp:ImageButton ID="imgF2TD" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />

                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="rivF2ToDate" runat="server" TriggerIndex="3" CssClass="text-danger"
                                                    ControlToCompare="rblFrequency" ControlToValidate="txtFortnightly2ToDate" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter second fortnightly: End Date.">
                                    *
                                                </a:RequiredIfValidatorRadioButtonList>
                                                <asp:RegularExpressionValidator ID="revF2ToDate" runat="server" ControlToValidate="txtFortnightly2ToDate" CssClass="text-danger"
                                                    ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <ajaxToolkit:CalendarExtender ID="ceF2ToDate" runat="server" PopupButtonID="imgF2TD"
                                                    TargetControlID="txtFortnightly2ToDate" Format="dd"></ajaxToolkit:CalendarExtender>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </EditItemTemplate>
                        </asp:FormView>
                        <div class="mt-3 text-center">
                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSave" runat="server" Text="  Save  " ValidationGroup="SaveSubmissionDetails"
                                OnClick="btnSave_Click">
                                <i class="fa fa-save me-2"></i> Save                    
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnCancel" runat="server" CausesValidation="false"
                                Text="Cancel" OnClick="btnCancel_Click">
                                <i class="fa fa-arrow-left me-2"></i> Cancel                   
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlBack" Visible="false" runat="server">
                        <div class="mt-3">
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack" runat="server" CausesValidation="false"
                                Text="Back" OnClick="btnBack_Click">
                                <i class="fa fa-arrow-left me-2"></i>  Back                  
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->





</asp:Content>
