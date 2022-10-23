using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Command
{
    /// <summary>
    /// 形状方面的命令。
    /// </summary>
    public  abstract class ShapeCommand
    {
        /// <summary>
        /// 原先的形状
        /// </summary>
        public ShapeEle OldShape { get; set; }

        /// <summary>
        /// 新的形状
        /// </summary>
        public ShapeEle NewShape { get; set; }

        /// <summary>
        /// 画布
        /// </summary>
        public UserControlCanvas canvas { get; set; }

        // 如下是两个操作。

        public virtual void Undo() { this.canvas.shapes.replaceShape(this.OldShape.ID, this.NewShape); }

        public virtual void Redo() { this.canvas.shapes.replaceShape(this.OldShape.ID, this.OldShape); }
    }
}
