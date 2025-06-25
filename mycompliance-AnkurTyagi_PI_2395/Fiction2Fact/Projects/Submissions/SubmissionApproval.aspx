<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Submissions.SubmissionApproval" Title="Submission Approval"
    Async="true" CodeBehind="SubmissionApproval.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="a" Namespace="Fiction2Fact.Controls" Assembly="RequiredIfValidatorRadioButtonList" %>
<%@ Register TagPrefix="b" Namespace="Fiction2Fact.Controls" Assembly="RequiredIfValidator" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/dateformatvalidation.js") %>'></script>

    <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/DateValidator.js") %>'></script>

    <script type="text/javascript">

        function onClientViewCircClick(CMId) {
            window.open('../Circulars/ViewCircularDetails.aspx?CircularId=' + CMId, '', 'location=0,status=0,resizable=1,scrollbars=1,width=1000,height=800');
            return false;
        }

        function editSMId(SMId) {
            var url = '../Submissions/CommonSubmission.aspx?Type=RR&SMId=' + SMId
            window.open(url, '_blank').focus();
            return false;
        }

        function onClientApproveClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Approve";
        }
        function onClientRejectClick() {
            if (!confirm('Are you sure that you want to reject this record?')) return false;
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Reject";
        }

        const onActionClick = (SMId) => {
            var RequirementIdDecoded = "";
            try {
                RequirementIdDecoded = SMId;
            } catch (e) {
                RequirementIdDecoded = SMId;
            }

            $('[id$="hfSelectedRecord"]').val(RequirementIdDecoded);
            $("#divAction").modal('show');
            return false;
        }

        function openModal(action) {

            const lbApprove = document.getElementById('ctl00_ContentPlaceHolder1_lbApprove')
            const lbReject = document.getElementById("ctl00_ContentPlaceHolder1_lbReject");

            if (action == 'A') {
                lbApprove.style.display = "inline-block";
                lbReject.style.display = "none";
            }
            else {
                lbApprove.style.display = "none";
                lbReject.style.display = "inline-block";
            }

            var isAnyChkboxChecked = false;

            for (var i = 1; i < CheckBoxIDs.length; i++) {
                var cb = document.getElementById(CheckBoxIDs[i]);
                if (cb != 'undefined') {
                    if (cb.checked) {
                        isAnyChkboxChecked = true;
                    }
                }
            }
            if (!isAnyChkboxChecked) {
                alert('Please select one or more records.');
            }
            else {
                $("#divAction").modal('show');
            }

            return false;
        }

        function validateApprove() {
            var IsDoubleClickFlagSet = document.getElementById('<%=hfDoubleClickFlag.ClientID %>').value;
            var validated = Page_ClientValidate('Remarks');
            if (IsDoubleClickFlagSet == 'Yes') {
                alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                return false;
            }
            else {
                if (validated) {
                    document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                    return true;
                }
            }
        }

        function ChangeCheckBoxState(id, checkState) {
            var cb = document.getElementById(id);
            if (cb != null)
                cb.checked = checkState;
        }

        function ChangeAllCheckBoxStates(checkState) {
            if (CheckBoxIDs != null) {
                for (var i = 0; i < CheckBoxIDs.length; i++)
                    ChangeCheckBoxState(CheckBoxIDs[i], checkState);
            }
        }

        function onRowCheckedUnchecked(cbid) {
            ChangeHeaderAsNeeded();
            return;
        }
        function ChangeHeaderAsNeeded() {
            if (CheckBoxIDs != null) {
                for (var i = 1; i < CheckBoxIDs.length; i++) {
                    var cb = document.getElementById(CheckBoxIDs[i]);
                    if (!cb.checked) {
                        ChangeCheckBoxState(CheckBoxIDs[0], false);
                        return;
                    }
                }
                ChangeCheckBoxState(CheckBoxIDs[0], true);
            }
        }

    </script>
    <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />
    <asp:HiddenField ID="hfDoubleClickFlagSubmit" runat="server" />
    <asp:HiddenField ID="hfSelectedOperation" runat="server" />
    <asp:HiddenField ID="hfSelectedRecord" runat="server" />
    <asp:Literal ID="CheckBoxIDsArray" runat="server"></asp:Literal>

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">
                            <asp:Label ID="lbSelectedMonth" runat="server" Text="Task Approval"></asp:Label></h4>
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
                            <label class="form-label">Reporting Department:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlReportDept" AppendDataBoundItems="true"
                                runat="server" DataValueField="SRD_ID" DataTextField="SRD_NAME">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Status:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlStatus" runat="server" AppendDataBoundItems="true"
                                DataTextField="RC_NAME" DataValueField="RC_CODE">
                            </asp:DropDownList>
                        </div>
                        <%--<<Added by Ankur Tyagi on 18-Apr-2025 for Project Id : 2395--%>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Select Reporting to:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlSegment" runat="server" AppendDataBoundItems="true"
                                DataTextField="SSM_NAME" DataValueField="SSM_ID">
                                <asp:ListItem Value="">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6 mb-3">
                            <label class="form-label">Global search:</label>
                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtGlobalSearch" runat="server"></F2FControls:F2FTextBox>
                        </div>
                        <%-->>--%>
                    </div>
                    <div class="mt-3">
                        <asp:LinkButton ID="lnkSearch" runat="server" CssClass="btn btn-outline-secondary" Text="lnkSearch" OnClick="lnkSearch_Click">
                                <i class="fa fa-download"></i> Search
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnExportToExcel" runat="server" CssClass="btn btn-outline-secondary" Text="Export to Excel"
                            OnClick="btnExportToExcel_Click" Visible="false">
                            <i class="fa fa-download"></i> Export to Excel               
                        </asp:LinkButton>
                    </div>
                    <div class="mt-3">
                        <asp:LinkButton ID="lnkApprove" runat="server" CssClass="btn btn-outline-success" Width="120" Visible="false"
                            Text="Approve" OnClientClick="return openModal('A');">
<i class="fa fa-save me-2"></i>  Approve
                        </asp:LinkButton>
                        <asp:LinkButton ID="lnkReject" runat="server" CssClass="btn btn-outline-danger" Width="120" Visible="false"
                            Text="Reject" OnClientClick="return openModal('R');">
<i class="fa fa-ban"></i>  Reject
                        </asp:LinkButton>
                    </div>
                    <div class="mt-3">
                        <div class="table-responsive">
                            <asp:GridView ID="gvApprovalList" EmptyDataText="Checklist Not Available for Selected Month."
                                CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                EmptyDataRowStyle-ForeColor="red" AllowPaging="False" runat="server" AutoGenerateColumns="False"
                                DataKeyNames="SM_ID" GridLines="None" AllowSorting="true" CellPadding="4" OnSelectedIndexChanged="gvApprovalList_SelectedIndexChanged"
                                OnRowDataBound="gvApprovalList_RowDataBound" OnDataBound="gvApprovalList_DataBound" OnSorting="gvApprovalList_Sorting"
                                OnRowCreated="OnRowCreated">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="HeaderLevelCheckBox" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="RowLevelCheckBox" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sr No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>.
                                            <asp:Label ID="lblSM_ID" Text='<%#Eval("SM_ID")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblSRD_ID" Text='<%#Eval("SRD_ID")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblSTM_ID" Text='<%#Eval("SM_STM_ID")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblSM_WORKFLOW_STATUS" Text='<%#Eval("SM_WORKFLOW_STATUS")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblEM_ID" Text='<%#Eval("EM_ID")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblEP_ID" Text='<%#Eval("EP_ID")%>' runat="server" Visible="false"></asp:Label>

                                            <asp:Label ID="lblSM_SUB_TYPE" Text='<%#Eval("SM_SUB_TYPE")%>' runat="server" Visible="false"></asp:Label>

                                            <asp:Label ID="lblOFD" Text='<%#Eval("SM_ONLY_ONCE_FROM_DATE")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblOTD" Text='<%#Eval("SM_ONLY_ONCE_TO_DATE")%>' runat="server" Visible="false"></asp:Label>

                                            <asp:Label ID="lblDaily" Text='<%#Eval("SM_DAILY")%>' runat="server" Visible="false"></asp:Label>

                                            <asp:Label ID="lblWDF" Text='<%#Eval("SM_WEEKLY_DUE_DATE_FROM")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblWDT" Text='<%#Eval("SM_WEEKLY_DUE_DATE_TO")%>' runat="server" Visible="false"></asp:Label>

                                            <asp:Label ID="lblMDF" Text='<%#Eval("SM_MONTHLY_DUE_DATE_FROM")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblMDT" Text='<%#Eval("SM_MONTHLY_DUE_DATE_TO")%>' runat="server" Visible="false"></asp:Label>

                                            <asp:Label ID="lblQ1DF" Text='<%#Eval("SM_Q1_DUE_DATE_FROM")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblQ1DT" Text='<%#Eval("SM_Q1_DUE_DATE_TO")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblQ2DF" Text='<%#Eval("SM_Q2_DUE_DATE_FROM")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblQ2DT" Text='<%#Eval("SM_Q2_DUE_DATE_TO")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblQ3DF" Text='<%#Eval("SM_Q3_DUE_DATE_FROM")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblQ3DT" Text='<%#Eval("SM_Q3_DUE_DATE_TO")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblQ4DF" Text='<%#Eval("SM_Q4_DUE_DATE_FROM")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblQ4DT" Text='<%#Eval("SM_Q4_DUE_DATE_TO")%>' runat="server" Visible="false"></asp:Label>

                                            <asp:Label ID="lblFHDF" Text='<%#Eval("SM_FIRST_HALF_YR_DUE_DATE_FROM")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblFHDT" Text='<%#Eval("SM_FIRST_HALF_YR_DUE_DATE_TO")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblSHDF" Text='<%#Eval("SM_SECOND_HALF_YR_DUE_DATE_FROM")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblSHDT" Text='<%#Eval("SM_SECOND_HALF_YR_DUE_DATE_TO")%>' runat="server" Visible="false"></asp:Label>

                                            <asp:Label ID="lblYDF" Text='<%#Eval("SM_YEARLY_DUE_DATE_FROM")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblYDT" Text='<%#Eval("SM_YEARLY_DUE_DATE_TO")%>' runat="server" Visible="false"></asp:Label>

                                            <asp:Label ID="lblFFD" Text='<%#Eval("SM_FIRST_FORTNIGHTLY_DUE_FROM_DATE")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblFTD" Text='<%#Eval("SM_FIRST_FORTNIGHTLY_DUE_TO_DATE")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblSFD" Text='<%#Eval("SM_SECOND_FORTNIGHTLY_DUE_FROM_DATE")%>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblSTD" Text='<%#Eval("SM_SECOND_FORTNIGHTLY_DUE_TO_DATE")%>' runat="server" Visible="false"></asp:Label>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderText="Action" ShowHeader="true" Visible="false">
                                        <ItemTemplate>
                                            <center>
                                                <asp:LinkButton CssClass="basic" ID="lnkAction" runat="server" Text="+">
                                                    <i class="fa fa-plus"></i>
                                                </asp:LinkButton>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbEdit" runat="server" CssClass="btn btn-sm btn-soft-success btn-circle">
                                        <i class="fa fa-pen"></i>
                                            </asp:LinkButton>
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
                                            <a href="javascript:void(0);" class="btn btn-sm btn-soft-info btn-circle" onclick="javascript:window.open('../Submissions/SubmissionDocuments.aspx?Id=<%# Eval("SM_ID") %>','','location=0,status=0,scrollbars=1,width=450,height=250,resizable=1');">
                                                <i class="fa fa-eye"></i>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Effective date" SortExpression="SM_EFFECTIVE_DT">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSM_EFFECTIVE_DT" Text='<%#Eval("SM_EFFECTIVE_DT","{0:dd-MMM-yyyy}")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Circular date" SortExpression="SM_CIRCULAR_DATE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSM_CIRCULAR_DATE" Text='<%#Eval("SM_CIRCULAR_DATE","{0:dd-MMM-yyyy}")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type" SortExpression="SubmissionType">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubmissionType" Text='<%#Eval("SubmissionType")%>' runat="server"></asp:Label>
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
                                            <asp:Label ID="lblSegment" Text='<%#Eval("Authority")%>' runat="server"></asp:Label>
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
                                            <asp:Label ID="lblParticulars" Width="300px" runat="server" ToolTip='<%#Eval("SM_PERTICULARS").ToString()%>'
                                                Text='<%#Eval("SM_PERTICULARS").ToString().Length > 200 ? (Eval("SM_PERTICULARS") as string).Substring(0, 200).Replace("\n", "<br />") 
                                                    + "..." : Eval("SM_PERTICULARS").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                            <asp:Label ID="lblParticulars1" runat="server" Visible="false" Text='<%#Eval("SM_PERTICULARS").ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" SortExpression="SC_DESCRIPTION">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblDescription" Text='<%#Eval("SC_DESCRIPTION").ToString().Replace("\n","<br />") %>' runat="server"
                                                Width="200px"></asp:Label>--%>
                                            <asp:Label ID="lblDescription" Width="300px" runat="server" ToolTip='<%#Eval("SM_BRIEF_DESCRIPTION").ToString()%>'
                                                Text='<%#Eval("SM_BRIEF_DESCRIPTION").ToString().Length > 200 ? (Eval("SM_BRIEF_DESCRIPTION") as string).Substring(0, 200).Replace("\n", "<br />") 
                                                    + " ..." : Eval("SM_BRIEF_DESCRIPTION").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                            <asp:Label ID="lblDescription1" runat="server" Visible="false" Text='<%#Eval("SM_BRIEF_DESCRIPTION").ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To be escalated" SortExpression="ToBeEscalated">
                                        <ItemTemplate>
                                            <asp:Label ID="lblToBeEscalated" Text='<%#Eval("ToBeEscalated")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reminder days for reporting function" SortExpression="SM_L0_ESCALATION_DAYS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLevel0EscDays" Text='<%#Eval("SM_L0_ESCALATION_DAYS")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Level 1 Escalation days" SortExpression="SM_L1_ESCALATION_DAYS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLevel1EscDays" Text='<%#Eval("SM_L1_ESCALATION_DAYS")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Level 2 Escalation days" SortExpression="SM_L2_ESCALATION_DAYS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLevel2EscDays" Text='<%#Eval("SM_L2_ESCALATION_DAYS")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Frequency" SortExpression="SC_FREQUENCY">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFrequency" Text='<%#Eval("SM_FREQUENCY")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Adhoc - Internal due date" SortExpression="SM_ONLY_ONCE_FROM_DATE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOnlyOnceFromDate" Text='<%#Eval("SM_ONLY_ONCE_FROM_DATE")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Adhoc - Regulatory due date" SortExpression="SM_ONLY_ONCE_TO_DATE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOnlyOnceToDate" Text='<%#Eval("SM_ONLY_ONCE_TO_DATE")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Weekly - Due Day From" SortExpression="SM_WEEKLY_DUE_DATE_FROM">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWeeklyFromDate" Text='<%#Eval("SM_WEEKLY_DUE_DATE_FROM")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Weekly - Due Day To" SortExpression="SM_WEEKLY_DUE_DATE_TO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWeeklyToDate" Text='<%#Eval("SM_WEEKLY_DUE_DATE_TO")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SM_FIRST_FORTNIGHTLY_DUE_FROM_DATE" HeaderText="Fortnightly 1 - Internal due date"
                                        SortExpression="SM_FIRST_FORTNIGHTLY_DUE_FROM_DATE" />
                                    <asp:BoundField DataField="SM_FIRST_FORTNIGHTLY_DUE_TO_DATE" HeaderText="Fortnightly 1 - Regulatory due date"
                                        SortExpression="SM_FIRST_FORTNIGHTLY_DUE_TO_DATE" />
                                    <asp:BoundField DataField="SM_SECOND_FORTNIGHTLY_DUE_FROM_DATE" HeaderText="Fortnightly 2 - Internal due date"
                                        SortExpression="SM_SECOND_FORTNIGHTLY_DUE_FROM_DATE" />
                                    <asp:BoundField DataField="SM_SECOND_FORTNIGHTLY_DUE_TO_DATE" HeaderText="Fortnightly 2 - Regulatory due date"
                                        SortExpression="SM_SECOND_FORTNIGHTLY_DUE_TO_DATE" />
                                    <asp:TemplateField HeaderText="Monthly - Internal due date" SortExpression="SM_MONTHLY_DUE_DATE_FROM">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMonthlyFromDate" Text='<%#Eval("SM_MONTHLY_DUE_DATE_FROM")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Monthly - Regulatory due date" SortExpression="SM_MONTHLY_DUE_DATE_TO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMonthlyToDate" Text='<%#Eval("SM_MONTHLY_DUE_DATE_TO")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quarter 1 - Internal due date" SortExpression="SM_Q1_DUE_DATE_FROM">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQ1FromDate" Text='<%#Eval("SM_Q1_DUE_DATE_FROM")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quarter 1 - Regulatory due date" SortExpression="SM_Q1_DUE_DATE_TO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQ1ToDate" Text='<%#Eval("SM_Q1_DUE_DATE_TO")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quarter 2 - Internal due date" SortExpression="SM_Q2_DUE_DATE_FROM">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQ2FromDate" Text='<%#Eval("SM_Q2_DUE_DATE_FROM")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quarter 2 - Regulatory due date" SortExpression="SM_Q2_DUE_DATE_TO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQ2ToDate" Text='<%#Eval("SM_Q2_DUE_DATE_TO")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quarter 3 - Internal due date" SortExpression="SM_Q3_DUE_DATE_FROM">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQ3FromDate" Text='<%#Eval("SM_Q3_DUE_DATE_FROM")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quarter 3 - Regulatory due date" SortExpression="SM_Q3_DUE_DATE_TO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQ3ToDate" Text='<%#Eval("SM_Q3_DUE_DATE_TO")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quarter 4 - Internal due date" SortExpression="SM_Q4_DUE_DATE_FROM">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQ4FromDate" Text='<%#Eval("SM_Q4_DUE_DATE_FROM")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quarter 4 - Regulatory due date" SortExpression="SM_Q4_DUE_DATE_TO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQ4ToDate" Text='<%#Eval("SM_Q4_DUE_DATE_TO")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Half Year 1 - Internal due date" SortExpression="SM_FIRST_HALF_YR_DUE_DATE_FROM">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHY1FromDate" Text='<%#Eval("SM_FIRST_HALF_YR_DUE_DATE_FROM")%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Half Year 1 - Regulatory due date" SortExpression="SM_FIRST_HALF_YR_DUE_DATE_TO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHY1ToDate" Text='<%#Eval("SM_FIRST_HALF_YR_DUE_DATE_TO")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Half Year 2 - Internal due date" SortExpression="SM_SECOND_HALF_YR_DUE_DATE_FROM">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHY2FromDate" Text='<%#Eval("SM_SECOND_HALF_YR_DUE_DATE_FROM")%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Half Year 2 - Regulatory due date" SortExpression="SM_SECOND_HALF_YR_DUE_DATE_TO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHY2ToDate" Text='<%#Eval("SM_SECOND_HALF_YR_DUE_DATE_TO")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Annual - Internal due date" SortExpression="SM_YEARLY_DUE_DATE_FROM">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAnnualFromDate" Text='<%#Eval("SM_YEARLY_DUE_DATE_FROM")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Annual - Regulatory due date" SortExpression="SM_YEARLY_DUE_DATE_TO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAnnualToDate" Text='<%#Eval("SM_YEARLY_DUE_DATE_TO")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SM_START_NO_OF_DAYS" HeaderText="Start No. of Days" SortExpression="SM_START_NO_OF_DAYS" Visible="false" />
                                    <asp:BoundField DataField="SM_END_NO_OF_DAYS" HeaderText="End No. of Days" SortExpression="SM_END_NO_OF_DAYS" Visible="false" />
                                    <asp:TemplateField HeaderText="Finance Approval" SortExpression="FSApprovalReq">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFSApprovalReq" Text='<%# Eval("FSApprovalReq").ToString()%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submitted By" SortExpression="SUB_SUBMITTED_BY">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubmittedby" Text='<%# Getfullname(Eval("SM_CREATE_BY").ToString())%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submitted On" SortExpression="SUB_SUBMIT_DATE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubmittedOn" Text='<%#Eval("SM_CREATE_DT","{0:dd-MMM-yyyy HH:mm:ss}")%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" Text='<%#Eval("Status")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Approve/Reject By" SortExpression="ARBy">
                                        <ItemTemplate>
                                            <asp:Label ID="lblARBy" runat="server" Text='<%# Getfullname(Eval("ARBy").ToString())%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Approve/Reject On" SortExpression="AROn">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAROn" Text='<%#Eval("AROn","{0:dd-MMM-yyyy HH:mm:ss}")%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Approver Remarks" SortExpression="ARRemarks">
                                        <ItemTemplate>
                                            <asp:Label ID="lblApproverRemarks" Text='<%#Eval("ARRemarks")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False" Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbApprove" runat="server" CssClass="btn btn-outline-success" Width="120"
                                                CommandName="Select" Text="Approve" OnClientClick="onClientApproveClick()">
                                                <i class="fa fa-save me-2"></i>  Approve
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False" Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbReject" runat="server" CssClass="btn btn-outline-danger" Width="120"
                                                CommandName="Select" Text="Reject" OnClientClick="onClientRejectClick()">
                                                    <i class="fa fa-ban"></i>  Reject
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

    <div class="modal fade" id="divAction" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog-centered modal-dialog modal-md">
            <div class="modal-content modal-lg">
                <div class="modal-header">
                    <h5 class="modal-title">Approve/Reject Remarks</h5>
                    <button id="btnCloseModalHeader" type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-12">
                            <label class="form-label">Remarks: <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtARRemarks" CssClass="form-control" TextMode="MultiLine" runat="server"
                                Rows="3" Columns="50">
                            </asp:TextBox>
                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtARRemarks" />
                            <asp:RequiredFieldValidator ID="rfvARRemarks" ValidationGroup="Remarks" runat="server" ForeColor="Red"
                                Display="Dynamic" ControlToValidate="txtARRemarks">Please enter remarks.</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <br />
                    <div class="col-12 text-center mt-2">
                        <asp:LinkButton ID="lbApprove" runat="server" CssClass="btn btn-outline-success" Width="120"
                            Text="Approve" ValidationGroup="Remarks" OnClick="lbApprove_Click" OnClientClick="return validateApprove();">
                                <i class="fa fa-save me-2"></i>  Approve
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbReject" runat="server" CssClass="btn btn-outline-danger" Width="120"
                            Text="Reject" ValidationGroup="Remarks" OnClick="lbReject_Click">
                                <i class="fa fa-ban"></i>  Reject
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
