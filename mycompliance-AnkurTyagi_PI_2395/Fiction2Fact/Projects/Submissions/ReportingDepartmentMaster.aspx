<%@ Page Title="" Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" Async="true"
    CodeBehind="ReportingDepartmentMaster.aspx.cs"
    Inherits="Fiction2Fact.Projects.Submissions.ReportingDepartmentMaster" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/legacy/populateUserDetsAJAX.js") %>'>
    </script>
    <script>

        function onSaveClick() {
            var rb = document.getElementById('ctl00_ContentPlaceHolder1_ddlType');
            if (rb.value == "D") {
                var validated = Page_ClientValidate("SaveDepartment");
                if (validated) {
                    var IsDoubleClickFlagSet = document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value;
                    if (IsDoubleClickFlagSet == 'Yes') {
                        alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                        return false;
                    }
                    else {
                        document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                        return true;
                    }
                }
            }
            if (rb.value == "O") {
                var validated = Page_ClientValidate("SaveOwners");
                if (validated) {
                    var IsDoubleClickFlagSet = document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value;
                    if (IsDoubleClickFlagSet == 'Yes') {
                        alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                        return false;
                    }
                    else {
                        document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                        return true;
                    }
                }
            }
        }

        function onClientEditClick(DeptId, LevelId, Level) {
            document.getElementById('<%=hfSRD_ID.ClientID%>').value = DeptId;
            document.getElementById('<%=hfLevelId.ClientID%>').value = LevelId;
            document.getElementById('<%=hfLevel.ClientID%>').value = Level;
        }

    </script>

    <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />
    <asp:HiddenField ID="hfSRD_ID" runat="server" />
    <asp:HiddenField ID="hfLevelId" runat="server" />
    <asp:HiddenField ID="hfLevel" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Reporting Department</h4>
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

                    <asp:MultiView ID="mvReportingDepartment" runat="server">
                        <asp:View ID="vSearch" runat="server">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Enter Department Name:</label>
                                    <asp:TextBox ID="txtDepartment" CssClass="form-control" runat="server"></asp:TextBox>
                                <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtDepartment" />
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Enter User Name:</label>
                                    <asp:TextBox ID="txtName" CssClass="form-control" runat="server"></asp:TextBox>
                               <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtName" />
                                    </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Select Level:</label>
                                    <asp:DropDownList ID="ddlLevel" CssClass="form-select" runat="server">
                                        <asp:ListItem Value="">(Select an option)</asp:ListItem>
                                        <asp:ListItem Value="0">Level 0</asp:ListItem>
                                        <asp:ListItem Value="1">Level 1</asp:ListItem>
                                        <asp:ListItem Value="2">Level 2</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="text-center mt-3">
                                    <asp:LinkButton CssClass="btn btn-outline-success" ID="lnkSearch" runat="server" Text="Search" OnClick="lnkSearch_Click">
								    <i class="fa fa-search"></i> Search                
                                    </asp:LinkButton>
                                    <asp:LinkButton CssClass="btn btn-outline-danger" ID="lnkReset" runat="server" CausesValidation="false"
                                        Text="Cancel" OnClick="lnkReset_Click">
								     <%--<i class="fa fa-arrow-left me-2"></i>--%> Reset
                                    </asp:LinkButton>
                                    <asp:LinkButton CssClass="btn btn-outline-success" ID="lnkAdd" runat="server" CausesValidation="false"
                                        OnClick="lnkAdd_Click">
                                            <i class="fa fa-plus"></i> Add
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnExportToExcel" Visible="false" runat="server" CssClass="btn btn-outline-secondary"
                                        Text="Export to Excel" OnClick="btnExportToExcel_Click">
                                                <i class="fa fa-download"></i> Export to Excel               
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="row mt-2">
                                <asp:GridView ID="gvReportingDepartment" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered footable"
                                    EmptyDataText="No records found....." AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" AllowSorting="true"
                                    AllowPaging="True" OnPageIndexChanging="gvReportingDepartment_PageIndexChanging" OnSorting="gvReportingDepartment_Sorting"
                                    OnSelectedIndexChanged="gvReportingDepartment_SelectedIndexChanged" OnRowCreated="OnRowCreated">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="10%">
                                            <HeaderTemplate>Sr.No.</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbEdit" runat="server" CausesValidation="False" CommandName="Select"
                                                    OnClientClick='<%# "onClientEditClick("+ Eval("DeptId")+","+Eval("LevelId")+","+Eval("Level")+")" %>'><i class="fa fa-edit me-2"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
                                        <asp:BoundField DataField="Department Status" HeaderText="Department Status" SortExpression="Department Status" />
                                        <asp:BoundField DataField="User Id" HeaderText="User Id" SortExpression="User Id" />
                                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                        <asp:BoundField DataField="Email Id" HeaderText="Email Id" SortExpression="Email Id" />
                                        <asp:BoundField DataField="Level" HeaderText="Level" SortExpression="Level" />
                                        <asp:BoundField DataField="User Status" HeaderText="User Status" SortExpression="User Status" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:View>
                        <asp:View ID="vAddEdit" runat="server">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Select Type:</label>
                                    <asp:DropDownList ID="ddlSType" CssClass="form-select"
                                        runat="server" OnSelectedIndexChanged="ddlSType_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Value="">(Select an option)</asp:ListItem>
                                        <asp:ListItem Value="D">Department</asp:ListItem>
                                        <asp:ListItem Value="U">Users</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <asp:Panel ID="pnlDepartment" runat="server" Visible="false">
                                    <div class="row">
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Enter Department Name:<span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtDepartmentName" CssClass="form-control" runat="server"></asp:TextBox>
                                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtDepartmentName" />
                                            <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" ErrorMessage="Please enter Department name." CssClass="text-danger"
                                                ValidationGroup="SaveDepartment" Display="Dynamic" ControlToValidate="txtDepartmentName"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Select Status:<span class="text-danger">*</span></label>
                                            <asp:DropDownList ID="ddlDepartmentStatus" CssClass="form-select" runat="server">
                                                <asp:ListItem Value="">(Select an option)</asp:ListItem>
                                                <asp:ListItem Value="A">Active</asp:ListItem>
                                                <asp:ListItem Value="I">Inactive</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvDepartmentStatus" runat="server" ErrorMessage="Please select status." CssClass="text-danger"
                                                ValidationGroup="SaveDepartment" Display="Dynamic" ControlToValidate="ddlDepartmentStatus"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4 mb-3" id="divDeptReason" runat="server">
                                            <label class="form-label">Enter reason for edit:<span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtDeptReasonForEdit" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtDeptReasonForEdit" />
                                            <asp:RequiredFieldValidator ID="rfvDeptReason" runat="server" ErrorMessage="Please enter Reason for edit." CssClass="text-danger"
                                                ValidationGroup="SaveDepartment" Display="Dynamic" ControlToValidate="txtDeptReasonForEdit"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="text-center mt-3">
                                            <asp:LinkButton CssClass="btn btn-outline-success" ID="btnDepartmentSave" runat="server" Text="Save" ValidationGroup="SaveDepartment"
                                                OnClientClick="return onSaveClick()" OnClick="btnDepartmentSave_Click">
								                <i class="fa fa-save me-2"></i> Save                    
                                            </asp:LinkButton>
                                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnReset1" runat="server" CausesValidation="false"
                                                Text="Cancel" OnClick="lnkReset1_Click">
								                <i class="fa fa-arrow-left me-2"></i> Back
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlUsers" runat="server" Visible="false">
                                    <div class="row">
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Select Department:<span class="text-danger">*</span></label>
                                            <asp:DropDownList ID="ddlODepartment" CssClass="form-select" AppendDataBoundItems="true"
                                                runat="server" DataValueField="SRD_ID" DataTextField="SRD_NAME">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvODepartment" runat="server" ErrorMessage="Please select Department name." CssClass="text-danger"
                                                ValidationGroup="SaveOwners" Display="Dynamic" ControlToValidate="ddlODepartment"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Enter User Id:<span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtOUserId" CssClass="form-control" runat="server"></asp:TextBox>
                                            <F2FControls:F2FFilteredEmail runat="server" TargetControlID="txtOUserId" />
                                            <asp:RequiredFieldValidator ID="rfvOUserId" runat="server" ErrorMessage="Please enter user id." CssClass="text-danger"
                                                ValidationGroup="SaveOwners" Display="Dynamic" ControlToValidate="txtOUserId"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Enter Name:<span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtOName" CssClass="form-control" runat="server"></asp:TextBox>
                                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtOName" />
                                            <asp:RequiredFieldValidator ID="rfvOName" runat="server" ErrorMessage="Please enter name." CssClass="text-danger"
                                                ValidationGroup="SaveOwners" Display="Dynamic" ControlToValidate="txtOName"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Enter Email Id:<span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtOEmailId" CssClass="form-control" runat="server"></asp:TextBox>
                                            <F2FControls:F2FFilteredEmail runat="server" TargetControlID="txtOEmailId" />
                                            <asp:RequiredFieldValidator ID="rfvOEmailId" runat="server" ErrorMessage="Please enter email id." CssClass="text-danger"
                                                ValidationGroup="SaveOwners" Display="Dynamic" ControlToValidate="txtOEmailId"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revOEmailId" runat="server" ControlToValidate="txtOEmailId" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                ErrorMessage="Incorrect Email Id." Display="Dynamic" CssClass="text-danger"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label">Select Status:<span class="text-danger">*</span></label>
                                            <asp:DropDownList ID="ddlOStatus" CssClass="form-select" runat="server">
                                                <asp:ListItem Value="">(Select an option)</asp:ListItem>
                                                <asp:ListItem Value="A">Active</asp:ListItem>
                                                <asp:ListItem Value="I">Inactive</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvOStatus" runat="server" ErrorMessage="Please select status." CssClass="text-danger"
                                                ValidationGroup="SaveOwners" Display="Dynamic" ControlToValidate="ddlOStatus"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4 mb-3" id="divULevel" runat="server">
                                            <label class="form-label">Select Level:</label>
                                            <asp:DropDownList ID="ddlOLevel" CssClass="form-select" runat="server">
                                                <asp:ListItem Value="">(Select an option)</asp:ListItem>
                                                <asp:ListItem Value="0">Level 0</asp:ListItem>
                                                <asp:ListItem Value="1">Level 1</asp:ListItem>
                                                <asp:ListItem Value="2">Level 2</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvLevel" runat="server" ErrorMessage="Please select level." CssClass="text-danger"
                                                ValidationGroup="SaveOwners" Display="Dynamic" ControlToValidate="ddlOLevel"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4 mb-3" id="divUsrReason" runat="server">
                                            <label class="form-label">Enter Reason for edit:<span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtOReasonForEdit" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtOReasonForEdit" />
                                            <asp:RequiredFieldValidator ID="rfvOReasonForEdit" runat="server" ErrorMessage="Please enter Reason for edit." CssClass="text-danger"
                                                ValidationGroup="SaveOwners" Display="Dynamic" ControlToValidate="txtOReasonForEdit"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="text-center mt-3">
                                            <asp:LinkButton CssClass="btn btn-outline-success" ID="lnkUsrSave" runat="server" Text="Save" ValidationGroup="SaveOwners"
                                                OnClientClick="return onSaveClick()" OnClick="lnkUsrSave_Click">
								    <i class="fa fa-save me-2"></i> Save                    
                                            </asp:LinkButton>
                                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="lnkReset2" runat="server" CausesValidation="false"
                                                Text="Reset" OnClick="lnkReset1_Click">
								    <i class="fa fa-arrow-left me-2"></i> Back
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </asp:View>
                    </asp:MultiView>









                    <%--<asp:MultiView ID="mvReportingDepartment" runat="server">
                        <asp:View ID="vSearch" runat="server">

                            <div class="col-md-4 mb-3">
                                <label class="form-label">Select Type:</label>
                                <asp:DropDownList ID="ddlSType" CssClass="form-select"
                                    runat="server" OnSelectedIndexChanged="ddlSType_SelectedIndexChanged" AutoPostBack="True">
                                    <asp:ListItem Value="D">Department</asp:ListItem>
                                    <asp:ListItem Value="U">Users</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <asp:Panel ID="pnlSDepartment" runat="server" Visible="false">
                                <div class="row">
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Enter Department Name:</label>
                                        <asp:TextBox ID="txtDeptDepartment" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Select Status:<span class="text-danger">*</span></label>
                                        <asp:DropDownList ID="ddlDeptStatus" CssClass="form-select" runat="server">
                                            <asp:ListItem Value="">(Select an option)</asp:ListItem>
                                            <asp:ListItem Value="A">Active</asp:ListItem>
                                            <asp:ListItem Value="I">Inactive</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="text-center mt-3">
                                        <asp:LinkButton CssClass="btn btn-outline-success" ID="lnkDeptSearch" runat="server" Text="Search" OnClick="lnkDeptSearch_Click">
								    <i class="fa fa-search"></i> Search                
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="lnkDeptReset" runat="server" CausesValidation="false"
                                            Text="Cancel" OnClick="lnkDeptReset_Click">
								     <i class="fa fa-arrow-left me-2"></i> Cancel
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-success" ID="lnkAddDepartment" runat="server" CausesValidation="false"
                                            OnClick="lnkAddDepartment_Click">
                                            <i class="fa fa-plus"></i> Add Reporting Department
                                        </asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row mt-2">
                                    <asp:GridView ID="gvDepartment" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered footable"
                                        EmptyDataText="No records found....." AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
                                        DataKeyNames="SRD_ID" AllowPaging="True" OnPageIndexChanging="gvDepartment_PageIndexChanging" OnSelectedIndexChanged="gvDepartment_SelectedIndexChanged">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="10%">
                                                <HeaderTemplate>
                                                    Sr.No.
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbEdit" runat="server" CausesValidation="False" CommandName="Select"
                                                        OnClientClick="return onClientEditClick();"><i class="fa fa-edit me-2"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="SRD_NAME" HeaderText="Department Name" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlSUsers" runat="server" Visible="false">
                                <div class="row">
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Select Department:<span class="text-danger">*</span></label>
                                        <asp:DropDownList ID="ddlUsrDepartment" CssClass="form-select" AppendDataBoundItems="true"
                                            runat="server" DataValueField="SRD_ID" DataTextField="SRD_NAME">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Enter User Id:<span class="text-danger">*</span></label>
                                        <asp:TextBox ID="txtUsrUserId" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Enter Name:<span class="text-danger">*</span></label>
                                        <asp:TextBox ID="txtUsrName" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Select Status:<span class="text-danger">*</span></label>
                                        <asp:DropDownList ID="ddlUsrStatus" CssClass="form-select" runat="server">
                                            <asp:ListItem Value="">(Select an option)</asp:ListItem>
                                            <asp:ListItem Value="A">Active</asp:ListItem>
                                            <asp:ListItem Value="I">Inactive</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="text-center mt-3">
                                        <asp:LinkButton CssClass="btn btn-outline-success" ID="lnkUsrSearch" runat="server" Text="Search" OnClick="lnkUsrSearch_Click">
								    <i class="fa fa-search"></i> Search                
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="lnkUsrReset" runat="server" CausesValidation="false"
                                            Text="Cancel" OnClick="lnkUsrReset_Click">
								    <i class="fa fa-arrow-left me-2"></i> Cancel
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-success" ID="lnkAddUsers" runat="server" CausesValidation="false"
                                            OnClick="lnkAddUsers_Click">
                                            <i class="fa fa-plus"></i> Add Reporting Users
                                        </asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row mt-2">
                                    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered footable"
                                        EmptyDataText="No records found....." AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
                                        DataKeyNames="SRDOM_ID" AllowPaging="True" OnPageIndexChanging="gvUsers_PageIndexChanging" OnSelectedIndexChanged="gvUsers_SelectedIndexChanged">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Sr.No.
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbEdit" runat="server" CausesValidation="False" CommandName="Select"
                                                        OnClientClick="return onClientEditClick();"><i class="fa fa-edit me-2"></i>
                                                </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="SRD_NAME" HeaderText="Department" />
                                            <asp:BoundField DataField="SRDOM_EMP_NAME" HeaderText="Name" />
                                            <asp:BoundField DataField="SRDOM_EMAILID" HeaderText="Email Id" />
                                            <asp:BoundField DataField="SRDOM_EMP_ID" HeaderText="User Id" />
                                            <asp:BoundField DataField="SRDOM_STATUS" HeaderText="Status" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                        </asp:View>
                        
                    </asp:MultiView>--%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
