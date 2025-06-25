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
using System.Data.OleDb;
using System.IO;
using System.Text;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;

namespace Fiction2Fact.Projects.Certification
{
    public partial class ImportComplianceChecklist : System.Web.UI.Page
    {
        //<< Code Added by Ramesh More on 04-Jul-2024 CR_2114
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        private string mstrDummyString = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
        string FileName = "";
        string strBatchId = "";
        ProcessBL pBL = new ProcessBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.User.Identity.Name.Equals(""))
                {
                    Response.Redirect(Global.site_url("Login.aspx"));
                    return;
                }
                if (!IsPostBack)
                {
                    if (Session["UserName"] != null)
                    {
                        hfUserName.Value = Page.User.Identity.Name;
                        hfDate.Value = System.DateTime.Now.ToString("dd-MMM-yyy hh:mm:ss");
                    }
                    else
                    {
                        string strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                       "alert('Your Session is Expire. Please Logout and Login Again');\r\nwindow.close();\r\n" +
                           "</script>\r\n";
                        ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);
                    }
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

        public void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuCheckData.HasFile)
                {
                    if (uploadFileToServer())
                    {
                        strBatchId = Authentication.GetUserID(Page.User.Identity.Name) + "_" + System.DateTime.Now.ToString("ddMMyyyyHHmmss");
                        hfBatchId.Value = strBatchId.ToString();
                        DataTable dt = new DataTable();
                        if (saveToTempTable(FileName, strBatchId))
                        {
                            dt = pBL.validateUploadedData(strBatchId, mstrConnectionString);
                            if (dt.Rows.Count == 0)
                            {
                                writeError("Data validated Successfully." + Environment.NewLine + " Kindly click on Save button to complete import.");
                                btnSave.Visible = true;
                                gvData.DataSource = dt;
                                gvData.DataBind();
                            }
                            else
                            {
                                writeError("Error in Data validation." + Environment.NewLine + " Please refer for Error Message column.");
                                btnSave.Visible = false;
                                gvData.DataSource = dt;
                                gvData.DataBind();
                            }
                        }
                        else
                        {
                            writeError("Error in Data Import to Temp Table.");
                            btnSave.Visible = false;
                        }
                    }
                }
                else
                {
                    writeError("Please select a file for import.");
                    btnSave.Visible = false;
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private bool uploadFileToServer()
        {
            try
            {

                DateTime dtUploadDatetime = System.DateTime.Now;
                string strSelectedFile = Authentication.GetUserID(Page.User.Identity.Name) + "_" + dtUploadDatetime.ToString("ddMMyyyyHHmmss") + "_" + fuCheckData.FileName;
                string strServerDirectory = "";
                strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["CertificationChecklistUpload"].ToString());
                FileName = strServerDirectory + "\\" + strSelectedFile;
                fuCheckData.SaveAs(FileName);
                btnSave.Visible = false;
                return true;
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
                btnSave.Visible = false;
                return false;
            }
        }
        public string getConStringForDownloadExcelFile(string strCompleteFileName, string strFileExtension)
        {
            string strMachineConfiguration = "", conString = "";
            strMachineConfiguration = (ConfigurationManager.AppSettings["MachineConfiguration"].ToString());

            if (strMachineConfiguration.Equals("32bit"))
            {
                if (strFileExtension.ToLower().Equals(".xls"))
                {
                    conString = "Provider=Microsoft.JET.OLEDB.4.0;" +
                     "Data Source=" + strCompleteFileName + ";" +
                     "Extended Properties='Excel 8.0;HDR=YES;'";
                }
                else if (strFileExtension.ToLower().Equals(".xlsx"))
                {
                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                     "Data Source=" + strCompleteFileName + ";" +
                     "Extended Properties='Excel 12.0;HDR=YES;'";
                }
            }
            else if (strMachineConfiguration.Equals("64bit"))
            {
                conString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                      "Data Source=" + strCompleteFileName + ";" +
                      "Extended Properties='Excel 12.0;HDR=YES;'";
            }

            return conString;
        }

        public bool saveToTempTable(string strFileName, string strBatchId)
        {
            try
            {
                string query = "";
                string strTempTableName = "";
                string strDoesDummyRowExist = "";
                StringBuilder sbQuery = new StringBuilder();
                strTempTableName = "TBL_TEMP_COMPLIANCE_CHECKLIST";
                sbQuery.Append(" Select ");
                sbQuery.Append(" [Serial Number] ");
                sbQuery.Append(" ,[ChecklistDets Id] ");
                sbQuery.Append(" ,[ChecklistMas Id] ");
                sbQuery.Append(" ,[Act/Regulation/Circular] ");
                sbQuery.Append(" ,[Reference Circular/Notification/Act] ");//
                sbQuery.Append(" ,[Section/Clause] ");
                sbQuery.Append(" ,[Compliance of/Heading of Compliance checklist] ");
                sbQuery.Append(" ,[Description] ");
                sbQuery.Append(" ,[Consequences of non Compliance] ");
                sbQuery.Append(" ,[Frequency] ");
                sbQuery.Append(" ,[Forms] ");
                sbQuery.Append(" ,[Compliance Status] ");
                sbQuery.Append(" ,[Remarks / Reason of non compliance] ");
                sbQuery.Append(" ,[Non-compliant since] ");
                sbQuery.Append(" ,[Action Plan] ");
                sbQuery.Append(" ,[Target Date] ");
                //sbQuery.Append(" ,[Checklist File] ");
                sbQuery.Append(" From [Sheet1$] ");
                string conString = "";
                string ext = Path.GetExtension(strFileName);
                conString = getConStringForDownloadExcelFile(strFileName, ext);

                OleDbConnection con = new OleDbConnection(conString);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                OleDbCommand cmd = new OleDbCommand(sbQuery.ToString(), con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                string strInsert = "";
                string strID = "";
                DataTable dt_s = new DataTable();
                da.Fill(dt_s);
                if (dt_s.Rows.Count > 0)
                {
                    DataRow dr = dt_s.Rows[0];

                    if (dr["Act/Regulation/Circular"].ToString().Equals(mstrDummyString))
                        strDoesDummyRowExist = "true";
                    else
                        strDoesDummyRowExist = "false";
                }

                if (strDoesDummyRowExist.Equals("false"))
                {
                    writeError("Please insert a dummy row at the second row of your excel (download the sample file for your reference which contains dummy row) so as to allow upload of more than 255 characters.");
                    return false;
                }
                else
                {
                    for (int i = 1; i < dt_s.Rows.Count; i++)
                    {
                        string strNonCompDate = null, strTargetDate = null;
                        DataRow dr = dt_s.Rows[i];

                        if (!string.IsNullOrEmpty(dr["Non-compliant since"].ToString()))
                            strNonCompDate = dr["Non-compliant since"].ToString();

                        if (!string.IsNullOrEmpty(dr["Target Date"].ToString()))
                            strTargetDate = dr["Target Date"].ToString();

                        strInsert = strInsert + "Insert into [" + strTempTableName + "]  " +
                           " ([BatchId]," +
                           " [Serial Number]," +
                           " [ChecklistDetsId] ," +
                           " [ChecklistMasId] ," +
                           " [Act/Regulation/Circular]," +
                           " [Reference Circular/Notification/Act]," +
                           " [Section/Clause]," +
                           " [Compliance of/Heading of Compliance checklist]," +
                           " [Description]," +
                           " [Consequences of non Compliance]," +
                           " [Frequency]," +
                           " [Forms]," +
                           " [Compliance Status]," +
                           " [Remarks / Reason of non compliance]," +
                           " [Non-compliant since]," +
                           " [Action Plan]," +
                           " [Target Date])" +
                           //" [Checklist File] ) " +
                           " values " +
                           " ('" + strBatchId + "'," +
                           " '" + dr["Serial Number"].ToString().Replace("'", "''") + "'," +
                           " '" + dr["ChecklistDets Id"].ToString().Replace("'", "''") + "'," +
                           " '" + dr["ChecklistMas Id"].ToString().Replace("'", "''") + "'," +
                           " '" + dr["Act/Regulation/Circular"].ToString().Replace("'", "''") + "'," +
                           " '" + dr["Reference Circular/Notification/Act"].ToString().Replace("'", "''") + "'," +
                           " '" + dr["Section/Clause"].ToString().Replace("'", "''") + "'," +
                           " '" + dr["Compliance of/Heading of Compliance checklist"].ToString().Replace("'", "''") + "'," +
                           " '" + dr["Description"].ToString().Replace("'", "''") + "', " +
                           " '" + dr["Consequences of non Compliance"].ToString().Replace("'", "''") + "'," +
                           " '" + dr["Frequency"].ToString().Replace("'", "''") + "'," +
                           " '" + dr["Forms"].ToString().Replace("'", "''") + "'," +
                           " '" + dr["Compliance Status"].ToString().Replace("'", "''") + "'," +
                           " '" + dr["Remarks / Reason of non compliance"].ToString().Replace("'", "''") + "',";
                        if (strNonCompDate != null)
                        {
                            strInsert = strInsert + " '" + dr["Non-compliant since"].ToString().Replace("'", "''") + "',";
                        }
                        else
                        {
                            strInsert = strInsert + "null,";
                        }
                        strInsert = strInsert + " '" + dr["Action Plan"].ToString().Replace("'", "''") + "',";
                        if (strTargetDate != null)
                        {
                            strInsert = strInsert + " '" + dr["Target Date"].ToString().Replace("'", "''") + "')";
                        }
                        else
                        {
                            strInsert = strInsert + "null)";
                        }
                        //strInsert = strInsert + " '" + dr["Checklist File"].ToString().Replace("'", "''") + "')";

                    }
                    da.Dispose();
                    con.Close();
                    con.Dispose();
                    F2FDatabase.F2FExecuteNonQuery(strInsert);
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string strLoggedUser = Page.User.Identity.Name;
                pBL.uploadCertChecklistData(hfBatchId.Value, strLoggedUser);
                writeError("Data Imported Successfully.....");
                btnSave.Visible = false;
                btnClose.Visible = true;
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        //>>
    }
}