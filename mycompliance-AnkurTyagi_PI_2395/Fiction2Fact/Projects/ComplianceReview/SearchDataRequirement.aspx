<%@ Page Title="" Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true" CodeBehind="SearchDataRequirement.aspx.cs" Inherits="Fiction2Fact.Projects.Compliance.SearchDataRequirement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">

    <script>
        function onAddRESClick(Id, Source) {
            window.open('AddResponse.aspx?DRId=' + Id + '&Type=Add&Src=' + Source + '&Source=RES&User=US', '', 'location=0,status=0,scrollbars=1,resizable=1,width=900,height=900');
            return false;
        }
        function onViewRESClick(Id, Source) {
            window.open('AddResponse.aspx?DRId=' + Id + '&Type=View&Src=' + Source + '&User=US', '', 'location=0,status=0,scrollbars=1,resizable=1,width=900,height=500');
            return false;
        }
        function onClientViewClick() {
            document.getElementById('<%=hfSelectedOperation.ClientID%>').value = "View";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField runat="server" ID="hfType" />
    <asp:HiddenField runat="server" ID="hfSrcType" />
    <asp:HiddenField runat="server" ID="hfSource" />
    <asp:HiddenField runat="server" ID="hfSrc" />
    <asp:HiddenField runat="server" ID="hfRefId" />
    <asp:HiddenField runat="server" ID="hfDRId" />
    <asp:HiddenField runat="server" ID="hfClickCounter" />
    <asp:HiddenField runat="server" ID="hfModalClosure" />
    <asp:HiddenField runat="server" ID="hfDoubleClickFlag" />
    <asp:Literal ID="CheckBoxIDsArray" runat="server"></asp:Literal>
    <asp:HiddenField ID="hfSelectedOperation" runat="server" />

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Search Query Tracker</h4>
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
                    <div class="table-responsive">
                        <asp:GridView ID="gvDRQM" runat="server" AutoGenerateColumns="False" DataKeyNames="CDQ_ID"
                            AllowSorting="false" AllowPaging="false" GridLines="Both" CellPadding="4"
                            CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            EmptyDataText="No record found..." OnRowDataBound="gvDRQM_RowDataBound"
                            OnSelectedIndexChanged="gvDRQM_SelectedIndexChanged">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="HeaderLevelCheckBox" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="RowLevelCheckBox" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sr.No." ControlStyle-Width="20px" HeaderStyle-Width="20px">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Response" >
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lnkViewRes" runat="server" CommandName="Select"                                                 
                                                CssClass="btn btn-sm btn-soft-info btn-circle" OnClientClick='<%# string.Format("return onViewRESClick(\"{0}\",\"{1}\");", Eval("CDQ_ID"), (string.IsNullOrEmpty(hfSource.Value) ? hfSrc.Value : hfSource.Value)) %>'>
                                                <i class="fa fa-eye"></i>	                            
                                            </asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Add Response">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lnkAddRes" runat="server" CommandName="Select" 
                                                CssClass="btn btn-sm btn-soft-primary btn-circle" OnClientClick='<%# string.Format("return onAddRESClick(\"{0}\",\"{1}\");", Eval("CDQ_ID"), (string.IsNullOrEmpty(hfSource.Value) ? hfSrc.Value : hfSource.Value)) %>'>
                                                <i class="fa fa-plus"></i>
                                            </asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Closure Remarks" Visible="false">
                                    <ItemTemplate>
                                        <div id="basic-modal">
                                            <center>
                                                <asp:LinkButton ID="lnkAddClosure" runat="server" CommandName="Select" 
                                                    CssClass="btn btn-sm btn-soft-primary btn-circle" OnClientClick='<%# string.Format("openModal(\"{0}\");", Eval("CDQ_ID")) %>'>
                                                    <i class="fa fa-plus"></i>
                                                </asp:LinkButton>
                                            </center>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Source" DataField="RC_NAME" Visible="false" />
                                <asp:TemplateField HeaderText="Source Identifier">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lnkViewSource" runat="server" CommandName="Select" Text='<%#Eval("Identifier") %>'
                                                CssClass="badge rounded-pill badge-soft-pink" 
                                                OnClientClick='<%# string.Format("return onClientViewClick();") %>'>
                                            </asp:LinkButton>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("CDQ_STATUS") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblSourceId" runat="server" Text='<%#Eval("CDQ_SOURCE_ID") %>' Visible="false"></asp:Label>
                                            <asp:HiddenField runat="server" ID="hfIsResponseDrafted" Value='<%#Eval("CDQ_IS_RESPONSE_DRAFTED") %>' />
                                            <asp:HiddenField runat="server" ID="hfDRId" Value='<%#Eval("CDQ_ID") %>' />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Source Process Name" DataField="Process" />
                                <asp:BoundField HeaderText="Responsible Unit" DataField="CSFM_NAME" />
                                <asp:TemplateField HeaderText="Data Requirement / Query" ShowHeader="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDRQM" Width="200px" runat="server" ToolTip='<%# Eval("CDQ_QUERY_DATA_REQUIREMENT").ToString() %>'
                                            Text='<%#Eval("CDQ_QUERY_DATA_REQUIREMENT").ToString().Length>200?(Eval("CDQ_QUERY_DATA_REQUIREMENT") as string).Substring(0,200).Replace(Environment.NewLine, "<br />") + " ...":Eval("CDQ_QUERY_DATA_REQUIREMENT").ToString().Replace(Environment.NewLine, "<br />")  %>'></asp:Label>
                                        <asp:Label ID="lblDRQM1" Visible="false" runat="server" Text='<%#Eval("CDQ_QUERY_DATA_REQUIREMENT").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Person Responsible" DataField="CDQ_PERSON_RESPONSIBLE" ControlStyle-Width="100px" HeaderStyle-Width="100px" />

                                <asp:BoundField HeaderText="Raised Date" DataField="CDQ_RAISED_DT" DataFormatString="{0: dd-MMM-yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Due Date" DataField="CDQ_EXPIRY_DT" DataFormatString="{0: dd-MMM-yyyy}" />
                                <asp:BoundField HeaderText="Status" DataField="Status" ControlStyle-Width="40px" HeaderStyle-Width="40px" />
                                <asp:BoundField HeaderText="Query pending with" DataField="Query pending with" />
                                <asp:TemplateField HeaderText="Ageing" ShowHeader="true">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblAgeing" runat="server" Text='<%#Eval("Ageing") %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Attachment(s)" ControlStyle-Width="100px" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:DataList ID="dlEvidenceFiles" runat="server" RepeatColumns="1" RepeatDirection="vertical"
                                            DataSource='<%# LoadDRQMFileList(Eval("CDQ_ID")) %>'>
                                            <ItemTemplate>
                                                <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownload.aspx?type=CRDRQ&downloadFileName=<%#getFileName(Eval("CRDF_SERVER_FILE_NAME"))%>&fileName=<%#getFileName(Eval("CRDF_CLIENT_FILE_NAME"))%>','','location=0,status=0,scrollbars=1,width=400,height=200');">
                                                    <%#Eval("CRDF_CLIENT_FILE_NAME")%>
                                            </a>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Added by" DataField="CDQ_CREATE_BY" ControlStyle-Width="40px" HeaderStyle-Width="40px" />
                                <asp:BoundField HeaderText="Added on" DataField="CDQ_CREATE_DT" DataFormatString="{0: dd-MMM-yyyy HH:mm:ss}" ControlStyle-Width="80px" HeaderStyle-Width="80px" />
                                <asp:BoundField HeaderText="Closed by" DataField="CDQ_CLOSED_BY" ControlStyle-Width="40px" HeaderStyle-Width="40px" />
                                <asp:BoundField HeaderText="Closed on" DataField="CDQ_CLOSED_ON" DataFormatString="{0: dd-MMM-yyyy HH:mm:ss}" ControlStyle-Width="80px" HeaderStyle-Width="80px" />
                                <asp:TemplateField HeaderText="Closure Remarks" ShowHeader="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosureRem" Width="200px" runat="server" ToolTip='<%# Eval("CDQ_CLOSURE_REMARKS").ToString() %>'
                                            Text='<%#Eval("CDQ_CLOSURE_REMARKS").ToString().Length>200?(Eval("CDQ_CLOSURE_REMARKS") as string).Substring(0,200).Replace(Environment.NewLine, "<br />") + " ...":Eval("CDQ_CLOSURE_REMARKS").ToString().Replace(Environment.NewLine, "<br />")  %>'></asp:Label>
                                        <asp:Label ID="lblClosureRem1" Visible="false" runat="server" Text='<%#Eval("CDQ_CLOSURE_REMARKS").ToString().Replace("\n", "<br />")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->


</asp:Content>
