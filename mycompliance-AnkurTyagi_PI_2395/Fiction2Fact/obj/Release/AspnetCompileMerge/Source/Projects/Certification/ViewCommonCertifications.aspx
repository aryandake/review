<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" EnableEventValidation="false"
    Async="true" Inherits="Fiction2Fact.Projects.Certification.Certification_TestViewCommonCertifications" Title="View Certifications "
    CodeBehind="ViewCommonCertifications.aspx.cs" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function printElement(el) {
            if (typeof el === 'string') {
                el = document.getElementById(el);
                if (!el) {
                    alert('Element doesnt exists');
                    return false;
                }
            }
            if (!el) {
                alert('Element doesnt exists');
                return false;
            }

            var content = document.getElementById('printarea').innerHTML;
            var shtm = "<html><head><title>Print</title><link id=\"Link1\" rel=\"stylesheet\" type=\"text/css\" href=\"../css/main.css\" /></head><body>" + content + "</body></html>";
            var w = window.open("", "", "width=750,height=650");
            w.document.write(shtm);
            w.document.close();
            w.focus();
            w.print();
            w.close();
        }

        function printData() {
            printElement('printarea');
            return false;
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

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/tabcontrol.js")%>'>
    </script>

    <asp:HiddenField runat="server" ID="hfType" />
    <asp:HiddenField runat="server" ID="hfTabberId" />
    <asp:HiddenField ID="hfDesignation" runat="server" />
    <asp:HiddenField ID="hfContent" runat="server" />
    <asp:HiddenField ID="hfQTRId" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">
                            <asp:Label runat="server" ID="lblHeader"></asp:Label></h4>
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

    <asp:MultiView ID="mvMultiView" runat="server">
        <asp:View ID="vwGrid" runat="server">

            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Quarter:</label>
                                    <asp:DropDownList CssClass="form-select" ID="ddlQuarter" runat="server" DataValueField="CQM_ID"
                                        DataTextField="Quarter">
                                    </asp:DropDownList>
                                </div>
                                <div class="mt-3 text-center">
                                    <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnSearch" Text="Search" runat="server" ValidationGroup="SEARCH"
                                        AccessKey="s" OnClick="btnSearch_Click">
                                    <i class="fa fa-search"></i> Search                          
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <%--<table border="0" cellpadding="3" cellspacing="1" bgcolor="#dddddd" width="35%">
                    <tr>
                        <td class="tabhead3">&nbsp; Quarter:
                        </td>
                        <td class="tabbody3">
                            <asp:DropDownList CssClass="form-select" ID="ddlQuarter" runat="server" DataValueField="CQM_ID"
                                DataTextField="Quarter">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvDepartment1" runat="server" ControlToValidate="ddlQuarter" CssClass="span"
                                Display="Dynamic" ValidationGroup="SEARCH" SetFocusOnError="True">Please select Quarter.</asp:RequiredFieldValidator>&nbsp;
                        </td>
                        <td class="tabbody3">
                            <asp:Button CssClass="html_button" ID="btnSearch" Text="Search" runat="server" ValidationGroup="SEARCH"
                                AccessKey="s" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>--%>
                            <asp:Label ID="lblInfo" runat="server" CssClass="label"></asp:Label>
                            <br />
                            <div id="printarea" class="table-responsive">
                                <asp:GridView ID="gvCommonCert" runat="server" AutoGenerateColumns="False" DataKeyNames="CC_ID"
                                    AllowSorting="true" AllowPaging="true" GridLines="Both" CellPadding="4"
                                    OnSelectedIndexChanged="gvCommonCert_SelectedIndexChanged" CssClass="table table-bordered footable"
                                    EmptyDataText="No Record found." PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:TemplateField HeaderText="View">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="ibView" CssClass="btn btn-sm btn-soft-info btn-circle"
                                                    CommandName="Select" ToolTip="View Details"><i class="fa fa-eye"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Sr.No.
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
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
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="Submitted by Compliance on">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCCOSubmissionDate" runat="server" Text='<%# Bind("CC_CCO_SUB_DT","{0:dd-MMM-yyyy}") %>'
                                                    Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Submitted by Chief Financial Officer(CFO) on">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCFOSubmissionDate" runat="server" Text='<%# Bind("CC_CFO_SUB_DT","{0:dd-MMM-yyyy}") %>'
                                                    Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Submitted by Chief Executive Officer(CEO) on">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCEOSubmissionDate" runat="server" Text='<%# Bind("CC_CEO_SUB_DT","{0:dd-MMM-yyyy}") %>'
                                                    Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View ID="vwForm" runat="server">
            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-body">
                            <div class="row">
                                <ul class="nav nav-tabs" role="tablist">
                                    <li class="nav-item"><a class="nav-link active" data-bs-toggle="tab" href="#Functional" role="tab" aria-selected="true">Functional Certification</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" data-bs-toggle="tab" href="#Compliance" role="tab" aria-selected="false">Compliance Checklist</a>
                                    </li>
                                </ul>
                                <div class="tab-content">
                                    <div class="tab-pane p-3 active" id="Functional" role="tabpanel">
                                        <asp:FormView ID="fvEditCert" runat="server" DataKeyNames="CERT_ID" Width="100%">
                                            <ItemTemplate>
                                                <div class="row">
                                                    <div class="col-md-3 mb-2" id="divcrtid" runat="server" visible="false">
                                                        <label class="form-label">Certificate Id:</label>
                                                        <label class="form-control"><%# Eval("CERT_ID")%></label>
                                                    </div>
                                                    <div class="col-md-3 mb-2">
                                                        <label class="form-label">Department:</label>
                                                        <label class="form-control"><%# Eval("CDM_NAME")%></label>
                                                    </div>
                                                    <div class="col-md-3 mb-2">
                                                        <label class="form-label">From Date:</label>
                                                        <label class="form-control"><%# Eval("CQM_FROM_DATE", "{0:dd-MMM-yyyy}")%></label>
                                                    </div>
                                                    <div class="col-md-3 mb-2">
                                                        <label class="form-label">To Date:</label>
                                                        <label class="form-control"><%# Eval("CQM_TO_DATE", "{0:dd-MMM-yyyy}")%></label>
                                                    </div>
                                                    <div class="col-md-3 mb-2">
                                                        <label class="form-label">Status:</label>
                                                        <label class="form-control"><%# Eval("Status")%></label>
                                                    </div>
                                                    <div class="col-md-12 mb-2">
                                                        <label class="form-label">Content:</label>
                                                        <label class="form-control"><%# Eval("CERT_CONTENT")%></label>
                                                    </div>
                                                    <div class="col-md-6 mb-2" style="display: none">
                                                        <label class="form-label">Remarks:</label>
                                                        <label class="form-control"><%# Eval("CERT_REMARKS").ToString()==""?"&nbsp;":Eval("CERT_REMARKS").ToString()%></label>
                                                    </div>
                                                    <div class="col-md-3 mb-2">
                                                        <label class="form-label">Submitted By:</label>
                                                        <label class="form-control"><%# Eval("CERT_APPROVED_BY_LEVEL3")%></label>
                                                        <%--CERT_SUBMITTED_BY--%>
                                                    </div>
                                                    <div class="col-md-3 mb-2">
                                                        <label class="form-label">Submitted on:</label>
                                                        <label class="form-control"><%# Eval("CERT_APPROVED_DT_LEVEL3", "{0:dd-MMM-yyyy HH:mm:ss tt}")%></label>
                                                        <%--CERT_SUBMITTED_ON--%>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:FormView>
                                    </div>

                                    <div class="tab-pane p-3" id="Compliance" role="tabpanel">
                                        <asp:GridView ID="gvCertApproval" runat="server" AutoGenerateColumns="False" DataKeyNames="CERT_ID"
                                            GridLines="None" CellPadding="4" CssClass="table table-bordered footable" PagerStyle-CssClass="pgr"
                                            AlternatingRowStyle-CssClass="alt">
                                            <Columns>
                                                <asp:TemplateField HeaderText="View">
                                                    <ItemTemplate>
                                                        <a href="javascript:void(0);" onclick="javascript:window.open('./ViewChecklist.aspx?Source=CXOApproval&CertID=<%# Eval("CERT_ID") %>','','location=0,status=0,scrollbars=1,width=900,height=800,resizable=1');"
                                                            style="text-align: center;" class="btn btn-sm btn-soft-info btn-circle">
                                                            <i class="fa fa-eye"></i>
                                                        </a>
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
                                                        <asp:Label ID="lblAuditTrail" runat="server" Text='<%# Eval("CERT_AUDIT_TRAIL").ToString().Replace(Environment.NewLine, "<br />") %>'
                                                            Visible="true"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
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
                                    <div class="mb-3">
                                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnViewCancel" runat="server" Text="Back"
                                            CausesValidation="false" OnClick="btnViewCancel_Click"><i class="fa fa-arrow-left me-2"></i> Back</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View ID="vwPanel" runat="server">
            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-body">
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
                            <div class="tab-content">
                                <div class="tab-pane p-3 active" id="home" role="tabpanel">

                                    <div class="mb-3">
                                        <%--<asp:ImageButton runat="server" ID="btnExport" ImageUrl="../../Content/images/legacy/print-icon.gif"
                                        OnClick="btnExport_Click" />--%>
                                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExport" runat="server" Text="Export to Doc"
                                            OnClick="btnExport_Click"><i class="fa fa-download"></i> Export to Doc</asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnConvertToPdf" runat="server" Text="Export to Pdf"
                                            OnClick="btnConvertToPdf_Click"><i class="fa fa-download"></i> Export to Pdf</asp:LinkButton>
                                    </div>
                                    <div class="row" id="tabCertDets" runat="server">
                                        <div class="col-md-4 mb-3" runat="server" id="trCertID" visible="false">
                                            <label class="form-label">Certificate Id:</label>
                                            <asp:Label ID="lblCCId" runat="server" CssClass="form-control"></asp:Label>
                                        </div>

                                        <div class="col-md-4 mb-3" runat="server" id="trfrmDate">
                                            <label class="form-label">From Date:</label>
                                            <asp:Label ID="lblFromDate" runat="server" CssClass="form-control"></asp:Label>
                                        </div>

                                        <div class="col-md-4 mb-3" runat="server" id="trtoDate">
                                            <label class="form-label">To Date:</label>
                                            <asp:Label ID="lblToDate" runat="server" CssClass="form-control"></asp:Label>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Certificate:</label>
                                            <asp:Label ID="lblContent" runat="server" CssClass="form-control"></asp:Label>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Remarks by Compliance:</label>
                                            <asp:Label ID="lblCHRemarks" runat="server" CssClass="form-control"></asp:Label>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Submitted by Compliance on:</label>
                                            <asp:Label ID="lblCHSubmittedOn" runat="server" CssClass="form-control"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane p-3" id="profile" role="tabpanel">
                                    <asp:GridView ID="gvCertEdit" runat="server" AutoGenerateColumns="False" DataKeyNames="CERT_ID"
                                        GridLines="Both" CellPadding="4" OnSelectedIndexChanged="gvCertEdit_SelectedIndexChanged"
                                        CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:TemplateField HeaderText="View">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="ibView" CssClass="btn btn-sm btn-soft-info btn-circle"
                                                        CommandName="Select" ToolTip="View Details"><i class="fa fa-eye"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Sr.No.
                               
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Department Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%# Bind("CDM_NAME") %>' Visible="true"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' Visible="true"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="tab-pane p-3" id="settings" role="tabpanel">
                                    <asp:GridView ID="gvAllException" runat="server" AutoGenerateColumns="False" DataKeyNames="CE_ID"
                                        GridLines="Both" CellPadding="4" CssClass="table table-bordered footable" PagerStyle-CssClass="pgr"
                                        AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <%--//<<Modified by Ankur Tyagi on 02Feb2024 for CR_1945--%>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Sr.No.
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-->>--%>
                                            <asp:BoundField DataField="CDM_NAME" HeaderText="Department" />
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
                                            <asp:TemplateField HeaderText="Current Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtCE_CLOSURE_STATUS" Text='<%#Eval("CE_CLOSURE_STATUS").ToString().Replace("\n", "<br />") %>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CE_TARGET_DATE" HeaderText="Target Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="tab-pane p-3" id="settings1" role="tabpanel">
                                    <asp:Literal runat="server" ID="litRegulatoryFilling" />
                                </div>
                            </div>
                            <br />
                            <div class="mt-3">
                                <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnCancel" runat="server" Text="Back" CausesValidation="false"
                                    OnClick="btnCancel_Click"><i class="fa fa-arrow-left me-2"></i> Back</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <br />
</asp:Content>
