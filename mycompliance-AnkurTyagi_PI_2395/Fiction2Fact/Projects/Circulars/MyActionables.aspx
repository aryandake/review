<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Circulars.Circulars_MyActionables" Title="My Actionables" CodeBehind="MyActionables.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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

    <center>
        <br />
        <asp:HiddenField ID="hfSelectedOperation" runat="server" />
        <asp:HiddenField ID="hfSelectedRecord" runat="server" />

        <div class="ContentHeader1">
        </div>
    </center>
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
                            <asp:CustomValidator ID="cvCompletionDate" runat="server" ClientValidationFunction="compareEndDates"
                                ControlToValidate="txtToDate" ErrorMessage="From Date should be less than or Equal to To Date."
                                Display="Dynamic" OnServerValidate="cvTodate_ServerValidate" ValidationGroup="SEARCH"></asp:CustomValidator>
                        </div>
                    </div>
                    <div class="text-center mt-3">
                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="Button1" Text="Search" runat="server" ValidationGroup="SEARCH"
                            AccessKey="s" OnClick="btnSearch_Click">
                            <i class="fa fa-search"></i> Search                     
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="lnkReset" Text="Reset" runat="server" 
                            OnClick="lnkReset_Click">
                             Reset                     
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnExportToExcel" Visible="false" runat="server" CssClass="btn btn-outline-secondary"
                            Text="Export to Excel" OnClick="btnExportToExcel_Click">
                            <i class="fa fa-download"></i> Export to Excel               
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvCircularMaster" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            GridLines="Both" DataKeyNames="CM_ID" OnPageIndexChanging="gvCircularMaster_PageIndexChanging"
                            OnSelectedIndexChanged="gvCircularMaster_SelectedIndexChanged" AllowPaging="true" AllowSorting="true"
                            CssClass="table table-bordered footable" PagerStyle-CssClass="pgr"
                            OnSorting="gvCircularMaster_Sorting" OnRowCreated="OnRowCreated"
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
                                        <asp:Label ID="lblActionableId" runat="server" Text='<%# Bind("CA_ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sr.No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line of Business" SortExpression="LEM_NAME" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOB" runat="server" Text='<%# Eval("LEM_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Circular No." SortExpression="CM_CIRCULAR_NO">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCircNo" runat="server" Text='<%# Eval("CM_CIRCULAR_NO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Circular Date" SortExpression="CM_DATE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCircularDate" runat="server" Text='<%# Eval("CM_DATE", "{0:dd-MMM-yyyy }") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actionable" SortExpression="CA_ACTIONABLE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActionable" Width="300px" runat="server" ToolTip='<%#Eval("CA_ACTIONABLE").ToString()%>'
                                            Text='<%#Eval("CA_ACTIONABLE").ToString().Length > 200 ? (Eval("CA_ACTIONABLE") as string).Substring(0, 200).Replace("\n", "<br />") + " ..." : Eval("CA_ACTIONABLE").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                        <asp:Label ID="lblActionable1" runat="server" Visible="false" Text='<%#Eval("CA_ACTIONABLE").ToString()%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SPOC From Compliance Department">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSpocFromcompliance" runat="server" Text='<%# Eval("SPOCName") %>'></asp:Label>
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
                                        <%--<asp:Label ID="lblRemarks" runat="server" Width="250" Text='<%# Eval("CA_REMARKS").ToString().Replace("\n", "<br />") %>'></asp:Label>--%>
                                        <asp:Label ID="lblRemarks1" Width="300px" runat="server" ToolTip='<%#Eval("CA_REMARKS").ToString()%>'
                                            Text='<%#Eval("CA_REMARKS").ToString().Length > 200 ? (Eval("CA_REMARKS") as string).Substring(0, 200).Replace("\n", "<br />") + " ..." : Eval("CA_REMARKS").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                        <asp:Label ID="lblRemarks" runat="server" Visible="false" Text='<%#Eval("CA_REMARKS").ToString()%>'></asp:Label>
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
                                        <asp:DataList ID="dlCircularActionableFiles" runat="server" DataKeyField="CAF_ID" RepeatColumns="1"
                                            RepeatDirection="Horizontal" DataSource='<%#LoadCircularActionableFileList(Eval("CA_ID"))%>'>
                                            <ItemTemplate>
                                                <asp:Label ID="lblServerFileName" Visible="false" runat="server" Text='<% # Eval("CAF_SERVERFILENAME") %>'></asp:Label>
                                                <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=Circular&downloadFileName=<%#Eval("CAF_SERVERFILENAME")%>','','location=0,status=0,scrollbars=1,width=400,height=200');">
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
</asp:Content>
