<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditActionPlan.aspx.cs" Inherits="Fiction2Fact.Projects.ComplianceReview.EditActionPlan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <asp:PlaceHolder runat="server">
        <title>Add/Edit Action Plan</title>
        <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/controlStyle.css") %>" />
        <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/main.css") %>" />
        <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js") %>'></script>

        <style>
            #acePersonResponsible_completionListElem li {
                list-style: none;
                height: 25px;
            }
        </style>
        <script>

            function onCloseClick() {
                window.close();
                window.opener.document.getElementById('btnRefresh').click();
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
<body>
    <form id="form1" runat="server">

        <asp:HiddenField ID="hfId" runat="server" />
        <asp:HiddenField ID="hfIssueId" runat="server" />
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

        <div>


            <center>
                <div class="ContentHeader1">
                    <asp:Label ID="lblHeader" Text="Edit Action Plan" runat="server"></asp:Label>
                </div>
            </center>
            <br />

            <asp:Panel ID="pnlUpdates" runat="server" CssClass="panel">
                <center>
                    <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblUpdId" runat="server" Visible="false" CssClass="label"></asp:Label>
                </center>

                <asp:Panel runat="server" ID="pnlRR" Visible="true">
                    <center>
                        <div class="ContentHeader1">
                            <asp:Label ID="lblHeader2" Text="Action Plan" runat="server"></asp:Label>
                        </div>
                    </center>
                    <br />
                    <table width="100%" style="background-color: #DDDDDD;" cellpadding="2" cellspacing="1">
                        <tr style="visibility: hidden; display: none;">
                            <td class="tabbody" width="50%">Issue Id:</td>
                            <td class="tabbody" width="50%">
                                <asp:Label CssClass="label" ID="lblRDIId" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td class="tabbody">Type of Action:<span class="label">*</span></td>
                            <td class="tabbody">Actionable:<span class="label">*</span></td>
                        </tr>

                        <tr>
                            <td class="tabbody">
                                <asp:DropDownList ID="ddlTypeofAction" runat="server" CssClass="form-select" Width="250px"
                                    DataTextField="RC_NAME" DataValueField="RC_CODE">
                                </asp:DropDownList><br />
                                <asp:RequiredFieldValidator ID="rfvTypeOfAction" runat="server" ControlToValidate="ddlTypeofAction"
                                    Display="Dynamic" ValidationGroup="Save" CssClass="span" SetFocusOnError="True" Text="Please select Type of Action.">Please select Type of Action.
                            </asp:RequiredFieldValidator></td>
                            <td class="tabbody">
                                <asp:TextBox CssClass="textbox1" ID="txtMitigationPlan" runat="server" Width="400px" TextMode="MultiLine"
                                    Rows="6" Columns="48"></asp:TextBox><br />
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtMitigationPlan" />
                                <asp:RequiredFieldValidator ID="rfvMitigationPlan" runat="server" ControlToValidate="txtMitigationPlan"
                                    Display="Dynamic" ValidationGroup="Save" CssClass="span" SetFocusOnError="True" Text="Please enter Mitigation plan for Residual Risk.">Please enter Mitigation plan for Residual Risk.
                            </asp:RequiredFieldValidator>

                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtMitigationPlan" ID="rev" ValidationGroup="Save"
                                ValidationExpression="^[\s\S]{0,4000}$" runat="server" ForeColor="Red" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>

                            </td>
                        </tr>


                        <tr>
                            <td class="tabbody">Action Plan Status:<span class="label">*</span></td>
                            <td class="tabbody">Unit Responsible : <span class="label">*</span></td>
                        </tr>

                        <tr>
                            <td class="tabbody">
                                <asp:DropDownList ID="ddlActionPlanStatus" runat="server" CssClass="form-select" Width="250px"
                                    DataTextField="RC_NAME" DataValueField="RC_CODE">
                                </asp:DropDownList><br />
                                <asp:RequiredFieldValidator ID="rfvActionPlanStatus" runat="server" ControlToValidate="ddlActionPlanStatus"
                                    Display="Dynamic" ValidationGroup="Save" CssClass="span" SetFocusOnError="True" Text="Please select status.">Please select status.
                            </asp:RequiredFieldValidator></td>
                            <td class="tabbody">
                                <asp:DropDownList runat="server" ID="ddlUnitId" CssClass="form-select" Width="250px"
                                    DataTextField="CSFM_NAME" DataValueField="CSFM_ID">
                                </asp:DropDownList><br />
                                <asp:RequiredFieldValidator runat="server" ID="rfvUnitId" ValidationGroup="Save" ControlToValidate="ddlUnitId"
                                    ErrorMessage="Please select Unit" CssClass="span" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>


                        <tr>
                            <td class="tabbody">Target Date:<span class="label">*</span></td>
                            <td class="tabbody">Closure Date:</td>
                        </tr>

                        <tr>
                            <td class="tabbody">
                                <asp:TextBox ID="txtTargetDate" runat="server" MaxLength="11" CssClass="textbox1"></asp:TextBox>
                                <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl="~/Content/images/legacy/calendar.jpg"
                                    ID="imgTargetDate" OnClientClick="return false" />
                                <ajaxToolkit:CalendarExtender ID="ceTargetDate" runat="server" PopupButtonID="imgTargetDate"
                                    TargetControlID="txtTargetDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                <br />
                                <asp:RequiredFieldValidator ID="rfvTargetDate" runat="server" ControlToValidate="txtTargetDate"
                                    Display="Dynamic" ValidationGroup="Save" CssClass="span" SetFocusOnError="True">Please enter Target Date.</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revTargetDate" CssClass="span" runat="server" ControlToValidate="txtTargetDate"
                                    ErrorMessage="Date Format has to be dd-MMM-yyyy."
                                    ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                    ValidationGroup="Save" Display="Dynamic"></asp:RegularExpressionValidator>
                                <asp:CustomValidator ID="cvTargetDate" CssClass="span" runat="server" ClientValidationFunction="compareTargetDateSystemDates"
                                    ControlToValidate="txtTargetDate" ErrorMessage="Target Date should be greater than or equal to system date."
                                    Display="Dynamic" OnServerValidate="cvTargetDate_ServerValidate" ValidationGroup="Save"></asp:CustomValidator></td>
                            <td class="tabbody">
                                <asp:TextBox ID="txtClosureDate" runat="server" MaxLength="11" CssClass="textbox1"></asp:TextBox>
                                <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl="~/Content/images/legacy/calendar.jpg"
                                    ID="imgClosureDate" OnClientClick="return false" />
                                <ajaxToolkit:CalendarExtender ID="ceClosureDate" runat="server" PopupButtonID="imgClosureDate"
                                    TargetControlID="txtClosureDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                <br />
                                <asp:RequiredFieldValidator ID="rfvClosureDate" runat="server" ControlToValidate="txtClosureDate"
                                    Display="Dynamic" ValidationGroup="Save" CssClass="span" SetFocusOnError="True">Please enter Closure Date.</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revClosureDate" CssClass="span" runat="server" ControlToValidate="txtClosureDate"
                                    ErrorMessage="Date Format has to be dd-MMM-yyyy."
                                    ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                    ValidationGroup="Save" Display="Dynamic"></asp:RegularExpressionValidator>
                                <asp:CustomValidator ID="cvClosureDate" CssClass="span" runat="server" ClientValidationFunction="compareClosureDateSystemDates"
                                    ControlToValidate="txtClosureDate" ErrorMessage="Closure Date should be less than or equal to system date."
                                    Display="Dynamic" OnServerValidate="cvClosureDate_ServerValidate" ValidationGroup="Save"></asp:CustomValidator>
                            </td>
                        </tr>

                        <tr>
                            <td class="tabbody">Person Responsible :<span class="label">*</span></td>
                            <td class="tabbody">Remarks:</td>
                        </tr>

                        <tr>
                            <td class="tabbody">
                                <asp:TextBox CssClass="textbox1" ID="txtPersonResponsible" MaxLength="100" runat="server"
                                    onchange="onPersonResponsibleChange();" Columns="30"></asp:TextBox>
                                <asp:HiddenField ID="hfResponsiblePersonId" runat="server" />
                                <ajaxToolkit:AutoCompleteExtender ServiceMethod="getUserDetailsbyPhoneBook" MinimumPrefixLength="2" CompletionInterval="100"
                                    ServicePath="~/Projects/AJAXDropdown.asmx" EnableCaching="false" CompletionSetCount="1" runat="server"
                                    TargetControlID="txtPersonResponsible" ID="acePersonResponsible" FirstRowSelected="True" CompletionListItemCssClass="cssListItem"
                                    CompletionListHighlightedItemCssClass="cssListItemHighlight" ShowOnlyCurrentWordInCompletionListItem="true"
                                    OnClientItemSelected="PersonResponsibleClientItemSelected">
                                </ajaxToolkit:AutoCompleteExtender>
                                <asp:RequiredFieldValidator runat="server" ID="rfvPersonResponsible" ValidationGroup="Save" ControlToValidate="txtPersonResponsible"
                                    ErrorMessage="Please enter Person Responsible" CssClass="span" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                            <td class="tabbody">
                                <asp:TextBox CssClass="textbox1" ID="txtRemarks" TextMode="MultiLine" Width="400px"
                                    Rows="6" Columns="48" runat="server"></asp:TextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                 <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtRemarks" ID="RegularExpressionValidator1" ValidationGroup="Save"
                                ValidationExpression="^[\s\S]{0,4000}$" runat="server"  ForeColor="Red" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                    </table>

                    <br />
                    <div style="text-align: left;">
                        <asp:Button CssClass="html_button" runat="server" ID="btnSave" OnClientClick="return onClientSaveClick();"
                            OnClick="btnSave_Click" Text="Save" ValidationGroup="Save" />
                        <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Cancel"
                            CssClass="html_button" />
                    </div>
                    <br />






                </asp:Panel>

            </asp:Panel>
        </div>







    </form>
</body>
</html>
