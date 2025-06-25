<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Submissions.ListOfReports" Title="List of Reports" CodeBehind="ListOfReports.aspx.cs" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="hfUserType" runat="server" />
    <asp:HiddenField ID="hfType" runat="server" />
    <asp:HiddenField ID="hfCircId" runat="server" />
    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">List Of Reports</h4>
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
                    <div class="row">
                        <div class="col-md-4 mb-3">
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
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Reporting to:</label>
                            <asp:DropDownList ID="ddlSegment" runat="server" CssClass="form-select" DataValueField="SSM_ID"
                                DataTextField="SSM_NAME" AppendDataBoundItems="True">
                                <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Type:</label>
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select">
                                <asp:ListItem Value="">All</asp:ListItem>
                                <asp:ListItem Value="F">Fixed Date</asp:ListItem>
                                <asp:ListItem Value="E">Event Based</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Depends On Event:</label>
                            <asp:DropDownList ID="ddlEventForSearch" CssClass="form-select" AutoPostBack="true"
                                AppendDataBoundItems="true" runat="server" OnSelectedIndexChanged="ddlEventForSearch_SelectedIndexChanged"
                                DataValueField="EM_ID" DataTextField="EM_EVENT_NAME">
                                <asp:ListItem Value="">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-8 mb-3">
                            <label class="form-label">Select Agendas:</label>
                            <div class="custom-checkbox-table">
                                <asp:CheckBoxList ID="cblAssociatedWith" RepeatColumns="4" CssClass="form-control" runat="server" DataTextField="EP_NAME"
                                    DataValueField="EP_ID" AppendDataBoundItems="true">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Status:</label>
                            <asp:DropDownList ID="ddlStatus" CssClass="form-select" runat="server">
                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                <asp:ListItem Value="Inactive">Inactive</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Reporting Function:</label>
                            <asp:DropDownList ID="ddlReportDept" CssClass="form-select" AppendDataBoundItems="true"
                                runat="server" DataValueField="SRD_ID" DataTextField="SRD_NAME">
                                <asp:ListItem Value="">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Tracked By:</label>
                            <asp:DropDownList ID="ddlSubType" CssClass="form-select" AppendDataBoundItems="true"
                                runat="server" DataValueField="STM_ID" DataTextField="STM_TYPE">
                                <asp:ListItem Value="">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-8 mb-3">
                            <label class="form-label">Global search:</label>
                            <asp:TextBox CssClass="form-control" ID="txtGlobalSearch" runat="server"></asp:TextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtGlobalSearch" />
                        </div>
                    </div>
                    <div class="text-center mt-3">
                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnSearch" runat="server" Text="Search" ValidationGroup="SEARCH"
                            OnClick="btnSearch_Click">
                            <i class="fa fa-search"></i> Search
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="lnkReset" runat="server" CausesValidation="false"
                            Text="Cancel" OnClick="lnkReset_Click">Reset
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcel" runat="server" Text="Export to Excel"
                            Visible="false" OnClick="btnExportToExcel_Click">
                            <i class="fa fa-download"></i>Export to Excel               
                       
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvSubmissionMaster" runat="server" AutoGenerateColumns="False"
                            OnSorting="gvSubmissionMaster_Sorting" DataKeyNames="SM_ID" AllowSorting="true"
                            AllowPaging="false" GridLines="both" CellPadding="4" CssClass="table table-bordered footable" AlternatingRowStyle-CssClass="alt"
                            OnRowCreated="OnRowCreated">
                            <Columns>
                                <asp:TemplateField HeaderText="​Relevant​ Files">
                                    <ItemTemplate>
                                        <a href="javascript:void(0);" class="btn btn-sm btn-soft-info btn-circle" onclick="javascript:window.open('../Submissions/SubmissionDocuments.aspx?Id=<%# Eval("SM_ID") %>','','location=0,status=0,scrollbars=1,width=450,height=250,resizable=1');">
                                            <i class="fa fa-eye"></i>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Submission Id" SortExpression="SM_ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSmId" Text='<%# Eval("SM_ID") %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fixed Date/Event Based" SortExpression="SM_SUB_TYPE">
                                    <ItemTemplate>
                                        <asp:Label ID="lbType" Text='<%# Eval("SM_SUB_TYPE").ToString()=="E"?"Event":"Fixed" %>' runat="server"> 
                                        </asp:Label>
                                        <asp:HiddenField ID="hfSubType" Value='<%# Eval("SM_SUB_TYPE") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tracking Function" SortExpression="STM_TYPE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstmType" Text='<%#Eval("STM_TYPE")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reporting Function" SortExpression="SRD_NAME">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReportFunc" Text='<%#Eval("SRD_NAME")%>' runat="server"></asp:Label>
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
                                        <asp:Label ID="lblEI_EVENT_DATE" Text='<%#Eval("EI_EVENT_DATE","{0:dd-MMM-yyyy}")%>' runat="server"></asp:Label>
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
                                <asp:TemplateField HeaderText="Particulars" SortExpression="SM_PERTICULARS">
                                    <ItemTemplate>
                                        <asp:Label ID="lblParticulars" Width="300px" runat="server" ToolTip='<%#Eval("SM_PERTICULARS").ToString()%>'
                                            Text='<%#Eval("SM_PERTICULARS").ToString().Length > 200 ? (Eval("SM_PERTICULARS") as string).Substring(0, 200).Replace("\n", "<br />") 
                                                    + " ..." : Eval("SM_PERTICULARS").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                        <asp:Label ID="lblParticulars1" runat="server" Visible="false" Text='<%#Eval("SM_PERTICULARS").ToString()%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" SortExpression="SM_BRIEF_DESCRIPTION">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" Width="300px" runat="server" ToolTip='<%#Eval("SM_BRIEF_DESCRIPTION").ToString()%>'
                                            Text='<%#Eval("SM_BRIEF_DESCRIPTION").ToString().Length > 200 ? (Eval("SM_BRIEF_DESCRIPTION") as string).Substring(0, 200).Replace("\n", "<br />") 
                                                    + " ..." : Eval("SM_BRIEF_DESCRIPTION").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                        <asp:Label ID="lblDescription1" runat="server" Visible="false" Text='<%#Eval("SM_BRIEF_DESCRIPTION").ToString()%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Effective date" SortExpression="SM_EFFECTIVE_DT">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEffectiveDate" Text='<%#Eval("SM_EFFECTIVE_DT","{0:dd-MMM-yyyy}")%>'
                                            runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Circular date" SortExpression="SM_CIRCULAR_DATE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSM_CIRCULAR_DATE" Text='<%#Eval("SM_CIRCULAR_DATE","{0:dd-MMM-yyyy}")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Priority" SortExpression="Priority">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPriority" Text='<%#Eval("Priority")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reporting to">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSegment" Text='<%#LoadSubmissionSegmentName(Eval("SM_ID"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Frequency" SortExpression="SM_FREQUENCY">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFrequency" Text='<%#Eval("SM_FREQUENCY")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To be escalated" SortExpression="ToBeEscalated">
                                    <ItemTemplate>
                                        <asp:Label ID="lblToBeEscalated" Text='<%#Eval("ToBeEscalated")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SM_L0_ESCALATION_DAYS" HeaderText="Reminder days for reporting function"
                                    SortExpression="SM_L0_ESCALATION_DAYS" />
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
            </div>
        </div>
    </div>
    <!-- end row -->


</asp:Content>
