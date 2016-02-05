using System.IO;
using System;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;


namespace BarcodeTerminator
{
    //这个类只是为了操作xmlApp比较方便建立的
    [Serializable]
    public  class ClsXmlApp
    {
        public string strPrintingName;//打印机名字

        public ArrayList arrlistBarcodeModel;

        public void addBarcodeModel(string strName)
        {
            if (arrlistBarcodeModel == null)
                arrlistBarcodeModel = new ArrayList();
            arrlistBarcodeModel.Add(strName);
        }


        public  ClsXmlApp()
        {
            /**
            //这个类的构造函数就是判断是否有文件xmlApp.xml，如果不存在就新建
            if (!File.Exists(Application.StartupPath + "\\xmlAPP.xml"))
            {
                createXmlApp();

            }
             * */
        }

        //如下的这个方法可以注释掉，没有任何的作用
        public void createXmlApp()
        {
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

        //我将这个对xmlApp的操作全改为属性操作。

    }

}
