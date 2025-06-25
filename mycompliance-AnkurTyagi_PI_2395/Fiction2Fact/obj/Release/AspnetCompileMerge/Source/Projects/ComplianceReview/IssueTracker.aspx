<%@ Page Title="" Language="C#" MasterPageFile="~/Projects/Temp3.master" Async="true" AutoEventWireup="true" CodeBehind="IssueTracker.aspx.cs" Inherits="Fiction2Fact.Projects.Compliance.IssueTracker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function onAddIssueActionClick(Id) {
            window.open('AddEditActionPlan.aspx?RefId=' + Id + '&Type=ViewSent&Src=IT', '', 'location=0,status=0,scrollbars=1,resizable=1,width=900,height=900');
            return false;
        }

        function onViewIssueTrackerClick(RDIId) {
            window.open('ViewIssuesTracker.aspx?Id=' + RDIId, '', 'location=0,status=0,scrollbars=1,resizable=1,width=900,height=900');
            return false;
        }
    </script>
    <asp:Button ID="btnrefresh" runat="server" Text="Refresh" Style="display: none;" OnClick="btnrefresh_Click" />

    <asp:HiddenField runat="server" ID="hfCR_ID" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Issue Tracker </h4>
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
                        <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-outline-success" OnClientClick="return confirm('Are you sure want to submit all issues');" OnClick="btnSubmit_Click" >
                            <i class="fa fa-save me-2"></i> Submit                    
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnBack" runat="server" Text="Back" CssClass="btn btn-outline-danger" OnClick="btnBack_Click" >
                            <i class="fa fa-arrow-left me-2"></i> Back                   
                        </asp:LinkButton>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvComplianceReview" AllowPaging="False" AllowSorting="true" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found." EmptyDataRowStyle-BorderColor="transparent"
                            CssClass="table table-bordered footable" DataKeyNames="CI_ID" OnRowDataBound="gvComplianceReview_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Sr.No.
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex+1  %>' runat="server"></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="View" Visible="true">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Select" CommandArgument='<%# Eval("CI_ID") %>' 
                                                CssClass="btn btn-sm btn-soft-info btn-circle" OnClientClick='<%# string.Format("return onViewIssueTrackerClick(\"{0}\");", Eval("CI_ID")) %>'>
                                                <i class="fa fa-eye"></i>	                            
                                            </asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Provide Response" Visible="true">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lnkAddIssueTracker" runat="server" CommandName="Select" 
                                                CssClass="btn btn-sm btn-soft-primary btn-circle" OnClientClick='<%# string.Format("return onAddIssueActionClick(\"{0}\");", Eval("CI_ID")) %>'>
                                                <i class="fa fa-plus"></i>	
                                            </asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField HeaderText="Responsible Unit" DataField="CSFM_NAME" />
                                <asp:BoundField HeaderText="Issue Title" DataField="CI_ISSUE_TITLE" />
                                <asp:TemplateField HeaderText="Issue Description" ShowHeader="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIssueDesc" Width="200px" runat="server" ToolTip='<%# Eval("CI_ISSUE_DESC").ToString() %>'
                                            Text='<%#Eval("CI_ISSUE_DESC").ToString().Length>200?(Eval("CI_ISSUE_DESC") as string).Substring(0,200).Replace(Environment.NewLine, "<br />") + " ...":Eval("CI_ISSUE_DESC").ToString().Replace(Environment.NewLine, "<br />")  %>'></asp:Label>
                                        <asp:Label ID="lblIssueDesc1" Visible="false" runat="server" Text='<%#Eval("CI_ISSUE_DESC").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                        <asp:HiddenField ID="hfStatus" runat="server" Value='<%#Eval("CI_STATUS")%>' />
                                        <asp:HiddenField ID="hfCI_ID" runat="server" Value='<%#Eval("CI_ID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Issue Type" DataField="IssueType" />
                                <asp:BoundField HeaderText="Issue Status" DataField="IssueStatus" />

                                <asp:BoundField HeaderText="Status" DataField="DraftIssuesStatus" />

                                <asp:TemplateField HeaderText="Annexures(s)">
                                    <ItemTemplate>
                                        <asp:DataList ID="dlFiles" runat="server" RepeatColumns="1" RepeatDirection="Vertical"
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
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="mt-3">
                        <asp:LinkButton ID="btnSubmit1" runat="server" Text="Submit" CssClass="btn btn-outline-success" OnClientClick="return confirm('Are you sure want to submit all issues');" OnClick="btnSubmit_Click" >
                            <i class="fa fa-save me-2"></i> Submit    
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnBack1" runat="server" Text="Back" CssClass="btn btn-outline-danger" OnClick="btnBack_Click" >
                            <i class="fa fa-arrow-left me-2"></i> Back
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->

</asp:Content>
