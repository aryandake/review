<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Submissions.SubmissionCheckListForClosure" Title="Submission Checklist For Admin"
    Async="true" CodeBehind="SubmissionCheckListForClosure.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
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

        function validateReOpenComments(ddlStatus, txtReOpenComments) {
            var ddl = document.getElementById(ddlStatus);
            var Status = ddl.options[ddl.selectedIndex].value;
            var ReOpenComments = document.getElementById(txtReOpenComments).value;

            if (ReOpenComments == '') {
                alert("Remarks Cannot be blank.");
                return false;
            }
            else {
                return true;
            }

        }
        function Validate(ele) {
            var tr = $(ele).closest('tr');
            var ddlHygiene = $(tr).find('[id*="ddlHygiene"]');
            var ddlFiling = $(tr).find('[id*="ddlModeOfFiling"]');
            var txtSubDate = $(tr).find('[id*="txtSubAuthorityDate"]');
            var ddlStatus = $(tr).find('[id*="ddlStatus"]');

            var hfSubmittedOn = $(tr).find('[id*="hfSubmittedOn"]');
            var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurDate');

            if (ddlHygiene.val() == '' && ddlStatus.val() != 'SR') { alert('Please select Hygiene check.'); ddlHygiene.focus(); return false; }
            if (ddlFiling.val() == '' && ddlStatus.val() != 'SR') { alert('Please select Mode of Filing'); ddlFiling.focus(); return false; }
            if (txtSubDate.val() == '' && ddlStatus.val() != 'SR') { alert('Please select date'); txtSubDate.focus(); return false; }

            if (txtSubDate.val() != '') {
                if (compare2DatesNew(SystemDate.value, txtSubDate.val()) > 1) {
                    alert('Submission to Authority on date should not be greater than current date.');
                    return false;
                }
            }
            
            if (txtSubDate.val() != '') {
                if (compare2DatesNew(txtSubDate.val(), hfSubmittedOn.val()) > 1) {
                    alert('Submission to Authority on date should not be less than submission on date.');
                    return false;
                }
            }

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
    </script>

    <%-->>--%>
    <asp:HiddenField ID="hfMonth" runat="server" />
    <asp:HiddenField ID="hfClientId" runat="server" />
    <%--<<Added by Ashish Mishra on 27Jul2017--%>
    <asp:HiddenField ID="hfActiontype" runat="server" />
    <asp:HiddenField ID="hfCurDate" runat="server" />
    <asp:HiddenField ID="hfUserType" runat="server" />
    <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Close Submissions</h4>
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
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Financial Year:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlFinYear" runat="server"
                                DataValueField="FYM_ID" DataTextField="FYM_FIN_YEAR">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvFinYear" ControlToValidate="ddlFinYear" CssClass="text-danger" Display="Dynamic" SetFocusOnError="True" ErrorMessage="*" ValidationGroup="Months">Please select Financial year
                            </asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-8 mb-3">
                            <label class="form-label">Status:</label>
                            <div class="custom-checkbox-table">
                                <asp:CheckBoxList ID="chkStatus" CssClass="form-control" AppendDataBoundItems="true"
                                    runat="server" RepeatColumns="5">
                                    <asp:ListItem Value="C" Text="Closed" />
                                    <asp:ListItem Value="R" Text="Reopened" />
                                    <asp:ListItem Value="S" Text="Submitted" />
                                </asp:CheckBoxList>
                            </div>
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
                        <div class="table-responsive" style="min-height: 500px">
                            <asp:GridView ID="gvChecklistDetails" AllowPaging="False" runat="server" AutoGenerateColumns="False"
                                OnSorting="gvChecklistDetails_Sorting" DataKeyNames="SC_ID" GridLines="None" EmptyDataText="No checklist available for selected month."
                                AllowSorting="true" CellPadding="4" OnRowDataBound="gvChecklistDetails_RowDataBound"
                                OnSelectedIndexChanged="gvChecklistDetails_SelectedIndexChanged" CssClass="table table-bordered footable"
                                PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:TemplateField HeaderText="Submission ID" SortExpression="SUB_ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSCId" Text='<%#Eval("SC_ID")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblSubId" Text='<%#Eval("SUB_ID")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lbStmId" Text='<%#Eval("SC_STM_ID")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblSrdId" Text='<%#Eval("SRD_ID")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblReportingDept" Text='<%#Eval("SRD_NAME")%>' runat="server" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submission Status" SortExpression="SUB_STATUS" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubStatus" Text='<%#Eval("SUB_STATUS")%>' runat="server" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sr No.">
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
                                            <%--<asp:Label ID="lblReference" runat="server" Text='<%#Eval("SM_ACT_REG_SECTION").ToString().Replace("\n","<br />") %>'></asp:Label>--%>
                                            <asp:Label ID="lblReference" Width="300px" runat="server" ToolTip='<%#Eval("SM_ACT_REG_SECTION").ToString()%>'
                                                Text='<%#Eval("SM_ACT_REG_SECTION").ToString().Length > 200 ? (Eval("SM_ACT_REG_SECTION") as string).Substring(0, 200).Replace("\n", "<br />") 
                                                    + " ..." : Eval("SM_ACT_REG_SECTION").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                            <asp:Label ID="lblReference1" runat="server" Visible="false" Text='<%#Eval("SM_ACT_REG_SECTION").ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Section/Clause" SortExpression="SM_SECTION_CLAUSE">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblSection" runat="server" Text='<%#Eval("SM_SECTION_CLAUSE").ToString().Replace("\n","<br />") %>'></asp:Label>--%>
                                            <asp:Label ID="lblSection" Width="300px" runat="server" ToolTip='<%#Eval("SM_SECTION_CLAUSE").ToString()%>'
                                                Text='<%#Eval("SM_SECTION_CLAUSE").ToString().Length > 200 ? (Eval("SM_SECTION_CLAUSE") as string).Substring(0, 200).Replace("\n", "<br />") 
                                                    + " ..." : Eval("SM_SECTION_CLAUSE").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                            <asp:Label ID="lblSection1" runat="server" Visible="false" Text='<%#Eval("SM_SECTION_CLAUSE").ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Particulars" SortExpression="SC_PARTICULARS">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblParticulars" Text='<%#Eval("SC_PARTICULARS").ToString().Replace("\n","<br />") %>' runat="server"
                                                Width="200px"></asp:Label>--%>
                                            <asp:Label ID="lblParticulars" Width="300px" runat="server" ToolTip='<%#Eval("SC_PARTICULARS").ToString()%>'
                                                Text='<%#Eval("SC_PARTICULARS").ToString().Length > 200 ? (Eval("SC_PARTICULARS") as string).Substring(0, 200).Replace("\n", "<br />") 
                                                    + " ..." : Eval("SC_PARTICULARS").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                            <asp:Label ID="lblParticulars1" runat="server" Visible="false" Text='<%#Eval("SC_PARTICULARS").ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" SortExpression="SC_DESCRIPTION">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblDescription" Text='<%#Eval("SC_DESCRIPTION").ToString().Replace("\n","<br />") %>' runat="server"
                                                Width="200px"></asp:Label>--%>
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
                                            <asp:DropDownList ID="ddlYesNoNA" runat="server" Width="100" CssClass="form-select">
                                                <asp:ListItem Text="-Select-" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                <asp:ListItem Text="NA" Value="NA"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblRemarks" Width="150" Text='<%# Eval("SUB_REMARKS").ToString().Replace("\n","<br />")  %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submitted By">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSubmittedBy" Text='<%# Getfullname(Eval("SUB_SUBMITTED_BY").ToString()) %>' />
                                            <asp:Label runat="server" ID="lblSubmittedBy1" Text='<%# Eval("SUB_SUBMITTED_BY") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="SUB_SUBMITTED_BY" HeaderText="Submitted By" SortExpression="SUB_SUBMITTED_BY" />--%>
                                    <asp:TemplateField HeaderText="Submitted On" SortExpression="SUB_SUBMIT_DATE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubmittedOn" Text='<%#Eval("SUB_SUBMIT_DATE","{0:dd-MMM-yyyy HH:mm:ss}")%>'
                                                runat="server"></asp:Label>
                                            <asp:HiddenField ID="hfSubmittedOn" runat="server" Value='<%#Eval("SUB_SUBMIT_DATE","{0:dd-MMM-yyyy}")%>'></asp:HiddenField>
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
                                            <asp:DataList ID="dlDescription" runat="server" CellPadding="1" EnableViewState="True"
                                                RepeatColumns="1" RepeatDirection="Horizontal" DataSource='<%#LoadSubmissionFileList(Eval("SC_ID"))%>'>
                                                <ItemTemplate>
                                                    <%--<a style="text-decoration: underline;" href="javascript:void(0);" onclick="javascript:window.open('../DownloadFile3.aspx?FileInformation=<%#getFileName(Eval("SF_SERVER_FILE_NAME"))%>','','location=0,status=0,scrollbars=0,width=400,height=200');">
                                                        <%#Eval("SF_FILE_NAME")%>
                            </a>--%>

                                                    <a id="Filelink" href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=SubmissionFiles&downloadFileName=<%# Eval("SF_SERVER_FILE_NAME")%>&fileName=<%#getFileName(Eval("SF_FILE_NAME"))%>','','location=0,status=0,scrollbars=0,width=20,height=20');">
                                                        <%#Eval("SF_FILE_NAME")%>
                                                    </a>

                                                </ItemTemplate>
                                            </asp:DataList>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Hygiene check">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlHygiene" runat="server" Width="100" CssClass="form-select">
                                                <asp:ListItem Text="-Select-" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                <asp:ListItem Text="NA" Value="NA"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hfSUB_HYGIENE_CHECK" runat="server" Value='<%# Eval("SUB_HYGIENE_CHECK") %>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<<Added by Ashish Mishra on 16Aug2017--%>
                                    <asp:TemplateField HeaderText="Mode of Filing">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlModeOfFiling" runat="server" Width="150" DataTextField="RC_NAME" DataValueField="RC_CODE"
                                                CssClass="form-select">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hfSUB_MODE_OF_FILING" runat="server" Value='<%# Eval("SUB_MODE_OF_FILING") %>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-->>--%>
                                    <asp:TemplateField HeaderText="Submitted to Authority On / Date of Receipt of Data">
                                        <ItemTemplate>
                                            <div class="input-group" style="width: 150px">
                                                <F2FControls:F2FTextBox ID="txtSubAuthorityDate" runat="server" Columns="15" MaxLength="11"
                                                    CssClass="form-control" Text='<%# Bind("SUB_SUBMITTED_TO_AUTHORITY_ON", "{0:dd-MMM-yyyy}") %>'></F2FControls:F2FTextBox>
                                                <asp:ImageButton ID="imgSubDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                                    ImageUrl="../../Content/images/legacy/calendar.jpg" OnClientClick="return false" />
                                            </div>
                                            <ajaxToolkit:CalendarExtender ID="ceSubDate" runat="server" PopupButtonID="imgSubDate"
                                                TargetControlID="txtSubAuthorityDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="revSubDate" runat="server" ControlToValidate="txtSubAuthorityDate" CssClass="text-danger"
                                                ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                                Display="Dynamic"></asp:RegularExpressionValidator>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-->>--%>
                                    <%--<<Added by Ashish Mishra on 28Jul2017--%>
                                    <asp:TemplateField HeaderText="Previous Closure Remarks">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReOpenComments" Rows="4" Width="200" CssClass="form-control"
                                                Text='<%#Eval("SUB_CLOSURE_COMMENTS").ToString().Replace("\n","<br />") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Closure Remarks">
                                        <ItemTemplate>
                                            <F2FControls:F2FTextBox ID="txtReOpenComments" TextMode="MultiLine" Rows="4" Width="200" CssClass="form-control"
                                                runat="server"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtReOpenComments" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-->>--%>
                                    <asp:TemplateField HeaderText="Change Status">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="150" CssClass="form-select" AppendDataBoundItems="true">
                                                <asp:ListItem Value="" Text="(None)" />
                                                <%--<asp:ListItem Value="P" Text="Pending" />--%>
                                                <asp:ListItem Value="SR" Text="Send Back to Reporting Department" />
                                                <asp:ListItem Value="S" Text="Submitted" />
                                                <%--<<Added by Ashish Mishra on 28Jul2017--%>
                                                <asp:ListItem Value="C" Text="Closed" />
                                                <%-->>--%>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbSave" runat="server" Text="Save" CssClass="btn btn-outline-success" CommandName="Select" OnClientClick="return Validate(this);">
                                                Save
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
