<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    ValidateRequest="false" Inherits="Fiction2Fact.Projects.Certification.Certification_CertificationDashboard" Title="Certification Dashboard " CodeBehind="CertificationDashboard.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        function hideData() {
            document.getElementById('tdhDepartmentType').style.visibility = "hidden";
            document.getElementById('tdhDepartmentType').style.display = "none";
            document.getElementById('tdbDepartmentType').style.visibility = "hidden";
            document.getElementById('tdbDepartmentType').style.display = "none";

            document.getElementById('tdhQuarter').style.visibility = "hidden";
            document.getElementById('tdhQuarter').style.display = "none";
            document.getElementById('tdbQuarter').style.visibility = "hidden";
            document.getElementById('tdbQuarter').style.display = "none";

            return false;
        }

        function showReportParam() {
            try {
                var ddlIdobj = document.getElementById('ctl00_ContentPlaceHolder1_ddlType');

                var reportType = ddlIdobj.options[ddlIdobj.selectedIndex].value;
                if (reportType == 2) {
                    hideData();

                    document.getElementById('tdhDepartmentType').style.visibility = "visible";
                    document.getElementById('tdhDepartmentType').style.display = "block";
                    document.getElementById('tdbDepartmentType').style.visibility = "visible";
                    document.getElementById('tdbDepartmentType').style.display = "block";

                    document.getElementById('spMandDept').style.visibility = "visible";

                    document.getElementById('tdhQuarter').style.visibility = "visible";
                    document.getElementById('tdhQuarter').style.display = "block";
                    document.getElementById('tdbQuarter').style.visibility = "visible";
                    document.getElementById('tdbQuarter').style.display = "block";

                    document.getElementById('spMandQt').style.visibility = "hidden";

                    return false;
                }
                else if (reportType == 1) {
                    hideData();
                    document.getElementById('tdhQuarter').style.visibility = "visible";
                    document.getElementById('tdhQuarter').style.display = "block";
                    document.getElementById('tdbQuarter').style.visibility = "visible";
                    document.getElementById('tdbQuarter').style.display = "block";

                    document.getElementById('spMandQt').style.visibility = "visible";

                    return false;
                }
                else {
                    document.getElementById('spMandDept').style.visibility = "hidden";
                    document.getElementById('spMandQt').style.visibility = "hidden";
                }
            }
            catch (e) {
                alert(e);
            }
        }

        function validateParams(ddlId, DepartmentType, Quarter, IssueCriticalityId, FromDtId, ToDtId) {
            try {
                var msg = '';
                var ddlIdobj = document.getElementById(ddlId);
                var ddlType = ddlIdobj.options[ddlIdobj.selectedIndex].value;
                if (ddlType == '') {
                    alert('Please Select Report Type.');
                    return false;
                }

                if (ddlType == 2) {
                    var DepartmentType = document.getElementById(DepartmentType).value;
                    if ((DepartmentType == '')) {
                        msg = msg + " Please select Department Type.\n";
                    }
                }
                else if (ddlType == 1) {
                    var Quarter = document.getElementById(Quarter).value;
                    if ((Quarter == '')) {
                        msg = msg + " Please select Quarter.\n";
                    }
                }

                if (msg == '')
                    return true;
                else {
                    alert(msg);
                    return false;
                }
            }
            catch (e1) {
                alert(e1);
            }
        }

    </script>

    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField runat="server" ID="hfContent" />
    <asp:HiddenField runat="server" ID="hfTabberId" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Certification Dashboard</h4>
                        <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
                        <asp:Label ID="lblInfo" runat="server" CssClass="custom-alert-box"></asp:Label>
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
                            <label class="form-label">Report Type : <span style="color: red; visibility: hidden">*</span></label>
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Select" Value="">
                        </asp:ListItem>
                                <asp:ListItem Text="Status wise reports" Value="1">
                        </asp:ListItem>
                                <asp:ListItem Text="Compliance status count wise reports" Value="2">
                        </asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label" id="tdhDepartmentType">Department Type: <span id="spMandDept" style="color: red; visibility: hidden">*</span></label>
                            <div id="tdbDepartmentType">
                                <asp:DropDownList ID="ddlDeptType" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Select" Value=""> </asp:ListItem>
                                    <asp:ListItem Text="Function" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Unit" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Sub-Unit" Value="3"> </asp:ListItem>
                                </asp:DropDownList>
                                <%-- <asp:RequiredFieldValidator ID="rfvDeptType" ValidationGroup="SEARCH" runat="server" CssClass="span"
                        ControlToValidate="ddlDeptType" Display="Dynamic" SetFocusOnError="True">*</asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label" id="tdhQuarter">Quarter: <span id="spMandQt" style="color: red; visibility: hidden">*</span></label>
                            <div class="tabbody3" id="tdbQuarter">
                                <asp:DropDownList CssClass="form-select" ID="ddlQuarter" runat="server" DataValueField="CQM_ID"
                                    DataTextField="Quarter">
                                </asp:DropDownList>
                            </div>
                        </div>

                    </div>
                    <div class="text-center mt-3">
                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnSearch" Text="Search" runat="server"
                            AccessKey="s" OnClick="btnSearch_Click">
                                 <i class="fa fa-search"></i> Search                     
                             </asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcel" Text="Export To Excel" runat="server"
                            OnClick="btnExportToExcel_Click">
                                    <i class="fa fa-download"></i> Export to Excel               
                                </asp:LinkButton>
                    </div>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:Literal runat="server" ID="litSummary"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->

</asp:Content>
