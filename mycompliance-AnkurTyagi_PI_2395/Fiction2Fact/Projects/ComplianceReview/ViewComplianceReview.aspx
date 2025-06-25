<%@ Page Title="" Language="C#" MasterPageFile="~/Projects/Temp3.master" Async="true" AutoEventWireup="true" CodeBehind="ViewComplianceReview.aspx.cs" Inherits="Fiction2Fact.Projects.Compliance.ViewComplianceReview1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/tabcontrol.js")%>"></script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/commonvalidations.js") %>"></script>

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


        function openAddQueryPopup() {
            var TypeOfView = document.getElementById('ctl00_ContentPlaceHolder1_hfType').value;
            var RRID = document.getElementById('ctl00_ContentPlaceHolder1_hfCCRId').value;


            var popup = window.open('AddDataRequirementQuery.aspx?RefId=' + RRID + '&Source=CR' + '&ViewType=' + TypeOfView, '',
                'fullscreen');
            if (popup.outerWidth < screen.availWidth || popup.outerHeight < screen.availHeight) {
                popup.moveTo(0, 0);
                popup.resizeTo(screen.availWidth, screen.availHeight);
            }


            return false;
        }


        function openAddRiskReviewIssuePopup() {
            var TypeOfView = document.getElementById('ctl00_ContentPlaceHolder1_hfType').value;
            var RRID = document.getElementById('ctl00_ContentPlaceHolder1_hfCCRId').value;
            var RRStatus = document.getElementById('ctl00_ContentPlaceHolder1_hfCCRStatus').value;

            //var Source = document.getElementById('ctl00_ContentPlaceHolder1_hfTypeOfView').value;
            window.open('AddEditIssuesDetails.aspx?RefId=' + RRID + '&Type=Add&Source=' + TypeOfView + '&ViewType=1&RRStatus=' + RRStatus, '', 'location=0,status=0,scrollbars=1,resizable=1,width=900,height=900');
            return false;
        }

        function onViewDRClick(Id) {
            window.open('ViewDataRequirement.aspx?DRId=' + Id + '&Type=ViewSent&Src=RR', '', 'location=0,status=0,scrollbars=1,resizable=1,width=900,height=900');
            return false;
        }

        function ChangeAllCheckBoxStates(checkState) {
            if (CheckBoxIDs != null) {
                for (var i = 0; i < CheckBoxIDs.length; i++)
                    ChangeCheckBoxState(CheckBoxIDs[i], checkState);
            }

            for (var i = 1; i <= CheckBoxIDs.length; i++) {
                //onRowCheckedUnchecked(CheckBoxIDs[i], commentsIDs[i]);
                onRowCheckedUnchecked(CheckBoxIDs[i]);
            }
        }

        function ChangeHeaderAsNeeded() {
            if (CheckBoxIDs != null) {
                for (var i = 1; i < CheckBoxIDs.length; i++) {
                    var cb = document.getElementById(CheckBoxIDs[i]);
                    if (cb != null) {
                        if (!cb.checked) {
                            ChangeCheckBoxState(CheckBoxIDs[0], false);
                            return;
                        }
                    }
                }
                ChangeCheckBoxState(CheckBoxIDs[0], true);
            }
        }

        function ChangeCheckBoxState(id, checkState) {
            var cb = document.getElementById(id);
            if (cb != null)
                cb.checked = checkState;
        }

        function onRowCheckedUnchecked(cbid) {
            var cb = document.getElementById(cbid);

            ChangeHeaderAsNeeded();
            return;
        }

        function onViewIssueTrackerClick(RDIId) {
            window.open('ViewIssuesTracker.aspx?Id=' + RDIId, '', 'location=0,status=0,scrollbars=1,resizable=1,width=900,height=900');
            return false;
        }

        function onStartWorkButtonClick() {
            if (!confirm('Are you sure, you want to initiate compliance review?'))
                return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField runat="server" ID="hfRRImpactedUnitIds" />
    <asp:HiddenField runat="server" ID="hfRRMId" />
    <asp:HiddenField runat="server" ID="hfTabberId" />
    <asp:HiddenField runat="server" ID="hfCCRId" />
    <asp:HiddenField runat="server" ID="hfCCRMId" />
    <asp:HiddenField runat="server" ID="hfCCRStatus" />
    <asp:HiddenField runat="server" ID="hfSource" />
    <asp:HiddenField runat="server" ID="hfType" />
    <asp:HiddenField runat="server" ID="hfRoleType" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Compliance Review</h4>
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
                    <div style="text-align: left;">
                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnEditComplianceReview1" Visible="false" runat="server"
                            Text="Edit Compliance Review" OnClick="btnEditComplianceReview1_Click" >
                            <i class="fa fa-pen"></i> Edit Compliance Review                               
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnWorkStarted1" Visible="false" runat="server"
                            Text="Initiate Compliance Review" OnClientClick="return onStartWorkButtonClick();" OnClick="btnWorkStarted1_Click" >
                            Initiate Compliance Review
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnAddQueryTop" Visible="true" runat="server"
                            Text="Add/Close Queries" OnClientClick="return openAddQueryPopup()" >
                            <i class="fa fa-plus"></i> Add/Close Queries  
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnIssueTrackerTop" runat="server"
                            Text="Add Issue Tracker" OnClientClick="return openAddRiskReviewIssuePopup()" >
                            <i class="fa fa-plus"></i> Add Issue Tracker  
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSubmitForApproval" runat="server"
                            Text="Submit Report for approval" OnClick="btnSubmitForApproval_Click" OnClientClick="return confirm('Are you sure want to submit this compliance review for approval to reviewer?');" Visible="False" >
                            <i class="fa fa-save me-2"></i> Submit Report for approval 
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnCancel1" runat="server" Text="Back" CausesValidation="false" OnClick="btnCancel1_Click" >
                            <i class="fa fa-arrow-left me-2"></i> Back
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcel1" runat="server" Text="Export to Excel" CausesValidation="false" Visible="false" >
                            <i class="fa fa-download"></i> Export to Excel
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnRefresh" runat="server" Style="visibility: hidden; display: none;"
                            Text="Refresh" OnClick="btnRefresh_Click" >
                            <i data-feather="refresh-cw"></i> Refresh
                        </asp:LinkButton>
                    </div>
                    <div class="mb-3 mt-3">
                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link active" data-bs-toggle="tab" href="#home" role="tab" aria-selected="true">Basic Details</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" data-bs-toggle="tab" href="#profile" role="tab" aria-selected="false">Queries</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" data-bs-toggle="tab" href="#settings" role="tab" aria-selected="false">Issue Details</a>
                            </li>
                        </ul>

                        <!-- Tab panes -->
                        <div class="tab-content">
                            <div class="tab-pane p-3 active" id="home" role="tabpanel">
                                <div class="tabular-view">
                                    <div style="visibility: hidden; display: none;">
                                        <div class="col-md-3">
                                            <label>Id:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label" ID="lblCCRId" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Complaince Review No.:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label" ID="lblIdentifier" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Universe to be Reviewed:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblUniversetoReview" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Business Unit(s):</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblBusinessUnits" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Reviewer Name:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblReviewerName" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>
                                                Review Type:
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblReviewType" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Tentative Start Date:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblTentativeStartDT" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>
                                                Tentative End Date:
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblTentativeEndDT" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>
                                                Compliance Review Initiated On:
                                   
                                            </label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblWorkStartedOn" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Submitted By :</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblSubmittedBy" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Submitted On :</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblSubmittedOn" runat="server"></asp:Label>
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
                                            <label>Status Remarks:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblStatusRemarks" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Approved by Unit Head:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblApprovedByUnitHead" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Approved by Reviewer:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblApprovedByL0" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Review Scope:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblReviewScope" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Linkage with earlier circular:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblLinkageWithEarilerCircular" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0" id="trSOC" runat="server" visible="false">
                                        <div class="col-md-3">
                                            <label>Supersedes or Extends/Amends Old Circular(s):</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblSupersedesorExtends" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0 border-bottom" id="trOC" runat="server" visible="false">
                                        <div class="col-md-3">
                                            <label>Old Circular Subject/No.:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblOrderCircularSubjectNo" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="mt-2">
                                    <div class="card mb-1 mt-1 border">
                                        <div class="card-header py-0 custom-ch-bg-color">
                                            <h6 class="font-weight-bold text-white mtb-5">Review Scope Document(s): </h6>
                                        </div>
                                        <div class="card-body mt-1">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gvCRAttachment" runat="server" AllowPaging="false" ShowFooter="false"
                                                    AllowSorting="false" BorderStyle="None" BorderWidth="1px" AutoGenerateColumns="false"
                                                    CssClass="table table-bordered footable" DataKeyNames="CCRF_ID" EmptyDataText="No records found...">
                                                    <Columns>
                                                        <asp:BoundField DataField="RC_NAME" HeaderText="File Type" />
                                                        <asp:TemplateField HeaderText="File Description" ShowHeader="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFileDesc" runat="server" Text='<%#Eval("CCRF_DESC").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="File Name">
                                                            <ItemTemplate>
                                                                <a href="#" onclick="javascript:window.open('../CommonDownload.aspx?type=CRI&downloadFileName=<%#Eval("CCRF_SERVER_FILE_NAME")%>&Filename=<%#Eval("CCRF_CLIENT_FILE_NAME")%>','','location=0,status=0,scrollbars=1,width=10,height=20');return false">
                                                                    <%#Eval("CCRF_CLIENT_FILE_NAME")%> </a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="tabular-view">
                                    <div class="row g-0 border-bottom">
                                        <div class="col-md-3">
                                            <label>Remarks:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label CssClass="label2" ID="lblRemark" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane p-3" id="profile" role="tabpanel">
                                <div runat="server" id="tbQueries">
                                    <div class="mt-2">
                                        <%#Eval("CCRF_CLIENT_FILE_NAME")%>
                                    </div>
                                    <div class="table-responsive mt-3 mb-3">
                                        <asp:GridView ID="gvQueries" runat="server" AllowPaging="false" ShowFooter="false"
                                            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" AllowSorting="false" BorderStyle="None"
                                            BorderWidth="1px" AutoGenerateColumns="false" DataKeyNames="CDQ_ID" CssClass="table table-bordered footable"
                                            EmptyDataText="No record found...">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Sr.No.</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <center>
                                                            <%# Container.DataItemIndex + 1%>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="View" >
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Select" 
                                                                CssClass="btn btn-sm btn-soft-info btn-circle" OnClientClick='<%# string.Format("return onViewDRClick(\"{0}\");", Eval("CDQ_ID")) %>'>
                                                                <i class="fa fa-eye"></i>	                            
                                                            </asp:LinkButton>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="View Response" Visible="false" >
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:LinkButton ID="lnkViewRes" runat="server" CommandName="Select" 
                                                                CssClass="btn btn-sm btn-soft-info btn-circle" OnClientClick='<%# string.Format("return onViewRRClick(\"{0}\");", Eval("CDQ_ID")) %>'>
                                                                <i class="fa fa-eye"></i>
                                                            </asp:LinkButton>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Type" DataField="CDQ_Type" />
                                                <asp:BoundField HeaderText="Responsible Unit" DataField="CSFM_NAME" />
                                                <asp:TemplateField HeaderText="Data Requirement / Query" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDRQM" Width="200px" runat="server" ToolTip='<%# Eval("CDQ_QUERY_DATA_REQUIREMENT").ToString() %>'
                                                            Text='<%#Eval("CDQ_QUERY_DATA_REQUIREMENT").ToString().Length>200?(Eval("CDQ_QUERY_DATA_REQUIREMENT") as string).Substring(0,200).Replace(Environment.NewLine, "<br />") + " ...":Eval("CDQ_QUERY_DATA_REQUIREMENT").ToString().Replace(Environment.NewLine, "<br />")  %>'></asp:Label>
                                                        <asp:Label ID="lblDRQM1" Visible="false" runat="server" Text='<%#Eval("CDQ_QUERY_DATA_REQUIREMENT").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Person Responsible" DataField="CDQ_PERSON_RESPONSIBLE" ControlStyle-Width="100px" HeaderStyle-Width="100px" />

                                                <asp:BoundField HeaderText="Raised Date" DataField="CDQ_RAISED_DT" DataFormatString="{0: dd-MMM-yyyy HH:mm:ss}" />
                                                <asp:BoundField HeaderText="Due Date" DataField="CDQ_EXPIRY_DT" DataFormatString="{0: dd-MMM-yyyy}" />
                                                <asp:BoundField HeaderText="Status" DataField="Status" ControlStyle-Width="40px" HeaderStyle-Width="40px" />
                                                <asp:BoundField HeaderText="Query pending with" DataField="Query pending with" />
                                                <asp:TemplateField HeaderText="Ageing" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lblAgeing" runat="server" Text='<%#Eval("Ageing") %>'></asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Attachment(s)" ControlStyle-Width="100px" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:DataList ID="dlEvidenceFiles" runat="server" RepeatColumns="1" RepeatDirection="vertical">
                                                            <ItemTemplate>
                                                                <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=CRDRQ&downloadFileName=<%#Eval("CRDF_SERVER_FILE_NAME")%>&fileName=<%#Eval("CRDF_CLIENT_FILE_NAME")%>','','location=0,status=0,scrollbars=1,width=400,height=200');">
                                                                    <%#Eval("CRDF_CLIENT_FILE_NAME")%>
                                                    </a>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Closed by" DataField="CDQ_CLOSED_BY" ControlStyle-Width="40px" HeaderStyle-Width="40px" />
                                                <asp:BoundField HeaderText="Closed on" DataField="CDQ_CLOSED_ON" DataFormatString="{0: dd-MMM-yyyy HH:mm:ss}" ControlStyle-Width="80px" HeaderStyle-Width="80px" />
                                                <asp:TemplateField HeaderText="Closure Remarks" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClosureRem" Width="200px" runat="server" ToolTip='<%# Eval("CDQ_CLOSURE_REMARKS").ToString() %>'
                                                            Text='<%#Eval("CDQ_CLOSURE_REMARKS").ToString().Length>200?(Eval("CDQ_CLOSURE_REMARKS") as string).Substring(0,200).Replace(Environment.NewLine, "<br />") + " ...":Eval("CDQ_CLOSURE_REMARKS").ToString().Replace(Environment.NewLine, "<br />")  %>'></asp:Label>
                                                        <asp:Label ID="lblClosureRem1" Visible="false" runat="server" Text='<%#Eval("CDQ_CLOSURE_REMARKS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div>
                                        <%#Eval("CCRF_CLIENT_FILE_NAME")%>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane p-3" id="settings" role="tabpanel">
                                <div runat="server" id="tbIssueDetails">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvIssues" AllowPaging="False" AllowSorting="true" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found." EmptyDataRowStyle-BorderColor="transparent"
                                            CssClass="table table-bordered footable" DataKeyNames="CI_ID">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Sr.No.
               
                       
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex+1  %>' runat="server"></asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="View"  Visible="true">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Select" CommandArgument='<%# Eval("CI_ID") %>'
                                                                CssClass="btn btn-sm btn-soft-info btn-circle" OnClientClick='<%# string.Format("return onViewIssueTrackerClick(\"{0}\");", Eval("CI_ID")) %>'>
                                                                <i class="fa fa-eye"></i>
                                                            </asp:LinkButton>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField HeaderText="Responsible Unit" DataField="CSFM_NAME" />
                                                <asp:BoundField HeaderText="Issue Title" DataField="CI_ISSUE_TITLE" />
                                                <asp:TemplateField HeaderText="Issue Description" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIssueDesc" Width="200px" runat="server" ToolTip='<%# Eval("CI_ISSUE_DESC").ToString() %>'
                                                            Text='<%#Eval("CI_ISSUE_DESC").ToString().Length>200?(Eval("CI_ISSUE_DESC") as string).Substring(0,200).Replace(Environment.NewLine, "<br />") + " ...":Eval("CI_ISSUE_DESC").ToString().Replace(Environment.NewLine, "<br />")  %>'></asp:Label>
                                                        <asp:Label ID="lblIssueDesc1" Visible="false" runat="server" Text='<%#Eval("CI_ISSUE_DESC").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                                        <asp:HiddenField ID="hfStatus" runat="server" Value='<%#Eval("CI_STATUS")%>' />
                                                        <asp:HiddenField ID="hfCI_ID" runat="server" Value='<%#Eval("CI_ID")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Issue Type" DataField="IssueType" />
                                                <asp:BoundField HeaderText="Issue Status" DataField="IssueStatus" />

                                                <asp:BoundField HeaderText="Status" DataField="DraftIssuesStatus" />

                                                <asp:TemplateField HeaderText="Annexures(s)">
                                                    <ItemTemplate>
                                                        <asp:DataList ID="dlFiles" runat="server" RepeatColumns="1" CssClass="custom-datalist-border" RepeatDirection="vertical"
                                                            DataSource='<%# LoadDraftedFileList(Eval("CI_ID")) %>'>
                                                            <ItemTemplate>
                                                                <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=ComplianceIssue&downloadFileName=<%#getFileName(Eval("CIF_SERVER_FILE_NAME"))%>&Filename=<%#getFileName(Eval("CIF_CLIENT_FILE_NAME"))%>','','location=0,status=0,scrollbars=1,width=400,height=200');">
                                                                    <%#Eval("CIF_CLIENT_FILE_NAME")%>
                                                                        </a>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="SPOC Responsible" DataField="CI_SPOC_RESPONSIBLE" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="text-align: left;">
                        <asp:LinkButton CssClass="html_button" ID="btnEditComplianceReview1_Bottom" Visible="true" runat="server"
                            Text="Edit Compliance Review" OnClick="btnEditComplianceReview1_Click" >
                            <i class="fa fa-pen"></i> Edit Compliance Review  
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnWorkStarted1_Bottom" Visible="false" runat="server"
                            Text="Initiate Compliance Review" OnClientClick="return onStartWorkButtonClick();" OnClick="btnWorkStarted1_Click" >
                            Initiate Compliance Review
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnAddQuery_Bottom" Visible="true" runat="server"
                            Text="Add/Close Queries" OnClientClick="return openAddQueryPopup()" >
                            <i class="fa fa-plus"></i> Add/Close Queries
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnIssueTracker_Bottom" Visible="true" runat="server"
                            Text="Add Issue Tracker" OnClientClick="return openAddRiskReviewIssuePopup()" >
                            <i class="fa fa-plus"></i> Add Issue Tracker  
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnCancel1_Bottom" runat="server" Text="Back" CausesValidation="false" OnClick="btnCancel1_Click" >
                            <i class="fa fa-arrow-left me-2"></i> Back
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcel1_Bottom" runat="server" Text="Export to Excel" Visible="false" CausesValidation="false" >
                             <i class="fa fa-download"></i> Export to Excel
                        </asp:LinkButton>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->

</asp:Content>
