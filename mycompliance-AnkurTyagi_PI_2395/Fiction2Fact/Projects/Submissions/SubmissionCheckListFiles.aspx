<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Submissions.Submissions_SubmissionCheckListFiles" Title="Checklist Files" CodeBehind="SubmissionCheckListFiles.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Submission Files</title>
    <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />
    <script type="text/javascript">

        //window.addEventListener("beforeunload", function (e) {
        //    window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnRefresh').click();
        //});

        function deleteRecord() {
            if (!confirm('Are you sure that you want to delete this record?')) return false;
            return true;
        }
        function onClientSaveClick() {

            var fuctrl = document.getElementById('fuSubmissionFiles').value;
            if (fuctrl == '') {
                alert("Please select a file.");
                return false;
            }

            var IsDoubleClickFlagSet = document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value;
            if (IsDoubleClickFlagSet == 'Yes') {
                alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                return false;
            }
            else {
                var validated = Page_ClientValidate('saveFiles');
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
</head>
<body class="d-block">
    <form id="form1" runat="server">

        <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />
        <asp:HiddenField ID="hfSubmissionCheklistId" runat="server" />
        <asp:HiddenField ID="hfFiletype" runat="server" />

        <div class="page-content">
            <div class="container-fluid">
                <!-- Page-Title -->
                <div class="row">
                    <div class="col-sm-12">
                        <div class="page-title-box">
                            <div class="row">
                                <div class="col">
                                    <h4 class="page-title mb-3" id="lblPageHeader" runat="server">Add Submission Files</h4>
                                    <span id="lblPageNote" runat="server" class="text-danger">Note : Click on done button after you have uploaded all the files.</span>
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
                                <asp:Panel ID="pnlUpload" runat="server">
                                    <div class="row">
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Select Type: <span class="text-danger">*</span></label>
                                            <asp:DropDownList ID="ddlType" AppendDataBoundItems="true" runat="server" DataValueField="RC_CODE"
                                                DataTextField="RC_NAME" CssClass="form-select">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="ddlType"
                                                ValidationGroup="saveFiles" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger" ErrorMessage="Please select type."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Description: <span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server" TextMode="MultiLine"
                                                Rows="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDescription" CssClass="text-danger"
                                                ValidationGroup="saveFiles" Display="Dynamic" SetFocusOnError="True"
                                                ErrorMessage="Please enter description"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Files Upload :</label>
                                            <div class="input-group">
                                                <asp:FileUpload ID="fuSubmissionFiles" runat="server"
                                                    CssClass="form-control" />
                                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnUploadSubFiles" runat="server" ValidationGroup="saveFiles"
                                                    Text="Upload" OnClick="btnUploadSubFiles_Click" OnClientClick="return onClientSaveClick()">
                                                <i class="fa fa-upload"></i> Upload 
                                                </asp:LinkButton>
                                            </div>
                                            <asp:RegularExpressionValidator ID="revSubmissionFiles" runat="server" ControlToValidate="fuSubmissionFiles" CssClass="span"
                                                Display="Dynamic" ErrorMessage="Upload of this file format is not supported."></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="mt-3">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvFileUpload" runat="server" AllowPaging="false" AllowSorting="false" CssClass="table table-bordered footable"
                                                OnRowDataBound="gvFileUpload_RowDataBound" GridLines="None" CellPadding="4" DataKeyNames="FileNameOnServer"
                                                OnSelectedIndexChanged="gvFileUpload_SelectedIndexChanged" AutoGenerateColumns="false">
                                                <Columns>
                                                    <%--<asp:CommandField ButtonType="Link" ShowSelectButton="true" HeaderText="Delete" SelectText="Delete" />--%>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbDelete" runat="server" CssClass="btn btn-sm btn-soft-danger btn-circle" CausesValidation="False" CommandName="Select">
                                                                   <i class="fa fa-trash"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="File Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFileType" Text='<%#Eval("File Type Name")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="File Description">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFileDescription" Text='<%#Eval("File Description")%>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="File Name">
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=SubmissionFiles&downloadFileName=<%#getFileName(Eval("FileNameOnServer"))%>&Filename=<%#getFileName(Eval("File Name"))%>','','location=0,status=0,scrollbars=1,width=400,height=200');">
                                                                <%#Eval("File Name")%></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Uploaded by" DataField="Uploaded by" />
                                                    <asp:BoundField HeaderText="Uploaded on" DataField="Uploaded on" />
                                                </Columns>
                                                <%-- <FooterStyle CssClass="GridFooter" />
                                            <RowStyle CssClass="GridRow" />
                                            <SelectedRowStyle CssClass="GridSelectedRow" />
                                            <PagerStyle CssClass="GridPager" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAlternatingRow" />--%>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="mt-3 text-center">
                                        <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnDone" CausesValidation="true" ValidationGroup="SaveQueryDetails"
                                            runat="server" Text="Done" OnClick="btnDone_Click">
                                        Done                          
                                        </asp:LinkButton>
                                    </div>
                                </asp:Panel>
                                <div class="mt-3">
                                    <div class="card mb-1 mt-1 border">
                                        <div class="card-header py-0 custom-ch-bg-color">
                                            <h6 class="font-weight-bold text-white mtb-5">Documents attached: </h6>
                                        </div>
                                        <div class="card-body mt-1">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gvUploadedFile" runat="server" BorderStyle="None" BorderWidth="1px" CssClass="table table-bordered footable"
                                                    CellPadding="4" GridLines="None" AutoGenerateColumns="False" DataKeyNames="SF_ID"
                                                    OnSelectedIndexChanged="gvUploadedFile_SelectedIndexChanged">
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblServerFileName" Text='<%#Eval("SF_SERVER_FILE_NAME") %>' runat="server"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbDelete" runat="server" CssClass="btn btn-sm btn-soft-danger btn-circle" CausesValidation="False" CommandName="Select"
                                                                    OnClientClick="return deleteRecord();">
                                                                   <i class="fa fa-trash"></i>
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="File Type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSF_FILE_TYPE" Text='<%#Eval("RC_NAME") %>' runat="server"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="File Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSF_FILE_DESCRIPTION" Text='<%#Eval("SF_FILE_DESCRIPTION") %>' runat="server"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="File Name">
                                                            <ItemTemplate>
                                                                <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=SubmissionFiles&downloadFileName=<%#getFileName(Eval("SF_SERVER_FILE_NAME"))%>&Filename=<%#getFileName(Eval("SF_FILE_NAME"))%>','','location=0,status=0,scrollbars=1,width=400,height=200');">
                                                                    <%#Eval("SF_FILE_NAME")%>
                                                                </a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Uploaded By">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUploaderName" runat="server" Text='<%# Getfullname(Eval("SF_UPLOADER").ToString()) %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Uploaded On">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCSFCreateDate" runat="server" Text='<%# Eval("SF_CREATE_DT", "{0:dd-MMM-yyyy HH:mm:ss}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Operation">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOType" runat="server" Text='<%# Eval("SF_OPERATION_TYPE").ToString() %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- end row -->
            </div>
        </div>

    </form>
</body>
</html>
