<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Circulars.Circulars_CircularReportList" Title="Circular Report" CodeBehind="CircularReportList.aspx.cs" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js")%>">
    </script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/ui.datepicker.js")%>">
    </script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/commonvalidations.js")%>">
    </script>

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/jquery.dataTables.js")%>">
    </script>

    <script type="text/javascript" charset="utf-8">
        var asInitVals = new Array();
        $(document)
            .ready(function () {
                var FromDate = document.getElementById('ctl00_ContentPlaceHolder1_txtFromDate').value;
                var ToDate = document.getElementById('ctl00_ContentPlaceHolder1_txtToDate').value;

                $("#ctl00_ContentPlaceHolder1_txtFromDate").datepicker({
                    showOn: 'button',
                    buttonImage: '../../Content/images/legacy/calendar.gif',
                    buttonImageOnly: true,
                    changeMonth: true,
                    changeYear: true,
                    dateFormat: 'dd-M-yy',
                    onSelect: function () {
                        $(this).change();
                    }
                });

                $("#ctl00_ContentPlaceHolder1_txtToDate").datepicker({
                    showOn: 'button',
                    buttonImage: '../../Content/images/legacy/calendar.gif',
                    buttonImageOnly: true,
                    changeMonth: true,
                    changeYear: true,
                    dateFormat: 'dd-M-yy',
                    onSelect: function () {
                        $(this).change();
                    }
                });

                var oTable = $('#circularReportList')
                                .dataTable(
                                        {
                                            "oLanguage": {
                                                "sSearch": "Search all columns:"
                                            },
                                            "bProcessing": true,
                                            "bServerSide": true,
                                            "sAjaxSource": "../DataTablesJSON.aspx?Type=CircularReport&FromDate="
                                            + FromDate + "&ToDate=" + ToDate,
                                            "bAutoWidth": false,
                                            "aoColumns": [{
                                                "sWidth": "4%"
                                            }, {
                                                "sWidth": "3%",
                                                "bSortable": false
                                            }, {
                                                "sWidth": "3%",
                                                "bSortable": false
                                            }, {
                                                "sWidth": "8%"
                                            }, {
                                                "sWidth": "8%"
                                            }, {
                                                "sWidth": "8%"
                                            }, {
                                                "sWidth": "8%"
                                            }, {
                                                "sWidth": "8%"
                                            }, {
                                                "sWidth": "8%"
                                            }, {
                                                "sWidth": "8%"
                                            }, {
                                                "sWidth": "8%"
                                            }, {
                                                "sWidth": "8%"
                                            }, {
                                                "sWidth": "8%"
                                            }, {
                                                "sWidth": "8%"
                                            }, {
                                                "sWidth": "8%"
                                            }, {
                                                "sWidth": "8%"
                                            }
                                            , {
                                                "sWidth": "8%"
                                            }
    //										,{
    //											"sWidth" : "8%"
    //										}
                                            ],
                                            "sPaginationType": "full_numbers"
                                        });
                $("#circularReportList thead input").keyup(
						function () {
						    /* Filter on the column (the index) of this element */
						    oTable.fnFilter(this.value, $(
									"#circularReportList thead input").index(this));
						});

                $("#circularReportList thead input").each(function (i) {
                    asInitVals[i] = this.value;
                });

                $("#circularReportList thead input").focus(function () {
                    if (this.className == "search_init") {
                        this.className = "";
                        this.value = "";
                    }
                });

                $("#circularReportList thead input").blur(
						function (i) {
						    if (this.value == "") {
						        this.className = "search_init";
						        this.value = asInitVals[$(
										"#circularReportList thead input").index(this)];
						    }
						});
            });


        function exportToExcel() {
            var Id = document.getElementById('search_Id').value;
            var CircularNo = document.getElementById('search_CircularNo').value;
            var CircularDt = document.getElementById('search_CircularDt').value;
            var Type = document.getElementById('search_Type').value;
            //var Function = document.getElementById('search_Function').value;
            var IssuingAuthority = document.getElementById('search_IssuingAuthority').value;
            var Topic = document.getElementById('search_Topic').value;
            var CircularSubject = document.getElementById('search_CircularSubject').value;
            var Gist = document.getElementById('search_Gist').value;
            var Implication = document.getElementById('search_Implication').value;
            var Actionables = document.getElementById('search_Actionables').value;
            var PersonResponsible = document.getElementById('search_PersonResponsible').value;
            var TargetDt = document.getElementById('search_TargetDt').value;
            var Status = document.getElementById('search_Status').value;
            var CompletionDt = document.getElementById('search_CompletionDt').value;
            var Remarks = document.getElementById('search_Remarks').value;

            var FromDate = document.getElementById('ctl00_ContentPlaceHolder1_txtFromDate').value;
            var ToDate = document.getElementById('ctl00_ContentPlaceHolder1_txtToDate').value;

            window.open
            ("../doExportToExcel.aspx?source=Actionable&Id=" + Id + "&CircularNo=" + CircularNo + "&CircularDt=" + CircularDt +
                "&Type=" + Type + "&IssuingAuthority=" + IssuingAuthority + "&Topic=" + Topic + "&CircularSubject=" + CircularSubject + "&Gist=" + Gist + "&Implication=" + Implication + "&Actionables=" + Actionables + "&PersonResponsible=" + PersonResponsible
                + "&TargetDt=" + TargetDt + "&Status=" + Status + "&CompletionDt=" + CompletionDt + "&Remarks=" + Remarks + "&FromDate=" + FromDate + "&ToDate=" + ToDate,
                "FILE", "location=0,status=0,scrollbars=1,width=5,height=5");
            return false;
        }
    </script>

    <br />
    <div style="text-align: left;">
        <%--<center>--%>
        <div class="ContentHeader1">
            Actionables List
        </div>
        <br />
        <%-- </center>--%>
        <table>
            <tr>
                <td class="tabhead3">From Date:</td>
                <td class="tabbody3">
                    <f2fcontrols:f2ftextbox id="txtFromDate" runat="server" CssClass="form-control" columns="20" maxlength="11"></f2fcontrols:f2ftextbox>
                    <%-- <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl='<%# Fiction2Fact.Global.site_url("Content/images/legacy/calendar.jpg")%>'
                    ID="imgDate3" Style="width: 21px" OnClientClick="return false;" />--%>
                    <%-- <cc1:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgDate3"
                    TargetControlID="txtFromDate" Format="dd-MMM-yyyy">
                </cc1:CalendarExtender>--%>
                </td>
                <td class="tabhead3">To Date:</td>
                <td class="tabbody3">
                    <f2fcontrols:f2ftextbox id="txtToDate" runat="server" columns="20" CssClass="form-control" maxlength="11"></f2fcontrols:f2ftextbox>
                    <%--   <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl='<%# Fiction2Fact.Global.site_url("Content/images/legacy/calendar.jpg")%>'
                    ID="imgTodate" Style="width: 21px" OnClientClick="return false;" />--%>
                    <%--<cc1:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgTodate"
                    TargetControlID="txtTodate" Format="dd-MMM-yyyy">
                </cc1:CalendarExtender>--%>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:RegularExpressionValidator ID="revNoticeRcvd" runat="server" ControlToValidate="txtFromDate" CssClass="span"
                        ErrorMessage="From Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                        ValidationGroup="Search" Display="Dynamic"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="rev" runat="server" ControlToValidate="txtToDate" CssClass="span"
                        ErrorMessage="To Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                        ValidationGroup="Search" Display="Dynamic"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Button CssClass="html_button" Text="Search" ID="btnSearch" runat="server" ValidationGroup="Search" />
                    <%--<asp:ImageButton ID="btnSearch" ValidationGroup="Search" runat="server" AlternateText="Search"
                    ImageUrl="../../Content/images/legacy/buttons_page/search.gif" />--%>
                    <asp:ImageButton runat="server" ID="imgExcel" OnClientClick="return exportToExcel()"
                        ImageUrl="../../Content/images/legacy/e-icon.gif" ToolTip="Export To Excel" />
                </td>
            </tr>
        </table>
        <br />
        <%--  <asp:ImageButton runat="server" ID="imgExcel" OnClientClick="return exportToExcel()" 
    ImageUrl="../../Content/images/legacy/e-icon.gif" ToolTip="Export To Excel" /> --%>
        <%--<br />
    <br />--%>
        <div id="dt_example">
            <table border="0" cellpadding="0" cellspacing="1" bgcolor="#D0D0D0" class="display"
                id="circularReportList" width="100%">
                <thead>
                    <tr>
                        <th>Id
                        </th>
                        <th>View
                        </th>
                        <th>Edit
                        </th>
                        <th>Circular No.
                        </th>
                        <th>Circular Date
                        </th>
                        <th>Type
                        </th>
                        <%-- <th>
                            Function
                        </th>--%>
                        <th>Issuing Authority
                        </th>
                        <th>Topic
                        </th>
                        <th>Subject of the Circular
                        </th>
                        <th>Gist
                        </th>
                        <th>Implication
                        </th>
                        <th>Actionables
                        </th>
                        <th>Person/Function Responsible
                        </th>
                        <th>Target date of the actionable
                        </th>
                        <th>Status of the actionable
                        </th>
                        <th>Actual Completion date
                        </th>
                        <th>Remarks
                        </th>
                    </tr>
                </thead>
                <thead>
                    <tr bgcolor="#FFffFF">
                        <td align="left">
                            <input type="text" name="search_Id" value="Search Id" size="3" class="search_init" />
                        </td>
                        <td align="left"></td>
                        <td align="left"></td>
                        <td align="left">
                            <input type="text" name="search_CircularNo" value="search Circular No." class="search_init" />
                        </td>
                        <td align="left">
                            <input type="text" name="search_CircularDt" value="Search Circular Date" class="search_init" />
                        </td>
                        <td align="left">
                            <input type="text" name="search_Type" value="Search Type" class="search_init" />
                        </td>
                        <%--<td align="left">
                            <input type="text" name="search_Function" value="Search Function" class="search_init" />
                        </td>--%>
                        <td align="left">
                            <input type="text" name="search_IssuingAuthority" value="Search Issuing Authority"
                                class="search_init" />
                        </td>
                        <td align="left">
                            <input type="text" name="search_Topic" value="Search Topic" class="search_init" />
                        </td>
                        <td align="left">
                            <input type="text" name="search_CircularSubject" value="Search Subject" class="search_init" />
                        </td>
                        <td align="left">
                            <input type="text" name="search_Gist" value="Search Gist" class="search_init" />
                        </td>
                        <td align="left">
                            <input type="text" name="search_Implication" value="Search Implication" class="search_init" />
                        </td>
                        <td align="left">
                            <input type="text" name="search_Actionables" value="Search Actionables" class="search_init" />
                        </td>
                        <td align="left">
                            <input type="text" name="search_PersonResponsible" value="Search Person/Function Responsible"
                                class="search_init" />
                        </td>
                        <td align="left">
                            <input type="text" name="search_TargetDt" value="Search Target Date" class="search_init" />
                        </td>
                        <td align="left">
                            <input type="text" name="search_Status" value="Search Status" class="search_init" />
                        </td>
                        <td align="left">
                            <input type="text" name="search_CompletionDt" value="Search Completion Date" class="search_init" />
                        </td>
                        <td align="left">
                            <input type="text" name="search_Remarks" value="Search Remarks" class="search_init" />
                        </td>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
</asp:Content>
