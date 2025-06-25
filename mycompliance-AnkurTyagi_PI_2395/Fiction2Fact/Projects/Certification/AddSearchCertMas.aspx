<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.Certification_AddSearchCertMas" Title="Certifications Master" CodeBehind="AddSearchCertMas.aspx.cs" ValidateRequest="false" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .ck-editor__editable {
            min-height: 250px;
        }
    </style>
    <script src="<%=Fiction2Fact.Global.site_url("Scripts/ckeditor/ckeditor.js")%>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //ClassicEditor
            //    .create(document.querySelector('#ctl00_ContentPlaceHolder1_fvSearchCertificate_FCKE_EditCertContents'), {
            //        // toolbar: [ 'heading', '|', 'bold', 'italic', 'link' ]
            //    })
            //    .then(editor => {
            //        window.editor = editor;
            //    })
            //    .catch(err => {
            //        console.error(err.stack);
            //    });
            //ClassicEditor
            //    .create(document.querySelector('#ctl00_ContentPlaceHolder1_fvSearchCertificate_FCKE_InsCertContents'), {
            //        // toolbar: [ 'heading', '|', 'bold', 'italic', 'link' ]
            //    })
            //    .then(editor => {
            //        window.editor = editor;
            //    })
            //    .catch(err => {
            //        console.error(err.stack);
            //    });

            //ClassicEditor
            //    .create(document.querySelector('#ctl00_ContentPlaceHolder1_fvSearchCertificate_FCKE_EditCertContents'), {
            //        toolbar: [
            //            'heading', '|',
            //            'bold', 'italic', 'underline', 'strikethrough', '|',
            //            'alignment', '|',
            //            'numberedList', 'bulletedList', '|',
            //            'blockQuote', 'insertTable', 'undo', 'redo'
            //        ],
            //        alignment: {
            //            options: ['left', 'center', 'right', 'justify']
            //        }
            //    })
            //    .then(editor => {
            //        window.editor = editor;
            //    })
            //    .catch(err => {
            //        console.error('CKEditor initialization error:', err);
            //    });


            //ClassicEditor
            //    .create(document.querySelector('#ctl00_ContentPlaceHolder1_fvSearchCertificate_FCKE_InsCertContents'), {
            //        toolbar: [
            //            'heading', '|',
            //            'bold', 'italic', 'underline', 'strikethrough', '|',
            //            'alignment', '|',
            //            'numberedList', 'bulletedList', '|',
            //            'blockQuote', 'insertTable', 'undo', 'redo'
            //        ],
            //        alignment: {
            //            options: ['left', 'center', 'right', 'justify']
            //        }
            //    })
            //    .then(editor => {
            //        window.editor = editor;
            //    })
            //    .catch(err => {
            //        console.error('CKEditor initialization error:', err);
            //    });

        });
    </script>
    <script type="text/javascript">
        function onViewClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "View";
        }
        function onClientEditClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
        }
        function onClientDeleteClick() {
            if (!confirm('Are you sure that you want to delete this record?')) return false;
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Delete";
        }
        /*
        //Added By Milan Yadav on 20-Feb-2016
        //<<
        function onLevelChange()
        {
              var ddlLevel = document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_ddlLevel").value;  
              
              
               if (ddlLevel =="0")                                                                        
            {                                                                                        
               
               document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trSpoc").style.visibility='visible';
               document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trSpoc").style.display='block';
               document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trUnitHead").style.display='none';
               document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trFunctionHead").style.display='none';
    
              }
              else if(ddlLevel =="1") 
              {
               document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trUnitHead").style.visibility='visible';
               document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trUnitHead").style.display='block';
               document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trSpoc").style.display='none';
               document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trFunctionHead").style.display='none';
              }
              else if(ddlLevel =="2") 
              {
               document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trFunctionHead").style.visibility='visible';
               document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trFunctionHead").style.display='block';
               document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trUnitHead").style.display='none';
               document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trSpoc").style.display='none';
              }
        }
        //>>
        */
        function onLevelEditChange() {
            var ddlLevel = document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_ddlEditLevel").value;


            if (ddlLevel == "0") {

                document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trEditSpoc").style.visibility = 'visible';
                //>>Added By Milan Yadav on 05-Apr-2017
                document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trEditSpoc").style.visibility = "";
                //<<
                document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trEditSpoc").style.display = '';
                document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trEditUnitHead").style.display = 'none';
                document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trEditFunctionHead").style.display = 'none';

            }
            else if (ddlLevel == "1") {
                document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trEditUnitHead").style.visibility = 'visible';
                //>>Added By Milan Yadav on 05-Apr-2017
                document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trEditUnitHead").style.visibility = "";
                //<<
                document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trEditUnitHead").style.display = '';
                document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trEditSpoc").style.display = 'none';
                document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trEditFunctionHead").style.display = 'none';
            }
            else if (ddlLevel == "2") {
                document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trEditFunctionHead").style.visibility = 'visible';
                //>>Added By Milan Yadav on 05-Apr-2017
                document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trEditFunctionHead").style.visibility = "";
                //<<
                document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trEditFunctionHead").style.display = '';
                document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trEditUnitHead").style.display = 'none';
                document.getElementById("ctl00_ContentPlaceHolder1_fvSearchCertificate_trEditSpoc").style.display = 'none';
            }
        }

        function checkmultiplesubmit() {
            var IsDoubleClickFlagSet = document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value;
            if (IsDoubleClickFlagSet == 'Yes') {
                alert("You have double clicked on the same button. " +
                    "Please wait till the operation is successfully completed.");
                return false;
            }
            else {
                document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                return true;
            }
        }

    </script>
    <asp:HiddenField runat="server" ID="hfDoubleClickFlag" />
    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField ID="hfSelectedRecord" runat="server" />
    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Certification Content Master</h4>
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
                <asp:MultiView ID="mvMultiView" runat="server">
                    <asp:View ID="vwGrid" runat="server">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Level:</label>
                                    <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlLevelSearch" runat="server">
                                    </f2f:DropdownListNoValidation>
                                    <ajaxToolkit:CascadingDropDown ID="cddLevelSearch" runat="server" TargetControlID="ddlLevelSearch"
                                        PromptText="Select a Level" ServicePath="AJAXDropdownCertification.asmx" ServiceMethod="GetCertLevels"
                                        Category="CertLevel" />
                                    <asp:RequiredFieldValidator ID="rfvSpocDeptName" ValidationGroup="SEARCH" runat="server" CssClass="text-danger"
                                        ControlToValidate="ddlLevelSearch" Display="Dynamic" SetFocusOnError="True">Please select Function Level.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Function Name:</label>

                                    <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlDeptName" runat="server">
                                    </f2f:DropdownListNoValidation>
                                    <ajaxToolkit:CascadingDropDown ID="cddDeptName" runat="server" TargetControlID="ddlDeptName"
                                        ParentControlID="ddlLevelSearch" PromptText="Select Department" Category="SelectedLevel"
                                        ServicePath="AJAXDropdownCertification.asmx" ServiceMethod="GetCertRelevantFuncsBasedOnLevel" />
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">&nbsp;</label>
                                    <div>
                                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnSearch" Text="Search" runat="server" ValidationGroup="SEARCH"
                                            AccessKey="s" OnClick="btnSearch_Click">
                                            <i class="fa fa-search"></i> Search                          
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnAddNewRecord" Text="Add New Record" runat="server"
                                            AccessKey="s" OnClick="btnAddNewRecord_Click">
                                            <i class="fa fa-plus"></i> Add New Record                               
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gvSearchCertificate" runat="server" AutoGenerateColumns="False"
                                    DataKeyNames="CERTM_ID" GridLines="None" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                    CellPadding="4" CssClass="table table-bordered footable" OnSelectedIndexChanged="gvSearchCertificate_SelectedIndexChanged">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr. No." HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Department Name" HeaderStyle-VerticalAlign="Middle"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="200px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("Dept_Name") %>' Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="View" ShowHeader="true" HeaderStyle-VerticalAlign="Middle"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:LinkButton CssClass="btn btn-sm btn-soft-info btn-circle" runat="server" ID="ibView"
                                                        CommandName="Select" OnClientClick="onViewClick()" ToolTip="View Details">
                                                        <i class="fa fa-eye"></i>
                                                    </asp:LinkButton>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit" ShowHeader="true" HeaderStyle-VerticalAlign="Middle"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:LinkButton CssClass="btn btn-sm btn-soft-success btn-circle" runat="server" ID="ibEdit"
                                                        CommandName="Select" OnClientClick="onClientEditClick()" ToolTip="Edit">
                                                        <i class="fa fa-pen"></i>
                                                    </asp:LinkButton>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Delete" ShowHeader="true" HeaderStyle-VerticalAlign="Middle"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl='<%# Fiction2Fact.Global.site_url("Content/images/legacy/delete.png")%>' runat="server" ID="ibDelete" CommandName="Select"
                                    OnClientClick="return onClientDeleteClick()" ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwForm" runat="server">
                        <div class="card-body">
                            <asp:FormView ID="fvSearchCertificate" runat="server" DataKeyNames="CERTM_ID" Width="100%">
                                <ItemTemplate>
                                    <div class="tabular-view">
                                        <div class="row g-0">
                                            <div class="col-md-3">
                                                <label>Certificate Id:</label>
                                            </div>
                                            <div class="col-md-9">
                                                <label>
                                                    <%# Eval("CERTM_ID")%>
                                                </label>
                                            </div>
                                        </div>
                                        <div class="row g-0">
                                            <div class="col-md-3">
                                                <label>Department:</label>
                                            </div>
                                            <div class="col-md-9">
                                                <label>
                                                    <%# Eval("Dept_Name")%>
                                                </label>
                                            </div>
                                        </div>
                                        <div class="row g-0">
                                            <div class="col-md-3">
                                                <label>Created By:</label>
                                            </div>
                                            <div class="col-md-9">
                                                <label>
                                                    <%# Eval("CERTM_CREATE_BY")%>
                                                </label>
                                            </div>
                                        </div>
                                        <div class="row g-0">
                                            <div class="col-md-3">
                                                <label>Created On:</label>
                                            </div>
                                            <div class="col-md-9">
                                                <label>
                                                    <%# Eval("CERTM_CREATE_DT", "{0:dd-MMM-yyyy HH:mm:ss tt}")%>
                                                </label>
                                            </div>
                                        </div>
                                        <div class="row g-0">
                                            <div class="col-md-3">
                                                <label>Last Updated By:</label>
                                            </div>
                                            <div class="col-md-9">
                                                <label>
                                                    <%# Eval("CERTM_LST_UPD_BY")%>
                                                </label>
                                            </div>
                                        </div>
                                        <div class="row g-0 border-bottom">
                                            <div class="col-md-3">
                                                <label>Last Updated On:</label>
                                            </div>
                                            <div class="col-md-9">
                                                <label>
                                                    <%# Eval("CERTM_LST_UPD_DT", "{0:dd-MMM-yyyy HH:mm:ss tt}")%>
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="mt-2">
                                        <div class="card mb-1 mt-1 border">
                                            <div class="card-header py-0 custom-ch-bg-color">
                                                <h6 class="font-weight-bold text-white mtb-5">Content: </h6>
                                            </div>
                                            <div class="card-body mt-1">
                                                <div>
                                                    <%# Eval("CERTM_TEXT")%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="mt-3">
                                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnViewCancel" runat="server" Text="Back"
                                            CausesValidation="false" OnClick="btnViewCancel_Click">
                                                    <i class="fa fa-arrow-left me-2"></i>Back                   
                                        </asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="card-body">
                                        <div class="row">
                                            <div id="tr1" class="col-md-6 mb-3">
                                                <label class="form-label">Level:</label>
                                                <asp:DropDownList CssClass="form-select" ID="ddlEditLevel" onChange="return onLevelEditChange()"
                                                    runat="server" DataValueField="RC_CODE" DataTextField="RC_NAME">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvLevel" ValidationGroup="SubmitGrp" runat="server" CssClass="text-danger"
                                                    ControlToValidate="ddlEditLevel" Display="Dynamic" SetFocusOnError="True">Please select Level.</asp:RequiredFieldValidator>
                                            </div>
                                            <div id="trEditFunctionHead" runat="server" style="display: none" class="col-md-6 mb-3">
                                                <label class="form-label">Department:</label>
                                                <asp:DropDownList CssClass="form-select" ID="ddlEditDeptName" runat="server" DataValueField="CDM_ID"
                                                    DataTextField="CDM_NAME">
                                                </asp:DropDownList>
                                                <%--  <asp:RequiredFieldValidator ID="rfvEditDeptName" ValidationGroup="SubmitGrp" runat="server" CssClass="text-danger"
                                    ControlToValidate="ddlEditDeptName" Display="Dynamic" SetFocusOnError="True">Please select Department.</asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div id="trEditSpoc" runat="server" style="display: none" class="col-md-6 mb-3">
                                                <label class="form-label">Department:</label>
                                                <asp:DropDownList CssClass="form-select" ID="ddlEditSpocDeptName" runat="server"
                                                    DataValueField="CSSDM_ID" DataTextField="CSSDM_NAME">
                                                </asp:DropDownList>
                                                <%-- <asp:RequiredFieldValidator ID="rfvSpocDeptName" ValidationGroup="SubmitGrp" runat="server" CssClass="text-danger"
                                    ControlToValidate="ddlSpocDeptName" Display="Dynamic" SetFocusOnError="True">Please select Department.</asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div id="trEditUnitHead" runat="server" style="display: none" class="col-md-6 mb-3">
                                                <label class="form-label">Department:</label>
                                                <asp:DropDownList CssClass="form-select" ID="ddlEditUnitHead" runat="server" DataValueField="CSDM_ID"
                                                    DataTextField="CSDM_NAME">
                                                </asp:DropDownList>
                                                <%--<asp:RequiredFieldValidator ID="rfvUnitDeptName" ValidationGroup="SubmitGrp" runat="server"
                                    ControlToValidate="ddlUnitHead" Display="Dynamic" SetFocusOnError="True">Please select Department.</asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="col-md-12 mb-3">
                                                <label class="form-label">Content:</label>
                                                <%--<F2FControls:F2FTextBox runat="server" ID="FCKE_EditCertContents" TextMode="MultiLine" CssClass="ckeditor" Height="100px"></F2FControls:F2FTextBox>--%>
                                                <%--<asp:RequiredFieldValidator ID="rfvEditCertContents" ValidationGroup="SubmitGrp" runat="server" CssClass="text-danger"
                                                    ControlToValidate="FCKE_EditCertContents" Display="Dynamic" SetFocusOnError="True">Please enter Content.</asp:RequiredFieldValidator>--%>
                                                <CKEditor:CKEditorControl ID="FCKE_EditCertContents" runat="server" BasePath="~/Content/ckeditors/ckeditorWithoutTrackChange" CssClass="form-control" Height="600px"></CKEditor:CKEditorControl>
                                            </div>
                                        </div>
                                        <div class="table-responsive mt-2">
                                            <table class="table table-bordered footable">
                                                <tr>
                                                    <td class="tabhead" colspan="2" align="left">
                                                        <b>Keywords</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th class="tabhead">~Name</th>
                                                    <th class="tabbody">Username</th>
                                                </tr>
                                                <tr>
                                                    <td class="tabhead">~Department</td>
                                                    <td class="tabbody">Department of User</td>
                                                </tr>
                                                <tr>
                                                    <td class="tabhead">~qtrStartDate</td>
                                                    <td class="tabbody">Start date of Quarter</td>
                                                </tr>
                                                <tr>
                                                    <td class="tabhead">~qtrEndDate</td>
                                                    <td class="tabbody">End date of Quarter</td>
                                                </tr>
                                                <tr>
                                                    <td class="tabhead">~Date</td>
                                                    <td class="tabbody">Current Date</td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="text-center mt-3">
                                        <asp:LinkButton CssClass="btn btn-outline-success" ID="btnUpdate" runat="server" ValidationGroup="SubmitGrp"
                                            Text="Update Details" CausesValidation="true" OnClick="btnUpdate_Click">
                                            <i class="fa fa-save me-2"></i> Update Details
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnEditCancel" runat="server" Text="Cancel"
                                            OnClick="btnEditCancel_Click">
                                            <i class="fa fa-arrow-left me-2"></i> Cancel  
                                        </asp:LinkButton>
                                    </div>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <div class="card-body">
                                        <div class="row">
                                            <div id="tr1" class="col-md-6 mb-3">
                                                <label class="form-label">Level:</label>
                                                <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlLevelSearch" runat="server">
                                                </f2f:DropdownListNoValidation>
                                                <ajaxToolkit:CascadingDropDown ID="cddLevel" runat="server" TargetControlID="ddlLevelSearch"
                                                    PromptText="Select a Level" ServicePath="AJAXDropdownCertification.asmx" ServiceMethod="GetCertLevels"
                                                    Category="CertLevel" />
                                                <asp:RequiredFieldValidator ID="rfvLevel" ValidationGroup="SubmitGrp" runat="server" CssClass="text-danger"
                                                    ControlToValidate="ddlLevelSearch" Display="Dynamic" SetFocusOnError="True">Please select Level.</asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label">Function Name:</label>
                                                <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlDeptName" runat="server">
                                                </f2f:DropdownListNoValidation>
                                                <ajaxToolkit:CascadingDropDown ID="cddDeptName" runat="server" TargetControlID="ddlDeptName"
                                                    ParentControlID="ddlLevelSearch" PromptText="Select Department" Category="SelectedLevel"
                                                    ServicePath="AJAXDropdownCertification.asmx" ServiceMethod="GetCertFuncsBasedOnLevelForContent" />

                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="SubmitGrp" runat="server" CssClass="text-danger"
                                                    ControlToValidate="ddlDeptName" Display="Dynamic" SetFocusOnError="True">Please select Function Name.</asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-12 mb-3">
                                                <label class="form-label">Content:</label>
                                                <%--<F2FControls:F2FTextBox runat="server" ID="FCKE_InsCertContents" TextMode="MultiLine" CssClass="ckeditor"></F2FControls:F2FTextBox>--%>
                                                <%--<asp:RequiredFieldValidator ID="rfvInsCertContents" ValidationGroup="SubmitGrp" runat="server" CssClass="text-danger"
                                                    ControlToValidate="FCKE_InsCertContents" Display="Dynamic" SetFocusOnError="True">Please enter Content.</asp:RequiredFieldValidator>--%>
                                                <CKEditor:CKEditorControl ID="FCKE_InsCertContents" runat="server" BasePath="~/Content/ckeditors/ckeditorWithoutTrackChange" CssClass="form-control" Height="600px"></CKEditor:CKEditorControl>
                                            </div>
                                        </div>
                                        <div class="mt-3">
                                            <div class="table-responsive">
                                                <table class="table table-bordered footable">
                                                    <tr>
                                                        <td class="tabhead" colspan="2" align="center">
                                                            <b>Keywords</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th class="tabhead">~Name</th>
                                                        <th class="tabbody">Username</th>
                                                    </tr>
                                                    <tr>
                                                        <td class="tabhead">~Department</td>
                                                        <td class="tabbody">Department of User</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tabhead">~qtrStartDate</td>
                                                        <td class="tabbody">Start date of Quarter</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tabhead">~qtrEndDate</td>
                                                        <td class="tabbody">End date of Quarter</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tabhead">~Date</td>
                                                        <td class="tabbody">Current Date</td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="text-center mt-3">
                                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnInsSave" runat="server" ValidationGroup="SubmitGrp"
                                                Text="Save" CausesValidation="true" OnClientClick="return checkmultiplesubmit();" OnClick="btnInsSave_Click">
                                                <i class="fa fa-save me-2"></i> Save
                                            </asp:LinkButton>
                                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnInsCancel" runat="server" Text="Cancel"
                                                OnClick="btnInsCancel_Click">
                                                <i class="fa fa-arrow-left me-2"></i> Cancel  
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </InsertItemTemplate>
                            </asp:FormView>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
    <!-- end row -->
</asp:Content>
