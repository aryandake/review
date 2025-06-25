<%@ Page Title="" Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" CodeBehind="ReviewerMaster.aspx.cs" Inherits="Fiction2Fact.Projects.Compliance.ReviewerMaster" %>

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
                        <h4 class="page-title">Reviewer Master</h4>
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
                <asp:MultiView ID="mvReviewMaster" runat="server">
                    <asp:View runat="server" ID="vwSearch">
                        <asp:Panel runat="server" ID="pnlSearch" DefaultButton="imgSearch">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Reviewer NT ID</label>
                                        <asp:TextBox CssClass="form-control" ID="txtSearchL0Reviewer" MaxLength="50" runat="server"></asp:TextBox>
                                        <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSearchL0Reviewer" />
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
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">&nbsp;</label>
                                        <div>
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
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gvReviewerMas" runat="server" AutoGenerateColumns="False" DataKeyNames="CRM_ID"
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
                                        <asp:BoundField DataField="CRM_L0_REVIEWER_NT_ID" HeaderText="Reviewer NT ID" />
                                        <asp:BoundField DataField="CRM_L0_REVIEWER_NAME" HeaderText="Reviewer Name" />
                                        <asp:BoundField DataField="CRM_L0_REVIEWER_EMAIL" HeaderText="Reviewer Email" />
                                        <asp:BoundField DataField="CRM_L1_REVIEWER_NT_ID" HeaderText="Level 1 Reviewer NT ID" />
                                        <asp:BoundField DataField="CRM_L1_REVIEWER_NAME" HeaderText="Level 1 Reviewer Name" />
                                        <asp:BoundField DataField="CRM_L1_REVIEWER_EMAIL" HeaderText="Level 1 Reviewer Email" />
                                        <asp:BoundField DataField="CRM_L2_REVIEWER_NT_ID" HeaderText="Level 2 Reviewer NT ID" Visible="false" />
                                        <asp:BoundField DataField="CRM_L2_REVIEWER_NAME" HeaderText="Level 2 Reviewer Name" Visible="false" />
                                        <asp:BoundField DataField="CRM_L2_REVIEWER_EMAIL" HeaderText="Level 2 Reviewer Email" Visible="false" />
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("CRM_STATUS").ToString()=="A"?"Active":"Inactive" %>'
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
                                    <asp:Label ID="lblID" runat="server" Visible="false" CssClass="label"></asp:Label>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Reviewer NT ID : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtRId" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtRId" />
                                    <asp:RequiredFieldValidator ID="rfvRId" runat="server" ControlToValidate="txtRId" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please select Reviewer NT ID."
                                        SetFocusOnError="True">Please select Reviewer NT ID.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Reviewer Name : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtRName" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtRName" />
                                    <asp:RequiredFieldValidator ID="rfvRName" runat="server" ControlToValidate="txtRName" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please select Reviewer Name."
                                        SetFocusOnError="True">Please select Reviewer Name.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Reviewer Email : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtREmail" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredEmail runat="server" TargetControlID="txtREmail" />
                                    <asp:RequiredFieldValidator ID="rfvREmail" runat="server" ControlToValidate="txtREmail" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please select Reviewer Email."
                                        SetFocusOnError="True">Please enter Reviewer Email.</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revL0Email" runat="server" ControlToValidate="txtREmail" CssClass="text-danger"
                                        ErrorMessage="Enter Valid Email address" ValidationGroup="AddReviewerMas" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">Please enter a valid Email address</asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Level 1 Reviewer NT ID : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtL1RId" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtL1RId" />
                                    <asp:RequiredFieldValidator ID="rfvL1RId" runat="server" ControlToValidate="txtL1RId" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please select Level 1 Reviewer NT ID."
                                        SetFocusOnError="True">Please select Level 1 Reviewer NT ID.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Level 1 Reviewer Name : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtL1RName" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtL1RName" />
                                    <asp:RequiredFieldValidator ID="rfvL1RName" runat="server" ControlToValidate="txtL1RName" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please select Level 1 Reviewer Name."
                                        SetFocusOnError="True">Please select Level 1 Reviewer Name.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Level 1 Reviewer Email : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtL1REmail" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredEmail runat="server" TargetControlID="txtL1REmail" />
                                    <asp:RequiredFieldValidator ID="rfvL1REmail" runat="server" ControlToValidate="txtL1REmail" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please select Level 1 Reviewer Email."
                                        SetFocusOnError="True">Please enter Level 1 Reviewer Email.</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revL1Email" runat="server" ControlToValidate="txtL1REmail" CssClass="text-danger"
                                        ErrorMessage="Enter Valid Email address"
                                        ValidationGroup="AddReviewerMas" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">Please enter a valid Email address</asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-4 mb-3 d-none">
                                    <label class="form-label">Level 2 Reviewer NT ID : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtL2RId" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtL2RId" />
                                    <asp:RequiredFieldValidator ID="rfvL2RId" runat="server" ControlToValidate="txtL2RId" CssClass="text-danger" Enabled="false"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please select Level 2 Reviewer NT ID."
                                        SetFocusOnError="True">Please select Level 2 Reviewer NT ID.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3 d-none">
                                    <label class="form-label">Level 2 Reviewer Name : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtL2RName" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtL2RName" />
                                    <asp:RequiredFieldValidator ID="rfvL2RName" runat="server" ControlToValidate="txtL2RName" CssClass="text-danger" Enabled="false"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please select Level 2 Reviewer Name."
                                        SetFocusOnError="True">Please select Level 2 Reviewer Name.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3 d-none">
                                    <label class="form-label">Level 2 Reviewer Email : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtL2REmail" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredEmail runat="server" TargetControlID="txtL2REmail" />
                                    <asp:RequiredFieldValidator ID="rfvL2REmail" runat="server" ControlToValidate="txtL2REmail" CssClass="text-danger" Enabled="false"
                                        Display="Dynamic" ValidationGroup="AddReviewerMas" ErrorMessage="Please enter Level 2 Reviewer Email."
                                        SetFocusOnError="True">Please enter Level 2 Reviewer Email.</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revL2Email" runat="server" ControlToValidate="txtL2REmail" CssClass="text-danger" Enabled="false"
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
