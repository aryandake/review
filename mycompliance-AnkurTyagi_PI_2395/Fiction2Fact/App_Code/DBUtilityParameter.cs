using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fiction2Fact.App_Code
{
    public class DBUtilityParameter
    {
        public string ColumnName { get; set; }
        public string Operator { get; set; } = "=";
        public object FirstValue { get; set; }
        public object SecondValue { get; set; }
        public string CondType { get; set; } = "AND";
        public object SubQuery { get; set; } = "0";
        public DBUtilityParameter(string sColumnName, object oFirstValue, string sOperator = "=", object oSecondValue = null, string sConditionType = "AND", object oSubQuery = null)
        {
            ColumnName = sColumnName;
            Operator = sOperator;
            FirstValue = oFirstValue;
            SecondValue = oSecondValue;
            CondType = sConditionType;
            SubQuery = oSubQuery == null ? "0" : oSubQuery;
        }
        public DBUtilityParameter()
        {

        }
    }
}