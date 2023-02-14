using Microsoft.VisualStudio.TestTools.UnitTesting;
using Io.Github.Kerwinxu.LibShapes.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Io.Github.Kerwinxu.LibShapes.Core.Serialize;

namespace Io.Github.Kerwinxu.LibShapes.Core.Tests
{
    [TestClass()]
    public class ShapesTests
    {
        [TestMethod()]
        public void DeepCloneTest()
        {
            // 先测试一个空白的Shape
            Shapes shapes = new Shapes();
            JsonSerialize jsonSerialize = new JsonSerialize();
            string json = jsonSerialize.SerializeObject(shapes);
            var shapes2 = jsonSerialize.DeserializeObject<Shapes>(json);
            Assert.AreNotEqual(shapes2, null);
            //Assert.Fail();
        }
    }
}