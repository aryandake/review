<%@ Page Title="" Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" CodeBehind="ActionableAcceptance.aspx.cs" Inherits="Fiction2Fact.Projects.Compliance.ActionableAcceptance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align: left;">
        <div class="ContentHeader1">
            Action Tracker(s) Acceptance      
        </div>
    </div>
    <br />
    <center>
        <asp:Label ID="lblMsg" runat="server" CssClass="label" />
    </center>
    <br />
    <div style="text-align: left;">
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="html_button" OnClientClick="return confirm('are you sure want to submit all issues');" OnClick="btnSubmit_Click" />
    </div>
    <br />
    <asp:GridView ID="gvActionables" runat="server" AutoGenerateColumns="False" DataKeyNames="CIA_ID"
        AllowSorting="false" AllowPaging="false" GridLines="Both" CellPadding="4"
        CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
        EmptyDataText="No record found..." OnSelectedIndexChanged="gvActionables_SelectedIndexChanged">
        <Columns>
            <asp:TemplateField HeaderText="Sr.No." ControlStyle-Width="20px" HeaderStyle-Width="20px">
                <ItemTemplate>
                    <center>
                        <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                    </center>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Type of Action" DataField="ActionType" />
            <asp:TemplateField HeaderText="Actionables" ShowHeader="true">
                <ItemTemplate>
                    <asp:Label ID="lblActionPlan" runat="server" Text='<%#Eval("CIA_ACTIONABLE").ToString().Replace("\n", "<br />")%>'></asp:Label>
                    <asp:Label ID="lblRecStatus" runat="server" Visible="false" Text='<%#Eval("CIA_STATUS") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Action Plan Status" DataField="ActionStatus" />
            <asp:BoundField HeaderText="Unit Responsible" DataField="CSFM_NAME" />
            <asp:BoundField DataField="CIA_TARGET_DT" HeaderText="Target Date" ControlStyle-Width="110px" HeaderStyle-Width="110px" DataFormatString="{0:dd-MMM-yyyy}" />
            <asp:BoundField DataField="CIA_CLOSURE_DT" HeaderText="Closure Date" ControlStyle-Width="110px" HeaderStyle-Width="110px" DataFormatString="{0:dd-MMM-yyyy}" />
            <asp:BoundField HeaderText="Person Responsible" DataField="CIA_SPECIFIED_PERSON_NAME" />
            <asp:TemplateField HeaderText="Remarks" ShowHeader="true">
                <ItemTemplate>
                    <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("CIA_REMARKS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Status" DataField="RecStatus" />
        </Columns>
    </asp:GridView>
    <br />
    <div style="text-align: left;">
        <asp:Button ID="btnSubmit1" runat="server" Text="Submit" CssClass="html_button" OnClientClick="return confirm('are you sure want to submit all issues');" OnClick="btnSubmit_Click" />
    </div>
    <br />
</asp:Content>
