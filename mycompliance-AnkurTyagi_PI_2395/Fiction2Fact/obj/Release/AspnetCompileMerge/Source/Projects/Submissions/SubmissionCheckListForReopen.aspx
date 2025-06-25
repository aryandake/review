<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Submissions.SubmissionCheckListForReopen" Title="Submission Checklist For Admin"
    Async="true" CodeBehind="SubmissionCheckListForReopen.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<<Added by Ashish Mishra on 27Jul2017--%>

    <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/dateformatvalidation.js") %>'>
    </script>

    <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/DateValidator.js") %>'>
    </script>

    <script type="text/javascript">
        function openSubmissionFilesWindow(scId, operationType, Type) {
            window.open('../Submissions/SubmissionCheckListFiles.aspx?SCId=' + scId + '&OperationType=' + operationType + '&Type=' + Type, '', 'location=0,status=0,scrollbars=1,width=900,height=600');
            return false;
        }
        function compareStartDateSystemDate(source, arguments) {
            try {
                var cnt = 0;
                var StartDate;
                var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurDate');
                var grid = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklistDetails");
                if (grid != null) {
                    if (grid.rows.length > 0) {
                        for (var j = 2; j < grid.rows.length + 1; j++) {
                            cnt++;
                            if (j < 10) {
                                j = "0" + j;
                            }
                            else {
                                j = j;
                            }
                            StartDate = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklistDetails_ctl" + j + "_txtSubAuthorityDate");
                            //alert(StartDate);
                            if (StartDate != null) {
                                if (compare2Dates(SystemDate, StartDate) > 1) {
                                    arguments.IsValid = false;
                                    break;
                                }
                                else {
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

        function validateReOpenComments(txtReOpenComments) {
            var ddlReopeningPurpose = document.getElementById(ddlReopeningPurpose);
            var ReopeningPurpose = ddlReopeningPurpose.options[ddlReopeningPurpose.selectedIndex].value;
            var ReOpenComments = document.getElementById(txtReOpenComments).value;

            if (ReopeningPurpose == '' || ReOpenComments == '') {
                alert("Purpose of reopening and Reopen Remarks Cannot be blank.");
                return false;
            }
            else {
                return true;
            }

        }

        function Validate(ID) {
            if (Page_ClientValidate('Save' + ID)) {
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
            else {
                return false;
            }
        }
    </script>

    <%-->>--%>
    <asp:HiddenField ID="hfMonth" runat="server" />
    <asp:HiddenField ID="hfClientId" runat="server" />
    <%--<<Added by Ashish Mishra on 27Jul2017--%>
    <asp:HiddenField ID="hfActiontype" runat="server" />
    <asp:HiddenField ID="hfCurDate" runat="server" />
    <asp:HiddenField ID="hfUserType" runat="server" />
    <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />
    <%-->>--%>

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Re-open Submissions</h4>
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
                    <asp:LinkButton runat="server" ID="btnRefresh"
                        Style="visibility: hidden; display: none;" OnClick="btnRefresh_Click">
Refresh
                    </asp:LinkButton>
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label class="form-label">Financial Year:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlFinYear" runat="server"
                                DataValueField="FYM_ID" DataTextField="FYM_FIN_YEAR">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvFinYear" ControlToValidate="ddlFinYear" Display="Dynamic" SetFocusOnError="True" ErrorMessage="*" ValidationGroup="Months">Please select Financial year
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="mt-3">
                        <asp:LinkButton ID="lbtnApr" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="Apr" OnClick="lbtnApr_Click" CausesValidation="true"
                            ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnMay" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="May" OnClick="lbtnMay_Click" ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnJune" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="June" OnClick="lbtnJune_Click"
                            ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnJuly" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="July" OnClick="lbtnJuly_Click"
                            ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnAug" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="Aug" OnClick="lbtnAug_Click" ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnSep" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="Sep" OnClick="lbtnSep_Click" ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnOct" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="Oct" OnClick="lbtnOct_Click" ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnNov" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="Nov" OnClick="lbtnNov_Click" ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnDec" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="Dec" OnClick="lbtnDec_Click" ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnJan" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="Jan" OnClick="lbtnJan_Click" ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnFeb" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="Feb" OnClick="lbtnFeb_Click" ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnMarch" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="Mar" OnClick="lbtnMarch_Click"
                            ValidationGroup="Months" />
                    </div>
                    <div class="mt-3">
                        <asp:LinkButton ID="btnExportToExcel" runat="server" CssClass="btn btn-outline-secondary" Text="Export to Excel" OnClick="btnExportToExcel_Click" Visible="false">
         <i class="fa fa-download"></i> Export to Excel               
                        </asp:LinkButton>
                    </div>
                    <div class="mt-3">
                        <div class="table-responsive">
                            <asp:GridView ID="gvChecklistDetails" AllowPaging="False" runat="server" AutoGenerateColumns="False"
                                OnSorting="gvChecklistDetails_Sorting" DataKeyNames="SC_ID" GridLines="None"
                                AllowSorting="true" CellPadding="4" OnRowDataBound="gvChecklistDetails_RowDataBound"
                                OnSelectedIndexChanged="gvChecklistDetails_SelectedIndexChanged" CssClass="table table-bordered footable"
                                PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:TemplateField HeaderText="Submission ID" SortExpression="SUB_ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubId" Text='<%#Eval("SUB_ID")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lbStmId" Text='<%#Eval("SC_STM_ID")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblSrdId" Text='<%#Eval("SRD_ID")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblSCId" Text='<%#Eval("SC_ID")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblReopenPurpose" Text='<%#Eval("SUB_REOPEN_PURPOSE")%>' runat="server" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submission Status" SortExpression="SUB_STATUS" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubStatus" Text='<%#Eval("SUB_STATUS")%>' runat="server" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Sr No.
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" Text='<%# Container.DataItemIndex + 1 %>' runat="server"> 
                                            </asp:Label>.
               
                                       
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="​Relevant​ Files">
                                        <ItemTemplate>
                                            <a href="javascript:void(0);" class="btn btn-sm btn-soft-info btn-circle" onclick="javascript:window.open('../Submissions/SubmissionDocuments.aspx?Id=<%# Eval("SC_SM_ID") %>','','location=0,status=0,scrollbars=1,width=450,height=250,resizable=1');">
                                                <i class="fa fa-eye"></i>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department" SortExpression="STM_TYPE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstmType" Text='<%#Eval("STM_TYPE")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reporting to" SortExpression="SSM_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSegment" Text='<%#Eval("SSM_NAME")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reference Circular / Notification / Act" SortExpression="SM_ACT_REG_SECTION">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReference" Width="300px" runat="server" ToolTip='<%#Eval("SM_ACT_REG_SECTION").ToString()%>'
                                                Text='<%#Eval("SM_ACT_REG_SECTION").ToString().Length > 200 ? (Eval("SM_ACT_REG_SECTION") as string).Substring(0, 200).Replace("\n", "<br />") 
                                                    + " ..." : Eval("SM_ACT_REG_SECTION").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                            <asp:Label ID="lblReference1" runat="server" Visible="false" Text='<%#Eval("SM_ACT_REG_SECTION").ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Section/Clause" SortExpression="SM_SECTION_CLAUSE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSection" Width="300px" runat="server" ToolTip='<%#Eval("SM_SECTION_CLAUSE").ToString()%>'
                                                Text='<%#Eval("SM_SECTION_CLAUSE").ToString().Length > 200 ? (Eval("SM_SECTION_CLAUSE") as string).Substring(0, 200).Replace("\n", "<br />") 
                                                    + " ..." : Eval("SM_SECTION_CLAUSE").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                            <asp:Label ID="lblSection1" runat="server" Visible="false" Text='<%#Eval("SM_SECTION_CLAUSE").ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Particulars" SortExpression="SC_PARTICULARS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblParticulars" Width="300px" runat="server" ToolTip='<%#Eval("SC_PARTICULARS").ToString()%>'
                                                Text='<%#Eval("SC_PARTICULARS").ToString().Length > 200 ? (Eval("SC_PARTICULARS") as string).Substring(0, 200).Replace("\n", "<br />") 
                                                    + " ..." : Eval("SC_PARTICULARS").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                            <asp:Label ID="lblParticulars1" runat="server" Visible="false" Text='<%#Eval("SC_PARTICULARS").ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" SortExpression="SC_DESCRIPTION">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" Width="300px" runat="server" ToolTip='<%#Eval("SC_DESCRIPTION").ToString()%>'
                                                Text='<%#Eval("SC_DESCRIPTION").ToString().Length > 200 ? (Eval("SC_DESCRIPTION") as string).Substring(0, 200).Replace("\n", "<br />") 
                                                    + " ..." : Eval("SC_DESCRIPTION").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                            <asp:Label ID="lblDescription1" runat="server" Visible="false" Text='<%#Eval("SC_DESCRIPTION").ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Frequency" SortExpression="SC_FREQUENCY">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFrequency" Text='<%#Eval("SC_FREQUENCY")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Internal due date" SortExpression="SC_DUE_DATE_FROM">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDueDateFrom" Width="75px" Text='<%#Eval("SC_DUE_DATE_FROM","{0:dd-MMM-yyyy}")%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Regulatory due date" SortExpression="SC_DUE_DATE_TO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDueDateTo" Width="75px" Text='<%#Eval("SC_DUE_DATE_TO","{0:dd-MMM-yyyy}")%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Yes/No/NA">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlYesNoNA" runat="server" Width="130" CssClass="form-select">
                                                <asp:ListItem Text="-Select-" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                <asp:ListItem Text="NA" Value="NA"></asp:ListItem>
                                            </asp:DropDownList>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblRemarks" Text='<%# Eval("SUB_REMARKS").ToString().Replace("\n","<br />")  %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="SUB_SUBMITTED_BY" HeaderText="Submitted By" SortExpression="SUB_SUBMITTED_BY" />--%>
                                    <asp:TemplateField HeaderText="Submitted By" SortExpression="SUB_SUBMITTED_BY">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubmittedBy" Text='<%# Getfullname(Eval("SUB_SUBMITTED_BY").ToString())%>'
                                                runat="server"></asp:Label>
                                            <asp:Label ID="lblSubmittedBy1" Text='<%# Eval("SUB_SUBMITTED_BY")%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Submitted On" SortExpression="SUB_SUBMIT_DATE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubmittedOn" Text='<%#Eval("SUB_SUBMIT_DATE","{0:dd-MMM-yyyy HH:mm:ss}")%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="File uploaded by Reporting Department" Visible="false">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="ClientFileName" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="ServerFileName" runat="server" Value='<%# Bind("SUB_SERVER_FILE_NAME") %>'></asp:HiddenField>
                                            <a id="Filelink<%# Container.DataItemIndex + 2  %>" href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=ChecklistFilesFolder&downloadFileName=<%# Eval("SUB_SERVER_FILE_NAME")%>&fileName=<%#getFileName(Eval("SUB_CLIENT_FILE_NAME"))%>','','location=0,status=0,scrollbars=0,width=20,height=20');">
                                                <%#Eval("SUB_CLIENT_FILE_NAME")%>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Attach Files">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" CssClass="btn btn-outline-primary" Text="Attach" ID="lbAttach">
                                             Attach
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Supportings" Visible="false">
                                        <ItemTemplate>
                                            <asp:DataList ID="dlDescription" runat="server" CssClass="custom-datalist-border" CellPadding="1" EnableViewState="True"
                                                RepeatColumns="1" RepeatDirection="Horizontal" DataSource='<%#LoadSubmissionFileList(Eval("SC_ID"))%>'>
                                                <ItemTemplate>
                                                    <a id="Filelink" href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=SubmissionFiles&downloadFileName=<%# Eval("SF_SERVER_FILE_NAME")%>&fileName=<%#getFileName(Eval("SF_FILE_NAME"))%>','','location=0,status=0,scrollbars=0,width=20,height=20');">
                                                        <%#Eval("SF_FILE_NAME")%>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<<Added by Ashish Mishra on 16Aug2017--%>
                                    <asp:TemplateField HeaderText="Mode of Filing" Visible="false">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlModeOfFiling" runat="server" DataTextField="RC_NAME" DataValueField="RC_CODE"
                                                CssClass="form-select">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-->>--%>
                                    <asp:TemplateField HeaderText="Submitted to Authority On / Date of Receipt of Data"
                                        SortExpression="SUB_SUBMITTED_TO_AUTHORITY_ON">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubAuthorityDate" Text='<%#Eval("SUB_SUBMITTED_TO_AUTHORITY_ON","{0:dd-MMM-yyyy}")%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submitted to Authority On / Date of Receipt of Data"
                                        Visible="false">
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <F2FControls:F2FTextBox ID="txtSubAuthorityDate" runat="server" Columns="15" MaxLength="11"
                                                            CssClass="form-control" Text='<%# Bind("SUB_SUBMITTED_TO_AUTHORITY_ON", "{0:dd-MMM-yyyy}") %>'></F2FControls:F2FTextBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="imgSubDate" runat="server" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" OnClientClick="return false" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--                    <asp:RequiredFieldValidator ID="rfvSubDate" runat="server" ControlToValidate="txtSubAuthorityDate" CssClass="text-danger"
                        Display="Dynamic" SetFocusOnError="True" ErrorMessage="Please select Submission Date.">*</asp:RequiredFieldValidator>--%>
                                            <ajaxToolkit:CalendarExtender ID="ceSubDate" runat="server" PopupButtonID="imgSubDate"
                                                TargetControlID="txtSubAuthorityDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="revSubDate" runat="server" ControlToValidate="txtSubAuthorityDate" CssClass="text-danger"
                                                ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                                Display="Dynamic"></asp:RegularExpressionValidator>
                                            <asp:CustomValidator ID="cvSubDate" runat="server" ControlToValidate="txtSubAuthorityDate" CssClass="text-danger" ErrorMessage="Submission to Authority on date should not be greater than current date."
                                                Display="Dynamic" ClientValidationFunction="compareStartDateSystemDate" OnServerValidate="cvSubDate_ServerValidate"> 
                                            </asp:CustomValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-->>--%>

                                    <asp:TemplateField HeaderText="Purpose of reopening">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlReopeningPurpose" runat="server" DataTextField="RC_NAME" DataValueField="RC_CODE"
                                                CssClass="form-select" Width="200">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvReopeningPurpose" runat="server" ControlToValidate="ddlReopeningPurpose"
                                                CssClass="text-danger" Display="Dynamic" SetFocusOnError="True" ErrorMessage="Please Select Purpose of reopening.">
                                                Select Purpose of reopening</asp:RequiredFieldValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<<Added by Ashish Mishra on 28Jul2017--%>
                                    <asp:TemplateField HeaderText="Closure Remarks">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSUB_CLOSURE_COMMENTS" Rows="4" Width="200" CssClass="form-control"
                                                Text='<%#Eval("SUB_CLOSURE_COMMENTS").ToString().Replace("\n","<br />") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Previous Reopen Remarks">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReOpenComments" Rows="4" Width="200" CssClass="form-control"
                                                Text='<%#Eval("SUB_REOPEN_COMMENTS").ToString().Replace("\n","<br />") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Reopen Remarks
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <F2FControls:F2FTextBox ID="txtReOpenComments" TextMode="MultiLine" Rows="4" Width="200" CssClass="form-control"
                                                runat="server"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtReOpenComments" />
                                            <asp:RequiredFieldValidator ID="rfvComments" runat="server" ControlToValidate="txtReOpenComments"
                                                CssClass="text-danger" Display="Dynamic" SetFocusOnError="True" ErrorMessage="Enter Reopen Comments.">
                                                Enter Reopen Comments</asp:RequiredFieldValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-->>--%>
                                    <%-- <asp:TemplateField HeaderText="Change Status">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                        <asp:ListItem Value="" Text="(None)" />
                        <%--<asp:ListItem Value="P" Text="Pending" />
                        <asp:ListItem Value="R" Text="Reopened" />
                        <asp:ListItem Value="S" Text="Submitted" />
                        <%--<<Added by Ashish Mishra on 28Jul2017
                        <%--<asp:ListItem Value="C" Text="Closed" />
                      
                    </asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>--%>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbSave" runat="server" CssClass="btn btn-outline-primary" Text="Reopen" CommandName="Select"
                                                OnClientClick='<%# String.Format("javascript:return Validate(\"{0}\")", Eval("SUB_ID").ToString()) %>'>
                                Reopen
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->



</asp:Content>
