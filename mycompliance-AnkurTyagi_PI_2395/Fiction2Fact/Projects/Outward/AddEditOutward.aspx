<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" Async="true"
    Inherits="Fiction2Fact.Projects.Outward.AddEditOutward" Title="Add / Edit Outward Tracker" 
    CodeBehind="AddEditOutward.aspx.cs" %>

<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="custom" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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

        <%--function onRepresentationStatusChange() {

            var ddlRepresentationStatus = document.getElementById('ctl00_ContentPlaceHolder1_ddlRepresentationStatus').value;
            //var lblRepresentationDt = document.getElementById('ctl00_ContentPlaceHolder1_lblRepresentationDt');
            var trRepresentationdate = document.getElementById('trRepresentationdate');
            if (ddlRepresentationStatus == 'C') {
                ValidatorEnable(document.getElementById('<%= rfvRepresentationDate.ClientID %>'), true);
                trRepresentationdate.style.visibility = "visible";
                trRepresentationdate.style.display = 'table-row';
            }
            else {
                ValidatorEnable(document.getElementById('<%= rfvRepresentationDate.ClientID %>'), false);
                trRepresentationdate.style.visibility = "hidden";
                trRepresentationdate.style.display = 'none';
            }
        }--%>
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
     <asp:HiddenField runat="server" ID="hfDocNo" />
    <asp:HiddenField runat="server" ID="hfstatus" />
    <asp:HiddenField runat="server" ID="hfcreator" />
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
                                <label class="form-label">Type Of Outward: <span class="text-danger">*</span></label>
                                <asp:DropDownList CssClass="form-select" ID="ddlTypeofOutward" runat="server"
                                    DataTextField="OTM_NAME" DataValueField="OTM_ID">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlTypeofOutward" runat="server" ControlToValidate="ddlTypeofOutward" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True" ErrorMessage="Please select type of outward."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Department: <span class="text-danger">*</span></label>
                                <asp:DropDownList CssClass="form-select" ID="ddlDept" runat="server"
                                    DataTextField="ODM_NAME" DataValueField="ODM_ID">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvDept" runat="server" ControlToValidate="ddlDept" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True" ErrorMessage="Please select department."></asp:RequiredFieldValidator>
                            </div>
                             <div class="col-md-4 mb-3">
                                <label class="form-label">Linked outward: </label>
                                <asp:TextBox CssClass="form-control" ID="txtexistingOutward" runat="server" MaxLength="200"
                                    Columns="50"></asp:TextBox>
                                 <%--//<< Added by ramesh more on 13-Mar-2024 CR_1991--%>
                                 <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtexistingOutward" />
                                 <%--//>>--%>
                            </div>
                          </div>
                         <div class="row">
                          <div class="col-md-4 mb-3">
                            <label class="form-label">Document Dated: <span class="text-danger">*</span></label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtOutwardDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                        <asp:ImageButton ID="ibDocDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />
                            </div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtOutwardDate" CssClass="text-danger"
                                Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                              <asp:RequiredFieldValidator ID="rfvDocumentDate" runat="server" ControlToValidate="txtOutwardDate" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True" ErrorMessage="Please select date."></asp:RequiredFieldValidator>
                             <%-- <asp:CustomValidator ID="cvOutwardDate" runat="server" ClientValidationFunction="compareOutwardDateSystemDates" CssClass="text-danger"
                                    ControlToValidate="txtOutwardDate" ErrorMessage="Document Dated should not be grater than System Date."
                                    Display="Dynamic" OnServerValidate="cvOutwardDate_ServerValidate" ValidationGroup="Save"></asp:CustomValidator>--%>

                        </div>
                            <cc1:CalendarExtender ID="ceDocumentDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibDocDate"
                                    TargetControlID="txtOutwardDate"></cc1:CalendarExtender>
                           
                             <div class="col-md-4 mb-3">
                                <label class="form-label">Subject: <span class="text-danger">*</span></label>
                                <asp:TextBox CssClass="form-control" ID="txtDocName" runat="server" MaxLength="500"
                                    Columns="50"></asp:TextBox>
                                 <%--//<< Added by ramesh more on 13-Mar-2024 CR_1991--%>
                                 <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtDocName" />
                                 <%--//>>--%>
                                <asp:RequiredFieldValidator ID="rfvDocName" runat="server" ControlToValidate="txtDocName" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True" ErrorMessage="Please enter subject."></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revDocumentName" ControlToValidate="txtDocName" CssClass="span"
                                    Display="Dynamic" Text="Exceeding 500 characters" ValidationExpression="^[\s\S]{0,500}$"
                                    runat="server" SetFocusOnError="True" ValidationGroup="Save" />
                            </div>
                             <div class="col-md-4 mb-3">
                                <label class="form-label">Regulatory Authority: <span class="text-danger">*</span></label>
                                <asp:DropDownList CssClass="form-select" ID="ddlAuthority" runat="server"
                                    DataTextField="ORAM_NAME" DataValueField="ORAM_ID">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvAuthority" runat="server" ControlToValidate="ddlAuthority" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="Save" SetFocusOnError="True" ErrorMessage="Please select regulatory authority."></asp:RequiredFieldValidator>
                            </div>
                         </div>
                        <div class="row">
                             <div class="col-md-12 mb-3">
                                    <label class="form-label">Remarks:</label>
                                    <asp:TextBox CssClass="form-control" ID="txtRemarks" runat="server" MaxLength="200"
                                        Rows="5" Columns="80" TextMode="MultiLine"></asp:TextBox>
                                  <%--//<< Added by ramesh more on 13-Mar-2024 CR_1991--%>
                                 <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                 <%--//>>--%>
                                </div>
                        </div>
                          <div class="row">
                            
                                <div class="col-md-4 mb-3" id="attacment2" runat="server" >
                                     <label class="form-label" style="margin-top:10px;">Attachments:</label>
                                    <asp:FileUpload ID="fuEditFileUpload" CssClass="form-control" runat="server" />
                                    <asp:RegularExpressionValidator ID="revFileUpload1" runat="server" ControlToValidate="fuEditFileUpload"
                                        Display="Dynamic" ErrorMessage="Upload of this file format is not supported." CssClass="span"
                                        ValidationExpression="^.+(.msg|.MSG|.mp4|.MP4|.eml|.EML|.avi|.AVI|.flv|.FLV|.jpg|.JPG|.png|.PNG|.bmp|.BMP|.xls|.XLS|.xlsx|.XLSX|.DOC|.DOCX|.docx|.doc|.pdf|.PDF|.html|.htm|.HTML.|HTM|.tif|.TIF|.ZIP|.zip|.txt|.TXT)$"
                                        ValidationGroup="SubmitGrp"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-4 mb-3" id="attacment3" runat="server">
                                    <label class="form-label" style="margin-top:10px;">&nbsp;</label>
                                    <asp:HiddenField ID="hfFileNameOnServer" runat="server" />
                                    <asp:Button CssClass="btn btn-outline-success" ID="btnAddAttachment" runat="server" Text="Attach" style="margin-top:37px;"
                                         OnClick="btnAddAttachment_Click" />
                                </div>

                          </div>
                        <div class="row">
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
                           
                          </div>
                        
                       
                        <div class="row">
                            <div class="col-md-4 mb-3">
                                <asp:ValidationSummary ID="vsOutward" runat="server" ShowMessageBox="True" ValidationGroup="Save"
                                    ShowSummary="false" />
                                <asp:Button CssClass="btn btn-outline-success" ID="btnSave" runat="server" CausesValidation="TRUE"
                                    ValidationGroup="Save" Text="Submit Request" OnClick="btnSave_Click" OnClientClick="return onClientSaveClick()" />
                                <asp:Button CssClass="btn btn-outline-danger" ID="BtnBack" runat="server" CausesValidation="false"
                                    Text="Cancel" OnClick="BtnBack_Click" />

                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlMsg" runat="server" Visible="false">
                        <br />
                        <asp:Button ID="btnbk" runat="server" CssClass="btn btn-outline-danger" Text="Back" OnClick="btnbk_Click" />
                        <asp:Button ID="btnHome" runat="server" CssClass="btn btn-outline-danger" Text="Go to search outward"
                            OnClick="btnHome_Click" />
                       
                        <br />
                        <br />
                    </asp:Panel>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
