using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Command
{
    /// <summary>
    /// 新建形状的命令,
    /// </summary>
    public class CommandCreate : ShapeCommand
    {
        public override void Redo()
        {
            this.canvas.shapes.lstShapes.Add(this.NewShape);
            //throw new NotImplementedException();
        }

        public override void Undo()
        {
            this.canvas.shapes.lstShapes.Remove(this.NewShape);
            //throw new NotImplementedException();
        }
    }
}
