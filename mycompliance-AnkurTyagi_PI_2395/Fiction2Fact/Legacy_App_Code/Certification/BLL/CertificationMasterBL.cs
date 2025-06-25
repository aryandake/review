using Fiction2Fact.Legacy_App_Code.Certification.DAL;
using System.Data;


namespace Fiction2Fact.Legacy_App_Code.Certification.BLL
{
    public class CertificationMasterBL
    {
        CertificationMasterDAL certDAL = new CertificationMasterDAL();

        public int saveCertificationMas(int cID, string strDepartment, int intLevel, string strContents, string strCreateBy,
                   string strConnectionString)
        {
            int retVal = 0;
            retVal = certDAL.saveCertificationMas(cID, strDepartment, intLevel, strContents, strCreateBy,
                     strConnectionString);
            return retVal;
        }
        public string saveCertification(int intCertId, int intCertMasId, int intCertQuarterId, string strContent, string strRemarks,
            string strStatus, string strCreateBy, DataTable dtNewException, DataTable dtchecklist, string strsubDepartment,
            string strLoggedInUser)
        {
            return certDAL.saveCertification(intCertId, intCertMasId, intCertQuarterId, strContent, strRemarks, strStatus,
                strCreateBy, dtNewException, dtchecklist, strsubDepartment, strLoggedInUser);
        }

        public DataSet SearchCert(int intCId, string strLevel, string strDeptName, string strConnectionString)
        {
            DataSet dsResult = new DataSet();
            dsResult = certDAL.SearchCert(intCId, strLevel, strDeptName, strConnectionString);
            return dsResult;
        }

        public DataSet searchCertifications(int intCertId, string strDeptName, string strQuarter,
            string strConnectionString)
        {

            DataSet dsResult = new DataSet();
            dsResult = certDAL.searchCertifications(intCertId, strDeptName, strQuarter, strConnectionString);
            return dsResult;

        }
        public DataSet searchEditCertifications(int intCertId, string strDeptName,
            string strQuarter, string strCreateBy, int intLevel,
            string strConnectionString)
        {

            DataSet dsResult = new DataSet();
            dsResult = certDAL.searchEditCertifications(intCertId, strDeptName, strQuarter,
                strCreateBy, intLevel, strConnectionString);
            return dsResult;

        }
        public int deleteCertificate(int intCertId, string strConnectionString)
        {
            int retVal = 0;
            retVal = certDAL.deleteCertificate(intCertId, strConnectionString);
            return retVal;

        }

        public int deleteCertContent(int intCertId, string strConnectionString)
        {
            int retVal = 0;
            retVal = certDAL.deleteCertContent(intCertId, strConnectionString);
            return retVal;

        }

        //<< Added by Nikhil Adhalikar on 20-Sep-2011
        public int saveCommonCertification(int intCertId, int intCertMasId, int intCertQuarterId, string strContent,
                            string strRemarks, string strStatus, string strRole,
                            string strCreateBy, string strConnectionString)
        {
            int retVal = 0;
            retVal = certDAL.saveCommonCertification(intCertId, intCertMasId, intCertQuarterId, strContent,
                     strRemarks, strStatus, strRole, strCreateBy, strConnectionString);
            return retVal;
        }
        //>>

        //<< Modified by Amarjeet on 23-Jul-2021
        public int saveCertificationChecklist
            (int intChecklistID, string strDepartmentID, string strReference, string strTitleofSection, string strSelfAssessmentStatus, string strCheckpoints, string strFrequency, string strDueDate,
            string strSourceDept, string strDeptRespFurnish, string strDeptRespSubmitting, string strTobeFilledwith,
             string strEffectiveDateFrom, string strStatus, string strEffectiveDateTo, string strCreatedBy, string strRemark,
            string strPenalty, string strActRegCirc, string strForms, string strConnectionString)
        {
            int retVal = 0;
            retVal = certDAL.saveCertificationChecklist(intChecklistID, strDepartmentID, strReference, strTitleofSection,
                strSelfAssessmentStatus, strCheckpoints, strFrequency, strDueDate, strSourceDept, strDeptRespFurnish,
                strDeptRespSubmitting, strTobeFilledwith, strEffectiveDateFrom, strStatus, strEffectiveDateTo, strCreatedBy,
                strRemark, strPenalty, strActRegCirc, strForms, strConnectionString);
            return retVal;
        }
        //>>

        //Added by Supriya on 25-Nov-2014
        public DataTable getCertificationsForApproval(string strId, string strStatus, string strUsername, string strConnectionString)
        {
            DataTable dsResult = new DataTable();
            dsResult = certDAL.getCertificationsForApproval(strId, strStatus, strUsername, strConnectionString);
            return dsResult;
        }

        public string updateCerification(int intCertId, string strOperationType, string strRemarks, string strCreateBy, string strType,
            string strLoggedInUserId, string strUserName)
        {
            return certDAL.updateCerification(intCertId, strOperationType, strRemarks, strCreateBy, strType, strLoggedInUserId,
                strUserName);
        }

        public DataTable searchCertificationsChecklist(int intCertId, string strDeptName, string strQuarter, string strStatus, string strConnectionString)
        {
            DataTable dsResult = new DataTable();
            dsResult = certDAL.searchCertificationsChecklist(intCertId, strDeptName, strQuarter, strStatus, strConnectionString);
            return dsResult;
        }

        //Added by Supriya on 10-Aug-2015

        public DataTable getCertificationForPendingApproval(string strType, string strLoggedInUser, string strConnectionString)
        {
            DataTable dt = new DataTable();
            dt = certDAL.getCertificationForPendingApproval(strType, strLoggedInUser, strConnectionString);
            return dt;
        }
        public DataTable getCertificationsChecklist(string strCertId, string strDeptId, string strConnectionString)
        {
            DataTable dt = new DataTable();
            dt = certDAL.getCertificationsChecklist(strCertId, strDeptId, strConnectionString);
            return dt;
        }

        public DataTable getCertificationsApproval(string strDeptId, string strType, string strUsername, string strConnectionString)
        {
            DataTable dt = new DataTable();
            dt = certDAL.getCertificationsApproval(strDeptId, strType, strUsername, strConnectionString);
            return dt;
        }

        //Modified By Milan Yadav on 13-Sep-2016
        public int saveCertificationQuarterMas(int intId, string strFromDt, string strToDt, string strDueDt, string strStatus, string strCreateBy, string strConnectionString)
        {
            int retVal = 0;
            retVal = certDAL.saveCertificationQuarterMas(intId, strFromDt, strToDt, strDueDt, strStatus, strCreateBy, strConnectionString);
            return retVal;
        }

        public DataTable searchCertificationQuarterMas(string strId, string strFromDt, string strToDt, string strStatus, string strConnectionString)
        {
            DataTable dt = new DataTable();
            dt = certDAL.searchCertificationQuarterMas(strId, strFromDt, strToDt, strStatus, strConnectionString);
            return dt;
        }

        public int generateQuarterlyCertification(int intQuarterId, string strConnectionString)
        {
            int retVal = 0;
            retVal = certDAL.generateQuarterlyCertification(intQuarterId, strConnectionString);
            return retVal;
        }


        //Added by Vivek on 20Nov2015
        public int saveSubCertificationDeptMas(int intId, string strCertDept, string strCertSubDeptName, string strUserId, string strUserName, string strEmailId,
                                  string strCreateBy, DataTable dtCertSubDepartment, string strConnectionString)
        {
            int retVal = 0;
            retVal = certDAL.saveSubCertificationDeptMas(intId, strCertDept, strCertSubDeptName, strUserId, strUserName, strEmailId, strCreateBy, dtCertSubDepartment, strConnectionString);
            return retVal;
        }

        public int insertException(int intCertId, string strApplicationLawPop, string strObservationPop, string strClientFileName,
            string strServerFileName, string strRootCausePop, string strActionTakenPop, string strTargetDatePop, string strCreateBy,
            string strConnectionString)
        {
            int retVal = 0;
            retVal = certDAL.insertException(intCertId, strApplicationLawPop, strObservationPop, strClientFileName, strServerFileName, strRootCausePop,
                     strActionTakenPop, strTargetDatePop, strCreateBy, strConnectionString);
            return retVal;
        }
        //Added By milan yadav on 1-june-2016
        //>>
        public DataTable GetCertChecklistDetailById(string strDepartmentId, string strCertChecklistMasId, string strConnectionString)
        {
            DataTable dt = new DataTable();
            dt = certDAL.GetCertChecklistDetailById(strDepartmentId, strCertChecklistMasId, strConnectionString);
            return dt;
        }

        public DataTable getCertIdWiseCertification(int intCertId, string strConnectionString)
        {
            DataTable dt = new DataTable();
            dt = certDAL.getCertIdWiseCertification(intCertId, strConnectionString);
            return dt;
        }

        public DataTable getChecklistByCertId(int intCertId, string strConnectionString)
        {
            DataTable dt = new DataTable();
            dt = certDAL.getChecklistByCertId(intCertId, strConnectionString);
            return dt;
        }

        public DataTable getAllChecklistByCertificationId(int intCertId, string strConnectionString)
        {
            DataTable dt = new DataTable();
            dt = certDAL.getAllChecklistByCertificationId(intCertId, strConnectionString);
            return dt;
        }
        //<<

        //Added By Milan Yadav on 28-Jun-2016
        //>>
        public int generateSeparateQuarterlyCertification(int intQuarterId, int intDepartmentId, string strConnectionString)
        {
            int retVal = 0;
            retVal = certDAL.generateSeparateQuarterlyCertification(intQuarterId, intDepartmentId, strConnectionString);
            return retVal;
        }
        //<<

        //Added By Milan Yadav on 27-Sep-2016
        //>>
        public int deActivateCertificationChecklist(int intChecklistID, string strStatus, string strEffectiveDateTo, string strCreatedBy, string strRemark, string strConnectionString)
        {
            int retVal = 0;
            retVal = certDAL.deActivateCertificationChecklist(intChecklistID, strStatus, strEffectiveDateTo, strCreatedBy, strRemark, strConnectionString);
            return retVal;
        }
        //<<

        //Added By Milan Yadav on 27-Sep-2016
        //>>
        public DataTable getChecklistByCertDetsId(int intCertDetsId, string strConnectionString)
        {
            DataTable dt = new DataTable();
            dt = certDAL.getChecklistByCertDetsId(intCertDetsId, strConnectionString);
            return dt;
        }
        //<<
        //Added By Milan Yadav on 20-Oct-2016
        //>>
        public DataSet viewPastCertifications(int intCertId, string strDeptName,
        string strQuarter, string strCreateBy, int intlevel,
            string strConnectionString)
        {
            DataSet dsResult = new DataSet();
            dsResult = certDAL.viewPastCertifications(intCertId, strDeptName, strQuarter,
                strCreateBy, intlevel, strConnectionString);
            return dsResult;

        }
        //<<

        public DataTable viewPastConsolidateChecklist(string strCertId, string strDeptId, string strQuarterId, string strLevel,
            string strUsername, string strConnectionString)
        {
            DataTable dt = new DataTable();
            dt = certDAL.viewPastConsolidateChecklist(strCertId, strDeptId, strQuarterId, strLevel, strUsername, strConnectionString);
            return dt;
        }

        //<<Added By Milan Yadav on 15-Apr-2017
        public int UpdateCertificationForApprover
           (int intCertId, string strStatus, string strCreatedBy, string strQuarterId,
            string strContent, string strConnectionString)
        {
            int retVal = 0;
            retVal = certDAL.UpdateCertificationForApprover(intCertId, strStatus, strCreatedBy,
                strQuarterId, strContent, strConnectionString);
            return retVal;
        }
        //<< Added By Milan Yadav on 24-Apr-2017
        public DataTable viewConsolidateExceptions(string strCertId, string strDeptId, string strQuarterId, string strLevel,
    string strUsername, string strConnectionString)
        {
            DataTable dt = new DataTable();
            dt = certDAL.viewConsolidateExceptions(strCertId, strDeptId, strQuarterId, strLevel, strUsername, strConnectionString);
            return dt;
        }
        //>>

        public int generateSeparateQuarterlyJointCertification(int intQuarterId, int intDepartmentId, string strConnectionString)
        {
            int retVal = 0;
            retVal = certDAL.generateSeparateQuarterlyJointCertification(intQuarterId, intDepartmentId, strConnectionString);
            return retVal;
        }
        public string ValidateCertificationRollout(int iQtrId, int iDeptId)
        {
            return certDAL.ValidateCertificationRollout(iQtrId, iDeptId);
        }

        //<< Common Certification content
        public DataTable getCertCommonContent()
        {
            return certDAL.getCertCommonContent();
        }

        public int saveCertCommonContent(string strId, string strContent, string strCreator)
        {
            return certDAL.saveCertCommonContent(strId, strContent, strCreator);
        }
        //>>

        //<<Added by Ankur Tyagi on 15-Sep-2023
        public DataTable CERT_getChklistForClosure(int intQtrId, int intCSSDM_ID, int intCSDM_ID, int intCDM_ID, string strUserId, string strConnectionString)
        {
            return certDAL.CERT_getChklistForClosure(intQtrId, intCSSDM_ID, intCSDM_ID, intCDM_ID, strUserId, strConnectionString);
        }

        public void CERT_saveChklistClosure(int intCCDId, string strClosureDate, string strClosureRemarks, string strUsername, string strType, string strConnectionString)
        {
            certDAL.CERT_saveChklistClosure(intCCDId, strClosureDate, strClosureRemarks, strUsername, strType, strConnectionString);
        }

        public DataTable CERT_getExceptionForClosure(int intQtrId, int intCSSDM_ID, int intCSDM_ID, int intCDM_ID, string strUserId, string strStatus, string strConnectionString)
        {
            return certDAL.CERT_getExceptionForClosure(intQtrId, intCSSDM_ID, intCSDM_ID, intCDM_ID, strUserId, strStatus, strConnectionString);
        }

        //<<Added by Ankur Tyagi on 23Jan2024
        public DataTable Cert_getRegulatoryRecords(int intQtrId, int intLevel, int intDeptId,
            string strIsExportedToPDForDoc, string strConnectionString)
        {
            return certDAL.Cert_getRegulatoryRecords(intQtrId, intLevel, intDeptId, strIsExportedToPDForDoc,
                strConnectionString);
        }
        //>>

    }
}
