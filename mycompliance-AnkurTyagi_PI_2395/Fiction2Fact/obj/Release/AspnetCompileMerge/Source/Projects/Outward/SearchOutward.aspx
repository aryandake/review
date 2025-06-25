<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" 
    EnableEventValidation="false" Async="true"
    Inherits="Fiction2Fact.Projects.Outward.SearchOutward" Title="Search Outwards" CodeBehind="SearchOutward.aspx.cs" %>

<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="custom" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/Actionables.js")%>"></script>

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js")%>"></script>

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/populateUserDetsAJAX.js")%>"></script>

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/dateformatvalidation.js")%>"></script>

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/commonvalidations.js")%>"></script>

    <%--<script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/base64.js") %>"></script>--%>

    <link type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/css/legacy/ui-lightness/jquery-ui-1.8.19.custom.css")%>" rel="stylesheet" />
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/jquery-ui-1.8.19.custom.min.js")%>"></script>

    <script type="text/javascript">

        /* Optional: Temporarily hide the "tabber" class so it does not "flash"
           on the page as plain HTML. After tabber runs, the class is changed
           to "tabberlive" and it will appear. */

        document.write('<style type="text/css">.tabber{display:none;}<\/style>');

        /*==================================================
          Set the tabber options (must do this before including tabber.js)
          ==================================================*/

    </script>
   
    <script type="text/javascript">
        function f(OId) {
            window.open('ViewOutward.aspx?Id=' + OId, '', 'location=0,status=0,resizable=1,scrollbars=1,width=1000,height=800');
            return false;
        }
        function onViewClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "View";
        }
        function onLinkedOutwardViewClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "ViewLinkedOutward";
        }
        function onClientEditClick() {

            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
        }
        function onClientRevisionClick() {

            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Revision";
        }
        function onClientDeleteClick() {
            if (!confirm('Are you sure that you want to delete this record?')) return false;
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Delete";

        }
        function compareEndSystemDates(source, arguments) {
            try {

                var Fromdate = document.getElementById('ctl00_ContentPlaceHolder1_txtFromDate');
                var ToDate = document.getElementById('ctl00_ContentPlaceHolder1_txtToDate');

                if (Fromdate.value != '') {
                    if (compare2Dates(ToDate, Fromdate) > 1) {
                        arguments.IsValid = false;
                    }
                    else {
                        arguments.IsValid = true;
                    }
                }
                else {
                    arguments.IsValid = true;
                }
            }
            catch (e) {
                alert(e);
                arguments.IsValid = false;
            }
        }
        function OnCancellation(OtId) {
            document.getElementById('<%=hfSelectedRecord.ClientID%>').value = OtId;
            $("#divModal").modal('show');
            return false;
        }
        function OnRevision(OtId) {
            document.getElementById('<%=hfSelectedRecord.ClientID%>').value = OtId;
            $("#divModal_Revision").modal('show');
            return false;
        }
        function OnDelete(OtId) {
            document.getElementById('<%=hfSelectedRecord.ClientID%>').value = OtId;
            $("#divModal_Deletion").modal('show');
            return false;
        }
    </script>

    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField runat="server" ID="hfTabberId" />
    <asp:HiddenField ID="hfSelectedRecord" runat="server" />
    <asp:HiddenField ID="hfOutwardIdForSearch" runat="server" />
     <asp:HiddenField ID="hftype" runat="server" />
    <br />
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">
                            <asp:Label ID="lblHeadrName" runat="server" Text="Search Outward Tracker"></asp:Label>

                        </h4>
                        <asp:Label ID="lblMsg" runat="server" CssClass="label" Visible="false"></asp:Label>
                    </div>
                    <!--end col-->
                </div>
                <!--end row-->
            </div>
            <!--end page-title-box-->
        </div>
        <!--end col-->
    </div>

    <asp:MultiView ID="mvMultiView" runat="server">
        <asp:View ID="vwGrid" runat="server">
            <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
                <div style="text-align: left">

                    <div class="row">
                         <div class="col-md-3 mb-3" >
                             Outward No:&nbsp;
                             <div class="input-group">
                             <asp:TextBox CssClass="form-control" ID="txtDocNo" runat="server"></asp:TextBox>
                                 <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtDocNo" />
                                 </div>
                         </div>
                        <div class="col-md-3 mb-3" >
                             Type of Outward:
                               <asp:DropDownList CssClass="form-select" ID="ddlOutwardType" runat="server" DataValueField="OTM_ID"
                                    DataTextField="OTM_NAME" >
                                </asp:DropDownList>
                         </div>
                        <div class="col-md-3 mb-3" >
                            Department:
                            <asp:DropDownList CssClass="form-select" ID="ddlDept" runat="server" DataValueField="ODM_ID"
                                    DataTextField="ODM_NAME" >
                                </asp:DropDownList>
                         </div>
                         <div class="col-md-3 mb-3" >
                             Subject:
                            <asp:TextBox CssClass="form-control" ID="txtDocumentName" runat="server"></asp:TextBox>
                             <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtDocumentName" />
                         </div>
                    </div>
                    <div class="row">
                        
                         <div class="col-md-3 mb-3" >
                             Status:
                             <asp:DropDownList CssClass="form-select" ID="ddlStatus" runat="server" >
                                    <asp:ListItem Value="">(Select)</asp:ListItem>
                                    <asp:ListItem Value="Cancelled">Cancelled</asp:ListItem>
                                    <asp:ListItem Value="Closed">Closed</asp:ListItem>
                                    <asp:ListItem Value="Changes suggested by Compliance">Changes suggested by Compliance</asp:ListItem>
                                    <asp:ListItem Value="Submitted">Submitted</asp:ListItem>
                                     <asp:ListItem Value="Deleted">Deleted</asp:ListItem>
                                 </asp:DropDownList>
                             
                         </div>
                        
                       <div class="col-md-3 mb-3">
                                    <label class="form-label">From Date</label>
                                    <div class="input-group">
                                         <asp:TextBox CssClass="form-control" ID="txtFromDate" runat="server"></asp:TextBox>
                                        <asp:ImageButton ID="ibFrmDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <asp:RegularExpressionValidator ID="revFromDate" runat="server" ControlToValidate="txtFromDate" CssClass="text-danger"
                                        Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                        ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-3 mb-3">
                                    <label class="form-label">To Date</label>
                                    <div class="input-group">
                                         <asp:TextBox CssClass="form-control" ID="txtToDate" runat="server"></asp:TextBox>
                                       <asp:ImageButton ID="ibToDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <asp:RegularExpressionValidator ID="revToDate" runat="server" ControlToValidate="txtToDate" CssClass="text-danger"
                                        Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                        ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                                    <asp:CustomValidator ID="cvCompletionDate" runat="server" ClientValidationFunction="compareEndDates"
                                        ControlToValidate="txtToDate" ErrorMessage="From Date should be less than or Equal to To Date."
                                        Display="Dynamic" OnServerValidate="cvTodate_ServerValidate" ValidationGroup="SEARCH"></asp:CustomValidator>
                                </div>
                         <cc1:CalendarExtender ID="ceFrmDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibFrmDate"
                                TargetControlID="txtFromDate"></cc1:CalendarExtender>
                         <cc1:CalendarExtender ID="ceTodate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibToDate"
                                    TargetControlID="txtToDate"></cc1:CalendarExtender>
                         <div class="col-md-3 mb-3" >

                             Global Search:
                               <asp:TextBox CssClass="form-control" ID="txtGlobalSearch" runat="server"></asp:TextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtGlobalSearch" />
                                
                         </div>
                    </div>
                    
                    <div class="row">
                        
                        
                    </div>
                    <div class="row">
                         <div class="col-md-3 mb-3" >
                             
                              <asp:Button ID="btnSearch" runat="server" ValidationGroup="SEARCH" AccessKey="s"
                                OnClick="btnSearch_Click" Text="Search" CssClass="btn btn-outline-success" />
                            <asp:Button ID="btnExportToExcel" runat="server" OnClick="btnExportToExcel_Click"
                                CssClass="btn btn-outline-success" Text="Export To Excel" />
                             <asp:Button ID="btnreset" runat="server" 
                                CssClass="btn btn-outline-success" Text="Reset" OnClick="btnreset_Click" />
                             
                        </div>
                    </div>
                    <div class="row">
                        <asp:Label ID="lblInfo" runat="server" CssClass="text-danger" Visible="false"></asp:Label><br />
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="gvOutward" PageSize="10"  runat="server" AutoGenerateColumns="False"
                    CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                    DataKeyNames="OT_ID" AllowSorting="false" AllowPaging="true" GridLines="Both"
                    CellPadding="4" OnSelectedIndexChanged="gvOutward_SelectedIndexChanged"
                    OnPageIndexChanging="gvOutward_PageIndexChanging"
                    OnSorting="gvOutward_Sorting" OnDataBound="gvOutward_DataBound" OnRowDataBound="gvOutward_RowDataBound" OnRowCreated="OnRowCreated">
                    <Columns>
                        <asp:TemplateField HeaderText="View" ShowHeader="true">
                            <ItemTemplate>
                                <center>
                                    <asp:LinkButton ID="lnkView" runat="server" CommandName="Select" CssClass="centerLink"
                                        
                                        OnClientClick="onViewClick()">
                                        <i class="fa fa-eye"></i>
                                    </asp:LinkButton>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" ShowHeader="true">
                            <ItemTemplate>
                                <center>
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CssClass="centerLink" 
                                       OnClientClick="onClientEditClick()">
                                          <i class="fa fa-pen"></i>
                                    </asp:LinkButton>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Revision Suggested" ShowHeader="true">
                            <ItemTemplate>
                                <center>
                                    <%--<asp:LinkButton ID="lnkRevisioSuggested" runat="server" CommandName="Select" CssClass="centerLink"
                                        
                                        OnClientClick="onClientRevisionClick()">
                                        <i class="fa fa-share"></i>
                                    </asp:LinkButton>--%>
                                     <asp:LinkButton ID="lnkRevisioSuggested" runat="server" CausesValidation="False" OnClientClick='<%# string.Format("return OnRevision(\"{0}\");", Eval("OT_ID")) %>'
                                        Text='<%# "<img alt=\"Cancellation\" height=\"25px\" src=\"" + Fiction2Fact.Global.site_url("Content/images/legacy/Revise-128.png") + "\" border=\"0\" >" %>'>
                                    </asp:LinkButton>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete" ShowHeader="true">
                            <ItemTemplate>
                                <center>
                                    <%--<asp:LinkButton ID="lnkDelete" runat="server" CommandName="Select" CssClass="centerLink"
                                        OnClientClick="return onClientDeleteClick()">
                                          <i class="fa fa-trash"></i>
                                    </asp:LinkButton>--%>
                                     <asp:LinkButton ID="lnkDeleteOutward" runat="server" CausesValidation="False" OnClientClick='<%# string.Format("return OnDelete(\"{0}\");", Eval("OT_ID")) %>'
                                        Text='<%# "<img alt=\"Delete\" height=\"25px\" src=\"" + Fiction2Fact.Global.site_url("Content/images/legacy/delete_icon.png") + "\" border=\"0\" >" %>'>
                                    </asp:LinkButton>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField ShowHeader="true" HeaderText="Cancellation">
                            <ItemTemplate>
                                <center>
                                    <asp:LinkButton ID="lnkDeactivate" runat="server" CausesValidation="False" OnClientClick='<%# string.Format("return OnCancellation(\"{0}\");", Eval("OT_ID")) %>'
                                        Text='<%# "<img alt=\"Cancellation\" height=\"25px\" src=\"" + Fiction2Fact.Global.site_url("Content/images/legacy/deactivate.png") + "\" border=\"0\" >" %>'>
                                    </asp:LinkButton>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Sr.No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                    <asp:Label ID="lbId" Text='<% # Eval("OT_ID") %>' Visible="false" runat="server"></asp:Label>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="OTM_NAME" HeaderText="Type of Outward" SortExpression="OTM_NAME" />
                        <asp:BoundField DataField="ODM_NAME" HeaderText="Department" SortExpression="ODM_NAME" />
                       
                        <asp:BoundField DataField="OT_DOCUMENT_NO" HeaderText="Outward No." SortExpression="OT_DOCUMENT_NO" />
                        
                        <asp:TemplateField HeaderText="Linked Outward" SortExpression="OT_DATE">
                            <ItemTemplate>
                                <asp:HiddenField ID="hfLinkedOutwardId" runat="server" Value='<%#Eval("OT_BASE_OUTWARD_ID") %>' /> 
                                <asp:LinkButton ID="lnkViewLinkedOutward" runat="server" CommandName="Select" CssClass="centerLink" style="text-decoration:underline;"
                                        
                                       OnClientClick="onLinkedOutwardViewClick()" Text='<%# Eval("OT_BASE_OUTWARD") %>'>
                                        
                                    </asp:LinkButton> <%-- OnClientClick="onViewClick1()"--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Document Dated" SortExpression="OT_DATE">
                            <ItemTemplate>
                                <asp:Label ID="lblOutwardDt" runat="server" Text='<%# Eval("OT_DATE", "{0:dd-MMM-yyyy }") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="OT_DOC_NAME" HeaderText="Subject" SortExpression="OT_DOC_NAME" />
                       <asp:BoundField DataField="ORAM_NAME" HeaderText="Regulatory Authority" SortExpression="ORAM_NAME" />
                        <%--<asp:BoundField DataField="AddressorName" HeaderText="From (Sender)" SortExpression="AddressorName" />
                        <asp:BoundField DataField="OT_ADRESSEE" HeaderText="To (Receiver)" SortExpression="OT_ADRESSEE" />
                        --%>
                        <asp:BoundField DataField="OT_CREATE_BY" HeaderText="Created By" SortExpression="OT_CREATE_BY" />
                        <asp:BoundField DataField="OT_CREATE_DATE" HeaderText="Created On" SortExpression="OT_CREATE_DATE" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}" />

                        <asp:TemplateField HeaderText="Status" SortExpression="OT_STAUTS">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("OT_STAUTS") %>'></asp:Label>
                                <asp:Label ID="lblCreator" runat="server" Text='<%# Eval("OT_CREATOR") %>' Visible="false"></asp:Label>
                               
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Attachments">
                            <ItemTemplate>
                                <asp:DataList ID="dlContractFiles" runat="server" RepeatColumns="1" RepeatDirection="vertical"
                                    DataSource='<%# LoadOutwardsFileList(Eval("OT_ID")) %>'>
                                    <ItemTemplate>
                                        <a href="#" onclick="javascript:window.open('../CommonDownloadAnyFile.aspx?type=Outward&downloadFileName=<%# Eval("OF_FILE_NAME_ON_SERVER")%>&fileName=<%#Eval("OF_FILE_NAME")%>','','location=0,status=0,scrollbars=1,width=400,height=200');">
                                            <%#Eval("OF_FILE_NAME")%>
                                            <%--<asp:Image ID="lblIssuerLink" runat="server" ImageUrl="../../Content/images/legacy/viewfulldetails.png" />--%>
                                        </a>
                                        <%--<asp:Label ID="lblfiles" runat="server" Text='<%# Eval("OF_FILE_NAME") %>' Visible="false" ></asp:Label>--%>
                                    </ItemTemplate>
                                </asp:DataList>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                        </div>
                    </div>
                        
                  

                    
                   
                </div>
                
            </asp:Panel>
        </asp:View>
        <asp:View ID="vwForm" runat="server">
            <div style="width: 100%;">
      <%--  <link type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/bootstrap.min.css") %>"
            rel="stylesheet" media="screen" />--%>
         
        
    </div>
        </asp:View>
        
    </asp:MultiView>
    <div style="width: 100%;">
        <div class="modal fade" id="divModal" tabindex="-1" role="dialog">
            <div class="modal-dialog-centered modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Outward Tracker Cancellation</h5>
                        <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>--%>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <table width="100%" style="background-color: #DDDDDD;" cellpadding="2" cellspacing="1">
                                <tr>
                                    <td class="tabhead" width="25%">Cancellation Remarks: <span class="label">*</span></td>
                                    <td class="tabbody" width="75%">
                                        <F2FControls:F2FTextBox ID="txtDeactRemarks" ToolTip="Deactivation Remarks"
                                            CssClass="textbox1" Width="300px" runat="server" Columns="64" Rows="5"
                                            TextMode="MultiLine">
                                        </F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtDeactRemarks" />
                                        <asp:RequiredFieldValidator ID="rfvDeactivationRemarks" runat="server" ControlToValidate="txtDeactRemarks"
                                            CssClass="span" ValidationGroup="Cancellation" Display="Dynamic" SetFocusOnError="True"
                                            ErrorMessage="Please enter Deactivation Remarks."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer" style="flex-direction: column;">
                        <center>
                            <input type="button" id="btnCancellation" runat="server" value="Cancel Outward" class="btn btn-primary"
                                validationgroup="Cancellation" onserverclick="btnCancellation_ServerClick" />
                            <asp:Button ID="btnbk" runat="server" CssClass="btn btn-outline-danger" Text="Back" OnClick="btnbk_Click"/>
                            <%--<input type="button" id="Button2" runat="server" value="Back" class="btn btn-secondary" data-dismiss="modal" />--%>
                        </center>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div style="width: 100%;">
        <div class="modal fade" id="divModal_Revision" tabindex="-1" role="dialog">
            <div class="modal-dialog-centered modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Outward Tracker Revision</h5>
                        <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>--%>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <table width="100%" style="background-color: #DDDDDD;" cellpadding="2" cellspacing="1">
                                <tr>
                                    <td class="tabhead" width="25%">Revision Remarks: <span class="label">*</span></td>
                                    <td class="tabbody" width="75%">
                                        <F2FControls:F2FTextBox ID="txtrevisionRemark" ToolTip="Revision Remarks"
                                            CssClass="textbox1" Width="300px" runat="server" Columns="64" Rows="5"
                                            TextMode="MultiLine">
                                        </F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtrevisionRemark" />
                                        <asp:RequiredFieldValidator ID="rfvrevisionRemark" runat="server" ControlToValidate="txtrevisionRemark"
                                            CssClass="span" ValidationGroup="Revision" Display="Dynamic" SetFocusOnError="True"
                                            ErrorMessage="Please enter revision remarks."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer" style="flex-direction: column;">
                        <center>
                            <input type="button" id="btnrevision" runat="server" value="Revision Suggested" class="btn btn-primary"
                                validationgroup="Revision" onserverclick="btnrevision_ServerClick" />
                            <asp:Button ID="btnclosed" runat="server" CssClass="btn btn-outline-danger" Text="Back" OnClick="btnclosed_Click"/>
                            <%--<input type="button" id="Button2" runat="server" value="Back" class="btn btn-secondary" data-dismiss="modal" />--%>
                        </center>
                    </div>
                </div>
            </div>
        </div>
    </div>

    
    <div style="width: 100%;">
        <div class="modal fade" id="divModal_Deletion" tabindex="-1" role="dialog">
            <div class="modal-dialog-centered modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Outward Tracker Deletion</h5>
                        <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>--%>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <table width="100%" style="background-color: #DDDDDD;" cellpadding="2" cellspacing="1">
                                <tr>
                                    <td class="tabhead" width="25%">Deletion Remarks: <span class="label">*</span></td>
                                    <td class="tabbody" width="75%">
                                        <F2FControls:F2FTextBox ID="txtdeletionRemark" ToolTip="Deletion Remarks"
                                            CssClass="textbox1" Width="300px" runat="server" Columns="64" Rows="5"
                                            TextMode="MultiLine">
                                        </F2FControls:F2FTextBox>
                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtdeletionRemark" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtdeletionRemark"
                                            CssClass="span" ValidationGroup="Deletion" Display="Dynamic" SetFocusOnError="True"
                                            ErrorMessage="Please enter deletion remarks."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer" style="flex-direction: column;">
                        <center>
                            <input type="button" id="btndelete" runat="server" value="Delete Outward" class="btn btn-primary"
                                validationgroup="Deletion" onserverclick="btndelete_ServerClick" />
                            <asp:Button ID="Button2" runat="server" CssClass="btn btn-outline-danger" Text="Back" OnClick="btnclosed_Click"/>
                            <%--<input type="button" id="Button2" runat="server" value="Back" class="btn btn-secondary" data-dismiss="modal" />--%>
                        </center>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
