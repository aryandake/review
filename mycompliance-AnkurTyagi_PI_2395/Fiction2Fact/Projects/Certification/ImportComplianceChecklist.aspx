<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Certification.ImportComplianceChecklist" CodeBehind="ImportComplianceChecklist.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Upload Checklist</title>

    <asp:PlaceHolder runat="server">
        <link id="Link2" rel="stylesheet" type="text/css" href="<%= Fiction2Fact.Global.site_url("Content/main.css")%>" />
        <link id="Link3" rel="stylesheet" type="text/css" href="<%= Fiction2Fact.Global.site_url("Content/controlStyle.css")%>" />
        <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />

        <script src="<%=Fiction2Fact.Global.site_url("Content/assets/js/jquery.min.js") %>"></script>
        <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Scripts/jquery.js") %>"></script>
        <script>
            function onCloseClick() {
                window.close();
                window.opener.location.reload();
            }
        </script>
    </asp:PlaceHolder>

    <style>
        .thwidth {
            width: 200px !important;
            color: red
        }

        .lblwidth {
            width: 200px !important;
        }
    </style>
</head>
<body class="bodyCSS">

    <form id="form1" runat="server">
        <div class="page-wrapper" style="margin-left: 0 !important; width: 1280px">
            <div class="page-content">
                <div class="container-fluid">
                    <asp:HiddenField ID="hfType" runat="server" />
                    <asp:HiddenField ID="hfDate" runat="server" />
                    <asp:HiddenField ID="hfBatchId" runat="server" />
                    <asp:HiddenField ID="hfUserName" runat="server" />
                    <asp:HiddenField ID="hfAuditIssueId" runat="server" />


                    <div class="row">
                        <div class="col-sm-12">
                            <div class="page-title-box">
                                <div class="row">
                                    <div class="col">
                                        <h4 class="page-title">Compliance Checklist Import</h4>

                                        <asp:Label ID="lblMsg" runat="server" CssClass="custom-alert-box"></asp:Label>
                                    </div>
                                    <!--end col-->
                                </div>
                                <!--end row-->
                            </div>
                            <!--end page-title-box-->
                        </div>
                        <!--end col-->
                    </div>


                    <div class="row">
                        <div class="col-12">
                            <div class="card">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-12 mb-3">
                                            <asp:Label ID="Label1" runat="server" Text="<strong> Note: </strong>"
                                                CssClass=""></asp:Label>

                                            <div class="input-group">
                                                <i class="fa fa-arrow-alt-circle-right me-2"></i>
                                                <span>To import a compliance checklist, click on choose file button,select the file and click on the Upload button.</span>
                                            </div>
                                            <div class="input-group">
                                                <i class="fa fa-arrow-alt-circle-right me-2"></i>
                                                <span>Checklist upload file should be in .xlsx format only.</span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12 mb-3">
                                        <label class="form-label">
                                            <asp:Label ID="lblAttachments" runat="server" Text="Select File:"></asp:Label></label>
                                        <div class="input-group">
                                            <asp:FileUpload ID="fuCheckData" runat="server" CssClass="form-control" Width="500px" />

                                            <asp:RegularExpressionValidator ID="revPGRCFiles" runat="server" ControlToValidate="fuCheckData"
                                                Display="Dynamic" ErrorMessage="Upload of this file format is not supported."
                                                ValidationGroup="upload" ValidationExpression="^.+(.xlsx|.XLSX)$"></asp:RegularExpressionValidator>

                                            <asp:Button ID="btnUpload" runat="server" CssClass="html_button" Text="Validate"
                                                ValidationGroup="upload" OnClick="btnUpload_Click" CausesValidation="true" />
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-12 mb-3 table-responsive">
                                            <asp:GridView ID="gvData" runat="server" AllowPaging="false" ShowFooter="false"
                                                AllowSorting="false" BorderStyle="None" BorderWidth="1px" AutoGenerateColumns="false"
                                                CssClass="table table-bordered footable">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            Sr.No.
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Error Message" ItemStyle-Width="150px" ItemStyle-CssClass="thwidth">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblError" runat="server" CssClass="lblwidth" Text='<%# Eval("ERROR_MESSAGE").ToString().Replace(Environment.NewLine, "<br />") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ChecklistDetsId" HeaderText="ChecklistDetsId" />
                                                    <asp:BoundField DataField="ChecklistMasId" HeaderText="ChecklistMasId" />
                                                    <asp:TemplateField HeaderText="Act/Regulation/Circular">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAct" runat="server" Text='<%# Eval("Act/Regulation/Circular").ToString().Replace(Environment.NewLine, "<br />") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reference Circular/Notification/Act">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Reference Circular/Notification/Act").ToString().Replace(Environment.NewLine, "<br />") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Compliance of/Heading of Compliance checklist">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCompliance" runat="server" Text='<%# Eval("Compliance of/Heading of Compliance checklist").ToString().Replace(Environment.NewLine, "<br />") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Description">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description").ToString().Replace(Environment.NewLine, "<br />") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Consequences of non Compliance">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblConsequences" runat="server" Text='<%# Eval("Consequences of non Compliance").ToString().Replace(Environment.NewLine, "<br />") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Frequency" HeaderText="Frequency" />
                                                    <asp:BoundField DataField="Forms" HeaderText="Forms" />
                                                    <asp:BoundField DataField="Compliance Status" HeaderText="Compliance Status" />
                                                    <asp:TemplateField HeaderText="Remarks / Reason of non compliance">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks / Reason of non compliance").ToString().Replace(Environment.NewLine, "<br />") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Non-compliant since">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNoncompliant" runat="server" Text='<%# Eval("Non-compliant since","{0:dd-MMM-yyyy}").ToString()%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Action Plan">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAction" runat="server" Text='<%# Eval("Action Plan").ToString().Replace(Environment.NewLine, "<br />") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Target Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTargetDate" runat="server" Text='<%# Eval("Target Date","{0:dd-MMM-yyyy}").ToString()%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="Checklist File" HeaderText="Checklist File" />--%>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div class="col-md-12 mb-3 text-center">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" Visible="false" OnClick="btnSave_Click"
                                                CssClass="html_button" />
                                            <asp:Button ID="btnClose" runat="server" Visible="true" CssClass="html_button" Text="Close" OnClientClick="return onCloseClick();" />

                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
