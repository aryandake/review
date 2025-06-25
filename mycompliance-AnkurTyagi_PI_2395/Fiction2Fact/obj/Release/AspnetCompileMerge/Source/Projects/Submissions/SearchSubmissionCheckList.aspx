<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" Async="true"
    EnableEventValidation="true" Inherits="Fiction2Fact.Projects.Submissions.Submissions_SearchSubmissionCheckList"
    Title="Submission CheckList" CodeBehind="SearchSubmissionCheckList.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="a" Namespace="Fiction2Fact.Controls" Assembly="RequiredIfValidatorRadioButtonList" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--Added BY Urvashi Gupta On 28Apr2016 --%>

    <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/dateformatvalidation.js") %>'>
    </script>

    <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/DateValidator.js") %>'></script>

    <script type="text/javascript">

        function openpopupAttachments(rowNo) {
            window.open("../UploadChecklistFile.aspx?rowNo=" + rowNo, "FILE",
                "location=0,status=0,scrollbars=0,width=400,height=100");
            return false;
        }

        const onExtensionClick = (SCId) => {

            document.getElementById('ctl00_ContentPlaceHolder1_txtExRegulatoryDate').value = '';
            document.getElementById('ctl00_ContentPlaceHolder1_txtExRemarks').value = '';
            document.getElementById('ctl00_ContentPlaceHolder1_hfSelectedRecord').value = '';

            var intSCId = "";
            try {
                intSCId = SCId;
            } catch (e) {
                intSCId = SCId;
            }

            $('[id$="hfSelectedRecord"]').val(intSCId);
            $("#divExtension").modal('show');
            return false;
        }

        function compareEndSystemDates(source, arguments) {
            try {
                //var ContractTemplateId = document.getElementById('ctl00_ContentPlaceHolder1_hfCTId').value;
                //if (ContractTemplateId == '' || ContractTemplateId == null) {

                var Fromdate = document.getElementById('ctl00_ContentPlaceHolder1_txtFromdate');
                var ToDate = document.getElementById('ctl00_ContentPlaceHolder1_txtTodate');

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
        function deleteAttachments(rowNo, subId) {
            var controlIdPrefix;
            if (rowNo >= 10) {
                controlIdPrefix = 'ctl00_ContentPlaceHolder1_gvChecklistDetails_ctl';
            }
            else {
                controlIdPrefix = 'ctl00_ContentPlaceHolder1_gvChecklistDetails_ctl0';
            }

            var filename = document.getElementById(controlIdPrefix + rowNo + '_ServerFileName').value;
            //var filename = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklistDetails_ctl0"+ rowNo+"_ServerFileName").value;	

            if (!confirm('Are you sure that you want to delete this record?')) return false;
            window.open(
                "../DeleteChecklistFile.aspx?calledFrom=Checklist&rowNo="
                + rowNo + "&filename=" + filename + "&SUBId=" + subId,
                "FILE", "location=0,status=0,scrollbars=0,width=10,height=10");
            return false;
        }

        function showHideButtons(boolVisible, index) {
            try {
                index = parseInt(index) + 2;
                if (boolVisible == 'true') {
                    document.getElementById('AttachFileImg' + index).style.visibility = 'visible';
                    document.getElementById('DeleteFileImg' + index).style.visibility = 'hidden';
                }
                else {
                    document.getElementById('AttachFileImg' + index).style.visibility = 'hidden';
                    document.getElementById('DeleteFileImg' + index).style.visibility = 'visible';
                }

            } catch (e) {
                alert(e.message);
            }
        }
        function hideButtons(index) {
            index = parseInt(index) + 2;
            document.getElementById('AttachFileImg' + index).style.visibility = 'hidden';
            document.getElementById('DeleteFileImg' + index).style.visibility = 'hidden';
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
            var ValidationGroup = document.getElementById(controlIdPrefix + rowNo + '_hfValidtionGroup').value;
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
            } else {
                return false
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

        function openSubmissionFilesWindow(scId, operationType, Type) {
            window.open('../Submissions/SubmissionCheckListFiles.aspx?SCId=' + scId + '&OperationType=' + operationType + '&Type=' + Type, '', 'location=0,status=0,scrollbars=1,width=900,height=600');
            return false;
        }

        function openSubmissionFilesWindowForExtension() {
            var SCId = document.getElementById('ctl00_ContentPlaceHolder1_hfSelectedRecord').value;
            if (SCId != '') {
                window.open('../Submissions/SubmissionCheckListFiles.aspx?SCId=' + SCId + '&OperationType=Extension&Type=Attach', 'location=0,status=0,scrollbars=1,width=900,height=600');
            }
            else {
                alert('Please refresh the page and try again.')
            }
            return false;
        }

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

    <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />
    <asp:HiddenField ID="hfDoubleClickFlagSubmit" runat="server" />
    <asp:HiddenField ID="hfMonth" runat="server" />
    <asp:HiddenField ID="hfRole" runat="server" />
    <asp:HiddenField ID="hfSelectedOperation" runat="server" />
    <asp:HiddenField ID="hfSelectedRecord" runat="server" />
    <asp:HiddenField ID="hfType" runat="server" />
    <asp:HiddenField ID="hfCurDate" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">
                            <asp:Label ID="lblTitle" runat="server" Text="Search Submissions"></asp:Label></h4>
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
                            <label class="form-label">Financial Year:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlFinYear" runat="server"
                                DataValueField="FYM_ID" DataTextField="FYM_FIN_YEAR">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvFinYear" ControlToValidate="ddlFinYear" CssClass="text-danger" Display="Dynamic" SetFocusOnError="True" ErrorMessage="*" ValidationGroup="Search">*</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Type:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlType" runat="server">
                                <asp:ListItem Value="">(None)</asp:ListItem>
                                <asp:ListItem Value="F">Fixed Date</asp:ListItem>
                                <asp:ListItem Value="E">Event Based</asp:ListItem>
                            </asp:DropDownList>
                            <%-- <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="ddlType" CssClass="text-danger" ValidationGroup="Search" Display="Dynamic" SetFocusOnError="True" ErrorMessage="Please select Submission Type.">*</asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Particulars:</label>
                            <F2FControls:F2FTextBox CssClass="form-control" Columns="40" ID="txtParticulars" runat="server"></F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtParticulars" />
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Month:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlMonth" runat="server">
                                <asp:ListItem Selected="True" Value="" Text="All"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Jan"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Feb"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Mar"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Apr"></asp:ListItem>
                                <asp:ListItem Value="5" Text="May"></asp:ListItem>
                                <asp:ListItem Value="6" Text="Jun"></asp:ListItem>
                                <asp:ListItem Value="7" Text="Jul"></asp:ListItem>
                                <asp:ListItem Value="8" Text="Aug"></asp:ListItem>
                                <asp:ListItem Value="9" Text="Sep"></asp:ListItem>
                                <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                                <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                                <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
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
                            <label class="form-label">Description:</label>
                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtDescription" Columns="40" runat="server"></F2FControls:F2FTextBox>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Select Reporting to:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlSegment" runat="server" AppendDataBoundItems="true"
                                DataTextField="SSM_NAME" DataValueField="SSM_ID">
                                <asp:ListItem Value="">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">From Date:</label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox CssClass="form-control" Columns="13" ID="txtFromdate" runat="server"></F2FControls:F2FTextBox>
                                <asp:ImageButton ID="imgFromDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />&nbsp;
                           
                           
                           
                            </div>
                            <asp:RegularExpressionValidator ID="revCircularDate" runat="server" ControlToValidate="txtFromdate"
                                ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                ValidationGroup="Search" Display="Dynamic" CssClass="text-danger"></asp:RegularExpressionValidator>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">To Date:</label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox CssClass="form-control" ID="txtTodate" runat="server" Columns="13"></F2FControls:F2FTextBox>
                                <asp:ImageButton ID="imgBtnToDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />&nbsp;
                  
                           
                           
                            </div>
                            <asp:RegularExpressionValidator ID="revToDate" runat="server" ControlToValidate="txtTodate"
                                ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                ValidationGroup="Search" Display="Dynamic" CssClass="text-danger"></asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="cvCompletionDate" runat="server" ClientValidationFunction="compareEndSystemDates" CssClass="text-danger"
                                ControlToValidate="txtToDate" ErrorMessage="From Date should be less than or Equal to To Date."
                                Display="Dynamic" OnServerValidate="cvTodate_ServerValidate" ValidationGroup="Search"></asp:CustomValidator>

                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Depends On Event:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlEvent" AutoPostBack="true" AppendDataBoundItems="true"
                                runat="server" OnSelectedIndexChanged="ddlEvent_SelectedIndexChanged" DataValueField="EM_ID"
                                DataTextField="EM_EVENT_NAME">
                                <asp:ListItem Value="">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-8 mb-3">
                            <label class="form-label">Select Agendas:</label>
                            <div class="custom-checkbox-table">
                                <asp:CheckBoxList ID="cblAssociatedWith" CssClass="form-control" RepeatColumns="5" runat="server" DataTextField="EP_NAME"
                                    DataValueField="EP_ID" AppendDataBoundItems="true">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        <div class="col-md-12 mb-3">
                            <label class="form-label">Status:</label>
                            <div class="custom-checkbox-table">
                                <asp:CheckBoxList ID="chkStatus" CssClass="form-control" AppendDataBoundItems="true"
                                    runat="server" RepeatColumns="6">
                                    <asp:ListItem Value="C" Text="Closed" />
                                    <asp:ListItem Value="P" Text="Pending" />
                                    <asp:ListItem Value="R" Text="Reopened" />
                                    <asp:ListItem Value="SR" Text="Sent back by tracking function" />
                                    <asp:ListItem Value="S" Text="Submitted" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        <%--<<Added by Ankur Tyagi on 18-Apr-2025 for Project Id : 2395--%>
                        <div class="col-md-8 mb-3">
                            <label class="form-label">Global search:</label>
                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtGlobalSearch" runat="server"></F2FControls:F2FTextBox>
                        </div>
                        <%-->>--%>
                    </div>
                    <div class="row">
                        <div class="col-md-4 mb-3">
                            <label class="form-label">&nbsp;</label>
                            <div>
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
                                <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcel" runat="server" Text="Export to Excel"
                                    OnClick="btnExportToExcel_Click" Visible="false">
                                        <i class="fa fa-download"></i> Export to Excel               
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvChecklistDetails" AllowPaging="false" runat="server" AutoGenerateColumns="False"
                            DataKeyNames="SC_ID" GridLines="Both" AllowSorting="true" CssClass="table table-bordered footable" CellPadding="4"
                            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnSelectedIndexChanged="gvChecklistDetails_SelectedIndexChanged"
                            OnRowDataBound="gvChecklistDetails_RowDataBound" OnPageIndexChanging="gvChecklistDetails_PageIndexChanging"
                            OnSorting="gvChecklistDetails_Sorting" OnRowCreated="OnRowCreated">
                            <Columns>
                                <asp:TemplateField HeaderText="Submission ID" SortExpression="SUB_ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubId" Text='<%#Eval("SUB_ID")%>' runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbStmId" Text='<%#Eval("SC_STM_ID")%>' runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lblSrdId" Text='<%#Eval("SRD_ID")%>' runat="server" Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sr No.">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfValidtionGroup" runat="server" />
                                        <asp:Label ID="lblSrNo" Text='<%# Container.DataItemIndex + 1 %>' runat="server"> 
                                        </asp:Label>.
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderText="Extension" ShowHeader="true">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton CssClass="basic" ID="lnkExtension" runat="server" Text="+">
                                                <i class="fa fa-plus"></i>
                                            </asp:LinkButton>
                                        </center>
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
                                <asp:TemplateField HeaderText="Reporting Function" SortExpression="SRD_NAME">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReportFun" Text='<%#Eval("SRD_NAME")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reporting to" SortExpression="SSM_NAME">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSegment" Text='<%#Eval("SSM_NAME")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
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
                                <%--<<Added by Ashish Mishra on 27Jul2017--%>
                                <asp:BoundField DataField="SUB_SUBMITTED_BY" HeaderText="Submitted By" SortExpression="SUB_SUBMITTED_BY" />
                                <asp:TemplateField HeaderText="Submitted On" SortExpression="SUB_SUBMIT_DATE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubmittedOn" Text='<%#Eval("SUB_SUBMIT_DATE","{0:dd-MMM-yyyy HH:mm:ss}")%>'
                                            runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SUB_CLOSED_BY" HeaderText="Closed By" SortExpression="SUB_CLOSED_BY" />
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
                                <asp:BoundField DataField="SUB_REOPENED_BY" HeaderText="Reopened By" SortExpression="SUB_REOPENED_BY" />
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
                                <asp:TemplateField HeaderText="Is Extended" SortExpression="SC_IS_EXTENDED">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIsExtended" Text='<%#Eval("SC_IS_EXTENDED").ToString()=="Y"?"Yes":"No"%>'
                                            runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Extension done by" SortExpression="SC_EXTENSION_DONE_BY">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSC_EXTENSION_DONE_BY" runat="server" Text='<%# Eval("SC_EXTENSION_DONE_BY").ToString()%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Extension done on" SortExpression="SC_EXTENSION_DONE_ON">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSC_EXTENSION_DONE_ON" Text='<%#Eval("SC_EXTENSION_DONE_ON","{0:dd-MMM-yyyy HH:mm:ss}")%>'
                                            runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Extension remarks" SortExpression="SC_EXTENSION_REMARKS">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSC_EXTENSION_REMARKS" Text='<%#Eval("SC_EXTENSION_REMARKS").ToString().Replace("\n", "<br />")%>'
                                            runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Submitted to Authority On / Date of Receipt of Data"
                                    SortExpression="SUB_SUBMITTED_TO_AUTHORITY_ON">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubAuthorityDate" Text='<%#Eval("SUB_SUBMITTED_TO_AUTHORITY_ON","{0:dd-MMM-yyyy}")%>'
                                            runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-->>--%>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Yes/No/NA
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlYesNoNA" runat="server" Width="150" CssClass="form-select">
                                            <asp:ListItem Text="-Select-" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                            <asp:ListItem Text="NA" Value="NA"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvYesNoNA" ControlToValidate="ddlYesNoNA" CssClass="text-danger" Display="Dynamic" ErrorMessage="Please select Yes/No/NA." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Remarks
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <F2FControls:F2FTextBox CssClass="form-control" runat="server" ID="txtRemarks" Width="200" MaxLength="1000"
                                            TextMode="MultiLine" Text='<%# Bind("SUB_REMARKS") %>' Rows="3" Columns="20" />
                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Attach Files" Visible="false">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="ClientFileName" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="ServerFileName" runat="server" Value='<%# Bind("SUB_SERVER_FILE_NAME") %>'></asp:HiddenField>
                                        <a style="visibility: hidden;" id="AttachFileImg<%# Container.DataItemIndex + 2  %>"
                                            onclick="return openpopupAttachments('<%# Container.DataItemIndex + 2  %>')">
                                            <img border="0" src="../../Content/images/legacy/attach.png" /></a> <a style="visibility: hidden;" class="btn btn-sm btn-soft-danger btn-circle"
                                                id="DeleteFileImg<%# Container.DataItemIndex + 2  %>" onclick="return deleteAttachments('<%# Container.DataItemIndex + 2  %>')">
                                                <i class="fa fa-trash"></i></a><a id="Filelink<%# Container.DataItemIndex + 2  %>"
                                                    href="javascript:void(0);" onclick="javascript:window.open('../CommonDownloadAnyFile.aspx?type=ChecklistFilesFolder&downloadFileName=<%# Eval("SUB_SERVER_FILE_NAME")%>&fileName=<%#Eval("SUB_CLIENT_FILE_NAME")%>','','location=0,status=0,scrollbars=0,width=400,height=200');">
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

                                <asp:TemplateField ShowHeader="False" Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbSave" runat="server" CommandName="Select" Width="120" CssClass="btn btn-outline-success" Text="Save Draft"
                                            OnClientClick="return onClientSaveClick(this)">
                                            <i class="fa fa-save me-2"></i> Save Draft
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False" Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbSubmit" runat="server" Width="120" CssClass="btn btn-outline-success" CommandName="Select" Text="Submit" OnClientClick="return onClientSubmitClick(this)">
                                           <i class="fa fa-save me-2"></i> Submit
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
    <!-- end row -->
    <div class="modal fade" id="divExtension" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog-centered modal-dialog modal-md">
            <div class="modal-content modal-lg">
                <div class="modal-header">
                    <h5 class="modal-title">Regulatory date extension details</h5>
                    <button id="btnCloseModalHeader" type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="row">


                        <div class="col-md-12 mb-3">
                            <label class="form-label">New Regulatory Date:<span id="spnRegulatoryDt" class="text-danger">*</span></label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtExRegulatoryDate" CssClass="form-control" runat="server"
                                    MaxLength="11" ToolTip="Regulatory Date"></F2FControls:F2FTextBox>
                                <asp:ImageButton ID="imgTargetDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" ToolTip="Select Date" />
                            </div>
                            <ajaxToolkit:CalendarExtender ID="ceRegulatoryDate" runat="server" PopupButtonID="imgTargetDate"
                                TargetControlID="txtExRegulatoryDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>

                            <asp:CustomValidator ID="cvRegulatoryDate" runat="server" ValidationGroup="Extension"
                                ControlToValidate="txtExRegulatoryDate" CssClass="text-danger" ErrorMessage="Regulatory date cannot be lower than current date."
                                Display="Dynamic" ValidateEmptyText="true" ClientValidationFunction="CompareRegulatoryDateSytemDate"> 
                            </asp:CustomValidator>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Extension" runat="server" ForeColor="Red"
                                Display="Dynamic" ControlToValidate="txtExRegulatoryDate">Please enter new regulatory date.</asp:RequiredFieldValidator>
                        </div>


                        <div class="col-12">
                            <label class="form-label">Extension Remarks: <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtExRemarks" CssClass="form-control" TextMode="MultiLine" runat="server"
                                Rows="3" Columns="50">
                            </asp:TextBox>
                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtExRemarks" />
                            <asp:RequiredFieldValidator ID="rfvARRemarks" ValidationGroup="Extension" runat="server" ForeColor="Red"
                                Display="Dynamic" ControlToValidate="txtExRemarks">Please enter remarks.</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-12">
                            <label class="form-label">Attach Extension attachment:</label><br />
                            <asp:LinkButton runat="server" CssClass="btn btn-outline-primary" Text="Attach" ID="lbExtAttach"
                                OnClientClick="return openSubmissionFilesWindowForExtension()">
                                Attach
                            </asp:LinkButton>
                        </div>

                    </div>
                    <br />
                    <div class="col-12 text-center mt-2">
                        <asp:LinkButton ID="lbExtension" runat="server" CssClass="btn btn-outline-success" Width="120"
                            Text="Submit" ValidationGroup="Extension" OnClick="lbExtension_Click">
                            <i class="fa fa-save me-2"></i>  Submit
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function CompareRegulatoryDateSytemDate(src, arg) {
            //if ($('[id*="ddlStatus"]').val() == 'P' && arg.Value != '') {
            if (arg.Value != '') {
                if (Date.parse($("#ctl00_ContentPlaceHolder1_hfCurDate").val()) > Date.parse($("#ctl00_ContentPlaceHolder1_txtExRegulatoryDate").val())) {
                    arg.IsValid = false;
                }
                else {
                    arg.IsValid = true;
                }
            } else {
                arg.IsValid = true;
            }
        }
    </script>
</asp:Content>
