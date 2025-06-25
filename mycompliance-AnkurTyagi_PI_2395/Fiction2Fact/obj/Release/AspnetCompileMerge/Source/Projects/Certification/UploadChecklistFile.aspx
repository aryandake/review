<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.Certification_UploadChecklistFile" CodeBehind="UploadChecklistFile.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<link rel="stylesheet" type="text/css" href="<%= Fiction2Fact.Global.site_url("Content/controlStyle.css")%>" />
<script type="text/javascript">

    function closeFileWindow() {
        var controlIdPrefix;

        var Type = document.getElementById('<%=hfType.ClientID%>').value;

        var clientFileName = document.getElementById('<%=hfClientFileName.ClientID%>').value;
        var serverfilename = document.getElementById('<%=hfServerFileName.ClientID%>').value;
        var uniqueRowId = document.getElementById('<%=hfUniqueRowId.ClientID%>').value;
        if (Type == 'ChecklistFile') {
            if (uniqueRowId >= 10) {
                controlIdPrefix = 'ctl00_ContentPlaceHolder1_gvChecklist_ctl';
            }
            else {
                controlIdPrefix = 'ctl00_ContentPlaceHolder1_gvChecklist_ctl0';
            }
        }

        window.opener.document.getElementById(controlIdPrefix + uniqueRowId + '_ClientFileName').value = clientFileName;
        window.opener.document.getElementById(controlIdPrefix + uniqueRowId + '_ServerFileName').value = serverfilename;
        window.opener.document.getElementById('Filelink' + uniqueRowId + '').innerHTML = clientFileName;
        window.opener.document.getElementById('Filelink' + uniqueRowId + '').href = '../CommonDownloadAnyFile.aspx?type=ChecklistFilesFolder&fileName=' + clientFileName + '&downloadFileName=' + serverfilename;
        window.opener.document.getElementById('AttachFileImg' + uniqueRowId + '').style.visibility = 'hidden';
        window.opener.document.getElementById('DeleteFileImg' + uniqueRowId + '').style.visibility = 'visible';
        window.close();
    }    
</script>

<head runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />
    <title>Upload File</title>
</head>
<body class="d-block">
    <form id="form1" runat="server">
        <asp:HiddenField ID="hfClientFileName" runat="server" />
        <asp:HiddenField ID="hfServerFileName" runat="server" />
        <asp:HiddenField ID="hfUniqueRowId" runat="server" />
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
                                        <asp:FileUpload ID="fuException" runat="server" CssClass="form-control" Width="100%" />
                                        <asp:RegularExpressionValidator ID="revFileUpload" runat="server" ControlToValidate="fuException"
                                            Display="Dynamic" ErrorMessage="Upload of this file format is not supported."
                                            ValidationExpression="^.+(.msg|.MSG|.eml|.EML|.Eml|.mp4|.MP4|.avi|.AVI|.flv|.FLV|.jpg|.JPG|.bmp|.BMP|.xls|.XLS|.xlsx|.XLSX|.DOC|.DOCX|.docx|.doc|.pdf|.PDF|.html|.htm|.HTML|.HTM|.xml|.XML|.mht|.MHT|.mhtml|.MHTML|.tif|.TIF|.ZIP|.zip|.txt|.TXT|.ppt|.pptx|.PPT|.PPTX|.pps|.ppsx|.gif|.GIF|.png|.PNG|.mp3|.MP3|.wav|.WAV|.3gp|.3GP|.vob|.VOB|.wmv|.WMV|.m4p|.M4P|.mpeg|.MPEG|.zip|.rar|.csv|.CSV|.7z)$"
                                            ValidationGroup="SubmitGrp"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="mt-3">
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
