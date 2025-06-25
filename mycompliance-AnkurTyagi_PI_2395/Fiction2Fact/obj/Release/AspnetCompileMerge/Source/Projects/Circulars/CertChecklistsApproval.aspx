<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" Async="true"
    Inherits="Fiction2Fact.Projects.Circulars.CertChecklistsApproval" Title="Certification Checklists Approval" CodeBehind="CertChecklistsApproval.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js")%>"></script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/dateformatvalidation.js")%>"></script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/base64.js") %>"></script>

    <script type="text/javascript">
        function validate(operation) {
            var focuson = null;
            var IsDoubleClickFlagSet = document.getElementById('ctl00_ContentPlaceHolder1_hfDoubleClickFlag').value;
            if (IsDoubleClickFlagSet == 'Yes') {
                alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                return false;
            }
            else {
                var msg = '';
                var isAnyChkboxChecked = false;
                if (CheckBoxIDs != null) {
                    for (var i = 1; i < CheckBoxIDs.length; i++) {
                        var cb = document.getElementById(CheckBoxIDs[i]);

                        if (cb.checked) {
                            isAnyChkboxChecked = true;

                            if (operation == 'R') {
                                var comment = document.getElementById(CommentsIDs[i]);
                                if (comment.value == '') {
                                    msg = msg + '- Please enter the comments for row no. ' + i + '.\n';
                                }
                            }
                            else if (operation == 'A') {
                                var status = document.getElementById(StatusIDs[i]);
                                if (status.value == '2') {
                                    msg = msg + '- Certification Checklist is not yet accepted by the function SPOC for row no. ' + i + '.\n';
                                }
                            }
                        }
                    }

                    if (!isAnyChkboxChecked) {
                        if (operation == 'A')
                            msg = msg + '- Please select one or more records to move the certification checklist';
                        else if (operation == 'R')
                            msg = msg + '- Please select one or more records to send back the certification checklist';
                    }
                }
                if (msg != '') {
                    alert("" + msg);
                    return false;
                }
                else {
                    if (operation == 'A') {
                        msg = 'Are you sure that you want to move the certification checklist?';
                    }
                    else if (operation == 'R') {
                        msg = 'Are you sure that you want to send back the certification checklist?';
                    }

                    if (!confirm(msg)) return false;
                    {
                        document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                        return true;
                    }
                }
            }
        }

        function showCircularGist(CMId, CCCId) {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "View";
            window.open('ViewCircularDetails.aspx?CircularId=' + CMId + '&CircCertCheclistId=' + CCCId, '', 'location=0,status=0,resizable=1,scrollbars=1,width=1000,height=800');
            return false;
        }

        function onEditClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
        }

        //<< Checkbox
        function ChangeAllCheckBoxStates(checkState) {
            if (CheckBoxIDs != null) {
                for (var i = 0; i < CheckBoxIDs.length; i++)
                    ChangeCheckBoxState(CheckBoxIDs[i], checkState);
            }

            if (CommentsIDs != null) {
                for (var i = 0; i < CommentsIDs.length; i++)
                    ChangeCommentsState(CommentsIDs[i], checkState);
            }

            for (var i = 1; i <= CheckBoxIDs.length; i++) {
                onRowCheckedUnchecked(CheckBoxIDs[i], CommentsIDs[i]);
            }
        }

        function ChangeCheckBoxState(id, checkState) {
            var cb = document.getElementById(id);
            if (cb != null)
                cb.checked = checkState;
        }

        function ChangeCommentsState(id, checkState) {
            var comment = document.getElementById(id);
            if (comment != null) {
                if (checkState) {
                    comment.disabled = false;
                }
                else {
                    comment.disabled = true;
                    comment.value = "";
                }
            }
        }

        function onRowCheckedUnchecked(cbid, commentId) {
            var cb = document.getElementById(cbid);

            var checkstate;
            if (cb != null) {
                checkState = cb.checked;
                ChangeCommentsState(commentId, checkState);
            }

            ChangeHeaderAsNeeded();
            return;
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
        //>>

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

    <asp:Literal ID="CheckBoxIDsArray" runat="server"></asp:Literal>
    <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />
    <asp:HiddenField ID="hfSelectedOperation" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Certification Checklists Approval</h4>
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
                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnRefresh" Text="Refresh" Style="visibility: hidden; display: none;" runat="server" OnClick="btnRefresh_Click">
                            <i data-feather="refresh-cw"></i> Refresh
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-success" ID="btnAccept" Text="Move to Certification Checklist" runat="server" OnClick="btnAccept_Click" OnClientClick="return validate('A');">
                            Move to Certification Checklist
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnReject" Text="Send to Stakeholder" runat="server" OnClick="btnReject_Click" OnClientClick="return validate('R');">
                             Send to Stakeholder
                        </asp:LinkButton>
                    </div>
                    <div>
                        <div class="table-responsive">
                            <asp:GridView ID="gvCircularMaster" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                GridLines="Both" DataKeyNames="CCC_ID" OnRowDataBound="gvCircularMaster_RowDataBound"
                                AllowPaging="false" AllowSorting="false" CssClass="table table-bordered footable" AlternatingRowStyle-CssClass="alt"
                                OnSelectedIndexChanged="gvCircularMaster_SelectedIndexChanged" EmptyDataText="No records found...">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="HeaderLevelCheckBox" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="RowLevelCheckBox" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="true" HeaderText="Edit">
                                        <ItemTemplate>
                                            <center>
                                                <asp:LinkButton ID="lbEdit" runat="server" CausesValidation="False" OnClientClick="onEditClick()" CssClass="btn btn-sm btn-soft-success btn-circle"
                                                    CommandName="Select">
                                                    <i class="fa fa-pen"></i>
                                                </asp:LinkButton>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sr.No.">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Circular No." SortExpression="CM_CIRCULAR_NO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCircNo" runat="server" Text='<%# Eval("CM_CIRCULAR_NO") %>'></asp:Label>
                                            <asp:HiddenField ID="hfCircCertChecklistId" runat="server" Value='<%# Eval("CCC_ID") %>' />
                                            <asp:HiddenField ID="hfcmId" runat="server" Value='<%# Eval("CM_ID") %>' />
                                            <asp:HiddenField ID="hfSPOCEmailId" runat="server" Value='<%# Eval("CSSDM_EMAIL_ID") %>' />
                                            <asp:HiddenField ID="hfUHEmailId" runat="server" Value='<%# Eval("CSDM_EMAIL_ID") %>' />
                                            <asp:HiddenField ID="hfFHEmailId" runat="server" Value='<%# Eval("CDM_CXO_EMAILID") %>' />
                                            <asp:HiddenField ID="hfCirType" runat="server" Value='<%# Eval("CDTM_TYPE_OF_DOC") %>' />
                                            <asp:HiddenField ID="hfCirIssAuthority" runat="server" Value='<%# Eval("CIA_NAME") %>' />
                                            <asp:HiddenField ID="hfCirSubject" runat="server" Value='<%# Eval("CM_TOPIC") %>' />
                                            <asp:HiddenField ID="hfDeptName" runat="server" Value='<%# Eval("DeptName") %>' />
                                            <asp:HiddenField ID="hfActRegCirc" runat="server" Value='<%# Eval("CDTM_TYPE_OF_DOC") %>' />
                                            <asp:HiddenField ID="hfFrequency" runat="server" Value='<%# Eval("CCC_FREQUENCY") %>' />
                                            <asp:HiddenField ID="hfForms" runat="server" Value='<%# Eval("CCC_FORMS") %>' />
                                            <asp:HiddenField ID="hfEffectiveFrom" runat="server" Value='<%# Convert.ToDateTime(Eval("CCC_EFFECTIVE_FROM")).ToString("dd-MMM-yyyy") %>' />
                                            <asp:HiddenField ID="hfStatus" runat="server" Value='<%# Eval("CCC_STATUS") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Circular Date" SortExpression="CM_DATE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCircularDate" runat="server" Text='<%# Eval("CM_DATE", "{0:dd-MMM-yyyy }") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="true" HeaderText="Subject of the Circular">
                                        <ItemTemplate>
                                            <center>
                                               <%-- <asp:LinkButton ID="lbView" runat="server" OnClientClick='<%# string.Format("return showCircularGist(\"{0}\",\"{1}\");", Eval("CM_ID"), Eval("CCC_ID")) %>'
                                                    CssClass="badge rounded-pill badge-soft-pink" Text='<%# Eval("CM_TOPIC") %>'>
                                                </asp:LinkButton>--%>
                                                <asp:LinkButton ID="lnkView" runat="server" CausesValidation="False" CssClass="badge rounded-pill badge-soft-pink" 
                                                        CommandName="Select" Text='<%# Eval("CM_TOPIC") %>'>>
                                                       
                                                    </asp:LinkButton>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DeptName" HeaderText="Department Name" />
                                    <asp:BoundField DataField="CDTM_TYPE_OF_DOC" HeaderText="Act/Regulation/Circular" />
                                    <asp:TemplateField HeaderText="Reference Circular / Notification / Act" ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReference" runat="server" Text='<%#Eval("CCC_REFERENCE").ToString().Replace("\n","<br />") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Section/Clause" ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClause" runat="server" Text='<%#Eval("CCC_CLAUSE").ToString().Replace("\n","<br />") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Compliance of/Heading of Compliance checklist" ShowHeader="true">--%>
                                    <asp:TemplateField HeaderText="Particulars" ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCheckpoints" runat="server" Text='<%#Eval("CCC_CHECK_POINTS").ToString().Replace("\n","<br />") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblParticulars" runat="server" Text='<%#Eval("CCC_PARTICULARS").ToString().Replace("\n","<br />") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Consequences of non Compliance" ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPenalty" runat="server" Text='<%#Eval("CCC_PENALTY").ToString().Replace("\n","<br />") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CCC_FREQUENCY" HeaderText="Frequency" />
                                    <asp:BoundField DataField="CCC_FORMS" HeaderText="Forms" />
                                    <asp:BoundField DataField="CCC_EFFECTIVE_FROM" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Effective From" />
                                    <asp:BoundField DataField="CCC_ACCEPTED_BY_FU" HeaderText="Accepted by (Function SPOC)" />
                                    <asp:BoundField DataField="CCC_ACCEPTED_ON_FU" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}" HeaderText="Accepted On (Function SPOC)" />
                                    <asp:TemplateField HeaderText="Acceptance Remarks (Function SPOC)" ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%#Eval("CCC_ACCEPTANCE_REMARKS_FU").ToString().Replace("\n","<br />") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CCC_REJECTED_BY_FU" HeaderText="Rejected by (Function SPOC)" />
                                    <asp:BoundField DataField="CCC_REJECTED_ON_FU" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}" HeaderText="Rejected On (Function SPOC)" />
                                    <asp:TemplateField HeaderText="Rejection Remarks (Function SPOC)" ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%#Eval("CCC_REJECTION_REMARKS_FU").ToString().Replace("\n","<br />") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comments" ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:TextBox TextMode="MultiLine" ID="txtComments" runat="server" CssClass="form-control"
                                                Enabled="false" Columns="30" Rows="5">
                    </asp:TextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtComments" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SM_DESC" HeaderText="Status" />
                                    <asp:TemplateField HeaderText="Audit Trail" ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfAuditTrail" runat="server" Value='<%# Convert.ToBase64String(Encoding.UTF8.GetBytes(Eval("CCC_AUDIT_TRAIL").ToString().Replace("\r\n", "<br>"))) %>'></asp:HiddenField>
                                            <asp:LinkButton ID="lbAuditTrail" CssClass="badge rounded-pill badge-soft-pink" runat="server" Text="View Audit Trail" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="mt-3">
                        <asp:LinkButton CssClass="btn btn-outline-success" ID="btnAccept1" Text="Move to Certification Checklist" runat="server" OnClick="btnAccept_Click" OnClientClick="return validate('A');">
                            Move to Certification Checklist
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnReject1" Text="Send to Stakeholder" runat="server" OnClick="btnReject_Click" OnClientClick="return validate('R');">
                            Send to Stakeholder
                        </asp:LinkButton>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <!-- end row -->

    <div style="width: 100%;">
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
</asp:Content>
