<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Submissions.Submissions_DepartmentApproval" Title="List of Submissions"
    Async="true" CodeBehind="DepartmentApproval.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="a" Namespace="Fiction2Fact.Controls" Assembly="RequiredIfValidatorRadioButtonList" %>
<%@ Register TagPrefix="b" Namespace="Fiction2Fact.Controls" Assembly="RequiredIfValidator" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/dateformatvalidation.js") %>'></script>

    <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/DateValidator.js") %>'></script>

    <script type="text/javascript">
        function openpopupAttachments(rowNo) {
            window.open("../UploadChecklistFile.aspx?rowNo=" + rowNo, "FILE",
                "location=0,status=0,scrollbars=0,width=450,height=320");
            return false;
        }

        function openSubmissionFilesWindow(scId, operationType, Type) {
            window.open('../Submissions/SubmissionCheckListFiles.aspx?SCId=' + scId + '&OperationType=' + operationType + '&Type=' + Type, '', 'location=0,status=0,scrollbars=1,width=900,height=600');
            return false;
        }

        function deleteAttachments(rowNo, subId) {
            var controlIdPrefix;
            if (rowNo >= 10) {
                controlIdPrefix = 'ctl00_ContentPlaceHolder1_gvChecklistDetails_ctl';
            }
            else {
                controlIdPrefix = 'ctl00_ContentPlaceHolder1_gvChecklistDetails_ctl0';
            }

            var filename = document.getElementById(controlIdPrefix + rowNo + '_ServerFileName').value;

            if (!confirm('Are you sure that you want to delete this record?')) return false;
            window.open(
                "../DeleteChecklistFile.aspx?calledFrom=Checklist&rowNo="
                + rowNo + "&filename=" + filename + "&SUBId=" + subId,
                "FILE", "location=0,status=0,scrollbars=0,width=10,height=10");
            return false;
        }

        function onClientSaveClick(lnkbtn) {
            //<< Modified By Vivek on 22-Jun-2017
            var row = lnkbtn.parentNode.parentNode;
            var rowNo = row.rowIndex + 1;
            var controlIdPrefix;
            if (rowNo >= 10) {
                controlIdPrefix = 'ctl00_ContentPlaceHolder1_gvChecklistDetails_ctl';
            }
            else {
                controlIdPrefix = 'ctl00_ContentPlaceHolder1_gvChecklistDetails_ctl0';
            }
            var ddlYesNoNA = $(lnkbtn).closest('tr').find('[id*="ddlYesNoNA"]');
            var txtRemark = $(lnkbtn).closest('tr').find('[id*="txtRemarks"]');
            var isRemark = true;
            if (($(ddlYesNoNA).val() != 'NA' && $(txtRemark).val() == '')) {
                isRemark = false;
                alert('Please Enter Remark');
                $(txtRemark).focus();
            }
            var ValidationGroup = document.getElementById(controlIdPrefix + rowNo + '_hfValidationGroup').value;
            var validated = Page_ClientValidate(ValidationGroup);
            if (validated && isRemark) {
                var IsDoubleClickFlagSet = document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value;
                if (IsDoubleClickFlagSet == 'Yes') {
                    alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                    return false;
                }
                document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Save Draft";
                document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                return true;
            }
            else {
                return false;
            }
        }
        //>>

        function onClientSubmitClick(lnkbtn) {
            //<< Modified By Vivek on 22-Jun-2017
            var row = lnkbtn.parentNode.parentNode;
            var rowNo = row.rowIndex + 1;
            var controlIdPrefix;
            var clientFilename;

            if (rowNo >= 10) {
                controlIdPrefix = 'ctl00_ContentPlaceHolder1_gvChecklistDetails_ctl';
            }
            else {
                controlIdPrefix = 'ctl00_ContentPlaceHolder1_gvChecklistDetails_ctl0';
            }
            var ddlYesNoNA = $(lnkbtn).closest('tr').find('[id*="ddlYesNoNA"]');
            var txtRemark = $(lnkbtn).closest('tr').find('[id*="txtRemarks"]');

            var isRemark = true;
            if ($(ddlYesNoNA).val() != '') {
                if (($(ddlYesNoNA).val() != 'NA' && $(txtRemark).val() == '')) {
                    isRemark = false;
                    alert('Please Enter Remark');
                    $(txtRemark).focus();
                }
            }
            else {
                isRemark = false;
                alert('Please select Yes/No/NA');
                $(ddlYesNoNA).focus();
            }

            var ValidationGroup = document.getElementById(controlIdPrefix + rowNo + '_hfValidationGroup').value;
            var validated = Page_ClientValidate(ValidationGroup);
            if (validated && isRemark) {
                var IsDoubleClickFlagSet = document.getElementById('<%=hfDoubleClickFlagSubmit.ClientID%>').value;
                if (IsDoubleClickFlagSet == 'Yes') {
                    alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                    return false;
                }
                document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Submit";
                document.getElementById('<%=hfDoubleClickFlagSubmit.ClientID%>').value = "Yes";
                return true;
            }
            else {
                return false;
            }

            //>>
        }

        //Added By Urvashi Gupta On 28Apr2016
        //    function compareStartDateSystemDate(source,arguments)
        //    {
        //        try
        //        {  
        //            var Startdate = document.getElementById(source.Subdate);
        //            if (Startdate == '')
        //            {
        //                arguments.IsValid = true;
        //            }   
        //            
        //            var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurDate');
        //            if(Startdate != 'undefined')
        //            {
        //                var retVal = compare2Dates(SystemDate,Startdate);
        //                if (retVal < 2)  
        //                {
        //                    arguments.IsValid = true;
        //                }
        //                else
        //                {
        //                    arguments.IsValid = false;
        //                }
        //            }
        //        }
        //        catch (e)
        //        {
        //            alert(e);
        //            arguments.IsValid = false;
        //        }
        //    }

        function onClientViewCircClick(CMId) {
            window.open('../Circulars/ViewCircularDetails.aspx?CircularId=' + CMId, '', 'location=0,status=0,resizable=1,scrollbars=1,width=1000,height=800');
            return false;
        }
    </script>
    <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />
    <asp:HiddenField ID="hfDoubleClickFlagSubmit" runat="server" />
    <asp:HiddenField ID="hfSelectedOperation" runat="server" />
    <asp:HiddenField ID="hfSelectedRecord" runat="server" />
    <asp:HiddenField ID="hfMonth" runat="server" />
    <asp:HiddenField ID="hfRole" runat="server" />
    <asp:HiddenField ID="hfCurDate" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">
                            <asp:Label ID="lbSelectedMonth" runat="server" Text="List of Submissions"></asp:Label></h4>
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
                            <label class="form-label">Financial Year:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlFinYear" runat="server"
                                DataValueField="FYM_ID" DataTextField="FYM_FIN_YEAR">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvFinYear" ControlToValidate="ddlFinYear" CssClass="span" Display="Dynamic" SetFocusOnError="True" ErrorMessage="*" ValidationGroup="Months">Please select Financial year
                            </asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-4 mb-3" id="divReportingDept" runat="server">
                            <label class="form-label">Reporting Department:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlReportDept" AppendDataBoundItems="true"
                                runat="server" DataValueField="SRD_ID" DataTextField="SRD_NAME">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 mb-3" id="div1" runat="server">
                            <label class="form-label">Frequency:</label>
                            <asp:DropDownList ID="ddlFrequency" runat="server" CssClass="form-select">
                                <asp:ListItem Value="">All</asp:ListItem>
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
                    </div>
                    <div class="row" id="divStatus" runat="server" visible="false">
                        <div class="col-md-12 mb-3">
                            <label class="form-label">Status:</label>
                            <div class="custom-checkbox-table">
                                <asp:CheckBoxList ID="chkStatus" CssClass="form-control" AppendDataBoundItems="true"
                                    runat="server" RepeatColumns="5">
                                    <asp:ListItem Value="C" Text="Closed" />
                                    <asp:ListItem Value="P" Text="Pending" />
                                    <asp:ListItem Value="SPDA" Text="Pending for Department Approval" />
                                    <asp:ListItem Value="R" Text="Reopened" />
                                    <%--<<Modified by Ankur Tyagi on 18-Apr-2025 for Project Id : 2395
                                    Changed Reporting function to tracking function--%>
                                    <asp:ListItem Value="SR" Text="Sent back by tracking function" />
                                    <%-->>--%>
                                    <asp:ListItem Value="S" Text="Submitted" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <%--<<Added by Ankur Tyagi on 18-Apr-2025 for Project Id : 2395--%>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Select Reporting to:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlSegment" runat="server" AppendDataBoundItems="true"
                                DataTextField="SSM_NAME" DataValueField="SSM_ID">
                                <asp:ListItem Value="">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-8 mb-3">
                            <label class="form-label">Global search:</label>
                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtGlobalSearch" runat="server"></F2FControls:F2FTextBox>
                        </div>
                        <%-->>--%>
                    </div>
                    <div class="mt-3">
                        <asp:LinkButton ID="lbtnApr" runat="server" Text="Apr" CssClass="btn btn-sm btn-outline-primary" OnClick="lbtnApr_Click" CausesValidation="true"
                            ValidationGroup="Months" />
                        <asp:LinkButton ID="lbtnMay" runat="server" Text="May" CssClass="btn btn-sm btn-outline-primary" OnClick="lbtnMay_Click" ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnJune" runat="server" Text="June" CssClass="btn btn-sm btn-outline-primary" OnClick="lbtnJune_Click"
                            ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnJuly" runat="server" Text="July" CssClass="btn btn-sm btn-outline-primary" OnClick="lbtnJuly_Click"
                            ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnAug" runat="server" Text="Aug" CssClass="btn btn-sm btn-outline-primary" OnClick="lbtnAug_Click" ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnSep" runat="server" Text="Sep" CssClass="btn btn-sm btn-outline-primary" OnClick="lbtnSep_Click" ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnOct" runat="server" Text="Oct" CssClass="btn btn-sm btn-outline-primary" OnClick="lbtnOct_Click" ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnNov" runat="server" Text="Nov" CssClass="btn btn-sm btn-outline-primary" OnClick="lbtnNov_Click" ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnDec" runat="server" Text="Dec" CssClass="btn btn-sm btn-outline-primary" OnClick="lbtnDec_Click" ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnJan" runat="server" Text="Jan" CssClass="btn btn-sm btn-outline-primary" OnClick="lbtnJan_Click" ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnFeb" runat="server" Text="Feb" CssClass="btn btn-sm btn-outline-primary" OnClick="lbtnFeb_Click" ValidationGroup="Months" />

                        <asp:LinkButton ID="lbtnMarch" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="Mar" OnClick="lbtnMarch_Click"
                            ValidationGroup="Months" />
                    </div>
                    <div class="mt-3">
                        <asp:LinkButton ID="btnExportToExcel" runat="server" CssClass="btn btn-outline-secondary" Text="Export to Excel" OnClick="btnExportToExcel_Click">
                            <i class="fa fa-download"></i> Export to Excel               
                        </asp:LinkButton>
                    </div>
                    <div class="mt-3">
                        <div class="table-responsive">
                            <asp:GridView ID="gvChecklistDetails" EmptyDataText="Checklist Not Available for Selected Month."
                                CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                EmptyDataRowStyle-ForeColor="red" AllowPaging="False" runat="server" AutoGenerateColumns="False"
                                DataKeyNames="SC_ID" GridLines="None" AllowSorting="true" CellPadding="4" OnSelectedIndexChanged="gvChecklistDetails_SelectedIndexChanged"
                                OnRowDataBound="gvChecklistDetails_RowDataBound" OnSorting="gvChecklistDetails_Sorting"
                                OnRowCreated="OnRowCreated">
                                <Columns>
                                    <asp:TemplateField HeaderText="Submission ID" SortExpression="SUB_ID">
                                        <ItemTemplate>
                                            <%--<<Added by Ankur Tyagi on 18-Apr-2025 for Project Id : 2395--%>
                                            <asp:Label ID="lblSubId" Text='<%#Eval("SUB_ID")%>' runat="server"></asp:Label>
                                            <%-->>--%>
                                            <asp:Label ID="lbStmId" Text='<%#Eval("SC_STM_ID")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblSrdId" Text='<%#Eval("SRD_ID")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblIsFSAppReq" Text='<%#Eval("SM_IS_FS_APPROVAL_REQUIRED")%>' runat="server" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sr No.">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfValidationGroup" runat="server" />
                                            <asp:Label ID="lblSrNo" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>.
                                       
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="View Circulars">
                                        <ItemTemplate>
                                            <center>
                                                <asp:LinkButton ID="lnkViewCirc" runat="server" CssClass="btn btn-sm btn-soft-info btn-circle"
                                                    CommandName="Select">
                                                    <%--OnClientClick='<%# string.Format("onClientViewCircClick(\"{0}\");", Eval("SM_CM_ID")) %>'--%>
                                                    <i class="fa fa-eye"></i>
                                                </asp:LinkButton>
                                                <asp:HiddenField ID="hfCircularId" runat="server" Value='<%#Eval("SM_CM_ID") %>' />
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Relevant Files">
                                        <ItemTemplate>
                                            <a href="javascript:void(0);" class="btn btn-sm btn-soft-info btn-circle" onclick="javascript:window.open('../Submissions/SubmissionDocuments.aspx?Id=<%# Eval("SC_SM_ID") %>','','location=0,status=0,scrollbars=1,width=450,height=250,resizable=1');">
                                                <i class="fa fa-eye"></i>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tracking Function" SortExpression="STM_TYPE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstmType" Text='<%#Eval("STM_TYPE")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reporting Department" SortExpression="SRD_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReportFun" Text='<%#Eval("SRD_NAME")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<<Modified by Ankur Tyagi on 29-Apr-2025 for Project Id : 2395--%>
                                    <asp:TemplateField HeaderText="Reporting to" SortExpression="SSM_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSegment" Text='<%#Eval("SSM_NAME")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-->>--%>
                                    <asp:TemplateField HeaderText="Event" SortExpression="EM_EVENT_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEvent" Text='<%#Eval("EM_EVENT_NAME")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Agenda" SortExpression="EP_NAME">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblAgenda" Text='<%#Eval("EP_NAME")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Event date" SortExpression="EI_EVENT_DATE">
                                        <ItemTemplate>
                                            <asp:Label ID="lbleventFrom" Text='<%#Eval("EI_EVENT_DATE","{0:dd-MMM-yyyy}")%>'
                                                runat="server"></asp:Label>
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
                                            <%--<asp:Label ID="lblSection" runat="server" Text='<%#Eval("SM_SECTION_CLAUSE").ToString().Replace("\n","<br />") %>'></asp:Label>--%>
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
                                                    + "..." : Eval("SC_PARTICULARS").ToString().Replace("\n", "<br />") %>'></asp:Label>
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
                                            <asp:Label ID="lblDueDateFrom" Text='<%#Eval("SC_DUE_DATE_FROM","{0:dd-MMM-yyyy}")%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Regulatory due date" SortExpression="SC_DUE_DATE_TO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDueDateTo" Text='<%#Eval("SC_DUE_DATE_TO","{0:dd-MMM-yyyy}")%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submitted By" SortExpression="SUB_SUBMITTED_BY">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubmittedby" Text='<%# Getfullname(Eval("SUB_SUBMITTED_BY").ToString())%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submitted On" SortExpression="SUB_SUBMIT_DATE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubmittedOn" Text='<%#Eval("SUB_SUBMIT_DATE","{0:dd-MMM-yyyy HH:mm:ss}")%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- >>>--%>
                                    <asp:TemplateField HeaderText="Yes/No/NA">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlYesNoNA" runat="server" Width="150" CssClass="form-select">
                                                <asp:ListItem Text="-Select-" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                <asp:ListItem Text="NA" Value="NA"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvYesNoNA" ControlToValidate="ddlYesNoNA"
                                                Display="Dynamic" ErrorMessage="Please select Yes/No/NA." CssClass="text-danger" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks">
                                        <ItemTemplate>
                                            <F2FControls:F2FTextBox CssClass="form-control" Width="180" runat="server" ID="txtRemarks" MaxLength="10" TextMode="MultiLine"
                                                Text='<%# Bind("SUB_REMARKS") %>' Rows="3" Columns="20" />
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                           </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Attach/View Supporting">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="ClientFileName" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="ServerFileName" runat="server" Value='<%# Bind("SUB_SERVER_FILE_NAME") %>'></asp:HiddenField>
                                            <asp:LinkButton runat="server" CssClass="btn btn-outline-primary" Text="Attach" ID="lbAttach">
                                                Attach
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<<Added by Ashish Mishra on 28Aug2017--%>
                                    <asp:TemplateField HeaderText="Submitted to Authority On / Date of Receipt of Data" SortExpression="SUB_SUBMITTED_TO_AUTHORITY_ON">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubAuthorityDate" Text='<%#Eval("SUB_SUBMITTED_TO_AUTHORITY_ON","{0:dd-MMM-yyyy}")%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="File Uploaded By Tracking Department" Visible="false">
                                        <ItemTemplate>
                                            <asp:DataList ID="dlDescription" runat="server" CssClass="custom-datalist-border" CellPadding="1" EnableViewState="True"
                                                RepeatColumns="1" RepeatDirection="Horizontal" DataSource='<%#LoadSubmissionFileList(Eval("SC_ID"))%>'>
                                                <ItemTemplate>
                                                    <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=SubmissionFiles&downloadFileName=<%#getFileName(Eval("SF_SERVER_FILE_NAME"))%>&Filename=<%#getFileName(Eval("SF_FILE_NAME"))%>','','location=0,status=0,scrollbars=1,width=400,height=200');">
                                                        <%#Eval("SF_FILE_NAME")%></a>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Closed By" SortExpression="SUB_CLOSED_BY">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClosedby" Text='<%# Getfullname(Eval("SUB_CLOSED_BY").ToString())%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Closed On" SortExpression="SUB_CLOSED_ON">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClosedOn" Text='<%#Eval("SUB_CLOSED_ON","{0:dd-MMM-yyyy HH:mm:ss}")%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reopened By" SortExpression="SUB_REOPENED_BY">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReopenedBy" Text='<%# Getfullname(Eval("SUB_REOPENED_BY").ToString())%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reopened On" SortExpression="SUB_REOPENED_ON">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReopenedOn" Text='<%#Eval("SUB_REOPENED_ON","{0:dd-MMM-yyyy HH:mm:ss}")%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Reopen due to">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReopenDueTo" runat="server" Text='<%#Eval("ReOpenPurpose")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Reopen Remarks" SortExpression="SUB_REOPEN_COMMENTS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReOpenComments" Text='<%#Eval("SUB_REOPEN_COMMENTS").ToString().Replace("\n","<br />") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mode of Filing" SortExpression="RC_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblModeOfFiling" Text='<%#Eval("RC_NAME")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-->>--%>
                                    <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" Text='<%#Eval("Status")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbSubmit" runat="server" CssClass="btn btn-outline-success" Width="120" CommandName="Select" Text="Submit" OnClientClick="return onClientSubmitClick(this)">
                                                <i class="fa fa-save me-2"></i>  Approve
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
