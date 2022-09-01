using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace MyDataStructureTests
{
    [TestClass()]
    public class BarcodePrintImplTest
    {
        [TestMethod()]
        public void PrintDocument_PrintPageTest()
        {
            // 这个是测试能否取得PrintDocument对象的
            PrintDocument printDocument = new PrintDocument();
            printDocument.DocumentName = "哈哈";
            printDocument.PrintController = new StandardPrintController();
            printDocument.PrintPage += PrintDocument_PrintPage1;

            printDocument.Print();
            printDocument.Dispose();
        }

        private void PrintDocument_PrintPage1(object sender, PrintPageEventArgs e)
        {
            PrintDocument printDocument = sender as PrintDocument;
            Assert.AreEqual(printDocument.DocumentName, "哈哈");
            //throw new NotImplementedException();
            e.Cancel = true;
        }
    }
}
