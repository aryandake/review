<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Admin.RefCodeMaster_Old" Title="Reference Codes" CodeBehind="RefCodeMaster_Old.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        function onClientEditClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
    }
    function onClientDeleteClick() {
        if (!confirm('Are you sure that you want to delete this record?')) return false;
        document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Delete";
    }

    </script>

    <br />
    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <center>
        <div class="ContentHeader1">
            Reference Code Master
            <br />
            <br />
            <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="label"></asp:Label>
        </div>
    </center>
    <asp:MultiView ID="mvRefCode" runat="server">
        <asp:View runat="server" ID="vwSearch">
            <center>
                <%--<table>
                    <tr>
                        <td class="tabhead3">
                            Ref Type</td>
                        <td class="tabhead3">
                            Ref Code</td>
                        <td class="tabhead3">
                            Ref Name</td>
                        <td class="tabhead3">
                        </td>
                        <td class="tabhead3">
                        </td>
                        <td class="tabhead3">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlSearchRefType" DataTextField="RC_TYPE"
                                DataValueField="RC_TYPE" runat="server">
                            </f2f:DropdownListNoValidation>
                        </td>

                        <td class="tabbody3">
                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtSearchRefCode" MaxLength="100" runat="server" Columns="30"></F2FControls:F2FTextBox>                            
                        </td>
                        <td class="tabbody3">
                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtSearchRefName" MaxLength="100" runat="server" Columns="30"></F2FControls:F2FTextBox>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnSearch" ImageUrl="../../Content/images/legacy/buttons_page/Search.gif" OnClick="btnSearch_Click"
                                runat="server" ValidationGroup="SEARCH" />
                        </td>
                        <td>

                        <asp:ImageButton ID="btnAddReference" ImageUrl="../../Content/images/legacy/buttons_page/add.gif"
                            OnClick="btnAddReference_Click" runat="server" />
                        </td>
                        
                        <td>
                        <asp:ImageButton runat="server" ID="imgExcel" OnClick="btnExportToExcel_Click" ImageUrl="../../Content/images/legacy/e-icon.gif"
                            Width="28" Height="28" border="0" Visible="false"/>
                        </td>

                    </tr>
                </table>--%>
                <table width="50%">
                    <tr>
                        <td colspan="3">
                            <img src="../../Content/images/legacy/spacer.gif" width="1" height="12"></td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center" valign="top">
                            <table width="70%" border="0" cellpadding="0" cellspacing="0">
                                <%-- <tr>
                                    <td width="1%" align="left" valign="top" bgcolor="#FED870">
                                        <img src="../../Content/images/legacy/table_left.gif" width="6" height="7"></td>
                                    <td width="98%" align="center" valign="top" bgcolor="#FED870">
                                        <img src="../../Content/images/legacy/spacer.gif" width="1" height="12"></td>
                                    <td width="1%" align="right" valign="top" bgcolor="#FED870">
                                        <img src="../../Content/images/legacy/table_right.gif" width="6" height="7"></td>
                                </tr>--%>
                                <tr>
                                    <td colspan="3" align="center" valign="top">
                                        <table width="98%" border="0" cellspacing="0" cellpadding="2">
                                            <tr>
                                                <td width="25%" class="tabhead3">Ref Type</td>
                                                <td width="25%" class="tabhead3">Ref Code</td>
                                                <td width="25%" class="tabhead3">Ref Name</td>
                                                <td width="10%" class="table"></td>
                                                <td width="7%" class="table"></td>
                                                <td width="8%" class="table"></td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" class="tabbody3">
                                                    <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlSearchRefType" DataTextField="RC_TYPE"
                                                        DataValueField="RC_TYPE" runat="server" Width="170px">
                                                    </f2f:DropdownListNoValidation>
                                                </td>
                                                <td valign="top" align="left" class="tabbody3">
                                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtSearchRefCode" MaxLength="100" runat="server"
                                                        Columns="30"></F2FControls:F2FTextBox>
                                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSearchRefCode" />
                                                </td>
                                                <td valign="top" align="left" class="tabbody3">
                                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtSearchRefName" MaxLength="100" runat="server"
                                                        Columns="30"></F2FControls:F2FTextBox>
                                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSearchRefName" />
                                                </td>
                                                <td valign="top">
                                                    <asp:Button ID="btnSearch" Text="Search" CssClass="html_button" OnClick="btnSearch_Click"
                                                        runat="server" ValidationGroup="SEARCH" />
                                                    <%--<asp:ImageButton ID="btnSearch" ImageUrl="../../Content/images/legacy/buttons_page/Search.gif" OnClick="btnSearch_Click"
                                                        runat="server" ValidationGroup="SEARCH" />--%>
                                                </td>
                                                <td valign="top">
                                                    <asp:Button ID="btnAddReference" Text="+" CssClass="html_button" OnClick="btnAddReference_Click"
                                                        runat="server" />
                                                    <%--<asp:ImageButton ID="btnAddReference" ImageUrl="../../Content/images/legacy/buttons_page/add.gif" OnClick="btnAddReference_Click"
                                                        runat="server" />--%>
                                                </td>
                                                <td valign="top">
                                                    <asp:Button runat="server" ID="imgExcel" Text="Export To Excel" CssClass="html_button"
                                                        OnClick="btnExportToExcel_Click" ImageUrl="../../Content/images/legacy/buttons_page/Excel_1.gif"
                                                        Visible="false" />
                                                    <%--<asp:ImageButton runat="server" ID="imgExcel" OnClick="btnExportToExcel_Click" ImageUrl="../../Content/images/legacy/buttons_page/Excel_1.gif"
                                                        Visible="false" />--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </center>
            <br />
            <asp:GridView ID="gvRefCode" runat="server" AutoGenerateColumns="False" DataKeyNames="RC_ID"
                AllowSorting="false" AllowPaging="true" GridLines="Both" CellPadding="4" OnPageIndexChanging="gvRefCode_PageIndexChanging"
                OnSelectedIndexChanged="gvRefCode_SelectedIndexChanged" CssClass="mGrid1" PagerStyle-CssClass="pgr"
                AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Sr.No.
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RC_TYPE" HeaderText="Reference Type " />
                    <asp:BoundField DataField="RC_NAME" HeaderText="Reference Name" />
                    <asp:TemplateField HeaderText="Edit" ShowHeader="true">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" Text="Edit" OnClientClick="onClientEditClick()"><img src="../../Content/images/legacy/EditInformationHS.png" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete" ShowHeader="true" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Select" Text="Delete"
                                OnClientClick="return onClientDeleteClick()"><img style="display: grid" src="../../Content/images/legacy/delete.png" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:View>
        <asp:View ID="vwInsert" runat="server">
            <table border="0" cellpadding="3" cellspacing="1" bgcolor="#dddddd" width="100%">
                <tr>
                    <td class="tabhead3">Id :</td>
                    <td class="tabbody3">
                        <asp:Label ID="lblID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tabhead3">Reference Type :</td>
                    <td class="tabbody3">
                        <F2FControls:F2FTextBox CssClass="form-control" Width="300px" ID="txtRefType" MaxLength="200" runat="server"
                            Columns="50"></F2FControls:F2FTextBox>
                        <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtRefType" />
                        <span class="label">*</span>
                        <asp:RequiredFieldValidator ID="rfvRefType" runat="server" ControlToValidate="txtRefType"
                            ErrorMessage="Please enter Reference Type" Display="None" ValidationGroup="AddReference"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="tabhead3">Reference Code :</td>
                    <td class="tabbody3">
                        <F2FControls:F2FTextBox CssClass="form-control" Width="300px" ID="txtRefCode" MaxLength="50" runat="server"
                            Columns="50"></F2FControls:F2FTextBox>
                        <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtRefCode" />
                        <span class="label">*</span>
                        <asp:RequiredFieldValidator ID="rfvRefCode" runat="server" ControlToValidate="txtRefCode"
                            ErrorMessage="Please enter Reference Code" Display="None" ValidationGroup="AddReference"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="tabhead3">Reference Name :
                    </td>
                    <td class="tabbody3">
                        <F2FControls:F2FTextBox CssClass="form-control" Width="300px" ID="txtRefName" MaxLength="200" runat="server"
                            Columns="50"></F2FControls:F2FTextBox>
                        <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtRefName" />
                        <span class="label">*</span>
                        <asp:RequiredFieldValidator ID="rfvRefName" runat="server" ControlToValidate="txtRefName"
                            ErrorMessage="Please enter Reference Name" Display="None" ValidationGroup="AddReference"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2" style="color: red" class="tabbody3">
                        <asp:ValidationSummary ID="vsRefCode" runat="server" ShowMessageBox="True" ShowSummary="false"
                            ValidationGroup="AddReference" />
                    </td>
                </tr>
            </table>
            <br />
            <center>
                <asp:Button ID="btnSave" OnClick="btnSave_Click" CssClass="html_button" Text="Save"
                    runat="server" ValidationGroup="AddReference" />
                <asp:Button ID="btnCancel" CssClass="html_button" Text="Cancel" OnClick="btnCancel_Click"
                    runat="server" />
            </center>
            <br />
        </asp:View>
    </asp:MultiView>
</asp:Content>
