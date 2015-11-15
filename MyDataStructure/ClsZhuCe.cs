using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Xuhengxiao.MyDataStructure
{
     public  class ClsZhuCe
    {
         public static bool isZhuCe = false;

         /// <summary>
         /// 读取机器码的
         /// </summary>
         public static void setStrCode()
         {

             #region
             //这个方法是在程序运行的时候验证是否已经注册了。

             //首先获得机器码。
             string strCode = (new Xuhengxiao.Hardware.clsHardwareInfo()).GetPcCode();

             //在读取保存的注册码
             string strKey = "";
             if (File.Exists(Application.StartupPath + "\\key.txt"))
             {
                 using (StreamReader sr = new StreamReader(Application.StartupPath + "\\key.txt"))
                 {
                     strKey = sr.ReadToEnd();
                 }

                 if (strCode == null)
                 {
                     strCode = "";
                 }

                 //解码注册码
                 string strJieMa = "";

                 foreach (string str in mySplit(strKey, 172))
                 {
                     strJieMa += Decrypt(str);
                 }


                 //验证字符串是否相同
                 if (strCode.Trim().Replace(" ", "") == strJieMa.Trim().Replace(" ", ""))
                 {
                     isZhuCe = true;

                 }
             }


             #endregion
         }

         private static string Decrypt(string base64code) //解密
         {
             try
             {

                 //Create a UnicodeEncoder to convert between byte array and string.
                 UnicodeEncoding ByteConverter = new UnicodeEncoding();

                 //Create a new instance of RSACryptoServiceProvider to generate
                 //public and private key data.
                 RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                 string strPrivateKey = @"<RSAKeyValue><Modulus>qnfhMY6XO+fJDmd84nbAyH51xR3gb8ow7GWr3RPl172sYnCCTprCgSg2Y7HexH43p38WHk6bR1hdkic2cYIcz7gqrLs3CsY/YlxljJQ0MGjfeK+OY1L2tB482cE/wjVKAbCG5J+4vzo13S+whKHxsvlkGRM5KpDHyd0ZnE37V8k=</Modulus><Exponent>AQAB</Exponent><P>7W3IhAKh8njPL4XeIf9xjX2HqIgWUS1aIcIEr7bXY5ey53aw47yfkixSudeSZolJMPpGC+GO6hIyEmznlB63iw==</P><Q>t81LaijAd3Utn7xX/QQ/x9c8ijWgyeWQVWyA4F+7Ay6O5Ztke4ufJq6VFslpI0CDe4DUrp2gBtqEAjN/XZB4ew==</Q><DP>UtX3nF8Sw3b0yh7JdlEZ/ARs3RbFuoK5LIf1fJytHxkhGPJnGr2Hasc+AYq9kDqbp5PZ9nE2nGHGyHjoftwMqw==</DP><DQ>Uzx+TZoc5zxCqBcURbnZ5HddrD1zDluOzJCxoGrZ9yvrfKGtlKF7NnpTfBlEKrm5kYGbT2SEpvXoWFLX+BhH5w==</DQ><InverseQ>xKYnwi/1O57Na9fS0GJHxy5/BXdEwqZ7KSeZsftFxrUiO60meb5yFN6MnGANE0A6pqf0tBLgciK8muJVYg7Tsg==</InverseQ><D>Qc7NrKfzUjkEsP7ag0J84emP5WzHO+C+SkRluI755/NdHRN5+oZcGChB9vKvoQNo0MyK6WBHKZ+/X7Crn94u6I7+1+owWeppsd5uie3rruMIZOzaUeGxmiNXsMDZuY7r5aQVb/zccX9+ccMk6DPfE1UVjTsLcUwg8t4tjJ/49lE=</D></RSAKeyValue>";

                 RSA.FromXmlString(strPrivateKey);

                 byte[] encryptedData;
                 byte[] decryptedData;
                 encryptedData = Convert.FromBase64String(base64code);

                 //Pass the data to DECRYPT, the private key information 
                 //(using RSACryptoServiceProvider.ExportParameters(true),
                 //and a boolean flag specifying no OAEP padding.
                 decryptedData = RSADecrypt(encryptedData, RSA.ExportParameters(true), false);



                 //Display the decrypted plaintext to the console. 
                 return ByteConverter.GetString(decryptedData);
             }
             catch (Exception exc)
             {
                 //Exceptions.LogException(exc);
                 ////ClsErrorFile.WriteLine(exc.Message);
                 //ClsErrorFile.WriteLine("", exc);
                 //Console.Error.WriteLine(exc.Message);
                 return "";
             }
         }

         private static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
         {
             try
             {
                 //Create a new instance of RSACryptoServiceProvider.
                 RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                 //Import the RSA Key information. This needs
                 //to include the private key information.
                 RSA.ImportParameters(RSAKeyInfo);

                 //Decrypt the passed byte array and specify OAEP padding.  
                 //OAEP padding is only available on Microsoft Windows XP or
                 //later.  
                 return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
             }
             //Catch and display a CryptographicException  
             //to the console.
             catch (CryptographicException e)
             {
                 //Exceptions.LogException(e);
                 //Console.Error.WriteLine(e.Message);
                 ////ClsErrorFile.WriteLine(e.Message);
                 //ClsErrorFile.WriteLine("", e);
                 return null;
             }

         }



         private static Array mySplit(string str, int splitLength)
         {
             //如下是计算能分割成多少份
             int n = str.Length / splitLength;
             if (str.Length % splitLength != 0)
                 n++;
             string[] strReturn = new string[n];//只是返回n个元素的数组而已。

             string strShengYu = str;//每次截取后剩下的字符串。
             for (int i = 0; i < n; i++)
             {
                 int intJieQuQty = splitLength;
                 if (strShengYu.Length < splitLength)
                     intJieQuQty = strShengYu.Length;//如果剩余的字符串不够截取的，就只是截取剩余的长度
                 strReturn[i] = strShengYu.Substring(0, intJieQuQty);
                 strShengYu = strShengYu.Remove(0, intJieQuQty);
             }


             return strReturn;
         }

    }
}
