<%@ Page Title="" Language="C#" MasterPageFile="~/Projects/Temp3.master" Async="true" AutoEventWireup="true" CodeBehind="ApprovalComplianceReview.aspx.cs" Inherits="Fiction2Fact.Projects.Compliance.ApprovalComplianceReview" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <script>
        function onClientViewClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "View";
        }

        function onViewIssueTrackerClick(RDIId) {
            window.open('ViewIssuesTracker.aspx?Id=' + RDIId, '', 'location=0,status=0,scrollbars=1,resizable=1,width=900,height=900');
            return false;
        }
        function onEditIssueTrackerClick(RDIId, CRID) {
            window.open('EditIssueDetails.aspx?IssueId=' + RDIId + '&CRId=' + CRID + '', '', 'location=0,status=0,scrollbars=1,resizable=1,width=900,height=900');
            return false;
        }
    </script>

    <script type="text/javascript">

        function approve_issues(validationgroup, msg) {
            if (validationgroup === '') {
                return confirm(msg.toString());
            }
            else {
                if (Page_ClientValidate(validationgroup)) {
                    return confirm(msg.toString());
                }
            }
        }

        function ChangeAllRRCheckBoxStates(checkState) {
            if (RRCheckBoxIDs != null) {
                for (var i = 0; i < RRCheckBoxIDs.length; i++)
                    ChangeCheckBoxState(RRCheckBoxIDs[i], checkState);
            }
        }
        function ChangeAllCheckBoxStates(checkState) {
            if (CheckBoxIDs != null) {
                for (var i = 0; i < CheckBoxIDs.length; i++)
                    ChangeCheckBoxState(CheckBoxIDs[i], checkState);
            }
            if (commentsIDs != null) {
                for (var i = 1; i < commentsIDs.length; i++)
                    ChangeCommentsState(commentsIDs[i], checkState, StatusIDs[i], rfv_commentsIDs[i]);
            }
        }

        function ChangeCheckBoxState(id, checkState) {
            var cb = document.getElementById(id);
            if (cb != null)
                cb.checked = checkState;
        }

        function ChangeCommentsState(id, checkState, hfStatusId, rfv_id) {
            var txtrejection = document.getElementById(id);
            var hfStatus = document.getElementById(hfStatusId).value;
            var UserType = document.getElementById('ctl00_ContentPlaceHolder1_hfType').value;
            var rfv = document.getElementById(rfv_id);

            if (txtrejection != null) {
                if (checkState) {
                    if (UserType == 'UH') {
                        txtrejection.disabled = false;
                        rfv.enabled = true;
                    }
                    else if (UserType == 'L1') {
                        txtrejection.disabled = false;
                        rfv.enabled = true;
                    }
                    else if (UserType == 'L2') {
                        txtrejection.disabled = false;
                        rfv.enabled = true;
                    }
                    else if (UserType == 'L0') {
                        txtrejection.disabled = false;
                        rfv.enabled = true;
                    }
                    else {
                        txtrejection.disabled = true;
                        rfv.enabled = false;
                    }
                }
                else {
                    txtrejection.disabled = true;
                    txtrejection.value = "";
                    rfv.enabled = false;
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


        function onRowCheckedUnchecked(cbid, txtrejectionid, hfStatusId, rfv_id) {
            var cb = document.getElementById(cbid);
            var txtrejection = document.getElementById(txtrejectionid);
            var hfStatus = document.getElementById(hfStatusId).value;
            var UserType = document.getElementById('ctl00_ContentPlaceHolder1_hfType').value;
            var rfv = document.getElementById(rfv_id);

            var checkState;
            if (cb != null) {
                checkState = cb.checked;
                if (cb.checked) {
                    if (UserType == 'UH') {
                        txtrejection.disabled = false;
                        rfv.enabled = true;
                    }
                    else if (UserType == 'L1') {
                        txtrejection.disabled = false;
                        rfv.enabled = true;
                    }
                    else if (UserType == 'L2') {
                        txtrejection.disabled = false;
                        rfv.enabled = true;
                    }
                    else if (UserType == 'L0') {
                        txtrejection.disabled = false;
                        rfv.enabled = true;
                    }
                    else {
                        txtrejection.disabled = true;
                        rfv.enabled = false;
                    }
                }
                else {
                    txtrejection.disabled = true;
                    txtrejection.value = "";
                    rfv.enabled = false;
                }
            }

            ChangeHeaderAsNeeded();
            return;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Literal ID="CheckBoxIDsArray" runat="server"></asp:Literal>
    <asp:Literal ID="CheckBoxRRIDsArray" runat="server"></asp:Literal>
    <asp:HiddenField ID="hfType" runat="server" />
    <asp:HiddenField ID="hfSelectedOperation" runat="server" />
    <asp:HiddenField ID="hfComplianceId" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">
                            <asp:Label ID="lblHeader" runat="server"></asp:Label></h4>
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
                    <asp:Panel ID="pnlComplianceReview" runat="server">
                        <div class="table-responsive">
                            <asp:GridView ID="gvComplianceReview" AllowPaging="False" AllowSorting="true" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found." EmptyDataRowStyle-BorderColor="transparent"
                                OnSelectedIndexChanged="gvComplianceReview_SelectedIndexChanged" OnRowDataBound="gvComplianceReview_RowDataBound"
                                CssClass="table table-bordered footable" DataKeyNames="CCR_ID">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="RRHeaderLevelCheckBox" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="RRRowLevelCheckBox" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
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

                                    <asp:TemplateField HeaderText="View">
                                        <ItemTemplate>
                                            <center>
                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="Select" 
                                                    CssClass="btn btn-sm btn-soft-info btn-circle">
                                                    <i class="fa fa-eye"></i>	                            
                                                </asp:LinkButton>
                                                <asp:Label ID="lblRRId" runat="server" Text='<%#Eval("CCR_ID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblRRStatus" runat="server" Text='<%#Eval("CCR_STATUS") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblRRInitiator" runat="server" Text='<%#Eval("CCR_CREATOR") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblRRMId" Visible="false" runat="server" Text='<%#Eval("CCR_CRM_ID")%>'></asp:Label>
                                                <asp:Label ID="lblUniverseToBeReviewed" Visible="false" runat="server" Text='<%#Eval("CCR_CRUM_ID")%>'></asp:Label>
                                                <%--<asp:Label ID="lblCntOfActiveIssues" Visible="false" runat="server" Text='<%#Eval("ActiveIssueCnt")%>'></asp:Label>--%>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Risk Review Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCRID" Visible="false" runat="server" Text='<%#Eval("CCR_ID")%>'></asp:Label>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("CCR_STATUS")%>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Compliance Review No.">
                                        <ItemTemplate>
                                            <center>
                                                <asp:LinkButton ID="lnkViewRR" runat="server" CommandName="Select" Text='<%#Eval("CCR_IDENTIFIER") %>'
                                                    CssClass="badge rounded-pill badge-soft-pink" OnClientClick="return onClientViewClick();">
                                                </asp:LinkButton>
                                                <asp:Label ID="lblIdentifier" Visible="false" runat="server" Text='<%#Eval("CCR_IDENTIFIER") %>'></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CRUM_UNIVERSE_TO_BE_REVIEWED" HeaderText="Universe to be Reviewed" />
                                    <asp:BoundField DataField="CRM_L0_REVIEWER_NAME" HeaderText="Reviewer Name" />
                                    <asp:BoundField DataField="CCR_REVIEW_TYPE" HeaderText="Review Type" />
                                    <asp:BoundField DataField="CCR_TENTATIVE_START_DATE" HeaderText="Tentative Start Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                    <asp:BoundField DataField="CCR_TENTATIVE_END_DATE" HeaderText="Tentative End Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                    <asp:BoundField DataField="SM_DESC" HeaderText="Status" />
                                    <asp:TemplateField HeaderText="Review Scope">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReviewScope" Width="200px" runat="server" ToolTip='<%# Eval("CCR_REVIEW_SCOPE").ToString() %>'
                                                Text='<%#Eval("CCR_REVIEW_SCOPE").ToString().Length>200?(Eval("CCR_REVIEW_SCOPE") as string).Substring(0,200).Replace(Environment.NewLine, "<br />") + " ...":Eval("CCR_REVIEW_SCOPE").ToString().Replace(Environment.NewLine, "<br />")  %>'></asp:Label>
                                            <asp:Label ID="lblReviewScope1" Visible="false" runat="server" Text='<%#Eval("CCR_REVIEW_SCOPE").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" Width="200px" runat="server" ToolTip='<%# Eval("CCR_REMARKS").ToString() %>'
                                                Text='<%#Eval("CCR_REMARKS").ToString().Length>200?(Eval("CCR_REMARKS") as string).Substring(0,200).Replace(Environment.NewLine, "<br />") + " ...":Eval("CCR_REMARKS").ToString().Replace(Environment.NewLine, "<br />")  %>'></asp:Label>
                                            <asp:Label ID="lblRemarks1" Visible="false" runat="server" Text='<%#Eval("CCR_REMARKS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlDraftedIssue" runat="server" Visible="false">
                        <div class="mb-3">
                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnApprove" runat="server" Text="Review Complete" Visible="false"
                                OnClick="btnApprove_Click" >
                                <i class="fa fa-paper-plane me-2"></i> Review Complete
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnReject" runat="server" Text="Convey Changes" Visible="false"
                                OnClick="btnReject_Click" > 
                                <i class="fa fa-ban me-2"></i> Convey Changes
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" >
                                <i class="fa fa-arrow-left me-2"></i> Back                   
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnRefresh" runat="server" Text="Refresh" Style="display: none;" OnClick="btnRefresh_Click">
                                <i data-feather="refresh-cw"></i> Refresh
                            </asp:LinkButton>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="gvDraftedIssue" AllowPaging="False" AllowSorting="true" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found." EmptyDataRowStyle-BorderColor="transparent"
                                CssClass="table table-bordered footable" DataKeyNames="CI_ID" OnRowDataBound="gvDraftedIssue_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="HeaderLevelCheckBox" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="RowLevelCheckBox" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
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

                                    <asp:TemplateField HeaderText="Edit" Visible="false">
                                        <ItemTemplate>
                                            <center>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" 
                                                    CssClass="btn btn-sm btn-soft-success btn-circle" OnClientClick='<%# string.Format("return onEditIssueTrackerClick(\"{0}\",\"{1}\");", Eval("CI_ID"),Eval("CI_CCR_ID")) %>'>
                                                    <i class="fa fa-pen"></i>	
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
                                            <asp:DataList ID="dlFiles" runat="server" RepeatColumns="1" RepeatDirection="vertical"
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
                                    <asp:TemplateField HeaderText="Comments" ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:TextBox TextMode="MultiLine" ID="txtComments" runat="server" CssClass="form-control"
                                                Enabled="false" Columns="30" Rows="5">
                                                    </asp:TextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtComments" />

                                            <asp:RequiredFieldValidator ID="rfv_comments" runat="server" ControlToValidate="txtComments" ForeColor="Red" ErrorMessage="Please enter comments" ValidationGroup="VComments"></asp:RequiredFieldValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="mt-3">
                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnApprove1" runat="server" Text="Review Complete" Visible="false"
                                OnClick="btnApprove_Click" >
                                <i class="fa fa-paper-plane me-2"></i> Review Complete
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnReject1" runat="server" Text="Convey Changes" Visible="false"
                                OnClick="btnReject_Click" >
                                <i class="fa fa-ban me-2"></i> Convey Changes
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack1" runat="server" Text="Back" OnClick="btnBack_Click" >
                                <i class="fa fa-arrow-left me-2"></i> Back  
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->



</asp:Content>
