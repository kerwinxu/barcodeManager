﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Command
{
   public   interface ICommand
    {
        void Undo();

        void Redo();
    }
}
