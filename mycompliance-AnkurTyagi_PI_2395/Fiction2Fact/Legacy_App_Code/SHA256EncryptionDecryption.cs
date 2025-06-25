using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Text;
using System.IO;

/// <summary>
/// Summary description for RSAEncryptionDecryption
/// </summary>

namespace Fiction2Fact.Legacy_App_Code
{
    public class SHA256EncryptionDecryption
    {
        private byte[] _keyByte = { };
        //Default Key
        private static string _key = "f2f@1234";
        //Default initial vector
        private byte[] _ivByte = { 0x01, 0x12, 0x23, 0x34, 0x45, 0x56, 0x67, 0x78 };

        public SHA256EncryptionDecryption()
        {
            //
            // TODO: Add constructor logic here

            //
        }

        /// <summary> 
        /// Encrypt text 
        /// </summary> 
        /// <param name="value">plain text</param> 
        /// <returns>encrypted text</returns> 
        public string Encrypt(string value)
        {
            return Encrypt(value, string.Empty);
        }

        /// <summary> 
        /// Encrypt text by key 
        /// </summary> 
        /// <param name="value">plain text</param> 
        /// <param name="key"> string key</param> 
        /// <returns>encrypted text</returns> 
        public string Encrypt(string value, string key)
        {
            return Encrypt(value, key, string.Empty);
        }

        /// <summary> 
        /// Encrypt text by key with initialization vector 
        /// </summary> 
        /// <param name="value">plain text</param> 
        /// <param name="key"> string key</param> 
        /// <param name="iv">initialization vector</param> 
        /// <returns>encrypted text</returns> 
        public string Encrypt(string value, string key, string iv)
        {
            string encryptValue = string.Empty;
            MemoryStream ms = null;
            CryptoStream cs = null;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        _keyByte = Encoding.UTF8.GetBytes
                                (key.Substring(0, 8));
                        if (!string.IsNullOrEmpty(iv))
                        {
                            _ivByte = Encoding.UTF8.GetBytes
                                (iv.Substring(0, 8));
                        }
                    }
                    else
                    {
                        _keyByte = Encoding.UTF8.GetBytes(_key);
                    }
                    using (DESCryptoServiceProvider des =
                            new DESCryptoServiceProvider())
                    {
                        byte[] inputByteArray =
                            Encoding.UTF8.GetBytes(value);
                        ms = new MemoryStream();
                        cs = new CryptoStream(ms, des.CreateEncryptor
                        (_keyByte, _ivByte), CryptoStreamMode.Write);
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        encryptValue = Convert.ToBase64String(ms.ToArray());
                    }
                }
                catch
                {
                    //TODO: write log 
                }
                finally
                {
                    cs.Dispose();
                    ms.Dispose();
                }
            }
            return encryptValue;
        }

        /// <summary> 
        /// Decrypt text 
        /// </summary> 
        /// <param name="value">encrypted text</param> 
        /// <returns>plain text</returns> 
        public string Decrypt(string value)
        {
            return Decrypt(value, string.Empty);
        }

        /// <summary> 
        /// Decrypt text by key 
        /// </summary> 
        /// <param name="value">encrypted text</param> 
        /// <param name="key">string key</param> 
        /// <returns>plain text</returns> 
        public string Decrypt(string value, string key)
        {
            return Decrypt(value, key, string.Empty);
        }

        /// <summary> 
        /// Decrypt text by key with initialization vector 
        /// </summary> 
        /// <param name="value">encrypted text</param> 
        /// <param name="key"> string key</param> 
        /// <param name="iv">initialization vector</param> 
        /// <returns>encrypted text</returns> 
        public string Decrypt(string value, string key, string iv)
        {
            string decrptValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                MemoryStream ms = null;
                CryptoStream cs = null;
                value = value.Replace(" ", "+");
                byte[] inputByteArray = new byte[value.Length];
                try
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        _keyByte = Encoding.UTF8.GetBytes
                                (key.Substring(0, 8));
                        if (!string.IsNullOrEmpty(iv))
                        {
                            _ivByte = Encoding.UTF8.GetBytes
                                (iv.Substring(0, 8));
                        }
                    }
                    else
                    {
                        _keyByte = Encoding.UTF8.GetBytes(_key);
                    }
                    using (DESCryptoServiceProvider des =
                            new DESCryptoServiceProvider())
                    {
                        inputByteArray = Convert.FromBase64String(value);
                        ms = new MemoryStream();
                        cs = new CryptoStream(ms, des.CreateDecryptor
                        (_keyByte, _ivByte), CryptoStreamMode.Write);
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        Encoding encoding = Encoding.UTF8;
                        decrptValue = encoding.GetString(ms.ToArray());
                    }
                }
                catch
                {
                    //TODO: write log 
                }
                finally
                {
                    cs.Dispose();
                    ms.Dispose();
                }
            }
            return decrptValue;
        }

    }
}