<%@ Page Title="" Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" CodeBehind="MyActionables.aspx.cs" Inherits="Fiction2Fact.Projects.ComplianceReview.MyActionables" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js")%>"></script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/dateformatvalidation.js")%>"></script>
    <script type="text/javascript">

        function onClientEditClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
        }
        function compareEndDates(source, arguments) {
            try {
                //var ContractTemplateId = document.getElementById('ctl00_ContentPlaceHolder1_hfCTId').value;
                //if (ContractTemplateId == '' || ContractTemplateId == null) {


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

    <asp:HiddenField ID="hfSelectedOperation" runat="server" />
    <asp:HiddenField ID="hfSelectedRecord" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">My Actionables</h4>
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
                            <label class="form-label">Status</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlStatus" runat="server" DataTextField="RC_NAME" DataValueField="RC_CODE">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Target Date From</label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtFromDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                                <asp:ImageButton ID="ibFrmDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />
                            </div>
                            <asp:RegularExpressionValidator ID="revFromDate" runat="server" ControlToValidate="txtFromDate" ForeColor="Red"
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
                            <asp:RegularExpressionValidator ID="revToDate" runat="server" ControlToValidate="txtToDate" ForeColor="Red"
                                Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                            <cc1:CalendarExtender ID="ceTodate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibToDate"
                                TargetControlID="txtToDate"></cc1:CalendarExtender>
                            <asp:CustomValidator ID="cvCompletionDate" runat="server" ClientValidationFunction="compareEndDates"
                                ControlToValidate="txtToDate" ErrorMessage="From Date should be less than or Equal to To Date." ForeColor="Red"
                                Display="Dynamic" OnServerValidate="cvTodate_ServerValidate" ValidationGroup="SEARCH"></asp:CustomValidator>
                        </div>
                    </div>
                    <div class="text-center mt-3">
                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="Button1" Text="Search" runat="server" ValidationGroup="SEARCH"
                            AccessKey="s" OnClick="btnSearch_Click" >
                            <i class="fa fa-search"></i> Search                     
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnExportToExcel" Visible="false" runat="server" CssClass="btn btn-outline-secondary    "
                            Text="Export to Excel" OnClick="btnExportToExcel_Click" >
                            <i class="fa fa-download"></i> Export to Excel               
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvActionables" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            GridLines="Both" DataKeyNames="CIA_CI_ID"
                            OnPageIndexChanging="gvActionables_PageIndexChanging"
                            OnSelectedIndexChanged="gvActionables_SelectedIndexChanged"
                            AllowPaging="true" AllowSorting="true"
                            CssClass="table table-bordered footable" PagerStyle-CssClass="pgr"
                            AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:TemplateField ShowHeader="true" HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbView" runat="server" OnClientClick="onClientEditClick()" CssClass="btn btn-sm btn-soft-success btn-circle"
                                            CommandName="Select">
                                            <i class="fa fa-pen"></i>	                            
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActionableId" runat="server" Text='<%# Bind("CIA_ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sr.No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actionable">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActionable" runat="server" Text='<%# Eval("CIA_ACTIONABLE").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Responsible Unit">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSpocFromcompliance" runat="server" Text='<%# Eval("CSFM_NAME") %>'></asp:Label>
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
                                <asp:TemplateField HeaderText="Completion By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosureBy" runat="server" Text='<%# Eval("CIA_CLOSURE_BY") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Completion On">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosureOn" runat="server" Text='<%# Eval("CIA_CLOSURE_DT", "{0:dd-MMM-yyyy }") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Completion Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosureRemarks" runat="server" Text='<%# Eval("CIA_CLOSURE_REMARKS").ToString().Replace("\n", "<br />") %>'></asp:Label>
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
</asp:Content>
