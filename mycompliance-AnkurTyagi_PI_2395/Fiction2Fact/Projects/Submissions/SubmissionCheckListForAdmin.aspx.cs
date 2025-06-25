using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code.BLL;
using Fiction2Fact.App_Code;
using Fiction2Fact.F2FControls;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class Admin_SubmissionCheckListForAdmin : System.Web.UI.Page
    {
        string strConnectionString = null;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        SubmissionMasterBLL SubmissionMasterBLL = new SubmissionMasterBLL();
        RefCodesBLL refBL = new RefCodesBLL();

        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //<< Added By Milan Yadav on 27Apr2016
                //<<
                ListItem li = new ListItem();
                li.Text = "(Select)";
                li.Value = "";

                //<< Modified By Vivek on 16-Jan-2018
                DataTable dtFinYear = utilBL.getDataset("AllFinYears", strConnectionString).Tables[0];
                ddlFinYear.DataSource = dtFinYear;
                ddlFinYear.DataBind();
                ddlFinYear.Items.Insert(0, li);

                for (int i = 0; i < dtFinYear.Rows.Count; i++)
                {
                    if (dtFinYear.Rows[i]["FYM_STATUS"].ToString().Equals("A"))
                    {
                        ddlFinYear.SelectedValue = dtFinYear.Rows[i]["FYM_ID"].ToString();
                    }
                }
                //>>

                //<<Added by Ashish Mishra on 23Aug2017
                DataTable dtModeofFiling = refBL.getRefCodeDetails("Mode of Filing", strConnectionString);
                Session["ModeofFiling"] = dtModeofFiling;
                //ddlModeOfFiling.DataSource = refBL.getRefCodeDetails("Mode of Filing", strConnectionString);
                //ddlModeOfFiling.DataBind();
                //ddlModeOfFiling.Items.Insert(0, li);
                //>>
            }
        }

        protected void lbtnJan_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "1";
            BindGridView();

        }

        protected void lbtnFeb_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "2";
            BindGridView();
        }

        protected void lbtnMarch_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "3";
            BindGridView();
        }

        protected void lbtnApr_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "4";
            BindGridView();
        }

        protected void lbtnMay_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "5";
            BindGridView();
        }

        protected void lbtnJune_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "6";
            BindGridView();
        }

        protected void lbtnJuly_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "7";
            BindGridView();
        }

        protected void lbtnAug_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "8";
            BindGridView();
        }

        protected void lbtnSep_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "9";
            BindGridView();
        }

        protected void lbtnOct_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "10";
            BindGridView();
        }

        protected void lbtnNov_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "11";
            BindGridView();
        }

        protected void lbtnDec_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "12";
            BindGridView();

        }

        protected void BindGridView()
        {
            //gvChecklistDetails.DataSource = utilBL.getDatasetWithThreeConditionInString("getChecklistForAdminByMonth",
            //    hfMonth.Value, Authentication.GetUserID(Page.User.Identity.Name), ddlFinYear.SelectedValue,
            //    strConnectionString);
            gvChecklistDetails.DataSource = (new SubmUtilitiesBLL()).GetDataTable("getChecklistForAdminByMonth"
                    , new DBUtilityParameter("(MONTH(SC_DUE_DATE_TO)", hfMonth.Value)
                    , new DBUtilityParameter("FYM_ID", ddlFinYear.SelectedValue)
                    , new DBUtilityParameter("EM_USERNAME", Authentication.GetUserID(Page.User.Identity.Name)));

            gvChecklistDetails.DataBind();

            if (gvChecklistDetails.Rows.Count == 0)
            {
                writeError("No checklist available for selected month.");
            }
            else
            {
                writeError("");
            }
        }

        private void writeError(string strMsg)
        {
            lblMsg.Text = strMsg;
        }

        protected DataTable LoadSubmissionFileList(object ScId)
        {
            DataTable dt = utilBL.getDatasetWithCondition("SUBMISSIONSFILES", Convert.ToInt32(ScId), strConnectionString);
            return dt;
        }

        protected void gvChecklistDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //<<Added by Ashish Mishra on 23Aug2017
            ListItem li = new ListItem();
            li.Text = "--Select--";
            li.Value = "";
            //>>
            GridViewRow gvr;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                gvr = e.Row;
                System.Data.DataRowView drv = e.Row.DataItem as System.Data.DataRowView;

                if (drv != null)
                {
                    //<<Added by Ashish Mishra on 28Aug2017
                    //string strStatus = "";
                    //>>
                    String strSubmissionId = drv["SUB_ID"].ToString();
                    //<<Added by Ashish Mishra on 27Jul2017
                    int intScId = Convert.ToInt32(drv["SC_ID"]);
                    //>>
                    //RadioButtonList rbl = (RadioButtonList)(gvr.FindControl("rblYesNoNA"));
                    DropDownList ddlYesNo = (DropDownList)(gvr.FindControl("ddlYesNoNA"));
                    ddlYesNo.SelectedValue = drv["SUB_YES_NO_NA"].ToString();
                    DropDownList ddl = (DropDownList)(gvr.FindControl("ddlStatus"));
                    LinkButton lbSave = (LinkButton)(gvr.FindControl("lbSave"));
                    //<< Added By Ashish Mishra on 27Jul2017
                    LinkButton lb1 = (LinkButton)(gvr.FindControl("lbAttach"));
                    F2FTextBox txtSubAuthorityDate = (F2FTextBox)(gvr.FindControl("txtSubAuthorityDate"));
                    //>>
                    //<<Added by Ashish Mishra on 28Jul2017 
                    F2FTextBox txtReOpenComments = (F2FTextBox)(gvr.FindControl("txtReOpenComments"));
                    //>>
                    //<<Added by Ashish Mishra on 16Aug2017
                    DropDownList ddlModeOfFiling = (DropDownList)(gvr.FindControl("ddlModeOfFiling"));
                    ddlModeOfFiling.DataSource = Session["ModeOfFiling"];
                    ddlModeOfFiling.DataBind();
                    ddlModeOfFiling.Items.Insert(0, li);
                    //>>
                    if (drv["SUB_STATUS"] != null || drv["SUB_STATUS"].ToString() != "")
                    {
                        ddl.SelectedValue = drv["SUB_STATUS"].ToString();
                        //<<Added by Ashish Mishra on 28Aug2017
                        //strStatus = drv["SUB_STATUS"].ToString();
                        //>>
                    }

                    ////<<Added by Ashish Mishra on 28Aug2017
                    //if (strStatus.Equals("C"))
                    //{
                    //    ddl.Enabled = false;
                    //    ddlModeOfFiling.Enabled = false;
                    //    lbSave.Visible = false;
                    //    lb1.Visible = false;
                    //    txtSubAuthorityDate.Enabled = false;
                    //    txtReOpenComments.Enabled = false;
                    //}
                    //>>
                    if (strSubmissionId == null || strSubmissionId == "")
                    {
                        ddl.Enabled = false;
                        lbSave.Visible = false;
                        //<<Added by Ashish Mishra on 27Jul2017
                        lb1.Visible = false;
                        //>>
                    }
                    //<<Added By Ashish Mishra on 27Jul2017
                    else
                    {
                        lb1.Attributes.Add("onclick", "javascript:return openSubmissionFilesWindow('" + Convert.ToString(intScId) + "')");
                    }
                    //>>
                    //<<Added by Ashish Mishra on 16Aug2017
                    if (drv["SUB_MODE_OF_FILING"] != null || drv["SUB_MODE_OF_FILING"].ToString() != "")
                    {
                        ddlModeOfFiling.SelectedValue = drv["SUB_MODE_OF_FILING"].ToString();
                    }
                    //>>


                    lbSave.Attributes.Add("OnClick", "javascript:return validateReOpenComments('" + ddl.ClientID + "','" + txtReOpenComments.ClientID + "')");
                }
            }
        }

        protected void gvChecklistDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            try
            {
                string strStatus = "", strCreateBy = "";
                GridViewRow gvr = gvChecklistDetails.SelectedRow;
                Label lbl = ((Label)(gvr.FindControl("lblSubId")));
                int intSubmissionId = Convert.ToInt32(lbl.Text);

                DropDownList ddl = (DropDownList)(gvr.FindControl("ddlStatus"));
                strStatus = ddl.SelectedValue;
                if (!Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)).Equals(""))
                {
                    strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                }

                //<<Added by Ashish Mishra on 28Jul2017
                F2FTextBox txtSubAuthorityDate = (F2FTextBox)(gvr.FindControl("txtSubAuthorityDate"));
                F2FTextBox txtReOpenComments = (F2FTextBox)(gvr.FindControl("txtReOpenComments"));
                DropDownList ddlModeOfFiling = (DropDownList)(gvr.FindControl("ddlModeOfFiling"));
                //>>

                SubmissionMasterBLL.saveAdminChecklist(intSubmissionId, strStatus, strCreateBy, txtSubAuthorityDate.Text, txtReOpenComments.Text,
                                                        ddlModeOfFiling.SelectedValue, "", "", strConnectionString);

                BindGridView();
                lblMsg.Text = "Submission updated successfully.";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void gvChecklistDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataView dvDataView = new DataView(utilBL.getDatasetWithCondition("getChecklistForAdminByMonth", Convert.ToInt32(hfMonth.Value), strConnectionString));
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
            gvChecklistDetails.DataSource = dvDataView;
            gvChecklistDetails.DataBind();
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

        //<<Added by Ashish Mishra on 27Jul2017
        public void cvSubDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
        //>>

    }
}