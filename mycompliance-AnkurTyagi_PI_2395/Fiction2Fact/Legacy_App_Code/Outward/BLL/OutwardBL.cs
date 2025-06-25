using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using Fiction2Fact.Legacy_App_Code.Outward.DAL;

namespace Fiction2Fact.Legacy_App_Code.Outward.BLL
{
    public class OutwardBL
    {
        OutwardDAL otBal = new OutwardDAL();
        
        public DataSet searchOutwardformail(string intId, string strCreateBy, string strStatus)
        {
            DataSet dsResults = new DataSet();
            dsResults = otBal.searchOutwardformail(intId,strCreateBy,strStatus);
            return dsResults;
        }
        public string SaveOutwardTrackers(int intId, string strDocName,
                            string strOutwardDate, int intAuthority, int intDept, string strCreateBy, int intTypeOfoutward,
                            DataTable mdtEditFileUpload, 
                            string strCreator,string strStatus,string strRemarks,string strOldOutward)
        {
            string strDocNo;
            strDocNo = otBal.SaveOutwardTrackers(intId, strDocName,
                            strOutwardDate, intAuthority, intDept, strCreateBy, intTypeOfoutward, mdtEditFileUpload,
                            strCreator, strStatus, strRemarks, strOldOutward);
            return strDocNo;
        }

        public string UpdateOutwardTrackers(int intId, string strAddressor, string strAddressee, string strDocName,
                            string strOutwardDate, int intAuthority, int intDept, string strCreateBy, int intTypeOfoutward,
                            string strRemarks, string strClosureRemark, DataTable mdtEditFileUpload,
                            string strPODNumber, string strDispatchDate, string strCourierName,
                            string strEmailSentDate, string strHardCopy, string strCreator,
                            string strRepresentation, string strRepresentationStatus,
                            string strRepresentationDate, string strTobeSend, string strStatus)
        {
            string strDocNo;
            strDocNo = otBal.UpdateOutwardTrackers(intId, strAddressor, strAddressee, strDocName,
                            strOutwardDate, intAuthority, intDept, strCreateBy, intTypeOfoutward, strRemarks, strClosureRemark, mdtEditFileUpload,
                            strPODNumber, strDispatchDate, strCourierName,
                            strEmailSentDate, strHardCopy, strCreator, strRepresentation,
                            strRepresentationStatus, strRepresentationDate, strTobeSend, strStatus);
            return strDocNo;
        }
        public string SuggestRevisionOutwardTrackers(string strId, string SuggestRemark, string strCreateBy,string strStatus)
        {
            string strDocNo;
            strDocNo = otBal.SuggestRevisionOutwardTrackers(strId, SuggestRemark, strCreateBy,strStatus);
            return strDocNo;
        }
        public string AddUpdatesOutwardTrackers(int intId, string UpdateDate, string Remark, string strCreateBy)
        {
            string strDocNo;
            strDocNo = otBal.AddUpdatesOutwardTrackers(intId, UpdateDate, Remark, strCreateBy);
            return strDocNo;
        }
        
        public void cancelOutwardTrackers(int Id, string Remarks, string strCreateBy, string CancelOn)
        {
            otBal.cancelOutwardTrackers(Id, Remarks, strCreateBy, CancelOn);
        }
        public void deleteOutwardTrackers(int Id, string Remarks, string strCreateBy, string deleteOn)
        {
            otBal.deleteOutwardTrackers(Id, Remarks, strCreateBy, deleteOn);
        }
        public void revisionOutwardTrackers(int Id, string Remarks, string strCreateBy, string CancelOn)
        {
            otBal.revisionOutwardTrackers(Id, Remarks, strCreateBy, CancelOn);
        }
        
        public DataSet SearchOutwards(string strDocNo, string strAddressor, string strAddressee,
                                            string strDocumentName, string strOutwardType, string strOutwardDate,
                                            string strRegAuth, string strDept, string strStatus, string strType,
                                            string strLoggedInUser, string strGlobalSearch,
                                            string FromDate,string ToDate , string FileName, string strFlag)
        {
            DataSet dsResults = new DataSet();
            dsResults = otBal.SearchOutwards(strDocNo, strAddressor, strAddressee, strDocumentName, strOutwardType, strOutwardDate,
                                            strRegAuth, strDept, strStatus, strType, strLoggedInUser,
                                            strGlobalSearch, FromDate, ToDate, FileName, strFlag);
            return dsResults;
        }
    }
}
