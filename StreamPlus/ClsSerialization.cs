using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Xuhengxiao.StreamPlus
{
    /// <summary>
    /// 这个类做的事情就是
    /// 文件<<->>流<<->>对象转化的
    /// </summary>
    public class ClsXmlSerialization
    {
        /// <summary>
        /// 在对象和流之间转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="outputStream"></param>
        /// <param name="MyObject"></param>
        /// <returns></returns>
        public static string Write<T>(ref Stream  outputStream, T MyObject) where T : class
        {
            try
            {
                XmlSerializer xmls = new XmlSerializer(typeof(T));
                xmls.Serialize(outputStream, MyObject);
                return null;//成功返回true;

            }
            catch (Exception exception)
            {
                return GetExceptionMsg(exception);
            }
        }

        /// <summary>
        /// 将MYObject序列化保存到XML文件，类型为T，
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strFileName"></param>
        /// <param name="MyObject"></param>
        /// <returns>如果返回null表示成功，如果返回字符串表示错误编码</returns>
        public static string  Write<T>(string strFileName, T MyObject) where T : class
        {
            try
            {
                using (Stream stream = new FileStream(strFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    //这个也是可以调用重载函数来实现，不过这里还是用这个吧。
                    XmlSerializer xmls = new XmlSerializer(typeof(T));
                    xmls.Serialize(stream, MyObject);
                    return null;//成功返回true;
                }

            }
            catch (Exception exception)
            {
                return GetExceptionMsg(exception);
            }
            //return "Write ,failed , but i do not know why . but i do not think the program can  do here";//意味着失败了。

        }


        /// <summary>
        /// 从指定的流读取，并序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="streamInput"></param>
        /// <param name="MyObject"></param>
        /// <returns></returns>
        public static string Read<T>(Stream streamInput, out T MyObject) where T : class
        {
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                MyObject = formatter.Deserialize(streamInput) as T;
                return null;

            }
            catch (Exception exception)
            {
                //设置返回null值
                MyObject = null;
                return GetExceptionMsg(exception);
            }
        }

        /// <summary>
        /// 从指定的文件读取XML文档，并反序列化成对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strFileName"></param>
        /// <param name="MyObject"></param>
        /// <returns></returns>
        public static string Read<T>(string strFileName, out T MyObject) where T : class
        {
            try
            {
                using (Stream stream = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {

                    XmlSerializer formatter = new XmlSerializer(typeof(T));

                    MyObject = formatter.Deserialize(stream) as T;
                    return null;
                }
            }
            catch (Exception exception)
            {
                MyObject = null;
                return GetExceptionMsg(exception);
            }
        }

        /// <summary>
        /// 深度拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myobject"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(T myobject) where T:class
        {
            T TReturn = default(T);//返回的对象,default此关键字对于引用类型会返回空，对于数值类型会返回零
            try
            {
                using (MemoryStream memory3 = new MemoryStream())
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));

                    xs.Serialize(memory3, myobject);//序列化
                    memory3.Seek(0, SeekOrigin.Begin);//移动到开头

                    //反序列化
                    XmlSerializer xs2 = new XmlSerializer(typeof(T));
                    TReturn = xs2.Deserialize(memory3) as T;//这样就深度拷贝了

                    memory3.Close();//销毁

                    return TReturn;
                }
            }
            catch (System.Exception ex)
            {

            }
            return null;

        }

        /// <summary>
        /// 这个方法只是取得异常的信息而已
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static string GetExceptionMsg(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return ex.Message + ":" + ex.InnerException;
            }
            else
            {
                return ex.Message;
            }
        }
    }
}
