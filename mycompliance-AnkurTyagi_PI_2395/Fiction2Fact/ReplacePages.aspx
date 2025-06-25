<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="ReplacePages.aspx.cs" Inherits="Fiction2Fact.ReplacePages_Test" Title=" Replace Page"  EnableEventValidation="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title> Replace Pages</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <center>
        <div class="ContentHeader">
            Replace Pages
        </div>
        <br />
        <asp:Label ID="lblMsg" runat="server" />
    </center>
    <table width="100%" cellpadding="2" cellspacing="1">
        <tr>
            <td class="tabhead">
                Folder :</td>
            <td class="tabbody">
                <asp:TextBox CssClass="form-control" ID="txtFolder" MaxLength="50" runat="server" Columns="50"></asp:TextBox>
               
            </td>
            </tr>
            <tr>
                <td class="tabhead">
                    File : </td>
                <td class="tabbody">
                    <asp:FileUpload ID="fuFileUpload" runat="server" Width="411px" Height="25px" />
                </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button CssClass="Button" ID="btnUpload" runat="server" CausesValidation="true"
                    Text="Upload" OnClick="btnUpload_Click" ValidationGroup="Save" />
            </td>
        </tr>
    </table>
 </div>
    </form>
</body>
</html>

