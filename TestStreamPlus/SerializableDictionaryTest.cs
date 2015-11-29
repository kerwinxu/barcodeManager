using Xuhengxiao.StreamPlus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;

namespace TestStreamPlus
{
    
    
    /// <summary>
    ///这是 SerializableDictionaryTest 的测试类，旨在
    ///包含所有 SerializableDictionaryTest 单元测试
    ///</summary>
    [TestClass()]
    public class SerializableDictionaryTest
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
        ///WriteXml 的测试
        ///</summary>
        public void WriteXmlTestHelper<TKey, TValue>()
        {
            SerializableDictionary<TKey, TValue> target = new SerializableDictionary<TKey, TValue>(); // TODO: 初始化为适当的值
            XmlWriter write = null; // TODO: 初始化为适当的值
            target.WriteXml(write);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        [TestMethod()]
        public void WriteReadXmlTest()
        {
            //这个测试方法是这样的，首先创建一个要保存的类，然后写入，然后读取，然后判断是否相同
            //WriteXmlTestHelper<GenericParameterHelper, GenericParameterHelper>();
            SerializableDictionary<string, string> tmp1 = new SerializableDictionary<string, string>();
            SerializableDictionary<string, string> tmp2 = null;
            tmp1.Add("key1", "value1");
            string strFileName = "FileSerializableDictionary.test";
            ClsXmlSerialization.Write<SerializableDictionary<string, string>>(strFileName, tmp1);
            ClsXmlSerialization.Read<SerializableDictionary<string, string>>(strFileName, out tmp2);
            Assert.AreEqual(tmp1["key1"], tmp2["key1"]);
        }
    }
}
