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
using System.IO;
using Fiction2Fact.Legacy_App_Code.Circulars.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using System.Reflection;
using Fiction2Fact.Legacy_App_Code.Circulars;
using System.Text;

namespace Fiction2Fact.Projects.Circulars
{
    public partial class CertChecklistsApproval : System.Web.UI.Page
    {
        CircularMasterBLL cirBLL = new CircularMasterBLL();
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();
        int intCnt = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    Session["strMidArray"] = "";
                    Session["strRemarks"] = "";
                    Session["strStatus"] = "";
                    bindGrid();
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void bindGrid()
        {
            try
            {
                DataTable dtCircCertChecklist = cirBLL.SearchCircularCertChecklist(0, "", Page.User.Identity.Name, "Approval");
                Session["CircCertChecklists"] = dtCircCertChecklist;
                gvCircularMaster.DataSource = dtCircCertChecklist;
                gvCircularMaster.DataBind();

                if (gvCircularMaster.Rows.Count > 0)
                {
                    btnAccept.Visible = true;
                    btnAccept1.Visible = true;
                    btnReject.Visible = true;
                    btnReject1.Visible = true;
                }
                else
                {
                    btnAccept.Visible = false;
                    btnAccept1.Visible = false;
                    btnReject.Visible = false;
                    btnReject1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private DataTable getAcceptanceRejectionDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr;

            try
            {
                dt.Columns.Add("CCCId", typeof(string));
                dt.Columns.Add("Comments", typeof(string));
                dt.Columns.Add("SPOCEmailId", typeof(string));
                dt.Columns.Add("UHEmailId", typeof(string));
                dt.Columns.Add("FHEmailId", typeof(string));
                dt.Columns.Add("DeptName", typeof(string));
                dt.Columns.Add("CirType", typeof(string));
                dt.Columns.Add("CirSubject", typeof(string));
                dt.Columns.Add("CirIssAuthority", typeof(string));
                dt.Columns.Add("CirDate", typeof(string));
                dt.Columns.Add("Reference", typeof(string));
                dt.Columns.Add("Clause", typeof(string));
                dt.Columns.Add("CheckPoints", typeof(string));
                dt.Columns.Add("Particulars", typeof(string));
                dt.Columns.Add("Penalty", typeof(string));
                dt.Columns.Add("Frequency", typeof(string));
                dt.Columns.Add("EffectiveFrom", typeof(string));
                dt.Columns.Add("ActRegCirc", typeof(string));
                dt.Columns.Add("Forms", typeof(string));

                foreach (GridViewRow gvr in gvCircularMaster.Rows)
                {
                    CheckBox RowLevelCheckBox = (CheckBox)gvr.FindControl("RowLevelCheckBox");
                    HiddenField hfCircCertChecklistId = (HiddenField)gvr.FindControl("hfCircCertChecklistId");
                    TextBox txtComments = (TextBox)gvr.FindControl("txtComments");
                    HiddenField hfSPOCEmailId = (HiddenField)gvr.FindControl("hfSPOCEmailId");
                    HiddenField hfUHEmailId = (HiddenField)gvr.FindControl("hfUHEmailId");
                    HiddenField hfFHEmailId = (HiddenField)gvr.FindControl("hfFHEmailId");
                    HiddenField hfCirType = (HiddenField)gvr.FindControl("hfCirType");
                    HiddenField hfCirIssAuthority = (HiddenField)gvr.FindControl("hfCirIssAuthority");
                    HiddenField hfCirSubject = (HiddenField)gvr.FindControl("hfCirSubject");
                    Label lblCircularDate = (Label)gvr.FindControl("lblCircularDate");
                    HiddenField hfDeptName = (HiddenField)gvr.FindControl("hfDeptName");
                    HiddenField hfFrequency = (HiddenField)gvr.FindControl("hfFrequency");
                    HiddenField hfEffectiveFrom = (HiddenField)gvr.FindControl("hfEffectiveFrom");
                    Label lblReference = (Label)gvr.FindControl("lblReference");
                    Label lblClause = (Label)gvr.FindControl("lblClause");
                    Label lblCheckpoints = (Label)gvr.FindControl("lblCheckpoints");
                    Label lblParticulars = (Label)gvr.FindControl("lblParticulars");
                    Label lblPenalty = (Label)gvr.FindControl("lblPenalty");
                    HiddenField hfActRegCirc = (HiddenField)gvr.FindControl("hfActRegCirc");
                    HiddenField hfForms = (HiddenField)gvr.FindControl("hfForms");

                    if (RowLevelCheckBox.Checked)
                    {
                        dr = dt.NewRow();
                        dr["CCCId"] = hfCircCertChecklistId.Value;
                        dr["Comments"] = txtComments.Text;
                        dr["SPOCEmailId"] = hfSPOCEmailId.Value;
                        dr["UHEmailId"] = hfUHEmailId.Value;
                        dr["FHEmailId"] = hfFHEmailId.Value;
                        dr["DeptName"] = hfDeptName.Value;
                        dr["CirType"] = hfCirType.Value;
                        dr["CirSubject"] = hfCirSubject.Value;
                        dr["CirIssAuthority"] = hfCirIssAuthority.Value;
                        dr["CirDate"] = lblCircularDate.Text;
                        dr["Reference"] = lblReference.Text;
                        dr["Clause"] = lblClause.Text;
                        dr["CheckPoints"] = lblCheckpoints.Text;
                        dr["Particulars"] = lblParticulars.Text;
                        dr["Penalty"] = lblPenalty.Text;
                        dr["Frequency"] = hfFrequency.Value;
                        dr["EffectiveFrom"] = hfEffectiveFrom.Value;
                        dr["ActRegCirc"] = hfActRegCirc.Value;
                        dr["Forms"] = hfForms.Value;
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return dt;
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            string strLoggedInUser = "";

            try
            {
                DataTable dt = getAcceptanceRejectionDetails();
                strLoggedInUser = new Authentication().getUserFullName(Page.User.Identity.Name);

                cirBLL.acceptRejectCertChecklist(dt, "MTCC", strLoggedInUser);

                sendMailOnAcceptanceRejection(dt, "MTCC", strLoggedInUser);
                bindGrid();
                writeError("Selected certification checklist have been moved in the Certification Checklist successfully.");

                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            string strLoggedInUser = "";

            try
            {
                DataTable dt = getAcceptanceRejectionDetails();
                strLoggedInUser = new Authentication().getUserFullName(Page.User.Identity.Name);

                cirBLL.acceptRejectCertChecklist(dt, "STS", strLoggedInUser);

                sendMailOnAcceptanceRejection(dt, "STS", strLoggedInUser);
                bindGrid();
                writeError("Selected certification checklist have been sent to the stakeholders for acceptance.");

                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void sendMailOnAcceptanceRejection(DataTable dt, string strType, string strLoggedInUser)
        {
            try
            {
                MailContent_Circulars mail = new MailContent_Circulars();

                DataTable dtDistinctData = new DataView(dt).ToTable(true, "DeptName");

                for (int i = 0; i < dtDistinctData.Rows.Count; i++)
                {
                    string strDeptName = "", strMailTo = "";
                    DataRow dr = dtDistinctData.Rows[i];
                    strDeptName = dr["DeptName"].ToString();

                    DataView dvData = new DataView(dt);
                    dvData.RowFilter = "DeptName = '" + strDeptName + "'";

                    DataTable dtFilteredData = dvData.ToTable();

                    DataRow drFilter = dtFilteredData.Rows[0];

                    strMailTo = (string.IsNullOrEmpty(strMailTo) ? "" : strMailTo + ",") + drFilter["SPOCEmailId"].ToString();
                    strMailTo = (string.IsNullOrEmpty(strMailTo) ? "" : strMailTo + ",") + drFilter["UHEmailId"].ToString();
                    strMailTo = (string.IsNullOrEmpty(strMailTo) ? "" : strMailTo + ",") + drFilter["FHEmailId"].ToString();

                    mail.ParamMap.Clear();
                    if (strType.Equals("STS"))
                        mail.ParamMap.Add("ConfigId", 1087); //Send to Stakeholder
                    else if (strType.Equals("MTCC"))
                        mail.ParamMap.Add("ConfigId", 1088); //Moved to Certification Chec

                    mail.ParamMap.Add("To", "ProvidedAsParam"); // Certification_Coordinator (SPOC), Certification_Unit_Head, Certification_Function_Head
                    mail.ParamMap.Add("cc", "CertCompUser,CertAdmin,CircularUser,CircularAdmin"); // Certification_Compliance_User, Certification_Admin, CirularUser, and CircularAdmin
                    mail.ParamMap.Add("ToEmailIds", strMailTo);
                    mail.ParamMap.Add("CirType", drFilter["CirType"].ToString());
                    mail.ParamMap.Add("CirSubject", drFilter["CirSubject"].ToString());
                    mail.ParamMap.Add("CirIssAuthority", drFilter["CirIssAuthority"].ToString());
                    mail.ParamMap.Add("CirDate", drFilter["CirDate"].ToString());
                    mail.ParamMap.Add("ApprovalType", strType);
                    mail.ParamMap.Add("LoggedInUserName", strLoggedInUser);
                    mail.setCircularMailContent(dtFilteredData);
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void gvCircularMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)(Session["CircCertChecklists"]);
                string strMidArray = "", strRemarks = "", strStatus = "";
                int intCntRec = dt.Rows.Count;

                if ((e.Row.RowType == DataControlRowType.Header))
                {
                    CheckBox HeaderLevelCheckBox = (CheckBox)(e.Row.FindControl("HeaderLevelCheckBox"));
                    HeaderLevelCheckBox.Attributes["onClick"] = "ChangeAllCheckBoxStates(this.checked);";
                    strMidArray = HeaderLevelCheckBox.ClientID;
                    Session["strMidArray"] = strMidArray;
                    Session["strRemarks"] = strRemarks;
                    Session["strStatus"] = strStatus;
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox RowLevelCheckBox = (CheckBox)(e.Row.FindControl("RowLevelCheckBox"));
                    TextBox txtComments = (TextBox)(e.Row.FindControl("txtComments"));
                    HiddenField hfStatus = (HiddenField)(e.Row.FindControl("hfStatus"));
                    HiddenField hfAuditTrail = (HiddenField)(e.Row.FindControl("hfAuditTrail"));
                    LinkButton lbAuditTrail = (LinkButton)(e.Row.FindControl("lbAuditTrail"));
                    
                    lbAuditTrail.Attributes["onClick"] = "return onViewChklistAuditTrailClick('" + hfAuditTrail.ClientID + "');";

                    RowLevelCheckBox.Attributes["onClick"] = "onRowCheckedUnchecked('" + RowLevelCheckBox.ClientID + "', '" + txtComments.ClientID + "');";

                    strMidArray = RowLevelCheckBox.ClientID;
                    strMidArray = "','" + strMidArray;

                    strRemarks = txtComments.ClientID;
                    strRemarks = "','" + strRemarks;

                    strStatus = hfStatus.ClientID;
                    strStatus = "','" + strStatus;

                    Session["strMidArray"] = Session["strMidArray"].ToString() + strMidArray;
                    Session["strRemarks"] = Session["strRemarks"].ToString() + strRemarks;
                    Session["strStatus"] = Session["strStatus"].ToString() + strStatus;
                    //<< Added by ramesh more on 14-Mar-2024 CR_1991
                    //hfCircCertChecklistId
                    //hfcmId
                    HiddenField hfCircCertChecklistId = (HiddenField)(e.Row.FindControl("hfCircCertChecklistId"));
                    HiddenField hfcmId = (HiddenField)(e.Row.FindControl("hfcmId"));
                    LinkButton lnkView = (LinkButton)(e.Row.FindControl("lnkView"));
                    lnkView.OnClientClick = "return showCircularGist('" + encdec.Encrypt(hfcmId.Value) + "','" + encdec.Encrypt(hfCircCertChecklistId.Value) + "');";
                    //>>
                }

                intCnt = intCnt + 1;
                if (intCnt == intCntRec + 1)
                    CheckBoxIDsArray.Text = "<script type='text/javascript'>" +
                                            "var CheckBoxIDs = new Array('" + Session["strMidArray"].ToString() + "');" +
                                            "var CommentsIDs = new Array('" + Session["strRemarks"].ToString() + "');" +
                                            "var StatusIDs = new Array('" + Session["strStatus"].ToString() + "');" +
                                            "</script>";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void gvCircularMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (hfSelectedOperation.Value.Equals("Edit"))
                {
                    string script = "window.open('AddCircularCertChecklists.aspx?CCCId=" + gvCircularMaster.SelectedValue.ToString() + "','_blank');";
                    ClientScript.RegisterStartupScript(this.GetType(), "AddCertChecklist", script, true);
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                bindGrid();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
    }
}