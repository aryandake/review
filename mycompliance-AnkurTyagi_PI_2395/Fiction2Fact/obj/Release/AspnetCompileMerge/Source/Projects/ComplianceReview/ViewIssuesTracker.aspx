<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewIssuesTracker.aspx.cs" Inherits="Fiction2Fact.Projects.Compliance.ViewIssuesTracker" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Issue Tracker</title>
    <asp:PlaceHolder runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />
        <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/bootstrap.bundle.min.js") %>"></script>

        <script type="text/javascript">
            if (window.opener == null || window.opener.location == null) {
                window.location = '<%= Fiction2Fact.Global.site_url() %>' + 'Login.aspx';
            }

            function onCloseClick() {
                window.close();
            }
        </script>

        <script type="text/javascript">

            /* Optional: Temporarily hide the "tabber" class so it does not "flash"
               on the page as plain HTML. After tabber runs, the class is changed
               to "tabberlive" and it will appear. */

            document.write('<style type="text/css">.tabber{display:none;}<\/style>');

            /*==================================================
              Set the tabber options (must do this before including tabber.js)
              ==================================================*/
            var tabberOptions = {
                'onLoad': function (argsObj) {
                    var t = argsObj.tabber;
                    var i;
                    i = document.getElementById('<%=hfTabberId.ClientID%>').value;
                    if (isNaN(i)) { return; }
                    t.tabShow(i);
                },

                'onClick': function (argsObj) {
                    var i = argsObj.index;
                    document.getElementById('<%=hfTabberId.ClientID%>').value = i;
                }
            };
        </script>
    </asp:PlaceHolder>
</head>
<body class="d-block">
    <form id="form1" runat="server">
        <div>
            <asp:HiddenField runat="server" ID="hfTabberId" />
            <asp:HiddenField ID="hfIssueId" runat="server" />
            <asp:HiddenField ID="hfSource" runat="server" />
            <asp:HiddenField ID="hfType" runat="server" />
            <asp:HiddenField ID="hfUser" runat="server" />
            <asp:HiddenField ID="hfUserType" runat="server" />

            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            </asp:ScriptManager>

            <div class="page-content">
                <div class="container-fluid">
                    <!-- Page-Title -->
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="page-title-box">
                                <div class="row">
                                    <div class="col">
                                        <h4 class="page-title">View Issue Tracker</h4>
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
                                    <!-- Nav tabs -->
                                    <ul class="nav nav-tabs" role="tablist">
                                        <li class="nav-item">
                                            <a class="nav-link active" data-bs-toggle="tab" href="#home" role="tab" aria-selected="true">Basic Details</a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link" data-bs-toggle="tab" href="#profile" role="tab" aria-selected="false">Issue Actions</a>
                                        </li>
                                    </ul>

                                    <!-- Tab panes -->
                                    <div class="tab-content">
                                        <div class="tab-pane p-3 active" id="home" role="tabpanel">
                                            <div class="tabular-view">
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Responsible Unit:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblUnit" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Issue Title:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblIssueTitle" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Issue Description:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblIssueDescription" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Issue Type:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblIssueType" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Issue Status:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblIssueStatus" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0 border-bottom">
                                                    <div class="col-md-3">
                                                        <label>SPOC Responsible:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblSPOCResponsible" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="mt-2">
                                                <div class="card mb-1 mt-1 border">
                                                    <div class="card-header py-0 custom-ch-bg-color">
                                                        <h6 class="font-weight-bold text-white mtb-5">Attachment(s): </h6>
                                                    </div>
                                                    <div class="card-body mt-1">
                                                        <div class="table-responsive">
                                                            <asp:GridView ID="gvAttachments" runat="server" AllowPaging="false" ShowFooter="false"
                                                                AllowSorting="false" BorderStyle="None" BorderWidth="1px" AutoGenerateColumns="false"
                                                                CssClass="table table-bordered footable" DataKeyNames="CIF_ID" EmptyDataText="No records found...">
                                                                <Columns>
                                                                    <asp:BoundField DataField="FileType" HeaderText="File Type" SortExpression="FileType" />
                                                                    <asp:TemplateField HeaderText="File Description" ShowHeader="true">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFileDesc" runat="server" Text='<%#Eval("CIF_DESC").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="File Name">
                                                                        <ItemTemplate>
                                                                            <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=CRDRQ&downloadFileName=<%#getFileName(Eval("CIF_SERVER_FILE_NAME"))%>&fileName=<%#getFileName(Eval("CIF_CLIENT_FILE_NAME"))%>','','location=0,status=0,scrollbars=1,width=400,height=200');">
                                                                                <%#Eval("CIF_CLIENT_FILE_NAME")%>
                                                                </a>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tabular-view">
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Management Response:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblManagementRemark" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Remarks:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblRemarks" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Status:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblStatus" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Acceptance Remarks:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblAcceptanceRemark" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Accepted By:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblAcceptedBy" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Accepted On:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblAccepetedOn" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Rejected Remarks:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblRejectedRemarks" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Rejected By:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblRejectedBy" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Rejected On:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblRejectedOn" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Resubmitted On:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblResubmittedOn" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Resubmitted By:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblResubmittedBy" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0 border-bottom">
                                                    <div class="col-md-3">
                                                        <label>Resubmitted Remark:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label CssClass="label2" ID="lblResubmittedRemark" runat="server"></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="mt-2">
                                                <div class="card mb-1 mt-1 border">
                                                    <div class="card-header py-0 custom-ch-bg-color">
                                                        <h6 class="font-weight-bold text-white mtb-5">Issue Tracker Approval - Reviewer </h6>
                                                    </div>
                                                    <div class="card-body mt-1">
                                                        <div class="tabular-view">
                                                            <div class="row g-0">
                                                                <div class="col-md-3">
                                                                    <label>Approved By (Reviewer):</label>
                                                                </div>
                                                                <div class="col-md-9">
                                                                    <label>
                                                                        <asp:Label CssClass="label2" ID="lblApprovedByRM1" runat="server"></asp:Label>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <div class="row g-0">
                                                                <div class="col-md-3">
                                                                    <label>Approved On (Reviewer):</label>
                                                                </div>
                                                                <div class="col-md-9">
                                                                    <label>
                                                                        <asp:Label CssClass="label2" ID="lblApprovedOnRM1" runat="server"></asp:Label>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <div class="row g-0">
                                                                <div class="col-md-3">
                                                                    <label>Approval Remarks (Reviewer):</label>
                                                                </div>
                                                                <div class="col-md-9">
                                                                    <label>
                                                                        <asp:Label CssClass="label2" ID="lblApprovalRemRM1" runat="server"></asp:Label>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <div class="row g-0">
                                                                <div class="col-md-3">
                                                                    <label>Changes Conveyed By (Reviewer):</label>
                                                                </div>
                                                                <div class="col-md-9">
                                                                    <label>
                                                                        <asp:Label CssClass="label2" ID="lblRejectedByRM1" runat="server"></asp:Label>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <div class="row g-0">
                                                                <div class="col-md-3">
                                                                    <label>Changes Conveyed On (Reviewer):</label>
                                                                </div>
                                                                <div class="col-md-9">
                                                                    <label>
                                                                        <asp:Label CssClass="label2" ID="lblRejectedOnRM1" runat="server"></asp:Label>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <div class="row g-0 border-bottom">
                                                                <div class="col-md-3">
                                                                    <label>Changes Conveyed Remarks (Reviewer):</label>
                                                                </div>
                                                                <div class="col-md-9">
                                                                    <label>
                                                                        <asp:Label CssClass="label2" ID="lblRejectionRemRM1" runat="server"></asp:Label>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="mt-2">
                                                <div class="card mb-1 mt-1 border">
                                                    <div class="card-header py-0 custom-ch-bg-color">
                                                        <h6 class="font-weight-bold text-white mtb-5">Issue Tracker Approval - UH </h6>
                                                    </div>
                                                    <div class="card-body mt-1">
                                                        <div class="tabular-view">
                                                            <div class="row g-0">
                                                                <div class="col-md-3">
                                                                    <label>Approved By (Unit Head):</label>
                                                                </div>
                                                                <div class="col-md-9">
                                                                    <label>
                                                                        <asp:Label CssClass="label2" ID="lblApprovedByUH" runat="server"></asp:Label>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <div class="row g-0">
                                                                <div class="col-md-3">
                                                                    <label>Approved On (Unit Head):</label>
                                                                </div>
                                                                <div class="col-md-9">
                                                                    <label>
                                                                        <asp:Label CssClass="label2" ID="lblApprovedOnUH" runat="server"></asp:Label>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <div class="row g-0">
                                                                <div class="col-md-3">
                                                                    <label>Approval Remarks (Unit Head):</label>
                                                                </div>
                                                                <div class="col-md-9">
                                                                    <label>
                                                                        <asp:Label CssClass="label2" ID="lblApprovalRemUH" runat="server"></asp:Label>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <div class="row g-0">
                                                                <div class="col-md-3">
                                                                    <label>Changes Conveyed By (Unit Head):</label>
                                                                </div>
                                                                <div class="col-md-9">
                                                                    <label>
                                                                        <asp:Label CssClass="label2" ID="lblRejectedByUH" runat="server"></asp:Label>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <div class="row g-0">
                                                                <div class="col-md-3">
                                                                    <label>Changes Conveyed On (Unit Head):</label>
                                                                </div>
                                                                <div class="col-md-9">
                                                                    <label>
                                                                        <asp:Label CssClass="label2" ID="lblRejectedOnUH" runat="server"></asp:Label>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <div class="row g-0">
                                                                <div class="col-md-3">
                                                                    <label>Changes Conveyed Remarks (Unit Head):</label>
                                                                </div>
                                                                <div class="col-md-9">
                                                                    <label>
                                                                        <asp:Label CssClass="label2" ID="lblRejectionRemUH" runat="server"></asp:Label>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <div class="row g-0 border-bottom">
                                                                <div class="col-md-3">
                                                                    <label>Audit Trail:</label>
                                                                </div>
                                                                <div class="col-md-9">
                                                                    <label>
                                                                        <asp:Label CssClass="label2" ID="lblAuditTrail" runat="server"></asp:Label>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="tab-pane p-3" id="profile" role="tabpanel">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gvActionables" runat="server" AutoGenerateColumns="False" DataKeyNames="CIA_ID"
                                                    AllowSorting="false" AllowPaging="false" GridLines="Both" CellPadding="4"
                                                    CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                    EmptyDataText="No record found...">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr.No." ControlStyle-Width="20px" HeaderStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Type of Action" DataField="ActionType" />
                                                        <asp:TemplateField HeaderText="Actionables" ShowHeader="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblActionPlan" runat="server" Text='<%#Eval("CIA_ACTIONABLE").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                                                <asp:Label ID="lblRecStatus" runat="server" Visible="false" Text='<%#Eval("CIA_STATUS") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Action Plan Status" DataField="ActionStatus" />
                                                        <asp:BoundField HeaderText="Unit Responsible" DataField="CSFM_NAME" />
                                                        <asp:BoundField DataField="CIA_TARGET_DT" HeaderText="Target Date" ControlStyle-Width="110px" HeaderStyle-Width="110px" DataFormatString="{0:dd-MMM-yyyy}" />
                                                        <asp:BoundField DataField="CIA_CLOSURE_DT" HeaderText="Closure Date" ControlStyle-Width="110px" HeaderStyle-Width="110px" DataFormatString="{0:dd-MMM-yyyy}" />
                                                        <asp:BoundField HeaderText="Person Responsible" DataField="CIA_SPECIFIED_PERSON_NAME" />
                                                        <asp:TemplateField HeaderText="Remarks" ShowHeader="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("CIA_REMARKS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Status" DataField="RecStatus" Visible="false" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                <div class="mt-3">
                                    <asp:LinkButton CssClass="btn btn-outline-danger" runat="server" ID="btnClose"
                                        Text="Close" OnClientClick="return onCloseClick();" >
                                        <i class="fa fa-arrow-left me-2"></i> Close                   
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- end row -->
            </div>
        </div>
    </form>
</body>
</html>
