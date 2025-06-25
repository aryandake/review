<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Admin.Admin_CommonMasterEntryPage"
    Title=":: TRU-RISK : Common Masters ::" CodeBehind="CommonMasterEntryPage.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        function onAddClick() {
            document.getElementById('<%=hdfSelectedOperation.ClientID%>').value = "Add";
        }

        function onEditClick() {
            document.getElementById('<%=hdfSelectedOperation.ClientID%>').value = "Edit";
        }

        function onDeleteClick() {
            if (!confirm('Are you sure that you want to delete this record?')) return false;
            document.getElementById('<%=hdfSelectedOperation.ClientID%>').value = "Delete";
        }

        function onMasterSelect(id, name) {
            document.getElementById('<%=hdfSelectedOperation.ClientID%>').value = "PopulateSearchParams";
            document.getElementById('<%=hfSelectedMasterId.ClientID%>').value = id;
            document.getElementById('<%=hfSelectedMasterName.ClientID%>').value = name;
        }

    </script>

    <asp:SqlDataSource ID="sdsMasterEntryList" runat="server" ConnectionString="<%$ ConnectionStrings:MsSQL %>"
        SelectCommand="SELECT [Name], [MasterEntryDetailsId] FROM [MasterEntryDetails]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsAllData" runat="server" DataSourceMode="DataReader"></asp:SqlDataSource>
    <asp:HiddenField ID="hfSelectedMasterId" runat="server" Value="0" />
    <asp:HiddenField ID="hdfSelectedOperation" runat="server" />
    <asp:HiddenField ID="hfSelectedRecordId" runat="server" />
    <asp:HiddenField ID="hfMasterID" runat="server" />
    <asp:HiddenField ID="hfSelectedMasterName" runat="server" />
    <asp:HiddenField ID="hfFilterCondition" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Masters</h4>
                        <asp:Label ID="lblInfo" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
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
                        <div class="col-3">
                            <div class="table-responsive">
                                <asp:Table ID="masterListTable" runat="server" CssClass="table table-bordered footable">
                                    <asp:TableRow>
                                        <asp:TableCell CssClass="custom-common-master">Main Menu</asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                        </div>
                        <div class="col-9">
                            <div class="card">
                                <div class="card-header border-bottom">
                                    <asp:Label ID="lblSelectedMasterName" runat="server"></asp:Label>
                                </div>
                                <div class="card-body">
                                    <asp:Panel ID="SearchPanel" runat="server" DefaultButton="btnSearch">
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <div class="row">
                                                    <div class="col-sm-3 align-self-center">
                                                        <asp:Label ID="lblSearchBy" CssClass="form-label mb-0" runat="server">Search By:<span class="text-danger">*</span></asp:Label>
                                                    </div>
                                                    <div class="col-sm-9">
                                                        <asp:DropDownList CssClass="form-select" ID="ddlSearchBy" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlSearchBy_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList CssClass="form-select" ID="ddlFieldType" runat="server" Style="display: none; visibility: hidden">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvSearchBy" runat="server" ControlToValidate="ddlSearchBy" CssClass="text-danger"
                                                            ErrorMessage="Please select the Search By." Display="Dynamic" EnableClientScript="true"
                                                            ValidationGroup="OnSearchButtonClick"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnSearch" runat="server" AccessKey="s" Text="Search"
                                                    OnClick="btnSearch_Click" CausesValidation="true" ValidationGroup="OnSearchButtonClick">
                                                        <i class="fa fa-search"></i> Search
                                                </asp:LinkButton>
                                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                                                    OnClientClick="onAddClick()" Text="Add New Record">
                                                        <i class="fa fa-plus"></i> Add
                                                </asp:LinkButton>
                                                <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcel" runat="server" OnClick="btnExportToExcel_Click"
                                                    Text="Export To Excel">
                                                        <i class="fa fa-download"></i> Export To Excel
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="TextPanel" DefaultButton="btnSearch" runat="server" Visible="False">
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <div class="row">
                                                    <div class="col-sm-3 align-self-center">
                                                        <asp:Label ID="lblTextSearchLabel" CssClass="form-label mb-0" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-9">
                                                        <asp:TextBox CssClass="form-control" ID="txtSearchText" runat="server" Width="300px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvSearchText" runat="server" ControlToValidate="txtSearchText" CssClass="text-danger"
                                                            ErrorMessage="Please Enter the Search Text." Display="Dynamic" EnableClientScript="true"
                                                            ValidationGroup="OnSearchButtonClick"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                &nbsp;
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel DefaultButton="btnSearch" ID="TextRangePanel" runat="server" Visible="False">
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <div class="row">
                                                    <div class="col-sm-3 align-self-center">
                                                        <asp:Label ID="lblTextRangeFromLabel" CssClass="form-label mb-0" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-9">
                                                        <asp:TextBox CssClass="form-control" ID="txtSearchTextRangeFrom" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvSearchTextRangeFrom" runat="server" ControlToValidate="txtSearchTextRangeFrom" CssClass="text-danger"
                                                            ErrorMessage="Please Enter the Search Text Range From." Display="Dynamic" EnableClientScript="true"
                                                            ValidationGroup="OnSearchButtonClick"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <div class="row">
                                                    <div class="col-sm-3 align-self-center">
                                                        <asp:Label ID="lblTextRangeToLabel" CssClass="form-label mb-0" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-9">
                                                        <asp:TextBox CssClass="form-control" ID="txtSearchTextRangeTo" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvSearchTextRangeTo" runat="server" ControlToValidate="txtSearchTextRangeTo" CssClass="text-danger"
                                                            ErrorMessage="Please Enter the Search Text Range To." Display="Dynamic" EnableClientScript="true"
                                                            ValidationGroup="OnSearchButtonClick"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                &nbsp;
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel DefaultButton="btnSearch" ID="DatePanel" runat="server" Visible="False">
                                        <div class="row">
                                            <div class="col-sm-2 align-self-center">
                                                <asp:Label ID="lblDateSearchLabel" CssClass="form-label mb-0" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-sm-10">
                                                <asp:TextBox CssClass="form-control" ID="txtSearchDate" runat="server" Columns="11" Width="260px"></asp:TextBox>&nbsp;<asp:ImageButton
                                                    ID="ImageButton2" runat="server" ImageUrl="~/images/calendar.jpg" />

                                                <asp:RequiredFieldValidator ID="rfvSearchDate" runat="server" ControlToValidate="txtSearchDate" CssClass="text-danger"
                                                    ErrorMessage="Please Enter The Search Date." Display="Dynamic" EnableClientScript="true"
                                                    ValidationGroup="OnSearchButtonClick"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel DefaultButton="btnSearch" ID="DateRangePanel" runat="server" Visible="False">
                                        <div class="row">
                                            <div class="col-sm-2 align-self-center">
                                                <asp:Label ID="lblDateRangeFromLabel" CssClass="form-label mb-0" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-sm-10">
                                                <asp:TextBox CssClass="form-control" ID="txtSearchDateRangeFrom" runat="server" Columns="11" Width="260px"></asp:TextBox>&nbsp;<asp:ImageButton
                                                    ID="ImageButton1" runat="server" ImageUrl="~/images/calendar.jpg" />
                                                <asp:RequiredFieldValidator ID="rfvSearchDateRangeFrom" runat="server" ControlToValidate="txtSearchDateRangeFrom" CssClass="text-danger"
                                                    ErrorMessage="Please Enter The Search Date Range From." Display="Dynamic" EnableClientScript="true"
                                                    ValidationGroup="OnSearchButtonClick"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-sm-2 align-self-center">
                                                <asp:Label ID="lblDateRangeToLabel" CssClass="form-label mb-0" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-sm-10">
                                                <asp:TextBox CssClass="form-control" ID="txtSearchDateRangeTo" runat="server" Columns="11" Width="260px"></asp:TextBox>
                                                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/images/calendar.jpg" />
                                                <asp:RequiredFieldValidator ID="rfvSearchDateRangeTo" runat="server" ControlToValidate="txtSearchDateRangeTo" CssClass="text-danger"
                                                    ErrorMessage="Please Enter The Search Date Range To." Display="Dynamic" EnableClientScript="true"
                                                    ValidationGroup="OnSearchButtonClick"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel DefaultButton="btnSearch" ID="DropdownPanel" runat="server" Visible="False">
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <div class="row">
                                                    <div class="col-sm-2 align-self-center">
                                                        <asp:Label ID="lblDropdownSearchLabel" CssClass="form-label mb-0" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-10">
                                                        <asp:DropDownList CssClass="form-select" ID="ddlSearchValues" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                             <div class="col-md-6 mb-3">
                                                 &nbsp;
                                             </div>
                                        </div>
                                    </asp:Panel>

                                    <asp:Label ID="lblInfoMsg" runat="server" CssClass="custom-alert-box" Visible="false"></asp:Label>
                                    <div class="card-body">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvAllRecords" runat="server" OnSelectedIndexChanged="gvAllRecords_SelectedIndexChanged"
                                                CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                GridLines="both" DataKeyNames="Id_Col" HeaderStyle-HorizontalAlign="Center"
                                                OnPageIndexChanging="gvAllRecords_PageIndexChanging" PageIndex="20" AllowPaging="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr.No." HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lblSrNo" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Edit" ItemStyle-CssClass="text-center">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbEdit" runat="server" CausesValidation="False" CommandName="Select"
                                                                CssClass="btn btn-sm btn-soft-success btn-circle"
                                                                OnClientClick="onEditClick()">
                                                            <i class="fa fa-pen"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete" ItemStyle-CssClass="text-center">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:LinkButton ID="lbDelete" runat="server" CausesValidation="False" CommandName="Select" CssClass="btn btn-sm btn-soft-danger btn-circle"
                                                                    OnClientClick="return onDeleteClick()">
                                                                    <i class="fa fa-trash"></i>
                                                                </asp:LinkButton>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <asp:Table CellPadding="3" CellSpacing="0" ID="masterEntryTable"
                                        runat="server">
                                    </asp:Table>

                                    <div class="text-center mt-2">
                                        <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSave" runat="server" OnClick="btnSave_Click"
                                            Text="Save" ValidationGroup="Save">
                                                <i class="fa fa-save"></i> Save
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnCancel" runat="server" OnClick="btnCancel_Click"
                                            Text="Cancel" CausesValidation="False">
                                                <i class="fa fa-arrow-left"></i> Cancel
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->


</asp:Content>
