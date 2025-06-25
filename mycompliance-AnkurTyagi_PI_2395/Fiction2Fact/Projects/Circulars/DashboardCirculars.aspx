<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Circulars.Circulars_DashboardCirculars" Title="Dashboard" CodeBehind="DashboardCirculars.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .Legends {
            text-align: left;
            padding: 4px 2px;
            color: #4d5355;
            font: 15px Trebuchet MS;
        }
    </style>
    <%--<link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/css/legacy/bootstrap.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/css/legacy/morrisChart.css")%>" />--%>
    <!-- <link rel="stylesheet" type="text/css" href="css/morris.css"> -->

    <script src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/MorisChart/raphael-min.js")%>"></script>

    <script src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/MorisChart/morris.js")%>"></script>

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js")%>"></script>

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/dateformatvalidation.js")%>"></script>

    <script type="text/javascript">

        function compareEndDates(source, arguments) {
            try {
                //var ContractTemplateId = document.getElementById('ctl00_ContentPlaceHolder1_hfCTId').value;
                //if (ContractTemplateId == '' || ContractTemplateId == null) {


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
    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Reports & Dashboards</h4>
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
                    <div class="row">
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Circular No.</label>
                            <F2FControls:F2FTextBox ID="txtCircularNo" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtCircularNo" />
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Type</label>
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select" AppendDataBoundItems="true"
                                DataTextField="CDTM_TYPE_OF_DOC" DataValueField="CDTM_ID">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Issuing Authority</label>
                            <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlCircularAuthority"
                                runat="server">
                            </f2f:DropdownListNoValidation>
                            <ajaxToolkit:CascadingDropDown ID="cddIssuingAuthority" runat="server" TargetControlID="ddlCircularAuthority"
                                Category="IssuingAuthority" PromptText="(Select an Issuing Authority)" ServicePath="AJAXDropdownCirculars.asmx"
                                ServiceMethod="GetIssuingAuthority" />
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Person Responsible</label>
                            <F2FControls:F2FTextBox ID="txtPersonResponsible" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtPersonResponsible" />
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Circular From Date</label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtCircularFromDate" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                                <asp:ImageButton ID="imgCircularFromDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />
                            </div>
                            <cc1:CalendarExtender ID="ceCircularFromDate" runat="server" PopupButtonID="imgCircularFromDate"
                                TargetControlID="txtCircularFromDate" Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                            <asp:RegularExpressionValidator ID="revCircFromDate" runat="server" ControlToValidate="txtCircularFromDate" CssClass="span"
                                Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Circular To Date</label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtCircularToDate" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                                <asp:ImageButton ID="imgCircularToDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />
                            </div>
                            <cc1:CalendarExtender ID="ceCircularToDate" runat="server" PopupButtonID="imgCircularToDate"
                                TargetControlID="txtCircularToDate" Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                            <asp:RegularExpressionValidator ID="revCircToDate" runat="server" ControlToValidate="txtCircularToDate" CssClass="span"
                                Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Target Date From</label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtFromDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                                <asp:ImageButton ID="ibFrmDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />
                            </div>
                            <asp:RegularExpressionValidator ID="revFromDate" runat="server" ControlToValidate="txtFromDate" CssClass="span"
                                Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                            <cc1:CalendarExtender ID="ceFrmDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibFrmDate"
                                TargetControlID="txtFromDate"></cc1:CalendarExtender>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Target Date To</label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtToDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                                <asp:ImageButton ID="ibToDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />
                            </div>
                            <asp:RegularExpressionValidator ID="revToDate" runat="server" ControlToValidate="txtToDate"
                                Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                            <cc1:CalendarExtender ID="ceTodate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibToDate"
                                TargetControlID="txtToDate"></cc1:CalendarExtender>
                            <asp:CustomValidator ID="cvCompletionDate" runat="server" ClientValidationFunction="compareEndDates"
                                ControlToValidate="txtToDate" ErrorMessage="From Date should be less than or Equal to To Date."
                                Display="Dynamic" OnServerValidate="cvTodate_ServerValidate" ValidationGroup="SEARCH"></asp:CustomValidator>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">&nbsp;</label>
                            <div>
                                <asp:LinkButton ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-outline-primary" OnClick="btnSearch_Click"
                                    ValidationGroup="SEARCH">
                                <i class="fa fa-search"></i> Search                     
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="mt-4">
                        <asp:Panel ID="pnlActionableStatus" runat="server" Visible="false">
                            <div class="row mb-4">
                                <div class="col-md-12">
                                    <button type="button" style="background-color: #65C3A8; color: #fff;" class="btn btn-square btn-skew"><span>Not Yet Due</span></button>
                                    <button type="button" style="background-color: #7ccc32; color: #fff;" class="btn btn-square btn-skew"><span>Completed within Target Date:</span></button>
                                    <button type="button" style="background-color: #ffbf00; color: #fff;" class="btn btn-square btn-skew"><span>Completed after Target Date:</span></button>
                                    <button type="button" style="background-color: #cc0001; color: #fff;" class="btn btn-square btn-skew"><span>Due but not Completed:</span></button>
                                </div>
                            </div>
                            <div class="row mb-4">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="chart-box">
                                            <div class="heading custom-info-alert">
                                                Circular Actionable Dashboard(Percentage)
                           
                                            </div>
                                            <div id="donut-chart-color">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="chart-box">
                                            <center>
                                                <div class="heading custom-info-alert">
                                                    Circular Actionable Dashboard(Count)
                                                   
                                                </div>
                                            </center>
                                            <div id="bar-color-chart">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="table-responsive mb-4">
                                <asp:Literal runat="server" ID="litActionableStatus" />
                            </div>
                        </asp:Panel>
                        <div class="table-responsive mt-4">
                            <asp:Literal runat="server" ID="litChart" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->

</asp:Content>
