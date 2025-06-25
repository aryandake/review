<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewDataRequirement.aspx.cs" Inherits="Fiction2Fact.Projects.Compliance.ViewDataRequirement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <asp:PlaceHolder runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />
        <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/bootstrap.bundle.min.js") %>"></script>

        <script type="text/javascript">
            if (window.opener == null || window.opener.location == null) {
                window.location = '<%= Fiction2Fact.Global.site_url() %>' + 'Login.aspx';
            }

            function onCloseClick() {
                window.close();
            }
        </script>

        <script type="text/javascript">

            /* Optional: Temporarily hide the "tabber" class so it does not "flash"
               on the page as plain HTML. After tabber runs, the class is changed
               to "tabberlive" and it will appear. */

            document.write('<style type="text/css">.tabber{display:none;}<\/style>');

            /*==================================================
              Set the tabber options (must do this before including tabber.js)
              ==================================================*/
            var tabberOptions = {
                'onLoad': function (argsObj) {
                    var t = argsObj.tabber;
                    var i;
                    i = document.getElementById('<%=hfTabberId.ClientID%>').value;
                    if (isNaN(i)) { return; }
                    t.tabShow(i);
                },

                'onClick': function (argsObj) {
                    var i = argsObj.index;
                    document.getElementById('<%=hfTabberId.ClientID%>').value = i;
                }
            };
        </script>
    </asp:PlaceHolder>
</head>
<body class="d-block">
    <form id="form1" runat="server">
        <div class="page-content">
            <div class="container-fluid">
                <asp:HiddenField runat="server" ID="hfTabberId" />
                <asp:HiddenField ID="hfDRId" runat="server" />
                <asp:HiddenField ID="hfSource" runat="server" />
                <asp:HiddenField ID="hfType" runat="server" />
                <asp:HiddenField ID="hfUser" runat="server" />
                <asp:HiddenField ID="hfUserType" runat="server" />

                <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
                </asp:ScriptManager>

                <!-- Page-Title -->
                <div class="row">
                    <div class="col-sm-12">
                        <div class="page-title-box">
                            <div class="row">
                                <div class="col">
                                    <h4 class="page-title">View Query</h4>
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
                                <!-- Nav tabs -->
                                <ul class="nav nav-tabs" role="tablist">
                                    <li class="nav-item">
                                        <a class="nav-link active" data-bs-toggle="tab" href="#home" role="tab" aria-selected="true">Basic Details</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" data-bs-toggle="tab" href="#profile" role="tab" aria-selected="false">Responses</a>
                                    </li>
                                </ul>

                                <!-- Tab panes -->
                                <div class="tab-content">
                                    <div class="tab-pane p-3 active" id="home" role="tabpanel">
                                        <div class="tabular-view">
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Type:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label CssClass="label2" ID="lblType" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Responsible Unit:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label CssClass="label2" ID="lblUnit" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Data Requirement / Query:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label CssClass="label2" ID="lblDRQ" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Person Responsible:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label CssClass="label2" ID="lblPersonRes" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Raised Date:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label CssClass="label2" ID="lblRaisedDate" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Due Date:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label CssClass="label2" ID="lblDueDate" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Status:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label CssClass="label2" ID="lblStatus" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Query pending with:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label CssClass="label2" ID="lblQueryPendingWith" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0 border-bottom">
                                                <div class="col-md-3">
                                                    <label>Ageing:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label CssClass="label2" ID="lblAgeing" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="mt-2">
                                            <div class="card mb-1 mt-1 border">
                                                <div class="card-header py-0 custom-ch-bg-color">
                                                    <h6 class="font-weight-bold text-white mtb-5">Attachment(s): </h6>
                                                </div>
                                                <div class="card-body mt-1">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gvAttachments" runat="server" AllowPaging="false" ShowFooter="false"
                                                            AllowSorting="false" BorderStyle="None" BorderWidth="1px" AutoGenerateColumns="false"
                                                            CssClass="table table-bordered footable" DataKeyNames="CRDF_ID" EmptyDataText="No records found...">
                                                            <Columns>
                                                                <asp:BoundField DataField="FileType" HeaderText="File Type" SortExpression="FileType" />
                                                                <asp:TemplateField HeaderText="File Description" ShowHeader="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFileDesc" runat="server" Text='<%#Eval("CRDF_DESC").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="File Name">
                                                                    <ItemTemplate>
                                                                        <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=CRDRQ&downloadFileName=<%#getFileName(Eval("CRDF_SERVER_FILE_NAME"))%>&fileName=<%#getFileName(Eval("CRDF_CLIENT_FILE_NAME"))%>','','location=0,status=0,scrollbars=1,width=400,height=200');">
                                                                            <%#Eval("CRDF_CLIENT_FILE_NAME")%>
                                                                </a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="tabular-view">
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Closed by:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label CssClass="label2" ID="lblClosedBy" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Closed on:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label CssClass="label2" ID="lblClosedOn" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="row g-0 border-bottom">
                                                <div class="col-md-3">
                                                    <label>Closure Remarks:</label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label CssClass="label2" ID="lblClosureRemarks" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane p-3" id="profile" role="tabpanel">
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
                                                                DataSource='<%# LoadDRQMResponseFileList(Eval("CRDU_ID")) %>'>
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
                                    </div>
                                </div>
                                <div class="mt-3">
                                    <asp:LinkButton CssClass="btn btn-outline-danger" runat="server" ID="btnClose"
                                        Text="Close" OnClientClick="return onCloseClick();" >
                                        <i class="fa fa-arrow-left me-2"></i> Close  
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
        </div>
                <!-- end row -->
        </div>
        </div>
    </form>
</body>
</html>
