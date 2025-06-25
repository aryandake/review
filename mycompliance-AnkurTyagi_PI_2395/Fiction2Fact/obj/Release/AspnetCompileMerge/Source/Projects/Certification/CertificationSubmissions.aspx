<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.Certification_CertificationSubmissions" Title="Quarterly Certifications submissions" CodeBehind="CertificationSubmissions.aspx.cs" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/commonvalidations.js") %>'>
    
    </script>



    <script type="text/javascript">
        document.write('<style type="text/css">.tabber{display:none;}<\/style>');
        /* Set the tabber options (must do this before including tabber.js)*/
        var tabberOptions = {
            'onLoad': function (argsObj) {
                var t = argsObj.tabber;
                var i;
                i = document.getElementById('<%=hfTabberId.ClientID%>').value;
            if (isNaN(i)) { return; }
            t.tabShow(i);
        },
            'onClick': function (argsObj) {
                var i = argsObj.index;
                document.getElementById('<%=hfTabberId.ClientID%>').value = i;
          }
        };
    </script>

    <script type="text/javascript">
        function checkCertificationStatus() {
            var certStatus = document.getElementById('<%= hfCertStatus.ClientID %>').value;
         if (certStatus == '') {
             alert('Please First Click on "Save Draft" button and than click on "Export to Excel".');
             return false;
         }
     }

     function onChklistClick() {

         document.getElementById('ctl00_ContentPlaceHolder1_hfSeeChecklistStatus').value = 'Y';
         var deptId = document.getElementById('ctl00_ContentPlaceHolder1_hfDepartmentID').value

         window.open('CertificationChecklistDetails.aspx?Id=' + deptId,
                'FILE', 'location=0,status=0,scrollbars=1,width=800,height=600');
         return false;
     }
    </script>

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/tabcontrol.js")%>'>
    </script>

    <script type="text/javascript" src="../js/Exception.js">
    </script>

    <br />
    <center>
        <asp:Label ID="lblMsg" runat="server" CssClass="label"></asp:Label>
    </center>
    <asp:HiddenField ID="hfSeeChecklistStatus" runat="server" />
    <asp:HiddenField ID="hfQuarterId" runat="server" />
    <asp:HiddenField ID="hfCertMId" runat="server" />
    <asp:HiddenField ID="hfUserFullName" runat="server" />
    <asp:HiddenField ID="hfExceptions" runat="server" />
    <asp:HiddenField ID="hfSymbol" runat="server" />
    <asp:HiddenField ID="hfCertId" runat="server" />
    <asp:HiddenField runat="server" ID="hfTabberId" />
    <asp:HiddenField ID="hfQuarterEndDt" runat="server" />
    <asp:HiddenField ID="hfCertDepartment" runat="server" />
    <asp:HiddenField ID="hfEmailId" runat="server" />
    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField ID="hfDepartmentID" runat="server" />
    <asp:HiddenField ID="hfCertStatus" runat="server" />
    <center>
        <table border="0" cellpadding="2" cellspacing="2" width="50%">
            <tr>
                <td class="tabhead3" width="20%">Department Name:
                </td>
                <td class="tabbody3" width="40%">
                    <asp:DropDownList ID="ddlDepartmentName" runat="server" AppendDataBoundItems="true"
                        CssClass="form-select" Width="250px" DataValueField="CDM_ID" DataTextField="CDM_NAME">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvddlDepartmentName" runat="server" Display="Dynamic"
                        ControlToValidate="ddlDepartmentName" ErrorMessage="*" InitialValue="" CssClass="span" ValidationGroup="Search"></asp:RequiredFieldValidator>
                </td>
                <td align="left">&nbsp;<asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="Search"
                    CssClass="html_button" OnClick="btnSearch_Click" />
                </td>
            </tr>
        </table>
    </center>
    <asp:Panel ID="PnlCertStatus" runat="server" Visible="true">
        <br />
        <center>
            <asp:Button CssClass="html_button" ID="btnSaveDraft1" runat="server" OnClientClick="return getExceptionDetails()"
                Text="Save Draft" OnClick="btnSaveDraft_Click" />
            <asp:Button CssClass="html_button" ID="btnSubmit1" runat="server" Text="Submit" OnClientClick="return getExceptionDetailsOnSave();"
                OnClick="btnSubmit_Click" />
        </center>
        <br />
        <div class="tabber">
            <div class="tabbertab" title="Certification" id="demo-basic1">
                <center>
                    <div class="ContentHeader1">
                        My Certification
                    </div>
                </center>
                <br />
                <table border="0" cellpadding="3" cellspacing="1" bgcolor="#dddddd" width="100%">
                    <tr>
                        <td class="tabhead3" colspan="2">I certify the following:</td>
                    </tr>
                    <tr>
                        <td colspan="2" class="tabbody3">
                            <asp:Label runat="server" ID="lblCertContents"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tabbertab" title="Compliance Checklist" id="demo-basic2" visible="false">
                <center>
                    <div class="ContentHeader1">
                        Compliance Checklist
                    </div>
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="25%" style="text-align: left;">
                                <div style="float: left; margin-left: 10px; margin-right: 10px;" id="divlink" runat="server">
                                    <asp:LinkButton ID="lnkExporttoExcel" runat="server" Text="Export to Excel " OnClick="btnExporttoExcel_Click"
                                        OnClientClick="return checkCertificationStatus();"></asp:LinkButton>
                                    <asp:Label ID="lbl" runat="server" Text=" | "></asp:Label>
                                    <asp:LinkButton ID="lnkImportExcel" runat="server" Text="Import from Excel" OnClientClick="javascript:window.open('./ImportCertificationChecklist.aspx?Type=Checklist','','location=0,status=0,scrollbars=1,width=800,height=600,resizable=1');return false;"></asp:LinkButton>
                                </div>
                            </td>
                            <td width="50%" style="text-align: center;">
                                <center>
                                    <asp:Label ID="lblImportMsg" runat="server" CssClass="label"></asp:Label>
                                </center>
                            </td>
                            <td width="25%" style="text-align: right;">
                                <asp:ImageButton ID="btnRefresh" runat="server" ImageUrl="../../Content/images/legacy/Refresh.png" Style="text-align: right"
                                    ToolTip="Refresh" OnClick="btnRefresh_Click" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlChecklistShow" runat="server">
                        <table border="0" cellpadding="3" cellspacing="1" width="100%">
                            <tr>
                                <td>
                                    <asp:GridView ID="gvChecklist" runat="server" BorderStyle="None" BorderWidth="1px"
                                        CellPadding="4" GridLines="None" AutoGenerateColumns="false" CssClass="mGrid1"
                                        DataKeyNames="CCM_ID" OnRowDataBound="gvChecklist_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="10px">
                                                <HeaderTemplate>
                                                    Serial Number
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChecklistDetsId" runat="server" Text='<%#Eval("CCD_ID") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField HeaderText="Department Name" DataField="CDM_NAME" Visible="false" />--%>
                                            <asp:BoundField HeaderText="Reference" DataField="CCM_REFERENCE" />
                                            <asp:BoundField HeaderText="Clause" DataField="CCM_CLAUSE" />
                                            <asp:BoundField HeaderText="Particulars" DataField="CCM_PARTICULARS" />
                                            <%--<asp:BoundField HeaderText="Check Points" DataField="CCM_CHECK_POINTS" />--%>
                                            <asp:BoundField HeaderText="Frequency" DataField="CCM_FREQUENCY" />
                                            <%--<asp:BoundField HeaderText="Due Date" DataField="CCM_DUE_DATE" />
                                            <asp:BoundField HeaderText="Source Department" DataField="CCM_SOURCE_DEPT" />
                                            <asp:BoundField HeaderText="Department responsible for furnishing the data" DataField="CCM_DEPT_RESP_FURNISH" />
                                            <asp:BoundField HeaderText="Department responsible for submitting it" DataField="CCM_DEPT_RESP_SUBMITTING" />--%>
                                            <asp:BoundField HeaderText="Authority" DataField="CCM_TO_BE_FILLED_WITH" />
                                            <asp:TemplateField HeaderText="Compliance Status">
                                                <ItemTemplate>
                                                    <asp:RadioButtonList ID="rbyesnona" runat="server" RepeatColumns="1" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="Compliant" Value="C"></asp:ListItem>
                                                        <asp:ListItem Text="Not Compliant" Value="N"></asp:ListItem>
                                                        <asp:ListItem Text="Not yet applicable" Value="NA"></asp:ListItem>
                                                        <asp:ListItem Text="Work in progress" Value="W"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <asp:Label ID="lblaction" runat="server" Text='<%#Eval("CCD_YES_NO_NA") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <F2FControls:F2FTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control"
                                                        Width="150px" Text='<%#Eval ("CCD_REMARKS") %>'></F2FControls:F2FTextBox>
                                                    <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </center>
            </div>
            <div class="tabbertab" title="Exception Details" id="demo-basic3">
                <center>
                    <div class="ContentHeader1">
                        Exception Details
                    </div>
                </center>
                <table width="100%">
                    <tr>
                        <td>
                            <table width="20%" align="left">
                                <tr>
                                    <td>
                                        <asp:ImageButton runat="server" ID="imgAdd" OnClientClick="return addExceptionRow()"
                                            ImageUrl="../../Content/images/legacy/iconAdd.gif" />
                                    </td>
                                    <td>
                                        <asp:ImageButton runat="server" ID="imgDelete" OnClientClick="return deleteExceptionRow()"
                                            ImageUrl="../../Content/images/legacy/BatchDelete.gif" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="litControls" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="3" cellspacing="1" bgcolor="#dddddd" width="100%">
                    <tr>
                        <td class="tabhead3" width="10%">Remarks:</td>
                        <td class="tabbody3">
                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtRemarks" runat="server" TextMode="MultiLine"
                                Rows="5" Columns="120" Width="100%"></F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <br />
        <center>
            <%-- <asp:CheckBox ID="chkIAgree" runat="server" />--%>
            <asp:Literal ID="litAgree" runat="server"></asp:Literal>
        </center>
        <br />
        <center>
            <asp:Button CssClass="html_button" ID="btnSaveDraft" runat="server" OnClientClick="return getExceptionDetails()"
                Text="Save Draft" OnClick="btnSaveDraft_Click" />
            <asp:Button CssClass="html_button" ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return getExceptionDetailsOnSave();"
                OnClick="btnSubmit_Click" />
        </center>
    </asp:Panel>
    <br />
    <br />
</asp:Content>
