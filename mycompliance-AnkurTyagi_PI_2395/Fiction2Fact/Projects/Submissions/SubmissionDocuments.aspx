<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Submissions.Submissions_SubmissionDocuments" CodeBehind="SubmissionDocuments.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Relevant​ Files</title>
    <link id="Link2" rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css")%>" />
 <%-- // << code Added by ramesh more on 13-Mar-2024 CR_1991 for VAPT--%>
    <script type="text/javascript">
        if (window.opener == null || window.opener.location == null) {
            window.location = '../Login.aspx';
        }
    </script>
<%-- // >>--%>
</head>
<body class="d-block">
    <form id="form1" runat="server">
        <div class="page-content">
            <div class="container-fluid">

                <!-- Page-Title -->
                <div class="row">
                    <div class="col-sm-12">
                        <div class="page-title-box">
                            <div class="row">
                                <div class="col">
                                    <h4 class="page-title">​Relevant​ Files</h4>
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
                                <asp:GridView ID="gvFileUpload" runat="server" BorderStyle="None" BorderWidth="1px"
                                    CellPadding="4" GridLines="None" AutoGenerateColumns="False" DataKeyNames="SMF_ID"
                                    CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:TemplateField HeaderText="File Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFileType" runat="server" Text='<%# Eval("FileType") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="File Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFileDescription" runat="server" Text='<%# Eval("SMF_FILE_DESC").ToString().Replace(Environment.NewLine, "<br />") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="File Name">
                                            <ItemTemplate>
                                                <a href="javascript:void(0);" class="badge rounded-pill badge-soft-pink" onclick="javascript:window.open('../CommonDownload.aspx?type=SubmissionFiles&downloadFileName=<%# Eval("SMF_SERVER_FILE_NAME")%>&fileName=<%#getFileName(Eval("SMF_FILE_NAME"))%>','','location=0,status=0,scrollbars=0,width=10,height=10');">
                                                    <%#Eval("SMF_FILE_NAME")%>
                                                 </a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div class="mt-3">
                                    <asp:LinkButton ID="btnClose" runat="server" Visible="true" CssClass="btn btn-outline-danger" Text="Close"
                                        OnClick="btnClose_Click">
                                        <i class="fa fa-arrow-left me-2"></i> Close                   
                                    </asp:LinkButton>
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
