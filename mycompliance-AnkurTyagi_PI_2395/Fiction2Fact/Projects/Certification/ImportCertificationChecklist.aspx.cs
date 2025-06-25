using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Data.OleDb;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_ImportCertificationChecklist : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        string FileName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //<<Added by Ankur Tyagi on 19Mar2024 for CR_1991
                if (Page.User.Identity.Name.Equals(""))
                {
                    Response.Redirect(Global.site_url("Login.aspx"));
                    return;
                }
                //>>
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
            //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
            //>>
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Page != null)
            {
                Page.RegisterRequiresViewStateEncryption();
            }
        }


        public void btnAddAttachment_Click(object sender, EventArgs e)
        {
            if (fuCheckData.HasFile)
            {
                if (uploadFileToServer())
                {
                    DataTable dt = new DataTable();
                    DataSet ds = new DataSet();
                    if (saveToTempTable(FileName))
                    {
                        ds = validateUploadedData(mstrConnectionString);
                        dt = ds.Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            writeError("Data validated Successfully....." + Environment.NewLine + "Kindly Click on Save Button to complete import.");
                            btnSave.Visible = true;
                            grdData.DataSource = dt;
                            grdData.DataBind();
                        }
                        else
                        {
                            writeError("Error in Data validation....." + Environment.NewLine + "Please refer for Error Message column.");
                            btnSave.Visible = false;
                            grdData.DataSource = dt;
                            grdData.DataBind();
                            deletefromTempTable("Temp_Certification_Checklist_Import", hfUserName.Value, hfDate.Value);
                        }
                    }
                    else
                    {
                        writeError("Error in Data Import to Temp Table.....");
                        btnSave.Visible = false;
                    }
                }

            }
            else
            {
                writeError("Please select a file for import.....");
                btnSave.Visible = false;
                return;
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
            catch (Exception)
            {
                writeError("Error in File save on to the server.....");
                btnSave.Visible = false;
                return false;
            }
        }

        public bool saveToTempTable(string strFileName)
        {
            try
            {
                string query = "";
                string strTempTableName = "";
                strTempTableName = "Temp_Certification_Checklist_Import";
                query = "SELECT * from [Sheet1$]";

                string conString = "";
                conString = "Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + strFileName + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";


                OleDbConnection con = new OleDbConnection(conString);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                OleDbCommand cmd = new OleDbCommand(query, con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                string strInsert = "";
                string strID = "";
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt_s = new DataTable();
                dt_s = ds.Tables[0];
                dt_s.Rows[0].Delete();
                dt_s.AcceptChanges();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    //strInsert = strInsert + "Insert into [" + strTempTableName + "]  " +
                    //    " ([SrNo],[Reference], [Clause] , [Particulars] , [Checkpoints] , [Frequency],[Due Date],[Source Department],[Department responsible for furnishing the data]," +
                    //    " [Department responsible for submitting it],[To be filed with],[YesNoNa],[Remarks],[Department_ID],[ChecklistDetsId]," +
                    //    " [UPLOAD_USER_NAME], [UPLOAD_TIME]) " +
                    //    " values " +
                    //    " (" + dr["SrNo"].ToString().Replace("'", "''") + ",'" + dr["Reference"].ToString().Replace("'", "''") + "','" + dr["Clause"].ToString().Replace("'", "''") + "','"
                    //    + dr["Particulars"].ToString().Replace("'", "''") + "', '" + dr["Checkpoints"].ToString().Replace("'", "''") + "','" + dr["Frequency"].ToString().Replace("'", "''") + "','"
                    //    + dr["Due Date"].ToString().Replace("'", "''") + "', '" + dr["Source Department"].ToString().Replace("'", "''") + "','" + dr["Department responsible for furnishing the data"].ToString().Replace("'", "''") + "','"
                    //    + dr["Department responsible for submitting it"].ToString().Replace("'", "''") + "', '" + dr["To be filed with"].ToString().Replace("'", "''") + "','" + dr["Select"].ToString().Replace("'", "''") + "','"
                    //    + dr["Remarks"].ToString().Replace("'", "''") + "', '" + dr["DepartmentId"].ToString().Replace("'", "''") + "', '" + dr["CheklistDetsId"].ToString().Replace("'", "''") +
                    //    "' ,'" + hfUserName.Value + "','" + hfDate.Value + "')";

                    strInsert = strInsert + "Insert into [" + strTempTableName + "]  " +
                       " ([SrNo],[Reference], [Clause] , [Particulars] , [Frequency]," +
                       " [To be filed with],[YesNoNa],[Remarks],[Department_ID],[ChecklistMasId],[ChecklistDetsId]," +
                       " [UPLOAD_USER_NAME], [UPLOAD_TIME]) " +
                       " values " +
                       " (" + dr["SrNo"].ToString().Replace("'", "''") + ",'" + dr["Reference"].ToString().Replace("'", "''") + "','" + dr["Clause"].ToString().Replace("'", "''") + "','"
                       + dr["Particulars"].ToString().Replace("'", "''") + "','" + dr["Frequency"].ToString().Replace("'", "''") + "','"
                       + dr["To be filed with"].ToString().Replace("'", "''") + "','" + dr["Compliance Status"].ToString().Replace("'", "''") + "','"
                       + dr["Remarks"].ToString().Replace("'", "''") + "', '" + dr["DepartmentId"].ToString().Replace("'", "''") + "', '"
                       + dr["ChecklistMasId"].ToString().Replace("'", "''") + "','" + dr["ChecklistDetsId"].ToString().Replace("'", "''") + "','"
                       + hfUserName.Value + "','" + hfDate.Value + "')";

                    strID = strID + dr["ChecklistMasId"].ToString() + ",";
                }
                da.Dispose();
                con.Close();
                con.Dispose();
                strID = strID.Remove(strID.Length - 1);
                hfAuditIssueId.Value = strID;
                F2FDatabase.F2FExecuteNonQuery(strInsert);
                return true;

            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("In saveToTempTable: " + ex.Message);
                return false;
            }
        }


        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        public DataSet validateUploadedData(string mstrConnectionString)
        {
            DataSet ds = new DataSet();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "CERT_validateImportCertificationChecklistData";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@UploadName", F2FDatabase.F2FDbType.VarChar, hfUserName.Value));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@UploadTime", F2FDatabase.F2FDbType.VarChar, hfDate.Value));
                    DB.F2FDataAdapter.Fill(ds);
                }

                return ds;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("system exception in validateUploadedIssueData() " + ex);
            }
        }

        public void deletefromTempTable(string strTempTableName, string strUserName, string strUploadTime)
        {
            string strInsert = "";
            strInsert = "Delete from  " + strTempTableName +
              " where  " + strTempTableName + ".UPLOAD_USER_NAME = '" + strUserName + "' " +
              " and " + strTempTableName + ".UPLOAD_TIME = '" + strUploadTime + "'";
            F2FDatabase.F2FExecuteNonQuery(strInsert);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (saveToFinalTable(hfUserName.Value, hfDate.Value, hfAuditIssueId.Value))
            {
                writeError("Data Imported Successfully.....");
            }
            else
            {
                writeError("Error in Data Import.....");
            }
            deletefromTempTable("Temp_Certification_Checklist_Import", hfUserName.Value, hfDate.Value);

            btnSave.Visible = false;
            btnClose.Visible = true;
        }

        public bool saveToFinalTable(string strUserName, string strUploadTime, string strIssueID)
        {
            try
            {
                string strInsert = "";
                string strTempTableName = "";
                strTempTableName = "Temp_Certification_Checklist_Import";
                strInsert = strInsert + "UPDATE [TBL_CERT_CHECKLIST_DETS] SET " +
                  //" TBL_CERT_CHECKLIST_DETS.CCD_YES_NO_NA = " + strTempTableName + ".YesNoNa, " +
                  " TBL_CERT_CHECKLIST_DETS.CCD_YES_NO_NA = " + strTempTableName + ".ComplianceStatusValue, " +
                  " TBL_CERT_CHECKLIST_DETS.CCD_REMARKS = " + strTempTableName + ".Remarks," +
                  " TBL_CERT_CHECKLIST_DETS.CCD_LST_UPD_BY = " + strTempTableName + ".UPLOAD_USER_NAME, " +
                  " TBL_CERT_CHECKLIST_DETS.CCD_LST_UPD_DT = " + strTempTableName + ".UPLOAD_TIME " +

                  " from " + strTempTableName +
                  " where TBL_CERT_CHECKLIST_DETS.CCD_ID = " + strTempTableName + ".ChecklistDetsId and "
                  + strTempTableName + ".UPLOAD_USER_NAME = '" + strUserName + "' " +
                  " and " + strTempTableName + ".UPLOAD_TIME = '" + strUploadTime + "';";


                //strInsert = strInsert + "UPDATE [TBL_CERT_CHECKLIST_DETS] SET " +
                // " TBL_CERT_CHECKLIST_DETS.CCD_YES_NO_NA = " + strTempTableName + ".YesNoNa, " +
                // " TBL_CERT_CHECKLIST_DETS.CCD_REMARKS = " + strTempTableName + ".Remarks," +
                // " TBL_CERT_CHECKLIST_DETS.CCD_LST_UPD_BY = " + strTempTableName + ".UPLOAD_USER_NAME, " +
                // " TBL_CERT_CHECKLIST_DETS.CCD_LST_UPD_DT = " + strTempTableName + ".UPLOAD_TIME " +

                // " from " + strTempTableName +
                // " where TBL_CERT_CHECKLIST_DETS.CCD_ID = " + strTempTableName + ".ChecklistDetsId and "
                // + strTempTableName + ".UPLOAD_USER_NAME = '" + strUserName + "' " +
                // " and " + strTempTableName + ".UPLOAD_TIME = '" + strUploadTime + "'";
                F2FDatabase.F2FExecuteNonQuery(strInsert);
                return true;
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("In saveToFinalTable : " + ex.Message);
                return false;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            deletefromTempTable("Temp_Certification_Checklist_Import", hfUserName.Value, hfDate.Value);
            string strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                   "window.close();\r\n" +
                       "</script>\r\n";
            ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);
        }
    }
}