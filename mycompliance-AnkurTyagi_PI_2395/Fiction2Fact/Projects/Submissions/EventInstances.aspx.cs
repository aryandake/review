using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using Fiction2Fact.App_Code;
using System.Globalization;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class Admin_EventInstances : System.Web.UI.Page
    {
        UtilitiesBLL utilityBL = new UtilitiesBLL();
        string mstrConnectionString = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            ddlEvent.Focus();
            if (!Page.IsPostBack)
            {
                hfCurrDate.Value = System.DateTime.Now.ToString("dd-MMM-yyyy");
                ddlEvent.DataSource = utilityBL.getDataset("EVENT", mstrConnectionString);
                ddlEvent.DataBind();
                //ddlCompany.DataSource = utilityBL.getDataset("COMPANY", mstrConnectionString);
                //ddlCompany.DataBind();

                Session["EventInstances"] = Server.UrlEncode(System.DateTime.Now.ToString());
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["EventInstances"] = Session["EventInstances"];
        }

        protected void ddlEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            cblAssociatedWith.Items.Clear();
            if (!ddlEvent.SelectedValue.Equals(""))
            {
                cblAssociatedWith.DataSource = utilityBL.getDatasetWithCondition("EVENTPURPOSE", Convert.ToInt32(ddlEvent.SelectedValue), mstrConnectionString);
                cblAssociatedWith.DataBind();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            insertEventdetalis();


        }
        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            //<<Modified by Ankur Tyagi on 28-Apr-2025 for Project Id : 2395
            //Response.Redirect("~/default.aspx");
            Response.Redirect(Request.RawUrl, false);
            //>>
        }

        private void insertEventdetalis()
        {
            try
            {
                var givenYear = DateTime.Parse(txtDateOfEvent.Text, new CultureInfo("en-US")).Year;
                var currYear = DateTime.Parse(System.DateTime.Now.ToString(), new CultureInfo("en-US")).Year;
                if (currYear != givenYear)
                {
                    writeError("Event Instance can only be created for the current year.");
                    return;
                }
                if (ViewState["EventInstances"].ToString() == Session["EventInstances"].ToString())
                {
                    ListItem liAgenda;
                    Boolean blCheckForAgenda = false;
                    for (int j = 0; j <= cblAssociatedWith.Items.Count - 1; j++)
                    {

                        liAgenda = cblAssociatedWith.Items[j];
                        if (liAgenda.Selected)
                        {
                            blCheckForAgenda = true;
                        }
                    }
                    if (!blCheckForAgenda)
                    {
                        writeError("Please select atleast one Agenda.");
                        return;
                    }
                    EventInstancesBLL eventInstancesBL = new EventInstancesBLL();
                    String strEvent = null, strCompany = null;
                    String strDateOfEvent = null, strDescription = null, strCreateBy = null;
                    int intRowsSaved = 0;
                    strEvent = ddlEvent.SelectedValue;
                    //strCompany = ddlCompany.SelectedValue;
                    strDateOfEvent = txtDateOfEvent.Text;
                    strDescription = txtDescription.Text;

                    if (!Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)).Equals(""))
                    {
                        strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                    }

                    intRowsSaved = eventInstancesBL.SaveEventInstances(0, strEvent, strCompany, strDateOfEvent,
                                            strDescription, strCreateBy, getAssociateWithdt(), mstrConnectionString);


                    lblMsg.Text = "Event Instance saved successfully with Id " + intRowsSaved + ".";

                    Session["EventInstances"] = Server.UrlEncode(System.DateTime.Now.ToString());
                    clearControl();
                    lblMsg.Visible = true;
                    Panel1.Visible = false;
                    Panel2.Visible = true;
                }
                else
                {
                    writeError("Your attempt to refresh the page was blocked as it would lead to duplication of data.");
                }
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

        private DataTable getAssociateWithdt()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            ListItem liChkBoxListItem;
            dt = new DataTable();

            dt.Columns.Add(new DataColumn("AgendaId", typeof(string)));
            for (int i = 0; i <= cblAssociatedWith.Items.Count - 1; i++)
            {

                liChkBoxListItem = cblAssociatedWith.Items[i];
                if (liChkBoxListItem.Selected)
                {
                    dr = dt.NewRow();
                    dr["AgendaId"] = liChkBoxListItem.Value;
                    dt.Rows.Add(dr);

                }
            }
            return dt;
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        private void clearControl()
        {
            ddlEvent.SelectedValue = "";
            cblAssociatedWith.Items.Clear();
            txtDateOfEvent.Text = "";
            txtDescription.Text = "";
        }

        private void hideError()
        {
            lblMsg.Text = "";
            lblMsg.Visible = false;
        }
        protected void cvdate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
    }
}