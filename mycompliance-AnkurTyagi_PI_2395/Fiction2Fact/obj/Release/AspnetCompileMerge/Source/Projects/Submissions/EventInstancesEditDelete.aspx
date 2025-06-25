<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Submissions.Admin_EventInstancesEditDelete" Title="Event Instances" CodeBehind="EventInstancesEditDelete.aspx.cs" %>

<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="custom" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        function onClientEditClick() {
            document.getElementById('<%=hdfClientOperation.ClientID%>').value = "Edit";
        }
        function onClientDeleteClick() {
            if (!confirm('Are you sure that you want to delete this record?')) return false;
            document.getElementById('<%=hdfClientOperation.ClientID%>').value = "Delete";
        }
    </script>

    <asp:HiddenField ID="hdfClientOperation" runat="server" />
    <asp:HiddenField ID="hfEventId" runat="server" />
    <asp:HiddenField ID="hfSelectedRecord" runat="server" />
    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Edit/Delete Events Instances</h4>
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
    <div class="row">
        <div class="col-12">
            <div class="card">
                <asp:MultiView ID="mvMultiView" runat="server">
                    <asp:View ID="vwGrid" runat="server">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Event Name</label>
                                    <asp:DropDownList ID="ddlEvent" CssClass="form-select" AppendDataBoundItems="true"
                                        runat="server" DataValueField="EM_ID" DataTextField="EM_EVENT_NAME">
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Event Date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtDateOfEvent" CssClass="form-control" Width="275px" runat="server"
                                            Columns="10"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="imgEventDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />&nbsp;
                                    </div>
                                    <asp:RegularExpressionValidator ID="rev" runat="server" ControlToValidate="txtDateOfEvent" CssClass="text-danger"
                                        ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                        ValidationGroup="SearchEventsDetails" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <ajaxToolkit:CalendarExtender ID="ceOnceFromDate" runat="server" PopupButtonID="imgEventDate"
                                        TargetControlID="txtDateOfEvent" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                </div>
                            </div>
                            <div class="mt-3 text-center">
                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnSearch1" Text="Search" AccessKey="s" OnClick="btnSearch_Click"
                                    runat="server" ValidationGroup="SearchEventsDetails">
                                        <i class="fa fa-search"></i> Search                     
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gvEventMaster" runat="server" AutoGenerateColumns="False" DataKeyNames="EI_ID"
                                    AllowSorting="true" AllowPaging="true" GridLines="None" OnPageIndexChanging="gvEventMaster_PageIndexChanging"
                                    OnSorting="gvEventMaster_Sorting" CellPadding="4" OnSelectedIndexChanged="gvEventMaster_SelectedIndexChanged"
                                    CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Sr No.
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSrNo" Text='<%# Container.DataItemIndex + 1 %>' runat="server"> 
                                                    </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="EM_EVENT_NAME" SortExpression="EM_EVENT_NAME" HeaderText="Event Name" />
                                        <asp:TemplateField HeaderText="Event Agenda" SortExpression="EI_ID">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEventAgenda" Text='<%#LoadEventAgenda(Eval("EI_ID"))%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Event Date" SortExpression="EI_EVENT_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="EI_EVENT_DATE" Text='<%#Eval("EI_EVENT_DATE","{0:dd-MMM-yyyy}")%>'
                                                    runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="EI_DESCRIPTION" SortExpression="EI_DESCRIPTION" HeaderText="Event Description" />--%>
                                        <asp:TemplateField HeaderText="Event Description" SortExpression="EI_DESCRIPTION">
                                            <ItemTemplate>
                                                <asp:Label ID="EI_DESCRIPTION" Text='<%#Eval("EI_DESCRIPTION").ToString().Replace(Environment.NewLine, "<br />")%>'
                                                    runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit" ShowHeader="true">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-success btn-circle" OnClientClick="onClientEditClick()">
                                                    <i class="fa fa-pen"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete" ShowHeader="true">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-danger btn-circle" OnClientClick="onClientDeleteClick()">
                                                    <i class="fa fa-trash"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" Text="&lt;img alt='Edit' src='../../Content/images/legacy/EditInformationHS.png' border='0' &gt;"
                                                        OnClientClick="onClientEditClick()">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        <%--<asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Select" Text="&lt;img alt='Delete' src='../../Content/images/legacy/delete.png' border='0' &gt;"
                                                        OnClientClick=" return onClientDeleteClick()">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwForm" runat="server">
                        <asp:FormView ID="fvEventMaster" runat="server" OnDataBound="fvEventMaster_DataBound"
                            DefaultMode="Edit" AllowPaging="True" DataKeyNames="EI_ID" Width="100%">
                            <EditItemTemplate>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Depends On Event:<span class="text-danger">*</span></label>
                                            <asp:DropDownList ID="ddlEventEdit" CssClass="form-select" runat="server"
                                                AutoPostBack="true" DataValueField="EM_ID" DataTextField="EM_EVENT_NAME"
                                                OnSelectedIndexChanged="ddlEvent_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvEvent" runat="server" ControlToValidate="ddlEventEdit" CssClass="text-danger"
                                                ValidationGroup="SaveEventsDetails" Display="Dynamic" ErrorMessage="Please select Event."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Event Agenda:<span class="text-danger">*</span></label>
                                            <div class="custom-checkbox-table">
                                                <asp:CheckBoxList ID="cblAgenda" runat="server" RepeatColumns="7" AppendDataBoundItems="true"
                                                    DataTextField="EP_NAME" DataValueField="EP_ID" CssClass="form-control">
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Event Date:<span class="text-danger">*</span></label>
                                            <div class="input-group">
                                                <F2FControls:F2FTextBox ID="txtEventDate" CssClass="form-control" runat="server" Text='<%# Eval("EI_EVENT_DATE", "{0:dd-MMM-yyyy}")%>'
                                                    MaxLength="11"></F2FControls:F2FTextBox>
                                                <asp:ImageButton ID="imgMFD" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                                    ImageUrl="../../Content/images/legacy/calendar.jpg" />&nbsp;
                                            </div>
                                            <asp:RegularExpressionValidator ID="revEventDate" runat="server" ControlToValidate="txtEventDate"
                                                ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" CssClass="span"
                                                ValidationGroup="SaveEventDetails" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEventDate" CssClass="text-danger"
                                                ValidationGroup="SaveEventDetails" Display="Dynamic" SetFocusOnError="True" ErrorMessage="Please enter Event Date."></asp:RequiredFieldValidator>
                                            <ajaxToolkit:CalendarExtender ID="ceEventDate" runat="server" PopupButtonID="imgMFD"
                                                TargetControlID="txtEventDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        </div>
                                        <div class="col-md-12 mb-3">
                                            <label class="form-label">Event Description:<span class="text-danger">*</span></label>
                                            <F2FControls:F2FTextBox ID="txtEventDescription" CssClass="form-control" TextMode="MultiLine"
                                                Columns="70" Rows="3" MaxLength="200" runat="server" Text='<%# Eval("EI_DESCRIPTION")%>'>
                                            </F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtEventDescription" />
                                            <asp:RequiredFieldValidator ID="rfvEventDescription" runat="server" ControlToValidate="txtEventDescription" CssClass="text-danger"
                                                ValidationGroup="SaveEventDetails" Display="Dynamic" SetFocusOnError="True" ErrorMessage="Please enter Description."></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revPerticulars" ControlToValidate="txtEventDescription"
                                                Display="Dynamic" Text="Exceeding 200 characters" ValidationExpression="^[\s\S]{0,200}$" CssClass="text-danger"
                                                runat="server" SetFocusOnError="True" ValidationGroup="SaveEventDetails" />
                                        </div>
                                    </div>
                                </div>
                                <div class="mt-3 text-center">
                                    <asp:LinkButton ID="btnSave1" runat="server" OnClick="btnSave_Click" Text="Update" CssClass="btn btn-outline-success" ValidationGroup="SaveEventDetails">
                                    <i class="fa fa-save me-2"></i> Update                  
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel1" runat="server" OnClick="btnCancel_Click" Text="Cancel" CssClass="btn btn-outline-danger">
                                    <i class="fa fa-arrow-left me-2"></i> Cancel                  
                                    </asp:LinkButton>
                                </div>
                            </EditItemTemplate>
                        </asp:FormView>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
    <!-- end row -->
</asp:Content>
