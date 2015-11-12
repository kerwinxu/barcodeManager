/******************************************************************************** 
** Copyright(c) 2015 All Rights Reserved. 
** auther： kerwin
** mail： kerwin.cn@gmail.com
** Created： 2015/4/13 21:45:54 
** Compiler: Visual Studio 2010
** 命名空间 : Xuhengxiao.MyDataStructure
** Version : V1.0.0 
*********************************************************************************/
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Xuhengxiao.MyDataStructure
{
    public class clsString
    {
        #region 如下的这个是截取数据的

        /// <summary>
        /// 这个方法是取得一个字符串中，由str2和str3包含的字符串的队列
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="strStart"></param>
        /// <param name="strEnd"></param>
        /// <returns></returns>
        public static List<string> getBetweenString(string str1, string strStart, string strEnd)
        {
            List<string> listReturn = new List<string>();

            int int_index_start = str1.IndexOf(strStart);
            int int_index_end = 0;

            while (int_index_start >= 0)
            {
                //加上这个长度就是包含的字符的第一个啦
                int_index_start += strStart.Length;
                int_index_end = str1.IndexOf(strEnd, int_index_start);
                //这里强制这个是只取包含的
                if (int_index_end >= 0)
                {
                    string strTmp = str1.Substring(int_index_start, int_index_end - int_index_start);
                    listReturn.Add(strTmp);
                }
                int_index_start = str1.IndexOf(strStart, int_index_end + strEnd.Length);//接着看下一个包含的

            }
            return listReturn;
        }

        /// <summary>
        /// 这个只是取得前面的第一个数据
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="strStart"></param>
        /// <param name="strEnd"></param>
        /// <returns></returns>
        public static string getBetweenString1(string str1, string strStart, string strEnd)
        {
            int int_index_start = str1.IndexOf(strStart);

            if (int_index_start < 0)
            {
                return string.Empty;
            }

            int_index_start += strStart.Length;

            int int_index_end = str1.IndexOf(strEnd, int_index_start);

            if (int_index_end >= 0
                && int_index_end > int_index_start)
            {
                return str1.Substring(int_index_start, int_index_end - int_index_start);
            }
            else
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 根据一个字符串分割字符串的,只是用正则分开而已。
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="strSplit"></param>
        /// <returns></returns>
        public static string[] getSplitString(string str1, string strSplit)
        {
            return Regex.Split(str1, strSplit);
        }

        #endregion

        /// <summary>
        /// 将Unicode编码转换为汉字字符串
        /// </summary>
        /// <param name="str">Unicode编码字符串</param>
        /// <returns>汉字字符串</returns>
        public static string ToGB2312(string str)
        {
            string r = "";
            MatchCollection mc = Regex.Matches(str, @"\\u([\w]{2})([\w]{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            byte[] bts = new byte[2];
            foreach (Match m in mc)
            {
                bts[0] = (byte)int.Parse(m.Groups[2].Value, NumberStyles.HexNumber);
                bts[1] = (byte)int.Parse(m.Groups[1].Value, NumberStyles.HexNumber);
                r += Encoding.Unicode.GetString(bts);
            }
            return r;
        }

    }
}
