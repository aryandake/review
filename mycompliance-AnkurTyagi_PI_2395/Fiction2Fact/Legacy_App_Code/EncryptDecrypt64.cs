using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;


/// <summary>
/// Summary description for EncryptDecrypt64
/// </summary>
public class EncryptDecrypt64
{
	public EncryptDecrypt64()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string Encryption(string str)
    {
        // == Encode then decode 64 test. DecPass64 should equal password == //

        // Encodes to Base64 using ToBase64Transform
        string EncPass64 = Base64Helper.EncodeString(str);

        // Decodes a Base64 string using FromBase64Transform
       // string DecPass64 = Base64Helper.DecodeString(EncPass64);

        // Test if base 64 ecoding / decoding works
        //Worked = (Password == DecPass64);
        //Console.WriteLine();
        //Console.WriteLine("Base64 Pass Encoded: " + EncPass64);
        //Console.WriteLine("Base64 Pass Decoded: " + DecPass64);
        //Console.WriteLine("Base64 Encode to Base64 Decode Worked? : " + Worked); // True

        //// gspassenc uses XOR to switch passwords back and forth between encrypted and decrypted
        //string GsEncodedPass = gspassenc(Password);
        //string GsDecodedPass = gspassenc(GsEncodedPass);
        //Worked = (Password == GsDecodedPass);

        //// GsDecodedPass should equal the original Password
        //Console.WriteLine();
        //Console.WriteLine("GsPass Encoded: " + GsEncodedPass);
        //Console.WriteLine("GsPass Decoded: " + GsDecodedPass);
        //Console.WriteLine("GsEncode to GsDecode Worked? : " + Worked); // True

        //// Bas64 encode the encrypted password. Then decode the base64. B64_GsDecodedPass should equal
        //// the GsEncoded Password... But it doesn't for some reason!
        //string B64_GsEncodedPass = Base64Helper.EncodeString(GsEncodedPass);
        //string B64_GsDecodedPass = Base64Helper.DecodeString(B64_GsEncodedPass);
        //Worked = (B64_GsDecodedPass == GsEncodedPass);

        //// Print results
        //Console.WriteLine();
        //Console.WriteLine("Base64 Encoded GsPass: " + B64_GsEncodedPass);
        //Console.WriteLine("Base64 Decoded GsPass: " + B64_GsDecodedPass);
        //Console.WriteLine("Decoded == GS Encoded Pass? : " + Worked); // False

        //// Stop console from closing till we say so
        //Console.Read();

        return EncPass64;
    }

    public string Decryption(string str)
    {
        // == Encode then decode 64 test. DecPass64 should equal password == //

        // Encodes to Base64 using ToBase64Transform

        // Decodes a Base64 string using FromBase64Transform
        string DecPass64 = Base64Helper.DecodeString(str);

        // Test if base 64 ecoding / decoding works
        //Worked = (Password == DecPass64);
        //Console.WriteLine();
        //Console.WriteLine("Base64 Pass Encoded: " + EncPass64);
        //Console.WriteLine("Base64 Pass Decoded: " + DecPass64);
        //Console.WriteLine("Base64 Encode to Base64 Decode Worked? : " + Worked); // True

        //// gspassenc uses XOR to switch passwords back and forth between encrypted and decrypted
        //string GsEncodedPass = gspassenc(Password);
        //string GsDecodedPass = gspassenc(GsEncodedPass);
        //Worked = (Password == GsDecodedPass);

        //// GsDecodedPass should equal the original Password
        //Console.WriteLine();
        //Console.WriteLine("GsPass Encoded: " + GsEncodedPass);
        //Console.WriteLine("GsPass Decoded: " + GsDecodedPass);
        //Console.WriteLine("GsEncode to GsDecode Worked? : " + Worked); // True

        //// Bas64 encode the encrypted password. Then decode the base64. B64_GsDecodedPass should equal
        //// the GsEncoded Password... But it doesn't for some reason!
        //string B64_GsEncodedPass = Base64Helper.EncodeString(GsEncodedPass);
        //string B64_GsDecodedPass = Base64Helper.DecodeString(B64_GsEncodedPass);
        //Worked = (B64_GsDecodedPass == GsEncodedPass);

        //// Print results
        //Console.WriteLine();
        //Console.WriteLine("Base64 Encoded GsPass: " + B64_GsEncodedPass);
        //Console.WriteLine("Base64 Decoded GsPass: " + B64_GsDecodedPass);
        //Console.WriteLine("Decoded == GS Encoded Pass? : " + Worked); // False

        //// Stop console from closing till we say so
        //Console.Read();

        return DecPass64;
    }

    private static int gslame(int num)
    {
        int c = (num >> 16) & 0xffff;
        int a = num & 0xffff;

        c *= 0x41a7;
        a *= 0x41a7;
        a += ((c & 0x7fff) << 16);

        if (a < 0)
        {
            a &= 0x7fffffff;
            a++;
        }

        a += (c >> 15);

        if (a < 0)
        {
            a &= 0x7fffffff;
            a++;
        }

        return a;
    }

    private static string gspassenc(string pass)
    {
        int a = 0;
        int num = 0x79707367; // gspy
        int len = pass.Length;
        char[] newPass = new char[len];

        for (int i = 0; i < len; ++i)
        {
            num = gslame(num);
            a = num % 0xFF;
            newPass[i] = (char)(pass[i] ^ a);
        }

        return new String(newPass);
    }

    class Base64Helper
    {
        public static string DecodeString(string encoded)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(encoded));
        }

        public static string EncodeString(string decoded)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(decoded));
        }
    }
}
