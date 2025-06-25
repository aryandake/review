<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.Certification_ImportCertificationChecklist" Codebehind="ImportCertificationChecklist.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <asp:PlaceHolder runat="server">
    <link id="Link2" rel="stylesheet" type="text/css" href="<%= Fiction2Fact.Global.site_url("Content/main.css")%>" />
    <link id="Link3" rel="stylesheet" type="text/css" href="<%= Fiction2Fact.Global.site_url("Content/controlStyle.css")%>" />
        <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />
    </asp:PlaceHolder>
    <title>Compliance Checklist Import</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <center>
                <div class="ContentHeader1">
                    Compliance Checklist Import
                </div>
            </center>
            <br />
            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="left" valign="top">
                        &nbsp;</td>
                    <td>
                        <br />
                        <table width="100%" border="0" cellspacing="2" cellpadding="2">
                            <tr>
                                <td colspan="2">
                                    <asp:HiddenField ID="hfType" runat="server" />
                                    <asp:HiddenField ID="hfDate" runat="server" />
                                    <asp:HiddenField ID="hfUserName" runat="server" />
                                    <asp:HiddenField ID="hfAuditIssueId" runat="server" />
                                    <asp:Label ID="lblMsg" runat="server" CssClass="label"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="25%" class="tabhead3">
                                    Select File :
                                </td>
                                <td class="tabbody3">
                                    <asp:FileUpload ID="fuCheckData" runat="server" CssClass="form-control" Width="300px" />
                                    <asp:Button ID="btnAddAttachment" runat="server" CssClass="html_button" Text="Upload"
                                        ValidationGroup="upload" OnClick="btnAddAttachment_Click" CausesValidation="true" />
                                    <asp:RegularExpressionValidator ID="revPGRCFiles" runat="server" ControlToValidate="fuCheckData"
                                        Display="Dynamic" ErrorMessage="Upload of this file format is not supported."
                                        ValidationGroup="upload" ValidationExpression="^.+(.xls|.XLS)$"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:GridView ID="grdData" runat="server" AllowPaging="false" ShowFooter="false"
                            AllowSorting="false" BorderStyle="None" BorderWidth="1px" AutoGenerateColumns="true"
                            CssClass="mGrid1">
                            <Columns>
                                <asp:TemplateField HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        Sr.No.</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <center>
                            <br />
                            <asp:Button ID="btnSave" runat="server" Text="Save" Visible="false" OnClick="btnSave_Click"
                                CssClass="html_button" />
                            <asp:Button ID="btnClose" runat="server" Visible="true" CssClass="html_button" Text="Close"
                                OnClick="btnClose_Click" />
                            <br />
                            <br />
                        </center>
                    </td>
                    <td align="right" valign="top">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
