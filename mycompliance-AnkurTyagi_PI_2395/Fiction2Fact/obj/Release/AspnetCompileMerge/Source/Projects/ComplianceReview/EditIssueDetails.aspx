<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditIssueDetails.aspx.cs" Inherits="Fiction2Fact.Projects.ComplianceReview.EditIssueDetails" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <asp:PlaceHolder runat="server">
        <title></title>
        <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/controlStyle.css") %>" />
        <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/main.css") %>" />
        <link type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/css/legacy/Popup/basic.css") %>" rel="stylesheet" media="screen" />
        <link href="<%=Fiction2Fact.Global.site_url("Content/css/legacy/tabcontrol.css") %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/tabcontrol.js")%>"></script>
        <style>
            #acePersonResponsible_completionListElem li {
                list-style: none;
                height: 25px;
            }
        </style>

        <script>

            function onEditActionPlanClick(RDIId, IssueId) {
                window.open('EditActionPlan.aspx?ActionPlanId=' + RDIId + '&IssueId=' + IssueId, '', 'location=0,status=0,scrollbars=1,resizable=1,width=900,height=900');
                return false;
            }
        </script>
        <script>
            function onCloseClick() {
                window.close();
                window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnRefresh').click();
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
            function onClientEditClick() {
                document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
            }


            function onStatusChanges() {

                var StatusObj = document.getElementById('ddlActionPlanStatus');

                if (StatusObj != null) {
                    var Status = StatusObj.options[StatusObj.selectedIndex].value;

                    if (Status == 'O') {
                        document.getElementById('txtTargetDate').disabled = false;
                        document.getElementById('imgTargetDate').disabled = false;
                        document.getElementById('txtTargetDate').style.backgroundColor = '';
                        ValidatorEnable(document.getElementById('rfvTargetDate'), true)

                        document.getElementById('txtClosureDate').disabled = true;
                        document.getElementById('imgClosureDate').disabled = true;
                        document.getElementById('txtClosureDate').value = '';
                        document.getElementById('txtClosureDate').style.backgroundColor = '#f8f8f8';
                        ValidatorEnable(document.getElementById('rfvClosureDate'), false);
                    }
                    else if (Status == 'C') {
                        document.getElementById('txtTargetDate').disabled = true;
                        document.getElementById('imgTargetDate').disabled = true;
                        document.getElementById('txtTargetDate').value = '';
                        document.getElementById('txtTargetDate').style.backgroundColor = '#f8f8f8';
                        ValidatorEnable(document.getElementById('rfvTargetDate'), false);

                        document.getElementById('txtClosureDate').disabled = false;
                        document.getElementById('imgClosureDate').disabled = false;
                        document.getElementById('txtClosureDate').style.backgroundColor = '';
                        ValidatorEnable(document.getElementById('rfvClosureDate'), true);
                    }
                    else {
                        document.getElementById('txtTargetDate').disabled = false;
                        document.getElementById('txtClosureDate').disabled = false;
                        document.getElementById('imgTargetDate').disabled = false;
                        document.getElementById('imgClosureDate').disabled = false;

                        document.getElementById('txtTargetDate').value = '';

                        document.getElementById('txtTargetDate').style.backgroundColor = '';
                        document.getElementById('txtClosureDate').style.backgroundColor = '';

                        ValidatorEnable(document.getElementById('rfvTargetDate'), true);
                        ValidatorEnable(document.getElementById('rfvClosureDate'), true);
                    }
                }
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
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">


        <div>

            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" Style="display: none;" OnClick="btnRefresh_Click" />

            <asp:HiddenField runat="server" ID="hfIssueId" />
            <asp:HiddenField runat="server" ID="hfModalStatus" />
            <asp:HiddenField runat="server" ID="hfModalStatusRem" />
            <asp:HiddenField runat="server" ID="hfActionId" />


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

            <center>
                <div class="ContentHeader1">
                    <asp:Label ID="lblHeader" Text="Edit Issue Details" runat="server"></asp:Label>
                </div>
            </center>
            <br />

            <div style="min-height: 420px">

                <div class="tabber">
                    <div class="tabbertab">
                        <h2>Edit Issue Details</h2>
                        <br />
                        <asp:Panel ID="pnlUpdates" runat="server" CssClass="panel">
                            <center>
                                <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblUpdId" runat="server" Visible="false" CssClass="label"></asp:Label>
                            </center>
                            <asp:Panel runat="server" ID="pnlRR">
                                <table width="100%" style="background-color: #DDDDDD;" cellpadding="2" cellspacing="1">
                                    <tr style="visibility: hidden; display: none;">
                                        <td class="tabhead" width="25%">Issue Id:</td>
                                        <td class="tabbody" width="75%">
                                            <asp:Label CssClass="label" ID="lblRDIId" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tabhead">Responsible Unit: <span class="label">*</span>
                                        </td>
                                        <td class="tabbody pl-1">
                                            <asp:DropDownList runat="server" ID="ddlUnitId" CssClass="dropdownlist1 w-25"
                                                DataTextField="CSFM_NAME" DataValueField="CSFM_ID" Width="325px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvUnitId" ValidationGroup="Save"
                                                ControlToValidate="ddlUnitId"
                                                ErrorMessage="Please select Responsible Unit" CssClass="span">
                                                </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tabhead">Issue Title: <span class="label">*</span></td>
                                        <td class="tabbody">
                                            <asp:TextBox CssClass="form-control" ID="txtIssueTitle" MaxLength="1000" Rows="3" runat="server" Columns="43">
                                                </asp:TextBox>
                                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtIssueTitle" />
                                            <asp:RequiredFieldValidator runat="server" ID="rfvIssueTitle" ValidationGroup="Save"
                                                ControlToValidate="txtIssueTitle" ErrorMessage="Please enter Issue Title"
                                                CssClass="span">
                                                </asp:RequiredFieldValidator>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tabhead">Issue Description: <span class="label">*</span></td>
                                        <td class="tabbody">
                                            <asp:TextBox CssClass="form-control" ID="txtIssueDescription" TextMode="MultiLine"
                                                Rows="15" runat="server" Columns="80"></asp:TextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtIssueDescription" />
                                            <asp:RequiredFieldValidator runat="server" ID="rfvIssueDescription" ValidationGroup="Save"
                                                ControlToValidate="txtIssueDescription" ErrorMessage="Please enter Issue Description"
                                                CssClass="span">
                                                </asp:RequiredFieldValidator>

                                            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtIssueDescription" ID="RegularExpressionValidator1" ValidationGroup="Save"
                                                ValidationExpression="^[\s\S]{0,4000}$" runat="server"  ForeColor="Red" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tabhead">Issue Type: <span class="label">*</span></td>
                                        <td class="tabbody">
                                            <asp:DropDownList CssClass="form-select" ID="ddlIssueType" runat="server" DataTextField="RC_NAME"
                                                DataValueField="RC_CODE" Width="325px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvIssueType" ValidationGroup="Save"
                                                ControlToValidate="ddlIssueType" ErrorMessage="Please select Issue Type"
                                                CssClass="span">
                                                </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tabhead">Issue Status: <span class="label">*</span></td>
                                        <td class="tabbody">
                                            <asp:DropDownList CssClass="form-select" ID="ddlIsueStatus" runat="server" DataTextField="RC_NAME"
                                                DataValueField="RC_CODE" Width="325px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvIsueStatus" ValidationGroup="Save"
                                                ControlToValidate="ddlIsueStatus" ErrorMessage="Please select Issue Status"
                                                CssClass="span">
                                                </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tabhead">Annexures:</td>
                                        <td class="tabbody">
                                            <table width="8%" align="left">
                                                <tr>
                                                    <td>
                                                        <asp:ImageButton runat="server" ID="imgBtnAddAttachment" OnClientClick="return addAttachmentRow()"
                                                            ImageUrl="~/Content/images/legacy/file_add.png" ToolTip="Click on add icon to add attachment(s)."
                                                            Style="width: 14px; height: auto; padding: 5px 3px 5px 0;" />
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton runat="server" ID="imgBtnDeleteAttachment" OnClientClick="return deleteAttachmentRow()"
                                                            ImageUrl="~/Content/images/legacy/delete_icon.png" ToolTip="Click on delete icon to delete attachment(s)."
                                                            Style="width: 14px; height: auto; padding: 5px 3px 5px 0;" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Literal ID="litAttachment" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tabhead">SPOC Responsible: <span class="label">*</span></td>
                                        <td class="tabbody">
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
                                                ErrorMessage="Please enter SPOC Responsible" CssClass="span" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tabhead">Remarks:</td>
                                        <td class="tabbody">
                                            <asp:TextBox CssClass="form-control" ID="txtRemarks" TextMode="MultiLine" Rows="10" runat="server"
                                                Columns="80"></asp:TextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />

                                            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtRemarks" ID="RegularExpressionValidator2" ValidationGroup="Save"
                                                ValidationExpression="^[\s\S]{0,4000}$" runat="server"  ForeColor="Red" ErrorMessage="Exceeding 4000 characters."></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div style="text-align: left;">
                                    <asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text="Update Issue Details"
                                        CssClass="html_button" OnClientClick="return onClientSaveClick('Save');" ValidationGroup="Save" />
                                    <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Cancel" OnClientClick="onCloseClick();"
                                        CssClass="html_button" />
                                </div>
                                <br />
                            </asp:Panel>

                        </asp:Panel>
                    </div>

                    <div class="tabbertab">
                        <h2>Issue Actions</h2>
                        <br />
                        <asp:GridView ID="gvActionables" runat="server" AutoGenerateColumns="False" DataKeyNames="CIA_ID"
                            AllowSorting="false" AllowPaging="false" GridLines="Both" CellPadding="4"
                            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            EmptyDataText="No record found...">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr.No." ControlStyle-Width="20px" HeaderStyle-Width="20px">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" ShowHeader="true">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select"
                                            Text='<%#"<img alt=\"Edit\" src=\""+ Fiction2Fact.Global.site_url("Content/images/legacy/EditInformationHS.png")+"\" border=\"0\" />" %>'
                                            CssClass="centerLink" OnClientClick='<%# string.Format("return onEditActionPlanClick(\"{0}\",\"{1}\");", Eval("CIA_ID"),Eval("CIA_CI_ID")) %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Type of Action" DataField="ActionType" />
                                <asp:TemplateField HeaderText="Actionables" ShowHeader="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActionPlan" runat="server" Text='<%#Eval("CIA_ACTIONABLE").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                        <asp:Label ID="lblRecStatus" runat="server" Visible="false" Text='<%#Eval("CIA_STATUS") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Action Plan Status" DataField="ActionStatus" />
                                <asp:BoundField HeaderText="Unit Responsible" DataField="CSFM_NAME" />
                                <asp:BoundField DataField="CIA_TARGET_DT" HeaderText="Target Date" ControlStyle-Width="110px" HeaderStyle-Width="110px" DataFormatString="{0:dd-MMM-yyyy}" />
                                <asp:BoundField DataField="CIA_CLOSURE_DT" HeaderText="Closure Date" ControlStyle-Width="110px" HeaderStyle-Width="110px" DataFormatString="{0:dd-MMM-yyyy}" />
                                <asp:BoundField HeaderText="Person Responsible" DataField="CIA_SPECIFIED_PERSON_NAME" />
                                <asp:TemplateField HeaderText="Remarks" ShowHeader="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("CIA_REMARKS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Status" DataField="RecStatus" Visible="false" />
                            </Columns>
                        </asp:GridView>
                    </div>

                </div>

            </div>
        </div>






    </form>
</body>
</html>
