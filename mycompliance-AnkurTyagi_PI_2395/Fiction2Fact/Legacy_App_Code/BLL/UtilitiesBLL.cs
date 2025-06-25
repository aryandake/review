using System.Data;
using Fiction2Fact.Legacy_App_Code.DAL;
using Fiction2Fact.Legacy_App_Code.VO;
using System.Collections.Generic;

/// <summary>
/// Summary description for UtilitiesBLL
/// </summary>
namespace Fiction2Fact.Legacy_App_Code.BLL
{
    public class UtilitiesBLL
    {
        UtilitiesDAL utlDAL = new UtilitiesDAL();
        UtilitiesDAL udl = new UtilitiesDAL();
        public UtilitiesBLL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataTable getData(string code, UtilitiesVO objUVO)
        {
            DataTable dtResults = new DataTable();
            dtResults = utlDAL.getData(code, objUVO);
            return dtResults;
        }

        public DataSet getDataSet(string code)
        {
            DataSet dtResults = new DataSet();
            dtResults = utlDAL.getDataSet(code);
            return dtResults;
        }

        public DataTable getDataSetwithTwoCondition(string code, UtilitiesVO objVO)
        {
            DataTable dtResults = new DataTable();
            dtResults = utlDAL.getDataSetwithTwoCondition(code, objVO);
            return dtResults;
        }
        public DataTable getDatasetWithConditionInString(string code, string str, string conStr)
        {
            DataTable dtResult = new DataTable();
            dtResult = utlDAL.getDatasetWithConditionInString(code, str, conStr);
            return dtResult;
        }
        public DataSet getDataSetFromSP(string spName, Dictionary<string, string> dictParams = null)
        {
            DataSet dtResults = new DataSet();
            dtResults = utlDAL.getDataSetFromSP(spName, dictParams);
            return dtResults;
        }
        public string roleMenuMapping(string strUser, int iCmpId, string sPagePath, string mstrConnectionString = null)
        {
            string retVal = "";
            retVal = utlDAL.roleMenuMapping(strUser, iCmpId, sPagePath, mstrConnectionString);
            return retVal;
        }
        public static DataTable SetProductName(string sPagePath)
        {
            return UtilitiesDAL.SetProductName(sPagePath);
        }
        public DataSet getDataset(string code, string conStr)
        {
            DataSet resultDataSet = new DataSet();
            resultDataSet = udl.getDataset(code, conStr);
            return resultDataSet;
        }
        public DataTable getDatasetWithCondition(string code, int id, string conStr)
        {
            DataTable dtResult = new DataTable();
            dtResult = udl.getDatasetWithCondition(code, id, conStr);
            return dtResult;
        }
        
        public DataTable getDatasetWithMoreCondition(string code, int condition1, int condition2, string conStr)
        {
            DataTable dtResult = new DataTable();
            dtResult = udl.getDatasetWithMoreCondition(code, condition1, condition2, conStr);
            return dtResult;
        }

        public DataTable getDatasetWithTwoConditionInString(string code, string condition1, string condition2, string conStr)
        {
            DataTable dtResult = new DataTable();
            dtResult = udl.getDatasetWithTwoConditionInString(code, condition1, condition2, conStr);
            return dtResult;
        }

        public DataTable getDatasetWithThreeConditionInString(string code, string str1, string str2, string str3, string conStr)
        {
            DataTable dtResult = new DataTable();
            dtResult = udl.getDatasetWithThreeConditionInString(code, str1, str2, str3, conStr);
            return dtResult;
        }
        public DataTable getNewsAlertForLitigation(string conStr)
        {
            DataTable dtnews;
            dtnews = udl.getNewsAlertForLitigation(conStr);
            return dtnews;
        }
        public DataTable getNewsAlertForLSLitigation(string conStr)
        {
            DataTable dtnews;
            dtnews = udl.getNewsAlertForLSLitigation(conStr);
            return dtnews;
        }
        public DataTable getDatasetWithThreeConditionsInString(string code, string str, int intLevel, string conStr)
        {
            DataTable dtResult = new DataTable();
            dtResult = udl.getDatasetWithThreeConditionsInString(code, str, intLevel, conStr);
            return dtResult;
        }

        //<< Added by Kiran Kharat on 29-Jan-2017 
        public DataTable getDataForGv(string conStr)
        {
            DataTable dtResult = new DataTable();
            dtResult = udl.getDataForGv(conStr);
            return dtResult;
        }

        public DataTable getDataForEdit(string strId, string conStr)
        {
            DataTable dtResult = new DataTable();
            dtResult = udl.getDataForEdit(strId, conStr);
            return dtResult;
        }

        public DataTable getDescriptionOfClauses(string strListOfIds, string conStr)
        {
            DataTable dtResult = new DataTable();
            dtResult = udl.getDescriptionOfClauses(strListOfIds, conStr);
            return dtResult;
        }
        //>>

        //<< Added by Amarjeet on 06-Aug-2021
        public void savePDFContentInTable(string strRefId, string strRefFileId, string strContent, string strModuleName, 
            string strLoggedInUser)
        {
            udl.savePDFContentInTable(strRefId, strRefFileId, strContent, strModuleName, strLoggedInUser);
        }
        //>>

        public void updateConfigParams(int intId, string strValue, string strConnectionString)
        {
            udl.updateConfigParams(intId, strValue, strConnectionString);
        }
    }
}