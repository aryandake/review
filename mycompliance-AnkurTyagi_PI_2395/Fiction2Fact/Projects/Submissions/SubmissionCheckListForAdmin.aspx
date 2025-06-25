<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Submissions.Admin_SubmissionCheckListForAdmin" Title="Submission Checklist For Admin" CodeBehind="SubmissionCheckListForAdmin.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<<Added by Ashish Mishra on 27Jul2017--%>

    <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/dateformatvalidation.js") %>'>
    </script>

    <script type="text/javascript" src='<%=Fiction2Fact.Global.site_url("Content/js/Legacy/DateValidator.js") %>'>
    </script>

    <script type="text/javascript">
        function openSubmissionFilesWindow(scId) {
            window.open('../Submissions/SubmissionCheckListFiles.aspx?SCId=' + scId, '', 'location=0,status=0,scrollbars=1,width=600,height=400');
            return false;
        }
        function compareStartDateSystemDate(source, arguments) {
            try {
                var cnt = 0;
                var StartDate;
                var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurDate');
                var grid = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklistDetails");
                if (grid != null) {
                    if (grid.rows.length > 0) {
                        for (var j = 2; j < grid.rows.length + 1; j++) {
                            cnt++;
                            if (j < 10) {
                                j = "0" + j;
                            }
                            else {
                                j = j;
                            }
                            StartDate = document.getElementById("ctl00_ContentPlaceHolder1_gvChecklistDetails_ctl" + j + "_txtSubAuthorityDate");
                            //alert(StartDate);
                            if (StartDate != null) {
                                if (compare2Dates(SystemDate, StartDate) > 1) {
                                    arguments.IsValid = false;
                                    break;
                                }
                                else {
                                    arguments.IsValid = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (e) {
                alert(e);
                arguments.IsValid = true;
            }
        }

        function validateReOpenComments(ddlStatus, txtReOpenComments) {
            var ddl = document.getElementById(ddlStatus);
            var Status = ddl.options[ddl.selectedIndex].value;
            var ReOpenComments = document.getElementById(txtReOpenComments).value;

            if (Status == 'R' && ReOpenComments == '') {
                alert("Re-open Comments Cannot be blank.");
                return false;
            }
            else {
                return true;
            }

        }
    </script>

    <%-->>--%>
    <asp:HiddenField ID="hfMonth" runat="server" />
    <asp:HiddenField ID="hfClientId" runat="server" />
    <%--<<Added by Ashish Mishra on 27Jul2017--%>
    <asp:HiddenField ID="hfActiontype" runat="server" />
    <asp:HiddenField ID="hfCurDate" runat="server" />
    <%-->>--%>
    <br />
    <center>
        <div class="ContentHeader1">
            Re-open Submissions
        </div>
    </center>
    <asp:Label ID="lblMsg" runat="server" CssClass="label"></asp:Label>
    <br />
    <table border="0" cellpadding="3" cellspacing="1" bgcolor="#dddddd">
        <tr>
            <td class="tabhead3">Financial Year:
            </td>
            <td class="tabbody3">
                <asp:DropDownList CssClass="form-select" Width="170px" ID="ddlFinYear" runat="server"
                    DataValueField="FYM_ID" DataTextField="FYM_FIN_YEAR">
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="rfvFinYear" ControlToValidate="ddlFinYear" CssClass="span" Display="Dynamic" SetFocusOnError="True" ErrorMessage="*" ValidationGroup="Months">Please select Financial year
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="tabbody3 MonthLinks" colspan="2">
                <asp:LinkButton ID="lbtnApr" runat="server" Text="Apr" OnClick="lbtnApr_Click" CausesValidation="true"
                    ValidationGroup="Months" />|
                <asp:LinkButton ID="lbtnMay" runat="server" Text="May" OnClick="lbtnMay_Click" ValidationGroup="Months" />|
                <asp:LinkButton ID="lbtnJune" runat="server" Text="June" OnClick="lbtnJune_Click"
                    ValidationGroup="Months" />|
                <asp:LinkButton ID="lbtnJuly" runat="server" Text="July" OnClick="lbtnJuly_Click"
                    ValidationGroup="Months" />|
                <asp:LinkButton ID="lbtnAug" runat="server" Text="Aug" OnClick="lbtnAug_Click" ValidationGroup="Months" />|
                <asp:LinkButton ID="lbtnSep" runat="server" Text="Sep" OnClick="lbtnSep_Click" ValidationGroup="Months" />|
                <asp:LinkButton ID="lbtnOct" runat="server" Text="Oct" OnClick="lbtnOct_Click" ValidationGroup="Months" />|
                <asp:LinkButton ID="lbtnNov" runat="server" Text="Nov" OnClick="lbtnNov_Click" ValidationGroup="Months" />|
                <asp:LinkButton ID="lbtnDec" runat="server" Text="Dec" OnClick="lbtnDec_Click" ValidationGroup="Months" />|
                <asp:LinkButton ID="lbtnJan" runat="server" Text="Jan" OnClick="lbtnJan_Click" ValidationGroup="Months" />|
                <asp:LinkButton ID="lbtnFeb" runat="server" Text="Feb" OnClick="lbtnFeb_Click" ValidationGroup="Months" />|
                <asp:LinkButton ID="lbtnMarch" runat="server" Text="Mar" OnClick="lbtnMarch_Click"
                    ValidationGroup="Months" />
            </td>
        </tr>
    </table>
    <br />
    <asp:GridView ID="gvChecklistDetails" AllowPaging="False" runat="server" AutoGenerateColumns="False"
        OnSorting="gvChecklistDetails_Sorting" DataKeyNames="SC_ID" GridLines="None"
        AllowSorting="true" CellPadding="4" OnRowDataBound="gvChecklistDetails_RowDataBound"
        OnSelectedIndexChanged="gvChecklistDetails_SelectedIndexChanged" CssClass="mGrid1"
        PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:TemplateField HeaderText="Submission ID" SortExpression="SUB_ID" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblSubId" Text='<%#Eval("SUB_ID")%>' runat="server" Visible="false"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Submission Status" SortExpression="SUB_STATUS" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblSubStatus" Text='<%#Eval("SUB_STATUS")%>' runat="server" Visible="false"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Sr No.
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblSrNo" Text='<%# Container.DataItemIndex + 1 %>' runat="server"> 
                    </asp:Label>.
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="​Relevant​ Files">
                <ItemTemplate>
                    <a href="javascript:void(0);" onclick="javascript:window.open('../Submissions/SubmissionDocuments.aspx?Id=<%# Eval("SC_SM_ID") %>','','location=0,status=0,scrollbars=1,width=450,height=250,resizable=1');">
                        <asp:Image ID="imgView" runat="server" ImageUrl="../../Content/images/legacy/viewfulldetails.png" ToolTip="View ​Relevant​ Files" />
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Department" SortExpression="STM_TYPE">
                <ItemTemplate>
                    <asp:Label ID="lblstmType" Text='<%#Eval("STM_TYPE")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Segment" SortExpression="CSGM_NAME">
                <ItemTemplate>
                    <asp:Label ID="lblSegment" Text='<%#Eval("SSM_NAME")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <%-- Commented By Milan Yadav on 28-Nov-2016
           <asp:TemplateField HeaderText="Event" SortExpression="EM_EVENT_NAME">
                <ItemTemplate>
                    <asp:Label ID="lblEvent" Text='<%#Eval("EM_EVENT_NAME")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Agenda">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblAgenda" Text='<%#Eval("EP_NAME")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Event date" SortExpression="SC_DUE_DATE_FROM">
                <ItemTemplate>
                    <asp:Label ID="lbleventFrom" Text='<%#Eval("EI_EVENT_DATE","{0:dd-MMM-yyyy}")%>'
                        runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>--%>
            <%-- <asp:BoundField DataField="SC_PARTICULARS" SortExpression="SC_PARTICULARS" HeaderText="Particulars" />
            <asp:BoundField DataField="SC_DESCRIPTION" SortExpression="SC_DESCRIPTION" HeaderText="Descriptions" />--%>
            <asp:TemplateField HeaderText="Particulars" SortExpression="SC_PARTICULARS">
                <ItemTemplate>
                    <asp:Label ID="lblParticulars" Width="300px" Text='<%#Eval("SC_PARTICULARS").ToString().Replace(Environment.NewLine,"<br />") %>'
                        runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description" SortExpression="SC_DESCRIPTION">
                <ItemTemplate>
                    <asp:Label ID="lblDescription" Width="300px" Text='<%#Eval("SC_DESCRIPTION").ToString().Replace(Environment.NewLine,"<br />") %>'
                        runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Frequency" SortExpression="SC_FREQUENCY">
                <ItemTemplate>
                    <asp:Label ID="lblFrequency" Text='<%#Eval("SC_FREQUENCY")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Internal due date" SortExpression="SC_DUE_DATE_FROM">
                <ItemTemplate>
                    <asp:Label ID="lblDueDateFrom" Width="75px" Text='<%#Eval("SC_DUE_DATE_FROM","{0:dd-MMM-yyyy}")%>'
                        runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Regulatory due date" SortExpression="SC_DUE_DATE_TO">
                <ItemTemplate>
                    <asp:Label ID="lblDueDateTo" Width="75px" Text='<%#Eval("SC_DUE_DATE_TO","{0:dd-MMM-yyyy}")%>'
                        runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Yes/No/NA
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:DropDownList ID="ddlYesNoNA" runat="server" CssClass="form-select">
                        <asp:ListItem Text="-Select-" Value=""></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                        <asp:ListItem Text="NA" Value="NA"></asp:ListItem>
                    </asp:DropDownList>
                    <%--<asp:RadioButtonList ID="rblYesNoNA" runat="server" RepeatColumns="1" RepeatDirection="horizontal"
                        Width="100" Enabled="false">
                        <asp:ListItem Value="Y">Y</asp:ListItem>
                        <asp:ListItem Value="N">N</asp:ListItem>
                        <asp:ListItem Value="NA">NA</asp:ListItem>
                    </asp:RadioButtonList>--%>
                </ItemTemplate>
            </asp:TemplateField>
            <%--<<Commented by Ashish Mishra on 29Aug2017--%>
            <%--<asp:TemplateField HeaderText="Submission Date" SortExpression="SUB_SUBMIT_DATE">
                <ItemTemplate>
                    <asp:Label ID="lblSubDate" Width="75px" Text='<%#Eval("SUB_SUBMIT_DATE","{0:dd-MMM-yyyy}")%>'
                        runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>--%>
            <%-->>--%>
            <asp:TemplateField>
                <HeaderTemplate>
                    Remarks
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblRemarks" Text='<%# Bind("SUB_REMARKS") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="SUB_CREAT_BY" HeaderText="Submitted By" SortExpression="SUB_CREAT_BY" />
            <asp:TemplateField HeaderText="Submitted On" SortExpression="SUB_CREAT_DT">
                <ItemTemplate>
                    <asp:Label ID="lblSubmittedOn" Text='<%#Eval("SUB_CREAT_DT","{0:dd-MMM-yyyy HH:mm:ss}")%>'
                        runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="File uploaded by Reporting Function">
                <ItemTemplate>
                    <asp:HiddenField ID="ClientFileName" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="ServerFileName" runat="server" Value='<%# Bind("SUB_SERVER_FILE_NAME") %>'></asp:HiddenField>
                    <a id="Filelink<%# Container.DataItemIndex + 2  %>" href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=ChecklistFilesFolder&downloadFileName=<%# Eval("SUB_SERVER_FILE_NAME")%>','','location=0,status=0,scrollbars=0,width=20,height=20');">
                        <%#Eval("SUB_CLIENT_FILE_NAME")%>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Attach Files">
                <ItemTemplate>
                    <asp:LinkButton runat="server" Text="Attach" ID="lbAttach">
                     <img border="0" alt="" src="../../Content/images/legacy/attach.png" />
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Supportings">
                <ItemTemplate>
                    <asp:DataList ID="dlDescription" runat="server" CellPadding="1" EnableViewState="True"
                        RepeatColumns="1" RepeatDirection="Horizontal" DataSource='<%#LoadSubmissionFileList(Eval("SC_ID"))%>'>
                        <ItemTemplate>
                            <a style="text-decoration: underline;" href="javascript:void(0);" onclick="javascript:window.open('../DownloadFile3.aspx?FileInformation=<%#getFileName(Eval("SF_SERVER_FILE_NAME"))%>','','location=0,status=0,scrollbars=0,width=400,height=200');">
                                <%#Eval("SF_FILE_NAME")%>
                            </a>
                        </ItemTemplate>
                    </asp:DataList>
                </ItemTemplate>
            </asp:TemplateField>
            <%--<<Added by Ashish Mishra on 16Aug2017--%>
            <asp:TemplateField HeaderText="Mode of Filing">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlModeOfFiling" runat="server" DataTextField="RC_NAME" DataValueField="RC_CODE" CssClass="form-select">
                        <%--<asp:ListItem Value="" Text="(Select)" />
                        <asp:ListItem Value="E" Text="Email" />
                        <asp:ListItem Value="H" Text="Hardcopy" />--%>
                    </asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>
            <%-->>--%>
            <asp:TemplateField HeaderText="Submitted to Authority On / Date of Receipt of Data">
                <ItemTemplate>
                    <table>
                        <tr>
                            <td>
                                <F2FControls:F2FTextBox ID="txtSubAuthorityDate" runat="server" Columns="15" MaxLength="11"
                                    CssClass="form-control" Text='<%# Bind("SUB_SUBMITTED_TO_AUTHORITY_ON", "{0:dd-MMM-yyyy}") %>'></F2FControls:F2FTextBox>
                            </td>
                            <td>
                                <asp:ImageButton ID="imgSubDate" runat="server" AlternateText="Click to show calendar"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" OnClientClick="return false" />
                            </td>
                        </tr>
                    </table>
                    <%--                    <asp:RequiredFieldValidator ID="rfvSubDate" runat="server" ControlToValidate="txtSubAuthorityDate" CssClass="span"
                        Display="Dynamic" SetFocusOnError="True" ErrorMessage="Please select Submission Date.">*</asp:RequiredFieldValidator>--%>
                    <ajaxToolkit:CalendarExtender ID="ceSubDate" runat="server" PopupButtonID="imgSubDate"
                        TargetControlID="txtSubAuthorityDate" Format="dd-MMM-yyyy" CssClass="MyCalendar"></ajaxToolkit:CalendarExtender>
                    <asp:RegularExpressionValidator ID="revSubDate" runat="server" ControlToValidate="txtSubAuthorityDate" CssClass="span"
                        ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                        Display="Dynamic"></asp:RegularExpressionValidator>
                    <asp:CustomValidator ID="cvSubDate" runat="server" ControlToValidate="txtSubAuthorityDate" CssClass="span" ErrorMessage="Submission to Authority on date should not be greater than current date."
                        Display="Dynamic" ClientValidationFunction="compareStartDateSystemDate" OnServerValidate="cvSubDate_ServerValidate"> 
                    </asp:CustomValidator>
                </ItemTemplate>
            </asp:TemplateField>
            <%-->>--%>
            <%--<<Added by Ashish Mishra on 28Jul2017--%>
            <asp:TemplateField>
                <HeaderTemplate>
                    Comments
                </HeaderTemplate>
                <ItemTemplate>
                    <F2FControls:F2FTextBox ID="txtReOpenComments" TextMode="MultiLine" Rows="4" Width="200" CssClass="form-control"
                        Text='<%# Bind("SUB_REOPEN_COMMENTS") %>' runat="server"></F2FControls:F2FTextBox>
                    <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtReOpenComments" />
                </ItemTemplate>
            </asp:TemplateField>
            <%-->>--%>
            <asp:TemplateField HeaderText="Change Status">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                        <asp:ListItem Value="" Text="(None)" />
                        <asp:ListItem Value="P" Text="Pending" />
                        <asp:ListItem Value="R" Text="Reopened" />
                        <asp:ListItem Value="S" Text="Submitted" />
                        <%--<<Added by Ashish Mishra on 28Jul2017--%>
                        <asp:ListItem Value="C" Text="Closed" />
                        <%-->>--%>
                    </asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="lbSave" runat="server" Text="Save" CommandName="Select">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
