<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Async="true" Inherits="Fiction2Fact.Projects.Certification.Certification_TestCommonCertification" Title="Common Certifications" CodeBehind="CommonCertification.aspx.cs" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%-- Added by Nikhil Adhalikar on 20-Sep-2011 --%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        /* Optional: Temporarily hide the "tabber" class so it does not "flash"
           on the page as plain HTML. After tabber runs, the class is changed
           to "tabberlive" and it will appear. */

        document.write('<style type="text/css">.tabber{display:none;}<\/style>');

        /*==================================================
          Set the tabber options (must do this before including tabber.js)
          ==================================================*/
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

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/tabcontrol.js")%>'>
    </script>

    <asp:HiddenField runat="server" ID="hfTabberId" />
    <asp:HiddenField ID="hfQuarterId" runat="server" />
    <asp:HiddenField ID="hfCertMId" runat="server" />
    <asp:HiddenField ID="hfCCId" runat="server" />
    <asp:HiddenField ID="hfDesignation" runat="server" />
    <asp:HiddenField ID="hfCAOEmailId" runat="server" />
    <asp:HiddenField ID="hfCEOEmailId" runat="server" />
    <asp:HiddenField ID="hfCFOEmailId" runat="server" />
    <asp:HiddenField ID="hfEmailId" runat="server" />
    <asp:HiddenField ID="hfSymbol" runat="server" />
    <asp:HiddenField ID="hfQuarterEndDt" runat="server" />
    <br />
    <center>
        <div class="ContentHeader1">
            Quarterly Certification
        </div>
    </center>
    <br />
    <center>
        <asp:Label ID="lblMsg" runat="server" CssClass="label"></asp:Label>
    </center>
    <asp:Panel ID="PnlCertStatus" runat="server" Visible="true">
        <asp:MultiView ID="MvCert" runat="server">
            <asp:View ID="View1" runat="server">
                <div class="tabber">
                    <div class="tabbertab" title="Joint Certification">
                        <br />
                        <table border="0" cellpadding="3" cellspacing="1" bgcolor="#dddddd" width="100%">
                            <tr>
                                <td class="tabhead3">I certify the following:</td>
                            </tr>
                            <tr>
                                <td class="tabbody3">
                                    <asp:Label runat="server" ID="lblCertContents"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <%--<asp:Panel ID="pnlCHRemarks" runat="server" Visible="true">
                            <table border="0" cellpadding="3" cellspacing="1" bgcolor="#dddddd" width="100%">
                                <tr>
                                    <td class="tabhead3" width="25%">
                                        Remarks by Chief Compliance Officer:</td>
                                    <td class="tabbody3">
                                        <asp:Label runat="server" ID="lblCHRemarks"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tabhead3" width="25%">
                                        Chief Compliance Officer(Submitted on):</td>
                                    <td class="tabbody3">
                                        <asp:Label runat="server" ID="lblCHSubmittedOn"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>--%>
                        <%--<asp:Panel ID="pnlCAORemarks" runat="server" Visible="false">
                            <table border="0" cellpadding="3" cellspacing="1" bgcolor="#dddddd" width="100%">
                                <tr>
                                    <td class="tabhead3" width="25%">
                                        Remarks by Chief Actuarial Officer:</td>
                                    <td class="tabbody3">
                                        <asp:Label runat="server" ID="lblCAORemarks"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>--%>
                        <asp:Panel ID="pnlCFORemarks" runat="server" Visible="false">
                            <table border="0" cellpadding="3" cellspacing="1" bgcolor="#dddddd" width="100%">
                                <tr>
                                    <td class="tabhead3" width="25%">Remarks by Chief Financial Officer:</td>
                                    <td class="tabbody3">
                                        <asp:Label runat="server" ID="lblCFORemarks"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tabhead3" width="25%">Chief Financial Officer(Submitted on):</td>
                                    <td class="tabbody3">
                                        <asp:Label runat="server" ID="lblSubmittedOn"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table border="0" cellpadding="3" cellspacing="1" bgcolor="#dddddd" width="100%">
                            <tr>
                                <td class="tabhead3">Remarks:</td>
                                <td class="tabbody3">
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtRemarks" runat="server" TextMode="MultiLine"
                                        Rows="5" Columns="120"></F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                    <%--<asp:RequiredFieldValidator ID="rfvRemarks" runat="server" ControlToValidate="txtRemarks" CssClass="span"
                                    Display="Dynamic" ValidationGroup="SubmitGrp" SetFocusOnError="True">Please enter Remarks.</asp:RequiredFieldValidator>--%>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <center>
                            <table>
                                <tr>
                                    <td align="center">
                                        <asp:Button CssClass="html_button" ID="btnSaveDraft" runat="server" Text="Save Draft"
                                            OnClick="btnSaveDraft_Click" />
                                        <asp:Button CssClass="html_button" ID="btnSubmit" ValidationGroup="SubmitGrp" runat="server"
                                            OnClick="btnSubmit_Click" Text="Submit" CausesValidation="true" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                    <div class="tabbertab" title="Executive Committee Certifications">
                        <br />
                        <asp:Label ID="lblGrid" runat="server" Visible="false" CssClass="label"></asp:Label>
                        <asp:GridView ID="gvCertView" runat="server" AutoGenerateColumns="False" DataKeyNames="CERT_ID"
                            CssClass="mGrid1" Width="70%" GridLines="Both" CellPadding="4" OnSelectedIndexChanged="gvCertView_SelectedIndexChanged">
                            <Columns>
                                <asp:CommandField HeaderText="View" ShowSelectButton="True" SelectText="&lt;img alt='View' src='../../Content/images/legacy/viewfulldetails.png' border='0' &gt;">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:CommandField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Sr.No.
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CDM_NAME" HeaderText="Department Name" SortExpression="CDM_NAME" />
                                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="tabbertab" title="Exception Details">
                        <br />
                        <asp:Button CssClass="html_button" ID="btnExport" Text="Export to Excel" runat="server"
                            OnClick="btnExportToExcel_Click" />
                        <br />
                        <asp:GridView ID="gvAllException" runat="server" AutoGenerateColumns="False" DataKeyNames="CE_ID"
                            GridLines="Both" CellPadding="4" CssClass="mGrid1" PagerStyle-CssClass="pgr"
                            AlternatingRowStyle-CssClass="alt" Width="80%">
                            <Columns>
                                <asp:BoundField DataField="CDM_NAME" HeaderText="Department" />
                                <asp:TemplateField HeaderText="Attached File">
                                    <ItemTemplate>
                                        <a href="#" onclick="javascript:window.open('../DownloadFileCertification.aspx?FileInformation=<%#(Eval("CE_SERVER_FILE_NAME"))%>','','location=0,status=0,scrollbars=0,width=10,height=10');">
                                            <%#Eval("CE_CLIENT_FILE_NAME")%>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CE_EXCEPTION_TYPE" HeaderText="Applicable law/Regulation" />
                                <asp:BoundField DataField="CE_DETAILS" HeaderText="Observation and finding" />
                                <asp:BoundField DataField="CE_ROOT_CAUSE_OF_DEVIATION" HeaderText="Root Cause Of deviation" />
                                <asp:BoundField DataField="CE_ACTION_TAKEN" HeaderText="Action taken/Proposed" />
                                <asp:BoundField DataField="CE_TARGET_DATE" HeaderText="Target Date" DataFormatString="{0:dd-MMM-yyyy}" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="tabbertab" title="Regulatory FIlings">
                        <br />
                        <asp:Literal runat="server" ID="litRegulatoryFilling" />
                    </div>
                    <div class="tabbertab" title="Certification Dashboard">
                        <br />
                        <asp:Literal runat="server" ID="litSummary"></asp:Literal>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="vwInsert" runat="server">
                <div class="tabber">
                    <div class="tabbertab" title="Certification Details">
                        <table border="0" cellpadding="3" cellspacing="1" bgcolor="#dddddd" width="100%">
                            <tr>
                                <td class="tabhead3" width="20%">ID :</td>
                                <td class="tabbody3" width="80%">
                                    <asp:Label ID="lblCId" runat="server">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tabhead3">Department Name :</td>
                                <td class="tabbody3">
                                    <asp:Label ID="lblDept" runat="server">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tabhead3">Content:</td>
                                <td class="tabbody3">
                                    <asp:Label ID="lblContent" runat="server">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tabhead3">Remarks :</td>
                                <td class="tabbody3">
                                    <asp:Label ID="lblRmks" runat="server">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tabhead3">Submitted by :</td>
                                <td class="tabbody3">
                                    <asp:Label ID="lblSubmittedBy" runat="server">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tabhead3">Submitted On :</td>
                                <td class="tabbody3">
                                    <asp:Label ID="lblSubmittedDt" runat="server">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="tabbertab" title="Compliance Checklist">
                        <asp:GridView ID="gvCertApproval" runat="server" AutoGenerateColumns="False" DataKeyNames="CERT_ID"
                            GridLines="None" CellPadding="4" CssClass="mGrid1" PagerStyle-CssClass="pgr"
                            AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:TemplateField HeaderText="UH Certificate">
                                    <ItemTemplate>
                                        <a href="javascript:void(0);" onclick="javascript:window.open('./ViewCertificationContent.aspx?Type=L1&DeptId=<%# Eval("CSDM_ID") %>&Quarter=<%# Eval("CQM_ID") %>','','location=0,status=0,scrollbars=1,width=900,height=800,resizable=1');"
                                            style="text-align: center;">
                                            <img src="../../Content/images/legacy/viewfulldetails.png" border="0" alt="View Details" style="text-align: center;" />
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View">
                                    <ItemTemplate>
                                        <a href="javascript:void(0);" onclick="javascript:window.open('./ViewChecklist.aspx?Source=CXOApproval&CertID=<%# Eval("CERT_ID") %>','','location=0,status=0,scrollbars=1,width=900,height=800,resizable=1');"
                                            style="text-align: center;">
                                            <img src="../../Content/images/legacy/viewfulldetails.png" border="0" alt="View Details" style="text-align: center;" />
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Sr.No.
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblId" Text='<%# Bind("CERT_ID") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartmentId" runat="server" Text='<%# Bind("DeptId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblDepartment" runat="server" Text='<%# Bind("DeptName") %>' Visible="true"></asp:Label>
                                        <asp:HiddenField ID="hfStatus" runat="server" Value='<%#Eval("CERT_STATUS")%>'></asp:HiddenField>
                                        <asp:HiddenField ID="hfCertStatus" runat="server" Value='<%#Eval("Status")%>'></asp:HiddenField>
                                        <asp:HiddenField ID="hfFunctionName" runat="server" Value='<%#Eval("CDM_NAME") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quarter">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuarter" runat="server" Text='<%# Eval("CQM_FROM_DATE","{0:dd-MMM-yyyy}") +" to " + Eval("CQM_TO_DATE","{0:dd-MMM-yyyy}") %>'
                                            Visible="true"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Submitted By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubmittedBy" runat="server" Text='<%# Bind("Approval_By") %>' Visible="true"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Submitted On">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubmittedOn" runat="server" Text='<%# Bind("Approval_Date","{0:dd-MMM-yyyy HH:mm:ss}") %>'
                                            Visible="true"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Submission/Approval Comments">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubmittedRemarks" runat="server" Text='<%# Eval("Approval_Comments").ToString().Replace(Environment.NewLine, "<br />") %>'
                                            Visible="true"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Status" DataField="Status" SortExpression="Status" />
                                <asp:TemplateField HeaderText="Audit Trail">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAuditTrail" runat="server" Text='<%# Eval("CERT_AUDIT_TRAIL").ToString().Replace(Environment.NewLine, "<br />") %>'
                                            Visible="true"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Remarks
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <F2FControls:F2FTextBox ID="txtRemarks" TextMode="MultiLine" Rows="5" Columns="50" Enabled="False"
                                            CssClass="form-control" runat="server"></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                        <%--<cc1:FilteredTextBoxExtender ID="txtCharacters_FilteredTextBoxExtender" runat="server" Enabled="True"    
	                                TargetControlID="txtRemarks" FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers"    
	                                FilterMode="ValidChars" ValidChars="@;:_-.'/\?!, " >    
                                </cc1:FilteredTextBoxExtender>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <br />
                <center>
                    <asp:Button CssClass="html_button" ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" /></center>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
    <br />
    <br />
</asp:Content>
