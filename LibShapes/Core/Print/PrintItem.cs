using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Print
{
    /// <summary>
    /// 打印项目
    /// </summary>
    public class PrintItem
    {
        /// <summary>
        /// 图形
        /// </summary>
        public Shapes Shapes { get; set; }

        /// <summary>
        /// 变量
        /// </summary>
        public Dictionary<string, string>Vals { get; set; }

        /// <summary>
        /// 打印数量
        /// </summary>
        public int PrintCount { get; set; }
    }
}
