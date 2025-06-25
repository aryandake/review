using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Fiction2Fact.Legacy_App_Code.Circulars.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;

namespace Fiction2Fact.Projects.Circulars
{
    public partial class Circulars_ViewCircularDetails : System.Web.UI.Page
    {
        string mstrConnectionString = null;
        CircularMasterBLL CircularMasterBLL = new CircularMasterBLL();
        SubmissionMasterBLL SubmissionMasterBLL = new SubmissionMasterBLL();
        //UtilitiesBLL UtilitiesBLL = new UtilitiesBLL();
        CircUtilitiesBLL circUtilBLL = new CircUtilitiesBLL();
        RefCodesBLL rcBL = new RefCodesBLL();
        //<< Added by ramesh more on 13-Mar-2024 CR_1991
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();
        //>>
        protected void Page_Load(object sender, EventArgs e)
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
                if (!Page.IsPostBack)
                {
                    if (Request.QueryString["CircularId"] != null)
                    {
                        //<< Modified by ramesh more on 13-Mar-2024 CR_1991
                        string strId = Request.QueryString["CircularId"].ToString();
                        hfCircularId.Value = encdec.Decrypt(strId);
                        //>>
                    }

                    if (Request.QueryString["CircCertCheclistId"] != null)
                    {
                        hfCircCertChecklistId.Value = Request.QueryString["CircCertCheclistId"].ToString();
                    }

                    getDetails();
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Page != null)
            {
                Page.RegisterRequiresViewStateEncryption();
            }
        }

        protected String getFileName(object inputFileName)
        {
            string strFileName = "";

            try
            {
                strFileName = inputFileName.ToString();
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return strFileName.Replace("'", "\\'");
        }

        protected void bindIntimationDetails(CheckBoxList cbSubmissions, string CircularId)
        {
            try
            {
                DataTable dtIntimationName;
                string strName = null;
                //dtIntimationName = UtilitiesBLL.getDatasetWithCondition("CIRCULARINTIMATION", Convert.ToInt32(hfCircularId.Value), mstrConnectionString);
                dtIntimationName = circUtilBLL.GetDataTable("CIRCULARINTIMATION", new DBUtilityParameter("CMI_CM_ID", hfCircularId.Value));
                for (int i = 0; i <= dtIntimationName.Rows.Count - 1; i++)
                {
                    strName = dtIntimationName.Rows[i]["CMI_CIM_ID"].ToString();
                    cbSubmissions.Items.FindByValue(strName).Selected = true;
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            //lblMsg.Visible = true;
            string script = "\r\n<script language=\"javascript\">\r\n" +
                       " alert('" + strError.Replace("'", "\\'") + "');" +
                       "   </script>\r\n";

            ClientScript.RegisterStartupScript(this.GetType(), "script", script);
        }

        private void getDetails()
        {
            try
            {
                Hashtable ParamMap = new Hashtable();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;
                DataSet dsCircularDetails = new DataSet();
                string strAssociatedKeyword = "";
                string[] strarrAssociatedKeyword;

                ds = CircularMasterBLL.SearchCircularDetails(Convert.ToInt32(hfCircularId.Value), "", "", "", "", "", "", "", "", "", "", "", "", "", "");

                dt = ds.Tables[0];
                if (dt.Rows.Count == 0)
                    writeError("No records found.");
                else
                {
                    dr = dt.Rows[0];
                    lblCreator.Text = dr["CDM_NAME"].ToString();

                    lblLOB.Text = dr["LEM_NAME"].ToString();

                    //<< Added by Amarjeet on 04-Aug-2021
                    lblLinkageWithEarlierCircular.Text = dr["LinkageWithEarlierCircular"].ToString();
                    lblSOCEOC.Text = dr["SOCEOC"].ToString();
                    lblOldCircSubNo.Text = dr["CM_OLD_CIRC_SUB_NO"].ToString();
                    //>>
                    lblAuthority.Text = dr["CIA_NAME"].ToString();
                    lblTypeofDocument.Text = dr["CDTM_TYPE_OF_DOC"].ToString();
                    lblSubTypeofDocument.Text = dr["Type"].ToString();
                    lblTopic.Text = dr["CAM_NAME"].ToString();
                    lblCircularNo.Text = dr["CM_CIRCULAR_NO"].ToString();

                    if (dr["CM_DATE"].ToString().Equals(""))
                        lblCircularDate.Text = "";
                    else
                        lblCircularDate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dr["CM_DATE"]));

                    if (dr["CM_CIRC_EFF_DATE"].ToString().Equals(""))
                        lblCircEffDate.Text = "";
                    else
                        lblCircEffDate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dr["CM_CIRC_EFF_DATE"]));

                    lblSubject.Text = dr["CM_TOPIC"].ToString();

                    //<< Added by Amarjeet on 26-Jul-2021
                    //cbAssociatedKeywords.DataSource = UtilitiesBLL.getDataset("AssociatedKeywords", mstrConnectionString);
                    //cbAssociatedKeywords.DataBind();

                    strAssociatedKeyword = dr["CM_ASSOCIATED_KEY"].ToString();
                    strarrAssociatedKeyword = strAssociatedKeyword.Split(',');

                    //cbAssociatedKeywords = CommonCodes.getCheckboxSelectedValuesFromArray(strarrAssociatedKeyword, cbAssociatedKeywords);
                    if (strarrAssociatedKeyword.Length > 0)
                    {
                        CommonCodes.SetCheckboxDataSource(cbAssociatedKeywords, circUtilBLL.GetDataTable("AssociatedKeywords", sOrderBy: "CKM_NAME"), arrValues: strarrAssociatedKeyword);
                    }

                    //>>

                    lblSPOCFromComplianceFunction.Text = dr["SPOCName"].ToString();
                    lblGist.Text = dr["CM_DETAILS"].ToString().Replace("\n", "<br />");
                    lblImplications.Text = dr["CM_IMPLICATIONS"].ToString().Replace("\n", "<br />");
                    lblLink.Text = dr["CM_ISSUING_LINK"].ToString();

                    lblRequirementForTheBoard.Text = dr["AuditCommitteeToApprove"].ToString();
                    //cbToBePlacedBefore.DataSource = rcBL.getRefCodeDetails("Circulars - New Circular - To be placed before", mstrConnectionString);
                    //cbToBePlacedBefore.DataBind();
                    //cbToBePlacedBefore = CommonCodes.getCheckboxSelectedValuesFromArray(dr["CM_TO_BE_PLACED_BEFORE"].ToString().Split(','), cbToBePlacedBefore);

                    CommonCodes.SetCheckboxDataSource(cbToBePlacedBefore, rcBL.getRefCodeDetails("Circulars - New Circular - To be placed before"), arrValues: (dr["CM_TO_BE_PLACED_BEFORE"] != DBNull.Value ? (dr["CM_TO_BE_PLACED_BEFORE"].ToString().Split(',')) : null));

                    lblDetails.Text = dr["CM_REMARKS"].ToString().Replace("\n", "<br />");
                    lblNameOfThePolicy.Text = dr["CM_NAME_OF_THE_POLICY"].ToString();
                    lblFrequency.Text = dr["CM_FREQUENCY"].ToString();

                    lblStatus.Text = dr["CircStatus"].ToString();
                    lblDeactivatedBy.Text = dr["CM_DEACTIVATED_BY"].ToString();

                    if (dr["CM_DEACTIVATED_ON"].ToString().Equals(""))
                        lblDeactivatedOn.Text = "";
                    else
                        lblDeactivatedOn.Text = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", Convert.ToDateTime(dr["CM_DEACTIVATED_ON"]));

                    lblDeactivationRemarks.Text = dr["CM_DEACTIVATION_REMARKS"].ToString().Replace("\n", "<br />");

                    DataTable dtCircularFiles;
                    //dtCircularFiles = UtilitiesBLL.getDatasetWithCondition("CIRCULARFILES", Convert.ToInt32(hfCircularId.Value), mstrConnectionString);
                    dtCircularFiles = circUtilBLL.GetDataTable("CIRCULARFILES", new DBUtilityParameter("CF_CM_ID", hfCircularId.Value));
                    gvViewFileUpload.DataSource = dtCircularFiles;
                    gvViewFileUpload.DataBind();

                    ParamMap.Add("CMId", string.IsNullOrEmpty(hfCircularId.Value) ? "0" : hfCircularId.Value);
                    dsCircularDetails = CircularMasterBLL.getCircularDetails(ParamMap, mstrConnectionString);
                    DataTable dtActionables = dsCircularDetails.Tables[0];
                    gvActionables.DataSource = dtActionables;
                    gvActionables.DataBind();

                    int intCircularId = 0;
                    bool res = false;
                    res = int.TryParse(hfCircularId.Value, out intCircularId);

                    gvCertChecklists.DataSource = CircularMasterBLL.SearchCircularCertChecklist(intCircularId, hfCircCertChecklistId.Value);
                    gvCertChecklists.DataBind();

                    gvSubmissionMaster.DataSource = SubmissionMasterBLL.getListOfReports(0, "", "", "Active", "", "", "", "", "",
                        Page.User.Identity.Name.ToString(), "Admin", hfCircularId.Value,"", mstrConnectionString);
                    gvSubmissionMaster.DataBind();
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected string LoadSubmissionSegmentName(object SubmissionID)
        {
            DataTable dtSegmentName;
            string strSegmentName = null;
            string strName = null;

            try
            {
                //dtSegmentName = UtilitiesBLL.getDatasetWithCondition("LOADSUBSEGMENTS", Convert.ToInt32(SubmissionID), mstrConnectionString);
                dtSegmentName = circUtilBLL.GetDataTable("LOADSUBSEGMENTS", new DBUtilityParameter("SM_ID", SubmissionID));
                for (int i = 0; i <= dtSegmentName.Rows.Count - 1; i++)
                {
                    strName = dtSegmentName.Rows[i]["SSM_NAME"].ToString();

                    if (strSegmentName != null)
                    {
                        strSegmentName = strSegmentName + ", " + strName;
                    }
                    else
                    {
                        strSegmentName = strName;
                    }
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return strSegmentName;
        }

        protected void gvCertChecklists_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HiddenField hfAuditTrail = (HiddenField)(e.Row.FindControl("hfAuditTrail"));
                    LinkButton lbAuditTrail = (LinkButton)(e.Row.FindControl("lbAuditTrail"));

                    lbAuditTrail.Attributes["onClick"] = "return onViewChklistAuditTrailClick('" + hfAuditTrail.ClientID + "');";
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
    }
}