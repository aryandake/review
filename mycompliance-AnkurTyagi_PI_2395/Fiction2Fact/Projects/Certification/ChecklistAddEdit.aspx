<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.Certification_ChecklistAddEdit" Title="Certifications Checklist" CodeBehind="ChecklistAddEdit.aspx.cs" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="rif" Namespace="Fiction2Fact.Controls" Assembly="RequiredIfValidator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/commonvalidations.js") %>'>
    </script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/dateformatvalidation.js")%>'>
    </script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/CertificationDataUpload.js")%>'>
    </script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js")%>'>
    </script>

    <script type="text/javascript">

        function compareFromToDates(source, arguments) {
            try {
                var PlannedStarDate = document.getElementById('ctl00_ContentPlaceHolder1_txtEffectiveFromDate');
                var PlannedEndDate = document.getElementById('ctl00_ContentPlaceHolder1_txtEffectiveToDate');

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


    </script>


    <center>
        <asp:HiddenField runat="server" ID="hfSrc" />
        <asp:HiddenField runat="server" ID="hfCId" />
        <asp:HiddenField runat="server" ID="hftype" />
    </center>

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">
                            <asp:Label ID="lblHeader" runat="server"></asp:Label></h4>
                        <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
                        <asp:Label runat="server" ID="lblErrorMsg" CssClass="text-danger"></asp:Label>
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
                <asp:Panel ID="pnlChecklistEdit" runat="server">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Department Name: <span class="text-danger">*</span></label>
                                <asp:DropDownList CssClass="form-select" ID="ddlSearchDeptName" runat="server"
                                    DataValueField="CSSDM_ID" DataTextField="DeptName">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Act/Regulation/Circular: <span class="text-danger">*</span></label>
                                <asp:DropDownList CssClass="form-select" ID="ddlActRegCirc" runat="server"
                                    DataValueField="CDTM_ID" DataTextField="CDTM_TYPE_OF_DOC">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Reference Circular / Notification / Act:<span class="text-danger">*</span> </label>
                                <F2FControls:F2FTextBox ID="txtReference" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    Rows="3" MaxLength="500"></F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtReference" />
                                <asp:Label ID="lblNote" runat="server" CssClass="text-danger" Text="Please enter the Cirular No. and Subject of the Circular."></asp:Label>
                                <%--   <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"    
	                    TargetControlID="txtReference" FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers"    
	                    FilterMode="ValidChars" ValidChars="@;:_-.'/\?!, " >    
                    </cc1:FilteredTextBoxExtender>--%>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Section/Clause: <span class="text-danger">*</span></label>
                                <F2FControls:F2FTextBox ID="txtTitleofSection" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    Rows="3" MaxLength="500">
                                </F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtTitleofSection" />
                                <%--     <cc1:FilteredTextBoxExtender ID="txtCharacters_FilteredTextBoxExtender" runat="server" Enabled="True"    
	                    TargetControlID="txtTitleofSection" FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers"    
	                    FilterMode="ValidChars" ValidChars="@;:_-.'/\?!, " >    
                    </cc1:FilteredTextBoxExtender>--%>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Compliance of/Heading of Compliance checklist: <span class="text-danger">*</span></label>
                                <F2FControls:F2FTextBox CssClass="form-control" ID="txtCheckpoints" runat="server"
                                    Rows="3" Columns="60" TextMode="MultiLine"></F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtCheckpoints" />
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Description: <span class="text-danger">*</span></label>
                                <F2FControls:F2FTextBox ID="txtParticulars" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    Rows="3"></F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtParticulars" />
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Consequences of non Compliance:</label>
                                <F2FControls:F2FTextBox CssClass="form-control" ID="txtPenalty" runat="server" TextMode="MultiLine"
                                    Rows="3"></F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtPenalty" />
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Frequency:</label>
                                <F2FControls:F2FTextBox CssClass="form-control" ID="txtFrequency" runat="server" MaxLength="250"></F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtFrequency" />
                                <%--   <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"    
	                    TargetControlID="txtFrequency" FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers"    
	                    FilterMode="ValidChars" ValidChars="@;:_-.'/\?!, " >    
                    </cc1:FilteredTextBoxExtender>--%>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Forms:</label>
                                <F2FControls:F2FTextBox CssClass="form-control" ID="txtForms" runat="server" MaxLength="250"></F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtForms" />
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Status:<span class="text-danger">*</span> </label>
                                <asp:DropDownList ID="ddlActInAct" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Active" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="Inactive" Value="I"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Effective From: <span class="text-danger">*</span></label>
                                <div class="input-group">
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtEffectiveFromDate" runat="server"
                                        MaxLength="11" Columns="30"></F2FControls:F2FTextBox>
                                    <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl="../../Content/images/legacy/calendar.jpg"
                                        ID="imgDate" CssClass="custom-calendar-icon" />
                                </div>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEffectiveFromDate"
                                    ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                    Display="Dynamic" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Effective To :</label>
                                <div class="input-group">
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtEffectiveToDate" runat="server"
                                        MaxLength="11" Columns="30"></F2FControls:F2FTextBox>
                                    <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl="../../Content/images/legacy/calendar.jpg"
                                        ID="imgTDate" CssClass="custom-calendar-icon" />
                                </div>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEffectiveToDate"
                                    ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                    Display="Dynamic" ValidationGroup="Save"></asp:RegularExpressionValidator>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="compareFromToDates"
                                    ControlToValidate="txtEffectiveToDate" ErrorMessage="Effective From Date should be less than or equal to Effective To Date."
                                    Display="Dynamic" OnServerValidate="cvEffectiveEndDate_ServerValidate" ValidationGroup="Save">Effective From should be less than or equal to Effective To Date.</asp:CustomValidator>

                            </div>
                            <div class="col-md-12 mb-3 text-danger">Note : Deactivation Remarks is only required when status is selected as 'Inactive'.</div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Deactivation Remarks: </label>
                                <F2FControls:F2FTextBox CssClass="form-control" ID="txtRemarks" runat="server" TextMode="MultiLine"
                                    Columns="60"></F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                            </div>
                        </div>
                        <div class="text-center mt-3">
                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                                OnClientClick="return validateonsavedata();">
                                <i class="fa fa-save me-2"></i> Submit                    
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack" runat="server" Text="Back" CausesValidation="false"
                                OnClick="btnBack_Click">
                                <i class="fa fa-arrow-left me-2"></i> Back                  
                            </asp:LinkButton>
                        </div>
                        <cc1:CalendarExtender ID="ceDate" runat="server" PopupButtonID="imgDate" TargetControlID="txtEffectiveFromDate"
                            Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                        <cc1:CalendarExtender ID="ceTDate" runat="server" PopupButtonID="imgTDate" TargetControlID="txtEffectiveToDate"
                            Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                    </div>

                </asp:Panel>
                <asp:Panel ID="pnlAdd" runat="server" Visible="false">
                    <div class="card-body">
                        <div class="text-center mt-3">
                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnAdd" runat="server" Text="Submit" OnClick="btnAdd_Click">
                                <i class="fa fa-plus me-2"></i> Add more                    
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="LinkButton2" runat="server" Text="Back" CausesValidation="false"
                                OnClick="btnBack_Click">
                                <i class="fa fa-arrow-left me-2"></i> Back                  
                            </asp:LinkButton>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <!-- end row -->


</asp:Content>
