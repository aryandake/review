using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Fiction2Fact.App_Code
{
    public class ProductFeatures
    {
        public class Defaults
        {
            public static string[] ContractManagement = new string[]
            {
                "CreateDraftButtonInGrid",
                "ShowContractExecutionToRequestor"
            };
        }
        public class ContractManagement
        {

            public static string ContractCCUsers { get; set; } = "ContractCCUsers";
            public static string CreateDraftButtonBesidesSearch { get; set; } = "CreateDraftButtonBesidesSearch";
            public static string CreateDraftButtonInGrid { get; set; } = "CreateDraftButtonInGrid";
            public static string ShowContractTemplateLinkInGrid { get; set; } = "ShowContractTemplateLinkInGrid";
            public static string CreateContractAtProceedForFinalization { get; set; } = "CreateContractAtProceedForFinalization";
            public static string ShowContractRepositoryGridInCreateDraft { get; set; } = "ShowContractRepositoryGridInCreateDraft";
            public static string ShowContractExecutionToRequestor { get; set; } = "ShowContractExecutionToRequestor";
        }

        public class Advertisement
        {
            public static string AdvtVernacularRequest { get; set; } = "AdvtVernacularRequest";
            public static string AdvtVernacularRequestAutoApproval { get; set; } = "Advt_Vernacular_Request_Approval";
            public static string IsComplianceToMarkInCCForProductApproval { get; set; } = "Is_Compliance_To_Mark_In_CC_For_Product_Approval";
        }

        /// <summary>
        /// Get Current Page Features as DataTable
        /// </summary>
        /// <param name="StrPage">Name of the page</param>
        /// <param name="StrProjectName">Name of Project</param>>
        /// <returns></returns>
        public static DataTable GetPageAllFeatures(string StrPage, string StrProjectName)
        {

            if (!string.IsNullOrEmpty(StrPage))
            {
                try
                {
                    using (F2FDatabase DB = new F2FDatabase())
                    {
                        string strCompanyName = HttpContext.Current.Session["CMP_NAME"].ToString();
                        string StrSql = "SELECT * FROM TBL_CP_FEATURES " +
                            "INNER JOIN TBL_FEATURE_MAS ON FM_ID = CPF_FM_ID " +
                            "INNER JOIN TBL_PAGE_MAS ON PGM_ID = CPF_PGM_ID " +
                            "INNER JOIN TBL_COMPANY_PRODUCTS ON CP_ID = CPF_CP_ID " +
                            "INNER JOIN TBL_PRODUCT_MAS ON PM_ID = PGM_PM_ID " +
                            "INNER JOIN TBL_COMPANY ON CMP_ID = CP_CMP_ID " +
                            "WHERE PGM_NAME = @PageName" +
                            " AND CMP_NAME = @CompanyName" +
                            " AND PM_NAME = @ProjectName" +
                            " AND TBL_CP_FEATURES.STATUS = 'y' AND TBL_PAGE_MAS.STATUS = 'y' ";
                        DB.F2FCommand.CommandText = StrSql;
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@PageName", F2FDatabase.F2FDbType.VarChar, StrPage));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CompanyName", F2FDatabase.F2FDbType.VarChar, strCompanyName));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ProjectName", F2FDatabase.F2FDbType.VarChar, StrProjectName));
                        DataTable dt = new DataTable();
                        DB.F2FDataAdapter.Fill(dt);
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
            }
            return null;
        }
        
        /// <summary>
        /// Get Current Page Features as DataTable
        /// </summary>
        /// <param name="StrPage">Name of the page</param>
        /// <param name="StrProjectName">Name of Project</param>
        /// <param name="StrFeatureName">Name of Feature</param>
        /// <returns></returns>
        public static bool GetPageFeatures(string StrPage, string StrProjectName, string StrFeatureName)
        {

            if (!string.IsNullOrEmpty(StrPage))
            {
                try
                {
                    using (F2FDatabase DB = new F2FDatabase())
                    {
                        //string strCompanyName = HttpContext.Current.Session["CMP_NAME"].ToString();

                        #region //<< Commented by Vivek on 16-Dec-2020
                        //string strSql = "SELECT * FROM TBL_CP_FEATURES " +
                        //    "INNER JOIN TBL_FEATURE_MAS ON FM_ID = CPF_FM_ID AND tbl_feature_mas.status = 'y' " +
                        //    "INNER JOIN TBL_PAGE_MAS ON PGM_ID = CPF_PGM_ID " +
                        //    "INNER JOIN TBL_COMPANY_PRODUCTS ON CP_ID = CPF_CP_ID " +
                        //    "INNER JOIN TBL_PRODUCT_MAS ON PM_ID = PGM_PM_ID " +
                        //    "INNER JOIN TBL_COMPANY ON CMP_ID = CP_CMP_ID " +
                        //    "WHERE PGM_NAME = @PageName" +
                        //    " AND CMP_NAME = @CompanyName" +
                        //    " AND PM_NAME = @ProjectName" +
                        //    " AND FM_NAME = @FeatureName" +
                        //    " AND TBL_CP_FEATURES.STATUS = 'y' AND TBL_PAGE_MAS.STATUS = 'y'"; /* AND CPF_ENABLED = 'y' ";*/
                        #endregion

                        string strSql = " SELECT dbo.checkIsProductFeatureEnabled('APP', null, @ProjectName, null, @FeatureName, @PageName) as [CPF_ENABLED] ";
                        DB.F2FCommand.CommandText = strSql;
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@PageName", F2FDatabase.F2FDbType.VarChar, StrPage));
                        //DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CompanyName", F2FDatabase.F2FDbType.VarChar, strCompanyName));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ProjectName", F2FDatabase.F2FDbType.VarChar, StrProjectName));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@FeatureName", F2FDatabase.F2FDbType.VarChar, StrFeatureName));
                        DataTable dt = new DataTable();
                        DB.F2FDataAdapter.Fill(dt);

                        if (dt != null && dt.Rows.Count != 0)
                        {
                            return (dt.Rows[0]["CPF_ENABLED"].ToString().ToUpper().Equals("Y") ? true : false);
                        }
                        else
                        {
                            return Defaults.ContractManagement.Contains(StrFeatureName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
            }
            return false;
        }
    }
}