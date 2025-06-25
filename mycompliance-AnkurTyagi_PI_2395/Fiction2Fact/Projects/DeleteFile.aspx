<%@ Page Language="C#" AutoEventWireup="true" Inherits="UploadFile" Codebehind="DeleteFile.aspx.cs" Title="Delete File" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<script type="text/javascript">
function closeFileWindow()
{
var clientFileName = document.getElementById('<%=hfClientFileName.ClientID%>').value;
var serverfilename = document.getElementById('<%=hfServerFileName.ClientID%>').value;
var uniqueRowId = document.getElementById('<%=hfUniqueRowId.ClientID%>').value;
window.opener.document.getElementById('ClientFileName'+uniqueRowId).value = '';
window.opener.document.getElementById('ServerFileName'+uniqueRowId).value = '';
window.opener.document.getElementById('EX_Filelink'+uniqueRowId).innerHTML = '';
window.opener.document.getElementById('EX_Filelink'+uniqueRowId).href = '#';
window.opener.document.getElementById('EX_AttachFileImg' + uniqueRowId).style.visibility = 'visible';
window.opener.document.getElementById('EX_DeleteFileImg' + uniqueRowId).style.visibility = 'hidden';
window.close();
}
</script>
<head runat="server">
    <title>Upload File</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Label ID="lblMsg" runat="server" CssClass="label"></asp:Label>
    <asp:HiddenField ID="hfClientFileName" runat="server" />
    <asp:HiddenField ID="hfServerFileName" runat="server" />
    <asp:HiddenField ID="hfUniqueRowId" runat="server" />
    <asp:HiddenField ID="hfFileId" runat="server" />
    </form>
</body>
</html>
