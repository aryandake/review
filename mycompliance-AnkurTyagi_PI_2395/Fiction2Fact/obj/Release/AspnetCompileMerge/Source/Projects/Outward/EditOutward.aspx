<%@ Page Title="" Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" 
    CodeBehind="EditOutward.aspx.cs" Async="true"
    Inherits="Fiction2Fact.Projects.Outward.EditOutward" %>

<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="custom" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js")%>"></script>

    <script type="text/javascript">
        function compareOutwardDateSystemDates(source, arguments) {
            try {
                var OutwardDate = document.getElementById('ctl00_ContentPlaceHolder1_txtOutwardDate');
                var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurDate');
                if (compare2Dates(OutwardDate, SystemDate) == 0) {
                    arguments.IsValid = false;
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

        function compareEndSystemDates(source, arguments) {
            try {
                var Fromdate = document.getElementById('ctl00_ContentPlaceHolder1_txtCircularDate');
                var SystemDate = document.getElementById('ctl00_ContentPlaceHolder1_hfCurDate');

                if (compare2Dates(Fromdate, SystemDate) == 0) {
                    arguments.IsValid = false;
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
        function ValidateDate(source, arguments) {
            try {
                //alert(source.id)
                var control = "";
                // alert(source.id);
                if (source.id == "ctl00_ContentPlaceHolder1_cvEmailsentDate") {
                    control = "txtEmailsentDate";
                }

                var txtDate = document.getElementById("ctl00_ContentPlaceHolder1_" + control);
                if (txtDate == null)
                    txtDate = document.getElementById(control);

                var hfDateField = document.getElementById("ctl00_ContentPlaceHolder1_hfCurDate");

                if (hfDateField == null)
                    hfDateField = document.getElementById("hfCurDate");


                if (compare2Dates(hfDateField, txtDate) > 1) {
                    //source.innerHTML = "Email sent date should be grater than System Date.";
                    arguments.IsValid = false;
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
        function ValidateDate_RepClDate(source, arguments) {
            try {
                //alert(source.id)
                var control = "";
                // alert(source.id);
                if (source.id == "ctl00_ContentPlaceHolder1_cvClosureDate") {
                    control = "txtRepresentationDate";
                }

                var txtDate = document.getElementById("ctl00_ContentPlaceHolder1_" + control);
                if (txtDate == null)
                    txtDate = document.getElementById(control);

                var hfDateField = document.getElementById("ctl00_ContentPlaceHolder1_hfCurDate");

                if (hfDateField == null)
                    hfDateField = document.getElementById("hfCurDate");


                if (compare2Dates(hfDateField, txtDate) > 1) {
                    //source.innerHTML = "Email sent date should be grater than System Date.";
                    arguments.IsValid = false;
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

        function ValidateDate_CuriorDate(source, arguments) {
            try {
                //alert(source.id)
                var control = "";
                // alert(source.id);
                if (source.id == "ctl00_ContentPlaceHolder1_cvCourier") {
                    control = "txtDispatchDate";
                }

                var txtDate = document.getElementById("ctl00_ContentPlaceHolder1_" + control);
                if (txtDate == null)
                    txtDate = document.getElementById(control);

                var hfDateField = document.getElementById("ctl00_ContentPlaceHolder1_hfCurDate");

                if (hfDateField == null)
                    hfDateField = document.getElementById("hfCurDate");


                if (compare2Dates(hfDateField, txtDate) > 1) {
                    //source.innerHTML = "Email sent date should be grater than System Date.";
                    arguments.IsValid = false;
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
        function onClientSaveClick() {
            var validated = Page_ClientValidate('Save');
            if (validated) {
                var fu = document.getElementById('ctl00_ContentPlaceHolder1_fuEditFileUpload').value;
                if (fu != '') {
                    alert('Please upload or clear the file selected.');
                    return false;
                }
                else {
                    var IsDoubleClickFlagSet = document.getElementById('ctl00_ContentPlaceHolder1_hfDoubleClickFlag').value;
                    if (IsDoubleClickFlagSet == 'Yes') {
                        alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                        return false;
                    }
                    else {
                        if (!confirm('Are you sure that you want to submit these request?')) {
                            return false;
                        }
                        else {
                            document.getElementById('ctl00_ContentPlaceHolder1_hfDoubleClickFlag').value = "Yes";
                            return true;
                        }

                    }
                }
            }
        }
    </script>

    <asp:HiddenField runat="server" ID="hfOTId" />
    <asp:HiddenField runat="server" ID="hfCurDate" />
    <asp:HiddenField runat="server" ID="hfDoubleClickFlag" />
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Outward Tracker</h4>
                        <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                    </div>
                    <!--end col-->
                </div>
                <!--end row-->
            </div>
            <!--end page-title-box-->
        </div>
        <!--end col-->
    </div>
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-body">
                    <asp:Panel ID="pnlOutward" runat="server" Visible="true">
                        <div class="row">
                            <div class="col-md-4 mb-3" id="outwardNo" runat="server" visible="false">
                                <label class="form-label">Outward No: </label>
                                <asp:TextBox CssClass="form-control" ID="txtDocNumber" runat="server" MaxLength="200"
                                    Enabled="false" Columns="50"></asp:TextBox>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Type of Outward: </label>
                                <asp:DropDownList CssClass="form-select" ID="ddlTypeofOutward" runat="server" Enabled="false"
                                    DataTextField="OTM_NAME" DataValueField="OTM_ID">
                                </asp:DropDownList>
                                
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Department: </label>
                                <asp:DropDownList CssClass="form-select" ID="ddlDept" runat="server" Enabled="false"
                                    DataTextField="ODM_NAME" DataValueField="ODM_ID">
                                </asp:DropDownList>
                              
                            </div>
                         </div>
                         <div class="row">
                            <div class="col-md-4 mb-3">
                            <label class="form-label">Linked outward: </label>
                            <asp:TextBox CssClass="form-control" ID="txtexistingOutward" Enabled="false" runat="server" MaxLength="200"
                                Columns="50"></asp:TextBox>
                                 <%--//<< Added by ramesh more on 13-Mar-2024 CR_1991--%>
                                <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtexistingOutward" />
                                <%--//>>--%>
                        </div>
                          <div class="col-md-4 mb-3">
                            <label class="form-label">Document Dated:</label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtOutwardDate" Enabled="false" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                       <%-- <asp:ImageButton ID="ibDocDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />
                            
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtOutwardDate" CssClass="text-danger"
                                Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                             --%>
                            </div>
                        </div>
                            <cc1:CalendarExtender ID="ceDocumentDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibDocDate"
                                    TargetControlID="txtOutwardDate"></cc1:CalendarExtender>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Subject: </label>
                                <asp:TextBox CssClass="form-control" ID="txtDocName" runat="server" MaxLength="500" Enabled="false"
                                    Columns="50"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revDocumentName" ControlToValidate="txtDocName" CssClass="span"
                                    Display="Dynamic" Text="Exceeding 500 characters" ValidationExpression="^[\s\S]{0,500}$"
                                    runat="server" SetFocusOnError="True" ValidationGroup="Save" />
                            </div>
                             
                        </div>
                        <div class="row">
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Regulatory Authority: </label>
                                <asp:DropDownList CssClass="form-select" ID="ddlAuthority" runat="server" Enabled="false"
                                    DataTextField="ORAM_NAME" DataValueField="ORAM_ID">
                                </asp:DropDownList>
                               
                            </div>
                             <div class="col-md-4 mb-3">
                                <label class="form-label">From (Sender): <span class="text-danger">*</span></label>
                                <asp:DropDownList CssClass="form-select" ID="ddlAddressor" runat="server"
                                    DataTextField="OAM_NAME" DataValueField="OAM_ID">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvAddressor" runat="server" ControlToValidate="ddlAddressor" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True"
                                    ErrorMessage="Please select from (sender)."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 mb-3" >
                                <label class="form-label">To (Receiver):</label>
                                <asp:TextBox CssClass="form-control" ID="txtAddressee" runat="server" MaxLength="500"
                                    Columns="50"></asp:TextBox>
                                <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtAddressee" />
                                <asp:RegularExpressionValidator ID="revAddressee" ControlToValidate="txtAddressee" CssClass="span"
                                    Display="Dynamic" Text="Exceeding 500 characters" ValidationExpression="^[\s\S]{0,500}$"
                                    runat="server" SetFocusOnError="True" ValidationGroup="Save" />
                            </div>

                        </div>
                         <div class="row">
                             <div class="col-md-4 mb-3">
                                <label class="form-label">To be sent via:</label>
                                <asp:DropDownList CssClass="form-select" ID="ddlTobeSend" runat="server"
                                    DataTextField="RC_NAME" DataValueField="RC_CODE" AutoPostBack="True" OnSelectedIndexChanged="ddlTobeSend_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                              <div class="col-md-4 mb-3" id="CourierRef" runat="server" visible="false">
                                <label class="form-label">Courier Ref No:</label>
                                <asp:TextBox CssClass="form-control" ID="txtAWDPODNumber" runat="server" MaxLength="200"
                                    Columns="80"></asp:TextBox>
                                  <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtAWDPODNumber" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtAWDPODNumber" Display="Dynamic"
                                    Text="Exceeding 200 characters" ValidationExpression="^[\s\S]{0,200}$" runat="server" CssClass="span"
                                    SetFocusOnError="True" ValidationGroup="Save" />
                            </div>
                             <div class="col-md-4 mb-3" id="CourierName" runat="server" visible="false">
                                <label class="form-label">Courier Name:</label>
                                <asp:TextBox CssClass="form-control" ID="txtCourier" runat="server" MaxLength="100"
                                    Columns="80"></asp:TextBox>
                                 <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtCourier" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtCourier" Display="Dynamic"
                                    Text="Exceeding 200 characters" ValidationExpression="^[\s\S]{0,100}$" runat="server" CssClass="span"
                                    SetFocusOnError="True" ValidationGroup="Save" />
                            </div>
                         
                              <div class="col-md-4 mb-3" id="CourierSendDate" runat="server" visible="false">
                                 <label class="form-label">Courier Sent date:</label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtDispatchDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                        <asp:ImageButton ID="ImageButton2" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />
                            </div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtDispatchDate" CssClass="text-danger"
                                    ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                    ValidationGroup="Save" Display="Dynamic"></asp:RegularExpressionValidator>
                                
                                 <asp:CustomValidator ID="cvCourier" runat="server" ClientValidationFunction="ValidateDate_CuriorDate" CssClass="text-danger"
                                    ControlToValidate="txtDispatchDate" ErrorMessage="Courier sent date should be grater than System Date."
                                    Display="Dynamic" ValidationGroup="Save"></asp:CustomValidator>
                                
                            </div>
                             <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ImageButton2"
                                    TargetControlID="txtDispatchDate"></cc1:CalendarExtender>
                           
                             <div class="col-md-4 mb-3" id="EmailSendDate" runat="server" visible="false">
                                <label class="form-label">Email sent date: <span class="text-danger">*</span></label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtEmailsentDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                        <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />
                            </div>
                             <asp:RequiredFieldValidator ID="rfvEmailsentDate" Width="275px" Visible="false" runat="server" ControlToValidate="txtEmailsentDate" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True"
                                    ErrorMessage="Please enter email sent date."></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator Width="275px" ID="revEmailsentDate" runat="server" ControlToValidate="txtEmailsentDate"
                                    ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" CssClass="text-danger"
                                    ValidationGroup="Save" Display="Dynamic"></asp:RegularExpressionValidator>
                                <asp:CustomValidator ID="cvEmailsentDate" runat="server" ClientValidationFunction="ValidateDate" CssClass="text-danger"
                                    ControlToValidate="txtEmailsentDate" ErrorMessage="Email sent date should be grater than System Date."
                                    Display="Dynamic" ValidationGroup="Save"></asp:CustomValidator>
                                
                            </div>
                             <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ImageButton1"
                                    TargetControlID="txtEmailsentDate"></cc1:CalendarExtender>
                           
                              <div class="col-md-4 mb-3">
                                <label class="form-label">Status of Representation: <span id="sr" runat="server" visible="false" class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlRepresentationStatus" CssClass="form-select" runat="server" DataTextField="RC_NAME"
                                    DataValueField="RC_CODE" AppendDataBoundItems="True" ToolTip="Status of Representations" AutoPostBack="True" OnSelectedIndexChanged="ddlRepresentationStatus_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvRepresentationStatus" Visible="false" runat="server" ControlToValidate="ddlRepresentationStatus" CssClass="text-danger"
                                    ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please select status of representations"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 mb-3" runat="server" id="Representationdate" visible="false">
                                <label class="form-label">Closure Date of Representation: <span class="text-danger">*</span></label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtRepresentationDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                        <asp:ImageButton ID="ImageButton3" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />
                            </div>
                           <asp:RequiredFieldValidator ID="rfvRepresentationDate" Visible="false" Width="275px" runat="server"
                                    ControlToValidate="txtRepresentationDate" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True"
                                    ErrorMessage="Please enter representation (date of closure)."></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator Width="275px" ID="revRepresentationDate" runat="server" ControlToValidate="txtRepresentationDate"
                                    ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" CssClass="text-danger"
                                    ValidationGroup="Save" Display="Dynamic"></asp:RegularExpressionValidator>
                                  <asp:CustomValidator ID="cvClosureDate" runat="server" ClientValidationFunction="ValidateDate_RepClDate" CssClass="text-danger"
                                    ControlToValidate="txtRepresentationDate" ErrorMessage="Closure Date should be grater than System Date."
                                    Display="Dynamic" ValidationGroup="Save"></asp:CustomValidator>
                            </div>

                        </div>
                           
                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ImageButton3"
                                    TargetControlID="txtRepresentationDate"></cc1:CalendarExtender>
                           
                           
                            <asp:Panel ID="pnlDocAttached" runat="server">

                                <div class="col-md-12">
                                    Documents attached:<br />
                                    <br />
                                    <asp:GridView ID="gvFileUpload" runat="server" BorderStyle="None" BorderWidth="1px"
                                        OnSelectedIndexChanged="gvFileUpload_SelectedIndexChanged" CellPadding="4" GridLines="None"
                                        AutoGenerateColumns="False" DataKeyNames="OF_ID" CssClass="table table-bordered footable" PagerStyle-CssClass="pgr"
                                        AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServerFileName" Text='<%#Eval("OF_FILE_NAME") %>' runat="server"> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File Name">
                                                <ItemTemplate>
                                                    <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownloadAnyFile.aspx?type=Outward&downloadFileName=<%# Eval("OF_FILE_NAME_ON_SERVER")%>&fileName=<%#Eval("OF_FILE_NAME")%>','','location=0,status=0,scrollbars=0,width=10,height=10');">
                                                        <%#Eval("OF_FILE_NAME")%>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Uploaded By">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUploaderName" runat="server" Text='<%# Eval("OF_CREATE_BY") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Uploaded On">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCreateDate" runat="server" Text='<%# Eval("OF_CREATE_DATE", "{0:dd-MMM-yyyy HH:mm:ss}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                          <%--  <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" Text='<%#Eval("OF_ID") %>' Visible="false" runat="server"> </asp:Label>
                                                    <asp:LinkButton ID="lbDelete" runat="server" CommandName="Select"
                                                        Text='<%#"<img alt=\"Delete\" src=\""+ Fiction2Fact.Global.site_url("Content/images/legacy/delete_1.png")+"\" border=\"0\" />" %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>
                                </div>

                            </asp:Panel>
                            
                            <div class="row">
                                <div class="col-md-4 mb-3" >
                                    <label class="form-label" style="margin-top:10px;">Attachments:</label>
                                    <asp:FileUpload ID="fuEditFileUpload" CssClass="form-control" runat="server" />
                                    <asp:RegularExpressionValidator ID="revFileUpload1" runat="server" ControlToValidate="fuEditFileUpload"
                                        Display="Dynamic" ErrorMessage="Upload of this file format is not supported." CssClass="span"
                                        ValidationExpression="^.+(.msg|.MSG|.mp4|.MP4|.eml|.EML|.avi|.AVI|.flv|.FLV|.jpg|.JPG|.png|.PNG|.bmp|.BMP|.xls|.XLS|.xlsx|.XLSX|.DOC|.DOCX|.docx|.doc|.pdf|.PDF|.html|.htm|.HTML.|HTM|.tif|.TIF|.ZIP|.zip|.txt|.TXT)$"
                                        ValidationGroup="SubmitGrp"></asp:RegularExpressionValidator>
                                   
                                </div>
                                 <div class="col-md-4 mb-3" >
                                        <asp:HiddenField ID="hfFileNameOnServer" runat="server" />
                                            <asp:Button CssClass="btn btn-outline-success" style="margin-top:37px;" ID="btnAddAttachment" runat="server" Text="Attach"
                                                 OnClick="btnAddAttachment_Click" />
                                  </div>
                           </div>
                            <asp:Panel ID="pnlFiles" runat="server">

                                <div class="col-md-12">
                                    Files:<br />
                                    <br />
                                    <asp:GridView ID="gvInsertFileUpload" runat="server" AllowPaging="false" AllowSorting="false"
                                        BorderStyle="None" BorderWidth="1px"  DataKeyNames="FileNameOnServer"
                                        OnSelectedIndexChanged="gvInsertFileUpload_SelectedIndexChanged" OnRowDataBound="gvInsertFileUpload_RowDataBound"
                                        CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbDelete" runat="server" CausesValidation="False" CommandName="Select"
                                                        Text='<%#"<img alt=\"Delete\" src=\""+ Fiction2Fact.Global.site_url("Content/images/legacy/delete_1.png")+"\" border=\"0\" />" %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File Name">
                                                <ItemTemplate>
                                                    <a href="Javascript:void(0);" onclick="javascript:window.open('../CommonDownloadAnyFile.aspx?type=Outward&downloadFileName=<%# Eval("FileNameOnServer")%>&fileName=<%#Eval("FileName")%>','','location=0,status=0,scrollbars=0,width=10,height=10');">
                                                        <%#Eval("FileName")%>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>

                            </asp:Panel>
                        <div class="row">
                             <div class="col-md-4 mb-3">
                                    <label class="form-label">Function Remarks:</label>
                                    <asp:TextBox CssClass="form-control" ID="txtuserRemark" ReadOnly="true" runat="server" MaxLength="200"
                                        Rows="5" Columns="80" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Closure Remarks:</label>
                                    <asp:TextBox CssClass="form-control" ID="txtRemarks" runat="server" MaxLength="200"
                                        Rows="5" Columns="80" TextMode="MultiLine"></asp:TextBox>
                                    <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                </div>
                                <div class="col-md-4 mb-3" id="cremark" runat="server" visible="false">
                                    <label class="form-label">Cancellation Remarks: <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control" ID="txtCancelRemarks" runat="server" MaxLength="200"
                                        Visible="true" Rows="5" Columns="80" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvRemarks" Visible="false" runat="server" ControlToValidate="txtCancelRemarks" CssClass="text-danger"
                                        Display="Dynamic" ValidationGroup="Cancel" SetFocusOnError="True" ErrorMessage="Please enter remarks."></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revRemarks" ControlToValidate="txtCancelRemarks" Display="Dynamic"
                                        Text="Exceeding 200 characters" ValidationExpression="^[\s\S]{0,200}$" runat="server" CssClass="span"
                                        SetFocusOnError="True" ValidationGroup="Save" />
                                </div>
                              </div>

                        
                        <div class="row">
                            <div class="col-md-4 mb-3">
                                <asp:ValidationSummary ID="vsOutward" runat="server" ShowMessageBox="True" ValidationGroup="Save"
                                    ShowSummary="false" />
                                <asp:Button CssClass="btn btn-outline-success" ID="btnSave" runat="server" CausesValidation="TRUE"
                                    ValidationGroup="Save" Text="Close Request" OnClick="btnSave_Click" OnClientClick="return onClientSaveClick()" />
                                <asp:Button CssClass="btn btn-outline-danger" ID="BtnBack" runat="server" CausesValidation="false"
                                    Text="Cancel" OnClick="BtnBack_Click" />

                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlMsg" runat="server" Visible="false">
                         <br />
                        <asp:Button ID="btnbk" runat="server" CssClass="btn btn-outline-danger" Text="Back" OnClick="btnbk_Click" />
                        <asp:Button ID="btnHome" runat="server" CssClass="btn btn-outline-danger" Text="Go to HomePage"
                            OnClick="btnHome_Click" />
                        <br />
                        <br />
                    </asp:Panel>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
