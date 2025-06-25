<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.Certification_SearchCertification" Title="Search Certifications " Codebehind="SearchCertification.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField runat="server" ID="hfContent" />
    <asp:HiddenField runat="server" ID="hfTabberId" />
    <center>
        <div class="ContentHeader1">
            Search Certificates</div>
    </center>
    <br />
    <asp:Label ID="lblMsg" runat="server" CssClass="label"></asp:Label>
    <center>
        <table style="border-collapse: collapse">
            <tr>
                <td class="tabhead3">
                    &nbsp; Department Name :
                </td>
                <td class="tabhead3">
                    &nbsp; Quarter :
                </td>
                <td class="tabhead3">
                    &nbsp; Select :
                </td>
            </tr>
            <tr>
                <td class="tabbody3">
                    <asp:DropDownList CssClass="form-select" ID="ddlSearchDeptName" runat="server"
                        DataValueField="CDM_ID" DataTextField="CDM_NAME">
                    </asp:DropDownList>
                </td>
                <td class="tabbody3">
                    <asp:DropDownList CssClass="form-select" ID="ddlQuarter" runat="server" DataValueField="CQM_ID"
                        DataTextField="Quarter">
                    </asp:DropDownList>
                </td>
                <td class="tabbody3">
                    <asp:DropDownList CssClass="form-select" ID="ddlStatus" runat="server" >
                        <asp:ListItem Text="(Select an Option)" Value=""></asp:ListItem>
                        <asp:ListItem Text="Compliant" Value="C"></asp:ListItem>
                        <asp:ListItem Text="Not Compliant" Value="N"></asp:ListItem>
                        <asp:ListItem Text="Not yet applicable" Value="NA"></asp:ListItem>
                        <asp:ListItem Text="Work in progress" Value="W"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="tabbody3">
                    <asp:Button CssClass="html_button" ID="btnSearch" Text="Search" runat="server" ValidationGroup="SEARCH"
                        AccessKey="s" OnClick="btnSearch_Click" />
                        <asp:Button CssClass="html_button" ID="btnExport" Visible="false" Text="Export to Excel"
                    runat="server" OnClick="btnExportToExcel_Click" />
                </td>
            </tr>
        </table>
    </center>
    <asp:GridView ID="gvChecklist" runat="server" BorderStyle="None" BorderWidth="1px"
        CellPadding="4" GridLines="None" AutoGenerateColumns="false" CssClass="mGrid1" 
        DataKeyNames="CDM_NAME"  AllowSorting="true" AllowPaging="true" OnPageIndexChanging="gvChecklist_PageIndexChanging" 
        OnSorting="gvChecklist_Sorting" OnRowCreated="OnRowCreated" >
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    Sr.No.</HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>.
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Department Name" SortExpression="CDM_NAME">
                <ItemTemplate>
                    <asp:Label ID="lblId" runat="server" Text='<%# Bind("CDM_NAME") %>' Visible="true"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Quarter" SortExpression="CDM_NAME">
                <ItemTemplate>  
                    <asp:Label ID="lblQuarter" runat="server" Text='<%# Eval("CQM_FROM_DATE","{0:dd-MMM-yyyy}").ToString() + " to " + Eval("CQM_TO_DATE","{0:dd-MMM-yyyy}").ToString()%>'
                       ></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
           <asp:BoundField HeaderText="Regulatory Requirements" DataField="CCM_REFERENCE" />
                                            <asp:BoundField DataField="CCM_CLAUSE" HeaderText="Applicable Regulation/Guideline/Circular" />
                                            <asp:BoundField DataField="CCM_PARTICULARS" HeaderText="Control Process" />
            <asp:BoundField HeaderText="Check Points" DataField="CCM_CHECK_POINTS" SortExpression="CCM_CHECK_POINTS"/>
            <asp:BoundField HeaderText="Frequency" DataField="CCM_FREQUENCY" SortExpression="CCM_FREQUENCY"/>
            <asp:BoundField HeaderText="Due Date" DataField="CCM_DUE_DATE" SortExpression="CCM_DUE_DATE"/>
            <asp:BoundField HeaderText="Source Department" DataField="CCM_SOURCE_DEPT" SortExpression="CCM_SOURCE_DEPT"/>
            <asp:BoundField HeaderText="Department responsible for furnishing the data" DataField="CCM_DEPT_RESP_FURNISH" SortExpression="CCM_DEPT_RESP_FURNISH"/>
            <asp:BoundField HeaderText="Department responsible for submitting it" DataField="CCM_DEPT_RESP_SUBMITTING" SortExpression="CCM_DEPT_RESP_SUBMITTING"/>
            <asp:BoundField DataField="CCM_TO_BE_FILLED_WITH" HeaderText="To be filed with" />
           <asp:BoundField HeaderText="Status" DataField="ChecklistStatus" SortExpression="ChecklistStatus"/>           
            <asp:TemplateField HeaderText="Remarks" SortExpression="CCD_REMARKS">
                <ItemTemplate>
                 <asp:Label ID="lblId" runat="server" Text='<%# Bind("CCD_REMARKS") %>' Visible="true"></asp:Label>                 
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br />
</asp:Content>
