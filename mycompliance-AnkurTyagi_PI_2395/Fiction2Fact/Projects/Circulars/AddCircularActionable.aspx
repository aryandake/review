<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCircularActionable.aspx.cs" Async="true"
    Inherits="Fiction2Fact.Projects.Circular.AddCircularActionable" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Language" content="en-us" />
    <meta name="GENERATOR" content="Microsoft FrontPage 6.0" />
    <meta name="ProgId" content="FrontPage.Editor.Document" />
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <title>Circular Actionable</title>
    <asp:PlaceHolder runat="server">
        <link id="Link2" rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css")%>" />
        <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/populateUserDetsAJAX.js")%>"></script>
        <%--<script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/jquery-3.5.0.js") %>"></script>--%>
        <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/jQuery_3.6.0.js") %>"></script>
        <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/jQuery_Migrate_3_3_2.js") %>"></script>
        <%--<script src="<%=Fiction2Fact.Global.site_url("Scripts/legacy/AutoComplete/jquery-ui.min.js")%>" type="text/javascript"></script>
        <link href="<%=Fiction2Fact.Global.site_url("Scripts/legacy/AutoComplete/jquery-ui.css")%>" rel="Stylesheet" type="text/css" />--%>

        <script type="text/javascript">
            var ret = false;
            //window.onbeforeunload = function () {
            //    var btnRefresh = window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnRefresh');
            //    if (btnRefresh == null) {
            //        btnRefresh = window.opener.document.getElementById('ctl00_ContentPlaceHolder1_fvCircularMaster_btnRefresh');
            //    }

            //    btnRefresh.click();
            //}
            $(document).ready(function () {

                $("#<%= btnSubmit.ClientID%>").click(function () {
                    if (Page_ClientValidate("SaveActionable")) {
                        if ($("#<%= hfDoubleClick.ClientID%>").val() == "Yes") {
                            alert("You have double clicked the button.\r\nKindly wait for the process to complete.");
                            return false;
                        }
                        else {
                            $("#<%= hfDoubleClick.ClientID %>").val("Yes");
                            return true;
                        }
                    }
                    else {
                        return false;
                    }
                });

            });
            function closeWindowRef() {
                ret = true;
                //window.opener.location.reload(true);
                window.close();
            }

            function onClientDeleteClick() {
                if (!confirm('Are you sure that you want to delete this record?'))
                    return false;

                document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Delete";
                return true;
            }

            function onClientEditClick() {
                document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
                return true;
            }

            function onClientCopyClick() {
                document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Copy";
                return true;
            }
        </script>

        <script type="text/javascript">
            function onPersonResponsibleChange() {
                var txtPersonResponsibleObj = document.getElementById('<%= txtResponsiblePerson.ClientID %>');
                document.getElementById('<%= hfFlag.ClientID %>').value = 'true';
                if (txtPersonResponsibleObj.value == '') {
                    document.getElementById('<%= hfResponsiblePersonId.ClientID %>').value = '';
                }
            }

            function PersonResponsibleClientItemSelected(sender, e) {
                $get("<%= hfResponsiblePersonId.ClientID %>").value = e.get_value();
            }


            function onReportingManagerChange() {
                var txtReportingManagerObj = document.getElementById('<%= txtReportingMgr.ClientID %>');
                document.getElementById('<%= hfRMFlag.ClientID %>').value = 'true';
                if (txtReportingManagerObj.value == '') {
                    document.getElementById('<%= hfReportingManagerId.ClientID %>').value = '';
                }
            }

            function ReportingManagerClientItemSelected(sender, e) {
                $get("<%= hfReportingManagerId.ClientID %>").value = e.get_value();
            }

            function onStatusChange() {
                if ($('[id*="ddlStatus"]').val() == 'C') {
                    $('#spnTargetDt').css({ "visibility": "hidden", "display": "none" });
                    ValidatorEnable(document.getElementById("cvTargetDate"), false);
                    $('#spnRegDueDt').css({ "visibility": "hidden", "display": "none" });
                    ValidatorEnable(document.getElementById("cvRegDueDate"), false);
                    $('#spnCompDt').css({ "visibility": "visible", "display": "inline-block" });
                    ValidatorEnable(document.getElementById("cvCompletionDate"), true);
                    $('#divCompletion').css({ "visibility": "visible", "display": "inline-block" });
                    $('#divRegulatory').css({ "visibility": "hidden", "display": "none" });
                    $('#divTarget').css({ "visibility": "hidden", "display": "none" });
                }
                else if ($('[id*="ddlStatus"]').val() == 'P') {
                    $('#spnTargetDt').css({ "visibility": "visible", "display": "inline-block" });
                    ValidatorEnable(document.getElementById("cvTargetDate"), true);
                    $('#spnRegDueDt').css({ "visibility": "visible", "display": "inline-block" });
                    ValidatorEnable(document.getElementById("cvRegDueDate"), true);
                    $('#spnCompDt').css({ "visibility": "hidden", "display": "none" });
                    ValidatorEnable(document.getElementById("cvCompletionDate"), false);
                    $('#divCompletion').css({ "visibility": "hidden", "display": "none" });
                    $('#divRegulatory').css({ "visibility": "visible", "display": "inline-block" });
                    $('#divTarget').css({ "visibility": "visible", "display": "inline-block" });
                }
                else {
                    $('#spnTargetDt').css({ "visibility": "hidden", "display": "none" });
                    ValidatorEnable(document.getElementById("cvTargetDate"), false);
                    $('#lblDueDateText').html("Regulatory Due Date");
                    ValidatorEnable(document.getElementById("cvRegDueDate"), false);
                    $('#spnCompDt').css({ "visibility": "hidden", "display": "none" });
                    ValidatorEnable(document.getElementById("cvCompletionDate"), false);
                    $('#divCompletion').css({ "visibility": "hidden", "display": "none" });
                }
            }
        </script>
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hfSelectedOperation" runat="server" />
        <asp:HiddenField ID="hfCurrDate" runat="server" />
        <asp:HiddenField ID="hfCirId" runat="server" />
        <asp:HiddenField ID="hfActionableId" runat="server" />
        <asp:HiddenField ID="hfDoubleClick" runat="server" />
        <asp:HiddenField ID="hfSpocFromComplianceFnId" runat="server" />
        <asp:HiddenField ID="hfCirType" runat="server" />
        <asp:HiddenField ID="hfCirSubject" runat="server" />
        <asp:HiddenField ID="hfCirIssAuthority" runat="server" />
        <asp:HiddenField ID="hfCirDate" runat="server" />
        <asp:HiddenField ID="hfFlag" runat="server" />
        <asp:HiddenField ID="hfRMFlag" runat="server" />
        <asp:HiddenField ID="hfResponsiblePersonEdit" runat="server" />
        <asp:HiddenField ID="hfReportingManagerEdit" runat="server" />

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>

        <div class="page-content">
            <div class="container-fluid">
                <!-- Page-Title -->
                <div class="row">
                    <div class="col-sm-12">
                        <div class="page-title-box">
                            <div class="row">
                                <div class="col">
                                    <h4 class="page-title">Add Circular Actionables</h4>
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
                                <div runat="server" id="tblForm">
                                    <div class="row">
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Actionable:<span class="text-danger">*</span></label>
                                            <F2FControls:F2FTextBox ID="txtActionTaken" CssClass="form-control" runat="server"
                                                ToolTip="Please enter actionable." TextMode="MultiLine" Rows="2">
                                    </F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtActionTaken" />
                                            <asp:RequiredFieldValidator ID="rfvActionTaken" runat="server" ControlToValidate="txtActionTaken"
                                                ValidationGroup="SaveActionable" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please enter Action to be taken."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label">Responsible Department:<span class="text-danger">*</span></label>
                                            <asp:DropDownList CssClass="form-select" ID="ddlResFunc" runat="server">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvResFunc" runat="server" ControlToValidate="ddlResFunc"
                                                ValidationGroup="SaveActionable" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please select Responsible Function."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label">Responsible Person:<span class="text-danger">*</span></label>
                                            <F2FControls:F2FTextBox runat="server" ID="txtResponsiblePerson" CssClass="form-control" MaxLength="100"
                                                onchange="onPersonResponsibleChange();" Columns="40"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtResponsiblePerson" />
                                            <asp:HiddenField ID="hfResponsiblePersonId" runat="server" />
                                            <ajaxToolkit:AutoCompleteExtender ServiceMethod="getUserDetailsbyPhoneBook" MinimumPrefixLength="2" CompletionInterval="100"
                                                ServicePath="~/Projects/AJAXDropdown.asmx" EnableCaching="false" CompletionSetCount="1" runat="server"
                                                TargetControlID="txtResponsiblePerson" ID="acePersonResponsible" FirstRowSelected="True" CompletionListItemCssClass="cssListItem"
                                                CompletionListHighlightedItemCssClass="cssListItemHighlight"
                                                ShowOnlyCurrentWordInCompletionListItem="true" OnClientItemSelected="PersonResponsibleClientItemSelected">
                                            </ajaxToolkit:AutoCompleteExtender>
                                            <asp:RequiredFieldValidator ID="rfvtxtResponsiblePerson" runat="server" ControlToValidate="txtResponsiblePerson"
                                                ValidationGroup="SaveActionable" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please enter Responsible Person Id."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label">Reporting Manager:<span class="text-danger">*</span></label>
                                            <F2FControls:F2FTextBox runat="server" ID="txtReportingMgr" CssClass="form-control" MaxLength="100"
                                                onchange="onReportingManagerChange();" Columns="40"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtReportingMgr" />
                                            <asp:HiddenField ID="hfReportingManagerId" runat="server" />
                                            <ajaxToolkit:AutoCompleteExtender ServiceMethod="getUserDetailsbyPhoneBook" MinimumPrefixLength="2" CompletionInterval="100"
                                                ServicePath="~/Projects/AJAXDropdown.asmx" EnableCaching="false" CompletionSetCount="1" runat="server"
                                                TargetControlID="txtReportingMgr" ID="aceReportingManger" FirstRowSelected="True" CompletionListItemCssClass="cssListItem"
                                                CompletionListHighlightedItemCssClass="cssListItemHighlight"
                                                ShowOnlyCurrentWordInCompletionListItem="true" OnClientItemSelected="ReportingManagerClientItemSelected">
                                            </ajaxToolkit:AutoCompleteExtender>
                                            <asp:RequiredFieldValidator ID="rfvtxtReportingMgr" runat="server" ControlToValidate="txtReportingMgr"
                                                ValidationGroup="SaveActionable" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please enter Reporting Manager Id."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label">Status:<span class="text-danger">*</span></label>
                                            <asp:DropDownList CssClass="form-select" ID="ddlStatus" runat="server" onchange="onStatusChange();">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus" ValidationGroup="SaveActionable" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger" ErrorMessage="Please select actionable status."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4 mb-3" id="divTarget">
                                            <label class="form-label">Target Date:<span id="spnTargetDt" style="visibility: hidden; display: none;" class="text-danger">*</span></label>
                                            <div class="input-group">
                                                <F2FControls:F2FTextBox ID="txtTargetDate" CssClass="form-control" runat="server"
                                                    MaxLength="11" ToolTip="Target Date"></F2FControls:F2FTextBox>
                                                <asp:ImageButton ID="imgTargetDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                                    ImageUrl="../../Content/images/legacy/calendar.jpg" ToolTip="Select Date" />
                                            </div>
                                            <ajaxToolkit:CalendarExtender ID="ceDate" runat="server" PopupButtonID="imgTargetDate"
                                                TargetControlID="txtTargetDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                            <asp:CustomValidator ID="cvTargetDate" runat="server" ValidationGroup="SaveActionable"
                                                ControlToValidate="txtTargetDate" CssClass="text-danger" ErrorMessage="Target date cannot be lower than current date."
                                                Display="Dynamic" ValidateEmptyText="true" ClientValidationFunction="CompareTargetDateSytemDate"> 
                                    </asp:CustomValidator>
                                        </div>
                                        <div class="col-md-4 mb-3" id="divRegulatory">
                                            <label class="form-label">Regulatory Due Date:<span id="spnRegDueDt" style="visibility: hidden; display: none;" class="text-danger">*</span></label>
                                            <div class="input-group">
                                                <F2FControls:F2FTextBox ID="txtRegDueDate" CssClass="form-control" runat="server"
                                                    MaxLength="11" ToolTip="Regulatory Due Date"></F2FControls:F2FTextBox>
                                                <asp:ImageButton ID="imgRegDueDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                                    ImageUrl="../../Content/images/legacy/calendar.jpg" ToolTip="Select Date" />
                                            </div>
                                            <ajaxToolkit:CalendarExtender ID="ceRegDueDate" runat="server" PopupButtonID="imgRegDueDate"
                                                TargetControlID="txtRegDueDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>

                                            <asp:CustomValidator ID="cvRegDueDate" runat="server" ValidationGroup="SaveActionable"
                                                ControlToValidate="txtRegDueDate" CssClass="text-danger" ErrorMessage="Regulatory Due Date cannot be lower than target date."
                                                Display="Dynamic" ValidateEmptyText="true" ClientValidationFunction="CheckRegDueDate" />
                                            <asp:CustomValidator ID="cvRegulatorDueDate" runat="server" ValidationGroup="SaveActionable"
                                                ControlToValidate="txtRegDueDate" CssClass="text-danger" ErrorMessage="Regulatory Due Date shall be greater than or equal to Target Date."
                                                Display="Dynamic" ValidateEmptyText="true" ClientValidationFunction="CompareRegulatoryDueDateTargetDate" />
                                        </div>
                                        <div class="col-md-4 mb-3" id="divCompletion">
                                            <label class="form-label">Completion Date:<span id="spnCompDt" style="visibility: hidden; display: none;" class="text-danger">*</span></label>
                                            <div class="input-group">
                                                <F2FControls:F2FTextBox ID="txtCompletionDate" CssClass="form-control" runat="server"
                                                    MaxLength="11" ToolTip="Completion Date"></F2FControls:F2FTextBox>
                                                <asp:ImageButton ID="imgCompletionDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                                    ImageUrl="../../Content/images/legacy/calendar.jpg" ToolTip="Select Date" />
                                            </div>
                                            <ajaxToolkit:CalendarExtender ID="ceCompDate" runat="server" PopupButtonID="imgCompletionDate"
                                                TargetControlID="txtCompletionDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                            <asp:CustomValidator ID="cvCompletionDate" runat="server" ValidationGroup="SaveActionable"
                                                ControlToValidate="txtCompletionDate" CssClass="text-danger" ErrorMessage="Completion Date cannot be greater than current date."
                                                Display="Dynamic" ValidateEmptyText="true" ClientValidationFunction="CompareCompletionDateSytemDate" />
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Remark:</label>
                                            <F2FControls:F2FTextBox runat="server" ID="txtRemark" CssClass="form-control" Rows="2" TextMode="MultiLine" Style="width: 97%;"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemark" />
                                        </div>
                                    </div>
                                    <div class="mt-3 text-center">
                                        <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Add Actionable" CssClass="btn btn-outline-success" ValidationGroup="SaveActionable">
                                        <i class="fa fa-plus"></i> Add Actionable                     
                                        </asp:LinkButton>
                                        <%--<< Added by Amarjeet on 14-Jul-2021--%>
                                        <asp:LinkButton ID="btnSubmit_Circulate" runat="server" OnClick="btnSubmit_Circulate_Click" Text="Submit & Circulate" CssClass="btn btn-outline-success" Visible="false">
                                            <i class="fa fa-save me-2"></i> Submit & Circulate                    
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnCancel" runat="server" Text="Close" CssClass="btn btn-outline-danger" OnClientClick="closeWindowRef();">
                                        <i class="fa fa-arrow-left me-2"></i>  Close                  
                                        </asp:LinkButton>
                                        <%-->>--%>
                                    </div>
                                </div>
                                <div class="mt-3" id="divActionables">
                                    <div class="card mb-1 mt-1 border">
                                        <div class="card-header py-0 custom-ch-bg-color">
                                            <h6 class="font-weight-bold text-white mtb-5">Actionables </h6>
                                        </div>
                                        <div class="card-body mt-1">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gvActionables" runat="server" AutoGenerateColumns="false" DataKeyNames="CA_ID" ShowHeaderWhenEmpty="true"
                                                    AllowSorting="false" AllowPaging="false" GridLines="Both" CellPadding="4"
                                                    OnRowDataBound="gvActionables_RowDataBound" OnSelectedIndexChanged="gvActionables_SelectedIndexChanged"
                                                    CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                    EmptyDataText="No record found...">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr.No.">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Copy" ShowHeader="true">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:LinkButton ID="lnkCopy" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-primary btn-circle"
                                                                        CausesValidation="false" OnClientClick="return onClientCopyClick();">
                                                                        <i class="fa fa-copy"></i>
                                                                    </asp:LinkButton>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit" ShowHeader="true">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-success btn-circle"
                                                                        CausesValidation="false" OnClientClick="return onClientEditClick();">
                                                                        <i class="fa fa-pen"></i>
                                                                    </asp:LinkButton>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Delete" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-danger btn-circle" OnClientClick="return onClientDeleteClick();">
                                                                        <i class="fa fa-trash"></i>
                                                                    </asp:LinkButton>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Actionable" ShowHeader="true">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%#Eval("CA_ACTIONABLE").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="CFM_NAME" HeaderText="Responsible Function" />
                                                        <asp:BoundField DataField="CA_PERSON_RESPONSIBLE" HeaderText="Responsible Person" />
                                                        <%--<asp:BoundField DataField="CA_PERSON_RESPONSIBLE_NAME" HeaderText="Person Responsible Name" />
                                    <asp:BoundField DataField="CA_PERSON_RESPONSIBLE_EMAIL_ID" HeaderText="Person Responsible Email" />--%>
                                                        <asp:BoundField DataField="CA_REPORTING_MANAGER" HeaderText="Reporting Manager" />
                                                        <%--<asp:BoundField DataField="CA_Reporting_Mgr_Name" HeaderText="Reporting Manager Name" />
                                    <asp:BoundField DataField="CA_Reporting_Mgr_EMAIL_ID" HeaderText="Reporting Manager Email" />--%>
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
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- end row -->
            </div>
        </div>

    </form>
    <script type="text/javascript">
        function CheckCompletionDate(src, arg) {
            //if ($('[id*="ddlStatus"]').val() == 'C' && arg.Value == '') {
            if ($('[id*="ddlStatus"]').val() != '' && arg.Value == '') {
                arg.IsValid = false;
            } else {
                arg.IsValid = true;
            }
        }
        function CheckTargetDate(src, arg) {
            if ($('[id*="ddlStatus"]').val() == 'P' && arg.Value == '') {
                arg.IsValid = false;
            } else {
                arg.IsValid = true;
            }
        }
        function CheckRegDueDate(src, arg) {
            if ($('[id*="ddlStatus"]').val() == 'P' && arg.Value == '') {
                arg.IsValid = false;
            } else {
                arg.IsValid = true;
            }
        }

        function CompareRegulatoryDueDateTargetDate(src, arg) {
            if ($('[id*="ddlStatus"]').val() == 'P' && arg.Value != '') {
                if (Date.parse($("#txtRegDueDate").val()) < Date.parse($("#txtTargetDate").val())) {
                    arg.IsValid = false;
                }
                else {
                    arg.IsValid = true;
                }
            }
            else {
                arg.IsValid = true;
            }
        }

        function CompareTargetDateSytemDate(src, arg) {
            //if ($('[id*="ddlStatus"]').val() == 'P' && arg.Value != '') {
            if (arg.Value != '') {
                if (Date.parse($("#hfCurrDate").val()) > Date.parse($("#txtTargetDate").val())) {
                    arg.IsValid = false;
                }
                else {
                    arg.IsValid = true;
                }
            } else {
                arg.IsValid = true;
            }
        }

        function CompareCompletionDateSytemDate(src, arg) {
            if ($('[id*="ddlStatus"]').val() == 'C' && arg.Value != '') {
                if (Date.parse($("#hfCurrDate").val()) < Date.parse($("#txtCompletionDate").val())) {
                    arg.IsValid = false;
                }
                else {
                    arg.IsValid = true;
                }
            } else {
                arg.IsValid = true;
            }
        }
    </script>
</body>
</html>
