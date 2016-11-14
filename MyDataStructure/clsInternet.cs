/******************************************************************************** 
** Copyright(c) 2015 All Rights Reserved. 
** auther： kerwin
** mail： kerwin.cn@gmail.com
** Created： 2015/4/13 21:49:17 
** Compiler: Visual Studio 2010
** 命名空间 : Xuhengxiao.MyDataStructure
** Version : V1.0.0 
*********************************************************************************/
using System;
using System.Collections.Generic;
////using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Globalization;


namespace Xuhengxiao.MyDataStructure
{
    public  class clsInternet
    {
        /// <summary>
        /// 获取网页的HTML码,这个是设置编码
        /// </summary>
        /// <param name="strUrl">链接地址</param>
        /// <param name="encoding">编码类型</param>
        /// <returns></returns>
        public static string GetHtmlFromInternet3(string strUrl)
        {
            string htmlStr = "";
            string strCharset = null;
            if (!String.IsNullOrEmpty(strUrl))
            {

                try
                {

                    WebRequest request = WebRequest.Create(strUrl);            //实例化WebRequest对象
                    //创建WebResponse对象
                    //用两次流，第一次输出，第二次是取得判断编码后的输出
                    using (WebResponse response = request.GetResponse())
                    {

                        using (Stream datastream = response.GetResponseStream())
                        {

                            //用两次流，第一次输出，第二次是取得判断编码后的输出
                            using (StreamReader reader = new StreamReader(datastream))
                            {
                                htmlStr = reader.ReadToEnd();                           //读取数据
                            }
                           

                            //datastream.Close();
                        }



                        //response.Close();
                    }


                }
                catch (Exception ex)
                {
                    //ClsErrorFile.WriteLine("GetHtmlFromInternet:" + ex.Message);
                    return string.Empty;
                    //throw;
                }

            }
            return htmlStr;
        }
        /// <summary>
        /// 获取网页的HTML码,这个会自动判断网页编码
        /// </summary>
        /// <param name="strUrl">链接地址</param>
        /// <param name="encoding">编码类型</param>
        /// <returns></returns>
        public static string GetHtmlFromInternet2(string strUrl)
        {
            string htmlStr = "";
            Encoding encoding = Encoding.Default;
            if (!String.IsNullOrEmpty(strUrl))
            {
                try
                {
                    WebRequest request = WebRequest.Create(strUrl);            //实例化WebRequest对象
                    //创建WebResponse对象
                    using (WebResponse response = request.GetResponse())
                    {
                        //取得编码信息，如果有的话
                        string str_Content_type = response.ContentType;
                        //获得chardet
                        Regex regex = new Regex("charset\\s*=\\s*(\\S+)", RegexOptions.IgnoreCase);
                        Match match = null;
                        if (str_Content_type!=null)
                        {
                            match = regex.Match(str_Content_type);

                            //如果有编码信息
                            if (match.Success)
                            {
                                try
                                {
                                    encoding = Encoding.GetEncoding(match.Groups[1].Value.Trim());
                                }
                                catch (Exception ex)
                                {
                                    //ClsErrorFile.WriteLine("GetHtmlFromInternet2:" + ex.Message);
                                    return null;
                                    //throw;
                                }
                            }
                        }
                        //////如上就取得了编码信息了，如果有的话，如果没有就是默认编码啦
                        //如下是读取啦。
                        try
                        {
                            using (TextReader reader = new StreamReader(response.GetResponseStream(), encoding))
                            {
                                htmlStr = reader.ReadToEnd();

                            }
                        }
                        catch (Exception ex)
                        {
                            //ClsErrorFile.WriteLine("GetHtmlFromInternet2:" + ex.Message);
                            return null;
                            //Console.WriteLine(exx);
                        }

                        //response.Close();
                    }

                }
                catch (Exception ex)
                {
                    //ClsErrorFile.WriteLine("GetHtmlFromInternet:" + ex.Message);
                    return string.Empty;
                    //throw;
                }

            }
            return convertUnicodeToString(htmlStr);
        }
        /// <summary>
        /// 获取网页的HTML码
        /// </summary>
        /// <param name="strUrl">链接地址</param>
        /// <param name="encoding">编码类型</param>
        /// <returns></returns>
        public static string GetHtmlFromInternet(string strUrl)
        {
            string htmlStr = "";
            string strCharset = null;
            if (!String.IsNullOrEmpty(strUrl))
            {

                try
                {

                    WebRequest request = WebRequest.Create(strUrl);            //实例化WebRequest对象
                    //创建WebResponse对象
                    //用两次流，第一次输出，第二次是取得判断编码后的输出
                    using (WebResponse response = request.GetResponse())
                    {
                        
                        using (Stream datastream = response.GetResponseStream())
                        {
                            
                            //用两次流，第一次输出，第二次是取得判断编码后的输出
                            using (StreamReader reader = new StreamReader(datastream))
                            {
                                htmlStr = reader.ReadToEnd();                           //读取数据
                            }
                            strCharset = getCharSet(htmlStr);

                            //datastream.Close();
                        }

                       

                        //response.Close();
                    }

                    //根据编码读取流
                    if (strCharset != null)
                    {
                        request = WebRequest.Create(strUrl);     
                        using (WebResponse response = request.GetResponse())
                        {
                            using (Stream datastream = response.GetResponseStream())
                            {
                                Encoding ec = Encoding.GetEncoding(strCharset);
                                using (StreamReader reader = new StreamReader(datastream, ec))
                                {
                                    htmlStr = reader.ReadToEnd();         //读取数据
                                    //这里的就是编码后的数据啦
                                }

                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    //ClsErrorFile.WriteLine("GetHtmlFromInternet:" + ex.Message);
                    return string.Empty;
                    //throw;
                }

            }
            return htmlStr;
        }

        /// <summary>
        /// 根据html的编码信息自动获得更改编码
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public  static string getCharSet(string strHtml)
        {
            string charSet = null;
            //获取网页字符编码描述信息 
            Match charSetMatch = Regex.Match(strHtml, "<meta([^<]*)charset=\"([^<]*)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string webCharSet = charSetMatch.Groups[2].Value;
            if (charSet == null || charSet == "")
                charSet = webCharSet;

            return charSet; 
        }

        /// <summary>
        /// 这个输入的是关键词，输出的是URL编码形式
        /// </summary>
        /// <param name="strKeyWord"></param>
        /// <returns></returns>
        public static string myUrlEncode(string strKeyWord)
        {
            //我用正则来合并多个空格
            Regex regex = new Regex(@"\s+");
            strKeyWord = regex.Replace(strKeyWord, " ");
            //然后再切割
            string[] arrStr = strKeyWord.Split(' ');
            string[] arrStr2 = new string[arrStr.Length];

            //编码
            for (int i = 0; i < arrStr.Length; i++)
            {
                arrStr2[i] = HttpUtility.UrlEncode(arrStr[i]);
            }

            return string.Join("+", arrStr2);
        }

        /// <summary>
        /// 这个方法就是把unicode数字格式的转化成字符串格式的
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string convertUnicodeToString(string strHtml)
        {
            //我的方法是，不断的查询字符串，发现这个字符串，然后替换
            //   \u751f\u6001\u519c\u4e1a\u5b9d\u8d1d
            string strUnicodeStart = @"\u";
            int int_index_UnicodeStart = 0;
            //这个只是保存旧的值的
            int int_index_UnicodeStart_old = 0;
            string strReturn = string.Empty;

            int_index_UnicodeStart = strHtml.IndexOf(strUnicodeStart);

            
            
            //判断条件只有一个，就是是否查找到这个编码的开头，如果没有查到，表示某个位置后边全部没有unicode编码啦
            while (int_index_UnicodeStart>=0)
            {
                //把前面的加上去
                strReturn += strHtml.Substring(int_index_UnicodeStart_old, int_index_UnicodeStart - int_index_UnicodeStart_old);

                //查找到这个开头并不表示这个字符串就是unicode字符串
                //我只是判断6个字符而已
                string strTmp = strHtml.Substring(int_index_UnicodeStart, 6);
                string strRegex = @"[0-9A-Fa-f]{4}";//匹配中文字符的表达式

                if (Regex.IsMatch(strTmp,strRegex))
                {
                    byte[] bytes = new byte[2];
                    bytes[1] = byte.Parse(int.Parse(strTmp.Substring(2, 2), NumberStyles.HexNumber).ToString());
                    bytes[0] = byte.Parse(int.Parse(strTmp.Substring(4, 2), NumberStyles.HexNumber).ToString());
                    strReturn += Encoding.Unicode.GetString(bytes);
                    int_index_UnicodeStart += 6;//这个是加6个字符
                    //
                }
                else
                {
                    strReturn += strUnicodeStart;
                    int_index_UnicodeStart += 2;//这个只是加2个字符
                }

                //保存旧的值
                int_index_UnicodeStart_old = int_index_UnicodeStart;

                //再次寻找下一个
                int_index_UnicodeStart = strHtml.IndexOf(strUnicodeStart,int_index_UnicodeStart);

            }

            if (int_index_UnicodeStart < 0)
            {
                strReturn += strHtml.Substring(int_index_UnicodeStart_old);
            }

            return strReturn;
        }
    }
}
