﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testmail.aspx.cs" Inherits="ContraST.testmail" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:TextBox ID="txtMail" runat="server" Columns="50"></asp:TextBox> 
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />

		<asp:Label ID="lblMsg" runat="server" />
    </form>
</body>
</html>
