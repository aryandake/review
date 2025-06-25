<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Circulars.Circulars_ViewCircular" Title="View Circular Details"
    EnableEventValidation="true" CodeBehind="ViewCircular.aspx.cs" %>

<%--MasterPageFile="~/HomeMaster.master"--%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js") %>"></script>

    <script type="text/javascript">
        function showCircularGist(CMId) {
            window.open('ViewCircularDetails.aspx?CircularId=' + CMId, '', 'location=0,status=0,resizable=1,scrollbars=1,width=1000,height=800');
            return false;
        }
        function onViewClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "View";
        }
        function compareEndSystemDates(source, arguments) {
            try {
                //var ContractTemplateId = document.getElementById('ctl00_ContentPlaceHolder1_hfCTId').value;
                //if (ContractTemplateId == '' || ContractTemplateId == null) {

                var Fromdate = document.getElementById('ctl00_ContentPlaceHolder1_txtFromDate');
                var ToDate = document.getElementById('ctl00_ContentPlaceHolder1_txtToDate');

                if (Fromdate.value != '') {
                    if (compare2Dates(ToDate, Fromdate) > 1) {
                        arguments.IsValid = false;
                    }
                    else {
                        arguments.IsValid = true;
                    }
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
    </script>

    <asp:HiddenField ID="hfSelectedRecord" runat="server" />
    <asp:HiddenField ID="hfSelectedOperation" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">View Circulars</h4>
                        <asp:Label ID="lblInfo" runat="server" CssClass="custom-alert-box"></asp:Label>
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
                <asp:MultiView ID="mvMultiView" runat="server">
                    <asp:View ID="vwGrid" runat="server">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-3 mb-3">
                                    <label class="form-label">Global Search:</label>
                                    <F2FControls:F2FTextBox ID="txtGlobalSearch" ToolTip="Implications" CssClass="form-control" runat="server"
                                        Columns="30"></F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtGlobalSearch" />
                                </div>
                                <div class="col-md-4 mb-3" style="display: none">
                                    <label class="form-label">Created By:</label>
                                    <asp:DropDownList ID="ddlDepartment" CssClass="form-select" runat="server" DataTextField="CDM_NAME"
                                        DataValueField="CDM_ID" AppendDataBoundItems="True" ToolTip="Created By">
                                        <asp:ListItem Selected="True" Value="">(Select)</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4 mb-3" style="display: none">
                                    <label class="form-label">Issuing Authority</label>
                                    <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlSIssuingauthority"
                                        runat="server">
                                    </f2f:DropdownListNoValidation>
                                    <ajaxToolkit:CascadingDropDown ID="cddIssuingAuthority" runat="server" TargetControlID="ddlSIssuingauthority"
                                        Category="IssuingAuthority" PromptText="Select a Issuing Authority" ServicePath="AJAXDropdownCirculars.asmx"
                                        ServiceMethod="GetIssuingAuthority" />
                                </div>
                                <div class="col-md-4 mb-3" style="display: none">
                                    <label class="form-label">Topic</label>
                                    <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlSArea" runat="server">
                                    </f2f:DropdownListNoValidation>
                                    <ajaxToolkit:CascadingDropDown ID="cddTopic" runat="server" TargetControlID="ddlSArea"
                                        ParentControlID="ddlSIssuingauthority" Category="Topic" PromptText="Select Topic"
                                        ServicePath="AJAXDropdownCirculars.asmx" ServiceMethod="GetTopicByIssuingAuthority" />
                                </div>
                                <div class="col-md-3 mb-3">
                                    <label class="form-label">From Date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtFromDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="ibFrmDate" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" runat="server" />
                                    </div>
                                    <asp:RegularExpressionValidator ID="revFromDate" runat="server" ControlToValidate="txtFromDate"
                                        Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" span class="text-danger"
                                        ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-3 mb-3">
                                    <label class="form-label">To Date</label>
                                    <div class="input-group">
                                        <F2FControls:F2FTextBox ID="txtToDate" CssClass="form-control" runat="server" MaxLength="11" Columns="11"></F2FControls:F2FTextBox>
                                        <asp:ImageButton ID="ibToDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                            ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                    </div>
                                    <asp:RegularExpressionValidator ID="revToDate" runat="server" ControlToValidate="txtToDate"
                                        Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$" span class="text-danger"
                                        ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                                    <asp:CustomValidator ID="cvCompletionDate" runat="server" ClientValidationFunction="compareEndSystemDates"
                                        ControlToValidate="txtToDate" ErrorMessage="From Date should be less than or Equal to To Date." span class="text-danger"
                                        Display="Dynamic" OnServerValidate="cvTodate_ServerValidate" ValidationGroup="SEARCH"></asp:CustomValidator>
                                </div>
                                <div class="col-md-4 mb-3" style="display: none">
                                    <label class="form-label">Type of Document:</label>
                                    <f2f:DropdownListNoValidation CssClass="form-select" ID="ddlTypeofDocument" AppendDataBoundItems="true"
                                        runat="server" ToolTip="Type of Document" DataTextField="CDTM_TYPE_OF_DOC" DataValueField="CDTM_ID">
                                    </f2f:DropdownListNoValidation>
                                </div>
                                <div class="col-md-4 mb-3" style="display: none">
                                    <label class="form-label">Circular No.:</label>
                                    <F2FControls:F2FTextBox ID="txtCircularNo" CssClass="form-control" runat="server" MaxLength="50"
                                        ToolTip="Circular Number" Columns="30"></F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtCircularNo" />
                                </div>
                                <div class="col-md-4 mb-3" style="display: none">
                                    <label class="form-label">Subject of the Circular:</label>
                                    <F2FControls:F2FTextBox ID="txtSubject" ToolTip="Subject of the Circular" CssClass="form-control"
                                        runat="server" MaxLength="200" Columns="30"></F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtSubject" />
                                </div>
                                <div class="col-md-4 mb-3" style="display: none">
                                    <label class="form-label">Gist of the Circular:</label>
                                    <F2FControls:F2FTextBox ID="txtGist" CssClass="form-control" runat="server" Columns="30" ToolTip="Gist of the Circular"> </F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtGist" />
                                </div>
                                <div class="col-md-4 mb-3" style="display: none">
                                    <label class="form-label">Implications:</label>
                                    <F2FControls:F2FTextBox ID="txtImplications" ToolTip="Implications" CssClass="form-control" runat="server"
                                        Columns="30"></F2FControls:F2FTextBox>
                                    <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtImplications" />
                                </div>

                                <div class="col-md-12 mb-3" style="display: none">
                                    <label class="form-label">Associated Keywords:</label>
                                    <div class="custom-checkbox-table">
                                        <asp:Panel ID="pnlAssociatedKeywords" runat="server" ScrollBars="Auto" CssClass="form-control">
                                            <asp:CheckBoxList ID="cbAssociatedKeywordsSearch" RepeatColumns="4" runat="server" DataTextField="CKM_NAME"
                                                DataValueField="CKM_ID" AppendDataBoundItems="True" ToolTip="Associated Keywords" RepeatDirection="Horizontal">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div class="col-md-12 mb-3" style="display: none">
                                    <label class="form-label">To be placed before:</label>
                                    <div class="custom-checkbox-table">
                                        <asp:Panel ID="pnlToBePlacedBeforeSearch" runat="server" ScrollBars="Auto" CssClass="form-control">
                                            <asp:CheckBoxList ID="cbToBePlacedBeforeSearch" RepeatColumns="4" runat="server" DataTextField="RC_NAME"
                                                DataValueField="RC_CODE" AppendDataBoundItems="True" ToolTip="To be placed before" RepeatDirection="Horizontal">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div class="col-md-3 mb-3">
                                    <label class="form-label">Status:</label>
                                    <asp:DropDownList ID="ddlStatus" CssClass="form-select" runat="server"
                                        ToolTip="Select status">
                                        <asp:ListItem Value="">(Select)</asp:ListItem>
                                        <asp:ListItem Value="A">Active</asp:ListItem>
                                        <asp:ListItem Value="I">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="text-center mt-3">
                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnSearch" Text="Search" runat="server" ValidationGroup="SEARCH"
                                    AccessKey="s" OnClick="btnSearch_Click">
                                    <i class="fa fa-search"></i> Search                          
                                </asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-outline-danger" ID="lnkReset" Text="Reset" runat="server"
                                    OnClick="lnkReset_Click">Reset
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExportToExcel" Visible="false" runat="server" CssClass="btn btn-outline-secondary"
                                    Text="Export to Excel" OnClick="btnExportToExcel_Click">
                                <i class="fa fa-download"></i> Export to Excel               
                                </asp:LinkButton>
                                <%--<asp:Button ID="btnExportToExcel" runat="server" CssClass="Button" Text="Export to Excel" OnClick="btnExportToExcel_Click" />--%>
                            </div>
                        </div>

                        <cc1:CalendarExtender ID="ceFrmDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibFrmDate"
                            TargetControlID="txtFromDate"></cc1:CalendarExtender>
                        <cc1:CalendarExtender ID="ceTodate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibToDate"
                            TargetControlID="txtToDate"></cc1:CalendarExtender>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gvCircularMaster" runat="server" AutoGenerateColumns="False" DataKeyNames="CM_ID"
                                    AllowSorting="true" AllowPaging="true" GridLines="None" OnSorting="gvCircularMaster_Sorting"
                                    CellPadding="4" OnPageIndexChanging="gvCircularMaster_PageIndexChanging" OnRowDataBound="gvCircularMaster_RowDataBound"
                                    OnSelectedIndexChanged="gvCircularMaster_SelectedIndexChanged" CssClass="table table-bordered footable"
                                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnRowCreated="OnRowCreated">
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCircularId" runat="server" Text='<%# Bind("CM_ID") %>' Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="View Details">
                                            <ItemTemplate>
                                                <center>
                                                    <%-- <asp:LinkButton ID="lblView" runat="server" CssClass="btn btn-sm btn-soft-info btn-circle" OnClientClick="onViewClick()" CommandName="Select">
                                                        <i class="fa fa-eye"></i>
                                                    </asp:LinkButton>--%>
                                                    <asp:LinkButton ID="lnkView" runat="server" CausesValidation="False" CssClass="btn btn-sm btn-soft-success btn-circle" OnClientClick="onViewClick()"
                                                        CommandName="Select">
                                                        <i class="fa fa-eye"></i>
                                                    </asp:LinkButton>
                                                </center>
                                                <asp:Label ID="lbId" Text='<% # Eval("CM_ID") %>' Visible="false" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Line of Business" SortExpression="LEM_NAME" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLOB" runat="server" Text='<%# Eval("LEM_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Subject" SortExpression="CM_TOPIC">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTopic" runat="server" Text='<%# Eval("CM_TOPIC") %>' Width="400px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Topic" SortExpression="CAM_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblArea" runat="server" Text='<%# Eval("CAM_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Issuing Authority" SortExpression="CIA_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssAuth" runat="server" Text='<%# Eval("CIA_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Circular No" SortExpression="CM_CIRCULAR_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCircNo" runat="server" Text='<%# Eval("CM_CIRCULAR_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Circular Date" SortExpression="CM_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCircularDate" runat="server" Text='<%# Eval("CM_DATE", "{0:dd-MMM-yyyy }") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SPOC From Compliance Department">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSpocFromcompliance" runat="server" Text='<%# Eval("SPOCName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Circular Effective Date" SortExpression="CM_CIRC_EFF_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCircularEffDate" runat="server" Text='<%# Eval("CM_CIRC_EFF_DATE", "{0:dd-MMM-yyyy }") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Old Circular Subject/No." SortExpression="CM_OLD_CIRC_SUB_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOldCircSubNo" runat="server" Text='<%# Eval("CM_OLD_CIRC_SUB_NO") %>' Width="400px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Files">
                                            <ItemTemplate>
                                                <%--<asp:DataList ID="dlCircularFiles" runat="server" DataKeyField="CF_ID" RepeatColumns="1"
                                                    RepeatDirection="Horizontal" DataSource='<%#LoadCircularFileList(Eval("CM_ID"))%>'>--%>
                                                <asp:DataList ID="dlCircularFiles" runat="server" DataKeyField="CF_ID" RepeatColumns="1" CssClass="custom-datalist-border"
                                                    RepeatDirection="Vertical" DataSource='<%#LoadCircularFileList(Eval("CM_ID"))%>'>
                                                    <ItemTemplate>
                                                        <%--<a href="#" onclick="javascript:window.open('../CommonDownload.aspx?type=Circular&downloadFileName=<%#getFileName(Eval("CF_SERVERFILENAME"))%>','','location=1,status=0,scrollbars=2,width=650,height=500');">
                                                            <asp:Image ID="lblIssuerLink" runat="server" ImageUrl="../../Content/images/legacy/viewfulldetails.png" />
                                                        </a>--%>
                                                        <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=Circular&downloadFileName=<%#getFileName(Eval("CF_SERVERFILENAME"))%>','','location=0,status=0,scrollbars=1,width=10,height=20');return false">
                                                            <%#Eval("CF_FILENAME")%></a>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Issuer Link">
                                            <ItemTemplate>
                                                <a href='<%# IsValidUrl(Eval("CM_ISSUING_LINK").ToString()) %>' target="_blank" onclick='<%# IsValidUrl(Eval("CM_ISSUING_LINK").ToString())=="#"?"return false;":"return true;" %>'>
                                                    <%--<i class="fa fa-eye"></i>--%>
                                                    <%# Eval("CM_ISSUING_LINK") %>
                                                </a>
                                                <asp:Label ID="lblIssuerLink" runat="server" Text='<%# Eval("CM_ISSUING_LINK") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="CircStatus" SortExpression="CircStatus" />

                                        <asp:TemplateField HeaderText="Created By" SortExpression="CDM_NAME" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CDM_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<< Added by Amarjeet on 04-Aug-2021--%>
                                        <asp:TemplateField HeaderText="Linkage With Earlier Circular" SortExpression="LinkageWithEarlierCircular" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLinkageWithEarlierCircular" runat="server" Text='<%# Eval("LinkageWithEarlierCircular") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Supersedes or Extends/Amends Old Circular(s)" SortExpression="SOCEOC" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSOCEOC" runat="server" Text='<%# Eval("SOCEOC") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-->>--%>
                                        <asp:TemplateField HeaderText="Type of Document" SortExpression="CDTM_TYPE_OF_DOC" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTypeofDocument" runat="server" Text='<%# Eval("CDTM_TYPE_OF_DOC") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gist of the Circular" SortExpression="CM_DETAILS" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGist" runat="server" ToolTip='<%# replaceHTMLTagsFromLableTooltip(Eval("CM_DETAILS").ToString()) %>'
                                                    Text='<%#Eval("CM_DETAILS").ToString().Length > 200 ? (Eval("CM_DETAILS") as string).Substring(0, 200).Replace("\n", "<br />") + " ..." : Eval("CM_DETAILS").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                                <asp:Label ID="lblGist1" Visible="false" runat="server" Text='<%#Eval("CM_DETAILS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Implications" SortExpression="CM_IMPLICATIONS" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblImplications" runat="server" ToolTip='<%# replaceHTMLTagsFromLableTooltip(Eval("CM_IMPLICATIONS").ToString()) %>'
                                                    Text='<%#Eval("CM_IMPLICATIONS").ToString().Length > 200 ? (Eval("CM_IMPLICATIONS") as string).Substring(0, 200).Replace("\n", "<br />") + " ..." : Eval("CM_IMPLICATIONS").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                                <asp:Label ID="lblImplications1" Visible="false" runat="server" Text='<%#Eval("CM_IMPLICATIONS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Requirement for the Board/Audit committee to approve" DataField="AuditCommitteeToApprove" SortExpression="AuditCommitteeToApprove" Visible="false" />
                                        <asp:BoundField HeaderText="To be placed before" DataField="ToBePlacedBefore" SortExpression="ToBePlacedBefore" Visible="false" />
                                        <asp:TemplateField HeaderText="Details" SortExpression="CM_IMPLICATIONS" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDetails" runat="server" ToolTip='<%# replaceHTMLTagsFromLableTooltip(Eval("CM_REMARKS").ToString()) %>'
                                                    Text='<%#Eval("CM_REMARKS").ToString().Length > 200 ? (Eval("CM_REMARKS") as string).Substring(0, 200).Replace("\n", "<br />") + " ..." : Eval("CM_REMARKS").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                                <asp:Label ID="lblDetails1" Visible="false" runat="server" Text='<%#Eval("CM_REMARKS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Name of the Policy/Guidelines" DataField="CM_NAME_OF_THE_POLICY" SortExpression="CM_NAME_OF_THE_POLICY" Visible="false" />
                                        <asp:BoundField HeaderText="Frequency" DataField="CM_FREQUENCY" SortExpression="CM_FREQUENCY" Visible="false" />
                                        <asp:TemplateField HeaderText="Certification Checklist Added" SortExpression="IsCertChecklistAdded" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCertChklistAdded" runat="server" Text='<%# Eval("IsCertChecklistAdded") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reporting Actionable Added" SortExpression="IsRegulatoryReportingAdded" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRegulatoryReportingAdded" runat="server" Text='<%# Eval("IsRegulatoryReportingAdded") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Deactivated By" DataField="CM_DEACTIVATED_BY" SortExpression="CM_DEACTIVATED_BY" Visible="false" />
                                        <asp:BoundField HeaderText="Deactivated On" DataField="CM_DEACTIVATED_ON" DataFormatString="{0: dd-MMM-yyyy HH:mm:ss}" SortExpression="CM_DEACTIVATED_ON" Visible="false" />
                                        <asp:TemplateField HeaderText="Deactivation Remarks" SortExpression="CM_DEACTIVATION_REMARKS" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeactivationRemarks" runat="server" ToolTip='<%# replaceHTMLTagsFromLableTooltip(Eval("CM_DEACTIVATION_REMARKS").ToString()) %>'
                                                    Text='<%#Eval("CM_DEACTIVATION_REMARKS").ToString().Length > 200 ? (Eval("CM_DEACTIVATION_REMARKS") as string).Substring(0, 200).Replace("\n", "<br />") + " ..." : Eval("CM_DEACTIVATION_REMARKS").ToString().Replace("\n", "<br />") %>'></asp:Label>
                                                <asp:Label ID="lblDeactivationRemarks1" Visible="false" runat="server" Text='<%#Eval("CM_DEACTIVATION_REMARKS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Circular Creation Date" DataField="CM_CREAT_DT" DataFormatString="{0: dd-MMM-yyyy HH:mm:ss}" SortExpression="CM_CREAT_DT" Visible="false" />
                                        <asp:BoundField HeaderText="Circular Broadcast Date" DataField="CM_BROADCAST_DATE" DataFormatString="{0: dd-MMM-yyyy HH:mm:ss}" SortExpression="CM_BROADCAST_DATE" Visible="false" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
    <!-- end row -->

</asp:Content>
