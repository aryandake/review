using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.Circulars.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Label = System.Web.UI.WebControls.Label;

namespace Fiction2Fact.Projects.Circulars
{
    public partial class ViewCircularAdd : System.Web.UI.Page
    {
        CircUtilitiesBLL utilitiesBL = new CircUtilitiesBLL();
        CircularMasterBLL circularMasterBL = new CircularMasterBLL();
        DataServer dserv = new DataServer();

        DataTable dtPreviousAdded = new DataTable();


        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
                if (Page.User.Identity.Name.Equals(""))
                {
                    Response.Redirect(Global.site_url("Login.aspx"));
                    return;
                }
                //>>
                string strType = "", strID = "", strActiontype = "", strTopic = "";
                if (!IsPostBack)
                {
                    ListItem li = new ListItem();
                    li.Text = "-Select-";
                    li.Value = "";

                    Session["PreviousUploaded"] = null;

                    if (Request.QueryString["Type"] != null)
                    {
                        strType = Request.QueryString["Type"].ToString();
                        hfType.Value = strType;
                        strID = Request.QueryString["ID"].ToString();
                        hfID.Value = strID;
                        strActiontype = Request.QueryString["ActionType"].ToString();
                        hfActionType.Value = strActiontype;
                        strTopic = Request.QueryString["Text"].ToString();
                        hfTopic.Value = strTopic;
                        if (!hfID.Value.Equals(""))
                        {
                            bindPreviousGrid();
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "tabber", "tabChange(2);", true);
                        }
                    }

                    //bindgrid();
                    if (dtPreviousAdded.Columns.Count == 0)
                    {
                        initdt();
                    }
                }
            }
            //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
            //>>
        }

        public void initdt()
        {
            dtPreviousAdded.Columns.Add(new DataColumn("Issuing Authority", typeof(string)));
            dtPreviousAdded.Columns.Add(new DataColumn("Topic", typeof(string)));
            dtPreviousAdded.Columns.Add(new DataColumn("Circular No", typeof(string)));
            dtPreviousAdded.Columns.Add(new DataColumn("Circular Date", typeof(string)));
            dtPreviousAdded.Columns.Add(new DataColumn("Subject", typeof(string)));
            dtPreviousAdded.Columns.Add(new DataColumn("ID", typeof(string)));
        }
        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }


        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            bindgrid();
        }

        public void bindPreviousGrid()
        {
            //<< Modified by Amarjeet on 22-Oct-2020
            dtPreviousAdded = dserv.Getdata(" select " +
                " CIA_NAME as [Issuing Authority],CAM_NAME as [Topic]," +
                " CM_CIRCULAR_NO as [Circular No],CM_DATE as [Circular Date]," +
                " CM_ID as [ID], CM_TOPIC as [Subject] from TBL_CIRCULAR_MASTER" +
                " INNER JOIN TBL_CIRCULAR_ISSUING_AUTHORITIES ON CM_CIA_ID = CIA_ID " +
                " INNER JOIN TBL_CIRCULAR_AREA_MAS ON CM_CAM_ID = CAM_ID " +
                " where CM_ID in (" + hfID.Value + ")");


            //dtPreviousAdded = utilitiesBL.GetDataTable("getCirculars", new DBUtilityParameter("CM_ID", hfID.Value, "IN", oSubQuery: 1));


            //dtPreviousAdded = CircularMasterBLL.SearchCircular(Convert.ToInt32(hfID.Value), "", "", "", "", "", "", "",
            //        "", "", "OldCircular", "", "", "", "", strCircularIds: hfID.Value).Tables[0];

            //>>
            gvCircularUploaded.DataSource = dtPreviousAdded;
            gvCircularUploaded.DataBind();
            Session["PreviousUploaded"] = dtPreviousAdded;
        }

        public void bindgrid()
        {
            try
            {
                string strFilterExpression = String.Empty;
                string strIssuingAuthority = "";
                string strarea = "";
                string FromDate = txtFromDate.Text;
                string ToDate = txtToDate.Text;
                string strSubject = "";
                DataSet dsViewCircular = new DataSet();

                //if (!string.IsNullOrEmpty(strIssuingAuthority))
                //{
                dsViewCircular = circularMasterBL.SearchCircular(0, "", "", "",
                            "", "", "", "", FromDate, ToDate,
                            "", "", "", "",
                            "", "", txtGlobalSearch.Text);


                //dsViewCircular = circularMasterBL.SearchCircular(0, strIssuingAuthority, "", "",
                //                            strarea, "", "", "",
                //                            FromDate, ToDate, "View", "", strSubject,
                //                            mstrConnectionString);

                DataView dvDataView = new DataView(dsViewCircular.Tables[0]);

                if (!hfID.Value.Equals(""))
                    dvDataView.RowFilter = "CM_ID NOT IN (" + hfID.Value + ")";

                Session["ViewCircularSelectCommand"] = dvDataView;
                gvCircularMaster.DataSource = dvDataView;
                gvCircularMaster.DataBind();

                if ((this.gvCircularMaster.Rows.Count == 0))
                {
                    this.lblMsg.Text = "No Records found satisfying the criteria.";
                    this.lblMsg.ForeColor = System.Drawing.Color.Red;
                    this.lblMsg.Visible = true;
                }
                else
                {
                    this.lblMsg.Text = String.Empty;
                    this.lblMsg.Visible = false;
                }
                //}
            }
            catch (Exception ex)
            {
                this.lblMsg.Text = ex.Message;
                this.lblMsg.ForeColor = System.Drawing.Color.Red;
                this.lblMsg.Visible = true;
            }
        }
        protected void gvCircularMaster_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (Session["ViewCircular"] != null)
            {
                DataTable dt = (DataTable)Session["ViewCircularSelectCommand"];
                DataView dvDataView = new DataView(dt);
                string strSortExpression = "";

                if (ViewState["_SortExpression_"] != null)
                {
                    strSortExpression = ViewState["_SortExpression_"].ToString();
                }

                if (ViewState["_SortDirection_"] == null)
                {
                    dvDataView.Sort = (e.SortExpression + (" " + "ASC"));
                    ViewState["_SortDirection_"] = "ASC";
                }
                else
                {
                    //ONLY IF THE USER HAS CLICKED ON THE SAME COLUMN AGAIN, SHOULD IT BE SORTED IN THE REVERSE ORDER.
                    //IF ANOTHER COLUMN IS SELECTED, IT SHOULD AGAIN BE SORTED IN ASCENDING ORDER. 
                    if (strSortExpression.Equals(e.SortExpression))
                    {
                        if (ViewState["_SortDirection_"].ToString().Equals("ASC"))
                        {
                            dvDataView.Sort = (e.SortExpression + (" " + "DESC"));
                            ViewState["_SortDirection_"] = "DESC";
                        }
                        else if (ViewState["_SortDirection_"].ToString().Equals("DESC"))
                        {
                            dvDataView.Sort = (e.SortExpression + (" " + "ASC"));
                            ViewState["_SortDirection_"] = "ASC";
                        }
                    }
                    else
                    {
                        dvDataView.Sort = (e.SortExpression + (" " + "ASC"));
                        ViewState["_SortDirection_"] = "ASC";
                    }


                }
                ViewState["_SortExpression_"] = e.SortExpression;
                gvCircularMaster.DataSource = dvDataView;
                gvCircularMaster.DataBind();
            }
        }
        protected void gvCircularMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCircularMaster.PageIndex = e.NewPageIndex;
            gvCircularMaster.DataSource = Session["ViewCircularSelectCommand"];
            gvCircularMaster.DataBind();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            string strIssAuth = "", strArea = "", strCircNo = "", strCircularDate = "", strTopic = "",
                strID = "";

            int i = 1;
            foreach (GridViewRow gvr in gvCircularMaster.Rows)
            {
                CheckBox chk = (CheckBox)gvr.FindControl("chkbx");

                if (chk != null & chk.Checked)
                {
                    strID = gvCircularMaster.DataKeys[gvr.RowIndex].Value.ToString();
                    hfID.Value = string.IsNullOrEmpty(hfID.Value) ? strID : hfID.Value + "," + strID;

                    strIssAuth = ((Label)gvr.FindControl("lblIssAuth")).Text;
                    strArea = ((Label)gvr.FindControl("lblArea")).Text;
                    strCircNo = ((Label)gvr.FindControl("lblCircNo")).Text;
                    strCircularDate = ((Label)gvr.FindControl("lblCircularDate")).Text;
                    strTopic = ((Label)gvr.FindControl("lblTopic")).Text;

                    if (Session["PreviousUploaded"] == null)
                    {
                        if (dtPreviousAdded.Columns.Count == 0)
                            initdt();

                        DataRow drRow = dtPreviousAdded.NewRow();
                        drRow["Issuing Authority"] = strIssAuth;
                        drRow["Topic"] = strArea;
                        drRow["Circular No"] = strCircNo;
                        drRow["Circular Date"] = strCircularDate;
                        drRow["Subject"] = strTopic;
                        drRow["ID"] = strID;
                        dtPreviousAdded.Rows.Add(drRow);
                    }
                    else
                    {
                        dtPreviousAdded = (DataTable)Session["PreviousUploaded"];
                        DataRow drRow = dtPreviousAdded.NewRow();
                        drRow["Issuing Authority"] = strIssAuth;
                        drRow["Topic"] = strArea;
                        drRow["Circular No"] = strCircNo;
                        drRow["Circular Date"] = strCircularDate;
                        drRow["Subject"] = strTopic;
                        drRow["ID"] = strID;
                        dtPreviousAdded.Rows.Add(drRow);
                    }

                    gvCircularUploaded.DataSource = dtPreviousAdded;
                    gvCircularUploaded.DataBind();
                    Session["PreviousUploaded"] = dtPreviousAdded;
                }
            }

            DataView dvDataView = (DataView)Session["ViewCircularSelectCommand"];
            if (!hfID.Value.Equals(""))
                dvDataView.RowFilter = "CM_ID NOT IN (" + hfID.Value + ")";
            Session["ViewCircularSelectCommand"] = dvDataView;
            gvCircularMaster.DataSource = dvDataView;
            gvCircularMaster.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabber", "tabChange(2);", true);
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Visible = true;
            this.lblMsg.Font.Size = 12;
        }

        protected void gvCircularMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string[] arrstrId;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCircularId = (Label)e.Row.FindControl("lblCircularId");
                CheckBox chkbx = (CheckBox)e.Row.FindControl("chkbx");
                if (!hfID.Value.Equals(""))
                {
                    arrstrId = hfID.Value.Split(',');
                    for (int i = 0; i < arrstrId.Length; i++)
                    {
                        //if (arrstrId[i].Equals(lblCircularId.Text))
                        //{
                        //    chkbx.Checked = true;
                        //}
                    }
                }
            }
        }
        protected void gvCircularMaster_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string strCircularId;
            try
            {
                strCircularId = gvCircularMaster.SelectedValue.ToString();
                hfSelectedRecord.Value = strCircularId;
                if (hfSelectedRecord.Value.Equals("View"))
                {
                    Response.Redirect("ViewCircularDetails.aspx?CircularId=" + strCircularId);
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void gvCircularUploaded_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strId = gvCircularUploaded.SelectedValue.ToString();

            dtPreviousAdded = (DataTable)Session["PreviousUploaded"];
            dtPreviousAdded.Rows.RemoveAt(gvCircularUploaded.SelectedIndex);
            gvCircularUploaded.DataSource = dtPreviousAdded;
            gvCircularUploaded.DataBind();

            Session["PreviousUploaded"] = dtPreviousAdded;

            if (!hfID.Value.Equals(""))
            {
                List<string> listId = new List<string>();
                listId.AddRange(hfID.Value.Split(','));
                listId.Remove(strId);
                hfID.Value = String.Join(",", listId);
            }
            if (gvCircularMaster.Rows.Count > 0)
            {
                bindgrid();
            }
        }
        protected void cvTodate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
        protected void btnDone_Click(object sender, EventArgs e)
        {
            string strName = "", strID = "";
            foreach (GridViewRow gvr in gvCircularUploaded.Rows)
            {

                strID = (string.IsNullOrEmpty(strID) ? "" : strID + ", ") + gvCircularUploaded.DataKeys[gvr.RowIndex].Value.ToString();
                strName = (string.IsNullOrEmpty(strName) ? "" : strName + ", ") + ((Label)gvr.FindControl("lblSubject")).Text;
            }
            Session["PreviousUploaded"] = null;


            if (strName.EndsWith(","))
                strName = strName.Substring(0, strName.Length - 1);

            if (strID.EndsWith(","))
                strID = strID.Substring(0, strID.Length - 1);

            hfID.Value = strID;
            hfName.Value = strName;

            ClientScript.RegisterStartupScript(this.GetType(), "script", " closePopupWindow();", true);

        }
    }
}