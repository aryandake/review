<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false"
    Inherits="Fiction2Fact.Projects.Certification.ViewChecklistData" Title="View Checklist" EnableEventValidation="false" CodeBehind="ViewChecklistData.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ViewChecklist Details</title>
    <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css") %>" />
</head>
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
<script type="text/javascript" src='<%= Fiction2Fact.Global.site_url("Content/js/legacy/tabcontrol.js")%>'>
</script>

<script type="text/javascript">
    function onClientChanges() {
        var commentVal = document.getElementById("ctl00_ContentPlaceHolder1_txtComments").value;
        if (commentVal == '') {
            alert("Please enter comments for changes recommended.");
            var Comments = document.getElementById("ctl00_ContentPlaceHolder1_txtComments");
            if (Comments != null) {
                document.getElementById("ctl00_ContentPlaceHolder1_txtComments").focus();
            }
            return false;
        }
        else if (commentVal.length > 4000) {
            alert("Comments lenth can not be greater than 4000 characters.");
            document.getElementById("ctl00_ContentPlaceHolder1_txtComments").focus();
            return false;
        }
        if (commentVal != '') {
            if (!confirm('Are you sure that you want to recommend changes to this process?')) return false;

        }
    }
    function onApproveClick() {
        if (confirm('Are you sure that you want to mark the selected processes as "Review Complete"?')) {
            return true;
        } else {
            return false;
        }
    }
    function onUndoApproveClick() {
        if (confirm('Are you sure that you want to undo the approval for this process?')) {
            return true;
        } else {
            return false;
        }
    }


</script>

<body class="d-block">
    <form id="form1" runat="server">
        <center>
            <asp:HiddenField runat="server" ID="hftabhead33" />
            <asp:HiddenField runat="server" ID="hfTabberId" />
            <asp:HiddenField runat="server" ID="hfChecklistID" />
            <asp:HiddenField runat="server" ID="hfSource" />
            <asp:HiddenField runat="server" ID="hfType" />
            <asp:HiddenField runat="server" ID="hfRole" />
            <asp:HiddenField runat="server" ID="hfFrom" />
            <asp:HiddenField ID="hfName" runat="server" />
            <asp:HiddenField runat="server" ID="hfUnitId" />
            <asp:HiddenField runat="server" ID="hfUnitName" />
            <asp:HiddenField runat="server" ID="hfDesc" />

        </center>

        <!-- Page-Title -->
        <div class="page-content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="page-title-box">
                            <div class="row">
                                <div class="col">
                                    <h4 class="page-title">Compliance Checklist</h4>
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
                                <div class="tabular-view">
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Act/Regulation/Circular:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label ID="lblActRegCirc" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Reference Circular/Notification/Act:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label ID="lblSectionRegulation" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Section/Clause:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label ID="lblTitleOfSectionReq" runat="server"></asp:Label>
                                                <asp:Label ID="lblID" Visible="false" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Compliance of/Heading of Compliance checklist:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label ID="lblCheckPoint" runat="server"></asp:Label></label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>
                                                Description:
               
                                            </label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label ID="lblParticulars" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>
                                                Consequences of non Compliance:
               
                                            </label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label ID="lblPenalty" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>
                                                Frequency:
               
                                            </label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label ID="lblTimeLimit" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>
                                                Forms:
               
                                            </label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label ID="lblForms" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>Compliance Status:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label ID="lblComplianceStatus" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <%--<div class="col-md-3"><label>Remarks:--%>
                                        <div class="col-md-3">
                                            <label>
                                                Reason of non compliance:
               
                                            </label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label ID="lblRemarks" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>
                                                Target Date:
               
                                            </label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label ID="lblTargetDate" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0">
                                        <div class="col-md-3">
                                            <label>
                                                Action Plan:
               
                                            </label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label ID="lblActionPlan" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row g-0 border-bottom">
                                        <div class="col-md-3">
                                            <label>
                                                Checklist File:
               
                                            </label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>
                                                <asp:Label ID="lblChecklistFile" runat="server"></asp:Label>
                                                <asp:DataList ID="dlAttachedFiles" runat="server" CellPadding="1" EnableViewState="True"
                                                    RepeatColumns="1" RepeatDirection="Horizontal">
                                                    <ItemTemplate>
                                                        <a href="javascript:void(0);" onclick="javascript:window.open('../CommonDownloadAnyFile.aspx?type=ChecklistFilesFolder&downloadFileName=<%#Eval("CCD_SERVER_FILENAME")%>&fileName=<%#Eval("CCD_CLIENT_FILENAME")%>','','location=0,status=0,scrollbars=0,width=300,height=150');">
                                                            <%#Eval("CCD_CLIENT_FILENAME")%>
                            </a>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="mt-3">
                                    <asp:LinkButton CssClass="btn btn-outline-danger" ID="btnBack" runat="server" Text="Close" OnClientClick="window.close();" >
                                        <i class="fa fa-arrow-left me-2"></i> Close                   
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- end row -->
    </form>
</body>
</html>
