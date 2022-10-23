using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Event
{
    /// <summary>
    /// 状态改变的事件
    /// </summary>
    public class StateChangedEventArgs : EventArgs
    {
        public State.State CurrentState { get; set; }

        public StateChangedEventArgs(State.State state)
        {
            this.CurrentState = state;
        }
    }
}
