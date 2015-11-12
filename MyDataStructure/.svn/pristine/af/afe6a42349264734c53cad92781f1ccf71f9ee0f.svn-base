using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Xuhengxiao.MyDataStructure
{
    /// <summary>
    /// 这个类只是将错误流输出到文件而已。
    /// </summary>
    public  class ClsErrorFile
    {
        /// <summary>
        /// 将信息保存到文件，调试用的。
        /// </summary>
        /// <param name="strError"></param>
        public static void WriteLine(string strError)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Application.StartupPath + "\\error.txt",true))
                {
                    sw.WriteLine(DateTime.Now.ToString("s")+"  :  " + strError);
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message);
                //throw;
            }
        }

        public static void WriteLine(String strError, Exception exception)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Application.StartupPath + "\\error.txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString("s") + "\t:\t" + strError +"\tSource :\t"+exception.Source+ "\tTargetsite : \t"+exception.TargetSite.ToString()+"\tMessage : \t"+exception.Message);
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message);
                //throw;
            }
        }
        public static void WriteLine( Exception exception)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Application.StartupPath + "\\error.txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString("s") + "\t:\t" + "\tSource :\t" + exception.Source + "\tTargetsite : \t" + exception.TargetSite.ToString() + "\tMessage : \t" + exception.Message);
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message);
                //throw;
            }
        }

        /// <summary>
        /// 这个方法只是删除前面的记录，只保留一万行的数据
        /// </summary>
        public static void DeleteBefore()
        {
            //首先取得这个文件的行数

            int iLine=0;

            try
            {
                using (StreamReader sr = new StreamReader(Application.StartupPath + "\\error.txt", System.Text.Encoding.Default))
                {
                    string  strFile = sr.ReadToEnd();
                    string[] arraFile = strFile.Split('\n');//or '\r'，仅限于读取Windows下的文件

                    iLine = arraFile.Length;//文件行数
                }


            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                //throw;
            }


            iLine = iLine - 10000;
            //行数减去1万。


            //如果大于零就调用删除的

            if (iLine > 0)
            {
                try
                {
                    string[] lines = System.IO.File.ReadAllLines(Application.StartupPath + "\\error.txt");

                    System.IO.File.WriteAllText(Application.StartupPath + "\\error.txt", string.Join(Environment.NewLine, lines, iLine, lines.Length - iLine));

                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    //throw;
                }


            }


        }


    }
}
