using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Event
{
    /// <summary>
    /// 对象被选择事件
    /// </summary>
    public class ObjectSelectEventArgs : EventArgs
    {
        /// <summary>
        /// 这个被选择了
        /// </summary>
        public Object obj { get; set; }

        public ObjectSelectEventArgs(Object obj)
        {
            this.obj = obj;
        }
    }
}
