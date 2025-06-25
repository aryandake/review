<%@ Page Language="C#" MasterPageFile="~/Projects/Temp4.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Admin.Admin_RoleMenuMapping" Title="Role Menu Mapping" CodeBehind="RoleMenuMapping.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Assign Menu for Roles</h4>
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
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label class="form-label">&nbsp;</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlRole" runat="server" AppendDataBoundItems="true"
                                AutoPostBack="true" DataTextField="RoleName" DataValueField="RoleId" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-5 mb-3">
                            <asp:ListBox ID="lstMenu" CssClass="form-control" runat="server" DataTextField="MM_MENU_NAME" SelectionMode="Multiple"
                                DataValueField="MM_ID" Height="500px" ></asp:ListBox>
                        </div>
                        <div class="col-md-1 mb-1">
                             <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnAdd" Text=" >> " OnClick="btnAdd_Click"
                                runat="server" >
                                 <i class="fa fa-arrow-circle-right"></i>                      
                             </asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnRemove" Text=" << " OnClick="btnRemove_Click"
                                runat="server" >
                                <i class="fa fa-arrow-circle-left"></i>  
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-5 mb-3">
                            <asp:ListBox ID="lstMapping" CssClass="form-control" runat="server" DataTextField="MM_MENU_NAME" SelectionMode="Multiple"
                                DataValueField="MM_ID" Height="500px" ></asp:ListBox>
                        </div>
                    </div>
                </div> 
            </div>
        </div>
    </div>
    <!-- end row -->
    
</asp:Content>
