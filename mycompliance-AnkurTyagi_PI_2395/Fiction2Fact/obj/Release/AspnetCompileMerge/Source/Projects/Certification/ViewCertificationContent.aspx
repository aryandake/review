<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.Certification_ViewCertificationContent" CodeBehind="ViewCertificationContent.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Certification</title>
    <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />
</head>

<script type="text/javascript">

    function closeFileWindow() {
        window.close();
    }

</script>

<body class="d-block">
    <form id="form1" runat="server">
        <asp:HiddenField ID="hfDeptId" runat="server" />
        <asp:HiddenField ID="hfType" runat="server" />
        <asp:HiddenField ID="hfQuarterId" runat="server" />

        <div class="page-content">
            <div class="container-fluid">
                <!-- Page-Title -->
                <div class="row">
                    <div class="col-sm-12">
                        <div class="page-title-box">
                            <div class="row">
                                <div class="col">
                                    <h4 class="page-title">Certification Details</h4>
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
                                <div class="tabular-view">
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Department:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label runat="server" ID="lblDeptName"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Quarter From Date:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label runat="server" ID="lblFromDt"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Quarter To Date:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label runat="server" ID="lblToDt"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Approved By:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label runat="server" ID="lblApprovedby"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0 border-bottom">
                                        <div class="col-md-3">
                                            <label>Approved On:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label runat="server" ID="lblApprovedOn"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="mt-2">
                                    <div class="card mb-1 mt-1 border">
                                        <div class="card-header py-0 custom-ch-bg-color">
                                            <h6 class="font-weight-bold text-white mtb-5">Certificate: </h6>
                                        </div>
                                        <div class="card-body mt-1">
                                            <asp:Label runat="server" ID="lblCertificate"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="mt-3">
                                    <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack1" runat="server" Text="Close" OnClientClick="closeFileWindow()">
                                <i class="fa fa-arrow-left me-2"></i> Close                    
                                    </asp:LinkButton>
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
