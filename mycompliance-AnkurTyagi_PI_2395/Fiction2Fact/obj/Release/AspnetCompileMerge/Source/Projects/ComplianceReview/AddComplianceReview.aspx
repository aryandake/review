<%@ Page Title="" Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" CodeBehind="AddComplianceReview.aspx.cs" Inherits="Fiction2Fact.Projects.Compliance.AddComplianceReview" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js") %>"></script>
    <style>
        .label {
            color: red;
        }
    </style>
    <script>

        $(document).ready(function () {
            try {
                onLinkageWithEarlierCircularChange();
                $("[id*='ddlLinkageWithEarlierCircular']").change(() => {
                    onLinkageWithEarlierCircularChange();
                });

            } catch (e) {
                alert(e);
            }
        });

        const onLinkageWithEarlierCircularChange = () => {
            if ($("[id*='ddlLinkageWithEarlierCircular']").val() == "Y") {
                $("#trSOCEOC").css({ "visibility": "visible", "display": "table-row" });
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvSOCEOC'), true);

                $("#trOldCircSubjectNo").css({ "visibility": "visible", "display": "table-row" });
            }
            else {
                $("#trSOCEOC").css({ "visibility": "hidden", "display": "none" });
                $("[id*='ddlSOCEOC']").val("");
                ValidatorEnable(document.getElementById('ctl00_ContentPlaceHolder1_rfvSOCEOC'), false);

                $("#trOldCircSubjectNo").css({ "visibility": "hidden", "display": "none" });
                $("[id*='dlUserList'] tr:not(:first-child)").remove();
                $("[id*='hfOldCircularId']").val("");
            }
        }

        function validateOldCircularSubjectNo(source, arg) {
            var valid = false;
            var ddlLinkageWithEarlierCircular = document.getElementById('ctl00_ContentPlaceHolder1_ddlLinkageWithEarlierCircular').value;
            var table = document.getElementById("ctl00_ContentPlaceHolder1_dlUserList");

            if (ddlLinkageWithEarlierCircular == 'Y') {
                if (table != null) {
                    var rowCount = table.rows.length;

                    if (rowCount <= 0) {
                        valid = false;
                    }
                    else {
                        valid = true;
                    }
                }
                else {
                    valid = false;
                }
            }
            else {
                valid = true;
            }

            arg.IsValid = valid;
        }

        function compareStartEndDates(source, arguments) {
            try {
                var TentativeStartDt = document.getElementById('ctl00_ContentPlaceHolder1_txtTentativeStartDT');
                var TentativeEndDate = document.getElementById('ctl00_ContentPlaceHolder1_txtTentativeEndDT');

                if (compare2Dates(TentativeStartDt, TentativeEndDate) == 0) {
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


        function OldCircSubjectNoClientItemSelected(source, eventArgs) {
            var hdnValueID = "<%= hfOldCircularId.ClientID %>";
            document.getElementById(hdnValueID).value = eventArgs.get_value();
            __doPostBack(hdnValueID, "");
        }

        function addAttachmentRow() {
            window.open("../UploadFileDesciption.aspx?type=CRI", "FILE",
                "location=0,status=0,scrollbars=0,resizable=1,width=650,height=410");
            return false;
        }

        function onClientValidateSave() {

            var IsDoubleClickFlagSet = document.getElementById('ctl00_ContentPlaceHolder1_hfDoubleClickFlag').value;
            if (IsDoubleClickFlagSet == 'Yes') {
                alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                return false;
            }
            else {
                var validated = Page_ClientValidate('Save');
                if (validated) {
                    var message = "";
                    var edit_selection = document.getElementById('ctl00_ContentPlaceHolder1_hfSelectedOperation').value
                    if (edit_selection.toString() === '') {
                        message = "Are you sure want to planned compliance review?";
                    }
                    else {
                        message = "Are you sure want to update compliance review details?";
                    }
                    if (confirm(message)) {
                        document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                        return getAttachmentData();
                    }
                    else {
                        return false;
                    }
                }
                else {
                    return false;
                }
            }
        }
        function getAttachmentData() {
            try {
                var table = document.getElementById("tblAttachment");
                var hiddenFormData = document.getElementById("<%=hfAttachment.ClientID%>");
                if (table == null) {
                    return;
                }
                var rowCount = table.rows.length;
                var row;
                var attachmentData = "";
                var uniqueRowId;
                hiddenFormData.value = "";

                for (var i = 1; i < rowCount; i++) {
                    row = table.rows[i];
                    var cellElements0 = row.cells[0].getElementsByTagName("input");
                    uniqueRowId = cellElements0[0].value;
                    var AttachmentIdField = document.getElementById('attachId' + uniqueRowId);
                    var AttachmentNo = AttachmentIdField.value;
                    var attachClientFileNameField = document.getElementById('attachClientFileName' + uniqueRowId);
                    var AttachClientFileName = attachClientFileNameField.value;
                    var attachServerFileNameField = document.getElementById('attachServerFileName' + uniqueRowId);
                    var AttachServerFileName = attachServerFileNameField.value;
                    var attachFileTypeIDField = document.getElementById('attachFileTypeID' + uniqueRowId);
                    var AttachFileTypeID = attachFileTypeIDField.value;
                    var AttachFileType = '';
                    var attachFileDescField = document.getElementById('attachFileDesc' + uniqueRowId);
                    var AttachFileDesc = attachFileDescField.value;

                    attachmentData = attachmentData + AttachmentNo + "|" + AttachClientFileName + "|" + AttachServerFileName + "|" +
                        AttachFileTypeID + "|" + AttachFileType + "|" + AttachFileDesc + "~";
                }

                if (attachmentData != "") {
                    hiddenFormData.value = attachmentData;
                    //document.getElementById('ctl00_ContentPlaceHolder1_hfDoubleClickFlag').value = "";
                }

                return true;
            }
            catch (e) {
                alert(e.message);
                return false;
            }
        }

        function validBusinessUnits(source, arg) {
            var atLeast = 1;
            var valid = false;
            var CHK = document.getElementById("<%=cblBusinessUnits.ClientID%>");
            if (CHK != null) {
                var checkbox = CHK.getElementsByTagName("input");
                var counter = 0;
                for (var i = 0; i < checkbox.length; i++) {
                    if (checkbox[i].checked) {
                        counter++;
                    }
                }
                if (atLeast <= counter) {
                    valid = true;
                } else { valid = false; }
            }
            else {
                valid = false;
            }
            arg.IsValid = valid;
        }

        function onAppAttachUploaded(serverfilename, FileTypeID, clientfilename, FileType, FileDesc) {
            try {
                var table = document.getElementById('tblAttachment');
                var rowCount = table.rows.length;
                var idx = rowCount - 1;
                var uniqueId = 0;
                var cellElements;

                //check if there are no rows in the table.
                if (idx != 0) {

                    cellElements = table.rows[idx].cells[0]
                        .getElementsByTagName("input");
                    uniqueId = cellElements[0].value;
                    uniqueId = parseInt(uniqueId) + 1;
                } else {
                    //alert("no records in table");
                }

                var row = table.insertRow(rowCount);
                var cell0 = row.insertCell(0);

                var cellElement = document.createElement("input");
                cellElement.type = "hidden";
                cellElement.id = "uniqueRowId" + uniqueId;
                cellElement.value = uniqueId;
                cell0.appendChild(cellElement);

                cellElement = document.createElement("input");
                cellElement.type = "hidden";
                cellElement.id = "attachId" + uniqueId;
                cellElement.value = 0;
                cell0.appendChild(cellElement);

                cellElement = document.createElement("input");
                cellElement.type = "checkbox";
                cellElement.id = "checkAttachment" + uniqueId;
                cellElement.value = '0';
                cell0.className = "contentBody";
                cell0.appendChild(cellElement);

                cellElement = document.createElement("input");
                cellElement.type = "hidden";
                cellElement.id = "attachClientFileName" + uniqueId;
                cellElement.value = clientfilename;
                cell0.appendChild(cellElement);

                cellElement = document.createElement("input");
                cellElement.type = "hidden";
                cellElement.id = "attachServerFileName" + uniqueId;
                cellElement.value = serverfilename;
                cell0.appendChild(cellElement);

                cellElement = document.createElement("input");
                cellElement.type = "hidden";
                cellElement.id = "attachFileTypeID" + uniqueId;
                cellElement.value = FileTypeID;
                cell0.appendChild(cellElement);

                cellElement = document.createElement("input");
                cellElement.type = "hidden";
                cellElement.id = "attachFileDesc" + uniqueId;
                cellElement.value = FileDesc;
                cell0.appendChild(cellElement);

                var cell1 = row.insertCell(1);
                cell1.innerHTML = FileType;
                cell1.className = "contentBody";

                var cell2 = row.insertCell(2);
                cell2.innerHTML = FileDesc;
                cell2.className = "contentBody";

                var cell3 = row.insertCell(3);

                cellElement = document.createElement("a");
                cellElement.id = "attachfilelink" + uniqueId;
                cellElement.innerHTML = clientfilename;
                cellElement.href = "../CommonDownload.aspx?type=CRI&downloadFileName=" + escape(serverfilename) + "&fileName=" + escape(clientfilename);
                //>>
                cell3.className = "contentBody";
                cell3.appendChild(cellElement);

            } catch (e) {
                alert(e.message);
            }
        }


        function onAttachmentHeaderRowChecked() {
            var table = document.getElementById("tblAttachment");
            var allCheck = document.getElementById("HeaderLevelCheckBoxAttachment");
            var rowCount = table.rows.length;
            var uniqueRowId;
            var check;
            for (var i = 1; i < rowCount; i++) {
                var row = table.rows[i];
                cellElements = table.rows[i].cells[0].getElementsByTagName("input");
                uniqueRowId = cellElements[0].value;
                check = document.getElementById("checkAttachment" + uniqueRowId);
                check.checked = allCheck.checked;
            }
        }


        function deleteAttachmentRow() {
            try {
                var table = document.getElementById("tblAttachment");
                var ccrid = document.getElementById("<%=hfCCR_ID.ClientID%>");
                var rowCount = table.rows.length;
                var cellElements, cellUniqueRow, uniqueRowId;
                var strUniqueRowId = "", strId = "", strAttachId = "";
                var rowIdFrmTbl;
                var AttachId = "";
                var deleteFromDB = 'N';

                for (var i = 1; i < rowCount; i++) {
                    cellElements = table.rows[i].cells[0].getElementsByTagName("input");
                    rowIdchecked = cellElements[2].checked;
                    if (rowIdchecked == true) {
                        cellUniqueRow = table.rows[i].cells[0].getElementsByTagName("input");
                        uniqueRowId = cellUniqueRow[0].value;
                        AttachId = document.getElementById("attachId" + uniqueRowId).value;
                        if (ccrid != '') {
                            deleteFromDB = 'Y';
                        }
                        strAttachId = strAttachId + AttachId + ",";
                        strUniqueRowId = strUniqueRowId + uniqueRowId + ",";
                    }
                }

                if (strUniqueRowId == '') {
                    alert('Please select one or more records for deletion.');
                    return false;
                }

                if (!confirm('Are you sure that you want to delete these records?')) {
                    return false;
                }
                else {
                    deleteFromHTML("tblAttachment", strUniqueRowId);
                    if (deleteFromDB == 'Y') {
                        window.open("../DeleteDBRecords.aspx?calledFrom=CFIFiles&Ids=" + strAttachId,
                            "FILE", "location=0,status=0,scrollbars=0,width=10,height=10");
                    }
                    return false;
                }
            }
            catch (e) {
                alert(e);
                return false;
            }
        }

        function deleteFromHTML(strTable, strUniqueRowId) {
            var table = document.getElementById(strTable);
            var rowCount = table.rows.length;
            var cellElements;
            var uniqueRowId;
            var uniqueRowIds = strUniqueRowId.split(",");

            for (var cnt1 = 0; cnt1 < uniqueRowIds.length - 1; cnt1++) {
                uniqueRowId = uniqueRowIds[cnt1];
                for (var cnt2 = 1; cnt2 < rowCount; cnt2++) {
                    var row = table.rows[cnt2];
                    cellElements = table.rows[cnt2].cells[0].getElementsByTagName("input");
                    rowIdFrmTbl = cellElements[0].value;
                    if (rowIdFrmTbl == uniqueRowId) {
                        table.deleteRow(cnt2);
                        break;
                    }
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfSelectedRecord" runat="server" />
    <asp:HiddenField runat="server" ID="hfDoubleClickFlag" />
    <asp:HiddenField runat="server" ID="hfAttachment" />
    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField runat="server" ID="hfCCR_ID" />
    <asp:HiddenField runat="server" ID="hfSource" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">
                            <asp:Label ID="lblHeader" Text="Compliance Review Initiation" runat="server"></asp:Label></h4>
                        <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
                    </div>
                    <!--end col-->
                </div>
                <!--end row-->
            </div>
            <!--end page-title-box-->
        </div>
        <!--end col-->
    </div>
    <!--end row-->
    <!-- end page title end breadcrumb -->

    <div class="row">
        <div class="col-12">
            <div class="card">
                <asp:Panel runat="server" ID="pnlRR">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-4 mb-3 d-none">
                                <label class="form-label">Compliance Review Id:</label>
                                <asp:Label CssClass="label" ID="lblCCIRd" runat="server" Visible="false"></asp:Label>
                                <asp:Label CssClass="label" ID="lblCCRRefNo" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Review Universe: <span class="text-danger">*</span></label>
                                <asp:DropDownList CssClass="form-select" ID="ddlActRegulationCircular" runat="server" DataValueField="CRUM_ID" DataTextField="CRUM_UNIVERSE_TO_BE_REVIEWED">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvUniverseReviwed" runat="server" ControlToValidate="ddlActRegulationCircular" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="Save" SetFocusOnError="true" ErrorMessage="Please select review universe."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Reviewer Name: <span class="text-danger">*</span></label>
                                <asp:DropDownList runat="server" ID="ddlReviewerName" CssClass="form-select" DataValueField="CRM_ID" DataTextField="CRM_L0_REVIEWER_NAME">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="rfvReviewerName" ValidationGroup="Save" ControlToValidate="ddlReviewerName" ErrorMessage="Please select Reviewer Name" CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Review Type: <span class="text-danger">*</span></label>
                                <asp:DropDownList CssClass="form-select" ID="ddlReviewType" runat="server" DataTextField="RC_NAME"
                                    DataValueField="RC_CODE">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="rfvReviewType" ValidationGroup="Save" ControlToValidate="ddlReviewType"
                                    ErrorMessage="Please select Review Type" CssClass="text-danger">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Tentative Start Date:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtTentativeStartDT" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                                    <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl="~/Content/images/legacy/calendar.jpg" CssClass="custom-calendar-icon"
                                        ID="imgTentativeStartDT" OnClientClick="return false" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgTentativeStartDT"
                                    TargetControlID="txtTentativeStartDT" Format="dd-MMM-yyyy" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" CssClass="text-danger" runat="server" ControlToValidate="txtTentativeStartDT"
                                    ErrorMessage="Date Format has to be dd-MMM-yyyy."
                                    ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                    ValidationGroup="Save" Display="Dynamic">
                        </asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Tentative End Date:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtTentativeEndDT" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                                    <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl="~/Content/images/legacy/calendar.jpg" CssClass="custom-calendar-icon"
                                        ID="imgTentativeEndDT" OnClientClick="return false" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgTentativeEndDT"
                                    TargetControlID="txtTentativeEndDT" Format="dd-MMM-yyyy" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" CssClass="text-danger" runat="server" ControlToValidate="txtTentativeEndDT"
                                    ErrorMessage="Date Format has to be dd-MMM-yyyy."
                                    ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                    ValidationGroup="Save" Display="Dynamic"></asp:RegularExpressionValidator>
                                <asp:CustomValidator ID="cvEndDate" runat="server" ClientValidationFunction="compareStartEndDates" CssClass="text-danger"
                                    ControlToValidate="txtTentativeEndDT" ErrorMessage="Tentative Start Date be less than or equal to To Tentative End Date."
                                    Display="Dynamic" OnServerValidate="cvEndDate_ServerValidate" ValidationGroup="Save">Tentative Start Date be less than or equal to To Tentative End Date.
                        </asp:CustomValidator>
                            </div>
                            <div class="col-md-4 mb-3" style="visibility: hidden; display: none;">
                                <label class="form-label">Status: <span class="text-danger">*</span></label>
                                <asp:DropDownList CssClass="form-select" ID="ddlStatus" runat="server" DataTextField="SM_DESC"
                                    DataValueField="SM_NAME">
                                </asp:DropDownList>
                                <%--<asp:RequiredFieldValidator runat="server" ID="rfvStatus" ValidationGroup="Save" ControlToValidate="ddlStatus"
                                        ErrorMessage="Please select Status" CssClass="text-danger">
                                    </asp:RequiredFieldValidator>--%>
                            </div>
                            <div class="col-md-12 mb-3">
                                <label class="form-label">Review Scope:</label>
                                <asp:TextBox CssClass="form-control" ID="txtReviewScope" TextMode="MultiLine" Rows="7" runat="server"
                                    Columns="100"></asp:TextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtReviewScope" />
                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtReviewScope" ID="rev" ValidationGroup="Save" ValidationExpression="^[\s\S]{0,4000}$" runat="server" ForeColor="Red" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Linkage with earlier circular:</label>
                                <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlLinkageWithEarlierCircular"
                                    runat="server" ToolTip="Linkage with earlier circular">
                                </f2f:DropdownListNoValidation>
                                <asp:RequiredFieldValidator ID="rfvLinkageWithEarlierCircular" runat="server" ControlToValidate="ddlLinkageWithEarlierCircular"
                                    CssClass="text-danger" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please select Linkage with earlier circular."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 mb-3" id="trSOCEOC" style="visibility: hidden; display: none;">
                                <label class="form-label">Supersedes or Extends/Amends Old Circular(s): <span class="text-danger">*</span></label>
                                <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlSOCEOC" DataValueField="RC_CODE" DataTextField="RC_NAME"
                                    runat="server" ToolTip="Supersedes or Extends/Amends Old Circular(s)">
                                </f2f:DropdownListNoValidation>
                                <asp:RequiredFieldValidator ID="rfvSOCEOC" runat="server" ControlToValidate="ddlSOCEOC"
                                    CssClass="text-danger" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="True"
                                    ErrorMessage="Please select Supersedes or Extends/Amends Old Circular(s)."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-12 mb-3" id="trOldCircSubjectNo" style="visibility: hidden; display: none;">
                                <label class="form-label">Old Circular Subject/No.:</label>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <asp:DataList ID="dlUserList" runat="server" CellPadding="2" Font-Size="Small" CssClass="custom-datalist"
                                                Font-Underline="False" HorizontalAlign="Left" RepeatDirection="Horizontal" Width="100%">
                                                <FooterStyle Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle Font-Bold="True" ForeColor="White" />
                                                <ItemStyle Font-Size="Small" CssClass="items" />
                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" VerticalAlign="Top" Wrap="true" />
                                                <ItemTemplate>
                                                     <div class="input-group mb-2">
                                                        <span class="form-control custom-span-input"><%# Eval("Name") %></span>
                                                        <asp:LinkButton ToolTip="Remove" ID="ImageButton1" runat="server" CssClass="btn btn-sm btn-soft-danger btn-circle d-flex align-self-stretch align-items-center"
                                                            OnClick="ImageButton1_Click">
                                                                 <i class="fa fa-trash"></i> 
                                                        </asp:LinkButton>
                                                    </div> 
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                        <F2FControls:F2FTextBox ID="txtOldCircSubjectNo" CssClass="form-control"  runat="server"
                                            MaxLength="4000" ToolTip="Old Circular Subject/No.">
                            </F2FControls:F2FTextBox>
                                        <ajaxToolkit:AutoCompleteExtender ServiceMethod="getCircularDetailsFromSubjectAndCircularNo" MinimumPrefixLength="2"
                                            ServicePath="~/Projects/Circulars/AJAXDropdownCirculars.asmx" EnableCaching="false" runat="server"
                                            TargetControlID="txtOldCircSubjectNo" ID="aceOldCircSubjectNo" FirstRowSelected="true"
                                            CompletionListItemCssClass="cssListItem" CompletionListHighlightedItemCssClass="cssListItemHighlight"
                                            OnClientItemSelected="OldCircSubjectNoClientItemSelected">
                                        </ajaxToolkit:AutoCompleteExtender>
                                        <asp:HiddenField ID="hfOldCircularId" runat="server" OnValueChanged="hfOldCircularId_ValueChanged" />
                                        <%--<asp:CustomValidator ID="cvOldCircSubjectNo" ValidationGroup="SaveCircularDetails" CssClass="text-danger"
                                ClientValidationFunction="validateOldCircularSubjectNo" Display="Dynamic"
                                ErrorMessage="Please enter Old Circular Subject/No." runat="server"></asp:CustomValidator>--%>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="mt-1">
                            <div class="card mb-1 mt-1 border">
                                <div class="card-header py-0 custom-ch-bg-color">
                                    <h6 class="font-weight-bold text-white mtb-5">Review Scope Document(s): </h6>
                                </div>
                                <div class="card-body mt-1">
                                    <div class="mb-3">
                                        <asp:LinkButton runat="server" ID="imgBtnAddAttachment" CssClass="btn btn-sm btn-soft-primary btn-circle" OnClientClick="return addAttachmentRow()"
                                            ToolTip="Click on add icon to add attachment(s)." >
                                            <i class="fa fa-plus"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="imgBtnDeleteAttachment" CssClass="btn btn-sm btn-soft-danger btn-circle" OnClientClick="return deleteAttachmentRow()"
                                            ToolTip="Click on delete icon to delete attachment(s).">
                                            <i class="fa fa-trash"></i>
                                        </asp:LinkButton>
                                    </div>
                                    <div class="table-responsive">
                                        <asp:Literal ID="litAttachment" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 mb-3">
                                <label class="form-label">Business Unit(s): <span class="text-danger">*</span></label>
                                <div class="custom-checkbox-table">
                                    <asp:Panel ID="pnlBusinessUnits" runat="server">
                                        <asp:CheckBoxList ID="cblBusinessUnits" runat="server" DataTextField="CSFM_NAME" DataValueField="CSFM_ID"
                                            RepeatColumns="5" RepeatDirection="Horizontal" CssClass="form-control">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                </div>
                                <asp:CustomValidator ID="cvBusinessUnits" ValidationGroup="Save" CssClass="text-danger" ClientValidationFunction="validBusinessUnits" Display="Dynamic" Enabled="true"
                                    ErrorMessage="Please Select Business Unit(s)" runat="server"></asp:CustomValidator>
                            </div>
                            <div class="col-md-12 mb-3">
                                <label class="form-label">Remarks:</label>
                                <asp:TextBox CssClass="form-control" ID="txtRemarks" TextMode="MultiLine" Rows="3" runat="server"
                                    Columns="100"></asp:TextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtRemarks" ID="RegularExpressionValidator3" ValidationGroup="Save" ValidationExpression="^[\s\S]{0,4000}$" runat="server" ForeColor="Red" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="text-center mt-3">
                            <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-outline-success" OnClientClick="return onClientValidateSave();" OnClick="btnSubmit_Click">
                                <i class="fa fa-save me-2"></i> Save                    
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-outline-danger" OnClientClick="return confirm('are you sure want to cancel this?');" OnClick="btnCancel_Click" >
                                <i class="fa fa-arrow-left me-2"></i> Cancel                    
                            </asp:LinkButton>
                        </div>
                    </div>
                </asp:Panel>

            </div>
        </div>
    </div>
    <!-- end row -->




</asp:Content>
