<%@ Page Language="C#" AutoEventWireup="true" Inherits="DeleteChecklistFile" Title="Delete File"
    Codebehind="DeleteChecklistFile.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<script type="text/javascript">
function closeFileWindow()
{
var controlIdPrefix;
var clientFileName = document.getElementById('<%=hfClientFileName.ClientID%>').value;
var serverfilename = document.getElementById('<%=hfServerFileName.ClientID%>').value;
var uniqueRowId = document.getElementById('<%=hfUniqueRowId.ClientID%>').value;

//window.opener.document.getElementById('ctl00_ContentPlaceHolder1_gvSubmission_ctl0'+uniqueRowId+'_ClientFileName').value = '';
//window.opener.document.getElementById('ctl00_ContentPlaceHolder1_gvSubmission_ctl0'+uniqueRowId+'_ServerFileName').value = '';

//window.opener.document.getElementById('ctl00_ContentPlaceHolder1_gvChecklistDetails_ctl0'+uniqueRowId+'_ClientFileName').value = '';
//window.opener.document.getElementById('ctl00_ContentPlaceHolder1_gvChecklistDetails_ctl0'+uniqueRowId+'_ServerFileName').value = '';

if (uniqueRowId >= 10)
{
    controlIdPrefix = 'ctl00_ContentPlaceHolder1_gvChecklistDetails_ctl';
}
else
{
    controlIdPrefix = 'ctl00_ContentPlaceHolder1_gvChecklistDetails_ctl0';
}
   
window.opener.document.getElementById(controlIdPrefix + uniqueRowId + '_ClientFileName').value = '';
window.opener.document.getElementById(controlIdPrefix + uniqueRowId + '_ServerFileName').value = '';

window.opener.document.getElementById('Filelink'+uniqueRowId+'').innerHTML = '';
window.opener.document.getElementById('Filelink'+uniqueRowId+'').href = '#';
window.opener.document.getElementById('AttachFileImg'+uniqueRowId+'').style.visibility = 'visible';
window.opener.document.getElementById('DeleteFileImg'+uniqueRowId+'').style.visibility = 'hidden';
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
