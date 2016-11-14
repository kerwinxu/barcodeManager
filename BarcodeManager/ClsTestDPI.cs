using System.Windows.Forms;
using System.Drawing.Printing;
using System.Xml;
using System.IO;
using System;
using Xuhengxiao.MyDataStructure;



namespace BarcodeTerminator
{
    //这个类不需要窗体
    class ClsTestDPI
    {
        //这个类的构造函数是一个打印机名称
        public  ClsTestDPI(string strPrintName)
        {
            try
            {
                //这个测试分辨率就是用打印一张纸的方式来测试的

                PrintDocument myPrintDoc = new PrintDocument();
                myPrintDoc.PrintController = new StandardPrintController();//这个据说可以不显示那个打印进度对框框
                myPrintDoc.DocumentName = "测试分辨率";
                myPrintDoc.PrinterSettings.PrinterName = strPrintName;//还有设置打印机  
                myPrintDoc.PrintPage += new PrintPageEventHandler(myPrintDoc_PrintPage);

                myPrintDoc.Print();
            }
            catch (Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
                //TextWriter errorWriter = Console.Error;
                //errorWriter.WriteLine(ex.Message);
            }

        }

        void myPrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {

            //分辨率可以从e中得到
            /**


            //保存到xmlAPP.xml中
            //首先判断文件是否在，如果不在则新建
            if (!File.Exists(Application.StartupPath + "\\xmlAPP.xml"))
            {

                //createXmlApp();//如下是createXmlApp的实现
                // 首先创建文件
                FileStream fs = File.Create(Application.StartupPath + "\\xmlAPP.xml");
                fs.Close();//关闭文件

                //创建 XmlDocument 以便操作
                XmlDocument xmlDoc = new XmlDocument();

                //xml 唯一的根，我设置的根都是root 
                XmlElement xmlEleRoot = xmlDoc.CreateElement("root");

                // 这个配置还有一个是必须的，就是打印机的DPI, 其他的都不是必要的
                XmlElement xmlElePrinter = xmlDoc.CreateElement("printer");

                //打印机名称，这里直接设置默认打印机
                PrintDocument printDoc = new PrintDocument();
                xmlElePrinter.SetAttribute("PrinterName", printDoc.PrinterSettings.PrinterName);
                printDoc.Dispose();//释放资源

                //DPI有两个
                xmlElePrinter.SetAttribute("DPIX", "600");
                xmlElePrinter.SetAttribute("DPIY", "600");

                //如下是两个添加操作了
                xmlEleRoot.AppendChild(xmlElePrinter);
                xmlDoc.AppendChild(xmlEleRoot);

                //保存操作
                xmlDoc.Save(Application.StartupPath + "\\xmlAPP.xml");
            }

            XmlDocument xmlDoc2 = new XmlDocument();

            xmlDoc2.Load(Application.StartupPath + "\\xmlAPP.xml");

            XmlElement xmlElePrinter2 = (XmlElement)xmlDoc2.SelectNodes("//printer").Item(0);
            xmlElePrinter2.SetAttribute("DPIX", e.Graphics.DpiX.ToString());
            xmlElePrinter2.SetAttribute("DPIY", e.Graphics.DpiY.ToString());

            xmlDoc2.Save(Application.StartupPath + "\\xmlAPP.xml");

            e.Cancel = true;
            e.HasMorePages = false;
            //throw new System.NotImplementedException();
             * */
        }
    }
}
