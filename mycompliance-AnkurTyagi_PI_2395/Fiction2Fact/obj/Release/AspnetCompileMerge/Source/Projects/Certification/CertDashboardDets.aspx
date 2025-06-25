<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.Certification_CertDashboardDets" Title="Certification Dashboard " CodeBehind="CertDashboardDets.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />
    <title>Report Details</title>
</head>
<body>
    <%--style="background-color: #f4f4f4;"--%>
    <form id="form1" runat="server">


        <div class="page-content">
            <div class="container-fluid">
                <!-- Page-Title -->
                <div class="row">
                    <div class="col-sm-12">
                        <div class="page-title-box">
                            <div class="row">
                                <div class="col">
                                    <h4 class="page-title">Certification Dashboard Details</h4>
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
                                <div class="mb-3">
                                    <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcel" Text="Export To Excel" runat="server"
                                        OnClick="btnExportToExcel_Click">
                                        <i class="fa fa-download"></i> Export to Excel               
                                    </asp:LinkButton>
                                </div>
                                <div class="table-responsive">
                                    <asp:Literal ID="litDetails" runat="server"></asp:Literal>
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
