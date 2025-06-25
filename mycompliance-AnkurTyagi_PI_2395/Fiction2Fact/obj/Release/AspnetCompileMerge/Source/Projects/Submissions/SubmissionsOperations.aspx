<%@ Page Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Submissions.SubmissionsOperations" Title="Submissions Operations" CodeBehind="SubmissionsOperations.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Submissions Checklist Modifications</h4>
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
                    <div class="EditSubmissionsClass">
                        <b><a class="badge rounded-pill badge-soft-pink" href="ViewSubmissions.aspx?OpType=1">* Deactivating an existing checklist action
                        item:</a></b>
                    </div>
                    <div class="EditSubmissionsDetsClass">
                        If a compliance action item is no longer applicable from an effective date, use
                    this option and enter an effective date. This shall make this submission inactive
                    and all the entries in the Submissions Checklist, wherein the "Due Date To" is greater
                    than the effective date, shall be deleted.
                    </div>
                    <div class="EditSubmissionsClass mt-3">
                        <b><a class="badge rounded-pill badge-soft-pink" href="CommonSubmissionEditDelete.aspx">* Edit/Delete an existing checklist action
                        item:</a></b>
                    </div>
                    <div class="EditSubmissionsDetsClass">
                        This option is useful for editing/deleting the compliance action items. User can edit any 
                    detail of the compliance action item except the authority, Type (Fixed Date/Event Based)
                    and Frequency. Deletion shall be possible only in those cases, if no submissions have been
                    done against that action item.
                    
                    </div>
                    <%--<div class="EditSubmissionsClass">
                   <a href="SubmissionOwnerMapping.aspx">* Changing ownership of an existing checklist action item:</a>
                </div>
                <div class="EditSubmissionsDetsClass">
                    If a person is no longer associated with a compliance action item (due to inter-dept. transfer or resignation), use this option to change ownership of an action item
                    to some other person.
                    <br /><br />
                </div> --%>
                    <%--<div class="EditSubmissionsClass">
                   <b> <a href="ViewSubmissions.aspx?OpType=3">* Change details of an existing checklist action
                        item:</a></b>
                </div>
                <div class="EditSubmissionsDetsClass">
                    This option is useful for changing Particulars, Description, Priority, "To be Escalated",
                    "Escalation No. Of Days" etc. This shall update all the entries related to this
                    compliance wherein the "Due Date To" is greater than the effective date.
                    <br />
                    <br />
                </div>--%>
                    <%--<div class="EditSubmissionsClass">
                    <b><a href="ViewSubmissions.aspx?OpType=4">* Change due dates of an existing checklist
                        action item:</a></b>
                </div>
                <div class="EditSubmissionsDetsClass">
                    This option is useful for changing Due Dates for compliances, where submissions
                    have already been done or event instances have been created. This shall update all
                    the entries related to this compliance wherein the "Due Date To" is greater than
                    the effective date.
                    <br />
                    <br />
                </div>--%>
                    <%--<div class="EditSubmissionsClass">
                  <b>  <a href="../Submissions/SubmissionOwnerMapping.aspx">* Change Ownership of a particular
                        filing:</a></b>
                </div>
                <div class="EditSubmissionsDetsClass">
                    This option is useful for changing the ownership (both, of tracking department &
                    reporting department) of a particular filing.
                    <br />
                    <br />
                </div>--%>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->

     
</asp:Content>
