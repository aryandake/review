using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code;
using Microsoft.VisualBasic.FileIO;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_UploadChecklistData : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        CertificationMasterBL certBL = new CertificationMasterBL();
        ProcessBL procBL = new ProcessBL();
        DataSet data = new DataSet();
        string strFilePath = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["Type"] != null)
                {
                    hfType.Value = Request.QueryString["Type"].ToString();
                }

                if (Request.QueryString["CircId"] != null)
                {
                    hfCircId.Value = Request.QueryString["CircId"].ToString();
                }

                bool flag = false;

                if (hfType.Value.Equals("CERT") || (hfType.Value.Equals("CIRC") && (!hfCircId.Value.Equals("") && !hfCircId.Value.Equals("0"))))
                {
                    flag = true;
                }

                if (!flag)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Script", "alert('Invalid Type'); window.location.href = '" + Global.site_url() + "';", true);
                }

                string strDetails, strEmail;
                string[] strNameAndEmail = new string[0];
                Authentication auth = new Authentication();
                strDetails = auth.GetUserDetails(Page.User.Identity.Name);
                strNameAndEmail = strDetails.Split(',');
                strEmail = strNameAndEmail[1];
                string strName = strNameAndEmail[0];
                hfName.Value = Authentication.GetUserID(Page.User.Identity.Name);
                hfEmail.Value = strEmail;
            }
        }

        protected void btnAddAttachment_Click(object sender, EventArgs e)
        {
            lblvalidatedata.Text = "";
            lblvalidatedata.Text = "";
            if (SaveFile())
            {
                if (uploadIntoTempTable1("Temp_Cert_Data"))
                {
                    gvDataUpload.DataSource = null;
                    gvDataUpload.DataBind();
                    data = procBL.validateUploadedCertData(hfBatchId.Value, hfType.Value, hfCircId.Value, strConnectionString);
                    DataTable dt_validdata = new DataTable();
                    dt_validdata = data.Tables[0];
                    if (dt_validdata.Rows.Count > 0)
                    {
                        gvDataUpload.DataSource = dt_validdata;
                        gvDataUpload.DataBind();
                        btnSave.Visible = false;
                        writeError("There were validation failures for the uploaded excel. " +
                            "Kindly refer ERROR_MESSAGE column in the below grid. Kindly rectify and re-upload " +
                            "the excel.");
                    }
                    else
                    {
                        writeError("All the records are validated sucessfully. " +
                                "Please click on import button to import the records in the system.");
                        btnSave.Visible = true;
                    }
                }
            }
        }

        private bool SaveFile()
        {
            try
            {
                if (fuCertChklistUpload.HasFile)
                {
                    string strSelectedFile = fuCertChklistUpload.FileName;
                    string strDate = DateTime.Now.ToString("ddMMyyyyHHmmss");
                    if (strSelectedFile.Length > 50)
                    {
                        writeError("File Name Exceed 50 Characters");
                        return false;
                    }
                    else if (strSelectedFile.Contains("&"))
                    {
                        writeError("File Name can't have special character '&'.");
                        return false;
                    }
                    else if (strSelectedFile.Contains("\'"))
                    {
                        writeError("File Name can't have special character '.");
                        return false;
                    }
                    string strServerDirectory;

                    strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["CertificationFilesFolder"].ToString());
                    fuCertChklistUpload.SaveAs(strServerDirectory + "\\\\" + strDate + "_" + strSelectedFile);
                    strFilePath = strServerDirectory + "\\\\" + strDate + "_" + strSelectedFile;
                    return true;
                }
                else
                {
                    writeError("Please Select File To upload Data.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Error in File Save:" + ex);
                return false;
            }

        }

        private bool uploadIntoTempTable1(string TempTableName)
        {
            try
            {
                string strDoesDummyRowExist = "";
                hfBatchId.Value = Authentication.GetUserID(Page.User.Identity.Name) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss");

                int intNoOfFieldsLeft = 0, intRowLength = 0, intHeaderLength = 0,
                    intInnerRowLength = 0, intFieldCounter = 0;

                char strDelimiter;
                string strInsert = "", strCellContent = "";
                DataTable dtExcelData;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string[] strInnerRows;

                StreamReader sr = new StreamReader(strFilePath);
                TextFieldParser parser = new TextFieldParser(strFilePath);

                if (!txtDelimiter.Text.Equals(""))
                {
                    strDelimiter = Convert.ToChar(txtDelimiter.Text);
                }
                else
                {
                    strDelimiter = ',';
                }

                parser.Delimiters = new string[] { strDelimiter.ToString() };

                string[] headers = parser.ReadFields();
                intHeaderLength = headers.Length;
                dtExcelData = new DataTable();

                foreach (string header in headers)
                {
                    dtExcelData.Columns.Add(header);
                }

                Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                while (true)
                {
                    string str = sr.ReadLine();
                    string[] rows;
                    DataRow dr = dtExcelData.NewRow();

                    rows = parser.ReadFields();
                    if (rows == null)
                    {
                        break;
                    }
                    intRowLength = rows.Length;

                    for (int i = 0; i < rows.Length; i++)
                    {
                        rows[i] = rows[i].TrimStart(' ', '"');
                        rows[i] = rows[i].TrimEnd('"');
                        if (rows[i] == "")
                            rows[i] = null;
                    }

                    #region
                    if (intRowLength < intHeaderLength)
                    {
                        //this means the data is split over multiple lines.
                        for (int j = 0; j < intRowLength; j++)
                        {
                            dr[j] = rows[j];
                        }

                        //<<Here, we will need to continue reading next line.
                        //This loop will continue until all the fields have been read.
                        //blnContinueReading = true;
                        //+ 1 has been put in because there is a line break which has come in
                        //and the current field is yet to be read.
                        intNoOfFieldsLeft = intHeaderLength - intRowLength + 1;
                        intFieldCounter = intRowLength - 1;

                        while (intNoOfFieldsLeft != 0)
                        {
                            //Here, there is a past field, which is yet to be completely read.
                            //This would create problem when the line read contains a comma.
                            //For instance, if the string is:
                            //d

                            //"abcd,efgh"
                            //In the above mentioned case, it would be split up into 2 cells.
                            //abcd & efgh

                            strInnerRows = sr.ReadLine().Split(strDelimiter);
                            intInnerRowLength = strInnerRows.Length;

                            //<< This is specifically for the first cell.
                            strCellContent = strInnerRows[0];

                            dr[intFieldCounter] = dr[intFieldCounter] + Environment.NewLine + strCellContent;

                            if (strCellContent.EndsWith("\""))
                            {
                                intNoOfFieldsLeft = intNoOfFieldsLeft - 1;
                                intFieldCounter = intFieldCounter + 1;
                            }
                            //>>

                            for (int k = 1; k < intInnerRowLength; k++)
                            {
                                strCellContent = strInnerRows[k];
                                dr[intFieldCounter] = strCellContent;

                                if (!strCellContent.StartsWith("\""))
                                {
                                    intNoOfFieldsLeft = intNoOfFieldsLeft - 1;
                                    intFieldCounter = intFieldCounter + 1;
                                }
                                else if (strCellContent.StartsWith("\"") && strCellContent.EndsWith("\""))
                                {
                                    intNoOfFieldsLeft = intNoOfFieldsLeft - 1;
                                    intFieldCounter = intFieldCounter + 1;
                                }
                            }
                        }
                        //>>
                    }
                    else
                    {
                        //this means the data is not split over multiple lines.
                        for (int j = 0; j < headers.Length; j++)
                        {
                            strCellContent = rows[j];
                            dr[j] = strCellContent;
                        }
                    }
                    #endregion

                    dtExcelData.Rows.Add(dr);
                }

                for (int i = 0; i < dtExcelData.Rows.Count; i++)
                {
                    strInsert = strInsert + "Insert into [" + TempTableName + "]" +
                               "([BatchId], " +
                               "[SrNo], " +
                               "[DepartmentName], " +
                               "[Act/Regulation/Circular], " +
                               "[Section/Regulation Rule/Circulars], " +
                               "[Title Of Section/Requirement], " +
                               "[Checkpoint], " +
                               "[Self Assessment Status], " +
                               "[Penalty], " +
                               "[Time Limit], " +
                               "[Forms], " +
                               "[Start Date], " +
                               "[End Date] " +
                               ") values('" +
                               hfBatchId.Value + "','" +
                               dtExcelData.Rows[i]["Sr No"].ToString() + "','" +
                               dtExcelData.Rows[i]["Department Name"].ToString() + "','" +
                               dtExcelData.Rows[i]["Act/Regulation/Circular"].ToString().Replace("\"", "").Replace("'", "''") + "','" +
                               dtExcelData.Rows[i]["Reference Circular/Notification/Act"].ToString().Replace("\"", "").Replace("'", "''") + "','" +
                               dtExcelData.Rows[i]["Section/Clause"].ToString().Replace("\"", "").Replace("'", "''") + "','" +
                               dtExcelData.Rows[i]["Compliance of/Heading of Compliance checklist"].ToString().Replace("\"", "").Replace("'", "''") + "','" +
                               dtExcelData.Rows[i]["Description"].ToString().Replace("\"", "").Replace("'", "''") + "','" +
                               dtExcelData.Rows[i]["Consequences of non Compliance"].ToString().Replace("\"", "").Replace("'", "''") + "','" +
                               dtExcelData.Rows[i]["Frequency"].ToString().Replace("\"", "").Replace("'", "''") + "','" +
                               dtExcelData.Rows[i]["Forms"].ToString().Replace("\"", "").Replace("'", "''") + "','" +
                               dtExcelData.Rows[i]["Effective From"].ToString() + "','" +
                               dtExcelData.Rows[i]["Effective To"].ToString() + "');";
                }

                string sqlTruncate = " truncate table " + TempTableName + "; " + strInsert;
                F2FDatabase.F2FExecuteNonQuery(sqlTruncate);
                sr.Close();
                return true;
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("In uploadIntoTempTable1: " + ex.Message);
                return false;
            }
        }

        private void writeError(string strError)
        {
            lblvalidatedata.Text = strError;
            lblvalidatedata.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveToFinalTable())
            {
                writeError("Data Uploaded Successfully...");
            }
            else
            {
                writeError("Data Not Uploaded Successfully...");
            }
            btnSave.Visible = false;
        }

        private bool SaveToFinalTable()
        {
            int intCircId = 0;
            bool res = int.TryParse(hfCircId.Value, out intCircId);

            try
            {
                procBL.uploadChecklistData(hfBatchId.Value, hfType.Value, intCircId, new Authentication().getUserFullName(Page.User.Identity.Name));
                return true;
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("In SaveToFinalTable: " + ex.Message);
                return false;
            }
        }
    }
}