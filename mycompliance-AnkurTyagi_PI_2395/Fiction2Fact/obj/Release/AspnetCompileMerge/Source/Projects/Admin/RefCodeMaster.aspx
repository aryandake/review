<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Admin.RefCodeMaster" Title="Reference Codes" CodeBehind="RefCodeMaster.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
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

        function onClientSaveClick() {
            var IsDoubleClickFlagSet = document.getElementById('ctl00_ContentPlaceHolder1_hfDoubleClickFlag').value;
            if (IsDoubleClickFlagSet == 'Yes') {
                alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                return false;
            }
            else {
                var validated = Page_ClientValidate('AddReference');
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

    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField runat="server" ID="hfDoubleClickFlag" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Reference Code Master</h4>
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
                <asp:MultiView ID="mvRefCode" runat="server">
                    <asp:View runat="server" ID="vwSearch">
                        <asp:Panel runat="server" ID="pnlSearch" DefaultButton="imgSearch">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Reference Type</label>
                                        <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlSearchRefType" DataTextField="RC_TYPE"
                                            DataValueField="RC_TYPE" runat="server">
                                        </f2f:DropdownListNoValidation>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Reference Code</label>
                                        <F2FControls:F2FTextBox CssClass="form-control" ID="txtSearchRefCode" MaxLength="100" runat="server"
                                            Columns="30"></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSearchRefCode" />
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <label class="form-label">Reference Name</label>
                                        <F2FControls:F2FTextBox CssClass="form-control" ID="txtSearchRefName" MaxLength="100" runat="server"
                                            Columns="30"></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSearchRefName" />
                                    </div>
                                </div>
                                <div class="text-center mt-3">
                                    <asp:LinkButton CssClass="btn btn-outline-primary" ID="imgSearch" Text="Search" OnClick="btnSearch_Click"
                                        runat="server" ValidationGroup="SEARCH">
                                        <i class="fa fa-search"></i> Search                     
                                    </asp:LinkButton>
                                    <asp:LinkButton CssClass="btn btn-outline-primary" ID="imgAdd" Text="Add" OnClick="btnAddReference_Click"
                                        runat="server">
                                        <i class="fa fa-plus"></i> Add                               
                                    </asp:LinkButton>
                                    <asp:LinkButton CssClass="btn btn-outline-secondary" ID="imgExcel" Text="Export to Excel" OnClick="btnExportToExcel_Click"
                                        runat="server" Visible="false">
                                        <i class="fa fa-download"></i> Export to Excel               
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gvRefCode" runat="server" AutoGenerateColumns="False" DataKeyNames="RC_ID"
                                    AllowSorting="false" AllowPaging="true" GridLines="Both" CellPadding="4" CssClass="table table-bordered footable"
                                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnSelectedIndexChanged="gvRefCode_SelectedIndexChanged"
                                    OnPageIndexChanging="gvRefCode_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Sr.No.
                       
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="RC_TYPE" HeaderText="Reference Type " />
                                        <asp:BoundField DataField="RC_NAME" HeaderText="Reference Name" />
                                        <asp:BoundField DataField="RC_CODE" HeaderText="Reference Code" />
                                        <asp:BoundField DataField="RC_STATUS" HeaderText="Status" />
                                        <asp:BoundField DataField="RC_SORT_ORDER" HeaderText="Sort Order" />
                                        <asp:TemplateField HeaderText="Edit" ShowHeader="true">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select"  CssClass="btn btn-sm btn-soft-success btn-circle" OnClientClick="onClientEditClick()">
                                                    <i class="fa fa-pen"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Delete" ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Select" Text="&lt;img alt='Delete' src='<%=Fiction2Fact.Global.site_url("Content/images/legacy/delete_1.png") %>' border='0' &gt;"
                                                CssClass="centerLink" OnClientClick="onClientDeleteClick()">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
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
                                    <asp:Label ID="lblID" runat="server" CssClass="form-control custom-span-input"></asp:Label>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Reference Type : <span class="text-danger">*</span></label>
                                    <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlRefType" 
                                        DataTextField="RC_TYPE" DataValueField="RC_TYPE" runat="server">
                                    </f2f:DropdownListNoValidation>
                                    <asp:RequiredFieldValidator ID="rfvRefType" runat="server" ControlToValidate="ddlRefType" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReference" SetFocusOnError="True"> Please enter Abbreviation Type.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Reference Code : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtRefCode" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtRefCode" />
                                    <asp:RequiredFieldValidator ID="rfvRefCode" runat="server" ControlToValidate="txtRefCode" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReference" SetFocusOnError="True"> Please enter Abbreviation Code.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Reference Name : <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtRefName" MaxLength="200" runat="server" Columns="50"></asp:TextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtRefName" />
                                    <asp:RequiredFieldValidator ID="rfvRefName" runat="server" ControlToValidate="txtRefName" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReference" SetFocusOnError="True"> Please enter Abbreviation Name.</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Sort Order :</label>
                                    <asp:TextBox CssClass="form-control" ID="txtSortOrder" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="fteSortOrder" runat="server"
                                        TargetControlID="txtSortOrder" FilterType="Custom, Numbers" ValidChars=""></ajaxToolkit:FilteredTextBoxExtender>
                                    <%--<asp:RequiredFieldValidator ID="rfvSortOrder" runat="server" ControlToValidate="txtSortOrder" CssClass="text-danger"
                                            Display="Dynamic" ValidationGroup="AddReference" SetFocusOnError="True"> Please enter Sort Order.</asp:RequiredFieldValidator>--%>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Status : <span class="text-danger">*</span></label>
                                    <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlStatus" runat="server">
                                        <asp:ListItem Text="-- Select --" Value=""></asp:ListItem>
                                        <asp:ListItem Text="Active" Value="A"></asp:ListItem>
                                        <asp:ListItem Text="Inactive" Value="I"></asp:ListItem>
                                    </f2f:DropdownListNoValidation>
                                    <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="AddReference" SetFocusOnError="True"> Please enter Abbreviation Status.</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="text-center mt-3">
                                <asp:LinkButton CssClass="btn btn-outline-success" ID="imgSave" Text="Save" OnClick="btnSave_Click"
                                    runat="server" ValidationGroup="AddReference" OnClientClick="return onClientSaveClick()" >
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
