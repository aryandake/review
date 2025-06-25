<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" CodeBehind="ConfigParams.aspx.cs"
    Inherits="Fiction2Fact.Projects.Admin.ConfigParams" Title="Config Parameters" %>

<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .hiddn {
            visibility: hidden
        }
    </style>
    <script type="text/javascript">
        function printElement(el) {
            if (typeof el === 'string') {
                el = document.getElementById(el);
                if (!el) {
                    alert('Element doesnt exists');
                    return false;
                }
            }
            if (!el) {
                alert('Element doesnt exists');
                return false;
            }
            var content = document.getElementById('printarea').innerHTML;
            var shtm = "<html><head><title>Print</title><link id=\"Link1\" rel=\"stylesheet\" type=\"text/css\" href=\"../css/style.css\" /></head><body>" + content + "</body></html>";
            var w = window.open("", "", "width=1024,height=768");
            w.document.write(shtm);
            w.document.close();
            w.focus();
            w.print();
            w.close();
        }

        function printData() {
            printElement('printarea');
            return false;
        }
    </script>


    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">
                            <asp:Label ID="lblHeader" runat="server" Text="Configuration Parameters"></asp:Label></h4>
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
                    <div class="text-center">
                        <asp:LinkButton runat="server" ID="imgExcel" OnClick="btnExportToExcel_Click" CssClass="btn btn-outline-secondary"
                            ToolTip="Export to Excel">
                            <i class="fa fa-download"></i> Export to Excel 
                        </asp:LinkButton>
                    </div>
                    <div class="mt-3">
                        <div id="printarea" class="table-responsive">
                            <asp:GridView ID="gvConfigueParams" runat="server" AutoGenerateColumns="False" DataKeyNames="CP_ID"
                                OnSelectedIndexChanged="gvConfigueParams_SelectedIndexChanged" CellPadding="4"
                                CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("CP_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Value">
                                        <ItemTemplate>
                                            <F2FControls:F2FTextBox CssClass="form-control" ID="txtValue" runat="server" Text='<%# Eval("CP_VALUE") %>' Columns="50" TextMode="MultiLine"></F2FControls:F2FTextBox>
                                            <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtValue" />
                                            <asp:Label ID="lblprms" runat="server" Text='<%# Eval("CP_VALUE") %>' CssClass="hiddn"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Save" ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbSave" runat="server" CssClass="btn btn-outline-success" CommandName="Select" Text="Save">
                                                <i class="fa fa-save me-2"></i> Save                    
                                    </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->

</asp:Content>
