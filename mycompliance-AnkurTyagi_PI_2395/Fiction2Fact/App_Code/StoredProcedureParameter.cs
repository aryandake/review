using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Fiction2Fact.App_Code
{
    public class StoredProcedureParameter
    {
        private string _ParamName;
        private string _ParamValue = null;
        private string _ParamDataType = "F2FVarChar";
        private ParameterDirection _ParamDirection = ParameterDirection.Input;

        public string ParamName
        {
            get
            {
                return _ParamName;
            }

            set
            {
                _ParamName = value;
            }
        }

        public string ParamValue
        {
            get
            {
                return _ParamValue;
            }

            set
            {
                _ParamValue = value;
            }
        }

        public string ParamDataType
        {
            get
            {
                return _ParamDataType;
            }

            set
            {
                _ParamDataType = value;
            }
        }

        public ParameterDirection ParamDirection
        {
            get
            {
                return _ParamDirection;
            }

            set
            {
                _ParamDirection = value;
            }
        }

        ///<summary>
        /// Parameter to be passed to Stored Procedure with its Name, Value, DataType and Direction
        ///</summary>
        /// <param name="sParamName">Parameter Name to be passed</param>
        /// <param name="sParamValue">Parameter Value</param>
        /// <param name="sParamDataType">Parameter Data Type</param>
        /// <param name="sParamIOType">Parameter Direction as Input/InputOutput/Output/ReturnType</param>
        /// 
        public StoredProcedureParameter(string sParamName = null, string sParamValue = null, string sParamDataType = "F2FVarChar", ParameterDirection sParamDirection = ParameterDirection.Input)
        {
            ParamName = sParamName;
            ParamValue = sParamValue;
            ParamDataType = sParamDataType;
            ParamDirection = sParamDirection;
        }

    }
}