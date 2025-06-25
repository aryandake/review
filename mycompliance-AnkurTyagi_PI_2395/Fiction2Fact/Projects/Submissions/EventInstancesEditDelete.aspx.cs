using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using Fiction2Fact.F2FControls;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class Admin_EventInstancesEditDelete : System.Web.UI.Page
    {
        UtilitiesBLL utilitiesBL = new UtilitiesBLL();
        private string mstrEEPIds = "";
        string mstrConnectionString = null;
        DataSet dsView1 = new DataSet();
        EventInstancesBLL eventInstancesBL = new EventInstancesBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            ddlEvent.Focus();
            if (!Page.IsPostBack)
            {
                ddlEvent.DataSource = utilitiesBL.getDataset("EVENT", mstrConnectionString);
                ddlEvent.DataBind();
                //ddlCompany.DataSource = utilitiesBL.getDataset("COMPANY", mstrConnectionString);
                //ddlCompany.DataBind();
                mvMultiView.ActiveViewIndex = 0;
                Session["EditEventInstances"] = Server.UrlEncode(System.DateTime.Now.ToString());
            }
            else
            {
                if (!(Session["ViewEventInstances"] == null))
                {
                    gvEventMaster.DataSource = (DataSet)(Session["ViewEventInstances"]);
                    gvEventMaster.DataBind();
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["EditEventInstances"] = Session["EditEventInstances"];
        }

        protected void ddlEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlEventEdit = ((DropDownList)(fvEventMaster.FindControl("ddlEventEdit")));
            hfEventId.Value = ddlEventEdit.SelectedValue;
            CheckBoxList cbl = ((CheckBoxList)(fvEventMaster.FindControl("cblAgenda")));
            if (hfEventId.Value != "")
            {
                cbl.DataSource = utilitiesBL.getDatasetWithCondition("EVENTPURPOSE", Convert.ToInt32(hfEventId.Value), mstrConnectionString);
                cbl.DataBind();
            }
            else
            {
                cbl.Items.Clear();
                cbl.DataBind();
            }
            mvMultiView.ActiveViewIndex = 1;

        }

        protected void fvEventMaster_DataBound(object sender, EventArgs e)
        {
            //DropDownList ddlEvent = ((DropDownList)(fvEventMaster.FindControl("ddlEvent")));
            //hfEventId.Value = ddlEvent.SelectedValue;
            //CheckBoxList cbl = ((CheckBoxList)(fvEventMaster.FindControl("cblAgenda")));
            //cbl.DataBind();
            //bindAgenda();
        }

        protected string LoadEventAgenda(object EventID)
        {
            DataTable dtAgenda;
            string strSegmentName = null;
            string strName = null;
            dtAgenda = utilitiesBL.getDatasetWithCondition("LoadEventAgenda", Convert.ToInt32(EventID), mstrConnectionString);
            for (int i = 0; i <= dtAgenda.Rows.Count - 1; i++)
            {
                strName = dtAgenda.Rows[i]["EP_NAME"].ToString();

                if (strSegmentName != null)
                {
                    strSegmentName = strSegmentName + ", " + strName;
                }
                else
                {
                    strSegmentName = strName;
                }
            }
            return strSegmentName;
        }

        private DataTable readerForId(int id)
        {
            DataTable dt = utilitiesBL.getDatasetWithCondition("readerForId", Convert.ToInt32(id), mstrConnectionString);
            return dt;
        }

        private void getEEMId()
        {
            string strTemp = "";
            DataTable dt = readerForId((Convert.ToInt32(hfSelectedRecord.Value)));
            foreach (DataRow dr in dt.Rows)
            {
                strTemp = strTemp + "," + (dr["EEM_ID"].ToString());
            }
            if (!strTemp.Equals(""))
            {
                mstrEEPIds = strTemp.Substring(1);
            }

        }

        private int areSubmissionsDone()
        {
            int intCount = 0;
            DataTable dt = utilitiesBL.getDatasetWithConditionInString("areSubmissionsDone", mstrEEPIds, mstrConnectionString);
            intCount = Convert.ToInt32(dt.Rows.Count);
            return intCount;

        }

        protected void gvEventMaster_SelectedIndexChanged(object sender, EventArgs e)
        {

            hfSelectedRecord.Value = gvEventMaster.SelectedValue.ToString();
            lblMsg.Text = "";
            int intInstanceId = Convert.ToInt32(hfSelectedRecord.Value);
            int intCount;
            getEEMId();
            intCount = areSubmissionsDone();
            if (hdfClientOperation.Value == "Edit")
            {
                if (intCount > 0)
                {
                    writeError(intCount + " Submission(s) is/are present for this Event instance so this record cannot be edited.");
                    return;
                }
                else
                {
                    DataSet ds;
                    mvMultiView.ActiveViewIndex = 1;
                    fvEventMaster.ChangeMode(FormViewMode.Edit);
                    ds = eventInstancesBL.SearchEventInstances(Convert.ToInt32(hfSelectedRecord.Value), 0, null, mstrConnectionString);
                    DataTable dt = ds.Tables[0];
                    DataRow dr = dt.Rows[0];
                    ListItem li = new ListItem();
                    li.Text = "--Select--";
                    li.Value = "";

                    fvEventMaster.DataSource = ds;
                    fvEventMaster.DataBind();

                    DropDownList ddlEventEdit = ((DropDownList)(fvEventMaster.FindControl("ddlEventEdit")));
                    DataSet dsEventEdit = utilitiesBL.getDataset("EVENT_EDIT", mstrConnectionString);
                    CommonCodes.SetDropDownDataSourceForEdit(ddlEventEdit, dsEventEdit.Tables[0], "EM_STATUS");
                    //ddlEventEdit.DataSource = utilitiesBL.getDataset("EVENT_EDIT", mstrConnectionString);
                    //ddlEventEdit.DataBind();
                    //ddlEventEdit.Items.Insert(0, li);
                    ddlEventEdit.SelectedValue = Convert.ToString(dr["EI_EM_ID"]);

                    CheckBoxList cbl = ((CheckBoxList)(fvEventMaster.FindControl("cblAgenda")));

                    //cbl.DataSource = utilitiesBL.getDataset("GETEVENTPURPOSE", mstrConnectionString);
                    cbl.DataSource = utilitiesBL.getDatasetWithCondition("EVENTPURPOSE", Convert.ToInt32(dr["EI_EM_ID"]), mstrConnectionString);
                    cbl.DataBind();
                    bindCbl(cbl, Convert.ToInt32(hfSelectedRecord.Value));

                }
            }
            else if (hdfClientOperation.Value == "Delete")
            {

                if (intCount > 0)
                {
                    writeError(intCount + " Submission(s) is/are present for this Event instance so this record cannot be deleted.");
                }
                else
                {
                    int intRetVal = 0;
                    intRetVal = eventInstancesBL.deleteEventInstances(intInstanceId, mstrConnectionString);
                    writeError("Record has been successfully deleted with Id: " + intInstanceId + "");
                    updateGridView();
                }
            }
        }

        private void updateGridView()
        {
            DataSet dsView2 = new DataSet();
            int intEventType = Convert.ToInt32(ddlEvent.SelectedValue);
            string strEventDate = txtDateOfEvent.Text;


            dsView2 = eventInstancesBL.SearchEventInstances(0, intEventType, strEventDate, mstrConnectionString);
            gvEventMaster.DataSource = dsView2;
            Session["ViewEventInstances"] = dsView2;
            gvEventMaster.DataBind();
        }


        protected void bindCbl(CheckBoxList cbl, int EIID)
        {
            DataTable dt;
            string strName = null;
            dt = utilitiesBL.getDatasetWithCondition("EVENTPURPOSEBYEPID", EIID, mstrConnectionString);
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                strName = dt.Rows[i]["EEM_EP_ID"].ToString();
                cbl.Items.FindByValue(strName).Selected = true;
            }
        }

        protected void gvEventMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEventMaster.PageIndex = e.NewPageIndex;
            gvEventMaster.DataSource = Session["ViewEventInstances"];
            gvEventMaster.DataBind();
        }
        private string ConvertSortDirectionToSql(SortDirection SortDirection)
        {
            string m_SortDirection = String.Empty;
            switch (SortDirection)
            {
                case SortDirection.Ascending:
                    m_SortDirection = "DESC";
                    break;
                case SortDirection.Descending:
                    m_SortDirection = "ASC";
                    break;
            }
            return m_SortDirection;
        }

        protected void gvEventMaster_Sorting(object sender, GridViewSortEventArgs e)
        {

            if (!(dsView1 == null))
            {


                DataTable dt = new DataTable();
                dt = (DataTable)Session["ViewEventInstances1"];
                DataView dvDataView = new DataView(dt);
                if (Convert.ToString(Session["sort"]) == "" || Convert.ToString(Session["sort"]) == "ASC")
                {
                    e.SortDirection = SortDirection.Ascending;
                    dvDataView.Sort = (e.SortExpression + (" " + ConvertSortDirectionToSql(e.SortDirection)));
                    Session["sort"] = "DESC";
                }
                else
                {
                    e.SortDirection = SortDirection.Descending;
                    dvDataView.Sort = (e.SortExpression + (" " + ConvertSortDirectionToSql(e.SortDirection)));
                    Session["sort"] = "ASC";
                }
                gvEventMaster.DataSource = dvDataView;
                gvEventMaster.DataBind();
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            editEventInstaceDetails();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mvMultiView.ActiveViewIndex = 0;
        }


        public void editEventInstaceDetails()
        {
            try
            {
                if (ViewState["EditEventInstances"].ToString() == Session["EditEventInstances"].ToString())
                {

                    ListItem liAgenda;
                    Boolean blCheckForAgenda = false;
                    CheckBoxList cblAgenda = ((CheckBoxList)(fvEventMaster.FindControl("cblAgenda")));
                    for (int j = 0; j <= cblAgenda.Items.Count - 1; j++)
                    {

                        liAgenda = cblAgenda.Items[j];
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

                    int intId = Convert.ToInt32(hfSelectedRecord.Value);
                    EventInstancesBLL eventInstancesBL = new EventInstancesBLL();
                    String strEvent = null, strCompany = null;
                    String strDateOfEvent = null, strDescription = null, strCreateBy = null;
                    int intRowsSaved = 0;
                    DropDownList ddlEventEdit = ((DropDownList)(fvEventMaster.FindControl("ddlEventEdit")));
                    F2FTextBox txtEventDate = ((F2FTextBox)(fvEventMaster.FindControl("txtEventDate")));
                    F2FTextBox txtEventDescription = ((F2FTextBox)(fvEventMaster.FindControl("txtEventDescription")));

                    strEvent = ddlEventEdit.SelectedValue;
                    //strCompany = ddlCompany.SelectedValue;
                    strDateOfEvent = txtEventDate.Text;
                    strDescription = txtEventDescription.Text;

                    if (!Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)).Equals(""))
                    {
                        strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                    }
                    //<<Commented by prajakta salvi on 6-Jul-10
                    //utilitiesBL.getDatasetWithCondition("DeleteEiEpMapping", intId, mstrConnectionString);
                    //>>
                    intRowsSaved = eventInstancesBL.SaveEventInstances(intId, strEvent, strCompany, strDateOfEvent,
                                            strDescription, strCreateBy, getAssociateWithdt(), mstrConnectionString);


                    writeError("Event Instance updated successfully with Id " + intId + ".");
                    mvMultiView.ActiveViewIndex = 0;
                    updateGridView();
                    Session["EditEventInstances"] = Server.UrlEncode(System.DateTime.Now.ToString());
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
            CheckBoxList cblAgenda = ((CheckBoxList)(fvEventMaster.FindControl("cblAgenda")));
            DataTable dt = new DataTable();
            DataRow dr;
            ListItem liChkBoxListItem;
            dt = new DataTable();

            dt.Columns.Add(new DataColumn("AgendaId", typeof(string)));
            for (int i = 0; i <= cblAgenda.Items.Count - 1; i++)
            {

                liChkBoxListItem = cblAgenda.Items[i];
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

        private void hideError()
        {
            lblMsg.Text = "";
            lblMsg.Visible = false;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            try
            {

                int intEventType = Convert.ToInt32(ddlEvent.SelectedValue);
                string strEventDate = txtDateOfEvent.Text;


                dsView1 = eventInstancesBL.SearchEventInstances(0, intEventType, strEventDate, mstrConnectionString);

                Session["ViewEventInstances"] = dsView1;
                Session["ViewEventInstances1"] = dsView1.Tables[0];
                gvEventMaster.DataSource = dsView1;
                gvEventMaster.DataBind();
                if ((this.gvEventMaster.Rows.Count == 0))
                {
                    writeError("No Records found satisfying the criteria.");
                }
                else
                {
                    writeError("");
                }
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

    }
}