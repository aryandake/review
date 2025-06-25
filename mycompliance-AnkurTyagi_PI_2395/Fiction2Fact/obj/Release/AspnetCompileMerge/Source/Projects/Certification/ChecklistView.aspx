<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.Certification_ChecklistView" Title="View Certification Checklist" CodeBehind="ChecklistView.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField runat="server" ID="hfSrc" />
    <asp:HiddenField ID="hfCId" runat="server" />
    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">View Certification Compliance Checklist</h4>
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
                    <asp:Panel ID="pnlDetails" runat="server">
                        <div class="tabular-view">
                            <div class="row g-0">
                                <div class="col-md-3">
                                    <label>Checklist No:</label>
                                </div>
                                <div class="col-md-9">
                                    <label>
                                        <asp:Label ID="lblChecklistNo" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="row g-0">
                                <div class="col-md-3">
                                    <label>Department Name:</label>
                                </div>
                                <div class="col-md-9">
                                    <label>
                                        <asp:Label ID="lblDepartment" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="row g-0">
                                <div class="col-md-3">
                                    <label>Act/Regulation/Circular:</label>
                                </div>
                                <div class="col-md-9">
                                    <label>
                                        <asp:Label ID="lblActRegCird" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="row g-0">
                                <div class="col-md-3">
                                    <label>Reference Circular/Notification/Act:</label>
                                </div>
                                <div class="col-md-9">
                                    <label>
                                        <asp:Label ID="lblReference" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="row g-0">
                                <div class="col-md-3">
                                    <label>Section/Clause:</label>
                                </div>
                                <div class="col-md-9">
                                    <label>
                                        <asp:Label ID="lblTitleofSection" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="row g-0">
                                <div class="col-md-3">
                                    <label>Compliance of/Heading of Compliance checklist:</label>
                                </div>
                                <div class="col-md-9">
                                    <label>
                                        <asp:Label ID="lblCheckpoints" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="row g-0">
                                <div class="col-md-3">
                                    <label>Description:</label>
                                </div>
                                <div class="col-md-9">
                                    <label>
                                        <asp:Label ID="lblParticulars" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="row g-0">
                                <div class="col-md-3">
                                    <label>Consequences of non Compliance:</label>
                                </div>
                                <div class="col-md-9">
                                    <label>
                                        <asp:Label ID="lblPenalty" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="row g-0">
                                <div class="col-md-3">
                                    <label>Frequency:</label>
                                </div>
                                <div class="col-md-9">
                                    <label>
                                        <asp:Label ID="lblFrequency" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="row g-0">
                                <div class="col-md-3">
                                    <label>Forms:</label>
                                </div>
                                <div class="col-md-9">
                                    <label>
                                        <asp:Label ID="lblForms" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="row g-0">
                                <div class="col-md-3">
                                    <label>Active/Inactive:</label>
                                </div>
                                <div class="col-md-9">
                                    <label>
                                        <asp:Label ID="lblActIAct" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="row g-0">
                                <div class="col-md-3">
                                    <label>Effective From:</label>
                                </div>
                                <div class="col-md-9">
                                    <label>
                                        <asp:Label ID="lblEffectiveFrom" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="row g-0">
                                <div class="col-md-3">
                                    <label>Effective To:</label>
                                </div>
                                <div class="col-md-9">
                                    <label>
                                        <asp:Label ID="lblEffectiveTo" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="row g-0 border-bottom">
                                <div class="col-md-3">
                                    <label>Deactivation Remarks:</label>
                                </div>
                                <div class="col-md-9">
                                    <label>
                                        <asp:Label ID="lblRemark" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                        </div>

                    </asp:Panel>
                    <div class="mt-3">
                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" >
                            <i class="fa fa-arrow-left me-2"></i> Back                   
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->
</asp:Content>
