<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" EnableEventValidation="false"
    ValidateRequest="false" Inherits="Fiction2Fact.Projects.Certification.CommonMasters_MailConfigMas"
    Title=":: Certification :  Mail Configuration Master ::" CodeBehind="MailConfigMas.aspx.cs" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="<%=Fiction2Fact.Global.site_url("Scripts/ckeditor/ckeditor.js")%>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            ClassicEditor
            .create(document.querySelector('#ctl00_ContentPlaceHolder1_FCKContent'), {
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
        function onClientEditClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
    }
    function onClientDeleteClick() {
        if (!confirm('Are you sure that you want to delete this record?')) return false;
        document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Delete";
    }
    </script>

    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <center>
        <br />
        <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="label"></asp:Label>
        <br />
    </center>
    <table>
        <tr>
            <td>&nbsp;</td>
        </tr>
    </table>
    <asp:MultiView ID="mvMailConfig" runat="server">
        <asp:View runat="server" ID="vwSearch">
            <center>
                <table>
                    <tr>
                        <td class="tabhead3">Mail Configuration ID:</td>
                        <td class="tabhead3">Configuration Type</td>
                        <td class="tabhead3"></td>
                        <td class="tabhead3"></td>
                    </tr>
                    <tr>
                        <td class="tabbody3">
                            <f2fcontrols:f2ftextbox CssClass="form-control" id="txtSearchMailConfigId" runat="server" columns="10">
                            </f2fcontrols:f2ftextbox>
                            <F2FControls:F2FFilteredNumber runat="server" TargetControlID="txtSearchMailConfigId" />
                        </td>
                        <td class="tabbody3">
                            <%--<asp:DropDownList CssClass="form-select" ID="ddlConfigType" DataTextField="RC_NAME"
                                            DataValueField="RC_CODE" runat="server">
                                        </asp:DropDownList>--%>
                            <f2fcontrols:f2ftextbox CssClass="form-control" id="txtConfigType" runat="server" columns="50">
                            </f2fcontrols:f2ftextbox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtConfigType" />
                        </td>
                        <td valign="top">
                            <asp:Button CssClass="html_button" ID="btnSearch" Text="Search" OnClick="btnSearch_Click"
                                runat="server" ValidationGroup="SEARCH" />
                        </td>
                        <td valign="top">
                            <asp:Button CssClass="html_button" ID="btnAddMailConfig" Text="Add" OnClick="btnAddMailConfig_Click"
                                runat="server" />
                        </td>
                    </tr>
                </table>
            </center>
            <br />
            <asp:GridView ID="gvMailConfig" runat="server" AutoGenerateColumns="False" DataKeyNames="MCM_ID"
                AllowSorting="true" AllowPaging="true" GridLines="Both" CellPadding="4" CssClass="mGrid1"
                PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnSelectedIndexChanged="gvMailConfig_SelectedIndexChanged"
                OnPageIndexChanging="gvMailConfig_PageIndexChanging" OnSorting="gvMailConfig_Sorting">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Sr.No.
                        </HeaderTemplate>
                        <ItemTemplate>
                            &nbsp;&nbsp;&nbsp;<asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>'
                                runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="MCM_FROM" HeaderText="From" />
                                <asp:BoundField DataField="MCM_TO" HeaderText="To" />
                                <asp:BoundField DataField="MCM_CC" HeaderText="CC" />
                                <asp:BoundField DataField="MCM_BCC" HeaderText="BCC" />--%>
                    <asp:BoundField DataField="MCM_TYPE" HeaderText="Type" />
                    <asp:BoundField DataField="MCM_SUBJECT" HeaderText="Subject" />
                    <asp:TemplateField HeaderText="Mail Content">
                        <ItemTemplate>
                            <asp:Label ID="lblContent" runat="server" Text='<%# Bind("MCM_CONTENT") %>' Visible="true"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="MCM_CONTENT" HeaderText="Mail Content" />--%>
                    <asp:TemplateField HeaderText="Edit" ShowHeader="true">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" Text='<%#"<img alt=\"Edit\" src=\"" + Fiction2Fact.Global.site_url("Content/images/legacy/EditInformationHS.png") + "\" border=\"0\"" %>'
                                CssClass="centerLink" OnClientClick="onClientEditClick()">
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Delete" ShowHeader="true">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Select" Text="&lt;img alt='Delete' src='../../Content/images/legacy/delete.png' border='0' &gt;"
                                CssClass="centerLink" OnClientClick="onClientDeleteClick()">
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                </Columns>
            </asp:GridView>
        </asp:View>
        <asp:View ID="vwInsert" runat="server">
            <table width="100%">
                <tr>
                    <td class="tabhead3">Mail Configuration ID:</td>
                    <td class="tabbody3">
                        <asp:Label ID="lblID" runat="server" CssClass="label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tabhead3">Mail Configuration Type :</td>
                    <td class="tabbody3">
                        <%-- <asp:DropDownList CssClass="form-select" ID="ddlType" DataTextField="RC_NAME" DataValueField="RC_CODE"
                                        runat="server">
                                    </asp:DropDownList>--%>
                        <f2fcontrols:f2ftextbox CssClass="form-control" id="txtType" runat="server" columns="50">
                        </f2fcontrols:f2ftextbox>
                        <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtType" />
                        <%--  <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="ddlType"
                            Display="Dynamic" ValidationGroup="AddMailConfig" SetFocusOnError="True">*</asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <%-- <tr>
                                <td class="tabhead3">
                                    From :
                                </td>
                                <td class="tabbody3">
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtFrom" MaxLength="50" runat="server" Columns="50">
                                    </F2FControls:F2FTextBox>
                                    <asp:RequiredFieldValidator ID="rfvFrom" runat="server" ControlToValidate="txtFrom" CssClass="span"
                                        Display="Dynamic" ValidationGroup="AddMailConfig" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revFrom" runat="server" ControlToValidate="txtFrom"
                                        ErrorMessage="Enter Valid Email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        ValidationGroup="AddMailConfig" Display="Dynamic">Please enter a valid Email address</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="tabhead3">
                                    To :
                                </td>
                                <td class="tabbody3">
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtTo" MaxLength="50" runat="server" Columns="50">
                                    </F2FControls:F2FTextBox>
                                    <asp:RequiredFieldValidator ID="rfvTo" runat="server" ControlToValidate="txtTo" Display="Dynamic" CssClass="span"
                                        ValidationGroup="AddMailConfig" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="tabhead3">
                                    CC :
                                </td>
                                <td class="tabbody3">
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtCC" MaxLength="4000" runat="server" Columns="50"
                                        TextMode="MultiLine" Rows="3"></F2FControls:F2FTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tabhead3">
                                    BCC :
                                </td>
                                <td class="tabbody3">
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtBCC" MaxLength="4000" runat="server" Columns="50"
                                        TextMode="MultiLine" Rows="3"></F2FControls:F2FTextBox>
                                </td>
                            </tr>--%>
                <tr>
                    <td class="tabhead3">Subject :
                    </td>
                    <td class="tabbody3">
                        <f2fcontrols:f2ftextbox CssClass="form-control" id="txtSubject" maxlength="200" runat="server" columns="50"></f2fcontrols:f2ftextbox>
                          <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSubject" />
                        <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject"
                            Display="Dynamic" ValidationGroup="AddMailConfig" CssClass="span" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="tabhead3">Content :
                    </td>
                </tr>
                <tr>
                    <td class="tabbody3" colspan='2'>
                        <%--<F2FControls:F2FTextBox CssClass="form-control" ID="txtContent" runat="server" Columns="50" TextMode="MultiLine"
                            Rows="3"></F2FControls:F2FTextBox>
                        <asp:RequiredFieldValidator ID="rfvContent" runat="server" ControlToValidate="txtContent"
                            Display="Dynamic" ValidationGroup="AddMailConfig" CssClass="span" SetFocusOnError="True">*</asp:RequiredFieldValidator>--%>

                        <f2fcontrols:f2ftextbox runat="server" id="FCKContent" textmode="MultiLine" cssclass="ckeditor" height="500px"></f2fcontrols:f2ftextbox>
                    </td>
                </tr>
            </table>
            <br />
            <center>
                <asp:Button CssClass="html_button" ID="btnSave" Text="Save" OnClick="btnSave_Click"
                    runat="server" ValidationGroup="AddMailConfig" />
                <asp:Button CssClass="html_button" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click"
                    runat="server" />
                <%-- <asp:Button CssClass="Button" ID="btnSave" runat="server" OnClick="btnSave_Click"
                                Text="Save" ValidationGroup="AddMailConfig" />
                            <asp:Button CssClass="Button" ID="btnCancel" runat="server" OnClick="btnCancel_Click"
                                Text="Cancel" />--%>
            </center>
            <br />
        </asp:View>
    </asp:MultiView>
</asp:Content>
