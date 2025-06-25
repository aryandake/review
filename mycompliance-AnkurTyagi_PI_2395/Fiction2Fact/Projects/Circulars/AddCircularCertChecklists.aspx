<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCircularCertChecklists.aspx.cs" Inherits="Fiction2Fact.Projects.Circular.AddCircularCertChecklists" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Language" content="en-us" />
    <meta name="GENERATOR" content="Microsoft FrontPage 6.0" />
    <meta name="ProgId" content="FrontPage.Editor.Document" />
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <title>Circular Certification Checklist</title>
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
                    if (Page_ClientValidate("SaveChecklist")) {
                        if ($("#<%= hfDoubleClick.ClientID%>").val() == "Yes") {
                            alert("You have double clicked the button.\r\nKindly wait for the process to complete.");
                            return false;
                        }
                        else {
                            $("#<%= hfDoubleClick.ClientID%>").val("Yes");
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

        </script>
    </asp:PlaceHolder>
</head>
<body class="d-block">
    <form id="form1" runat="server">
        <asp:HiddenField ID="hfSelectedOperation" runat="server" />
        <asp:HiddenField ID="hfCirId" runat="server" />
        <asp:HiddenField ID="hfCircularCertChecklistId" runat="server" />
        <asp:HiddenField ID="hfDoubleClick" runat="server" />
        <asp:HiddenField ID="hfRACorrespondentEmail" runat="server" />
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
                                    <h4 class="page-title">Add Certification Checklists</h4>
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
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label">Department Name: <span class="text-danger">*</span></label>
                                            <asp:DropDownList CssClass="form-select" ID="ddlDeptName" runat="server"
                                                DataValueField="CSSDM_ID" DataTextField="DeptName">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvDeptName" runat="server" ControlToValidate="ddlDeptName"
                                                ValidationGroup="SaveChecklist" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please select department name."></asp:RequiredFieldValidator>

                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label">Act/Regulation/Circular: <span class="text-danger">*</span> </label>
                                            <asp:DropDownList CssClass="form-select" ID="ddlActRegCirc" runat="server"
                                                DataValueField="CDTM_ID" Enabled="false" DataTextField="CDTM_TYPE_OF_DOC">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvActRegCirc" runat="server" ControlToValidate="ddlActRegCirc"
                                                ValidationGroup="SaveChecklist" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please select Act/Regulation/Circular."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Reference Circular / Notification / Act: (Combination of name  and reference no. of circular) <span class="text-danger">*</span></label>
                                            <F2FControls:F2FTextBox ID="txtReference" runat="server" CssClass="form-control"
                                                TextMode="MultiLine" Enabled="false"
                                                Rows="3" MaxLength="500"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtReference" />
                                            <asp:RequiredFieldValidator ID="rfvReference" runat="server" ControlToValidate="txtReference"
                                                ValidationGroup="SaveChecklist" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please enter Reference Circular / Notification / Act."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label">Section/Clause: <span class="text-danger">*</span> </label>
                                            <F2FControls:F2FTextBox ID="txtTitleofSection" runat="server" CssClass="form-control" TextMode="MultiLine"
                                                Rows="3" MaxLength="500">
                                    </F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtTitleofSection" />
                                            <asp:RequiredFieldValidator ID="rfvTitleofSection" runat="server" ControlToValidate="txtTitleofSection"
                                                ValidationGroup="SaveChecklist" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please enter Section/Clause."></asp:RequiredFieldValidator>

                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label">Particulars: <span class="text-danger">*</span> </label>

                                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtCheckpoints" runat="server"
                                                Rows="3" Columns="60" TextMode="MultiLine"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtCheckpoints" />
                                            <%--<asp:RequiredFieldValidator ID="rfvCheckpoints" runat="server" ControlToValidate="txtCheckpoints"
                                        ValidationGroup="SaveChecklist" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                        ErrorMessage="Please enter Compliance of/Heading of Compliance checklist."></asp:RequiredFieldValidator>--%>
                                            <asp:RequiredFieldValidator ID="rfvCheckpoints" runat="server" ControlToValidate="txtCheckpoints"
                                                ValidationGroup="SaveChecklist" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please enter Particulars."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label">Description: <span class="text-danger">*</span></label>
                                            <F2FControls:F2FTextBox ID="txtParticulars" runat="server" CssClass="form-control" TextMode="MultiLine"
                                                Rows="3">
                                    </F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtParticulars" />
                                            <asp:RequiredFieldValidator ID="rfvParticulars" runat="server" ControlToValidate="txtParticulars"
                                                ValidationGroup="SaveChecklist" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please enter Description."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label">Consequences of non Compliance:</label>

                                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtPenalty" runat="server" TextMode="MultiLine"
                                                Rows="3"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtPenalty" />
                                            <%--<asp:RequiredFieldValidator ID="rfvPenalty" runat="server" ControlToValidate="txtPenalty"
                                                ValidationGroup="SaveChecklist" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please enter Consequences of non Compliance."></asp:RequiredFieldValidator>--%>
                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label">Frequency:</label>
                                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtFrequency" runat="server" MaxLength="250"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtFrequency" />
                                            <%--<asp:RequiredFieldValidator ID="rfvFrequency" runat="server" ControlToValidate="txtFrequency"
                                                ValidationGroup="SaveChecklist" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please enter Frequency."></asp:RequiredFieldValidator>--%>
                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label">Forms:</label>
                                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtForms" runat="server" MaxLength="250"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtForms" />
                                            <%--<asp:RequiredFieldValidator ID="rfvForms" runat="server" ControlToValidate="txtForms"
                                                ValidationGroup="SaveChecklist" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please enter Forms."></asp:RequiredFieldValidator>--%>
                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label">Effective From:</label>
                                            <div class="input-group">
                                                <F2FControls:F2FTextBox Width="325px" CssClass="form-control" ID="txtEffectiveFromDate" runat="server"
                                                    MaxLength="11" Columns="30"></F2FControls:F2FTextBox>
                                                <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl="../../Content/images/legacy/calendar.jpg"
                                                    ID="imgDate" CssClass="custom-calendar-icon" />
                                            </div>
                                            <cc1:CalendarExtender ID="ceDate" runat="server" PopupButtonID="imgDate" TargetControlID="txtEffectiveFromDate"
                                                Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="rfvEffectiveFromDate" runat="server" ControlToValidate="txtEffectiveFromDate"
                                                ValidationGroup="SaveChecklist" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger"
                                                ErrorMessage="Please enter Effective From."></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEffectiveFromDate"
                                                ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                                Display="Dynamic" ValidationGroup="SaveChecklist"></asp:RegularExpressionValidator>

                                        </div>
                                    </div>
                                    <div class="mt-3 text-center">
                                        <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Save & Add New" CssClass="btn btn-outline-success" ValidationGroup="SaveChecklist">
                                        <i class="fa fa-save me-2"></i> Save & Add New                    
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnCancel" runat="server" Text="Close" CssClass="btn btn-outline-danger" OnClientClick="closeWindowRef();">
                                        <i class="fa fa-arrow-left me-2"></i> Close                   
                                        </asp:LinkButton>
                                    </div>
                                </div>
                                <div id="divCertChklist">
                                    <div class="table-responsive mt-3">
                                        <asp:GridView ID="gvCertChecklist" runat="server" AutoGenerateColumns="false" DataKeyNames="CCC_ID"
                                            ShowHeaderWhenEmpty="true" AllowSorting="false" AllowPaging="false" GridLines="Both" CellPadding="4"
                                            OnSelectedIndexChanged="gvCertChecklist_SelectedIndexChanged" CssClass="table table-bordered footable"
                                            OnRowDataBound="gvCertChecklist_RowDataBound" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                            EmptyDataText="No record found...">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Edit" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-success btn-circle"
                                                            CausesValidation="false" OnClientClick="return onClientEditClick();"> 
                                                            <i class="fa fa-pen"></i>	                            
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Select"
                                                            CssClass="btn btn-sm btn-soft-danger btn-circle" OnClientClick="return onClientDeleteClick();">
                                                            <i class="fa fa-trash"></i>
                                            </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DeptName" HeaderText="Department Name" />
                                                <asp:TemplateField HeaderText="Act/Regulation/Circular" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("CDTM_TYPE_OF_DOC").ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reference Circular / Notification / Act" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("CCC_REFERENCE").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                        <asp:HiddenField ID="hfStatus" runat="server" Value='<%# Eval("CCC_STATUS") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Section/Clause" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("CCC_CLAUSE").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="Compliance of/Heading of Compliance checklist" ShowHeader="true">--%>
                                                <asp:TemplateField HeaderText="Particulars" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("CCC_CHECK_POINTS").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("CCC_PARTICULARS").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Consequences of non Compliance" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("CCC_PENALTY").ToString().Replace("\n","<br />") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CCC_FREQUENCY" HeaderText="Frequency" />
                                                <asp:BoundField DataField="CCC_FORMS" HeaderText="Forms" />
                                                <asp:BoundField DataField="CCC_EFFECTIVE_FROM" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Effective From" />
                                            </Columns>
                                        </asp:GridView>

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
</body>
</html>
