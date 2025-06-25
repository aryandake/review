<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" Async="true"
    EnableEventValidation="false" Inherits="Fiction2Fact.Projects.Circulars.Circulars_CircularList" Title="Circular Actionable List" CodeBehind="CircularList.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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

        const CIRC_compareEndDates = (source, arguments) => {
            try {
                var Fromdate = document.getElementById('ctl00_ContentPlaceHolder1_txtCircFromDate');
                var ToDate = document.getElementById('ctl00_ContentPlaceHolder1_txtCircToDate');

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
                        <div class="col-md-3 mb-3">
                            <label class="form-label">Circular No.</label>
                            <F2FControls:F2FTextBox ID="txtCircularNo" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtCircularNo" />
                        </div>
                        <div class="col-md-3 mb-3">
                            <label class="form-label">Circular Date From</label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtCircFromDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                                <asp:ImageButton ID="ibCircFrmDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />
                            </div>
                            <asp:RegularExpressionValidator ID="revCircFromDate" runat="server" ControlToValidate="txtCircFromDate" CssClass="text-danger"
                                Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                            <cc1:CalendarExtender ID="ceCircFrmDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibCircFrmDate"
                                TargetControlID="txtCircFromDate"></cc1:CalendarExtender>
                        </div>
                        <div class="col-md-3 mb-3">
                            <label class="form-label">Circular Date To</label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtCircToDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                                <asp:ImageButton ID="ibCircToDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />
                            </div>
                            <asp:RegularExpressionValidator ID="revCircToDate" runat="server" ControlToValidate="txtCircToDate" CssClass="text-danger"
                                Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                            <cc1:CalendarExtender ID="ceCircTodate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibCircToDate"
                                TargetControlID="txtCircToDate"></cc1:CalendarExtender>
                            <asp:CustomValidator ID="cvCircToDate" runat="server" ClientValidationFunction="CIRC_compareEndDates" CssClass="text-danger"
                                ControlToValidate="txtCircToDate" ErrorMessage="From Date should be less than or Equal to To Date."
                                Display="Dynamic" OnServerValidate="cvCircToDate_ServerValidate" ValidationGroup="SEARCH"></asp:CustomValidator>
                        </div>
                        <div class="col-md-3 mb-3">
                            <label class="form-label">Type</label>
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select" AppendDataBoundItems="true"
                                DataTextField="CDTM_TYPE_OF_DOC" DataValueField="CDTM_ID">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3 mb-3">
                            <label class="form-label">Issuing Authority</label>
                            <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlCircularAuthority"
                                runat="server">
                            </f2f:DropdownListNoValidation>
                            <ajaxToolkit:CascadingDropDown ID="cddIssuingAuthority" runat="server" TargetControlID="ddlCircularAuthority"
                                Category="IssuingAuthority" PromptText="(Select an Issuing Authority)" ServicePath="AJAXDropdownCirculars.asmx"
                                ServiceMethod="GetIssuingAuthority" />
                        </div>
                        <div class="col-md-3 mb-3">
                            <label class="form-label">Topic</label>
                            <F2FControls:F2FTextBox ID="txtTopic" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtTopic" />
                        </div>
                        <div class="col-md-3 mb-3">
                            <label class="form-label">Subject of Circular</label>
                            <F2FControls:F2FTextBox ID="txtSubjectofCircular" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSubjectofCircular" />
                        </div>
                        <div class="col-md-3 mb-3">
                            <label class="form-label">Actionable</label>
                            <F2FControls:F2FTextBox ID="txtActionable" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtActionable" />
                        </div>
                        <div class="col-md-3 mb-3">
                            <label class="form-label">Person Responsible</label>
                            <F2FControls:F2FTextBox ID="txtPersonResponsible" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtPersonResponsible" />
                        </div>
                        <div class="col-md-3 mb-3">
                            <label class="form-label">Status</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select" DataValueField="RC_CODE"
                                DataTextField="RC_NAME" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3 mb-3">
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
                        <div class="col-md-3 mb-3">
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
                    </div>
                    <div class="text-center">
                        <asp:LinkButton ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-outline-primary" OnClick="btnSearch_Click" ValidationGroup="SEARCH">
                            <i class="fa fa-search"></i> Search                     
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="lnkReset" Text="Reset" runat="server"
                            OnClick="lnkReset_Click">Reset
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnExportToExcel" Visible="false" runat="server" CssClass="btn btn-outline-secondary"
                            Text="Export to Excel" OnClick="btnExportToExcel_Click">
                            <i class="fa fa-download"></i> Export to Excel               
                        </asp:LinkButton>
                        <%--<< Added by Amarjeet on 14-Jul-2021--%>
                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnRefresh" Text="Refresh" Style="visibility: hidden; display: none;" runat="server" OnClick="btnRefresh_Click">
                            <i data-feather="refresh-cw"></i> Refresh Grid 
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvCircularActionableList" runat="server" AutoGenerateColumns="False"
                            PageSize="20" CellPadding="4" GridLines="Both" DataKeyNames="CA_ID" AllowPaging="true"
                            AllowSorting="true" CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            EmptyDataText="No Records Found...." OnSelectedIndexChanged="gvCircularActionableList_SelectedIndexChanged"
                            OnPageIndexChanging="gvCircularActionableList_PageIndexChanging" OnSorting="gvCircularActionableList_Sorting"
                            OnRowCreated="OnRowCreated" OnRowDataBound="gvCircularActionableList_RowDataBound">
                            <Columns>
                                <asp:TemplateField ShowHeader="true" HeaderText="View">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lbView" runat="server" OnClientClick="onClientViewClick()" CommandName="Select"
                                                CssClass="btn btn-sm btn-soft-info btn-circle">
                                                <i class="fa fa-eye"></i>
                                            </asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="true" HeaderText="Edit">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lbEdit" runat="server" OnClientClick="onClientEditClick()" CommandName="Select"
                                                CssClass="btn btn-sm btn-soft-success btn-circle">
                                                <i class="fa fa-pen"></i>
                                            </asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<< Added by Amarjeet on 01-Oct-2021--%>
                                <asp:TemplateField ShowHeader="true" HeaderText="Close Actionable">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lbCloseActionable" runat="server" CssClass="btn btn-sm btn-soft-danger btn-circle" CausesValidation="False" OnClientClick='<%# string.Format("return onClientCloseActionableClick(\"{0}\");", Eval("CA_ID")) %>'
                                                CommandName="Select">
                                                <i class="fa fa-ban"></i>
                                            </asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-->>--%>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCircularMasId" runat="server" Text='<%# Bind("CA_CM_ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sr.No.">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line of Business" SortExpression="LEM_NAME" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOB" runat="server" Text='<%# Eval("LEM_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Circular Number" SortExpression="CM_CIRCULAR_NO">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCircNo" runat="server" Text='<%# Eval("CM_CIRCULAR_NO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Circular Date" SortExpression="CM_DATE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCircularDate" runat="server" Text='<%# Eval("CM_DATE", "{0:dd-MMM-yyyy }") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Circular Type" SortExpression="CDTM_TYPE_OF_DOC">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCircType" runat="server" Text='<%# Eval("CDTM_TYPE_OF_DOC") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Issuing Authority" SortExpression="CIA_NAME">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIssuingAuthority" runat="server" Text='<%# Eval("CIA_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Topic" SortExpression="CAM_NAME">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTopic" runat="server" Text='<%# Eval("CAM_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SPOC From Compliance Function">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSpocFromcompliance" runat="server" Text='<%# Eval("SPOCName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Subject of Circular" SortExpression="CM_TOPIC">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCircSubject" runat="server" Text='<%# Eval("CM_TOPIC") %>' Width="400px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actionable" SortExpression="CA_ACTIONABLE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActionable" Width="300px" runat="server" ToolTip='<%#Eval("CA_ACTIONABLE").ToString()%>'
                                            Text='<%#Eval("CA_ACTIONABLE").ToString().Length > 200 ? (Eval("CA_ACTIONABLE") as string).Substring(0, 200).Replace("\n", "<br />") + " ..." : Eval("CA_ACTIONABLE").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                        <asp:Label ID="lblActionable1" runat="server" Visible="false" Text='<%#Eval("CA_ACTIONABLE").ToString()%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Responsible Function" SortExpression="CFM_NAME">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResponsibleFunction" runat="server" Text='<%# Eval("CFM_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Person Responsible User Name" SortExpression="CA_PERSON_RESPONSIBLE_NAME">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPersonResponsibleUserName" runat="server" Text='<%# Eval("CA_PERSON_RESPONSIBLE_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Target Date" SortExpression="CA_TARGET_DATE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTargetDate" runat="server" Text='<%# Eval("CA_TARGET_DATE", "{0:dd-MMM-yyyy }") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Completion Date" SortExpression="CA_COMPLETION_DATE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompletionDate" runat="server" Text='<%# Eval("CA_COMPLETION_DATE", "{0:dd-MMM-yyyy }") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                        <asp:HiddenField ID="hfStatus" runat="server" Value='<%# Eval("CA_STATUS") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks" SortExpression="CA_REMARKS">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" Width="300px" runat="server" ToolTip='<%# Eval("CA_REMARKS") %>'
                                            Text='<%#Eval("CA_REMARKS").ToString().Length > 200 ? (Eval("CA_REMARKS") as string).Substring(0, 200).Replace("\n", "<br />") + " ..." : Eval("CA_REMARKS").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                        <asp:Label ID="lblRemarks1" runat="server" Visible="false" Text='<%#Eval("CA_REMARKS").ToString()%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Completion By" SortExpression="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosureBy" runat="server" Text='<%# Eval("CA_CLOSED_BY") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Completion On" SortExpression="CA_CLOSED_ON">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosureOn" runat="server" Text='<%# Eval("CA_CLOSED_ON", "{0:dd-MMM-yyyy }") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Completion Remarks" SortExpression="CA_CLOSURE_REMARKS">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosureRemarks" Width="300px" runat="server" ToolTip='<%#Eval("CA_CLOSURE_REMARKS").ToString()%>'
                                            Text='<%#Eval("CA_CLOSURE_REMARKS").ToString().Length > 200 ? (Eval("CA_CLOSURE_REMARKS") as string).Substring(0, 200).Replace("\n", "<br />") + " ..." : Eval("CA_CLOSURE_REMARKS").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                        <asp:Label ID="lblClosureRemarks1" runat="server" Visible="false" Text='<%#Eval("CA_CLOSURE_REMARKS").ToString()%>'></asp:Label>

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Files">
                                    <ItemTemplate>
                                        <asp:DataList ID="dlCircularActionableFiles" runat="server" DataKeyField="CAF_ID"
                                            RepeatColumns="1" RepeatDirection="Horizontal" DataSource='<%#LoadCircularActionableFileList(Eval("CA_ID"))%>'>
                                            <ItemTemplate>
                                                <asp:Label ID="lblServerFileName" Visible="false" runat="server" Text='<% # Eval("CAF_SERVERFILENAME")%>'></asp:Label>
                                                <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=Circular&downloadFileName=<%#Eval("CAF_SERVERFILENAME") %>','','location=0,status=0,scrollbars=1,width=400,height=200');">
                                                    <%#Eval("CAF_FILENAME")%>
                                                </a>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->



    <div>
        <div class="modal fade bd-example-modal-lg" id="divModal" tabindex="-1" aria-labelledby="myExtraLargeModalLabel" role="dialog">
            <div class="modal-dialog modal-lg" role="document">
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
                                <label class="form-label">Completion Remarks: <span class="label">*</span></label>
                                <F2FControls:F2FTextBox ID="txtClosureRemarks" ToolTip="Completion Remarks" CssClass="form-control"
                                    runat="server" Columns="64" Rows="5" TextMode="MultiLine">
                                        </F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtClosureRemarks" />
                                <asp:RequiredFieldValidator ID="rfvClosureRemarks" runat="server" ControlToValidate="txtClosureRemarks" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="SaveClosureDetails" SetFocusOnError="True">Please enter Completion Remarks.</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer" style="flex-direction: column;">
                        <center>
                            <input type="button" id="btnCloseActionable" runat="server" value="Close Actionable"
                                class="btn btn-outline-success" validationgroup="SaveClosureDetails" onserverclick="btnCloseActionable_ServerClick" />
                            <input id="btncCancel" runat="server" type="button" value="Cancel" data-bs-dismiss="modal"
                                class="modalCloseImg simplemodal-close btn btn-outline-danger" />
                        </center>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
