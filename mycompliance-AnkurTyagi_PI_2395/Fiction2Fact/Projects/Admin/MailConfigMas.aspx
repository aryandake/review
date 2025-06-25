<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Admin.CommonMasters_MailConfigMas" Title=":: ORMS : Email Content Master ::" CodeBehind="MailConfigMas.aspx.cs" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- <link href="../../Content/js/legacy/jquery-ui.structure.css" rel="stylesheet" />
    <link href="../../Content/js/legacy/jquery-ui.theme.css" rel="stylesheet" />
    <script src="../../Content/js/legacy/jquery-ui.js"></script>
    --%>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/ckeditor/ckeditor.js")%>"></script>
    <style>
        .ck-editor__editable {
            min-height: 250px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if (document.querySelector('#ctl00_ContentPlaceHolder1_FCKContent') != null) {
                ClassicEditor
                    .create(document.querySelector('#ctl00_ContentPlaceHolder1_FCKContent'), {
                        // toolbar: [ 'heading', '|', 'bold', 'italic', 'link' ]
                    })
                    .then(editor => {
                        window.editor = editor;
                    })
                    .catch(err => {
                        console.error(err.stack);
                    });
            }
        });
    </script>
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
                var validated = Page_ClientValidate('AddMailConfig');
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

    <script type="text/javascript">
        $(function () {
            $(document).tooltip({
                position: {
                    my: "center bottom-10",
                    at: "center top",
                    using: function (position, feedback) {
                        $(this).css(position);
                    }
                }
            });
        });

    </script>

    <%--Added By Milan Yadav on 19-Nov-2016--%>
    <link type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/css/legacy/ui-lightness/jquery-ui-1.8.19.custom.css")%>" rel="stylesheet" />

    <%--<script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/jquery-ui-1.8.19.custom.min.js")%>"></script>--%>
    <style type="text/css">
        .ui-tooltip, .arrow:after {
            background: #E5F3FC; /* #FFFFFF; #EBEBEB; #FFFF80;*/
            border: 0px solid #5ACCF3;
        }

        .ui-tooltip {
            padding: 10px 10px;
            color: black;
            border-radius: 5px;
            font: normal 12px Trebuchet MS;
            /*text-transform: uppercase;*/
            box-shadow: 0 0 7px black;
        }
    </style>
    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField runat="server" ID="hfDoubleClickFlag" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Email Content Master</h4>
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
                <asp:MultiView ID="mvMailConfig" runat="server">
                    <asp:View runat="server" ID="vwSearch">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Mail Config Id</label>
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtMailConfigId" MaxLength="200" runat="server"
                                        Columns="10"></F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredNumber runat="server" TargetControlID="txtMailConfigId" />
                                    <ajaxToolkit:FilteredTextBoxExtender ID="fteId" runat="server" TargetControlID="txtMailConfigId"
                                        FilterType="Custom,Numbers" ValidChars="" InvalidChars=""></ajaxToolkit:FilteredTextBoxExtender>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Module Name</label>
                                    <asp:DropDownList ID="ddlSearchModuleName" CssClass="form-select" runat="server"
                                        DataTextField="RC_NAME" DataValueField="RC_CODE">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Configuration Type</label>
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtConfigType" MaxLength="200" runat="server"
                                        Columns="50"></F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtConfigType" />
                                </div>
                            </div>
                            <div class="text-center mt-3">
                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnSearch" Text="Search" OnClick="btnSearch_Click"
                                    runat="server" ValidationGroup="SEARCH">
                                    <i class="fa fa-search"></i> Search                     
                                </asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnAddMailConfig" Text="Add" OnClick="btnAddMailConfig_Click"
                                    runat="server">
                                    <i class="fa fa-plus"></i> Add                               
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="imgExcel" OnClick="btnExportToExcel_Click" CssClass="btn btn-outline-secondary"
                                    Visible="false">
                                    <i class="fa fa-download"></i> Export to Excel               
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gvMailConfig" runat="server" AutoGenerateColumns="False" DataKeyNames="MCM_ID"
                                    AllowSorting="true" AllowPaging="true" GridLines="Both" CellPadding="4" CssClass="table table-bordered footable"
                                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnSelectedIndexChanged="gvMailConfig_SelectedIndexChanged"
                                    OnPageIndexChanging="gvMailConfig_PageIndexChanging" OnSorting="gvMailConfig_Sorting" EmptyDataText="No records found satisfying this criteria.">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Sr.No.
                                       
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MCM_ID" HeaderText="Mail Config Id" />
                                        <asp:BoundField DataField="ModuleName" HeaderText="Module Name" />
                                        <asp:BoundField DataField="MCM_TYPE" HeaderText="Configuration Type" />
                                        <asp:BoundField DataField="MCM_SUBJECT" HeaderText="Subject" />
                                        <asp:TemplateField HeaderText="Mail Content">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContent" Width="350px" runat="server" Text='<%# Bind("MCM_CONTENT") %>'
                                                    Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MCM_REC_STATUS" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="Edit" ShowHeader="true">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select"
                                                    CssClass="btn btn-sm btn-soft-success btn-circle" OnClientClick="onClientEditClick()">
                                                    <i class="fa fa-pen"></i>	                            
                                            </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Delete" ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Select" Text="&lt;img alt='Delete' src='../../Content/images/legacy/delete_1.png' border='0' &gt;"
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
                                    <label class="form-label">Mail Config ID :</label>
                                    <asp:Label ID="lblID" runat="server" CssClass="form-control custom-span-input"></asp:Label>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Module Name :</label>
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddlModuleName" CssClass="form-select" runat="server"
                                            DataTextField="RC_NAME" DataValueField="RC_CODE">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="ImageButton7" CssClass="custom-calendar-icon" runat="server" ImageUrl="../../Content/images/legacy/info1.jpg" OnClientClick="return false;"
                                            ToolTip="Specify Module Name for which mail content to be entered." />
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvModuleName" runat="server" ControlToValidate="ddlModuleName"
                                        Display="Dynamic" ValidationGroup="AddMailConfig" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Mail Configuration Type :</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox CssClass="form-control" ID="txtDesc" MaxLength="2000" runat="server" Columns="80"></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtDesc" />
                                        <asp:ImageButton ID="ImageButton1" runat="server" CssClass="custom-calendar-icon" ImageUrl="../../Content/images/legacy/info1.jpg" OnClientClick="return false;"
                                            ToolTip="Specify action at which mail will be sent." />
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="txtDesc"
                                        Display="Dynamic" ValidationGroup="AddMailConfig" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Status :</label>
                                    <div class="input-group">
                                        <asp:DropDownList CssClass="form-select" ID="ddlStatus" runat="server">
                                            <asp:ListItem Value="">Select Status</asp:ListItem>
                                            <asp:ListItem Value="A">Active</asp:ListItem>
                                            <asp:ListItem Value="I">In Active</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="ImageButton2" runat="server" CssClass="custom-calendar-icon" ImageUrl="../../Content/images/legacy/info1.jpg" OnClientClick="return false;"
                                            ToolTip="Select Active / Inactive." />
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus"
                                        Display="Dynamic" ValidationGroup="AddMailConfig" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-8 mb-3">
                                    <label class="form-label">Subject :</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox CssClass="form-control" ID="txtSubject" MaxLength="200" runat="server" Columns="80"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="ImageButton5" runat="server" CssClass="custom-calendar-icon" ImageUrl="../../Content/images/legacy/info1.jpg" OnClientClick="return false;"
                                            ToolTip="Specify subject of mail." />
                                        <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSubject" />
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject"
                                        Display="Dynamic" ValidationGroup="AddMailConfig" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-12 mb-3">
                                    <label class="form-label">
                                        Content : 
                                        <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="../../Content/images/legacy/info1.jpg" OnClientClick="return false;"
                                            ToolTip="Enter content of mail, which will be triggered from system based on activity performed." /></label>

                                    <F2FControls:F2FTextBox runat="server" ID="FCKContent" TextMode="MultiLine" CssClass="ckeditor"></F2FControls:F2FTextBox>
                                </div>
                            </div>
                            <div class="text-center mt-3">
                                <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSave" Text="Save" OnClick="btnSave_Click"
                                    runat="server" ValidationGroup="AddMailConfig" OnClientClick="return onClientSaveClick()">
                                    <i class="fa fa-save me-2"></i> Save                     
                                </asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click"
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
