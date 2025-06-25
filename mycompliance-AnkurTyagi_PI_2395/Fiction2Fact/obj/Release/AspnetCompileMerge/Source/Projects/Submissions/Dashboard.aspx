<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Submissions.Submissions_Dashboard" Title="Dashboard:" CodeBehind="Dashboard.aspx.cs" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .Legends {
            text-align: left;
            padding: 4px 2px;
            color: #4d5355;
            font: 15px Trebuchet MS;
        }
    </style>
    <script src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/MorisChart/raphael-min.js") %>'></script>
    <script src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/MorisChart/morris.js") %>'></script>
    <script type="text/javascript">

        function hidetd() {
            var e = document.getElementById('<%=ddlReportType.ClientID%>');
            var strReportTye = e.options[e.selectedIndex].value;
            //alert(strReportTye);
            var trDate = document.getElementById("<%=trDate.ClientID%>");


            if (strReportTye == "4") {
                trDate.style.display = "none";
            }
            else {
                trDate.style.display = "table-row";
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
                            <label class="form-label">Report Type: <span class="text-danger">*</span></label>
                            <asp:DropDownList CssClass="form-select" ID="ddlReportType" runat="server" onchange="hidetd()">
                                <asp:ListItem Value="">All</asp:ListItem>
                                <asp:ListItem Value="1">Tracking function wise Compliance Status</asp:ListItem>
                                <asp:ListItem Value="5">Reportings function wise Compliance Status</asp:ListItem>
                                <asp:ListItem Value="2">Organization wide Compliance Status</asp:ListItem>
                                <asp:ListItem Value="3">Monthly Compliance Report</asp:ListItem>
                                <asp:ListItem Value="4">Reportings Dashboard for Previous Month</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvReportType" ControlToValidate="ddlReportType" CssClass="text-danger" Display="Dynamic" SetFocusOnError="True" ErrorMessage="*" ValidationGroup="Search">*</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Tracked By:</label>
                            <asp:DropDownList ID="ddlSubType" CssClass="form-select" AppendDataBoundItems="true"
                                runat="server" DataValueField="STM_ID" DataTextField="STM_TYPE">
                                <asp:ListItem Value="">Select</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Reporting Function:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlReportDept" AppendDataBoundItems="true"
                                runat="server" DataValueField="SRD_ID" DataTextField="SRD_NAME">
                                <asp:ListItem Value="">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Frequency:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlFrequency" runat="server">
                                <asp:ListItem Value="">Select</asp:ListItem>
                                <asp:ListItem Value="Only Once">Adhoc</asp:ListItem>
                                <asp:ListItem Text="Daily" Value="Daily">Daily</asp:ListItem>
                                <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                                <asp:ListItem Value="Fortnightly">Fortnightly</asp:ListItem>
                                <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>
                                <asp:ListItem Value="Half Yearly">Half Yearly</asp:ListItem>
                                <asp:ListItem Value="Yearly">Yearly</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Priority:</label>
                            <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-select">
                                <asp:ListItem Value="">Select</asp:ListItem>
                                <asp:ListItem Value="H">High</asp:ListItem>
                                <asp:ListItem Value="M">Medium</asp:ListItem>
                                <asp:ListItem Value="L">Low</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-12 mb-3" runat="server" id="trDate">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label runat="server" id="tdFromDate" class="form-label">From Date:</label>
                                    <div runat="server" id="tdFromDatetxt">
                                        <div class="input-group">
                                            <F2FControls:F2FTextBox CssClass="form-control" Columns="13" ID="txtFromdate" runat="server"></F2FControls:F2FTextBox>
                                            <asp:ImageButton ID="imgFromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                ImageUrl="../../Content/images/legacy/calendar.jpg" />&nbsp;
                                   
                                       
                                        </div>
                                        <asp:RegularExpressionValidator ID="revCircularDate" runat="server" ControlToValidate="txtFromdate"
                                            ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                            ValidationGroup="Search" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label runat="server" id="tdToDate" class="form-label">To Date:</label>
                                    <div runat="server" id="tdToDatetxt">
                                        <div class="input-group">
                                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtTodate" runat="server" Columns="13"></F2FControls:F2FTextBox>
                                            <asp:ImageButton ID="imgBtnToDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                        </div>
                                        <asp:RegularExpressionValidator ID="revToDate" runat="server" ControlToValidate="txtTodate"
                                            ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                            ValidationGroup="Search" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="text-center mt-3 mb-4">
                        <ajaxToolkit:CalendarExtender ID="ceDate" runat="server" PopupButtonID="imgFromDate"
                            TargetControlID="txtFromdate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:CalendarExtender ID="ceDate1" runat="server" PopupButtonID="imgBtnToDate"
                            TargetControlID="txtTodate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnsearch" runat="server" CausesValidation="true"
                            ValidationGroup="Search" Text="Search" OnClick="btnSearch_Click">
                            <i class="fa fa-search"></i> Search                     
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="lnkReset" runat="server" CausesValidation="false"
                            Text="Cancel" OnClick="lnkReset_Click">Reset
                        </asp:LinkButton>
                    </div>

                    <asp:Panel ID="pnlDeptWiseBarchart" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-md-12 mb-3">
                                <button type="button" style="background-color: #65C3A8; color: #fff;" class="btn btn-square btn-skew"><span>Not Yet Due</span></button>
                                <button type="button" style="background-color: #8F002C; color: #fff;" class="btn btn-square btn-skew"><span>Due but not Submitted:</span></button>
                                <button type="button" style="background-color: #C45850; color: #fff;" class="btn btn-square btn-skew"><span>Not yet submitted:</span></button>
                                <button type="button" style="background-color: #3e95cd; color: #fff;" class="btn btn-square btn-skew"><span>Compliant:</span></button>
                                <button type="button" style="background-color: #8e5ea2; color: #fff;" class="btn btn-square btn-skew"><span>Not Compliant:</span></button>
                                <button type="button" style="background-color: #267DD4; color: #fff;" class="btn btn-square btn-skew"><span>Not Applicable:</span></button>
                                <button type="button" style="background-color: #508ec4; color: #fff;" class="btn btn-square btn-skew"><span>Reopened / Sent back:</span></button>
                            </div>
                        </div>
                        <div class="mt-2">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="chart-box">
                                        <div class="heading custom-info-alert" id="divgraphheading" runat="server">
                                            Tracking Function wise Compliant, Non-Compliant, Not Applicable & Not Submitted
                            Tasks Count
                                       
                                        </div>
                                        <div id="bar-chart">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="mt-2 mb-2">
                                <center>
                                    <asp:Literal runat="server" ID="litDeptWiseBarchart" />
                                </center>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlComplianceStatus" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-md-12 mb-3">
                                <button type="button" style="background-color: #65C3A8; color: #fff;" class="btn btn-square btn-skew"><span>Not Yet Due</span></button>
                                <button type="button" style="background-color: #8F002C; color: #fff;" class="btn btn-square btn-skew"><span>Due but not Submitted:</span></button>
                                <button type="button" style="background-color: #009933; color: #fff;" class="btn btn-square btn-skew"><span>Compliant/Not Applicable:</span></button>
                                <button type="button" style="background-color: #ffcc00; color: #fff;" class="btn btn-square btn-skew"><span>Not Compliant:</span></button>
                                <button type="button" style="background-color: #508ec4; color: #fff;" class="btn btn-square btn-skew"><span>Reopened / Sent back:</span></button>
                            </div>
                        </div>
                        <div class="mt-2">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="chart-box">
                                        <div class="heading  custom-info-alert">
                                            Organization-wide Compliance Status
                                       
                                        </div>
                                        <div id="donut-chart-color">
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="chart-box">
                                        <center>
                                            <div class="heading custom-info-alert">
                                                Organization-wide Compliance Status
                                           
                                            </div>
                                        </center>
                                        <div id="bar-color-chart">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <asp:Literal runat="server" ID="litComplianceStatus" />
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlMonthlyCompliance" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-md-12 mb-3">
                                <button type="button" style="background-color: #65C3A8; color: #fff;" class="btn btn-square btn-skew"><span>Not Yet Due:</span></button>
                                <button type="button" style="background-color: #8F002C; color: #fff;" class="btn btn-square btn-skew"><span>Due but not Submitted:</span></button>
                                <button type="button" style="background-color: #009933; color: #fff;" class="btn btn-square btn-skew"><span>Compliant:</span></button>
                                <button type="button" style="background-color: #ffcc00; color: #fff;" class="btn btn-square btn-skew"><span>Not Compliant:</span></button>
                            </div>
                        </div>
                        <div class="mt-2">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="chart-box">
                                        <center>
                                            <div class="heading  custom-info-alert">
                                                Monthly Compliance Report in Percentage
                           
                                           
                                            </div>
                                        </center>
                                        <div id="stacked-bars">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="mt-2 mb-3">
                                <asp:Literal ID="litMonthlyReport" runat="server" />
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlRegulatoryreportingPrevmonth" runat="server" Visible="false">
                        <center>
                            <div class="heading custom-info-alert" style="text-align: center;">
                                Regulatory Reporting for the Month of
               
                               

                                <asp:Label ID="lblCurrMnth" runat="server" Visible="false" CssClass="heading"></asp:Label>
                            </div>
                        </center>
                    </asp:Panel>
                    <asp:Literal runat="server" ID="litChart" />
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->

</asp:Content>
