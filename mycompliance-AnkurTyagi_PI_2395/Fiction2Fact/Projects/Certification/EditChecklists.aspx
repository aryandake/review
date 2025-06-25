<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.Certification_EditChecklists" Title="Edit Checklist" CodeBehind="EditChecklists.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="a" Namespace="Fiction2Fact.Controls" Assembly="RequiredIfValidatorRadioButtonList" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href='<%= Fiction2Fact.Global.site_url("Content/js/legacy/jquery-ui.css") %>' rel="stylesheet" />

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js") %>'></script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/dateformatvalidation.js")%>'></script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/Exception.js")%>'>
    </script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/tabcontrol.js")%>'>
    </script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator1.js")%>'>
    </script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/commonvalidations.js") %>'>
    </script>
    
    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/jquery-ui.min.js")%>'></script>

    <script type="text/javascript">
        function onClientViewCircClick(CMId) {
            window.open('../Circulars/ViewCircularDetails.aspx?CircularId=' + CMId, '', 'location=0,status=0,resizable=1,scrollbars=1,width=1000,height=800');
            return false;
        }

        function compareTargetDtSystemDates(source, arguments) {
            try {
                var cnt = 0;
                var TargetDate;
                var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurrentDate');
                //alert('SystemDate: '+SystemDate.value);

                var grid = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist");
                if (grid != null) {
                    if (grid.rows.length > 0) {
                        for (var j = 2; j < grid.rows.length + 1; j++) {
                            cnt++;
                            if (j < 10) {
                                j = "0" + j;
                            } else {
                                j = j;
                            }
                            TargetDate = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_txtTargetDate");

                            if (TargetDate != null) {
                                //alert(TargetDate.value);
                                //alert(compare2Dates(SystemDate,TargetDate));
                                if (compare2Dates(SystemDate, TargetDate) == 0) {
                                    //alert("False");
                                    arguments.IsValid = false;
                                    break;
                                }
                                else {
                                    //alert("True");
                                    arguments.IsValid = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (e) {
                alert(e);
                arguments.IsValid = true;
            }
        }

        <%--<<Added by Ankur Tyagi on 05Feb2024 for CR_1948--%>
        function compareNCSinceDtSystemDates(source, arguments) {
            try {
                var cnt = 0;
                var TargetDate;
                var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurrentDate');
                //alert('SystemDate: '+SystemDate.value);

                var grid = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist");
                if (grid != null) {
                    if (grid.rows.length > 0) {
                        for (var j = 2; j < grid.rows.length + 1; j++) {
                            cnt++;
                            if (j < 10) {
                                j = "0" + j;
                            } else {
                                j = j;
                            }
                            NCSince = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklist_ctl" + j + "_txtNCSinceDate");

                            if (NCSince != null) {
                                //alert(TargetDate.value);
                                //alert(compare2Dates(SystemDate,TargetDate));
                                if (compare2Dates(NCSince, SystemDate) == 0) {
                                    //alert("False");
                                    arguments.IsValid = false;
                                    break;
                                }
                                else {
                                    //alert("True");
                                    arguments.IsValid = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (e) {
                alert(e);
                arguments.IsValid = true;
            }
        }
        //>>

    </script>
    <script type="text/javascript">
        //Added by Milan yadav on 09-Aug-2016
        //>>
        function openViewChecklistPopup(requestId) {

            //alert(requestId);
            window.open('../Certification/ViewChecklistData.aspx?ChecklistId=' + requestId,
                '', 'location=0,status=0,scrollbars=1,resizable=1,width=700,height=500');
            return false;
        }
        //<<
    </script>
    <script>
        var $j = jQuery.noConflict();
        $j(document).ready(function () {
            $j('.targetDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd-M-yy'
            });
        });
    </script>

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Edit Compliance Checklist</h4>
                        <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>

                        <asp:HiddenField ID="hfCertId" runat="server" />
                        <asp:HiddenField ID="hfExceptions" runat="server" />
                        <asp:HiddenField ID="hfQuarterEndDt" runat="server" />
                        <asp:HiddenField ID="hfQuarterId" runat="server" />
                        <asp:HiddenField ID="hfCertDepartment" runat="server" />
                        <asp:HiddenField ID="hfQuarter" runat="server" />
                        <asp:HiddenField ID="hiddenFormData" runat="server" />
                        <asp:HiddenField ID="hfCurrentDate" runat="server" />
                        <asp:HiddenField ID="hfType" runat="server" />
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
                    <div class="mb-3">
                        <asp:LinkButton ID="btnChklistSubmit" runat="server" Text="Save" OnClick="btnSubmit_click"
                            CssClass="btn btn-outline-success" ValidationGroup="Submit" OnClientClick="return getExceptionDetailsOnSaveFH();">
    <i class="fa fa-save me-2"></i> Save                                              
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnChklistBack" runat="server" Text="Back"
                            OnClick="btnClose_click">
    <i class="fa fa-arrow-left me-2"></i> Back                                
                        </asp:LinkButton>
                    </div>
                    <!-- Nav tabs -->
                    <ul class="nav nav-tabs" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link active" data-bs-toggle="tab" href="#home" role="tab" aria-selected="true">Certification Details</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-bs-toggle="tab" href="#profile" role="tab" aria-selected="false">Compliance Deviations</a>
                        </li>
                    </ul>

                    <!-- Tab panes -->
                    <div class="tab-content">
                        <div class="tab-pane p-3 active" id="home" role="tabpanel">
                            <div>
                                <div>
                                    <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcel" runat="server" Text="Export To Excel"
                                        OnClick="btnExportToExcel_Click">
                                        <i class="fa fa-download"></i> Export to Excel               
                                    </asp:LinkButton>
                                </div>
                                <asp:Panel ID="pnlChecklistShow" runat="server">
                                    <div class="mt-3">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvChecklist" runat="server" BorderStyle="None" BorderWidth="1px"
                                                CellPadding="4" GridLines="None" AutoGenerateColumns="false" CssClass="table table-bordered footable"
                                                DataKeyNames="CCM_ID" OnRowDataBound="gvChecklist_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Sr. No.
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblChecklistDetsId" runat="server" Text='<%#Eval("CCD_ID") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="View Circulars">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:LinkButton ID="lnkViewCirc" runat="server" CssClass="btn btn-sm btn-soft-info btn-circle"
                                                                    CommandName="Select">
                                                                    <%--OnClientClick='<%# string.Format("onClientViewCircClick(\"{0}\");", Eval("CCM_CM_ID")) %>'--%>
                                                                    <i class="fa fa-eye"></i>
                                                                </asp:LinkButton>
                                                                <asp:HiddenField ID="hfCircularId" runat="server" Value='<%#Eval("CCM_CM_ID") %>' />
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Act/Regulation/Circular">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblActRegCirc" Text='<%#Eval("CDTM_TYPE_OF_DOC") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reference Circular/Notification/Act">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtRegulations" Text='<%#Eval("CCM_REFERENCE").ToString().Length>100?(Eval("CCM_REFERENCE") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_REFERENCE").ToString().Replace("\n", "<br />")  %>'
                                                                ToolTip='<%#Eval("CCM_REFERENCE")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Section/Clause">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtntxtSections" CssClass="badge rounded-pill badge-soft-pink" runat="server" Text='<%#Eval("CCM_CLAUSE").ToString().Length>100?(Eval("CCM_CLAUSE") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_CLAUSE").ToString().Replace("\n", "<br />")  %>'
                                                                ToolTip='<%#Eval("CCM_CLAUSE")%>' Font-Underline="false" OnClientClick='<%# string.Format("return openViewChecklistPopup(\"{0}\");", Eval("CCD_ID")) %>'> </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Compliance of/Heading of Compliance checklist">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtCheckpoints" Text='<%#Eval("CCM_CHECK_POINTS").ToString().Length>100?(Eval("CCM_CHECK_POINTS") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_CHECK_POINTS").ToString().Replace("\n", "<br />")  %>'
                                                                ToolTip='<%#Eval("CCM_CHECK_POINTS")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Description">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtSelfAssessmentStatus" Text='<%#Eval("CCM_PARTICULARS").ToString().Length>100?(Eval("CCM_PARTICULARS") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_PARTICULARS").ToString().Replace("\n", "<br />")  %>'
                                                                ToolTip='<%#Eval("CCM_PARTICULARS")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Consequences of non Compliance">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPenalty"
                                                                Text='<%#Eval("CCM_PENALTY").ToString().Length>100?(Eval("CCM_PENALTY") as string).Substring(0,100).Replace("\n", "<br />") + " ...":Eval("CCM_PENALTY").ToString().Replace("\n", "<br />")  %>'
                                                                ToolTip='<%#Eval("CCM_PENALTY")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Frequency" DataField="CCM_FREQUENCY" />
                                                    <asp:BoundField HeaderText="Forms" DataField="CCM_FORMS" HeaderStyle-Width="200px" />
                                                    <asp:TemplateField HeaderText="Compliance Status">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="rbyesnona" runat="server" CssClass="form-select"
                                                                Width="210" DataTextField="RC_NAME" DataValueField="RC_CODE">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="lblaction" runat="server" Text='<%#Eval("CCD_YES_NO_NA") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="Remarks">--%>
                                                    <asp:TemplateField HeaderText="Reason of non compliance">
                                                        <ItemTemplate>
                                                            <F2FControls:F2FTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control"
                                                                Width="150px" Text='<%#Eval ("CCD_REMARKS") %>'></F2FControls:F2FTextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<<Added by Ankur Tyagi on 05Feb2024 for CR_1948--%>
                                                    <asp:TemplateField HeaderText="Non-compliant since">
                                                        <ItemTemplate>
                                                            <div style="width: 160px;">
                                                                <div class="input-group">
                                                                    <F2FControls:F2FTextBox ID="txtNCSinceDate" runat="server" CssClass="form-control" MaxLength="11"
                                                                        Columns="8" Text='<%#Bind("CCD_NC_SINCE_DT","{0:dd-MMM-yyyy}") %>'></F2FControls:F2FTextBox>
                                                                    <asp:ImageButton ToolTip="PopUp Calendar" CssClass="custom-calendar-icon" runat="server" ImageUrl="~/Content/images/legacy/calendar.jpg"
                                                                        ID="imgNCSince" OnClientClick="return false" />
                                                                </div>
                                                            </div>
                                                            <cc1:CalendarExtender ID="ceNCSinceDt" runat="server" PopupButtonID="imgNCSince"
                                                                TargetControlID="txtNCSinceDate" Format="dd-MMM-yyyy"></cc1:CalendarExtender>

                                                            <asp:RegularExpressionValidator ID="revNCSinceDate" runat="server" ControlToValidate="txtNCSinceDate" CssClass="text-danger"
                                                                ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                                                ValidationGroup="Save" Display="Dynamic">Date Format has to be dd-MMM-yyyy.</asp:RegularExpressionValidator>

                                                            <asp:CustomValidator ID="cvNCSinceDt" runat="server" ClientValidationFunction="compareNCSinceDtSystemDates" CssClass="text-danger"
                                                                ControlToValidate="txtNCSinceDate" ErrorMessage="Non Compliant Date should be less than or equal to System Date."
                                                                Display="Dynamic" OnServerValidate="cvTargetDt_ServerValidate" ValidationGroup="Save">Non-compliant since Date should be less than or equal to System Date.</asp:CustomValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%-->>--%>
                                                    <%--<< Added by Amarjeet on 28-Jul-2021--%>

                                                    <asp:TemplateField HeaderText="Action Plan">
                                                        <ItemTemplate>
                                                            <F2FControls:F2FTextBox ID="txtActionPlan" runat="server" TextMode="MultiLine" Rows="5" Columns="20" Width="150px"
                                                                CssClass="form-control" Text='<%#Eval("CCD_ACTION_PLAN") %>'></F2FControls:F2FTextBox>
                                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtActionPlan" />
                                                            <asp:RegularExpressionValidator ID="revActionPlan" ControlToValidate="txtActionPlan" CssClass="text-danger"
                                                                Display="Dynamic" Text="Exceeding 4000 characters" ValidationExpression="^[\s\S]{0,4000}$"
                                                                runat="server" SetFocusOnError="True" ValidationGroup="Save" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Target Date">
                                                        <ItemTemplate>
                                                            <div class="input-group" style="width: 160px;">
                                                                <F2FControls:F2FTextBox ID="txtTargetDate" runat="server" CssClass="form-control" MaxLength="11"
                                                                    Text='<%#Bind("CCD_TARGET_DATE","{0:dd-MMM-yyyy}") %>'></F2FControls:F2FTextBox>
                                                                <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl="~/Content/images/legacy/calendar.jpg"
                                                                    ID="ImageButton2" CssClass="custom-calendar-icon" OnClientClick="return false" />
                                                                <cc1:CalendarExtender ID="ceTargetDt" runat="server" PopupButtonID="ImageButton2"
                                                                    TargetControlID="txtTargetDate" Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                                                            </div>
                                                            <asp:RegularExpressionValidator ID="revTargetDate" runat="server" ControlToValidate="txtTargetDate" CssClass="text-danger"
                                                                ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                                                ValidationGroup="Save" Display="Dynamic">Date Format has to be dd-MMM-yyyy.</asp:RegularExpressionValidator>

                                                            <asp:CustomValidator ID="cvTargetDt" runat="server" ClientValidationFunction="compareTargetDtSystemDates" CssClass="text-danger"
                                                                ControlToValidate="txtTargetDate" ErrorMessage="Target Date should be greater than or equal to System Date."
                                                                Display="Dynamic" OnServerValidate="cvTargetDt_ServerValidate" ValidationGroup="Save">Target Date should be greater than or equal to System Date.</asp:CustomValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%-->>--%>
                                                    <asp:TemplateField HeaderText="Checklist File">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="ClientFileName" runat="server" Value='<%#Eval("CCD_CLIENT_FILENAME") %>' />
                                                            <asp:HiddenField ID="ServerFileName" runat="server" Value='<%#Eval("CCD_SERVER_FILENAME") %>' />
                                                            <asp:DataList ID="dlChecklistFiles" BackColor="White" runat="server" CellPadding="1"
                                                                EnableViewState="True" RepeatColumns="1" RepeatDirection="Horizontal" DataSource='<%# LoadChecklistFile(Eval("CCD_ID")) %>'>
                                                                <ItemTemplate>
                                                                    <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownloadAnyFile.aspx?type=ChecklistFilesFolder&downloadFileName=<%#Eval("CCD_SERVER_FILENAME")%>&fileName=<%#Eval("CCD_CLIENT_FILENAME")%>','','location=0,status=0,scrollbars=0,width=400,height=200');">
                                                                        <%#Eval("CCD_CLIENT_FILENAME")%>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="tab-pane p-3" id="profile" role="tabpanel">
                            <div class="mt-1">
                                <div class="card mb-1 mt-1 border">

                                    <div class="card-body mt-1">
                                        <div class="mt-2 mb-3">
                                            <asp:LinkButton runat="server" ID="imgAdd" CssClass="btn btn-sm btn-soft-info btn-circle" OnClientClick="return addExceptionRow();">
                                                <i class="fa fa-plus"></i>	                            
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="imgDelete" CssClass="btn btn-sm btn-soft-danger btn-circle" OnClientClick="return deleteExceptionRow();">
                                                <i class="fa fa-trash"></i>	                            
                                            </asp:LinkButton>
                                        </div>
                                        <div class="table-responsive">
                                            <asp:Literal ID="litControls" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="tblRejctionRemarks" runat="server" class="mt-3">
                                <div class="row">
                                    <div class="col-md-12 mb-3">
                                        <label class="form-label">Rejection Remarks, if any:</label>
                                        <asp:Label runat="server" ID="lblRejectionRemarks" CssClass="form-control custom-span-input"></asp:Label>
                                    </div>
                                    <div class="col-md-12 mb-3">
                                        <label class="form-label">Remarks, if any:</label>
                                        <F2FControls:F2FTextBox CssClass="form-control" ID="txtRemarks" runat="server" TextMode="MultiLine"
                                            Rows="5" Columns="120" Width="100%"></F2FControls:F2FTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="mt-3">
                        <asp:LinkButton ID="btnSubmit" runat="server" OnClientClick="return getExceptionDetailsOnSaveFH();"
                            Text="Save" OnClick="btnSubmit_click" CssClass="btn btn-outline-success" ValidationGroup="Submit">
                            <i class="fa fa-save me-2"></i> Save                    
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnClose" runat="server" Text="Back" OnClick="btnClose_click">
                            <i class="fa fa-arrow-left me-2"></i> Back                   
                        </asp:LinkButton>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <!-- end row -->

</asp:Content>
