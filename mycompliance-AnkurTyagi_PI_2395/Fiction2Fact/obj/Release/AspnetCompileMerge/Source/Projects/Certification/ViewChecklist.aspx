<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false"
    Inherits="Fiction2Fact.Projects.Certification.Certification_ViewChecklist" EnableEventValidation="false" CodeBehind="ViewChecklist.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ViewChecklist</title>
    <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/assets/js/bootstrap.bundle.min.js") %>"></script>
</head>

<script type="text/javascript">

    function closeFileWindow() {
        window.close();
    }

    document.write('<style type="text/css">.tabber{display:none;}<\/style>');

    /* Set the tabber options (must do this before including tabber.js)*/
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

<script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/tabcontrol.js")%>'>
</script>

<script type="text/javascript">
    //start - added by Hari on 3 oct 2016
    function openViewChecklistPopup(requestId) {

        //alert(requestId);
        window.open('../Certification/ViewChecklistData.aspx?ChecklistId=' + requestId,
            '', 'location=0,status=0,scrollbars=1,resizable=1,width=700,height=500');
        return false;
    }
    //End - added by Hari on 3 oct 2016

    function onClientViewCircClick(CMId) {
        window.open('../Circulars/ViewCircularDetails.aspx?CircularId=' + CMId, '', 'location=0,status=0,resizable=1,scrollbars=1,width=1000,height=800');
        return false;
    }
</script>

<body class="d-block">
    <form id="form1" runat="server">
        <asp:HiddenField ID="hfcertId" runat="server" />
        <asp:HiddenField ID="hfContent" runat="server" />
        <asp:HiddenField ID="hfTabberId" runat="server" />
        <asp:HiddenField ID="hfSource" runat="server" />
        <asp:HiddenField ID="hfLevel" runat="server" />

        <div class="page-content">
            <div class="container-fluid">
                <!-- Page-Title -->
                <div class="row">
                    <div class="col-sm-12">
                        <div class="page-title-box">
                            <div class="row">
                                <div class="col">
                                    <h4 class="page-title">View Checklist</h4>
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

                                <div class="mb-3">
                                    <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnClose1" runat="server" Text="Close" OnClick="BtnClose_click">
        <i class="fa fa-arrow-left me-2"></i> Close                   
                                    </asp:LinkButton>
                                </div>

                                <!-- Nav tabs -->
                                <ul class="nav nav-tabs" role="tablist">
                                    <li class="nav-item">
                                        <a class="nav-link active" data-bs-toggle="tab" href="#home" role="tab" aria-selected="true">Certification Details</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" data-bs-toggle="tab" href="#profile" role="tab" aria-selected="false">Compliance Checklist</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" data-bs-toggle="tab" href="#settings" role="tab" aria-selected="false">Compliance Deviations</a>
                                    </li>
                                </ul>

                                <!-- Tab panes -->
                                <div class="tab-content">
                                    <div class="tab-pane p-3 active" id="home" role="tabpanel">
                                        <div class="tabular-view">
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Department:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblDeptName"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Quarter From Date:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblFromDt"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Quarter To Date:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblToDt"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Status:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblStatus"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Submitted By:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblSubmittedBy"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Submitted On:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblSubmittedOn"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Submission/Resubmission Remarks:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblSubmittedRemarks"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <%--//UH--%>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Approved By (Unit Head):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblHODApprovedBy"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Approved By Unit Head On:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblHODApprovedOn"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Unit Head Approval Remarks:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblHODApprovedRemarks"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0" style="display:none;visibility:hidden">
                                                <div class="col-md-3">
                                                    <label>Revision Suggested By (Unit Head):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblHODRejectedBy"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0" style="display:none;visibility:hidden">
                                                <div class="col-md-3">
                                                    <label>Revision Suggested on(Unit Head ):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblHODRejectedOn"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0" style="display:none;visibility:hidden">
                                                <div class="col-md-3">
                                                    <label>Suggested Revision( Unit Head):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblHODRejectedRemarks"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <%--//FH--%>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Approved By (Function Head):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblCXOApprovedBy"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Approved By Function Head On:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblCXOApprovedOn"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Function Head Approval Remarks:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblCXOApprovedRemarks"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Revision Suggested By (Function Head):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblCXORejectedBy"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Revision Suggested on(Function Head):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblCXORejectedOn"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0 border-bottom">
                                                <div class="col-md-3">
                                                    <label>Suggested Revision(Function Head):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblCXORejectedRemarks"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <%--//CU--%>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Reviewed by (Compliance User):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblCUby"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Reviewed On (Compliance User):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblCUon"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Revision Suggested By (Compliance User):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblCURSby"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Revision Suggested on (Compliance User):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblCURSon"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Revision Suggested (Compliance User):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblCURS"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <%--//ECO--%>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Approved By (Executive Committee):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblECOApprovedBy"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Approved By Executive Committee On:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblECOApprovedOn"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Executive Committee Approval Remarks:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblECOApprovedRemarks"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Revision Suggested By (Executive Committee):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblECORejectedBy"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Revision Suggested on(Executive Committee):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblECORejectedOn"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0 border-bottom">
                                                <div class="col-md-3">
                                                    <label>Suggested Revision(Executive Committee):</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblECORejectedRemarks"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0 border-bottom">
                                                <div class="col-md-3">
                                                    <label>Audit Trail</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label runat="server" ID="lblAuditTrail"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="mt-3">
                                            <div class="card mb-1 mt-1 border">
                                                <div class="card-header py-0 custom-ch-bg-color">
                                                    <h6 class="font-weight-bold text-white mtb-5">Certificate: </h6>
                                                </div>
                                                <div class="card-body mt-1">
                                                    <div>
                                                        <asp:Label runat="server" ID="lblCertificate"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="mt-3" style="display: none; visibility: hidden">
                                            <div class="card mb-1 mt-1 border">
                                                <div class="card-header py-0 custom-ch-bg-color">
                                                    <h6 class="font-weight-bold text-white mtb-5">Remarks </h6>
                                                </div>
                                                <div class="card-body mt-1">
                                                    <div>
                                                        <asp:Label runat="server" ID="lblRemarks"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane p-3" id="profile" role="tabpanel">
                                        <asp:Panel ID="pnlChecklistShow" runat="server">
                                            <div class="mb-3">
                                                <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcel" runat="server" Text="Export To Excel"
                                                    OnClick="btnExportToExcel_Click">
                                                    <i class="fa fa-download"></i> Export to Excel               
                                                </asp:LinkButton>
                                            </div>
                                            <div class="table-responsive">
                                                <asp:GridView ID="gvChecklist" runat="server" BorderStyle="None" BorderWidth="1px"
                                                    CellPadding="4" GridLines="None" AutoGenerateColumns="false" CssClass="table table-bordered footable"
                                                    DataKeyNames="ID" OnRowDataBound="gvChecklist_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Serial Number
                                               
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="View Circulars">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:LinkButton ID="lnkViewCirc" runat="server" CssClass="btn btn-sm btn-soft-info btn-circle"
                                                                        CommandName="Select">
                                                                        <%--OnClientClick='<%# string.Format("onClientViewCircClick(\"{0}\");", Eval("CCM_CM_ID")) %>'--%>
                                                                        <i class="fa fa-eye"></i> 
                                                                    </asp:LinkButton>
                                                                    <asp:HiddenField ID="hfCircularId" runat="server" Value='<%#Eval("CCM_CM_ID") %>' />
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Act/Regulation/Circular">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblActRegCirc" Text='<%#Eval("CDTM_TYPE_OF_DOC") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Reference Circular/Notification/Act" SortExpression="CCM_REFERENCE">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtRegulations" Text='<%#Eval("CCM_REFERENCE").ToString().Length>100?(Eval("CCM_REFERENCE") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_REFERENCE").ToString().Replace("\n", "<br />")  %>'
                                                                    ToolTip='<%#Eval("CCM_REFERENCE")%>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Section/Clause" SortExpression="CCM_CLAUSE">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkbtntxtSections" runat="server" Text='<%#Eval("CCM_CLAUSE").ToString().Length>100?(Eval("CCM_CLAUSE") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_CLAUSE").ToString().Replace("\n", "<br />")  %>'
                                                                    ToolTip='<%#Eval("CCM_CLAUSE")%>' Font-Underline="false" CssClass="badge rounded-pill badge-soft-pink" OnClientClick='<%# string.Format("return openViewChecklistPopup(\"{0}\");", Eval("CCD_ID")) %>'> </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Compliance of/Heading of Compliance checklist" SortExpression="CCM_CHECK_POINTS">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtCheckPoint" Text='<%#Eval("CCM_CHECK_POINTS").ToString().Replace("\n", "<br />") %>'
                                                                    runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description" SortExpression="CCM_PARTICULARS">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtSelfAssessmentStatus" Text='<%#Eval("CCM_PARTICULARS").ToString().Replace("\n", "<br />") %>'
                                                                    runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Consequences of non Compliance" SortExpression="CCM_PENALTY">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtPenalty" Text='<%#Eval("CCM_PENALTY").ToString().Length>100?(Eval("CCM_PENALTY") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_PENALTY").ToString().Replace("\n", "<br />")  %>'
                                                                    ToolTip='<%#Eval("CCM_PENALTY")%>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Frequency" SortExpression="CCM_FREQUENCY">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtTimeLimit" Text='<%#Eval("CCM_FREQUENCY").ToString().Length>100?(Eval("CCM_FREQUENCY") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_FREQUENCY").ToString().Replace("\n", "<br />")  %>'
                                                                    ToolTip='<%#Eval("CCM_FREQUENCY")%>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Forms" SortExpression="CCM_FORMS">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtForms" Text='<%#Eval("CCM_FORMS").ToString().Length>100?(Eval("CCM_FORMS") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_FORMS").ToString().Replace("\n", "<br />")  %>'
                                                                    ToolTip='<%#Eval("CCM_FORMS")%>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Compliance Status" DataField="Compliance_Status" />
                                                        <%--<asp:TemplateField HeaderText="Remarks" SortExpression="CCD_REMARKS">--%>
                                                        <asp:TemplateField HeaderText="Reason of non compliance" SortExpression="CCD_REMARKS">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtRemarks" Text='<%#Eval("CCD_REMARKS").ToString().Length>100?(Eval("CCD_REMARKS") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCD_REMARKS").ToString().Replace("\n", "<br />")  %>'
                                                                    ToolTip='<%#Eval("CCD_REMARKS")%>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Non Compliance since" DataField="CCD_NC_SINCE_DT" DataFormatString="{0: dd-MMM-yyyy}" />
                                                        <asp:TemplateField HeaderText="Action Plan" SortExpression="CCD_ACTION_PLAN">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtActionPlan" Text='<%#Eval("CCD_ACTION_PLAN").ToString().Length>100?(Eval("CCD_ACTION_PLAN") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCD_ACTION_PLAN").ToString().Replace("\n", "<br />")  %>'
                                                                    ToolTip='<%#Eval("CCD_ACTION_PLAN")%>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Target Date" DataField="CCD_TARGET_DATE" DataFormatString="{0: dd-MMM-yyyy}" />
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
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <div class="tab-pane p-3" id="settings" role="tabpanel">
                                        <div class="mb-3">
                                            <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcelExceptions" runat="server"
                                                Text="Export To Excel" OnClick="btnExportToExcelExceptions_Click">
                                                <i class="fa fa-download"></i> Export to Excel               
                                            </asp:LinkButton>
                                        </div>
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvException" runat="server" AutoGenerateColumns="False" DataKeyNames="CE_ID"
                                                AllowSorting="true" AllowPaging="true" GridLines="Both" CellPadding="4" CssClass="table table-bordered footable"
                                                PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Serial Number
                                               
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Attached File">
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0)" onclick="javascript:window.open('../DownloadFileCertification.aspx?FileInformation=<%#(Eval("CE_SERVER_FILE_NAME"))%>','','location=0,status=0,scrollbars=0,width=10,height=10');">
                                                                <%#Eval("CE_CLIENT_FILE_NAME")%>
                                                            </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FHDept" HeaderText="Department" />
                                                    <asp:TemplateField HeaderText="Deviation (Detailed)" SortExpression="CE_EXCEPTION_TYPE">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtApplicableLaw" Text='<%#Eval("CE_EXCEPTION_TYPE").ToString().Replace("\n", "<br />") %>'
                                                                runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Regulatory Reference (Detailed)" SortExpression="CE_DETAILS">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtObservations" Text='<%#Eval("CE_DETAILS").ToString().Replace("\n", "<br />") %>'
                                                                runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Root Cause for the Deviation" SortExpression="CE_ROOT_CAUSE_OF_DEVIATION">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtRootCause" Text='<%#Eval("CE_ROOT_CAUSE_OF_DEVIATION").ToString().Replace("\n", "<br />") %>'
                                                                runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Action taken" SortExpression="CE_ACTION_TAKEN">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtActionTaken" Text='<%#Eval("CE_ACTION_TAKEN").ToString().Replace("\n", "<br />") %>'
                                                                runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Current Status" SortExpression="CE_CLOSURE_STATUS">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtCE_CLOSURE_STATUS" Text='<%#Eval("CE_CLOSURE_STATUS").ToString().Replace("\n", "<br />") %>'
                                                                runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Target/Closure Date" SortExpression="CE_TARGET_DATE">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtTargetDate"
                                                                runat="server"></asp:Label>

                                                            <asp:HiddenField ID="hfTargetDate" runat="server" Value='<%#Bind("CE_TARGET_DATE","{0:dd-MMM-yyyy}") %>' />
                                                            <asp:HiddenField ID="hfClosureDate" runat="server" Value='<%#Bind("CE_CLOSURE_DATE","{0:dd-MMM-yyyy}") %>' />

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
                </div>
                <!-- end row -->
            </div>
        </div>
    </form>
</body>
</html>
