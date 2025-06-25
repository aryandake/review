using System;
using System.Data;
using System.Data.SqlClient;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class Submissions_SampleCharts : System.Web.UI.Page
    {
        string mstrConnectionString = null;
        SubmissionMasterBLL SubmissionMasterBLL = new SubmissionMasterBLL();
        UtilitiesBLL utilityBL = new UtilitiesBLL();
        UtilitiesBLL utilBLL = new UtilitiesBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
            if (Page.User.Identity.Name.Equals(""))
            {
                Response.Redirect(Global.site_url("Login.aspx"));
                return;
            }
            //>>
            if (!IsPostBack)
            {
                ddlSubType.DataSource = utilityBL.getDataset("SUBTYPE", mstrConnectionString);
                ddlSubType.DataBind();

                ddlReportDept.DataSource = utilityBL.getDataset("REPORTINGDEPT", mstrConnectionString);
                ddlReportDept.DataBind();
            }

            getTrackingDeptWiseCompliedNotCompliedCount();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Page != null)
            {
                Page.RegisterRequiresViewStateEncryption();
            }
        }


        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            string strReportType = "";

            strReportType = ddlReportType.SelectedValue.ToString();

            if (strReportType.Equals("1"))
            {
                getTrackingDeptWiseCompliedNotCompliedCount();
            }
        }
        public void getTrackingDeptWiseCompliedNotCompliedCount()
        {
            DataTable dt = new DataTable();

            DataRow dr, dr1;
            SqlConnection myconnection = new SqlConnection(mstrConnectionString);
            string strDeptName = "", strDeptId = "", strQuery1 = "", strChart = "";
            int intCompliedCount, intNonCompliedCount, intNACount, intNotSubmitted;
            dt = (utilBLL.getDataSet("TrackingDept")).Tables[0];

            strChart = "<script type=\"text/javascript\"> Morris.Bar({element: 'bar-chart' ,data: [";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    strDeptName = dr["STM_TYPE"].ToString();
                    strDeptId = dr["STM_ID"].ToString();


                    strQuery1 = " select  STM_TYPE,SUB_YES_NO_NA,count(1) as TotCount,STM_ID from  TBL_SUB_CHKLIST " +
                                " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  and STM_ID = " + strDeptId +
                                " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
                                " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                " where 1=1 ";


                    if (!ddlReportDept.SelectedValue.Equals(""))
                    {
                        strQuery1 += " and SM_SRD_ID = " + ddlReportDept.SelectedValue;
                    }

                    if (!ddlFrequency.SelectedValue.Equals(""))
                    {
                        strQuery1 += " and SM_FREQUENCY = '" + ddlFrequency.SelectedValue + "' ";
                    }

                    if (!txtFromdate.Text.Equals(""))
                    {
                        strQuery1 += " and SC_DUE_DATE_FROM >= '" + txtFromdate.Text + "' ";
                    }

                    if (!txtTodate.Text.Equals(""))
                    {
                        strQuery1 += " and SC_DUE_DATE_TO <= '" + txtTodate.Text + "' ";
                    }

                    if (!ddlPriority.SelectedValue.Equals(""))
                    {
                        strQuery1 += " and SC_PRIORITY = '" + ddlPriority.SelectedValue + "' ";
                    }

                    strQuery1 += " group by STM_TYPE,STM_ID,SUB_YES_NO_NA order by STM_TYPE ASC ";

                    DataTable dtCount = new DataTable();
                    SqlCommand cmd = new SqlCommand(strQuery1, myconnection);
                    SqlDataAdapter searchDataAdaptor = new SqlDataAdapter(cmd);
                    searchDataAdaptor.Fill(dtCount);


                    intCompliedCount = 0;
                    intNonCompliedCount = 0;
                    intNACount = 0;
                    intNotSubmitted = 0;

                    if (dtCount.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtCount.Rows.Count; j++)
                        {
                            dr1 = dtCount.Rows[j];
                            if (dr1["SUB_YES_NO_NA"].Equals("Y"))
                            {
                                intCompliedCount = Convert.ToInt32(dr1["TotCount"].ToString());
                            }
                            else if (dr1["SUB_YES_NO_NA"].Equals("N"))
                            {
                                intNonCompliedCount = Convert.ToInt32(dr1["TotCount"].ToString());
                            }
                            else if (dr1["SUB_YES_NO_NA"].Equals("NA"))
                            {
                                intNACount = Convert.ToInt32(dr1["TotCount"].ToString());
                            }
                            else
                            {
                                intNotSubmitted = Convert.ToInt32(dr1["TotCount"].ToString());
                            }
                        }

                        strChart += "{x: '" + strDeptName + "', Y: " + intCompliedCount + " , N: " + intNonCompliedCount + " ," +
                                    " NA: " + intNACount + " , NS: " + intNotSubmitted + "},";
                    }
                    #region CommentedCode
                    //    strQuery2 = "   select  count(1)as NonCompliedCount from TBL_SUB_TYPE_MAS " +
                    //                "  left outer join TBL_SUBMISSIONS_MAS on  SM_STM_ID = STM_ID " +
                    //                "  inner  join TBL_SUB_CHKLIST on SC_SM_ID = SM_ID  and STM_ID = " + strDeptId + " " +
                    //                "  inner  join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] and SUB_YES_NO_NA = 'N'  ";

                    //    DataTable dtNonCompliedCount = new DataTable();
                    //    SqlCommand cmd1 = new SqlCommand(strQuery2, myconnection);
                    //    SqlDataAdapter searchDataAdaptor1 = new SqlDataAdapter(cmd1);
                    //    searchDataAdaptor1.Fill(dtNonCompliedCount);

                    //    if (dtNonCompliedCount.Rows.Count > 0)
                    //    {
                    //        for (int k = 0; k < dtNonCompliedCount.Rows.Count; k++)
                    //        {
                    //            dr2 = dtNonCompliedCount.Rows[k];
                    //            strNonCompliedCount = strNonCompliedCount + dr2["NonCompliedCount"].ToString() + ",";
                    //        }
                    //    }
                    //}

                    //strQuery3 = "   select  STM_TYPE,SUB_YES_NO_NA,count(1) as NotSubmitted,STM_ID from  TBL_SUB_CHKLIST " +
                    //            "   inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID " +
                    //            "   inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
                    //            "   left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] where SUB_YES_NO_NA is NULL" +
                    //            "   group by STM_TYPE,STM_ID,SUB_YES_NO_NA order by STM_TYPE ASC ";

                    //DataTable dtNotSubmittedCount = new DataTable();
                    //SqlCommand cmd2 = new SqlCommand(strQuery3, myconnection);
                    //SqlDataAdapter searchDataAdaptor2 = new SqlDataAdapter(cmd2);
                    //searchDataAdaptor2.Fill(dtNotSubmittedCount);

                    //if (dtNotSubmittedCount.Rows.Count > 0)
                    //{
                    //    for (int l = 0; l < dtNotSubmittedCount.Rows.Count; l++)
                    //    {
                    //        dr2 = dtNotSubmittedCount.Rows[l];

                    //        strNotSubmitted = strNotSubmitted + dr2["NotSubmitted"].ToString() + ",";
                    //    }

                    //}
                    #endregion

                }
                strChart = strChart.Substring(0, strChart.Length - 1);
                strChart += " ], xkey: 'x', ykeys: ['Y','N','NA','NS'], " +
                            " labels: ['Complied','Not Complied','Not Applicable','Not Submitted']," +
                            " barColors: ['#3e95cd','#8e5ea2','#267DD4','#C45850'], xLabelAngle: 0, resize: true});";


                strChart += "</script>";
                litChart.Text = strChart;
            }
        }
    }
}