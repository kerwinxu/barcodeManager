using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Command
{
    public  class CommandPropertyChanged : Command
    {
        public ShapeEle old_shape { get; set; }
        public ShapeEle new_shape { get; set; }

        public override void Redo()
        {
            // todo
            throw new NotImplementedException();
        }

        public override void Undo()
        {
            // todo
            throw new NotImplementedException();
        }
    }
}
