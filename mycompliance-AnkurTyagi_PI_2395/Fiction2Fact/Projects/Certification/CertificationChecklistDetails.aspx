<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.Certification_CertificationChecklistDetails" Codebehind="CertificationChecklistDetails.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link id="Link2" rel="stylesheet" type="text/css" href="<%= Fiction2Fact.Global.site_url("Content/main.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%= Fiction2Fact.Global.site_url("Content/controlStyle.css")%>" />
    <title>Certification Checklist Details</title>
</head>
<body style="background-color: #f4f4f4;">
    <form id="form1" runat="server">
        <br />
        <center>
            <div class="ContentHeader1">
                <%--Certification Checklist Details--%>
                Key Regulatory Compliance Requirements
                <br />
            </div>
        </center>
        <br />
        <asp:GridView ID="gvChecklist" runat="server" BorderStyle="None" BorderWidth="1px"
            CellPadding="4" GridLines="None" AutoGenerateColumns="false" CssClass="mGrid1"
            DataKeyNames="ID">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Serial Number</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:BoundField HeaderText="Relevant To" DataField="Relevant To" />
                <asp:BoundField HeaderText="Name of the Act/Regulation" DataField="Act Regulation" />--%>
                <asp:BoundField HeaderText="Key Regulations / Sections/ Guidelines /Circulars" DataField="Nature Of Compliance" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
