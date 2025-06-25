<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BulkUserCreation.aspx.cs" Inherits="Fiction2Fact.BulkUserCreation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
            <br />
            <asp:Label ID="lblMsg" runat="server" CssClass="label"></asp:Label>
        </div>
    </form>
</body>
</html>
