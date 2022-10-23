using Microsoft.VisualStudio.TestTools.UnitTesting;
using Io.Github.Kerwinxu.LibShapes.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Io.Github.Kerwinxu.LibShapes.Core.Command.Tests
{
    [TestClass()]
    public class CommandRecorderTests
    {
        [TestMethod()]
        public void UndoTest()
        {
            // 这个是测试一下是否可以有undo和redo
            // 1. 先有画布
            UserControlCanvas canvas = new UserControlCanvas();
            // 2. 然后新建一个线段
            Shape.ShapeLine line = new Shape.ShapeLine() { 
                X=10,
                Y=20,
                Width=30,
                Height=40
            };
            //手动添加进去
            canvas.addShape(line);
            // 需要手动建立这个新建命令
            canvas.commandRecorder.addCommand(
                new Command.CommandCreate() {
                    canvas = canvas,
                    NewShape=line
                });
            // 3. 添加一个矩形
            Shape.ShapeRectangle rect = new Shape.ShapeRectangle()
            {
                X = 50,
                Y = 60,
                Width = 70,
                Height = 80
            };
            //手动添加进去
            canvas.addShape(rect);
            // 需要手动建立这个新建命令
            canvas.commandRecorder.addCommand(
                new Command.CommandCreate()
                {
                    canvas = canvas,
                    NewShape = rect
                });
            // 4. 删除最后一个
            canvas.changeSelect(rect);
            canvas.deleteShapes();
            // 5. 判断有几个形状
            Assert.AreEqual(1, canvas.shapes.lstShapes.Count);
            // 6. 然后执行undo
            canvas.commandRecorder.Undo();
            // 7. 判断有几个形状
            Assert.AreEqual(2, canvas.shapes.lstShapes.Count);
            // 8. 执行redo
            canvas.commandRecorder.Redo();
            // 9. 判断有几个形状
            Assert.AreEqual(1, canvas.shapes.lstShapes.Count);

        }
    }
}