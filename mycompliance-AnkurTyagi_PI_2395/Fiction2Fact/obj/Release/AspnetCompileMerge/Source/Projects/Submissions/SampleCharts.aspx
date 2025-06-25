<%@ Page Language="C#" AutoEventWireup="true" Inherits="Fiction2Fact.Projects.Submissions.Submissions_SampleCharts" CodeBehind="SampleCharts.aspx.cs" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" type="text/css" href="../css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../css/morrisChart.css" />
    <!-- <link rel="stylesheet" type="text/css" href="css/morris.css"> -->
    <link id="Link2" rel="stylesheet" type="text/css" href="~/css/main.css" runat="server" />
    <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/main.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/controlStyle.css")%>" />

    <%--<script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/jquery-3.5.0.js") %>"></script>--%>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/jQuery_3.6.0.js") %>"></script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/jQuery_Migrate_3_3_2.js") %>"></script>

    <script src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/MorisChart/raphael-min.js") %>'></script>

    <script src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/MorisChart/morris.js") %>'></script>

</head>
<body>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div>
            <table bgcolor="#dddddd" cellspacing="1" cellpadding="2">
                <tr>
                    <td class="tabhead3">Report Type:</td>
                    <td class="tabbody3">
                        <asp:DropDownList CssClass="form-select" Width="127px" ID="ddlReportType" runat="server">
                            <asp:ListItem Value="">Select</asp:ListItem>
                            <asp:ListItem Value="1">Tracking Department wise Complied ,Non-Complied and Not submitted</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="rfvReportType" ControlToValidate="ddlReportType" CssClass="span" Display="Dynamic" SetFocusOnError="True" ErrorMessage="*" ValidationGroup="Search">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="tabhead3">Tracked By:<asp:Label ID="Label4" runat="server" ForeColor="red" Text="*"></asp:Label></td>
                    <td class="tabbody3">
                        <asp:DropDownList ID="ddlSubType" CssClass="form-select" Width="302px" AppendDataBoundItems="true"
                            runat="server" DataValueField="STM_ID" DataTextField="STM_TYPE">
                            <asp:ListItem Value="">Select</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="tabhead3">Reporting Function:</td>
                    <td class="tabbody3">
                        <asp:DropDownList CssClass="form-select" ID="ddlReportDept" AppendDataBoundItems="true"
                            runat="server" DataValueField="SRD_ID" DataTextField="SRD_NAME" Width="127px">
                            <asp:ListItem Value="">All</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tabhead3">Frequency:
                    </td>
                    <td class="tabbody3">
                        <asp:DropDownList CssClass="form-select" Width="127px" ID="ddlFrequency" runat="server">
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
                    </td>
                    <td class="tabhead3">From Date:</td>
                    <td class="tabbody3">
                        <F2FControls:F2FTextBox CssClass="form-control" Columns="13" ID="txtFromdate" runat="server"></F2FControls:F2FTextBox>
                        <asp:ImageButton ID="imgFromDate" runat="server" AlternateText="Click to show calendar"
                            ImageUrl="../../Content/images/legacy/calendar.jpg" />&nbsp;
                        <asp:RegularExpressionValidator ID="revCircularDate" runat="server" ControlToValidate="txtFromdate"
                            ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                            ValidationGroup="Search" Display="Dynamic"></asp:RegularExpressionValidator>
                    </td>
                    <td class="tabhead3">To Date:</td>
                    <td class="tabbody3">
                        <F2FControls:F2FTextBox CssClass="form-control" ID="txtTodate" runat="server" Columns="13"></F2FControls:F2FTextBox>
                        <asp:ImageButton ID="imgBtnToDate" runat="server" AlternateText="Click to show calendar"
                            ImageUrl="../../Content/images/legacy/calendar.jpg" />&nbsp;
                        <asp:RegularExpressionValidator ID="revToDate" runat="server" ControlToValidate="txtTodate"
                            ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                            ValidationGroup="Search" Display="Dynamic"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="tabhead3">Priority:<asp:Label ID="Label2" runat="server" ForeColor="red" Text="*"></asp:Label>
                    </td>
                    <td class="tabbody3">
                        <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-select" Width="302px">
                            <asp:ListItem Value="">Select</asp:ListItem>
                            <asp:ListItem Value="H">High</asp:ListItem>
                            <asp:ListItem Value="M">Medium</asp:ListItem>
                            <asp:ListItem Value="L">Low</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <ajaxToolkit:CalendarExtender ID="ceDate" runat="server" PopupButtonID="imgFromDate"
                TargetControlID="txtFromdate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
            <ajaxToolkit:CalendarExtender ID="ceDate1" runat="server" PopupButtonID="imgBtnToDate"
                TargetControlID="txtTodate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
            <br />
            <asp:Button CssClass="html_button" ID="btnsearch" runat="server" CausesValidation="true"
                ValidationGroup="Search" Text="Search" OnClick="btnSearch_Click" />
        </div>
        <br />
        <div class="row">
            <div class="chart-box">
                <div class="heading">
                    <center>Tracking Department wise Complied, Not Complied, Not Applicable & Not Submitted</center>
                </div>
                <div id="Legend">
                    <asp:Label ID="lblLegend1" runat="server" Width="15px" BackColor="#3e95cd"></asp:Label>
                    <asp:Label ID="lblLegend2" runat="server" Width="5px"></asp:Label>
                    <asp:Label ID="lblLegend3" runat="server" Width="5px"></asp:Label>
                    <asp:Label ID="lblLegend4" runat="server" Width="5px"></asp:Label>
                </div>
                <div id="bar-chart">
                </div>

            </div>
        </div>
        <asp:Literal runat="server" ID="litChart" />
    </form>
</body>
</html>
