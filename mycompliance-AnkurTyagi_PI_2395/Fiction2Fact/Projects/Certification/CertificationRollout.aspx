<%@ Page Title="Compliance Certificate Roll-out" Language="C#" MasterPageFile="~/Projects/Temp3.master"
    AutoEventWireup="true" Async="true"
    Inherits="Fiction2Fact.Projects.Certification.Certification_AddSeparateChecklist" EnableEventValidation="true" CodeBehind="CertificationRollout.aspx.cs" %>

<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="custom" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <script type="text/javascript">
        $(document).ready(() => {
            $("#<%= cbSelectAll.ClientID %>").change((event) => {
                $("[id*='cbSearchDeptName']").prop('checked', $(event.currentTarget).is(":checked"));
            });

            $("[id*='cbSearchDeptName']").change(() => {
                $("#<%= cbSelectAll.ClientID %>").prop('checked',
                    ($('[id*="cbSearchDeptName"]').length - 1) === $('[id*="cbSearchDeptName"]:checked').length);
            });
        });

        function validateDepartment() {

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

            var atLeast = 1;
            var CHK = document.getElementById("<%=cbSearchDeptName.ClientID%>");
            var checkbox = CHK.getElementsByTagName("input");
            var counter = 0;
            for (var i = 0; i < checkbox.length; i++) {
                if (checkbox[i].checked) {
                    counter++;
                }
            }
            if (atLeast > counter) {
                alert("Please select Department.");
                return false;
            }
            else {
                return true;
            }
        }

    </script>

    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField runat="server" ID="hfQuarter" />
    <asp:HiddenField runat="server" ID="hfDoubleClickFlag" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Compliance Certificate Roll-out</h4>
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
                <asp:MultiView ID="mvSeparateChecklistrMaster" runat="server">
                    <asp:View runat="server" ID="vwSearch">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-12 mb-3">
                                    <label class="form-label">Quarter  :  <span class="text-danger">*</span></label>
                                    <asp:DropDownList CssClass="form-select" ID="ddlActiveQuarter" runat="server"
                                        DataValueField="CQM_ID" DataTextField="Quarter">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvActiveQuarter" ValidationGroup="SaveChecklist" runat="server" CssClass="text-danger"
                                        ControlToValidate="ddlActiveQuarter" Display="Dynamic">Please Select Quarter.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-12 mb-3">
                                    <label class="form-label">Department Name : <span class="text-danger">*</span></label>
                                    <div class="custom-checkbox-table">
                                        <asp:CheckBox ID="cbSelectAll" runat="server" CssClass="form-control mb-3" Text="Select all"></asp:CheckBox>
                                        <asp:CheckBoxList ID="cbSearchDeptName" RepeatColumns="3" CssClass="form-control" AppendDataBoundItems="true"
                                            runat="server" DataValueField="CDM_ID" DataTextField="CDM_NAME">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </div>
                            <div class="text-center mt-3">
                                <asp:Button CssClass="btn btn-outline-success" ID="imgSave" Text="Roll-out" OnClick="btnSave_Click"
                                    runat="server" ValidationGroup="SaveChecklist" OnClientClick="return validateDepartment()" />
                            </div>
                        </div> 
                    </asp:View>
                </asp:MultiView> 
            </div>
        </div>
    </div>
    <!-- end row -->

</asp:Content>

