<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" CodeBehind="ActionableUpdates.aspx.cs" Inherits="Fiction2Fact.Projects.ComplianceReview.ActionableUpdates" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js") %>'></script>
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

        <%--function validateUpdates() {
            if (Page_ClientValidate("Save")) {
                var IsDoubleClickFlagSet = document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value;
                if (IsDoubleClickFlagSet == 'Yes') {
                    alert("You have double clicked on the same button. " +
                        "Please wait till the operation is successfully completed.");
                    return false;
                }
                else {
                    document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                    return true;
                }
            }
        }--%>

        $(document).ready(function () {
            //<< when status = Completed 
            if ($("#<%= hfStatus.ClientID %>").val() == "C") {
                $("#divAddUpdates").css({ "display": "none", "visibility": "hidden" });
            }
            //>>

            //<< on Save Update click event
            $("#<%= btnSave.ClientID %>").click(() => {
                if (Page_ClientValidate("Save")) {
                    if ($("#<%= hfDoubleClickFlag.ClientID %>").val() == "Yes") {
                        alert("You have double clicked on the same button. " +
                            "Please wait till the operation is successfully completed.");
                        return false;
                    }
                    else {
                        $("#<%= hfDoubleClickFlag.ClientID %>").val("Yes");
                        return true;
                    }
                }
            });
            //>>

            //<< on Update Type change event
            $("#<%= ddlUpdateType.ClientID %>").change(() => {
                if ($("#<%= ddlUpdateType.ClientID %>").val() == "ET") { // Extension in Target Date
                    $("#<%= lblDate.ClientID %>").html("Revised Target Date");
                    $("#trDate").css({ "visibility": "visible", "display": "table-row" });
                    $("#<%= rfvDate.ClientID %>").html("Please enter Revised Target Date");
                    ValidatorEnable(document.getElementById("<%= rfvDate.ClientID %>"), true);
                    ValidatorEnable(document.getElementById("<%= cvTargetDate.ClientID %>"), true);
                    ValidatorEnable(document.getElementById("<%= cvClosureDate.ClientID %>"), false);
                    $("#<%= lblAttachmentStar.ClientID %>").html("*");
                    //ValidatorEnable(document.getElementById("<%= rfvFileUpload.ClientID %>"), true);
                }
                else if ($("#<%= ddlUpdateType.ClientID %>").val() == "AC") { // Actionable Closure
                    $("#<%= lblDate.ClientID %>").html("Actionable Closure Date");
                    $("#trDate").css({ "visibility": "visible", "display": "table-row" });
                    $("#<%= rfvDate.ClientID %>").html("Please enter Actionable Closure Date");
                    ValidatorEnable(document.getElementById("<%= rfvDate.ClientID %>"), true);
                    $("#<%= lblAttachmentStar.ClientID %>").html("");
                    ValidatorEnable(document.getElementById("<%= cvTargetDate.ClientID %>"), false);
                    ValidatorEnable(document.getElementById("<%= cvClosureDate.ClientID %>"), true);
                    //ValidatorEnable(document.getElementById("<%= rfvFileUpload.ClientID %>"), false);
                }
                else {
                    $("#trDate").css({ "visibility": "hidden", "display": "none" });
                    ValidatorEnable(document.getElementById("<%= rfvDate.ClientID %>"), false);
                    $("#<%= lblAttachmentStar.ClientID %>").html("");
                    ValidatorEnable(document.getElementById("<%= cvTargetDate.ClientID %>"), false);
                    ValidatorEnable(document.getElementById("<%= cvClosureDate.ClientID %>"), false);
                    //ValidatorEnable(document.getElementById("<%= rfvFileUpload.ClientID %>"), false);
                }
            });
            //>>
        });


        function compareTargetDateSystemDates(source, arguments) {
            try {
                var Targetdate = document.getElementById('ctl00_ContentPlaceHolder1_txtDate');
                var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurrDate');

                if (compare2Dates(SystemDate, Targetdate) == 0) {
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

        function compareClosureDateSystemDates(source, arguments) {
            try {
                var Fromdate = document.getElementById('ctl00_ContentPlaceHolder1_txtDate');
                var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurrDate');

                if (compare2Dates(Fromdate, SystemDate) == 0) {
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
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/tabcontrol.js")%>">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <center>
        <asp:HiddenField ID="hfCurrDate" runat="server" />
        <asp:HiddenField runat="server" ID="hfSource" />
        <asp:HiddenField runat="server" ID="hfTabberId" />
        <asp:HiddenField ID="hfIssueId" runat="server" />
        <asp:HiddenField ID="hfActionableId" runat="server" />
        <asp:HiddenField ID="hfSpocFromComplianceFunction" runat="server" />
        <asp:HiddenField ID="hfPersonResponsibleMailId" runat="server" />
        <asp:HiddenField ID="hfReportingMgrMailId" runat="server" />
        <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />
        <asp:HiddenField ID="hfStatus" runat="server" />
    </center>
    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Actionable Updates</h4>
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
                            <a class="nav-link active" data-bs-toggle="tab" href="#home" role="tab" aria-selected="true">Actionable Details</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-bs-toggle="tab" href="#profile" role="tab" aria-selected="false">Add New Update</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-bs-toggle="tab" href="#settings" role="tab" aria-selected="false">Updates Trail</a>
                        </li>
                    </ul>

                    <!-- Tab panes -->
                    <div class="tab-content">
                        <div class="tab-pane p-3 active" id="home" role="tabpanel">
                            <div class="tabular-view">
                                <div class="row g-0">
                                    <div class="col-md-3">
                                        <label>
                                            Created By:                       
                                        </label>
                                    </div>
                                    <div class="col-md-9">
                                        <label>
                                            <asp:Label ID="lblCreator" runat="server"></asp:Label>
                                        </label>
                                    </div>
                                </div>
                                <div class="row g-0">
                                    <div class="col-md-3">
                                        <label>
                                            Responsible Unit:
                       
                                        </label>
                                    </div>
                                    <div class="col-md-9">
                                        <label>
                                            <asp:Label ID="lblIssueUnit" runat="server"></asp:Label>
                                        </label>
                                    </div>
                                </div>
                                <div class="row g-0">
                                    <div class="col-md-3">
                                        <label>
                                            Issue Title:
                       
                                        </label>
                                    </div>
                                    <div class="col-md-9">
                                        <label>
                                            <asp:Label ID="lblIssueTitle" runat="server"></asp:Label>
                                        </label>
                                    </div>
                                </div>
                                <div class="row g-0">
                                    <div class="col-md-3">
                                        <label>
                                            Issue Description:
                       
                                        </label>
                                    </div>
                                    <div class="col-md-9">
                                        <label>
                                            <asp:Label ID="lblIssueDescription" runat="server"></asp:Label>
                                        </label>
                                    </div>
                                </div>
                                <div class="row g-0">
                                    <div class="col-md-3">
                                        <label>Issue Type:</label>
                                    </div>
                                    <div class="col-md-9">
                                        <label>
                                            <asp:Label ID="lblIssueType" runat="server"></asp:Label>
                                        </label>
                                    </div>
                                </div>
                                <div class="row g-0">
                                    <div class="col-md-3">
                                        <label>
                                            Issue Status:
                       
                                        </label>
                                    </div>
                                    <div class="col-md-9">
                                        <label>
                                            <asp:Label ID="lblIssueStatus" runat="server"></asp:Label>
                                        </label>
                                    </div>
                                </div>
                                <div class="row g-0 border-bottom">
                                    <div class="col-md-3">
                                        <label>SPOC Responsible:</label>
                                    </div>
                                    <div class="col-md-9">
                                        <label>
                                            <asp:Label ID="lblSPOCResponsible" runat="server"></asp:Label>
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div class="mt-2">
                                <div class="card mb-1 mt-1 border">
                                    <div class="card-header py-0 custom-ch-bg-color">
                                        <h6 class="font-weight-bold text-white mtb-5">Attachment(s): </h6>
                                    </div>
                                    <div class="card-body mt-1">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvAttachments" runat="server" AllowPaging="false" ShowFooter="false"
                                                AllowSorting="false" BorderStyle="None" BorderWidth="1px" AutoGenerateColumns="false"
                                                CssClass="table table-bordered footable" DataKeyNames="CIF_ID" EmptyDataText="No records found...">
                                                <Columns>
                                                    <asp:BoundField DataField="FileType" HeaderText="File Type" SortExpression="FileType" />
                                                    <asp:TemplateField HeaderText="File Description" ShowHeader="true">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFileDesc" runat="server" Text='<%#Eval("CIF_DESC").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="File Name">
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=CRDRQ&downloadFileName=<%#getFileName(Eval("CIF_SERVER_FILE_NAME"))%>&fileName=<%#getFileName(Eval("CIF_CLIENT_FILE_NAME"))%>','','location=0,status=0,scrollbars=1,width=400,height=200');">
                                                                <%#Eval("CIF_CLIENT_FILE_NAME")%>
                                                                </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="mt-1">
                                <div class="card mb-1 mt-1 border">
                                    <div class="card-header py-0 custom-ch-bg-color">
                                        <h6 class="font-weight-bold text-white mtb-5">Actionable Details: </h6>
                                    </div>
                                    <div class="card-body mt-1">
                                        <div class="tabular-view">
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Type of Action:
                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblActionTypeOfAction" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Actionable:
                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblActionActionable" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Unit Responsible:
                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblActionUnitResponsible" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Person Responsible:
                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblActionPersonResponsible" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Target Date:
                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblActionTargetDate" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Status:
                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblActionStatus" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Remarks:
                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblActionRemarks" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Completion By:
                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblActionClosureBy" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>
                                                        Completion On:
                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblActionClosureOn" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0 border-bottom">
                                                <div class="col-md-3">
                                                    <label>
                                                        Completion Remarks:                                   
                                                    </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lblActionClosureRemarks" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="tab-pane p-3" id="profile" role="tabpanel">
                            <div id="divAddUpdates">
                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label">Update Type: <span class="text-danger">*</span></label>
                                        <asp:DropDownList CssClass="form-select" ID="ddlUpdateType" runat="server" DataTextField="RC_NAME"
                                            DataValueField="RC_CODE">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvUpdateType" runat="server" ControlToValidate="ddlUpdateType" CssClass="text-danger"
                                            Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True">Please select Update Type.</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6 mb-3" id="trDate" style="visibility: hidden; display: none;">
                                        <label class="form-label">
                                            <asp:Label ID="lblDate" runat="server" Text="" />: <span class="text-danger">*</span></label>
                                        <div class="input-group">
                                            <F2FControls:F2FTextBox ID="txtDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                                            <asp:ImageButton ID="ibDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                                ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                        </div>
                                        <cc1:CalendarExtender ID="ceDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibDate"
                                            TargetControlID="txtDate"></cc1:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="revDate" runat="server" ControlToValidate="txtDate" CssClass="text-danger"
                                            Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                            ValidationGroup="Save"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate" CssClass="text-danger"
                                            Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True">Please enter Date.</asp:RequiredFieldValidator>

                                        <asp:CustomValidator ID="cvTargetDate" CssClass="text-danger" runat="server" ClientValidationFunction="compareTargetDateSystemDates"
                                            ControlToValidate="txtDate" ErrorMessage="Revised Target Date should be greater than or equal to system date."
                                            Display="Dynamic" OnServerValidate="cvTargetDate_ServerValidate" ValidationGroup="Save"></asp:CustomValidator>

                                        <asp:CustomValidator ID="cvClosureDate" CssClass="text-danger" runat="server" ClientValidationFunction="compareClosureDateSystemDates"
                                            ControlToValidate="txtDate" ErrorMessage="Closure Date should be less than or equal to system date."
                                            Display="Dynamic" OnServerValidate="cvClosureDate_ServerValidate" ValidationGroup="Save"></asp:CustomValidator>

                                    </div>
                                    <div class="col-md-12 mb-3">
                                        <label class="form-label">Details: <span class="text-danger">*</span></label>
                                        <F2FControls:F2FTextBox CssClass="form-control" ID="txtRemarks" runat="server" TextMode="MultiLine"
                                            Columns="50" Rows="3"></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                        <asp:RequiredFieldValidator ID="rfvRemarks" runat="server" ControlToValidate="txtRemarks" CssClass="text-danger"
                                            Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True">Please enter Details.</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtRemarks" ID="rev" ValidationGroup="Save" ValidationExpression="^[\s\S]{0,4000}$" runat="server" ForeColor="Red" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>
                                    </div>
                                    
                                    <div class="col-md-12 mb-3">
                                        <label class="form-label">
                                            <asp:Label ID="lblAttachments" runat="server" Text="Upload File" />:
                                            <asp:Label ID="lblAttachmentStar" runat="server" Text="" CssClass="text-danger" /></label>
                                            <asp:FileUpload ID="fuFileUpload" runat="server" CssClass="form-control" />
                                        <asp:RegularExpressionValidator ID="revFileUpload" runat="server" ControlToValidate="fuFileUpload" CssClass="text-danger"
                                            Display="Dynamic" ErrorMessage="Upload of this file format is not supported."
                                            ValidationExpression="^.+(.msg|.MSG|.eml|.EML|.Eml|.jpg|.JPG|.bmp|.BMP|.xls|.XLS|.DOC|.doc|.pdf|.PDF|.PPSX|.ppsx|.)$"
                                            ValidationGroup="FeesDetails"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvFileUpload" runat="server" ControlToValidate="fuFileUpload" CssClass="text-danger"
                                            Display="Dynamic" ValidationGroup="Save" Enabled="false" SetFocusOnError="True">Please upload HOD approval copy for extension.</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="text-center mt-3">
                                    <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSave" ValidationGroup="Save" CausesValidation="true" OnClientClick="return confirm('Are you sure want to save this update?');"
                                        runat="server" Text="Save Update" OnClick="btnSave_Click" >
                                        <i class="fa fa-save me-2"></i> Save Update                    
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <div class="tab-pane p-3" id="settings" role="tabpanel">
                            <asp:GridView ID="gvActionableUpdates" runat="server" AutoGenerateColumns="False"
                                AllowSorting="false" AllowPaging="true" GridLines="Both" CellPadding="4" CssClass="table table-bordered footable1"
                                PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                OnPageIndexChanging="gvActionableUpdates_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Sr.No.
                           
                           
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RC_NAME" HeaderText="Update Type" />
                                    <asp:BoundField DataField="CIAU_REVISED_TARGET_DT" HeaderText="Revised Target Date" DataFormatString="{0: dd-MMM-yyyy}" />
                                    <asp:BoundField DataField="CIAU_CLOSURE_DT" HeaderText="Actionable Closure Date" DataFormatString="{0: dd-MMM-yyyy}" />
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Remarks

                           
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" Text='<%#Eval("CIAU_REMARKS").ToString().Replace("\n", "<br />") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Uploaded File">
                                        <ItemTemplate>
                                            <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=ComplianceIssueActionUpdate&downloadFileName=<%#getFileName(Eval("CIAUF_SERVER_FILE_NAME"))%>','','location=0,status=0,scrollbars=1,width=10,height=20');return false">
                                                <%#Eval("CIAUF_CLIENT_FILE_NAME")%>
                                </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CIAU_CREATE_BY" HeaderText="Update Added By" />
                                    <asp:TemplateField HeaderText="Update Added On">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreateDate" runat="server" Text='<%# Eval("CIAU_CREATE_DT", "{0:dd-MMM-yyyy HH:mm:ss}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="mt-3">
                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" >
                            <i class="fa fa-arrow-left me-2"></i> Back                   
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->

</asp:Content>
