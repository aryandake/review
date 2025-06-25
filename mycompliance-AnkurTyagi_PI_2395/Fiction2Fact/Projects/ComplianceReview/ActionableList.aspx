<%@ Page Title="" Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" CodeBehind="ActionableList.aspx.cs" Inherits="Fiction2Fact.Projects.ComplianceReview.ActionableList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js")%>"></script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/dateformatvalidation.js")%>"></script>
    <script type="text/javascript">

        $(document).ready(() => {
            $("#<%= btnCloseActionable.ClientID %>").click(() => {
                if (Page_ClientValidate("SaveClosureDetails")) {
                    $("#<%= btnCloseActionable.ClientID %>").attr('disabled', 'disabled')
                    $("#<%= btnCloseActionable.ClientID %>").css({ "background-color": "#d2d2d2" });
                    $("#<%= btnCloseActionable.ClientID %>").val("Please wait...");
                }
            });
        });

        const onClientCloseActionableClick = (ActionableId) => {
            $("#<%= hfActionableId.ClientID %>").val(ActionableId);
            $("#divModal").modal('show');
            return false;
        };

        const CompareCompletionDateSytemDate = (src, arg) => {
            if (Date.parse($("#<%= hfCurrDate.ClientID %>").val()) < Date.parse($("#<%= txtCompDate.ClientID %>").val())) {
                arg.IsValid = false;
            }
            else {
                arg.IsValid = true;
            }
        }

        const onClientViewClick = () => {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "View";
        }

        const onClientEditClick = () => {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
        }

        const compareEndDates = (source, arguments) => {
            try {
                var Fromdate = document.getElementById('ctl00_ContentPlaceHolder1_txtFromDate');
                var ToDate = document.getElementById('ctl00_ContentPlaceHolder1_txtToDate');

                if (Fromdate.value != '') {
                    if (compare2Dates(ToDate, Fromdate) > 1) {
                        arguments.IsValid = false;
                    }
                    else {
                        arguments.IsValid = true;
                    }
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField ID="hfActionableId" runat="server" />
    <asp:HiddenField ID="hfCurrDate" runat="server" />
    <asp:HiddenField ID="hfSelectedOperation" runat="server" />
    <asp:HiddenField ID="hfClosureDate" runat="server" />
    <asp:HiddenField ID="hfClosureRemarks" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Actionable List / Status</h4>
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
                            <label class="form-label">Compliance Review No.</label>
                            <F2FControls:F2FTextBox ID="txtComplianceReviewNo" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtComplianceReviewNo" />
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Issue</label>
                            <F2FControls:F2FTextBox ID="txtIssue" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtIssue" />
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Actionable</label>
                            <F2FControls:F2FTextBox ID="txtActionable" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtActionable" />
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Person Responsible</label>
                            <F2FControls:F2FTextBox ID="txtPersonResponsible" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtPersonResponsible" />
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Status</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select" DataValueField="RC_CODE"
                                DataTextField="RC_NAME" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Target Date From</label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtFromDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                                <asp:ImageButton ID="ibFrmDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />
                            </div>
                            <asp:RegularExpressionValidator ID="revFromDate" runat="server" ControlToValidate="txtFromDate" CssClass="text-danger"
                                Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                            <cc1:CalendarExtender ID="ceFrmDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibFrmDate"
                                TargetControlID="txtFromDate"></cc1:CalendarExtender>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Target Date To</label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtToDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                                <asp:ImageButton ID="ibToDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />
                            </div>
                            <asp:RegularExpressionValidator ID="revToDate" runat="server" ControlToValidate="txtToDate" CssClass="text-danger"
                                Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                            <cc1:CalendarExtender ID="ceTodate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibToDate"
                                TargetControlID="txtToDate"></cc1:CalendarExtender>
                            <asp:CustomValidator ID="cvCompletionDate" runat="server" ClientValidationFunction="compareEndDates" CssClass="text-danger"
                                ControlToValidate="txtToDate" ErrorMessage="From Date should be less than or Equal to To Date."
                                Display="Dynamic" OnServerValidate="cvTodate_ServerValidate" ValidationGroup="SEARCH"></asp:CustomValidator>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">&nbsp;</label>
                            <div>
                                <asp:LinkButton ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-outline-primary" OnClick="btnSearch_Click" ValidationGroup="SEARCH" >
                                    <i class="fa fa-search"></i> Search                     
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-outline-secondary" OnClick="btnReset_Click" >
                                    <i class="fa fa-undo"></i> Reset
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExportToExcel" Visible="false" runat="server" CssClass="btn btn-outline-secondary"
                                    Text="Export to Excel" OnClick="btnExportToExcel_Click" >
                                    <i class="fa fa-download"></i> Export to Excel               
                                </asp:LinkButton>
                                <%--<< Added by Amarjeet on 14-Jul-2021--%>
                                <asp:LinkButton CssClass="html_button" ID="btnRefresh" Text="Refresh" Style="visibility: hidden; display: none;" runat="server" OnClick="btnRefresh_Click" >
                                    <i data-feather="refresh-cw"></i> Refresh
                                </asp:LinkButton>
                                <%-->>--%>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvComplianceActionableList" runat="server" AutoGenerateColumns="False"
                            PageSize="20" CellPadding="4" GridLines="Both" DataKeyNames="CIA_ID" AllowPaging="true"
                            AllowSorting="true" CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            EmptyDataText="No Records Found...." OnSelectedIndexChanged="gvComplianceActionableList_SelectedIndexChanged"
                            OnPageIndexChanging="gvComplianceActionableList_PageIndexChanging" OnRowDataBound="gvComplianceActionableList_RowDataBound">
                            <Columns>
                                <asp:TemplateField ShowHeader="true" HeaderText="View">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lbView" runat="server" CssClass="btn btn-sm btn-soft-info btn-circle" OnClientClick="onClientViewClick()" CommandName="Select">
                                                <i class="fa fa-eye"></i>
                                            </asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="true" HeaderText="Edit" Visible="False">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lbEdit" runat="server" CssClass="btn btn-sm btn-soft-success btn-circle" OnClientClick="onClientEditClick()" CommandName="Select">
                                                <i class="fa fa-pen"></i>
                                            </asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<< Added by Amarjeet on 01-Oct-2021--%>
                                <asp:TemplateField ShowHeader="true" HeaderText="Close Actionable">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lbCloseActionable" runat="server" CssClass="btn btn-sm btn-soft-danger btn-circle" CausesValidation="False" OnClientClick='<%# string.Format("return onClientCloseActionableClick(\"{0}\");", Eval("CIA_ID")) %>'
                                                CommandName="Select">
                                                <i class="fa fa-ban"></i>
                                            </asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-->>--%>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIssueId" runat="server" Text='<%# Bind("CI_ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sr.No.">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Compliance Identifier No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCircNo" runat="server" Text='<%# Eval("CCR_IDENTIFIER") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Issue Title">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIssueTitle" runat="server" Text='<%# Eval("CI_ISSUE_TITLE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Issue Responsible Unit">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIssueResponsibleUnit" runat="server" Text='<%# Eval("IssueReponsibleUnit") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Issue Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCI_ISSUE_DESC" runat="server" Text='<%#Eval("CI_ISSUE_DESC").ToString().Length>200?(Eval("CI_ISSUE_DESC") as string).Substring(0,200).Replace(Environment.NewLine, "<br />") + " ...":Eval("CI_ISSUE_DESC").ToString().Replace(Environment.NewLine, "<br />")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Issue Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIssueType" runat="server" Text='<%# Eval("IssueType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type of Action">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTypeOfAction" runat="server" Text='<%# Eval("RC_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actionable">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActionable" runat="server" Text='<%# Eval("CIA_ACTIONABLE").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Person Responsible User Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPersonResponsibleUserName" runat="server" Text='<%# Eval("CIA_SPECIFIED_PERSON_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Target Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTargetDate" runat="server" Text='<%# Eval("CIA_TARGET_DT", "{0:dd-MMM-yyyy }") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Completion Date" SortExpression="CIA_CLOSURE_DT">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompletionDate" runat="server" Text='<%# Eval("CIA_CLOSURE_DT", "{0:dd-MMM-yyyy }") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("ActionStatus") %>'></asp:Label>
                                        <asp:HiddenField ID="hfStatus" runat="server" Value='<%# Eval("CIA_ACTIONABLE_STATUS") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("CIA_REMARKS").ToString().Replace("\n", "<br />") %>'></asp:Label>
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

    <div style="width: 100%;">
        <%--<div class="modal fade" id="divModal" tabindex="-1" role="dialog">--%>
        <div class="modal fade" id="divModal" tabindex="-1" aria-labelledby="exampleModalFullscreenLgLabel" aria-hidden="true">
            <div class="modal-dialog modal-fullscreen-lg-down">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title">Actionable Closure</h6>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12 mb-3">
                                <label class="form-label">Completion Date: <span class="text-danger">*</span></label>
                                <div class="input-group">
                                    <F2FControls:F2FTextBox ID="txtCompDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                                    <asp:ImageButton ID="ibCompDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                        ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                </div>
                                <cc1:CalendarExtender ID="ceCompDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibCompDate"
                                    TargetControlID="txtCompDate"></cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvCompDate" runat="server" ControlToValidate="txtCompDate" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="SaveClosureDetails" SetFocusOnError="True">Please enter Completion Date.</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revCompDate" runat="server" ControlToValidate="txtCompDate" CssClass="text-danger"
                                    ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                    Display="Dynamic" ValidationGroup="SaveClosureDetails"></asp:RegularExpressionValidator>
                                <asp:CustomValidator ID="cvCompDate" runat="server" ValidationGroup="SaveClosureDetails"
                                    ControlToValidate="txtCompDate" CssClass="text-danger" ErrorMessage="Completion Date shall be less than or equal to System Date."
                                    Display="Dynamic" ValidateEmptyText="true" ClientValidationFunction="CompareCompletionDateSytemDate" />
                            </div>
                            <div class="col-md-12 mb-3">
                                <label class="form-label">Completion Remarks: <span class="text-danger">*</span></label>
                                <F2FControls:F2FTextBox ID="txtClosureRemarks" ToolTip="Completion Remarks" CssClass="form-control"
                                    runat="server" Columns="64" Rows="5" TextMode="MultiLine">
                                        </F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtClosureRemarks" />
                                <asp:RequiredFieldValidator ID="rfvClosureRemarks" runat="server" ControlToValidate="txtClosureRemarks" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="SaveClosureDetails" SetFocusOnError="True">Please enter Completion Remarks.</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtClosureRemarks" ID="rev" ValidationGroup="SaveClosureDetails" ForeColor="Red"
                                    ValidationExpression="^[\s\S]{0,4000}$" runat="server" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <input type="button" id="btnCloseActionable" runat="server" value="Close Actionable"
                            class="btn btn-outline-success" validationgroup="SaveClosureDetails" onserverclick="btnCloseActionable_ServerClick" />
                        <input type="button" id="Button2" runat="server" value="Cancel" class="btn btn-danger" data-bs-dismiss="modal" aria-label="Close" /> 
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
