<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="AddDataRequirementQuery.aspx.cs" Inherits="Fiction2Fact.Projects.Compliance.AddDataRequirementQuery" %>

<!DOCTYPE html>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Raise Query</title>
    <asp:PlaceHolder runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />
        <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/jquery.min.js") %>"></script>
        <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/popup/jquery.simplemodal.js")%>"></script>
        <%--<script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/popup/basic.js")%>"></script>--%>

        <!-- jQuery  -->
        <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/bootstrap.bundle.min.js") %>"></script>
        <!-- jQuery  -->

        <style>
            #acePersonResponsible_completionListElem li {
                list-style: none;
                height: 25px;
            }
        </style>
        <script>

            function onCloseClick() {
                var Source = document.getElementById('<%= hfSource.ClientID %>').value;
                window.close();
                if (Source == 'CR')
                    window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnRefresh').click();
                else
                    window.opener.location.reload(false);
            }

            function onPersonResponsibleChange() {
                var txtPersonResponsibleObj = document.getElementById('txtPersonResponsible');
                document.getElementById('hfFlag').value = 'true';
                if (txtPersonResponsibleObj.value == '') {
                    document.getElementById('hfResponsiblePersonId').value = '';
                }
            }

            function PersonResponsibleClientItemSelected(sender, e) {
                $get("<%= hfResponsiblePersonId.ClientID %>").value = e.get_value();
            }

            function addAttachmentRow() {
                window.open("../UploadFileDesciption.aspx?type=CRDRQ", "FILE",
                    "location=0,status=0,scrollbars=0,resizable=1,width=650,height=410");
                return false;
            }

            function onClientEditClick() {
                document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "";
                document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
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
                        deleteFromHTML("tblAttachment", strUniqueRowId);
                        if (deleteFromDB == 'Y') {
                            window.open("../DeleteDBRecords.aspx?calledFrom=CRDRQ&Ids=" + strAttachId,
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
                    var table = document.getElementById("tblAttachment");
                    var hiddenFormData = document.getElementById("<%= hfAttachment.ClientID %>");
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
                    cellElement.href = "../CommonDownloadAnyFile.aspx?type=DRQM&downloadFileName=" + escape(serverfilename) + "&fileName=" + escape(clientfilename);
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

            function onClientSaveClick() {
                var validated = Page_ClientValidate('Save');
                var IsDoubleClickFlagSet = document.getElementById('hfDoubleClickFlag').value;
                if (validated) {
                    if (IsDoubleClickFlagSet == 'Yes') {
                        alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                        return false;
                    }
                    else {
                        var message = "";
                        var edit_selection = document.getElementById('hfSelectedOperation').value
                        if (edit_selection.toString() === '') {
                            message = "Are you sure want to raise a query?";
                        }
                        else {
                            message = "Are you sure want to update query?";
                        }
                        if (confirm(message)) {
                            document.getElementById('<%=hfDoubleClickFlag.ClientID%>').value = "Yes";
                            return getAttachmentData();
                        }
                        else {
                            return false;
                        }
                    }
                }
                else {
                    return false;
                }
            }


            function onAddRESClick(Id, Source) {
                window.open('AddResponse.aspx?DRId=' + Id + '&Type=Add&Src=' + Source + '&Source=REQ&User=RM', '', 'location=0,status=0,scrollbars=1,resizable=1,width=900,height=900');
                return false;
            }

            function openModal(DRId) {
                document.getElementById("hfId").value = DRId;
                return false;
            }

            function doubleClickPreventionModal() {
                var hfClickCounter = document.getElementById('hfClickCounter');
                if (Page_ClientValidate("Comments")) {
                    if (confirm('Are you sure want to close this raised query?')) {
                        hfClickCounter.value = hfClickCounter.value + 1;
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

            function setCommentChange() {
                var Clousre = document.getElementById('txtModalClosure').value;

                document.getElementById('hfModalClosure').value = Clousre;

                if (Clousre != "") {
                    return true;
                }
            }

            function onViewDRClick(Id) {
                window.open('ViewDataRequirement.aspx?DRId=' + Id + '&Type=ViewSent&Src=RR', '', 'location=0,status=0,scrollbars=1,resizable=1,width=900,height=900');
                return false;
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
    </asp:PlaceHolder>
</head>
<body class="d-block">
    <form id="form1" runat="server">
        <asp:Literal ID="CheckBoxIDsArray" runat="server"></asp:Literal>
        <asp:HiddenField ID="hfId" runat="server" />
        <asp:HiddenField ID="hfUnitId" runat="server" />
        <asp:HiddenField ID="hfRefId" runat="server" />
        <asp:HiddenField ID="hfSource" runat="server" />
        <asp:HiddenField ID="hfSelectedOperation" runat="server" />
        <asp:HiddenField ID="hfAttachment" runat="server" />
        <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />
        <asp:HiddenField ID="hfFlag" runat="server" />
        <asp:HiddenField ID="hfResPersonEdit" runat="server" />
        <asp:HiddenField runat="server" ID="hfClickCounter" />
        <asp:HiddenField runat="server" ID="hfModalClosure" />
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="page-content">
            <div class="container-fluid">
                <!-- Page-Title -->
                <div class="row">
                    <div class="col-sm-12">
                        <div class="page-title-box">
                            <div class="row">
                                <div class="col">
                                    <h4 class="page-title">
                                        <asp:Label ID="lblHeader" Text="Raise Query" runat="server"></asp:Label></h4>
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
                            <div class="card-body">
                                <div class="mb-3">
                                    <asp:LinkButton CssClass="btn btn-outline-secondary" runat="server" ID="btnRefresh"
                                        Style="visibility: hidden; display: none;" OnClick="btnRefresh_Click">
                                        <i data-feather="refresh-cw"></i> Refresh
                                    </asp:LinkButton>
                                </div>
                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label">Type :<span class="text-danger">*</span></label>
                                        <asp:DropDownList runat="server" ID="ddlQueryType" CssClass="form-select" DataTextField="RC_NAME"
                                            DataValueField="RC_CODE">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlQueryType" CssClass="text-danger"
                                            Display="Dynamic" ValidationGroup="Save" SetFocusOnError="true" ErrorMessage="Please select Query Type."></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label">Responsible Unit :<span class="text-danger">*</span></label>
                                        <asp:DropDownList runat="server" ID="ddlUnit" CssClass="form-select" DataTextField="CSFM_NAME"
                                            DataValueField="CSFM_ID">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvUnit" runat="server" ControlToValidate="ddlUnit" CssClass="text-danger"
                                            Display="Dynamic" ValidationGroup="Save" SetFocusOnError="true" ErrorMessage="Please select Responsible Unit."></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-12 mb-3">
                                        <label class="form-label">Data Requirement / Query :<span class="text-danger">*</span></label>
                                        <asp:TextBox CssClass="form-control" ID="txtQuery" runat="server" TextMode="MultiLine"
                                            Rows="5" Columns="48"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvQuery" ValidationGroup="Save" ControlToValidate="txtQuery"
                                            ErrorMessage="Please enter Query / Data Requirement" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtQuery" ID="rev" ValidationGroup="Save" ValidationExpression="^[\s\S]{0,4000}$" runat="server" ForeColor="Red" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label">Person Responsible :<span class="text-danger">*</span></label>
                                        <asp:TextBox CssClass="form-control" ID="txtPersonResponsible" MaxLength="100" runat="server"
                                            onchange="onPersonResponsibleChange();"></asp:TextBox>
                                        <asp:HiddenField ID="hfResponsiblePersonId" runat="server" />
                                        <ajaxToolkit:AutoCompleteExtender ServiceMethod="getUserDetailsbyPhoneBook" MinimumPrefixLength="2" CompletionInterval="100"
                                            ServicePath="~/Projects/AJAXDropdown.asmx" EnableCaching="false" CompletionSetCount="1" runat="server"
                                            TargetControlID="txtPersonResponsible" ID="acePersonResponsible" FirstRowSelected="True" CompletionListItemCssClass="cssListItem"
                                            CompletionListHighlightedItemCssClass="cssListItemHighlight"
                                            ShowOnlyCurrentWordInCompletionListItem="true" OnClientItemSelected="PersonResponsibleClientItemSelected">
                                        </ajaxToolkit:AutoCompleteExtender>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvPersonResponsible" ValidationGroup="Save" ControlToValidate="txtPersonResponsible"
                                            ErrorMessage="Please enter Person Reponsible" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>

                                    </div>
                                </div>
                                <div class="mt-3">
                                    <div class="card mb-1 mt-1 border">
                                        <div class="card-header py-0 custom-ch-bg-color">
                                            <h6 class="font-weight-bold text-white mtb-5">Attachments : </h6>
                                        </div>
                                        <div class="card-body mt-1">
                                            <div class="mb-2">
                                                <asp:LinkButton runat="server" ID="imgBtnAddAttachment" CssClass="btn btn-sm btn-soft-success btn-circle" OnClientClick="return addAttachmentRow()"
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
                                    <asp:LinkButton CssClass="btn btn-outline-success" runat="server" ID="btnSave" OnClick="btnSave_Click"
                                        Text="Save & add more" ValidationGroup="Save" OnClientClick="return onClientSaveClick();">
                                        <i class="fa fa-save me-2"></i> Save                    
                                    </asp:LinkButton>
                                    <asp:LinkButton CssClass="btn btn-outline-danger" runat="server" ID="Button1"
                                        Text="Close" OnClientClick="return onCloseClick();">
                                         <i class="fa fa-arrow-left me-2"></i> Close                   
                                    </asp:LinkButton>
                                </div>
                                <div class="mt-3">
                                    <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcel" runat="server" Text="Export To Excel"
                                        Visible="false" OnClick="btnExportToExcel_Click">
                                        <i class="fa fa-download"></i> Export to Excel               
                                    </asp:LinkButton>
                                    <asp:LinkButton CssClass="btn btn-outline-success" ID="btnSubmitAllQueries" runat="server" Text="Submit all Queries"
                                        Visible="false" OnClick="btnSubmitAllQueries_Click" OnClientClick=" return confirm('Are you sure want to submit selected queries?');">
                                        <i class="fa fa-save me-2"></i> Submit all Queries                    
                                    </asp:LinkButton>
                                </div>
                                <div class="mt-3">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvDRQM" runat="server" AutoGenerateColumns="False" DataKeyNames="CDQ_ID"
                                            AllowSorting="false" AllowPaging="false" GridLines="Both" CellPadding="4" OnRowDataBound="gvDRQM_RowDataBound"
                                            CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                            EmptyDataText="No record found..." OnSelectedIndexChanged="gvDRQM_SelectedIndexChanged">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox runat="server" ID="HeaderLevelCheckBox" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="RowLevelCheckBox" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sr.No." ControlStyle-Width="20px" HeaderStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                        </center>
                                                        <asp:HiddenField runat="server" ID="hfDRID" Value='<%#Eval("CDQ_ID")%>' />
                                                        <asp:HiddenField runat="server" ID="hfIsMailSent" Value='<%#Eval("CDQ_IS_MAIL_SENT")%>' />
                                                        <asp:HiddenField runat="server" ID="hfQueryPendingWith" Value='<%#Eval("CDQ_PENDING_WITH")%>' />
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
                                                <asp:TemplateField HeaderText="View">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Select"
                                                                CssClass="btn btn-sm btn-soft-primary btn-circle" OnClientClick='<%# string.Format("return onViewDRClick(\"{0}\",\"{1}\");", Eval("CDQ_ID"), hfSource.Value) %>'>
                                                                <i class="fa fa-eye"></i>
                                                            </asp:LinkButton>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="View Response" Visible="false">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:LinkButton ID="lnkViewRes" runat="server" CommandName="Select"
                                                                CssClass="btn btn-sm btn-soft-primary btn-circle" OnClientClick='<%# string.Format("return onViewRESClick(\"{0}\",\"{1}\");", Eval("CDQ_ID"), hfSource.Value) %>'>
                                                                <i class="fa fa-eye"></i>
                                                            </asp:LinkButton>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("CDQ_STATUS") %>' Visible="false"></asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Add Response">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:LinkButton ID="lnkAddRes" runat="server" CommandName="Select"
                                                                CssClass="btn btn-sm btn-soft-primary btn-circle" OnClientClick='<%# string.Format("return onAddRESClick(\"{0}\",\"{1}\");", Eval("CDQ_ID"), hfSource.Value) %>'>
                                                                <i class="fa fa-plus"></i>
                                                            </asp:LinkButton>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Closure Details">
                                                    <ItemTemplate>
                                                        <div id="basic-modal">
                                                            <center>
                                                                <asp:LinkButton ID="lnkAddClosure" runat="server" CommandName="Select" data-bs-toggle="modal" data-bs-target="#basic-modal-content"
                                                                    CssClass="btn btn-sm btn-soft-danger btn-circle basic" OnClientClick='<%# string.Format("openModal(\"{0}\");", Eval("CDQ_ID")) %>'>
                                                                    <i class="fa fa-ban"></i>
                                                                </asp:LinkButton>
                                                            </center>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Type" DataField="QType" />
                                                <asp:BoundField HeaderText="Responsible Unit" DataField="CSFM_NAME" />
                                                <asp:TemplateField HeaderText="Data Requirement / Query" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDRQM" Width="200px" runat="server" ToolTip='<%# Eval("CDQ_QUERY_DATA_REQUIREMENT").ToString() %>'
                                                            Text='<%#Eval("CDQ_QUERY_DATA_REQUIREMENT").ToString().Length>200?(Eval("CDQ_QUERY_DATA_REQUIREMENT") as string).Substring(0,200).Replace(Environment.NewLine, "<br />") + " ...":Eval("CDQ_QUERY_DATA_REQUIREMENT").ToString().Replace(Environment.NewLine, "<br />")  %>'></asp:Label>
                                                        <asp:Label ID="lblDRQM1" Visible="false" runat="server" Text='<%#Eval("CDQ_QUERY_DATA_REQUIREMENT").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Person Responsible" DataField="CDQ_PERSON_RESPONSIBLE" />

                                                <asp:BoundField HeaderText="Raised Date" DataField="CDQ_RAISED_DT" DataFormatString="{0: dd-MMM-yyyy HH:mm:ss}" />
                                                <asp:BoundField HeaderText="Due Date" DataField="CDQ_EXPIRY_DT" DataFormatString="{0: dd-MMM-yyyy}" />
                                                <asp:BoundField HeaderText="Status" DataField="Status" />
                                                <asp:BoundField HeaderText="Query pending with" DataField="Query pending with" />
                                                <asp:TemplateField HeaderText="Ageing" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lblAgeing" runat="server" Text='<%#Eval("Ageing") %>'></asp:Label>
                                                        </center>
                                                        <asp:HiddenField ID="hfStatus" runat="server" Value='<%#Eval("CDQ_STATUS") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Attachment(s)">
                                                    <ItemTemplate>
                                                        <asp:DataList ID="dlFiles" runat="server" RepeatColumns="1" CssClass="custom-datalist-border" RepeatDirection="vertical"
                                                            DataSource='<%# LoadDRQMFileList(Eval("CDQ_ID")) %>'>
                                                            <ItemTemplate>
                                                                <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=CRDRQ&downloadFileName=<%#getFileName(Eval("CRDF_SERVER_FILE_NAME"))%>&fileName=<%#getFileName(Eval("CRDF_CLIENT_FILE_NAME"))%>','','location=0,status=0,scrollbars=1,width=400,height=200');">
                                                                    <%#Eval("CRDF_CLIENT_FILE_NAME")%>
                                                                </a>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Added by" DataField="CDQ_CREATE_BY" ControlStyle-Width="40px" HeaderStyle-Width="40px" />
                                                <asp:BoundField HeaderText="Added on" DataField="CDQ_CREATE_DT" DataFormatString="{0: dd-MMM-yyyy HH:mm:ss}" ControlStyle-Width="80px" HeaderStyle-Width="80px" />
                                                <asp:BoundField HeaderText="Closed by" DataField="CDQ_CLOSED_BY" ControlStyle-Width="40px" HeaderStyle-Width="40px" />
                                                <asp:BoundField HeaderText="Closed on" DataField="CDQ_CLOSED_ON" DataFormatString="{0: dd-MMM-yyyy HH:mm:ss}" ControlStyle-Width="80px" HeaderStyle-Width="80px" />
                                                <asp:TemplateField HeaderText="Closure Remarks" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClosureRem" Width="200px" runat="server" ToolTip='<%# Eval("CDQ_CLOSURE_REMARKS").ToString() %>'
                                                            Text='<%#Eval("CDQ_CLOSURE_REMARKS").ToString().Length>200?(Eval("CDQ_CLOSURE_REMARKS") as string).Substring(0,200).Replace(Environment.NewLine, "<br />") + " ...":Eval("CDQ_CLOSURE_REMARKS").ToString().Replace(Environment.NewLine, "<br />")  %>'></asp:Label>
                                                        <asp:Label ID="lblClosureRem1" Visible="false" runat="server" Text='<%#Eval("CDQ_CLOSURE_REMARKS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- end row -->
            </div>
        </div>

        <div class="modal fade bd-example-modal-lg" id="basic-modal-content" tabindex="-1" aria-labelledby="myExtraLargeModalLabel" role="dialog">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title">
                            <asp:Label ID="Label1" Text="Closure Remark" runat="server"></asp:Label></h6>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:Panel runat="server" ID="pnlModalRCA">
                            <div class="row">
                                <div class="col-md-12 mb-3">
                                    <label class="form-label">Closure Remarks : <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtModalClosure" TextMode="MultiLine" Rows="4" runat="server"
                                        MaxLength="11" CssClass="form-control" onChange="return setCommentChange();"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvClosure" runat="server" ControlToValidate="txtModalClosure"
                                        Display="Dynamic" ValidationGroup="Comments" CssClass="text-danger">Please enter Closure Remarks.</asp:RequiredFieldValidator>

                                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtModalClosure" ID="rev_modal" ValidationGroup="Comments" ValidationExpression="^[\s\S]{0,4000}$" runat="server" ForeColor="Red" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>

                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="modal-footer" style="flex-direction: column;">
                        <center>
                            <input type="button" id="btnSubmit" runat="server" value="Submit" class="btn btn-outline-success" onserverclick="btnSubmit_Click"
                                validationgroup="Comments" onclick="doubleClickPreventionModal();" />
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
