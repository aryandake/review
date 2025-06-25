<%@ Page Language="C#" AutoEventWireup="true" Inherits="UploadAnyFile" CodeBehind="UploadAnyFile.aspx.cs" Title="Upload File" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload File</title>
    <asp:PlaceHolder runat="server">
        <link id="Link2" rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css")%>" />
        <script type="text/javascript">
            function closeFileWindow() {
                var clientFileName = document.getElementById('<%=hfClientFileName.ClientID%>').value;
                var serverfilename = document.getElementById('<%=hfServerFileName.ClientID%>').value;
                window.opener.onAppAttachUploaded(serverfilename, clientFileName);
                window.close();
            }
        </script>
    </asp:PlaceHolder>
</head>
<body class="d-block">
    <form id="form1" runat="server">
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
                                        <label class="form-label">Select File :</label>
                                        <asp:FileUpload ID="fuException" runat="server" CssClass="form-control" />
                                        <asp:RegularExpressionValidator ID="revFileUpload" runat="server" ControlToValidate="fuException" CssClass="text-danger"
                                            Display="Dynamic" ErrorMessage="Upload of this file format is not supported."
                                            ValidationExpression="^.+(.msg|.MSG|.eml|.EML|.Eml|.mp4|.MP4|.avi|.AVI|.flv|.FLV|.jpg|.JPG|.bmp|.BMP|.xls|.XLS|
                                            .xlsx|.XLSX|.DOC|.DOCX|.docx|.doc|.pdf|.PDF|.html|.htm|.HTML|.HTM|.xml|.XML|.mht|.MHT|.mhtml|.MHTML|.tif|.TIF|.ZIP|.zip|.txt|.TXT|.ppt|.pptx|.PPT|.PPTX|.pps|.ppsx|.gif|.GIF|.png|.PNG|.mp3|.MP3|.wav|.WAV|.3gp|.3GP|.vob|.VOB|.wmv|.WMV|.m4p|.M4P|.mpeg|.MPEG|.zip|.rar|.csv|.CSV|.7z)$"
                                            ValidationGroup="SubmitGrp"></asp:RegularExpressionValidator>
                                        <%--ValidationExpression="^.+(.msg|.MSG|.mp4|.MP4|.eml|.EML|.Eml|.avi|.AVI|.flv|.FLV|.jpg|.JPG|.bmp|.BMP|.xls|.XLS|.xlsx|.XLSX|.DOC|.doc|.DOCX|.docx|.pdf|.PDF|.html|.htm|.HTML.|HTM|.htm|.tif|.TIF|.ZIP|.zip|.txt|.TXT|.wav|.WAV|.WMA|.wma|.png|.PNG|.gif|.GIF|.PPSX|.ppsx|.PPTX|.pptx|.PPT|.ppt|.PPS|.pps|.RAR|.rar|.ZIP|.zip)$"--%>
                                    </div>
                                </div>
                                <div class="text-center mt-3">
                                    <asp:LinkButton ID="btnProcess" CssClass="btn btn-outline-primary" runat="server" Text="Upload File" ValidationGroup="SubmitGrp"
                                        OnClick="btnProcess_Click">
                                <i class="fa fa-upload"></i> Upload File                          
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
