<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="AddEditIssuesDetails.aspx.cs" Inherits="Fiction2Fact.Projects.Compliance.AddEditIssuesDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />
    <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/jquery.min.js") %>"></script>
    <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/bootstrap.bundle.min.js") %>"></script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/popup/jquery.simplemodal.js")%>"></script>
    <%-- <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/popup/basic.js")%>"></script>--%>

    <script>

        function openModal(Id) {
            document.getElementById('hfModalStatus').value = "";
            document.getElementById('hfModalStatusRem').value = "";
            document.getElementById("hfIssueId").value = Id;

        }
        function setStatusChange(Control) {
            var Status = document.getElementById(Control).value;

            document.getElementById('hfModalStatus').value = Status;

            if (Status != "") {
                return true;
            }
        }

        function check_Acceptance_Rejections(message, validation_group) {
            var validated = Page_ClientValidate(validation_group);
            if (validated) {
                if (confirm(message)) {
                    document.getElementById('<%=hfModalStatus_txt.ClientID%>').value = document.getElementById('<%=txtStatusRemark.ClientID%>').value;
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }

    </script>
    <style>
        #acePersonResponsible_completionListElem li {
            list-style: none;
            height: 25px;
        }
    </style>
    <script>
        function onCloseClick() {
            var hfType = document.getElementById('hfType').value;
            window.close();
            if (hfType == 'Add') {
                window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnRefresh').click();
            }
        }

        function onViewIssueTrackerClick(RDIId) {
            window.open('ViewIssuesTracker.aspx?Id=' + RDIId, '', 'location=0,status=0,scrollbars=1,resizable=1,width=900,height=900');
            return false;
        }

        function onPersonResponsibleChange() {
            var txtSPOCResponsibleObj = document.getElementById('txtSPOCResponsible');
            document.getElementById('hfFlag').value = 'true';
            if (txtSPOCResponsibleObj.value == '') {
                document.getElementById('hfResponsibleSPOCId').value = '';
            }
        }

        function PersonResponsibleClientItemSelected(sender, e) {
            $get("<%= hfResponsibleSPOCId.ClientID %>").value = e.get_value();
        }

        function onClientSaveClick(operationType) {
            var IsDoubleClickFlagSet = document.getElementById('hfDoubleClickFlag').value;

            var table = document.getElementById("tblAttachment");
            if (table == null) {
                return;
            }
            var rowCount = table.rows.length;

            if (operationType == 'Save') {
                var validated = Page_ClientValidate('Save');
                if (validated) {
                    if (IsDoubleClickFlagSet == 'Yes') {
                        alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                        return false;
                    }
                    else {
                        document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                        return getAttachmentData();
                    }
                }
                else {
                    return false;
                }
            }
            else {
                if (IsDoubleClickFlagSet == 'Yes') {
                    alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                    return false;
                }
                else {

                    if (operationType == 'Submit') {
                        var RiskReviewStatus = document.getElementById('hfRRStatus').value;
                        if (RiskReviewStatus == '3') {
                            alert('Please close all the queries raised before submitting the issues identified.');
                            return false;
                        }
                        else {
                            var isAnyChkboxChecked = false;
                            if (CheckBoxIDs != null) {
                                for (var i = 1; i < CheckBoxIDs.length; i++) {
                                    var cb = document.getElementById(CheckBoxIDs[i]);
                                    if (cb != null) {
                                        if (cb.checked) {
                                            isAnyChkboxChecked = true;
                                        }
                                    }
                                }

                                if (!isAnyChkboxChecked) {
                                    alert('Please select one or more records for submitting the issues.');
                                    return false;
                                }
                            }
                        }
                    }

                    if (!confirm('Are you sure that you want to submit all Issues?')) return false;
                    document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                    return getAttachmentData();
                }
            }
        }


        function onClientEditClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "";
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
        }

        function onClientCopyClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "";
            if (!confirm('Are you sure that you want to create a copy of this record?')) return false;
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Copy";
        }

        function onClientDeleteClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "";
            if (!confirm('Are you sure that you want to delete this record?')) return false;
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Delete";
        }

    </script>
    <script>
        function addAttachmentRow() {
            window.open("../UploadFileDesciption.aspx?type=ComplianceIssue", "FILE",
                "location=0,status=0,scrollbars=0,resizable=1,width=650,height=410");
            return false;
        }

        function deleteAttachmentRow() {
            try {
                var table = document.getElementById("tblAttachment");
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
                        deleteFromDB = 'Y';
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
                    debugger;
                    deleteFromHTML("tblAttachment", strUniqueRowId);
                    if (deleteFromDB == 'Y') {
                        window.open("../DeleteDBRecords.aspx?calledFrom=ComplianceIssue&Ids=" + strAttachId,
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

        function getAttachmentData() {
            try {
                debugger;
                var table = document.getElementById("tblAttachment");
                var hiddenFormData = document.getElementById("hfAttachment");
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
                    document.getElementById('hfDoubleClickFlag').value = "";
                }

                return true;
            }
            catch (e) {
                alert(e.message);
                return false;
            }
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
                cellElement.href = "../CommonDownload.aspx?type=ComplianceIssue&downloadFileName=" + escape(serverfilename) + "&fileName=" + escape(clientfilename);
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

    </script>
    <script>
            <%----checkbox selection start--%>
        function ChangeAllCheckBoxStates(checkState) {
            if (CheckBoxIDs != null) {
                for (var i = 0; i < CheckBoxIDs.length; i++)
                    ChangeCheckBoxState(CheckBoxIDs[i], checkState);
            }

            for (var i = 1; i <= CheckBoxIDs.length; i++) {
                onRowCheckedUnchecked(CheckBoxIDs[i]);
            }
        }

        function ChangeHeaderAsNeeded() {
            if (CheckBoxIDs != null) {
                for (var i = 1; i < CheckBoxIDs.length; i++) {
                    var cb = document.getElementById(CheckBoxIDs[i]);
                    if (cb != null) {
                        if (!cb.checked) {
                            ChangeCheckBoxState(CheckBoxIDs[0], false);
                            return;
                        }
                    }
                }
                ChangeCheckBoxState(CheckBoxIDs[0], true);
            }
        }

        function ChangeCheckBoxState(id, checkState) {

            var cb = document.getElementById(id);
            if (cb != null)
                cb.checked = checkState;
        }

        function onRowCheckedUnchecked(cbid) {
            var cb = document.getElementById(cbid);

            ChangeHeaderAsNeeded();
            return;
        }

             <%----checkbox selection end--%>
</script>
</head>
<body class="d-block">
    <form id="form1" runat="server">
        <div>

            <asp:HiddenField runat="server" ID="hfModalStatus_txt" />
            <asp:HiddenField runat="server" ID="hfIssueId" />
            <asp:HiddenField runat="server" ID="hfModalStatus" />
            <asp:HiddenField runat="server" ID="hfModalStatusRem" />

            <asp:HiddenField runat="server" ID="hfType" />
            <asp:HiddenField runat="server" ID="hfRefID" />
            <asp:HiddenField runat="server" ID="hfRDIId" />
            <asp:HiddenField runat="server" ID="hfRRStatus" />
            <asp:HiddenField runat="server" ID="hfSource" />
            <asp:HiddenField runat="server" ID="hfSelectedOperation" />
            <asp:HiddenField runat="server" ID="hfDoubleClickFlag" />
            <asp:HiddenField runat="server" ID="hfAttachment" />
            <asp:HiddenField runat="server" ID="hfViewType" />
            <asp:HiddenField ID="hfFlag" runat="server" />
            <asp:Literal ID="CheckBoxIDsArray" runat="server"></asp:Literal>
            <asp:HiddenField ID="hfResPersonEdit" runat="server" />
            <asp:HiddenField ID="hfInherentRiskRating" runat="server" />
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            </asp:ScriptManager>

            <div class="page-content">
                <div class="container-fluid">
                    <!-- Page-Title -->
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="page-title-box">
                                <div class="row">
                                    <div class="col">
                                        <h4 class="page-title">
                                            <asp:Label ID="lblHeader" Text="Add/Edit Issue Details" runat="server"></asp:Label></h4>
                                        <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
                                        <asp:Label ID="lblUpdId" runat="server" Visible="false" CssClass="label"></asp:Label>
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
                                <div class="card-body">
                                    <asp:MultiView ID="mvIssueTracker" runat="server">
                                        <asp:View ID="vwAddEditIssueDetails" runat="server">
                                            <asp:Panel ID="pnlUpdates" runat="server" CssClass="panel">
                                                <asp:Panel runat="server" ID="pnlRR">
                                                    <div class="row">
                                                        <div class="col-md-4 mb-3" style="visibility: hidden; display: none;">
                                                            <label class="form-label">Issue Id:</label>
                                                            <asp:Label CssClass="label" ID="lblRDIId" runat="server"></asp:Label>
                                                        </div>
                                                        <div class="col-md-6 mb-3">
                                                            <label class="form-label">Responsible Unit: <span class="text-danger">*</span></label>
                                                            <asp:DropDownList runat="server" ID="ddlUnitId" CssClass="form-select"
                                                                DataTextField="CSFM_NAME" DataValueField="CSFM_ID">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvUnitId" ValidationGroup="Save"
                                                                ControlToValidate="ddlUnitId"
                                                                ErrorMessage="Please select Responsible Unit" CssClass="text-danger">
                                                              </asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-6 mb-3">
                                                            <label class="form-label">Issue Title: <span class="text-danger">*</span></label>
                                                            <asp:TextBox CssClass="form-control" ID="txtIssueTitle" MaxLength="1000" Rows="3" runat="server" Columns="43">
                                                </asp:TextBox>
                                                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtIssueTitle" />
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvIssueTitle" ValidationGroup="Save"
                                                                ControlToValidate="txtIssueTitle" ErrorMessage="Please enter Issue Title"
                                                                CssClass="text-danger">
                                                </asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-12 mb-3">
                                                            <label class="form-label">Issue Description: <span class="text-danger">*</span></label>
                                                            <asp:TextBox CssClass="form-control" ID="txtIssueDescription" TextMode="MultiLine"
                                                                Rows="8" runat="server" Columns="80"></asp:TextBox>
                                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtIssueDescription" />
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvIssueDescription" ValidationGroup="Save"
                                                                ControlToValidate="txtIssueDescription" ErrorMessage="Please enter Issue Description"
                                                                CssClass="text-danger">
                                                </asp:RequiredFieldValidator>

                                                            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtIssueDescription" ID="rev_remarks1" ValidationGroup="Save"
                                                                ValidationExpression="^[\s\S]{0,4000}$" runat="server" ForeColor="Red" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>
                                                        </div>
                                                        <div class="col-md-4 mb-3">
                                                            <label class="form-label">Issue Type: <span class="text-danger">*</span></label>
                                                            <asp:DropDownList CssClass="form-select" ID="ddlIssueType" runat="server" DataTextField="RC_NAME"
                                                                DataValueField="RC_CODE">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvIssueType" ValidationGroup="Save"
                                                                ControlToValidate="ddlIssueType" ErrorMessage="Please select Issue Type"
                                                                CssClass="text-danger">
                                                                 </asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4 mb-3">
                                                            <label class="form-label">Issue Status: <span class="text-danger">*</span></label>
                                                            <asp:DropDownList CssClass="form-select" ID="ddlIsueStatus" runat="server" DataTextField="RC_NAME"
                                                                DataValueField="RC_CODE">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvIsueStatus" ValidationGroup="Save"
                                                                ControlToValidate="ddlIsueStatus" ErrorMessage="Please select Issue Status"
                                                                CssClass="text-danger">
                                                </asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-12 mb-3">
                                                            <label class="form-label">SPOC Responsible: <span class="text-danger">*</span></label>
                                                            <asp:TextBox CssClass="form-control" ID="txtSPOCResponsible" MaxLength="100" runat="server"
                                                                onchange="onPersonResponsibleChange();" Columns="30"></asp:TextBox>
                                                            <asp:HiddenField ID="hfResponsibleSPOCId" runat="server" />
                                                            <ajaxToolkit:AutoCompleteExtender ServiceMethod="getUserDetailsbyPhoneBook" MinimumPrefixLength="2" CompletionInterval="100"
                                                                ServicePath="~/Projects/AJAXDropdown.asmx" EnableCaching="false" CompletionSetCount="1" runat="server"
                                                                TargetControlID="txtSPOCResponsible" ID="acePersonResponsible" FirstRowSelected="True" CompletionListItemCssClass="cssListItem"
                                                                CompletionListHighlightedItemCssClass="cssListItemHighlight" ShowOnlyCurrentWordInCompletionListItem="true"
                                                                OnClientItemSelected="PersonResponsibleClientItemSelected">
                                                            </ajaxToolkit:AutoCompleteExtender>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvPersonResponsible" ValidationGroup="Save" ControlToValidate="txtSPOCResponsible"
                                                                ErrorMessage="Please enter SPOC Responsible" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>

                                                        </div>
                                                        <div class="col-md-12 mb-3">
                                                            <label class="form-label">Remarks:</label>
                                                            <asp:TextBox CssClass="form-control" ID="txtRemarks" TextMode="MultiLine" Rows="5" runat="server"
                                                                Columns="80"></asp:TextBox>
                                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                                            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtRemarks" ID="RegularExpressionValidator1" ValidationGroup="Save"
                                                                ValidationExpression="^[\s\S]{0,4000}$" runat="server" ForeColor="Red" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>

                                                        </div>
                                                    </div>
                                                    <div class="mt-1">
                                                        <div class="card mb-1 mt-1 border">
                                                            <div class="card-header py-0 custom-ch-bg-color">
                                                                <h6 class="font-weight-bold text-white mtb-5">Annexures: </h6>
                                                            </div>
                                                            <div class="card-body mt-1">
                                                                <div class="mb-3">
                                                                    <asp:LinkButton runat="server" ID="imgBtnAddAttachment" CssClass="btn btn-sm btn-soft-primary btn-circle" OnClientClick="return addAttachmentRow()"
                                                                        ToolTip="Click on add icon to add attachment(s).">
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
                                                    <div class="mt-3 text-center">
                                                        <asp:LinkButton runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text="Save & add more"
                                                            CssClass="btn btn-outline-success" OnClientClick="return onClientSaveClick('Save');" ValidationGroup="Save">
                                                               <i class="fa fa-save me-2"></i> Save       
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Cancel"
                                                            CssClass="btn btn-outline-danger">
                                                             <i class="fa fa-arrow-left me-2"></i> Close    
                                                        </asp:LinkButton>
                                                    </div>
                                                    <div class="mt-3">
                                                        <asp:LinkButton CssClass="btn btn-outline-secondary" runat="server" ID="btnSubmitForIdentification" Visible="false"
                                                            OnClick="btnSubmitForIdentification_Click" Text="Submit issues for response" OnClientClick="return onClientSaveClick('Submit');">
                                                            <i class="fa fa-download"></i> Export to Excel  
                                                        </asp:LinkButton>
                                                        <asp:LinkButton CssClass="btn btn-outline-success" ID="btnRefresh" runat="server" Style="visibility: hidden; display: none;" OnClick="btnRefresh_Click">
                                                            <i class="fa fa-save me-2"></i> Submit all Queries   
                                                        </asp:LinkButton>
                                                    </div>
                                                    <div class="mt-3">
                                                        <div class="table-responsive">
                                                            <asp:GridView ID="gvDraftedIssue" runat="server" AutoGenerateColumns="False" DataKeyNames="CI_ID" OnRowDataBound="gvDraftedIssue_RowDataBound"
                                                                AllowSorting="false" AllowPaging="false" GridLines="Both" CellPadding="4" OnSelectedIndexChanged="gvDraftedIssue_SelectedIndexChanged"
                                                                CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                                EmptyDataText="No record found...">
                                                                <Columns>
                                                                    <%--<< Added by Shwetan on 26Feb2021--%>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox runat="server" ID="HeaderLevelCheckBox" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox runat="server" ID="RowLevelCheckBox" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%---->>--%>
                                                                    <asp:TemplateField HeaderText="Sr.No." ControlStyle-Width="20px" HeaderStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                                            </center>
                                                                            <asp:HiddenField runat="server" ID="hfRDIID" Value='<%#Eval("CI_ID")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="View" Visible="true">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="Select" CommandArgument='<%# Eval("CI_ID") %>'
                                                                                    CssClass="btn btn-sm btn-soft-primary btn-circle" OnClientClick='<%# string.Format("return onViewIssueTrackerClick(\"{0}\");", Eval("CI_ID")) %>'>
                                                                                    <i class="fa fa-eye"></i>
                                                                                </asp:LinkButton>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="Response Acceptance">
                                                                        <ItemTemplate>
                                                                            <div id="basic-modal">
                                                                                <center>
                                                                                    <asp:LinkButton ID="lnkSetStatus" runat="server" CssClass="btn btn-sm btn-soft-primary btn-circle" CommandName="Select" data-bs-toggle="modal" data-bs-target="#basic-modal-content"
                                                                                         OnClientClick='<%# string.Format("openModal(\"{0}\");", Eval("CI_ID")) %>'>
                                                                                        <i class="fa fa-plus"></i>
                                                                                    </asp:LinkButton>
                                                                                </center>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Edit" ShowHeader="true">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-success btn-circle"
                                                                                    CausesValidation="false" OnClientClick="return onClientEditClick();">
                                                                                    <i class="fa fa-pen"></i>
                                                                                </asp:LinkButton>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Delete" ShowHeader="true" Visible="false">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Select"
                                                                                    CssClass="btn btn-sm btn-soft-danger btn-circle" OnClientClick="return onClientDeleteClick();">
                                                                                    <i class="fa fa-trash"></i>
                                                                                </asp:LinkButton>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Copy Issue" ShowHeader="true">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:LinkButton ID="lnkCopy" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-primary btn-circle"
                                                                                    CausesValidation="false"
                                                                                    OnClientClick="return onClientCopyClick();">
                                                                                    <i class="fa fa-copy"></i>
                                                                                </asp:LinkButton>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Id" Visible="false" DataField="CI_ID" />
                                                                    <asp:BoundField HeaderText="Responsible Unit" DataField="CSFM_NAME" />
                                                                    <asp:BoundField HeaderText="Issue Title" DataField="CI_ISSUE_TITLE" />
                                                                    <asp:TemplateField HeaderText="Issue Description" ShowHeader="true">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIssueDesc" Width="200px" runat="server" ToolTip='<%# Eval("CI_ISSUE_DESC").ToString() %>'
                                                                                Text='<%#Eval("CI_ISSUE_DESC").ToString().Length>200?(Eval("CI_ISSUE_DESC") as string).Substring(0,200).Replace(Environment.NewLine, "<br />") + " ...":Eval("CI_ISSUE_DESC").ToString().Replace(Environment.NewLine, "<br />")  %>'></asp:Label>
                                                                            <asp:Label ID="lblIssueDesc1" Visible="false" runat="server" Text='<%#Eval("CI_ISSUE_DESC").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                                                            <asp:HiddenField ID="hfStatus" runat="server" Value='<%#Eval("CI_STATUS")%>' />
                                                                            <asp:HiddenField ID="hfManagementResponse" runat="server" Value='<%#Eval("CI_MNGMT_RESPONSE") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Issue Type" DataField="IssueType" />
                                                                    <asp:BoundField HeaderText="Issue Status" DataField="IssueStatus" />

                                                                    <asp:BoundField HeaderText="Status" DataField="DraftIssuesStatus" />

                                                                    <asp:TemplateField HeaderText="Annexures(s)">
                                                                        <ItemTemplate>
                                                                            <asp:DataList ID="dlFiles" runat="server" RepeatColumns="1" CssClass="custom-datalist-border" RepeatDirection="vertical"
                                                                                DataSource='<%# LoadDraftedFileList(Eval("CI_ID")) %>'>
                                                                                <ItemTemplate>
                                                                                    <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=ComplianceIssue&downloadFileName=<%#getFileName(Eval("CIF_SERVER_FILE_NAME"))%>&Filename=<%#getFileName(Eval("CIF_CLIENT_FILE_NAME"))%>','','location=0,status=0,scrollbars=1,width=400,height=200');">
                                                                                        <%#Eval("CIF_CLIENT_FILE_NAME")%>
                                                                        </a>
                                                                                </ItemTemplate>
                                                                            </asp:DataList>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="SPOC Responsible" DataField="CI_SPOC_RESPONSIBLE" />

                                                                    <asp:BoundField HeaderText="Management Response" DataField="CI_MNGMT_RESPONSE" />
                                                                    <asp:BoundField HeaderText="Management Remarks" DataField="CI_MNGMT_REMARKS" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                    <div class="mt-3">
                                                        <asp:LinkButton CssClass="btn btn-outline-success" runat="server" ID="btnSubmitForIdentification1" Visible="false"
                                                            OnClick="btnSubmitForIdentification_Click" Text="Submit issues for response" OnClientClick="return onClientSaveClick('Submit');">
                                                            <i class="fa fa-save me-2"></i> Submit issues for response
                                                        </asp:LinkButton>
                                                    </div>
                                                </asp:Panel>
                                            </asp:Panel>
                                        </asp:View>
                                    </asp:MultiView>
                                </div>

                            </div>
                        </div>
                    </div>
                    <!-- end row -->
                </div>
            </div>
        </div>

        <div class="modal fade bd-example-modal-lg" id="basic-modal-content" tabindex="-1" aria-labelledby="myExtraLargeModalLabel" role="dialog">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title">
                            <asp:Label ID="Label2" Text="Provide Response" runat="server"></asp:Label></h6>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:Panel runat="server" ID="pnlModalRCA">
                            <div class="row">
                                <div class="col-md-12 mb-3">
                                    <label class="form-label">Remarks: <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtStatusRemark" TextMode="MultiLine" Rows="10" Columns="100" MaxLength="4000" runat="server"
                                        CssClass="form-control"></asp:TextBox>
                                    <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtStatusRemark" />
                                    <asp:RequiredFieldValidator ID="rfvStatusRem" runat="server" ControlToValidate="txtStatusRemark"
                                        Display="Dynamic" ValidationGroup="Status2" CssClass="text-danger">Please enter Status Remarks.</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtStatusRemark" ID="rev" ValidationGroup="Status2"
                                        ValidationExpression="^[\s\S]{0,4000}$" runat="server" ForeColor="Red" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="modal-footer" style="flex-direction: column;">
                        <center>
                            <asp:LinkButton ID="btnAccept" runat="server" Text="Accept Response" CssClass="btn btn-outline-success" OnClientClick="return check_Acceptance_Rejections('Are you sure want to accept response of selected issue?','Status2');" OnClick="btnAccept_ServerClick"></asp:LinkButton>
                            <asp:LinkButton ID="btnReject" runat="server" Text="Send Back" CssClass="btn btn-outline-danger" OnClientClick="return check_Acceptance_Rejections('Are you sure want to send back this selected issue?','Status2');" OnClick="btnReject_ServerClick"></asp:LinkButton>
                            <input id="btncCancel" runat="server" type="button" value="Cancel" data-bs-dismiss="modal"
                                class="modalCloseImg simplemodal-close btn btn-outline-danger" />
                        </center>
                    </div>
                </div>
            </div>
        </div>


    </form>
</body>
</html>
