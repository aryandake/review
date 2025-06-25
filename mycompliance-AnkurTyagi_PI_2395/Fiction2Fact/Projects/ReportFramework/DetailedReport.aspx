<%@ Page Language="C#" AutoEventWireup="true" Inherits="Fiction2Fact.Projects.Reports.ReportFramework_DetailedReport" Codebehind="DetailedReport.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>:: ORMS : QA Action Plan : Detailed Report ::</title>

    <script type="text/javascript">

        //window.onscroll = function () { scrollFunction() };

        //function scrollFunction() {
        //    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        //        document.getElementById("myBtn").style.display = "block";
        //    } else {
        //        document.getElementById("myBtn").style.display = "none";
        //    }
        //}

        //function topFunction() {
        //    document.body.scrollTop = 0;
        //    document.documentElement.scrollTop = 0;
        //}

    </script>
</head>
<body>
    <%--<button onclick="topFunction()" id="myBtn" title="Go to top"><span class="glyphicon glyphicon-eject"></span></button>--%>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hfReportType" runat="server" />
        <asp:HiddenField ID="hfDateofReport" runat="server" />
        <asp:HiddenField ID="hfX" runat="server" />
        <asp:HiddenField ID="hfY" runat="server" />
        <asp:HiddenField ID="hfFilter1" runat="server" />
        <asp:HiddenField ID="hfFilter2" runat="server" />
        <asp:HiddenField ID="hfFilter3" runat="server" />

        <div style="min-height: 450px; font-family: 'Trebuchet MS';" class="container">
            <asp:Panel ID="pnlRRDetailedReport" runat="server">
                <div style="font-size: 27px; text-align: center; font-weight: bold; font-family: 'Trebuchet MS';">
                    Detailed Report
                    <br />
                    <asp:Label ID="lblEmptyRowMessage" runat="server" CssClass="label"></asp:Label>
                </div>
                <div class="container">
                    <asp:Label ID="lblMsg" runat="server" CssClass="label"></asp:Label>
                </div>
                <br />
                <asp:Button ID="btnExportToExcel" runat="server" Text="Export to Excel" CssClass="html_button"
                    OnClick="btnExportToExcel_Click" Enabled="false" />
                <asp:GridView ID="gvRRDetailedData" runat="server" AutoGenerateColumns="true" AllowSorting="false" AllowPaging="false"
                    GridLines="Both" CellPadding="4" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Sr.No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
            </asp:Panel>
        </div>
        <script type="text/javascript">
            if (document.getElementById('btnExportToExcel') != null) {
                document.getElementById('btnExportToExcel').disabled = false;
            }
        </script>
    </form>
</body>
</html>
