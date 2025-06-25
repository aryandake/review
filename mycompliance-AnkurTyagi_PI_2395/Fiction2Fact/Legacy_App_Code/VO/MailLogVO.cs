/// <summary>
/// Summary description for MailLogVO
/// </summary>
namespace Fiction2Fact.Legacy_App_Code.VO
{
    public class MailLogVO : CommonVO
    {
        public MailLogVO()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        private int mailId;
        private string mailTO;
        private string mailStatus;
        private string mailFromDate;
        private string mailToDate;
        private string mailError;
        private string mailCC;
        private string mailBCC;
        private string mailSubject;
        private int MailFailCount = 0;
        private string mailType;
        private string mailAttachments;
        private string mailContent;
        private string mailSendOn;


        public void setMailId(int mailId)
        {
            this.mailId = mailId;
        }
        public int getMailId()
        {
            return mailId;
        }

        public void setMailStatus(string mailStatus)
        {
            this.mailStatus = mailStatus;
        }
        public string getMailStatus()
        {
            return mailStatus;
        }

        public void setmailFromDate(string mailFromDate)
        {
            this.mailFromDate = mailFromDate;
        }
        public string getmailFromDate()
        {
            return mailFromDate;
        }

        public void setmailToDate(string mailToDate)
        {
            this.mailToDate = mailToDate;
        }
        public string getmailToDate()
        {
            return mailToDate;
        }

        public void setErrorMsg(string mailError)
        {
            this.mailError = mailError;
        }
        public string getErrorMsg()
        {
            return mailError;
        }
        public void setMailTO(string mailTO)
        {
            this.mailTO = mailTO;
        }
        public string getMailTO()
        {
            return mailTO;
        }

        public void setMailCC(string mailCC)
        {
            this.mailCC = mailCC;
        }
        public string getMailCC()
        {
            return mailCC;
        }

        public void setMailBCC(string mailBCC)
        {
            this.mailBCC = mailBCC;
        }
        public string getMailBCC()
        {
            return mailBCC;
        }

        public void setMailSubject(string mailSubject)
        {
            this.mailSubject = mailSubject;
        }
        public string getMailSubject()
        {
            return mailSubject;
        }

        public void setMailContent(string mailContent)
        {
            this.mailContent = mailContent;
        }
        public string getMailContent()
        {
            return mailContent;
        }

        public void setMailSendOn(string mailSendOn)
        {
            this.mailSendOn = mailSendOn;
        }
        public string getMailSendOn()
        {
            return mailSendOn;
        }

        public int getMailFailCount()
        {
            return MailFailCount;
        }
        //>>

        //<< Added by Milan Yadav Patel @1 Octo 2015
        public void setMailType(string mailType)
        {
            this.mailType = mailType;
        }

        public string getMailType()
        {
            return mailType;
        }

        public void setMailAttachments(string mailAttachments)
        {
            this.mailAttachments = mailAttachments;
        }

        public string getMailAttachments()
        {
            return mailAttachments;
        }



    }
}