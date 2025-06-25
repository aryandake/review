<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Submissions.Submissions_SubmissionEscalations" Title="Escalations" CodeBehind="SubmissionEscalations.aspx.cs" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        function onClientEditClick() {
            document.getElementById('<%=hdfEscalationOperation.ClientID%>').value = "Edit";
        }
        function onClientDeleteClick() {
            if (!confirm('Are you sure that you want to delete this record?')) return false;
            document.getElementById('<%=hdfEscalationOperation.ClientID%>').value = "Delete";
        }
    </script>

    <asp:HiddenField ID="hdfEscalationOperation" runat="server" />
    <asp:HiddenField ID="hfSelectedLevel" runat="server" />
    <asp:HiddenField ID="hfSelectedId" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Escalations Master</h4>
                        <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
                        <asp:Label ID="lbInfo" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
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
                <asp:MultiView runat="server" ActiveViewIndex="0" ID="mvMultiView">
                    <asp:View ID="vwSearch" runat="server">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Reporting Department:</label>
                                    <asp:DropDownList ID="ddlSearchReportDept" CssClass="form-select" AppendDataBoundItems="true"
                                        runat="server" DataValueField="SRD_ID" DataTextField="SRD_NAME">
                                        <asp:ListItem Value="">(Select Reporting Dept.)</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Level:</label>
                                    <asp:DropDownList ID="ddlSearchLevel" runat="server" CssClass="form-select">
                                        <asp:ListItem Value="">(Select Level)</asp:ListItem>
                                        <asp:ListItem Value="1">Level 1</asp:ListItem>
                                        <asp:ListItem Value="2">Level 2</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">&nbsp;</label>
                                    <div>
                                        <asp:LinkButton ID="btnSearch" CssClass="btn btn-outline-primary" runat="server" Text="Search Escalations"
                                            OnClick="btnSearch_Click" >
                                            <i class="fa fa-search"></i> Search                    
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnAddNew" OnClick="btnAddNew_Click" runat="server"
                                            Text=" + " >
                                            <i class="fa fa-plus"></i> Add                               
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcel" OnClick="btnExportToExcel_Click"
                                            runat="server" Text="Export to Excel" >
                                            <i class="fa fa-download"></i> Export to Excel               
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gvEscalationMaster" runat="server" AutoGenerateColumns="False"
                                    CellPadding="4" GridLines="both" DataKeyNames="SE_ID" AllowSorting="true" OnSelectedIndexChanged="gvEscalationMaster_SelectedIndexChanged"
                                    OnSorting="gvEscalationMaster_Sorting" OnRowCreated="OnRowCreated" CssClass="table table-bordered footable"
                                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" EmptyDataText="No Records Found.....">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Sr.No.
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reporting Department" SortExpression="SRD_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReporting" runat="server" Text='<%# Eval("SRD_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="SE_FIRST_NAME" HeaderText="First Name" SortExpression="SE_FIRST_NAME" />
                                        <asp:BoundField DataField="SE_MIDDEL_NAME" HeaderText="Middle Name" SortExpression="SE_MIDDEL_NAME" />
                                        <asp:BoundField DataField="SE_LAST_NAME" HeaderText="Last Name" SortExpression="SE_LAST_NAME" />
                                        <%--<<Added by Ashish Mishra on 27Jul2017--%>
                                        <asp:BoundField DataField="Escalation Level" HeaderText="Level" SortExpression="Escalation Level" />
                                        <%-->>--%>
                                        <asp:BoundField DataField="SE_EMAIL_ID" HeaderText="Email ID" SortExpression="SE_EMAIL_ID" />
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-success btn-circle"
                                                    OnClientClick="onClientEditClick()">
                                                    <i class="fa fa-pen"></i>	                            
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-danger btn-circle"
                                                    OnClientClick="return onClientDeleteClick()">
                                                    <i class="fa fa-trash"></i>                                 
                                                 </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwEdit" runat="server">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Reporting Department:<span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="ddlReportDept" CssClass="form-select" AppendDataBoundItems="true"
                                        runat="server" DataValueField="SRD_ID" DataTextField="SRD_NAME">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvReportDept" runat="server" ControlToValidate="ddlReportDept" CssClass="text-danger" ErrorMessage="Please select Reporting Department." ValidationGroup="SaveEscalation"
                                        Display="Dynamic">Please select Reporting Department.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">First Name:<span class="text-danger">*</span></label>
                                    <F2FControls:F2FTextBox ID="txtFirstName" runat="server" CssClass="form-control" MaxLength="50"> </F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtFirstName" />
                                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" CssClass="text-danger" ErrorMessage="Please enter  First Name." ValidationGroup="SaveEscalation" Display="Dynamic">Please enter  First Name </asp:RequiredFieldValidator></td>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Middle Name:</label>
                                    <F2FControls:F2FTextBox ID="txtMiddelName" runat="server" CssClass="form-control"
                                        MaxLength="50"> </F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtMiddelName" />
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Last Name:<span class="text-danger">*</span></label>
                                    <F2FControls:F2FTextBox ID="txtLastName" runat="server" CssClass="form-control" MaxLength="50"> </F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtLastName" />
                                    <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" CssClass="text-danger" ErrorMessage="Please enter  Last Name." ValidationGroup="SaveEscalation" Display="Dynamic">Please enter  Last Name </asp:RequiredFieldValidator></td>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Email ID:<span class="text-danger">*</span></label>
                                    <F2FControls:F2FTextBox ID="txtEmailId" Columns="30" runat="server" CssClass="form-control"
                                        MaxLength="50"> </F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredEmail runat="server" TargetControlID="txtEmailId" />
                                    <asp:RequiredFieldValidator ID="rfvEmailId" runat="server" ControlToValidate="txtEmailId" CssClass="text-danger" ErrorMessage="Please enter an Email ID" ValidationGroup="SaveEscalation" Display="Dynamic">Please enter an Email ID </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revEmailId" runat="server" ControlToValidate="txtEmailId"
                                        ErrorMessage="Please enter a valid Email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="text-danger"
                                        ValidationGroup="SaveEscalation" Display="Dynamic">Please enter Valid Email ID</asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Level:<span class="text-danger">*</span></label>
                                    <div class="custom-checkbox-table">
                                        <asp:RadioButtonList ID="rblLevels" runat="server" RepeatColumns="3" CssClass="form-control" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1">Level 1</asp:ListItem>
                                            <asp:ListItem Value="2">Level 2</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvLevels" runat="server" ControlToValidate="rblLevels" CssClass="text-danger" ValidationGroup="SaveEscalation" Display="Dynamic" SetFocusOnError="True">Please select Level.</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="text-center mt-3">
                                <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSave" runat="server" Text="Save" ValidationGroup="SaveEscalation"
                                    OnClick="btnSave_Click" >
                                    <i class="fa fa-save me-2"></i> Save                    
                                </asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnCancel" runat="server" CausesValidation="false"
                                    Text="Cancel" OnClick="btnCancel_Click" >
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
