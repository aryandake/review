using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.IO;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.App_Code;
using System.Data.SqlClient;

namespace Fiction2Fact.Projects
{
    public partial class DeleteDBRecords : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        UtilitiesBLL utilBL = new UtilitiesBLL();

        protected void Page_Load(object sender, EventArgs e)
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
                try
                {
                    string strMsg = "", strQuery2 = "";
                    string strCalledFrom = Request.QueryString["calledFrom"];
                    string strIds = Request.QueryString["Ids"];
                    if (strIds.Contains(","))
                    {
                        int intLastInd = strIds.LastIndexOf(',');
                        strIds = strIds.Remove(intLastInd);
                    }
                    string strQuery = "";
                    //if (strCalledFrom.Equals("Exception"))
                    //{
                    //    string strServerDirectory, strExistingFileName, strCompleteFileName = null;
                    //    strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["CertificationFilesFolder"].ToString());
                    //    DataTable dtFiles = utilBL.getDatasetWithCondition("getCertException", Convert.ToInt32(strIds),
                    //                mstrConnectionString);
                    //    FileInfo fileInfo;
                    //    foreach (DataRow dr1 in dtFiles.Rows)
                    //    {
                    //        strExistingFileName = dr1["CE_SERVER_FILE_NAME"].ToString();
                    //        strCompleteFileName = strServerDirectory + "\\" + strExistingFileName;
                    //        fileInfo = new FileInfo(strCompleteFileName);
                    //        if (fileInfo.Exists)
                    //        {
                    //            fileInfo.Delete();
                    //        }
                    //    }
                    //    utilBL.getDatasetWithConditionInString("deleteExceptions", strIds, mstrConnectionString);
                    //}
                    if (strCalledFrom.Equals("Exception"))
                    {
                        string strServerDirectory, strExistingFileName, strCompleteFileName = null;
                        strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["CertificationFilesFolder"].ToString());

                        DataTable dtFiles = new DataTable();

                        using (F2FDatabase DB = new F2FDatabase())
                        {
                            strQuery2 = "select * FROM TBL_CERT_EXCEPTION WHERE CE_ID " +
                                    "  in (" + strIds + ");";
                            DB.F2FCommand.CommandText = strQuery2;
                            DataSet ds = new DataSet();
                            DB.F2FDataAdapter.Fill(ds);
                            dtFiles = ds.Tables[0];
                        }

                        FileInfo fileInfo;
                        foreach (DataRow dr1 in dtFiles.Rows)
                        {
                            if (!string.IsNullOrEmpty(dr1["CE_SERVER_FILE_NAME"].ToString()))
                            {
                                strExistingFileName = dr1["CE_SERVER_FILE_NAME"].ToString();
                                strCompleteFileName = strServerDirectory + "\\" + strExistingFileName;
                                fileInfo = new FileInfo(strCompleteFileName);
                                if (fileInfo.Exists)
                                {
                                    fileInfo.Delete();
                                }
                            }
                        }

                        utilBL.getDatasetWithConditionInString("deleteExceptions", strIds, null);
                    }
                    //<<Added by Rahuldeb for CR_319 on 04Jan2021
                    else if (strCalledFrom.Equals("CertificationHeadSpoc"))
                    {
                        if (!checkDependencyPriorDeletion(strCalledFrom, strIds))
                            strQuery = "DELETE FROM TBL_CERT_SUB_SUB_DEPT_MAS where CSSDM_ID in (" + strIds + ")";
                        else
                        {
                            string script = "\r\n<script language=\"javascript\">\r\n" +
                               "alert('There is dependency on another master for the same. Hence, cannot be deleted.');window.close();" +
                               "</script>\r\n";
                            ClientScript.RegisterStartupScript(this.GetType(), "script", script);
                            return;
                        }
                    }
                    //>>
                    if (strCalledFrom.Equals("CircularAttachment"))
                    {
                        string strServerDirectory, strExistingFileName, strCompleteFileName = null;
                        strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["CircularFilesFolder"].ToString());
                        DataTable dtFiles = utilBL.getDatasetWithConditionInString("CircularAttachments", strIds,
                                    null);
                        FileInfo fileInfo;
                        foreach (DataRow dr in dtFiles.Rows)
                        {
                            strExistingFileName = dr["CF_SERVERFILENAME"].ToString();
                            strCompleteFileName = strServerDirectory + "\\" + strExistingFileName;
                            fileInfo = new FileInfo(strCompleteFileName);
                            if (fileInfo.Exists)
                            {
                                fileInfo.Delete();
                            }
                        }
                        strQuery = "DELETE FROM TBL_CIRCULAR_FILES WHERE CF_ID in (" + strIds + ");";
                    }
                    if (strCalledFrom.Equals("ContractDirectorDetailsId"))
                    {
                        strQuery = "DELETE FROM TBL_CON_VENDOR_RELATED_PARTY_DETS WHERE CVRPD_ID in (" + strIds + ");";
                    }
                    if (strCalledFrom.Equals("ContractDepartmentDetailsId"))
                    {
                        strQuery = "DELETE FROM TBL_CT_APPROVING_DEPT WHERE CAD_ID in (" + strIds + ");";
                    }


                    if (strMsg.Equals(""))
                    {
                        if (!strQuery.Equals(""))
                        {
                            using (F2FDatabase DB = new F2FDatabase(strQuery))
                            {
                                DB.OpenConnection();
                                DB.F2FCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    if (strMsg.Equals(""))
                    {
                        string script = "\r\n<script language=\"javascript\">\r\n" +
                                  //" alert('Record(s) deleted successfully.');window.close();" +
                                  " window.close();" +
                                  "   </script>\r\n";
                        ClientScript.RegisterStartupScript(this.GetType(), "script", script);
                    }
                    else
                    {
                        string script = "\r\n<script language=\"javascript\">\r\n" +
                                    " alert('Certification department details cannot be deleted because there are certification(s) associated with it.');window.close();" +
                                    "   </script>\r\n";
                        ClientScript.RegisterStartupScript(this.GetType(), "script", script);
                    }
                }
                catch (Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    writeError("Exception in delete():" + ex);
                }
                finally
                {
                    //myconnection.Close();
                }
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


        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }


        //<<Added by Rahuldeb for CR_319 on 04Jan2021
        private bool checkDependencyPriorDeletion(string strCalledFrom, string strIds)
        {
            bool retval = true;
            string strCount = "", strQuery = "";
            try
            {
                using (SqlConnection con = new SqlConnection(mstrConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;

                    if (strCalledFrom.Equals("CertificationHeadSpoc"))
                    {
                        strQuery = "select " +
                           " (select Count(1) from TBL_CERT_MAS where CERTM_LEVEL_ID = 0 and CERTM_DEPT_ID in (" + strIds + ")) + " +
                           " (Select Count(1) from TBL_CERT_CHECKLIST_MAS where CCM_CSSDM_ID in (" + strIds + ")) + " +
                           " (Select Count(1) from TBL_SRD_CSSDM_MAPPING where SRCM_CSSDM_ID in (" + strIds + ")) ";
                    }

                    cmd.CommandText = strQuery;
                    con.Open();
                    strCount = cmd.ExecuteScalar().ToString();

                    if (strCount.Equals("0"))
                    {
                        retval = false;
                    }
                    else
                    {
                        retval = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("System Exception in checkDependencyPriorDeletion():" + ex);
            }

            return retval;
        }
        //>>

    }
}