<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="AddResponse.aspx.cs" Inherits="Fiction2Fact.Projects.Compliance.AddResponse" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Response</title>
    <asp:PlaceHolder runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />

        <script type="text/javascript">
            if (window.opener == null || window.opener.location == null) {
                window.location = '<%= Fiction2Fact.Global.site_url() %>' + 'Login.aspx';
            }
        </script>

        <script type="text/javascript">
            function onCloseClick() {
                window.close();
                var hfUser = document.getElementById('hfUser').value;
                if (hfUser != 'RM') {
                    window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnSearch').click();
                }
            }

            window.onbeforeunload = function () {
                onCloseClick();
            }

            function onClientSaveClick() {
                var validated = Page_ClientValidate('Save');
                var IsDoubleClickFlagSet = document.getElementById('hfDoubleClickFlag').value;
                var UpdateTypeObj = document.getElementById('ddlUpdateType');
                var UpdateType = UpdateTypeObj.options[UpdateTypeObj.selectedIndex].value;

                if (validated) {
                    if (IsDoubleClickFlagSet == 'Yes') {
                        alert("You have double clicked on the same button. Please wait till the operation is successfully completed.");
                        return false;
                    }
                    else {
                        var message = "Are you sure want to save this response to query?";
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
        </script>

        <script type="text/javascript">
            function addAttachmentRow() {
                window.open("../UploadFileDesciption.aspx?type=CRDRQ", "FILE",
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
                    cellElement.href = "../CommonDownload.aspx?type=CRDRQ&downloadFileName=" + escape(serverfilename) + "&fileName=" + escape(clientfilename);
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

    </asp:PlaceHolder>
</head>
<body class="d-block">
    <form id="form1" runat="server">

        <asp:HiddenField ID="hfId" runat="server" />
        <asp:HiddenField ID="hfDRId" runat="server" />
        <asp:HiddenField ID="hfType" runat="server" />
        <asp:HiddenField ID="hfSource" runat="server" />
        <asp:HiddenField ID="hfAttachment" runat="server" />
        <asp:HiddenField ID="hfDoubleClickFlag" runat="server" />
        <asp:HiddenField ID="hfUserType" runat="server" />
        <asp:HiddenField ID="hfUser" runat="server" />
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
                                        <asp:Label ID="lblHeader" runat="server"></asp:Label></h4>
                                    <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
                                    <asp:Label ID="lblMsg1" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
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
                            <asp:MultiView ID="mvDRQMResponse" runat="server">
                                <asp:View runat="server" ID="vwAddResponse">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label">Data Requirement / Query :<span class="text-danger">*</span></label>
                                                <asp:Label CssClass="form-control custom-span-input" ID="lblQuery" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label">Update Type :<span class="text-danger">*</span></label>
                                                <asp:DropDownList runat="server" ID="ddlUpdateType" CssClass="form-select" DataTextField="RC_NAME"
                                                    DataValueField="RC_CODE" >
                                                </asp:DropDownList><br />
                                                <asp:RequiredFieldValidator runat="server" ID="rfvUpdateType" ValidationGroup="Save" ControlToValidate="ddlUpdateType"
                                                    ErrorMessage="Please select Update Type" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-12 mb-3">
                                                <label class="form-label">Response :<span class="text-danger">*</span></label>
                                                <asp:TextBox CssClass="form-control" ID="txtResponse" runat="server" TextMode="MultiLine"
                                                    Rows="6"></asp:TextBox><br />
                                                <asp:RequiredFieldValidator runat="server" ID="rfvResponse" ValidationGroup="Save" ControlToValidate="txtResponse"
                                                    ErrorMessage="Please enter Response" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtResponse" ID="rev" ValidationGroup="Save"
                                                    ValidationExpression="^[\s\S]{0,4000}$" runat="server" ForeColor="Red" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class="mt-2">
                                            <div class="card mb-1 mt-1 border">
                                                <div class="card-header py-0 custom-ch-bg-color">
                                                    <h6 class="font-weight-bold text-white mtb-5">Attachments : </h6>
                                                </div>
                                                <div class="card-body mt-1">
                                                    <div class="mb-2">
                                                        <asp:LinkButton runat="server" ID="imgBtnAddAttachment" CssClass="btn btn-sm btn-soft-primary btn-circle" OnClientClick="return addAttachmentRow()" ToolTip="Click on add icon to add attachment(s).">
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
                                                Text="Submit" ValidationGroup="Save" OnClientClick="return onClientSaveClick();">
                                                <i class="fa fa-save me-2"></i> Submit                    
                                            </asp:LinkButton>
                                            <asp:LinkButton CssClass="btn btn-outline-danger" runat="server" ID="Button1"
                                                Text="Close" OnClientClick="return onCloseClick();">
                                                <i class="fa fa-arrow-left me-2"></i> Close                    
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="card-body">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvEditResponse" runat="server" AutoGenerateColumns="False" DataKeyNames="CRDU_ID"
                                                AllowSorting="false" AllowPaging="false" GridLines="Both" CellPadding="4"
                                                CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                OnSelectedIndexChanged="gvEditResponse_SelectedIndexChanged" OnRowDataBound="gvEditResponse_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr.No." ControlStyle-Width="20px" HeaderStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Edit" ShowHeader="true" Visible="false">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-success btn-circle"
                                                                    CausesValidation="false">
                                                                    <i class="fa fa-pen"></i>
                                                                </asp:LinkButton>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Update Type" DataField="UpdateType" ControlStyle-Width="40px" HeaderStyle-Width="40px" />
                                                    <asp:TemplateField HeaderText="Response" ShowHeader="true">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblResponse" runat="server" Text='<%#Eval("CRDU_REMARKS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                                            <asp:HiddenField ID="hfStatus" runat="server" Value='<% #Eval("CRDU_STATUS") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Attachment(s)" ControlStyle-Width="100px" HeaderStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:DataList ID="dlFiles" runat="server" RepeatColumns="1" RepeatDirection="vertical"
                                                                DataSource='<%# LoadDRQMFileList(Eval("CRDU_ID")) %>'>
                                                                <ItemTemplate>
                                                                    <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=CRDRQ&downloadFileName=<%#getFileName(Eval("CRDF_SERVER_FILE_NAME"))%>&fileName=<%#getFileName(Eval("CRDF_CLIENT_FILE_NAME"))%>','','location=0,status=0,scrollbars=1,width=400,height=200');">
                                                                        <%#Eval("CRDF_CLIENT_FILE_NAME")%>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Status" DataField="CRDU_STATUS" ControlStyle-Width="40px" HeaderStyle-Width="40px" />
                                                    <asp:BoundField HeaderText="Response From" DataField="CRDU_RESPONDED_BY" ControlStyle-Width="40px" HeaderStyle-Width="40px" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </asp:View>
                                <asp:View runat="server" ID="vwViewResponse">
                                    <div class="card-body">
                                        <div class="mb-3">
                                            <asp:LinkButton CssClass="btn btn-outline-danger" runat="server" ID="Button2"
                                                Text="Close" OnClientClick="return onCloseClick();">
                                                <i class="fa fa-arrow-left me-2"></i> Close                    
                                            </asp:LinkButton>
                                        </div>
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvResponse" runat="server" AutoGenerateColumns="False" DataKeyNames="CRDU_ID"
                                                AllowSorting="false" AllowPaging="false" GridLines="Both" CellPadding="4"
                                                CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                EmptyDataText="No record found...">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr.No." ControlStyle-Width="20px" HeaderStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Update Type" DataField="UpdateType" ControlStyle-Width="40px" HeaderStyle-Width="40px" />
                                                    <asp:TemplateField HeaderText="Response" ShowHeader="true">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblResponse" runat="server" Text='<%#Eval("CRDU_REMARKS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Attachment(s)" ControlStyle-Width="100px" HeaderStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:DataList ID="dlFiles" runat="server" RepeatColumns="1" RepeatDirection="vertical"
                                                                DataSource='<%# LoadDRQMFileList(Eval("CRDU_ID")) %>'>
                                                                <ItemTemplate>
                                                                    <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=CRDRQ&downloadFileName=<%#getFileName(Eval("CRDF_SERVER_FILE_NAME"))%>&fileName=<%#getFileName(Eval("CRDF_CLIENT_FILE_NAME"))%>','','location=0,status=0,scrollbars=1,width=400,height=200');">
                                                                        <%#Eval("CRDF_CLIENT_FILE_NAME")%>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Status" DataField="CRDU_STATUS" ControlStyle-Width="40px" HeaderStyle-Width="40px" />
                                                    <asp:BoundField HeaderText="Response From" DataField="CRDU_RESPONDED_BY" ControlStyle-Width="40px" HeaderStyle-Width="40px" />
                                                    <asp:BoundField HeaderText="Added by" DataField="CRDU_CREATE_BY" ControlStyle-Width="40px" HeaderStyle-Width="40px" />
                                                    <asp:BoundField HeaderText="Added on" DataField="CRDU_CREATE_DT" DataFormatString="{0: dd-MMM-yyyy HH:mm:ss}" ControlStyle-Width="80px" HeaderStyle-Width="80px" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div class="mt-3">
                                            <asp:LinkButton CssClass="btn btn-outline-danger" runat="server" ID="Button3"
                                                Text="Close" OnClientClick="return onCloseClick();">
                                                <i class="fa fa-arrow-left me-2"></i> Close
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </asp:View>
                            </asp:MultiView>
                        </div>
                    </div>
                </div>
                <!-- end row -->
            </div>
        </div>
    </form>
</body>
</html>
