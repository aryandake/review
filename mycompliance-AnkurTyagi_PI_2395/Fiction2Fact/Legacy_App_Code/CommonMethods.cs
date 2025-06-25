using System;
using System.Web.UI.WebControls;
using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.App_Code;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Crypto;
using System.Data;
using System.Configuration;

namespace Fiction2Fact.Legacy_App_Code
{
    public class CommonMethods
    {
        UtilitiesBLL utilBLL = new UtilitiesBLL();

        public static string GetSqlText(string Text)
        {
            return "'" + Text.Replace("'", "''") + "'";
        }

        public void GenerateExcelReport(GridView gvExcelData)
        {

        }
        public string getSanitizedString(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                double dblVal = 0;

                if (!Double.TryParse(str, out dblVal))
                {
                    string strVulChars = ConfigurationManager.AppSettings["CSV_Inj_VulnerableChars"] == null
                        ? "@~-~+~=~|" : ConfigurationManager.AppSettings["CSV_Inj_VulnerableChars"].ToString();

                    string strCSV_Inj_Escape = ConfigurationManager.AppSettings["CSV_Inj_Escape"] == null
                        ? "`" : ConfigurationManager.AppSettings["CSV_Inj_Escape"].ToString();

                    string[] strArrVulChar = strVulChars.Split('~');

                    foreach (string item in strArrVulChar)
                    {
                        if (str.StartsWith(item))
                        {
                            str = strCSV_Inj_Escape + str;
                            break;
                        }
                    }
                }
            }
            //str = str.Replace("\r\n", "~~");
            //str = str.Replace("+", "~plus");
            //string strSanitizedString = "";
            //try
            //{
            //    strSanitizedString = Sanitizer.GetSafeHtmlFragment(str);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return strSanitizedString.Replace("~~", "\r\n").Replace("~plus", "+");
            return str;
        }

        //<< Added by Amarjeet on 06-Aug-2021
        public void savePDFContent(string strRefId, string strRefFileId, string strFilePath, string strModuleName,
            string strLoggedInUser)
        {
            try
            {
                if (strFilePath != "")
                {
                    StringBuilder pageText = new StringBuilder();
                    using (PdfDocument pdfDocument = new PdfDocument(new PdfReader(strFilePath)))
                    {
                        int pageNumbers = pdfDocument.GetNumberOfPages();
                        for (int i = 1; i <= pageNumbers; i++)
                        {
                            LocationTextExtractionStrategy strategy = new LocationTextExtractionStrategy();
                            PdfCanvasProcessor parser = new PdfCanvasProcessor(strategy);
                            parser.ProcessPageContent(pdfDocument.GetFirstPage());
                            pageText.Append(strategy.GetResultantText());

                            //string str = PdfTextExtractor.GetTextFromPage(pdfDocument.GetFirstPage(), strategy);
                        }
                    }

                    utilBLL.savePDFContentInTable(strRefId, strRefFileId, pageText.ToString(), strModuleName, strLoggedInUser);
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
        }

        public string searchPDFContent(string strRefId, string strFilePath, string strSearchText)
        {
            StringBuilder pageText = new StringBuilder();

            try
            {
                if (strFilePath != "")
                {
                    if (File.Exists(strFilePath) && Path.GetExtension(strFilePath).Equals(".pdf"))
                    {
                        try
                        {
                            using (PdfDocument pdfDocument = new PdfDocument(new PdfReader(strFilePath)))
                            {
                                int pageNumbers = pdfDocument.GetNumberOfPages();
                                for (int i = 1; i <= pageNumbers; i++)
                                {
                                    LocationTextExtractionStrategy strategy = new LocationTextExtractionStrategy();
                                    PdfCanvasProcessor parser = new PdfCanvasProcessor(strategy);
                                    parser.ProcessPageContent(pdfDocument.GetFirstPage());
                                    pageText.Append(strategy.GetResultantText());
                                }
                            }
                        }
                        catch (BadPasswordException)
                        { }
                    }
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }

            if (pageText.ToString().Contains(strSearchText))
                return strRefId;
            else
                return "0";
        }

        public string[] RemoveDuplicates(string[] inputArray)
        {
            int length = inputArray.Length;

            for (int i = 0; i < length; i++)
            {
                for (int j = (i + 1); j < length;)
                {
                    if (inputArray[i].ToLower() == inputArray[j].ToLower())
                    {
                        for (int k = j; k < length - 1; k++)
                            inputArray[k] = inputArray[k + 1];
                        length--;
                    }
                    else
                        j++;
                }
            }

            string[] distinctArray = new string[length];
            for (int i = 0; i < length; i++)
                distinctArray[i] = inputArray[i].Trim();

            return distinctArray;
        }
        //>>
        //<<Added by Ankur Tyagi on 18Jan2024
        public bool checkDuplicate(string strtableName, string strcolName, string strVal1, string strCondition2 = null)
        {
            DataServer d = new DataServer();
            string Qry = "SELECT * FROM " + strtableName + " WHERE 1 = 1 ";
            if (!string.IsNullOrEmpty(strcolName) && !string.IsNullOrEmpty(strcolName))
            {
                Qry += " AND " + strcolName + " = '" + strVal1 + "'";
            }
            if (!string.IsNullOrEmpty(strCondition2))
            {
                Qry += strCondition2;
            }
            DataTable dt = d.Getdata(Qry);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}