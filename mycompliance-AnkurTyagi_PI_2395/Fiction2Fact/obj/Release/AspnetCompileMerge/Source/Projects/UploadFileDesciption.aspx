<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadFileDesciption.aspx.cs" Inherits="Fiction2Fact.Projects.UploadFileDesciption" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <asp:PlaceHolder runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />
        <script type="text/javascript">
            function closeFileWindow() {
                var clientFileName = document.getElementById('<%=hfClientFileName.ClientID%>').value;
                var FileTypeID = document.getElementById('<%=hfFileTypeID.ClientID%>').value;
                var serverfilename = document.getElementById('<%=hfServerFileName.ClientID%>').value;
                var FileType = document.getElementById('<%=hfFileType.ClientID%>').value;
                var FileDesc = document.getElementById('<%=hfFileDesc.ClientID%>').value;

                var Type = document.getElementById('<%=hfType.ClientID%>').value;

                window.opener.onAppAttachUploaded(serverfilename, FileTypeID, clientFileName, FileType, FileDesc);

                window.close();
            }

        </script>
    </asp:PlaceHolder>
</head>
<body class="d-block">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <script type="text/javascript">
            if (window.opener == null || window.opener.location == null) {
                window.location = '<%= Fiction2Fact.Global.site_url() %>' + 'Login.aspx';
            }

            function onCloseClick() {
                window.close();
            }
    </script>
        <asp:HiddenField ID="hfFileTypeID" runat="server" />
        <asp:HiddenField ID="hfFileType" runat="server" />
        <asp:HiddenField ID="hfFileDesc" runat="server" />
        <asp:HiddenField ID="hfClientFileName" runat="server" />
        <asp:HiddenField ID="hfServerFileName" runat="server" />
        <asp:HiddenField ID="hfType" runat="server" />

        <div class="page-content">
            <div class="container-fluid">
                <!-- Page-Title -->
                <div class="row">
                    <div class="col-sm-12">
                        <div class="page-title-box">
                            <div class="row">
                                <div class="col">
                                    <h4 class="page-title">Upload File</h4>
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
                                    <div class="col-md-12 mb-3">
                                        <label class="form-label">Type :<span class="text-danger">*</span></label>
                                        <asp:DropDownList ID="ddlFileType" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvFileType" runat="server" ControlToValidate="ddlFileType"
                                            ErrorMessage="Please select File Type." ValidationGroup="SubmitGrp" CssClass="text-danger"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-12 mb-3">
                                        <label class="form-label">Description :</label>
                                        <asp:TextBox TextMode="MultiLine" runat="server" ID="txtFileDesc" MaxLength="4000"
                                            CssClass="form-control" Rows="5"></asp:TextBox>
                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtFileDesc" />
                                    </div>
                                    <div class="col-md-12 mb-3">
                                        <label class="form-label">Select File :</label>
                                        <asp:FileUpload ID="fuException" runat="server" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="rfvfuException" ControlToValidate="fuException" runat="server" CssClass="text-danger"
                                            ErrorMessage="Please choose File." Display="Dynamic" ValidationGroup="SubmitGrp"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="fuException" CssClass="span"
                                            Display="Dynamic" ErrorMessage="Upload of this file format is not supported."
                                            ValidationExpression="^.+(.msg|.MSG|.mp4|.MP4|.eml|.EML|.avi|.AVI|.flv|.FLV|.jpg|.JPG|.bmp|.BMP|
                                            .xls|.XLS|.xlsx|.XLSX|.DOC|.DOCX|.docx|.doc|.pdf|.PDF|.html|.htm|.HTML.|HTM|.tif|.TIF|.ZIP|.zip|.txt|.TXT|.MSG|.msg|.eml|.EML|.wav|.WAV|.WMA|.wma|.png|.PNG|.gif|.GIF)$"
                                            ValidationGroup="SubmitGrp"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="mt-3">
                                    <asp:LinkButton ID="btnProcess" CssClass="btn btn-outline-primary" runat="server" Text="Upload File" ValidationGroup="SubmitGrp"
                                        OnClick="btnProcess_Click" >
                                        <i class="fa fa-upload me-2"></i> Upload File                    
                                    </asp:LinkButton>

                                    <asp:LinkButton CssClass="btn btn-outline-danger" runat="server" ID="Button1"
                                        Text="Close" OnClientClick="return onCloseClick();" >
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

