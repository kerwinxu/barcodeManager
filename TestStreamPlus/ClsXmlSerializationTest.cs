using Xuhengxiao.StreamPlus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Xml.Serialization;

namespace TestStreamPlus
{
    /// <summary>
    /// 这个是用来测试对象的序列化的
    /// </summary>
    [Serializable]
    public class cls1
    {
        /// <summary>
        /// 这个类中有几个
        /// </summary>
        [XmlElement]
        public string attr1;

    }
    
    
    /// <summary>
    ///这是 ClsXmlSerializationTest 的测试类，旨在
    ///包含所有 ClsXmlSerializationTest 单元测试
    ///</summary>
    [TestClass()]
    public class ClsXmlSerializationTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///ClsXmlSerialization 构造函数 的测试
        ///</summary>
        [TestMethod()]
        public void ClsXmlSerializationConstructorTest()
        {
            ClsXmlSerialization target = new ClsXmlSerialization();
            //Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }

        [TestMethod()]
        public void StreamObjTest()
        {
            //这个是测试stream和对象的两个方法，包括一个Read和write
            string strAttr1Value = "attr1 value";
            cls1 c1 = new cls1();
            c1.attr1 = strAttr1Value;
            cls1 c2 = null;
            string strFileName = "StreamObjTest.test";
            Stream stream1 = new FileStream(strFileName, FileMode.Create, FileAccess.Write, FileShare.None);
            string s1= ClsXmlSerialization.Write<cls1>(ref stream1, c1);
            stream1.Close();
            Stream stream2 = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            string s2=ClsXmlSerialization.Read<cls1>(stream2,out c2);
            stream2.Close();
            Assert.AreEqual(c1.attr1,c2.attr1);
            Assert.AreEqual(strAttr1Value, c2.attr1);
        }
        /// <summary>
        /// 这个方法是测试文件和对象之间的转换的，实践证明，这个文件转换比那个流和对象转换更省时间。
        /// </summary>
        [TestMethod()]
        public void FileOjbTest()
        {
            string strAttr1Value = "attr1 value";
            cls1 c1 = new cls1();
            c1.attr1 = strAttr1Value;
            cls1 c2 = null;
            string strFileName = "FileObjTest.test";
            ClsXmlSerialization.Write<cls1>(strFileName, c1);
            ClsXmlSerialization.Read<cls1>(strFileName, out c2);
            Assert.AreEqual(c1.attr1, c2.attr1);
            Assert.AreEqual(strAttr1Value, c2.attr1);

        }

    }
}
