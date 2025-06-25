<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    ValidateRequest="false" Inherits="Fiction2Fact.Projects.Certification.Certification_CertSearch" Title="View Certifications " CodeBehind="CertSearch.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
        TagPrefix="f2f" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

    <script type="text/javascript">
        function onViewClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "View";
        }

        function onClientDeleteClick() {
            if (!confirm('Are you sure that you want to delete this record?')) return false;
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Delete";
        }
    </script>

    <script type="text/javascript">

        /* Optional: Temporarily hide the "tabber" class so it does not "flash"
           on the page as plain HTML. After tabber runs, the class is changed
           to "tabberlive" and it will appear. */

        document.write('<style type="text/css">.tabber{display:none;}<\/style>');

        /*==================================================ce
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

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/tabcontrol.js")%>'>
    </script>

    <%--start - added by Hari on 3 oct 2016--%>

    <script type="text/javascript">

        function openViewChecklistPopup(requestId) {

            //alert(requestId);
            window.open('../Certification/ViewChecklistData.aspx?ChecklistId=' + requestId,
                '', 'location=0,status=0,scrollbars=1,resizable=1,width=700,height=500');
            return false;
        }
    </script>

    <%--End - added by Hari on 3 oct 2016--%>
    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField runat="server" ID="hfContent" />
    <asp:HiddenField runat="server" ID="hfTabberId" />
    <asp:HiddenField runat="server" ID="hfLevel" />


    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Search Certificates</h4>
                        <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
                        <asp:Label ID="lblInfo" runat="server" CssClass="label"></asp:Label>
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
                <asp:MultiView ID="mvMultiView" runat="server">
                    <asp:View ID="vwGrid" runat="server">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Quarter:</label>
                                    <asp:DropDownList CssClass="form-select" ID="ddlQuarter" runat="server" DataValueField="CQM_ID"
                                        DataTextField="Quarter">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Level:</label>
                                    <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlLevelSearch" runat="server">
                                    </f2f:DropdownListNoValidation>
                                    <ajaxToolkit:CascadingDropDown ID="cddLevelSearch" runat="server" TargetControlID="ddlLevelSearch"
                                        PromptText="Select a Level" ServicePath="AJAXDropdownCertification.asmx" ServiceMethod="GetCertLevels"
                                        Category="CertLevel" />
                                    <asp:RequiredFieldValidator ID="rfvLevelSearch" ValidationGroup="SEARCH" runat="server"
                                        ControlToValidate="ddlLevelSearch" Display="Dynamic" CssClass="text-danger" SetFocusOnError="True">Please select Level.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Function / Unit / Sub-Unit:</label>
                                    <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlDeptName" runat="server">
                                    </f2f:DropdownListNoValidation>
                                    <ajaxToolkit:CascadingDropDown ID="cddDeptName" runat="server" TargetControlID="ddlDeptName"
                                        ParentControlID="ddlLevelSearch" PromptText="Select Department" Category="SelectedLevel"
                                        ServicePath="AJAXDropdownCertification.asmx" ServiceMethod="GetCertRelevantFuncsBasedOnLevel" />
                                </div>
                            </div>
                            <div class="mt-3 text-center">
                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnSearch" Text="Search" runat="server" ValidationGroup="SEARCH"
                                    AccessKey="s" OnClick="btnSearch_Click">
                                    <i class="fa fa-search"></i> Search                          
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gvSearchCert" runat="server" AutoGenerateColumns="False" DataKeyNames="CERT_ID"
                                    GridLines="None" CellPadding="4" OnSelectedIndexChanged="gvSearchCert_SelectedIndexChanged"
                                    CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Sr.No.
                           
                                           
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                </center>
                                                <asp:HiddenField ID="hfLevel" runat="server" Value='<%# Bind("CERTM_LEVEL_ID") %>'></asp:HiddenField>
                                                <asp:HiddenField ID="hfDeptId" runat="server" Value='<%# Bind("DeptId") %>'></asp:HiddenField>
                                                <asp:HiddenField ID="hfQuarterId" runat="server" Value='<%# Bind("CQM_ID") %>'></asp:HiddenField>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Department Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("DeptName") %>' Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="From Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFromDate" runat="server" Text='<%# Bind("CQM_FROM_DATE","{0:dd-MMM-yyyy}") %>'
                                                    Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="To Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblToDate" runat="server" Text='<%# Bind("CQM_TO_DATE","{0:dd-MMM-yyyy}") %>'
                                                    Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="View" ShowHeader="true">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:LinkButton runat="server" ID="ibView" CssClass="btn btn-sm btn-soft-info btn-circle"
                                                        CommandName="Select" OnClientClick="onViewClick()" ToolTip="View Details">
                                                        <i class="fa fa-eye"></i>
                                                    </asp:LinkButton>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete" ShowHeader="true" Visible="false">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:LinkButton CssClass="btn btn-sm btn-soft-danger btn-circle" runat="server" ID="ibDelete" CommandName="Select"
                                                        OnClientClick="return onClientDeleteClick()" ToolTip="Delete">
                                                        <i class="fa fa-trash"></i>
                                                    </asp:LinkButton>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwForm" runat="server">
                        <div class="card-body">
                            <!-- Nav tabs -->
                            <ul class="nav nav-tabs" role="tablist">
                                <li class="nav-item">
                                    <a class="nav-link active" data-bs-toggle="tab" href="#home" role="tab" aria-selected="true">Certification</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" data-bs-toggle="tab" href="#profile" role="tab" aria-selected="false">Compliance Checklist</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" data-bs-toggle="tab" href="#settings" role="tab" aria-selected="false">Compliance Deviations</a>
                                </li>
                                <li class="nav-item" style="display: none; visibility: hidden">
                                    <a class="nav-link" data-bs-toggle="tab" href="#settings1" role="tab" aria-selected="false">Regulatory Filing</a>
                                </li>
                            </ul>

                            <!-- Tab panes -->
                            <div class="tab-content">
                                <div class="tab-pane p-3 active" id="home" role="tabpanel">
                                    <div class="mb-3">
                                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToPdf1" runat="server" Text="Export to Doc"
                                            OnClick="btnExportToDoc_Click">
                                            <i class="fa fa-download"></i> Export to Doc               
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnConvertToPdf" runat="server" Text="Export to Pdf"
                                            OnClick="btnConvertToPdf_Click">
                                            <i class="fa fa-download"></i> Export to Pdf 
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack1" runat="server" Text="Back" CausesValidation="false"
                                            OnClick="btnViewCancel_Click">
                                            <i class="fa fa-arrow-left me-2"></i> Back                   
                                        </asp:LinkButton>
                                    </div>
                                    <asp:FormView ID="fvEditCert" runat="server" DataKeyNames="CERT_ID" Width="100%">
                                        <ItemTemplate>
                                            <div class="tabular-view">
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Certificate Id:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_ID")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Department:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("DeptName")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>From Date:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label ID="lblQtrFromDT" runat="server" Text='<%# Eval("CQM_FROM_DATE", "{0:dd-MMM-yyyy}")%>'></asp:Label>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>To Date:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <asp:Label ID="lblQtrToDT" runat="server" Text='<%# Eval("CQM_TO_DATE", "{0:dd-MMM-yyyy}")%>'></asp:Label>

                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Status:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("Status")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Submitted By:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_SUBMITTED_BY")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Submitted on:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_SUBMITTED_ON", "{0:dd-MMM-yyyy HH:mm:ss tt}")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Remarks:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_SUBMITTED_REMARKS").ToString().Replace("\n", "<br />")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Unit Head Approved By:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_APPROVED_BY_LEVEL1")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Unit Head Approved on:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_APPROVED_DT_LEVEL1", "{0:dd-MMM-yyyy HH:mm:ss tt}")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Unit Head Approved Remarks:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_APPROVED_REMARKS_LEVEL1").ToString().Replace("\n", "<br />")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0" style="display:none;visibility:hidden">
                                                    <div class="col-md-3">
                                                        <label>Revision Suggested By( Unit Head ):</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_REJECTED_BY_LEVEL1")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0" style="display:none;visibility:hidden">
                                                    <div class="col-md-3">
                                                        <label>Revision Suggested on(Unit Head ):</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_REJECTED_DT_LEVEL1", "{0:dd-MMM-yyyy HH:mm:ss tt}")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0" style="display:none;visibility:hidden">
                                                    <div class="col-md-3">
                                                        <label>Suggested Revision( Unit Head):</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_REJECTED_REMARKS_LEVEL1").ToString().Replace("\n", "<br />")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <%--<<Added by Ankur tyagi on 18Jan2024--%>
                                                <%-->>--%>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Function Head Approved By:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_APPROVED_BY_LEVEL2")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Function Head Approved on:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_APPROVED_DT_LEVEL2", "{0:dd-MMM-yyyy HH:mm:ss tt}")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Function Head Approved Remarks:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_APPROVED_REMARKS_LEVEL2").ToString().Replace("\n", "<br />")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Revision Suggested By(Function Head):</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_REJECTED_BY_LEVEL2")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Revision Suggested on(Function Head):</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_REJECTED_DT_LEVEL2", "{0:dd-MMM-yyyy HH:mm:ss tt}")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0 border-bottom">
                                                    <div class="col-md-3">
                                                        <label>
                                                            Suggested Revision(Function Head):</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_REJECTED_REMARKS_LEVEL2").ToString().Replace("\n", "<br />")%>
                                                        </label>
                                                    </div>
                                                </div>

                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Compliance User Approved By:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_APPROVED_BY_LEVEL3")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Compliance User Approved on:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_APPROVED_DT_LEVEL3", "{0:dd-MMM-yyyy HH:mm:ss tt}")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Compliance User Approved Remarks:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_APPROVED_REMARKS_LEVEL3").ToString().Replace("\n", "<br />")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Revision Suggested By( Compliance User ):</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_REJECTED_BY_LEVEL3")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Revision Suggested on( Compliance User ):</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_REJECTED_DT_LEVEL3", "{0:dd-MMM-yyyy HH:mm:ss tt}")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Suggested Revision( Compliance User):</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_REJECTED_REMARKS_LEVEL3").ToString().Replace("\n", "<br />")%>
                                                        </label>
                                                    </div>
                                                </div>

                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Executive Committee Approved By:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_APPROVED_BY_LEVEL4")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Executive Committee Approved on:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_APPROVED_DT_LEVEL4", "{0:dd-MMM-yyyy HH:mm:ss tt}")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Executive Committee Approved Remarks:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_APPROVED_REMARKS_LEVEL4").ToString().Replace("\n", "<br />")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Revision Suggested By(Executive Committee):</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_REJECTED_BY_LEVEL4")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0">
                                                    <div class="col-md-3">
                                                        <label>Revision Suggested on(Executive Committee):</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_REJECTED_DT_LEVEL4", "{0:dd-MMM-yyyy HH:mm:ss tt}")%>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="row g-0 border-bottom">
                                                    <div class="col-md-3">
                                                        <label>
                                                            Suggested Revision(Executive Committee):</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label>
                                                            <%# Eval("CERT_REJECTED_REMARKS_LEVEL4").ToString().Replace("\n", "<br />")%>
                                                        </label>
                                                    </div>
                                                </div>


                                            </div>

                                            <div class="mt-3">
                                                <div class="card mb-1 mt-1 border">
                                                    <div class="card-header py-0 custom-ch-bg-color">
                                                        <h6 class="font-weight-bold text-white mtb-5">Content: </h6>
                                                    </div>
                                                    <div class="card-body mt-1">
                                                        <div>
                                                            <%# Eval("CERT_CONTENT").ToString().Replace("\n", "<br />")%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="mt-3" style="display:none;visibility:hidden">
                                                <div class="card mb-1 mt-1 border">
                                                    <div class="card-header py-0 custom-ch-bg-color">
                                                        <h6 class="font-weight-bold text-white mtb-5">Remarks: </h6>
                                                    </div>
                                                    <div class="card-body mt-1">
                                                        <div>
                                                            <%# Eval("CERT_REMARKS").ToString().Replace("\n", "<br />")%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:FormView>
                                    <div class="mt-3">
                                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="Button2" runat="server" Text="Export to Doc"
                                            OnClick="btnExportToDoc_Click">
                                            <i class="fa fa-download"></i> Export to Doc 
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="Button5" runat="server" Text="Export to Pdf"
                                            OnClick="btnConvertToPdf_Click">
                                            <i class="fa fa-download"></i> Export to Pdf 
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack2" runat="server" Text="Back" CausesValidation="false"
                                            OnClick="btnViewCancel_Click">
                                            <i class="fa fa-arrow-left me-2"></i> Back   
                                        </asp:LinkButton>
                                    </div>
                                </div>
                                <div class="tab-pane p-3" id="profile" role="tabpanel">
                                    <asp:Panel ID="pnlChecklistShow" runat="server">
                                        <div class="mb-3">
                                            <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcel" runat="server" Text="Export To Excel"
                                                OnClick="btnExportToExcel_Click">
                                                <i class="fa fa-download"></i> Export to Excel               
                                            </asp:LinkButton>
                                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack3" runat="server" Text="Back" CausesValidation="false"
                                                OnClick="btnViewCancel_Click">
                                                <i class="fa fa-arrow-left me-2"></i> Back                   
                                            </asp:LinkButton>
                                        </div>
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvChecklist" runat="server" BorderStyle="None" BorderWidth="1px"
                                                CellPadding="4" GridLines="None" AutoGenerateColumns="false" CssClass="table table-bordered footable"
                                                DataKeyNames="ID">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Sr.No.
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Department Name" SortExpression="DeptName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDeptName" Text='<%#Eval("DeptName").ToString().Replace("\n", "<br />") %>'
                                                                runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reference Circular/Notification/Act" SortExpression="CCM_REFERENCE">
                                                        <ItemTemplate>
                                                            <%--Text='<%#Eval("CCM_REFERENCE").ToString().Replace("\n", "<br />") %>'--%>
                                                            <asp:Label ID="txtRegulations" Text='<%#Eval("CCM_REFERENCE").ToString().Length>100?(Eval("CCM_REFERENCE") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_REFERENCE").ToString().Replace("\n", "<br />")  %>'
                                                                ToolTip='<%#Eval("CCM_REFERENCE")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Section/Clause" SortExpression="CCM_CLAUSE">
                                                        <ItemTemplate>
                                                            <%--Text='<%#Eval("CCM_CLAUSE").ToString().Replace("\n", "<br />") %>'--%>
                                                            <%--Start - commented by Hari on 3 oct 2016 --%>
                                                            <%-- <asp:Label ID="txtSections"
                                                          Text='<%#Eval("CCM_CLAUSE").ToString().Length>100?(Eval("CCM_CLAUSE") as string).Substring(0,100).Replace(Environment.NewLine, "<br />") + " ...":Eval("CCM_CLAUSE").ToString().Replace(Environment.NewLine, "<br />")  %>'
                                                        ToolTip='<%#Eval("CCM_CLAUSE")%>'
                                                             runat="server"></asp:Label>--%>
                                                            <%--End - commented by Hari on 3 oct 2016 --%>
                                                            <%--Start - added by Hari on 3 oct 2016 --%>
                                                            <asp:LinkButton ID="lnkbtntxtSections" runat="server" CssClass="badge rounded-pill badge-soft-pink" Text='<%#Eval("CCM_CLAUSE").ToString().Length>100?(Eval("CCM_CLAUSE") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_CLAUSE").ToString().Replace("\n", "<br />")  %>'
                                                                ToolTip='<%#Eval("CCM_CLAUSE")%>' Font-Underline="false" OnClientClick='<%# string.Format("return openViewChecklistPopup(\"{0}\");", Eval("CCD_ID")) %>'>
                                                            </asp:LinkButton>
                                                            <%--End - added by Hari on 3 oct 2016 --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Compliance of/Heading of Compliance checklist" SortExpression="CCM_CHECK_POINTS">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtCheckpoints" Text='<%#Eval("CCM_CHECK_POINTS").ToString().Length>100?(Eval("CCM_CHECK_POINTS") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_CHECK_POINTS").ToString().Replace("\n", "<br />")  %>'
                                                                ToolTip='<%#Eval("CCM_CHECK_POINTS")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Description" SortExpression="CCM_PARTICULARS">
                                                        <ItemTemplate>
                                                            <%--Text='<%#Eval("CCM_REFERENCE").ToString().Replace("\n", "<br />") %>'--%>
                                                            <asp:Label ID="txtDescription" Text='<%#Eval("CCM_PARTICULARS").ToString().Length>100?(Eval("CCM_PARTICULARS") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_PARTICULARS").ToString().Replace("\n", "<br />")  %>'
                                                                ToolTip='<%#Eval("CCM_PARTICULARS")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Consequences of non Compliance" SortExpression="CCM_PENALTY">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtPenalty" Text='<%#Eval("CCM_PENALTY").ToString().Replace("\n", "<br />") %>'
                                                                runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="CCM_CLAUSE" HeaderText="Title of Section/Requirement" />
                                                <asp:BoundField HeaderText="Section/Regulation Rule/Circulars" DataField="CCM_REFERENCE" />--%>
                                                    <asp:TemplateField HeaderText="Frequency" SortExpression="CCM_FREQUENCY">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtTimeLimit" Text='<%#Eval("CCM_FREQUENCY").ToString().Length>100?(Eval("CCM_FREQUENCY") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_FREQUENCY").ToString().Replace("\n", "<br />")  %>'
                                                                ToolTip='<%#Eval("CCM_FREQUENCY")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Compliance Status" DataField="Compliance_Status" />
                                                    <asp:TemplateField HeaderText="Remarks" SortExpression="CCD_REMARKS">
                                                        <ItemTemplate>
                                                            <%--Text='<%#Eval("CCD_REMARKS").ToString().Replace("\n", "<br />") %>'--%>
                                                            <asp:Label ID="txtRemarks" Text='<%#Eval("CCD_REMARKS").ToString().Length>100?(Eval("CCD_REMARKS") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCD_REMARKS").ToString().Replace("\n", "<br />")  %>'
                                                                ToolTip='<%#Eval("CCD_REMARKS")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--//<<Modified by Ankur Tyagi on 05Feb2024 for CR_1948--%>
                                                    <asp:TemplateField HeaderText="Non-compliant since" SortExpression="CCD_NC_SINCE_DT">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtCCD_NC_SINCE_DT" Text='<%#Bind("CCD_NC_SINCE_DT","{0:dd-MMM-yyyy}") %>'
                                                                runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--//<<Modified by Ankur Tyagi on 02Feb2024 for CR_1945--%>
                                                    <asp:TemplateField HeaderText="Action Plan" SortExpression="CCD_ACTION_PLAN">
                                                        <ItemTemplate>
                                                            <%--Text='<%#Eval("CCD_REMARKS").ToString().Replace("\n", "<br />") %>'--%>
                                                            <asp:Label ID="txtAp" Text='<%#Eval("CCD_ACTION_PLAN").ToString().Length>100?(Eval("CCD_ACTION_PLAN") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCD_ACTION_PLAN").ToString().Replace("\n", "<br />")  %>'
                                                                ToolTip='<%#Eval("CCD_ACTION_PLAN")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Target Date" SortExpression="CCD_TARGET_DATE">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtccdTargetDate" Text='<%#Bind("CCD_TARGET_DATE","{0:dd-MMM-yyyy}") %>'
                                                                runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%-->>--%>
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
                                                    <%--<asp:TemplateField HeaderText="Target Date" SortExpression="CCD_TARGET_DATE">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtTargetDate" Text='<%#Bind("CCD_TARGET_DATE","{0:dd-MMM-yyyy}") %>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action Plan" SortExpression="CCD_ACTION_PLAN">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtActionPlan" Text='<%#Eval("CCD_ACTION_PLAN").ToString().Replace("\n", "<br />")%>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>

                                    <div class="mt-3">
                                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcel1" runat="server" Text="Export To Excel"
                                            OnClick="btnExportToExcel_Click">
                                            <i class="fa fa-download"></i> Export to Excel   
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="Button1" runat="server" Text="Back" CausesValidation="false"
                                            OnClick="btnViewCancel_Click">
                                            <i class="fa fa-arrow-left me-2"></i> Back    
                                        </asp:LinkButton>
                                    </div>
                                </div>
                                <div class="tab-pane p-3" id="settings" role="tabpanel">
                                    <div class="mb-3">
                                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcelExceptions" runat="server"
                                            Text="Export To Excel" OnClick="btnExportToExcelExceptions_Click">
                                            <i class="fa fa-download"></i> Export to Excel 
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnViewCancel2" runat="server" Text="Back"
                                            CausesValidation="false" OnClick="btnViewCancel_Click">
                                            <i class="fa fa-arrow-left me-2"></i> Back    
                                        </asp:LinkButton>
                                    </div>
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvException" runat="server" AutoGenerateColumns="False" DataKeyNames="CE_ID"
                                            AllowSorting="true" AllowPaging="true" GridLines="Both" CellPadding="4" CssClass="table table-bordered footable"
                                            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Sr.No.
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Department Name" SortExpression="DeptName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDeptName" Text='<%#Eval("DeptName").ToString().Replace("\n", "<br />") %>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Attached File">
                                                    <ItemTemplate>
                                                        <a href="javascript:void(0)" onclick="javascript:window.open('../DownloadFileCertification.aspx?FileInformation=<%#(Eval("CE_SERVER_FILE_NAME"))%>','','location=0,status=0,scrollbars=0,width=10,height=10');">
                                                            <%#Eval("CE_CLIENT_FILE_NAME")%>
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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
                                    <div class="mt-3">
                                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="Button3" runat="server" Text="Export To Excel"
                                            OnClick="btnExportToExcelExceptions_Click">
                                            <i class="fa fa-download"></i> Export to Excel 
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="Button4" runat="server" Text="Back" CausesValidation="false"
                                            OnClick="btnViewCancel_Click">
                                            <i class="fa fa-arrow-left me-2"></i> Back
                                        </asp:LinkButton>
                                    </div>
                                </div>
                                <div class="tab-pane p-3" id="settings1" role="tabpanel">
                                    <div id="demo-basic4">
                                        <div class="table-responsive">
                                            <asp:Literal runat="server" ID="litRegulatoryFilling" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
    <!-- end row -->

</asp:Content>
