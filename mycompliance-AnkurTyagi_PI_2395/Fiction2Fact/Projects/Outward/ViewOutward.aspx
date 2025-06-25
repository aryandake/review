<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Outward.ViewOutward" Title="View Outward" CodeBehind="ViewOutward.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                "location=0,status=0,resizable=1,scrollbars=1,width=800,height=300");
            return false;
        }
    </script>
     <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">View Outward</h4>
                        <asp:Label ID="lblMsg" runat="server" CssClass="label" Visible="false"></asp:Label>
                    </div>
                    <!--end col-->
                </div>
                <!--end row-->
            </div>
            <!--end page-title-box-->
        </div>
        <!--end col-->
    </div>

	 <asp:HiddenField runat="server" ID="hfTabberId" />
    <asp:HiddenField runat="server" ID="hfoldId" />
    <asp:HiddenField runat="server" ID="hfOTId" />
      <div class="col-12">
            <div class="card">
                <div class="card-body">
					<ul class="nav nav-tabs" role="tablist">
                                    <li class="nav-item">
                                        <a class="nav-link active" data-bs-toggle="tab" href="#home" role="tab" aria-selected="true">Basic Details</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" data-bs-toggle="tab" href="#profile" role="tab" aria-selected="false">Add an update</a>
                                    </li>
                                    
                                </ul>
							 <div class="tab-content">
                                    <div class="tab-pane p-3 active" id="home" role="tabpanel">
										 
                                        <div class="tabular-view" style="border-bottom:1px solid #ddd;">
											 <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Outward No: </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                      <asp:Label ID="lblDocNumber" runat="server"></asp:Label>          
                                                    </label>
                                                </div>
                                            </div>
											 <div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label>  Status:    </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                          <asp:Label ID="lblStatus" runat="server"></asp:Label>            
			                                        </label>
		                                        </div>
	                                        </div>
                                            <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Type of Outward: </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                        <asp:Label ID="lbltypeofoutward" runat="server" ></asp:Label>  
                                                    </label>
                                                </div>
                                            </div>
                                             <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Department: </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                       <asp:Label ID="lblDept" runat="server"></asp:Label>  
                                                    </label>
                                                </div>
                                            </div>
											<div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Linked Outward: </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
														<asp:LinkButton ID="lnklinkedOutward" runat="server" OnClick="lnklinkedOutward_Click" style="text-decoration:underline;" >
															<asp:Label ID="lbllinkedOutward" style="color:cornflowerblue;" runat="server"></asp:Label>  
														</asp:LinkButton>
														
                                                    </label>
                                                </div>
                                            </div>
                                             <div class="row g-0">
                                                <div class="col-md-3">
                                                    <label>Subject: </label>
                                                </div>
                                                <div class="col-md-9">
                                                    <label>
                                                       <asp:Label ID="lblDocName" runat="server"></asp:Label>   
                                                    </label>
                                                </div>
                                            </div>
                                            
                                            <div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label>Document Dated: </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                            <asp:Label ID="lblOutwardDate" runat="server"></asp:Label>  
			                                        </label>
		                                        </div>
	                                        </div>
											<div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label>  Regulatory Authority:     </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lblAuthority" runat="server"></asp:Label>             
			                                        </label>
		                                        </div>
	                                        </div>
											<div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label>  Function Remarks:     </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lblremark" runat="server"></asp:Label>             
			                                        </label>
		                                        </div>
	                                        </div>
                                            <div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label>   To be send via:     </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lblSentTo" runat="server"></asp:Label>            
			                                        </label>
		                                        </div>
	                                        </div>
                                             <div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label>From (Sender):  </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                             <asp:Label ID="lblAddressor" runat="server"></asp:Label>     
			                                        </label>
		                                        </div>
	                                        </div>
                                            <div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label>To (Receiver):   </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                            <asp:Label ID="lblAddressee" runat="server"></asp:Label>       
			                                        </label>
		                                        </div>
	                                        </div>
                                             <%--<div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label>Subject:   </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                            <asp:Label ID="lblSubject" runat="server"></asp:Label>          
			                                        </label>
		                                        </div>
	                                        </div>--%>
                                             <div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label> Email sent date:    </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                            <asp:Label ID="lblEmailSentDate" runat="server"></asp:Label>            
			                                        </label>
		                                        </div>
	                                        </div>
                                             <div class="row g-0" style="display:none;">
		                                        <div class="col-md-3">
			                                        <label> Hard copy sent:    </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                           <asp:Label ID="lblHardCopySent" runat="server"></asp:Label>           
			                                        </label>
		                                        </div>
	                                        </div>
                                               <div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label> Courier Ref No:    </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                             <asp:Label ID="lblPODNumber" runat="server"></asp:Label>            
			                                        </label>
		                                        </div>
	                                        </div>
                                            <div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label> Courier Name:    </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                            <asp:Label ID="lblCourierName" runat="server"></asp:Label>            
			                                        </label>
		                                        </div>
	                                        </div>
                                             <div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label> Courier Sent date:     </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                            <asp:Label ID="lblCourierSentDt" runat="server"></asp:Label>             
			                                        </label>
		                                        </div>
	                                        </div>
                                             <%-- <div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label> Representation:      </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                           <asp:Label ID="lblRepresentation" runat="server"></asp:Label>             
			                                        </label>
		                                        </div>
	                                        </div>--%>
                                             <div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label> Status of Representation:      </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                          <asp:Label ID="lblRepresentationStatus" runat="server"></asp:Label>               
			                                        </label>
		                                        </div>
	                                        </div>
                                            <div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label>  Closure Date of Representation:     </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lblRepresentationDt" runat="server"></asp:Label>              
			                                        </label>
		                                        </div>
	                                        </div>
                                             
                                            <div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label>   Documents attached:    </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label> 
			                                         <asp:GridView ID="gvFileUpload" runat="server" BorderStyle="None" BorderWidth="1px" Width="100%"
                                                        CellPadding="4" GridLines="None" AutoGenerateColumns="False" DataKeyNames="OF_ID"
                                                        CssClass="table table-bordered footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                                        <Columns>
                                                            <asp:TemplateField Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServerFileName" Text='<%#Eval("OF_FILE_NAME") %>' runat="server"> </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="File Name">
                                                                <ItemTemplate>
                                                                    <a href="Javascript:void(0);" onclick="javascript:window.open('../CommonDownloadAnyFile.aspx?type=Outward&downloadFileName=<%# Eval("OF_FILE_NAME_ON_SERVER")%>&fileName=<%#Eval("OF_FILE_NAME")%>','','location=0,status=0,scrollbars=0,width=10,height=10');">
                                                                        <%#Eval("OF_FILE_NAME")%>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Uploaded By">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUploaderName" runat="server" Text='<%# Eval("OF_CREATE_BY") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Uploaded On">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCreateDate" runat="server" Text='<%# Eval("OF_CREATE_DATE", "{0:dd-MMM-yyyy HH:mm:ss}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>           
			                                        </label>
		                                        </div>
	                                        </div>
                                            <div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label> Closure Remarks:    </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                          <asp:Label ID="lblclosureRemarks" runat="server"></asp:Label>            
			                                        </label>
		                                        </div>
	                                        </div>
											<div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label>   Closure By:      </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lblclosedBy" runat="server"></asp:Label>          
			                                        </label>
		                                        </div>
	                                        </div>
											<div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label>   Closure On:      </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lblclosedOn" runat="server"></asp:Label>          
			                                        </label>
		                                        </div>
	                                        </div>
                                             <div class="row g-0" >
		                                        <div class="col-md-3">
			                                        <label>   Cancellation Remarks:   </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lblCancelRemarks" runat="server" Text=""></asp:Label>         
			                                        </label>
		                                        </div>
	                                        </div>
											 <div class="row g-0" >
		                                        <div class="col-md-3">
			                                        <label>   Cancellation By:   </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lblcancelBy" runat="server" Text=""></asp:Label>         
			                                        </label>
		                                        </div>
	                                        </div>
											 <div class="row g-0" >
		                                        <div class="col-md-3">
			                                        <label>   Cancellation On:   </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lblcancelOn" runat="server" Text=""></asp:Label>         
			                                        </label>
		                                        </div>
	                                        </div>
                                              <div class="row g-0" >
		                                        <div class="col-md-3">
			                                        <label>   Suggest Revision Remarks:   </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lblsuggestRevision" runat="server" Text=""></asp:Label>         
			                                        </label>
		                                        </div>
	                                        </div>
											 <div class="row g-0" >
		                                        <div class="col-md-3">
			                                        <label>   Suggest Revision By:   </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lblsuggestRevisionBy" runat="server" Text=""></asp:Label>         
			                                        </label>
		                                        </div>
	                                        </div>
											<div class="row g-0" >
		                                        <div class="col-md-3">
			                                        <label>   Suggest Revision On:   </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lblsuggestRevisionOn" runat="server" Text=""></asp:Label>         
			                                        </label>
		                                        </div>
	                                        </div>

											 <div class="row g-0" >
		                                        <div class="col-md-3">
			                                        <label>   Deletion Remarks:   </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lbldeleteRemark" runat="server" Text=""></asp:Label>         
			                                        </label>
		                                        </div>
	                                        </div>
											 <div class="row g-0" >
		                                        <div class="col-md-3">
			                                        <label>   Deletion By:   </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lbldeleteBy" runat="server" Text=""></asp:Label>         
			                                        </label>
		                                        </div>
	                                        </div>
											<div class="row g-0" >
		                                        <div class="col-md-3">
			                                        <label>   Deletion On:   </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lbldeleteOn" runat="server" Text=""></asp:Label>         
			                                        </label>
		                                        </div>
	                                        </div>

											 <div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label>   Created By:      </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lblCreatedBy" runat="server"></asp:Label>          
			                                        </label>
		                                        </div>
	                                        </div>
											<div class="row g-0">
		                                        <div class="col-md-3">
			                                        <label>   Created On:      </label>
		                                        </div>
		                                        <div class="col-md-9">
			                                        <label>
			                                         <asp:Label ID="lblCreatedOn" runat="server"></asp:Label>          
			                                        </label>
		                                        </div>
	                                        </div>
                                            </div>
										<br />
										<asp:Button CssClass="btn btn-outline-danger" ID="BtnBack" runat="server" CausesValidation="false"
											 Text="Back" OnClick="BtnBack_Click" />
										
                                        </div>
								 <div class="tab-pane p-3" id="profile" role="tabpanel">
									  <asp:Button CssClass="btn btn-outline-success" ID="btnaddUpdate" runat="server" CausesValidation="false" Visible="false"
											Text="Add an Update" OnClick="btnaddUpdate_Click"  />
									 <asp:Button CssClass="btn btn-outline-danger" ID="Button1" runat="server" CausesValidation="false"
											 Text="Back" OnClick="BtnBack_Click" />
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvUpdates" runat="server" Visible="false" CssClass="table table-bordered footable" GridLines="Both" AllowPaging="false" style="margin-top:10px;"
												EmptyDataText="No record found." ShowHeaderWhenEmpty="true" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr.No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
													
													<asp:TemplateField HeaderText="Outward Id">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbloutwardNumber" runat="server" Text='<%# Eval("OTU_OT_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
													<asp:TemplateField HeaderText="Update Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblupdateDate" runat="server" Text='<%# Eval("OTU_DATE", "{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblRemarks" Text='<%# Eval("OTU_REMARK").ToString().Replace(Environment.NewLine, "<br />")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
													<asp:TemplateField HeaderText="Created By">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblcreatedBy" runat="server" Text='<%# Eval("OTU_CREATED_BY") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
													<asp:TemplateField HeaderText="Created Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblcreatedDate" runat="server" Text='<%# Eval("OTU_CREATED_DT", "{0:dd-MMM-yyyy HH:mm;ss }") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    
                                                </Columns>

                                            </asp:GridView>
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

</asp:Content>
