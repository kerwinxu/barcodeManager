using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Command
{
    public class CommandResize : Command
    {
        public float old_x { get; set; }
        public float old_y { get; set; }
        public float old_width { get; set; }
        public float old_height { get; set; }

        public float new_x { get; set; }
        public float new_y { get; set; }
        public float new_width { get; set; }
        public float new_height { get; set; }

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
