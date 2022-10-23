using Microsoft.VisualStudio.TestTools.UnitTesting;
using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape.Tests
{
    [TestClass()]
    public class ShapeEleTests
    {
        [TestMethod()]
        public void DeepCloneTest()
        {
            // 这里用一个线段来测试一下
            var line1 = new ShapeLine();
            line1.X = 10;
            line1.Y = 20;
            line1.Width = 30;
            line1.Height = 40;
            var line1_2 = line1.DeepClone();
            Assert.AreEqual<ShapeLine>(line1, (ShapeLine)line1_2);

            //Assert.Fail();
        }
    }
}