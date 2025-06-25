<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Circulars.Circulars_EditCircularActionables" Title="Edit Circular Actionables"
    Async="true" CodeBehind="EditCircularActionables.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register TagPrefix="rif" Namespace="Fiction2Fact.Controls" Assembly="RequiredIfValidator" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js")%>"></script>

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

    //Added By Milan Yadav on 17-Aug-2016
    //>>
    function compareEndSystemDates(source, arguments) {
        try {
            //var ContractTemplateId = document.getElementById('ctl00_ContentPlaceHolder1_hfCTId').value;
            //if (ContractTemplateId == '' || ContractTemplateId == null) {

            var Fromdate = document.getElementById('ctl00_ContentPlaceHolder1_txtCompletionDate');
            var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurDate');

            if (compare2Dates(SystemDate, Fromdate) > 1) {
                arguments.IsValid = false;
            }
            else {
                arguments.IsValid = true;
            }
        }
        //    else {
        //        arguments.IsValid = true;
        //    }
        //}
        catch (e) {
            alert(e);
            arguments.IsValid = false;
        }
    }
    //<<
    //Added By Milan Yadav on 30-Aug-2016
    //>>
    function deletefile() {
        if (!confirm('Are you sure that you want to delete this file?')) return false;
    }
    //<<

    function validateUpdates() {
        if (Page_ClientValidate("Save")) {
            var IsDoubleClickFlagSet = document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value;
              if (IsDoubleClickFlagSet == 'Yes') {
                  alert("You have double clicked on the same button. " +
                          "Please wait till the operation is successfully completed.");
                  return false;
              }
              else {
                  document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                    return true;
                }
            }
        }
    </script>

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/tabcontrol.js")%>">
    </script>

    <center>
        <br />
        <asp:HiddenField runat="server" ID="hfSource" />
        <asp:HiddenField runat="server" ID="hfTabberId" />
        <asp:HiddenField ID="hfCircularId" runat="server" />
        <asp:HiddenField ID="hfActionableId" runat="server" />
        <asp:HiddenField ID="hfCurDate" runat="server" />
        <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />
        <asp:HiddenField ID="hfReportingMgrEmailId" runat="server" />
        <div class="ContentHeader1">
            Edit Actionable
        </div>
    </center>
    <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
    <div style="text-align: left;">
        <div class="tabber">
            <div class="tabbertab">
                <h2>Actionable Details
                </h2>
                <br />
                <table border="0" cellpadding="3" cellspacing="1" bgcolor="#dddddd" width="100%">
                    <tr>
                        <td class="tabhead3" width="30%">Created By:
                        </td>
                        <td class="tabbody3" width="70%">
                            <asp:Label ID="lblCreator" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tabhead3">Issuing Authority:
                        </td>
                        <td class="tabbody3">
                            <asp:Label ID="lblAuthority" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tabhead3">Type of Document:
                        </td>
                        <td class="tabbody3">
                            <asp:Label ID="lblTypeofDocument" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <%-- <tr>
                        <td class="tabhead3">
                            Intimated To:
                        </td>
                        <td class="tabbody3">
                            <asp:CheckBoxList ID="cbSubmissions" RepeatColumns="5" runat="server" DataTextField="CIM_TYPE"
                                DataValueField="CIM_ID" AppendDataBoundItems="True" Enabled="false">
                            </asp:CheckBoxList>
                        </td>
                    </tr>--%>
                    <tr>
                        <td class="tabhead3">Topic:
                        </td>
                        <td class="tabbody3">
                            <asp:Label ID="lblTopic" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tabhead3">Circular No.:</td>
                        <td class="tabbody3">
                            <asp:Label ID="lblCircularNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tabhead3">Circular Date:
                        </td>
                        <td class="tabbody3">
                            <asp:Label ID="lblCircularDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tabhead3">Subject of the Circular:</td>
                        <td class="tabbody3">
                            <asp:Label ID="lblSubject" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tabhead3">Gist of the Circular:</td>
                        <td class="tabbody3">
                            <asp:Label ID="lblGist" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tabhead3">Implications:</td>
                        <td class="tabbody3">
                            <asp:Label ID="lblImplications" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tabhead3">Issuer Link:</td>
                        <td class="tabbody3">
                            <asp:Label ID="lblLink" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tabhead3" colspan='2'>Attachment :</td>
                    </tr>
                    <tr>
                        <td class="tabbody3" colspan='2'>
                            <asp:GridView ID="gvViewFileUpload" runat="server" CssClass="mGrid1" PagerStyle-CssClass="pgr"
                                AlternatingRowStyle-CssClass="alt" CellPadding="4" GridLines="None" AutoGenerateColumns="False"
                                DataKeyNames="CF_ID">
                                <Columns>
                                    <asp:TemplateField Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblServerFileName" Text='<%#Eval("CF_FILENAME") %>' runat="server"> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="File Name">
                                        <ItemTemplate>
                                            <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=Circular&downloadFileName=<%#getFileName(Eval("CF_SERVERFILENAME"))%>','','location=0,status=0,scrollbars=1,width=10,height=20');return false">
                                                <%#Eval("CF_FILENAME")%>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Uploaded By">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUploaderName" runat="server" Text='<%# Eval("CF_CREAT_BY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Uploaded On">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreateDate" runat="server" Text='<%# Eval("CF_CREAT_DT", "{0:dd-MMM-yyyy HH:mm:ss}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td class="tabhead3" colspan='2'>Actionable Details:</td>
                    </tr>
                    <tr>
                        <td class="tabbody3" colspan='2'>
                            <table border="0" cellpadding="3" cellspacing="1" bgcolor="#dddddd" width="100%">
                                <tr>
                                    <td class="tabhead3">Actionable:
                                    </td>
                                    <td class="tabbody3">
                                        <F2FControls:F2FTextBox runat="server" ID="txtActionable" CssClass="form-control" TextMode="MultiLine"
                                            Rows="3" Columns="48"></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtActionable" />
                                        <asp:RequiredFieldValidator runat="server" ID="rfvActionable" ControlToValidate="txtActionable" CssClass="span"
                                            ValidationGroup="SaveActionable" Display="Dynamic" ErrorMessage="Please enter Actionable.">Please enter Actionable.</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td class="tabhead3">
                                        Person/Function Responsible:
                                    </td>
                                    <td class="tabbody3">
                                        <asp:DropDownList ID="ddlPersonResponsible" runat="server" CssClass="form-select"
                                            DataTextField="CPRM_NAME" DataValueField="CPRM_ID">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvPersonResponsible" ControlToValidate="ddlPersonResponsible"
                                            ValidationGroup="SaveActionable" Display="Dynamic" ErrorMessage="Please select Person/Function Responsible.">Please select Person/Function Responsible.</asp:RequiredFieldValidator>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="tabhead3">Person Responsible User Id:
                                    </td>
                                    <td class="tabbody3">
                                        <F2FControls:F2FTextBox runat="server" ID="txtPersonResponsibleUserId" Columns="50" CssClass="form-control"></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtPersonResponsibleUserId" />
                                        <asp:RequiredFieldValidator runat="server" ID="rfvPersonResponsibleUserId" ControlToValidate="txtPersonResponsibleUserId" CssClass="span"
                                            ValidationGroup="SaveActionable" Display="Dynamic" ErrorMessage="Please enter Person Responsible User Id.">Please enter Person Responsible User Id.</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tabhead3">Person Responsible User Name:
                                    </td>
                                    <td class="tabbody3">
                                        <F2FControls:F2FTextBox runat="server" ID="txtPersonResponsibleUserName" Columns="50" CssClass="form-control"></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtPersonResponsibleUserName" />
                                        <asp:RequiredFieldValidator runat="server" ID="rfvPersonResponsibleUserName" ControlToValidate="txtPersonResponsibleUserName" CssClass="span"
                                            ValidationGroup="SaveActionable" Display="Dynamic" ErrorMessage="Please enter Person Responsible User Name.">Please enter Person Responsible User Name.</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tabhead3">Person Responsible Email Id:
                                    </td>
                                    <td class="tabbody3">
                                        <F2FControls:F2FTextBox runat="server" ID="txtPersonResponsibleEmailId" Columns="50" CssClass="form-control"></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredEmail runat="server" TargetControlID="txtPersonResponsibleEmailId" />
                                        <asp:RequiredFieldValidator runat="server" ID="rfvPersonResponsibleEmailId" ControlToValidate="txtPersonResponsibleEmailId" CssClass="span"
                                            ValidationGroup="SaveActionable" Display="Dynamic" ErrorMessage="Please enter Person Responsible Email Id.">Please enter Person Responsible Email Id.</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tabhead3">Target Date:
                                    </td>
                                    <td class="tabbody3">
                                        <F2FControls:F2FTextBox runat="server" ID="txtTargetDate" CssClass="form-control" Columns="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgTargetDate" runat="server" AlternateText="Click to show calendar"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                        <ajaxToolkit:CalendarExtender ID="ceTargetDate" runat="server" PopupButtonID="imgTargetDate"
                                            TargetControlID="txtTargetDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="revTargetDate" runat="server" ControlToValidate="txtTargetDate" CssClass="span"
                                            ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                            ValidationGroup="SaveActionable" Display="Dynamic"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvTargetDate" ControlToValidate="txtTargetDate" CssClass="span"
                                            ValidationGroup="SaveActionable" Display="Dynamic" ErrorMessage="Please enter Target Date.">Please enter Target Date.</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tabhead3">Remarks:
                                    </td>
                                    <td class="tabbody3">
                                        <F2FControls:F2FTextBox runat="server" ID="txtCirActRemarks" CssClass="form-control" TextMode="MultiLine"
                                            Rows="3" Columns="48"></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtCirActRemarks" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tabhead3">Status:
                                    </td>
                                    <td class="tabbody3">
                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select" DataTextField="RC_NAME"
                                            DataValueField="RC_CODE">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvStatus" ControlToValidate="ddlStatus" CssClass="span"
                                            ValidationGroup="SaveActionable" Display="Dynamic" ErrorMessage="Please select Status.">Please select Status.</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tabhead3">Completion Date:
                                    </td>
                                    <td class="tabbody3">
                                        <F2FControls:F2FTextBox runat="server" ID="txtCompletionDate" CssClass="form-control" Columns="50"></F2FControls:F2FTextBox>
                                        <asp:ImageButton runat="server" ID="imgCompletionDate" ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                        <ajaxToolkit:CalendarExtender runat="server" ID="ceCompletionDate" PopupButtonID="imgCompletionDate"
                                            TargetControlID="txtCompletionDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="revCompletionDate" runat="server" ControlToValidate="txtCompletionDate" CssClass="span"
                                            ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                            ValidationGroup="SaveActionable" Display="Dynamic"></asp:RegularExpressionValidator>
                                        <rif:RequiredIfValidator ID="rIfValidatorCompletionDate" runat="server" TriggerValue="C" CssClass="span"
                                            ControlToCompare="ddlStatus" ControlToValidate="txtCompletionDate" Display="Dynamic"
                                            SetFocusOnError="True" EnableClientScript="true" ValidationGroup="SaveActionable"
                                            ErrorMessage="Please enter Completion Date.">
                                    Please enter Completion Date.
                                        </rif:RequiredIfValidator>
                                        <%-- //Added By Milan Yadav on 25-Aug-2016
                                        >>--%>
                                        <asp:CustomValidator ID="cvCompletionDate" runat="server" ClientValidationFunction="compareEndSystemDates" CssClass="span"
                                            ControlToValidate="txtCompletionDate" ErrorMessage="Completion Date should be less than or Equal to System Date."
                                            Display="Dynamic" OnServerValidate="cvTodate_ServerValidate" ValidationGroup="SaveActionable"></asp:CustomValidator>
                                        <%--<<--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tabhead3">Completion Remarks:
                                    </td>
                                    <td class="tabbody3">
                                        <F2FControls:F2FTextBox runat="server" ID="txtClosureRemarks" CssClass="form-control" TextMode="MultiLine"
                                            Rows="3" Columns="48"></F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtClosureRemarks" />
                                        <rif:RequiredIfValidator ID="rIfValidatorClosureRemarks" runat="server" TriggerValue="C" CssClass="span"
                                            ControlToCompare="ddlStatus" ControlToValidate="txtClosureRemarks" Display="Dynamic"
                                            SetFocusOnError="True" EnableClientScript="true" ValidationGroup="SaveActionable"
                                            ErrorMessage="Please enter Completion Remarks.">
                                    Please enter Completion Remarks.
                                        </rif:RequiredIfValidator>
                                    </td>
                                </tr>
                                <%--                                //Added By Milan Yadav on 30-Aug-2016
                                //>>--%>
                                <tr id="trAttachment" runat="server">
                                    <td class="tabhead3">Attachments:</td>
                                    <td class="tabbody3">
                                        <asp:FileUpload ID="fuActionableFileUpload" runat="server" Width="350px" CssClass="form-control" />
                                        <asp:HiddenField ID="hfFileNameOnServer" runat="server" />
                                        <asp:Button CssClass="html_button" ID="btnActionableAttachment" runat="server" Text="Attach"
                                            Width="95px" Height="25px" OnClick="btnActionableAttachment_Click" />
                                    </td>
                                </tr>
                                <tr id="trAttachmentFiles" runat="server">
                                    <td class="tabhead3">Files:
                                    </td>
                                    <td colspan="2" class="tabbody3">
                                        <asp:GridView ID="gvFileUpload" runat="server" AllowPaging="false" AllowSorting="false"
                                            BorderStyle="None" BorderWidth="1px" Width="500" DataKeyNames="FileNameOnServer"
                                            OnSelectedIndexChanged="gvFileUpload_SelectedIndexChanged" OnRowDataBound="gvFileUpload_RowDataBound"
                                            CssClass="mGrid1" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                            <Columns>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbDelete" runat="server" CausesValidation="False" CommandName="Select"
                                                            OnClientClick="return deletefile();">
                                                            <img id="imgdel" alt="Delete" src="../../Content/images/legacy/delete.png" runat="server" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="File Name">
                                                    <ItemTemplate>
                                                        <a href="javascript:void(0)" onclick="javascript:window.open('../CommonDownload.aspx?type=Circular&downloadFileName=<%#(Eval("FileNameOnServer"))%>','','location=0,status=0,scrollbars=0,width=50,height=50');">
                                                            <%#Eval("FileName")%>
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <%-- //<<--%>
                            </table>
                            <center>
                                <asp:Button CssClass="html_button" ID="btnSaveActionable" ValidationGroup="SaveActionable"
                                    runat="server" Text="Save" OnClick="btnSaveActionable_Click" />
                            </center>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
            </div>
            <div class="tabbertab" style="text-align: left;">
                <h2>Add New Update</h2>
                <br />
                <table border="0" cellpadding="3" cellspacing="1" bgcolor="#dddddd" width="98%">
                    <tr>
                        <td class="tabhead3">Update Type :
                            <asp:Label ID="Label2" runat="server" CssClass="label" Text="*"></asp:Label>
                        </td>
                        <td class="tabbody3">
                            <asp:DropDownList CssClass="form-select" ID="ddlUpdateType" runat="server" DataTextField="RC_NAME"
                                DataValueField="RC_CODE">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvUpdateType" runat="server" ControlToValidate="ddlUpdateType" CssClass="span"
                                Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True">Please select Update Type.</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tabhead3">Details :
                            <asp:Label ID="Label1" runat="server" CssClass="label" Text="*"></asp:Label>
                        </td>
                        <td class="tabbody3">
                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtRemarks" runat="server" TextMode="MultiLine"
                                Columns="50" Rows="3"></F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                            <asp:RequiredFieldValidator ID="rfvRemarks" runat="server" ControlToValidate="txtRemarks" CssClass="span"
                                Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True">Please enter Details.</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tabhead3">
                            <asp:Label ID="lblAttachments" runat="server" Text="Upload File"></asp:Label></td>
                        <td class="tabbody3">
                            <asp:FileUpload ID="fuFileUpload" runat="server" Width="411px" CssClass="form-control" />
                            <asp:RegularExpressionValidator ID="revFileUpload" runat="server" ControlToValidate="fuFileUpload" CssClass="span"
                                Display="Dynamic" ErrorMessage="Upload of this file format is not supported."
                                ValidationExpression="^.+(.msg|.MSG|.eml|.EML|.Eml|.jpg|.JPG|.bmp|.BMP|.xls|.XLS|.DOC|.doc|.pdf|.PDF|.PPSX|.ppsx|.)$"
                                ValidationGroup="FeesDetails"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
                <br />
                <center>
                    <asp:Button CssClass="html_button" ID="btnSave" ValidationGroup="Save" CausesValidation="true"
                        runat="server" Text="Save Update" OnClientClick="return validateUpdates();" OnClick="btnSave_Click" />
                </center>
                <br />
                <br />
            </div>
            <div class="tabbertab" style="text-align: left;">
                <h2>Updates Trail</h2>
                <br />
                <asp:GridView ID="gvCircularActionableUpdates" runat="server" AutoGenerateColumns="False"
                    AllowSorting="false" AllowPaging="true" GridLines="Both" CellPadding="4" CssClass="mGrid1"
                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvCircularActionableUpdates_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Sr.No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="UPDATE_TYPE" HeaderText="Update Type" />
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Remarks
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRemarks" Text='<%#Eval("CAU_REMARKS").ToString().Replace(Environment.NewLine, "<br />") %>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Uploaded File">
                            <ItemTemplate>
                                <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=Circular&downloadFileName=<%#getFileName(Eval("CAU_SERVER_FILE_NAME"))%>','','location=0,status=0,scrollbars=1,width=10,height=20');return false">
                                    <%#Eval("CAU_FILE_NAME")%>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CREATE_BY" HeaderText="Update Added By" />
                        <asp:TemplateField HeaderText="Update Added On">
                            <ItemTemplate>
                                <asp:Label ID="lblCreateDate" runat="server" Text='<%# Eval("CREATE_DT", "{0:dd-MMM-yyyy HH:mm:ss}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <br />
    <center>
        <asp:Button CssClass="html_button" ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
    </center>
    <br />
    <br />
</asp:Content>
