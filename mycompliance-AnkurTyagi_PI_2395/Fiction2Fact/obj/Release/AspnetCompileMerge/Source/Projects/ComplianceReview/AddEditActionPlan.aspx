<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEditActionPlan.aspx.cs" Inherits="Fiction2Fact.Projects.Compliance.AddEditActionPlan" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <asp:PlaceHolder runat="server">
        <title>Add/Edit Action Plan</title>
        <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />

        <style>
            #acePersonResponsible_completionListElem li {
                list-style: none;
                height: 25px;
            }
        </style>
        <script>
            function onCloseClick() {
                window.close();
                window.opener.location.reload(true);
            }

            function onPersonResponsibleChange() {
                var txtPersonResponsibleObj = document.getElementById('txtPersonResponsible');
                document.getElementById('hfFlag').value = 'true';
                if (txtPersonResponsibleObj.value == '') {
                    document.getElementById('hfResponsiblePersonId').value = '';
                }
            }

            function PersonResponsibleClientItemSelected(sender, e) {
                $get("<%= hfResponsiblePersonId.ClientID %>").value = e.get_value();
            }
            function onStatusChanges() {

                var StatusObj = document.getElementById('ddlActionPlanStatus');

                if (StatusObj != null) {
                    var Status = StatusObj.options[StatusObj.selectedIndex].value;

                    if (Status == 'O') {
                        document.getElementById('txtTargetDate').disabled = false;
                        document.getElementById('imgTargetDate').disabled = false;
                        document.getElementById('txtTargetDate').style.backgroundColor = '';
                        ValidatorEnable(document.getElementById('rfvTargetDate'), true)

                        document.getElementById('txtClosureDate').disabled = true;
                        document.getElementById('imgClosureDate').disabled = true;
                        document.getElementById('txtClosureDate').value = '';
                        document.getElementById('txtClosureDate').style.backgroundColor = '#f8f8f8';
                        ValidatorEnable(document.getElementById('rfvClosureDate'), false);
                    }
                    else if (Status == 'C') {
                        document.getElementById('txtTargetDate').disabled = true;
                        document.getElementById('imgTargetDate').disabled = true;
                        document.getElementById('txtTargetDate').value = '';
                        document.getElementById('txtTargetDate').style.backgroundColor = '#f8f8f8';
                        ValidatorEnable(document.getElementById('rfvTargetDate'), false);

                        document.getElementById('txtClosureDate').disabled = false;
                        document.getElementById('imgClosureDate').disabled = false;
                        document.getElementById('txtClosureDate').style.backgroundColor = '';
                        ValidatorEnable(document.getElementById('rfvClosureDate'), true);
                    }
                    else {
                        document.getElementById('txtTargetDate').disabled = false;
                        document.getElementById('txtClosureDate').disabled = false;
                        document.getElementById('imgTargetDate').disabled = false;
                        document.getElementById('imgClosureDate').disabled = false;

                        document.getElementById('txtTargetDate').value = '';

                        document.getElementById('txtTargetDate').style.backgroundColor = '';
                        document.getElementById('txtClosureDate').style.backgroundColor = '';

                        ValidatorEnable(document.getElementById('rfvTargetDate'), true);
                        ValidatorEnable(document.getElementById('rfvClosureDate'), true);
                    }
                }
            }

            function onClientSaveClick() {
                var validated = Page_ClientValidate('Save');
                var IsDoubleClickFlagSet = document.getElementById('hfDoubleClickFlag').value;
                if (validated) {
                    <%--if (IsDoubleClickFlagSet == 'Yes') {
                        alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                        return false;
                    }
                    else {
                        document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                        return true;
                    }--%>
                    return true;
                }
                else {
                    return false;
                }
            }

            function compareTargetDateSystemDates(source, arguments) {
                try {
                    var Targetdate = document.getElementById('txtTargetDate');
                    var SystemDate = document.getElementById('hfCurrDate');

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
                    var Fromdate = document.getElementById('txtClosureDate');
                    var SystemDate = document.getElementById('hfCurrDate');

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

            function onClientEditClick() {
                document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "";
                document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
            }

            function onClientDeleteClick() {
                document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "";
                if (!confirm('Are you sure that you want to delete this record?')) return false;
                document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Delete";
            }
        </script>

        <script>
            function check_validation(validationgroup, msg) {
                if (Page_ClientValidate(validationgroup.toString())) {
                    return confirm(msg);
                }
                return false;
            }

        </script>
    </asp:PlaceHolder>
</head>
<body class="d-block">
    <form id="form1" runat="server">

        <asp:HiddenField ID="hfId" runat="server" />
        <asp:HiddenField ID="hfRefId" runat="server" />
        <asp:HiddenField ID="hfUnitId" runat="server" />
        <asp:HiddenField ID="hfModule" runat="server" />
        <asp:HiddenField ID="hfSelectedOperation" runat="server" />
        <asp:HiddenField ID="hfCurrDate" runat="server" />
        <asp:HiddenField ID="hfType" runat="server" />
        <asp:HiddenField ID="hfIsApproved" runat="server" />
        <asp:HiddenField ID="hfFlag" runat="server" />
        <asp:HiddenField ID="hfResPersonEdit" runat="server" />
        <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />
        <asp:HiddenField ID="hfRecStatus" runat="server" />

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
                                    <h4 class="page-title">
                                        <asp:Label ID="lblHeader" Text="Provide Response" runat="server"></asp:Label></h4>
                                    <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
                                    <asp:Label ID="lblUpdId" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
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
                            <asp:Panel ID="pnlUpdates" runat="server" CssClass="panel">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Management Response <span class="text-danger">*</span></label>
                                            <asp:TextBox CssClass="form-control" ID="txtManagementResponse" runat="server" TextMode="MultiLine"  Rows="4"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvManagementResponse" ValidationGroup="SaveMgmtRes"
                                                ControlToValidate="txtManagementResponse" ErrorMessage="Please enter Management Response"
                                                CssClass="text-danger">
                                                </asp:RequiredFieldValidator>

                                            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtManagementResponse" ID="rev_mgmt" ValidationGroup="SaveMgmtRes" ValidationExpression="^[\s\S]{0,4000}$" runat="server" ForeColor="Red" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Remarks:</label>
                                            <asp:TextBox CssClass="form-control" ID="txtManagementRemark" runat="server" TextMode="MultiLine"  Rows="4"></asp:TextBox>
                                            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtManagementRemark" ID="rev_remarks" ValidationGroup="SaveMgmtRes" ValidationExpression="^[\s\S]{0,4000}$" runat="server" ForeColor="Red" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Whether action plan is required? :</label>
                                            <div class="custom-checkbox-table">
                                                <asp:CheckBox ID="chkActionPlanAdd" runat="server" CssClass="form-control" AutoPostBack="true" OnCheckedChanged="chkActionPlanAdd_CheckedChanged" />
                                            </div>
                                        </div> 
                                    </div>
                                    <div class="mt-3">
                                        <asp:Panel runat="server" ID="pnlRR" Visible="false">
                                            <div class="mt-1">
                                                <div class="card mb-1 mt-1 border">
                                                    <div class="card-header py-0 custom-ch-bg-color">
                                                        <h6 class="font-weight-bold text-white mtb-5">
                                                            <asp:Label ID="lblHeader2" Text="Action Plan" runat="server"></asp:Label>
                                                        </h6>
                                                    </div>
                                                    <div class="card-body mt-1">
                                                        <div class="row">
                                                            <div class="col-md-4 mb-3" style="visibility: hidden; display: none;">
                                                                <label class="form-label">Issue Id:</label>
                                                                <asp:Label CssClass="label" ID="lblRDIId" runat="server"></asp:Label>
                                                            </div>
                                                            <div class="col-md-4 mb-3">
                                                                <label class="form-label">Type of Action:<span class="text-danger">*</span></label>
                                                                <asp:DropDownList ID="ddlTypeofAction" runat="server" CssClass="form-select" Width="250px"
                                                                    DataTextField="RC_NAME" DataValueField="RC_CODE">
                                                                </asp:DropDownList><br />
                                                                <asp:RequiredFieldValidator ID="rfvTypeOfAction" runat="server" ControlToValidate="ddlTypeofAction"
                                                                    Display="Dynamic" ValidationGroup="Save" CssClass="text-danger" SetFocusOnError="True" Text="Please select Type of Action.">Please select Type of Action.
                                                                </asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4 mb-3">
                                                                <label class="form-label">Actionable:<span class="text-danger">*</span></label>
                                                                <asp:TextBox CssClass="form-control" ID="txtMitigationPlan" runat="server"  TextMode="MultiLine"
                                                                    Rows="6" Columns="48"></asp:TextBox><br />
                                                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtMitigationPlan" />
                                                                <asp:RequiredFieldValidator ID="rfvMitigationPlan" runat="server" ControlToValidate="txtMitigationPlan"
                                                                    Display="Dynamic" ValidationGroup="Save" CssClass="text-danger" SetFocusOnError="True" Text="Please enter Mitigation plan for Residual Risk.">Please enter Mitigation plan for Residual Risk.
                                                                </asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4 mb-3">
                                                                <label class="form-label">Action Plan Status:<span class="text-danger">*</span></label>
                                                                <asp:DropDownList ID="ddlActionPlanStatus" runat="server" CssClass="form-select" Width="250px"
                                                                    DataTextField="RC_NAME" DataValueField="RC_CODE">
                                                                </asp:DropDownList><br />
                                                                <asp:RequiredFieldValidator ID="rfvActionPlanStatus" runat="server" ControlToValidate="ddlActionPlanStatus"
                                                                    Display="Dynamic" ValidationGroup="Save" CssClass="text-danger" SetFocusOnError="True" Text="Please select status.">Please select status.
                                                                 </asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4 mb-3">
                                                                <label class="form-label">Unit Responsible : <span class="text-danger">*</span></label>
                                                                <asp:DropDownList runat="server" ID="ddlUnitId" CssClass="form-select" Width="250px"
                                                                    DataTextField="CSFM_NAME" DataValueField="CSFM_ID">
                                                                </asp:DropDownList><br />
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvUnitId" ValidationGroup="Save" ControlToValidate="ddlUnitId"
                                                                    ErrorMessage="Please select Unit" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4 mb-3">
                                                                <label class="form-label">Target Date:<span class="text-danger">*</span></label>
                                                                <div class="input-group">
                                                                    <asp:TextBox ID="txtTargetDate" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                                                                    <asp:ImageButton ToolTip="PopUp Calendar" runat="server" CssClass="custom-calendar-icon" ImageUrl="~/Content/images/legacy/calendar.jpg"
                                                                        ID="imgTargetDate" OnClientClick="return false" />
                                                                </div>
                                                                <ajaxToolkit:CalendarExtender ID="ceTargetDate" runat="server" PopupButtonID="imgTargetDate"
                                                                    TargetControlID="txtTargetDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                                                <asp:RequiredFieldValidator ID="rfvTargetDate" runat="server" ControlToValidate="txtTargetDate"
                                                                    Display="Dynamic" ValidationGroup="Save" CssClass="text-danger" SetFocusOnError="True">Please enter Target Date.</asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="revTargetDate" CssClass="text-danger" runat="server" ControlToValidate="txtTargetDate"
                                                                    ErrorMessage="Date Format has to be dd-MMM-yyyy."
                                                                    ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                                                    ValidationGroup="Save" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                <asp:CustomValidator ID="cvTargetDate" CssClass="text-danger" runat="server" ClientValidationFunction="compareTargetDateSystemDates"
                                                                    ControlToValidate="txtTargetDate" ErrorMessage="Target Date should be greater than or equal to system date."
                                                                    Display="Dynamic" OnServerValidate="cvTargetDate_ServerValidate" ValidationGroup="Save"></asp:CustomValidator></td>
                                                            </div>
                                                            <div class="col-md-4 mb-3">
                                                                <label class="form-label">Closure Date:</label>
                                                                <div class="input-group">
                                                                    <asp:TextBox ID="txtClosureDate" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                                                                    <asp:ImageButton ToolTip="PopUp Calendar" runat="server" CssClass="custom-calendar-icon" ImageUrl="~/Content/images/legacy/calendar.jpg"
                                                                        ID="imgClosureDate" OnClientClick="return false" />
                                                                </div>
                                                                <ajaxToolkit:CalendarExtender ID="ceClosureDate" runat="server" PopupButtonID="imgClosureDate"
                                                                    TargetControlID="txtClosureDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                                                <asp:RequiredFieldValidator ID="rfvClosureDate" runat="server" ControlToValidate="txtClosureDate"
                                                                    Display="Dynamic" ValidationGroup="Save" CssClass="text-danger" SetFocusOnError="True">Please enter Closure Date.</asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="revClosureDate" CssClass="text-danger" runat="server" ControlToValidate="txtClosureDate"
                                                                    ErrorMessage="Date Format has to be dd-MMM-yyyy."
                                                                    ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                                                    ValidationGroup="Save" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                <asp:CustomValidator ID="cvClosureDate" CssClass="text-danger" runat="server" ClientValidationFunction="compareClosureDateSystemDates"
                                                                    ControlToValidate="txtClosureDate" ErrorMessage="Closure Date should be less than or equal to system date."
                                                                    Display="Dynamic" OnServerValidate="cvClosureDate_ServerValidate" ValidationGroup="Save"></asp:CustomValidator>
                                                            </div>
                                                            <div class="col-md-4 mb-3">
                                                                <label class="form-label">Person Responsible :<span class="text-danger">*</span></label>
                                                                <asp:TextBox CssClass="form-control" ID="txtPersonResponsible" MaxLength="100" runat="server"
                                                                    onchange="onPersonResponsibleChange();" Columns="30"></asp:TextBox>
                                                                <asp:HiddenField ID="hfResponsiblePersonId" runat="server" />
                                                                <ajaxToolkit:AutoCompleteExtender ServiceMethod="getUserDetailsbyPhoneBook" MinimumPrefixLength="2" CompletionInterval="100"
                                                                    ServicePath="~/Projects/AJAXDropdown.asmx" EnableCaching="false" CompletionSetCount="1" runat="server"
                                                                    TargetControlID="txtPersonResponsible" ID="acePersonResponsible" FirstRowSelected="True" CompletionListItemCssClass="cssListItem"
                                                                    CompletionListHighlightedItemCssClass="cssListItemHighlight" ShowOnlyCurrentWordInCompletionListItem="true"
                                                                    OnClientItemSelected="PersonResponsibleClientItemSelected">
                                                                </ajaxToolkit:AutoCompleteExtender>
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvPersonResponsible" ValidationGroup="Save" ControlToValidate="txtPersonResponsible"
                                                                    ErrorMessage="Please enter Person Responsible" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4 mb-3">
                                                                <label class="form-label">Remarks:</label>
                                                                <asp:TextBox CssClass="form-control" ID="txtRemarks" TextMode="MultiLine"
                                                                    Rows="6" Columns="48" runat="server"></asp:TextBox>
                                                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtRemarks" ID="rev_remarks1" ValidationGroup="Save" ValidationExpression="^[\s\S]{0,4000}$" runat="server" ForeColor="Red" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>
                                                            </div>
                                                        </div>
                                                        <div class="text-center mt-3">
                                                            <asp:LinkButton CssClass="btn btn-outline-success" runat="server" ID="btnSave" OnClientClick="return onClientSaveClick();"
                                                                OnClick="btnSave_Click" Text="Save" ValidationGroup="Save" >
                                                                <i class="fa fa-save me-2"></i> Save                    
                                                            </asp:LinkButton>
                                                            <asp:LinkButton runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Cancel"
                                                                CssClass="btn btn-outline-danger" >
                                                                <i class="fa fa-arrow-left me-2"></i> Cancel                   
                                                            </asp:LinkButton>
                                                        </div>
                                                        <div class="table-responsive mt-3">
                                                            <asp:GridView ID="gvActionables" runat="server" AutoGenerateColumns="False" DataKeyNames="CIA_ID"
                                                            AllowSorting="false" AllowPaging="false" GridLines="Both" CellPadding="4"
                                                            CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                            EmptyDataText="No record found..." OnSelectedIndexChanged="gvActionables_SelectedIndexChanged" OnRowDataBound="gvActionables_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sr.No." ControlStyle-Width="20px" HeaderStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Edit" ShowHeader="true" ControlStyle-Width="20px" HeaderStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-success btn-circle"
                                                                                CausesValidation="false" 
                                                                                OnClientClick="return onClientEditClick();">
                                                                                <i class="fa fa-pen"></i>	                            
                                                                            </asp:LinkButton>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delete" ShowHeader="true" ControlStyle-Width="20px" HeaderStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Select"  
                                                                                CssClass="btn btn-sm btn-soft-danger btn-circle" OnClientClick="return onClientDeleteClick();">
                                                                                <i class="fa fa-trash"></i>  
                                                                            </asp:LinkButton>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="Type of Action" DataField="ActionType" />
                                                                <asp:TemplateField HeaderText="Actionables" ShowHeader="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblActionPlan" runat="server" Text='<%#Eval("CIA_ACTIONABLE").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                                                        <asp:Label ID="lblRecStatus" runat="server" Visible="false" Text='<%#Eval("CIA_STATUS") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="Action Plan Status" DataField="ActionStatus" />
                                                                <asp:BoundField HeaderText="Unit Responsible" DataField="CSFM_NAME" />
                                                                <asp:BoundField DataField="CIA_TARGET_DT" HeaderText="Target Date" ControlStyle-Width="110px" HeaderStyle-Width="110px" DataFormatString="{0:dd-MMM-yyyy}" />
                                                                <asp:BoundField DataField="CIA_CLOSURE_DT" HeaderText="Closure Date" ControlStyle-Width="110px" HeaderStyle-Width="110px" DataFormatString="{0:dd-MMM-yyyy}" />
                                                                <asp:BoundField HeaderText="Person Responsible" DataField="CIA_SPECIFIED_PERSON_NAME" />
                                                                <asp:TemplateField HeaderText="Remarks" ShowHeader="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("CIA_REMARKS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="Status" DataField="RecStatus" />
                                                            </Columns>
                                                        </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <div class="text-center mt-3">
                                        <asp:LinkButton ID="btnSubmitasDraft" runat="server" CssClass="btn btn-outline-success" Text="Save as Draft" OnClick="btnSubmitasDraft_Click" >
                                            <i class="fa fa-save me-2"></i> Save as Draft                    
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-danger" runat="server" ID="btnclose" Text="Close" OnClientClick="return onCloseClick();" >
                                            <i class="fa fa-arrow-left me-2"></i> Close 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnReadyToSubmit" runat="server" CssClass="btn btn-outline-success" Text="Ready to Submit" ValidationGroup="SaveMgmtRes" OnClientClick="return confirm('Is this response is ready to submit?');" OnClick="btnReadyToSubmit_Click" >
                                            <i class="fa fa-save me-2"></i> Ready to Submit
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <!-- end row -->
            </div>
        </div>
    </form>
</body>
</html>
