<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.CommonCertContent" Title="Common Content Master"
    CodeBehind="CommonCertContent.aspx.cs" ValidateRequest="false" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .ck-editor__editable {
            min-height: 250px;
        }

        .header-center {
            text-align: center !important;
        }
    </style>
    <script src="<%=Fiction2Fact.Global.site_url("Scripts/ckeditor/ckeditor.js")%>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            ClassicEditor
                .create(document.querySelector('#ctl00_ContentPlaceHolder1_fvSearchCertificate_FCKE_EditCertContents'), {
                    // toolbar: [ 'heading', '|', 'bold', 'italic', 'link' ]
                })
                .then(editor => {
                    window.editor = editor;
                })
                .catch(err => {
                    console.error(err.stack);
                });
        });
    </script>
    <script type="text/javascript">
        function onViewClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "View";
        }
        function onClientEditClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
        }
    </script>

    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <br />
    <center>
        <div class="ContentHeader1">
            Common Content Master
        </div>
    </center>
    <asp:HiddenField ID="hfSelectedRecord" runat="server" />
    <br />
    <asp:Label ID="lblMsg" runat="server" CssClass="label"></asp:Label>
    <asp:MultiView ID="mvMultiView" runat="server">
        <asp:View ID="vwGrid" runat="server">
            <center>
                <asp:Label ID="lblInfo" runat="server" CssClass="label"></asp:Label>
                <br />
                <asp:GridView ID="gvSearchCertificate" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="CCC_ID" GridLines="None" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                    CellPadding="4" CssClass="mGrid1" OnSelectedIndexChanged="gvSearchCertificate_SelectedIndexChanged"
                    Width="80%">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr. No." HeaderStyle-CssClass="header-center" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="View" ShowHeader="true" HeaderStyle-CssClass="header-center">
                            <ItemTemplate>
                                <%--<asp:ImageButton ImageUrl='<%# Fiction2Fact.Global.site_url("Content/images/legacy/viewfulldetails.png")%>' runat="server" ID="ibView"
                                        CommandName="Select" OnClientClick="onViewClick()" ToolTip="View Details"></asp:ImageButton>--%>
                                <asp:Label ID="lblViewContent" Text='<%# Eval("CCC_CONTENT")%>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" ShowHeader="true" HeaderStyle-CssClass="header-center">
                            <ItemTemplate>
                                <center>
                                    <asp:ImageButton ImageUrl='<%# Fiction2Fact.Global.site_url("Content/images/legacy/EditInformationHS.png")%>' runat="server" ID="ibEdit"
                                        CommandName="Select" OnClientClick="onClientEditClick()"></asp:ImageButton>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </center>
        </asp:View>
        <asp:View ID="vwForm" runat="server">
            <asp:FormView ID="fvSearchCertificate" runat="server" DataKeyNames="CCC_ID" Width="100%">
                <ItemTemplate>
                    <table border="0" cellpadding="3" cellspacing="1" bgcolor="#dddddd" width="100%"
                        align="left">
                        <tr>
                            <td class="tabhead3">&nbsp;&nbsp;Content:</td>
                            <td class="tabbody3">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="tabbody3">
                                <%# Eval("CCC_CONTENT")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="tabbody3">
                                <center>
                                    <asp:Button CssClass="html_button" ID="btnViewCancel" runat="server" Text="Back"
                                        CausesValidation="false" OnClick="btnViewCancel_Click" />
                                </center>
                            </td>
                        </tr>
                    </table>
                    <br />
                </ItemTemplate>
                <EditItemTemplate>
                    <table border="0" cellpadding="3" cellspacing="1" bgcolor="#dddddd" width="100%">
                        <tr>
                            <td class="tabhead3">&nbsp;&nbsp;Content:</td>
                        </tr>
                        <tr>
                            <td class="tabbody3">
                                <table>
                                    <tr>
                                        <td style="width: 65vw;">
                                            <F2FControls:F2FTextBox runat="server" ID="FCKE_EditCertContents" TextMode="MultiLine" CssClass="ckeditor" Height="100px"></F2FControls:F2FTextBox>
                                            <asp:RequiredFieldValidator ID="rfvEditCertContents" ValidationGroup="SubmitGrp" runat="server" CssClass="span"
                                                ControlToValidate="FCKE_EditCertContents" Display="Dynamic" SetFocusOnError="True">Please enter Content.</asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 25vw;">
                                            <table style="padding: 4px; background: #ddd; margin-left: auto; margin-right: auto;" width="30%" cellpadding="2">
                                                <tr>
                                                    <td class="tabhead" colspan="2" align="center">
                                                        <b>Keywords</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tabhead">~Name</td>
                                                    <td class="tabbody">Username</td>
                                                </tr>
                                                <tr>
                                                    <td class="tabhead">~Department</td>
                                                    <td class="tabbody">Department of User</td>
                                                </tr>
                                                <tr>
                                                    <td class="tabhead">~qtrStartDate</td>
                                                    <td class="tabbody">Start date of Quarter</td>
                                                </tr>
                                                <tr>
                                                    <td class="tabhead">~qtrEndDate</td>
                                                    <td class="tabbody">End date of Quarter</td>
                                                </tr>
                                                <tr>
                                                    <td class="tabhead">~Date</td>
                                                    <td class="tabbody">Current Date</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <center>
                        <asp:Button CssClass="html_button" ID="btnUpdate" runat="server" ValidationGroup="SubmitGrp"
                            Text="Update Details" CausesValidation="true" OnClick="btnUpdate_Click" />
                        <asp:Button CssClass="html_button" ID="btnEditCancel" runat="server" Text="Cancel"
                            OnClick="btnEditCancel_Click" /></center>
                </EditItemTemplate>
            </asp:FormView>
        </asp:View>
    </asp:MultiView>
    <br />
</asp:Content>
