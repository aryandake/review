<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.Certification_SearchChecklist" Title="Search Certification Checklist" CodeBehind="SearchChecklist.aspx.cs" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/commonvalidations.js") %>'>
    </script>
    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/Exception.js")%>'></script>
    <script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/CertificationDataUpload.js")%>'>
    </script>

    <script type="text/javascript">
        function onClientEditClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Edit";
        }

        function onClientDeleteClick() {
            if (!confirm('Are you sure that you want to delete this record?')) return false;
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Delete";
        }

        function onClientViewClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "View";
        }

        function onClientViewCircClick(CMId) {
            window.open('../Circulars/ViewCircularDetails.aspx?CircularId=' + CMId, '', 'location=0,status=0,resizable=1,scrollbars=1,width=1000,height=800');
            return false;
        }

        function onClientCopyClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "Copy";
        }

    </script>

    <%--//Added By Milan Yadav on 27-Sep-2016--%>
    <script type="text/javascript">
        function DeactivateClick() {
            var checked = false;
            if (CheckBoxIDs != null) {
                for (var i = 1; i < CheckBoxIDs.length; i++) {
                    var cb = document.getElementById(CheckBoxIDs[i]);
                    if (cb.checked) {
                        var validated = Page_ClientValidate('SaveDeactivationDetails');
                        if (validated) {
                            return getDeactivationDetails();
                        }
                        else {
                            return false;
                        }
                        checked = true;
                        break;
                    }
                }
                if (!checked) {
                    alert('Please select atleast one record for de-activation.');
                    return false;
                }
                else {
                    return confirm('Are you sure that you want to De-activate checklist for the selected record(s)?');
                }
            }
        }

        function ChangeCheckBoxState(id, checkState) {
            var cb = document.getElementById(id);
            if (cb != null)
                cb.checked = checkState;
        }

        function ChangeAllCheckBoxStates(checkState) {
            if (CheckBoxIDs != null) {
                for (var i = 0; i < CheckBoxIDs.length; i++)
                    ChangeCheckBoxState(CheckBoxIDs[i], checkState);
            }
        }

        function onRowCheckedUnchecked(cbid) {
            ChangeHeaderAsNeeded();
            return;
        }
        function ChangeHeaderAsNeeded() {
            if (CheckBoxIDs != null) {
                for (var i = 1; i < CheckBoxIDs.length; i++) {
                    var cb = document.getElementById(CheckBoxIDs[i]);
                    if (!cb.checked) {
                        ChangeCheckBoxState(CheckBoxIDs[0], false);
                        return;
                    }
                }
                ChangeCheckBoxState(CheckBoxIDs[0], true);
            }
        }
    </script>

    <asp:HiddenField runat="server" ID="hfSelectedOperation" />
    <asp:HiddenField ID="hfDepartmentID" runat="server" />
    <asp:Literal ID="CheckBoxIDsArray" runat="server"></asp:Literal>

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Certification Compliance Checklist</h4>
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
                    <%--//<<Added by Ankur Tyagi on 16-May-2024 for CR_2070--%>
                    <div class="mb-4">
                        <asp:Label ID="Label1" runat="server" Text="<strong> Note: </strong>"
                            CssClass=""></asp:Label>

                        <div class="input-group">
                            <i class="fa fa-arrow-alt-circle-right me-2"></i>
                            <span>
                                <asp:LinkButton ID="lnkExportAllData" runat="server" OnClick="lnkExportAllData_Click">Click here</asp:LinkButton>
                                to download all dump of checklist.</span>
                        </div>

                    </div>
                    <%-->>--%>
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label class="form-label">Department Name:</label>
                            <asp:DropDownList CssClass="form-select" ID="ddlSearchDeptName" runat="server"
                                DataValueField="CSSDM_ID" DataTextField="DeptName">
                            </asp:DropDownList>

                        </div>
                        <div class="col-md-6 mb-3">
                            <label class="form-label">&nbsp;</label>
                            <div>
                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnSearch" Text="Search" runat="server" OnClientClick="return SearchChecklist();"
                                    OnClick="btnSearch_Click">
                                    <i class="fa fa-search"></i> Search                     
                                </asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnAdd" Text="Add" runat="server" OnClick="btnAdd_Click">
                                    <i class="fa fa-plus"></i> Add
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <%--//Added BY Milan Yadav on 27-Sep-2016--%>
                    <asp:Panel ID="pnlDeactivate" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Effective To :<span class="text-danger">*</span></label>
                                <div class="input-group">
                                    <F2FControls:F2FTextBox CssClass="form-control" ID="txtEffectiveToDate" runat="server"
                                        MaxLength="11" Columns="50" Rows="3"></F2FControls:F2FTextBox>
                                    <asp:ImageButton ToolTip="PopUp Calendar" runat="server" ImageUrl="../../Content/images/legacy/calendar.jpg"
                                        ID="imgTDate" CssClass="custom-calendar-icon" />
                                </div>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEffectiveToDate"
                                    ErrorMessage="Date Format has to be dd-MMM-yyyy." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                    Display="Dynamic" ValidationGroup="Save"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfvEffectiveToDate" Width="275px" runat="server" ControlToValidate="txtEffectiveToDate" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="SaveDeactivationDetails" SetFocusOnError="True"
                                    ErrorMessage="Enter Effective To Date.">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-5 mb-3">
                                <label class="form-label">Deactivation Remarks :</label>
                                <F2FControls:F2FTextBox CssClass="form-control" ID="txtRemarks" runat="server" TextMode="MultiLine" Columns="50" Rows="1"> </F2FControls:F2FTextBox>
                                <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtRemarks" />
                                <asp:RequiredFieldValidator ID="rfvRemarks" runat="server" ControlToValidate="txtRemarks"
                                    ValidationGroup="SaveDeactivationDetails" Display="Dynamic" SetFocusOnError="True" CssClass="text-danger" ErrorMessage="Enter Deactivation Remarks.">*</asp:RequiredFieldValidator>

                            </div>
                            <div class="col-md-3 mb-3">
                                <label class="form-label">&nbsp;</label>
                                <div>
                                    <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnDeactivate" ValidationGroup="SaveDeactivationDetails" OnClick="btnDeactivate_Click" OnClientClick="return DeactivateClick();" Text="Deactivate" runat="server">
                                        <i class="fa fa-ban"></i> Deactivate               
                                    </asp:LinkButton>
                                    <%--  Start - added by Hari on 12 Oct 2016--%>
                                    <asp:LinkButton CssClass="btn btn-outline-secondary" ID="btnExportToExcel" runat="server" Text="Export To Excel"
                                        OnClick="btnExportToExcel_Click">
                                        <i class="fa fa-download"></i> Export to Excel               
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="Pnlsearchchecklist" runat="server" Visible="true">
                        <div class="custom-checkbox-table">
                            <asp:GridView ID="gvChecklist" runat="server" BorderStyle="None" BorderWidth="1px"
                                CellPadding="4" GridLines="None" AutoGenerateColumns="false" CssClass="table table-bordered footable"
                                DataKeyNames="CCM_ID" OnSelectedIndexChanged="gvChecklist_SelectedIndexChanged"
                                EmptyDataText="No records found..." OnDataBound="gvChecklist_DataBound"
                                OnRowDataBound="gvChecklist_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="HeaderLevelCheckBox" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="RowLevelCheckBox" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View">
                                        <ItemTemplate>
                                            <center>
                                                <asp:LinkButton ID="lnkview" CssClass="btn btn-sm btn-soft-info btn-circle" ImageAlign="Top"
                                                    OnClientClick="onClientViewClick()" CommandName="Select" runat="server">
                                                    <i class="fa fa-eye"></i>
                                                </asp:LinkButton>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View Circulars">
                                        <ItemTemplate>
                                            <center>
                                                <asp:LinkButton ID="lnkViewCirc" runat="server" CssClass="btn btn-sm btn-soft-info btn-circle"
                                                    CommandName="Select">
                                                    <%--OnClientClick='<%# string.Format("onClientViewCircClick(\"{0}\");", Eval("CCM_CM_ID")) %>'--%>
                                                    <i class="fa fa-eye"></i>
                                                </asp:LinkButton>
                                                <asp:HiddenField ID="hfCircularId" runat="server" Value='<%#Eval("CCM_CM_ID") %>' />
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <center>
                                                <asp:LinkButton ID="lnkEdit" CssClass="btn btn-sm btn-soft-success btn-circle" ImageUrl="../../Content/images/legacy/EditInformationHS.png" ImageAlign="Top"
                                                    OnClientClick="onClientEditClick()" CommandName="Select" runat="server">
                                                    <i class="fa fa-pen"></i>
                                                </asp:LinkButton>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Copy">
                                        <ItemTemplate>
                                            <center>
                                                <asp:LinkButton ID="lnkCopy" CssClass="btn btn-sm btn-soft-primary btn-circle" ImageAlign="Top"
                                                    OnClientClick="onClientCopyClick()" CommandName="Select" runat="server">
                                                    <i class="fa fa-copy"></i>
                                                </asp:LinkButton>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:ImageButton ID="lnkDelete" ImageUrl="../../Content/images/legacy/delete.png" ImageAlign="Top" OnClientClick="return onClientDeleteClick()"
                                        CommandName="Select" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Sr. No.
                               
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfChecklistMasId" runat="server" Value='<%#Eval("CCM_ID") %>' />
                                            <center>
                                                <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Department Name" DataField="CSSDM_NAME" />
                                    <asp:TemplateField HeaderText="Act/Regulation/Circular" SortExpression="CCM_ACT_REGULATION_CIRCULAR">
                                        <ItemTemplate>
                                            <asp:Label ID="lblActRegCird" runat="server" Text='<%#Eval("CDTM_TYPE_OF_DOC").ToString().Replace("\n","<br />") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reference Circular/Notification/Act" SortExpression="CCM_REFERENCE">
                                        <ItemTemplate>
                                            <asp:Label ID="txtRegulations" Text='<%#Eval("CCM_REFERENCE").ToString().Replace("\n", "<br />") %>'
                                                runat="server"></asp:Label><%--Text='<%#Eval("CCM_REFERENCE").ToString().Replace("\n", "<br />") %>'--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Section/Clause" SortExpression="CCM_CLAUSE">
                                        <ItemTemplate>
                                            <asp:Label ID="txtSections" Text='<%#Eval("CCM_CLAUSE").ToString().Replace("\n", "<br />") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Compliance of/Heading of Compliance checklist" SortExpression="CCM_CHECK_POINTS">
                                        <ItemTemplate>
                                            <asp:Label ID="txtCheckPoint" Text='<%#Eval("CCM_CHECK_POINTS").ToString().Replace("\n", "<br />") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" SortExpression="CCM_PARTICULARS">
                                        <ItemTemplate>
                                            <asp:Label ID="txtSelfAssessmentStatus" Text='<%#Eval("CCM_PARTICULARS").ToString().Replace("\n", "<br />") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Consequences of non Compliance" SortExpression="CCM_PENALTY">
                                        <ItemTemplate>
                                            <asp:Label ID="txtPenalty" Text='<%#Eval("CCM_PENALTY").ToString().Replace("\n", "<br />") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CCM_FREQUENCY" HeaderText="Frequency" />
                                    <asp:BoundField DataField="CCM_FORMS" HeaderText="Forms" />
                                    <asp:BoundField DataField="Effective From" HeaderText="Effective From" />
                                    <asp:BoundField DataField="Effective To" HeaderText="Effective To" />
                                    <asp:BoundField HeaderText="Status" DataField="Status" />
                                    <asp:TemplateField HeaderText="Deactivation Remarks" SortExpression="Remark">
                                        <ItemTemplate>
                                            <asp:Label ID="txtDeactivationRemarks" Text='<%#Eval("Remark").ToString().Replace("\n", "<br />") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="Deactivation Remarks" DataField="Remark" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <cc1:CalendarExtender ID="ceTDate" runat="server" PopupButtonID="imgTDate" TargetControlID="txtEffectiveToDate"
                        Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->


</asp:Content>
