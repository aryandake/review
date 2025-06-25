<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Submissions.Admin_CommonSubmissionEditDelete" Async="true" Title="Common Submission Edit Delete" CodeBehind="CommonSubmissionEditDelete.aspx.cs" %>

<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="custom" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="a" Namespace="Fiction2Fact.Controls" Assembly="RequiredIfValidatorRadioButtonList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/DateValidator.js") %>'>
    </script>

    <script type="text/javascript">

        function showhideTypeBased(rbltypeName) {
            var rb = document.getElementById(rbltypeName + '_0');
            var elem = document.getElementById('FixedDateBaseSection');
            var elem1 = document.getElementById('EventBasedSection');
            var hf = document.getElementById('hfFixedOrEvent');

            if (rb.checked) {
                hf.value = 'F';
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
                elem1.style.display = 'none';
                elem1.style.visibility = 'hidden';
            }
            else {
                hf.value = 'E';
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
                elem1.style.display = 'block';
                elem1.style.visibility = 'visible';
            }
        }

        function onClientEditClick() {
            document.getElementById('<%=hdfClientOperation.ClientID%>').value = "Edit";
        }

        function onClientDeleteClick() {
            if (!confirm("Are you sure that you want to delete this record? " +
                " This shall also delete the checklist entries.")) {
                return false;
            }
            document.getElementById('<%=hdfClientOperation.ClientID%>').value = "Delete";
        }

        function showhideOtherFrequencyPanels(rblName) {
            var rb = document.getElementById(rblName + '_0');
            var flag = 0;
            var elem = document.getElementById('OnlyOnceSection');
            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }


            rb = document.getElementById(rblName + '_2');
            elem = document.getElementById('WeeklySection');

            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }

            rb = document.getElementById(rblName + '_3');
            elem = document.getElementById('MonthlySection');
            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }

            rb = document.getElementById(rblName + '_4');
            elem = document.getElementById('QuarterSection');
            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }

            rb = document.getElementById(rblName + '_5');
            elem = document.getElementById('FirstHalfSection');
            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }

            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }

            rb = document.getElementById(rblName + '_6');
            elem = document.getElementById('YearSection');

            if (rb.checked) {

                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }

            rb = document.getElementById(rblName + '_7');
            elem = document.getElementById('FortnightlySection');
            if (rb.checked) {
                elem.style.display = 'block';
                elem.style.visibility = 'visible';
            }
            else {
                elem.style.display = 'none';
                elem.style.visibility = 'hidden';
            }
        }

        function compareEffectiveDate(source, arguments) {
            try {
                var EffectiveDate = document.getElementById('ctl00_ContentPlaceHolder1_fvSubmissionMaster_txtEffectiveDate');
                var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurDate');

                if (compare2Dates(SystemDate, EffectiveDate) == 0) {
                    arguments.IsValid = false;
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
        function onClientDeleteClick() {
            if (!confirm('Are you sure that you want to delete this record?')) return false;
            return true;
        }
        function onClientDeleteSubClick() {
            if (!confirm('Are you sure that you want to delete this record?')) return false;
            document.getElementById('<%=hdfClientOperation.ClientID%>').value = "Delete";
        }
    </script>

    <asp:HiddenField ID="hfSTMID" runat="server" />
    <input id="hfFixedOrEvent" type="hidden" />
    <asp:HiddenField ID="hfType" runat="server" />
    <asp:HiddenField ID="hfEventId" runat="server" />
    <asp:HiddenField ID="hdfClientOperation" runat="server" />
    <asp:HiddenField ID="hfSelectedRecord" runat="server" />
    <asp:HiddenField ID="hfEPId" runat="server" />
    <asp:HiddenField ID="hfSubFileId" runat="server" />
    <asp:HiddenField ID="hfCurDate" runat="server" />
    <asp:HiddenField ID="hfUserType" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">View Submissions</h4>
                        <asp:Label ID="lblInfo" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
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
                    <asp:MultiView ID="mvMultiView" runat="server">
                        <asp:View ID="vwGrid" runat="server">

                            <div class="row">
                                <div class="col-md-6 mb-3">
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
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Reporting to:</label>
                                    <asp:DropDownList ID="ddlSegment" runat="server" DataValueField="SSM_ID" DataTextField="SSM_NAME"
                                        AppendDataBoundItems="True" CssClass="form-select">
                                        <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Type:</label>
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select">
                                        <asp:ListItem Value=""> All</asp:ListItem>
                                        <asp:ListItem Value="F"> Fixed Date</asp:ListItem>
                                        <asp:ListItem Value="E"> Event Based</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Tracked By:</label>
                                    <asp:DropDownList ID="ddlSubType" AppendDataBoundItems="true" runat="server" DataValueField="STM_ID"
                                        DataTextField="STM_TYPE" CssClass="form-select">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-12 mb-3">
                                    <label class="form-label">Depends On Event:</label>
                                    <asp:DropDownList ID="ddlEventForSearch" AutoPostBack="true" AppendDataBoundItems="true"
                                        runat="server" OnSelectedIndexChanged="ddlEventForSearch_SelectedIndexChanged"
                                        DataValueField="EM_ID" DataTextField="EM_EVENT_NAME" CssClass="form-select">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-12 mb-3">
                                    <label class="form-label">Select Agendas:</label>
                                    <div class="custom-checkbox-table">
                                        <asp:CheckBoxList ID="cblAssociatedWith" RepeatColumns="6" CssClass="form-control" runat="server" DataTextField="EP_NAME"
                                            DataValueField="EP_ID" AppendDataBoundItems="true">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Status:</label>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                                        <asp:ListItem Value="Active">Active</asp:ListItem>
                                        <asp:ListItem Value="Inactive">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Reporting Function:</label>
                                    <asp:DropDownList ID="ddlReportDept" AppendDataBoundItems="true" runat="server" DataValueField="SRD_ID"
                                        DataTextField="SRD_NAME" CssClass="form-select">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="mt-2 text-center">
                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnSearch" runat="server" Text="Search" ValidationGroup="SEARCH"
                                    AccessKey="s" OnClick="btnSearch_Click">
                                    <i class="fa fa-search"></i> Search                          
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExportToExcel" Visible="false" runat="server" CssClass="btn btn-outline-secondary"
                                    Text="Export to Excel" OnClick="btnExportToExcel_Click">
                                    <i class="fa fa-download"></i> Export to Excel               
                                </asp:LinkButton>
                            </div>
                            <div class="mt-2">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvSubmissionMaster" runat="server" AutoGenerateColumns="False"
                                        OnSorting="gvSubmissionMaster_Sorting" DataKeyNames="SM_ID" AllowSorting="true"
                                        GridLines="both" CellPadding="4" OnSelectedIndexChanged="gvSubmissionMaster_SelectedIndexChanged"
                                        CssClass="table table-bordered footable" AlternatingRowStyle-CssClass="alt" OnRowCreated="OnRowCreated">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Sr No.
                                               
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrNo" Text='<%# Container.DataItemIndex + 1 %>' runat="server"> 
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label Visible="false" ID="lbFrequency" Text='<%# Eval("SM_FREQUENCY") %>' runat="server"> 
                                                    </asp:Label>
                                                    <asp:HiddenField ID="hfEPId" Value='<%# Eval("SM_EP_ID") %>' runat="server"></asp:HiddenField>
                                                    <asp:HiddenField ID="hfEmId" Value='<%# Eval("SM_EM_ID") %>' runat="server"></asp:HiddenField>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label Visible="false" ID="lbEPId" Text='<%# Eval("SM_EP_ID") %>' runat="server"> 
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label Visible="false" ID="lbEmId" Text='<%# Eval("SM_EM_ID") %>' runat="server"> 
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- Added by Prajakta Salvi--%>
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-success btn-circle"
                                                        OnClientClick="onClientEditClick()">
                                                        <i class="fa fa-pen"></i>	                            
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" Visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-danger btn-circle"
                                                        OnClientClick=" return onClientDeleteSubClick()">
                                                        <i class="fa fa-trash"></i>                                 
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="true" HeaderText="Submission Id" SortExpression="SM_ID">
                                                <ItemTemplate>
                                                    <asp:Label Visible="true" ID="lbsmId" Text='<%# Eval("SM_ID") %>' runat="server"> 
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="LEM_NAME" HeaderText="Line of Business" SortExpression="LEM_NAME" Visible="false" />

                                            <asp:TemplateField HeaderText="Fixed Date/Event Based" SortExpression="SM_SUB_TYPE">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbType" Text='<%# Eval("SM_SUB_TYPE").ToString()=="E"?"Event":"Fixed" %>' runat="server"> 
                                                    </asp:Label>
                                                    <asp:HiddenField ID="hfSubType" Value='<%# Eval("SM_SUB_TYPE") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Effective From" SortExpression="SM_EFFECTIVE_DT">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Width="75px" ID="lblEffDate" Text='<%# Eval("SM_EFFECTIVE_DT" , "{0:dd-MMM-yyyy}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Notification/Email date" SortExpression="SM_CIRCULAR_DATE">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Width="75px" ID="lblSM_CIRCULAR_DATE" Text='<%# Eval("SM_CIRCULAR_DATE" , "{0:dd-MMM-yyyy}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reporting to">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Width="75px" ID="lblSegment" Text='<%#LoadSubmissionSegmentName(Eval("SM_ID"))%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="SRD_NAME" HeaderText="Reporting Function" SortExpression="SRD_NAME" />
                                            <%--<asp:BoundField DataField="SM_PERTICULARS" HeaderText="Particulars" SortExpression="SM_PERTICULARS" />
                                <asp:BoundField DataField="SM_BRIEF_DESCRIPTION" HeaderText="Brief Description" SortExpression="SM_BRIEF_DESCRIPTION" />--%>
                                            <asp:TemplateField HeaderText="Reference Circular / Notification / Act" SortExpression="SM_ACT_REG_SECTION">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReference" runat="server" Text='<%#Eval("SM_ACT_REG_SECTION").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Section/Clause" SortExpression="SM_SECTION_CLAUSE">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSection" runat="server" Text='<%#Eval("SM_SECTION_CLAUSE").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Particulars" SortExpression="SM_PERTICULARS">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblParticulare" runat="server" Text='<%#Eval("SM_PERTICULARS").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Brief Description" SortExpression="SM_BRIEF_DESCRIPTION">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBriefDescription" runat="server" Text='<%#Eval("SM_BRIEF_DESCRIPTION").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="SM_FREQUENCY" HeaderText="Frequency" SortExpression="SM_FREQUENCY" />
                                            <asp:BoundField DataField="SM_TO_BE_ESC" HeaderText="To Be Escalated" SortExpression="SM_TO_BE_ESC" />
                                            <asp:BoundField DataField="SM_L0_ESCALATION_DAYS" HeaderText="Reminder days for reporting function"
                                                SortExpression="SM_L0_ESCALATION_DAYS" />
                                            <asp:BoundField DataField="SM_L1_ESCALATION_DAYS" HeaderText="Escalation 1 Days"
                                                SortExpression="SM_L1_ESCALATION_DAYS" />
                                            <asp:BoundField DataField="SM_L2_ESCALATION_DAYS" HeaderText="Escalation 2 Days"
                                                SortExpression="SM_L2_ESCALATION_DAYS" />
                                            <asp:BoundField DataField="SM_ONLY_ONCE_FROM_DATE" HeaderText="Adhoc From Date"
                                                SortExpression="SM_ONLY_ONCE_FROM_DATE" />
                                            <asp:BoundField DataField="SM_ONLY_ONCE_TO_DATE" HeaderText="Adhoc To Date" SortExpression="SM_ONLY_ONCE_TO_DATE" />
                                            <asp:BoundField DataField="SM_MONTHLY_DUE_DATE_FROM" HeaderText="Monthly Internal Due Date"
                                                SortExpression="SM_MONTHLY_DUE_DATE_FROM" />
                                            <asp:BoundField DataField="SM_MONTHLY_DUE_DATE_TO" HeaderText="Monthly Regulatory Due Date"
                                                SortExpression="SM_MONTHLY_DUE_DATE_TO" />
                                            <asp:BoundField DataField="SM_Q1_DUE_DATE_FROM" HeaderText="Q1 Internal Due Date" SortExpression="SM_Q1_DUE_DATE_FROM" />
                                            <asp:BoundField DataField="SM_Q1_DUE_DATE_TO" HeaderText="Q1 Regulatory Due Date" SortExpression="SM_Q1_DUE_DATE_TO" />
                                            <asp:BoundField DataField="SM_Q2_DUE_DATE_FROM" HeaderText="Q2 Internal Due Date" SortExpression="SM_Q2_DUE_DATE_FROM" />
                                            <asp:BoundField DataField="SM_Q2_DUE_DATE_TO" HeaderText="Q2 Regulatory Due Date" SortExpression="SM_Q2_DUE_DATE_TO" />
                                            <asp:BoundField DataField="SM_Q3_DUE_DATE_FROM" HeaderText="Q3 Internal Due Date" SortExpression="SM_Q3_DUE_DATE_FROM" />
                                            <asp:BoundField DataField="SM_Q3_DUE_DATE_TO" HeaderText="Q3 Regulatory Due Date" SortExpression="SM_Q3_DUE_DATE_TO" />
                                            <asp:BoundField DataField="SM_Q4_DUE_DATE_FROM" HeaderText="Q4 Internal Due Date" SortExpression="SM_Q4_DUE_DATE_FROM" />
                                            <asp:BoundField DataField="SM_Q4_DUE_DATE_TO" HeaderText="Q4 Regulatory Due Date" SortExpression="SM_Q4_DUE_DATE_TO" />
                                            <asp:BoundField DataField="SM_FIRST_HALF_YR_DUE_DATE_FROM" HeaderText="First Half Year Internal Due Date"
                                                SortExpression="SM_FIRST_HALF_YR_DUE_DATE_FROM" />
                                            <asp:BoundField DataField="SM_FIRST_HALF_YR_DUE_DATE_TO" HeaderText="First Half Year Regulatory Due Date"
                                                SortExpression="SM_FIRST_HALF_YR_DUE_DATE_TO" />
                                            <asp:BoundField DataField="SM_SECOND_HALF_YR_DUE_DATE_FROM" HeaderText="Second Half Year Internal Due Date"
                                                SortExpression="SM_SECOND_HALF_YR_DUE_DATE_FROM" />
                                            <asp:BoundField DataField="SM_SECOND_HALF_YR_DUE_DATE_TO" HeaderText="Second Half Year Regulatory Due Date"
                                                SortExpression="SM_SECOND_HALF_YR_DUE_DATE_FROM" />
                                            <asp:BoundField DataField="SM_YEARLY_DUE_DATE_FROM" HeaderText="Yearly Internal Due Date"
                                                SortExpression="SM_YEARLY_DUE_DATE_FROM" />
                                            <asp:BoundField DataField="SM_YEARLY_DUE_DATE_TO" HeaderText="Yearly Regulatory Due Date"
                                                SortExpression="SM_YEARLY_DUE_DATE_TO" />
                                            <asp:BoundField DataField="SM_FIRST_FORTNIGHTLY_DUE_FROM_DATE" HeaderText="First Fortnightly Internal Due Date"
                                                SortExpression="SM_FIRST_FORTNIGHTLY_DUE_FROM_DATE" />
                                            <asp:BoundField DataField="SM_FIRST_FORTNIGHTLY_DUE_TO_DATE" HeaderText="First Fortnightly Regulatory Due Date"
                                                SortExpression="SM_FIRST_FORTNIGHTLY_DUE_TO_DATE" />
                                            <asp:BoundField DataField="SM_FIRST_FORTNIGHTLY_DUE_FROM_DATE" HeaderText="Second Fortnightly Internal Due Date"
                                                SortExpression="SM_FIRST_FORTNIGHTLY_DUE_FROM_DATE" />
                                            <asp:BoundField DataField="SM_SECOND_FORTNIGHTLY_DUE_TO_DATE" HeaderText="Second Fortnightly Regulatory Due Date"
                                                SortExpression="SM_SECOND_FORTNIGHTLY_DUE_TO_DATE" />
                                            <asp:BoundField DataField="SM_START_NO_OF_DAYS" HeaderText="Start No. of Days" SortExpression="SM_START_NO_OF_DAYS" />
                                            <asp:BoundField DataField="SM_END_NO_OF_DAYS" HeaderText="End No. of Days" SortExpression="SM_END_NO_OF_DAYS" />
                                            <asp:BoundField DataField="SM_STATUS" HeaderText="Status" SortExpression="SM_STATUS" />
                                            <%--<asp:BoundField DataField="SM_EFFECTIVE_DT" HeaderText="Effective Date" SortExpression="SM_EFFECTIVE_DT" />--%>
                                            <asp:TemplateField HeaderText="Finance Approval" SortExpression="FSApprovalReq">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFSApprovalReq" Text='<%# Eval("FSApprovalReq").ToString()%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" Text='<%#Eval("Status")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </asp:View>
                        <asp:View ID="vwForm" runat="server">
                            <asp:FormView ID="fvSubmissionMaster" runat="server" DefaultMode="Edit" AllowPaging="false"
                                DataKeyNames="SM_ID" OnDataBound="fvSubmissionMaster_DataBound" Width="100%">
                                <EditItemTemplate>
                                    <div class="row" style="display: none; visibility: hidden">
                                        <div class="col-md-4 mb-3" style="display: none; visibility: hidden">
                                            <label class="form-label">Line of Business: <span class="text-danger">*</span></label>
                                            <asp:DropDownList ID="ddlLOB" CssClass="form-select" runat="server" DataTextField="LEM_NAME"
                                                DataValueField="LEM_ID" AppendDataBoundItems="True" ToolTip="Line of Business">
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please select Line of Business."></asp:RequiredFieldValidator>--%>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Tracked By: <span class="text-danger">*</span></label>
                                            <asp:DropDownList ID="ddlSubType" AppendDataBoundItems="true" runat="server" DataValueField="STM_ID"
                                                DataTextField="STM_TYPE" CssClass="form-select">
                                                <asp:ListItem Value="">All</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvSubType" runat="server" ControlToValidate="ddlSubType" CssClass="text-danger"
                                                ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                                ErrorMessage="Please select Submission Type."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Reporting Department:</label>
                                            <asp:DropDownList ID="ddlReportDept" AppendDataBoundItems="true" CssClass="form-select"
                                                runat="server" DataValueField="SRD_ID" DataTextField="SRD_NAME">
                                                <asp:ListItem Value="0">(Select)</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-4 mb-3" style="display: none;">
                                            <label class="form-label">Reporting Department Owners:</label>
                                            <asp:CheckBoxList ID="cbDeptOwner" RepeatColumns="5" AppendDataBoundItems="true"
                                                runat="server" DataValueField="SRDOM_ID" DataTextField="SRDOM_EMP_NAME">
                                            </asp:CheckBoxList>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Effective Date: <span class="text-danger">*</span></label>
                                            <div class="input-group">
                                                <F2FControls:F2FTextBox ID="txtEffectiveDate" CssClass="form-control" runat="server"
                                                    Columns="15" MaxLength="20"></F2FControls:F2FTextBox>
                                                <asp:ImageButton ID="imgEffectiveDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />

                                            </div>
                                            <ajaxToolkit:CalendarExtender ID="ceEffectiveDate" runat="server" PopupButtonID="imgEffectiveDate"
                                                TargetControlID="txtEffectiveDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="rfvEffctDate" runat="server" ControlToValidate="txtEffectiveDate" CssClass="text-danger"
                                                ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                                ErrorMessage="Please select Effective Date."></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revEffectiveDate" runat="server" ControlToValidate="txtEffectiveDate" CssClass="text-danger"
                                                ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                                ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                            <%-- Commented By Milan Yadav on 25Apr2016--%>
                                            <%-- <asp:CustomValidator ID="cvEffectiveDate" runat="server" ClientValidationFunction="compareEffectiveDate"
                                    ControlToValidate="txtEffectiveDate" ErrorMessage="Effective Date cannot be less than current date." CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="SaveSubmissionDetails">
                                </asp:CustomValidator>--%>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Reporting to:</label>
                                            <div class="custom-checkbox-table">
                                                <asp:CheckBoxList ID="cblSegment" runat="server" CssClass="form-control" RepeatColumns="7" DataTextField="SSM_NAME"
                                                    DataValueField="SSM_ID" Enabled="false">
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Priority: <span class="text-danger">*</span></label>
                                            <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-select">
                                                <asp:ListItem Value="">Select One</asp:ListItem>
                                                <asp:ListItem Value="H">High</asp:ListItem>
                                                <asp:ListItem Value="M">Medium</asp:ListItem>
                                                <asp:ListItem Value="L">Low</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvPriority" runat="server" ControlToValidate="ddlPriority" CssClass="text-danger"
                                                ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                                ErrorMessage="Please select Submission priority."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Reference Circular / Notification / Act : <span class="text-danger">*</span></label>
                                            <F2FControls:F2FTextBox ID="txtReference" runat="server" CssClass="form-control" TextMode="MultiLine"
                                                Rows="3" MaxLength="500" Text='<%# Eval("SM_ACT_REG_SECTION")%>'></F2FControls:F2FTextBox>
                                            <%--<F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtReference" />--%>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtReference" CssClass="text-danger"
                                                ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                                ErrorMessage="Please enter Reference Circular / Notification / Act."></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="SaveSubmissionDetails"
                                                ControlToValidate="txtEffectiveDate" CssClass="text-danger" ErrorMessage="Report Filing Effective Date cannot be lower than Notification/Email date."
                                                Display="Dynamic" ValidateEmptyText="true" ClientValidationFunction="CompareFilingEffectiveDateSytemDate"> 
                                            </asp:CustomValidator>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Section/Clause :<span class="text-danger">*</span></label>
                                            <F2FControls:F2FTextBox ID="txtSection" runat="server" CssClass="form-control" TextMode="MultiLine"
                                                Rows="3" MaxLength="500" Text='<%# Eval("SM_SECTION_CLAUSE")%>'>
                                            </F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtSection" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSection" CssClass="text-danger"
                                                ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                                ErrorMessage="Please enter Section/Clause."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Particulars: <span class="text-danger">*</span></label>
                                            <F2FControls:F2FTextBox ID="txtPerticulars" TextMode="MultiLine" CssClass="form-control"
                                                runat="server" Columns="50" Rows="3" MaxLength="500" Text='<%# Eval("SM_PERTICULARS")%>'></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtPerticulars" />
                                            <asp:RequiredFieldValidator ID="rfvPerticulars" runat="server" ControlToValidate="txtPerticulars" CssClass="text-danger"
                                                ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                                ErrorMessage="Please enter Particulars."></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revPerticulars" ControlToValidate="txtPerticulars" CssClass="text-danger"
                                                Display="Dynamic" Text="Exceeding 500 characters" ValidationExpression="^[\s\S]{0,500}$"
                                                runat="server" SetFocusOnError="True" ValidationGroup="SaveSubmissionDetails" />
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Brief Description: <span class="text-danger">*</span></label>
                                            <F2FControls:F2FTextBox ID="txtDescription" CssClass="form-control" TextMode="MultiLine"
                                                Columns="70" Rows="3" MaxLength="500" runat="server" Text='<%# Eval("SM_BRIEF_DESCRIPTION")%>'>
                                            </F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtDescription" />
                                            <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" CssClass="text-danger"
                                                ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                                ErrorMessage="Please enter Description."></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revDescription" ControlToValidate="txtDescription" CssClass="text-danger"
                                                Display="Dynamic" Text="Exceeding 500 characters" ValidationExpression="^[\s\S]{0,500}$"
                                                runat="server" SetFocusOnError="True" ValidationGroup="SaveSubmissionDetails" />
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Reminder days for reporting function:</label>
                                            <F2FControls:F2FTextBox ID="txtlevel0" runat="server" MaxLength="2" CssClass="form-control" Text='<%# Eval("SM_L0_ESCALATION_DAYS")%>'
                                                Columns="4"></F2FControls:F2FTextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="ftelevel0" runat="server" TargetControlID="txtlevel0"
                                                FilterType="Numbers"></ajaxToolkit:FilteredTextBoxExtender>

                                            <asp:RequiredFieldValidator ID="rfvlvl0Esc" runat="server" ControlToValidate="txtlevel0"
                                                ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger" ErrorMessage="Please enter level 0 escalation days."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">To be Escalated: <span class="text-danger">*</span></label>
                                            <div class="custom-checkbox-table">
                                                <asp:RadioButtonList ID="rblEscalate" runat="server" RepeatColumns="2" CssClass="form-control" SelectedValue='<%#Eval("SM_TO_BE_ESC") %>'>
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfvEscalate" runat="server" ControlToValidate="rblEscalate" CssClass="text-danger"
                                                ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                                ErrorMessage="Please select any One."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Level 1 Escalation Days:</label>

                                            <F2FControls:F2FTextBox ID="txtlevel1" CssClass="form-control" runat="server" MaxLength="4"
                                                Columns="4" Text='<%# Eval("SM_L1_ESCALATION_DAYS")%>'></F2FControls:F2FTextBox>
                                            <%--  <asp:RequiredFieldValidator ID="rfvL1EscDays" runat="server" ControlToValidate="txtlevel1" CssClass="text-danger"
                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please enter Level 1 Days."></asp:RequiredFieldValidator>--%>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="ftelevel1" runat="server" TargetControlID="txtlevel1" FilterType="Numbers"></ajaxToolkit:FilteredTextBoxExtender>

                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Level 2 Escalation Days:</label>
                                            <F2FControls:F2FTextBox ID="txtlevel2" runat="server" CssClass="form-control" MaxLength="4"
                                                Columns="4" Text='<%# Eval("SM_L2_ESCALATION_DAYS")%>'></F2FControls:F2FTextBox>
                                            <%--<asp:RequiredFieldValidator ID="rfvL2EscDays" runat="server" ControlToValidate="txtlevel2" CssClass="text-danger"
                                    ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please enter Level 2 Days."></asp:RequiredFieldValidator>--%>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="fteleve2" runat="server" TargetControlID="txtlevel2" FilterType="Numbers"></ajaxToolkit:FilteredTextBoxExtender>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Type: <span class="text-danger">*</span></label>
                                            <div class="custom-checkbox-table">
                                                <asp:RadioButtonList ID="rblType" Enabled="false" runat="server" CssClass="form-control" RepeatColumns="7"
                                                    SelectedValue='<%#Eval("SM_SUB_TYPE") %>'>
                                                    <asp:ListItem Value="F"> Fixed Date</asp:ListItem>
                                                    <asp:ListItem Value="E"> Event Based</asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="rblType" CssClass="text-danger" ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                                    ErrorMessage="Please select Type."></asp:RequiredFieldValidator>
                                                <asp:HiddenField ID="hfFreq" Value='<%# Eval("SM_FREQUENCY") %>' runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Finance Approval Required: <span class="text-danger">*</span></label>
                                            <asp:DropDownList ID="ddlFSAppReq" CssClass="form-select" runat="server" ToolTip="FS Approval Required"
                                                SelectedValue='<%# Eval("SM_IS_FS_APPROVAL_REQUIRED")%>'>
                                                <asp:ListItem Value="">(Select)</asp:ListItem>
                                                <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                <asp:ListItem Value="N">No</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlFSAppReq"
                                                CssClass="text-danger" ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                                ErrorMessage="Please select Finance Approval Required."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Notification/Email date :</label>
                                            <div class="input-group">
                                                <F2FControls:F2FTextBox ID="txtCircularDate" runat="server" Columns="15" MaxLength="20"
                                                    CssClass="form-control"></F2FControls:F2FTextBox>
                                                <asp:ImageButton ID="imgCircularDate" runat="server" AlternateText="Click to show calendar"
                                                    CssClass="custom-calendar-icon" ImageUrl="../../Content/images/legacy/calendar.jpg" />&nbsp;
        <ajaxToolkit:CalendarExtender ID="CalendarExtender6" runat="server" PopupButtonID="imgCircularDate"
            TargetControlID="txtCircularDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="EventBasedSection" style="display: none; visibility: hidden">
                                        <div class="row">
                                            <div class="col-md-12 mb-3">
                                                <label class="form-label">Depends On Event:</label>
                                                <asp:DropDownList ID="ddlEvent" AppendDataBoundItems="true" runat="server" OnSelectedIndexChanged="ddlEvent_SelectedIndexChanged"
                                                    AutoPostBack="true" DataValueField="EM_ID" DataTextField="EM_EVENT_NAME" CssClass="form-select">
                                                    <asp:ListItem Value="">All</asp:ListItem>
                                                </asp:DropDownList>
                                                <a:RequiredIfValidatorRadioButtonList ID="RequiredIfValidatorRadioButtonList5" runat="server"
                                                    TriggerIndex="1" ControlToCompare="rblType" ControlToValidate="ddlEvent" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Select Event Type">
                            *
                                                </a:RequiredIfValidatorRadioButtonList>
                                            </div>
                                            <div class="col-md-12 mb-3">
                                                <label class="form-label">Agenda:</label>
                                                <div class="custom-checkbox-table">
                                                    <asp:RadioButtonList ID="rblAssociatedWith" runat="server" CssClass="form-control" RepeatColumns="7" AppendDataBoundItems="true"
                                                        DataTextField="EP_NAME" DataValueField="EP_ID">
                                                    </asp:RadioButtonList>
                                                </div>
                                                <asp:RequiredFieldValidator ID="rfvAgenda" runat="server" ControlToValidate="rblAssociatedWith" CssClass="text-danger" ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                                    ErrorMessage="Please select Agenda."></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Start Date (No of Days before / after the event):</label>
                                                <F2FControls:F2FTextBox ID="txtStartDays" Text='<%# Eval("SM_START_NO_OF_DAYS")%>' runat="server"
                                                    MaxLength="4" Columns="4" CssClass="form-control"></F2FControls:F2FTextBox>
                                                <F2FControls:F2FFilteredNumber runat="server" TargetControlID="txtStartDays" FilterType="Numbers, Custom" ValidChars="-" />
                                                <a:RequiredIfValidatorRadioButtonList ID="rfvStart" runat="server" TriggerIndex="1"
                                                    ControlToCompare="rblType" ControlToValidate="txtStartDays" Display="Dynamic"
                                                    EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter Start No of Days" CssClass="text-danger">
        
                                                </a:RequiredIfValidatorRadioButtonList>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">End Date (No of Days before / after the event):</label>
                                                <F2FControls:F2FTextBox ID="txtEndDays" Text='<%# Eval("SM_END_NO_OF_DAYS")%>' runat="server"
                                                    MaxLength="4" Columns="4" CssClass="form-control"></F2FControls:F2FTextBox>
                                                <F2FControls:F2FFilteredNumber runat="server" TargetControlID="txtEndDays" FilterType="Numbers, Custom" ValidChars="-" />
                                                <a:RequiredIfValidatorRadioButtonList ID="rfvEndDate" runat="server" TriggerIndex="1"
                                                    ControlToCompare="rblType" ControlToValidate="txtEndDays" Display="Dynamic" EnableClientScript="true" CssClass="text-danger"
                                                    ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter End No of Days">
        
                                                </a:RequiredIfValidatorRadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="FixedDateBaseSection" style="display: none; visibility: hidden">
                                        <div class="row">
                                            <div class="col-md-12 mb-3">
                                                <label class="form-label">Frequency:</label>
                                                <div class="custom-checkbox-table">
                                                    <asp:RadioButtonList ID="rblFrequency" runat="server" CssClass="form-control" RepeatColumns="8" Enabled="false">
                                                        <asp:ListItem Value="Only Once">Adhoc</asp:ListItem>
                                                        <asp:ListItem Text="Daily" Value="Daily">Daily</asp:ListItem>
                                                        <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                                                        <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                                        <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>
                                                        <asp:ListItem Value="Half Yearly">Half Yearly</asp:ListItem>
                                                        <asp:ListItem Value="Yearly">Yearly</asp:ListItem>
                                                        <asp:ListItem Value="Fortnightly">Fortnightly</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                                <a:RequiredIfValidatorRadioButtonList ID="RequiredIfValidatorRadioButtonList2" runat="server"
                                                    TriggerIndex="0" ControlToCompare="rblType" ControlToValidate="rblFrequency"
                                                    Display="Dynamic" EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" CssClass="text-danger"
                                                    ErrorMessage="Please Select Frequency">
        
                                                </a:RequiredIfValidatorRadioButtonList>
                                            </div>
                                        </div>
                                        <div id="OnlyOnceSection" style="display: none; visibility: hidden">
                                            <div class="row">
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Adhoc From date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtOnceFromDate" CssClass="form-control" runat="server"
                                                            MaxLength="200" Text='<%# Eval("SM_ONLY_ONCE_FROM_DATE", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgMFD" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />

                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivMontthlyFrom" runat="server" TriggerIndex="0" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtOnceFromDate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revMFD" runat="server" ControlToValidate="txtOnceFromDate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="ceFMonthlyFromDate" runat="server" PopupButtonID="imgMFD"
                                                        TargetControlID="txtOnceFromDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Adhoc To date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtOnceToDate" runat="server" CssClass="form-control"
                                                            MaxLength="200" Text='<%# Eval("SM_ONLY_ONCE_TO_DATE", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgOnceTODate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rifOnceToDate" runat="server" TriggerIndex="0" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtOnceToDate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter date">
                        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revOnceDateTo" runat="server" ControlToValidate="txtOnceToDate"
                                                        ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" CssClass="text-danger"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="ceOnceTodate" runat="server" PopupButtonID="imgOnceTODate"
                                                        TargetControlID="txtOnceToDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>

                                                </div>
                                            </div>
                                        </div>
                                        <div id="WeeklySection" style="display: none; visibility: hidden">
                                            <div class="row">
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Weekly From Day:</label>
                                                    <asp:DropDownList ID="ddlFromWeekDays" CssClass="form-select" runat="server"
                                                        SelectedValue='<%# Eval("SM_WEEKLY_DUE_DATE_FROM")%>'>
                                                        <asp:ListItem Value="">(None)</asp:ListItem>
                                                        <asp:ListItem Value="Monday">Monday</asp:ListItem>
                                                        <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                                                        <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                                                        <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                                                        <asp:ListItem Value="Friday">Friday</asp:ListItem>
                                                        <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                                                        <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rifvFromWeekDays" runat="server" TriggerIndex="2"
                                                        ControlToCompare="rblFrequency" ControlToValidate="ddlFromWeekDays" Display="Dynamic" CssClass="text-danger"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please select Weekly From day">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Weekly To Day:</label>
                                                    <asp:DropDownList ID="ddlTOWeekDays" CssClass="form-select" runat="server"
                                                        SelectedValue='<%# Eval("SM_WEEKLY_DUE_DATE_TO")%>'>
                                                        <asp:ListItem Value="">(None)</asp:ListItem>
                                                        <asp:ListItem Value="Monday">Monday</asp:ListItem>
                                                        <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                                                        <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                                                        <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                                                        <asp:ListItem Value="Friday">Friday</asp:ListItem>
                                                        <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                                                        <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rifvTOWeekDays" runat="server" TriggerIndex="2"
                                                        ControlToCompare="rblFrequency" ControlToValidate="ddlTOWeekDays" Display="Dynamic" CssClass="text-danger"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please select Weekly To day">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="MonthlySection" style="display: none; visibility: hidden">
                                            <div class="row">
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Monthly From Date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtMonthlyFromDate" CssClass="form-control" runat="server"
                                                            MaxLength="200" Text='<%# Eval("SM_MONTHLY_DUE_DATE_FROM", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="ImageButton1" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="RequiredIfValidatorRadioButtonList1" runat="server" CssClass="text-danger"
                                                        TriggerIndex="3" ControlToCompare="rblFrequency" ControlToValidate="txtMonthlyFromDate"
                                                        Display="Dynamic" EnableClientScript="true" ValidationGroup="SaveSubmissionDetails"
                                                        ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtMonthlyFromDate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="ImageButton1"
                                                        TargetControlID="txtMonthlyFromDate" Format="dd"></ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Monthly To Date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtMonthlyTodate" CssClass="form-control" runat="server"
                                                            MaxLength="200" Text=' <%# Eval("SM_MONTHLY_DUE_DATE_TO", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgMTD" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivMonthlyTo" runat="server" TriggerIndex="3" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtMonthlyTodate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revMonthlyToDate" runat="server" ControlToValidate="txtMonthlyTodate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="ceMTD" runat="server" PopupButtonID="imgMTD" TargetControlID="txtMonthlyTodate"
                                                        Format="dd"></ajaxToolkit:CalendarExtender>
                                                </div>
                                            </div>

                                        </div>
                                        <div id="QuarterSection" style="display: none; visibility: hidden">
                                            <div class="row">
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Q1 From Date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtQ1fromDate" CssClass="form-control" runat="server"
                                                            MaxLength="200" Text='<%# Eval("SM_Q1_DUE_DATE_FROM", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgFromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivQ1From" runat="server" TriggerIndex="4" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtQ1fromDate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revCircularDate" runat="server" ControlToValidate="txtQ1fromDate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="ceDate" runat="server" PopupButtonID="imgFromDate"
                                                        TargetControlID="txtQ1fromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Q1 To Date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtQ1ToDate" CssClass="form-control" runat="server" MaxLength="200"
                                                            Text='<%# Eval("SM_Q1_DUE_DATE_TO", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgQ1toDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivQ1To" runat="server" TriggerIndex="4" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtQ1ToDate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revQuarterDateto" runat="server" ControlToValidate="txtQ1ToDate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>&nbsp;
           
                           

                        <ajaxToolkit:CalendarExtender ID="ce1" runat="server" PopupButtonID="imgQ1toDate"
                            TargetControlID="txtQ1ToDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Q2 From Date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtQ2FromDate" CssClass="form-control" runat="server"
                                                            MaxLength="200" Text='<%# Eval("SM_Q2_DUE_DATE_FROM", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgq2FromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivQ2T0" runat="server" TriggerIndex="4" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtQ2FromDate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtQ2FromDate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="ce2" runat="server" PopupButtonID="imgq2FromDate"
                                                        TargetControlID="txtQ2FromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Q2 To Date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtQ2ToDate" CssClass="form-control" runat="server" MaxLength="200"
                                                            Text='<%# Eval("SM_Q2_DUE_DATE_TO", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgQ2todate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rfvQ2To" runat="server" TriggerIndex="4" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtQ2ToDate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revQ2toDate" runat="server" ControlToValidate="txtQ2ToDate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="ce3" runat="server" PopupButtonID="imgQ2todate"
                                                        TargetControlID="txtQ2ToDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Q3 From Date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtQ3FromDate" CssClass="form-control" runat="server"
                                                            MaxLength="200" Text='<%# Eval("SM_Q3_DUE_DATE_FROM", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgQ3FromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivQ3From" runat="server" TriggerIndex="4" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtQ3FromDate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revQ3Date" runat="server" ControlToValidate="txtQ3FromDate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="ce4" runat="server" PopupButtonID="imgQ3FromDate"
                                                        TargetControlID="txtQ3FromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Q3 To Date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtQ3Todate" CssClass="form-control" runat="server" MaxLength="200"
                                                            Text='<%# Eval("SM_Q3_DUE_DATE_TO", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgQ3todate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />

                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="ivQ3To" runat="server" TriggerIndex="4"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtQ3Todate" Display="Dynamic" CssClass="text-danger"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revQ3Todate" runat="server" ControlToValidate="txtQ3Todate"
                                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic" CssClass="text-danger"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="ce5" runat="server" PopupButtonID="imgQ3todate"
                                                        TargetControlID="txtQ3Todate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Q4 From Date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtQ4fFromDate" CssClass="form-control" runat="server"
                                                            MaxLength="200" Text=' <%# Eval("SM_Q4_DUE_DATE_FROM", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgQ4fromdate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />

                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivQ4From" runat="server" TriggerIndex="4" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtQ4fFromDate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revQ4fromdate" runat="server" ControlToValidate="txtQ4fFromDate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="ce6" runat="server" PopupButtonID="imgQ4fromdate"
                                                        TargetControlID="txtQ4fFromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Q4 To Date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtQ4Todate" CssClass="form-control" runat="server" MaxLength="200"
                                                            Text='<%# Eval("SM_Q4_DUE_DATE_TO", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgQ4ToDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />

                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivQ4To" runat="server" TriggerIndex="4" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtQ4Todate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revQ4ToDate" runat="server" ControlToValidate="txtQ4Todate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="de7" runat="server" PopupButtonID="imgQ4ToDate"
                                                        TargetControlID="txtQ4Todate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="FirstHalfSection" style="display: none; visibility: hidden">
                                            <div class="row">
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">First Half Year From Date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtFirstHalffromDate" CssClass="form-control" runat="server"
                                                            MaxLength="200" Text='<%# Eval("SM_FIRST_HALF_YR_DUE_DATE_FROM", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgFHFromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivFHFrom" runat="server" TriggerIndex="5" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtFirstHalffromDate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revFHFDate" runat="server" ControlToValidate="txtFirstHalffromDate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="ce8" runat="server" PopupButtonID="imgFHFromDate"
                                                        TargetControlID="txtFirstHalffromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">First Half Year To Date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtFirstHalfToDate" CssClass="form-control" runat="server"
                                                            MaxLength="200" Text='<%# Eval("SM_FIRST_HALF_YR_DUE_DATE_TO", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgFHTDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />

                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivFHTo" runat="server" TriggerIndex="5" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtFirstHalfToDate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revFHTdate" runat="server" ControlToValidate="txtFirstHalfToDate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgFHTDate"
                                                        TargetControlID="txtFirstHalfToDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Second Half Year From Date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtSecondtHalffromDate" CssClass="form-control" runat="server"
                                                            MaxLength="200" Text='<%# Eval("SM_SECOND_HALF_YR_DUE_DATE_FROM", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgSHFromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />

                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivSHForm" runat="server" TriggerIndex="5" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtSecondtHalffromDate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revSHFDate" runat="server" ControlToValidate="txtFirstHalffromDate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="ce9" runat="server" PopupButtonID="imgSHFromDate"
                                                        TargetControlID="txtSecondtHalffromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Second Half Year To Date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtSecondtHalffromTo" CssClass="form-control" runat="server"
                                                            MaxLength="200" Text='<%# Eval("SM_SECOND_HALF_YR_DUE_DATE_TO", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgSHTDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivSHFrom" runat="server" TriggerIndex="5" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtSecondtHalffromTo" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revSHTdate" runat="server" ControlToValidate="txtSecondtHalffromTo" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgSHTDate"
                                                        TargetControlID="txtSecondtHalffromTo" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                                </div>
                                            </div>
                                        </div>
                                        <div id="YearSection" style="display: none; visibility: hidden">
                                            <div class="row">
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Yearly From Date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtYearlyfromDate" CssClass="form-control" runat="server"
                                                            MaxLength="200" Text='<%# Eval("SM_YEARLY_DUE_DATE_FROM", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgYearlyFromDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivYearlyFrom" runat="server" TriggerIndex="6" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtYearlyfromDate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revYearlyFDate" runat="server" ControlToValidate="txtFirstHalffromDate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" PopupButtonID="imgYearlyFromDate"
                                                        TargetControlID="txtYearlyfromDate" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Yearly To Date:</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtYearlyDateTo" CssClass="form-control" runat="server"
                                                            MaxLength="200" Text='<%# Eval("SM_YEARLY_DUE_DATE_TO", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgYearlyDate" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rfvYearlyTo" runat="server" TriggerIndex="6" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtYearlyDateTo" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please Enter Date">
        *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revYearlydate" runat="server" ControlToValidate="txtYearlyDateTo" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD-MMM." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" PopupButtonID="imgYearlyDate"
                                                        TargetControlID="txtYearlyDateTo" Format="dd-MMM"></ajaxToolkit:CalendarExtender>

                                                </div>
                                            </div>
                                        </div>
                                        <div id="FortnightlySection" style="display: none; visibility: hidden">
                                            <div class="row">
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">First Fortnightly: Start Date</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtFortnightly1FromDate" runat="server" MaxLength="50" CssClass="form-control"
                                                            Text='<%# Eval("SM_FIRST_FORTNIGHTLY_DUE_FROM_DATE", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgF1FD" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivF1FromDate" runat="server" TriggerIndex="7" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtFortnightly1FromDate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter first fortnightly: Start Date.">
                *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revF1FromDate" runat="server" ControlToValidate="txtFortnightly1FromDate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="ceF1FromDate" runat="server" PopupButtonID="imgF1FD"
                                                        TargetControlID="txtFortnightly1FromDate" Format="dd"></ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">First Fortnightly: End Date</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtFortnightly1ToDate" runat="server" MaxLength="50" CssClass="form-control"
                                                            Text='<%# Eval("SM_FIRST_FORTNIGHTLY_DUE_TO_DATE", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgF1TD" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivF1ToDate" runat="server" TriggerIndex="7" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtFortnightly1ToDate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter first fortnightly: End Date.">
                *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revF1ToDate" runat="server" ControlToValidate="txtFortnightly1ToDate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="ceF1ToDate" runat="server" PopupButtonID="imgF1TD"
                                                        TargetControlID="txtFortnightly1ToDate" Format="dd"></ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Second Fortnightly: Start Date</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtFortnightly2FromDate" runat="server" MaxLength="50" CssClass="form-control"
                                                            Text='<%# Eval("SM_SECOND_FORTNIGHTLY_DUE_FROM_DATE", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgF2FD" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivF2FromDate" runat="server" TriggerIndex="7" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtFortnightly2FromDate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter second fortnightly: Start Date.">
                                *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revF2FromDate" runat="server" ControlToValidate="txtFortnightly2FromDate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="ceF2FromDate" runat="server" PopupButtonID="imgF2FD"
                                                        TargetControlID="txtFortnightly2FromDate" Format="dd"></ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Second Fortnightly: End Date</label>
                                                    <div class="input-group">
                                                        <F2FControls:F2FTextBox ID="txtFortnightly2ToDate" runat="server" MaxLength="50" CssClass="form-control"
                                                            Text='<%# Eval("SM_SECOND_FORTNIGHTLY_DUE_TO_DATE", "{0:dd-MMM-yyyy}")%>'></F2FControls:F2FTextBox>
                                                        <asp:ImageButton ID="imgF2TD" runat="server" CssClass="custom-calendar-icon" AlternateText="Click to show calendar"
                                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                                    </div>
                                                    <a:RequiredIfValidatorRadioButtonList ID="rivF2ToDate" runat="server" TriggerIndex="7" CssClass="text-danger"
                                                        ControlToCompare="rblFrequency" ControlToValidate="txtFortnightly2ToDate" Display="Dynamic"
                                                        EnableClientScript="true" ValidationGroup="SaveSubmissionDetails" ErrorMessage="Please enter second fortnightly: End Date.">
                *
                                                    </a:RequiredIfValidatorRadioButtonList>
                                                    <asp:RegularExpressionValidator ID="revF2ToDate" runat="server" ControlToValidate="txtFortnightly2ToDate" CssClass="text-danger"
                                                        ErrorMessage="Date Format has to be DD." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))$"
                                                        ValidationGroup="SaveSubmissionDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:CalendarExtender ID="ceF2ToDate" runat="server" PopupButtonID="imgF2TD"
                                                        TargetControlID="txtFortnightly2ToDate" Format="dd"></ajaxToolkit:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="mt-3">
                                        <div class="card mb-1 mt-1 border">
                                            <div class="card-header py-0 custom-ch-bg-color">
                                                <h6 class="font-weight-bold text-white mtb-5">Attachments: </h6>
                                            </div>
                                            <div class="card-body mt-1">
                                                <div class="table-responsive">
                                                    <asp:GridView ID="gvAlreadyUploadedFiles" runat="server" BorderStyle="None" BorderWidth="1px"
                                                        CellPadding="4" GridLines="None" AutoGenerateColumns="False" DataKeyNames="SMF_ID"
                                                        CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                        OnSelectedIndexChanged="gvAlreadyUploadedFiles_SelectedIndexChanged">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="File Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFileType" runat="server" Text='<%# Eval("FileType") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="File Name">
                                                                <ItemTemplate>
                                                                    <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx.aspx?type=SubmissionFiles&downloadFileName=<%# Eval("SMF_SERVER_FILE_NAME")%>','','location=0,status=0,scrollbars=0,width=10,height=10');">
                                                                        <%#Eval("SMF_FILE_NAME")%>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="File Description">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFileDesc" runat="server" Text='<%# Eval("SMF_FILE_DESC").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Uploaded By">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUploaderName" runat="server" Text='<%# Eval("SMF_CREATE_BY") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Uploaded On">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCreateDate" runat="server" Text='<%# Eval("SMF_CREATE_DT", "{0:dd-MMM-yyyy HH:mm:ss}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lbDelete1" runat="server" CssClass="btn btn-sm btn-soft-danger btn-circle"
                                                                        OnClientClick="return onClientDeleteClick();" CommandName="Select">
                                                                        <i class="fa fa-trash"></i>
                                                                    </asp:LinkButton>
                                                                    <asp:HiddenField ID="hfFileId" Value='<%#Eval("SMF_ID") %>' runat="server"></asp:HiddenField>
                                                                    <asp:HiddenField ID="hfServerFileName" Value='<%#Eval("SMF_SERVER_FILE_NAME") %>'
                                                                        runat="server"></asp:HiddenField>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">File Type:</label>
                                            <asp:DropDownList CssClass="form-select" AppendDataBoundItems="true" ID="ddlFileType"
                                                runat="server" DataTextField="RC_NAME" DataValueField="RC_CODE">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvddlFileType" runat="server" ControlToValidate="ddlFileType" CssClass="text-danger"
                                                ValidationGroup="Upload" Display="Dynamic" SetFocusOnError="True" ErrorMessage="Please select File Type.">Please select file type.</asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">File Description:</label>
                                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtFileDesc" runat="server" Rows="3" Columns="50"
                                                TextMode="MultiLine"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtFileDesc" />
                                            <asp:RegularExpressionValidator ID="revtxtFileDescs" ControlToValidate="txtFileDesc"
                                                ErrorMessage="File description cannot exceed 1000 characters." ValidationExpression="^[\s\S]{0,1000}$"
                                                runat="server" ValidationGroup="SaveSubmissionDetails" />
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">
                                                <asp:Label ID="lblAttachments" runat="server" Text="Browse File:"></asp:Label></label>
                                            <div class="input-group">
                                                <asp:FileUpload ID="fuEditFileUpload" CssClass="form-control" runat="server" />
                                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnAddAttachment" runat="server" Text="Attach"
                                                    OnClick="btnAddAttachment_Click" ValidationGroup="Upload">
                                                    <i class="fa fa-upload"></i> Attach
                                                </asp:LinkButton>
                                            </div>
                                            <asp:HiddenField ID="hfFileNameOnServer" runat="server" />
                                            <asp:RegularExpressionValidator ID="revFileUpload1" runat="server" ControlToValidate="fuEditFileUpload"
                                                Display="Dynamic" ErrorMessage="Upload of this file format is not supported."
                                                ValidationExpression="^.+(.msg|.MSG|.eml|.EML|.Eml|.mp4|.MP4|.avi|.AVI|.flv|.FLV|.jpg|.JPG|.bmp|.BMP|
                                                .xls|.XLS|.xlsx|.XLSX|.DOC|.DOCX|.docx|.doc|.pdf|.PDF|.html|.htm|.HTML|.HTM|.xml|.XML|.mht|.MHT|.mhtml|.MHTML|.tif|.TIF|.ZIP|.zip|.txt|.TXT|.ppt|.pptx|.PPT|.PPTX|.pps|.ppsx|.gif|.GIF|.png|.PNG|.mp3|.MP3|.wav|.WAV|.3gp|.3GP|.vob|.VOB|.wmv|.WMV|.m4p|.M4P|.mpeg|.MPEG|.zip|.rar|.csv|.CSV|.7z)$"
                                                ValidationGroup="SaveSubmissionDetails"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="mt-3">
                                        <div class="card mb-1 mt-1 border">
                                            <div class="card-header py-0 custom-ch-bg-color">
                                                <h6 class="font-weight-bold text-white mtb-5">Uploaded Files: </h6>
                                            </div>
                                            <div class="card-body mt-1">
                                                <div class="table-responsive">
                                                    <asp:GridView ID="gvInsertFileUpload" runat="server" AllowPaging="false" AllowSorting="false"
                                                        BorderStyle="None" BorderWidth="1px" DataKeyNames="FileNameOnServer"
                                                        OnSelectedIndexChanged="gvInsertFileUpload_SelectedIndexChanged" CssClass="table table-bordered footable"
                                                        PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="false">
                                                        <Columns>
                                                            <asp:TemplateField ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lbDelete" runat="server" CausesValidation="False" CommandName="Select"
                                                                        OnClientClick="return onClientDeleteClick();" Text="&lt;img alt='Delete' src='../../Content/images/legacy/delete.png' border='0' &gt;">
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="File Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbFileType" runat="server" Text='<%#Eval("FileType")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="File Name">
                                                                <ItemTemplate>
                                                                    <a href="Javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=SubmissionFiles&downloadFileName=<%# Eval("FileNameOnServer")%>','','location=0,status=0,scrollbars=0,width=10,height=10');">
                                                                        <%#Eval("FileName")%>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="File Description">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbFileDesc" runat="server" Text='<%#Eval("File Description").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Uploaded By">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbUploadedBy" runat="server" Text='<%#Eval("Uploaded By")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Uploaded On">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbUploadedDt" runat="server" Text='<%#Eval("Uploaded On")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Previous Reasons:</label><br />
                                            <asp:Label ID="lblPrevReason" runat="server" Text='<%#Eval("SM_REASON_FOR_EDIT").ToString().Replace("\n","<br />") %>'></asp:Label>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Reason for Edit: <span class="text-danger">*</span></label>
                                            <F2FControls:F2FTextBox ID="txtReasonForEdit" CssClass="form-control" runat="server" TextMode="MultiLine"
                                                Rows="3"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtReasonForEdit" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtReasonForEdit" CssClass="text-danger"
                                                ValidationGroup="SaveSubmissionDetails" Display="Dynamic" SetFocusOnError="True"
                                                ErrorMessage="Please enter reason for edit"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="mt-3 text-center">
                                        <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSave" runat="server" Text="  Save  " ValidationGroup="SaveSubmissionDetails"
                                            OnClick="btnSave_Click">
                                            <i class="fa fa-save me-2"></i> Save                    
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnCancel" runat="server" CausesValidation="false"
                                            Text="Cancel" OnClick="btnCancel_Click">
                                            <i class="fa fa-arrow-left me-2"></i> Cancel                   
                                        </asp:LinkButton>
                                    </div>
                                </EditItemTemplate>
                            </asp:FormView>
                        </asp:View>
                    </asp:MultiView>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->
    <script>

        function CompareFilingEffectiveDateSytemDate(src, arg) {
            //if ($('[id*="ddlStatus"]').val() == 'P' && arg.Value != '') {
            if (arg.Value != '') {
                if (Date.parse($("#ctl00_ContentPlaceHolder1_txtCircularDate").val()) > Date.parse($("#ctl00_ContentPlaceHolder1_txtEffectiveDate").val())) {
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
