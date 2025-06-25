<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Async="true" Inherits="Fiction2Fact.Projects.Certification.Certification_CommonCertification_CCO" Title="Common Certifications" CodeBehind="CommonCertification_CCO.aspx.cs" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%-- Added by Nikhil Adhalikar on 20-Sep-2011 --%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/tabcontrol.js")%>'>
    </script>

    <asp:HiddenField runat="server" ID="hfTabberId" />
    <asp:HiddenField ID="hfQuarterId" runat="server" />
    <asp:HiddenField ID="hfCertMId" runat="server" />
    <asp:HiddenField ID="hfCCId" runat="server" />
    <asp:HiddenField ID="hfDesignation" runat="server" />
    <asp:HiddenField ID="hfCAOEmailId" runat="server" />
    <asp:HiddenField ID="hfCEOEmailId" runat="server" />
    <asp:HiddenField ID="hfCFOEmailId" runat="server" />
    <asp:HiddenField ID="hfEmailId" runat="server" />
    <asp:HiddenField ID="hfSymbol" runat="server" />
    <asp:HiddenField ID="hfQuarterEndDt" runat="server" />

    <%--<center>
        <div class="ContentHeader1">
            Quarterly Certification
        </div>
    </center>
    <br />
    <center>
        <asp:Label ID="lblMsg" runat="server" CssClass="label"></asp:Label>
    </center>--%>
    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Compliance Certification</h4>
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
    <asp:Panel ID="PnlCertStatus" runat="server" Visible="true">
        <asp:MultiView ID="MvCert" runat="server">
            <asp:View ID="View1" runat="server">
                <div class="row">
                    <div class="col-12">
                        <div class="card">
                            <div class="card-body">
                                <ul class="nav nav-tabs" role="tablist">
                                    <li class="nav-item">
                                        <a class="nav-link active" data-bs-toggle="tab" href="#home" role="tab" aria-selected="true">Compliance Certification</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" data-bs-toggle="tab" href="#profile" role="tab" aria-selected="false">Executive Committee Certifications</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" data-bs-toggle="tab" href="#settings" role="tab" aria-selected="false">Deviation Details</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" data-bs-toggle="tab" href="#settings1" role="tab" aria-selected="false">Regulatory Filing</a>
                                    </li>
                                    <li class="nav-item" style="display: none; visibility: hidden">
                                        <a class="nav-link" data-bs-toggle="tab" href="#settings2" role="tab" aria-selected="false">Certification Dashboard</a>
                                    </li>
                                </ul>
                                <div class="tab-content">
                                    <div class="tab-pane p-3 active" id="home" role="tabpanel">
                                        <div class="row">
                                            <div class="col-md-12 mb-3">
                                                <label class="form-label">Certificate:</label>
                                                <asp:Label ID="lblCertContents" runat="server" CssClass="form-control"></asp:Label>
                                            </div>
                                            <div class="tabbertab" title="Joint Certification">
                                                <div class="col-md-12 mb-3">
                                                    <label class="form-label">Remarks:</label>
                                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtRemarks" runat="server" TextMode="MultiLine"
                                                        Rows="5" Columns="120"></F2FControls:F2FTextBox>
                                                    <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                                    <%--<asp:RequiredFieldValidator ID="rfvRemarks" runat="server" ControlToValidate="txtRemarks" ErrorMessage="Please enter remarks."
                                                        ValidationGroup="SubmitGrp" SetFocusOnError="true" CssClass="text-danger"></asp:RequiredFieldValidator>--%>
                                                </div>
                                                <div class="mb-3">
                                                    <asp:Button CssClass="btn btn-outline-success" ID="btnSaveDraft" runat="server" Text="Save Draft"
                                                        OnClick="btnSaveDraft_Click" />
                                                    <asp:Button CssClass="btn btn-outline-success" ID="btnSubmit" ValidationGroup="SubmitGrp" runat="server"
                                                        OnClick="btnSubmit_Click" Text="Submit" CausesValidation="true" />
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="tab-pane p-3" id="profile" role="tabpanel">
                                        <asp:Label ID="lblGrid" runat="server" Visible="false" CssClass="label"></asp:Label>
                                        <asp:GridView ID="gvCertView" runat="server" AutoGenerateColumns="False" DataKeyNames="CERT_ID"
                                            CssClass="table table-bordered footable" GridLines="Both" CellPadding="4" OnSelectedIndexChanged="gvCertView_SelectedIndexChanged">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Sr. No.
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:CommandField HeaderText="View" ShowSelectButton="True" SelectText="&lt;i class='fa fa-eye'&gt;&lt;/i&gt;"></asp:CommandField>
                                                <asp:BoundField DataField="CDM_NAME" HeaderText="Department Name" SortExpression="CDM_NAME" />
                                                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="tab-pane p-3" id="settings" role="tabpanel">
                                        <div class="mb-3">
                                            <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExport" Text="Export to Excel" runat="server"
                                                OnClick="btnExportToExcel_Click">
                                                <i class="fa fa-download"></i> Export to Excel
                                            </asp:LinkButton>
                                        </div>
                                        <asp:GridView ID="gvAllException" runat="server" AutoGenerateColumns="False" DataKeyNames="CE_ID"
                                            GridLines="Both" CellPadding="4" CssClass="table table-bordered footable" PagerStyle-CssClass="pgr"
                                            AlternatingRowStyle-CssClass="alt">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Sr. No.
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="FHDept" HeaderText="Department" />
                                                <asp:TemplateField HeaderText="Attached File">
                                                    <ItemTemplate>
                                                        <a href="#" onclick="javascript:window.open('../DownloadFileCertification.aspx?FileInformation=<%#(Eval("CE_SERVER_FILE_NAME"))%>','','location=0,status=0,scrollbars=0,width=10,height=10');">
                                                            <%#Eval("CE_CLIENT_FILE_NAME")%>
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CE_EXCEPTION_TYPE" HeaderText="Deviation (Detailed)" />
                                                <asp:BoundField DataField="CE_DETAILS" HeaderText="Regulatory Reference (Detailed)" />
                                                <asp:BoundField DataField="CE_ROOT_CAUSE_OF_DEVIATION" HeaderText="Root Cause for the Deviation" />
                                                <asp:BoundField DataField="CE_ACTION_TAKEN" HeaderText="Action taken" />
                                                <%--<asp:BoundField DataField="CE_CLOSURE_STATUS" HeaderText="Current Status" />--%>
                                                <%--<asp:BoundField DataField="CE_TARGET_DATE" HeaderText="Target Date" DataFormatString="{0:dd-MMM-yyyy}" />--%>
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
                                    <div class="tab-pane p-3" id="settings1" role="tabpanel">
                                        <asp:Literal runat="server" ID="litRegulatoryFilling" />
                                    </div>
                                    <div class="tab-pane p-3" id="settings2" role="tabpanel">
                                        <asp:Literal runat="server" ID="litSummary"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </asp:View>
            <asp:View ID="vwInsert" runat="server">
                <div class="row">
                    <ul class="nav nav-tabs" role="tablist">
                        <li class="nav-item"><a class="nav-link active" data-bs-toggle="tab" href="#Functional" role="tab" aria-selected="true">Certification Details</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-bs-toggle="tab" href="#Compliance" role="tab" aria-selected="false">Compliance Checklist</a>
                        </li>
                    </ul>
                    <div class="tab-content">
                        <div class="tab-pane p-3 active" id="Functional" role="tabpanel">
                            <div class="tabular-view">
                                <div class="row g-0">
                                    <div class="col-md-3">
                                        <label>Certificate Id:</label>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:Label ID="lblCId" runat="server" class="form-control"></asp:Label>
                                    </div>
                                </div>
                                <div class="row g-0">
                                    <div class="col-md-3">
                                        <label>Department:</label>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:Label ID="lblDept" runat="server" class="form-control"></asp:Label>
                                    </div>
                                </div>
                                <div class="row g-0 border-bottom">
                                    <div class="col-md-3">
                                        <label>Approved By:</label>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:Label ID="lblSubmittedBy" runat="server" class="form-control">
                                        </asp:Label>
                                    </div>
                                </div>
                                <div class="row g-0 border-bottom">
                                    <div class="col-md-3">
                                        <label>Approved On:</label>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:Label ID="lblSubmittedDt" runat="server" class="form-control">
                                        </asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="mt-2">
                                <div class="card mb-1 mt-1 border">
                                    <div class="card-header py-0 custom-ch-bg-color">
                                        <h6 class="font-weight-bold text-white mtb-5">Content: </h6>
                                    </div>
                                    <div class="card-body mt-1">
                                        <asp:Label ID="lblContent" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="mt-2" style="display: none">
                                <div class="card mb-1 mt-1 border">
                                    <div class="card-header py-0 custom-ch-bg-color">
                                        <h6 class="font-weight-bold text-white mtb-5">Remarks: </h6>
                                    </div>
                                    <div class="card-body mt-1">
                                        <asp:Label ID="lblRmks" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="tab-pane p-3 table-responsive" id="Compliance" role="tabpanel">
                            <asp:GridView ID="gvCertApproval" runat="server" AutoGenerateColumns="False" DataKeyNames="CERT_ID"
                                GridLines="None" CellPadding="4" CssClass="table table-bordered footable" PagerStyle-CssClass="pgr"
                                AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Sr.No.

                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblId" Text='<%# Bind("CERT_ID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FH Certificate" Visible="false">
                                        <ItemTemplate>
                                            <a href="javascript:void(0);" class="btn btn-sm btn-soft-info btn-circle" onclick="javascript:window.open('./ViewCertificationContent.aspx?Type=L2&DeptId=<%# Eval("CDM_ID") %>&Quarter=<%# Eval("CQM_ID") %>','','location=0,status=0,scrollbars=1,width=900,height=800,resizable=1');"
                                                style="text-align: center;">
                                                <i class="fa fa-eye"></i>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="UH Certificate" Visible="false">
                                        <ItemTemplate>
                                            <a href="javascript:void(0);" class="btn btn-sm btn-soft-info btn-circle" onclick="javascript:window.open('./ViewCertificationContent.aspx?Type=L1&DeptId=<%# Eval("CSDM_ID") %>&Quarter=<%# Eval("CQM_ID") %>','','location=0,status=0,scrollbars=1,width=900,height=800,resizable=1');"
                                                style="text-align: center;">
                                                <i class='fa fa-eye'></i>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View">
                                        <ItemTemplate>
                                            <a href="javascript:void(0);" class="btn btn-sm btn-soft-info btn-circle" onclick="javascript:window.open('./ViewChecklist.aspx?Source=CXOApproval&CertID=<%# Eval("CERT_ID") %>','','location=0,status=0,scrollbars=1,width=900,height=800,resizable=1');"
                                                style="text-align: center;">
                                                <i class='fa fa-eye'></i>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Unit Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDepartmentId" runat="server" Text='<%# Bind("DeptId") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblDepartment" runat="server" Text='<%# Bind("DeptName") %>' Visible="true"></asp:Label>
                                            <asp:HiddenField ID="hfStatus" runat="server" Value='<%#Eval("CERT_STATUS")%>'></asp:HiddenField>
                                            <asp:HiddenField ID="hfCertStatus" runat="server" Value='<%#Eval("Status")%>'></asp:HiddenField>
                                            <asp:HiddenField ID="hfFunctionName" runat="server" Value='<%#Eval("CDM_NAME") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quarter">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuarter" runat="server" Text='<%# Eval("CQM_FROM_DATE","{0:dd-MMM-yyyy}") +" to " + Eval("CQM_TO_DATE","{0:dd-MMM-yyyy}") %>'
                                                Visible="true"></asp:Label>
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
                                            <asp:Label ID="lblSubmittedRemarks" runat="server" Text='<%# Eval("Approval_Comments").ToString().Replace(Environment.NewLine, "<br />") %>'
                                                Visible="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Status" DataField="Status" SortExpression="Status" />
                                    <asp:TemplateField HeaderText="Audit Trail">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAuditTrail" runat="server" Width="300px" Text='<%# Eval("CERT_AUDIT_TRAIL").ToString().Replace(Environment.NewLine, "<br />") %>'
                                                Visible="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <HeaderTemplate>
                                            Remarks
                                   
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <F2FControls:F2FTextBox ID="txtRemarks" TextMode="MultiLine" Rows="5" Columns="50" Enabled="False"
                                                CssClass="form-control" runat="server"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                            <%--<cc1:FilteredTextBoxExtender ID="txtCharacters_FilteredTextBoxExtender" runat="server" Enabled="True"    
	                                TargetControlID="txtRemarks" FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers"    
	                                FilterMode="ValidChars" ValidChars="@;:_-.'/\?!, " >    
                                </cc1:FilteredTextBoxExtender>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="mb-3">
                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click">
                            <i class="fa fa-arrow-left me-2"></i> Back
                        </asp:LinkButton>
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
    <br />
    <br />
</asp:Content>
