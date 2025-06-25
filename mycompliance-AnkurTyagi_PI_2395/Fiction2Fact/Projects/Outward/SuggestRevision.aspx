<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuggestRevision.aspx.cs" Inherits="Fiction2Fact.Projects.Outward.SuggestRevision" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Revision Suggested</title>
    <link id="Link2" rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/Allstyle.css")%>" />
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hfTabberId" />
  <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
    <asp:HiddenField runat="server" ID="hfOTId" />
      <div class="col-12">
            <div class="card">
                <div class="card-body">
					<ul class="nav nav-tabs" role="tablist">
                                    <li class="nav-item">
                                        <a class="nav-link active" data-bs-toggle="tab" href="#home" role="tab" aria-selected="true">Revision Suggested</a>
                                    </li>
                                   
                                </ul>
							 <div class="tab-content">
                                    <div class="tab-pane p-3 active" id="home" role="tabpanel">
                                        <div class="tabular-view">
											 <div class="row g-0">
                                                <div class="col-md-2">
                                                    <label>Remark: <span class="text-danger">*</span></label>
                                                </div>
                                                <div class="col-md-6">
													<asp:TextBox ID="txtremark" runat="server" CssClass="form-control" TextMode="MultiLine" style="height:100px;"></asp:TextBox>
													 <asp:RequiredFieldValidator ID="rfvremark" runat="server" ControlToValidate="txtremark" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="revision" SetFocusOnError="True" ErrorMessage="Please enter remark."></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="row g-0"><br /></div>
											 <div class="row g-0">
		                                        <div class="col-md-3">
			                                        
		                                        </div>
		                                        <div class="col-md-6">
			                                       
			                                             <asp:Button CssClass="btn btn-outline-success" ID="btnSave" runat="server" CausesValidation="TRUE"
                                    ValidationGroup="revision" Text="Submit" OnClick="btnSave_Click"/> 
													<asp:Button CssClass="btn btn-outline-danger" ID="btnback" runat="server" CausesValidation="false"
											 Text="Close"  OnClientClick="CloseWindow()" />
			                                        
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

