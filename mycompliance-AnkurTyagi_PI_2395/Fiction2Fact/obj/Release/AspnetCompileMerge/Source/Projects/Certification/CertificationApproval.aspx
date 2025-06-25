<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.Certification_CertificationApproval" Title="Certification Approval" Async="true" ValidateRequest="false" EnableEventValidation="false" CodeBehind="CertificationApproval.aspx.cs" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
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
        function ChangeCheckBoxState(id, checkState) {
            var cb = document.getElementById(id);
            if (cb != null && !cb.disabled)
                cb.checked = checkState;
        }

        function ChangeAllCheckBoxStates(checkState) {
            if (CheckBoxIDs != null) {
                for (var i = 0; i < CheckBoxIDs.length; i++) {
                    ChangeCheckBoxState(CheckBoxIDs[i], checkState);
                    if (i != 0) {
                        if (checkState) {
                            document.getElementById(commentsIDs[i]).disabled = false;
                        } else {
                            document.getElementById(commentsIDs[i]).disabled = true;
                        }
                    }
                }
            }
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

        function onRowCheckedUnchecked(cbid, txtrejectionid) {
            var cb = document.getElementById(cbid);
            var txtrejection = document.getElementById(txtrejectionid);
            if (cb != null) {
                if (cb.checked) {
                    txtrejection.disabled = false;
                }
                else {
                    txtrejection.disabled = true;
                    txtrejection.value = "";
                }
            }
            ChangeHeaderAsNeeded();
            return;
        }


        function validate(operationType) {
            var msg = '';
            var isAnyChkboxChecked = false;
            var isAllCheckBoxChecked = false;
            var statusValid = true;

            if (operationType == 'Approve') {
                if (CheckBoxIDs != null) {

                    for (var i = 1; i < CheckBoxIDs.length; i++) {
                        var cb = document.getElementById(CheckBoxIDs[i]);
                        var rowstatus = document.getElementById(statusIDs[i]).value;

                        if (rowstatus == 'L2R' || rowstatus == 'PFA' || rowstatus == 'D') {
                            msg = msg + ' There are approvals pending at the lower levels (as shown in the Status column). ' +
                                'Hence, you cannot approve the checklist for row no. ' + i + '.\n';
                            isAllCheckBoxChecked = false;
                            break;
                        }
                        else {
                            if (cb.checked) {
                                isAllCheckBoxChecked = true;
                            }
                            else {
                                if (rowstatus == 'L1A') {
                                    isAllCheckBoxChecked = false;
                                    break;
                                }
                            }
                        }

                    }

                    if (!isAllCheckBoxChecked) {
                        msg = msg + 'Approval of all records (with status as Submitted) has to be done together. Please select all records (with status as Submitted) to approve.' + '\n';
                    }
                }
            }
            else if (operationType == 'Reject') {
                if (CheckBoxIDs != null) {
                    for (var i = 1; i < CheckBoxIDs.length; i++) {
                        var cb = document.getElementById(CheckBoxIDs[i]);
                        if (cb.checked) {
                            isAnyChkboxChecked = true;
                            var rowstatus = document.getElementById(statusIDs[i]).value;

                            if (rowstatus == 'L1A' || rowstatus == 'L3R') //Submitted
                            {
                                var comment = document.getElementById(commentsIDs[i]).value;
                                if (comment == '') {
                                    msg = msg + 'Comments are mandatory for rejection for row no. ' + i + '.\n';
                                }
                            }
                            else {
                                msg = msg + ' There are approvals pending at the lower/upper levels (as shown in the Status column). ' +
                                    'Hence, you cannot reject for row no. ' + i + '.\n';
                            }
                        }
                    }

                    if (!isAnyChkboxChecked) {
                        msg = msg + '- Please select one or more records for rejection.';
                    }
                }
            }

            if (!statusValid) {
                msg = msg + 'Please select records with valid status.';
            }

            if (msg != '') {
                alert("" + msg);
                return false;
            }
            else {
                if (operationType == 'Approve') {
                    if (!confirm('Are you sure that you want to approve the selected certificate(s)?')) return false;
                    return true;
                }
                else if (operationType == 'Reject') {
                    if (!confirm('Are you sure that you want to reject the selected certificate(s)?')) return false;
                    return true;
                }
            }
        }


    </script>

    <script type="text/javascript">
        function onViewClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "View";
        }

        function switchTab(tabName) {
            var targetTab = document.querySelector('a[href="#' + tabName + '"]');
            var tab = new bootstrap.Tab(targetTab);
            tab.show();
        }

    </script>

    <asp:HiddenField runat="server" ID="hfContent" />
    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField runat="server" ID="hfTabberId" />
    <asp:HiddenField runat="server" ID="hfDepartmentID" />
    <asp:HiddenField runat="server" ID="hfCertDepartment" />
    <asp:HiddenField runat="server" ID="hfType" />
    <asp:HiddenField runat="server" ID="HiddenField1" />
    <asp:HiddenField runat="server" ID="hfCXOName" />
    <asp:HiddenField runat="server" ID="hfFunctionName" />
    <asp:HiddenField runat="server" ID="hfSPOCName" />
    <asp:HiddenField runat="server" ID="hfCertMId" />
    <asp:HiddenField ID="hfQuarterId" runat="server" />
    <asp:HiddenField runat="server" ID="hfCertContent" />
    <asp:HiddenField ID="hfFunctionId" runat="server" />

    <asp:Literal ID="CheckBoxIDsArray" runat="server"></asp:Literal>

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Unit Head Certification</h4>
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
                    <asp:Panel ID="pnlCertificationDashboards" runat="server" Visible="true">
                        <div class="mb-3">
                            <asp:Label ID="Label1" runat="server" Text="Quarterly Compliance Certificate Checklist for the quarter is pending for approval."
                                CssClass="custom-info-alert"></asp:Label>
                        </div>
                        <div class="mb-3">
                            <asp:Label ID="Label2" runat="server" Text="Please click on the 'View' icon to view the quarterly compliance certification."
                                CssClass="custom-info-alert mt-2"></asp:Label>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="gvCertDashboard" runat="server" BorderStyle="None" BorderWidth="1px"
                                CellPadding="4" GridLines="None" AutoGenerateColumns="false" CssClass="table table-bordered footable"
                                OnSelectedIndexChanged="gvCertDashboard_SelectedIndexChanged" DataKeyNames="DeptId"
                                AllowSorting="false" AllowPaging="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="View" ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Select"
                                                CssClass="btn btn-sm btn-soft-info btn-circle">
                                                <i class="fa fa-eye"></i>	                            
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit Name" SortExpression="DeptName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDeptName" runat="server" Text='<%# Bind("DeptName") %>' Visible="true"></asp:Label>
                                            <asp:Label ID="lblDeptId" runat="server" Text='<%# Bind("DeptId") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblQuarterId" runat="server" Text='<%# Bind("CERT_CQM_ID") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Quarter" SortExpression="CDM_NAME">
                                        <ItemTemplate>  
                                            <asp:Label ID="lblQuarter" runat="server" Text='<%# Eval("CQM_FROM_DATE","{0:dd-MMM-yyyy}").ToString() + " to " + Eval("CQM_TO_DATE","{0:dd-MMM-yyyy}").ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--<asp:BoundField HeaderText="Status" DataField="Status" SortExpression="Status" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlCertDetails" runat="server" Visible="false">
                        <div class="mt-3 mb-3">
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click">
                                    <i class="fa fa-arrow-left me-2"></i> Back
                            </asp:LinkButton>
                        </div>
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
                            <li class="nav-item">
                                <a class="nav-link" data-bs-toggle="tab" href="#settings1" role="tab" aria-selected="false">Regulatory Filing</a>
                            </li>
                        </ul>

                        <!-- Tab panes -->
                        <div class="tab-content">
                            <div class="tab-pane p-3 active" id="home" role="tabpanel">
                                <div class="row">
                                    <div class="col-md-12 mb-3">
                                        <label class="form-label">I certify the following:</label>
                                        <asp:Label runat="server" ID="lblCertContents"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane p-3" id="profile" role="tabpanel">
                                <div id="demo-basic2" visible="false">
                                    <div class="mb-3">
                                        <div class="mb-2">
                                            <asp:Label ID="lblNote" runat="server" Text="<strong> Kindly note the following points: </strong>"
                                                CssClass=""></asp:Label>
                                        </div>
                                        <div class="input-group">
                                            <i class="fa fa-arrow-alt-circle-right me-2"></i>
                                            <span>1. Approvals for all the records have to be done together.</span>
                                        </div>
                                        <div class="input-group">
                                            <i class="fa fa-arrow-alt-circle-right me-2"></i>
                                            <span>2. For revision suggestion, please select the record (click the checkbox), enter the comments and then click on 'Suggest Revision' button.</span>
                                        </div>
                                    </div>
                                    <div class="mb-3">
                                        <asp:LinkButton runat="server" ID="ImageButton1" CssClass="btn btn-outline-secondary" OnClick="btnExportToExcel_Click" ToolTip="Export to Excel">
                                            <i class="fa fa-download"></i> Export to Excel               
                                        </asp:LinkButton>
                                    </div>
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvCertApproval" runat="server" AutoGenerateColumns="False" DataKeyNames="CERT_ID"
                                            GridLines="None" CellPadding="4" OnDataBound="gvCertApproval_DataBound" CssClass="table table-bordered footable"
                                            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnRowDataBound="gvCertApproval_OnRowDataBound">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox runat="server" ID="HeaderLevelCheckBox" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="RowLevelCheckBox" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="View">
                                                    <ItemTemplate>
                                                        <a href="javascript:void(0);" onclick="javascript:window.open('./ViewChecklist.aspx?CertID=<%# Eval("CERT_ID") %>','','location=0,status=0,scrollbars=1,width=900,height=800,resizable=1');"
                                                            class="btn btn-sm btn-soft-info btn-circle" style="text-align: center;">
                                                            <i class="fa fa-eye"></i>
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:LinkButton CssClass="btn btn-sm btn-soft-success btn-circle" CommandArgument='<%#Eval("CERT_ID") %>'
                                                            ID="lnlEdit" runat="server" OnClick="lnlEdit_Click">
                                                            <i class="fa fa-pen"></i>	                            
                                                        </asp:LinkButton>
                                                        <%--<a href="javascript:void(0);" onclick="javascript:window.open('./EditChecklists.aspx?CertID=<%# Eval("CERT_ID") %>','','location=0,status=0,scrollbars=1,width=900,height=800,resizable=1');"
                            style="text-align: center;">
                            <img src="../../Content/images/legacy/EditInformationHS.png" border="0" alt="View Details" style="text-align: center;" />
                        </a>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Sr.No.
                           
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblId" Text='<%# Bind("CERT_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sub Unit Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDepartmentId" runat="server" Text='<%# Bind("DeptId") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblDepartment" runat="server" Text='<%# Bind("DeptName") %>' Visible="true"></asp:Label>
                                                        <asp:HiddenField ID="hfStatus" runat="server" Value='<%#Eval("CERT_STATUS")%>'></asp:HiddenField>
                                                        <asp:HiddenField ID="hfCertStatus" runat="server" Value='<%#Eval("Status")%>'></asp:HiddenField>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quarter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuarter" runat="server" Text='<%# Eval("CQM_FROM_DATE","{0:dd-MMM-yyyy}") +" to " + Eval("CQM_TO_DATE","{0:dd-MMM-yyyy}") %>'
                                                            Visible="true" Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Submitted By">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubmittedBy" runat="server" Text='<%# Bind("Approval_By") %>' Visible="true"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Submitted On">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubmittedOn" runat="server" Text='<%# Bind("Approval_Date","{0:dd-MMM-yyyy HH:mm:ss}") %>'
                                                            Visible="true"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Submission/Approval Comments">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="lblSubmittedRemarks" runat="server" Text='<%# Eval("Approval_Comments").ToString().Replace("\n", "<br />") %>'
                                                            Visible="true"></asp:Label>--%>
                                                        <%--<<Modified by Ankur Tyagi on 01Feb2024 for CR_1945--%>
                                                        <asp:Label ID="lblSubmittedRemarks" Width="150px" runat="server" ToolTip='<%#Eval("Approval_Comments").ToString()%>'
                                                            Text='<%#Eval("Approval_Comments").ToString().Length > 200 ? (Eval("Approval_Comments") as string).Substring(0, 200).Replace("\n", "<br />") 
                                                    + " ..." : Eval("Approval_Comments").ToString().Replace("\n", "<br />") %>'
                                                            Visible="true"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Status" DataField="Status" SortExpression="Status" />
                                                <asp:TemplateField HeaderText="Suggested Revision">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="lblRejectionRemarks" runat="server" Text='<%# Eval("Rejection_Comments").ToString().Replace("\n", "<br />") %>'
                                                            Visible="true"></asp:Label>--%>
                                                        <%--<<Modified by Ankur Tyagi on 01Feb2024 for CR_1945--%>
                                                        <asp:Label ID="lblRejectionRemarks" Width="150px" runat="server" ToolTip='<%#Eval("Rejection_Comments").ToString()%>'
                                                            Text='<%#Eval("Rejection_Comments").ToString().Length > 200 ? (Eval("Rejection_Comments") as string).Substring(0, 200).Replace("\n", "<br />") 
                                                    + " ..." : Eval("Rejection_Comments").ToString().Replace("\n", "<br />") %>'
                                                            Visible="true"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Audit Trail" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAuditTrail" Width="300px" runat="server" Text='<%#Eval("CERT_AUDIT_TRAIL").ToString().Replace("\n", "<br />") %>'
                                                            Visible="true"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Remarks
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <F2FControls:F2FTextBox ID="txtRemarks" TextMode="MultiLine" Width="300px" Rows="5" Columns="50" Enabled="False"
                                                            CssClass="form-control" runat="server"></F2FControls:F2FTextBox>
                                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="mt-3">
                                        <asp:LinkButton ID="btnApprove" runat="server" Text="Approve" CssClass="btn btn-outline-success"
                                            OnClick="btnApprove_Click" OnClientClick="return validate('Approve')">
                                            <i class="fa fa-paper-plane me-2"></i> Approve
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnReject" runat="server" Text="Suggest Revision" CssClass="btn btn-outline-primary" OnClick="btnReject_Click"
                                            OnClientClick="return validate('Reject')">
                                            <i class="fa fa-info"></i> Suggest Revision
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane p-3" id="settings" role="tabpanel">
                                <div class="table-responsive">
                                    <asp:Literal ID="litControls" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="tab-pane p-3" id="settings1" role="tabpanel">
                                <div title="Regulatory Filing" id="demo-basic4">
                                    <div class="table-responsive">
                                        <asp:Literal runat="server" ID="litRegulatoryFilling" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
