<%@ Page Title="Framework Report" Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Reports.ReportFramework_Reports" CodeBehind="Reports.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField runat="server" ID="hfType" />
    <div class="text-center" style="width: 100%;">
        <center>
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <td bgcolor="#FFFFFF" colspan="3">
                        <div class="ContentHeader1">
                            <asp:Label ID="lblHeaderTxt" runat="server" Text="View Reports"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
            <div align="center">
                <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="label"></asp:Label>
            </div>
        </center>
    </div>
    <div style="padding: 5px;" align="left">
        <table border="0" cellspacing="1" cellpadding="3" bgcolor="#bbbbbb">
            <tr>
                <td class="tabhead3">Report Type <span class="span">*</span></td>
                <td class="tabhead3">Date of Report <%--<span class="label">*</span>--%></td>
                <td class="tabhead3">Filter 1</td>
            </tr>
            <tr>
                <td class="tabbody3">
                    <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-select" Width="300px"
                        OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                    <br />
                    <asp:RequiredFieldValidator ID="rfvReportType" runat="server" ControlToValidate="ddlReportType"
                        ValidationGroup="View" Display="Dynamic" SetFocusOnError="True" CssClass="span">Please select Report Type.</asp:RequiredFieldValidator>
                </td>
                <td class="tabbody3">
                    <asp:TextBox ID="txtDateOfReport" CssClass="form-control" Width="260px" runat="server"
                        MaxLength="11"></asp:TextBox>
                    <asp:ImageButton ID="imgDateOfReport" runat="server" AlternateText="Click to show calendar"
                        ImageUrl="../../Content/images/legacy/calendar.jpg" />&nbsp;
                        <ajaxToolkit:CalendarExtender ID="ceDateOfReport" runat="server" PopupButtonID="imgDateOfReport"
                            TargetControlID="txtDateOfReport" Format="dd-MMM-yyyy">
                        </ajaxToolkit:CalendarExtender>
                    <br />
                    <asp:RegularExpressionValidator ID="revDateOfReport" runat="server" ControlToValidate="txtDateOfReport" CssClass="span"
                        Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                        ValidationGroup="View"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvDateOfReport1" runat="server" ControlToValidate="txtDateOfReport" Enabled="false" CssClass="span"
                        ValidationGroup="View" Display="Dynamic" SetFocusOnError="True">Please enter Date of Report.</asp:RequiredFieldValidator>
                </td>
                <td class="tabbody3">
                    <asp:DropDownList CssClass="form-select" ID="ddlFilter1" runat="server" Width="300px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tabhead3">Filter 2</td>
                <td class="tabhead3">Filter 3</td>
                <td class="tabhead3"></td>
            </tr>
            <tr>
                <td class="tabbody3">
                    <asp:DropDownList CssClass="form-select" ID="ddlFilter2" runat="server" Width="300px"
                        DataTextField="SM_DESC" DataValueField="SM_NAME">
                    </asp:DropDownList>
                </td>
                <td class="tabbody3">
                    <asp:DropDownList CssClass="form-select" ID="ddlFilter3" runat="server" Width="300px"
                        DataTextField="SM_DESC" DataValueField="SM_NAME">
                    </asp:DropDownList>
                </td>
                <td class="tabbody3"></td>
            </tr>
        </table>
        <br />
        <div style="padding: 5px;" align="left">
            <asp:Button ID="btnView" runat="server" CssClass="html_button" Text="View Report"
                ValidationGroup="View" OnClick="btnView_Click" />
            <asp:Button ID="btnExportToExcel" runat="server" CssClass="html_button" Text="Export to Excel"
                Visible="false" OnClick="btnExportToExcel_Click" />
            <br />
            <br />
            <%--<asp:GridView
                runat="server" ID="gvResult" AutoGenerateColumns="true" CssClass="mGrid1">
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
            </asp:GridView>--%>
            <asp:Literal ID="litSummary" runat="server"></asp:Literal>
        </div>
    </div>

</asp:Content>
