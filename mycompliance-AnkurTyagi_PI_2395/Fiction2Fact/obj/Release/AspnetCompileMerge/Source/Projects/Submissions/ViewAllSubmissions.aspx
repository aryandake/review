<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Submissions.Submissions_ViewAllSubmissions" Title="Department Checklist" CodeBehind="ViewAllSubmissions.aspx.cs" %>

<%--Added BY Urvashi Gupta On 28Apr2016 --%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="a" Namespace="Fiction2Fact.Controls" Assembly="RequiredIfValidatorRadioButtonList" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--Added BY Urvashi Gupta On 28Apr2016 --%>

    <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/dateformatvalidation.js") %>'>
    </script>

    <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/DateValidator.js") %>'>
    </script>

    <script type="text/javascript">

        function openSubmissionFilesWindow(scId, operationType, Type) {
            window.open('../Submissions/SubmissionCheckListFiles.aspx?SCId=' + scId + '&OperationType=' + operationType + '&Type=' + Type, '', 'location=0,status=0,scrollbars=1,width=900,height=600');
            return false;
        }

        function openpopupAttachments(rowNo) {
            window.open("../UploadChecklistFile.aspx?rowNo=" + rowNo, "FILE",
                "location=0,status=0,scrollbars=0,width=400,height=100");
            return false;
        }

        function deleteAttachments(rowNo) {
            var filename = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklistDetails_ctl0" + rowNo + "_ServerFileName").value;
            window.open(
                "../DeleteChecklistFile.aspx?calledFrom=Checklist&rowNo="
                + rowNo + "&filename=" + filename,
                "FILE", "location=0,status=0,scrollbars=0,width=10,height=10");
            return false;
        }

        //    function showHideButtons(boolVisible, index) {
        //	    try
        //	    {
        //	        index = parseInt(index) + 2;
        //	        if(boolVisible == 'true'){
        //	            document.getElementById('AttachFileImg'+index).style.visibility = 'visible';
        //                document.getElementById('DeleteFileImg'+index).style.visibility = 'hidden';
        //	        }
        //	        else{
        //	            document.getElementById('AttachFileImg'+index).style.visibility = 'hidden';
        //                document.getElementById('DeleteFileImg'+index).style.visibility = 'visible';
        //	        }
        //            
        //	    } catch (e) {
        //		    alert(e.message);
        //	    }
        //    }
        //    function hideButtons(index) {
        //        index = parseInt(index) + 2;
        //        document.getElementById('AttachFileImg'+index).style.visibility = 'hidden';
        //        document.getElementById('DeleteFileImg'+index).style.visibility = 'hidden';
        //    }

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
            var ValidationGroup = document.getElementById(controlIdPrefix + rowNo + '_hfValidtionGroup').value;
            var validated = Page_ClientValidate(ValidationGroup);
            if (validated) {
                var IsDoubleClickFlagSet = document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value;
                if (IsDoubleClickFlagSet == 'Yes') {
                    alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                    return false;
                }
                else {
                    document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Save Draft";
                    document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                }
            }
            //>>
        }

        function onClientSubmitClick(lnkbtn) {
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
            var ValidationGroup = document.getElementById(controlIdPrefix + rowNo + '_hfValidtionGroup').value;
            var validated = Page_ClientValidate(ValidationGroup);
            if (validated) {
                var IsDoubleClickFlagSet = document.getElementById('<%=hfDoubleClickFlagSubmit.ClientID%>').value;
                if (IsDoubleClickFlagSet == 'Yes') {
                    alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                    return false;
                }
                else {
                    document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Submit";
                    document.getElementById('<%=hfDoubleClickFlagSubmit.ClientID%>').value = "Yes";
                }
            }
            //>>
        }
        //    function openSubmissionFilesWindow(scId)
        //    {
        //        window.open('./SubmissionCheckListFiles.aspx?SCId='+scId,'','location=0,status=0,scrollbars=1,width=600,height=400');
        //        return false;
        //    }

        //Added By Urvashi Gupta On 28Apr2016
        function compareStartDateSystemDate(source, arguments) {
            try {
                var Startdate = document.getElementById(source.Subdate);
                if (Startdate == '') {
                    arguments.IsValid = true;
                }

                var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurDate');
                if (Startdate != 'undefined') {
                    var retVal = compare2Dates(SystemDate, Startdate);
                    if (retVal < 2) {
                        arguments.IsValid = true;
                    }
                    else {
                        arguments.IsValid = false;
                    }
                }
            }
            catch (e) {
                alert(e);
                arguments.IsValid = false;
            }
        }


    </script>

    <br />
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
                        <h4 class="page-title">View Past Submissions</h4>
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
                        <div class="col-md-6 mb-3">
                            <label class="form-label">Financial Year:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlFinYear" runat="server"
                                DataValueField="FYM_ID" DataTextField="FYM_FIN_YEAR">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvFinYear" ControlToValidate="ddlFinYear" CssClass="text-danger" Display="Dynamic" SetFocusOnError="True" ErrorMessage="*" ValidationGroup="Months">Please select Financial year
                            </asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-6 mb-3">
                            <label class="form-label">Global search:</label>
                            <asp:TextBox CssClass="form-control" ID="txtGlobalSearch" runat="server"></asp:TextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtGlobalSearch" />
                        </div>
                        <div class="col-md-12 mb-3">
                            <asp:LinkButton ID="lbtnApr" runat="server" Text="Apr" CssClass="btn btn-sm btn-outline-primary" CausesValidation="true" ValidationGroup="Months"
                                OnClick="lbtnApr_Click" />
                            <asp:LinkButton ID="lbtnMay" runat="server" Text="May" CssClass="btn btn-sm btn-outline-primary" ValidationGroup="Months" OnClick="lbtnMay_Click" />
                            <asp:LinkButton ID="lbtnJune" runat="server" Text="June" CssClass="btn btn-sm btn-outline-primary" ValidationGroup="Months"
                                OnClick="lbtnJune_Click" />
                            <asp:LinkButton ID="lbtnJuly" runat="server" Text="July" CssClass="btn btn-sm btn-outline-primary" ValidationGroup="Months"
                                OnClick="lbtnJuly_Click" />
                            <asp:LinkButton ID="lbtnAug" runat="server" Text="Aug" CssClass="btn btn-sm btn-outline-primary" ValidationGroup="Months" OnClick="lbtnAug_Click" />
                            <asp:LinkButton ID="lbtnSep" runat="server" Text="Sep" CssClass="btn btn-sm btn-outline-primary" ValidationGroup="Months" OnClick="lbtnSep_Click" />
                            <asp:LinkButton ID="lbtnOct" runat="server" Text="Oct" CssClass="btn btn-sm btn-outline-primary" ValidationGroup="Months" OnClick="lbtnOct_Click" />
                            <asp:LinkButton ID="lbtnNov" runat="server" Text="Nov" CssClass="btn btn-sm btn-outline-primary" ValidationGroup="Months" OnClick="lbtnNov_Click" />
                            <asp:LinkButton ID="lbtnDec" runat="server" Text="Dec" CssClass="btn btn-sm btn-outline-primary" ValidationGroup="Months" OnClick="lbtnDec_Click" />
                            <asp:LinkButton ID="lbtnJan" runat="server" Text="Jan" CssClass="btn btn-sm btn-outline-primary" ValidationGroup="Months" OnClick="lbtnJan_Click" />
                            <asp:LinkButton ID="lbtnFeb" runat="server" Text="Feb" CssClass="btn btn-sm btn-outline-primary" ValidationGroup="Months" OnClick="lbtnFeb_Click" />
                            <asp:LinkButton ID="lbtnMarch" runat="server" Text="Mar" CssClass="btn btn-sm btn-outline-primary" ValidationGroup="Months"
                                OnClick="lbtnMarch_Click" />
                        </div>
                        <div class="col-md-12 mb-3">
                            <asp:LinkButton ID="btnExportToExcel" runat="server" Text="Export to Excel" CssClass="btn btn-outline-secondary" OnClick="btnExportToExcel_Click">
                            <i class="fa fa-download"></i> Export to Excel               
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvChecklistDetails" AllowPaging="False" runat="server" AutoGenerateColumns="False"
                            DataKeyNames="SC_ID" GridLines="both" AllowSorting="false" CssClass="table table-bordered footable"
                            CellPadding="4">
                            <Columns>
                                <asp:TemplateField HeaderText="Submission ID" SortExpression="SUB_ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubId" Text='<%#Eval("SUB_ID")%>' runat="server"></asp:Label>
                                        <asp:Label ID="lbStmId" Text='<%#Eval("SC_STM_ID")%>' runat="server" Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sr No.">
                                    <HeaderTemplate>
                                        Sr No.
               
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfValidtionGroup" runat="server" />
                                        <asp:Label ID="lblSrNo" Text='<%# Container.DataItemIndex + 1 %>' runat="server"> 
                                        </asp:Label>.
               
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Department" SortExpression="STM_TYPE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstmType" Text='<%#Eval("STM_TYPE")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reporting Department" SortExpression="STM_TYPE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReportFun" Text='<%#Eval("SRD_NAME")%>' runat="server"></asp:Label>
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
                                <asp:TemplateField HeaderText="Yes/No/NA">
                                    <ItemTemplate>
                                        <asp:Label ID="lblYesNoNa" Text='<%#Eval("YesNoNA")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" Text='<%#Eval("Status")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Submitted By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubmitby" Text='<%#Eval("SUB_SUBMITTED_BY")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Submitted On" SortExpression="SUB_SUBMIT_DATE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubmittedOn" Text='<%#Eval("SUB_SUBMIT_DATE","{0:dd-MMM-yyyy HH:mm:ss}")%>'
                                            runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" Text='<%#Eval("SUB_REMARKS").ToString().Replace("\n","<br />")%>'
                                            runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Closed By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosedBy" Text='<%#Eval("SUB_CLOSED_BY")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Closed On" SortExpression="SUB_CLOSED_ON">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosedOn" Text='<%#Eval("SUB_CLOSED_ON","{0:dd-MMM-yyyy HH:mm:ss}")%>'
                                            runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Closure Remarks" SortExpression="SUB_CLOSURE_COMMENTS">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSUB_CLOSURE_COMMENTS" Text='<%#Eval("SUB_CLOSURE_COMMENTS").ToString().Replace("\n","<br />") %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reopened By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReopenedBy" Text='<%#Eval("SUB_REOPENED_BY")%>' runat="server"></asp:Label>
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
                                <asp:TemplateField HeaderText="Submitted to Authority On / Date of Receipt of Data"
                                    SortExpression="SUB_SUBMITTED_TO_AUTHORITY_ON">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubAuthorityDate" Text='<%#Eval("SUB_SUBMITTED_TO_AUTHORITY_ON","{0:dd-MMM-yyyy}")%>'
                                            runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File uploaded by Reporting Department">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="ClientFileName" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="ServerFileName" runat="server" Value='<%# Bind("SUB_SERVER_FILE_NAME") %>'></asp:HiddenField>
                                        <%--<a id="AttachFileImg<%# Container.DataItemIndex + 2  %>" onclick="return openpopupAttachments('<%# Container.DataItemIndex + 2  %>')">
                        <img border="0" alt="" src="../../Content/images/legacy/attach.png" /></a> <a id="DeleteFileImg<%# Container.DataItemIndex + 2  %>"
                            onclick="return deleteAttachments('<%# Container.DataItemIndex + 2  %>','<%# Eval("SUB_SC_ID") %>')">
                            <img border="0" alt="" src="../../Content/images/legacy/delete.gif" /></a> --%>
                                        <a id="Filelink<%# Container.DataItemIndex + 2  %>" href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=ChecklistFilesFolder&downloadFileName=<%# Eval("SUB_SERVER_FILE_NAME")%>&fileName=<%#getFileName(Eval("SUB_CLIENT_FILE_NAME"))%>','','location=0,status=0,scrollbars=0,width=400,height=200');">
                                            <%#Eval("SUB_CLIENT_FILE_NAME")%>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Uploaded by Tracking Department" Visible="false">
                                    <ItemTemplate>
                                        <asp:DataList ID="dlDescription" runat="server" CellPadding="1" EnableViewState="True"
                                            RepeatColumns="1" RepeatDirection="Horizontal" DataSource='<%#LoadSubmissionFileList(Eval("SC_ID"))%>'>
                                            <ItemTemplate>
                                                <a style="text-decoration: underline;" href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=SubmissionFiles&downloadFileName=<%#getFileName(Eval("SF_SERVER_FILE_NAME"))%>&fileName=<%#getFileName(Eval("SF_FILE_NAME"))%>','','location=0,status=0,scrollbars=0,width=400,height=200');">
                                                    <%#Eval("SF_FILE_NAME")%>
                                                </a>

                                            </ItemTemplate>
                                        </asp:DataList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Attach Files">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CssClass="btn btn-outline-primary" Text="Attach" ID="lbAttach"
                                            OnClientClick='<%# string.Format("return openSubmissionFilesWindow(\"{0}\",\"{1}\",\"{2}\");", Eval("SC_ID"),"Submission","View") %>'>
                                                View
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="lbSave" runat="server" CommandName="Select" Text="Save Draft"
                        OnClientClick="return onClientSaveClick(this)">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="lbSubmit" runat="server" CommandName="Select" Text="Submit" OnClientClick="return onClientSubmitClick(this)">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->

</asp:Content>
