using System;
using System.Data;
using System.Web;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.App_Code;
using System.Collections.Generic;
using System.Data.Common;
using static Fiction2Fact.App_Code.F2FDatabase;

/// <summary>
/// Summary description for UtilitiesDAL
/// </summary>
namespace Fiction2Fact.Legacy_App_Code.DAL
{
    public class UtilitiesDAL
    {
        public DataTable getData(string code, UtilitiesVO utlVO)
        {
            DataTable resultdata = new DataTable();
            string strSql = "";
            try
            {
                switch (code)
                {
                    //<< Added by ramesh more on 01-Jan-2024 for Outward

                    case "OutwardId":
                        {
                            strSql = "select OT_ID from TBL_OUTWARD_TRACKING where 1=1 " + utlVO.getCode();
                            break;
                        }
                    case "OutwarNo":
                        {
                            strSql = "select OT_DOCUMENT_NO,OT_DOC_NAME from TBL_OUTWARD_TRACKING where 1=1 " + utlVO.getCode();
                            break;
                        }
                    // >>
                    case "getChecklistAttachments":
                        {
                            strSql = "select * from TBL_TAX_SUBMISSION_FILES where " + utlVO.getCode();
                            break;
                        }
                    case "deleteChecklistAttachments":
                        {
                            strSql = "delete from TBL_TAX_SUBMISSION_FILES where " + utlVO.getCode();
                            break;
                        }
                    case "deleteRefCode":
                        {
                            strSql = "delete from TBL_TAX_REF_CODES where " + utlVO.getCode();
                            break;
                        }
                    case "getOSTemplate":
                        {
                            strSql = "select * from TBL_OS_CHECKLIST " + utlVO.getCode();
                            break;
                        }
                    case "getOSAttachment":
                        {
                            strSql = "select * from TBL_OS_FILE_ATTACHEMENT " + utlVO.getCode();
                            break;
                        }
                    case "deleteOSAttachment":
                        {
                            strSql = "delete from TBL_OS_FILE_ATTACHEMENT " + utlVO.getCode();
                            break;
                        }
                    //<<Added By Bhavik @ 07 Mar 2014
                    case "deleteChecklist":
                        {
                            strSql = "delete from TBL_OS_CHECKLIST " + utlVO.getCode();
                            break;
                        }
                    //>>
                    case "getVendorDetails":
                        {
                            strSql = "select *,case when OSR_OUTSOURCE_NONOUTSOURCE = 'O' then 'Outsourcing'" +
                                    "when OSR_OUTSOURCE_NONOUTSOURCE ='N' then 'Non Outsourcing' else ''  end as IsOutSource," +
                                    "case when OSR_NEW_EXISTING_VENDOR ='N' then 'New' " +
                                    "when OSR_NEW_EXISTING_VENDOR ='R' then 'Renewal' else ''  end as VendorType," +
                                    "case when OSR_TYPE ='C' then 'Company' " +
                                    "when OSR_TYPE ='P' then 'Partnership'" +
                                    "when OSR_TYPE ='I' then 'Individual' else ''  end as [Type]," +
                                    "case when OSR_CORE ='N' then 'Non Core' " +
                                    "when OSR_CORE ='S' then 'Support to Core' else ''  end as Core," +
                                    "case when OSR_ACTIVITY_GUIDELINES ='N' then 'No' " +
                                    "when OSR_ACTIVITY_GUIDELINES ='Y' then 'Yes' else ''  end as ActivityGuideLines," +
                                    "case when OSR_RELATION_ABG_ABFSG_GRP ='N' then 'No' " +
                                    "when OSR_RELATION_ABG_ABFSG_GRP ='Y' then 'Yes' else ''  end as RelationShipwithgroup, " +
                                    "case when  OSR_ORM_ACC_REJ = 'AR' then 'Additional Requirement' else OSR_ORM_ACC_REJ end as ORM_STATUS, " +
                                    "case when  OSR_REG_RISK_ACC_REJ = 'AR' then 'Additional Requirement' else OSR_REG_RISK_ACC_REJ end as RR_STATUS, " +
                                    "case when  OSR_INFO_SEC_ACC_REJ = 'AR' then 'Additional Requirement' else OSR_INFO_SEC_ACC_REJ end as IS_STATUS, " +
                                    "case when OSR_ORM_ACC_REJ = 'Accept' then OSR_ORM_ACC_COMMENTS when OSR_ORM_ACC_REJ = 'Reject' " +
                                    "then OSR_ORM_REJ_COMMENTS when OSR_ORM_ACC_REJ = 'AR' then OSR_ORM_AR_COMMENTS else '' end as ORMRemarks, " +
                                    "case when OSR_REG_RISK_ACC_REJ = 'Accept' then OSR_REG_RISK_ACC_COMMENTS when OSR_REG_RISK_ACC_REJ = 'Reject' " +
                                    "then OSR_REG_RISK_REJ_COMMENTS when OSR_REG_RISK_ACC_REJ = 'AR' then OSR_REG_RISK_AR_COMMENTS else '' end as RRRemarks, " +
                                    "case when OSR_INFO_SEC_ACC_REJ = 'Accept' then OSR_INFO_SEC_ACC_COMMENTS when OSR_INFO_SEC_ACC_REJ = 'Reject' " +
                                    "then OSR_INFO_SEC_REJ_COMMENTS when OSR_INFO_SEC_ACC_REJ = 'AR' then OSR_INFO_SEC_AR_COMMENTS else '' end as ISRemarks " +
                                    "from TBL_OS_REQUESTS " + utlVO.getCode();
                            break;
                        }
                    case "getAuditTrailData":
                        {
                            strSql = "select *,REPLACE(REPLACE(AT_AUDIT_TRAIL, CHAR(10), ''), CHAR(13), '')  as Audit_Trail " +
                                " from TBL_AUDIT_TRAIL " + utlVO.getCode();
                            break;
                        }
                    case "getActivitySynopsisData":
                        {
                            strSql = "select * from TBL_OS_ACTIVITY_SYNOPSIS_MAS " + utlVO.getCode();
                            break;
                        }
                    case "getFuncDeptData":
                        {
                            strSql = "select * from TBL_OS_FUNC_DEPT_MASTER " + utlVO.getCode();
                            break;
                        }
                    //Added supriya on 20-Feb-2014
                    case "BindTaxationOwners":
                        {
                            strSql = "SELECT * FROM TBL_TAX_REPORTING_OWNERS " + utlVO.getCode();
                            break;
                        }
                    //Added supriya on 21-Feb-2014
                    case "CheckSubmissionEntry":
                        {
                            strSql = "select tsc_id from TBL_TAX_SUB_CHKLIST " +
                                " inner join TBL_TAX_SUBMISSIONS on tsub_tsc_id=tsc_id " + utlVO.getCode();
                            break;
                        }
                    case "DeleteSubmission":
                        {
                            strSql = "delete from TBL_TAX_REPORTING_OWNERS " + utlVO.getCode();
                            break;
                        }
                    //Added supriya on 22-Feb-2014
                    case "getSubmissionOwnersBySMId":
                        {
                            strSql = "SELECT * FROM TBL_TAX_REPORTING_OWNERS " +
                                " inner join TBL_TAX_OWNER_MAS on TRO_TOM_ID =TOM_ID " + utlVO.getCode();
                            break;
                        }
                    case "DeleteExistingTaxOwners":
                        {
                            strSql = "delete from TBL_TAX_REPORTING_OWNERS " + utlVO.getCode();
                            break;
                        }

                    case "areTaxSubmissionsDone":
                        {
                            strSql = "select count(*) as cnt from TBL_TAX_SUB_CHKLIST inner join TBL_TAX_SUBMISSIONS " +
                                    " on TSUB_TSC_ID =TSC_ID " + utlVO.getCode();
                            break;
                        }
                    case "taxChecklistEntries":
                        {
                            strSql = "select  count(*) as cnt from TBL_TAX_SUB_CHKLIST inner join TBL_TAX_SUBMISSIONS_MAS on tsc_tsm_id=tsm_id "
                           + utlVO.getCode();
                            break;
                        }

                    //case "checkTaxationRoleExist":
                    //    {
                    //        strSql = "select * from aspnet_Users " +
                    //        " left outer join aspnet_UsersInRoles on aspnet_Users.UserId =aspnet_UsersInRoles.UserId " +
                    //        " where RoleId in (select RoleId from aspnet_Roles where RoleName in  " +
                    //        " ('TaxationAdmin','TaxationUser')) " + utlVO.getCode();
                    //        break;
                    //    }
                    //<<Added By Bhavik @ 07 Mar 2014
                    case "getAllStatus":
                        {
                            strSql = "select * from TBL_STATUS_MAS " + utlVO.getCode();
                            break;
                        }
                    //>>
                    //<<Added By Bhavik @ 07 Mar 2014
                    case "getPastVendorDetail":
                        {
                            strSql = "select max(OSR_ID) as ID from TBL_OS_REQUESTS " + utlVO.getCode();
                            break;
                        }
                    //>>
                    //<<Added By Bhavik @ 02 Jul 2014
                    case "getUserMasterDetails":
                        {
                            strSql = "select *,case when DTUM_IS_USER_MANAGER = 'Y' then 'Yes' when DTUM_IS_USER_MANAGER = 'N' then 'No' else '' end as [IsManager], "
                                + " case when DTUM_STATUS = 'A' then 'Active' when DTUM_STATUS = 'I' then 'Inactive' else '' end as [Status] "
                                + " from TBL_DAILY_TASK_USER_MAS where 1=1 "
                                + utlVO.getCode();
                            break;
                        }
                    case "DeleteUserMasterDetails":
                        {
                            strSql = "Delete from TBL_DAILY_TASK_USER_MAS " + utlVO.getCode();
                            break;
                        }
                    case "getUserMasterMappingDetails":
                        {
                            strSql = "select * from TBL_DAILY_TASK_USER_MAS  "
                                + " left outer join TBL_DAILY_TASK_USER_MNGR_MAPPING on DTUMM_USER_DTUM_ID = DTUM_ID "
                                + utlVO.getCode();
                            break;
                        }
                    case "getDailyTaskDetails":
                        {
                            strSql = "select *,case when DTUM_IS_USER_MANAGER = 'Y' then 'Yes' when DTUM_IS_USER_MANAGER = 'N' then 'No' else '' end as [IsManager], "
                                + " case when DTUM_STATUS = 'A' then 'Active' when DTUM_STATUS = 'I' then 'Inactive' else '' end as [Status] "
                                + " from TBL_DAILY_TASK_TRACKER  "
                                + " inner join TBL_DAILY_TASK_USER_MAS on DTT_ASSIGNED_TO = DTUM_ID "
                                + " inner join TBL_DAILY_TASK_TYPE_OF_WORK_MAS on DTTWM_ID = DTT_TYPE_OF_WORK "
                                + utlVO.getCode();
                            break;
                        }
                    case "getUserMasterMappinglist":
                        {
                            strSql = "select A.DTUM_USER_NAME AS [USER_NAME], B.DTUM_USER_NAME AS [MANAGER_NAME],DTUMM_ID,DTUMM_USER_DTUM_ID,DTUMM_MANAGER_DTUM_ID"
                                + " from  TBL_DAILY_TASK_USER_MNGR_MAPPING  "
                                + " inner join TBL_DAILY_TASK_USER_MAS A on  DTUMM_USER_DTUM_ID = A.DTUM_ID"
                                + " inner join TBL_DAILY_TASK_USER_MAS B on  DTUMM_MANAGER_DTUM_ID = B.DTUM_ID"
                                + utlVO.getCode();
                            break;
                        }
                    case "DeleteUserMgrMapping":
                        {
                            strSql = "Delete from TBL_DAILY_TASK_USER_MNGR_MAPPING " + utlVO.getCode();
                            break;
                        }
                    //>>
                    //<< Added by Bhavik For Advertisement Module @ 09 Jul 2014
                    case "getAdvtCategory":
                        {
                            strSql = "select * from TBL_ADVT_CATEGORY_MAS where 1=1 " + utlVO.getCode();
                            break;
                        }
                    case "getAdvtNature":
                        {
                            strSql = "select * from TBL_ADVT_NATURE_MAS where 1=1 " + utlVO.getCode();
                            break;
                        }
                    case "getAdvtType":
                        {
                            strSql = "select * from TBL_ADVT_TYPE_MAS where 1=1 " + utlVO.getCode();
                            break;
                        }
                    case "getAdvtMedia":
                        {
                            strSql = "select * from TBL_ADVT_MEDIA_MAS where 1=1 " + utlVO.getCode();
                            break;
                        }
                    case "getFormatofAdvt":
                        {
                            strSql = "select * from TBL_ADVT_FORMAT_MAS where 1=1 " + utlVO.getCode();
                            break;
                        }
                    //case "getAdvtTypeOfProduct":
                    //    {
                    //        strSql = "select * from TBL_REF_CODES where 1=1  " + utlVO.getCode();
                    //        break;
                    //    }
                    case "getAdvtTypeOfProduct":
                        {
                            strSql = "select DISTINCT(APM_PRODUCT_TYPE) from TBL_ADVT_PRODUCT_MAS where 1=1  " + utlVO.getCode();
                            break;
                        }
                    case "getAdvtPendingList":
                        {
                            strSql = " select *, CONVERT(VARCHAR(100),ARM_RECEIVED_DATE,106) as [Received_Date],CONVERT(VARCHAR(100),ARM_ARN_NO_RELEASE_DATE,106) as [ARN_Release_Date]," +
                                    " CONVERT(VARCHAR(100),APM_PRODUCT_APPROVAL_DATE ,106) as [Approval_Date] " +
                                    " from [dbo].[TBL_ADVT_REQUEST_MAS] " +
                                    " inner join TBL_ADVT_CATEGORY_MAS on ACM_ID = ARM_ACM_ID " +
                                    " LEFT OUTER join TBL_ADVT_NATURE_MAS on ANM_ID = ARM_ANM_ID " +
                                    " inner join TBL_ADVT_TYPE_MAS on ATM_ID = ARM_ATM_ID " +
                                    " inner join TBL_ADVT_MEDIA_MAS on  AMM_ID = ARM_AMM_ID " +
                                    " inner join TBL_ADVT_FORMAT_MAS on AFM_ID =  ARM_AFM_ID " +
                                    " inner join TBL_ADVT_INTERNAL_EXTERNAL_MAS on AIEM_ID =  ARM_AIEM_ID " +
                                    //" inner join TBL_REF_CODES on RC_ID = ARM_TYPE_OF_PRODUCT and RC_TYPE ='ADVT' " +
                                    " inner join TBL_ADVT_LANGUAGE_MAS on ALM_ID = ARM_NAME_OF_LANGUAGE " +
                                    " left outer join TBL_ADVT_PRODUCT_MAS on APM_ID = ARM_NAME_OF_PRODUCT " +
                                    " inner join TBL_ADVT_STATUS_MAS on ASM_CODE = ARM_STATUS where 1=1  " + utlVO.getCode();
                            break;
                        }
                    case "getRevisionSuggested":
                        {
                            strSql = "select * from TBL_ADVT_REVISION_SUGGESTED where ARS_ARM_ID = " + utlVO.getCode();
                            break;
                        }
                    case "getAdvtStatus":
                        {
                            strSql = "select * from TBL_ADVT_STATUS_MAS where 1=1  " + utlVO.getCode();
                            break;
                        }
                    case "getAdvtAttachment":
                        {
                            strSql = "select * from TBL_ADVT_REQUEST_MAS_FILES  " + utlVO.getCode();
                            break;
                        }
                    case "getTypeOfWork":
                        {
                            strSql = "select * from TBL_DAILY_TASK_TYPE_OF_WORK_MAS where 1=1 " + utlVO.getCode();
                            break;
                        }

                    case "getProductName":
                        {
                            strSql = "select * from TBL_ADVT_PRODUCT_MAS where 1=1  " + utlVO.getCode();
                            break;
                        }
                    case "getLanguage":
                        {
                            strSql = "select * from TBL_ADVT_LANGUAGE_MAS where 1=1  " + utlVO.getCode();
                            break;
                        }
                    //>>
                    //<< Added By Bhavik @11Aug2014
                    case "getDirectorMasterDetails":
                        {
                            strSql = "select *,case when SDM_STATUS = 'A' then 'Active' when SDM_STATUS = 'I' then 'Inactive' else '' end as [Status]  " +
                                "from TBL_SEC_DIRECTOR_MAS where 1=1 " + utlVO.getCode();
                            break;
                        }

                    case "DeleteDirectorMasterDetails":
                        {
                            strSql = "delete from TBL_SEC_DIRECTOR_MAS " + utlVO.getCode();
                            break;
                        }
                    case "SecMeetingType":
                        {
                            strSql = "SELECT * FROM TBL_SEC_MEETING_TYPE " + utlVO.getCode() + " ORDER BY SMT_NAME ";
                            break;
                        }
                    //>>
                    //<< Added By Bhavik @14Aug2014
                    case "getSittingFeesDetail":
                        {
                            strSql = "select * from TBL_SITTING_FEES_DETAILS " +
                                " inner join TBL_SEC_DIRECTOR_MAS on SDM_ID = SFD_SDM_ID" +
                                " inner join TBL_SEC_MEETING_TYPE on SMT_ID = SFD_SMT_ID" +
                                " inner join TBL_SEC_MEETING on SM_ID = SFD_SM_ID " + utlVO.getCode();
                            break;
                        }
                    case "SecMeeting":
                        {
                            strSql = "SELECT SM_ID,SMT_NAME + ' - '+ CAST(SM_NO_OF_MEETINGS AS VARCHAR)+ ' - '+ CONVERT(VARCHAR,SM_DATE ,106) AS MeetingDets " +
                                "FROM TBL_SEC_MEETING INNER JOIN  TBL_SEC_MEETING_TYPE ON TBL_SEC_MEETING.SM_SMT_ID = TBL_SEC_MEETING_TYPE.SMT_ID " + utlVO.getCode();
                            break;
                        }
                    //>>
                    //<< Added By Bhavik @16Aug2014
                    case "DeleteSittingFeesDetail":
                        {
                            strSql = "Delete from TBL_SITTING_FEES_DETAILS where " + utlVO.getCode();
                            break;
                        }
                    //>>
                    //<< Added By Bhavik @18Aug2014
                    case "getMeetingDetails":
                        {
                            strSql = "select * from TBL_SEC_MEETING " +
                                    " inner join TBL_SEC_MEETING_TYPE on SMT_ID = SM_SMT_ID " +
                                    " where 1=1 " + utlVO.getCode();
                            break;
                        }
                    case "getMeetingConstnDetails":
                        {
                            strSql = " select * from TBL_SEC_COMM_CONSTN_MAS " +
                                     " inner join TBL_SEC_DIRECTOR_MAS on SCCM_SDM_ID = SDM_ID " +
                                     " inner join TBL_SEC_MEETING_TYPE on SMT_ID = SCCM_SMT_ID where 1=1 " + utlVO.getCode();
                            break;
                        }
                    case "getMeetingInviteesDetails":
                        {
                            strSql = " select * from TBL_SEC_MEET_INVITEE_MAS where 1=1  " + utlVO.getCode();
                            break;
                        }
                    case "getMeetingDocumentDetails":
                        {
                            strSql = " select * from TBL_SEC_MEETING_DOCS " +
                            " inner join TBL_SEC_MEETING_TYPE on SMT_ID = SMD_SMT_ID " +
                            " inner join TBL_SEC_MEETING on SM_ID = SMD_SM_ID " +
                            " where 1=1  " + utlVO.getCode();
                            break;
                        }
                    //>>
                    //<< Added By Bhavik @19Aug2014
                    case "getSecPersonResponsible":
                        {
                            strSql = " select * from TBL_SEC_PERSON_RESPONSIBLE_MAS where 1=1  " + utlVO.getCode();
                            break;
                        }
                    //>>
                    //<< Added By Bhavik @20Aug2014
                    case "getCompanyName":
                        {
                            strSql = " select * from TBL_SEC_RELATED_PARTY_MAS where 1=1  " + utlVO.getCode();
                            break;
                        }
                    case "getNatureofInterest":
                        {
                            strSql = " select * from TBL_SEC_NATURE_OF_INTEREST_MAS where 1=1  " + utlVO.getCode();
                            break;
                        }
                    case "getThirdpartyDetails":
                        {
                            strSql = " select * from TBL_SEC_THIRD_PARTY_INTEREST " +
                                    " inner join TBL_SEC_DIRECTOR_MAS on SDM_ID = STPI_SDM_ID" +
                                    " inner join TBL_SEC_RELATED_PARTY_MAS on SRPM_ID = STPI_SRPM_ID " +
                                    " inner join TBL_SEC_NATURE_OF_INTEREST_MAS on SNIM_ID = STPI_SNIM_ID where 1=1 " + utlVO.getCode();
                            break;
                        }
                    case "DeleteThirdPartyInterestDetail":
                        {
                            strSql = " delete from TBL_SEC_THIRD_PARTY_INTEREST  where " + utlVO.getCode();
                            break;
                        }
                    case "InsertSecThirdPartyNatureofChange":
                        {
                            strSql = " Insert into  TBL_SEC_THIRD_PARTY_NATURE_CHANGE   " + utlVO.getCode();
                            break;
                        }
                    //>>
                    //<< Added By Bhavik @26Aug2014
                    case "getFAQList":
                        {
                            strSql = "select * from TBL_FAQ_MAS where 1=1 " + utlVO.getCode();
                            break;
                        }
                    case "getFAQListFiles":
                        {
                            strSql = "select * from TBL_FAQ_FILES where 1=1 " + utlVO.getCode();
                            break;
                        }
                    case "DeleteFAQList":
                        {
                            strSql = "delete from TBL_FAQ_MAS where  FAQM_ID = " + utlVO.getCode();
                            break;
                        }
                    case "DeleteFAQListFiles":
                        {
                            strSql = "delete from TBL_FAQ_FILES where  FAQF_FAQM_ID = " + utlVO.getCode();
                            break;
                        }
                    //>>
                    //<< Added By Bhavik @03Sep2014
                    case "getAuditorMasterDetails":
                        {
                            strSql = "select *,case when SAM_STATUS = 'A' then 'Active' when SAM_STATUS = 'I' then 'Inactive' else '' end as [Status]  " +
                               "from TBL_SEC_AUDITOR_MAS " +
                               " inner join TBL_SEC_AUDITOR_TYPE_MAS on SATM_ID = SAM_TYPE_OF_AUDITOR " +
                               "where 1=1 " + utlVO.getCode();
                            break;
                        }
                    case "DeleteAuditorMasterDetails":
                        {
                            strSql = "Delete from TBL_SEC_AUDITOR_MAS " + utlVO.getCode();
                            break;
                        }
                    case "getManagementDetails":
                        {
                            strSql = "select *,case when SSMCDM_STATUS = 'A' then 'Active' when SSMCDM_STATUS = 'I' then 'Inactive' else '' end as [Status]  " +
                                "from TBL_SEC_SR_MGHT_COMM_MEMBER_MAS where 1=1 " + utlVO.getCode();
                            break;
                        }
                    case "DeleteManagementMasterDetails":
                        {
                            strSql = "Delete from TBL_SEC_SR_MGHT_COMM_MEMBER_MAS " + utlVO.getCode();
                            break;
                        }
                    case "getKeyManagerialDetails":
                        {
                            strSql = "select *,case when SKMPM_STATUS = 'A' then 'Active' when SKMPM_STATUS = 'I' then 'Inactive' else '' end as [Status]  " +
                                "from TBL_SEC_KEY_MANAGERIAL_PERSON_MAS where 1=1 " + utlVO.getCode();
                            break;
                        }
                    case "DeleteKeyManagerialMasterDetails":
                        {
                            strSql = "Delete from TBL_SEC_KEY_MANAGERIAL_PERSON_MAS " + utlVO.getCode();
                            break;
                        }
                    case "getAuditorType":
                        {
                            strSql = "select * from TBL_SEC_AUDITOR_TYPE_MAS where 1=1 " + utlVO.getCode();
                            break;
                        }
                    //>>
                    //<< Added By Bhavik @08Sep2014
                    case "getSecFinanceDeptMailId":
                        {
                            strSql = "select * " + "from TBL_SEC_FINANCE_MAIL_CONFIG_MAS where 1=1 AND SFMCM_STATUS = 'A' " + utlVO.getCode();
                            break;
                        }
                    case "deleteSecDocMeetingFiles":
                        {
                            strSql = "delete from TBL_SEC_MEETING_DOC_FILES where SMDF_ID = " + utlVO.getCode();
                            break;
                        }
                    //>>
                    //<< Added By Bhavik @08Sep2014
                    case "deleteFAQFiles":
                        {
                            strSql = "delete from TBL_FAQ_FILES where FAQF_ID = " + utlVO.getCode();
                            break;
                        }
                    //>>
                    //<< Added By Bhavik @23Sep2014
                    case "getDataforThirdPartyInterestMail":
                        {
                            strSql = "select * from TBL_SEC_DIRECTOR_MAS " +
                                    " left outer join TBL_SEC_THIRD_PARTY_INTEREST on STPI_SDM_ID = SDM_ID " +
                                    " left outer join TBL_SEC_RELATED_PARTY_MAS on SRPM_ID = STPI_SRPM_ID " +
                                    " left outer join TBL_SEC_NATURE_OF_INTEREST_MAS on SNIM_ID = STPI_SNIM_ID " +
                                    " where 1=1 " + utlVO.getCode();
                            break;
                        }

                    //>>

                    // Added By Supriya on 17-Nov-2014
                    case "AdvertisementAppDept":
                        {
                            strSql = "SELECT * FROM TBL_ADVT_APPROVING_DEPT where 1=1 " + utlVO.getCode() + "  ORDER BY AAD_NAME ";
                            break;
                        }
                    case "AdvtReqForApproval":
                        {
                            strSql = "select *, CONVERT(VARCHAR(100),ARM_RECEIVED_DATE,106) as [Received_Date], " +
                                "  CONVERT(VARCHAR(100),ARM_ARN_NO_RELEASE_DATE,106) as [ARN_Release_Date],  " +
                                "  CONVERT(VARCHAR(100),APM_PRODUCT_APPROVAL_DATE ,106) as [Approval_Date]  " +
                                "  FROM TBL_ADVT_APPROVING_DEPT " +
                                "  INNER JOIN TBL_ADVT_USERS ON AAD_ID = AU_AAD_ID  " +
                                "  INNER JOIN TBL_ADVT_DEPT_APPROVERS ON AAD_ID = ADA_DEPT_ID  " +
                                "  INNER JOIN TBL_ADVT_REQUEST_MAS ON ADA_ARM_ID = ARM_ID  " +
                                "  INNER JOIN TBL_ADVT_PRODUCT_MAS ON ARM_NAME_OF_PRODUCT = APM_ID  " +
                                "  inner join TBL_ADVT_CATEGORY_MAS on ACM_ID = ARM_ACM_ID   " +
                                "  left outer join TBL_ADVT_NATURE_MAS on ANM_ID = ARM_ANM_ID  " +
                                "  inner join TBL_ADVT_TYPE_MAS on ATM_ID = ARM_ATM_ID  " +
                                "  inner join TBL_ADVT_MEDIA_MAS on  AMM_ID = ARM_AMM_ID " +
                                "  inner join TBL_ADVT_LANGUAGE_MAS on ALM_ID = ARM_NAME_OF_LANGUAGE " +
                                "  inner join TBL_ADVT_STATUS_MAS on ASM_CODE = ARM_STATUS " +
                                "  inner join TBL_ADVT_FORMAT_MAS on AFM_ID = ARM_AFM_ID " +
                                "  inner join TBL_ADVT_INTERNAL_EXTERNAL_MAS on AIEM_ID = ARM_AIEM_ID " +
                                "  WHERE ADA_ADVT_STATUS IN ('PFA', 'RS') "
                                + utlVO.getCode();
                            break;
                        }
                    case "AdvtApprovalDeptById":
                        {
                            strSql = "select  * , case when ADA_ADVT_STATUS = 'PFA' then 'Pending for Approval' when ADA_ADVT_STATUS = 'A' then 'Approved'  " +
                                    " when ADA_ADVT_STATUS = 'R' then 'Rejected' end as [Status] " +
                                    "  from TBL_ADVT_DEPT_APPROVERS " +
                                    " inner join TBL_ADVT_APPROVING_DEPT on ADA_DEPT_ID =AAD_ID " +
                                    " where  1=1 " + utlVO.getCode();
                            break;
                        }

                    case "AdvtDeptOwners":
                        {
                            strSql = "select * from TBL_ADVT_USERS where 1 =1 " + utlVO.getCode();
                            break;
                        }
                    case "AdvertisementRequest":
                        {
                            strSql = "select * from TBL_ADVT_REQUEST_MAS where 1 =1 " + utlVO.getCode();
                            break;
                        }
                    case "AdvtPendingApprovalDeptOwners":
                        {
                            strSql = "select * from TBL_ADVT_DEPT_APPROVERS " +
                                     "  inner join TBL_ADVT_USERS on AU_AAD_ID = ADA_DEPT_ID " +
                                     " where 1 =1 " + utlVO.getCode();
                            break;
                        }
                    //<<Added by Bhavik @17Dec2014
                    case "getDataRepositoryDetails":
                        {
                            strSql = " Select * from TBL_DATA_REPOSITORY_MAS  " +
                                     " where 1=1 " + utlVO.getCode();
                            break;
                        }
                    case "deleteDataRepositoryDetails":
                        {
                            strSql = " delete from TBL_DATA_REPOSITORY_MAS  where DRM_ID = " + utlVO.getCode();
                            break;
                        }
                    //>>

                    //<<Added by Rakesh on 02Apr2015
                    case "getRevisionSuggestionFiles":
                        {
                            strSql = " select * from TBL_ADVT_REVISION_SUGGESTED_FILES where ARSF_ARS_ID = " + utlVO.getCode();
                            break;
                        }
                    //>>
                    //Added By Milan yadav on 11Jul2016
                    //>>
                    case "getChecklistFile":
                        {
                            strSql = " select * from TBL_CERT_CHECKLIST_DETS where 1=1 " + utlVO.getCode();
                            break;
                        }
                    //<<     
                    //<< Code Added by Milan on 8-Mar-2017
                    case "getInternalExternalofAdvt":
                        {
                            strSql = "select * from TBL_ADVT_INTERNAL_EXTERNAL_MAS where AIEM_STATUS ='A'" + utlVO.getCode();
                            break;
                        }
                    //>>
                    //<< Code Added by Milan on 8-Mar-2017
                    case "getADVWithdrawingDeptMailId":
                        {
                            strSql = "select * from TBL_ADVT_DEPT_USERS " + utlVO.getCode();
                            break;
                        }
                    //>>
                    //<< Code Added by Milan on 8-Mar-2017
                    case "getWithdrawnDetails":
                        {
                            strSql = " select * from TBL_ADV_WITHDRAWING_PRODUCT " +
                                     " inner join TBL_REF_CODES ON RC_CODE = AWP_WITHDRAWN where 1=1 " + utlVO.getCode();
                            break;
                        }

                    case "getWithdrawnFiles":
                        {
                            strSql = "select * from TBL_ADV_WITHDRAWING_PRODUCT_FILES where 1=1 " + utlVO.getCode();
                            break;
                        }
                    //>>

                    //<< Code Added by Milan on 8-Mar-2017
                    case "getAdvtFiledURNno":
                        {
                            strSql = "select * from TBL_ADVT_REQUEST_MAS where 1=1 " + utlVO.getCode();
                            break;
                        }
                    //<< Code Added by Supriya on 14-Jun-2017
                    case "getCXOandDepartmentName":
                        {
                            strSql = " select CDM_NAME, CDM_CXO_USERID from TBL_CERT_CHECKLIST_DETS " +
                            " inner join TBL_CERT_CHECKLIST_MAS on CCM_ID = CCD_CCM_ID " + utlVO.getCode() +
                            " inner join TBL_CERT_SUB_SUB_DEPT_MAS on CCM_CSSDM_ID = CSSDM_ID " +
                            " inner join TBL_CERT_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID " +
                            " inner join TBL_CERT_DEPT_MAS on CSDM_CDM_ID = CDM_ID";
                            break;
                        }
                    case "getCountofCertificate":
                        {
                            strSql = " select Count(*) as Count from TBL_CERT_MAS Where 1=1 " + utlVO.getCode();
                            break;
                        }
                    //>>
                    case "getDeptCXO":
                        {
                            strSql = " select CDM_CXO_USERID,CDM_CXO_NAME,* from TBL_CERT_SUB_SUB_DEPT_MAS " +
                                    " inner join TBL_CERT_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID " +
                                    " inner join TBL_CERT_DEPT_MAS on CSDM_CDM_ID = CDM_ID " +
                                    " where 1=1 " + utlVO.getCode();
                            break;
                        }
                    case "getDeptUnitHead":
                        {
                            strSql = " select distinct CSDM_EMP_NAME from TBL_CERT_SUB_SUB_DEPT_MAS " +
                                    " inner join TBL_CERT_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID " +
                                    " where 1=1 " + utlVO.getCode();
                            break;
                        }
                    //>>
                    case "getCertificationQuarter":
                        {
                            strSql = " select * from TBL_CERT_QUARTER_MAS " +
                                    " inner join TBL_CERTIFICATIONS on CERT_CQM_ID = CQM_ID " +
                                    " inner join TBL_CERT_MAS on CERTM_ID = CERT_CERTM_ID " +
                                    " where CQM_STATUS = 'A' " + utlVO.getCode();
                            break;
                        }
                    case "GetCircularAdditionalMails":
                        strSql = "select * from TBL_CIRCULAR_ADDITIONAL_EMAILS WHERE 1=1 " + utlVO.getCode();
                        break;
                    case "ReportingDeptAllLevels":
                        {
                            //<<Modified by Ankur Tyagi on 29Mar2024 for CR_2005
                            strSql = " select UserName,EmailId from " +
                                     " (SELECT SRDOM_EMP_NAME as UserName,SRDOM_EMAILID as EmailId From " +
                                     " TBL_SUB_SRD_OWNER_MASTER " +
                                     " INNER JOIN TBL_SUB_REPORTING_DEPT on " +
                                     " SRD_ID = SRDOM_SRD_ID " +
                                     " where SRDOM_STATUS='A' AND SRD_STATUS='A' AND SRD_ID = " + utlVO.getCode() +
                                     " ) as tab ";
                            //" union " +
                            //" select SE_FIRST_NAME as UserName,SE_EMAIL_ID as EmailId from TBL_SUB_ESCALATION " +
                            //" where SE_STATUS='A' AND [SE_SRD_ID ]= " + utlVO.getCode() + " ) as tab ";
                            break;
                            //>>
                        }
                }
                using (F2FDatabase DB = new F2FDatabase(strSql))
                {
                    DB.F2FDataAdapter.Fill(resultdata);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return resultdata;
        }

        public DataSet getDataSet(string code, string conStr = null)
        {
            DataSet resultdata = new DataSet();
            string strSql = "";
            try
            {
                switch (code)
                {
                    case "getState":
                        {
                            strSql = "Select * from TBL_TAX_STATE_MAS where SM_STATUS ='A' order by SM_NAME;";
                            break;
                        }
                    case "getTaxType":
                        {
                            strSql = "Select * from TBL_TAX_TAX_TYPE_MAS where TTTM_STATUS ='A' order by TTTM_NAME;";
                            break;
                        }
                    case "getTaxSubType":
                        {
                            strSql = "Select * from TBL_TAX_REF_CODES where TRC_STATUS ='A' and TRC_TYPE='Tax Sub Type' order by TRC_NAME;";
                            break;
                        }
                    case "getTaxOwners":
                        {
                            strSql = "Select * from TBL_TAX_OWNER_MAS where TOM_STATUS ='A' order by TOM_EMP_NAME;";
                            break;
                        }
                    case "getModeofPayment":
                        {
                            strSql = "Select * from TBL_TAX_REF_CODES where TRC_STATUS ='A' and TRC_TYPE='Mode of Payment' order by TRC_NAME;";
                            break;
                        }
                    case "getFileType":
                        {
                            strSql = "Select * from TBL_TAX_REF_CODES where TRC_STATUS ='A' and TRC_TYPE='File Type' order by TRC_NAME;";
                            break;
                        }
                    case "getTaxationFinYear":
                        {
                            strSql = "Select * from TBL_TAX_FIN_YEAR_MAS where TFYM_STATUS ='A'";
                            break;
                        }
                    //// Added By Supriya on 18-Feb-2014
                    //case "getReferences":
                    //    {
                    //        strSql = "Select DISTINCT RC_TYPE from TBL_REF_CODES WHERE RC_STATUS = 'A' " +
                    //                " ORDER BY RC_TYPE";
                    //        break;
                    //    }
                    //case "Roles":
                    //    {
                    //        strSql = "select Id as RoleId, Name as RoleName from my_aspnet_Roles order by Name ";
                    //        break;
                    //    }
                    case "TrackingDept":
                        {
                            strSql = " select  distinct STM_TYPE,STM_ID from TBL_SUB_TYPE_MAS " +
                                     " left outer join TBL_SUBMISSIONS_MAS on  SM_STM_ID = STM_ID " +
                                     " inner  join TBL_SUB_CHKLIST on SC_SM_ID = SM_ID " +
                                     " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID]  order by STM_TYPE ASC ";
                            break;
                        }

                    case "ReportingDept":
                        {
                            strSql = "select  distinct SRD_NAME,SRD_ID from TBL_SUB_REPORTING_DEPT " +
                                    " left outer join TBL_SUBMISSIONS_MAS on SM_SRD_ID = SRD_ID " +
                                    " inner join TBL_SUB_CHKLIST on SC_SM_ID = SM_ID " +
                                    " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID]  order by SRD_NAME ASC";
                            break;
                        }

                    case "getReferences":
                        {
                            strSql = "Select RC_ID, RC_TYPE from TBL_REF_CODES ORDER BY RC_TYPE";
                            break;
                        }
                }
                DataSet ds = new DataSet();
                using (F2FDatabase DB = new F2FDatabase(strSql))
                {
                    DB.F2FDataAdapter.Fill(ds);
                }
                resultdata = ds;
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return resultdata;
        }

        public DataTable getDataSetwithTwoCondition(string code, UtilitiesVO objVO)
        {
            DataTable resultdata = new DataTable();
            string strSql = "";
            try
            {
                switch (code)
                {
                    case "getAssignedTo":
                        {
                            strSql = " select * from ( "
                                + " select DTUM_ID,DTUM_USER_NAME,DTUM_USER_ID,DTUM_IS_USER_MANAGER from TBL_DAILY_TASK_USER_MAS WHERE DTUM_USER_ID = '" + objVO.getCondition1() + "' "
                                + "UNION "
                                + "select DTUM_ID,DTUM_USER_NAME,DTUM_USER_ID,DTUM_IS_USER_MANAGER from TBL_DAILY_TASK_USER_MAS "
                                + "INNER join TBL_DAILY_TASK_USER_MNGR_MAPPING on DTUMM_USER_DTUM_ID = DTUM_ID "
                                + "and "
                                + "DTUMM_MANAGER_DTUM_ID in (select  DTUM_ID from TBL_DAILY_TASK_USER_MAS where DTUM_USER_ID = '" + objVO.getCondition2() + "') "
                                + ") as tab order by DTUM_USER_NAME";
                            break;
                        }
                    case "getRepositryDetail":
                        {
                            strSql = "Select * , case when RD_Is_Active = 'A' then 'Active' when RD_Is_Active = 'I' then 'Inactive' " +
                                        " else '' end as [Status]  from TBL_REPOSITORY_DOCS ";
                            break;
                        }
                }
                using (F2FDatabase DB = new F2FDatabase(strSql))
                {
                    DB.F2FDataAdapter.Fill(resultdata);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return resultdata;
        }

        public DataTable getDatasetWithConditionInString(string code, string str, string conStr)
        {
            DataTable resultDataSet = new DataTable();
            try
            {
                string strSql = "";
                switch (code)
                {
                    case "REPORTINGDEPT_BY_SRDOM_EMP_ID":
                        {
                            strSql = "select * from TBL_SUB_REPORTING_DEPT " +
                                " inner join TBL_SUB_SRD_OWNER_MASTER on SRDOM_SRD_ID = SRD_ID AND SRDOM_STATUS = 'A' AND SRDOM_EMP_ID = '" + str + "' " +
                                " where SRD_STATUS = 'A' order by SRD_NAME";
                            break;
                        }
                    case "getAllNonComplianceChecklistusingQuarterId":
                        {
                            string qry = @"SELECT CSDM_NAME as [For_UH], CSDM_NAME+' - '+CSSDM_NAME as [Dept_FH], CDM_NAME+' - '+CSDM_NAME+' - '+CSSDM_NAME as [For_Others],
                            CCM_ID as [ID], rc1.RC_NAME as [Compliance_Status],*,
                            REPLACE(CONVERT(VARCHAR(11), CCM_EFFECTIVE_FROM, 106), ' ', '-') as [Effective From] FROM TBL_CERT_CHECKLIST_DETS
                            INNER JOIN TBL_CERT_CHECKLIST_MAS ON CCM_ID = CCD_CCM_ID AND CCD_YES_NO_NA in ('N', 'W')
                            inner join TBL_CERT_SUB_SUB_DEPT_MAS on CCM_CSSDM_ID = cssdm_id
                            inner join TBL_CERT_SUB_DEPT_MAS on CSDM_ID = CSSDM_CSDM_ID
                            inner join TBL_CERT_DEPT_MAS on CDM_ID = CSDM_CDM_ID
                            left outer join TBL_REF_CODES as rc1 on CCD_YES_NO_NA = rc1.RC_CODE and rc1.RC_TYPE = 'Certification Compliance Status'
                            
                            WHERE CCD_CERT_ID IN
                            (
                                SELECT SubUnit.CERT_ID FROM TBL_CERTIFICATIONS SubUnit--Func.CERT_ID, Unit.CERT_ID,
                                INNER JOIN TBL_CERTIFICATIONS Unit ON Unit.CERT_ID = SubUnit.CERT_PARENT_ID

                                AND SubUnit.CERT_LEVEL = 0 AND Unit.CERT_LEVEL = 1

                                INNER JOIN TBL_CERTIFICATIONS Func ON Func.CERT_ID = Unit.CERT_PARENT_ID

                                AND Func.CERT_LEVEL = 2

                                ";
                            qry += " WHERE Func.CERT_CQM_ID = " + str + ")";
                            strSql = qry;
                            break;
                        }

                    case "CircularAttachments":
                        {
                            strSql = "select * from TBL_CIRCULAR_FILES where CF_ID in (" + str + ")";
                            break;
                        }
                    case "areSubmissionsDone":
                        {
                            strSql = "select * from TBL_SUB_CHKLIST inner join tbl_submissions on sub_sc_id=sc_id where SC_EEM_ID IN ( " + str + " )";
                            break;
                        }

                    case "CERTCONTENT":
                        {
                            strSql = "select CERTM_ID,CERTM_TEXT,CDO_DESG,CDO_NAME, CDM_NAME,CDM_ID from TBL_CERT_MAS " +
                                        " inner join TBL_CERT_DEPT_MAS on (CERTM_DEPT_ID = CDM_ID and CDM_NAME != 'Common')" +
                                        " inner join TBL_CERT_DEPT_OWNERS " +
                                        " ON CDM_ID=CDO_CDM_ID where CDO_USER_NAME= '" + str + "'" +
                                        " and not exists (select CERT_ID from TBL_CERTIFICATIONS " +
                                        " inner join TBL_CERT_QUARTER_MAS on CERT_CERTM_ID = CERTM_ID AND CQM_ID = CERT_CQM_ID " +
                                        " and CQM_STATUS = 'A' and CERT_STATUS in ('S', 'A'))";
                            //" and not exists (select CERT_ID from TBL_CERTIFICATIONS " +
                            //" inner join TBL_CERT_QUARTER_MAS on CQM_ID = CERT_CQM_ID " +
                            //" and CQM_STATUS = 'A' )";
                            break;
                        }


                    case "deleteExceptions":
                        {
                            strSql = "DELETE FROM TBL_CERT_EXCEPTION where CE_ID in (" + str + ")";
                            break;
                        }
                    case "certStatus":
                        {
                            strSql = "SELECT * FROM TBL_CERTIFICATIONS  inner join " +
                                   " TBL_CERT_QUARTER_MAS on (CQM_ID = CERT_CQM_ID and CQM_STATUS = 'A' AND cert_status in ('D', 'R')) " +
                                   " INNER JOIN TBL_CERT_MAS on CERT_CERTM_ID = CERTM_ID " +
                                   " inner join TBL_CERT_DEPT_MAS on CERTM_DEPT_ID = CDM_ID " +
                                   " inner join TBL_CERT_DEPT_OWNERS " +
                                   " ON CDM_ID = CDO_CDM_ID  where CDO_USER_NAME=  '" + str + "'";
                            break;
                        }
                    case "getDesignation":
                        {
                            strSql = "select CDO_DESG from TBL_CERT_DEPT_OWNERS inner join TBL_CERT_DEPT_MAS " +
                            " on CDO_CDM_ID = CDM_ID and CDM_NAME = 'Common' and CDO_USER_NAME='" + str + "'";
                            break;
                        }
                    case "CommonCertByQuarter":
                        {
                            strSql = "select * from TBL_COMMON_CERTIFICATIONS " +
                                    " inner join TBL_CERT_QUARTER_MAS on (CQM_ID = CC_CQM_ID and CQM_STATUS = 'A')" +
                                    " where CC_STATUS != 'D' " + str;
                            break;
                        }


                    case "getCompUserCodebyZone":
                        {
                            strSql = "SELECT EZM_EMAIL_ID as EmailId FROM TBL_EMP_ZONE_MAPPING WHERE EZM_ZONE_ID = '" + str + "'";
                            break;
                        }
                    //>>

                    //Added by Supriya on 28-Jun-2013
                    case "getApprovalDets":
                        {

                            strSql = "select * from TBL_CERT_DEPT_OWNERS where CDO_DESG = '" + str + "'";
                            break;
                        }

                    // Added By Supriya on 06-Sep-2013
                    case "useConfigParams":
                        {
                            strSql = " Select CP_VALUE FROM TBL_CONFIG_PARAMS WHERE CP_NAME = '" + str + "'";
                            break;
                        }
                    case "DeleteLitgationAdvFeesbyId":
                        {
                            strSql = "delete from TBL_COMP_ADV_FEES where CLAF_ID=" + str;
                            break;
                        }

                    // Modify By milan Yadav on 09Jul2016
                    case "ChecklistDetails":
                        {
                            strSql = "select *,Replace(CONVERT(VARCHAR(11), CCM_EFFECTIVE_FROM, 106), ' ', '-') as [Effective From], " +
                                     "case when CCM_STATUS ='A' then 'Active' when CCM_STATUS ='I' then 'Inactive' else '' end as [Status], CCM_REMARK as [Remark] from dbo.TBL_CERT_CHECKLIST_MAS " +
                                     "inner join  TBL_CERT_DEPT_MAS on CDM_ID = CCM_CDM_ID " +
                                     "inner join  TBL_CERT_QUARTER_MAS on CQM_STATUS ='A' " +
                                     "and CCM_CDM_ID=" + str + " and " +//and CCM_STATUS ='A'
                                     " CCM_EFFECTIVE_FROM <= CQM_FROM_DATE";
                            break;
                        }


                    // Added By Bhavik on 04-Aug-2014
                    case "getCertificationChecklistDetails":
                        {
                            strSql = "select *,Replace(CONVERT(VARCHAR(11), CCM_EFFECTIVE_FROM, 106), ' ', '-') as [Effective From], " +
                                     "case when CCM_STATUS ='A' then 'Active' when CCM_STATUS ='I' then 'Inactive' else '' end as [Status], CCM_REMARK as [Remark] from dbo.TBL_CERT_CHECKLIST_MAS " +
                                     "inner join  TBL_CERT_DEPT_MAS on CDM_ID = CCM_CDM_ID " +
                                     "inner join  TBL_CERT_QUARTER_MAS on CQM_STATUS ='A' " +
                                     "and CCM_CDM_ID=" + str + "and " +//and CCM_STATUS ='A'
                                     " CCM_EFFECTIVE_FROM <= current_timestamp " + //CQM_FROM_DATE
                                     "left outer join TBL_CERT_CHECKLIST_DETS on CCD_CCM_ID = CCM_ID ";
                            break;
                        }
                    // Added By Bhavik on 30-Sep-2013
                    case "GetAllChecklistDetails":
                        {
                            //strSql = "select CCM_ID as [ID],CDM_NAME as [Department Name],CCM_RELEVANT_TO as [Relevant To],CCM_ACT_REGULATION_NAME as [Act Regulation],CCM_NATURE_OF_COMPLIANCE as [Nature Of Compliance],Replace(CONVERT(VARCHAR(11), CCM_EFFECTIVE_FROM, 106), ' ', '-') as [Effective From], " +
                            //         "case when CCM_STATUS ='A' then 'Active' when CCM_STATUS ='I' then 'Inactive' else '' end as [Status], CCM_REMARK as [Remark] from dbo.TBL_CERT_CHECKLIST_MAS " +
                            //         "inner join  TBL_CERT_DEPT_MAS on CDM_ID = CCM_CDM_ID " +
                            //         "inner join  TBL_CERT_QUARTER_MAS on CQM_STATUS ='A' " +
                            //         "and CCM_CDM_ID=" + str +
                            //         "and CCM_EFFECTIVE_FROM <= CQM_FROM_DATE";
                            strSql = "select *,Replace(CONVERT(VARCHAR(11), CCM_EFFECTIVE_FROM, 106), ' ', '-') as [Effective From], " +
                                     "case when CCM_STATUS ='A' then 'Active' when CCM_STATUS ='I' then 'Inactive' else '' end as [Status], CCM_REMARK as [Remark] from dbo.TBL_CERT_CHECKLIST_MAS " +
                                     "inner join  TBL_CERT_DEPT_MAS on CDM_ID = CCM_CDM_ID " +
                                     "and CCM_CDM_ID=" + str;

                            break;
                        }


                    //<<Added by Bhavik @ 08Jul2014
                    case "getTypeofCircular":
                        {
                            strSql = "select * from TBL_CIRCULAR_DOCUMENT_TYPE_MAS where 1=1 " + str + " order by CDTM_TYPE_OF_DOC";
                            break;
                        }
                    //>>

                    //<<Added by Milan Yadav on 09Jul2016
                    case "getSubTypeofCircular":
                        {
                            strSql = "select * from TBL_REF_CODES where RC_TYPE='Circular Document subtype' order by RC_NAME";
                            break;
                        }
                    //>>
                    // <<Added By Bhavik on 19 Jul 2014
                    case "CircularPersonResponsible":
                        {
                            strSql = "select *  from TBL_CIRCULAR_PERSON_RESPONSIBLE_MAS where 1=1 " + str + " order by CPRM_NAME ";
                            break;
                        }
                    case "getTypeofOutward":
                        {
                            strSql = "select *  from TBL_OUTWARD_TYPE_MASTER where 1=1 " + str + " order by OTM_NAME ";
                            break;
                        }
                    //>>

                    //>>
                    // <<Added By Bhavik on 21 Jul 2014
                    case "getStatus":
                        {
                            strSql = "Select * FROM TBL_STATUS_MAS WHERE SM_TYPE = '" + str + "'" +
                                     "ORDER BY SM_DESC";
                            break;
                        }
                    case "getRoleExist":
                        {
                            strSql = "select * from aspnet_Users " +
                                " left outer join aspnet_UsersInRoles on aspnet_Users.UserId= aspnet_UsersInRoles.UserId " +
                                " left outer join aspnet_Roles on aspnet_Roles.RoleId = aspnet_UsersInRoles.RoleId " +
                                " where aspnet_Roles.RoleName in " +
                                " ('ContractAdmin', 'ContractUser') and UserName = '" + str + "'";
                            break;
                        }


                    //>>
                    // <<Added By Bhavik on 5 Aug 2014
                    case "certStatusBbyDeptId":
                        {
                            strSql = "SELECT * FROM TBL_CERTIFICATIONS  inner join " +
                                   " TBL_CERT_QUARTER_MAS on (CQM_ID = CERT_CQM_ID and CQM_STATUS = 'A' AND cert_status = 'D') " +
                                   " INNER JOIN TBL_CERT_MAS on CERT_CERTM_ID = CERTM_ID " +
                                   " inner join TBL_CERT_DEPT_MAS on CERTM_DEPT_ID = CDM_ID and CDM_NAME != 'Common'" +
                                   " inner join TBL_CERT_DEPT_OWNERS " +
                                   " ON CDM_ID = CDO_CDM_ID  where CDM_ID =  '" + str + "'";
                            break;
                        }
                    case "CERTCONTENTbyDeptId":
                        {
                            strSql = "select CERTM_ID,CERTM_TEXT,CDO_DESG,CDO_NAME, CDM_NAME,CDM_ID from TBL_CERT_MAS " +
                                        " inner join TBL_CERT_DEPT_MAS on (CERTM_DEPT_ID = CDM_ID and CDM_NAME != 'Common')" +
                                        " inner join TBL_CERT_DEPT_OWNERS " +
                                        " ON CDM_ID=CDO_CDM_ID where CDM_ID= '" + str + "'" +
                                        " and not exists (select CERT_ID from TBL_CERTIFICATIONS " +
                                        " inner join TBL_CERT_QUARTER_MAS on CERT_CERTM_ID = CERTM_ID AND CQM_ID = CERT_CQM_ID " +
                                        " and CQM_STATUS = 'A' and CERT_STATUS = 'S')";
                            break;
                        }
                    //>>

                    //>>
                    //<< Added By Bhavik on 08-Aug-2014
                    case "getCircularActionablePersonRespEmail":
                        {
                            strSql = "select* from TBL_CIRCULAR_PERSON_RESPONSIBLE_MAS where 1=1  " + str;
                            break;
                        }
                    //>>  



                    //Modify By Milan Yadav on 11Jul2016
                    //>>
                    case "getCertDeptById":
                        {
                            strSql = "SELECT CSSDM_ID, CDM_NAME  + ' - ' + CSDM_NAME + ' - ' + CSSDM_NAME as DeptName, " +
                                    " CSSDM_ID as RelevantDeptId " +
                                    " FROM  TBL_CERT_SUB_SUB_DEPT_MAS " +
                                    " inner join TBL_CERT_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID " +
                                    " inner join TBL_CERT_DEPT_MAS on CSDM_CDM_ID = CDM_ID where 1=1 ";

                            if (!str.Equals(""))
                            {
                                strSql = strSql + " and CSSDM_ID = " + str;
                            }
                            //Added By Milan Yadav on 09Jul2016
                            strSql = strSql + "order by CDM_NAME,CSDM_NAME,CSSDM_NAME";

                            break;
                        }
                    //<<

                    //Added by Supriya on 14-Sep-2015
                    case "getCertContentById":
                        {
                            //strSql = "select distinct TBL_CERT_MAS.* from TBL_CERT_MAS "+
                            //    " inner join  TBL_CERT_DEPT_MAS on CERTM_DEPT_ID = CDM_ID "+
                            //    " inner join TBL_CERT_SUB_DEPT_MAS on CSDM_CDM_ID = CDM_ID "+
                            //    " inner join TBL_CERT_SUB_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID where 1=1 ";

                            strSql = "select distinct CERT_CONTENT from TBL_CERTIFICATIONS " +
                                   " inner join TBL_CERT_MAS on CERT_CERTM_ID = CERTM_ID " +
                                   " inner join  TBL_CERT_DEPT_MAS on CERTM_DEPT_ID = CDM_ID " +
                                   " inner join TBL_CERT_SUB_DEPT_MAS on CSDM_CDM_ID = CDM_ID " +
                                   " inner join TBL_CERT_SUB_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID where 1=1 ";
                            if (!str.Equals(""))
                            {
                                strSql = strSql + " and CDM_ID = " + str;
                            }
                            break;
                        }
                    //Added by Supriya on 30-Sep-2015
                    case "DeleteSubCertDept":
                        {
                            strSql = "delete from TBL_CERT_SUB_SUB_DEPT_MAS where CSSDM_CSDM_ID =" + str;
                            strSql = strSql + " delete from TBL_CERT_SUB_DEPT_MAS where CSDM_ID =" + str;
                            break;
                        }

                    //Modify by Supriya on 20Jun2017
                    //>>

                    case "GetCertChecklistDetailFileById":
                        {
                            strSql = "select * from TBL_CCM_FILES " +
                                " inner join TBL_REF_CODES on RC_CODE = CCMF_FILE_TYPE and RC_TYPE = 'Certification File Type' " +
                                " where CCMF_CCM_ID in (" + str + ")";
                            break;
                        }

                    case "CERTACTIVEQUARTERS":
                        {
                            strSql = "SELECT CQM_ID,CQM_FROM_DATE,CQM_TO_DATE,replace(convert(varchar,CQM_DUE_DATE,106),' ','-') AS DueDate, " +
                            " (replace(convert(varchar,CQM_FROM_DATE,106),' ','-') + ' to '+ replace(convert(varchar,CQM_TO_DATE,106),' ','-')) AS Quarter " +
                            " FROM  TBL_CERT_QUARTER_MAS where CQM_STATUS='A' ";
                            if (!str.Equals(""))
                            {
                                strSql = strSql + " and CQM_ID != " + str;
                            }
                            strSql = strSql + " order by CQM_FROM_DATE";
                            break;
                        }
                    //<< Added By Hari on 20 Dec 2016
                    case "getADVTStatusByRefCode":
                        {
                            strSql = "select * from TBL_REF_CODES where 1=1 " + str;
                            break;
                        }
                    //<<Added By Rahuldeb on 01Jun2017
                    case "CircularPersonResponsibleWithFunction":
                        {
                            strSql = "select (CPRM_NAME + '(' + CFM_NAME + ')') as  Text,*  from TBL_CIRCULAR_PERSON_RESPONSIBLE_MAS " +
                                        "inner join TBL_CIRCULAR_FUNCTION_MAS on CPRM_CFM_ID = CFM_ID " +
                                        "where 1=1 " + str + " order by CPRM_NAME ";
                            break;
                        }
                    //>>
                    //<< Added by Ashish on 04May2017
                    case "ContractDocumentsBYDCId":
                        {
                            strSql = "select * from TBL_DRAFT_CONTRACTS where DC_ID in (" + str + ")";
                            break;
                        }
                    case "getContractDraftFilesByDCId":
                        {
                            strSql = "select * from TBL_DRAFT_CONTRACT_FILES where DCF_DC_ID in (" + str + ")";
                            break;
                        }

                    // Added by Ashish on 13May2017
                    case "deleteContractAttachments":
                        {
                            strSql = "DELETE FROM TBL_DRAFT_CONTRACT_FILES WHERE DCF_ID= " + str + "";
                            break;
                        }
                    //>>
                    case "getContractUpdateType":
                        {
                            strSql = "select * from TBL_REF_CODES where 1=1 and RC_TYPE='" + str + "'";
                            break;
                        }
                    case "getParamSubtype":
                        {
                            strSql = "select * from TBL_REF_CODES where 1=1 and RC_TYPE='" + str + "'";
                            break;
                        }

                    //Added By Milan Yadav on 24-Jun-2016
                    //>>
                    case "getContractDepartmentOwnerDetails":
                        {
                            strSql = "select * from TBL_CON_DEPT_OWNERS inner join TBL_CON_DEPT_MAS on CDM_ID=CDO_CDM_ID where 1=1 and CDO_ID='" + str + "'";
                            break;
                        }

                    case "getContractDirectorDetails":
                        {
                            strSql = "select * from TBL_CON_KMP_DIRECTORS where 1=1 and CKD_ID='" + str + "'";
                            break;
                        }

                    //Added By Milan yadav on 16-Aug-2016
                    //>>
                    case "ContractDraftResponseFiles":
                        {
                            strSql = "SELECT * FROM TBL_CONTRACT_DRAFT_RESPONSE_FILES  where 1=1 and CDRF_CDR_ID = " + str + "";
                            break;
                        }
                    //<<

                    //Added By Milan Yadav on 17-Aug-2016
                    //>>
                    case "getContractActivity":
                        {
                            strSql = "SELECT * FROM TBL_CONTRACT_ACTIVITY_MAS  where 1=1 and CAM_ID = " + str + "";
                            break;
                        }

                    case "getContractDepartmentDetails":
                        {
                            strSql = "SELECT * FROM TBL_CON_DEPT_MAS  where 1=1 and CDM_ID = " + str + "";
                            break;
                        }

                    case "getContractTypeDetails":
                        {
                            strSql = "SELECT * FROM TBL_CONTRACT_TYPE_MAS  where 1=1 and CTM_ID = " + str + "";
                            break;
                        }

                    case "getDepartmentRunningNo":
                        {
                            strSql = "select * from TBL_CON_DEPT_RUNNING_NO where 1=1 and CDRN_ID = " + str + "";
                            break;
                        }
                    //<<
                    //Added By Milan Yadav on 19-Sep-2016
                    //>>
                    case "getPanNumberDuplicationCheck":
                        {
                            strSql = "select distinct CVM_PAN_NUMBER from TBL_CON_VENDOR_MASTER where 1=1 and CVM_PAN_NUMBER = '" + str + "'";
                            break;
                        }
                    case "getKMPPanNumberDuplicationCheck":
                        {
                            strSql = "select distinct CKD_PAN_NUMBER from TBL_CON_KMP_DIRECTORS where 1=1 and CKD_PAN_NUMBER = '" + str + "'";
                            break;
                        }
                    case "getContractOtherDepartmentApprover":
                        {
                            strSql = "select * from TBL_CON_DEPT_MAS where CDM_IS_REF_DEPT ='N' and CDM_ID in (" + str + ")";
                            break;
                        }

                    case "getContractOtherDepartmentApprovers":
                        {
                            strSql = "select * from TBL_CON_DEPT_MAS where  CDM_ID in (" + str + ")";
                            break;
                        }
                    //<<
                    //Added By Milan Yadav on 11-Jan-2017
                    //<<
                    case "getContractOtherDepartmentReference":
                        {
                            //strSql = "select * from TBL_CON_DEPT_MAS where CDM_IS_REF_DEPT ='Y' AND CDM_ID in (" + str + ")";
                            strSql = " select * from TBL_CONTRACT_TEMPLATE inner join TBL_CT_APPROVING_DEPT on CT_ID = CAD_CT_ID " +
                                    " Inner Join TBL_CON_DEPT_MAS on CAD_DEPT_ID = CDM_ID and  CDM_IS_REF_DEPT ='Y' where CAD_ID ='" + str + "'";
                            break;
                        }
                    //>>

                    //Added By Milan Yadav on 17-Jan-2017
                    //>>
                    case "ContractTemplateId":
                        {
                            strSql = "select * from TBL_DRAFT_CONTRACTS where DC_ID =" + str + "";
                            break;
                        }
                    case "ContractChecklistApprovalEdit":
                        {
                            strSql = "select * from TBL_CON_APPROVAL_CHECKLIST_MAS " +
                                     "left outer join TBL_CON_APPROVAL_CHECKLIST_DETS on CACM_ID = CPCD_CACM_ID " +
                                     " where 1=1 " + str;
                            break;
                        }

                    //Added By Milan Yadav on 22-Feb-2017
                    //<<
                    case "getContractLevelApprovalOwnerDetails":
                        {
                            strSql = "select * from TBL_CON_APPROVAL_OWNERS where 1=1 and CAO_ID='" + str + "'";
                            break;
                        }
                    //>>
                    //<<Added By Milan Yadav on 22-Feb-2017
                    case "ContractExecutionResponseFiles":
                        {
                            strSql = "SELECT * FROM TBL_CON_EXECUTION_DETS_FILES WHERE CEDF_FILE_TYPE='Executed Contract Copy uploaded by Requester' and CEDF_CED_ID = " + str + "";
                            break;
                        }
                    //<<Added By Milan Yadav on 22-Feb-2017
                    case "getContractExecutionDetails":
                        {
                            strSql = "SELECT * FROM TBL_CON_EXECUTION_DETS inner join TBL_DRAFT_CONTRACTS on DC_ID = CED_DC_ID  WHERE CED_ID = " + str + "";
                            break;
                        }
                    //>>
                    //<< Added By Milan Yadav on 01-Mar-2017
                    case "getContractPreExecutedFilesCount":
                        {
                            strSql = "select * FROM TBL_DRAFT_CONTRACT_FILES where DCF_FILE_TYPE='Final Pre-executed contract copy' and DCF_DC_ID =" + str + "";
                            break;
                        }

                    case "ContractLegalUploadedFiles":
                        {
                            strSql = "SELECT * FROM TBL_CON_EXECUTION_DETS_FILES WHERE CEDF_FILE_TYPE='Uploaded Agreement: Uploaded by Legal' and CEDF_CED_ID = " + str + "";
                            break;
                        }
                    //>>
                    //Added By Milan Yadav on 14-Mar-2017
                    case "getMaildIdofApprovalDepartment":
                        {
                            strSql = " select  * FROM TBL_DRAFT_CONTRACTS    " +
                                    "inner join TBL_CONTRACT_TEMPLATE on DC_CT_ID = CT_ID    " +
                                    "INNER JOIN TBL_DC_APPROVING_DEPT on DAD_DC_ID = DC_ID   " +
                                    //"inner join TBL_CT_APPROVING_DEPT on CAD_ID = DAD_CAD_ID  " +
                                    "Inner Join TBL_CON_DEPT_MAS on DAD_DEPT_ID = CDM_ID     " +
                                    "Inner Join TBL_CON_DEPT_OWNERS on CDO_CDM_ID = CDM_ID and DAD_APPROVER_TYPE = CDO_OWNER_TYPE " +
                                    "where DAD_STATUS = 'SC' and DC_ID = " + str + "";
                            break;
                        }
                    //>>
                    //<< Added By Milan Yadav on 25-Feb-2017
                    case "getLevel1ApproverMailId":
                        {
                            strSql = "SELECT CAO_LEVEL2_APPROVAL_EMAIL,* FROM TBL_CON_APPROVAL_OWNERS " +
                                    " where 1=1 ";
                            if (!str.Equals(""))
                            {
                                strSql = strSql + " and CAO_USERNAME = '" + str + "'";
                            }
                            break;
                        }
                    case "ContractChecklistApproval":
                        {
                            strSql = "select * from TBL_CON_APPROVAL_CHECKLIST_MAS where 1=1 " + str;
                            break;
                        }
                    //>>
                    //<< Added by Ashish on 23-Jun-2017
                    case "getContractChecklistFileType":
                        {
                            strSql = "Select * FROM TBL_REF_CODES WHERE RC_TYPE = '" + str + "'";
                            break;
                        }
                    case "getConfigParamsbyId":
                        {
                            strSql = " SELECT * from TBL_CONFIG_PARAMS where CP_ID = " + str;
                            break;
                        }
                    //>>
                    //<< Added by Ashish on 27-Jun-2017
                    case "getContractOtherDepartmentName":
                        {
                            strSql = "select * from TBL_CON_DEPT_MAS where CDM_IS_REF_DEPT ='Y' AND CDM_ID in (" + str + ")";
                            break;
                        }
                    //>>

                    //<< Added By Vivek on 28-Dec-2017

                    case "getSpocFromComplianceFunction":
                        {
                            strSql = "select * from TBL_CIRC_COMPLIANCE_SPOCS where 1=1 " + str + " order by CCS_NAME";
                            break;
                        }
                    case "SUBTYPE":
                        {
                            strSql = "Select * from TBL_SUB_TYPE_MAS " +
                            " inner join TBL_EM_STM_MAPPING on ESM_STM_ID = STM_ID" +
                            " inner join EmployeeMaster on EM_ID = ESM_EM_ID " +
                            " and EM_USERNAME = '" + str.Replace("'", "'") + "' " +
                            " ORDER BY STM_TYPE ";
                            break;
                        }
                    case "ContractAttachments":
                        {
                            strSql = "select * from TBL_CONTRACT_FILES where CONF_ID in (" + str + ")";
                            break;
                        }

                    //>>
                    case "getCertDeptByDepId":
                        strSql = "select * from TBL_CERT_DEPT_MAS " +
                                    "Inner join TBL_CERT_MAS on CDM_ID = CERTM_DEPT_ID and CERTM_LEVEL_ID = 3 " +
                                    "and CERTM_ID not in " +
                                    "( " +
                                    "select CERT_CERTM_ID from TBL_CERTIFICATIONS  inner join TBL_CERT_QUARTER_MAS on CQM_ID = CERT_CQM_ID" +
                                    " and CQM_STATUS = 'A') " +
                                    " and CERTM_ID not in " +
                                    "( " +
                                    "select CC_CERTM_ID from TBL_COMMON_CERTIFICATIONS  inner join TBL_CERT_QUARTER_MAS on CQM_ID = CC_CQM_ID " +
                                    " and CQM_STATUS = 'A') " +
                                    " where 1=1 ";

                        if (!str.Equals(""))
                        {
                            strSql = strSql + " and CDM_ID = " + str;
                        }

                        break;
                    case "getCertException":
                        {
                            strSql = "select * FROM TBL_CERT_EXCEPTION WHERE CE_ID IN  (" + str + ")";
                            break;
                        }
                    case "getExceptionByCertId":
                        {
                            strSql = "select * FROM TBL_CERT_EXCEPTION WHERE CE_CERT_ID IN (" + str + ")";
                            break;
                        }
                    case "getExceptionByCertIdWithDeptName":
                        {
                            strSql = "SELECT " +
                                " CDM_NAME + ' - ' + CSDM_NAME as [FHDept], " +
                                " CSDM_NAME as [UHDept], " +
                                " TBL_CERT_EXCEPTION.* " +
                                " FROM TBL_CERT_EXCEPTION " +
                                " INNER JOIN TBL_CERTIFICATIONS ON CERT_ID = CE_CERT_ID AND CERT_LEVEL = 0 " +
                                " INNER JOIN TBL_CERT_MAS ON CERTM_ID = CERT_CERTM_ID AND CERTM_LEVEL_ID = 0 " +
                                " INNER JOIN TBL_CERT_SUB_SUB_DEPT_MAS ON CSSDM_ID = CERTM_DEPT_ID " +
                                " INNER JOIN TBL_CERT_SUB_DEPT_MAS ON CSDM_ID = CSSDM_CSDM_ID " +
                                " INNER JOIN TBL_CERT_DEPT_MAS ON CDM_ID = CSDM_CDM_ID WHERE CE_CERT_ID IN (" + str + ")";
                            break;
                        }
                    case "LegalHelpDesk":
                        {
                            strSql = "SELECT  distinct HD_HCM_ID, HCM_NAME, " +
                                   " HD_ID, HD_REQUESTOR_NAME, HD_REQUESTOR_EMAIL, HD_STATUS, HD_AUDIT_TRAIL,HD_HDM_ID, " +
                                   " case when HD_STATUS='S' then 'Submitted' end as Status, " +
                                   " HD_CREATE_BY, HD_CREATE_DT  FROM TBL_HELP_DESK " +
                                   " INNER JOIN TBL_HELPDESK_CATEGORY_MAS ON HD_HCM_ID = HCM_ID " +
                                   " INNER JOIN TBL_HELPDESK_SUBCATEGORY_MAS on HSM_HCM_ID = HCM_ID " +
                                   " Inner JOIN TBL_HSM_HO_MAPPING on HHM_HSM_ID = HSM_ID " +
                                   " inner Join TBL_HELPDESK_OWNERS on HO_ID = HHM_HO_ID " +
                                   " where HD_STATUS='S' " + str;
                            break;
                        }
                    case "LegalHelpDeskForReallocation":
                        {
                            strSql = " SELECT distinct HD_HCM_ID, HCM_NAME, " +
                                     " HD_ID, HD_REQUESTOR_NAME, HD_REQUESTOR_EMAIL, HD_STATUS, HD_AUDIT_TRAIL,HD_HDM_ID, " +
                                     " case when HD_STATUS='A' then 'Allocated' end as Status, " +
                                     " HD_CREATE_BY, HD_CREATE_DT  FROM TBL_HELP_DESK " +
                                     " INNER JOIN TBL_HELPDESK_CATEGORY_MAS ON HD_HCM_ID = HCM_ID " +
                                     " INNER JOIN TBL_HELPDESK_SUBCATEGORY_MAS on HSM_HCM_ID = HCM_ID " +
                                     " Inner JOIN TBL_HSM_HO_MAPPING on HHM_HSM_ID = HSM_ID " +
                                     " inner Join TBL_HELPDESK_OWNERS on HO_ID = HHM_HO_ID " +
                                     " where HD_STATUS='A' " + str;
                            break;
                        }
                    case "SEARCHCERTFORACTIVEQUATER":
                        {
                            strSql = "SELECT distinct CDM_NAME, CERT_ID,cqm_id, CDM_ID,CERTM_DEPT_ID,CERTM_ID,CERTM_TEXT,CERTM_CREATE_BY, " +
                                    " CERTM_CREATE_DT,CERTM_LST_UPD_BY,CERT_REMARKS, " +
                                    " CSM_DESC [Status], " +
                                    " CERTM_LST_UPD_DT " +
                                    " FROM  TBL_CERTIFICATIONS " +
                                    " INNER JOIN TBL_CERT_QUARTER_MAS ON CERT_CQM_ID=CQM_ID " +
                                    " inner join TBL_CERT_STATUS_MAS on CERT_STATUS = CSM_NAME " +
                                    " INNER JOIN TBL_CERT_MAS on CERTM_ID=CERT_CERTM_ID " +
                                    " INNER JOIN TBL_CERT_DEPT_MAS on CERTM_DEPT_ID = CDM_ID AND CERTM_LEVEL_ID = 2 " +
                                    " WHERE 1=1 AND CQM_ID = " + str;
                            break;
                        }
                    //>> End 
                    //<<Added by Ankur Tyagi on 05-May-2025 for Project Id : 2395
                    case "SUBMISSIONSFILESFromType":
                        {
                            strSql = "SELECT TBL_SUBMISSION_FILES.* FROM TBL_SUBMISSION_FILES WHERE 1 = 1 " + str;
                            break;
                        }
                        //>>
                }

                using (F2FDatabase DB = new F2FDatabase(strSql))
                {
                    DB.F2FDataAdapter.Fill(resultDataSet);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return resultDataSet;
        }

        public DataSet getDataSetFromSP(string strSP, Dictionary<string, string> dictParams = null)
        {
            DataSet ds = new DataSet();
            using (F2FDatabase DB = new F2FDatabase(strSP))
            {
                DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                if (dictParams != null)
                {
                    foreach (var param in dictParams)
                    {
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@" + param.Key, F2FDbType.VarChar, param.Value));
                    }
                }

                DB.F2FDataAdapter.Fill(ds);
            }

            return ds;
        }

        public DataTable getDatasetWithCondition(string code, int id, string conStr)
        {
            DataTable resultDataSet = new DataTable();
            try
            {
                string strSql = "";
                switch (code)
                {

                    case "SUBAREA":
                        {
                            strSql = "select   CSM_ID   , CSM_NAME    from  TBL_CIRCULAR_SUBAREA_MAS where CSM_CAM_ID=" + id + " order by  CSM_NAME  ";
                            break;
                        }
                    case "CIRCULARSEGMENT":
                        {
                            strSql = "SELECT CSM_CM_ID,CSGM_NAME FROM TBL_CIRCULAR_SEGMENT_MAS INNER JOIN TBL_CIRCULAR_SEGMENT_MAPPING ON CSGM_ID = CSM_CSGM_ID WHERE CSM_CM_ID = " + id + "";
                            break;
                        }
                    case "BINDSEGMENT":
                        {
                            strSql = "SELECT CSM_CSGM_ID FROM TBL_CIRCULAR_SEGMENT_MAPPING WHERE CSM_CM_ID = " + id + "";
                            break;
                        }
                    case "CIRCULARINTIMATION":
                        {
                            strSql = "SELECT CMI_CIM_ID FROM TBL_CM_INTIMATIONS WHERE CMI_CM_ID = " + id + "";
                            break;
                        }

                    case "CIRCINTIMATIONOWNERS":
                        {
                            strSql = "SELECT CIU_EMAIL_ID FROM TBL_CIRC_INTIMATION_USERS WHERE CIU_CIM_ID = " + id + "";
                            break;
                        }
                    case "CIRCULARFILES":
                        {
                            strSql = "SELECT * FROM TBL_CIRCULAR_FILES WHERE CF_CM_ID = " + id + "";
                            break;
                        }

                    //<<Modified By Vivek on 23-Jun-2017
                    case "OWNERS":
                        {
                            strSql = "SELECT EM_EMPNAME, EM_ID, EM_EMAIL FROM EmployeeMaster " +
                                    " INNER JOIN TBL_EM_STM_MAPPING ON EM_ID = ESM_EM_ID " +
                                    " WHERE ESM_STM_ID = " + id + " order by EM_EMPNAME ";
                            break;
                        }

                    case "OWNERS_MAIL":
                        {
                            strSql = "SELECT EM_EMPNAME, EM_ID, EM_EMAIL FROM EmployeeMaster " +
                                    " INNER JOIN TBL_EM_STM_MAPPING ON EM_ID = ESM_EM_ID " +
                                    " WHERE ESM_STM_ID = " + id + " AND EM_STATUS='A' order by EM_EMPNAME ";
                            break;
                        }
                    //>>
                    case "EVENTPURPOSE":
                        {
                            strSql = "SELECT EP_NAME, EP_ID FROM  TBL_EVENT_PURPOSE where EP_EM_ID=" + id + "";
                            break;
                        }

                    case "EVENTPURPOSE_ACTIVE":
                        {
                            strSql = "SELECT EP_NAME, EP_ID FROM  TBL_EVENT_PURPOSE where EP_STATUS = 'A' AND EP_EM_ID=" + id + "";
                            break;
                        }

                    case "SUBMISSIONSFILES":
                        {
                            strSql = "SELECT TBL_SUBMISSION_FILES.* FROM TBL_SUBMISSION_FILES WHERE SF_SC_ID = " + id + "";
                            break;
                        }
                    case "DELETESUBFILES":
                        {
                            strSql = "DELETE FROM TBL_SUBMISSION_FILES WHERE SF_ID= " + id + "";
                            break;
                        }
                    case "GETESCALATIONS":
                        {
                            strSql = "SELECT SE_ID, SE_EMAIL_ID, SE_STM_ID FROM  TBL_SUB_ESCALATION WHERE SE_STM_ID =" + id + "";
                            break;
                        }
                    //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
                    case "SUBMISSIONSFILES_With_RefCode":
                        {
                            strSql = "SELECT TBL_SUBMISSION_FILES.*,RC_NAME FROM TBL_SUBMISSION_FILES " +
                                " INNER JOIN TBL_REF_CODES ON RC_CODE = SF_FILE_TYPE AND RC_TYPE = 'Submisssion Operation Type'" +
                                " WHERE SF_SC_ID = " + id + "";
                            break;
                        }
                    //>>



                    #region Commented Code
                    /*
                     * 
                     *  case "RESPONSESFILES":
                    {
                        strSql = "SELECT * FROM TBL_HELPDESK_RESPONSE_FILES WHERE HRF_HR_ID = " + id + "";
                        break;
                    }
                      //<< Added by Supriya on 14-Mar-13
                    case "LitPolicyDets":
                    {
                        strSql = "select *, a.RC_NAME as [Channel], b.RC_NAME as [FFAStatus],c.RC_NAME as [Policy Type] from TBL_COMP_LTGN_POLICYDETS " +
                                    "  left outer JOIN TBL_REF_CODES a ON a.RC_CODE = CLP_CHANNEL AND  a.RC_TYPE = 'Policy Channel' " +
                                    " left outer JOIN TBL_REF_CODES b ON b.RC_CODE = CLP_FFA_STATUS AND  b.RC_TYPE = 'Policy Status'  " +
                                    " left outer JOIN TBL_REF_CODES c ON c.RC_CODE = CLP_POLICY_TYPE AND  c.RC_TYPE = 'Policy Type' " +
                                    " where CLP_LGM_ID=" + id;
                        break;
                    }
                    case "PreviousPolicyDetails":
                    {
                        strSql = "SELECT [CLP_POLICY_NO] as PolicyNo  ,[CLP_POLICYHOLDER] as PolicyHolder ,[CLP_POLICY_DATE] as PolicyDate " +
                                " ,[CLP_SUM_ASSURED] as SumAssured,[CLP_PREMIUM_AMOUNT] as PremiumAmt ,[CLP_PRODUCT_DETAILS] as ProductDetails FROM [TBL_COMP_LTGN_POLICYDETS] where CLP_LGM_ID=" + id;
                        break;
                    }
                    case "LNPolicyDets":
                    {
                        strSql = "select * from TBL_COMP_LN_POLICYDETS where CLNP_LM_ID=" + id;
                        break;
                    }
                    case "LNComplainantDets":
                    {
                        strSql = "select * from TBL_COMP_CLM_COMPLAINAINT_DETS where CCD_CLM_ID=" + id;
                        break;
                    }

                    case "SUBCATEGORY":
                    {
                        strSql = "select   HSM_ID   , HSM_NAME    from  TBL_HELPDESK_SUBCATEGORY_MAS where HSM_STATUS='A' AND HSM_HCM_ID=" + id + " order by  HSM_NAME  ";
                        break;
                    }

                    case "SUBCATEGORYOWNERS":
                    {
                        strSql = "SELECT  HO_ID, HO_EMAIL,HO_USERNAME FROM  TBL_HELPDESK_OWNERS inner join TBL_HSM_HO_MAPPING on HHM_HO_ID=HO_ID  where HHM_LEVEL=0 and HO_IS_AVAILABLE='Y' and HHM_HSM_ID=" + id + " and HHM_PRIORITY =( " +
                                 " SELECT  min(HHM_PRIORITY) as Priprity FROM  TBL_HELPDESK_OWNERS inner join TBL_HSM_HO_MAPPING on HHM_HO_ID=HO_ID  where HHM_LEVEL=0 and HO_IS_AVAILABLE='Y'  and HHM_HSM_ID=" + id + " )";

                        //strSql = "SELECT  HO_ID, HO_EMAIL,HO_USERNAME FROM  TBL_HELPDESK_OWNERS inner join TBL_HSM_HO_MAPPING on HHM_HO_ID=HO_ID  where HHM_LEVEL=0  and HHM_HSM_ID= " + id;
                        break;
                    }

    case "DELETELGLOWNERS":
                    {
                        strSql = "DELETE FROM TBL_LGL_LTGN_OWNER_MAPPING WHERE LLOM_LLM_ID=" + id;
                        break;
                    }
                    case "DELETELGLNOTICEOWNERS":
                    {
                        strSql = "DELETE FROM TBL_LGL_NOTICE_OWNER_MAPPING WHERE LNOM_LNM_ID=" + id;
                        break;
                    }
                    case "LegalFormat":
                    {
                        strSql = "select * from TBL_LGL_NOTICE_MASTER where LNM_ID=" + id;
                        break;
                    }

    case "SECDOCTYPE":
                    {
                        strSql = "SELECT SD_ID , SDT_NAME,SD_NAME  ,SD_DESC ,SD_FILING_DATE ,SD_FILE_NAME ,SD_SERVER_FILE_NAME ,SD_CREATE_BY  " +
                                 " FROM TBL_SEC_DOCS ,TBL_SEC_DOCS_TYPE WHERE SD_SDT_ID=SDT_ID AND SDT_ID=" + id;
                        break;
                    }
                    case "AllSecDocType":
                    {
                        strSql = "SELECT SD_ID ,SD_TYPE, SD_STATUS, SDT_NAME,SD_NAME  ,SD_DESC ,SD_FILING_DATE ,SD_FILE_NAME ,SD_SERVER_FILE_NAME ,SD_CREATE_BY  " +
                                 " FROM TBL_SEC_DOCS ,TBL_SEC_DOCS_TYPE WHERE SD_SDT_ID=SDT_ID and SD_STATUS='Active' AND SDT_ID=" + id;
                        break;
                    }
                    case "PublicSecDocType":
                    {
                        strSql = "SELECT SD_ID ,SD_TYPE, SD_STATUS, SDT_NAME,SD_NAME  ,SD_DESC ,SD_FILING_DATE ,SD_FILE_NAME ,SD_SERVER_FILE_NAME ,SD_CREATE_BY  " +
                                 " FROM TBL_SEC_DOCS ,TBL_SEC_DOCS_TYPE WHERE SD_SDT_ID=SDT_ID and SD_STATUS='Active' " +
                        " and SD_TYPE ='Public' AND SDT_ID=" + id;
                        break;
                    }
                    case "DELETECONTDETAILS":
                    {
                        strSql = "DELETE FROM TBL_CONTRACT_TEMPLATE WHERE CT_ID=" + id;
                        break;
                    }
                    case "DELETESECDETAILS":
                    {
                        strSql = "DELETE FROM TBL_SEC_DOCS WHERE SD_ID=" + id;
                        break;
                    }

                    case "DELETESTANDARDDETAILS":
                    {
                        strSql = "DELETE FROM TBL_SEC_STD_TEMPLATE WHERE SST_ID=" + id;
                        break;
                    }

                    case "SECSUBCATEGORY":
                    {
                        strSql = "select * from TBL_SEC_SUBCATEGORY_MAS where SSM_SCM_ID=" + id + " order by  SSM_SORT_ORDER  ";
                        break;
                    }
                    case "deleteSecMeeting":
                    {
                        strSql = "DELETE FROM TBL_SEC_MEETING WHERE SM_ID=" + id + "; DELETE FROM TBL_SEC_MEETING_MEMBERS " +
                       "WHERE SMM_SM_ID=" + id;
                        break;
                    }
                    case "deleteSecMeetingDoc":
                    {
                        strSql = "DELETE FROM TBL_SEC_MEETING_DOCS WHERE SMD_ID= " + id + "";
                        break;
                    }
                    case "deleteSecMeetingDocFiles":
                    {
                        strSql = "DELETE FROM TBL_SEC_MEETING_DOC_FILES WHERE SMDF_SMD_ID = " + id + "";
                        break;
                    }

                    case "SECMEETINGDETAILS":
                    {
                        strSql = "SELECT    SMD_ID,SMD_SERVER_FILE,SDT_NAME, SMT_NAME,  SM_DATE,  SMD_DETAILS,  SMD_FILE_NAME, SMD_CREATE_BY, SMD_CREATE_DATE FROM  TBL_SEC_MEETING " +
                                 " left outer JOIN  TBL_SEC_MEETING_DOCS ON TBL_SEC_MEETING.SM_ID = TBL_SEC_MEETING_DOCS.SMD_SM_ID left outer JOIN  TBL_SEC_MEETING_TYPE ON TBL_SEC_MEETING_DOCS.SMD_SMT_ID = TBL_SEC_MEETING_TYPE.SMT_ID INNER JOIN " +
                                 " TBL_SEC_MEETING_DOC_TYPE ON TBL_SEC_MEETING_DOCS.SMD_SDT_ID = TBL_SEC_MEETING_DOC_TYPE.SDT_ID where SMD_SM_ID=" + id;
                        break;
                    }
                    case "DELETESECACTIONABLE":
                    {
                        strSql = "delete from TBL_SEC_ACTIONABLE where SA_ID=" + id + "; DELETE FROM TBL_SEC_ACTIONABLE_OWNER WHERE SAO_SA_ID=" + id;
                        break;
                    }

                    case "DeleteLGLAdvocate":
                    {
                        strSql = "DELETE FROM TBL_LGL_LTGN_ADV_MAPPING WHERE LAM_LLM_ID= " + id + "";
                        break;
                    }
                    case "AdvacateByLTGNId":
                    {
                        strSql = "SELECT LAM_ADV_ID,LAM_ADV_NAME FROM TBL_LGL_LTGN_ADV_MAPPING inner join TBL_LGL_ADVOCATE_MASTER on TBL_LGL_LTGN_ADV_MAPPING.LAM_ADV_ID=TBL_LGL_ADVOCATE_MASTER.LAM_ID WHERE LAM_LLM_ID=" + id;
                        break;
                    }


                    case "deleteLglLitAdvFees":
                    {
                        strSql = "DELETE FROM TBL_LGL_LTGN_ADV_FEES WHERE LLAF_ID= " + id + "";
                        break;
                    }

                    case "UpdatesAndTAT":
                    {
                        strSql = "SELECT  convert(varchar,LNU_STATUS_DATE,106) as [Status Date],LNUT_NAME as [Update Type], LNU_DETAILS as Details,  LNU_CREATE_BY as [Create by], " +
                                " dbo.[getWorkingDaysDiff](LNM_TAT_START_DATE,LNU_STATUS_DATE)  as [TAT]  FROM     TBL_LGL_NOTICE_UPDATES INNER JOIN TBL_LGL_NOTICE_MASTER ON TBL_LGL_NOTICE_UPDATES.LNU_LNM_ID = TBL_LGL_NOTICE_MASTER.LNM_ID inner JOIN " +
                                " TBL_LGL_NOTICE_UPDATE_TYPES on  LNUT_ID=LNU_UPDATE_TYPE where LNU_LNM_ID=" + id;
                        break;
                    }


                    case "DeleteLITIGATIONFILES":
                    {
                        strSql = "delete FROM TBL_COMP_LITIGATION_FILES WHERE CLF_ID = " + id + "";
                        break;
                    }
                    case "DeleteLegalLITIGATIONFILES":
                    {
                        strSql = "delete FROM TBL_LGL_LTGN_FILES WHERE LLF_ID = " + id + "";
                        break;
                    }
                    case "DeleteLEGALNOTICEFILE":
                    {
                        strSql = "delete FROM TBL_COMP_LEGAL_NOTICE_FILES WHERE LNF_ID = " + id + "";
                        break;
                    }

                    case "deleteLGLLEGALFILES":
                    {
                        strSql = "delete FROM TBL_LGL_NOTICE_FILES WHERE LNF_ID = " + id + "";
                        break;
                    }
                    case "LitAdvFeesDets":
                    {
                        strSql = "Select * from TBL_COMP_ADV_FEES where CLAF_CLGM_ID=" + id;
                        break;
                    }
                    case "HelpDeskCloseMail":
                    {
                        strSql = "SELECT  HHM_HSM_ID,HO_ID, HO_EMAIL,HO_USERNAME FROM  TBL_HELPDESK_OWNERS inner join TBL_HSM_HO_MAPPING on HHM_HO_ID=HO_ID  " +
                                " where HHM_PRIORITY=0 and HHM_LEVEL=0  and HO_IS_AVAILABLE='Y' and HHM_HSM_ID= " + id + "" +
                                " union SELECT  HHM_HSM_ID,HO_ID, HO_EMAIL,HO_USERNAME FROM  TBL_HELPDESK_OWNERS inner join TBL_HSM_HO_MAPPING on HHM_HO_ID=HO_ID " +
                                " where HHM_LEVEL=1 and HO_IS_AVAILABLE='Y' and HHM_HSM_ID=" + id;
                        break;
                    }

                    case "getKeyLearnings":
                    {
                        strSql = "SELECT CLGM_KEY_LEARNINGS from TBL_COMP_LITIGATION_MASTER where CLGM_ID = " + id;
                        break;

                    }
                    case "CompLegalNoticeUpdateByID":
                    {
                        strSql = "select * from TBL_COMP_LEGAL_NOTICE_UPDATES where LNU_ID = " + id;
                        break;
                    }
                    case "DeleteCompLegalNoticeUpdate":
                    {
                        strSql = "delete from TBL_COMP_LEGAL_NOTICE_UPDATES where LNU_ID = " + id + "; DELETE FROM TBL_COMP_LEGALNOTICE_UPDATE_FILES WHERE LUF_LNU_ID=" + id;
                        break;
                    }

                    case "CompLegalNoticeUpdateFileByUpdateId":
                    {
                        strSql = "SELECT LUF_ID, LUF_LNU_ID, LUF_FILE_NAME as FileName, LUF_FILE_NAME_ON_SERVER as FileNameOnServer, LUF_CREATE_BY as UserName, LUF_CREATE_DATE as UploadDatetime FROM TBL_COMP_LEGALNOTICE_UPDATE_FILES WHERE LUF_LNU_ID = " + id;
                        break;
                    }
                    case "DeleteCompLitUpdateFiles":
                    {
                        strSql = "delete from TBL_COMP_LIT_UPDATE_FILES where CLUF_ID = " + id;
                        break;
                    }
                    case "DeleteCompLegalNoticeUpdateFiles":
                    {
                        strSql = "delete from TBL_COMP_LEGALNOTICE_UPDATE_FILES where LUF_ID = " + id;
                        break;
                    }
                    case "DeleteLegalLitUpdateFiles":
                    {
                        strSql = "delete from TBL_LGL_LTGN_UPDATES_FILES where LLUF_ID = " + id;
                        break;
                    }
                    case "LGLLegalNoticeUpdateByID":
                    {
                        strSql = "select * from TBL_LGL_NOTICE_UPDATES where LNU_ID = " + id;
                        break;
                    }
                    case "DeleteLGLLegalNoticeUpdate":
                    {
                        strSql = "delete from TBL_LGL_NOTICE_UPDATES where LNU_ID = " + id + "; DELETE FROM TBL_LGL_NOTICE_UPDATE_FILES WHERE LNUF_LNU_ID=" + id;
                        break;
                    }
                    case "DeleteLGLLegalNoticeUpdateFiles":
                    {
                        strSql = "DELETE FROM TBL_LGL_NOTICE_UPDATE_FILES WHERE LNUF_ID= " + id;
                        break;
                    }

                    case "getHelpDeskById":
                    {
                        strSql = "SELECT HD_HCM_ID, HCM_NAME," +
                    "HD_ID, HD_REQUESTOR_NAME, HD_REQUESTOR_EMAIL, HD_QUESTION, HD_STATUS, HD_AUDIT_TRAIL, " +
                    "case when HD_STATUS='S' then 'Submitted' end as Status, " +
                    " HD_CREATE_BY, HD_CREATE_DT  FROM TBL_HELP_DESK INNER JOIN " +
                    "TBL_HELPDESK_CATEGORY_MAS ON HD_HCM_ID = HCM_ID " +
                    " where HD_ID = " + id;
                        break;
                    }
                    case "HelpDeskLegalOwner":
                    {
                        strSql = "SELECT * FROM TBL_HD_ALLOCATIONS where HA_HD_ID = " + id;
                        break;
                    }
                    case "deleteLegalAllocation":
                    {
                        strSql = "delete from TBL_HD_ALLOCATIONS where HA_HD_ID = " + id;
                        break;
                    }
                    case "HelpDeskCatByDept":
                    {
                        strSql = "SELECT * FROM TBL_HELPDESK_CATEGORY_MAS where HCM_HDM_ID = " + id + " ORDER BY HCM_NAME";
                        break;
                    }
                    case "HelpDeskOwnerMailById":
                    {
                        strSql = "SELECT HO_EMAIL FROM TBL_HELPDESK_OWNERS where HO_ID = " + id;
                        break;
                    }
                    case "LEGALOWNER":
                    {
                        strSql = "SELECT CO_ID,CO_EMPNAME FROM TBL_COMP_OWNERS, TBL_COMP_LEG_OWNER_MAPPING WHERE CO_ID = LOM_EM_ID AND LOM_LM_ID = " + id + "";
                        break;
                    }

                    case "LITIGATIONOWNER":
                    {
                        strSql = "SELECT CO_ID,CO_EMPNAME FROM TBL_COMP_OWNERS, TBL_COMP_LIT_OWNER_MAPPING WHERE CO_ID = CLOM_CO_ID AND CLOM_CLGM_ID = " + id + "";
                        break;
                    }

                    case "LEGALOWNERS":
                    {
                        strSql = "SELECT LO_ID,LO_EMPNAME,LO_USERNAME FROM TBL_LGL_OWNERS, TBL_LGL_LTGN_OWNER_MAPPING WHERE LO_ID = LLOM_LO_ID AND LLOM_LLM_ID = " + id + "";
                        break;
                    }
                    case "LGLLEGALOWNERS":
                    {
                        strSql = "SELECT LO_ID,LO_EMPNAME,LO_USERNAME FROM TBL_LGL_OWNERS, TBL_LGL_NOTICE_OWNER_MAPPING WHERE LO_ID = LNOM_LO_ID AND LNOM_LNM_ID = " + id + "";
                        break;
                    }


                    case "LEGALFILES":
                    {
                        strSql = "SELECT * FROM TBL_COMP_LEGAL_NOTICE_FILES WHERE LNF_LM_ID = " + id + "";
                        break;
                    }

                    case "LITIGATIONFILES":
                    {
                        strSql = "SELECT * FROM TBL_COMP_LITIGATION_FILES WHERE CLF_CLGM_ID = " + id + "";
                        break;
                    }
                    case "LEGALLITIGATIONFILES":
                    {
                        strSql = "SELECT * FROM TBL_LGL_LTGN_FILES WHERE LLF_LLM_ID = " + id + "";
                        break;
                    }
                    case "LGLLEGALFILES":
                    {
                        strSql = "SELECT * FROM TBL_LGL_NOTICE_FILES WHERE LNF_LNM_ID = " + id + "";
                        break;
                    }

                    case "Owners1":
                    {
                        strSql = "SELECT * from EmployeeMaster, TBL_LEG_OWNER_MAPPING WHERE EM_ID = LOM_EM_ID AND LOM_LM_ID= " + id + " order by EM_EMPNAME ";
                        break;
                    }
                    case "Owners2":
                    {
                        strSql = "SELECT * from EmployeeMaster, TBL_LIT_OWNER_MAPPING WHERE EM_ID = LGOM_EM_ID AND LGOM_LGM_ID= " + id + " order by EM_EMPNAME ";
                        break;
                    }
                    case "LEGALUPDATEFILES":
                    {
                        strSql = "SELECT * FROM TBL_COMP_LEGALNOTICE_UPDATE_FILES WHERE LUF_LNU_ID = " + id + "";
                        break;
                    }

                    case "LGLLEGALUPDATEFILES":
                    {
                        strSql = "SELECT * FROM TBL_LGL_NOTICE_UPDATE_FILES WHERE LNUF_LNU_ID = " + id + "";
                        break;
                    }
                    case "LITIGATIONUPDATEFILES":
                    {
                        strSql = "SELECT * FROM TBL_COMP_LIT_UPDATE_FILES WHERE CLUF_CLU_ID = " + id + "";
                        break;
                    }

                    case "LEGALLITIGATIONUPDATEFILES":
                    {
                        strSql = "SELECT * FROM TBL_LGL_LTGN_UPDATES_FILES WHERE LLUF_LLU_ID = " + id + "";
                        break;
                    }
                    case "LEGALUPDATES":
                    {
                        strSql = "SELECT * FROM TBL_COMP_LEGAL_NOTICE_UPDATES left outer join TBL_COMP_NOTICE_UPDATE_TYPES on NUT_ID=LNU_UPDATE_TYPE WHERE LNU_LM_ID = " + id + " order by LNU_STATUS_DATE desc";
                        break;
                    }

                    case "LGLLEGALUPDATES":
                    {
                        strSql = "SELECT *,LNUT_NAME FROM TBL_LGL_NOTICE_UPDATES inner join TBL_LGL_NOTICE_UPDATE_TYPES on LNU_UPDATE_TYPE=LNUT_ID WHERE LNU_LNM_ID = " + id + " order by LNU_CREATE_DATE desc";
                        break;
                    }
                    case "LITIGATIONUPDATES":
                    {
                        strSql = "SELECT  CLU_ID, CLU_CLGM_ID, CLU_STATUS_DATE,  CLUT_NAME,CLU_PREV_HEARING_STAGE, CLU_UPDATE_TYPE, CLU_DETAILS, CLU_NEXT_HEARING_DT, CLU_NEXT_HEARING_STAGE, CLU_CREATE_BY, CLU_CREATE_DATE, CLU_UPDATE_BY, " +
                                 " CLU_UPDATE_DATE, a.HM_NAME AS PrevHearingStage, b.HM_NAME AS nextHearingStage FROM         TBL_COMP_LITIGATION_UPDATES LEFT OUTER JOIN  TBL_HEARINGSTAGE_MASTER AS a ON CLU_PREV_HEARING_STAGE = a.HM_ID LEFT OUTER JOIN" +
                                 " TBL_HEARINGSTAGE_MASTER AS b ON CLU_NEXT_HEARING_STAGE = b.HM_ID inner join   TBL_COMP_LIT_UPDATE_TYPES on CLUT_ID=CLU_UPDATE_TYPE where CLU_CLGM_ID= " + id + " order by CLU_STATUS_DATE Desc";
                        break;
                    }

                    case "LITIGATIONUPDATES2":
                    {
                        strSql = "SELECT CLU_ID, CLUT_NAME, CLU_CLGM_ID, CLU_STATUS_DATE, CLU_UPDATE_TYPE, CLU_DETAILS, CLU_CREATE_BY, CLU_CREATE_DATE " +
                                " FROM  TBL_COMP_LITIGATION_UPDATES2 INNER JOIN  TBL_COMP_LIT_UPDATE_TYPES ON CLU_UPDATE_TYPE = CLUT_ID where CLU_CLGM_ID= " + id + " order by CLU_STATUS_DATE Desc";
                        break;
                    }

                    case "LGLLITIGATIONUPDATES2":
                    {
                        strSql = "SELECT LLU_ID, LLUT_NAME, LLU_CLGM_ID, LLU_STATUS_DATE, LLU_UPDATE_TYPE, LLU_DETAILS, LLU_CREATE_BY, LLU_CREATE_DATE " +
                                " FROM  TBL_LGL_LTGN_UPDATES2 INNER JOIN  TBL_LGL_LIT_UPDATE_TYPES ON LLU_UPDATE_TYPE = LLUT_ID where LLU_CLGM_ID= " + id + "";
                        break;
                    }

                    case "LEGALLITIGATIONUPDATES":
                    {
                        strSql = "SELECT LLU_ID, LLU_LLM_ID, LLU_STATUS_DATE,  LLUT_NAME, LLU_PREV_HEARING_STAGE, LLU_UPDATE_TYPE, LLU_DETAILS, LLU_NEXT_HEARING_DT, LLU_NEXT_HEARING_STAGE, " +
                                 " LLU_CREATE_BY, LLU_CREATE_DATE, LLU_UPDATE_BY, LLU_UPDATE_DATE, a.LHM_NAME AS PrevHearingStage, b.LHM_NAME AS nextHearingStage FROM TBL_LGL_LTGN_UPDATES LEFT OUTER JOIN " +
                                 " TBL_LGL_HEARINGSTAGE_MASTER AS b ON LLU_NEXT_HEARING_STAGE = b.LHM_ID LEFT OUTER JOIN  TBL_LGL_HEARINGSTAGE_MASTER AS a ON LLU_PREV_HEARING_STAGE = a.LHM_ID " +
                                " inner join TBL_LGL_LIT_UPDATE_TYPES on LLUT_ID=LLU_UPDATE_TYPE WHERE  LLU_LLM_ID = " + id + " order by LLU_CREATE_DATE desc";
                        break;
                    }
                     case "PrevHearingStage":
                        {
                            strSql = "select top 1 LU_CREATE_DATE, TBL_HEARINGSTAGE_MASTER.*  from TBL_HEARINGSTAGE_MASTER, TBL_LITIGATION_UPDATES where LU_LGM_ID= " + id + " and LU_NEXT_HEARING_STAGE = HM_ID order by LU_CREATE_DATE desc";
                            break;
                        }

      case "PRPById":
                        {
                            strSql = "select * from TBL_PRP, TBL_PRP_DEPT, TBL_PRODUCT_MASTER where PRP_PRODUCT_ID = PM_ID and PRP_RECEIVED_FROM_ID = PD_ID and PRP_ID=" + id;
                            break;
                        }
                        case "PRPFILES":
                        {
                            strSql = "select * from TBL_PRP_FILES where PF_PRP_ID=" + id;
                            break;
                        }


                        case "PRPAppById":
                        {
                            strSql = "select case when PDA_PRP_STATUS = 'PFA' then 'Pending for Approval' when PDA_PRP_STATUS = 'A' then 'Approved'  when PDA_PRP_STATUS = 'R' then 'Rejected' end as [Status], * from TBL_PRP_DEPT_APPROVERS, TBL_PRP_APPROVING_DEPT where  PDA_DEPT_ID = PAD_ID and PDA_PRP_ID=" + id;
                            break;
                        }
                        case "PRPProductById":
                        {
                            strSql = "select PM_ID from TBL_PRP, TBL_PRODUCT_MASTER where PRP_PRODUCT_ID = PM_ID and PRP_ID=" + id;
                            break;
                        }
                        case "PRPDeptById":
                        {
                            strSql = "select PD_ID from TBL_PRP, TBL_PRP_DEPT where PRP_RECEIVED_FROM_ID = PD_ID and PRP_ID=" + id;
                            break;
                        }

                        case "PRPOWNERSFORMAIL":
                        {

                            strSql = "select PU_EMAIL_ID from TBL_PRP_USERS WHERE PU_LEVEL=0 AND PU_PAD_ID= " + id;
                            break;
                        }
                        case "DELETEPREVPLICYDETAILS":
                        {
                            strSql = "DELETE FROM TBL_COMP_LTGN_POLICYDETS WHERE CLP_LGM_ID=" + id;
                            break;
                        }
                        case "DELETEPREVOWNERS":
                        {
                            strSql = "DELETE FROM TBL_COMP_LIT_OWNER_MAPPING WHERE CLOM_CLGM_ID=" + id;
                            break;
                        }

    case "DELETEPREVPLGLDETAILS":
                        {
                            strSql = "DELETE FROM TBL_COMP_LN_POLICYDETS WHERE CLNP_LM_ID=" + id;
                            break;
                        }
                        case "DELETEPREVLGLOWNERS":
                        {
                            strSql = "DELETE FROM TBL_COMP_LEG_OWNER_MAPPING WHERE LOM_LM_ID=" + id;
                            break;
                        }

                        case "HELPDESKTAT":
                        {
                            strSql = "SELECT HD_ID, HR_ID, HR_RESPONSE,HR_RESPONSE_DATETIME,HR_RESPONDENT,(cast((TATInSecs/3600) as varchar) + ' hrs ' + cast((TATInSecs%3600/60) as varchar) + ' min ' + cast((TATInSecs%60) as varchar) + ' sec') as TAT FROM vwHelpDeskResponsesTAT WHERE HD_ID = " + id;
                            break;
                        }

       case "MISCCONTENTS":
                        {
                            strSql = " SELECT MC_CONTENT FROM TBL_MISC_CONTENT WHERE MC_TYPE_ID=" + id;
                            break;
                        }
                        case "MISCCONTENTSEDIT":
                        {
                            strSql = " SELECT * FROM TBL_MISC_CONTENT WHERE MC_ID=" + id;
                            break;
                        }

                        case "DELETEMISCCONTENTS":
                        {
                            strSql = "DELETE FROM TBL_MISC_CONTENT WHERE MC_ID=" + id;
                            break;
                        }
     case "DELETEUPDATE":
                        {
                            strSql = "DELETE FROM TBL_COMP_LITIGATION_UPDATES WHERE CLU_ID=" + id + "; DELETE FROM TBL_COMP_LIT_UPDATE_FILES WHERE CLUF_CLU_ID=" + id;
                            break;
                        }
                        case "DELETELGLUPDATE":
                        {
                            strSql = "DELETE FROM TBL_LGL_LTGN_UPDATES WHERE LLU_ID=" + id + "; DELETE FROM TBL_LGL_LTGN_UPDATES_FILES WHERE LLUF_LLU_ID=" + id;
                            break;
                        }

      case "CATEGORYBYTYPE":
                        {
                            strSql = "SELECT * FROM TBL_HELPDESK_CATEGORY_MAS WHERE HCM_HDM_ID=1 and HCM_TYPE=" + id + " ORDER BY HCM_NAME";
                            break;
                        }

     case "PRPDEPTUSERS":
                        {
                            strSql = "select PDU_EMAIL_ID from TBL_PRP_DEPT_USERS where PDU_PD_ID=" + id;
                            break;
                        }
                        case "GETPRPAPPROVERS":
                        {
                            strSql = "SELECT     PU_EMAIL_ID, PDA_PRP_ID FROM  TBL_PRP_USERS INNER JOIN TBL_PRP_DEPT_APPROVERS ON PU_PAD_ID = PDA_DEPT_ID where PDA_PRP_ID=" + id;
                            break;
                        }

     case "ContractFiles":
                        {
                            strSql = "select * from TBL_CONTRACT_FILES WHERE CONF_CON_ID= " + id;
                            break;
                        }
                        case "DeleteContractFiles":
                        {
                            strSql = "delete from TBL_CONTRACT_FILES WHERE CONF_ID= " + id;
                            break;
                        }
                        case "DeleteContract":
                        {
                            strSql = "delete from TBL_CONTRACT_FILES WHERE CONF_ID= " + id + ";delete from TBL_CONTRACT_MAS WHERE CON_ID= " + id;
                            break;
                        }

                        // Added By Ashwin on 13 Nov 2010
                        case "PGRCPolicyDets":
                        {
                            strSql = "select * from TBL_PGRC_POLICY_DETS where PPD_PC_ID=" + id;
                            break;
                        }
                        case "UWPolicyDets":
                        {
                            strSql = "select * from TBL_UW_POLICY_DETS where UWPD_UWC_ID=" + id;
                            break;
                        }
                        case "WBPolicyDets":
                        {
                            strSql = "select * from TBL_WB_POLICY_DETS where WBPD_WBC_ID=" + id;
                            break;
                        }
                        case "IRDAPolicyDets":
                        {
                            strSql = "select * from TBL_IRDA_POLICY_DETS where IPD_IC_ID=" + id;
                            break;
                        }
                        case "PGRCFILES":
                        {
                            strSql = "SELECT * FROM TBL_PGRC_FILES WHERE PGRCF_PC_ID = " + id + "";
                            break;
                        }
                        case "UWFILES":
                        {
                            strSql = "SELECT * FROM TBL_UW_FILES WHERE UWF_UW_ID = " + id + "";
                            break;
                        }
                        case "WBFILES":
                        {
                            strSql = "SELECT * FROM TBL_WB_FILES WHERE WBF_WBC_ID = " + id + "";
                            break;
                        }

                        case "DeletePGRCFILES":
                        {
                            strSql = "delete FROM TBL_PGRC_FILES WHERE PGRCF_ID = " + id + "";
                            break;
                        }
                        case "DeleteWBFILES":
                        {
                            strSql = "delete FROM TBL_WB_FILES WHERE WBF_ID = " + id + "";
                            break;
                        }
                        case "DeleteUWFILES":
                        {
                            strSql = "delete FROM TBL_UW_FILES WHERE UWF_ID = " + id + "";
                            break;
                        }
                        case "DeleteIRDAFILES":
                        {
                            strSql = "delete FROM TBL_IRDA_FILES WHERE IRDAF_ID = " + id + "";
                            break;
                        }
                        case "DeletePGRCUpdateFiles":
                        {
                            strSql = "delete from TBL_PGRC_UPDATE_FILES where PUF_ID = " + id;
                            break;
                        }
                        case "DeleteIRDAUpdateFiles":
                        {
                            strSql = "delete from TBL_IRDA_UPDATE_FILES where IUF_ID = " + id;
                            break;
                        }

                        case "PGRCUPDATEFILES":
                        {
                            strSql = "SELECT * FROM TBL_PGRC_UPDATE_FILES WHERE PUF_PU_ID = " + id + "";
                            break;
                        }
                        case "WBUPDATEFILES":
                        {
                            strSql = "SELECT * FROM TBL_WB_UPDATE_FILES WHERE WBUF_WBU_ID = " + id + "";
                            break;
                        }
                        case "UWUPDATEFILES":
                        {
                            strSql = "SELECT * FROM TBL_UW_UPDATE_FILES WHERE UWUF_UWU_ID = " + id + "";
                            break;
                        }
                        case "IRDAUPDATEFILES":
                        {
                            strSql = "SELECT * FROM TBL_IRDA_UPDATE_FILES WHERE IUF_IU_ID = " + id + "";
                            break;
                        }
                        case "PGRCUPDATES":
                        {
                            strSql = "SELECT PU_ID, PU_PC_ID,PUT_NAME, PU_STATUS_DATE, PU_DETAILS,PU_CREATE_BY,PU_CREATE_DT,PUT_ID " +
                                     " FROM TBL_PGRC_UPDATES INNER JOIN TBL_PGRC_UPDATE_TYPES ON PU_UPDATE_TYPE=PUT_ID " +
                                     "WHERE PU_PC_ID=" + id + "";
                            break;
                        }
                        case "UWUPDATES":
                        {
                            strSql = "SELECT UWU_ID, UWU_UWC_ID,UWUT_NAME, UWU_STATUS_DT, UWU_DETAILS,UWU_CREATE_BY,UWU_CREATE_DT,UWUT_ID,UWU_LST_UPD_BY,UWU_LST_UPD_DT " +
                                     " FROM TBL_UW_UPDATES INNER JOIN TBL_UW_UPDATE_TYPE ON UWU_UPDATE_ID=UWUT_ID " +
                                     "WHERE UWU_UWC_ID=" + id + "";
                            break;
                        }

                        case "WBUPDATES":
                        {
                            strSql = "SELECT WBU_ID, WBU_WBC_ID,WBUT_NAME, WBU_STATUS_DT, WBU_DETAILS,WBU_CREATE_BY,WBU_CREATE_DT,WBUT_ID,WBU_LST_UPD_BY,WBU_LST_UPD_DT " +
                                     " FROM TBL_WB_UPDATES INNER JOIN TBL_WB_UPDATE_TYPE ON WBU_UPDATE_ID=WBUT_ID " +
                                     "WHERE WBU_WBC_ID=" + id + "";
                            break;
                        }
                        case "IRDAUPDATES":
                        {
                            strSql = "SELECT IU_ID, IU_IC_ID,IUT_NAME, IU_STATUS_DATE, IU_DETAILS,IU_CREATE_BY,IU_CREATE_DT,IUT_ID " +
                                     " FROM TBL_IRDA_UPDATES INNER JOIN TBL_IRDA_UPDATE_TYPES ON IU_UPDATE_TYPE=IUT_ID " +
                                     "WHERE IU_IC_ID=" + id + "";
                            break;
                        }
                        case "DELETEPGRCUPDATE":
                        {
                            strSql = "DELETE FROM TBL_PGRC_UPDATES WHERE PU_ID=" + id + "; DELETE FROM TBL_PGRC_UPDATE_FILES " +
                            "WHERE PUF_PU_ID=" + id;
                            break;
                        }
                        case "DELETEUWUPDATE":
                        {
                            strSql = "DELETE FROM TBL_UW_UPDATES WHERE UWU_ID=" + id + "; DELETE FROM TBL_UW_UPDATE_FILES " +
                            "WHERE UWUF_UWU_ID=" + id;
                            break;
                        }
                        case "DELETEWBUPDATE":
                        {
                            strSql = "DELETE FROM TBL_WB_UPDATES WHERE WBU_ID=" + id + "; DELETE FROM TBL_WB_UPDATE_FILES " +
                            "WHERE WBUF_WBU_ID=" + id;
                            break;
                        }
                        case "DELETEIRDAUPDATE":
                        {
                            strSql = "DELETE FROM TBL_IRDA_UPDATES WHERE IU_ID=" + id + "; DELETE FROM TBL_IRDA_UPDATE_FILES " +
                            "WHERE IUF_IU_ID=" + id;
                            break;
                        }

    case "IRDAFiles":
                        {
                            strSql = "SELECT * FROM TBL_IRDA_FILES WHERE IRDAF_IC_ID = " + id + "";
                            break;
                        }
                        case "UWBRANCH":
                        {
                            strSql = "select   UWBM_ID   , UWBM_NAME    from  TBL_UW_WB_BRANCH_MAS where UWBM_UWZM_ID= "
                                      + id + " order by  UWBM_NAME  ";
                            break;
                        }
                        case "DeleteUWUpdateFiles":
                        {
                            strSql = "delete from TBL_UW_UPDATE_FILES where UWUF_ID = " + id;
                            break;
                        }
                        case "DeleteWBUpdateFiles":
                        {
                            strSql = "delete from TBL_WB_UPDATE_FILES where WBUF_ID = " + id;
                            break;
                        }
                        case "DeleteUWPolicyDets":
                        {
                            strSql = "delete from TBL_UW_POLICY_DETS where UWPD_ID = " + id;
                            break;
                        }
                        case "DeleteWBPolicyDets":
                        {
                            strSql = "delete from TBL_WB_POLICY_DETS where WBPD_ID = " + id;
                            break;
                        }
                        case "DeleteIRDAPolicyDets":
                        {
                            strSql = "delete from TBL_IRDA_POLICY_DETS where IPD_ID = " + id;
                            break;
                        }
                        case "DeletePGRCPolicyDets":
                        {
                            strSql = "delete from TBL_PGRC_POLICY_DETS where PPD_ID = " + id;
                            break;
                        }

      case "IRDASubCat":
                        {
                            strSql = "select ISM_ID, ISM_NAME " +
                                     " FROM TBL_IRDA_SUBCAT_MAS where ISM_ICM_ID =" + id + " order by ISM_NAME";
                            break;
                        }

                        case "PGRCSubCat":
                        {
                            strSql = "select PSM_ID, PSM_NAME " +
                                 " FROM TBL_PGRC_SUBCAT_MAS where PSM_PCM_ID =" + id + " order by PSM_NAME";
                            break;
                        }

     //<<Added by Denil Shah on 15-Jul-2011 for fetching active categories.
                        case "ACTIVECATEGORYBYTYPE":
                        {
                            strSql = "SELECT * FROM TBL_HELPDESK_CATEGORY_MAS WHERE HCM_STATUS = 'A' AND HCM_HDM_ID=1 and HCM_TYPE=" + id + " ORDER BY HCM_NAME";
                            break;
                        }
                        //>>

                        //<<Added by Prajakta on 31-Aug-2011 for fetching active categories.
                        case "ACTIVECATEGORY":
                        {
                            strSql = "SELECT * FROM TBL_HELPDESK_CATEGORY_MAS WHERE HCM_STATUS = 'A' AND HCM_HDM_ID=1 ORDER BY HCM_NAME";
                            break;
                        }
                        //>>
                        case "GROUPMEMBERS":
                        {
                            strSql = "select   HO_ID   , HO_EMPNAME    from  TBL_HELPDESK_OWNERS " +
                            " where HO_HDG_ID=" + id + " order by  HO_EMPNAME  ";
                            break;
                        }

     //<< Added by Prajakta on 21-May-2012
                        case "getUpdByFCId":
                        {
                            strSql = "SELECT TBL_FRAUD_CASE_UPDATES.*, CASE WHEN FCU_STATUS = 'O' " +
                                    " THEN 'Open' when FCU_STATUS = 'C' then 'Closed' " +
                                    " else '' end as Status, CASE WHEN FCU_ACCEPTED_REJECTED = 'A' " +
                                    " THEN 'Accepted' when FCU_ACCEPTED_REJECTED = 'R' then 'Rejected' " +
                                    " else '' end as AccRej " +
                                    " FROM TBL_FRAUD_CASE_UPDATES INNER JOIN " +
                                    " TBL_FRAUD_CASE_MAS ON FCM_ID = FCU_FCM_ID AND FCU_REC_STATUS = 'A' " +
                                    " AND FCM_ID = " + id;
                            break;
                        }
                        //>>
                        //<< Added by Nikhil Adhalikar on 28-Jan-2012
                        case "getFCUpdatesbyId":
                        {
                            strSql = "SELECT * FROM TBL_FRAUD_CASE_UPDATES WHERE FCU_ID = " + id;
                            break;
                        }
                        //>>

                        //<< Modify by Supriya on 05-Aug-2014
                        case "getFraudCaseById":
                        {
                            strSql = "SELECT *, a.RC_NAME as CompSrc, b.RC_NAME as CompNature, c.RC_NAME as Zone, " +
                                " d.RC_NAME as AmtType, e.RC_NAME as Category, f.RC_NAME as Scope, " +
                                " g.RC_NAME as Channel, " +
                                " CASE WHEN FCM_CASE_STATUS = 'O' THEN 'Open' when FCM_CASE_STATUS = 'C' then " +
                                " 'Closed' else '' end as Status, CASE WHEN FCM_CASE_STATUS = 'O' " +
                                " THEN datediff(day,FCM_COMPLAINT_DT, CURRENT_TIMESTAMP) else '' end as Ageing, " +
                                " CASE WHEN FCM_IS_FIR_REGISTERED = 'Y' THEN " +
                                " 'Yes' when FCM_IS_FIR_REGISTERED = 'N' then 'No' else '' end as IsFIRRegd, " +
                                " CASE when FCM_ACCEPTED_REJECTED = 'A' then 'Accepted' " +
                                " when FCM_ACCEPTED_REJECTED = 'R' then 'Rejected' " +
                                " else '' end as AccRej, " +
                                " CASE WHEN FCM_CLOSURE_DT is null THEN '' " +
                                " else datediff(day,FCM_COMPLAINT_DT, FCM_CLOSURE_DT) end as TAT " +
                                " FROM TBL_FRAUD_CASE_MAS LEFT OUTER JOIN TBL_REF_CODES a on " +
                                " (a.RC_CODE = FCM_COMPLAINT_SOURCE AND a.RC_TYPE = 'Fraud Complaint Source') " +
                                " LEFT OUTER JOIN TBL_REF_CODES b on (b.RC_CODE = FCM_NATURE_OF_COMPLAINT AND " +
                                " b.RC_TYPE = 'Fraud Complaint Nature') " +
                                " LEFT OUTER JOIN TBL_REF_CODES c on " +
                                " (c.RC_CODE = FCM_ZONE_ID AND c.RC_TYPE = 'Zone')" +
                                " LEFT OUTER JOIN TBL_REF_CODES d on " +
                                " (d.RC_CODE = FCM_DISPUTE_FINANCIAL_IMP_AMT AND d.RC_TYPE = 'Dispute Amount') " +
                                " LEFT OUTER JOIN TBL_REF_CODES e ON " +
                                " (e.RC_CODE = FCM_CATEGORY AND e.RC_TYPE = 'Fraud Category') " +
                                " LEFT OUTER JOIN TBL_REF_CODES f ON " +
                                " (f.RC_CODE = FCM_SCOPE AND f.RC_TYPE = 'Fraud Scope') " +
                                " LEFT OUTER JOIN TBL_REF_CODES g ON " +
                                " (g.RC_CODE = FCM_CHANNEL AND g.RC_TYPE = 'Channel') " +
                                " LEFT OUTER JOIN TBL_FC_UW_BRANCH_MAS ON " +
                                " FCM_FUBM_ID = FUBM_ID " +
                                " LEFT OUTER JOIN TBL_UW_ACTION_TAKEN ON FCM_ACTION_TAKEN = UWAT_ID " +
                                 " LEFT OUTER JOIN TBL_FRAUD_BUSINESS_SEGMENT ON FCM_BUSINESS_SEGMENT = FBS_ID " +
                                 " LEFT OUTER JOIN TBL_FRAUD_CASE_REPORTED_TO ON FCM_CASE_REPORTED_TO = FCRT_ID " +
                                 " LEFT OUTER JOIN TBL_FRAUD_CASE_CLOSING_BASIS ON FCM_BASIS_OF_CLOSING_CASE = FCCB_ID " +
                                 " LEFT OUTER JOIN TBL_FRAUD_COMPLAINT_CATEGORY ON FCM_COMPLAINT_CATEGORY = FCC_ID " +
                                " LEFT OUTER JOIN TBL_FRAUD_SOURCE_DEPT ON FCM_TYPE = FSD_ID " +

                                " WHERE FCM_ID = " + id + " AND FCM_REC_STATUS = 'A'";
                            break;
                        }

                        //<< Added by Nikhil Adhalikar on 30-Jan-2012
                        case "getUWById":
                        {
                            strSql = "SELECT *,replace(CONVERT(VARCHAR,UWCN_CASE_RECEIVED_DT ,106),' ','-') AS CaseRecDate, " +
                                " replace(CONVERT(VARCHAR,UWCN_COMP_DECISION_DT ,106),' ','-') AS CompDecDate,  b.RC_NAME as Zone, " +
                                " replace(CONVERT(VARCHAR,UWCN_COMPLIANCE_CLOSURE_DATE ,106),' ','-') AS CompClosureDate,  " +
                                " replace(CONVERT(VARCHAR,UWCN_CHANNEL_CLOSURE_DATE ,106),' ','-') AS ChannelClosureDate, " +
                                " c.RC_NAME as Channel, CASE when UWCN_ACCEPTED_REJECTED = 'A' then 'Accepted' " +
                                " when UWCN_ACCEPTED_REJECTED = 'R' then 'Rejected' " +
                                " else '' end as AccRej " +
                                " FROM TBL_UW_COMPLAINTS_NEW LEFT OUTER JOIN TBL_FC_UW_BRANCH_MAS ON " +
                                " UWCN_FUBM_ID = FUBM_ID " +
                                " LEFT OUTER JOIN TBL_UW_NATURE_OF_COMPLAINTS on UWNC_ID= UWCN_COMPLAINT_NATURE " +
                                " LEFT OUTER JOIN TBL_UW_ACTION_TAKEN on UWAT_ID= UWCN_ACTION_TAKEN " +
                                //" LEFT OUTER JOIN TBL_REF_CODES a on " +
                                //" (a.RC_CODE = UWCN_COMPLAINT_NATURE AND " +
                                //" a.RC_TYPE = 'Fraud Complaint Nature') "+
                                " LEFT OUTER JOIN TBL_REF_CODES b on " +
                                " (b.RC_CODE = UWCN_ZONE AND b.RC_TYPE = 'Zone')" +
                                " LEFT OUTER JOIN TBL_REF_CODES c on (c.RC_CODE = UWCN_CHANNEL and c.RC_TYPE = 'Channel') " +
                                " WHERE UWCN_ID = " + id + "AND UWCN_REC_STATUS = 'A'";
                            break;
                        }
                        //<< Added by Nikhil Adhalikar on 30-Jan-2012
                        case "getUWFiles":
                        {
                            strSql = "SELECT * FROM TBL_UW_FILES WHERE UWF_UW_ID = " + id + "";
                            break;
                        }

                        //<< Added by Nikhil Adhalikar on 29-Feb-2012 
                        case "getBrCodebyId":
                        {
                            strSql = "SELECT * FROM TBL_FC_UW_BRANCH_MAS WHERE FUBM_ID = " + id;
                            break;
                        }
                        //>>

                        //<< Added by Nikhil Adhalikar on 23-Apr-2012 
                        case "getPolicyDetsbyFraudId":
                        {
                            strSql = "SELECT * FROM TBL_FRAUD_POLICY_DETS WHERE FPD_FCM_ID = " + id;
                            break;
                        }

    case "getMiscReportsMasterById":
                        {
                            strSql = "select * from TBL_MISC_REPORTS_MAS where MRM_ID=" + id;
                            break;
                        }
                        case "getMiscReportsDataById":
                        {
                            strSql = "select * from TBL_MISC_REPORTS_DATA where MRD_MRM_ID=" + id;
                            break;
                        }
                        //<< Added by Supriya on 30-Nov-2012
                        case "DeleteMiscReportDataById":
                        {
                            strSql = "DELETE FROM TBL_MISC_REPORTS_DATA WHERE MRD_ID=" + id;
                            break;
                        }
                        case "getMiscReportsByMRDId":
                        {
                            strSql = "select * from TBL_MISC_REPORTS_DATA where MRD_ID=" + id;
                            break;
                        }
                        case "getReportsDataById":
                        {
                            strSql = "select * FROM TBL_MISC_REPORTS_MAS inner join TBL_MISC_REPORTS_DATA on MRD_MRM_ID=MRM_ID WHERE MRD_ID=" + id;
                            break;
                        }
                        //Added by supriya on 14-Mar-13
                        case "LitAdvFeesDetsById":
                        {
                            strSql = "Select * from TBL_COMP_ADV_FEES where CLAF_ID=" + id;
                            break;
                        }

                        case "DeleteAppealPaymetsDets":
                        {
                            strSql = "DELETE FROM TBL_COMP_LITIGATION_UPDATES2 WHERE CLU_ID=" + id + "; DELETE FROM TBL_COMP_LITIGATION_PAY_DETS WHERE CLPD_CLU_ID=" + id;
                            break;
                        }
                        //Added by supriya on 20-Jul-2013
                        case "DeleteUWAddCommentFiles":
                        {
                            strSql = "delete from TBL_UW_ADD_COMMENTS_FILES where UACF_ID = " + id;
                            break;
                        }
                        case "UWAddCommentFiles":
                        {
                            strSql = "SELECT * FROM TBL_UW_ADD_COMMENTS_FILES WHERE UACF_UAC_ID = " + id + "";
                            break;
                        }
                        case "getUWAddComment":
                        {
                            strSql = "SELECT * FROM TBL_UW_ADD_COMMENTS WHERE UAC_UWCN_ID = " + id + "";
                            break;
                        }
                        case "deleteUWAddComment":
                        {
                            strSql = "DELETE FROM TBL_UW_ADD_COMMENTS WHERE UAC_ID=" + id + "; DELETE FROM TBL_UW_ADD_COMMENTS_FILES WHERE UACF_UAC_ID=" + id;
                            break;
                        }
                        //>>
                        //Added by supriya on 24-Jul-2013
                        case "getAttachmentDetsbyFraudId":
                        {
                            strSql = "SELECT * FROM TBL_FRAUD_CASE_FILES WHERE FCF_FCM_ID = " + id + "";
                            break;
                        }
                        //Added by Bhavik 25-Jul-2013
                        case "getNVCById":
                        {
                            strSql = "select *,replace(CONVERT(VARCHAR,NVC_DATE_INIT_TO_COMP ,106),' ','-') AS CaseInitDate," +
                               "replace(CONVERT(VARCHAR,NVC_LETTER_DISP_DATE ,106),' ','-') AS LetterDispDate," +
                               "replace(CONVERT(VARCHAR,NVC_PREMIUM_PAID_DATE ,106),' ','-') AS PremPaidDate," +
                               "replace(CONVERT(VARCHAR,NVC_POLAD_CLOSURE_DATE ,106),' ','-') AS POLADDate,RF.RC_NAME as Zone," +
                               "replace(CONVERT(VARCHAR,NVC_COMP_CLOSURE_DATE ,106),' ','-') AS CompDate," +
                               "replace(CONVERT(VARCHAR,NVC_CREATE_DT ,106),' ','-') AS CaseInitDT," +
                               "replace(CONVERT(VARCHAR,NVC_CHANNEL_CLOSURE_DATE ,106),' ','-') AS ChannelClsDate, " +
                               "CASE when NVC_ACCEPTED_REJECTED = 'A' then 'Accepted'when NVC_ACCEPTED_REJECTED = 'R' then 'Rejected'  else '' end as AccRej, " +
                               "UWAT_NAME As ChannelImpl " +
                               "from TBL_NULL_VOID_CASES " +
                               "LEFT OUTER JOIN TBL_REF_CODES RF on(RF.RC_CODE = NVC_ZONE AND RF.RC_TYPE = 'Zone') " +
                               "LEFT OUTER JOIN TBL_NVC_UW_TL_MAS on NUTM_ID =NVC_CASE_REFERRED_BY " +
                               "LEFT OUTER JOIN TBL_FC_UW_BRANCH_MAS on FUBM_ID=NVC_BR_NAME " +
                               "LEFT OUTER JOIN TBL_UW_NATURE_OF_COMPLAINTS on UWNC_ID= NVC_NATURE_OF_COMPLAINT " +
                               "Left OUTER JOIN TBL_UW_ACTION_TAKEN on UWAT_ID= NVC_CHANNEL_ACTION_IMP Where NVC_CID = " + id;
                            break;
                        }
                        case "getNVCFiles":
                        {
                            strSql = "SELECT * FROM TBL_NULL_VOID_CASES_FILES WHERE NVCF_NVC_ID = " + id + "";
                            break;
                        }
                        case "getNVCUWTLById":
                        {
                            strSql = "SELECT * FROM TBL_NVC_UW_TL_MAS WHERE NUTM_ID = " + id + "";
                            break;
                        }
                        //>>
                        //Added by supriya on 08-Aug-2013
                        case "DeleteFraudAddCommentFiles":
                        {
                            strSql = "delete from TBL_FRAUD_ADD_COMMENTS_FILES where FACF_ID = " + id;
                            break;
                        }
                        case "FraudAddCommentFiles":
                        {
                            strSql = "SELECT * FROM TBL_FRAUD_ADD_COMMENTS_FILES WHERE FACF_FAC_ID = " + id + "";
                            break;
                        }
                        case "getFraudAddComment":
                        {
                            strSql = "SELECT * FROM TBL_FRAUD_ADD_COMMENTS WHERE FAC_FCM_ID = " + id + "";
                            break;
                        }
                        case "deleteFraudAddComment":
                        {
                            strSql = "DELETE FROM TBL_FRAUD_ADD_COMMENTS WHERE FAC_ID=" + id + "; DELETE FROM TBL_FRAUD_ADD_COMMENTS_FILES WHERE FACF_FAC_ID=" + id;
                            break;
                        }
                        //>>
                        //Added by supriya on 27-Aug-2013
                        case "SecHelpdeskCategoryOwners":
                        {
                            strSql = "SELECT  SHO_ID, sHO_EMAIL,SHO_USERNAME FROM  TBL_SEC_HELPDESK_OWNERS " +
                            " inner join TBL_SEC_SHCM_HO_MAPPING on SHHM_SHO_ID=SHO_ID where SHHM_LEVEL=0 and SHO_IS_AVAILABLE='Y' " +
                            " and SHHM_SHCM_ID=" + id + " and SHHM_PRIORITY =( " +
                            " SELECT  min(SHHM_PRIORITY) as Priprity FROM  TBL_SEC_HELPDESK_OWNERS " +
                            "inner join TBL_SEC_SHCM_HO_MAPPING on SHHM_SHO_ID=SHO_ID  where SHHM_LEVEL=0 and SHO_IS_AVAILABLE='Y' " +
                            " and SHHM_SHCM_ID=" + id + " )";
                            break;
                        }
                        case "SecHelpdeskCategoryOwnersMail":
                        {
                            strSql = "SELECT  SHHM_SHCM_ID,SHO_ID, SHO_EMAIL,SHO_USERNAME FROM TBL_SEC_HELPDESK_OWNERS " +
                                    " inner join TBL_SEC_SHCM_HO_MAPPING on SHHM_SHO_ID=SHO_ID  " +
                                    " where SHHM_PRIORITY=0 and SHHM_LEVEL=0  and SHO_IS_AVAILABLE='Y'  " +
                                    " and SHHM_SHCM_ID= " + id + "" +
                                    " union SELECT  SHHM_SHCM_ID,SHO_ID, SHO_EMAIL,SHO_USERNAME FROM  TBL_SEC_HELPDESK_OWNERS " +
                                    " inner join TBL_SEC_SHCM_HO_MAPPING on SHHM_SHO_ID=SHO_ID " +
                                    " where SHHM_LEVEL=1 and SHO_IS_AVAILABLE='Y' and SHHM_SHCM_ID=" + id;

                            break;
                        }
                        //>>
                        //Added by supriya on 28-Aug-2013
                        case "LitPaymentDetsById":
                        {
                            strSql = "Select * from TBL_COMP_LITIGATION_PAY_DETS " +
                                "  where CLPD_CLU_ID=" + id;
                            break;
                        }
                        case "getBaseLitigationById":
                        {
                            strSql = "select  CLGM_ID, CAST(CLGM_ID AS VARCHAR)+ ' - '+ isnull (CLF_NAME , '')+ ' - '+ isnull (CLGM_CASE_NO , '')+ ' - '+ CLGM_COMPLAINT as [BaseLitigationId]   " +
                                  "  from TBL_COMP_LITIGATION_MASTER " +
                                  " LEFT OUTER JOIN TBL_COMP_LITIGATION_FORUM ON CLGM_CLF_ID = CLF_ID " +
                                  "where CLGM_ID = " + id + "";
                            break;
                        }
                        case "getLitigationById":
                        {
                            strSql = "SELECT * from TBL_COMP_LITIGATION_MASTER where CLGM_ID = " + id;
                            break;

                        }

                        //<< Added By Bhavik 03-sep-2013
                        case "getMSCById":
                        {
                            strSql = "select *," +
                                    "Replace(CONVERT(VARCHAR,MSC_COMPLAINT_RCV_DT_FRM_SA ,106),'','-') AS CMP_RCV_DT_FRM_SA, " +
                                    "Replace(CONVERT(VARCHAR,MSC_COMPLAINT_RCV_DT_FRM_CLIENT ,106),'','-') AS CMP_RCV_DT_FRM_CL, " +
                                    "Replace(CONVERT(VARCHAR,MSC_MS_CLOSURE_DATE ,106),'','-') AS MS_CLS_DATE," +
                                    "Replace(CONVERT(VARCHAR,MSC_DATE_LITIGATION_FIR_REG ,106),'','-') AS FIR_REG_DATE," +
                                    "Replace(CONVERT(VARCHAR,MSC_CHANNEL_CLOSURE_DATE ,106),'','-') AS Channel_CLS_DATE," +
                                    "CASE when MSC_ACCEPTED_REJECTED_PA = 'A' then 'Accepted'when MSC_ACCEPTED_REJECTED_PA = 'R' then 'Rejected'when MSC_ACCEPTED_REJECTED_PA = 'PA' then 'Partially Accepted'  else '' end as AccRej, " +
                                    "CASE when MSC_LITIGATION_FIR_REG = 'Y' then 'Yes'when MSC_LITIGATION_FIR_REG = 'N' then 'No' else '' end as FIR_Reg " +
                                    "FROM TBL_MISSELLING_CASES " +
                                    "LEFT OUTER JOIN TBL_REF_CODES RF on(RF.RC_CODE = MSC_ZONES AND RF.RC_TYPE = 'Zone') " +
                                    "LEFT OUTER JOIN TBL_FC_UW_BRANCH_MAS on FUBM_ID=MSC_BR_NAME " +
                                    "LEFT OUTER JOIN TBL_UW_ACTION_TAKEN  on UWAT_ID= MSC_CHANNEL_ACTION_TAKEN " +
                                    "LEFT OUTER JOIN TBL_MSC_NATURE_OF_COMPLAINTS on MSCNC_ID= MSC_NATURE_OF_COMPLAINT " +
                                    "LEFT OUTER JOIN TBL_MSC_ST_NATURE_OF_COMPLAINTS on STMSCNC_ID= MSC_SUB_TYPE_NATURE_OF_COMPLAINT Where MSC_ID=" + id;
                            break;
                        }
                        case "getMSCFiles":
                        {
                            strSql = "SELECT * FROM TBL_MISSELLING_CASES_FILES WHERE MSCF_MSC_ID = " + id + "";
                            break;
                        }
                        //>>

                        //<< Added By Supriya 07-Sep-2013
                        case "getSecretarialActionableOwnerDetsById":
                        {
                            //strSql = "select * from TBL_SEC_ACTIONABLE_OWNER where SAO_SA_ID=" + id;
                            //<< Modified By Bhavik Patel @13Aug2014
                            strSql = "select * from TBL_SEC_ACTIONABLE_OWNER " +
                                " inner join TBL_SEC_PERSON_RESPONSIBLE_MAS on SPRM_ID = SAO_PERSON_RESPONSIBLE " +
                                " where SAO_SA_ID=" + id;
                            //>>
                            break;
                        }
                        case "LitigationLegalDeposit":
                        {
                            strSql = "SELECT  * FROM TBL_COMP_LITIGATION_DEPOSITDETS " +
                                     " where CLD_CLGM_ID= " + id + " order by CLD_ID ";
                            break;
                        }
                        case "LitigationLegalDepositById":
                        {
                            strSql = "SELECT  * FROM TBL_COMP_LITIGATION_DEPOSITDETS " +
                                     " where CLD_ID= " + id + " order by CLD_ID ";
                            break;
                        }
                        //<< Added By Supriya 11-Sep-2013
                        case "SecActionableUpdateByID":
                        {
                            strSql = "select * from TBL_SEC_ACTIONABLE_UPDATES where SAU_ID = " + id;
                            break;
                        }
                        case "SecActionableUpdateBySAID":
                        {
                            strSql = "select * from TBL_SEC_ACTIONABLE_UPDATES where SAU_SA_ID = " + id;
                            break;
                        }

                        case "SecActionableUpdateFiles":
                        {
                            strSql = "SELECT * FROM TBL_SEC_ACTIONABLE_FILES WHERE SAF_SAU_ID = " + id + "";
                            break;
                        }
                        case "DeleteSecActionableUpdateFiles":
                        {
                            strSql = "delete from TBL_SEC_ACTIONABLE_FILES where SAF_ID = " + id;
                            break;
                        }
                        case "DeleteSecActionableUpdate":
                        {
                            strSql = "DELETE FROM TBL_SEC_ACTIONABLE_UPDATES WHERE SAU_ID=" + id + "; DELETE FROM TBL_SEC_ACTIONABLE_FILES WHERE SAF_SAU_ID=" + id;
                            break;
                        }

                        //Added by Bhavik on 13-Sep-2013
                        case "DeleteMissellingAddCommentFiles":
                        {
                            strSql = "delete from TBL_MISSELLING_ADD_COMMENTS where MACF_ID = " + id;
                            break;
                        }
                        case "MissellingAddCommentFiles":
                        {
                            strSql = "SELECT * FROM TBL_MISSELLING_ADD_COMMENTS_FILES WHERE MACF_MAC_ID = " + id + "";
                            break;
                        }
                        case "getMissellingAddComment":
                        {
                            strSql = "SELECT * FROM TBL_MISSELLING_ADD_COMMENTS WHERE MAC_MSC_ID = " + id + "";
                            break;
                        }
                        case "deleteMissellingAddComment":
                        {
                            strSql = "DELETE FROM TBL_MISSELLING_ADD_COMMENTS WHERE MAC_ID=" + id + "; DELETE FROM TBL_MISSELLING_ADD_COMMENTS_FILES WHERE MACF_MAC_ID=" + id;
                            break;
                        }
                        //>>

      //Added by bhavik on 17-Sep-2013
                        case "DeleteNVCAddCommentFiles":
                        {
                            strSql = "delete from TBL_NULL_VOID_CASE_ADD_COMMENTS_FILE where NVCACF_ID = " + id;
                            break;
                        }
                        case "NVCAddCommentFiles":
                        {
                            strSql = "SELECT * FROM TBL_NULL_VOID_CASE_ADD_COMMENTS_FILE WHERE NVCACF_NVCAC_ID = " + id + "";
                            break;
                        }
                        case "getNVCAddComment":
                        {
                            strSql = "SELECT * FROM TBL_NULL_VOID_CASES_ADD_COMMENTS WHERE NVCAC_NVC_ID = " + id + "";
                            break;
                        }
                        case "deleteNVCAddComment":
                        {
                            strSql = "DELETE FROM TBL_NULL_VOID_CASES_ADD_COMMENTS WHERE NVCAC_ID=" + id + "; DELETE FROM TBL_NULL_VOID_CASE_ADD_COMMENTS_FILE WHERE NVCACF_NVCAC_ID=" + id;
                            break;
                        }
                        //>>
                        //Added by supriya on 01-Oct-2013
                        case "getAttachmentDetsAddSecActionablebyId":
                        {
                            strSql = "SELECT * FROM TBL_SEC_ACTIONABLEFILES WHERE SAF_SA_ID = " + id + "";
                            break;
                        }
                        //Added by supriya on 19-Oct-2013
                        case "SecDocIntimation":
                        {
                            strSql = "SELECT * FROM TBL_SD_INTIMATIONS WHERE SDI_SD_ID = " + id + "";
                            break;
                        }
                        case "DeleteSecDocIntimation":
                        {
                            strSql = "DELETE FROM TBL_SD_INTIMATIONS WHERE SDI_SD_ID=" + id;
                            break;
                        }
                        case "UpdateSecDocuments":
                        {
                            strSql = "UPDATE TBL_SEC_DOCS SET SD_STATUS ='InActive' WHERE SD_ID=" + id;
                            break;
                        }
                        //Added by supriya on 09-Nov-2013
                        case "getLitigationType":
                        {
                            strSql = "SELECT * FROM TBL_COMP_LITIGATION_TYPE_MASTER WHERE CLTM_ID = " + id + "";
                            break;
                        }
                        //Added by supriya on 11-Nov-2013
                        case "LitgationTypeById":
                        {
                            strSql = "SELECT * FROM  TBL_COMP_LITIGATION_TYPE_MASTER where CLTM_ID=" + id;
                            break;
                        }
                        //<< Added by Bhavik on 21-Jul-2014
                        case "getContractDetsById":
                        {
                            strSql = "Select * FROM TBL_CONTRACT_TEMPLATE WHERE CT_ID = " + id;
                            break;
                        }

                        //>>
                        //<< Added by Bhavik on 23-Jul-2014
                        case "ContractResponseComments":
                        {
                            strSql = "select * from TBL_CON_RESP_COMMENTS inner join TBL_CONTRACT_DRAFT_RESPONSE on CRC_CDR_ID=CDR_ID inner join TBL_CONTRACT_CHANGES on CRC_CC_ID=CC_ID where CRC_CDR_ID = " + id;
                            break;
                        }
                        case "ContractDraftResponsePDFFiles":
                        {
                            strSql = "SELECT * FROM TBL_CONTRACT_DRAFT_RESPONSE_FILES WHERE CDRF_FILE_TYPE ='PDF' and CDRF_CDR_ID = " + id + "";
                            break;
                        }
                        case "ContractDraftResponseFiles":
                        {
                            strSql = "SELECT * FROM TBL_CONTRACT_DRAFT_RESPONSE_FILES WHERE CDRF_CDR_ID = " + id + "";
                            break;
                        }
                        //>>
                        //<<Added by Supriya on 04-Aug-2014
                        case "getActionableDetsbyFraudId":
                        {
                            strSql = "SELECT * FROM TBL_FRAUD_ACTIONABLES WHERE FA_FCM_ID = " + id;
                            break;
                        }

    //<< Added by Bhavik on 25-Aug-2014
                        case "getMeetingCharterDetails":
                        {
                            strSql = " SELECT *,MCT_NAME FROM TBL_MISC_CONTENT inner join TBL_MISC_CONTENT_TYPE on  MC_TYPE_ID=MCT_ID where MC_ID =" + id;
                            break;
                        }

    //<< Added by Bhavik on 15-Dec-2014
                        case "getContractTemplatebyDCId":
                        {
                            strSql = "select * from TBL_DRAFT_CONTRACTS inner join TBL_CONTRACT_TEMPLATE on DC_CT_ID= CT_ID where DC_ID = " + id;
                            break;
                        }
                        //>> 
                     */
                    #endregion
                    case "BRANCH":
                        {
                            strSql = "SELECT * FROM  TBL_BRANCH_MASTER where BM_PARENT_ID = " + id + "ORDER BY BM_NAME ";
                            break;
                        }

                    case "PRPREVISIONS":
                        {
                            //strSql = "SELECT * FROM TBL_PRP_SUGGESTIONS WHERE PS_PRP_ID=" + id + "";
                            strSql = "SELECT * FROM TBL_ADVT_REVISION_SUGGESTED WHERE ARS_ARM_ID=" + id + "";
                            break;

                        }

                    case "PRPREVISIONSFILES":
                        {
                            //strSql = "SELECT * FROM TBL_PRP_SUGGESTIONS_FILES WHERE PSF_PS_ID=" + id + " ";
                            strSql = "SELECT * FROM TBL_ADVT_REVISION_SUGGESTED_FILES WHERE ARSF_ARS_ID=" + id + " ";
                            break;
                        }

                    case "REGION":
                        {
                            strSql = "SELECT * FROM  TBL_REGION_MASTER where RM_ZM_ID = " + id + "ORDER BY RM_NAME ";
                            break;
                        }
                    case "TERRITORY":
                        {
                            strSql = "SELECT * FROM  TBL_TERRITORY_MASTER where TM_RM_ID = " + id + "ORDER BY TM_NAME ";
                            break;
                        }


                    case "State":
                        {
                            strSql = "SELECT * FROM  TBL_STATE_MASTER ";
                            break;
                        }
                    case "DeleteSubEsc":
                        {
                            strSql = "delete FROM TBL_SUB_ESCALATION where SE_ID = " + id + "";
                            break;
                        }
                    case "SubEscById":
                        {
                            strSql = "SELECT * FROM TBL_SUB_ESCALATION where SE_ID = " + id + "";
                            break;
                        }
                    case "DELETESEGMENT":
                        {
                            strSql = "DELETE FROM TBL_CIRCULAR_SEGMENT_MAPPING WHERE CSM_CM_ID=" + id;
                            break;
                        }
                    case "DELETEINTIMATIONS":
                        {
                            strSql = "DELETE FROM TBL_CM_INTIMATIONS WHERE CMI_CM_ID=" + id;
                            break;
                        }
                    case "DELETECIRCULARS":
                        {
                            strSql = "DELETE FROM TBL_CIRCULAR_MASTER WHERE CM_ID= " + id + " DELETE FROM TBL_CIRCULAR_FILES WHERE CF_CM_ID=" + id;
                            break;
                        }
                    case "DELETECIRCULARFILES":
                        {
                            strSql = "DELETE FROM TBL_CIRCULAR_FILES WHERE CF_ID=" + id;
                            break;
                        }
                    case "BINDSUBSEGMENTS":
                        {
                            strSql = "SELECT SSM_CSGM_ID FROM TBL_SUBMISSIONS_SEGMENTS_MAPPING WHERE SSM_SM_ID=" + id;
                            break;
                        }

                    case "BINDCOMPANY":
                        {
                            strSql = "SELECT SCM_CM_ID FROM TBL_SM_COMPANY_MAPPING WHERE SCM_SM_ID =" + id;
                            break;
                        }
                    case "BINDOWNERS":
                        {
                            strSql = "SELECT SMO_EM_ID FROM TBL_SM_OWNERS WHERE SMO_SM_ID=" + id;
                            break;
                        }

                    case "BINDREPORTINGOWNERS":
                        {
                            strSql = "SELECT SRO_SRDOM_ID FROM TBL_SM_REPORTING_OWNERS WHERE SRO_SM_ID=" + id;
                            break;
                        }
                    case "DELETESUBSEGMENT":
                        {
                            strSql = "DELETE FROM TBL_SUBMISSIONS_SEGMENTS_MAPPING WHERE SSM_SM_ID=" + id;
                            break;
                        }
                    case "DELETESUBOWNRES":
                        {
                            strSql = "DELETE FROM TBL_SM_OWNERS WHERE SMO_SM_ID=" + id;
                            break;
                        }
                    case "DELETEREPORTINGOWNERS":
                        {
                            strSql = "DELETE FROM TBL_SM_REPORTING_OWNERS WHERE SRO_SM_ID=" + id;
                            break;
                        }
                    //case "GETOWNERS":
                    //    {
                    //        strSql = "SELECT  EM_ID,EM_EMPNAME from EmployeeMaster WHERE EM_STM_ID= " + id + " order by EM_EMPNAME" ;
                    //        break;
                    //    }
                    case "LOADSUBSEGMENTS":
                        {
                            strSql = "SELECT SSM_NAME, SSM_CSGM_ID FROM  TBL_SUBMISSIONS_SEGMENTS_MAPPING INNER JOIN TBL_SUBMISSIONS_MAS " +
                                     "ON SSM_SM_ID = SM_ID INNER JOIN TBL_SUBMISSION_SEGMENT_MAS ON SSM_CSGM_ID = TBL_SUBMISSION_SEGMENT_MAS.SSM_ID WHERE SM_ID =" + id;
                            break;
                        }

                    case "DELETESUBCHECKLIST":
                        {
                            strSql = "delete from TBL_SUB_CHKLIST where SC_SM_ID=" + id;
                            break;
                        }
                    case "DELETESUBMISSION":
                        {
                            strSql = "delete from TBL_SUBMISSIONS_SEGMENTS_MAPPING where SSM_SM_ID=" + id;
                            strSql = strSql + "delete from TBL_SM_COMPANY_MAPPING where SCM_SM_ID=" + id;
                            strSql = strSql + " DELETE FROM TBL_SM_OWNERS WHERE SMO_SM_ID=" + id;
                            strSql = strSql + "DELETE FROM TBL_SUBMISSIONS_MAS WHERE SM_ID=" + id;
                            break;
                        }
                    case "CHECKSUBMISSIONENTRY":
                        {
                            strSql = "select sc_id from TBL_SUB_CHKLIST inner join tbl_submissions on sub_sc_id=sc_id where SC_SM_ID=" + id;
                            break;
                        }
                    //Added By Milan yadav on 5-Feb-2016
                    case "ADVTById":
                        {
                            //strSql = "select * from TBL_PRP, TBL_PRP_DEPT, TBL_PRODUCT_MASTER where PRP_PRODUCT_ID = PM_ID and PRP_RECEIVED_FROM_ID = PD_ID and PRP_ID=" + id;

                            //strSql = " select * from TBL_ADVT_DEPT_APPROVERS inner join TBL_ADVT_REQUEST_MAS on ADA_ARM_ID=ARM_ID where  ARM_ID=" + id;


                            strSql = "  select Distinct *, CONVERT(VARCHAR(100),ARM_RECEIVED_DATE,106) as [Received_Date], " +
                                       "  CONVERT(VARCHAR(100),APM_PRODUCT_APPROVAL_DATE ,106) as [Approval_Date] " +
                                       "  from [dbo].[TBL_ADVT_REQUEST_MAS] " +
                                       " inner join TBL_ADVT_CATEGORY_MAS on ACM_ID = ARM_ACM_ID " +
                                       " inner join TBL_ADVT_NATURE_MAS on ANM_ID = ARM_ANM_ID " +
                                       " inner join TBL_ADVT_TYPE_MAS on ATM_ID = ARM_ATM_ID " +
                                       " inner join TBL_ADVT_MEDIA_MAS on  AMM_ID = ARM_AMM_ID " +
                                       " inner join TBL_ADVT_DEPT_APPROVERS on ADA_ARM_ID=ARM_ID " +
                                       " inner join TBL_ADVT_LANGUAGE_MAS on ALM_ID = ARM_NAME_OF_LANGUAGE " +
                                       " left outer join TBL_ADVT_PRODUCT_MAS on APM_ID = ARM_NAME_OF_PRODUCT " +
                                       "inner join TBL_ADVT_STATUS_MAS on ASM_CODE = ARM_STATUS where ARM_ID=" + id;

                            break;
                        }

                    case "NewsTickerById":
                        {
                            strSql = "select * from TBL_NEWS_TICKER where NT_ID=" + id;
                            break;
                        }
                    case "DELETENEWSFROMTICKER":
                        {
                            strSql = "Delete from TBL_NEWS_TICKER where NT_ID=" + id;
                            break;
                        }



                    case "CIRCULARDETAILS":
                        {
                            strSql = "select CM_DETAILS FROM TBL_CIRCULAR_MASTER WHERE CM_ID=" + id;
                            break;
                        }

                    case "DELETEREFFILE":
                        {
                            strSql = "DELETE FROM TBL_UPLOADED_FILES WHERE UF_ID=" + id;
                            break;
                        }
                    case "LoadEventAgenda":
                        {
                            strSql = "SELECT EP_NAME, EEM_ID FROM TBL_EVENT_PURPOSE INNER JOIN TBL_EI_EP_MAPPING ON EP_ID = EEM_EP_ID INNER JOIN TBL_EVENT_INSTANCES ON EEM_EI_ID = EI_ID" +
                                        " WHERE EI_ID =" + id;
                            break;
                        }

                    case "readerForId":
                        {
                            strSql = "SELECT EEM_ID FROM TBL_EI_EP_MAPPING WHERE EEM_EI_ID =" + id;
                            break;
                        }
                    case "EVENTPURPOSEBYEPID":
                        {
                            strSql = "SELECT * FROM TBL_EI_EP_MAPPING WHERE EEM_EI_ID =" + id;
                            break;
                        }
                    case "DeleteEiEpMapping":
                        {
                            strSql = "DELETE FROM TBL_EI_EP_MAPPING WHERE EEM_EI_ID=" + id;
                            break;

                        }
                    //<<Modified By Vivek on 23-Jun-2017
                    case "REPORTINGOWNERS":
                        {
                            strSql = "SELECT SRDOM_EMP_NAME, SRDOM_ID, SRDOM_EMAILID FROM  TBL_SUB_SRD_OWNER_MASTER " +
                                    " where SRDOM_SRD_ID=" + id;
                            break;
                        }
                    //>>
                    case "TopicByIssuingAuth":
                        {
                            strSql = "select CAM_NAME,CAM_ID from TBL_CIRCULAR_AREA_MAS where CAM_STATUS='A' AND CAM_CIA_ID=" + id + " order by CAM_NAME";
                            break;
                        }
                    case "TrackedByOwners":
                        {
                            strSql = "select EM_ID, EM_EMPNAME from EmployeeMaster INNER JOIN TBL_SM_OWNERS ON EM_ID = SMO_EM_ID where SMO_SM_ID =" + id;
                            break;
                        }
                    case "TrackedByOwnersAll":
                        {
                            strSql = "SELECT EM_EMPNAME, EM_ID FROM  EmployeeMaster INNER JOIN TBL_EM_STM_MAPPING ON EM_ID = ESM_EM_ID WHERE ESM_STM_ID = (select SM_STM_ID from TBL_SUBMISSIONS_MAS where SM_ID = " + id + " ) and EM_ID not in (select SMO_EM_ID from TBL_SM_OWNERS where SMO_SM_ID = " + id + ")";

                            break;
                        }
                    case "ReportingDeptOwners":
                        {
                            strSql = "select SRDOM_ID, SRDOM_EMP_NAME from TBL_SUB_SRD_OWNER_MASTER INNER JOIN TBL_SM_REPORTING_OWNERS ON SRDOM_ID = SRO_SRDOM_ID where SRO_SM_ID =" + id;
                            break;
                        }
                    case "ReportingDeptOwnersAll":
                        {
                            strSql = "SELECT SRDOM_ID, SRDOM_EMP_NAME from TBL_SUB_SRD_OWNER_MASTER WHERE SRDOM_SRD_ID = (select SM_SRD_ID from TBL_SUBMISSIONS_MAS where SM_ID = " + id + ") and SRDOM_ID not in (select SRO_SRDOM_ID from TBL_SM_REPORTING_OWNERS where SRO_SM_ID = " + id + ")";
                            break;
                        }
                    case "TrackedByDeptBySMID":
                        {
                            strSql = "SELECT STM_ID from TBL_SUB_TYPE_MAS where STM_ID = (select SM_STM_ID from TBL_SUBMISSIONS_MAS where SM_ID = " + id + ")";
                            break;
                        }
                    case "ReportingDeptBySMID":
                        {
                            strSql = "SELECT SRD_ID from TBL_SUB_REPORTING_DEPT where SRD_ID = (select SM_SRD_ID from TBL_SUBMISSIONS_MAS where SM_ID = " + id + ")";
                            break;
                        }


                    case "CERTCONTENT":
                        {
                            strSql = "SELECT CERTM_TEXT FROM TBL_CERT_MAS WHERE  CERTM_ID =" + id;
                            break;
                        }
                    //Added By Milan yadav on 24-Jun-2016
                    //>>
                    case "DeleteCertificationFiles":
                        {
                            strSql = "delete from TBL_CCM_FILES where CCMF_ID =" + id;
                            break;
                        }
                    //<<

                    case "REASONFOREDIT":
                        {
                            strSql = "select RFE_ID,RFE_MODULE_ID,RFE_RECORD_ID, RFE_REASON_FOR_EDIT, " +
                                     "RFE_CREATE_BY, RFE_CREATE_DT FROM TBL_REASON_FOR_EDIT where RFE_RECORD_ID =" + id;
                            break;
                        }



                    case "deleteComplaintsDoc":
                        {
                            strSql = "DELETE FROM TBL_COMPLAINT_DOCS WHERE CD_ID= " + id + "";
                            break;
                        }

                    case "deleteCertException":
                        {
                            strSql = "DELETE FROM TBL_CERT_EXCEPTION WHERE CE_ID= " + id + "";
                            break;
                        }
                    case "getExceptionByCertId":
                        {
                            strSql = "select * FROM TBL_CERT_EXCEPTION WHERE CE_CERT_ID= " + id + "";
                            break;
                        }
                    case "getCertException":
                        {
                            strSql = "select * FROM TBL_CERT_EXCEPTION WHERE CE_ID= " + id + "";
                            break;
                        }
                    //<< Added by Nikhil Adhalikar on 20-Sep-2011
                    case "GETCOMMONCERTIFICATIONS":
                        {
                            strSql = "SELECT distinct CDM_NAME, TBL_CERTIFICATIONS.* FROM " +
                                    " TBL_CERTIFICATIONS INNER JOIN TBL_CERT_MAS ON " +
                                    " CERT_CERTM_ID = CERTM_ID " +
                                    " INNER JOIN TBL_CERT_DEPT_MAS on " +
                                    " CERTM_DEPT_ID=CDM_ID AND CDM_NAME != 'Common'" +
                                    " AND CERT_ID = " + id;
                            break;
                        }
                    //>>


                    //>>
                    //>>
                    //<<Added by Prajakta on 04-Jun-2012
                    case "getOutwardById":
                        {
                            strSql = "select *,  replace(CONVERT(VARCHAR,OT_DATE ,106),' ','-') AS OTDate  from TBL_OUTWARD_TRACKING " +
                            " inner join TBL_OUTWARD_REG_AUTH_MAS " +
                            " on ORAM_ID = OT_REG_AUTH inner join TBL_OUTWARD_DEPT_MAS on ODM_ID = OT_DEPT_ID " +
                            " inner join TBL_OUTWARD_TYPE_MASTER on OTM_ID = OT_TYPE_ID " +
                            " where OT_ID=" + id;
                            break;
                        }

                    case "getOutwardFilesByOTId":
                        {
                            strSql = "select * from TBL_OUTWARD_FILES where OF_OT_ID=" + id;
                            break;
                        }
                    case "deleteOutwardFileById":
                        {
                            strSql = "delete from TBL_OUTWARD_FILES where OF_ID=" + id;
                            break;
                        }
                    case "getActionablesById":
                        {
                            strSql = "select *,  replace(CONVERT(VARCHAR,CA_TARGET_DATE ,106),' ','-') AS TargetDate, " +
                                "replace(CONVERT(VARCHAR,CA_COMPLETION_DATE ,106),' ','-') AS CompletionDate  from TBL_COMPLIANCE_ACTIONABLES where CA_ID=" + id;
                            break;
                        }
                    case "getActionableFilesByCAId":
                        {
                            strSql = "select * from TBL_ACTIONABLE_FILES where AF_CA_ID=" + id;
                            break;
                        }
                    case "deleteActionableFileById":
                        {
                            strSql = "delete from TBL_ACTIONABLE_FILES where AF_ID=" + id;
                            break;
                        }
                    case "ClientCityByClientState":
                        {
                            strSql = "select CM_NAME,CM_ID from TBL_CITY_MASTER where CM_SM_ID=" + id + " order by CM_NAME";
                            break;
                        }


                    //<<Added By Bhavik 10-sep-2013
                    case "getChecklistById":
                        {
                            strSql = "SELECT *,Replace(CONVERT(VARCHAR(11), CCM_EFFECTIVE_FROM, 106), ' ', '-') as [Effective From]," +
                                     "Replace(CONVERT(VARCHAR(11), CCM_EFFECTIVE_TO, 106), ' ', '-') as [Effective To] " +
                                     " FROM TBL_CERT_CHECKLIST_MAS " +
                                     "inner join  TBL_CERT_DEPT_MAS on CDM_ID = CCM_CDM_ID " +
                                     "Where CCM_ID=" + id + "";
                            break;
                        }

                    case "DeleteComplianceChecklist":
                        {
                            //strSql = "delete from TBL_CERT_CHECKLIST_MAS where CCM_ID = " + id;
                            strSql = "update TBL_CERT_CHECKLIST_MAS set CCM_STATUS ='I' where CCM_ID = " + id;
                            break;
                        }


                    case "getChecklistByCertId":
                        {
                            strSql = "select CCM_ID as [ID], RC_NAME as [Compliance_Status],  " +
                                    " Replace(CONVERT(VARCHAR(11), CCM_EFFECTIVE_FROM, 106), ' ', '-') as [Effective From], " +
                                    " * from TBL_CERT_CHECKLIST_DETS " +
                                    " inner join TBL_CERT_CHECKLIST_MAS on CCM_ID = CCD_CCM_ID and CCD_CERT_ID='" + id + "' " +
                                    " inner join TBL_CERTIFICATIONS on CERT_ID = CCD_CERT_ID " +
                                    " left outer join TBL_REF_CODES on CCD_YES_NO_NA = RC_CODE and RC_TYPE='Certification Compliance Status' " +
                                    " order by CCM_SR_NO";

                            break;
                        }
                    //>>                  



                    //<< Added by Supriya on 19-Jan-2015
                    case "getCircularActionableById":
                        {
                            strSql = "select RC_NAME as [Status] , * from TBL_CIRCULAR_ACTIONABLES " +
                                " inner join TBL_CIRCULAR_PERSON_RESPONSIBLE_MAS on CA_PERSON_RESPONSIBLE  = CPRM_ID  " +
                                " inner join TBL_REF_CODES on RC_CODE = CA_STATUS and RC_TYPE='Actionable Status' " +
                                " where CA_CM_ID = " + id;
                            break;
                        }

                    //<< Added by Narendra Naidu on 27-Mar-2015 for deleting files
                    case "deleteSubmissionFilesById":
                        {
                            strSql = "delete from TBL_SUB_MAS_FILES where SMF_ID=" + id;
                            break;
                        }
                    //Added by Supriya on 11-Aug-2015
                    case "getAllChecklistByCertId":
                        {
                            strSql = "select CCM_ID as [ID], RC_NAME as [Compliance_Status], *," +
                                    "Replace(CONVERT(VARCHAR(11), CCM_EFFECTIVE_FROM, 106), ' ', '-') as [Effective From] " +
                                    "from TBL_CERT_CHECKLIST_DETS " +
                                    "inner join TBL_CERT_CHECKLIST_MAS on CCM_ID = CCD_CCM_ID " +
                                    " inner join TBL_REF_CODES on CCD_YES_NO_NA = RC_CODE and RC_TYPE='Certification Compliance Status' " +
                                    " where CCM_CSSDM_ID in (select CSSDM_ID from " +
                                    "  TBL_CERT_SUB_SUB_DEPT_MAS " +
                                    "  inner join TBL_CERT_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID " +
                                    "  inner join TBL_CERT_DEPT_MAS on CSDM_CDM_ID = CDM_ID  " +
                                    "  where CDM_ID in ( select CDM_ID  " +
                                    "  from TBL_CERTIFICATIONS inner join  " +
                                    "  TBL_CERT_SUB_SUB_DEPT_MAS on CERT_CSSDM_ID= CSSDM_ID " +
                                    "  inner join TBL_CERT_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID " +
                                    "  inner join TBL_CERT_DEPT_MAS on CSDM_CDM_ID = CDM_ID where CERT_ID= " + id + ")) order by CCM_SR_NO ";
                            break;
                        }


                    case "getSubSubCertDetsByCertSubId":
                        {
                            strSql = "select * from  TBL_CERT_SUB_SUB_DEPT_MAS " +
                                    " inner join TBL_CERT_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID " +
                                    " where CSDM_ID= " + id;
                            break;
                        }
                    //Added BY Milan Yadav on 24-Jun-2016
                    //>>
                    case "GetCertChecklistDetailFileById":
                        {
                            strSql = "select * from TBL_CCM_FILES where CCMF_CCM_ID in (" + id + ")";
                            break;
                        }
                    //<< Added by Supriya On 10-Jun-2017

                    case "getChecklistByCertIdForView":
                        {
                            strSql = "select CCM_ID as [ID], RC_NAME as [Compliance_Status],*, " +
                                    " CSSDM_NAME as DeptName, " +
                                    " Replace(CONVERT(VARCHAR(11), CCM_EFFECTIVE_FROM, 106), ' ', '-') as [Effective From] " +
                                    " from TBL_CERT_CHECKLIST_DETS " +
                                    " inner join TBL_CERT_CHECKLIST_MAS on CCM_ID = CCD_CCM_ID and CCD_CERT_ID='" + id + "' " +
                                    " inner join TBL_CERT_SUB_SUB_DEPT_MAS on CCM_CSSDM_ID = CSSDM_ID " +
                                    " left outer join TBL_REF_CODES on CCD_YES_NO_NA = RC_CODE and RC_TYPE='Certification Compliance Status' " +
                                    " order by CCM_SR_NO";

                            break;
                        }
                    //>>
                    //<< Added on 05-May-2020
                    case "Owners1":
                        {
                            strSql = "SELECT * from EmployeeMaster, TBL_LEG_OWNER_MAPPING WHERE EM_ID = LOM_EM_ID AND LOM_LM_ID= " + id + " order by EM_EMPNAME ";
                            break;
                        }
                    case "Owners2":
                        {
                            strSql = "SELECT * from EmployeeMaster, TBL_LIT_OWNER_MAPPING WHERE EM_ID = LGOM_EM_ID AND LGOM_LGM_ID= " + id + " order by EM_EMPNAME ";
                            break;
                        }

                    case "DELETECONTDETAILS":
                        {
                            strSql = "DELETE FROM TBL_CONTRACT_TEMPLATE WHERE CT_ID=" + id;
                            break;
                        }
                    case "DELETESECDETAILS":
                        {
                            strSql = "DELETE FROM TBL_SEC_DOCS WHERE SD_ID=" + id;
                            break;
                        }
                    case "DELETEMISCCONTENTS":
                        {
                            strSql = "DELETE FROM TBL_MISC_CONTENT WHERE MC_ID=" + id;
                            break;
                        }
                    case "DELETEUPDATE":
                        {
                            strSql = "DELETE FROM TBL_COMP_LITIGATION_UPDATES WHERE CLU_ID=" + id + "; DELETE FROM TBL_COMP_LIT_UPDATE_FILES WHERE CLUF_CLU_ID=" + id;
                            break;
                        }
                    case "DELETELGLUPDATE":
                        {
                            strSql = "DELETE FROM TBL_LGL_LTGN_UPDATES WHERE LLU_ID=" + id + "; DELETE FROM TBL_LGL_LTGN_UPDATES_FILES WHERE LLUF_LLU_ID=" + id;
                            break;
                        }
                    case "getKeyLearnings":
                        {
                            strSql = "SELECT CLGM_KEY_LEARNINGS from TBL_COMP_LITIGATION_MASTER where CLGM_ID = " + id;
                            break;

                        }
                    case "CompLegalNoticeUpdateByID":
                        {
                            strSql = "select * from TBL_COMP_LEGAL_NOTICE_UPDATES where LNU_ID = " + id;
                            break;
                        }
                    case "DeleteCompLegalNoticeUpdate":
                        {
                            strSql = "delete from TBL_COMP_LEGAL_NOTICE_UPDATES where LNU_ID = " + id + "; DELETE FROM TBL_COMP_LEGALNOTICE_UPDATE_FILES WHERE LUF_LNU_ID=" + id;
                            break;
                        }

                    case "CompLegalNoticeUpdateFileByUpdateId":
                        {
                            strSql = "SELECT LUF_ID, LUF_LNU_ID, LUF_FILE_NAME as FileName, LUF_FILE_NAME_ON_SERVER as FileNameOnServer, LUF_CREATE_BY as UserName, LUF_CREATE_DATE as UploadDatetime FROM TBL_COMP_LEGALNOTICE_UPDATE_FILES WHERE LUF_LNU_ID = " + id;
                            break;
                        }
                    case "DeleteCompLitUpdateFiles":
                        {
                            strSql = "delete from TBL_COMP_LIT_UPDATE_FILES where CLUF_ID = " + id;
                            break;
                        }
                    case "DeleteCompLegalNoticeUpdateFiles":
                        {
                            strSql = "delete from TBL_COMP_LEGALNOTICE_UPDATE_FILES where LUF_ID = " + id;
                            break;
                        }
                    case "DeleteLegalLitUpdateFiles":
                        {
                            strSql = "delete from TBL_LGL_LTGN_UPDATES_FILES where LLUF_ID = " + id;
                            break;
                        }
                    case "LGLLegalNoticeUpdateByID":
                        {
                            strSql = "select * from TBL_LGL_NOTICE_UPDATES where LNU_ID = " + id;
                            break;
                        }
                    case "DeleteLGLLegalNoticeUpdate":
                        {
                            strSql = "delete from TBL_LGL_NOTICE_UPDATES where LNU_ID = " + id + "; DELETE FROM TBL_LGL_NOTICE_UPDATE_FILES WHERE LNUF_LNU_ID=" + id;
                            break;
                        }
                    case "DeleteLGLLegalNoticeUpdateFiles":
                        {
                            strSql = "DELETE FROM TBL_LGL_NOTICE_UPDATE_FILES WHERE LNUF_ID= " + id;
                            break;
                        }
                    case "ContractFiles":
                        {
                            strSql = "select * from TBL_CONTRACT_FILES WHERE CONF_CON_ID= " + id;
                            break;
                        }
                    case "DeleteContractFiles":
                        {
                            strSql = "delete from TBL_CONTRACT_FILES WHERE CONF_ID= " + id;
                            break;
                        }
                    case "DeleteContract":
                        {
                            strSql = "delete from TBL_CONTRACT_FILES WHERE CONF_ID= " + id + ";delete from TBL_CONTRACT_MAS WHERE CON_ID= " + id;
                            break;
                        }
                    case "getHelpDeskById":
                        {
                            strSql = "SELECT HD_HCM_ID, HCM_NAME," +
                        "HD_ID, HD_REQUESTOR_NAME, HD_REQUESTOR_EMAIL, HD_QUESTION, HD_STATUS, HD_AUDIT_TRAIL, " +
                        "case when HD_STATUS='S' then 'Submitted' end as Status, " +
                        " HD_CREATE_BY, HD_CREATE_DT  FROM TBL_HELP_DESK INNER JOIN " +
                        "TBL_HELPDESK_CATEGORY_MAS ON HD_HCM_ID = HCM_ID " +
                        " where HD_ID = " + id;
                            break;
                        }
                    case "IRDAFiles":
                        {
                            strSql = "SELECT * FROM TBL_IRDA_FILES WHERE IRDAF_IC_ID = " + id + "";
                            break;
                        }


                    //<< Added by Prajakta on 21-May-2012
                    case "getUpdByFCId":
                        {
                            strSql = "SELECT TBL_FRAUD_CASE_UPDATES.*, CASE WHEN FCU_STATUS = 'O' " +
                                    " THEN 'Open' when FCU_STATUS = 'C' then 'Closed' " +
                                    " else '' end as Status, CASE WHEN FCU_ACCEPTED_REJECTED = 'A' " +
                                    " THEN 'Accepted' when FCU_ACCEPTED_REJECTED = 'R' then 'Rejected' " +
                                    " else '' end as AccRej " +
                                    " FROM TBL_FRAUD_CASE_UPDATES INNER JOIN " +
                                    " TBL_FRAUD_CASE_MAS ON FCM_ID = FCU_FCM_ID AND FCU_REC_STATUS = 'A' " +
                                    " AND FCM_ID = " + id;
                            break;
                        }
                    //>>

                    //<< Added by Bhavik on 26-Nov-2014
                    case "getSubmisssionMasFiles":
                        {
                            strSql = "select *, RC_NAME as [FileType] from TBL_SUB_MAS_FILES " +
                                " left outer join TBL_REF_CODES on SMF_FILE_TYPE = RC_CODE and RC_TYPE='Submisssion File Type' and RC_STATUS = 'A'" +
                                " where SMF_SM_ID= " + id + " ";
                            break;
                        }

                    case "getSubmisssionMasFiles_Edit":
                        {
                            strSql = "select SMF_ID, RC_NAME as [File Type],SMF_FILE_DESC as [File Description],SMF_FILE_TYPE as [FileTypeShortForm], " +
                                " SMF_FILE_NAME as [FileName],SMF_SERVER_FILE_NAME as [FileNameOnServer],SMF_CREATE_BY as [Uploaded By], " +
                                " SMF_CREATE_DT as [Uploaded On] from TBL_SUB_MAS_FILES " +
                                " left outer join TBL_REF_CODES on SMF_FILE_TYPE = RC_CODE and RC_TYPE = 'Submisssion File Type' and RC_STATUS = 'A'" +
                                " where SMF_SM_ID= " + id + " ";
                            break;
                        }

                    case "getVendorMasterById":
                        {
                            strSql = "select * from  TBL_CON_VENDOR_MASTER " +
                                    " where CVM_ID= " + id;
                            break;
                        }
                    //<<

                    //<<Modified by Ashish Mishra on 22Jul2017 (added CDO_OWNER_TYPE filter)
                    case "CONTRACTDEPTOWNERS":
                        {
                            strSql = "select * from TBL_CON_DEPT_MAS inner join TBL_CON_DEPT_OWNERS on CDM_ID =CDO_CDM_ID " +
                                     "where CDO_OWNER_TYPE = 'SPOC' and CDM_ID = " + id + "";
                            break;
                        }
                    //>>
                    case "getDirectorDetails":
                        {
                            strSql = "select * from TBL_CON_VENDOR_RELATED_PARTY_DETS inner join TBL_CON_VENDOR_MASTER " +
                                     "on CVRPD_CVM_ID = CVM_ID " +
                                     " where CVRPD_CVM_ID= " + id;
                            break;
                        }
                    //Added by Abhimanyu Singh on 06Sep2016
                    case "getContractMas":
                        {
                            strSql = "select * FROM TBL_CONTRACT_MAS " +
                                      "LEFT OUTER JOIN TBL_CON_VENDOR_MASTER on CON_VENDOR_NAME = CVM_ID ";
                            break;
                        }

                    //Added by Milan Yadav on 24-Nov-216
                    case "getContractOtherDepartmentApprover":
                        {
                            strSql = "select * from TBL_CON_DEPT_MAS where CDM_ID in (" + id + ")";
                            break;
                        }
                    case "getApprovalRequiredFrom":
                        {
                            strSql = "select * from TBL_CT_APPROVING_DEPT " +
                                     " inner join TBL_CON_DEPT_MAS on CAD_DEPT_ID = CDM_ID where CAD_CT_ID = " + id + "";
                            break;
                        }
                    //<<Added by Hari 30Dec2016
                    case "REFERENCEDEPTOWNER":
                        {
                            strSql = "select * from  TBL_CON_DEPT_MAS " +
                                     "inner join TBL_CON_DEPT_OWNERS on CDM_ID = CDO_CDM_ID " +
                                     "where CDM_IS_REF_DEPT ='Y' and CDM_ID =" + id + "";
                            break;
                        }
                    //>>
                    //Added By Milan Yadav on 10-Jan-2017
                    //<<
                    case "ReferenceDeptApprovalFiles":
                        {
                            strSql = "SELECT TBL_DC_APPROVING_DEPT_FILES.* FROM TBL_DC_APPROVING_DEPT_FILES WHERE DADF_DAD_ID = " + id + "";
                            break;
                        }

                    case "DeleteReferenceDeptApprovalFiles":
                        {
                            strSql = "DELETE FROM TBL_DC_APPROVING_DEPT_FILES WHERE DADF_ID = " + id + "";
                            break;
                        }
                    //>>

                    //<<Added By Milan Yadav on 22-Feb-2017
                    case "ContractExecutionResponseFiles":
                        {
                            strSql = "SELECT * FROM TBL_CON_EXECUTION_DETS_FILES WHERE CEDF_CED_ID = " + id + "";
                            break;
                        }
                    //>>

                    //<<Added By Milan Yadav on 22-Feb-2017
                    case "getContractExecutionDetails":
                        {
                            strSql = "SELECT * FROM TBL_CON_EXECUTION_DETS inner join TBL_DRAFT_CONTRACTS on DC_ID = CED_DC_ID  WHERE CED_ID = " + id + "";
                            break;
                        }
                    //>>
                    case "getContractDetsById":
                        {
                            strSql = "Select * FROM TBL_CONTRACT_TEMPLATE WHERE CT_ID = " + id;
                            break;
                        }
                    case "ContractDraftResponseFiles":
                        {
                            strSql = "SELECT * FROM TBL_CONTRACT_DRAFT_RESPONSE_FILES WHERE CDRF_CDR_ID = " + id + "";
                            break;
                        }
                    //<< Added By Ashish on 08-Jun-2017
                    case "ContractFieldDetailsbyDCId":
                        {
                            strSql = " SELECT * FROM TBL_CONTRACT_TEMP_PARAMS " +
                                    " INNER JOIN TBL_DRAFT_CONTRACT_DETS ON CTP_ID = DCD_CTP_ID " +
                                    " INNER JOIN TBL_CON_MNGMT_FIELDS ON CMF_ID = CTP_CMF_ID " +
                                    " INNER JOIN TBL_DRAFT_CONTRACTS ON DCD_DC_ID = DC_ID " +
                                    " WHERE DC_ID = " + id;
                            break;
                        }
                    //>>
                    //<< Added by Ashish on 22-Jun-2017
                    case "ChecklistFiles":
                        {
                            strSql = "SELECT * FROM TBL_CON_CHECKLIST_DETS_FILES " +
                                     " inner join TBL_REF_CODES on CCDF_FILE_TYPE = RC_CODE and RC_TYPE='Contract Checklist Files' " +
                                     " WHERE CCDF_CPCD_ID = " + id;
                            break;
                        }

                    case "DeleteChecklistFiles":
                        {
                            strSql = "DELETE FROM TBL_CON_CHECKLIST_DETS_FILES WHERE CCDF_ID = " + id + "";
                            break;
                        }
                    //>>

                    //<<Added by Ashish on 28-Jun-2017
                    case "getApproverDetails":
                        {
                            strSql = "select * from TBL_CT_APPROVING_DEPT inner join TBL_CONTRACT_TEMPLATE " +
                                     "on CT_ID = CAD_CT_ID inner join TBL_CON_DEPT_MAS on CAD_DEPT_ID = CDM_ID " +
                                     " where CAD_CT_ID= " + id;
                            break;
                        }
                    //>>

                    //<< Added by Ashish on 29-Jun-2017
                    case "getContractMailReference":
                        {
                            strSql = "select CDO_EMP_EMAIL_ID from [TBL_DRAFT_CONTRACTS] " +
                                     " inner join [TBL_CON_DEPT_MAS] on CDM_ID = [DC_PENDING_WITH_FUNCTION] " +
                                     " inner join [TBL_CON_DEPT_OWNERS] on [CDO_CDM_ID] = [CDM_ID] and CDO_OWNER_TYPE = [DC_PENDING_WITH_APPROVER] " +
                                     " and DC_ID = " + id;
                            break;
                        }
                    //>>
                    //<<Modified By Vivek on 03-Mar-2018
                    case "REPORTINGOWNERS_AllLevels":
                        {
                            strSql = " select UserName,EmailId from " +
                                       " (SELECT SRDOM_EMP_NAME as UserName,SRDOM_EMAILID as EmailId From " +
                                       " TBL_SUB_SRD_OWNER_MASTER " +
                                       " INNER JOIN TBL_SUB_REPORTING_DEPT on " +
                                       " SRD_ID = SRDOM_SRD_ID " +
                                       " where SRDOM_STATUS='A' AND SRD_STATUS='A' AND SRD_ID = " + id +
                                       " union " +
                                       " select SE_FIRST_NAME as UserName,SE_EMAIL_ID as EmailId from TBL_SUB_ESCALATION " +
                                       " where SE_STATUS='A' AND [SE_SRD_ID ]= " + id + ") as tab ";
                            break;
                        }
                    //>>

                    //<< Added By Vivek on 28-Dec-2017

                    case "LoadCircularActionableFileList":
                        {
                            strSql = "SELECT * FROM TBL_CIRCULAR_ACTIONABLES_FILES WHERE CAF_CA_ID = " + id + "";
                            break;
                        }

                    case "getCTApprovingDept":
                        {
                            strSql = "select * from [TBL_CT_APPROVING_DEPT] where CAD_CT_ID= " + id;
                            break;
                        }
                    case "ContractEscalations":
                        {
                            strSql = "select * from TBL_CON_ESCALATION_DETS where CED_CON_ID = " + id;
                            break;
                        }
                    //>>
                    case "getCertIdWiseCertification":
                        {
                            strSql = " SELECT distinct CSSDM_NAME, * FROM  " +
                                    " TBL_CERTIFICATIONS " +
                                    " inner join TBL_CERT_MAS  on CERTM_ID=CERT_CERTM_ID" +
                                    " INNER JOIN TBL_CERT_SUB_SUB_DEPT_MAS ON  " +
                                    " CERTM_DEPT_ID = CSSDM_ID and CERTM_LEVEL_ID= 0 " +
                                    " inner join TBL_CERT_QUARTER_MAS on CERT_CQM_ID =CQM_ID" +
                                    " where CERT_ID = " + id;
                            break;
                        }
                    case "HelpDeskOwner":
                        {
                            strSql = "SELECT * FROM TBL_HELPDESK_OWNERS where HO_HDM_ID = " + id + " ORDER BY HO_EMPNAME ";
                            break;
                        }
                    case "HelpDeskLegalOwner":
                        {
                            strSql = "SELECT * FROM TBL_HD_ALLOCATIONS where HA_HD_ID = " + id;
                            break;
                        }
                    case "HelpDeskOwnerDetsById":
                        {
                            strSql = "SELECT * FROM TBL_HELPDESK_OWNERS where HO_ID = " + id;
                            break;
                        }
                        //>> End   
                }
                using (F2FDatabase DB = new F2FDatabase(strSql))
                {
                    DB.F2FDataAdapter.Fill(resultDataSet);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return resultDataSet;
        }

        public string roleMenuMapping(string strUser, int iCmpId, string sPagePath, string mstrConnectionString = null)
        {
            try
            {

                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "getMenuProductDetails";
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Username", F2FDatabase.F2FDbType.VarChar, strUser));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CompanyId", F2FDatabase.F2FDbType.Int32, iCmpId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@PagePath", F2FDatabase.F2FDbType.VarChar, sPagePath));

                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;

                    DbParameter prmMenuString = (DbParameter)F2FDatabase.F2FParameter("@MenuString", F2FDatabase.F2FDbType.VarChar, "", ParameterDirection.Output, -1);
                    //DbParameter prmLevelMenuCount = F2FDatabase.F2FParameter("@Level1MenuCount", F2FDatabase.F2FDbType.Int32, 0, ParameterDirection.Output);
                    DB.F2FCommand.Parameters.Add(prmMenuString);
                    //DB.F2FCommand.Parameters.Add(prmLevelMenuCount);
                    DB.OpenConnection();
                    DB.F2FCommand.ExecuteReader();
                    if (string.IsNullOrEmpty(HttpContext.Current.Session["ProductFormalName"] as string))
                    {
                        HttpContext.Current.Session["ProductFormalName"] = prmMenuString.Value.ToString().Split('~')[1];
                    }
                    return Convert.ToString(prmMenuString.Value.ToString().Split('~')[0]);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                return null;
            }
        }

        public static DataTable SetProductName(string sPagePath)
        {
            DataTable dtProduct = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    string sSql = "select * FROM TBL_PRODUCT_MAS "
                        + "JOIN TBL_PRODUCT_MENU_MAPPING on PM_ID = PMM_PM_ID"
                        + " JOIN TBL_MENU_MAS ON MM_ID = PMM_MM_ID"
                        //+ " inner join tbl_company_products on CP_PM_ID = PM_ID"
                        + " where mm_html = @PagePath";
                    //+ " and cp_cmp_id = @CompanyId and tbl_product_mas.status = 'y';";
                    DB.F2FCommand.CommandText = sSql;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@PagePath", F2FDatabase.F2FDbType.VarChar, sPagePath));
                    DB.F2FDataAdapter.Fill(dtProduct);
                    if (dtProduct != null && dtProduct.Rows.Count != 0)
                    {
                        HttpContext.Current.Session["ProductFormalName"] = dtProduct.Rows[0]["PM_FORMAL_NAME"];
                    }
                    else
                    {
                        HttpContext.Current.Session["ProductFormalName"] = null;
                    }

                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return dtProduct;
        }


        //<< Added on 04-May-2020 For Certification, Filing, Circular

        public DataSet getDataset(string code, string conStr)
        {
            DataSet resultDataSet = new DataSet();
            try
            {
                string strSql = "";
                switch (code)
                {
                    case "DashBoardMaster":
                        {
                            strSql = "select * From TBL_DASHBOARD_MASTER order by DM_NAME";
                            break;
                        }
                    case "ReportMaster":
                        {
                            strSql = "select * From TBL_REPORT_MASTER order by RM_NAME";
                            break;
                        }

                    case "SUBMISSIONSGMT":
                        {
                            //<<Modified by Ankur Tyagi on 17-Apr-2025 for Project Id : 2395
                            //strSql = "select SSM_ID,SSM_NAME from TBL_SUBMISSION_SEGMENT_MAS where SSM_STATUS='A' order by SSM_NAME ";
                            strSql = "select SSM_ID,SSM_NAME from TBL_SUBMISSION_SEGMENT_MAS where SSM_STATUS='A' order by SSM_NAME ASC ";
                            //>>
                            break;
                        }

                    case "SUBMISSIONSGMT_EDIT":
                        {
                            //<<Modified by Ankur Tyagi on 17-Apr-2025 for Project Id : 2395
                            //strSql = "select * from TBL_SUBMISSION_SEGMENT_MAS order by SSM_NAME ";
                            strSql = "select * from TBL_SUBMISSION_SEGMENT_MAS order by SSM_NAME ASC ";
                            //>>
                            break;
                        }

                    case "TYPE":
                        {
                            strSql = "select STM_ID,STM_TYPE from TBL_SUB_TYPE_MAS order by STM_TYPE ";
                            break;
                        }

                    case "TYPE_EDIT":
                        {
                            strSql = "select * from TBL_SUB_TYPE_MAS order by STM_TYPE ";
                            break;
                        }

                    case "DEPT":
                        {
                            strSql = "select CDM_ID ,CDM_NAME  from TBL_CIRCULAR_DEPT_MAS order by CDM_NAME ";
                            break;
                        }

                    case "ACTIVE_DEPT":
                        {
                            strSql = "select CDM_ID ,CDM_NAME  from TBL_CIRCULAR_DEPT_MAS WHERE CDM_STATUS = 'A' order by CDM_NAME ";
                            break;
                        }

                    case "EVENT":
                        {
                            strSql = "SELECT EM_ID, EM_EVENT_NAME FROM TBL_EVENT_MAS WHERE EM_STATUS='A' ORDER BY EM_EVENT_NAME";
                            break;
                        }

                    case "EVENT_EDIT":
                        {
                            strSql = "SELECT * FROM TBL_EVENT_MAS ORDER BY EM_EVENT_NAME";
                            break;
                        }

                    case "Zone":
                        {
                            strSql = "SELECT * FROM  TBL_ZONE_MASTER ";
                            break;
                        }
                    case "FINYEAR":
                        {
                            strSql = "SELECT FYM_ID,FYM_FIN_YEAR FROM TBL_FIN_YEAR_MAS WHERE FYM_STATUS='A' ORDER BY FYM_FIN_YEAR";
                            break;
                        }

                    case "SUBTYPE":
                        {
                            strSql = "Select * from TBL_SUB_TYPE_MAS Where STM_STATUS='A' ORDER BY STM_TYPE ";
                            break;
                        }

                    case "SUBTYPE_EDIT":
                        {
                            strSql = "Select * from TBL_SUB_TYPE_MAS ORDER BY STM_TYPE ";
                            break;
                        }

                    case "EscLevel2":
                        {
                            strSql = "SELECT CASE when se_type=1 then 'Filing' when se_type=2 then 'PRP' end as 'Escalation Type',[SE_ID],[STM_TYPE],[SE_CM_ID],[SE_FIRST_NAME],[SE_MIDDEL_NAME],[SE_LAST_NAME], SE_EMAIL_ID,SE_LEVEL FROM TBL_SUB_ESCALATION INNER JOIN TBL_SUB_TYPE_MAS ON SE_STM_ID=STM_ID where [SE_LEVEL]=2 order by STM_TYPE";
                            break;
                        }
                    case "EscLevel1":
                        {
                            strSql = "SELECT CASE when se_type=1 then 'Filing' when se_type=2 then 'PRP' end as 'Escalation Type', [SE_ID],[STM_TYPE],[SE_CM_ID],[SE_FIRST_NAME],[SE_MIDDEL_NAME],[SE_LAST_NAME], SE_EMAIL_ID,SE_LEVEL FROM TBL_SUB_ESCALATION INNER JOIN TBL_SUB_TYPE_MAS ON SE_STM_ID=STM_ID where [SE_LEVEL]=1 order by STM_TYPE";
                            break;
                        }

                    case "GETEVENTPURPOSE":
                        {
                            strSql = "SELECT * FROM TBL_EVENT_PURPOSE ORDER BY EP_NAME";
                            break;
                        }
                    case "REPORTINGDEPT":
                        {
                            strSql = "select * from TBL_SUB_REPORTING_DEPT where SRD_STATUS='A' order by SRD_NAME";
                            break;
                        }

                    case "REPORTINGDEPT_EDIT":
                        {
                            strSql = "select * from TBL_SUB_REPORTING_DEPT order by SRD_NAME";
                            break;
                        }

                    case "AllFinYears":
                        {
                            strSql = "SELECT FYM_ID,FYM_FIN_YEAR, FYM_STATUS FROM TBL_FIN_YEAR_MAS  ORDER BY FYM_FIN_YEAR DESC ";
                            break;
                        }

                    case "ReportingDeptOwners":
                        {
                            strSql = "SELECT SRDOM_EMP_NAME, SRDOM_ID FROM  TBL_SUB_SRD_OWNER_MASTER ORDER BY SRDOM_EMP_NAME";
                            break;
                        }
                    case "Roles":
                        {
                            strSql = "select RoleId,RoleName from aspnet_Roles where ([Description]!= 'Taxation' or [Description]  is null) order by RoleName ";
                            break;
                        }

                    case "AllFinYears1":
                        {
                            strSql = "SELECT FYM_ID as Id,FYM_FIN_YEAR as Name FROM TBL_FIN_YEAR_MAS  ORDER BY FYM_FIN_YEAR ";
                            break;
                        }

                    //// Added By Supriya on 14 Dec 2012
                    //case "getReferences":
                    //    {
                    //        strSql = "Select DISTINCT RC_TYPE from TBL_REF_CODES WHERE RC_STATUS = 'A' " +
                    //                " ORDER BY RC_TYPE";
                    //        break;
                    //    }

                    case "getConfigParams":
                        {
                            strSql = " SELECT * from TBL_CONFIG_PARAMS order by CP_SORT_ORDER ";
                            break;
                        }

                    //Added By Milan Yadav on 10-Aug-2016
                    //>>
                    case "ContractVendor":
                        {
                            strSql = "select CDM_ID, CDM_NAME from TBL_CON_DEPT_MAS where CDM_STATUS='A' order by CDM_NAME";
                            break;
                        }
                    //<<
                    //Added By Milan Yadav on 12-Aug-2016
                    //>>
                    case "getDirectorDetails":
                        {
                            strSql = "select * from TBL_CON_KMP_DIRECTORS where STATUS='A' order by CKD_NAME";
                            break;
                        }
                    //<<
                    //<< Added By Milan Yadav on 21-Feb-2017
                    case "getFinalDepartmentApprovalStatusChiefManager":
                        {
                            strSql = "select * from TBL_REF_CODES where RC_TYPE ='Contract Final Department Status Chief Manager' and RC_STATUS='A' order by RC_NAME";
                            break;
                        }
                    case "getFinalDepartmentApprovalStatusLegalHead":
                        {
                            strSql = "select * from TBL_REF_CODES where RC_TYPE ='Contract Final Department Status  Legal Head' and RC_STATUS='A' order by RC_NAME";
                            break;
                        }
                    //>>
                    case "getParamType":
                        {
                            strSql = "select * from TBL_REF_CODES where RC_TYPE ='Parameter Type' and RC_STATUS='A' order by RC_NAME";
                            break;
                        }

                    //<< Modified by Archana Gosavi on 02-Jun-2016
                    case "ContractDept":
                        {
                            strSql = "select CDM_ID, CDM_NAME from TBL_CON_DEPT_MAS where CDM_STATUS='A' order by CDM_NAME";
                            break;
                        }
                    //>>
                    case "ContractClassification":
                        {
                            strSql = "select CCM_ID, CCM_NAME from TBL_CON_CLASSIFICATION_MAS order by CCM_NAME";
                            break;
                        }
                    case "ContractAuthSign":
                        {
                            strSql = "select CSM_ID, CSM_SIGNATORY from TBL_CON_SIGNATORY_MAS where CSM_STATUS='A' order by CSM_SIGNATORY";
                            break;
                        }
                    case "ContractAuthSign1":
                        {
                            strSql = "select CSM_ID, CSM_SIGNATORY from TBL_CON_SIGNATORY_MAS order by CSM_SIGNATORY";
                            break;
                        }
                    case "ContractNo":
                        {
                            strSql = "select max(CON_CONTRACT_NO) from TBL_CONTRACT_MAS";
                            break;
                        }
                    case "ContractCreateBy":
                        {
                            strSql = "select distinct CON_CREATE_BY from TBL_CONTRACT_MAS where CON_CREATE_BY is not null";
                            break;
                        }
                    case "ContractDept1":
                        {
                            strSql = "select CDM_ID as Id, CDM_NAME as Name from TBL_CON_DEPT_MAS order by CDM_NAME";
                            break;
                        }
                    case "ContractCreateBy1":
                        {
                            strSql = "select distinct CON_CREATE_BY as Id, CON_CREATE_BY as Name from TBL_CONTRACT_MAS where CON_CREATE_BY is not null";
                            break;
                        }
                    //<< Added by Ashish on 08-Jun-2017
                    case "getContractVendor":
                        {
                            strSql = "select * from TBL_CON_VENDOR_MASTER where CVM_STATUS = 'A' order by CVM_NAME";
                            break;
                        }

                    case "getContractTypeDetails":
                        {
                            strSql = "SELECT * FROM TBL_CONTRACT_TEMPLATE order by CT_NAME";
                            break;
                        }
                    //>>

                    //<< Added By Vivek on 28-Dec-2017
                    case "CIRCULARSGMT":
                        {
                            strSql = "select CSGM_ID,CSGM_NAME from TBL_CIRCULAR_SEGMENT_MAS order by CSGM_NAME ";
                            break;
                        }

                    case "CIRCULARINTIMATIONS":
                        {
                            strSql = "select CIM_ID,CIM_TYPE,CIM_STATUS from TBL_CIRCULAR_INTIMATIONS_MASTER order by CIM_TYPE ";
                            break;
                        }

                    case "AREA":
                        {
                            strSql = "SELECT * FROM TBL_CIRCULAR_AREA_MAS ORDER BY CAM_NAME";
                            break;
                        }

                    case "CircularFunction":
                        {
                            strSql = "select CFM_ID ,CFM_NAME  from TBL_CIRCULAR_FUNCTION_MAS order by CFM_NAME ";
                            break;
                        }

                    case "ISS":
                        {
                            strSql = "select CIA_ID,CIA_NAME, CIA_STATUS from TBL_CIRCULAR_ISSUING_AUTHORITIES WHERE CIA_STATUS='A' order by CIA_NAME ";
                            break;
                        }
                    case "getContractTypes":
                        {
                            strSql = "Select * FROM TBL_CONTRACT_TEMPLATE";
                            break;
                        }
                    //>>
                    case "CERTIFICATEDEPT":
                        {
                            strSql = "SELECT CDM_ID,CDM_NAME FROM  TBL_CERT_DEPT_MAS ORDER BY CDM_NAME";
                            break;
                        }
                    case "CertificationDepartmentLevel":
                        {
                            strSql = "Select * from TBL_REF_CODES where [RC_TYPE]='Certification Level' and [RC_STATUS]='A' order by RC_NAME ";
                            break;
                        }
                    case "CertificationSpocDepartment":
                        {
                            strSql = "SELECT CSSDM_ID,CSSDM_NAME FROM  TBL_CERT_SUB_SUB_DEPT_MAS ORDER BY CSSDM_EMP_NAME";
                            break;
                        }

                    case "CertificationUnitHeadDepartment":
                        {
                            strSql = "SELECT CSDM_ID,CSDM_NAME FROM  TBL_CERT_SUB_DEPT_MAS ORDER BY CSDM_NAME";
                            break;
                        }
                    case "CERTQUARTERS":
                        {
                            strSql = "SELECT CQM_ID,CQM_FROM_DATE,CQM_TO_DATE, " +
                                    " (replace(convert(varchar,CQM_FROM_DATE,106),' ','-')  " +
                                    " + ' to '+ " +
                                    " Replace(convert(varchar,CQM_TO_DATE,106),' ','-')) " +
                                    " AS Quarter " +
                                    " FROM  TBL_CERT_QUARTER_MAS where CQM_STATUS='A' " +
                                    " ORDER BY CQM_FROM_DATE";
                            break;
                        }
                    case "CERTIFICATEQUARTER":
                        {
                            strSql = "select CQM_ID,(replace(convert(varchar,CQM_FROM_DATE,106),' ','-') + ' to '+Replace(convert(varchar,CQM_TO_DATE,106),' ','-')) AS Quarter " +
                            "FROM TBL_CERT_QUARTER_MAS order by CQM_ID";
                            break;
                        }
                    case "LegalHelpDesk":
                        {
                            strSql = "SELECT HD_HCM_ID, HCM_NAME," +
                            "HD_ID, HD_REQUESTOR_NAME, HD_REQUESTOR_EMAIL, HD_STATUS, HD_AUDIT_TRAIL,HD_HDM_ID, " +
                            "case when HD_STATUS='S' then 'Submitted' end as Status, " +
                            " HD_CREATE_BY, HD_CREATE_DT  FROM TBL_HELP_DESK INNER JOIN " +
                            "TBL_HELPDESK_CATEGORY_MAS ON HD_HCM_ID = HCM_ID " +
                            " where HD_STATUS='S' ";
                            break;
                        }
                    case "LegalHelpDeskForReallocation":
                        {
                            strSql = "SELECT HD_HCM_ID, HCM_NAME," +
                            "HD_ID, HD_REQUESTOR_NAME, HD_REQUESTOR_EMAIL, HD_STATUS, HD_AUDIT_TRAIL,HD_HDM_ID, " +
                            "case when HD_STATUS='A' then 'Allocated' end as Status, " +
                            " HD_CREATE_BY, HD_CREATE_DT  FROM TBL_HELP_DESK INNER JOIN " +
                            "TBL_HELPDESK_CATEGORY_MAS ON HD_HCM_ID = HCM_ID " +
                            " where HD_STATUS='A'";
                            break;
                        }
                    case "HelpDeskOwner":
                        {
                            strSql = " SELECT * FROM TBL_HELPDESK_OWNERS where HO_IS_AVAILABLE = 'Y'";
                            break;
                        }
                    case "COMMONCERTSTATUS":
                        {
                            strSql = "SELECT * FROM TBL_COMMON_CERTIFICATIONS INNER JOIN TBL_CERT_QUARTER_MAS " +
                                    " on CQM_ID = CC_CQM_ID and CQM_STATUS = 'A'";
                            break;
                        }
                    case "SEARCHCERT":
                        {
                            strSql = "SELECT distinct CDM_NAME, CERT_ID, CDM_ID,CERTM_DEPT_ID,CERTM_ID,CERTM_TEXT,CERTM_CREATE_BY," +
                                    " CERTM_CREATE_DT,CERTM_LST_UPD_BY,CERT_REMARKS, " +
                                    " CERTM_LST_UPD_DT FROM TBL_CERTIFICATIONS INNER JOIN TBL_CERT_MAS ON " +
                                    " CERT_CERTM_ID = CERTM_ID AND CERT_STATUS = 'S'" +
                                    " INNER JOIN TBL_CERT_DEPT_MAS on " +
                                    " CERTM_DEPT_ID=CDM_ID AND CDM_NAME != 'Common'";
                            break;
                        }

                    case "SEARCHCERTFORACTIVEQUATER":
                        {
                            strSql = "SELECT distinct CDM_NAME, CERT_ID,cqm_id, CDM_ID,CERTM_DEPT_ID,CERTM_ID,CERTM_TEXT,CERTM_CREATE_BY, " +
                                    " CERTM_CREATE_DT,CERTM_LST_UPD_BY,CERT_REMARKS, " +
                                    " CSM_DESC [Status], " +
                                    " CERTM_LST_UPD_DT " +
                                    " FROM  TBL_CERTIFICATIONS " +
                                    " INNER JOIN TBL_CERT_QUARTER_MAS ON CERT_CQM_ID=CQM_ID " +
                                    " inner join TBL_CERT_STATUS_MAS on CERT_STATUS = CSM_NAME " +
                                    " INNER JOIN TBL_CERT_MAS on CERTM_ID=CERT_CERTM_ID " +
                                    " INNER JOIN TBL_CERT_DEPT_MAS on CERTM_DEPT_ID = CDM_ID AND CERTM_LEVEL_ID = 3 " +
                                    " WHERE 1=1 AND CQM_STATUS = 'A'";
                            break;
                        }

                    case "AssociatedKeywords":
                        {
                            strSql = "select * from TBL_CIRCULAR_KEYWORD_MAS where CKM_STATUS='A' order by CKM_NAME";
                            break;
                        }

                    case "getDistinctReferences":
                        {
                            strSql = "Select DISTINCT RC_TYPE from TBL_REF_CODES ORDER BY RC_TYPE";
                            break;
                        }

                    case "getReferences":
                        {
                            strSql = "Select RC_ID, RC_TYPE from TBL_REF_CODES ORDER BY RC_TYPE";
                            break;
                        }
                }

                if (!strSql.Equals(""))
                {
                    using (F2FDatabase DB = new F2FDatabase(strSql))
                    {
                        DB.F2FDataAdapter.Fill(resultDataSet);
                    }
                }

                return resultDataSet;
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return resultDataSet;
        }

        public DataTable getDatasetWithMoreCondition(string code, int condition1, int condition2, string conStr)
        {
            DataTable resultDataSet = new DataTable();
            try
            {
                string strSql = "";
                switch (code)
                {

                    case "CHECKEVENTINSTANCEENTRY":
                        {
                            strSql = "SELECT  EI_ID as cnt FROM TBL_EVENT_INSTANCES INNER JOIN TBL_EI_EP_MAPPING  ON EEM_EI_ID= EI_ID WHERE EEM_EP_ID = " + condition1 + "  AND EI_EM_ID=" + condition2 + "";
                            break;
                        }
                    case "deleteSMRepOwners":
                        {
                            strSql = "delete from TBL_SM_REPORTING_OWNERS where SRO_SRDOM_ID = " + condition1 + " and SRO_SM_ID= " + condition2;
                            break;
                        }
                    //Added by Milan Yadav on 27-Mar-2017
                    case "getApprovalRequired":
                        {
                            strSql = " select * from TBL_CT_APPROVING_DEPT where CAD_CT_ID =" + condition1 + " and CAD_DEPT_ID = " + condition2;
                            break;
                        }
                    //>>
                    //<< Added by Ashish on 29-Jun-2017
                    case "getContractApprovalCount":
                        {
                            strSql = " SELECT count(1) as count, min(DAD_ID) as NextDADId FROM TBL_DC_APPROVING_DEPT " +
                                     " WHERE DAD_DC_ID = " + condition1 +
                                     " AND DAD_SORT_ORDER > (SELECT DAD_SORT_ORDER FROM TBL_DC_APPROVING_DEPT WHERE DAD_ID = " + condition2 + ")";
                            break;
                        }
                        //>>
                }
                using (F2FDatabase DB = new F2FDatabase(strSql))
                {
                    DB.F2FDataAdapter.Fill(resultDataSet);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return resultDataSet;
        }

        public DataTable getDatasetWithTwoConditionInString(string code, string str1, string str2, string conStr)
        {
            DataTable resultDataSet = new DataTable();
            try
            {
                string strSql = "";
                switch (code)
                {

                    case "getReportingDeptUserFromLevel":
                        {
                            strSql = " select UserName, EmailId,[Level] from "
                                + " (SELECT SRDOM_EMP_NAME as UserName, SRDOM_EMAILID as EmailId, '0' as [Level] From "
                                + " TBL_SUB_SRD_OWNER_MASTER "
                                + " INNER JOIN TBL_SUB_REPORTING_DEPT on "
                                + " SRD_ID = SRDOM_SRD_ID "
                                + " where SRDOM_STATUS = 'A' AND SRD_STATUS = 'A' AND SRD_ID = '" + str1 + "' "
                                + " union "
                                + " select SE_FIRST_NAME as UserName, SE_EMAIL_ID as EmailId, SE_LEVEL as [Level] from TBL_SUB_ESCALATION "
                                + " where SE_STATUS = 'A' AND[SE_SRD_ID] = '" + str1 + "') as tab WHERE LEVEL IN (" + str2 + ")";
                            break;
                        }

                    case "getTrackingDeptUserFromLevel":
                        {
                            strSql = " Select EM_EMPNAME, EM_EMAIL, ESM_LEVEL from TBL_EM_STM_MAPPING "
                                + " inner join EmployeeMaster on EM_ID = ESM_EM_ID AND EM_STATUS = 'A' "
                                + " WHERE ESM_STM_ID = '" + str1 + "' AND ESM_LEVEL IN (" + str2 + ")";
                            break;
                        }

                    case "areSubmissionsDone":
                        {
                            strSql = "select count(*) as cnt from TBL_SUB_CHKLIST inner join tbl_submissions " +
                        " on sub_sc_id=sc_id where SC_SM_ID= " + str1 + " and SC_DUE_DATE_TO>='" + str2 + "'";
                            break;
                        }

                    case "checkedForChecklistEntries":
                        {
                            strSql = "select  count(*) as cnt from TBL_SUB_CHKLIST inner join TBL_SUBMISSIONS_MAS on sc_sm_id=sm_id " +
                          " where  sm_id=" + str1 + " and SC_DUE_DATE_TO>='" + str2 + "'";
                            break;
                        }
                    case "getChecklistForAdminByMonth":
                        {
                            strSql = "SELECT DISTINCT SC_ID, EM_EVENT_NAME,EP_NAME, EI_EVENT_DATE, SUB_ID, " +
                                        "SC_STM_ID,SUB_CLIENT_FILE_NAME, SUB_SERVER_FILE_NAME, " +
                                        "SUB_REMARKS, SUB_STATUS, SUB_YES_NO_NA, SUB_SC_ID, SC_ID, SC_SM_ID, " +
                                        "SC_DUE_DATE_FROM, STM_TYPE, SC_DUE_DATE_TO, SC_FREQUENCY, " +
                                        "SC_PARTICULARS, SC_CSGM_ID, SSM_NAME,SC_DESCRIPTION,SUB_CREAT_DT,SUB_CREAT_BY FROM  " +
                                        "TBL_SUB_CHKLIST INNER JOIN  TBL_SUBMISSION_SEGMENT_MAS " +
                                        "ON SC_CSGM_ID = SSM_ID " +
                                        "and (MONTH(SC_DUE_DATE_TO) = " + str1 + ") " +

                                        "INNER JOIN TBL_SUB_TYPE_MAS ON SC_STM_ID = STM_ID " +
                                        "INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID " +
                                        //"AND FYM_ID = " + str3 +
                                        "INNER JOIN TBL_EM_STM_MAPPING ON ESM_STM_ID = SC_STM_ID INNER JOIN " +
                                        "EmployeeMaster " +
                                        "ON ESM_EM_ID = EM_ID " +
                                        "AND EM_USERNAME = '" + str2 + "' " +
                                        "LEFT OUTER JOIN vwEventDetails on SC_EEM_ID = EEM_ID " +
                                        "LEFT OUTER JOIN TBL_SUBMISSIONS ON SC_ID = SUB_SC_ID " +
                                        "ORDER BY SC_STM_ID, SC_DUE_DATE_TO";
                            break;
                        }

                    //Added By Milan Yadav on 23-Sep-2016
                    //>>
                    case "getPanNumberDuplicationCheck":
                        {
                            strSql = "select distinct CVM_PAN_NUMBER from TBL_CON_VENDOR_MASTER where 1=1 and CVM_PAN_NUMBER = '" + str1 + "'";

                            if (!str2.Equals("0"))
                            {
                                strSql = strSql + " and CVM_ID != " + str2;
                            }
                            break;
                        }

                    case "getKMPPanNumberDuplicationCheck":
                        {
                            strSql = "select distinct CKD_PAN_NUMBER from TBL_CON_KMP_DIRECTORS where 1=1 and CKD_PAN_NUMBER = '" + str1 + "'";

                            if (!str2.Equals("0"))
                            {
                                strSql = strSql + " and CKD_ID != " + str2;
                            }
                            break;
                        }
                    //<< Added By Milan Yadav on 25-Feb-2017
                    case "getLevel2ApproverMailId":
                        {
                            strSql = "SELECT CAO_LEVEL2_APPROVAL_EMAIL,* FROM TBL_CON_APPROVAL_OWNERS " +
                                    " where 1=1 ";
                            if (!str1.Equals(""))
                            {
                                strSql = strSql + " and CAO_USERNAME = '" + str1 + "'";
                            }
                            if (!str2.Equals(""))
                            {
                                strSql = strSql + " and CAO_LEVEL1_APPROVAL_USERNAME = '" + str2 + "'";
                            }
                            break;
                        }
                    case "getLevel2ApproverMailIdLH":
                        {
                            strSql = "SELECT CAO_LEVEL2_APPROVAL_EMAIL,* FROM TBL_CON_APPROVAL_OWNERS " +
                                    " where 1=1 ";
                            if (!str1.Equals(""))
                            {
                                strSql = strSql + " and CAO_USERNAME = '" + str1 + "'";
                            }
                            if (!str2.Equals(""))
                            {
                                strSql = strSql + " and CAO_LEVEL2_APPROVAL_USERNAME = '" + str2 + "'";
                            }
                            break;
                        }
                    //>>
                    //<< Added by Ashish on 08-Jun-2017
                    case "getContractDeptByName":
                        {
                            strSql = "select * from TBL_CON_DEPT_MAS where CDM_NAME = '" + str1.Replace("'", "''") + "' ";

                            if (!str2.Equals("0"))
                            {
                                strSql = strSql + " and CDM_ID != '" + str2 + "'";
                            }

                            break;
                        }

                    case "getContractVendorByName":
                        {
                            strSql = "select * from TBL_CON_VENDOR_MASTER where CVM_NAME = '" + str1.Replace("'", "''") + "' ";

                            if (!str2.Equals("0"))
                            {
                                strSql = strSql + " and CVM_ID != '" + str2 + "'";
                            }

                            break;
                        }

                    case "getContractDirectorByName":
                        {
                            strSql = "select * from TBL_CON_KMP_DIRECTORS where CKD_NAME = '" + str1.Replace("'", "''") + "' ";

                            if (!str2.Equals("0"))
                            {
                                strSql = strSql + " and CKD_ID != '" + str2 + "'";
                            }

                            break;
                        }
                    case "getContractTypeByName":
                        {
                            strSql = "select * from TBL_CONTRACT_TYPE_MAS where CTM_NAME = '" + str1.Replace("'", "''") + "' ";

                            if (!str2.Equals("0"))
                            {
                                strSql = strSql + " and CTM_ID != '" + str2 + "'";
                            }

                            break;
                        }
                    //>>
                    case "GetCertChecklistDetailById":
                        {
                            strSql = "select *,Replace(CONVERT(VARCHAR(11), CCM_EFFECTIVE_FROM, 106), ' ', '-') as [Effective From], " +
                                    " case when CCM_STATUS ='A' then 'Active' when CCM_STATUS ='I' then 'Inactive' else '' end  as [Status], CCM_REMARK as [Remark] " +
                                    " from dbo.TBL_CERT_CHECKLIST_MAS " +
                                    " inner join  TBL_CERT_SUB_SUB_DEPT_MAS on CCM_CSSDM_ID = CSSDM_ID " +
                                    " LEFT OUTER JOIN TBL_CIRCULAR_DOCUMENT_TYPE_MAS ON CCM_ACT_REGULATION_CIRCULAR = CDTM_ID " +
                                    " where 1=1 ";

                            if (!str1.Equals(""))
                            {
                                strSql = strSql + " and CSSDM_ID = " + str1;
                            }
                            if (!str2.Equals(""))
                            {
                                strSql = strSql + " and CCM_ID = " + str2;
                            }

                            break;
                        }

                }
                using (F2FDatabase DB = new F2FDatabase(strSql))
                {
                    DB.F2FDataAdapter.Fill(resultDataSet);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("system exception in GetResult() " + ex);
            }
            return resultDataSet;
        }

        public DataTable getDatasetWithThreeConditionInString(string code, string str1, string str2,
            string str3, string conStr)
        {
            DataTable resultDataSet = new DataTable();
            try
            {
                string strSql = "";
                switch (code)
                {
                    //Added By Milan Yadav on 27Apr2016
                    //>>
                    //<<Modified by Ashish Mishra on 16Aug2017
                    case "getChecklistForAdminByMonth":
                        {
                            strSql = "SELECT DISTINCT SUB_SUBMIT_DATE, SUB_MODE_OF_FILING, SUB_REOPEN_COMMENTS, SUB_SUBMITTED_TO_AUTHORITY_ON, SC_ID,EM_EVENT_NAME,EP_NAME, EI_EVENT_DATE, SUB_ID, " +
                                    "SC_STM_ID,SUB_CLIENT_FILE_NAME, SUB_SERVER_FILE_NAME, " +
                                    "SUB_REMARKS, SUB_STATUS, SUB_YES_NO_NA, SUB_SC_ID, SC_ID, SC_SM_ID, SRD_ID,STM_ID," +
                                    "SC_DUE_DATE_FROM, STM_TYPE, SC_DUE_DATE_TO, SC_FREQUENCY, " +
                                    "SC_PARTICULARS, SC_CSGM_ID, SSM_NAME,SC_DESCRIPTION,SUB_CREAT_DT,SUB_CREAT_BY FROM  " +
                                    "TBL_SUB_CHKLIST INNER JOIN  TBL_SUBMISSION_SEGMENT_MAS " +
                                    "ON SC_CSGM_ID = SSM_ID " +
                                    "and (MONTH(SC_DUE_DATE_TO) = " + str1 + ") " +

                                    "INNER JOIN TBL_SUB_TYPE_MAS ON SC_STM_ID = STM_ID " +
                                    "INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                                    "INNER JOIN TBL_SUB_REPORTING_DEPT ON SM_SRD_ID = SRD_ID " +
                                    "INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID " +
                                    "AND FYM_ID = " + str3 +
                                    "INNER JOIN TBL_EM_STM_MAPPING ON ESM_STM_ID = SC_STM_ID INNER JOIN " +
                                    "EmployeeMaster " +
                                    "ON ESM_EM_ID = EM_ID " +
                                    "AND EM_USERNAME = '" + str2 + "' " +
                                    "LEFT OUTER JOIN vwEventDetails on SC_EEM_ID = EEM_ID " +
                                    "LEFT OUTER JOIN TBL_SUBMISSIONS ON SC_ID = SUB_SC_ID " +
                                    "ORDER BY SC_STM_ID, SC_DUE_DATE_TO";
                            break;
                        }
                    //>>
                    //<<
                    //<< Added by Ashish on 08-Jun-2017
                    case "getContractParamByName":
                        {
                            strSql = "select * from TBL_CONTRACT_TEMP_PARAMS where CTP_PARAM_NAME = '" + str1.Replace("'", "''") + "' ";
                            strSql = strSql + " and CTP_CT_ID = " + str2;

                            if (!str3.Equals("0"))
                            {
                                strSql = strSql + " and (CTP_ID != '" + str3 + "' or CTP_ID = '" + str3 + "')";
                            }

                            break;
                        }
                    //>>
                    //<<Added by Ashish Mishra
                    case "getContractTemplateByName":
                        {
                            strSql = "select * from TBL_CONTRACT_TEMPLATE where CT_NAME = '" + str1.Replace("'", "''") + "' ";
                            strSql = strSql + " and CT_TYPE = " + str2;
                            if (!str3.Equals("0"))
                            {
                                strSql = strSql + " and CT_ID != '" + str3 + "'";
                            }

                            break;
                        }
                    case "getCertSubDeptById":
                        {
                            strSql = "SELECT CSDM_NAME  as DeptName, * " +
                                    " FROM  TBL_CERT_SUB_DEPT_MAS " +
                                    " inner join TBL_CERT_DEPT_MAS on CSDM_CDM_ID = CDM_ID where 1=1 ";

                            if (!str1.Equals(""))
                            {
                                strSql = strSql + " and CDM_ID = " + str1;
                            }
                            if (!str2.Equals(""))
                            {
                                strSql = strSql + " and CSDM_ID = " + str2;
                            }
                            if (!str3.Equals(""))
                            {
                                strSql = strSql + " and CSDM_NAME = '" + str3 + "'";
                            }
                            break;
                        }
                }
                using (F2FDatabase DB = new F2FDatabase(strSql))
                {
                    DB.F2FDataAdapter.Fill(resultDataSet);
                }

            }
            catch (System.Exception ex)
            {
                throw new System.Exception("system exception in getDatasetWithThreeConditionInString() " + ex);
            }
            return resultDataSet;
        }

        public DataTable getNewsAlertForLitigation(string conStr)
        {
            DataTable dt = new DataTable();
            string strSql = "";
            try
            {
                strSql = "SELECT * FROM  TBL_MAIL_ALERTS_MASTER order by MAM_DATE ";
                using (F2FDatabase DB = new F2FDatabase(strSql))
                {
                    DB.F2FDataAdapter.Fill(dt);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("system exception in getNewsAlert() " + ex);
            }
            return dt;
        }

        public DataTable getNewsAlertForLSLitigation(string conStr)
        {
            DataTable dt = new DataTable();
            string strSql = "";
            try
            {
                strSql = "SELECT * FROM  TBL_LS_MAIL_ALERTS order by LMA_DATE ";
                using (F2FDatabase DB = new F2FDatabase(strSql))
                {
                    DB.F2FDataAdapter.Fill(dt);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("system exception in getNewsAlert() " + ex);
            }
            return dt;
        }


        //Added By Milan Yadav on 22-Feb-2016
        //<<
        public DataTable getDatasetWithThreeConditionsInString(string code, string str, int intLevel,
            string conStr)
        {
            DataTable resultDataSet = new DataTable();
            try
            {
                string strSql = "";
                switch (code)
                {
                    case "getCertContentById":
                        {
                            //strSql = " select distinct TBL_CERT_MAS.* FROM TBL_CERT_MAS " +
                            //        " where 1=1 ";

                            strSql = " select distinct * FROM TBL_CERT_MAS inner join TBL_CERTIFICATIONS on CERTM_ID = CERT_CERTM_ID  " +
                                     " inner join TBL_CERT_QUARTER_MAS ON CQM_ID = CERT_CQM_ID AND CQM_STATUS = 'A' " +
                                      " where 1=1 ";
                            //>>
                            if (!str.Equals(""))
                            {
                                strSql = strSql + " and CERTM_DEPT_ID = " + str;
                            }
                            if (!intLevel.Equals(""))
                            {
                                strSql = strSql + " and CERTM_LEVEL_ID = " + intLevel;
                            }
                            break;
                        }
                }
                using (F2FDatabase DB = new F2FDatabase(strSql))
                {
                    DB.F2FDataAdapter.Fill(resultDataSet);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return resultDataSet;
        }
        //>>

        //Added By Kiran Kharat on 29-Jan-2018
        //<<
        public DataTable getDataForGv(string conStr)
        {
            DataTable resultDataSet = new DataTable();
            try
            {
                string strSql = "select * from TBL_CON_CLAUSE_TYPE_MAS where CCTM_STATUS='A'";
                using (F2FDatabase DB = new F2FDatabase(strSql))
                {
                    DB.F2FDataAdapter.Fill(resultDataSet);
                }

            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return resultDataSet;
        }

        public DataTable getDataForEdit(string strId, string conStr)
        {
            DataTable resultDataSet = new DataTable();
            try
            {
                string strSql = " select * from  TBL_CON_CLAUSES_LIB inner join TBL_CON_CLAUSE_TYPE_MAS ON CCL_CCTM_ID = CCTM_ID  WHERE CCL_ID = " + strId;
                using (F2FDatabase DB = new F2FDatabase(strSql))
                {
                    DB.F2FDataAdapter.Fill(resultDataSet);
                }

            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return resultDataSet;
        }

        public DataTable getDescriptionOfClauses(string strListOfIds, string conStr)
        {
            DataTable dt = new DataTable();
            string strSql = "";
            try
            {
                strSql = "select CCL_DESC from TBL_CON_CLAUSES_LIB where CCL_ID in (" + strListOfIds + ")";
                using (F2FDatabase DB = new F2FDatabase(strSql))
                {
                    DB.F2FDataAdapter.Fill(dt);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return dt;
        }
        //>> End

        //<< Added by Amarjeet on 06-Aug-2021
        public void savePDFContentInTable(string strRefId, string strRefFileId, string strContent, string strModuleName,
            string strLoggedInUser)
        {
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.OpenConnection();

                    DB.F2FCommand.CommandText = "savePDFContents";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@RefId", F2FDbType.Int32, (string.IsNullOrEmpty(strRefId) ? 0 : Convert.ToInt32(strRefId))));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@RefFileId", F2FDbType.Int32, (string.IsNullOrEmpty(strRefFileId) ? 0 : Convert.ToInt32(strRefFileId))));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Content", F2FDbType.VarChar, strContent));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ModuleName", F2FDbType.VarChar, strModuleName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LoggedInUser", F2FDbType.VarChar, strLoggedInUser));
                    DB.F2FCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
        }
        //>>

        public void updateConfigParams(int intId, string strValue, string mstrConnectionString)
        {
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "updateConfigParams";
                    DB.F2FCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, intId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Value", F2FDbType.VarChar, strValue));
                    DB.OpenConnection();
                    DB.F2FCommand.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new System.Exception("system exception in upateConfigParams() " + ex);
            }
            finally
            {
            }
        }
    }
}