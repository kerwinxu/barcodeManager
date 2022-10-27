using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 * 我这里做一个省事，所有的更改都改成这种更改，而不是单独的区分了。
 * 
 * **/

namespace Io.Github.Kerwinxu.LibShapes.Core.Command
{

    /// <summary>
    /// Shapes的更改
    /// </summary>
    public class CommandShapesChanged:ICommand
    {
        /// <summary>
        /// 原先的
        /// </summary>
        public Shapes OldShapes { get; set; }

        /// <summary>
        /// 新的
        /// </summary>
        public Shapes NewShapes { get; set; }

        /// <summary>
        /// 画布
        /// </summary>
        public UserControlCanvas canvas { get; set; }

        public void Redo()
        {
            this.canvas.shapes = NewShapes;
            //throw new NotImplementedException();
        }

        public void Undo()
        {
            this.canvas.shapes = OldShapes;
            //throw new NotImplementedException();
        }
    }
}
