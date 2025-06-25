<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Submissions.ViewSubmissions" Title="Common Submission Edit Delete" CodeBehind="ViewSubmissions.aspx.cs" %>

<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="custom" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="a" Namespace="Fiction2Fact.Controls" Assembly="RequiredIfValidatorRadioButtonList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hfOpType" runat="server" />
    <asp:HiddenField ID="hfUserType" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">View Submissions</h4>
                        <asp:Label ID="lblInfo" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
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
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Frequency:</label>
                                    <asp:DropDownList ID="ddlFrequency" runat="server" CssClass="form-select">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                        <asp:ListItem Value="Only Once">Adhoc</asp:ListItem>
                                        <asp:ListItem Text="Daily" Value="Daily">Daily</asp:ListItem>
                                        <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                                        <asp:ListItem Value="Fortnightly">Fortnightly</asp:ListItem>
                                        <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                        <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>
                                        <asp:ListItem Value="Half Yearly">Half Yearly</asp:ListItem>
                                        <asp:ListItem Value="Yearly">Yearly</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Reporting to:</label>
                                    <asp:DropDownList ID="ddlSegment" runat="server" CssClass="form-select" DataValueField="SSM_ID"
                                        DataTextField="SSM_NAME" AppendDataBoundItems="True">
                                        <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Type:</label>
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                        <asp:ListItem Value="F">Fixed Date</asp:ListItem>
                                        <asp:ListItem Value="E">Event Based</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Tracked By:</label>
                                    <asp:DropDownList ID="ddlSubType" CssClass="form-select" AppendDataBoundItems="true"
                                        runat="server" DataValueField="STM_ID" DataTextField="STM_TYPE">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Depends On Event:</label>
                                    <asp:DropDownList ID="ddlEventForSearch" CssClass="form-select" AutoPostBack="true"
                                        AppendDataBoundItems="true" runat="server" OnSelectedIndexChanged="ddlEventForSearch_SelectedIndexChanged"
                                        DataValueField="EM_ID" DataTextField="EM_EVENT_NAME">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-12 mb-3">
                                    <label class="form-label">Select Agendas:</label>
                                    <div class="custom-checkbox-table">
                                        <asp:CheckBoxList ID="cblAssociatedWith" RepeatColumns="5" CssClass="form-control" runat="server" DataTextField="EP_NAME"
                                            DataValueField="EP_ID" AppendDataBoundItems="true">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Status:</label>
                                    <asp:DropDownList ID="ddlStatus" CssClass="form-select" runat="server">
                                        <asp:ListItem Value="Active">Active</asp:ListItem>
                                        <asp:ListItem Value="Inactive">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Reporting Function:</label>
                                    <asp:DropDownList ID="ddlReportDept" CssClass="form-select" AppendDataBoundItems="true"
                                        runat="server" DataValueField="SRD_ID" DataTextField="SRD_NAME">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="text-center mt-3">
                                <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnSearch" runat="server" Text="Search" ValidationGroup="SEARCH"
                                    AccessKey="s" OnClick="btnSearch_Click">
                                    <i class="fa fa-search"></i> Search                     
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExportToExcel" Visible="false" runat="server" CssClass="btn btn-outline-secondary"
                                    Text="Export to Excel" OnClick="btnExportToExcel_Click">
                                    <i class="fa fa-download"></i> Export to Excel               
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gvSubmissionMaster" runat="server" AutoGenerateColumns="False"
                                    OnSorting="gvSubmissionMaster_Sorting" DataKeyNames="SM_ID" AllowSorting="true"
                                    AllowPaging="true" GridLines="both" CellPadding="4" OnRowDataBound="gvSubmissionMaster_RowDataBound"
                                    OnPageIndexChanging="gvSubmissionMaster_PageIndexChanging" CssClass="table table-bordered footable"
                                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
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
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label Visible="false" ID="lbFrequency" Text='<%# Eval("SM_FREQUENCY") %>' runat="server"> 
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label Visible="false" ID="lbType" Text='<%# Eval("SM_SUB_TYPE") %>' runat="server"></asp:Label>
                                                <asp:Label Visible="false" ID="lbSmId" Text='<%# Eval("SM_ID") %>' runat="server"> 
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label Visible="false" ID="lbEPId" Text='<%# Eval("SM_EP_ID") %>' runat="server"> 
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label Visible="false" ID="lbEmId" Text='<%# Eval("SM_EM_ID") %>' runat="server"> 
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reporting to">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Width="75px" ID="lblSegment" Text='<%#LoadSubmissionSegmentName(Eval("SM_ID"))%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fixed Date/Event Based" SortExpression="SM_SUB_TYPE">
                                            <ItemTemplate>
                                                <asp:Label ID="lbFixedEventType" Text='<%# Eval("SM_SUB_TYPE") %>' runat="server"> 
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Effective From" SortExpression="SM_EFFECTIVE_DT">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Width="75px" ID="lblEffDate" Text='<%# Eval("SM_EFFECTIVE_DT" , "{0:dd-MMM-yyyy}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="SRD_NAME" HeaderText="Reporting Function" SortExpression="SRD_NAME" />
                                        <%--<asp:BoundField DataField="SM_PERTICULARS" HeaderText="Particulars" SortExpression="SM_PERTICULARS" />--%>
                                        <asp:TemplateField HeaderText="Particulars" SortExpression="SM_PERTICULARS">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParticulars" Text='<%#Eval("SM_PERTICULARS").ToString().Replace("\n","<br />") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="SM_FREQUENCY" HeaderText="Frequency" SortExpression="SM_FREQUENCY" />
                                        <asp:TemplateField HeaderText="Deactivation Remarks" SortExpression="SM_DEACTIVATION_REMARKS">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSM_DEACTIVATION_REMARKS" Text='<%#Eval("SM_DEACTIVATION_REMARKS").ToString().Replace("\n","<br />") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Deactivated By" SortExpression="SM_DEACTIVATION_DONE_BY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeactivatedby" Text='<%# Getfullname(Eval("SM_DEACTIVATION_DONE_BY").ToString())%>'
                                                    runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Deactivated On" SortExpression="SM_DEACTIVATION_DONE_ON">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeactivateddOn" Text='<%#Eval("SM_DEACTIVATION_DONE_ON","{0:dd-MMM-yyyy HH:mm:ss}")%>'
                                                    runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ShowHeader="true" HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CssClass="btn btn-sm btn-soft-success btn-circle">
                                                    <i class="fa fa-pen"></i>	                            
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
