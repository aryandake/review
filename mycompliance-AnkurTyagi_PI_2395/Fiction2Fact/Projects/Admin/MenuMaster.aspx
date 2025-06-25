<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    CodeBehind="MenuMaster.aspx.cs" Inherits="Fiction2Fact.Projects.Admin.MenuMaster" Title=":: Menu Master ::" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        function onClientEditClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
        }
        function onClientDeleteClick() {
            if (!confirm('Are you sure that you want to delete this record?')) return false;
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Delete";
        }

    </script>

    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">
                            <asp:Label ID="lblHeaderTxt" runat="server" Text="Menu Master"></asp:Label></h4>
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
                <asp:MultiView ID="mvSearch" runat="server">
                    <asp:View runat="server" ID="search">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Level</label>
                                    <asp:DropDownList ID="ddlSearchMenuLevel" CssClass="form-select" runat="server"
                                        OnSelectedIndexChanged="onSearchLevelChanged" AutoPostBack="true">
                                        <asp:ListItem Text="-- Select --" Value=""></asp:ListItem>
                                        <asp:ListItem Text="Level 1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Level 2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Level 3" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Parent Menu Name</label>
                                    <asp:DropDownList ID="ddlSearchParentMenu" CssClass="form-select" runat="server"
                                        DataTextField="MenuName" DataValueField="MenuId">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Menu Name</label>
                                    <F2FControls:F2FTextBox ID="txtSearchMenuName" CssClass="form-control" Columns="20" MaxLength="100" runat="server">
                                            </F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSearchMenuName" />
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Menu URL</label>
                                    <F2FControls:F2FTextBox ID="txtSearchMenuURL" CssClass="form-control" Columns="20" MaxLength="100" runat="server">
                                            </F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSearchMenuURL" />
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Status</label>
                                    <asp:DropDownList ID="ddlSearchStatus" CssClass="form-select" runat="server">
                                        <asp:ListItem Text="-- Select --" Value=""></asp:ListItem>
                                        <asp:ListItem Text="Active" Value="A"></asp:ListItem>
                                        <asp:ListItem Text="Inactive" Value="I"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">&nbsp;</label>
                                    <div>
                                        <asp:LinkButton ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-outline-primary" OnClick="btnSearch_Click" >
                                            <i class="fa fa-search"></i> Search                     
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnAdd" runat="server" Text="+" CssClass="btn btn-outline-primary" OnClick="btnAdd_Click" >
                                            <i class="fa fa-plus"></i> Add    
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="btnExportToExcel" OnClick="btnExportToExcel_Click"
                                            Text="Export to Excel" Visible="false" CssClass="btn btn-outline-secondary" >
                                            <i class="fa fa-download"></i> Export to Excel               
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gvMenuMas" runat="server" AutoGenerateColumns="false" EmptyDataText="No records Found.."
                                    DataKeyNames="MenuId" AllowSorting="false" AllowPaging="false" OnPageIndexChanging="gvMenuMas_PageIndexChanging"
                                    GridLines="Both" CellPadding="4" CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                    OnSelectedIndexChanged="gvMenuMas_SelectedIndexChanged">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Sr. No.
                                       
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit" ShowHeader="true">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-success btn-circle"
                                                    OnClientClick="onClientEditClick()">
                                                    <i class="fa fa-pen"></i>
                                            </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Delete" ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Select" Text="&lt;img alt='Delete' src='../../Content/images/legacy/delete_1.png' border='0' &gt;"
                                                OnClientClick="onClientDeleteClick()">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Move Up / Down">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lblSortOrder" Text='<%# Eval("SortOrder")%>' Visible="false" runat="server"></asp:Label>
                                                    <asp:LinkButton ID="lnkUp" CssClass="lnkbutton" CommandArgument="up" runat="server"
                                                        Text="&#x25B2;" OnClick="ChangeSection" />
                                                    <asp:LinkButton ID="lnkDown" CssClass="lnkbutton" CommandArgument="down" runat="server"
                                                        Text="&#x25BC;" OnClick="ChangeSection" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MenuLevel" HeaderText="Menu Level" />
                                        <asp:BoundField DataField="ParentMenuName" HeaderText="Parent Menu Name" />
                                        <asp:BoundField DataField="MenuName" HeaderText="Menu Name" />
                                        <asp:BoundField DataField="MenuURL" HeaderText="Menu URL" />
                                        <asp:BoundField DataField="SortOrder" HeaderText="Sort Order" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View runat="server" ID="insert">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Id :</label>
                                    <asp:Label ID="lblID" runat="server" CssClass="form-control custom-span-input"></asp:Label>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Menu Name :<span class="text-danger">*</span></label>
                                    <F2FControls:F2FTextBox CssClass="form-control"  ID="txtMenuName" MaxLength="200" runat="server">
                                        </F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtMenuName" />
                                    <asp:RequiredFieldValidator ID="rfvMenuName" runat="server" ControlToValidate="txtMenuName" CssClass="text-danger"
                                        ErrorMessage="Please enter Menu Name" Display="Dynamic" ValidationGroup="Save"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Menu Level :<span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="ddlMenuLevel" CssClass="form-select" runat="server"
                                        OnSelectedIndexChanged="onAddLevelChanged" AutoPostBack="true">
                                        <asp:ListItem Text="-- Select --" Value=""></asp:ListItem>
                                        <asp:ListItem Text="Level 1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Level 2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Level 3" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvLevel" runat="server" ControlToValidate="ddlMenuLevel" CssClass="text-danger"
                                        ErrorMessage="Please select Menu Level" Display="Dynamic" ValidationGroup="Save"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Parent Menu Name:<span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="ddlParentMenu" CssClass="form-select" runat="server"
                                        DataTextField="MenuName" DataValueField="MenuId">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvParentMenu" runat="server" ControlToValidate="ddlParentMenu" CssClass="text-danger"
                                        ErrorMessage="Please select Parent Menu Name" Display="Dynamic" ValidationGroup="Save"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Menu URL:<span class="text-danger">*</span></label>
                                    <F2FControls:F2FTextBox CssClass="form-control"  ID="txtMenuURL" MaxLength="500" runat="server">
                                        </F2FControls:F2FTextBox>
                                     <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtMenuURL" />
                                    <asp:RequiredFieldValidator ID="rfvMenuURL" runat="server" ControlToValidate="txtMenuURL" CssClass="text-danger"
                                        ErrorMessage="Please enter Menu URL" Display="Dynamic" ValidationGroup="Save"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Sort Order:<span class="text-danger">*</span></label>
                                    <F2FControls:F2FTextBox CssClass="form-control"  ID="txtSortOrder" MaxLength="500" runat="server">
                                        </F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredNumber runat="server" TargetControlID="txtSortOrder" />
                                    <asp:RequiredFieldValidator ID="rfvSortOrder" runat="server" ControlToValidate="txtSortOrder" CssClass="text-danger"
                                        ErrorMessage="Please enter Sort Order" Display="Dynamic" ValidationGroup="Save"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Status:<span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="ddlStatus" CssClass="form-select" runat="server">
                                        <asp:ListItem Text="-- Select --" Value=""></asp:ListItem>
                                        <asp:ListItem Text="Active" Value="A"></asp:ListItem>
                                        <asp:ListItem Text="Inactive" Value="I"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus" CssClass="text-danger"
                                        ErrorMessage="Please select Status" Display="Dynamic" ValidationGroup="Save"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="text-center mt-3">
                                <asp:LinkButton ID="btnSave" CssClass="btn btn-outline-success" Text="Save" runat="server" ValidationGroup="Save"
                                    OnClick="btnSave_Click" >
                                    <i class="fa fa-save me-2"></i> Save                    
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnCancel" CssClass="btn btn-outline-danger" Text="Cancel" runat="server" OnClick="btnCancel_Click" >
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
