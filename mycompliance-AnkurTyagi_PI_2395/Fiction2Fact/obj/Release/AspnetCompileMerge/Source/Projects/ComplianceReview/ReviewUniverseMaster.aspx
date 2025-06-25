<%@ Page Title="" Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" CodeBehind="ReviewUniverseMaster.aspx.cs" Inherits="Fiction2Fact.Projects.Compliance.ReviewUniverseMaster" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <script>
        function onClientEditClick() {
            document.getElementById('<%=hfSelectOperation1.ClientID%>').value = "Edit";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfSelectOperation1" runat="server" />
    <asp:HiddenField ID="hfUniverseId" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Review Universe Master</h4>
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
                <asp:MultiView ID="mvUniverseMaster" runat="server">
                    <asp:View runat="server" ID="vwSearch">
                        <asp:Panel runat="server" ID="pnlSearch" DefaultButton="imgSearch">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Area / Universe to be reviewed</label>
                                        <F2FControls:F2FTextBox CssClass="form-control" ID="txtSearchAreaUniverse" runat="server"></F2FControls:F2FTextBox>
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
                                                runat="server" ValidationGroup="SEARCH">
                                            <i class="fa fa-search"></i> Search                     
                                            </asp:LinkButton>
                                            <asp:LinkButton CssClass="btn btn-outline-primary" ID="imgAdd" Text="Add" OnClick="btnAdd_Click"
                                                runat="server">
                                            <i class="fa fa-plus"></i> Add                               
                                            </asp:LinkButton>
                                            <asp:LinkButton CssClass="btn btn-outline-secondary" runat="server" ID="imgExcel" OnClick="btnExportToExcel_Click"
                                                Text="Export To Excel" Visible="false">
                                            <i class="fa fa-download"></i> Export to Excel               
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gvAreaUniverseMaster" runat="server" AutoGenerateColumns="False" DataKeyNames="CRUM_ID"
                                    AllowSorting="false" AllowPaging="true" GridLines="Both" CellPadding="4" CssClass="table table-bordered footable"
                                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnSelectedIndexChanged="gvAreaUniverseMaster_SelectedIndexChanged"
                                    OnPageIndexChanging="gvAreaUniverseMaster_PageIndexChanging">
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

                                        <asp:TemplateField HeaderText="Area/Universe To Be Reviewed" SortExpression="CRUM_UNIVERSE_TO_BE_REVIEWED">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAreaUniverse" runat="server" Text='<%# Bind("CRUM_UNIVERSE_TO_BE_REVIEWED") %>'
                                                    Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("CRUM_STATUS").ToString()=="A"?"Active":"Inactive" %>'
                                                    Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit" ShowHeader="true">
                                            <ItemTemplate>
                                                <asp:LinkButton CssClass="btn btn-sm btn-soft-success btn-circle" ID="lnkEdit" runat="server" CommandName="Select"
                                                    OnClientClick="onClientEditClick()">
                                                    <i class="fa fa-pen"></i>	                            
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
                                <div class="col-md-4 mb-3 d-none">
                                    <label class="form-label">Id :</label>
                                    <asp:Label ID="lblID" runat="server" Visible="false" CssClass="form-control custom-span-input"></asp:Label>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Area / Universe to be reviewed : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtAreaUniverse" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvAreaUniverse" ValidationGroup="Save" ControlToValidate="txtAreaUniverse"
                                        ErrorMessage="Please enter area / universe to be reviewed" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Status : <span class="text-danger">*</span></label>
                                    <asp:DropDownList CssClass="form-select" ID="ddlStatus" runat="server">
                                        <asp:ListItem Text="Select" Value="">
                                             </asp:ListItem>
                                        <asp:ListItem Text="Active" Value="A">
                                        </asp:ListItem>
                                        <asp:ListItem Text="Inactive" Value="I">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True">Please select status.</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="text-center mt-3">
                                <asp:LinkButton CssClass="btn btn-outline-success" ID="imgSave" Text="Save" OnClick="btnSave_Click"
                                    runat="server" ValidationGroup="Save">
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
