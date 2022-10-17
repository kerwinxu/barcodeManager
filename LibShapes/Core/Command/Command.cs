using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Command
{
    public  abstract class Command
    {
        /// <summary>
        /// 对某个形状的操作
        /// </summary>
        public string  ID { get; set; }

        // 如下是两个操作。

        public abstract void Undo();

        public abstract void Redo();
    }
}
