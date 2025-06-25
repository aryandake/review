<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" CodeBehind="Cert_ClosureTracking.aspx.cs"
    ValidateRequest="false" Async="true" Title="Certifications Closure " Inherits="Fiction2Fact.Projects.Certification.Cert_ClosureTracking" %>

<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js") %>'></script>
    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator1.js") %>'></script>
    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/dateformatvalidation.js")%>'></script>
    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/commonvalidations.js") %>'></script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/tabcontrol.js")%>'>
    </script>

    <%--start - added by Hari on 3 oct 2016--%>

    <script type="text/javascript">

        function openViewChecklistPopup(requestId) {

            //alert(requestId);
            window.open('../Certification/ViewChecklistData.aspx?ChecklistId=' + requestId,
                '', 'location=0,status=0,scrollbars=1,resizable=1,width=700,height=500');
            return false;
        }
        //<<Modified by Ankur Tyagi on 01Feb2024 for CR_1945
        function validateChecklist(index) {
            var ClosureDate;
            var ClosureRemark;
            var errMsg = "", rowNo = '';
            var cnt = 0;

            var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurrDate');
            var grid = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist");

            if (index < 10) {
                rowNo = "0" + index;
            } else {
                rowNo = index;
            }

            //alert(rowNo);

            ClosureDate = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + rowNo + "_txtClosureDate1");
            ClosureRemark = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + rowNo + "_txtClosureRemarks1").value;

            if (ClosureDate.value == '') {
                errMsg = errMsg
                    + 'Please select closure date for Checklist Grid Row ' + (index - 1) + '.\n';
            }
            else {
                if (compare2Dates(ClosureDate, SystemDate) == 0) {
                    errMsg = errMsg
                        + 'Closure date cannot be greater for Checklist Grid Row ' + (index - 1) + '.\n';
                }
                if (ClosureDate.value == "01-Jan-1900") {
                    errMsg = errMsg
                        + 'Please enter correct closure date for Checklist Grid Row ' + (index - 1) + '.\n';
                }
            }
            if (ClosureRemark == '') {
                errMsg = errMsg
                    + 'Please enter closure remarks for Checklist Grid Row ' + (index - 1) + '.\n';
            }

            if (errMsg == '') {
                return true;
            }
            else {
                alert(errMsg);
                return false;
            }
        }

        function validateChecklistException(index) {
            var ClosureDate;
            var ClosureRemark;
            var errMsg = "", rowNo = '';
            var cnt = 0;

            var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurrDate');
            var grid = document.getElementById("ctl00_ContentPlaceHolder1_gvException");

            if (index < 10) {
                rowNo = "0" + index;
            } else {
                rowNo = index;
            }

            //alert(rowNo);

            ClosureDate = document.getElementById("ctl00_ContentPlaceHolder1_gvException_ctl" + rowNo + "_txtClosureDate2");
            ClosureRemark = document.getElementById("ctl00_ContentPlaceHolder1_gvException_ctl" + rowNo + "_txtClosureRemarks2").value;

            if (ClosureDate.value == '') {
                errMsg = errMsg
                    + 'Please select closure date for Checklist Grid Row ' + (index - 1) + '.\n';
            }
            else {
                if (compare2Dates(ClosureDate, SystemDate) == 0) {
                    errMsg = errMsg
                        + 'Closure date cannot be greater for Checklist Grid Row ' + (index - 1) + '.\n';
                }
                if (ClosureDate.value == "01-Jan-1900") {
                    errMsg = errMsg
                        + 'Please enter correct closure date for Checklist Grid Row ' + (index - 1) + '.\n';
                }
            }
            if (ClosureRemark == '') {
                errMsg = errMsg
                    + 'Please enter closure remarks for Checklist Grid Row ' + (index - 1) + '.\n';
            }

            if (errMsg == '') {
                return true;
            }
            else {
                alert(errMsg);
                return false;
            }
        }
        //>>

        function compareDateWithCurrDate(source, arguments) {
            try {
                var cnt = 0;
                var TargetDate;
                var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurrDate');
                //alert('SystemDate: '+SystemDate.value);

                var grid = document.getElementById("ctl00_ContentPlaceHolder1_gvException");
                if (grid != null) {
                    if (grid.rows.length > 0) {
                        for (var j = 2; j < grid.rows.length + 1; j++) {
                            cnt++;
                            if (j < 10) {
                                j = "0" + j;
                            } else {
                                j = j;
                            }
                            TargetDate = document.getElementById("ctl00_ContentPlaceHolder1_gvException_ctl" + j + "_txtClosureDate2");

                            if (TargetDate != null) {
                                //alert(TargetDate.value);
                                //alert(compare2Dates(SystemDate,TargetDate));
                                if (compare2Dates(TargetDate, SystemDate) == 0) {
                                    //alert("False");
                                    arguments.IsValid = false;
                                    break;
                                }
                                else {
                                    //alert("True");
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

    </script>

    <asp:HiddenField runat="server" ID="hfTabberId" />
    <asp:HiddenField runat="server" ID="hfCurrDate" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Certification Checkpoint Closure</h4>
                        <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
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

                <div class="card-body">
                    <div class="row">
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Quarter:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlQuarter" runat="server" DataValueField="CQM_ID"
                                DataTextField="Quarter">
                            </asp:DropDownList>
                            <%--<asp:RequiredFieldValidator ID="rfvQuarter" runat="server" ControlToValidate="ddlQuarter" CssClass="text-danger"
                                ValidationGroup="SEARCH" Display="Dynamic" SetFocusOnError="True"
                                ErrorMessage="Select Quarter.">*</asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Status:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlStatus" runat="server">
                                <asp:ListItem Value="">Select</asp:ListItem>
                                <asp:ListItem>Open</asp:ListItem>
                                <asp:ListItem>Closed</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="text-center mt-3">
                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnSearch" Text="Search" runat="server" ValidationGroup="SEARCH"
                            AccessKey="s" OnClick="btnSearch_Click">
                                    <i class="fa fa-search"></i>Search                          
                               
                        </asp:LinkButton>
                    </div>
                </div>

                <div class="card-body">
                    <div class="mb-3">
                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportCompDeviations" runat="server"
                            Text="Export Compliance Deviations To Excel" Visible="false">
                                    <i class="fa fa-download"></i>Export to Excel               
                               
                        </asp:LinkButton>
                    </div>
                    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                        <ul class="nav nav-tabs" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link active" data-bs-toggle="tab" href="#Exception" role="tab" aria-selected="false">Exception Details</a>
                            </li>
                            <li class="nav-item" style="display: none; visibility: hidden">
                                <a class="nav-link" data-bs-toggle="tab" href="#Checklist" role="tab" aria-selected="true">Checklist Details</a>
                            </li>
                        </ul>
                        <div class="tab-content">
                            <div class="tab-pane p-3" id="Checklist" role="tabpanel">
                                <div class="mb-3">
                                    <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcel" runat="server" Text="Export to Doc"
                                        OnClick="btnExportToExcel_Click">
                                            <i class="fa fa-download"></i>Export to Excel 
                                       
                                    </asp:LinkButton>
                                </div>
                                <div class="mt-3">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvChecklist" runat="server" BorderStyle="None" BorderWidth="1px"
                                            CellPadding="4" GridLines="None" AutoGenerateColumns="false" CssClass="table table-bordered footable"
                                            EmptyDataText="No Record Found..." DataKeyNames="CCD_ID" OnRowDataBound="gvChecklist_RowDataBound"
                                            OnSelectedIndexChanged="gvChecklist_SelectedIndexChanged">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Serial Number
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Department Name" SortExpression="DeptName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDeptName" Text='<%#Eval("DeptName").ToString().Replace("\n", "<br />") %>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reference Circular/Notification/Act" SortExpression="CCM_REFERENCE">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtRegulations" Text='<%#Eval("CCM_REFERENCE").ToString().Length>100?(Eval("CCM_REFERENCE") as string).Substring(0,100).Replace(Environment.NewLine, "<br />") + " ...":Eval("CCM_REFERENCE").ToString().Replace(Environment.NewLine, "<br />")  %>'
                                                            ToolTip='<%#Eval("CCM_REFERENCE")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Section/Clause" SortExpression="CCM_CLAUSE">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkbtntxtSections" runat="server" Text='<%#Eval("CCM_CLAUSE").ToString().Length>100?(Eval("CCM_CLAUSE") as string).Substring(0,100).Replace(Environment.NewLine, "<br />") + " ...":Eval("CCM_CLAUSE").ToString().Replace(Environment.NewLine, "<br />")  %>'
                                                            ToolTip='<%#Eval("CCM_CLAUSE")%>' Font-Underline="true" OnClientClick='<%# string.Format("return openViewChecklistPopup(\"{0}\");", Eval("CCD_ID")) %>'>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Compliance of/Heading of Compliance checklist" SortExpression="CCM_CHECK_POINTS">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtCheckpoints" Text='<%#Eval("CCM_CHECK_POINTS").ToString().Length>100?(Eval("CCM_CHECK_POINTS") as string).Substring(0,100).Replace(Environment.NewLine, "<br />") + " ...":Eval("CCM_CHECK_POINTS").ToString().Replace(Environment.NewLine, "<br />")  %>'
                                                            ToolTip='<%#Eval("CCM_CHECK_POINTS")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" SortExpression="CCM_PARTICULARS">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtDescription" Text='<%#Eval("CCM_PARTICULARS").ToString().Length>100?(Eval("CCM_PARTICULARS") as string).Substring(0,100).Replace(Environment.NewLine, "<br />") + " ...":Eval("CCM_PARTICULARS").ToString().Replace(Environment.NewLine, "<br />")  %>'
                                                            ToolTip='<%#Eval("CCM_PARTICULARS")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Consequences of non Compliance" SortExpression="CCM_PENALTY">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtPenalty" Text='<%#Eval("CCM_PENALTY").ToString().Replace(Environment.NewLine, "<br />") %>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Frequency" SortExpression="CCM_FREQUENCY">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtTimeLimit" Text='<%#Eval("CCM_FREQUENCY").ToString().Length>100?(Eval("CCM_FREQUENCY") as string).Substring(0,100).Replace(Environment.NewLine, "<br />") + " ...":Eval("CCM_FREQUENCY").ToString().Replace(Environment.NewLine, "<br />")  %>'
                                                            ToolTip='<%#Eval("CCM_FREQUENCY")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Compliance Status" DataField="Compliance_Status" />
                                                <asp:TemplateField HeaderText="Reasons for Compliance / Non-Compliance / WIP / Not Applicable" SortExpression="CCD_REMARKS">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtRemarks" Text='<%#Eval("CCD_REMARKS").ToString().Length>100?(Eval("CCD_REMARKS") as string).Substring(0,100).Replace(Environment.NewLine, "<br />") + " ...":Eval("CCD_REMARKS").ToString().Replace(Environment.NewLine, "<br />")  %>'
                                                            ToolTip='<%#Eval("CCD_REMARKS")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Checklist File">
                                                    <ItemTemplate>
                                                        <asp:DataList ID="dlChecklistFiles" BackColor="White" runat="server" CellPadding="1"
                                                            EnableViewState="True" RepeatColumns="1" RepeatDirection="Horizontal" DataSource='<%# LoadChecklistFile(Eval("CCD_ID")) %>'>
                                                            <ItemTemplate>
                                                                <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownloadAnyFile.aspx?type=ChecklistFilesFolder&downloadFileName=<%#Eval("CCD_SERVER_FILENAME")%>&fileName=<%#Eval("CCD_CLIENT_FILENAME")%>','','location=0,status=0,scrollbars=0,width=400,height=200');">
                                                                    <%#Eval("CCD_CLIENT_FILENAME")%>
                                                                </a>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Closure Date">
                                                    <ItemTemplate>
                                                        <div style="width: 160px;">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control" ID="txtClosureDate1" runat="server" Columns="11"
                                                                    Text='<%# String.Format("{0:dd-MMM-yyyy}",Eval("CCD_CLOSURE_DATE")) %>'>
                                                                </asp:TextBox>

                                                                <asp:ImageButton ToolTip="PopUp Calendar" CssClass="custom-calendar-icon" runat="server" ImageUrl="~/Content/images/legacy/calendar.jpg"
                                                                    ID="ImageButton1" OnClientClick="return false" />
                                                            </div>
                                                        </div>
                                                        <asp:RegularExpressionValidator ID="revtxtClosureDate1" runat="server" ControlToValidate="txtClosureDate1"
                                                            ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                                            Display="Dynamic" ValidationGroup="Save" CssClass="text-danger">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:RequiredFieldValidator ID="rfvtxtClosureDate1" runat="server" ControlToValidate="txtClosureDate1"
                                                            Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True" CssClass="text-danger">
                                                                    Please enter Closure Date.</asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvtxtClosureDate1" runat="server" ClientValidationFunction="compareDateWithCurrDate"
                                                            ControlToValidate="txtClosureDate1" ErrorMessage="Date should not be greater than current date."
                                                            Display="Dynamic" ValidationGroup="Save" CssClass="text-danger">
                                                        </asp:CustomValidator>
                                                        <ajaxToolkit:CalendarExtender ID="Calendarextender1" runat="server" PopupButtonID="ImageButton1"
                                                            TargetControlID="txtClosureDate1" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Closure Remarks">
                                                    <ItemTemplate>
                                                        <asp:TextBox CssClass="form-control" ID="txtClosureRemarks1" runat="server" Rows="3" TextMode="MultiLine" Width="120px"
                                                            Text='<%# Eval("CCD_CLOSURE_REMARKS") %>'>
                                                        </asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvtxtClosureRemarks1" runat="server" ControlToValidate="txtClosureRemarks1"
                                                            Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True" CssClass="text-danger">
                                                                Please enter Closure Remarks.</asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClosureStatus" Text='<%# Eval("CCD_CLOSURE_STATUS")  %>' Visible="false"
                                                            runat="server"></asp:Label>
                                                        <asp:LinkButton ID="lbSave" runat="server" CommandName="Select" Text="Save" CssClass="btn btn-outline-success">
                                                        </asp:LinkButton>
                                                        <%--OnClientClick="return validateChecklist()"--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>

                            </div>
                            <div class="tab-pane p-3 active" id="Exception" role="tabpanel">

                                <%-- <center>--%>
                                <div style="text-align: left">
                                    <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcelExceptions" runat="server" Text="Export To Excel"
                                        OnClick="btnExportToExcelExceptions_Click">
                                            <i class="fa fa-download"></i>Export to Excel               
                                           
                                    </asp:LinkButton>
                                </div>
                                <div class="mt-3">
                                    <div class="table-responsive">

                                        <asp:GridView ID="gvException" runat="server" AutoGenerateColumns="False" DataKeyNames="CE_ID"
                                            AllowSorting="true" AllowPaging="false" GridLines="Both" CellPadding="4" CssClass="table table-bordered footable"
                                            EmptyDataText="No Record Found..." PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                            OnRowDataBound="gvException_RowDataBound" OnSelectedIndexChanged="gvException_SelectedIndexChanged">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Serial Number
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Department Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" Text='<%#Eval("DeptName").ToString().Replace("\n", "<br />") %>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Attached File">
                                                    <ItemTemplate>
                                                        <a href="javascript:void(0)" onclick="javascript:window.open('../DownloadFileCertification.aspx?FileInformation=<%#(Eval("CE_SERVER_FILE_NAME"))%>','','location=0,status=0,scrollbars=0,width=10,height=10');">
                                                            <%#Eval("CE_CLIENT_FILE_NAME")%>
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Deviation (Detailed)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtApplicableLaw" Text='<%#Eval("CE_EXCEPTION_TYPE").ToString().Replace("\n", "<br />") %>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Regulatory Reference (Detailed)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtObservations" Text='<%#Eval("CE_DETAILS").ToString().Replace("\n", "<br />") %>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Root Cause for the Deviation">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtRootCause" Text='<%#Eval("CE_ROOT_CAUSE_OF_DEVIATION").ToString().Replace("\n", "<br />") %>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action taken">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtActionTaken" Text='<%#Eval("CE_ACTION_TAKEN").ToString().Replace("\n", "<br />") %>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Target Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtTargetDate" Text='<%#Bind("CE_TARGET_DATE","{0:dd-MMM-yyyy}") %>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Closure Date" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label CssClass="textbox1" ID="lblClosureDate" runat="server" Text='<%# String.Format("{0:dd-MMM-yyyy}",Eval("CE_CLOSURE_DATE")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Closure Remarks" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label CssClass="textbox1" ID="lblClosureRemarks" runat="server" Text='<%# Eval("CE_CLOSURE_REMARKS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Closure Date">
                                                    <ItemTemplate>
                                                        <div style="width: 160px;">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control" ID="txtClosureDate2" runat="server" Columns="11"
                                                                    Text='<%# String.Format("{0:dd-MMM-yyyy}",Eval("CE_CLOSURE_DATE")) %>'>
                                                                </asp:TextBox>
                                                                <asp:ImageButton ToolTip="PopUp Calendar" runat="server" CssClass="custom-calendar-icon" ImageUrl="~/Content/images/legacy/calendar.jpg"
                                                                    ID="ImageButton1" OnClientClick="return false" />
                                                            </div>
                                                        </div>
                                                        <asp:RegularExpressionValidator ID="revtxtClosureDate2" runat="server" ControlToValidate="txtClosureDate2" CssClass="text-danger"
                                                            ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                                            Display="Dynamic" ValidationGroup="Save1">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:RequiredFieldValidator ID="rfvtxtClosureDate2" runat="server" ControlToValidate="txtClosureDate2" CssClass="text-danger"
                                                            Display="Dynamic" ValidationGroup="Save1" SetFocusOnError="True">
                                                                    Please enter Closure Date.</asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvtxtClosureDate2" runat="server" ClientValidationFunction="compareDateWithCurrDate" CssClass="text-danger"
                                                            ControlToValidate="txtClosureDate2" ErrorMessage="Date should not be greater than current date."
                                                            Display="Dynamic" ValidationGroup="Save1">
                                                        </asp:CustomValidator>
                                                        <ajaxToolkit:CalendarExtender ID="Calendarextender2" runat="server" PopupButtonID="ImageButton1"
                                                            TargetControlID="txtClosureDate2" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Closure Remarks">
                                                    <ItemTemplate>
                                                        <asp:TextBox CssClass="form-control" ID="txtClosureRemarks2" runat="server" Rows="3" TextMode="MultiLine" Width="120px"
                                                            Text='<%# Eval("CE_CLOSURE_REMARKS") %>'>
                                                        </asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvtxtClosureRemarks2" runat="server" ControlToValidate="txtClosureRemarks2"
                                                            Display="Dynamic" ValidationGroup="Save1" SetFocusOnError="True">
                                                                Please enter Closure Remarks.</asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClosureStatus2" Text='<%# Eval("CE_CLOSURE_STATUS")  %>' Visible="false"
                                                            runat="server"></asp:Label>
                                                        <asp:LinkButton ID="lbSave2" runat="server" CommandName="Select" Text="Save" CssClass="btn btn-outline-success">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </asp:Panel>
                </div>

            </div>
        </div>
    </div>

</asp:Content>

