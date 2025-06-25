<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddUpdates.aspx.cs" Inherits="Fiction2Fact.Projects.Outward.AddUpdates" %>

<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="custom" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Fiction2FactControls" Namespace="Fiction2FactControls.DropdownListNoValidation"
    TagPrefix="f2f" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <asp:PlaceHolder runat="server">
        <title>Add an Update</title>
        <link id="Link2" rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css")%>" />
        <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/DateValidator.js")%>"></script>

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
        <script>
            function addUpdates() {
                document.getElementById('<%=hfTabberId.ClientID%>').value = 1;
                var CaseId = 1;
                window.open("AddUpdates.aspx?Id=" + CaseId, "FILE",
                    "location=0,status=0,scrollbars=1,resizable=1,width=750,height=650");
                return false;
            }
            function CloseWindow() {
                window.close();
            }
            function ValidateDate_UpdateDate(source, arguments) {
                try {
                    //alert(source.id)
                    var control = "";
                    // alert(source.id);
                    if (source.id == "ctl00_ContentPlaceHolder1_cvUpdate") {
                        control = "txtupdateDate";
                    }

                    var txtDate = document.getElementById("txtupdateDate");
                    if (txtDate == null)
                        txtDate = document.getElementById(control);

                    var hfDateField = document.getElementById("hfCurDate");

                    if (hfDateField == null)
                        hfDateField = document.getElementById("hfCurDate");


                    if (compare2Dates(hfDateField, txtDate) > 1) {
                        //source.innerHTML = "Email sent date should be grater than System Date.";
                        arguments.IsValid = false;
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
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:HiddenField runat="server" ID="hfTabberId" />
        <asp:HiddenField runat="server" ID="hfCurDate" />
        <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
        <asp:HiddenField runat="server" ID="hfOTId" />
        <div class="col-12">
            <div class="card">
                <div class="card-body">
                    <ul class="nav nav-tabs" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link active" data-bs-toggle="tab" href="#home" role="tab" aria-selected="true">Add an Update</a>
                        </li>

                    </ul>
                    <div class="tab-content">
                        <div class="tab-pane p-3 active" id="home" role="tabpanel">
                            <div class="tabular-view">
                                <div class="row g-0">
                                    <div class="col-md-2">
                                        <label>Update Date: <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtupdateDate" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:ImageButton ID="ibUpDate" runat="server" AlternateText="Click to show calendar" CssClass="custom-calendar-icon"
                                                ImageUrl="../../Content/images/legacy/calendar.jpg" />
                                        </div>
                                        <asp:RegularExpressionValidator ID="revUpdatedate" runat="server" ControlToValidate="txtupdateDate" CssClass="text-danger"
                                            Display="Dynamic" ErrorMessage="Date Format has to be DD-MMM-YYYY." ValidationExpression="^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$"
                                            ValidationGroup="SEARCH"></asp:RegularExpressionValidator>
                                        <cc1:CalendarExtender ID="ceUpdateDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibUpDate"
                                            TargetControlID="txtupdateDate"></cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvUpdatedate" runat="server" ControlToValidate="txtupdateDate" CssClass="text-danger"
                                            Display="Dynamic" ValidationGroup="update" SetFocusOnError="True" ErrorMessage="Please select date."></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvUpdate" runat="server" ClientValidationFunction="ValidateDate_UpdateDate" CssClass="text-danger"
                                            ControlToValidate="txtupdateDate" ErrorMessage="Update date should be grater than System Date."
                                            Display="Dynamic" OnServerValidate="cvUpdate_ServerValidate" ValidationGroup="Save"></asp:CustomValidator>

                                    </div>
                                </div>

                                <div class="row g-0">
                                    <div class="col-md-2">
                                        <label>Remark: <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtremark" runat="server" CssClass="form-control" TextMode="MultiLine" Style="height: 100px;"></asp:TextBox>
                                        <%--//<< Added by ramesh more on 13-Mar-2024 CR_1991--%>
                                        <F2FControls:F2FFilteredTextarea runat="server" TargetControlID="txtremark" />
                                        <%--//>>--%>
                                        <asp:RequiredFieldValidator ID="rfvremark" runat="server" ControlToValidate="txtremark" CssClass="text-danger"
                                            Display="Dynamic" ValidationGroup="update" SetFocusOnError="True" ErrorMessage="Please enter remark."></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row g-0">
                                    <br />
                                </div>
                                <div class="row g-0">
                                    <div class="col-md-3">
                                    </div>
                                    <div class="col-md-6">

                                        <asp:Button CssClass="btn btn-outline-success" ID="btnSave" runat="server" CausesValidation="TRUE"
                                            ValidationGroup="update" Text="Submit" OnClick="btnSave_Click" />
                                        <asp:Button CssClass="btn btn-outline-danger" ID="btnback" runat="server" CausesValidation="false"
                                            Text="Close" OnClientClick="CloseWindow()" />

                                    </div>
                                </div>

                            </div>

                        </div>

                    </div>

                    <div class="row">
                        <center>
                        </center>
                    </div>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
