<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" Async="true"
    Inherits="Fiction2Fact.Projects.Certification.CertificationQuarterMas" Title="Quarter Mas" CodeBehind="CertificationQuarterMas.aspx.cs" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("/Content/js/legacy/DateValidator.js") %>'></script>

    <script type="text/javascript">

        function compareFromToDates(source, arguments) {
            try {
                var PlannedStarDate = document.getElementById('ctl00_ContentPlaceHolder1_txtQuarterStartDate');
                var PlannedEndDate = document.getElementById('ctl00_ContentPlaceHolder1_txtQuarterEndDate');

                if (compare2Dates(PlannedStarDate, PlannedEndDate) == 0) {
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
        function compareFromToDates1(source, arguments) {
            try {
                var PlannedStarDate = document.getElementById('ctl00_ContentPlaceHolder1_txtSearchQuarterStartDate');
                var PlannedEndDate = document.getElementById('ctl00_ContentPlaceHolder1_txtSearchQuarterEndDate');

                if (compare2Dates(PlannedStarDate, PlannedEndDate) == 0) {
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
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
        }

        function onClientDeleteClick() {
            if (!confirm('Are you sure that you want to delete this record?')) return false;
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Delete";
        }
    </script>

    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField runat="server" ID="hfQuarterId" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Certification Quarter Master</h4>
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
                <asp:MultiView ID="mvQuarterMaster" runat="server">
                    <asp:View runat="server" ID="vwSearch">
                        <asp:Panel runat="server" ID="pnlSearch" DefaultButton="imgSearch">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Quarter From Date</label>
                                        <div class="input-group">
                                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtSearchQuarterStartDate" runat="server" Columns="15"></F2FControls:F2FTextBox>
                                            <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl="/"
                                                ID="ImageButton1" CssClass="custom-calendar-icon" OnClientClick="return false" />
                                        </div>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtSearchQuarterStartDate" span class="text-danger"
                                            ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                            Display="Dynamic" ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                                        <ajaxToolkit:CalendarExtender ID="Calendarextender1" runat="server" PopupButtonID="ImageButton1"
                                            TargetControlID="txtSearchQuarterStartDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Quarter To Date</label>
                                        <div class="input-group">
                                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtSearchQuarterEndDate" runat="server" Columns="15"></F2FControls:F2FTextBox>
                                            <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl="../../Content/images/legacy/calendar.jpg"
                                                ID="ImageButton2" CssClass="custom-calendar-icon" OnClientClick="return false" />
                                        </div>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtSearchQuarterEndDate"
                                            ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" span class="text-danger"
                                            Display="Dynamic" ValidationGroup="SEARCH">*</asp:RegularExpressionValidator>
                                        <ajaxToolkit:CalendarExtender ID="Calendarextender2" runat="server" PopupButtonID="ImageButton2"
                                            TargetControlID="txtSearchQuarterEndDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="compareFromToDates1"
                                            ControlToValidate="txtSearchQuarterEndDate" ErrorMessage="Quarter Start Date should be less than or equal to Quarter End Date." span class="text-danger"
                                            Display="Dynamic" OnServerValidate="cvQuarterEndDate1_ServerValidate" ValidationGroup="SEARCH">Quarter Start Date should be less than or equal to Quarter End Date..</asp:CustomValidator>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Status</label>
                                        <asp:DropDownList CssClass="form-select" ID="ddlSearchStatus" runat="server">
                                            <asp:ListItem Text="Select" Value="">
                                            </asp:ListItem>
                                            <asp:ListItem Text="Active" Value="A">
                                            </asp:ListItem>
                                            <asp:ListItem Text="Inactive" Value="I">
                                            </asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="text-center mt-3">
                                    <asp:LinkButton CssClass="btn btn-outline-primary" ID="imgSearch" Text="Search" OnClick="btnSearch_Click"
                                        runat="server" ValidationGroup="SEARCH" >
                                        <i class="fa fa-search"></i> Search                     
                                    </asp:LinkButton>
                                    <asp:LinkButton CssClass="btn btn-outline-primary" ID="imgAdd" Text="Add" OnClick="btnAdd_Click"
                                        runat="server" >
                                        <i class="fa fa-plus"></i> Add                               
                                    </asp:LinkButton>
                                    <asp:LinkButton CssClass="btn btn-outline-secondary" runat="server" ID="imgExcel" OnClick="btnExportToExcel_Click"
                                        Text="Export To Excel" Visible="false" >
                                        <i class="fa fa-download"></i> Export to Excel               
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gvQuarterMaster" runat="server" AutoGenerateColumns="False" DataKeyNames="CQM_ID"
                                    AllowSorting="false" AllowPaging="true" GridLines="Both" CellPadding="4" CssClass="table table-bordered footable"
                                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnSelectedIndexChanged="gvQuarterMaster_SelectedIndexChanged"
                                    OnPageIndexChanging="gvQuarterMaster_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Sr.No.
                       
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                &nbsp;&nbsp;&nbsp;<asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>'
                                                    runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quarter From Date" SortExpression="CQM_FROM_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuarterFromDate" runat="server" Text='<%# Bind("CQM_FROM_DATE","{0:dd-MMM-yyyy}") %>'
                                                    Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Quarter To Date" SortExpression="CQM_TO_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuarterToDate" runat="server" Text='<%# Bind("CQM_TO_DATE","{0:dd-MMM-yyyy}") %>'
                                                    Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Quarter Due Date" SortExpression="CQM_DUE_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuarterDueDate" runat="server" Text='<%# Bind("CQM_DUE_DATE","{0:dd-MMM-yyyy}") %>'
                                                    Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="Edit" ShowHeader="true">
                                            <ItemTemplate>
                                                <asp:LinkButton CssClass="btn btn-sm btn-soft-success btn-circle" ID="lnkEdit" runat="server" CommandName="Select" OnClientClick="onClientEditClick()">
                                                    <i class="fa fa-pen"></i>	                            
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Delete" ShowHeader="true">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="centerLink" ID="lnkDelete" runat="server" CommandName="Select"
                                Text="&lt;img alt='Delete' src='../../Content/images/legacy/delete.png' border='0' &gt;" OnClientClick=" return onClientDeleteClick()">
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwInsert" runat="server">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Id :</label>
                                    <asp:Label ID="lblID" runat="server" CssClass="form-control custom-span-input"></asp:Label>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Quarter Start Date : <span class="text-danger">*</span></label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox CssClass="form-control" ID="txtQuarterStartDate" runat="server" Columns="20"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl="../../Content/images/legacy/calendar.jpg"
                                            ID="imgQuarterStarDate" CssClass="custom-calendar-icon" OnClientClick="return false" />
                                    </div>
                                    <asp:RegularExpressionValidator ID="recQuarterStarDate" runat="server" ControlToValidate="txtQuarterStartDate"
                                        ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" span class="text-danger"
                                        Display="Dynamic" ValidationGroup="Save"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ceWDateofRcpt2" runat="server" PopupButtonID="imgQuarterStarDate"
                                        TargetControlID="txtQuarterStartDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtQuarterStartDate" span class="text-danger"
                                        Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True">Please enter Quarter Start Date.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Quarter End Date : <span class="text-danger">*</span></label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox CssClass="form-control" ID="txtQuarterEndDate" runat="server" Columns="20"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl="../../Content/images/legacy/calendar.jpg"
                                            ID="imgToDt" CssClass="custom-calendar-icon" OnClientClick="return false" />
                                    </div>
                                    <asp:RegularExpressionValidator ID="revQuarterEndDate" runat="server" ControlToValidate="txtQuarterEndDate"
                                        ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" span class="text-danger"
                                        Display="Dynamic" ValidationGroup="Save">*</asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ceQuarterEndDate" runat="server" PopupButtonID="imgToDt"
                                        TargetControlID="txtQuarterEndDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                    <asp:CustomValidator ID="cvQuarterEndDate" runat="server" ClientValidationFunction="compareFromToDates"
                                        ControlToValidate="txtQuarterEndDate" ErrorMessage="Quarter Start Date should be less than or equal to Quarter End Date." span class="text-danger"
                                        Display="Dynamic" OnServerValidate="cvQuarterEndDate_ServerValidate" ValidationGroup="Save">Quarter Start Date should be less than or equal to Quarter End Date..</asp:CustomValidator>
                                    <asp:RequiredFieldValidator ID="rfvQuarterEndDate" runat="server" ControlToValidate="txtQuarterEndDate" span class="text-danger"
                                        Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True">Please enter Quarter Start Date.</asp:RequiredFieldValidator>

                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Quarter Due Date : <span class="text-danger">*</span></label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox CssClass="form-control" ID="txtQuarterDueDate" runat="server" Columns="20"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl="../../Content/images/legacy/calendar.jpg"
                                            ID="imgDueDt" CssClass="custom-calendar-icon" OnClientClick="return false" />
                                    </div>
                                    <asp:RegularExpressionValidator ID="reQuarterDueDate" runat="server" ControlToValidate="txtQuarterDueDate"
                                        ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                        Display="Dynamic" ValidationGroup="Save">*</asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgDueDt"
                                        TargetControlID="txtQuarterDueDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>

                                    <asp:RequiredFieldValidator ID="rfQuarterDueDate" runat="server" ControlToValidate="txtQuarterDueDate" span class="text-danger"
                                        Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True">Please enter Quarter Due Date.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Status : <span class="text-danger">*</span></label>
                                    <asp:DropDownList CssClass="form-select" ID="ddlStatus" runat="server">
                                        <asp:ListItem Text="Select" Value=""> </asp:ListItem>
                                        <asp:ListItem Text="Active" Value="A"> </asp:ListItem>
                                        <asp:ListItem Text="Inactive" Value="I"> </asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus" span class="text-danger"
                                        Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True">Please select status.</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="text-center mt-3">
                                <asp:LinkButton CssClass="btn btn-outline-success" ID="imgSave" Text="Save" OnClick="btnSave_Click"
                                    runat="server" ValidationGroup="Save">
                                    <i class="fa fa-save me-2"></i> Save                    
                                </asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-outline-danger" ID="imgCancel" Text="Cancel" OnClick="btnCancel_Click"
                                    runat="server">
                                    <i class="fa fa-arrow-left me-2"></i> Cancel                   
                                </asp:LinkButton>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>

            </div>
        </div>
    </div>
    <!-- end row -->

</asp:Content>
