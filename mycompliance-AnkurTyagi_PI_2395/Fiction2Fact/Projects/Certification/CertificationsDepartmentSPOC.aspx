<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.CertificationsDepartmentSPOC" Title="Certifications Department SPOC Mas" CodeBehind="CertificationsDepartmentSPOC.aspx.cs" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("/Content/js/legacy/commonvalidations.js") %>'></script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js") %>'></script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/dateformatvalidation.js")%>'></script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js") %>'></script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/populateUserDetsAJAX.js")%>'>
    </script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/CertificationDeptSpocMas.js")%>'></script>

    <script type="text/javascript">

        function onClientEditClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
        }

        function onClientDeleteClick() {
            if (!confirm('Are you sure you want to delete the selected record(s)?')) return false;
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Delete";
        }

    </script>

    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField ID="hfMembersDetsData" runat="server" />
    <asp:HiddenField ID="hfStatus" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Certification Department Master</h4>
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
                <asp:MultiView ID="mvCertDeptMaster" runat="server">
                    <asp:View runat="server" ID="vwSearch">
                        <asp:Panel runat="server" ID="pnlSearch" DefaultButton="imgSearch">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Certifying Functions</label>
                                        <asp:DropDownList CssClass="form-select" ID="ddlSearchCertDept" runat="server"
                                            DataValueField="CDM_ID" DataTextField="CDM_NAME">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Department Name</label>
                                        <F2FControls:F2FTextBox CssClass="form-control" ID="txtSearchCertDeptName" MaxLength="200" runat="server"
                                            Columns="40"></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSearchCertDeptName" />
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
                                <asp:GridView ID="gvCertDeptMaster" runat="server" AutoGenerateColumns="False" DataKeyNames="CSDM_ID"
                                    AllowSorting="false" AllowPaging="true" GridLines="Both" CellPadding="4" CssClass="table table-bordered footable"
                                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnSelectedIndexChanged="gvCertDeptMaster_SelectedIndexChanged"
                                    OnPageIndexChanging="gvCertDeptMaster_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Sr.No.
                       
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                &nbsp;&nbsp;&nbsp;<asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>'
                                                    runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CDM_NAME" HeaderText="Certifying Function" />
                                        <asp:BoundField DataField="DeptName" HeaderText="Certifying Unit" />
                                        <asp:BoundField DataField="CSDM_USER_ID" HeaderText="Unit Head User Id" />
                                        <asp:BoundField DataField="CSDM_EMP_NAME" HeaderText="Unit Head User Name" />
                                        <asp:BoundField DataField="CSDM_EMAIL_ID" HeaderText="Unit Head Email Id" />
                                        <asp:TemplateField HeaderText="Edit" ShowHeader="true">
                                            <ItemTemplate>
                                                <asp:LinkButton CssClass="btn btn-sm btn-soft-success btn-circle" ID="lnkEdit" runat="server" CommandName="Select"
                                                    OnClientClick="onClientEditClick()">
                                                    <i class="fa fa-pen"></i>	
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete" ShowHeader="true">
                                            <ItemTemplate>
                                                <asp:LinkButton CssClass="btn btn-sm btn-soft-danger btn-circle" ID="lnkDelete" runat="server" CommandName="Select" OnClientClick=" return onClientDeleteClick()">
                                                    <i class="fa fa-trash"></i>	                            
                                                </asp:LinkButton>
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
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Id :</label>
                                    <asp:Label ID="lblID" runat="server"  CssClass="form-control custom-span-input"></asp:Label>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Certifying Function : <span class="text-danger">*</span></label>
                                    <asp:DropDownList CssClass="form-select" ID="ddlCertDept" runat="server" DataValueField="CDM_ID"
                                        DataTextField="CDM_NAME">
                                    </asp:DropDownList>
                                    <%--<asp:RequiredFieldValidator ID="rfvCertDept" runat="server"  CssClass="span" ControlToValidate="ddlCertDept"
                            Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True">Please select Certifying Function.</asp:RequiredFieldValidator>--%>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Certifying Unit : <span class="text-danger">*</span></label>
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtCertDeptName" MaxLength="200" runat="server"
                                        Columns="50"></F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtCertDeptName" />
                                    <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"    
	                        TargetControlID="txtCertDeptName" FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers"    
	                        FilterMode="ValidChars" ValidChars="@;:_-.'/\?!, " >    
                        </cc1:FilteredTextBoxExtender>--%>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Unit Head NT ID : <span class="text-danger">*</span></label>
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtCertDeptUserId" MaxLength="200" runat="server"
                                        Columns="50">
                        </F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtCertDeptUserId" />
                                    <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"    
	                        TargetControlID="txtCertDeptUserId" FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers"    
	                        FilterMode="ValidChars" ValidChars="@;:_-.'/\?!, " >    
                        </cc1:FilteredTextBoxExtender>--%>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Unit Head Name : <span class="text-danger">*</span></label>
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtCertDeptUserName" MaxLength="200" runat="server"
                                        Columns="50">
                        </F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtCertDeptUserName" />
                                    <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"    
	                        TargetControlID="txtCertDeptUserName" FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers"    
	                        FilterMode="ValidChars" ValidChars="@;:_-.'/\?!, " >    
                        </cc1:FilteredTextBoxExtender>--%>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Unit Head Email Id : <span class="text-danger">*</span></label>
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtCertDeptEmailID" MaxLength="200" runat="server"
                                        Columns="50">
                        </F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredEmail runat="server" TargetControlID="txtCertDeptEmailID" />
                                    <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"    
	                        TargetControlID="txtCertDeptEmailID" FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers"    
	                        FilterMode="ValidChars" ValidChars="@;:_-.'/\?!, " >    
                        </cc1:FilteredTextBoxExtender>--%>
                                </div>
                            </div>
                            <div class="mt-1">
                                <div class="card mb-1 mt-1 border">
                                    <div class="card-header py-0 custom-ch-bg-color">
                                        <h6 class="font-weight-bold text-white mtb-5">SPOC's Members Details : </h6>
                                    </div>
                                    <div class="card-body mt-1">
                                        <div class="mb-2">
                                            <asp:LinkButton CssClass="btn btn-sm btn-soft-primary btn-circle" ID="btnAddMember" Text="Add" runat="server" OnClientClick="return addMembersDetsRow()" >
                                                <i class="fa fa-plus"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton CssClass="btn btn-sm btn-soft-danger btn-circle" ID="brnDeleteMember" Text="Delete" runat="server"
                                                OnClientClick="return deleteMembersDetsRow()" >
                                                <i class="fa fa-trash"></i>
                                            </asp:LinkButton>
                                        </div>
                                        <div class="table-responsive">
                                            <asp:Literal ID="litMembersDetails" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="mt-3 text-center">
                                <asp:LinkButton CssClass="btn btn-outline-success" ID="imgSave" Text="Save" OnClick="btnSave_Click"
                                    runat="server" OnClientClick="return validateCertDeptDets()" >
                                    <i class="fa fa-save me-2"></i> Save                    
                                </asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-outline-danger" ID="imgCancel" Text="Cancel" OnClick="btnCancel_Click"
                                    runat="server">
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
