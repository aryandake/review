<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="True"
    Inherits="Fiction2Fact.Projects.Admin.UserManagement" Async="true" Title="User Management" CodeBehind="UserManagement.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        function onClientEditClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
        }

        function onClientDeleteClick() {
            if (!confirm('Are you sure that you want to delete this User?')) return false;
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Delete";
        }

        //<<Added by Ankur Tyagi on 28-Apr-2025 for Project Id : 2395
        function validateInput(input) {
            // Allow a-z, A-Z, 0-9, period (.),and hyphen (-)
            input.value = input.value.replace(/[^a-zA-Z0-9.-]/g, '');
        }
        //>>
    </script>
    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField ID="hfUser" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">User Management</h4>
                        <asp:Label ID="lblInfoMsg" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
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
        <div class="col-lg-12">
            <div class="card">
                <asp:MultiView ID="mvUserInformation" runat="server">
                    <asp:View ID="viewOptions" runat="server">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">User name:</label>
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtSearchUsername" runat="server"></F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSearchUsername" />
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Email id:</label>
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtEmail" runat="server"></F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredEmail runat="server" TargetControlID="txtEmail" />
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Role:</label>
                                    <asp:DropDownList CssClass="form-select" ID="ddlRoles" runat="server" DataTextField="RoleName"
                                        DataValueField="RoleId">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="text-center">
                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click">
                                    <i class="fa fa-search me-2"></i> Search
                                </asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnAddNew" runat="server" Text="Add New User"
                                    OnClick="btnAddNew_Click">
                                    <i class="fa fa-plus"></i> Add User
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExportToExcel" CssClass="btn btn-outline-secondary" runat="server" Text="Export to Excel"
                                    OnClick="btnExportToExcel_Click">
                                    <i class="fa fa-download"></i> Export to Excel
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="dgvUsers" runat="server" DataKeyNames="UserName" AutoGenerateColumns="False"
                                    AllowSorting="true" AllowPaging="true" OnSelectedIndexChanged="dgvUsers_SelectedIndexChanged"
                                    OnSorting="dgvUsers_Sorting" OnPageIndexChanging="dgvUsers_PageIndexChanging"
                                    GridLines="None" CellPadding="4" CssClass="table table-bordered footable" PagerStyle-CssClass="pgr"
                                    AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ShowHeader="true">
                                            <ItemTemplate>
                                                <div class="d-flex">
                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-success btn-circle me-2"
                                                        OnClientClick="onClientEditClick()">
                                                <i class="fa fa-pen"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-danger btn-circle"
                                                        OnClientClick=" return onClientDeleteClick()">
                                                <i class="fa fa-trash"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Username">
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" Text='<%# Eval("UserName")  %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmail" Text='<%# Eval("Email")  %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Roles">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRole" Text='<%# Eval("RoleName")  %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <%--<asp:TemplateField HeaderText="Edit" ShowHeader="true">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select"
                                                    OnClientClick="onClientEditClick()">
                                                <i class="fa fa-edit"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete" ShowHeader="true">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Select"
                                                    OnClientClick=" return onClientDeleteClick()">
                                                <i class="fa fa-trash"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="viewUserInformation" runat="server">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">User Id</label>
                                    <asp:Label ID="lbUserId" runat="server" CssClass="form-control custom-span-input"></asp:Label>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">User Name</label>
                                    <asp:Label ID="lblUser" runat="server" CssClass="form-control custom-span-input"></asp:Label>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Email Id</label>
                                    <asp:Label ID="lbEmail" runat="server" CssClass="form-control custom-span-input"></asp:Label>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Department</label>
                                    <asp:Label ID="lblDept" runat="server" CssClass="form-control custom-span-input"></asp:Label>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Contact</label>
                                    <asp:Label ID="lblCon" runat="server" CssClass="form-control custom-span-input"></asp:Label>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Designation</label>
                                    <asp:Label ID="lblDesg" runat="server" CssClass="form-control custom-span-input"></asp:Label>
                                </div>
                                <div class="col-md-12">
                                    <label class="form-label">Available Roles</label>
                                    <div class="custom-checkbox-table">
                                        <asp:CheckBoxList ID="lstRoles" runat="server" CssClass="form-control" AppendDataBoundItems="True" RepeatColumns="5">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="text-center mb-2">
                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSubmit" ValidationGroup="UpdateUserInfo"
                                runat="server" Text="Save" OnClick="btnSubmit_Click">
                                    <i class="fa fa-save me-2"></i> Save Changes
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack3" runat="server" Text="Back" OnClick="btnBack_Click">
                                    <i class="fa fa-arrow-left me-2"></i> Back
                            </asp:LinkButton>
                        </div>
                    </asp:View>
                    <asp:View ID="vwNewuser" runat="server">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">User Id <span class="text-danger">*</span></label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox CssClass="form-control" ID="txtUserID" runat="server" 
                                            onkeyblur="validateInput(this);" onkeypress="validateInput(this);" 
                                            onkeyup="validateInput(this);"></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtUserID" />
                                        <asp:Button ID="btnFetch" CssClass="btn btn-outline-primary" Text="Fetch" OnClick="btnFetch_Click"
                                            runat="server" ValidationGroup="UpdateUserInfo" />
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtUserID" CssClass="text-danger"
                                        ErrorMessage="Please Enter User ID." SetFocusOnError="True" ToolTip="Please Enter User ID."
                                        ValidationGroup="UpdateUserInfo">Please Enter User ID.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">User Name</label>
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtuserName" runat="server"></F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtuserName" />
                                    <%-- <asp:Label ID="lblUserName" runat="server" CssClass="form-control custom-span-input"></asp:Label>--%>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Email Id <span class="text-danger">*</span></label>
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtEmailId" runat="server" Columns="50" MaxLength="100"></F2FControls:F2FTextBox>
                                    <%-- <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmailId"
                                        ErrorMessage="Please Enter User ID." SetFocusOnError="True" ToolTip="Please Enter Email Id."
                                        ValidationGroup="UpdateUserInfo">Please Enter Email ID.</asp:RequiredFieldValidator>--%>
                                    <%--<asp:RegularExpressionValidator ID="revEmailId" runat="server" ControlToValidate="txtEmailId" CssClass="text-danger"
                                        ErrorMessage="Please enter a valid EMail address" Font-Names="Calibri" Font-Size="Small"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                        ValidationGroup="UpdateUserInfo">Please enter a valid mail address.</asp:RegularExpressionValidator>--%>
                                    <F2FControls:F2FFilteredEmail runat="server" TargetControlID="txtEmailId" />
                                    <asp:RegularExpressionValidator ID="revEmailId" runat="server" ControlToValidate="txtEmailId" CssClass="text-danger"
                                        ErrorMessage="Please enter a valid EMail address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        ValidationGroup="UpdateUserInfo">Please enter a valid mail address.</asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Department</label>
                                    <asp:Label ID="lblDepartment" runat="server" CssClass="form-control custom-span-input"></asp:Label>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Contact</label>
                                    <asp:Label ID="lblContact" runat="server" CssClass="form-control custom-span-input"></asp:Label>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Designation</label>
                                    <asp:Label ID="lblDesignation" runat="server" CssClass="form-control custom-span-input"></asp:Label>
                                </div>
                                <div class="col-md-12">
                                    <label class="form-label">Available Roles</label>
                                    <div class="custom-checkbox-table">
                                        <asp:CheckBoxList ID="cbAvRoles" runat="server" CssClass="form-control" AppendDataBoundItems="True" RepeatColumns="5">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="text-center mb-2">
                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSave" ValidationGroup="UpdateUserInfo"
                                runat="server" Text="Save" OnClick="btnSave_Click">
                                    <i class="fa fa-save me-2"></i> Save
                            </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click">
                                    <i class="fa fa-arrow-left me-2"></i> Back
                            </asp:LinkButton>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
    <!-- end row -->
</asp:Content>
