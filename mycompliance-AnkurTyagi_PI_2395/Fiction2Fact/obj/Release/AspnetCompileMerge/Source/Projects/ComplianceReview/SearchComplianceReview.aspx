<%@ Page Title="" Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" CodeBehind="SearchComplianceReview.aspx.cs" Inherits="Fiction2Fact.Projects.Compliance.ViewComplianceReview" %>


<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation" TagPrefix="f2f" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <link type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/css/legacy/Popup/basic.css") %>" rel="stylesheet" media="screen" />
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js") %>"></script>

    <script>
        function onClientEditClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
        }

        function onClientViewClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "View";
        }

        function onClientIssueClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Issue";
        }

        function compareStartEndDates(source, arguments) {
            try {
                var TentativeStartDt = document.getElementById('ctl00_ContentPlaceHolder1_txtTentativeStartDT');
                var TentativeEndDate = document.getElementById('ctl00_ContentPlaceHolder1_txtTentativeEndDT');

                if (compare2Dates(TentativeStartDt, TentativeEndDate) == 0) {
                    arguments.IsValid = false;
                }
                else {
                    arguments.IsValid = true;
                }
            }
            catch (e) {
                alert(e);
                arguments.IsValid = false;
            }
        }
    </script>

    <script>
        function openModal(Id, WorkStartedOn) {
            document.getElementById('ctl00_ContentPlaceHolder1_hfModalStatus').value = "";
            document.getElementById('ctl00_ContentPlaceHolder1_hfModalStatusRem').value = "";
            document.getElementById("ctl00_ContentPlaceHolder1_hfRiskReviewId").value = Id;
            document.getElementById("ctl00_ContentPlaceHolder1_hfWorkStartedOn").value = WorkStartedOn;

            if (WorkStartedOn == '') {
                document.getElementById('trStatusAfter').style.visibility = "hidden";
                document.getElementById('trStatusAfter').style.display = "none";
                document.getElementById('trStatusBefore').style.visibility = "visible";
                document.getElementById('trStatusBefore').style.display = "table-row";

                //ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvStatus1'), false);
                //ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvStatus2'), true);
            }
            else {
                document.getElementById('trStatusAfter').style.visibility = "visible";
                document.getElementById('trStatusAfter').style.display = "table-row";
                document.getElementById('trStatusBefore').style.visibility = "hidden";
                document.getElementById('trStatusBefore').style.display = "none";

                //ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvStatus1'), true);
                //ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvStatus2'), false);
            }
        }

        function setStatusChange(Control) {
            var Status = document.getElementById('ctl00_ContentPlaceHolder1_' + Control).value;

            document.getElementById('ctl00_ContentPlaceHolder1_hfModalStatus').value = Status;

            if (Status != "") {
                return true;
            }
        }

        function setCommentChange() {
            var StatusRem = document.getElementById('ctl00_ContentPlaceHolder1_txtStatusRem').value;

            document.getElementById('ctl00_ContentPlaceHolder1_hfModalStatusRem').value = StatusRem;

            if (StatusRem != "") {
                return true;
            }
        }

        function doubleClickPreventionModal() {
            var hfClickCounter = document.getElementById('hfClickCounter');
            if (Page_ClientValidate("Comments")) {
                hfClickCounter.value = hfClickCounter.value + 1;
                return true;
            }
            else {
                return false;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField runat="server" ID="hfRiskReviewId" />
    <asp:HiddenField runat="server" ID="hfModalStatus" />
    <asp:HiddenField runat="server" ID="hfModalStatusRem" />
    <asp:HiddenField runat="server" ID="hfWorkStartedOn" />

    <asp:HiddenField runat="server" ID="hfClickCounter" />
    <asp:HiddenField ID="hfType" runat="server" />
    <asp:HiddenField ID="hfTypeOfView" runat="server" />
    <asp:HiddenField ID="hfSelectedOperation" runat="server" />
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
                    <div class="row">
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Universe to be Reviewed</label>
                            <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlUniverseReviewed" runat="server"
                                DataTextField="CRUM_UNIVERSE_TO_BE_REVIEWED" DataValueField="CRUM_ID">
                            </f2f:DropdownListNoValidation>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Reviewer Name</label>
                            <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlReviewerName" runat="server"
                                DataTextField="CRM_L0_REVIEWER_NAME" DataValueField="CRM_ID">
                            </f2f:DropdownListNoValidation>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Tentative Start Date</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtTentativeStartDT" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                                <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl="~/Content/images/legacy/calendar.jpg" CssClass="custom-calendar-icon"
                                    ID="imgTentativeStartDT" OnClientClick="return false" />
                            </div>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgTentativeStartDT"
                                TargetControlID="txtTentativeStartDT" Format="dd-MMM-yyyy" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" CssClass="text-danger" runat="server" ControlToValidate="txtTentativeStartDT"
                                ErrorMessage="Date Format has to be dd-MMM-yyyy."
                                ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                ValidationGroup="SEARCH" Display="Dynamic">
                                                </asp:RegularExpressionValidator>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Tentative End Date</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtTentativeEndDT" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                                <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl="~/Content/images/legacy/calendar.jpg" CssClass="custom-calendar-icon"
                                    ID="imgTentativeEndDT" OnClientClick="return false" />
                            </div>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgTentativeEndDT"
                                TargetControlID="txtTentativeEndDT" Format="dd-MMM-yyyy" />
                            <br />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" CssClass="text-danger" runat="server" ControlToValidate="txtTentativeEndDT"
                                ErrorMessage="Date Format has to be dd-MMM-yyyy."
                                ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                ValidationGroup="SEARCH" Display="Dynamic"></asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="cvEndDate" runat="server" ClientValidationFunction="compareStartEndDates" CssClass="text-danger" Enabled="true"
                                ControlToValidate="txtTentativeEndDT" ErrorMessage="Tentative Start Date be less than or equal to To Tentative End Date."
                                Display="Dynamic" OnServerValidate="cvEndDate_ServerValidate" ValidationGroup="SEARCH">Tentative Start Date be less than<br /> or equal to To Tentative End Date.
                                                </asp:CustomValidator>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Status</label>
                            <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlComplianceStatus" runat="server"
                                DataTextField="SM_DESC" DataValueField="SM_NAME">
                            </f2f:DropdownListNoValidation>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">&nbsp;</label>
                            <div>
                                <asp:LinkButton ID="btnSearch" CssClass="btn btn-outline-primary" OnClick="btnSearch_Click" Text="Search"
                                    runat="server" ValidationGroup="SEARCH">
                                    <i class="fa fa-search"></i> Search                     
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnReset" CssClass="btn btn-outline-secondary" Text="Reset"
                                    runat="server" OnClick="btnReset_Click">
                                    <i data-feather="refresh-cw"></i> Reset
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExportToExcel" CssClass="btn btn-outline-secondary" Text="Export to Excel"
                                    runat="server" OnClick="btnExportToExcel_Click">
                                    <i class="fa fa-download"></i> Export to Excel               
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvComplianceReview" AllowPaging="False" AllowSorting="true" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found." EmptyDataRowStyle-BorderColor="transparent"
                            OnSelectedIndexChanged="gvComplianceReview_SelectedIndexChanged" OnRowDataBound="gvComplianceReview_RowDataBound"
                            CssClass="table table-bordered footable" DataKeyNames="CCR_ID">
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
                                <asp:TemplateField HeaderText="Edit" ShowHeader="true">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-success btn-circle"
                                                OnClientClick="onClientEditClick()">
                                                <i class="fa fa-pen"></i>
                                            </asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View" Visible="false">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Select" CommandArgument='<%# Eval("CCR_ID") %>'
                                                CssClass="btn btn-sm btn-soft-info btn-circle" OnClientClick="onClientViewClick()">
                                                <i class="fa fa-eye"></i>
                                            </asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Add Queries" Visible="false">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lnkQueries" runat="server" CommandName="Select"
                                                CssClass="btn btn-sm btn-soft-primary btn-circle" OnClientClick='<%# string.Format("return onAddDRQMClick(\"{0}\");", Eval("CCR_ID")) %>'>
                                                <i class="fa fa-plus"></i>
                                            </asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Queries Response" Visible="false">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lnkDRQMRES" runat="server" CommandName="Select" CommandArgument='<%# Eval("CCR_ID") %>'
                                                CssClass="btn btn-sm btn-soft-primary btn-circle">
                                                <i class="fas fa-eye"></i>
                                            </asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Add Issue Tracker" Visible="false">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lnkAddIssueTracker" runat="server" CommandName="Select"
                                                CssClass="btn btn-sm btn-soft-primary btn-circle" OnClientClick='<%# string.Format("return onAddRiskReviewIssueClick(\"{0}\",\"{1}\");", Eval("CCR_ID"), Eval("CCR_STATUS")) %>'>
                                                <i class="far fa-plus-square"></i>
                                            </asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Issue Tracker" Visible="false">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lnkViewRisks" runat="server" CommandName="Select"
                                                CssClass="btn btn-sm btn-soft-info btn-circle" OnClientClick="onClientIssueClick();">
                                                <i class="far fa-eye"></i>
                                            </asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Risk Review Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCRID" Visible="false" runat="server" Text='<%#Eval("CCR_ID")%>'></asp:Label>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("CCR_STATUS")%>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Update Status">
                                    <ItemTemplate>
                                        <div id="basic-modal">
                                            <center>
                                                <asp:LinkButton ID="lnkSetStatus" runat="server" CommandName="Select" data-bs-toggle="modal" data-bs-target="#basic-modal-content"
                                                    CssClass="btn btn-sm btn-soft-primary btn-circle" OnClientClick='<%# string.Format("openModal(\"{0}\",\"{1}\");", Eval("CCR_ID"), Eval("CCR_WORK_STARTED_ON")) %>'>
                                                    <i class="fas fa-plus-square"></i>
                                                </asp:LinkButton>
                                            </center>
                                        </div>
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
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->


    <!--modal-->
    <div class="modal fade bd-example-modal-lg" id="basic-modal-content" tabindex="-1" aria-labelledby="myExtraLargeModalLabel" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title">
                        <asp:Label ID="Label1" Text="Closure Remark" runat="server"></asp:Label></h6>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                   <asp:Panel runat="server" ID="pnlModalRCA">
                        <div class="row">
                            <div class="col-md-12 mb-3" id="trStatusAfter" style="visibility: hidden; display: none;">
                                <label class="form-label">Status: </label>
                                <asp:DropDownList runat="server" ID="ddlStatus1" CssClass="form-select" onChange="return setStatusChange('ddlStatus1')"
                                    DataTextField="RC_NAME" DataValueField="RC_CODE" >
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-12 mb-3" id="trStatusBefore" style="visibility: hidden; display: none;">
                                <label class="form-label">Status: </label>
                                <asp:DropDownList runat="server" ID="ddlStatus2" CssClass="form-select" onChange="return setStatusChange('ddlStatus2')"
                                    DataTextField="RC_NAME" DataValueField="RC_CODE" >
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-12 mb-3">
                                <label class="form-label">Status Remarks: <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtStatusRem" TextMode="MultiLine" Columns="50" Rows="3" runat="server"
                                    CssClass="form-control" onChange="return setCommentChange();"></asp:TextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtStatusRem" />
                                <asp:RequiredFieldValidator ID="rfvStatusRem" runat="server" ControlToValidate="txtStatusRem"
                                    Display="Dynamic" ValidationGroup="Comments" CssClass="text-danger">Please enter Status Remarks.</asp:RequiredFieldValidator>

                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div class="modal-footer" style="flex-direction: column;">
                    <center>
                        <input type="button" id="btnSubmit" runat="server" value="Submit" class="btn btn-outline-success" onserverclick="btnSubmit_ServerClick" validationgroup="Comments" />
                        <input id="btncCancel" runat="server" type="button" value="Cancel" data-bs-dismiss="modal"
                            class="modalCloseImg simplemodal-close btn btn-outline-danger" />
                    </center>
                </div>
            </div>
        </div>
    </div>

    <%--<script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/jQuery_3.6.0.js")%>"></script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/jQuery_Migrate_3_3_2.js")%>"></script>--%>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/popup/jquery.simplemodal.js")%>"></script>
    <%--<script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/popup/basic.js")%>"></script>--%>
</asp:Content>
