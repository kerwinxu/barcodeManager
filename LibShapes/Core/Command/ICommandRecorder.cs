using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Command
{
    public  interface ICommandRecorder
    {
         void Undo();

         void Redo();

        bool isUndoAble();

        bool isRedoAble();
    }
}
