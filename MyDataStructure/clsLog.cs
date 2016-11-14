/******************************************************************************** 
** Copyright(c) 2014 All Rights Reserved. 
** auther： kerwin
** mail： kerwin.cn@gmail.com
** Created： 2014/12/20 9:45:44 
** Compiler: Visual Studio 2010
** 命名空间 : Xuhengxiao.MyDataStructure
** Version : V1.0.0 
*********************************************************************************/
using System;
using System.Collections.Generic;
////using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Xuhengxiao.MyDataStructure
{
    /// <summary>
    /// 这个类仅仅是为了输出记录的
    /// </summary>
    public  class clsLog
    {
        /// <summary>
        /// 是否打开日志
        /// </summary>
        public bool isOpen { get; set; }
        /// <summary>
        /// 日志的文件名
        /// </summary>
        public string strLogFileName { get; set; }

        public clsLog()
        {
            isOpen = true;
        }

        /// <summary>
        /// 构造函数，参数问文件名
        /// </summary>
        /// <param name="strFileName"></param>
        public clsLog(string strFileName)
        {
            strLogFileName = strFileName;
            isOpen = true;
        }

        /// <summary>
        /// 输出一行，
        /// </summary>
        /// <param name="strLine"></param>
        public void writeLine(string strLine)
        {
            if (! isOpen)
            {
                return;//没有打开日志就返回啦
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(Application.StartupPath + "\\"+strLogFileName, true))
                {
                    sw.WriteLine(strLine);
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
        /// 输出一行，参数是文件名和行信息
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="strLine"></param>
        public void writeLine(string strFileName, string strLine)
        {
            if (!isOpen)
            {
                return;//没有打开日志就返回啦
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(strFileName, true))
                {
                    sw.WriteLine(strLine);
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message);
                //throw;
            }
        }
    }
}
