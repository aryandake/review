using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for UploadedFileContentCheck
/// </summary>
public class UploadedFileContentCheck
{
    public UploadedFileContentCheck()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static bool IsExeFile(byte[] FileContent)
    {
        byte[] twoBytes = SubByteArray(FileContent, 0, 2);
        return ((Encoding.UTF8.GetString(twoBytes) == "MZ") || (Encoding.UTF8.GetString(twoBytes) == "ZM"));
    }
    private static byte[] SubByteArray(byte[] data, int index, int length)
    {
        byte[] result = new byte[length];
        Array.Copy(data, index, result, 0, length);
        return result;
    }
    //<< Added by ramesh more on 13-Mar-2024 CR_1991
    public static bool checkForMaliciousFileFromHeaders(string ext, byte[] fileBytes)
    {
        bool isFileMalicious = true;

        ext = ext.ToUpper();

        List<HeaderCheckListValue> headerList = new List<HeaderCheckListValue>();
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".JPG", HeaderBytes = new byte[] { 0xFF, 0xD8, 0xFF, 0xDB } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".JPG", HeaderBytes = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".JPG", HeaderBytes = new byte[] { 0xFF, 0xD8, 0xFF, 0xEE } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".JPG", HeaderBytes = new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".JPEG", HeaderBytes = new byte[] { 0xFF, 0xD8, 0xFF, 0xDB } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".JPEG", HeaderBytes = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".JPEG", HeaderBytes = new byte[] { 0xFF, 0xD8, 0xFF, 0xEE } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".JPEG", HeaderBytes = new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".PNG", HeaderBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".TIF", HeaderBytes = new byte[] { 0x4D, 0x4D, 0x00, 0x2A } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".TIF", HeaderBytes = new byte[] { 0x49, 0x49, 0x2A, 0x00 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".TIFF", HeaderBytes = new byte[] { 0x4D, 0x4D, 0x00, 0x2A } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".TIFF", HeaderBytes = new byte[] { 0x49, 0x49, 0x2A, 0x00 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".GIF", HeaderBytes = new byte[] { 0x47, 0x49, 0x46, 0x38 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".BMP", HeaderBytes = new byte[] { 0x42, 0x4D } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".ICO", HeaderBytes = new byte[] { 0x00, 0x00, 0x01, 0x00 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".PDF", HeaderBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".XLS", HeaderBytes = new byte[] { 0xD0, 0xCF, 0x11, 0xE0 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".XLSX", HeaderBytes = new byte[] { 0x50, 0x4B, 0x03, 0x04 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".XLSX", HeaderBytes = new byte[] { 0x50, 0x4B, 0x05, 0x06 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".XLSX", HeaderBytes = new byte[] { 0x50, 0x4B, 0x07, 0x08 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".DOC", HeaderBytes = new byte[] { 0xD0, 0xCF, 0x11, 0xE0 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".DOCX", HeaderBytes = new byte[] { 0x50, 0x4B, 0x03, 0x04 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".DOCX", HeaderBytes = new byte[] { 0x50, 0x4B, 0x05, 0x06 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".DOCX", HeaderBytes = new byte[] { 0x50, 0x4B, 0x07, 0x08 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".PPT", HeaderBytes = new byte[] { 0xD0, 0xCF, 0x11, 0xE0 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".PPTX", HeaderBytes = new byte[] { 0x50, 0x4B, 0x03, 0x04 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".PPTX", HeaderBytes = new byte[] { 0x50, 0x4B, 0x05, 0x06 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".PPTX", HeaderBytes = new byte[] { 0x50, 0x4B, 0x07, 0x08 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".WAV", HeaderBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".ZIP", HeaderBytes = new byte[] { 0x50, 0x4B, 0x03, 0x04 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".ZIP", HeaderBytes = new byte[] { 0x50, 0x4B, 0x05, 0x06 } });
        headerList.Add(new HeaderCheckListValue() { ExtensionName = ".ZIP", HeaderBytes = new byte[] { 0x50, 0x4B, 0x07, 0x08 } });

        List<HeaderCheckListValue> SearchheaderList = new List<HeaderCheckListValue>();

        SearchheaderList = headerList.Where(x => x.ExtensionName == ext).ToList();

        if (SearchheaderList.Count > 0)
        {
            foreach (HeaderCheckListValue item in SearchheaderList)
            {
                byte[] header, tempHeader;

                byte[] tmp = item.HeaderBytes;
                header = new byte[tmp.Length];
                tempHeader = fileBytes;

                for (int i = 0; i < tmp.Length; i++)
                {
                    header[i] = tempHeader[i];
                }

                if (CompareArray(tmp, header))
                {
                    isFileMalicious = false;
                    break;
                }
            }
        }
        else
        {
            isFileMalicious = false;
        }

        return isFileMalicious;
    }
    public class HeaderCheckListValue
    {
        public string ExtensionName { get; set; }
        public byte[] HeaderBytes { get; set; }
    }
    private static bool CompareArray(byte[] a1, byte[] a2)
    {
        if (a1.Length != a2.Length)
            return false;
        for (int i = 0; i < a1.Length; i++)
        {
            if (a1[i] != a2[i])
                return false;
        }

        return true;
    }

    public static string checkValidFileForUpload(FileUpload objUpload, string strFileType = null)
    {
        bool blnIsValid, blnIsExeFile;
        byte[] fileData;
        string strReturnValue = "";

        //<< This is to check valid file extensions for file uploaded
        if (strFileType.Equals("EXCEL"))
            blnIsValid = IsValidExcelFile(objUpload.PostedFile.FileName);
        else if (strFileType.Equals("CSV"))
            blnIsValid = IsValidCSVFile(objUpload.PostedFile.FileName);
        else
            blnIsValid = IsValidFile(objUpload.PostedFile.FileName);

        if (!blnIsValid)
        {
            strReturnValue = "The upload of this file type is not supported. ";
        }
        //>>

        //<< Check whether any malicious file is uploaded or not
        System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(objUpload.PostedFile.InputStream);
        fileData = binaryReader.ReadBytes(objUpload.PostedFile.ContentLength);
        blnIsExeFile = IsExeFile(fileData);
        if (blnIsExeFile)
        {
            strReturnValue = "The file you're trying to upload seems to contain malicious content" +
                            " and cannot be uploaded. ";
        }
        //>>

        return strReturnValue;
    }

    public static bool checkForMultipleExtention(string strFileName)
    {
        bool blnIsFileMalicious = false;
        string strExtraExtension = "";

        strExtraExtension = Path.GetExtension(Path.GetFileNameWithoutExtension(strFileName));

        if (!strExtraExtension.Equals(""))
        {
            blnIsFileMalicious = true;
        }

        return blnIsFileMalicious;
    }
    //>>
    public static bool IsValidFile(string FileName)
    {
        string[] validFileTypes ={ "bmp", "gif", "png", "jpg", "jpeg", "doc", "docx",
        "xls", "xlsx",  "xlsm","pdf", "zip", "pptx", "txt", "html" ,"rar","ppt","tif","htm"
        ,"msg","MSG","mp4","MP4","eml","EML","avi","AVI","flv","FLV","jpg","JPG","bmp",
        "BMP","xls","XLS","xlsx","XLSX","DOC","DOCX","docx","doc","pdf",
        "PDF","html","htm","HTML","HTM","xml","XML","mht","MHT","mhtml","MHTML","tif","TIF",
        "ZIP","zip","txt","TXT","ppt","pptx","PPT","PPTX","pps","ppsx",
        "gif","GIF","png","PNG","mp3","MP3","wav","WAV","3gp","3GP","vob","VOB",
        "wmv","WMV","m4p","M4P","mpeg","MPEG","zip","rar","csv","CSV","7z"};

        string ext = System.IO.Path.GetExtension(FileName);
        ext = ext.ToLower();
        bool isValidFile = false;
        for (int i = 0; i < validFileTypes.Length; i++)
        {
            if (ext == "." + validFileTypes[i])
            {
                isValidFile = true;
                break;
            }
        }
        //<<<Commented By Supriya Patil on 06Jun2016
        //intNoOfDots = CommonMethods.countStringOccurrences(FileName, ".");

        //if(intNoOfDots > 1)
        //{
        //    isValidFile = false;
        //}
        //>>

        return isValidFile;
    }
    public static bool IsValidExcelFile(string FileName)
    {
        string[] validFileTypes = { "xls", "xlsx" };
        string ext = System.IO.Path.GetExtension(FileName);
        ext = ext.ToLower();
        bool isValidFile = false;
        for (int i = 0; i < validFileTypes.Length; i++)
        {
            if (ext == "." + validFileTypes[i])
            {
                isValidFile = true;
                break;
            }
        }
        //<<<Commented By Supriya Patil on 06Jun2016
        /* intNoOfDots = CommonMethods.countStringOccurrences(FileName, ".");

         if (intNoOfDots > 1)
         {
             isValidFile = false;
         }*/

        return isValidFile;
    }

    public static bool IsValidCSVFile(string FileName)
    {
        string[] validFileTypes = { "csv", "CSV" };
        string ext = System.IO.Path.GetExtension(FileName);
        ext = ext.ToLower();
        bool isValidFile = false;
        for (int i = 0; i < validFileTypes.Length; i++)
        {
            if (ext == "." + validFileTypes[i])
            {
                isValidFile = true;
                break;
            }
        }
        //<<<Commented By Supriya Patil on 06Jun2016
        /*intNoOfDots = CommonMethods.countStringOccurrences(FileName, ".");

        if (intNoOfDots > 1)
        {
            isValidFile = false;
        }*/
        return isValidFile;
    }
}