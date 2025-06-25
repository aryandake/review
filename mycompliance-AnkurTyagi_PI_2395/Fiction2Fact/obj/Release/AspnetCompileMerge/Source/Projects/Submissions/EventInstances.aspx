<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Submissions.Admin_EventInstances" Title="Event Instances" CodeBehind="EventInstances.aspx.cs" %>

<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="custom" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js")%>"></script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/dateformatvalidation.js")%>"></script>
    <script>
        //function compareDates(source, arguments) {
        //    try {
        //        //var ContractTemplateId = document.getElementById('ctl00_ContentPlaceHolder1_hfCTId').value;
        //        //if (ContractTemplateId == '' || ContractTemplateId == null) {

        //        var Fromdate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurrDate').value;
        //        var ToDate = document.getElementById('ctl00_ContentPlaceHolder1_txtDateOfEvent').value;

        //        if (Fromdate.value != '') {
        //            if (compare2DatesNew(Fromdate, ToDate) < 1) {
        //                arguments.IsValid = false;
        //            }
        //            else {
        //                arguments.IsValid = true;
        //            }
        //        }
        //        else {
        //            arguments.IsValid = true;
        //        }
        //    }
        //    catch (e) {
        //        alert(e);
        //        arguments.IsValid = false;
        //    }
        //}
    </script>
    <asp:HiddenField ID="hfCurrDate" runat="server" />
    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Add New Event Instance</h4>
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
                    <asp:Panel ID="Panel1" runat="server" Visible="true">
                        <asp:HiddenField ID="hfEventId" runat="server" />
                        <div runat="server" id="tblForm">
                            <div class="row">
                                <div class="col-md-12 mb-3">
                                    <label class="form-label">Event Name:<span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="ddlEvent" CssClass="form-select" AppendDataBoundItems="true"
                                        runat="server" AutoPostBack="true" DataValueField="EM_ID" DataTextField="EM_EVENT_NAME"
                                        OnSelectedIndexChanged="ddlEvent_SelectedIndexChanged">
                                        <asp:ListItem Value="">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvEvent" runat="server" ControlToValidate="ddlEvent" CssClass="text-danger" ValidationGroup="SaveEventsDetails" Display="Dynamic" SetFocusOnError="True"
                                        ErrorMessage="Please select Event."></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-12 mb-3">
                                    <label class="form-label">Event Agenda:<span class="text-danger">*</span></label>
                                    <div class="custom-checkbox-table">
                                        <asp:CheckBoxList ID="cblAssociatedWith" runat="server" RepeatColumns="7" AppendDataBoundItems="true"
                                            DataTextField="EP_NAME" DataValueField="EP_ID" CssClass="form-control">
                                        </asp:CheckBoxList>
                                        <%--<custom:CheckBoxListValidator runat="server" ID="cbvAssociatedWith" ControlToValidate="cblAssociatedWith"
                                        Display="Dynamic" ValidationGroup="SaveEventsDetails">Please select one or more Agenda</custom:CheckBoxListValidator>--%>
                                    </div>
                                </div>
                                <div class="col-md-12 mb-3">
                                    <label class="form-label">Date of Event:<span class="text-danger">*</span></label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtDateOfEvent" CssClass="form-control" runat="server"
                                            MaxLength="11"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgEventDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />&nbsp;
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvEventDate" runat="server" ControlToValidate="txtDateOfEvent"
                                        ValidationGroup="SaveEventsDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                        ErrorMessage="Please enter Event Date."></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="rev" runat="server" ControlToValidate="txtDateOfEvent" CssClass="text-danger" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                        ValidationGroup="SaveEventsDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ceOnceFromDate" runat="server" PopupButtonID="imgEventDate"
                                        TargetControlID="txtDateOfEvent" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>

                                    <%--<asp:CustomValidator ID="cvOnlyOnce" runat="server" ClientValidationFunction="compareDates"
                                        ControlToValidate="txtDateOfEvent" ErrorMessage="Event Date should be greater than or equal to current date." CssClass="text-danger"
                                        Display="Dynamic" OnServerValidate="cvdate_ServerValidate" ValidationGroup="SaveEventsDetails"></asp:CustomValidator>--%>
                                </div>
                                <div class="col-md-12 mb-3">
                                    <label class="form-label">Description:<span class="text-danger">*</span></label>
                                    <F2FControls:F2FTextBox ID="txtDescription" CssClass="form-control" TextMode="MultiLine"
                                        Columns="70" Rows="3" MaxLength="200" runat="server">
                                    </F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtDescription" />
                                    <asp:RequiredFieldValidator ID="rfvParticulars" runat="server" ControlToValidate="txtDescription"
                                        ValidationGroup="SaveEventsDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger" ErrorMessage="Please enter Description."></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revPerticulars" ControlToValidate="txtDescription" CssClass="text-danger" Display="Dynamic" Text="Exceeding 200 characters" ValidationExpression="^[\s\S]{0,200}$"
                                        runat="server" SetFocusOnError="True" ValidationGroup="SaveEventsDetails" />
                                </div>
                            </div>
                            <div class="mt-3 text-center">
                                <asp:LinkButton ID="btnSave1" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="btn btn-outline-success" ValidationGroup="SaveEventsDetails">
                                    <i class="fa fa-save me-2"></i> Save                  
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnBack" runat="server" OnClick="btnCancel_Click" Text="Close" CssClass="btn btn-outline-danger">
                                    <i class="fa fa-arrow-left me-2"></i> Close                  
                                </asp:LinkButton>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="Panel2" runat="server" Visible="false">
                        <div class="mt-3 text-center">
                            <asp:LinkButton ID="btnBack1" runat="server" OnClick="btnCancel_Click" Text="Back"
                                CausesValidation="false" CssClass="btn btn-outline-danger">
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
