using System;
using System.Web;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Services;
using AjaxControlToolkit;
using System.Data;
using System.Configuration;
using Fiction2Fact.Legacy_App_Code.BLL;

/// <summary>
/// </summary>
namespace Fiction2Fact.Legacy_App_Code.Certification
{
    [WebService(Namespace = "Fiction2Fact.Legacy_App_Code.Certification")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

    [System.Web.Script.Services.ScriptService]
    public class AJAXDropdownCertification : System.Web.Services.WebService
    {
        private string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ToString();
        UtilitiesBLL UtilitiesBL = new UtilitiesBLL();
        public AJAXDropdownCertification()
        {

        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetZone(
          string knownCategoryValues,
          string category)
        {

            DataSet dsZone = UtilitiesBL.getDataset("Zone", mstrConnectionString);
            DataTable dtZone = dsZone.Tables[0];
            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            foreach (DataRow dr in dtZone.Rows)
            {
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["ZM_NAME"], dr["ZM_ID"].ToString()));
            }
            return values.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetRegion(
          string knownCategoryValues,
          string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(
              knownCategoryValues);
            int ZoneId;
            DataTable dtSubArea = new DataTable();
            DataRow dr;
            if (!kv.ContainsKey("Zone") ||
                !Int32.TryParse(kv["Zone"], out ZoneId))
            {
                return null;
            }
            dtSubArea = UtilitiesBL.getDatasetWithCondition("REGION", ZoneId, mstrConnectionString);
            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            for (int i = 0; i < dtSubArea.Rows.Count; i++)
            {
                dr = dtSubArea.Rows[i];
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["RM_NAME"], dr["RM_ID"].ToString()));
            }
            return values.ToArray();
        }


        [WebMethod]
        public CascadingDropDownNameValue[] GetTerritory(
          string knownCategoryValues,
          string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(
              knownCategoryValues);
            int Id;
            DataTable dtSubArea = new DataTable();
            DataRow dr;
            if (!kv.ContainsKey("Region") ||
                !Int32.TryParse(kv["Region"], out Id))
            {
                return null;
            }
            dtSubArea = UtilitiesBL.getDatasetWithCondition("TERRITORY", Id, mstrConnectionString);
            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            for (int i = 0; i < dtSubArea.Rows.Count; i++)
            {
                dr = dtSubArea.Rows[i];
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["TM_NAME"], dr["TM_ID"].ToString()));
            }
            return values.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetBranch(
          string knownCategoryValues,
          string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(
              knownCategoryValues);
            int Id;
            DataTable dtSubArea = new DataTable();
            DataRow dr;
            if (!kv.ContainsKey("Zone") ||
                !Int32.TryParse(kv["Zone"], out Id))
            {
                return null;
            }
            dtSubArea = UtilitiesBL.getDatasetWithCondition("BRANCH", Id, mstrConnectionString);
            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            for (int i = 0; i < dtSubArea.Rows.Count; i++)
            {
                dr = dtSubArea.Rows[i];
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["BM_NAME"], dr["BM_ID"].ToString()));
            }
            return values.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetArea(
          string knownCategoryValues,
          string category)
        {

            DataSet dsArea = UtilitiesBL.getDataset("AREA", mstrConnectionString);
            DataTable dtArea = dsArea.Tables[0];
            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            foreach (DataRow dr in dtArea.Rows)
            {
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["CAM_NAME"], dr["CAM_ID"].ToString()));
            }
            return values.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetSubAreaForArea(
          string knownCategoryValues,
          string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(
              knownCategoryValues);
            int CircularAreaId;
            DataTable dtSubArea = new DataTable();
            DataRow dr;
            if (!kv.ContainsKey("CircularArea") ||
                !Int32.TryParse(kv["CircularArea"], out CircularAreaId))
            {
                return null;
            }
            dtSubArea = UtilitiesBL.getDatasetWithCondition("SUBAREA", CircularAreaId, mstrConnectionString);
            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            for (int i = 0; i < dtSubArea.Rows.Count; i++)
            {
                dr = dtSubArea.Rows[i];
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["CSM_NAME"], dr["CSM_ID"].ToString()));
            }
            return values.ToArray();
        }

        [WebMethod]
        public string[] GetCircularNo(string prefixText)
        {
            DataTable dtRecords;
            string strSQL;

            strSQL = "Select CM_CIRCULAR_NO from TBL_CIRCULAR_MASTER where CM_CIRCULAR_NO like '" + prefixText + "%'";

            dtRecords = new DataServer().Getdata(strSQL);
            String[] strRecords = new string[dtRecords.Rows.Count];
            int i = 0;
            foreach (DataRow row in dtRecords.Rows)
            {
                strRecords[i] = Convert.ToString(row["CM_CIRCULAR_NO"]);

                i = i + 1;

            }
            return strRecords;
        }

        [WebMethod]
        public string[] GetTopic(string prefixText)
        {
            DataTable dtRecords;
            string strSQL;

            strSQL = "Select CM_TOPIC from TBL_CIRCULAR_MASTER where CM_TOPIC like '" + prefixText + "%'";

            dtRecords = new DataServer().Getdata(strSQL);
            String[] strRecords = new string[dtRecords.Rows.Count];
            int i = 0;
            foreach (DataRow row in dtRecords.Rows)
            {
                strRecords[i] = Convert.ToString(row["CM_TOPIC"]);

                i = i + 1;

            }
            return strRecords;
        }

        [WebMethod]
        public string[] GetIssuingAuthority(string prefixText)
        {
            DataTable dtRecords;
            string strSQL;

            strSQL = "Select CIA_NAME from TBL_CIRCULAR_ISSUING_AUTHORITIES where CIA_NAME like '" + prefixText + "%'";

            dtRecords = new DataServer().Getdata(strSQL);
            String[] strRecords = new string[dtRecords.Rows.Count];
            int i = 0;
            foreach (DataRow row in dtRecords.Rows)
            {
                strRecords[i] = Convert.ToString(row["CIA_NAME"]);

                i = i + 1;

            }
            return strRecords;
        }

        [WebMethod]
        public string[] GetDownloadRefNo(string prefixText)
        {
            DataTable dtRecords;
            string strSQL;

            strSQL = "Select CM_DOWNLOADREF_NO from TBL_CIRCULAR_MASTER where CM_DOWNLOADREF_NO like '" + prefixText + "%'";

            dtRecords = new DataServer().Getdata(strSQL);
            String[] strRecords = new string[dtRecords.Rows.Count];
            int i = 0;
            foreach (DataRow row in dtRecords.Rows)
            {
                strRecords[i] = Convert.ToString(row["CM_DOWNLOADREF_NO"]);

                i = i + 1;

            }
            return strRecords;
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetIssuingAuthority(
          string knownCategoryValues,
          string category)
        {

            DataSet dsCategory = UtilitiesBL.getDataset("ISS", mstrConnectionString);
            DataTable dtCategory = dsCategory.Tables[0];
            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            foreach (DataRow dr in dtCategory.Rows)
            {
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["CIA_NAME"], dr["CIA_ID"].ToString()));
            }
            return values.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetTopicByIssuingAuthority(
          string knownCategoryValues,
          string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(
              knownCategoryValues);
            int intId;
            DataTable dtSubCategory = new DataTable();
            DataRow dr;
            if (!kv.ContainsKey("IssuingAuthority") ||
                !Int32.TryParse(kv["IssuingAuthority"], out intId))
            {
                return null;
            }
            dtSubCategory = UtilitiesBL.getDatasetWithCondition("TopicByIssuingAuth", intId, mstrConnectionString);
            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            for (int i = 0; i < dtSubCategory.Rows.Count; i++)
            {
                dr = dtSubCategory.Rows[i];
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["CAM_NAME"], dr["CAM_ID"].ToString()));
            }
            return values.ToArray();
        }


        //[System.Web.Services.WebMethod]
        //[System.Web.Script.Services.ScriptMethod]
        //public string[] GetVendorName(string strPrefixText, int intCount)
        //{
        //    DataTable dt = UtilitiesBL.getDatasetWithConditionInString("ContractVendor", strPrefixText, mstrConnectionString);
        //    DataRow dr;
        //    string[] strMatchingNames;  
        //    int intCnt = dt.Rows.Count;
        //    strMatchingNames = new string[intCnt];
        //    for (int i = 0; i < intCnt; i++)
        //    {
        //        dr = dt.Rows[i];
        //        strMatchingNames[i] = dr["CON_VENDOR_NAME"].ToString();
        //    }
        //    return strMatchingNames;
        //}

        [WebMethod]
        public string[] GetVendorName(string prefixText)
        {
            DataTable dtRecords;
            string strSQL;

            strSQL = "Select distinct(CON_VENDOR_NAME) FROM TBL_CONTRACT_MAS where CON_VENDOR_NAME like '" + prefixText + "%' order by CON_VENDOR_NAME";

            dtRecords = new DataServer().Getdata(strSQL);
            String[] strRecords = new string[dtRecords.Rows.Count];
            int i = 0;
            foreach (DataRow row in dtRecords.Rows)
            {
                strRecords[i] = Convert.ToString(row["CON_VENDOR_NAME"]);

                i = i + 1;

            }
            return strRecords;
        }


        [WebMethod]
        public CascadingDropDownNameValue[] GetTypeofCircular(
          string knownCategoryValues,
          string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(
              knownCategoryValues);
            DataTable dt = new DataTable();
            DataRow dr;

            dt = UtilitiesBL.getDatasetWithConditionInString("getTypeofCircular", "", mstrConnectionString);
            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["CDTM_TYPE_OF_DOC"], dr["CDTM_ID"].ToString()));
            }
            return values.ToArray();
        }

        //Added By Milan Yadav on 09Jul2016
        [WebMethod]
        public CascadingDropDownNameValue[] GetCertLevels(string knownCategoryValues,
          string category)
        {
            string strSQL;
            DataTable dtLevels;

            strSQL = "Select RC_NAME, RC_CODE from TBL_REF_CODES where [RC_TYPE] = 'Certification Level' " +
                    " and [RC_STATUS] = 'A' " +
                    " order by RC_NAME ";

            dtLevels = new DataServer().Getdata(strSQL);
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            //values.Add(new CascadingDropDownNameValue("Select a Level", ""));
            foreach (DataRow dr in dtLevels.Rows)
            {
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["RC_NAME"], dr["RC_CODE"].ToString()));
            }
            return values.ToArray();
        }

        [WebMethod]

        public CascadingDropDownNameValue[] GetCertRelevantFuncsBasedOnLevel(string knownCategoryValues,
                                     string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            int intLevel;
            string strLevel = "", strSQL = "";
            DataTable dtRelevantFunctions = new DataTable();
            DataRow dr;
            if (!kv.ContainsKey("CertLevel") ||
                !Int32.TryParse(kv["CertLevel"], out intLevel))
            {
                return null;
            }

            strLevel = intLevel.ToString();

            if (strLevel.Equals("0"))
            {
                strSQL = "Select CSSDM_ID as Id,CSSDM_NAME as Name" +
                        " from TBL_CERT_SUB_SUB_DEPT_MAS " +
                        " order by CSSDM_NAME";
            }
            else if (strLevel.Equals("1"))
            {
                strSQL = "Select CSDM_ID as Id,CSDM_NAME as Name" +
                        " from TBL_CERT_SUB_DEPT_MAS " +
                        " order by CSDM_NAME";
            }
            else if (strLevel.Equals("2"))
            {
                strSQL = "Select CDM_ID as Id,CDM_NAME as Name" +
                        " from TBL_CERT_DEPT_MAS WHERE CDM_NAME != 'CCO Certificate'" +
                        " order by CDM_NAME";
            }
            //<<Added by Ankur Tyagi on 15-May-2025
            else if (strLevel.Equals("3"))
            {
                strSQL = "Select CDM_ID as Id,CDM_NAME as Name" +
                        " from TBL_CERT_DEPT_MAS " +
                        " order by CDM_NAME";
            }
            //>>

            dtRelevantFunctions = new DataServer().Getdata(strSQL);

            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            for (int i = 0; i < dtRelevantFunctions.Rows.Count; i++)
            {
                dr = dtRelevantFunctions.Rows[i];
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["Name"], dr["Id"].ToString()));
            }
            return values.ToArray();
        }
        [WebMethod]
        public CascadingDropDownNameValue[] GetCertFuncsBasedOnLevelForContent(string knownCategoryValues,
                                     string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            int intLevel;
            string strLevel = "", strSQL = "";
            DataTable dtRelevantFunctions = new DataTable();
            DataRow dr;
            if (!kv.ContainsKey("CertLevel") ||
                !Int32.TryParse(kv["CertLevel"], out intLevel))
            {
                return null;
            }

            strLevel = intLevel.ToString();

            if (strLevel.Equals("0"))
            {
                strSQL = "Select CSSDM_ID as Id,CSSDM_NAME as Name" +
                        " from TBL_CERT_SUB_SUB_DEPT_MAS " +
                        " where CSSDM_ID not in (" +
                        " select CERTM_DEPT_ID from TBL_CERT_MAS" +
                        " WHERE CERTM_DEPT_ID = CSSDM_ID AND CERTM_LEVEL_ID = 0)" +
                        " order by CSSDM_NAME";
            }
            else if (strLevel.Equals("1"))
            {
                strSQL = "Select CSDM_ID as Id,CSDM_NAME as Name" +
                        " from TBL_CERT_SUB_DEPT_MAS " +
                        " where CSDM_ID not in (" +
                        " select CERTM_DEPT_ID from TBL_CERT_MAS" +
                        " WHERE CERTM_DEPT_ID = CSDM_ID AND CERTM_LEVEL_ID = 1)" +
                        " order by CSDM_NAME";
            }
            else if (strLevel.Equals("2"))
            {
                strSQL = "Select CDM_ID as Id,CDM_NAME as Name" +
                        " from TBL_CERT_DEPT_MAS " +
                        " where CDM_ID not in (" +
                        " select CERTM_DEPT_ID from TBL_CERT_MAS" +
                        " WHERE CERTM_DEPT_ID = CDM_ID AND CERTM_LEVEL_ID = 2) AND CDM_NAME != 'Compliance Certificate' " +
                        " order by CDM_NAME";
            }
            //<<Added by Ankur Tyagi on 15-May-2025
            else if (strLevel.Equals("3"))
            {
                strSQL = "Select CDM_ID as Id,CDM_NAME as Name" +
                        " from TBL_CERT_DEPT_MAS " +
                        " where CDM_ID not in (" +
                        " select CERTM_DEPT_ID from TBL_CERT_MAS" +
                        " WHERE CERTM_DEPT_ID = CDM_ID AND CERTM_LEVEL_ID = 3)" +
                        " order by CDM_NAME";
            }
            //>>

            dtRelevantFunctions = new DataServer().Getdata(strSQL);

            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            for (int i = 0; i < dtRelevantFunctions.Rows.Count; i++)
            {
                dr = dtRelevantFunctions.Rows[i];
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["Name"], dr["Id"].ToString()));
            }
            return values.ToArray();
        }

        //<<
        [WebMethod]

        public CascadingDropDownNameValue[] GetCertRelevantFuncsBasedOnLevelAndLoggedInUser
            (string knownCategoryValues, string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            int intLevel;
            string strLevel = "", strSQL = "", strLoggedInUser = "";
            DataTable dtRelevantFunctions = new DataTable();
            DataRow dr;
            try
            {
                if (!kv.ContainsKey("CertLevelForView") ||
                    !Int32.TryParse(kv["CertLevelForView"], out intLevel))
                {
                    return null;
                }

                strLevel = intLevel.ToString();
                strLoggedInUser = HttpContext.Current.User.Identity.Name.ToString();

                if (strLevel.Equals("0"))
                {
                    strSQL = "Select DISTINCT CSSDM_ID as Id,CSSDM_NAME as Name" +
                    " from TBL_CERT_SUB_SUB_DEPT_MAS a " +
                    " INNER JOIN TBL_CERT_SUB_DEPT_MAS b on b.CSDM_ID = a.CSSDM_CSDM_ID  " +
                    " INNER JOIN TBL_CERT_DEPT_MAS  c on c.CDM_ID = b.CSDM_CDM_ID " +
                    " WHERE 1 = 1 And ( CSSDM_USER_ID = '" + strLoggedInUser + "'" +
                    " OR  CSDM_USER_ID = '" + strLoggedInUser + "'" +
                    " OR CDM_CXO_USERID = '" + strLoggedInUser + "' OR CDM_EC_USER_ID = '" + strLoggedInUser + "')" +
                    " order by CSSDM_NAME";
                }
                else if (strLevel.Equals("1"))
                {
                    strSQL = "Select DISTINCT CSDM_ID as Id,CSDM_NAME as Name" +
                    " from TBL_CERT_SUB_SUB_DEPT_MAS a " +
                    " INNER JOIN TBL_CERT_SUB_DEPT_MAS b on b.CSDM_ID = a.CSSDM_CSDM_ID  " +
                    " INNER JOIN TBL_CERT_DEPT_MAS  c on c.CDM_ID = b.CSDM_CDM_ID " +
                    " WHERE 1 = 1 And ( CSSDM_USER_ID = '" + strLoggedInUser + "'" +
                    " OR  CSDM_USER_ID = '" + strLoggedInUser + "'" +
                    " OR CDM_CXO_USERID = '" + strLoggedInUser + "' OR CDM_EC_USER_ID = '" + strLoggedInUser + "')" +
                    " order by CSDM_NAME";
                }
                else if (strLevel.Equals("2"))
                {
                    strSQL = "Select DISTINCT CDM_ID as Id,CDM_NAME as Name" +
                    " from TBL_CERT_SUB_SUB_DEPT_MAS a " +
                    " INNER JOIN TBL_CERT_SUB_DEPT_MAS b on b.CSDM_ID = a.CSSDM_CSDM_ID  " +
                    " INNER JOIN TBL_CERT_DEPT_MAS c on c.CDM_ID = b.CSDM_CDM_ID " +
                    " WHERE 1 = 1 And ( CSSDM_USER_ID = '" + strLoggedInUser + "'" +
                    " OR  CSDM_USER_ID = '" + strLoggedInUser + "'" +
                    " OR CDM_CXO_USERID = '" + strLoggedInUser + "' OR CDM_EC_USER_ID = '" + strLoggedInUser + "')" +
                    " order by CDM_NAME";
                }
                else if (strLevel.Equals("3"))
                {
                    strSQL = "Select DISTINCT CDM_ID as Id,CDM_NAME as Name" +
                    " from TBL_CERT_SUB_SUB_DEPT_MAS a " +
                    " INNER JOIN TBL_CERT_SUB_DEPT_MAS b on b.CSDM_ID = a.CSSDM_CSDM_ID  " +
                    " INNER JOIN TBL_CERT_DEPT_MAS c on c.CDM_ID = b.CSDM_CDM_ID " +
                    " WHERE 1 = 1 And ( CSSDM_USER_ID = '" + strLoggedInUser + "'" +
                    " OR  CSDM_USER_ID = '" + strLoggedInUser + "'" +
                    " OR CDM_CXO_USERID = '" + strLoggedInUser + "' OR CDM_EC_USER_ID = '" + strLoggedInUser + "')" +
                    " order by CDM_NAME";
                }

                dtRelevantFunctions = new DataServer().Getdata(strSQL);

                List<CascadingDropDownNameValue> values =
                  new List<CascadingDropDownNameValue>();

                for (int i = 0; i < dtRelevantFunctions.Rows.Count; i++)
                {
                    dr = dtRelevantFunctions.Rows[i];
                    values.Add(new CascadingDropDownNameValue(
                      (string)dr["Name"], dr["Id"].ToString()));
                }
                return values.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Added by Ashish Mishra on 05May2017
        [WebMethod]
        public CascadingDropDownNameValue[] getParamType(
          string knownCategoryValues,
          string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(
              knownCategoryValues);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            DataRow dr;

            ds = UtilitiesBL.getDataset("getParamType", mstrConnectionString);
            dt = ds.Tables[0];
            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["RC_NAME"], dr["RC_CODE"].ToString()));
            }
            return values.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] getParamDataType(
          string knownCategoryValues,
          string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(
              knownCategoryValues);
            string strParamType;
            DataTable dt = new DataTable();
            DataRow dr;

            if (!kv.ContainsKey("ParamType"))
            {
                return null;
            }
            else
            {
                strParamType = kv["ParamType"].ToString();
            }
            dt = UtilitiesBL.getDatasetWithConditionInString("getParamSubtype", strParamType, mstrConnectionString);
            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["RC_NAME"], dr["RC_CODE"].ToString()));
            }

            return values.ToArray();
        }
        //>>

        //<<Added by Rahuldeb on 12Dec2018
        [WebMethod]
        public CascadingDropDownNameValue[] getHelpdeskDept(
          string knownCategoryValues,
          string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(
              knownCategoryValues);
            //String[] arrDeptId = knownCategoryValues.Split(':', ';');
            //intDeptId = Convert.ToInt32(arrDeptId[1]);
            DataTable dtCategory = new DataTable();
            DataRow dr;
            //if (!kv.ContainsKey("Department") ||
            //    !Int32.TryParse(kv["Department"], out intDeptId))
            //{
            //    return null;
            //}
            dtCategory = new DataServer().Getdata("Select * from TBL_HELPDESK_DEPT_MAS order by HDM_NAME");
            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            for (int i = 0; i < dtCategory.Rows.Count; i++)
            {
                dr = dtCategory.Rows[i];
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["HDM_NAME"], dr["HDM_ID"].ToString()));
            }
            return values.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetCategoryForHelpDesk(
          string knownCategoryValues,
          string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(
              knownCategoryValues);
            int intDeptId;

            //String[] arrDeptId = knownCategoryValues.Split(':', ';');
            //intDeptId = Convert.ToInt32(arrDeptId[1]);
            DataTable dtCategory = new DataTable();
            DataRow dr;
            if (!kv.ContainsKey("Department") ||
                !Int32.TryParse(kv["Department"], out intDeptId))
            {
                return null;
            }
            dtCategory = new DataServer().Getdata("Select * from TBL_HELPDESK_CATEGORY_MAS where HCM_HDM_ID = " + intDeptId + " order by HCM_NAME");
            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            for (int i = 0; i < dtCategory.Rows.Count; i++)
            {
                dr = dtCategory.Rows[i];
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["HCM_NAME"], dr["HCM_ID"].ToString()));
            }
            return values.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetSubCategoryForHelpDesk(
          string knownCategoryValues,
          string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(
              knownCategoryValues);
            int intCategoryId;
            //String[] arrCategoryId = knownCategoryValues.Split(':', ';');
            //intCategoryId = Convert.ToInt32(arrCategoryId[1]);
            DataTable dtSubCategory = new DataTable();
            DataRow dr;
            if (!kv.ContainsKey("Category") ||
                !Int32.TryParse(kv["Category"], out intCategoryId))
            {
                return null;
            }
            //UtilitiesBL.getDatasetWithCondition("SUBCATEGORY", intCategoryId, mstrConnectionString);
            dtSubCategory = new DataServer().Getdata("select HSM_ID , HSM_NAME    from  TBL_HELPDESK_SUBCATEGORY_MAS where HSM_STATUS='A' AND HSM_HCM_ID=" + intCategoryId.ToString() + " order by  HSM_NAME  ");
            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            for (int i = 0; i < dtSubCategory.Rows.Count; i++)
            {
                dr = dtSubCategory.Rows[i];
                values.Add(new CascadingDropDownNameValue(
                  (string)dr["HSM_NAME"], dr["HSM_ID"].ToString()));
            }
            return values.ToArray();
        }
        //>>
    }
}