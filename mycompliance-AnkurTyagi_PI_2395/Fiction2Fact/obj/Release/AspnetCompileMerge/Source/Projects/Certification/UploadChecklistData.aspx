<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.Certification_UploadChecklistData" ValidateRequest="false" Title="Upload Checklist Data " CodeBehind="UploadChecklistData.aspx.cs" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/commonvalidations.js") %>'>
    </script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/CertificationDataUpload.js")%>'>
    </script>

    <script type="text/javascript">
        //window.onbeforeunload = function () {
        //    var btnRefresh = window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnRefresh');
        //    if (btnRefresh == null) {
        //        btnRefresh = window.opener.document.getElementById('ctl00_ContentPlaceHolder1_fvCircularMaster_btnRefresh');
        //    }

        //    btnRefresh.click();
        //}

        $(document).ready(() => {
            if ($("#<%= hfType.ClientID %>").val() == "CIRC") {
                $("#spnCircNote").css("display", "block");
            }
        });
    </script>

    <asp:HiddenField runat="server" ID="hfName" />
    <asp:HiddenField runat="server" ID="hfEmail" />
    <asp:HiddenField runat="server" ID="hfType" />
    <asp:HiddenField runat="server" ID="hfCircId" />
    <asp:HiddenField runat="server" ID="hfBatchId" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Upload Checklist</h4>
                        <asp:Label ID="lblvalidatedata" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
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
                    <asp:Panel ID="pnlUpload" runat="server">
                        <div class="mb-4">
                            <asp:Label ID="Label1" runat="server" Text="<strong> Note: </strong>"
                                CssClass=""></asp:Label>

                            <div class="input-group">
                                <i class="fa fa-arrow-alt-circle-right me-2"></i>
                                <span>To import a compliance checklist, select the department, click on Browse button,select the file and click on the Validate button. </span>
                            </div>
                            <div class="input-group">
                                <i class="fa fa-arrow-alt-circle-right me-2"></i>
                                <span>Checklist upload file should be in .csv format only.</span>
                            </div>
                        </div>
                        <div class="mt-1">
                             <div class="input-group">
                                 <i class="fa fa-arrow-alt-circle-right me-2"></i>
                                <label class="mb-0"><span>To view the format of the file to be uploaded,</span></label>
                            </div>
                            <div class="input-group mt-2  mx-3">
                                <a href="javascript:void(0);" class="btn btn-info btn-sm" onclick="javascript:window.open('../CommonDownload.aspx?type=CertificationChecklistTemplate&downloadFileName=CertificationChecklistUploadTemplate.csv','','location=0,status=0,scrollbars=0,width=100,height=100,resizable=0')"
                                    id="hrffileformat" runat="server"><i class="fa fa-download"></i>Click here</a>
                                <div>
                                    <span id="spnCircNote" style="color: red; display: none;">&nbsp;&nbsp; Please do not add any data in the "Act/Regulation/Circular" and "Reference Circular/Notification/Act" columns.</span>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-md-12 mb-3">
                                <label class="form-label">Delimiter Type:</label>
                                <F2FControls:F2FTextBox ID="txtDelimiter" CssClass="form-control" runat="server"></F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtDelimiter" />
                            </div>
                            <div class="col-md-12 mb-3">
                                <label class="form-label">
                                    <asp:Label ID="lblAttachments" runat="server" Text="Attachments:"></asp:Label></label>
                                <div class="input-group">
                                    <asp:FileUpload ID="fuCertChklistUpload" runat="server" CssClass="form-control" />
                                    <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnAddAttachment" runat="server" Text="Validate"
                                        OnClick="btnAddAttachment_Click" ValidationGroup="Upload">
                                        <i class="fa fa-check-square"></i> Validate                    
                                    </asp:LinkButton>
                                </div>
                                <asp:RegularExpressionValidator ID="revCertChklistUpload" runat="server"
                                    ControlToValidate="fuCertChklistUpload"
                                    Display="Dynamic" ErrorMessage="Upload of this file format is not supported."
                                    ValidationExpression="^.+(.csv|.CSV|.txt)$"
                                    ValidationGroup="Upload">
                                  </asp:RegularExpressionValidator>

                            </div>
                        </div>
                    </asp:Panel>

                </div>
                <div class="card-body">
                    <asp:Panel ID="pnlGrid" runat="server">
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gvDataUpload" runat="server" AllowPaging="false" AllowSorting="false"
                                    Width="100%" CssClass="table table-bordered footable">
                                </asp:GridView>
                            </div>
                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSave" runat="server" Text="Import" Visible="false"
                                OnClick="btnSave_Click">
                                <i class="fa fa-save me-2"></i> Import                
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->
</asp:Content>
