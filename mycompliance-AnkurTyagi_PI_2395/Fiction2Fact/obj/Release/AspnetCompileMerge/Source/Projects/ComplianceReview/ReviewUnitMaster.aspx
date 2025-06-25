<%@ Page Title="" Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" CodeBehind="ReviewUnitMaster.aspx.cs" Inherits="Fiction2Fact.Projects.Compliance.ReviewUnitMaster" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/legacy/populateUserDetsAJAX.js") %>'>
    </script>

    <script type="text/javascript">

        function onClientEditClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
        }

        function onClientDeleteClick() {
            if (!confirm('Are you sure that you want to delete this record?')) {
                return false;
            }
            else {
                document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Delete";
                return true;
            }
        }

        function onClientSaveClick() {
            var IsDoubleClickFlagSet = document.getElementById('ctl00_ContentPlaceHolder1_hfDoubleClickFlag').value;

            if (IsDoubleClickFlagSet == 'Yes') {
                alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                return false;
            }
            else {
                var validated = Page_ClientValidate('AddReviewerMas');
                if (validated) {
                    document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                    return true;
                }
                else {
                    return false;
                }
            }
        }
    </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField runat="server" ID="hfDoubleClickFlag" />
    <asp:HiddenField runat="server" ID="hfId" />
    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Unit Master</h4>
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
                <asp:MultiView ID="mvReviewUnitMaster" runat="server">
                    <asp:View runat="server" ID="vwSearch">
                        <asp:Panel runat="server" ID="pnlSearch" DefaultButton="imgSearch">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Unit Name</label>
                                        <asp:TextBox CssClass="form-control" ID="txtSearchUnitName" MaxLength="50" runat="server"></asp:TextBox>
                                        <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSearchUnitName" />
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Unit Code</label>
                                        <asp:TextBox CssClass="form-control" ID="txtSearchUnitCode" MaxLength="50" runat="server"></asp:TextBox>
                                        <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSearchUnitCode" />
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Status</label>
                                        <asp:DropDownList CssClass="form-select" ID="ddlSearchStatus" runat="server">
                                            <asp:ListItem Text="Select" Value="">
                                            </asp:ListItem>
                                            <asp:ListItem Text="Active" Value="A">
                                            </asp:ListItem>
                                            <asp:ListItem Text="Inactive" Value="I">
                                            </asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="text-center mt-3">
                                    <asp:LinkButton CssClass="btn btn-outline-primary" ID="imgSearch" Text="Search" OnClick="btnSearch_Click"
                                        runat="server" ValidationGroup="SEARCH" >
                                        <i class="fa fa-search"></i> Search
                                    </asp:LinkButton>
                                    <asp:LinkButton CssClass="btn btn-outline-primary" ID="imgAdd" Text="Add" OnClick="btnAdd_Click"
                                        runat="server" >
                                        <i class="fa fa-plus"></i> Add                               
                                    </asp:LinkButton>
                                    <asp:LinkButton CssClass="btn btn-outline-secondary" runat="server" ID="imgExcel" OnClick="btnExportToExcel_Click"
                                        Text="Export To Excel" Visible="false" >
                                        <i class="fa fa-download"></i> Export to Excel 
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gvReviewerMas" runat="server" AutoGenerateColumns="False" DataKeyNames="CSFM_ID"
                                    AllowSorting="false" AllowPaging="false" GridLines="Both" CellPadding="4" CssClass="table table-bordered footable"
                                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnSelectedIndexChanged="gvReviewerMas_SelectedIndexChanged"
                                    EmptyDataText="No records found satisfying this criteria.">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Sr.No.
                       
                       
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit" ShowHeader="true">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select"
                                                        CssClass="btn btn-sm btn-soft-success btn-circle" OnClientClick="onClientEditClick()">
                                                        <i class="fa fa-pen"></i>
                                                    </asp:LinkButton>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CSFM_CODE" HeaderText="Unit Code" />
                                        <asp:BoundField DataField="CSFM_NAME" HeaderText="Unit Name" />
                                        <asp:BoundField DataField="CSFM_UNIT_HEAD_CODE" HeaderText="Unit SPOC Id" />
                                        <asp:BoundField DataField="CSFM_HEAD" HeaderText="Unit Head Name" />
                                        <asp:BoundField DataField="CSFM_UNIT_HEAD_EMAIL" HeaderText="Unit Head Email" />
                                        <asp:BoundField DataField="CSFM_UNIT_SPOC_CODE" HeaderText="Unit SPOC Id" />
                                        <asp:BoundField DataField="CSFM_UNIT_SPOC" HeaderText="Unit SPOC Name" />
                                        <asp:BoundField DataField="CSFM_UNIT_SPOC_EMAIL" HeaderText="Unit SPOC Email" />
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("CSFM_STATUS").ToString()=="A"?"Active":"Inactive" %>'
                                                    Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwInsert" runat="server">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4 mb-3 d-none">
                                    <label class="form-label">Id :</label>
                                    <asp:Label ID="lblID" runat="server" Visible="false" CssClass="form-control custom-span-input"></asp:Label>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Unit Code : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtUnitCode" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtUnitCode" />
                                    <asp:RequiredFieldValidator ID="rfvUnitCode" runat="server" ControlToValidate="txtUnitCode" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please enter Unit Code"
                                        SetFocusOnError="True">Please enter Unit Code</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Unit Name : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtUnitName" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtUnitName" />
                                    <asp:RequiredFieldValidator ID="rfvUnitName" runat="server" ControlToValidate="txtUnitName" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please enter Unit Name"
                                        SetFocusOnError="True">Please enter Unit Name.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Unit Head ID : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtUnitHeadId" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtUnitHeadId" />
                                    <asp:RequiredFieldValidator ID="rfvUnitHeadId" runat="server" ControlToValidate="txtUnitHeadId" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please select Unit Head ID."
                                        SetFocusOnError="True">Please select Unit Head ID.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Unit Head Name : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtUnitHeadName" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtUnitHeadName" />
                                    <asp:RequiredFieldValidator ID="rfvUnitHeadName" runat="server" ControlToValidate="txtUnitHeadName" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please select Unit Head Name."
                                        SetFocusOnError="True">Please select Unit Head Name.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Unit Head Email : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtUnitHeadEmail" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredEmail runat="server" TargetControlID="txtUnitHeadEmail" />
                                    <asp:RequiredFieldValidator ID="rfvUnitHeadEmail" runat="server" ControlToValidate="txtUnitHeadEmail" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please select Unit Head Email."
                                        SetFocusOnError="True">Please enter Unit Head Email.</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revL0Email" runat="server" ControlToValidate="txtUnitHeadEmail" CssClass="text-danger"
                                        ErrorMessage="Enter Valid Email address" ValidationGroup="AddReviewerMas" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">Please enter a valid Email address</asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Unit SPOC ID : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtUnitSPOCID" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtUnitSPOCID" />
                                    <asp:RequiredFieldValidator ID="rfvUnitSPOCId" runat="server" ControlToValidate="txtUnitSPOCID" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please enter Unit SPOC ID"
                                        SetFocusOnError="True">Please enter Unit SPOC ID.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Unit SPOC Name : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtUnitSPOCName" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtUnitSPOCName" />
                                    <asp:RequiredFieldValidator ID="rfvUnitSPOCName" runat="server" ControlToValidate="txtUnitSPOCName" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please select Unit SPOC Name."
                                        SetFocusOnError="True">Please select Unit SPOC Name.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Unit SPOC Email : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtUnitSPOCEmail" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredEmail runat="server" TargetControlID="txtUnitSPOCEmail" />
                                    <asp:RequiredFieldValidator ID="rfvUnitSPOCEmail" runat="server" ControlToValidate="txtUnitSPOCEmail" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please select Level 1 Reviewer Email."
                                        SetFocusOnError="True">Please enter Level 1 Reviewer Email.</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revUnitSPOCEmail" runat="server" ControlToValidate="txtUnitSPOCEmail" CssClass="text-danger"
                                        ErrorMessage="Enter Valid Email address"
                                        ValidationGroup="AddReviewerMas" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">Please enter a valid Email address</asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Status : <span class="text-danger">*</span></label>
                                    <asp:DropDownList CssClass="form-select" ID="ddlStatus" runat="server">
                                        <asp:ListItem Value="" Text="Select"></asp:ListItem>
                                        <asp:ListItem Value="A">Active</asp:ListItem>
                                        <asp:ListItem Value="I">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" SetFocusOnError="True">Please select status.</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="text-center mt-3">
                                <asp:LinkButton CssClass="btn btn-outline-success" ID="imgSave" Text="Save" OnClick="btnSave_Click"
                                    runat="server" ValidationGroup="AddReviewerMas" >
                                    <i class="fa fa-save me-2"></i> Save                    
                                </asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-outline-danger" ID="imgCancel" Text="Cancel" OnClick="btnCancel_Click"
                                    runat="server" >
                                    <i class="fa fa-arrow-left me-2"></i> Cancel
                                </asp:LinkButton>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>

            </div>
        </div>
    </div>
    <!-- end row -->

</asp:Content>

