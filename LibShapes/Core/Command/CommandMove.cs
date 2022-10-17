using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Command
{
    public class CommandMove : Command
    {
        public float old_x { get; set; }
        public float old_y { get; set; }
        public float new_x { get; set; }
        public float new_y { get; set; }

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
