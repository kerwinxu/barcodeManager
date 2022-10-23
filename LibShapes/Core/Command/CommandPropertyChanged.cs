using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Command
{
    public  class CommandPropertyChanged : ShapeCommand
    {
        public override void Redo()
        {
            this.canvas.shapes.replaceShape(this.OldShape.ID, this.NewShape);
            //throw new NotImplementedException();
        }

        public override void Undo()
        {
            this.canvas.shapes.replaceShape(this.OldShape.ID, this.OldShape);
            //throw new NotImplementedException();
        }
    }
}
