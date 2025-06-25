using Fiction2Fact;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;

/// <summary>
/// Summary description for CSVInjectionPrevention
/// </summary>
public class CSVInjectionPrevention
{
	public CSVInjectionPrevention()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static bool IsCSVInjection(DataTable dt)
    {
        DataRow dr ;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
             dr = dt.Rows[i];
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if(dr[j].ToString().StartsWith("=") || dr[j].ToString().StartsWith("%") || dr[j].ToString().StartsWith("+") 
                    || dr[j].ToString().StartsWith("-")
                   || dr[j].ToString().Equals("REF!") || dr[j].ToString().Contains(".exe") || dr[j].ToString().Contains("<script") ||
                   dr[j].ToString().Contains(".bat") || dr[j].ToString().Contains(".js") || dr[j].ToString().Contains("alert("))
                { 
                    return true;
                }
            }
        }
        return false;
    }

    static string sExcludeInputs = "FCKE_";

    public static bool CheckInputValidity(Page p)
    {
        bool bEnableServerSideInputValidation = (ConfigurationManager.AppSettings.AllKeys.Contains("EnableServerSideInputValidation") ? 
            (ConfigurationManager.AppSettings["EnableServerSideInputValidation"].ToString().Equals("true", StringComparison.CurrentCultureIgnoreCase) ? true : false) : false);

        if (bEnableServerSideInputValidation)
        {
            string sScript = string.Empty;
            foreach (string sInput in Global.lstInvalidInputs)
            {
                if (!sInput.Contains(sExcludeInputs) && !sInput.Contains("$hf"))
                {
                    string sErrorSpan = "<br /><span class=\"text-danger\">Invalid Input detected</span>";
                    sScript += "$('" + sErrorSpan + "').insertAfter($('[name=\"" + sInput + "\"]'));";
                }
            }
            Global.lstInvalidInputs = new List<string>();
            if (!string.IsNullOrEmpty(sScript))
            {
                p.ClientScript.RegisterStartupScript(p.GetType(), "", sScript, true);
                return false;
            }
        }
        return true;
    }
}