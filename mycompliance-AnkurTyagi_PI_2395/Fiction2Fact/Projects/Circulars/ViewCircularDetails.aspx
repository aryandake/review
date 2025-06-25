<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Circulars.Circulars_ViewCircularDetails" CodeBehind="ViewCircularDetails.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>View Circular Details</title>
    <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />
    <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/bootstrap.bundle.min.js") %>"></script>


    <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/jquery.min.js") %>"></script>


    <%--Added By Ajay Matekar (13/06/2023) for View modal --%>
    <script type="text/javascript">
        const onViewChklistAuditTrailClick = (hfAuditTrailId) => {
            var hfAuditTrailValue = $("#" + hfAuditTrailId).val();
            var hfAuditTrailValueDecoded = "";
            try {
                hfAuditTrailValueDecoded = atob(hfAuditTrailValue);
            } catch (e) {
                hfAuditTrailValueDecoded = hfAuditTrailValue;
            }
            $("#<%= lblChklistAuditTrail.ClientID %>").html(hfAuditTrailValueDecoded);
            $("#divModal").modal('show');
            return false;
        };
    </script>
</head>
<body class="d-block">
    <form id="form1" runat="server">

        <asp:HiddenField ID="hfCircularId" runat="server" />
        <asp:HiddenField ID="hfCircCertChecklistId" runat="server" />
        <asp:HiddenField ID="hfTabberId" runat="server" />

        <div class="page-content">
            <div class="container-fluid">
                <!-- Page-Title -->
                <div class="row">
                    <div class="col-sm-12">
                        <div class="page-title-box">
                            <div class="row">
                                <div class="col">
                                    <h4 class="page-title">View Circulars Details</h4>
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
                                        <a class="nav-link active" data-bs-toggle="tab" href="#home" role="tab" aria-selected="true">Circulars</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" data-bs-toggle="tab" href="#profile" role="tab" aria-selected="false">Actionables</a>
                                    </li>
                                    <li class="nav-item" style="display: none">
                                        <a class="nav-link" data-bs-toggle="tab" href="#settings" role="tab" aria-selected="false">Certification Checklist</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" data-bs-toggle="tab" href="#settings1" role="tab" aria-selected="false">Regulatory Reporting</a>
                                    </li>
                                </ul>

                                <!-- Tab panes -->
                                <div class="tab-content">
                                    <div class="tab-pane p-3 active" id="home" role="tabpanel">
                                        <div class="tabular-view">
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Created by Department: </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblCreator" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0" style="display: none">
                                                <div class="col-md-3">
                                                    <label>Line of Business: </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblLOB" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <%--<< Added by Amarjeet on 04-Aug-2021--%>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Linkage With Earlier Circular:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblLinkageWithEarlierCircular" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Supersedes or Extends/Amends Old Circular(s):
                                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblSOCEOC" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Old Circular Subject/No.:
                                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblOldCircSubNo" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <%-->>--%>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Issuing Authority:
                       
                                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblAuthority" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Topic:
                       
                                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblTopic" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Type of Document:
                       
                                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblTypeofDocument" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Public/Private:
                       
                                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblSubTypeofDocument" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        SPOC From Compliance Department:
                       
                                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblSPOCFromComplianceFunction" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Circular No.:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblCircularNo" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Circular Date:
                       
                                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblCircularDate" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Circular Effective Date:
                       
                                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblCircEffDate" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Subject of the Circular:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblSubject" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Gist of the Circular:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblGist" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Implications:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblImplications" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Issuer Link:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblLink" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Requirement for the Board/Audit committee to approve:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblRequirementForTheBoard" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Details:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblDetails" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Name of the Policy/Guidelines:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblNameOfThePolicy" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Frequency:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblFrequency" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Status:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Deactivated by:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblDeactivatedBy" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Deactivated on:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblDeactivatedOn" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0 border-bottom">
                                                <div class="col-md-3">
                                                    <label>Deactivation Remarks:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblDeactivationRemarks" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="mt-1">
                                            <div class="card mb-1 mt-1 border">
                                                <div class="card-header py-0 custom-ch-bg-color">
                                                    <h6 class="font-weight-bold text-white mtb-5">Associated Keywords: </h6>
                                                </div>
                                                <div class="card-body mt-1">
                                                    <div class="custom-checkbox-table">
                                                        <asp:Panel ID="pnlAssociatedKeywords" runat="server" ScrollBars="Auto" CssClass="form-control">
                                                            <asp:CheckBoxList ID="cbAssociatedKeywords" RepeatColumns="4" runat="server" DataTextField="CKM_NAME"
                                                                DataValueField="CKM_ID" AppendDataBoundItems="True" Enabled="false" RepeatDirection="Horizontal">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="mt-1">
                                            <div class="card mb-1 mt-1 border">
                                                <div class="card-header py-0 custom-ch-bg-color">
                                                    <h6 class="font-weight-bold text-white mtb-5">To be placed before: </h6>
                                                </div>
                                                <div class="card-body mt-1">
                                                    <asp:Panel ID="pnlToBePlacedBefore" runat="server" CssClass="form-control">
                                                        <div class="custom-checkbox-table">
                                                            <asp:CheckBoxList ID="cbToBePlacedBefore" RepeatColumns="4" runat="server" DataTextField="RC_NAME"
                                                                DataValueField="RC_CODE" AppendDataBoundItems="True" Enabled="false" RepeatDirection="Horizontal">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="mt-3">
                                            <div class="card mb-1 mt-1 border">
                                                <div class="card-header py-0 custom-ch-bg-color">
                                                    <h6 class="font-weight-bold text-white mtb-5">Attachment : </h6>
                                                </div>
                                                <div class="card-body mt-1">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gvViewFileUpload" runat="server" CssClass="table table-bordered footable" PagerStyle-CssClass="pgr"
                                                            AlternatingRowStyle-CssClass="alt" CellPadding="4" GridLines="None" AutoGenerateColumns="False"
                                                            DataKeyNames="CF_ID">
                                                            <Columns>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblServerFileName" Text='<%#Eval("CF_FILENAME") %>' runat="server"> </asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="File Name">
                                                                    <ItemTemplate>
                                                                        <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=Circular&downloadFileName=<%#getFileName(Eval("CF_SERVERFILENAME"))%>&fileName=<%#getFileName(Eval("CF_FILENAME"))%>','','location=0,status=0,scrollbars=1,width=10,height=20');return false">
                                                                            <%#Eval("CF_FILENAME")%>
                                                                        </a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Uploaded By">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUploaderName" runat="server" Text='<%# Eval("CF_CREAT_BY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Uploaded On">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCreateDate" runat="server" Text='<%# Eval("CF_CREAT_DT", "{0:dd-MMM-yyyy HH:mm:ss}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane p-3" id="profile" role="tabpanel">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvActionables" runat="server" CssClass="table table-bordered footable" GridLines="Both" AllowPaging="false" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr.No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Actionable" ShowHeader="true">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#Eval("CA_ACTIONABLE").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CFM_NAME" HeaderText="Responsible Function" />
                                                    <asp:BoundField DataField="CA_PERSON_RESPONSIBLE_NAME" HeaderText="Person Responsible Name" />
                                                    <asp:BoundField DataField="CA_PERSON_RESPONSIBLE_EMAIL_ID" HeaderText="Person Responsible Email" />
                                                    <asp:BoundField DataField="CA_Reporting_Mgr_Name" HeaderText="Reporting Manager Name" />
                                                    <asp:BoundField DataField="CA_Reporting_Mgr_EMAIL_ID" HeaderText="Reporting Manager Email" />
                                                    <asp:BoundField DataField="RC_NAME" HeaderText="Status" />
                                                    <asp:BoundField DataField="CA_TARGET_DATE" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Target Date" />
                                                    <asp:BoundField DataField="CA_REGULATORY_DUE_DATE" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Regulatory Due Date" />
                                                    <asp:BoundField DataField="CA_COMPLETION_DATE" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Completion Date" />
                                                    <asp:TemplateField HeaderText="Remarks" ShowHeader="true">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#Eval("CA_REMARKS").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>

                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="tab-pane p-3" id="settings" role="tabpanel">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvCertChecklists" runat="server" CssClass="table table-bordered footable" GridLines="Both"
                                                AllowPaging="false" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                OnRowDataBound="gvCertChecklists_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr.No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DeptName" HeaderText="Department Name" />
                                                    <asp:TemplateField HeaderText="Act/Regulation/Circular" ShowHeader="true">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#Eval("CDTM_TYPE_OF_DOC").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reference Circular / Notification / Act" ShowHeader="true">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#Eval("CCC_REFERENCE").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Section/Clause" ShowHeader="true">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#Eval("CCC_CLAUSE").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="Compliance of/Heading of Compliance checklist" ShowHeader="true">--%>
                                                    <asp:TemplateField HeaderText="Particulars" ShowHeader="true">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#Eval("CCC_CHECK_POINTS").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Description" ShowHeader="true">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#Eval("CCC_PARTICULARS").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Consequences of non Compliance" ShowHeader="true">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#Eval("CCC_PENALTY").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CCC_FREQUENCY" HeaderText="Frequency" />
                                                    <asp:BoundField DataField="CCC_FORMS" HeaderText="Forms" />
                                                    <asp:BoundField DataField="CCC_EFFECTIVE_FROM" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Effective From" />
                                                    <asp:BoundField DataField="MovedToCertification" HeaderText="Moved to Certification Checklist" />
                                                    <asp:TemplateField HeaderText="Audit Trail" ShowHeader="true">
                                                        <ItemTemplate>
                                                            <%--<asp:Label runat="server" Text='<%#Eval("CCC_AUDIT_TRAIL").ToString().Replace("\n","<br />") %>'></asp:Label>--%>
                                                            <asp:HiddenField ID="hfAuditTrail" runat="server" Value='<%# Convert.ToBase64String(Encoding.UTF8.GetBytes(Eval("CCC_AUDIT_TRAIL").ToString().Replace("\r\n", "<br>"))) %>'></asp:HiddenField>
                                                            <asp:LinkButton ID="lbAuditTrail" CssClass="badge rounded-pill badge-soft-pink" runat="server" Text="View Audit Trail" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>

                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="tab-pane p-3" id="settings1" role="tabpanel">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvSubmissionMaster" runat="server" CssClass="table table-bordered footable" GridLines="Both" AllowPaging="false" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="​Relevant​ Files">
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="javascript:window.open('../Submissions/SubmissionDocuments.aspx?Id=<%# Eval("SM_ID") %>','','location=0,status=0,scrollbars=1,width=450,height=250,resizable=1');">
                                                                <asp:Image ID="imgView" runat="server" ImageUrl="../../Content/images/legacy/viewfulldetails.png" ToolTip="View ​Relevant​ Files" />
                                                            </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sr. No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSrNo" Text='<%# Container.DataItemIndex + 1 %>' runat="server"> 
                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Submission Id" SortExpression="SM_ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSmId" Text='<%# Eval("SM_ID") %>' runat="server">
                                            </asp:Label>
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
                                                            <asp:Label ID="lblParticulars" Text='<%#Eval("SM_PERTICULARS").ToString().Replace("\n","<br />") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Description" SortExpression="SM_BRIEF_DESCRIPTION">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDescription" Text='<%#Eval("SM_BRIEF_DESCRIPTION").ToString().Replace("\n","<br />") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Effective Date" SortExpression="SM_EFFECTIVE_DT">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEffectiveDate" Text='<%#Eval("SM_EFFECTIVE_DT","{0:dd-MMM-yyyy}")%>'
                                                                runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Priority" SortExpression="Priority">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPriority" Text='<%#Eval("Priority")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Authority">
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
                                                    <asp:TemplateField HeaderText="Only Once - From Date" SortExpression="SM_ONLY_ONCE_FROM_DATE">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOnlyOnceFromDate" Text='<%#Eval("SM_ONLY_ONCE_FROM_DATE")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Only Once - To Date" SortExpression="SM_ONLY_ONCE_TO_DATE">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOnlyOnceToDate" Text='<%#Eval("SM_ONLY_ONCE_TO_DATE")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Annual - From Date" SortExpression="SM_YEARLY_DUE_DATE_FROM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAnnualFromDate" Text='<%#Eval("SM_YEARLY_DUE_DATE_FROM")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Annual - To Date" SortExpression="SM_YEARLY_DUE_DATE_TO">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAnnualToDate" Text='<%#Eval("SM_YEARLY_DUE_DATE_TO")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Monthly - From Date" SortExpression="SM_MONTHLY_DUE_DATE_FROM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMonthlyFromDate" Text='<%#Eval("SM_MONTHLY_DUE_DATE_FROM")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Monthly - To Date" SortExpression="SM_MONTHLY_DUE_DATE_TO">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMonthlyToDate" Text='<%#Eval("SM_MONTHLY_DUE_DATE_TO")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quarter 1 - From Date" SortExpression="SM_Q1_DUE_DATE_FROM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQ1FromDate" Text='<%#Eval("SM_Q1_DUE_DATE_FROM")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quarter 1 - To Date" SortExpression="SM_Q1_DUE_DATE_TO">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQ1ToDate" Text='<%#Eval("SM_Q1_DUE_DATE_TO")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quarter 2 - From Date" SortExpression="SM_Q2_DUE_DATE_FROM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQ2FromDate" Text='<%#Eval("SM_Q2_DUE_DATE_FROM")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quarter 2 - To Date" SortExpression="SM_Q2_DUE_DATE_TO">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQ2ToDate" Text='<%#Eval("SM_Q2_DUE_DATE_TO")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quarter 3 - From Date" SortExpression="SM_Q3_DUE_DATE_FROM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQ3FromDate" Text='<%#Eval("SM_Q3_DUE_DATE_FROM")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quarter 3 - To Date" SortExpression="SM_Q3_DUE_DATE_TO">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQ3ToDate" Text='<%#Eval("SM_Q3_DUE_DATE_TO")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quarter 4 - From Date" SortExpression="SM_Q4_DUE_DATE_FROM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQ4FromDate" Text='<%#Eval("SM_Q4_DUE_DATE_FROM")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quarter 4 - To Date" SortExpression="SM_Q4_DUE_DATE_TO">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQ4ToDate" Text='<%#Eval("SM_Q4_DUE_DATE_TO")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Half Year 1 - From Date" SortExpression="SM_FIRST_HALF_YR_DUE_DATE_FROM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHY1FromDate" Text='<%#Eval("SM_FIRST_HALF_YR_DUE_DATE_FROM")%>'
                                                                runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Half Year 1 - To Date" SortExpression="SM_FIRST_HALF_YR_DUE_DATE_TO">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHY1ToDate" Text='<%#Eval("SM_FIRST_HALF_YR_DUE_DATE_TO")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Half Year 2 - From Date" SortExpression="SM_SECOND_HALF_YR_DUE_DATE_FROM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHY2FromDate" Text='<%#Eval("SM_SECOND_HALF_YR_DUE_DATE_FROM")%>'
                                                                runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Half Year 2 - To Date" SortExpression="SM_SECOND_HALF_YR_DUE_DATE_TO">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHY2ToDate" Text='<%#Eval("SM_SECOND_HALF_YR_DUE_DATE_TO")%>' runat="server"></asp:Label>
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
                                                    <asp:BoundField DataField="SM_FIRST_FORTNIGHTLY_DUE_FROM_DATE" HeaderText="Fortnightly 1 - Due Date From"
                                                        SortExpression="SM_FIRST_FORTNIGHTLY_DUE_FROM_DATE" />
                                                    <asp:BoundField DataField="SM_FIRST_FORTNIGHTLY_DUE_TO_DATE" HeaderText="Fortnightly 1 - Due Date To"
                                                        SortExpression="SM_FIRST_FORTNIGHTLY_DUE_TO_DATE" />
                                                    <asp:BoundField DataField="SM_SECOND_FORTNIGHTLY_DUE_FROM_DATE" HeaderText="Fortnightly 2 - Due Date From"
                                                        SortExpression="SM_SECOND_FORTNIGHTLY_DUE_FROM_DATE" />
                                                    <asp:BoundField DataField="SM_SECOND_FORTNIGHTLY_DUE_TO_DATE" HeaderText="Fortnightly 2 - Due Date To"
                                                        SortExpression="SM_SECOND_FORTNIGHTLY_DUE_TO_DATE" />
                                                    <asp:BoundField DataField="SM_START_NO_OF_DAYS" HeaderText="Start No. of Days" SortExpression="SM_START_NO_OF_DAYS" />
                                                    <asp:BoundField DataField="SM_END_NO_OF_DAYS" HeaderText="End No. of Days" SortExpression="SM_END_NO_OF_DAYS" />
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


        <div style="width: 100%;">
            <%--<script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/jQuery_3.6.0.js") %>"></script>
            <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/jQuery_Migrate_3_3_2.js") %>"></script>
            <link type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/bootstrap.min.css") %>"
                rel="stylesheet" />
            <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/bootstrap.js") %>"></script>--%>

            <div class="modal fade bd-example-modal-lg" id="divModal" tabindex="-1" aria-labelledby="myExtraLargeModalLabel" role="dialog">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h6 class="modal-title">Checklist Acceptance Audit Trail</h6>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12 mb-3">
                                    <label class="form-label">&nbsp;</label>
                                    <asp:Label ID="lblChklistAuditTrail" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer" style="flex-direction: column;">
                            <center>
                                <input id="btncCancel" runat="server" type="button" value="Cancel" data-bs-dismiss="modal"
                                    class="modalCloseImg simplemodal-close btn btn-outline-danger" />
                            </center>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
