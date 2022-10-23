using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Command
{
    public class CommandDelete : ShapeCommand
    {
        /// <summary>
        /// 从哪一项删除的。
        /// </summary>
        public int index { get; set; }

        public override void Redo()
        {
            // 重新删除
            this.index = this.canvas.shapes.lstShapes.IndexOf(this.NewShape);
            this.canvas.shapes.lstShapes.Remove(this.NewShape);
            //throw new NotImplementedException();
        }

        public override void Undo()
        {
            // 从这个位置重新插入。
            this.canvas.shapes.lstShapes.Insert(this.index, this.NewShape);
            //throw new NotImplementedException();
        }
    }
}
