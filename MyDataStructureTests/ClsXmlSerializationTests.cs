using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xuhengxiao.MyDataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Io.Github.Kerwinxu.BarcodeManager.ClsBarcodePrint;
using VestShapes;

namespace Xuhengxiao.MyDataStructure.Tests
{
    [TestClass()]
    public class ClsXmlSerializationTests
    {
        [TestMethod()]
        public void DeepCopyTest()
        {
            // 这个是测试 PrintItem 可不可以进行赋值
            PrintItem printItem = new PrintItem();
            printItem.Shapes = new Shapes(); // 空白的模板
            printItem.Arr2ListRow = new List<List<clsKeyValue>>();
            printItem.Arr2ListRow.Add(new List<clsKeyValue>());
            printItem.Arr2ListRow[0].Add(new clsKeyValue("key", "value"));
            printItem.TableName = "表";
            printItem.PrintCount = 1;

            PrintItem printItem1 = ClsXmlSerialization.DeepCopy<PrintItem>(printItem);
            Assert.AreEqual(printItem.PrintCount, printItem1.PrintCount);
            Assert.AreEqual(printItem1.Arr2ListRow.Count, printItem1.Arr2ListRow.Count);

            //Assert.Fail();
        }
    }
}