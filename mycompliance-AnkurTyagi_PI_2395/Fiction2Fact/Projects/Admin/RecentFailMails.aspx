<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" Async="true" Inherits="Fiction2Fact.Projects.Admin.UnderWritingNew_RecentFailMails"
    Title="Recent Mail" CodeBehind="RecentFailMails.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="aspajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function SaveClick() {
            var checked = false;
            if (CheckBoxIDs != null) {
                for (var i = 1; i < CheckBoxIDs.length; i++) {
                    var cb = document.getElementById(CheckBoxIDs[i]);
                    if (cb.checked) {
                        checked = true;
                        break;
                    }
                }
                if (!checked) {
                    alert('Please select atleast one records.');
                    return false;
                }
                else {
                    return confirm('Are you sure that you want to re-send mail for the selected record(s)?');
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
    <script src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/jquery-ui.min.js")%>"></script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/jQuery_3.6.0.js") %>"></script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/jQuery_Migrate_3_3_2.js") %>"></script>

    <asp:Literal ID="CheckBoxIDsArray" runat="server"></asp:Literal>
    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Recent Mail(s)</h4>
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
                    <div class="row">
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Mail Status :</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                <asp:ListItem Text="Pending" Value="P"></asp:ListItem>
                                <asp:ListItem Text="Failed" Value="F"></asp:ListItem>
                                <asp:ListItem Text="Success" Value="S"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Subject:</label>
                            <F2FControls:F2FTextBox runat="server" ID="txtSearchSubject" CssClass="form-control"></F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSearchSubject" />
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">Content :</label>
                            <F2FControls:F2FTextBox runat="server" ID="txtSearchContent" CssClass="form-control"></F2FControls:F2FTextBox>
                            <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSearchContent" />
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">From Date :</label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtFromDate" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                                <asp:ImageButton ImageAlign="top" ToolTip="PopUp Calendar" CssClass="custom-calendar-icon" runat="server" ImageUrl="../../Content/images/legacy/calendar.jpg"
                                    ID="imgFromDate" />
                            </div>
                            <cc1:CalendarExtender ID="ceFromDate" runat="server" PopupButtonID="imgFromDate"
                                TargetControlID="txtFromDate" Format="dd-MMM-yyyy" CssClass="MyCalendar"></cc1:CalendarExtender>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">To Date :</label>
                            <div class="input-group">
                                <F2FControls:F2FTextBox ID="txtToDate" runat="server" CssClass="form-control"></F2FControls:F2FTextBox>
                                <asp:ImageButton ImageAlign="top" ToolTip="PopUp Calendar" CssClass="custom-calendar-icon" runat="server" ImageUrl="../../Content/images/legacy/calendar.jpg"
                                    ID="imgToDate" />
                            </div>
                            <cc1:CalendarExtender ID="ceToDate" runat="server" PopupButtonID="imgToDate" TargetControlID="txtToDate"
                                Format="dd-MMM-yyyy" CssClass="MyCalendar"></cc1:CalendarExtender>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label">&nbsp;</label>
                            <div>
                                <asp:LinkButton ID="btnSearch" runat="server" ValidationGroup="Save" CssClass="btn btn-outline-primary"
                                    Text="Search" OnClick="btnSearch_OnClick">
                                    <i class="fa fa-search"></i> Search                     
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExcelExport" runat="server" CssClass="btn btn-outline-secondary" Text="Export to Excel" OnClick="btnExcelExport_Click">
                                    <i class="fa fa-download"></i> Export to Excel 
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:Panel ID="pnlView" runat="server">
                            <asp:GridView ID="gvSearchMailSend" runat="server" BorderStyle="None" BorderWidth="1px"
                                CssClass="table table-bordered footable" AllowPaging="true" AutoGenerateColumns="false" DataKeyNames="ML_ID"
                                Width="100%" OnPageIndexChanging="gvSearchMailSend_PageIndexChanging" OnDataBound="gvSearchMailSend_DataBound"
                                EmptyDataText="No Records Found!!!!!!" PageSize="10">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="HeaderLevelCheckBox" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="RowLevelCheckBox" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Sr.No.
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mail ID" Visible="false" HeaderStyle-VerticalAlign="Middle"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                        <ItemTemplate>
                                            <div style="width: 200px; word-wrap: break-word;">
                                                <asp:Label ID="lblMailId" runat="server" Text='<%#Eval("ML_ID") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mail To" HeaderStyle-Width="10px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="10px">
                                        <ItemTemplate>
                                            <div style="width: 200px; word-wrap: break-word;">
                                                <asp:Label ID="lblMailTo" runat="server" Text='<%#Eval("ML_TO") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mail CC" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="10px">
                                        <ItemTemplate>
                                            <div style="width: 200px; word-wrap: break-word;">
                                                <asp:Label ID="lblMailCC" runat="server" Text='<%#Eval("ML_CC") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mail BCC" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="10px">
                                        <ItemTemplate>
                                            <div style="width: 200px; word-wrap: break-word; overflow: hidden;">
                                                <asp:Label ID="lblMailBCC" runat="server" Text='<%#Eval("ML_BCC") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mail Subject" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="10px">
                                        <ItemTemplate>
                                            <div style="width: 200px; word-wrap: break-word;">
                                                <asp:Label ID="lblMailSubject" runat="server" Text='<%#Eval("ML_SUBJECT") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mail Content" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <a href="javascript:void(0)" class="badge rounded-pill badge-soft-pink" onclick="showMailContent('<%# "dialog_" + Eval("ML_ID") %>')">View Mail Content</a>
                                            <%--<div class="dialog" id="<%# "dialog_" + Eval("ML_ID") %>" title="<%# "Subject: " +Eval("ML_SUBJECT") %>">
                                                <asp:Label ID="lblMailContentDialog" runat="server" Text='<%#Eval("ML_CONTENT") %>'></asp:Label>
                                            </div>--%>
                                            <div class="modal fade bd-example-modal-xl" id="<%# "dialog_" + Eval("ML_ID") %>" tabindex="-1" aria-labelledby="exampleModalFullscreenLgLabel" aria-hidden="true">
                                                <div class="modal-dialog modal-xl" role="document">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <h6 class="modal-title"><%# "Subject: " +Eval("ML_SUBJECT") %></h6>
                                                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                        </div>
                                                        <div class="modal-body">
                                                            <div class="row">
                                                                <div class="col-md-12 mb-3">
                                                                    <asp:Label ID="lblMailContentDialog" runat="server" Text='<%#Eval("ML_CONTENT") %>'></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <input id="btnCancelTerminationModal" runat="server" type="button" value="Cancel" data-bs-dismiss="modal"
                                                                class="modalCloseImg simplemodal-close btn btn-outline-danger" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Mail Content" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="40%">
                                <ItemTemplate>
                                    <div class="dialog" id="<%# "dialog_" + Eval("ML_ID") %>" title="<%# "Subject: " +Eval("ML_SUBJECT") %>">
                                        <asp:Label ID="lblMailContent" runat="server" Text='<%#Eval("ML_CONTENT") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="Mail Status">
                                        <ItemStyle Width="10%" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblMailStatus" runat="server" Text='<%#Eval("MailStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mail Send Date">
                                        <ItemStyle Width="10%" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblMailSendDate" runat="server" Text='<%#Eval("ML_SENT_ON","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mail Error">
                                        <ItemStyle Width="10%" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblMailError" runat="server" Text='<%#Eval("ML_ERROR_MSG") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mail Failed Count">
                                        <ItemStyle Width="10%" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblMailFailCount" runat="server" Text='<%#Eval("ML_NO_OF_FAILS") %>'></asp:Label>
                                            <asp:Label ID="lblMailType" runat="server" Text='<%#Eval("ML_MAIL_TYPE") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblAttachmentsVal" runat="server" Text='<%#Eval("ML_ATTACHMENT") %>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                    <div class="mt-3">
                        <asp:LinkButton ID="btnSendMail" runat="server" CssClass="btn btn-outline-success" Text="Send Mail"
                            OnClientClick="return SaveClick();" OnClick="btnSendMail_OnClick">
                            <i class="fa fa-paper-plane me-2"></i> Send Mail
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->


    <script type="text/javascript">
        $(document).ready(function () {
            $('.dialog').dialog({
                autoOpen: false
            });
        });
        function showMailContent(ContId) {
            $('#' + ContId).modal('show');
        }
    </script>
</asp:Content>
