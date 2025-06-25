using System;

/// <summary>
/// Summary description for UtilitiesVO
/// </summary>
/// 
namespace Fiction2Fact.Legacy_App_Code.VO
{
    public class UtilitiesVO
    {
        public UtilitiesVO()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private string code;
        private string condition1;
        private string condition2;

        public String getCode()
        {
            return code;
        }

        public void setCode(String code)
        {
            this.code = code;
        }

        public String getCondition1()
        {
            return condition1;
        }

        public String getCondition2()
        {
            return condition2;
        }

        public void setTwoCondition(String condition1, String condition2)
        {
            this.condition1 = condition1;
            this.condition2 = condition2;
        }
        
        //Added by Archana Gosavi on 11Sep2015
        public int CertId;
        public int CSSId;

        public int getCertId()
        {
            return CertId;
        }

        public int getCSSId()
        {
            return CSSId;
        }

        public void setCertId(int CertId)
        {
            this.CertId = CertId;
        }

        public void setCSSId(int CSSId)
        {
            this.CSSId = CSSId;
        }
    }
}
