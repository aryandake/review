using System;

/// <summary>
/// Summary description for CommonVO
/// </summary>
namespace Fiction2Fact.Legacy_App_Code.VO
{
    public class CommonVO
    {
        public CommonVO()
        {
            //
            // TODO: Add constructor logic here
            //
        }

       private string createdBy;
        private string createdDate;
        private string updatedBy;
        private string updatedDate;

       public String getCreatedBy()
        {
            return createdBy;
        }
        public void setCreatedBy(String createdBy)
        {
            this.createdBy = createdBy;
        }

        public String getCreatedDate()
        {
            return createdDate;
        }
        public void setCreatedDate(String createdDate)
        {
            this.createdDate = createdDate;
        }

        public String getUpdatedBy()
        {
            return updatedBy;
        }
        public void setUpdatedBy(String updatedBy)
        {
            this.updatedBy = updatedBy;
        }

        public String getUpdatedDate()
        {
            return updatedDate;
        }
        public void setUpdatedDate(String updatedDate)
        {
            this.updatedDate = updatedDate;
        }
	
    }
}