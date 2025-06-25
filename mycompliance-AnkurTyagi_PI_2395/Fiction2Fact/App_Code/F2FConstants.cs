using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fiction2Fact.App_Code
{
    public class F2FConstants
    {
        public class SessionMsgKeys
        {
            private static string _SaveMsgSuccess = "SaveMsgSuccess";
            private static string _SaveMsgError = "SaveMsgError";

            public static string SaveMsgSuccess
            {
                get
                {
                    return _SaveMsgSuccess;
                }

                set
                {
                    _SaveMsgSuccess = value;
                }
            }

            public static string SaveMsgError
            {
                get
                {
                    return _SaveMsgError;
                }

                set
                {
                    _SaveMsgError = value;
                }
            }
        }
    }
}