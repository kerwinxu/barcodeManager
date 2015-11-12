using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Xuhengxiao.MyDataStructure
{
    /// <summary>
    /// 这个只是我的XML序列化的类，
    /// </summary>
    public class ClsXmlSerialization
    {
        /// <summary>
        /// 将MYObject序列化保存到XML文件，类型为T，
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strFileName"></param>
        /// <param name="MyObject"></param>
        /// <returns></returns>
        public static bool Save<T>(string strFileName, T MyObject) where T : class
        {
            //就是保存myShapes这个
            try
            {
                using (Stream stream = new FileStream(strFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    XmlSerializer xmls = new XmlSerializer(typeof(T));
                    xmls.Serialize(stream, MyObject);
                    return true;//成功返回true;

                }

            }
            catch (Exception exception)
            {
                ClsErrorFile.WriteLine("保存XML不成功，原因是", exception);
                //MessageBox.Show("保存不成功，原因是" + exception.Message);

                if (exception.InnerException != null)
                    ClsErrorFile.WriteLine("保存XML不成功" + exception.InnerException.Message);
                //MessageBox.Show(exception.InnerException.Message);
            }
            return false;//意味着失败了。
        }



        /// <summary>
        /// 从指定文件反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static T Load<T>(string strFileName) where T : class
        {
            try
            {
                
                using (Stream stream = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {

                    XmlSerializer formatter = new XmlSerializer(typeof(T));

                    T tReturn = formatter.Deserialize(stream) as T;
                    return tReturn;
                }

            }
            catch (Exception exception)
            {
                ClsErrorFile.WriteLine("加载XML不成功，原因是", exception);
                //MessageBox.Show("加载不成功，原因是" + exception.Message);
                return null;
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
                ClsErrorFile.WriteLine("不能深度复制，原因是:", ex);
            }
            return null;

        }
    }
}
